#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageSubClientTenantPresenter.cs
// Purpose:
// 

#endregion

#region Namespaces

#region System Specified

using System;
using INTSOF.SharedObjects;


#endregion

#region Application Specified

using Business.RepoManagers;


#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    public class ManageSubClientTenantPresenter : Presenter<IManageSubClientTenantView>
    {
        #region Veriables
        #endregion

        #region Property
        #endregion

        #region Method

        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
        }

        ///// <summary>
        ///// Get client detail by id
        ///// </summary>
        ///// <param name="ClientId"></param>
        ///// <returns></returns>
        public string GetClientDetailById(Int32 ClientId)
        {
            return String.Empty; // ClientManager.GetClientById(ClientId).ClientName;
        }

        #endregion
    }
}




