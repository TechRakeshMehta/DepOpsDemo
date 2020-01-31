using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using System.Data;
using System.Xml.Linq;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using Entity.SharedDataEntity;
using System.Xml;
using System.IO;

namespace Business.RepoManagers
{
    public static class SharedUserClinicalRotationManager
    {
        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static SharedUserClinicalRotationManager()
        {
            BALUtils.ClassModule = "Shared User Clinical Rotation Manager";
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get shared user clinical rotations
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public static List<ClinicalRotationDetailContract> GetSharedUserClinicalRotations(Int32 tenantId, Int32 currentLoggedInUserId, String userID)
        {
            try
            {
                //Guid currentUserID = new Guid(userID);
                //List<ClinicalRotationDetailContract> lstClinicalRotationSearchData = BALUtils.GetSharedUserClinicalRotationRepoInstance()
                //                                                                                 .GetSharedUserClinicalRotations(currentLoggedInUserId, tenantId, currentUserID);

                //String clinicalRotationXML = CreateClinicalRotationXML(lstClinicalRotationSearchData);
                //List<ClinicalRotationDetailContract> finalClinicalRotation = new List<ClinicalRotationDetailContract>();

                //finalClinicalRotation = ClinicalRotationManager.GetClinicalRotationsByIDs(tenantId, clinicalRotationXML);

                //return BALUtils.GetSharedUserClinicalRotationRepoInstance().SetProfileInvitationDetailData(finalClinicalRotation, tenantId, currentLoggedInUserId);

                return null;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get shared user clinical rotations
        /// </summary>
        /// <param name="tenantIDs"></param>
        /// <param name="clinicalRotationDetailContract"></param>
        /// <param name="lstSelectedTenantNames"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<ClinicalRotationDetailContract> GetSharedUserClinicalRotationDetails(List<TenantDetailContract> tenantDetails, ClinicalRotationDetailContract clinicalRotationDetailContract,
                                                                Int32 currentLoggedInUserId, String userID)
        {
            try
            {
                Guid currentUserID = new Guid(userID);
                List<ClinicalRotationDetailContract> finalClinicalRotationDetailList = new List<ClinicalRotationDetailContract>();

                foreach (var tenant in tenantDetails)
                {
                    Int32 tenantId = tenant.TenantID;
                    List<ClinicalRotationDetailContract> lstClinicalRotationSearchData = BALUtils.GetSharedUserClinicalRotationRepoInstance()
                                                            .GetSharedUserClinicalRotations(currentLoggedInUserId, tenantId, currentUserID);
                    String clinicalRotationXML = CreateClinicalRotationXML(lstClinicalRotationSearchData);
                    List<ClinicalRotationDetailContract> finalClinicalRotationList = new List<ClinicalRotationDetailContract>();

                    finalClinicalRotationList = ClinicalRotationManager.GetClinicalRotationsByIDs(tenantId, currentLoggedInUserId, clinicalRotationXML, clinicalRotationDetailContract);

                    var tempRotationList = BALUtils.GetSharedUserClinicalRotationRepoInstance().SetProfileInvitationDetailData(finalClinicalRotationList, tenantId, tenant.TenantName, currentLoggedInUserId);
                    finalClinicalRotationDetailList.AddRange(tempRotationList);
                }
                return finalClinicalRotationDetailList;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<ClinicalRotationDetailContract> GetSharedUserClinicalRotationPackageDetails(ClinicalRotationDetailContract clinicalRotationDetailContract,
                                                                Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetSharedUserClinicalRotationRepoInstance().GetSharedUserClinicalRotationPackageDetails(clinicalRotationDetailContract, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get shared user clinical rotations
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public static List<Int32> GetSharedUserTenantIDs(String userID, List<String> sharedUserTypeCodes)
        {
            try
            {
                Guid currentUserID = new Guid(userID);
                Boolean isAgencyUser = false;
                Boolean isInstructor_Preceptor = false;
                isAgencyUser = sharedUserTypeCodes.Contains(OrganizationUserType.AgencyUser.GetStringValue());
                if (sharedUserTypeCodes.Contains(OrganizationUserType.Instructor.GetStringValue())
                    || sharedUserTypeCodes.Contains(OrganizationUserType.Preceptor.GetStringValue()))
                {
                    isInstructor_Preceptor = true;
                }
                return BALUtils.GetSharedUserClinicalRotationRepoInstance().GetSharedUserTenantIDs(currentUserID, isAgencyUser, isInstructor_Preceptor);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get shared user agencies
        /// </summary>
        /// <param name="tenantIDs"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<AgencyDetailContract> GetSharedUserAgencies(String userID)
        {
            try
            {
                return BALUtils.GetSharedUserClinicalRotationRepoInstance().GetSharedUserAgencies(userID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UpdateInvitationExpirationRequested(List<ApplicantDataListContract> applicantDataList, Int32 currentUserId)
        {
            return BALUtils.GetSharedUserClinicalRotationRepoInstance().UpdateInvitationExpirationRequested(applicantDataList, currentUserId);
        }

        //UAT-3425
        public static Boolean UpdateInvitationExpirationRequirementShares(List<Int32> profileSharingInvIDs, Int32 currentUserId)
        {
            return BALUtils.GetSharedUserClinicalRotationRepoInstance().UpdateInvitationExpirationRequirementShares(profileSharingInvIDs, currentUserId);
        }

        /// <summary>
        /// Get Invitation Expiration search data
        /// </summary>
        /// <param name="invitationSearchContract"></param>
        /// <param name="customPagingArgsContract"></param>
        /// <returns></returns>
        public static List<ProfileSharingInvitationSearchContract> GetInvitationExpirationSearchData(ProfileSharingInvitationSearchContract invitationSearchContract, CustomPagingArgsContract customPagingArgsContract)
        {
            return BALUtils.GetSharedUserClinicalRotationRepoInstance().GetInvitationExpirationSearchData(invitationSearchContract, customPagingArgsContract);
        }

        /// <summary>
        /// Save Update Profile Expiration Criteria
        /// </summary>
        /// <param name="invitationSearchContract"></param>
        /// <param name="lstInvitationIDs"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public static Boolean SaveUpdateProfileExpirationCriteria(ProfileSharingInvitationSearchContract invitationSearchContract, List<Int32> lstInvitationIDs, Int32 currentUserId)
        {
            try
            {
                //Update ExpirationTypeId based on Code.
                invitationSearchContract.ExpirationTypeId = ProfileSharingManager.GetExpirationTypes().Where(x => x.Code == invitationSearchContract.ExpirationTypeCode).Select(x => x.ExpirationTypeID).FirstOrDefault();
                return BALUtils.GetSharedUserClinicalRotationRepoInstance().SaveUpdateProfileExpirationCriteria(invitationSearchContract, lstInvitationIDs, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-1701, New Agency User Search
        /// <summary>
        /// UAT-1701, New Agency User Search
        /// </summary>
        /// <param name="tenantDetails"></param>
        /// <param name="clinicalRotationDetailContract"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<ClinicalRotationDetailContract> GetStudentRotationSearchDetails(List<TenantDetailContract> tenantDetails, ClinicalRotationDetailContract clinicalRotationDetailContract,
                                                                Int32 currentLoggedInUserId, String userID, Dictionary<Int32, string> dicCustomAttributes)
        {
            try
            {
                Guid currentUserID = new Guid(userID);
                List<ClinicalRotationDetailContract> finalClinicalRotationDetailList = new List<ClinicalRotationDetailContract>();

                foreach (var tenant in tenantDetails)
                {
                    Int32 tenantId = tenant.TenantID;
                    List<ClinicalRotationDetailContract> lstClinicalRotationSearchData = BALUtils.GetSharedUserClinicalRotationRepoInstance()
                                                            .GetSharedUserClinicalRotations(currentLoggedInUserId, tenantId, currentUserID);
                    String clinicalRotationXML = CreateClinicalRotationXML(lstClinicalRotationSearchData);
                    String customAttributeXML = getCustomAttributeXml(dicCustomAttributes, tenantId); //UAT-3165
                    List<ClinicalRotationDetailContract> finalClinicalRotationList = ClinicalRotationManager.GetClinicalRotationsWithStudentByIDs(tenantId, currentLoggedInUserId, clinicalRotationXML, clinicalRotationDetailContract, customAttributeXML);
                    var tempRotationList = BALUtils.GetSharedUserClinicalRotationRepoInstance().SetProfileInvitationDetailData(finalClinicalRotationList, tenantId, tenant.TenantName, currentLoggedInUserId);
                    finalClinicalRotationDetailList.AddRange(tempRotationList);
                }
                return finalClinicalRotationDetailList;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //UAT-3165
        private static String getCustomAttributeXml(Dictionary<Int32, string> dicCustomAttributes, Int32 tenantId)
        {
            String customAttributeXmlByTenantId;
            if (dicCustomAttributes.ContainsKey(tenantId))
            {
                customAttributeXmlByTenantId = dicCustomAttributes[tenantId];
            }
            else
            {
                customAttributeXmlByTenantId = "";
            }
            return customAttributeXmlByTenantId;
        }

        #region ADB Admin Applicant Data Audit History

        public static List<ApplicantDataAuditHistoryContract> GetApplicantDataAuditHistory(SearchItemDataContract searchDataContract, CustomPagingArgsContract customPagingArgsContract)
        {
            return BALUtils.GetSharedUserClinicalRotationRepoInstance().GetApplicantDataAuditHistory(searchDataContract, customPagingArgsContract);
        }

        #endregion

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Create clinical rotation XML
        /// </summary>
        /// <param name="lstClinicalRotationDetailContract"></param>
        /// <returns></returns>
        private static String CreateClinicalRotationXML(List<ClinicalRotationDetailContract> lstClinicalRotationDetailContract)
        {
            //XElement xmlElements = new XElement("ClinicalRotations", lstClinicalRotationDetailContract
            //                        .Select(i => new XElement("ClinicalRotation",
            //                             new XAttribute("ClinicalRotationID", i.RotationID),
            //                             new XAttribute("ProfileSharingInvGroupID", i.ProfileSharingInvGroupID)))
            //                             );
            //return xmlElements.ToString();

            XElement xmlElements = new XElement("ClientContacts", lstClinicalRotationDetailContract
                                    .Select(i => new XElement("ClientContact",
                                         new XAttribute("ClientContactID", i.ClientContactID),
                                         new XAttribute("AgencyID", i.AgencyID)))
                                         );
            return xmlElements.ToString();
        }

        #endregion

        #endregion

        #region UAT-1846 Phase 2 10: Agency> Reports

        public static List<ClinicalRotationDetailContract> GetAttestationReportDataWithoutSignature(Int32 LoggedinUserID)
        {
            try
            {
                return BALUtils.GetSharedUserClinicalRotationRepoInstance().GetAttestationReportWithoutSignature(LoggedinUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2313

        public static List<ClinicalRotationDetailContract> GetClinicalRotationDataFromFlatTable(ClinicalRotationDetailContract clinicalRotationDetailContract
                                                                                      , CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetSharedUserClinicalRotationRepoInstance()
                               .GetClinicalRotationDataFromFlatTable(clinicalRotationDetailContract, customPagingArgsContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<WeekDayContract> GetWeekDays()
        {

            try
            {
                return LookupManager.GetSharedDBLookUpData<lkpWeekDay>().Select(col =>
                                           new WeekDayContract
                                           {
                                               WeekDayID = col.WD_ID,
                                               Name = col.WD_Name,
                                               Description = col.WD_Description,
                                               Code = col.WD_Code
                                           }).ToList();
            }


            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<ClientContactContract> GetAllClientContacts()
        {
            try
            {
                return ConvertClientContactEntityToContact(BALUtils.GetSharedUserClinicalRotationRepoInstance().GetAllClientContacts());
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        private static List<ClientContactContract> ConvertClientContactEntityToContact(List<ClientContact> temporaryContactList)
        {

            List<ClientContactContract> clientContactContractList = new List<ClientContactContract>();
            if (!temporaryContactList.IsNullOrEmpty())
            {
                foreach (ClientContact clientContact in temporaryContactList)
                {
                    ClientContactContract clientContactContract = new ClientContactContract();
                    clientContactContract.ClientContactID = clientContact.CC_ID;
                    clientContactContract.Email = clientContact.CC_Email;
                    clientContactContract.Name = clientContact.CC_Name;
                    clientContactContract.Phone = clientContact.CC_Phone;
                    //clientContactContract.TenantID = clientContact.CC_TenantID;
                    //clientContactContract.TenantName = listTenants.Where(x => x.TenantID == clientContact.CC_TenantID && !x.IsDeleted && x.IsActive).Select(x => x.TenantName).FirstOrDefault();
                    clientContactContract.UserID = clientContact.CC_UserID;
                    clientContactContract.TokenID = clientContact.CC_TokenID;
                    clientContactContract.ClientContactTypeID = clientContact.CC_ClientContactTypeID.HasValue ? clientContact.CC_ClientContactTypeID.Value : AppConsts.NONE;

                    clientContactContractList.Add(clientContactContract);
                }
            }
            return clientContactContractList;
        }

        public static List<lkpArchiveState> GetArchiveStates()
        {

            try
            {
                String activeStatusCode = ArchiveState.Active.GetStringValue();
                String archiveStatusCode = ArchiveState.Archived.GetStringValue();
                List<lkpArchiveState> lkpArchiveSatetList = LookupManager.GetSharedDBLookUpData<lkpArchiveState>().Where(x => x.AS_IsDeleted == false
                                                                                                                                     && (x.AS_Code == activeStatusCode || x.AS_Code == archiveStatusCode))
                                                                                                                                     .ToList();
                lkpArchiveState lkpArchiveStateRow = new lkpArchiveState();
                lkpArchiveStateRow.AS_Name = ArchiveState.All.ToString();
                lkpArchiveStateRow.AS_Code = ArchiveState.All.GetStringValue();
                lkpArchiveSatetList.Add(lkpArchiveStateRow);
                return lkpArchiveSatetList;
            }


            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-3316
        public static String GetSharedUserTemplatePermissions(Int32 organizationUserID, Boolean isCompliancePermissions)
        {
            try
            {
                return BALUtils.GetSharedUserClinicalRotationRepoInstance().GetSharedUserTemplatePermissionsCode(organizationUserID, isCompliancePermissions);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion


        /// <summary>
        /// Get shared user agencies
        /// </summary>
        /// <param name="tenantIDs"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<INTSOF.ServiceDataContracts.Modules.AgencyHierarchy.AgencyHierarchyContract> GetSharedUserAgencyHierarchyRootNodes(String userID)
        {
            try
            {
                return BALUtils.GetSharedUserClinicalRotationRepoInstance().GetSharedUserAgencyHierarchyRootNodes(userID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
    }
}
