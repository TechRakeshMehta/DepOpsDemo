using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSiteUtils.SharedObjects;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IManageMultiTenantAssignementDataView
    {
        IManageMultiTenantAssignementDataView CurrentViewContext
        {
            get;
        }

        List<MultiInstitutionAssignmentDataContract> lstMultiInstitutionAssignmentData
        {
            get;
            set;
        }

        MultiInstitutionAssignmentDataContract VerificationViewContract
        {
            get;
        }

        List<Int32> SelectedTenantIds
        {
            get;
            set;
        }

        List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }

        List<Entity.OrganizationUser> lstOrganizationUser
        {
            set;
        }

        Int32 TenantId
        {

            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        List<AssignQueueRecords> SelectedVerificationItemsNew
        {
            get;
            set;
        }
        List<Int32> FVdIdList
        {
            get;
            set;
        }

        #region Custom Paging Parameters

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        Int32 PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        Int32 VirtualPageCount
        {
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
        }


        #endregion

        Boolean IsMutipleTimesAssignmentAllowed { get; set; }  //UAT 2809

        String ErrorMessage { get; set; }  //UAT 2809

        Boolean IsUserAlreadyAssigned { get; set; } //UAT 2809
    }
}
