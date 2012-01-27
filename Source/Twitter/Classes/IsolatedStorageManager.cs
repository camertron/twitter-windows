using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;

namespace Twitter
{
    public class IsolatedStorageManager
    {
        private static IsolatedStorageManager c_ismInstance = null;
        private static object c_lock = typeof(IsolatedStorageManager);
        private IsolatedStorageFile m_isfStore;

        private IsolatedStorageManager()
        {
            m_isfStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null);
        }

        public static IsolatedStorageManager GetManager()
        {
            lock (c_lock)
            {
                if (c_ismInstance == null)
                    c_ismInstance = new IsolatedStorageManager();

                return c_ismInstance;
            }
        }

        public IsolatedStorageFileStream Open(string sPath, FileMode fmFileMode = FileMode.OpenOrCreate)
        {
            return new IsolatedStorageFileStream(sPath, fmFileMode, m_isfStore);
        }

        public StreamWriter OpenWrite(string sPath, FileMode fmFileMode = FileMode.OpenOrCreate)
        {
            return new StreamWriter(Open(sPath, fmFileMode));
        }

        public StreamReader OpenRead(string sPath, FileMode fmFileMode = FileMode.OpenOrCreate)
        {
            return new StreamReader(Open(sPath, fmFileMode));
        }

        public IsolatedStorageFile Store
        {
            get { return m_isfStore; }
        }
    }
}
