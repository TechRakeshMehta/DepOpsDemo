using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class PaymentInstructions : BaseUserControl, IPaymentInstructionsView
    {
        public String InstructionsText { get; set; }
        public String PaymentModeText { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            litPaymentInstruction.Text = InstructionsText;

            var _prefixPIType = "Payment Instruction ";
            litPaymentInstructionType.Text = _prefixPIType + "(" + PaymentModeText + ")";
        }
    }
}