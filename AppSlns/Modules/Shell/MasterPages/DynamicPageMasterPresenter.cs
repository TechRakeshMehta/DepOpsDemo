#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  AppAssetMasterPresenter.cs
// Purpose:   Presenter  for AppAssetMaster.
//

#endregion

#region Namespaces

#region System Defined

using System;
using INTSOF.SharedObjects;


#endregion

#region Application Specific

using Entity;
using INTSOF.Utils;
using Business.RepoManagers;



#endregion

#endregion
namespace CoreWeb.Shell.MasterPages
{
    public class DynamicPageMasterPresenter : Presenter<IDynamicPageMasterView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IShellController _controller;
        // public DynamicPageMasterPresenter([CreateNew] IShellController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            object data = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_ID);
            if (data != null)
            {
                int webSiteID = (Int32)(data);
                WebSiteWebConfig webSiteWebConfig = WebSiteManager.GetWebSiteWebConfig(webSiteID);
                View.HeaderHtml = webSiteWebConfig.HeaderHtml;
            }
        }

        // TODO: Handle other view events and set state in the view
    }
}




