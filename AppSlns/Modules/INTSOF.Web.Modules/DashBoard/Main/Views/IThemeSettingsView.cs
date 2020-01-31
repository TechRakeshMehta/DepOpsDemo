using System;

using System.Collections.Generic;
using System.Text;
using Entity;


namespace CoreWeb.Main.Views
{
    public interface IThemeSettingsView
    {
        /// <summary>
        /// Gets and sets the aspnet_Membership
        /// </summary>
        aspnet_Membership aspnet_Membership
        {
            get;
            set;
        }

        /// <summary>
        /// DefaultLineOfBusiness</summary>
        /// <value>
        /// Gets or sets the value for Default Line of Business.</value>
        String DefaultLineOfBusiness
        {
            get;
            set;
        }
    }
}




