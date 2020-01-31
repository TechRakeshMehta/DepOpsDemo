using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.Utils;
using System.Configuration;

namespace CoreWeb.Shell.Views
{
    public class UserAttestationDisclosurePresenter : Presenter<IUserAttestationDisclosure>
    {
        /// <summary>
        /// Method to check if client admin has any Employment Node Permission
        /// </summary>
        /// <returns></returns>
        public Boolean CheckEmploymentNodePermission()
        {
            return ComplianceDataManager.CheckEmploymentNodePermission(View.TenantID, View.OrganizationUserID);
        }

        /// <summary>
        /// Method to partially Fill User Attestation Document with pre-required data
        /// </summary>
        /// <returns></returns>
        public Entity.UserAttestationDetail FillUserAttestationDocumentWithPrePopulatedData()
        {
            return SecurityManager.FillUserAttestationDocumentWithPrePopulatedData(View.OrganizationUserID, View.DocumentTypeCode);
        }

        /// <summary>
        /// Method to insert E-Sign into partially filled Docuement
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bufferSignature"></param>
        /// <returns></returns>
        public byte[] InsertSignatureToAttestationDocument(byte[] buffer, byte[] bufferSignature)
        {
            return SecurityManager.InsertSignatureToAttestationDocument(buffer, bufferSignature);
        }

        /// <summary>
        /// Method to save final document
        /// </summary>
        /// <param name="mergedSignedDocumentBuffer"></param>
        /// <param name="aWSUseS3"></param>
        /// <returns></returns>
        public bool SaveFullyFilledAttestationDocument(byte[] mergedSignedDocumentBuffer, bool aWSUseS3, String documentStage)
        {
            Entity.UserAttestationDetail userAttestationDetail = SecurityManager.SaveUserAttestationDocument(mergedSignedDocumentBuffer, aWSUseS3, View.DocumentTypeCode, documentStage, View.OrganizationUserID, View.UserAttestationDetails);
            if (!userAttestationDetail.IsNullOrEmpty())
                return true;
            return false;
        }

        public void GetOrganizationUserTypeMapping()
        {
            View.OrganizationUserTypeMapping = SecurityManager.GetOrganizationUserTypeMapping(View.UserId);
        }

        /// <summary>
        /// UAT-1741, 604 notification should only have to be clicked upon login once per 24 hours.
        /// </summary>
        /// <param name="organizationUserID"></param>
        /// <returns></returns>
        public Boolean IsEDFormPreviouslyAccepted(Int32 organizationUserID)
        {
            Double employmentDisclosureIntervalHours = AppConsts.NONE;
            if (!ConfigurationManager.AppSettings["EmploymentDisclosureIntervalHours"].IsNullOrEmpty())
            {
                employmentDisclosureIntervalHours = Convert.ToDouble(ConfigurationManager.AppSettings["EmploymentDisclosureIntervalHours"]);
            }
            return SecurityManager.IsEDFormPreviouslyAccepted(organizationUserID, employmentDisclosureIntervalHours);
        }

        public Int32 GetLineOfBusinessesByUser(String currentUserId, Int32 TenantId)
        {
            var result = SecurityManager.GetLineOfBusinessesByUser(currentUserId).ToList().Count();
            return result;
        }
    }
}
