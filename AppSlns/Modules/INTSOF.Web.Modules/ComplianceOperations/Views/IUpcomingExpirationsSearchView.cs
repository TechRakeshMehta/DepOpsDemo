using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IUpcomingExpirationsSearchView
    {
        Int32 CurrentLoggedInUserId { get; }
        Int32 TenantID { get; set; }
        Int32 SelectedTenantId { get; set; }
        String Categories { get; set; }
        String Items { get; set; }
        DateTime? DateFrom { get; set; }
        DateTime? DateTo { get; set; }
        String UserGroups { get; set; }
        List<Tenant> lstTenant { get; set; }
        List<UserGroup> lstUserGroup { get; set; }
        //revert back changes UAt-2834
        //List<ComplianceCategoryDetails> lstComplianceCategory { get; set; }
        List<ComplianceCategory> lstComplianceCategory { get; set; }
        
        List<ComplianceItem> lstComplianceItem { get; set; }
        List<Int32> SelectedCategoryIds { get; set; }
        String HierarchyIds { get; set; }
        List<UpcomingExpirationContract> lstUpcomingExpirations { get; set; }
        CustomPagingArgsContract customPagingArgsContract { get; set; }

        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// PageSize
        /// </summary>
        /// <value> Gets the value for PageSize.</value>
        Int32 PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// VirtualPageCount
        /// </summary>
        /// <value> Sets the value for VirtualPageCount.</value>
        Int32 VirtualPageCount
        {
            set;
            get;
        }

        Dictionary<Int32, String> AssignOrganizationUserIds { get; set; }

    }
}
