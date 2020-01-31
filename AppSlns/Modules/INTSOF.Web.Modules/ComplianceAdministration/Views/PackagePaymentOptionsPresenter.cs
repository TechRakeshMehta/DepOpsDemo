using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class PackagePaymentOptionsPresenter : Presenter<IPackagePaymentOptions>
    {
        /// <summary>
        /// Get the Payment Options, based on DPPId and the Package type selected i.e. Compliance or Background Type
        /// </summary>
        public void BindPaymentOptions()
        {
            View.PaymentOptions = StoredProcedureManagers.GetPackagePaymentOptions(View.PkgNodeMappingId, View.PackageTypeCode, View.TenantId).ToList();

            //UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.
            //Get Payment Approval List
            String notSpecifiedApprovalCode = PaymentApproval.NOT_SPECIFIED.GetStringValue();
            var paymentApprovalList = LookupManager.GetLookUpData<lkpPaymentApproval>(View.TenantId).Where(con => con.PA_IsDeleted == false).ToList();
            if (paymentApprovalList.IsNotNull())
            {
                foreach (var paymentApproval in paymentApprovalList)
                {
                    if (paymentApproval.PA_Code == PaymentApproval.APPROVAL_REQUIRED_BEFORE_PAYMENT.GetStringValue())
                        paymentApproval.PA_Name = AppConsts.YES;
                    else if (paymentApproval.PA_Code == PaymentApproval.APPROVAL_NOT_REQUIRED_BEFORE_PAYMENT.GetStringValue())
                        paymentApproval.PA_Name = AppConsts.NO;
                }

                View.NotSpecifiedPaymentApprovalID = paymentApprovalList.FirstOrDefault(con => con.PA_Code == notSpecifiedApprovalCode).PA_ID;
                View.PaymentApprovalList = paymentApprovalList;
            }

            //PkgNodeMappingId > 0 means edit mode, it Will be DPP_ID for Compliance and BPHM_ID for Background
            if (View.PkgNodeMappingId > 0)
            {
                if (View.PackageTypeCode == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue())
                {
                    var deptProgramPackage = ComplianceSetupManager.GetDeptProgramPackageByID(View.PkgNodeMappingId, View.TenantId);
                    if (deptProgramPackage.IsNotNull())
                    {
                        if (deptProgramPackage.DPP_PaymentApprovalID.IsNotNull())
                            View.PaymentApprovalID = Convert.ToInt32(deptProgramPackage.DPP_PaymentApprovalID);
                    }
                }
                else if (View.PackageTypeCode == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue())
                {
                    var bkgPackageHierarchyMapping = BackgroundSetupManager.GetBackgroundPackageDetail(View.PkgNodeMappingId, View.TenantId);
                    if (bkgPackageHierarchyMapping.IsNotNull())
                    {
                        if (bkgPackageHierarchyMapping.BPHM_PaymentApprovalID.IsNotNull())
                            View.PaymentApprovalID = Convert.ToInt32(bkgPackageHierarchyMapping.BPHM_PaymentApprovalID);
                    }
                }
            }
        }

        //#region UAT-3268
        //public void BindAdditionalPricePaymentOption()
        //{
        //    var lstPaymentOptions = LookupManager.GetLookUpData<Entity.ClientEntity.lkpPaymentOption>(View.TenantId).Where(con => con.IsDeleted == false).ToList();
        //    if (!lstPaymentOptions.IsNullOrEmpty())
        //    {
        //        View.AdditionalPaymentOptions = lstPaymentOptions.Where(cond => cond.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()
        //                                                                     || cond.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()).ToList();
        //    }
        //}
        //#endregion
    }
}
