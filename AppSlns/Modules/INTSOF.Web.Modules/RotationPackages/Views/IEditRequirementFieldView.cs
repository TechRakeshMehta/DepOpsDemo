using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.RotationPackages.Views
{
    public interface IEditRequirementFieldView
    {
        IEditRequirementFieldView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 RotationPackageID { get; set; }
        String PreviousPage { get; set; }
        String CategoryName { get; set; }
        List<String> LstDocumentFieldCodes { get; }
        Boolean IsDocumentSaved { get; }
        RequirementPackageContract RequirementPackageContractSessionData { get; set; }
        Boolean IsSharedUser { get; }
        List<RequirementItemContract> lstCategoryItems
        {
            get;
            set;
        }

        Int32 RequirementCategoryID
        {
            get;
            set;
        }

        Int32 RequirementItemID
        {
            get;
            set;
        }

        List<RequirementFieldContract> lstItemFields
        {
            get;
            set;
        }

        Int32 OrganisationUserID { get; }
    }
}



