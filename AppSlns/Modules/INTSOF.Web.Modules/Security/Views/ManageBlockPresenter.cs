#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageBlockPresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region Application Specific

using Entity;
using INTSOF.Utils;
using Business.RepoManagers;
using System.Collections.Generic;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class has the method's implementation which performs all the CRUD(Create/ Read/ Update/ Delete) operation for managing blocks with its details.
    /// </summary>
    public class ManageBlockPresenter : Presenter<IManageBlockView>
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Performs a delete operation for a Line of Business.
        /// </summary>
        public void DeleteLineOfBusinesses()
        {
            lkpSysXBlock sysXBlock = SecurityManager.GetLineOfBusiness(View.ViewContract.SysXBlockId);

            if (!SecurityManager.IsLineOfBusinessAssociatedWithProduct(View.ViewContract.SysXBlockId).Equals(false))
            {
                throw new SysXException(SysXUtils.GetMessage(ResourceConst.SECURITY_USERTYPE) + sysXBlock.Name +
                                    SysXUtils.GetMessage(ResourceConst.SPACE) +
                                    SysXUtils.GetMessage(ResourceConst.SECURITY_FEATURE_DETAILS));
            }
            else
            {
                if (!sysXBlock.IsNull())
                {
                    sysXBlock.ModifiedByID = View.CurrentUserId;
                    sysXBlock.IsDeleted = true;
                    sysXBlock.ModifiedOn = DateTime.Now;
                    View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.USER_TYPE) + SysXUtils.GetMessage(ResourceConst.SPACE) + sysXBlock.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.DELETED_SUCCESSFULLY);
                    sysXBlock.Name = sysXBlock.Name + "_" + Guid.NewGuid();
                    SecurityManager.DeleteLineOfBusiness(sysXBlock);
                }
            }
        }

        /// <summary>
        ///  Performs an update operation for a Line of Business.
        /// </summary>
        public void UpdateLineOfBusinesses()
        {
            lkpSysXBlock sysXBlock = SecurityManager.GetLineOfBusiness(View.ViewContract.SysXBlockId);

            if (SecurityManager.IsLineOfBusinessExists(View.ViewContract.Name, sysXBlock.Name))
            {
                View.ErrorMessage = View.ViewContract.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.USER_TYPE_ALREADY_EXISTS);
            }
            else
            {
                View.ErrorMessage = String.Empty;

                if (!sysXBlock.IsNull())
                {
                    sysXBlock.Name = View.ViewContract.Name;
                    sysXBlock.Description = View.ViewContract.Description;
                    sysXBlock.Code = View.ViewContract.Code.ToUpper();
                    sysXBlock.ModifiedByID = View.CurrentUserId;
                    sysXBlock.ModifiedOn = DateTime.Now;
                    sysXBlock.BusinessChannelTypeID = View.ViewContract.BusinessChannelTypeID;
                    SecurityManager.UpdateLineOfBusiness(sysXBlock);
                    View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.USER_TYPE) + SysXUtils.GetMessage(ResourceConst.SPACE) + sysXBlock.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.UPDATED_SUCCESSFULLY);
                }
            }
        }

        /// <summary>
        ///  Performs an insert operation for a Line of Business.
        /// </summary>
        public void AddLineOfBusinesses()
        {
            lkpSysXBlock sysXBlock = new lkpSysXBlock();

            if (SecurityManager.IsLineOfBusinessExists(View.ViewContract.Name))
            {
                View.ErrorMessage = View.ViewContract.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.USER_TYPE_ALREADY_EXISTS);
            }
            else
            {
                View.ErrorMessage = String.Empty;
                sysXBlock.Name = View.ViewContract.Name;
                sysXBlock.Code = View.ViewContract.Code.ToUpper();
                sysXBlock.IsActive = true;
                sysXBlock.IsDeleted = false;
                sysXBlock.Description = View.ViewContract.Description;
                sysXBlock.CreatedByID = View.CurrentUserId;
                sysXBlock.CreatedOn = DateTime.Now;
                sysXBlock.BusinessChannelTypeID = View.ViewContract.BusinessChannelTypeID;
                SecurityManager.AddLineOfBusiness(sysXBlock);
                View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.USER_TYPE) + SysXUtils.GetMessage(ResourceConst.SPACE) + sysXBlock.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SAVED_SUCCESSFULLY);
            }
        }

        public Boolean IsLOBCodeExist(String newCode, String existingCode)
        {
            return SecurityManager.IsLOBCodeExist(newCode, existingCode);
        }
        /// <summary>
        /// Retrieves a list of all Line of Business.
        /// </summary>
        public void RetrievingLineOfBusinesses()
        {
            View.Blocks = SecurityManager.GetLineOfBusinesses();
        }

        #endregion

        #region Private Methods

        #endregion

        #region AMS

        public List<lkpBusinessChannelType> GetBusinessChannelTypes()
        {
            List<lkpBusinessChannelType> lstBusinessChannels = SecurityManager.GetBusinessChannelTypes();
            return lstBusinessChannels.OrderBy(col => col.BusinessChannelTypeID).ToList();
        }

        #endregion

        #endregion
    }
}