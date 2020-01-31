using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class RequirementRuleContract
    {
        [DataMember]
        public Int32 ObjectTreeID { get; set; }

        [DataMember]
        public Int32 ObjectID { get; set; }

        [DataMember]
        public String ObjectTypeCode { get; set; }

        [DataMember]
        public Int32? RequirementObjectRuleId { get; set; }

        [DataMember]
        public String FixedRuleTypeCode { get; set; }

        [DataMember]
        public Boolean IsFieldRequired { get; set; }

        [DataMember]
        public String RequirementFieldRuleTypeCode { get; set; }

        [DataMember]
        public String RequirementFieldRuleTypeValue { get; set; }

        [DataMember]
        public String RequirementFieldDataTypeCode { get; set; }

        [DataMember]
        public Int32? RequirementFieldDataTypeID { get; set; }

        [DataMember]
        public String RequirementFieldDataTypeName { get; set; }

        [DataMember]
        public Boolean IsVideoRequiredToBeOpened { get; set; }

        [DataMember]
        public Int32? VideoOpenedSeconds { get; set; }

        [DataMember]
        public Int32? VideoOpenedMinutes { get; set; }

        /// <summary>
        /// VideoOpenedHours*60 + VideoOpenedMinutes
        /// </summary>
        [DataMember]
        public Int32? VideoOpenTimeDuration { get; set; }
        /// <summary>
        /// used to store "Expires In"/"Expires On" values
        /// </summary>
        [DataMember]
        public String RequirementItemExpirationTypeCode { get; set; }

        [DataMember]
        public String ExpirationDate { get; set; }

        [DataMember]
        public String ExpirationValue { get; set; }

        /// <summary>
        /// used to store "Days"/"Months"/"Years" code => not being popualted currently
        /// </summary>
        [DataMember]
        public String ExpirationValueTypeCode { get; set; }

        /// <summary>
        /// used to store "Days"/"Months"/"Years" ids
        /// </summary>
        [DataMember]
        public Int32? ExpirationValueTypeID { get; set; }

        [DataMember]
        public Int32? SelectedDateTypeFieldId { get; set; }

        [DataMember]
        public Boolean IsRequirementItemNeededExpiration { get; set; }

        [DataMember]
        public String RuleUIExpression { get; set; }

        [DataMember]
        public String RuleSqlExpression { get; set; }

        #region UAT-2165
        [DataMember]
        public DateTime? ExpirationCondStartDate { get; set; }

        [DataMember]
        public DateTime? ExpirationCondEndDate { get; set; }

        [DataMember]
        public String ErrorMessage { get; set; }
        
        //UAT-2366
        [DataMember]
        public Int32? UiRequirementItemID { get; set; }

        [DataMember]
        public Int32? UiRequirementFieldID { get; set; }

        [DataMember]
        public String UiRuleErrorMessage { get; set; }
       
        #endregion
    }
}
