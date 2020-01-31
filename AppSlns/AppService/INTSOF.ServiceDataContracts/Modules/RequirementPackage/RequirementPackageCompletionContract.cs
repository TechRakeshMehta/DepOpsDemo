using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class RequirementPackageCompletionContract
    {
        [DataMember]
        public Int32 RequirementPackageID { get; set; }

        [DataMember]
        public String RequirementPackageName { get; set; }

        [DataMember]
        public Boolean IsPackageInComplete { get; set; }

        [DataMember]
        public Boolean IsPackageRuleInComplete { get; set; }

        [DataMember]
        public Boolean IsPackageWithoutCategory { get; set; }

        [DataMember]
        public Boolean IsCategoryIncomplete { get; set; }

        [DataMember]
        public Boolean IsCategoryRuleIncomplete { get; set; }

        [DataMember]
        public Boolean IsItemIncomplete { get; set; }

        [DataMember]
        public Boolean IsItemRuleIncomplete { get; set; }

        [DataMember]
        public Boolean IsFieldIncomplete { get; set; }

        [DataMember]
        public Boolean IsFieldRuleIncomplete { get; set; }

        [DataMember]
        public List<String> IncompleteCategoryNames { get; set; }

        [DataMember]
        public List<String> IncompleteItemNames { get; set; }

        [DataMember]
        public List<String> IncompleteFieldNames { get; set; }

        [DataMember]
        public List<String> CategoriesWithoutRule { get; set; }

        [DataMember]
        public List<String> ItemsWithoutRule { get; set; }

        [DataMember]
        public List<String> FieldsWithoutRule { get; set; }

    }
}


