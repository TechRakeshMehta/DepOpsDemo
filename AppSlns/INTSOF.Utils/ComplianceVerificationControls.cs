using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace INTSOF.Utils
{
    public class ComplianceVerificationControls
    {
        public Int32 ComplianceItemID
        {
            get;
            set;
        }
        
        public VerificationDataMode VerificationDataMode
        {
            get;
            set;
        }

        public Control EditModeCntrlsList
        {
            get;
            set;
        }
    }
}
