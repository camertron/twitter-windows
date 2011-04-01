using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;

namespace Twitter
{
    public class ResourceManager
    {
        private static object c_lock = typeof(ResourceManager);
        private static ResourceManager c_instance = null;
        private const string C_RSRC_BASE = "Twitter.Resources.";

        private ResourceManager() { }

        public static ResourceManager GetManager()
        {
            lock (c_lock)
            {
                if (c_instance == null)
                    c_instance = new ResourceManager();
            }

            return c_instance;
        }

        public StreamReader GetResourceStream(string sPath)
        {
            return new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(C_RSRC_BASE + sPath));
        }

        public Bitmap GetBitmap(string sPath)
        {
            StreamReader srReader = GetResourceStream(sPath);
            return (Bitmap)Bitmap.FromStream(srReader.BaseStream);
        }
    }
}
