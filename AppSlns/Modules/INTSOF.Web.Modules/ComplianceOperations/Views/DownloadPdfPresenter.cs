using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public class DownloadPdfPresenter : Presenter<IDownloadPdfView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public DownloadPdfPresenter([CreateNew] IComplianceOperationsController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }


        public Boolean DeleteTempFile(Guid Id)
        {
            return SecurityManager.DeleteTempFile(Id, View.OrgUsrID);
        }
        public void GetFilePath(Guid id)
        {
            List<TempFile> tempRecords = SecurityManager.GetFilePath(id);

            if (tempRecords != null)
            {
                View.FilePath = tempRecords[0].TF_Path;
                View.PdfFileName = tempRecords[0].TF_FileName;
            }
        }

        // TODO: Handle other view events and set state in the view
        public Boolean SaveRecieptDocument(Int32 tenantID, String pdfDocPath, String filename, Int32 fileSize, String documentTypeCode, Int32 CurrentLoggedInUserID, Int32 orgUserID)
        {
            Boolean retVal=true;
            if (View.OrderIDs!=null)
            {
                foreach (Int32 orderId in View.OrderIDs)
                {
                    if (!ComplianceSetupManager.SaveRecieptDocument(tenantID, pdfDocPath, filename, fileSize, documentTypeCode, CurrentLoggedInUserID, orderId, orgUserID))
                        retVal = false;
                }
            }
            return retVal;
        }

        public Boolean IsReciptAlreadySaved()
        {
            Boolean retVal = true;
            if (View.OrderIDs!=null)
            {
                foreach (Int32 orderId in View.OrderIDs)
                {
                    if (!ComplianceSetupManager.IsReciptAlreadySaved(View.TenantID, orderId))
                        retVal = false;
                }
            }
            return retVal;
        }

        #region UAT-2970
        public List<OrderPaymentDetail> GetAllPaymentDetailsOfOrderByOrderID(Int32 tenantId, Int32 OrderId)
        {
            return ComplianceDataManager.GetAllPaymentDetailsOfOrderByOrderID(tenantId, OrderId);
        }
        public void SendOrderConfirmationDocEmail(Int32 tenantId, Int32 currentLoggedInUser, OrderPaymentDetail opd, String orderNumber)
        {
            CommunicationManager.SendOrderConfirmationDocEmail(tenantId, currentLoggedInUser, opd, orderNumber);
        }
        #endregion
    }
}




