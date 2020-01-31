using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [DataContract]
    [Serializable]
    public class PersonAliasContract
    {
        [DataMember]
        public Int32 ID { get; set; }
        [DataMember]
        public String FirstName { get; set; }
        [DataMember]
        public String LastName { get; set; }
        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
        [DataMember]
        public String MiddleName { get; set; }
        //public String UniqueId { get; set; }
        /// <summary>
        /// Represents the Sequence in which they were added in XML
        /// </summary>
        [DataMember]
        public Int32 AliasSequenceId { get; set; }
        [DataMember]
        public String Suffix { get; set; }
        [DataMember]
        public Int32? SuffixID { get; set; }
    }
}
