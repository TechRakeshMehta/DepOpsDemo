using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class ApplicantRequiredDocumentsContract
    {
        public String NavigationURL { get; set; }

        //UAT-3819
        public String NavigationLabel { get; set; }

        public Int32 ParentID { get; set; }

        public String Code { get; set; }

        public String DataValue { get; set; }

        public Int32 DataID { get; set; }
    }
}
