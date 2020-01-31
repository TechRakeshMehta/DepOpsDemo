using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.Utils;
using Entity;

namespace CoreWeb.Mobility.Views
{
    public interface ICompliancePakageMappingDependenciesView
    {
        Int32 FromTenantId { get; set; }
        String ApplicantFirstName {get; set;}
        String ApplicantLastName { get; set; }
        String SourcePackage { get; set; }
        String TargetPackage { get; set; }
        String SourceNode { get; set; }
        String TargetNode { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 PackageMappingMasterId { get; set; }
        List<Entity.ClientEntity.CompliancePkgMappingDependency> ApplicantPkgMappingDependencyList { get; set; }
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
    }
}




