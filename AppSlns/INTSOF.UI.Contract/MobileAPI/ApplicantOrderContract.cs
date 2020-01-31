using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace INTSOF.UI.Contract.MobileAPI
{
    [DataContract]
    public class ApplicantOrderContract
    {
        [DataMember]
        public List<AttributesForCustomFormContract> lstCustomAttribute { get; set; }
        [DataMember]
        public List<AttributesForCustomFormContract> lstCustomAttributeInSpanish { get; set; }
        [DataMember]
        public List<ApplicantOrderPaymentOptions> selectedPaymentModeData { get; set; }
        [DataMember] 
        public List<Entity.ClientEntity.BkgSvcAttributeGroup> lstSvcAttributeGrps { get; set; }
        [DataMember]
        public List<PkgPaymentGrouping> lstPaymentGrouping { get; set; }
        [DataMember]
        public FingerPrintAppointmentContract LocationDetail { get; set; }
        [DataMember]
        public UserContract userInfo { get; set; }
        [DataMember] 
        public Int32 OrderID { get; set; }
        [DataMember]
        public Int32 bkgPackageID { get; set; }
        [DataMember]
        public List<Int32> lstDepProgramMappingId { get; set; }
        [DataMember]
        public Int32 DepartmentId { get; set; }
        [DataMember]
        public Int32 SelectedHierarchyNodeID { get; set; }
        [DataMember]
        public Int32 bkgPkgHierarchyMappingID { get; set; }
        [DataMember]
        public Int32 customFormID { get; set; }
        [DataMember]
        public String ClientMachineIP { get; set; }
        [DataMember]
        public Decimal TotalPrice { get; set; }
        [DataMember]
        public Decimal GrandTotal { get; set; }
        [DataMember]
        public List<mailingInfoAtt> lstMailingAttribute { get; set; }
        [DataMember]
        public Int32 customFormInstanceId { get; set; }
        [DataMember]
        public String CbiUniqueId { get; set; }
        [DataMember]
        public String packageName { get; set; }
        [DataMember]
        public String selectedOrderType { get; set; }
        [DataMember]
        public String OrderNumber { get; set; }
        [DataMember]
        public String CbiBillingCode { get; set; }
        [DataMember]
        public String PaymentType { get; set; }
        [DataMember]
        public string LanguageCode { get; set; }
        [DataMember]
        public String BillingCodeAmount { get; set; }
        [DataMember]
        public List<PaymentTypeDetails> lstPaymentDataDetail { get; set; }
        //public ApplicantOrderContract()
        //{
        //    lstMailingAttribute = new List<mailingInfoAtt>();
        //    userInfo = new UserContract();
        //    LocationDetail = new FingerPrintAppointmentContract();
        //    lstCustomAttribute = new List<AttributesForCustomFormContract>();
        //}
    }
    [DataContract]
    public class mailingInfoAtt
    {
        [DataMember]
        public Int32 AttributeGroupID { get; set; }
        [DataMember]
        public Int32 AttributeID { get; set; }
        [DataMember]
        public Int32 InstanceId { get; set; }
        [DataMember]
        public String HeaderLabel { get; set; }
        [DataMember]
        public bool IsRequired { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public bool IsAttributeHidden { get; set; }
        [DataMember]
        public bool IsAttributeGroupHidden { get; set; }
    }

    [DataContract]
    public class OrderReceiptDetails
    {
        [DataMember]
        public String OrderNumber { get; set; }
        [DataMember]
        public String InvoiceNumber { get; set; }
        [DataMember]
        public Int32 OrderID { get; set; }


    }
    [DataContract]
    public class PaymentTypeDetails
    {
        [DataMember]
        public String PaymentType { get; set; }
        [DataMember]
        public Decimal Amount { get; set; }
        [DataMember]
        public String PaymentTypeCode { get; set; }
        [DataMember]
        public String InstructionText { get; set; }
    }

}