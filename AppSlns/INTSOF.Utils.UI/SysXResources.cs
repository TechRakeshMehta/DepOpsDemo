namespace INTSOF.Utils.UI
{
   using System;
   using System.Resources;

   using System.Web;

   public static class SysXResources
    {
        private static ResourceManager resourceManager;

        static SysXResources()
        {       
            resourceManager = ResourceManager.CreateFileBasedResourceManager("SysXResource", HttpContext.Current.ApplicationInstance.Server.MapPath("~"), null);
        }

        public static String GetString(String Key)
        {
            return resourceManager.GetString(Key);
        }
    }
}
