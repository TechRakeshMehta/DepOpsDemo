using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.Globalization
{
    [Serializable]
    [DataContract]
    public class LanguageContract
    {
        [DataMember]
        public int LanguageID { get; set; }

        [DataMember]
        public string LanguageName { get; set; }

        [DataMember]
        public string LanguageCode { get; set; }

        [DataMember]
        public string LanguageCulture { get; set; }
    }
}