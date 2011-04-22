using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Net;
using System.Threading;
using System.IO;

namespace Twitter
{
    public class AsyncContentManager
    {
        public const int C_DEFAULT_CACHE_LIMIT = 500;

        private static object c_acmLock = typeof(AsyncContentManager);
        private static AsyncContentManager c_acmInstance = null;

        private Thread m_thdWorker;
        private bool m_bKeepWorkerAlive;
        private Queue<AsyncContent> m_qWorkQueue;
        private Dictionary<string, AsyncContent> m_dctCache;
        private int m_iCacheLimit;

        public delegate void RequestImageCallbackHandler(object sender, Bitmap bmpImage, object objContext);

        private AsyncContentManager()
        {
            m_dctCache = new Dictionary<string, AsyncContent>();
            m_qWorkQueue = new Queue<AsyncContent>();
            m_bKeepWorkerAlive = true;
            m_iCacheLimit = C_DEFAULT_CACHE_LIMIT;

            m_thdWorker = new Thread(new ThreadStart(Work));
            m_thdWorker.Start();
        }

        public static AsyncContentManager GetManager()
        {
            lock (c_acmLock)
            {
                if (c_acmInstance == null)
                    c_acmInstance = new AsyncContentManager();
            }

            return c_acmInstance;
        }

        public void RequestImage(string sUrl, RequestImageCallbackHandler richCallback, object objContext = null)
        {
            if (m_dctCache.ContainsKey(sUrl))
            {
                if (m_dctCache[sUrl].Result == null)
                    m_dctCache[sUrl].Callbacks.Add(new AsyncContentCallback(richCallback, null));
                else
                    richCallback(this, (Bitmap)m_dctCache[sUrl].Result, objContext);
            }
            else
            {
                AsyncContent acCur = new AsyncContent()
                {
                    Url = sUrl,
                    Type = AsyncContent.ContentType.Image,
                    HitCount = 1
                };

                acCur.Callbacks.Add(new AsyncContentCallback(richCallback, objContext));
                m_qWorkQueue.Enqueue(acCur);
                m_dctCache.Add(sUrl, acCur);

                Cleanup();
            }
        }

        private void Cleanup()
        {
            if (m_dctCache.Count >= m_iCacheLimit)
            {
                Dictionary<int, List<AsyncContent>> dctBuckets = new Dictionary<int, List<AsyncContent>>();
                Dictionary<string, AsyncContent>.Enumerator dCacheEnum = m_dctCache.GetEnumerator();
                int iCounter = 0;

                while (dCacheEnum.MoveNext())
                {
                    if (! dctBuckets.ContainsKey(dCacheEnum.Current.Value.HitCount))
                        dctBuckets.Add(dCacheEnum.Current.Value.HitCount, new List<AsyncContent>());

                    dctBuckets[dCacheEnum.Current.Value.HitCount].Add(dCacheEnum.Current.Value);
                }

                while ((m_dctCache.Count > (m_iCacheLimit / 3)) && (m_dctCache.Count > 0))
                {
                    if (dctBuckets.ContainsKey(iCounter))
                    {
                        for (int i = 0; i < dctBuckets[iCounter].Count; i++)
                        {
                            if (m_dctCache.Count >= (m_iCacheLimit / 3))
                                m_dctCache.Remove(dctBuckets[iCounter][i].Url);
                            else
                                break;
                        }
                    }

                    iCounter ++;
                }
            }
        }

        //this is processed on a different thread (m_thdWorker)
        private void Work()
        {
            AsyncContent asCurContent;
            WebClient wcClient = new WebClient();  //make a web client to be shared amongst jobs

            while (m_bKeepWorkerAlive)
            {
                try
                {
                    if (m_qWorkQueue.Count > 0)
                    {
                        asCurContent = m_qWorkQueue.Dequeue();

                        switch (asCurContent.Type)
                        {
                            case AsyncContent.ContentType.Image:
                                WorkOnImage(asCurContent, wcClient); break;
                        }
                    }
                }
                catch (Exception e)
                {
                    //@todo: log this exception
                }
            }
        }

        private void WorkOnImage(AsyncContent acImage, WebClient wcClient)
        {
            Bitmap bmpImage;

            //@TODO: for production, don't look in desktop dir (duh!)
            if (File.Exists(Path.Combine("../../../../Documents/test/avatars", Path.GetFileName(acImage.Url))))
                bmpImage = (Bitmap)Bitmap.FromFile(Path.Combine("../../../../Documents/test/avatars", Path.GetFileName(acImage.Url)));
            else
            {
                Stream stmImage = wcClient.OpenRead(acImage.Url);
                bmpImage = (Bitmap)Bitmap.FromStream(stmImage);
                stmImage.Close();
            }

            //bmpImage.Save("C:/Users/le grand fromage/Desktop/avatars/" + Path.GetFileName(acImage.Url));

            for (int i = 0; i < acImage.Callbacks.Count; i ++)
                acImage.Callbacks[i].Callback.DynamicInvoke(this, bmpImage, acImage.Callbacks[i].Context);

            acImage.Result = bmpImage;
        }

        public static bool HasInstance
        {
            get { return (c_acmInstance != null); }
        }

        public static void Destroy()
        {
            if (c_acmInstance != null)
            {
                c_acmInstance.m_bKeepWorkerAlive = false;
                c_acmInstance.m_thdWorker.Abort();
            }
        }

        ~AsyncContentManager()
        {
            m_bKeepWorkerAlive = false;
            m_thdWorker.Abort();
            while (m_thdWorker.IsAlive) { }
        }
    }

    public class AsyncContent
    {
        public enum ContentType
        {
            Image = 1
        }

        private ContentType m_ctType;
        private List<AsyncContentCallback> m_lacCallbacks;
        private string m_sUrl;
        private object m_objResult;
        private int m_iHitCount;

        public AsyncContent()
        {
            m_lacCallbacks = new List<AsyncContentCallback>();
        }

        public ContentType Type
        {
            get { return m_ctType; }
            set { m_ctType = value; }
        }

        public List<AsyncContentCallback> Callbacks
        {
            get { return m_lacCallbacks; }
            set { m_lacCallbacks = value; }
        }

        public string Url
        {
            get { return m_sUrl; }
            set { m_sUrl = value; }
        }

        public object Result
        {
            get { return m_objResult; }
            set { m_objResult = value; }
        }

        public int HitCount
        {
            get { return m_iHitCount; }
            set { m_iHitCount = value; }
        }
    }

    public class AsyncContentCallback
    {
        private Delegate m_dgCallback;
        private object m_objContext;

        public AsyncContentCallback(Delegate dgInitCallback, object objInitContext)
        {
            m_dgCallback = dgInitCallback;
            m_objContext = objInitContext;
        }

        public Delegate Callback
        {
            get { return m_dgCallback; }
        }

        public object Context
        {
            get { return m_objContext; }
        }
    }
}
