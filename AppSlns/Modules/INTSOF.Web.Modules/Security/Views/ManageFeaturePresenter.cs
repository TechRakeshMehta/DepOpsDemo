#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageFeaturePresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using INTSOF.SharedObjects;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Business.RepoManagers;
using CoreWeb.IntsofSecurityModel.Providers;
using ModuleUtility;
using System.Collections.Generic;
#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class has the method's implementation which performs all the CRUD(Create/ Read/ Update/ Delete) operation for managing features with its details.
    /// </summary>
    public class ManageFeaturePresenter : Presenter<IManageFeatureView>
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

        public override void OnViewInitialized()
        {
            View.BusinessChannels = SecurityManager.GetBusinessChannelTypes().Where(cond => !cond.Code.Equals(BusinessChannelType.COMMON.GetStringValue())).ToList();
        }

        public override void OnViewLoaded()
        {

        }

        /// <summary>
        /// Performs a delete operation for product feature.
        /// </summary>
        public void DeleteProductFeature()
        {
            //if (SecurityManager.IsFeatureAssociatedWithLineOfBusiness(View.ViewContract.ProductFeatureId))
            //{
            //    throw new Exception(SysXUtils.GetMessage(ResourceConst.SECURITY_PRODUCT_FEATURE) +
            //                        SysXUtils.GetMessage(ResourceConst.SPACE) +
            //                        SecurityManager.GetProductFeature(View.ViewContract.ProductFeatureId).Name +
            //                        SysXUtils.GetMessage(ResourceConst.SPACE) +
            //                        SysXUtils.GetMessage(ResourceConst.SECURITY_FEATURE_DETAILS));
            //}

            ProductFeature productFeature = SecurityManager.GetProductFeature(View.ViewContract.ProductFeatureId);
            View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.FEATURE) + SysXUtils.GetMessage(ResourceConst.SPACE) + productFeature.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.DELETED_SUCCESSFULLY);
            SecurityManager.DeleteProductFeature(productFeature, View.CurrentUserId);
            //added the below to set the marker flag for refreshing the menu items
            //SysXSiteMapProvider.SecurityService.IsMenuRefreshRequired = true;
        }

        /// <summary>
        /// Performs an update operation for product feature.
        /// </summary>
        public void UpdateProductFeature()
        {
            ProductFeature productFeature = SecurityManager.GetProductFeature(View.ViewContract.ProductFeatureId);
            if (ProductFeatures.ToList().FindAll(productFeatureDetails => productFeatureDetails.Name.Equals(View.ViewContract.Name, StringComparison.InvariantCultureIgnoreCase) && productFeatureDetails.ParentProductFeatureID == AppConsts.NONE).Count > AppConsts.NONE)
            {
                View.ErrorMessage = View.ViewContract.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_FEATURE_EXISTS);
            }
            else
            {
                if (ProductFeatures.ToList().FindAll(productFeatureDetails => productFeatureDetails.ProductFeatureID == productFeature.ParentProductFeatureID ||
                                                                                                   productFeatureDetails.ParentProductFeatureID == productFeature.ParentProductFeatureID)
                    .SkipWhile(productFeatureDetails => productFeature.Name.Equals(View.ViewContract.Name, StringComparison.InvariantCultureIgnoreCase))
                    .Any(nameChecks => nameChecks.Name.ToLower().Equals(View.ViewContract.Name.ToLower())))
                {
                    View.ErrorMessage = View.ViewContract.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_FEATURE_EXISTS);
                }
                else
                {
                    View.ErrorMessage = String.Empty;
                    productFeature.Name = View.ViewContract.Name;
                    productFeature.Description = View.ViewContract.Description;
                    productFeature.UIControlID = View.ViewContract.UIControlId;
                    productFeature.NavigationURL = View.ViewContract.NavigationUrl;
                    productFeature.OpeninNewbrowser = View.ViewContract.OpeninNewbrowser;
                    productFeature.IconImageName = View.ViewContract.IconImageName;
                    productFeature.DisplayOrder = View.ViewContract.DisplayOrder;
                    productFeature.IsActive = true;
                    productFeature.IsDeleted = false;
                    productFeature.ModifiedByID = View.CurrentUserId;
                    productFeature.ModifiedOn = DateTime.Now;

                    //Onsite Changes for Dashboard - Start
                    productFeature.IsDashboardFeature = View.ViewContract.IsDashboardFeature;
                    productFeature.ForExternalUser = View.ViewContract.ForExternalUser;
                    //End
                    //Admin Entry Portal//
                    productFeature.FeatureAreaTypeID = View.ViewContract.FeatureAreaTypeID == AppConsts.NONE ? (int?)null : View.ViewContract.FeatureAreaTypeID;
                    //End

                    SecurityManager.UpdateProductFeature(productFeature);

                    //Onsite Changes for Dashboard - Start
                    //If Dashboard feature is selecetd
                    if (View.ViewContract.IsDashboardFeature)
                    {
                        SaveDashboardLayoutPreference();
                    }
                    //End

                    //added the below to set the marker flag for refreshing the menu items
                    //SysXSiteMapProvider.SecurityService.IsMenuRefreshRequired = true;
                    View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.FEATURE) + SysXUtils.GetMessage(ResourceConst.SPACE) + productFeature.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.UPDATED_SUCCESSFULLY);
                }
            }
        }

        /// <summary>
        /// Performs an insert operation for product feature.
        /// </summary>
        public void AddProductFeature()
        {
            // When adding Feature Not having parent
            if (ProductFeatures.ToList().FindAll(productFeatureDetails => productFeatureDetails.Name.Equals(View.ViewContract.Name, StringComparison.InvariantCultureIgnoreCase) && productFeatureDetails.ParentProductFeatureID == AppConsts.NONE).Count > AppConsts.NONE)
            {
                View.ErrorMessage = View.ViewContract.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_FEATURE_EXISTS);
            }
            else
            {
                if (SecurityManager.GetProductFeatures().ToList().FindAll(
                    condition => condition.ProductFeatureID == View.ViewContract.ParentProductFeatureId ||
                                 condition.ParentProductFeatureID == View.ViewContract.ParentProductFeatureId)
                    .Any(nameChecks => nameChecks.Name.ToLower().Equals(View.ViewContract.Name.ToLower())))
                {
                    View.ErrorMessage = View.ViewContract.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_FEATURE_EXISTS);
                }
                else
                {
                    View.ErrorMessage = String.Empty;
                    ProductFeature productFeature = new ProductFeature
                    {
                        Name = View.ViewContract.Name,
                        Description = View.ViewContract.Description,
                        UIControlID = View.ViewContract.UIControlId,
                        NavigationURL = View.ViewContract.NavigationUrl,
                        OpeninNewbrowser = View.ViewContract.OpeninNewbrowser,
                        IconImageName = View.ViewContract.IconImageName,
                        DisplayOrder = View.ViewContract.DisplayOrder,
                        ParentProductFeatureID = View.ViewContract.ParentProductFeatureId,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedByID = View.CurrentUserId,
                        CreatedOn = DateTime.Now,
                        AllowDelegation = true,
                        IsSystem = false,
                        //Onsite Changes for Dashboard - Start
                        IsDashboardFeature = View.ViewContract.IsDashboardFeature,
                        ForExternalUser = View.ViewContract.ForExternalUser,
                        BusinessChannelTypeID = Convert.ToInt16(View.SelectedBusinessChannel),
                        //End
                        //Admin Entry Portal//
                        FeatureAreaTypeID = View.ViewContract.FeatureAreaTypeID == AppConsts.NONE ? (int?)null : View.ViewContract.FeatureAreaTypeID,
                        //End

                    };


                    //if (View.ViewContract.ActionName.IsNotNull())
                    //  View.ViewContract.ActionName.ForEach(action => { productFeature.FeatureActions.Add(new FeatureAction { Action = action }); });

                    if (!View.ViewContract.ActionCollection.IsNull() && View.ViewContract.ActionCollection.Count > AppConsts.NONE)
                    {
                        View.ViewContract.ActionCollection.ForEach(action =>
                        {
                            productFeature.FeatureActions.Add(new FeatureAction
                            {
                                ChildScreenName = action.ScreenName,
                                ControlActionId = action.ControlActionId,
                                ControlActionLabel = action.ControlActionLabel,
                                CustomActionId = action.CustomActionId,
                                CustomActionLabel = action.CustomActionLabel
                            });

                        });


                    }



                    SecurityManager.AddProductFeature(productFeature);
                    //added the below to set the marker flag for refreshing the menu items
                    //SysXSiteMapProvider.SecurityService.IsMenuRefreshRequired = true;

                    #region "Dashboard Code"

                    //Onsite Changes for Dashboard - Start 
                    //If Dashboard feature is selecetd
                    if (View.ViewContract.IsDashboardFeature)
                    {
                        SaveDashboardLayoutPreference();
                    }
                    //End
                    #endregion


                    View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.FEATURE) + SysXUtils.GetMessage(ResourceConst.SPACE) + productFeature.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SAVED_SUCCESSFULLY);
                }
            }
        }

        /// <summary>
        /// Retrieves a list of all product features.
        /// </summary>
        public void RetrievingProductFeatures()
        {
            View.ProductFeatures = ProductFeatures;
        }


        public IQueryable<ProductFeature> ProductFeatures
        {
            get
            {
                Int16 selectedBusinessChannel = Convert.ToInt16(View.SelectedBusinessChannel);
                return SecurityManager.GetProductFeatures().Where(cond => cond.BusinessChannelTypeID.HasValue ? cond.BusinessChannelTypeID.Value == selectedBusinessChannel : true);
            }
        }


        #region Admin Entry Portal

        public void IsBkgBusinessChannel()
        {
            //View.IsBkgBusinessChannel= false;
            if (!View.SelectedBusinessChannel.IsNullOrEmpty())
            {
                String BkgBusinessChannelCode = BusinessChannelType.AMS.GetStringValue();
                Int16 selectedBusinessChannelID = Convert.ToInt16(View.SelectedBusinessChannel);
                View.IsBkgBusinessChannel = SecurityManager.GetBusinessChannelTypes().Where(cond => cond.Code.Equals(BusinessChannelType.AMS.GetStringValue())).FirstOrDefault().BusinessChannelTypeID == selectedBusinessChannelID ? true : false;
            }
            //return IsBkgBusinessChannel;
        }

        public void GetFeatureAreaType()
        {
            View.lstFeatureAreaType = new List<lkpFeatureAreaType>();
            View.lstFeatureAreaType = SecurityManager.GetFeatureAreaType();
        }

        #endregion
        #endregion

        #region Private Methods
        //Onsite Changes for Dashboard - Start
        /// <summary>
        /// Save dashboard markup from selected ascx in aspnet_personalizeallusers table
        /// Check for the user group that has been selected and correspondingly set the flag.
        /// </summary>
        private void SaveDashboardLayoutPreference()
        {
            string userControlFilePath = View.ViewContract.DashboardTemplatesPath + "/" + View.ViewContract.UIControlId;
            StreamReader ucMarkupReader =
                new StreamReader(userControlFilePath);
            string markup = ucMarkupReader.ReadToEnd();
            aspnet_Paths path = new aspnet_Paths();
            path.PathId = Guid.NewGuid();
            path.Path = userControlFilePath;
            path.LoweredPath = userControlFilePath.ToLower();
            aspnet_PersonalizationAllUsers groupPreference = new aspnet_PersonalizationAllUsers();
            groupPreference.DashboardLayout = markup;
            groupPreference.LastUpdatedDate = DateTime.Now;
            short businessChannelTypeId = AppConsts.ONE;
            BusinessChannelTypeMappingData businessChannelType = (BusinessChannelTypeMappingData)ModuleUtils.SessionService.GetCustomData(AppConsts.SESSION_BUSINESS_CHANNEL_TYPE);
            if (!businessChannelType.IsNull())
            {
                businessChannelTypeId = businessChannelType.BusinessChannelTypeID;
            }
            if (View.ViewContract.ForExternalUser)
            {
                // Save the markup for the the current control selected into the aspnet_PersonalizationAllUsers table
                if (businessChannelTypeId == AppConsts.ONE)
                {
                    groupPreference.ForExternalUser = 1;
                }
                else
                {
                    groupPreference.ForExternalUser = 4;
                }

            }
            else
            {
                // Save the markup for the the current control selected into the aspnet_PersonalizationAllUsers table
                if (businessChannelTypeId == AppConsts.ONE)
                {
                    groupPreference.ForExternalUser = 0;
                }
                else
                {
                    groupPreference.ForExternalUser = 3;
                }

            }
            SecurityManager.SaveGroupPreference(path, groupPreference);
        }
        //End
        #endregion

        #endregion
    }
}