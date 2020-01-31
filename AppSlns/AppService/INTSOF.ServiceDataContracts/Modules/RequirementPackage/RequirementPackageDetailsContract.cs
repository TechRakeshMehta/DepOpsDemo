using System;
using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class RequirementPackageDetailsContract
    {
        [DataMember]
        public Int32 RequirementPackageID { get; set; }
        [DataMember]
        public String RequirementPackageName { get; set; }
        [DataMember]
        public String RequirementPackageLabel { get; set; }
        [DataMember]
        public Boolean? IsActivePackage { get; set; }
        [DataMember]
        public Boolean IsPackageUsed { get; set; }
        [DataMember]
        public Int32 RequirementCategoryID { get; set; }
        [DataMember]
        public String RequirementCategoryName { get; set; }

        //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes)
        [DataMember]
        public String RequirementDocumentLink { get; set; }

        //UAT-3161
        [DataMember]
        public String RequirementDocumentLinkLabel { get; set; }

        [DataMember]
        public Int32 RequirementItemID { get; set; }
        [DataMember]
        public String RequirementItemName { get; set; }
        [DataMember]
        public Int32 RequirementFieldID { get; set; }
        [DataMember]
        public String RequirementFieldName { get; set; }
        [DataMember]
        public String AgencyNames { get; set; }
        [DataMember]
        public String LstAgencyIDs { get; set; }
        [DataMember]
        public Int32 SelectedTenantID { get; set; }
        [DataMember]
        public Int32 TotalCount { get; set; }
        [DataMember]
        public Int32 PackageObjectTreeID { get; set; }
        [DataMember]
        public String LstSelectedTenantIDs { get; set; }
        [DataMember]
        public Boolean IsSharedUserPackage { get; set; }
        [DataMember]
        public Int32? RequirementPkgTypeID { get; set; }
        [DataMember]
        public String RequirementPkgTypeName { get; set; }
        [DataMember]
        public String InstitutionNames { get; set; }
        [DataMember]
        public Boolean IsSharedUserLoggedIn { get; set; }
        [DataMember]
        public Int32 CurrentLoggedInUserID { get; set; }
        [DataMember]
        public Guid CurrentUserID { get; set; }
        [DataMember]
        public bool ShowAllAgencies { get; set; }
        //UAT-2357: Issue 14: ADB Manage Mappings : Admin navigates to first package when admin clicks on "Back to Search" link from "Manage Mappings" screen.
        [DataMember]
        public Int32 CurrentPageIndex { get; set; }

        [DataMember]
        public Int32? DefinedRequirementID { get; set; }

        [DataMember]
        public String DefinedRequirementDescription { get; set; }

        [DataMember]
        public String LstSelectedAgencyIDs { get; set; }

        [DataMember]
        public String SelectedAgencyHierarchyIds { get; set; }
    }
}



