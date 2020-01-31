using System;

namespace CoreWeb.Shell.MasterPages
{
    public interface IDynamicPageMasterView
    {
        #region Properties
        /// <summary>
        /// Property to set the Header of application
        /// </summary>
        String HeaderHtml
        {
            set;
        }
        #endregion
    }
}




