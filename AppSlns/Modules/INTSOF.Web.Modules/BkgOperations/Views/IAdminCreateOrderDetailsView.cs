using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IAdminCreateOrderDetailsView
    {
        Int32 SelectedTenantId { get; set; }
        
        Int32 CurrentLoggedInUserId { get; }

        Int32 OrderID { get; set; }
        Int32 BkgOrderId { get; set; }

        List<Entity.lkpGender> GenderList { set; }

        String FirstName { get; set; }
        String MiddleName { get; set; }
        String LastName { get; set; }
        String PhoneNumber { get; set; }
        String SSN { get; set; }
        DateTime DOB { get; set; }
        String Email { get; set; }
        String Address1 { get; set; }
        String Address2 { get; set; }
        Int32 ZipId { get; set; }
        List<PersonAliasContract> PersonAliasList { get; set; }
        String StateName { set; get; }
        String CityName { set; get; }
        String PostalCode { set; get; }
        Int32 CountryId { set; get; }

        List<BackgroundPackagesContract> lstBkgPackage { get; set; }

        OrganizationUser OrganizationUser { get; set; }
        OrganizationUserProfile OrganizationUserProfile { get; set; }

        Int32 SelectedNodeId { get; set; }
        Int32 HierarchyNodeID { get; set; }
        List<Int32> lstSelectedPackageIds { get; set; }

        String ClientMachineIP { get; }

        Int32 Gender { get; set; }

        List<AdminCreateOrderContract> AdminCreateOrderContract { get; set; }
        List<ApplicantDocumentContract> lstApplicantDocumentContract { get; set; }
        List<ApplicantDocument> lstApplicantDocument { get; set; }

        Boolean AttestFCRAPrevisions { get; set; }

        Boolean IsCustomFormDetailsSave { get; set; }

        List<PreviousAddressContract> ResidentialHistoryList { get; set; }
        List<PreviousAddressContract> ResidentialHistoryListAll { get; set; }
        Boolean IsOrderReadyForTransmit { get; set; }
        //DateTime? DateResidentFrom { get; }

        List<Entity.State> ListStates { get; set; }

        List<AttributeFieldsOfSelectedPackages> lstMvrAttGrp { get; set; }
        List<AttributeFieldsOfSelectedPackages> LstInternationCriminalSrchAttributes { get; set; }
        List<BackgroundOrderData> lstBkgOrderData { get; set; }

        Int32 MinPersonalAliasOccurances
        {
            get;
            set;
        }

        Int32? MaxPersonalAliasOccurances { get; set; }

        Int32 MinResidentailHistoryOccurances { get; set; }

        Int32? MaxResidentailHistoryOccurances { get; set; }

        Boolean IsInternationalPhoneNumber
        {
            get;
            set;
        }
    }
}
