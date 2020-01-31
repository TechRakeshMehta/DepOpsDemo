#region System Defined

using System;
using System.Linq;
using System.Web.UI;
using System.Web.Services;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region Application Specific

using Entity;
using INTSOF.Utils;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using Business.RepoManagers;
using CoreWeb.Shell.MasterPages;
using CoreWeb.Shell.Views;

#endregion


namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class Default : BasePage, IDefaultView
    {

        /// <summary>
        /// Page OnInitComplete event.
        /// </summary>
        /// <param name="e">Event</param>
        protected override void OnInitComplete(EventArgs e)
        {

            try
            {
                base.dynamicPlaceHolder = phDynamic;
                base.OnInitComplete(e);
                SetModuleTitle("Agency Hierarchy");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}