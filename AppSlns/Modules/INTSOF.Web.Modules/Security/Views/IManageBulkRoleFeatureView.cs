using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.UI.Contract.IntsofSecurityModel;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public interface IManageBulkRoleFeatureView
    {
        List<lkpBusinessChannelType> lstBusinessChannel { get; set; }

        Int32 SelectedBuisnessChannelTypeId { get; set; }

        List<ProductFeature> ProductFeatures
        {
            get;
            set;
        }

        List<lkpSysXBlock> lstUserType
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the list of all role details.
        /// </summary>
        List<ManageRoleContract> RoleDetails
        {
            get;
            set;
        }

        String lstSelectedFeature
        {
            get;
            set;
        }

        String lstSelectedUserTypes
        {
            get;
            set;
        }

        String lstSelectedRoles
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current user id.
        /// </summary>
        /// <remarks></remarks>
        Int32 CurrentUserId
        {
            get;
        }

        List<Int32> lstSelectedFeatureIds
        {
            get;
            set;
        }
    }
}
