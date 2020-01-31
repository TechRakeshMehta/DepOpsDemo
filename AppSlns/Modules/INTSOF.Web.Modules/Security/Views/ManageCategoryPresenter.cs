#region Header Comment Block
// 
// Copyright BestX, Inc. 2012
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename: ManageCategoryPresenter.cs
#endregion

#region Namespaces

#region System Defined

using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.CompositeWeb;

#endregion

#region Application Specific

using INTSOF.Utils;
using BESTX.Entity;
using BESTX.Business.RepoManagers;
using  BESTX.WEB.IntsofSecurityModel.Providers;
using INTSOF.Utils.Consts;
#endregion

#endregion

namespace  BESTX.WEB.IntsofSecurityModel.Views
{
    public class ManageCategoryPresenter : Presenter<IManageCategoryView>
    {
        #region Methods
        public void GetCategories()
        {
            Int32 stateID = View.ViewContract.StateID;
            Int32 tenantID = View.ViewContract.TenantID;

            if (stateID != 0)
            {
                View.StateCategories = SecurityManager.GetStateCategories(stateID, tenantID);
            }
        }

        public Boolean UpdateCategory()
        {
            RuleCategory ruleCategory = SecurityManager.GetCategory(View.ViewContract.CategoryID);
            ruleCategory.RLC_Name = View.ViewContract.CategoryName;
            ruleCategory.RLC_Description = View.ViewContract.CategoryDescription;
            ruleCategory.RLC_ModifiedOn = DateTime.Now;
            ruleCategory.RLC_ModifiedByID = 10;
            ruleCategory.RLC_StateID = View.ViewContract.StateID;
            Boolean IsSucess = SecurityManager.UpdateCategory(ruleCategory);
            View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.CATEGORY) + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.UPDATED_SUCCESSFULLY);
            return IsSucess;
        }

        public Boolean SaveCategory()
        {
            RuleCategory category = new RuleCategory();
            category.RLC_Name = View.ViewContract.CategoryName;
            category.RLC_Description = View.ViewContract.CategoryDescription;
            category.RLC_TenantID = View.ViewContract.TenantID;
            category.RLC_IsActive = true;
            category.RLC_CreatedByID = 10;
            category.RLC_StateID = View.ViewContract.StateID;
            category.RLC_CreatedOn = System.DateTime.Now;
            Int32 categoryID = SecurityManager.SaveCategory(category);
            View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.CATEGORY) + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SAVED_SUCCESSFULLY);
            return true;
        }

        public Boolean DeleteCategory()
        {
            Int32 stateID = View.ViewContract.StateID;
            Int32 categoryID = View.ViewContract.CategoryID;
            Boolean isSuccess = false;
            if (stateID != 0)
            {
                RuleCategory ruleCategory = SecurityManager.GetCategory(categoryID);
                ruleCategory.RLC_IsActive = false;
                isSuccess = SecurityManager.UpdateCategory(ruleCategory);
            }
            View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.CATEGORY) + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.DELETED_SUCCESSFULLY);
            return isSuccess;
        } 
        #endregion
    }
}




