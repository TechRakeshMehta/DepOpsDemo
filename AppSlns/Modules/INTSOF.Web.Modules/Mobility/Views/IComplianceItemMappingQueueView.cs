using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.Utils;
using Entity;

namespace CoreWeb.Mobility.Views
{
    public interface IComplianceItemMappingQueueView
    {
        Int32 TenantId { get; set; }
        List<Entity.Tenant> lstTenant { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        String SourcePackage { get; set; }
        String TargetPackage { get; set; }

        Int32 SelectedSourceTenantId
        {
            get;
            set;
        }

       


        List<Entity.ApplicantTransitionMappingList> ApplicantSearchData { get; set; }
        List<Entity.lkpPkgMappingStatu> lstMappingStatus
        {
            set;
        }

        Int16? SelectedMappingStatusID
        {
            get;
            set;
        }
        List<Entity.ClientEntity.DeptProgramPackage> lstPackage
        {
            get;
            set;
        }
       
        String ErrorMessage { get; set; }

        String SuccessMessage { get; set; }
        String InfoMessage { get; set; }
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

        //UAT-2258
        String SelectedSourceTenantIds { get; set; }
    }
}




