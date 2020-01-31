using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public class ManageBulkRoleFeaturePresenter : Presenter<IManageBulkRoleFeatureView>
    {
        public void GetBusinessChannelTypes()
        {
            List<lkpBusinessChannelType> lstBusinessChannels = SecurityManager.GetBusinessChannelTypes();
            View.lstBusinessChannel = lstBusinessChannels.OrderBy(col => col.BusinessChannelTypeID).ToList();
        }

        /// <summary>
        /// Retrieves a list of all the product features with its details.
        /// </summary>
        public void GetProductFeatures()
        {
            if (View.SelectedBuisnessChannelTypeId > AppConsts.NONE)
            {
                View.ProductFeatures = SecurityManager.GetProductFeatures().ToList().Where(cond => cond.BusinessChannelTypeID.HasValue ? cond.BusinessChannelTypeID.Value == View.SelectedBuisnessChannelTypeId : true).ToList();
            }
            else
            {
                View.ProductFeatures = new List<ProductFeature>();
            }
        }

        /// <summary>
        /// Retrieves a list of all Line of Business.
        /// </summary>
        public void GetUserTypes()
        {
            if (View.SelectedBuisnessChannelTypeId > AppConsts.NONE)
            {
                View.lstUserType = SecurityManager.GetLineOfBusinesses().Where(cond => cond.BusinessChannelTypeID == View.SelectedBuisnessChannelTypeId 
                                                                                && cond.Code!="BCAPPL").ToList();
            }
            else
            {
                View.lstUserType = new List<lkpSysXBlock>();
            }
        }

        /// <summary>
        /// Retrieves a list of all roles with it's details.
        /// </summary>
        public void GetRoleDetails()
        {
            if (View.lstSelectedUserTypes.IsNullOrEmpty())
            {
                View.RoleDetails = new List<INTSOF.UI.Contract.IntsofSecurityModel.ManageRoleContract>();
            }
            else
            {
                View.RoleDetails = SecurityManager.GetRolesMappedWithUserType(View.lstSelectedUserTypes);
            }
        }

        public void InsertBulkRoleFeatures()
        {
            SecurityManager.InsertBulkRoleFeatures(View.lstSelectedFeature, View.lstSelectedUserTypes, View.lstSelectedRoles, View.CurrentUserId);
        }
    }
}
