using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ComplianceVerificationDetailsContract
    {
        public static String GetItemDivType(String currentItemStatus)
        {
            if (String.IsNullOrEmpty(currentItemStatus))
                return VerificationDetailsItemDivTypes.INCOMPLETE_ITEM;

            else if (currentItemStatus == ApplicantItemComplianceStatus.Exception_Approved.GetStringValue()
                || currentItemStatus == ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue())
                return VerificationDetailsItemDivTypes.EXC_APPROVED_REJECTED;

            else if (currentItemStatus == ApplicantItemComplianceStatus.Pending_Review.GetStringValue()
                || currentItemStatus == ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue()
                || currentItemStatus == ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue())
                return VerificationDetailsItemDivTypes.PENDING_REVIEW;

            else if (currentItemStatus == ApplicantItemComplianceStatus.Not_Approved.GetStringValue()
                || currentItemStatus == ApplicantItemComplianceStatus.Approved.GetStringValue())
                return VerificationDetailsItemDivTypes.ITM_APPROVED_REJECTED;

            else
                return VerificationDetailsItemDivTypes.OTHER_ITEMS;
        }
    }
}
