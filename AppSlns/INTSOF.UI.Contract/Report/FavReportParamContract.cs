using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.Report
{
    public class FavReportParamContract
    {
        public Int32 ReportID { get; set; }
        public String ReportCode { get; set; }
        public String ReportName { get; set; }
        public String FavParamName { get; set; }
        public String FavParamDescription { get; set; }
        public Int32 CreatedByID { get; set; }
        public Dictionary<String, String> ParameterValues { get; set; }
        public Int32 UserID { get; set; } //UAT-3052
    }
}
