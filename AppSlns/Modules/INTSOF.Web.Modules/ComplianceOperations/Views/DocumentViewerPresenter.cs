using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
//using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using System.Linq;

namespace CoreWeb.ComplianceOperations.Views
{
    public class DocumentViewerPresenter : Presenter<IDocumentViewerView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public DocumentViewerPresenter([CreateNew] IComplianceOperationsController controller)
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

        public ApplicantDocument GetApplicantDocument()
        {
            return ComplianceDataManager.GetApplicantDocument(View.ApplicantDocumentId, View.TenantId);
        }

        public ApplicantDocumentContract GetClientSystemDocument()
        {
            return ApplicantRequirementManager.GetClientSystemDocument(View.ApplicantDocumentId, View.TenantId);
        }

        public ApplicantDocument GetFailedUnifiedApplicantDocument()
        {
            return ComplianceDataManager.GetFailedUnifiedApplicantDocument(View.ApplicantDocumentId, View.TenantId);
        }

        public Entity.OrganizationUser getOrganisationUser(Int32 userId)
        {
            return SecurityManager.GetOrganizationUser(userId);
        }

        public void GetLoginImageUrl()
        {
            if (View.WebsiteId > 0)
                View.LoginImageUrl = WebSiteManager.GetWebSiteLoginImage(View.WebsiteId);
        }

        public void GetRightLogoImageUrl()
        {
            if (View.WebsiteId > 0)
                View.RightLogoImageUrl = WebSiteManager.GetWebSiteRightLogoImage(View.WebsiteId);
        }

        public UnifiedPdfDocument GetUnifiedDocumentData()
        {
            return DocumentManager.GetPdfAsUnifiedDocument(View.TenantId, View.loggedinUserID);
        }

        /// <summary>
        /// Method to get the Disclosure document
        /// </summary>
        /// <returns></returns>
        public SystemDocument GetDisclosureDocument()
        {
            if (View.SystemDocumentID > 0)
            {
                return ComplianceSetupManager.GetDisclosureDocument(View.TenantId, View.SystemDocumentID);
            }
            return null;
        }

        public ApplicantDocument GetESignedeDocument(String documentTypeCode)
        {
            if (View.ApplicantDocumentId > 0)
            {
                return ComplianceSetupManager.GetESignedeDocument(View.TenantId, View.ApplicantDocumentId, documentTypeCode);
            }
            return null;
        }

        /// <summary>
        /// Method to get the ServiceForm document
        /// </summary>
        /// <returns></returns>
        public SystemDocument GetServiceFormDocumentData()
        {
            if (View.SystemDocumentID > 0)
            {
                return ComplianceSetupManager.GetServiceFormDocument(SecurityManager.DefaultTenantID, View.SystemDocumentID);
            }
            return null;
        }

        // TODO: Handle other view events and set state in the view

        public ApplicantDocument GetApplicantDocumentForEds()
        {
            String documentTypeCode = DocumentType.EDS_AuthorizationForm.GetStringValue();
            String recordTypeCode = RecordType.BackgroundProfile.GetStringValue();
            if (View.OrderID > 0 && View.TenantId > 0)
            {
                return BackgroundProcessOrderManager.GetApplicantDocumentForEds(View.TenantId, View.OrderID, documentTypeCode, recordTypeCode);
            }
            return null;
        }

        public ApplicantDocument GetRecieptDocumentDataForOrderID(int orderID)
        {
            if (View.TenantId > 0 && orderID > 0)
            {
                return ComplianceSetupManager.GetRecieptDocumentDataForOrderID(View.TenantId, orderID);
            }
            return null;
        }

        #region Profile Sharing Invitation Document
        public InvitationDocument GetInvitationDocument()
        {
            if (View.InvitationId > 0)
            {
                return ProfileSharingManager.GetInvitationDocuments(View.InvitationId, View.AttestationTypeCode);
            }
            return null;
        }
        #endregion

        #region UAT-1176 - EMPLOYMENT DISCLOSURE
        /// <summary>
        /// Method to Get Employment Disclosure Document by System Document ID 
        /// </summary>
        /// <param name="aWSUseS3"></param>
        public Entity.SystemDocument GetEmploymentDisclosureDocument()
        {
            return SecurityManager.GetEmploymentDisclosureDocument();
        }
        #endregion

        #region UAT-178

        public Entity.UserAttestationDetail GetPartiallyFilledUserAttestationDocument()
        {
            return SecurityManager.GetPartiallyFilledUserAttestationDocument(View.UserAttestationDocumentID);
        }

        //public Entity.SystemDocument GetUserAttestationDisclosureDocument(bool isClientAdmin)
        //{
        //    return SecurityManager.GetUserAttestationDisclosureDocument(isClientAdmin);
        //}

        //public Entity.UserAttestationDetail FillUserAttestationDocumentWithPrePopulatedData(Boolean isClient)
        //{
        //    return SecurityManager.FillUserAttestationDocumentWithPrePopulatedData(View.OrganizationUserID, View.DocumentType);
        //}

        #endregion

        public Byte[] GetFilledEmployementDisclosureDocument()
        {
            return SecurityManager.FillEmploymentDisclosureWithPrePopulatedData(View.DocumentPath, View.OrganizationUserID);
        }

        #region UAT-1201
        public InvitationDocument GetInvitationDocumentByDocumentID()
        {
            if (View.InvitationDocumentID > 0)
            {
                return ProfileSharingManager.GetInvitationDocumentByDocumentID(View.InvitationDocumentID);
            }
            else
            {
                return ProfileSharingManager.GetInvitationDocumentByProfileSharingInvitationID(View.ProfileSharingInvitationID);
            }


        }
        #endregion

        #region UAT-2706
        public Entity.SharedDataEntity.ClientSystemDocument GetSharedClientSystemDocument()
        {
            return ProfileSharingManager.GetSharedClientSystemDocument(View.ClientSysDocId);
        }
        #endregion

        #region UAT-2774
        public InvitationDocument GetSharedUserInvitationDocumentByDocumentID()
        {
            if (View.InvitationDocumentID > 0)
            {
                return ProfileSharingManager.GetInvitationDocumentByDocumentID(View.InvitationDocumentID);
            }
            else
            {
                return new InvitationDocument();
                //return ProfileSharingManager.GetInvitationDocumentByProfileSharingInvitationID(View.ProfileSharingInvitationID);
            }


        }
        #endregion

        #region Ticket Center
        public TicketDocument GetTicketDocument(Int32 documentId)
        {
            String documentTypeCode = DocumentType.TICKET_ISSUE_DOCUMENT.GetStringValue();
            if (documentId > 0 && View.TenantId > 0)
            {
                return TicketsCentreManager.GetTicketDocument(View.TenantId, documentId, documentTypeCode);
            }
            return null;
        }
        #endregion

        #region UAT-4592
        public bool IsOverrideDisclaimerDocument()
        {
            String disclaimerDocumentOverride = AppConsts.DISCLAIMER_DOCUMENT_OVERRIDE;
            Entity.ClientEntity.AppConfiguration appConfiguration = ComplianceDataManager.GetAppConfiguration(disclaimerDocumentOverride, View.TenantId);

            if (appConfiguration.IsNotNull() && !appConfiguration.AC_Value.IsNullOrEmpty())
            {
                View.DisclaimerDocumentSystemDocumentID = Convert.ToInt32(appConfiguration.AC_Value);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Method to get the Disclosure document
        /// </summary>
        /// <returns></returns>
        public Entity.SystemDocument GetDisclaimerDocumentData()
        {
            if (View.DisclaimerDocumentSystemDocumentID > 0)
            {
                return SecurityManager.GetSystemDocumentByID(View.DisclaimerDocumentSystemDocumentID);                
            }
            return null;
        }
        #endregion

    }
}




