using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;

namespace WebSiteUtils.SharedObjects
{
    public static class ProfileSharingHelper
    {
        public static String ExportAttestationDocument(List<AttestationDocumentContract> LstInvitationDocuments, Int32 tenantID, Int32 CurrentLoggedInUserId = 0, String userDefinedPath = "")
        {
            Int32 fileCount = AppConsts.NONE;
            String Url = String.Empty;
            if (LstInvitationDocuments.IsNotNull() && LstInvitationDocuments.Count > 0)
            {
                String consolidatedCode = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_CONSOLIDATED.GetStringValue();
                String AttestationWithoutSign = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_CONSOLIDATED_WITHOUGHT_SIGN.GetStringValue();
                String VerticalAttestation = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_VERTICAL.GetStringValue();
                String HorizontalAttestation = LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT.GetStringValue();

                String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                String folderName = String.Empty;

                if (!tempFilePath.EndsWith(@"\"))
                {
                    tempFilePath += @"\";
                }
                if (tenantID.IsNullOrEmpty() || tenantID == 0)
                {
                    folderName = "Documents_Zip_" + CurrentLoggedInUserId + (DateTime.Now.ToString("MMddyyyy")) + @"\";
                }
                else
                {
                    folderName = "Tenant_" + tenantID.ToString() + "_Attestation_Documents_Zip_" + CurrentLoggedInUserId + (DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss")) + @"\";
                }
                tempFilePath += folderName;

                if (!Directory.Exists(tempFilePath))
                    Directory.CreateDirectory(tempFilePath);
                DirectoryInfo dirInfo = new DirectoryInfo(tempFilePath);

                //UAT-2475
                if (!userDefinedPath.IsNullOrEmpty())
                {
                    tempFilePath += userDefinedPath;

                    if (!Directory.Exists(tempFilePath))
                        Directory.CreateDirectory(tempFilePath);
                    DirectoryInfo dirInfo1 = new DirectoryInfo(tempFilePath);
                }

                List<Int32> LstProfileSharingInvitationID = LstInvitationDocuments.DistinctBy(dst => dst.InvitationDocumentID).Select(cond => cond.ProfileSharingInvitationID).Distinct().ToList();

                foreach (Int32 Id in LstProfileSharingInvitationID)
                {
                    if (LstInvitationDocuments.Any(cond => cond.SharedSystemDocumentTypecode == consolidatedCode && cond.ProfileSharingInvitationID == Id))
                    {
                        List<AttestationDocumentContract> lstApplicantDocumentToExport = LstInvitationDocuments.Where(code => code.ProfileSharingInvitationID == Id
                            && code.SharedSystemDocumentTypecode == LKPSharedSystemDocumentTypes.ATTESTATION_DOCUMENT_CONSOLIDATED.GetStringValue()).ToList();
                        foreach (var applicantDocumentToExport in lstApplicantDocumentToExport)
                        {
                            String fileExtension = Path.GetExtension(applicantDocumentToExport.DocumentFilePath);
                            String fileNamePrefix = String.Empty;
                            fileNamePrefix = "Consolidated_Attestation_Report_";
                            String finalFileName = fileNamePrefix + applicantDocumentToExport.ProfileSharingInvitationGroupID.ToString() + fileExtension;
                            String newTempFilePath = Path.Combine(tempFilePath, finalFileName);
                            byte[] fileBytes = null;
                            fileBytes = CommonFileManager.RetrieveDocument(applicantDocumentToExport.DocumentFilePath, FileType.ApplicantFileLocation.GetStringValue());
                            if (fileBytes.IsNotNull())
                            {
                                File.WriteAllBytes(newTempFilePath, fileBytes);
                            }
                        }
                    }
                    else
                    {
                        foreach (AttestationDocumentContract applicantDocumentToExport in LstInvitationDocuments.Where(cond => cond.ProfileSharingInvitationID == Id).ToList())
                        {
                            String fileExtension = Path.GetExtension(applicantDocumentToExport.DocumentFilePath);
                            String fileNamePrefix = String.Empty;
                            if (applicantDocumentToExport.SharedSystemDocumentTypecode == VerticalAttestation)
                            {
                                fileNamePrefix = "Vertical_Attestation_Report_";
                            }
                            else if (applicantDocumentToExport.SharedSystemDocumentTypecode == HorizontalAttestation)
                            {
                                fileNamePrefix = "Horizontal_Attestation_Report_";
                            }
                            else if (applicantDocumentToExport.SharedSystemDocumentTypecode == AttestationWithoutSign)
                            {
                                fileNamePrefix = "Attestation_Report_";
                            }

                            String finalFileName = fileNamePrefix + applicantDocumentToExport.ProfileSharingInvitationGroupID.ToString() + fileExtension;

                            String newTempFilePath = Path.Combine(tempFilePath, finalFileName);
                            byte[] fileBytes = null;
                            fileBytes = CommonFileManager.RetrieveDocument(applicantDocumentToExport.DocumentFilePath, FileType.ApplicantFileLocation.GetStringValue());

                            if (fileBytes.IsNotNull())
                            {
                                File.WriteAllBytes(newTempFilePath, fileBytes);
                            }
                        }
                    }
                }
                fileCount = Directory.GetFiles(tempFilePath).Count();
                if (fileCount > AppConsts.ONE)
                {
                    Url = "~/ComplianceOperations/UserControl/DoccumentDownload.aspx?zipfolderName=" + folderName + "&IsRotationAppZipDoc=" + "True";
                }
                else if (fileCount == AppConsts.ONE)
                {
                    Url = "~/ComplianceOperations/UserControl/DoccumentDownload.aspx?zipfolderName=" + folderName + "&IsDirectFileDownload=" + "True";
                }
            }
            return Url;
        }

        #region UAT-3315
         public static String ExportApplicantBadgeDocument(List<ApplicantDocumentContract> LstAppDocuments, Int32 tenantID, Int32 CurrentLoggedInUserId = 0, String userDefinedPath = "")
        {
            Int32 fileCount = AppConsts.NONE;
            String Url = String.Empty;
            if (LstAppDocuments.IsNotNull() && LstAppDocuments.Count > 0)
            {
                String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                String folderName = String.Empty;

                if (!tempFilePath.EndsWith(@"\"))
                {
                    tempFilePath += @"\";
                }
                if (tenantID.IsNullOrEmpty() || tenantID == 0)
                {
                    folderName = "Documents_Zip_" + CurrentLoggedInUserId + (DateTime.Now.ToString("MMddyyyy")) + @"\";
                }
                else
                {
                    folderName = "Tenant_" + tenantID.ToString() + "_Badge_Documents_Zip_" + CurrentLoggedInUserId + (DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss")) + @"\";
                }
                tempFilePath += folderName;

                if (!Directory.Exists(tempFilePath))
                    Directory.CreateDirectory(tempFilePath);
                DirectoryInfo dirInfo = new DirectoryInfo(tempFilePath);

                if (!userDefinedPath.IsNullOrEmpty())
                {
                   // tempFilePath += userDefinedPath;

                    if (!Directory.Exists(tempFilePath))
                        Directory.CreateDirectory(tempFilePath);
                    DirectoryInfo dirInfo1 = new DirectoryInfo(tempFilePath);
                }
                foreach (var applicantDocumentToExport in LstAppDocuments)
                {
                    String fileExtension = Path.GetExtension(applicantDocumentToExport.DocumentPath);
                    String fileNamePrefix = String.Empty;
                    String [] str = applicantDocumentToExport.FileName.Split('.');
                    if (str.Count() > 1)
                        fileNamePrefix = str[0].ToString();
                                      
                    String finalFileName = fileNamePrefix + "_" + applicantDocumentToExport.ApplicantName.ToString() + "_" + applicantDocumentToExport.ApplicantDocumentId + fileExtension;
                    String newTempFilePath = Path.Combine(tempFilePath, finalFileName);
                    byte[] fileBytes = null;
                    fileBytes = CommonFileManager.RetrieveDocument(applicantDocumentToExport.DocumentPath, FileType.ApplicantFileLocation.GetStringValue());
                    if (fileBytes.IsNotNull())
                    {
                        File.WriteAllBytes(newTempFilePath, fileBytes);
                    }
                }
                fileCount = Directory.GetFiles(tempFilePath).Count();
                if (fileCount > AppConsts.ONE)
                {
                    Url = "~/ComplianceOperations/UserControl/DoccumentDownload.aspx?zipFilePath=" + tempFilePath + "&IsMultipleFileDownloadInZip=True";
                }
                else if (fileCount == AppConsts.ONE)
                {
                    Url = "~/ComplianceOperations/UserControl/DoccumentDownload.aspx?zipfolderName=" + folderName + "&IsDirectFileDownload=" + "True";
                }
            }
            return Url;
        }
        #endregion
    }
}
