using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class RequirementFieldDataContract
    {
        [DataMember]
        public String RequirementFieldDataTypeCode { get; set; }
        [DataMember]
        public Int32 RequirementFieldDataTypeID { get; set; }
        [DataMember]
        public String RequirementFieldDataTypeName { get; set; }
        [DataMember]
        public RequirementFieldVideoData VideoFieldData { get; set; }
        [DataMember]
        public List<RequirementFieldOptionsData> LstRequirementFieldOptions { get; set; }

        /// <summary>
        /// Used to store formatted value of option type data field. It will be populated only when field data type is "Options"
        /// </summary>
        [DataMember]
        public String RequiredFieldOptionsFormattedValue { get; set; }

        [DataMember]
        public RequirementFieldViewDocumentData FieldViewDocumentData { get; set; }
        [DataMember]
        public Int32? AttributeTypeID { get; set; }

        [DataMember]
        public Int32? AttributeGroupID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class RequirementFieldVideoData
    {
        [DataMember]
        public Int32 RequirementFieldVideoID { get; set; }
        [DataMember]
        public String VideoURL { get; set; }
        [DataMember]
        public String VideoName { get; set; }
        [DataMember]
        public Boolean IsVideoRequiredToBeOpened { get; set; }
        [DataMember]
        public Int32 VideoOpenedSeconds { get; set; }
        [DataMember]
        public Int32 VideoOpenedMinutes { get; set; }
        /// <summary>
        /// VideoOpenedHours*60 + VideoOpenedMinutes
        /// </summary>
        [DataMember]
        public Int32 VideoOpenTimeDuration { get; set; }
    }

    [Serializable]
    [DataContract]
    public class RequirementFieldOptionsData
    {
        [DataMember]
        public Int32 RequirementFieldOptionsID { get; set; }
        [DataMember]
        public String OptionText { get; set; }
        [DataMember]
        public String OptionValue { get; set; }
        [DataMember]
        public Int32 RequirementFieldID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class RequirementFieldViewDocumentData
    {
        [DataMember]
        public Int32 ClientSystemDocumentID { get; set; }
        [DataMember]
        public String DocumentFileName { get; set; }
        [DataMember]
        public String DocumentPath { get; set; }
        [DataMember]
        public Int32 DocumentSize { get; set; }
        [DataMember]
        public String DocumentDescription { get; set; }
        /// <summary>
        /// It denotes the Code column of lkpDocumentAcroFieldType table. It will store the types of acro fields required for document like signature,fullName and currentDate
        /// </summary>
        [DataMember]
        public List<String> LstDocumentAcroFieldTypeCodes { get; set; }
        [DataMember]
        public List<Int32> LstRequirementDocumentAcroFieldIDs { get; set; }
        [DataMember]
        public Boolean IsDocumentUpdated { get; set; }
    }
}





