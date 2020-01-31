using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class RequirementPackageContract
    {

        [DataMember]
        public String RItemURLSampleDocURL { get; set; }
        [DataMember]
        public String RItemURLLabel { get; set; }

        [DataMember]
        public Int32 RequirementPackageID { get; set; }
        [DataMember]
        public Guid RequirementPackageCode { get; set; }
        [DataMember]
        public String RequirementPackageName { get; set; }
        [DataMember]
        public String RequirementPackageLabel { get; set; }
        //[DataMember]
        //public String RequirementPackageDescription { get; set; }
        [DataMember]
        public Int32 PackageRuleTypeID { get; set; }
        [DataMember]
        public String PackageRuleTypeCode { get; set; }
        [DataMember]
        public List<Int32> LstAgencyIDs { get; set; }
        [DataMember]
        public List<Int32> LstAgencyIDsWithNoAgencyUerPermission { get; set; }
        [DataMember]
        public List<String> LstAgencyNames { get; set; }

        /// <summary>
        /// It holds the temp file path for document uploaded(if any) in "ViewDocument" field type. 
        /// It has to be changed everytime while clicking on "Preview and Save Document" button of "ManageField" user control
        /// </summary>
        [DataMember]
        public String TemporaryDocumentPath { get; set; }

        /// <summary>
        /// used to hold file path of temporary document with acro fields saved in it
        /// it is set from "PreviewDocumentWindowPage" 
        /// </summary>
        [DataMember]
        public String TemporaryAcroFieldDocumentPath { get; set; }

        /// <summary>
        /// Used to hold video URL entered by admin during requirement field creation. Used to preview video at admin side
        /// </summary>
        [DataMember]
        public String PreviewVideoURL { get; set; }

        [DataMember]
        public Boolean IsActive { get; set; }
        [DataMember]
        public Int32 FirstVersionID { get; set; }
        [DataMember]
        public Int32 CategoryDisplayOrder { get; set; }
        [DataMember]
        public Boolean IsUpdated { get; set; }
        [DataMember]
        public Boolean IsDeleted { get; set; }
        [DataMember]
        public List<RequirementCategoryContract> LstRequirementCategory { get; set; }
        [DataMember]
        public Int32 PackageObjectTreeID { get; set; }
        [DataMember]
        public Boolean IsFromRotationScreen { get; set; }
        //UAT 1352:As a rotation package creation admin, I should be able to create packages rotation for the instructor/preceptor to use
        [DataMember]
        public Int32 RequirementPkgTypeID { get; set; }
        [DataMember]
        public String RequirementPkgTypeCode { get; set; }
        [DataMember]
        public Dictionary<Int32, String> RequirementPkgTypes { get; set; }

        /// <summary>
        /// This property will be true if package is created by Agency User and to be created in Shared DB
        /// </summary>
        [DataMember]
        public Boolean IsSharedUserLoggedIn { get; set; }
        [DataMember]
        public List<Int32> LstSelectedTenantIDs { get; set; }
        [DataMember]
        public List<Int32> LstRemovedTenantIDs { get; set; }
        [DataMember]
        public List<String> LstSelectedTenantNames { get; set; }
        [DataMember]
        public Boolean IsSharedUserPackage { get; set; }
        [DataMember]
        public Boolean IsUsed { get; set; }
        [DataMember]
        public Boolean IsCopied { get; set; }
        [DataMember]
        public Int32 LoggedInUserAgencyID { get; set; }
        [DataMember]
        public Guid CurrentUserId { get; set; }
        [DataMember]
        public bool IsManageMasterPackage { get; set; }
        [DataMember]
        public Boolean IsCopyWithInMaster { get; set; }

        [DataMember]
        public Int32? DefinedRequirementID { get; set; }

        [DataMember]
        public Int32? ReqReviewByID { get; set; }

        #region UAT-2213
        [DataMember]
        public String AgencyNames { get; set; }

        [DataMember]
        public String lstSelectedAgencyIds { get; set; }

        [DataMember]
        public DateTime? EffectiveStartDate { get; set; }

        [DataMember]
        public DateTime? EffectiveEndDate { get; set; }

        [DataMember]
        public String RequirementPackageType { get; set; }

        [DataMember]
        public DateTime? PackageCreatedDate { get; set; }

        [DataMember]
        public DateTime? RotationEndDate { get; set; }

        [DataMember]
        public Int32 TotalCount { get; set; }

        [DataMember]
        public Boolean IsCategoryMappedWithPkg { get; set; }

        [DataMember]
        public string CategoryIDs { get; set; }

        [DataMember]
        public string AgencyIDs { get; set; }
        [DataMember]
        public Int32? PackageOptions { get; set; }
        [DataMember]
        public Boolean IsNewPackage { get; set; }
        [DataMember]
        public Boolean IsPkgArchived { get; set; }
        [DataMember]
        public CustomPagingArgsContract GridCustomPaging { get; set; }
        [DataMember]
        public Boolean IsArchivedPackage { get; set; }
        #endregion


        [DataMember]
        public Int32 AgencyHierarchyPackageID { get; set; } //UAT-2634


        [DataMember]
        public List<Int32> LstAgencyHierarchyIDs { get; set; } //UAT-2648//

        [DataMember]
        public Dictionary<Int32, String> SelectedAgencyHierarchyDeatils { get; set; }  //UAT-2650

        //UAT-2706
        [DataMember]
        public String RequirementPackageStatus { get; set; }

        [DataMember]
        public String RequirementPackageDescription { get; set; }

        [DataMember]
        public Int32 PageIndex { get; set; }

        [DataMember]
        public String RequirementPackageAgencyHierarchyNode { get; set; }

        #region UAT-3494

        [DataMember]
        public String ListAgencyIdsForCopyPkg { get; set; }

        [DataMember]
        public Int32 existingPackageId { get; set; }

        [DataMember]
        public String RequirementPackageCodeType { get; set; }
        #endregion

        [DataMember]
        public List<RequirementDocumentAcroFieldType> lstDocumentAcroFieldType { get; set; }
        [DataMember]

        public String PackageArchiveState { get; set; }


        #region UAT-4402
        [DataMember]
        public int PackageCategoryCount { get; set; }
        [DataMember]
        public int TempPackageCategoryCount { get; set; }

        #endregion
        #region UAT-4657
        [DataMember]
        public Int32 AddedPkgId { get; set; }
        public String HierarchyIds { get; set; }
        [DataMember]
        public Int32 RootParentID { get; set; }
        [DataMember]
        public Guid RootParentCode { get; set; }

        [DataMember]
        public String ParentPackageName { get; set; }

        [DataMember]
        public bool IsPackageCopy { get; set; }

        [DataMember]
        public Guid? ParentPackageCode { get; set; }
        #endregion
    }
}


