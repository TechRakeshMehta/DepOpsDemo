using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRequirementDataAuditHistory
    {
        Int32 TenantId
        {
            get;
            set;
        }

        IRequirementDataAuditHistory CurrentViewContext
        {
            get;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }

        String FirstName
        {
            get;
            set;
        }

        String LastName
        {
            get;
            set;
        }

        DateTime TimeStampFromDate
        {
            get;
            set;
        }

        DateTime TimeStampToDate
        {
            get;
            set;
        }

        List<UserGroup> lstUserGroup
        {
            get;
            set;
        }

        List<Int32> SelectedCategoryIds
        {
            get;
            set;
        }

        List<RequirementPackageContract> lstRequirementPackage
        {
            get;
            set;
        }

        List<RequirementCategoryContract> lstRequirementCategory
        {
            set;
        }

        List<RequirementItemContract> lstRequirementItems
        {
            set;
        }

        Int32 SelectedUserGroupId
        {
            get;
            set;
        }

        List<Int32> SelectedPackageIds
        {
            get;
            set;
        }

        Int32 SelectedItemID
        {
            get;
            set;
        }

        List<ApplicantRequirementDataAuditContract> ApplicantRequirementDataAuditList
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
        Int32 VirtualRecordCount
        {
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }

        #endregion

        String AdminFirstName
        {
            get;
            set;
        }

        String AdminLastName
        {
            get;
            set;
        }

        //UAT-3117
        String ComplioId
        {
            get;
            set;
        }

        #region UAT-4019
        Int32 SelectedReqPackageTypeID { get; set; }
        List<RequirementPackageTypeContract> lstRequirementPackageType
        {
            get;
            set;
        }
        #endregion


    }
}
