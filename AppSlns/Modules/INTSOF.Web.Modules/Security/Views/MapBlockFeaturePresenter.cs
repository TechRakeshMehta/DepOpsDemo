#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  MapBlockFeaturePresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Business.RepoManagers;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class has the method's implementation related to mapping of Line of Business with it's features.
    /// </summary>
    public class MapBlockFeaturePresenter : Presenter<IMapBlockFeatureView>
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
        /// This method handles the mapping between block and features.
        /// </summary>
        public void MappingBlockFeature()
        {
            IEnumerable<SysXBlocksFeature> blockFeatures = SecurityManager.GetFeaturesForLineOfBusiness(View.ViewContract.SysXBloxkId);
            var toBeDeleteItems = new List<Int32>();
            String unUsedFeatures = "";

            foreach (SysXBlocksFeature feature in blockFeatures)
            {
                if (View.ViewContract.Features.Count >= AppConsts.NONE)
                {
                    Boolean bFlag = View.ViewContract.Features.Exists(blockFeatureIDs => blockFeatureIDs == feature.ProductFeature.ProductFeatureID);

                    if (bFlag.Equals(false))
                    {
                        if (SecurityManager.IsSysXLineOfBusinessExistInProductFeature(feature.SysXBlockFeatureID))
                        {
                            unUsedFeatures = unUsedFeatures + ", " + feature.ProductFeature.Name;
                            //  throw new Exception(SysXUtils.GetMessage(ResourceConst.SECURITY_FEATURE) + SysXUtils.GetMessage(ResourceConst.SPACE) + feature.ProductFeature.Name + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_FEATURE_DETAILS));
                        }

                        toBeDeleteItems.Add(feature.SysXBlockFeatureID);
                    }
                    else
                    {
                        View.ViewContract.Features.Remove(feature.ProductFeature.ProductFeatureID);
                    }
                }
                else
                {
                    toBeDeleteItems.Add(feature.SysXBlockFeatureID);
                    break;
                }
            }

            //Display the error with all the used features.
            if (!unUsedFeatures.IsNullOrEmpty())
            {
                throw new SysXException(SysXUtils.GetMessage(ResourceConst.SECURITY_FEATURE) + SysXUtils.GetMessage(ResourceConst.SPACE) + unUsedFeatures.TrimStart(',').Trim() + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_FEATURE_DETAILS));
            }

            SecurityManager.LineOfBusinessFeatureMapping(SecurityManager.GetLineOfBusiness(View.ViewContract.SysXBloxkId), View.ViewContract.Features, toBeDeleteItems, View.CurrentUserId);
            View.RedirectToManageBlock();
        }

        /// <summary>
        /// Retrieves a list of all the product features with its details.
        /// </summary>
        public void RetrievingProductFeatures()
        {
            View.SysXBlocksFeatures = SecurityManager.GetFeaturesForLineOfBusiness(View.ViewContract.SysXBloxkId);
            View.ProductFeatures = SecurityManager.GetProductFeatures().Where(cond => cond.BusinessChannelTypeID.HasValue ? cond.BusinessChannelTypeID.Value == View.BusinessChannelTypeID : true);
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}