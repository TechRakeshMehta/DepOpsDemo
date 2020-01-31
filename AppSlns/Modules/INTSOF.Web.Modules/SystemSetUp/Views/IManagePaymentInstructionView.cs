using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.SystemSetUp.Views
{
    public interface IManagePaymentInstructionView
    {
        IManagePaymentInstructionView CurrentViewContext
        {
            get;
        }

        Int32 SelectedPaymentOption
        {
            get;
            set;
        }

        List<lkpPaymentOption> lstPaymentOption
        {
            get;
            set;
        }


        String InstructionText
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }



        String SuccessMessage
        {
            get;
            set;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 DefaultTenantId
        {
            get;
            set;
        }

    }
}
