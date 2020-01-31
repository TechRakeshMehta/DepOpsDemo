using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class FingerPrintArchivedOrderPresenter : Presenter<IArchivedOrderView>
    {
        #region Variables

        #region Private Variables



        #endregion

        #region Public Variables



        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties



        #endregion

        #endregion

        #region Methods

        #region Public Methods

        public override void OnViewLoaded()
        {

        }

        public void GetBkgPkgPrevOrderDetails(Int32 organizationUserId, Int32 tenantId)
        {
            if (View.IsApplicant)
            {
                View.FPArchivedOrderList = new List<FingerPrintArchivedOrderContract>();
                if (View.TenantId > 0)
                {
                    View.FPArchivedOrderList = FingerPrintDataManager.GetBkgPkgPrevOrderDetails(organizationUserId, tenantId).ToList();
                }
            }
        }
        #endregion

        #region Get Custom form data of Archived order

        //public Order GetOrderByOrderId(Int32 orderId)
        //{
        //    return ComplianceDataManager.GetOrderById(View.TenantId, orderId);
        //}

        //public List<OrderPaymentDetail> GetOPDOfPaidPaymentStatus(Order currentOrder)
        //{
        //    List<OrderPaymentDetail> paidPaymentDetail = new List<OrderPaymentDetail>();
        //    if (!currentOrder.IsNullOrEmpty() && !currentOrder.OrderPaymentDetails.IsNullOrEmpty())
        //    {
        //        String paidPaymentCode = ApplicantOrderStatus.Paid.GetStringValue();
        //        String ordrPkgTypeComplianceRushOrderCode = OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue();
        //        var paidPaymentDetailTemp = currentOrder.OrderPaymentDetails.Where(cnd => cnd.lkpOrderStatu != null
        //                                                                             && cnd.lkpOrderStatu.Code == paidPaymentCode && !cnd.OPD_IsDeleted).ToList();

        //        foreach (var opd in paidPaymentDetailTemp)
        //        {
        //            if (!opd.OrderPkgPaymentDetails.Any(OPPD => !OPPD.OPPD_IsDeleted && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeComplianceRushOrderCode))
        //            {
        //                paidPaymentDetail.Add(opd);
        //            }
        //        }
        //    }

        //    return paidPaymentDetail;
        //}

        //public Boolean IsBackgroundPackageIncluded(List<OrderPaymentDetail> paidPaymentDetailList)
        //{
        //    String ordrPkgTypeBkgCode = OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue();
        //    Boolean isBackgroundPackageIncluded = false;
        //    if (!paidPaymentDetailList.IsNullOrEmpty())
        //    {
        //        foreach (var opd in paidPaymentDetailList)
        //        {
        //            isBackgroundPackageIncluded = opd.OrderPkgPaymentDetails.Any(OPPD => OPPD.OPPD_BkgOrderPackageID != null && !OPPD.OPPD_IsDeleted
        //                                                                  && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeBkgCode);
        //            if (isBackgroundPackageIncluded)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //    return isBackgroundPackageIncluded;
        //}

        //public List<BkgOrderPackage> GetBkgOrderPackageDetail(List<OrderPaymentDetail> paidPaymentDetailList)
        //{
        //    String ordrPkgTypeBkgCode = OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue();
        //    List<Int32> lstBopIDs = new List<Int32>();
        //    List<BkgOrderPackage> lstBkgOrderPackageList = new List<BkgOrderPackage>();
        //    foreach (var opd in paidPaymentDetailList)
        //    {
        //        var bopIds = opd.OrderPkgPaymentDetails.Where(OPPD => OPPD.OPPD_BkgOrderPackageID != null && !OPPD.OPPD_IsDeleted
        //                                                       && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeBkgCode).Select(slct => slct.OPPD_BkgOrderPackageID.Value)
        //                                                       .ToList();
        //        lstBopIDs.AddRange(bopIds);
        //    }
        //    View.BopIds = lstBopIDs;
        //    lstBkgOrderPackageList = BackgroundProcessOrderManager.GetBackgroundOrderPackageListById(View.TenantId, lstBopIDs);
        //    return lstBkgOrderPackageList;

        //}

        ///// <summary>
        ///// Get MVR Attribute Data 
        ///// </summary>
        ///// <param name="packageIds">packageIds</param>
        ///// <returns></returns>
        //public void GetAttributeFieldsOfSelectedPackages(String packageIds)
        //{
        //    List<Entity.ClientEntity.AttributeFieldsOfSelectedPackages> lstAttributeFields = BackgroundProcessOrderManager.GetAttributeFieldsOfSelectedPackages(packageIds, View.TenantId);
        //    if (!lstAttributeFields.IsNullOrEmpty())
        //    {
        //        View.lstAttrMVRGrp = lstAttributeFields.Where(cond => (cond.BSA_Code.ToUpper().Equals("1ADA97AE-9100-4BE6-B829-C914B7FA8750")
        //                                                                || cond.BSA_Code.ToUpper().Equals("515BEF57-9072-4D2A-A97A-0C248BB045F9"))
        //                                                               && cond.AttributeGrpCode.ToUpper().Equals("CF76960D-2120-46FE-9E03-01C218F8A336")).ToList();
        //    }
        //    else
        //    {
        //        View.lstAttrMVRGrp = new List<AttributeFieldsOfSelectedPackages>();
        //    }
        //}

        ///// <summary>
        ///// Get Attribute Custom Form Data.
        ///// </summary>
        ///// <param name="masterOrderId">masterOrderId</param>
        ///// <param name="isIncludeMvrData">isIncludeMvrData</param>
        //public void GetAttributesCustomFormIdOrderId(Int32 masterOrderId, Boolean isIncludeMvrData)
        //{
        //    String bopIds = String.Join(",", View.BopIds);

        //    BkgOrderDetailCustomFormDataContract bkgOrderDetailCustomFormDataContract = BackgroundProcessOrderManager.GetBkgORDCustomFormAttrDataForCompletingOrder
        //                                                                                (View.TenantId, masterOrderId, bopIds, isIncludeMvrData);
        //    if (bkgOrderDetailCustomFormDataContract.IsNotNull())
        //    {
        //        if (bkgOrderDetailCustomFormDataContract.lstDataForCustomForm.IsNotNull())
        //            View.lstDataForCustomForm = bkgOrderDetailCustomFormDataContract.lstDataForCustomForm;
        //        else
        //            View.lstDataForCustomForm = new List<BkgOrderDetailCustomFormUserData>();
        //    }

        //}

        #endregion

        #endregion

    }
}
