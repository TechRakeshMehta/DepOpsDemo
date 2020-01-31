using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class ModifyShippingInfoPresenter : Presenter<IModifyShippingInfoView>
    {
        public override void OnViewInitialized()
        {
            View.OrganizationUser = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId);
        }

        public void IsLocationServiceTenant()
        {
            View.IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(View.TenantId);
        }

        public void GetMailingOption()
        {
            var FeeItemId = SecurityManager.GetFeeItemID();
            View.lstMailingOptionsWithPrice = BackgroundPricingManager.GetServiceItemFeeRecordContract(SecurityManager.DefaultTenantID, FeeItemId);
        }

        /// <summary>
        /// Get Order  by order id.
        /// </summary>
        /// <param name="orderId">orderId</param>
        /// <returns></returns>
        //public Order GetOrderByOrderId(Int32 orderId)
        //{
        //    return ComplianceDataManager.GetOrderById(View.TenantId, orderId);
        //}

        public void SaveModifyShippingData(ApplicantOrderCart applicantOrderCart)
        {
            SecurityManager.SaveModifyShippingData(applicantOrderCart, View.IsLocationServiceTenant, View.CurrentLoggedInUserId);
        }

        public void SendModifyShippingNotification(Int32 OrderID, string OrderNumber, PreviousAddressContract mailingAddress)
        {
            //Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();            
            //mockData.UserName = string.Concat(View.OrganizationUser.FirstName, " ", View.OrganizationUser.LastName);
            //mockData.EmailID = View.OrganizationUser.PrimaryEmailAddress;
            //mockData.ReceiverOrganizationUserID = View.OrganizationUser.OrganizationUserID;

            //Dictionary<String, object> dictMailData = new Dictionary<String, object>();
            //dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(View.OrganizationUser.FirstName, " ", View.OrganizationUser.LastName));

            //Int32 _heirarchyNodeId = FingerPrintDataManager.GetOrderHeirarchyNodeId(View.TenantId, OrderID);            
            //CommunicationManager.SentMailMessageNotification(CommunicationSubEvents.NOTIFICATION_FOR_MODIFYING_SHIPPING_ADDRESS, mockData, dictMailData, View.OrganizationUser.OrganizationUserID, View.TenantId, _heirarchyNodeId);
            CommunicationManager.SendModifyShippingNotification(View.OrganizationUser, View.TenantId, OrderID, OrderNumber, mailingAddress);
        }        

        public Address GetMailingAddressDetails(Int32 TenantID, Int32 OrderID)
        {
            return FingerPrintDataManager.GetMailingAddressDetails(TenantID, OrderID, View.CurrentLoggedInUserId);
        }
        public SelectedMailingData GetSelectedMailingOptionPrice(Int32 TenantID, Int32 OrderID)
        {
            return FingerPrintDataManager.GetSelectedMailingOptionPrice(TenantID, OrderID, View.CurrentLoggedInUserId);
        }

        public decimal GetSentForOnlinePaymentAmount(Int32 TenantID, Int32 OrderID)
        {
            return FingerPrintDataManager.GetSentForOnlinePaymentAmount(TenantID, OrderID, View.CurrentLoggedInUserId);
        }

        public PreviousAddressContract GetAddressData(Int32 TenantID, Int32 OrderID)
        {
            return FingerPrintDataManager.GetAddressData(TenantID, OrderID, View.CurrentLoggedInUserId);
        }
        public Boolean IsPrinterAvailableAtOldLoc(Int32 OrderId)
        {
            return FingerPrintSetUpManager.IsPrinterAvailableAtOldLoc(OrderId, View.TenantId);
        }

        public PreviousAddressContract GetShippingAddressData(Int32 TenantID, Int32 OrderID)
        {
            return FingerPrintDataManager.GetShippingAddressData(TenantID, OrderID, View.CurrentLoggedInUserId);
        }

        #region UAT-5088
        public bool SaveOrderPaymentInvoice(Int32 tenantId, Int32 orderID, Int32 currentLoggedInUserId, Boolean modifyShipping)
        {
            return ComplianceDataManager.SaveOrderPaymentInvoice(tenantId, orderID, currentLoggedInUserId, modifyShipping);
        }
        /// <summary>
        /// Get Bkg Order Service Details xml
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public string GetBkgOrderServiceDetails(int orderID)
        {
            return BackgroundProcessOrderManager.GetBkgOrderServiceDetails(Convert.ToInt32(View.TenantId), orderID);
        }
        #endregion
        //To update mailing address detail.
        public Guid UpdateMailingAddress(PreviousAddressContract mailingAddress)
        {
            Guid mailingaddressHandleId = SecurityManager.UpdateMailingAddress(mailingAddress, View.IsLocationServiceTenant, View.CurrentLoggedInUserId);
            return  mailingaddressHandleId;
        }
        public void UpdateMailingDetailXML(Int32 tenantId, Int32 orderID,Guid mailingAddressHandleId,String mailingoptionID,String mailingOptionPrice)
        {
            FingerPrintDataManager.UpdateMailingDetailXML(tenantId, orderID, mailingAddressHandleId,  mailingoptionID, mailingOptionPrice);
        }


    }
}
