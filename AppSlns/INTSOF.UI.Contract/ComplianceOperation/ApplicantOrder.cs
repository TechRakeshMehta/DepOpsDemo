using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class ApplicantOrder
    {
        public Int32 OrderId { get; set; }
        public List<Int32> DPPS_Id { get; set; }
        public OrganizationUserProfile OrganizationUserProfile { get; set; }
        public Boolean UpdatePersonalDetails { get; set; }
        public String ClientMachineIP { get; set; }
        public List<Int32> LstOrderStageTrackID { get; set; }
        public Boolean IsOnlineOrderFailed { get; set; }

        /// <summary>
        /// List of all the BackgroundPackages selected in the order.
        /// </summary>
        public List<BackgroundPackagesContract> lstPackages { get; set; }

        /// <summary>
        /// List of Data of all the Forms
        /// </summary>
        public List<BackgroundOrderData> lstBackgroundOrderData { get; set; }

        /// <summary>
        /// Stores the total number of steps, including the count of Custom order forms in order flow
        /// </summary>
        public Int32 TotalOrderSteps{ get; set; }

        /// <summary>
        /// To manage the next step number, store the Step number of the screen from which the next step is opened
        /// Nest step number will be this number + 1
        /// This will easily manage the steps for all the cases like Renew order, Rush order for existing order, Change subscription etc.
        /// </summary>
        public Int32 PreviousOrderStep { get; set; }

        /// <summary>
        /// XML with the Pricing calculation from the Stored procedure
        /// </summary>
        public String PricingDataXML { get; set; }

        public Int32 MVRDvrLicenseNumberID { get; set; }

        public Int32 MVRDvrLicenseNumberStateID { get; set; }

        /// <summary>
        /// Set true/false to Send Background Report of order-from BkgOrder-orderresultrequestedbyapplciant 
        /// </summary>
        public Boolean IsSendBackgroundReport { get; set; }

        public Boolean MVRIsValidDriverLicenseAndState { get; set; }

        //UAT-1578 : Addition of SMS notification
        public Boolean IsReceiveTextNotification { get; set; }
        public String PhoneNumber { get; set; }

        public String OrderNumber { get; set; }
        public Boolean IsHavingSSN { get; set; }

        public Int32? SelectedCommLang { get; set; }
        public Int32? SelectedMailingOption { get; set; }
        public decimal? MailingPrice { get; set; }
    }


    /// <summary>
    /// Stores Custom form data related to the Background Packages
    /// </summary>
    [Serializable]
    public class BackgroundOrderData
    {
        /// <summary>
        /// CF_ID - PK of ams.CustomForm in Security database
        /// </summary>
        public Int32 CustomFormId { get; set; }

        /// <summary>
        /// BSAD_ID - PK of ams.BkgSvcAttributeGroup in Tenant database
        /// </summary>
        public Int32 BkgSvcAttributeGroupId { get; set; }

        /// <summary>
        /// Id of Particular Instance of a repeatable Group
        /// </summary>
        public Int32 InstanceId { get; set; }

        /// <summary>
        /// Actual data of the Forms, entered by the applicant
        /// KEY - BAGM_ID - PK of ams.BkgAttributeGroupMapping in client database
        /// VALUE - Attribute Value
        /// </summary>
        public Dictionary<Int32, String> CustomFormData { get; set; }

        public Dictionary<Int32, String> CustomFormIntPhoneNumExtraData { get; set; }
    }
}
