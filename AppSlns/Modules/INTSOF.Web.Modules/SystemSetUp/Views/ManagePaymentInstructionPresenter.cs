using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.SystemSetUp.Views
{
    public class ManagePaymentInstructionPresenter : Presenter<IManagePaymentInstructionView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetlkpPaymentOptions()
        {
            View.lstPaymentOption = BackgroundSetupManager.GetSecurityPaymentOptions(View.DefaultTenantId);
        }

        public void GetCurrentPaymentOptionInstruction()
        {
            if (View.SelectedPaymentOption > 0)
            {
                View.InstructionText = BackgroundSetupManager.GetSecurityPaymentOptionById(View.DefaultTenantId, View.SelectedPaymentOption).InstructionText;
            }
            else
            {
                View.InstructionText = "";
            }
        }

        public void UpdatePaymentOptionInstructionText()
        {
            Entity.lkpPaymentOption lkpPmtOption = BackgroundSetupManager.GetSecurityPaymentOptionById(View.DefaultTenantId, View.SelectedPaymentOption);
            lkpPmtOption.InstructionText = View.InstructionText;
            lkpPmtOption.ModifiedByID = View.CurrentLoggedInUserId;
            lkpPmtOption.ModifiedOn = DateTime.UtcNow;
            if (BackgroundSetupManager.UpdateSecurityChanges(View.DefaultTenantId))
            {
                View.SuccessMessage = "Payment Option Instruction Text updated successfully.";
            }
            else
            {
                View.ErrorMessage = "Some error has occurred.Please contact administrator.";
            }
        }

    }
}
