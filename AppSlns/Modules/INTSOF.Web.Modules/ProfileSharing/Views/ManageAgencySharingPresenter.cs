using Business.RepoManagers;
using Entity;
using Entity.ClientEntity;
using Entity.SharedDataEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using Business.ReportExecutionService;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.UI.Contract.SysXSecurityModel;
using CoreWeb.CommonControls.Views;
using INTSOF.ServiceUtil;
using CoreWeb.IntsofSecurityModel;
using INTSOF.Utils.CommonPocoClasses;

namespace CoreWeb.ProfileSharing.Views
{
    public class ManageAgencySharingPresenter : Presenter<IManageAgencySharing>
    {
        /// <summary>
        /// On View Initialized Event
        /// </summary>
        public override void OnViewInitialized()
        {
            GetTenants();
            GetGranularPermissionForDOBandSSN();
        }

        /// <summary>
        /// On View Loaded Event
        /// </summary>
        public override void OnViewLoaded()
        {

        }

        /// <summary>
        /// Method To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = SecurityManager.GetTenants(SortByName, false, clientCode);
        }

        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// </summary>
        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.ClientTenantID);
        }

        /// <summary>
        /// Checking Granular permission for DOB and SSN
        /// </summary>
        public void GetGranularPermissionForDOBandSSN()
        {
            View.IsDOBDisable = false;
            View.SSNPermissionCode = EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue();
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            if (SecurityManager.GetUserGranularPermission(View.CurrentUserID, out dicPermissions))
            {
                if (dicPermissions.ContainsKey(EnumSystemEntity.DOB.GetStringValue()) && dicPermissions[EnumSystemEntity.DOB.GetStringValue()].ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
                {
                    View.IsDOBDisable = true;
                }
                if (dicPermissions.ContainsKey(EnumSystemEntity.SSN.GetStringValue()))
                {
                    View.SSNPermissionCode = dicPermissions[EnumSystemEntity.SSN.GetStringValue()];
                }
            }
        }

        /// <summary>
        /// Private Method to get ClientID based on logged in user
        /// </summary>
        /// <returns></returns>
        public Int32 GetClientID()
        {
            Int32 clientID = 0;
            if (View.IsAdminLoggedIn)
            {
                clientID = View.SelectedTenantID;
            }
            else
            {
                clientID = View.ClientTenantID;
            }
            return clientID;
        }

        /// <summary>
        /// To get User Groups
        /// </summary>
        public void GetAllUserGroups()
        {
            Int32 clientID = GetClientID();
            if (clientID == 0)
                View.lstUserGroup = new List<Entity.ClientEntity.UserGroup>();
            else
            {
                //UAT-2284: User Group permisson/access and availability by node
                Int32? currentUserId = View.IsAdminLoggedIn ? (Int32?)null : View.CurrentUserID;
                View.lstUserGroup = ComplianceSetupManager.GetAllUserGroupWithPermission(clientID, currentUserId).OrderBy(ex => ex.UG_Name).ToList();
            }
        }

        /// <summary>
        /// Method to Get agencies of an institution
        /// </summary>
        public void GetAllAgency()
        {
            Int32 clientID = GetClientID();
            if (clientID == 0)
                View.lstAgency = new List<Agency>();
            else
            {
                //UAT-1881
                IsAdminLoggedIn();
                if (View.IsAdminLoggedIn)
                {
                    View.lstAgency = ProfileSharingManager.GetAllAgency(clientID).OrderBy(x => x.AG_Name).ToList();//UAT- sort dropdowns by Name
                }
                else
                {
                    View.lstAgency = ProfileSharingManager.GetAllAgencyForOrgUser(clientID, View.CurrentUserID).OrderBy(x => x.AG_Name).ToList();
                }
            }
        }

        /// <summary>
        /// Method to Get List of Applicants for which the ProfileSharingInvitations are to be sent
        /// </summary>
        public void GetProfileSharingInvitationApplicants()
        {
            Int32 clientID = GetClientID();
            if (clientID == 0)
            {
                View.AgencySharingData = new List<AgencySharingDataContract>();
            }
            //else if (View.SelectedUserGroupID == 0)
            //{
            //    View.AgencySharingData = new List<AgencySharingDataContract>();
            //    View.ErrorMessage = "Please select a user group.";
            //}
            else if (View.SelectedAgencyID == 0)
            {
                View.AgencySharingData = new List<AgencySharingDataContract>();
                View.ErrorMessage = "Please select an agency.";
            }
            else
            {
                SearchItemDataContract searchDataContract = new INTSOF.UI.Contract.ComplianceManagement.SearchItemDataContract();

                // Screen is opened through Normal Profile Sharing
                if (View.SrcScreen.IsNullOrEmpty())
                {
                    searchDataContract.ApplicantFirstName = String.IsNullOrEmpty(View.ApplicantFirstName) ? null : View.ApplicantFirstName;
                    searchDataContract.ApplicantLastName = String.IsNullOrEmpty(View.ApplicantLastName) ? null : View.ApplicantLastName;
                    if (View.OrganizationUserID > SysXDBConsts.NONE)
                    {
                        searchDataContract.OrganizationUserId = View.OrganizationUserID;
                    }
                    searchDataContract.EmailAddress = String.IsNullOrEmpty(View.EmailAddress) ? null : View.EmailAddress;
                    //searchDataContract.ApplicantSSN = String.IsNullOrEmpty(View.SSN) ? null : View.SSN;
                    searchDataContract.ApplicantSSN = ApplicationDataManager.GetSSNForFilters(View.SSN);
                    searchDataContract.DateOfBirth = View.DOB;
                    //if (View.SelectedHierarchyID > SysXDBConsts.NONE)
                    //{
                    //    searchDataContract.DPM_Id = View.SelectedHierarchyID;
                    //}
                    if (!View.SelectedHierarchyIDs.IsNullOrEmpty())
                    {
                        searchDataContract.SelectedDPMIds = View.SelectedHierarchyIDs;
                    }
                    searchDataContract.CustomFields = String.IsNullOrEmpty(View.CustomFields) ? null : View.CustomFields;
                    if (View.SelectedUserGroupID > SysXDBConsts.NONE)
                    {
                        searchDataContract.FilterUserGroupID = View.SelectedUserGroupID;
                    }
                    searchDataContract.LoggedInUserId = View.CurrentUserID;
                    searchDataContract.LoggedInUserTenantId = View.ClientTenantID;
                    searchDataContract.AgencyID = View.SelectedAgencyID;


                    if (!View.OrderPaidFromDate.IsNullOrEmpty() && View.OrderPaidFromDate != DateTime.MinValue)
                    {
                        searchDataContract.OrderPaidFrom = View.OrderPaidFromDate;
                    }
                    else
                    {
                        searchDataContract.OrderPaidFrom = null;
                    }
                    if (!View.OrderPaidToDate.IsNullOrEmpty() && View.OrderPaidToDate != DateTime.MinValue)
                    {
                        searchDataContract.OrderPaidTo = View.OrderPaidToDate;
                    }
                    else
                    {
                        searchDataContract.OrderPaidTo = null;
                    }

                    //UAT-1762:- Manage Agency Sharing screen not displaying students for ADB Admins who are not the super admin
                    searchDataContract.IsADBAdmin = View.ClientTenantID == SecurityManager.DefaultTenantID ? true : false;
                }

                try
                {
                    if (View.IsRotationSharing)
                    {
                        View.AgencySharingData = ProfileSharingManager.GetRotationMembers(View.RotationId, View.SelectedAgencyID, View.AgencyDataGridCustomPaging, View.RotationMemberIds, clientID, View.InstructorPreceptorOrgUserIds);

                        var _selectedApplicants = new Dictionary<Int32, Boolean>();

                        foreach (var applicant in View.AgencySharingData)
                        {
                            _selectedApplicants.Add(applicant.OrganizationUserId, true);
                        }
                        View.AssignOrganizationUserIds = _selectedApplicants;

                        //UAT-3977// Records of applicants in grid at top and Instructors at bottom.
                        if (!View.InstructorPreceptorOrgUserIds.IsNullOrEmpty())
                        {
                            List<String> lstInstructorPreceptorOrgUserIds = View.InstructorPreceptorOrgUserIds.Split(',').ToList();
                            foreach (AgencySharingDataContract agencySharingData in View.AgencySharingData)
                            {
                                if (lstInstructorPreceptorOrgUserIds.Contains(agencySharingData.OrganizationUserId.ToString()))
                                    agencySharingData.IsInstructor = AppConsts.ONE ;
                                else
                                    agencySharingData.IsInstructor = AppConsts.NONE;
                            }
                            View.AgencySharingData = View.AgencySharingData.OrderBy(ord => ord.IsInstructor).ToList();
                        }
                    }
                    else
                    {
                        View.AgencySharingData = ProfileSharingManager.GetDataForAgencySharing(clientID, searchDataContract, View.AgencyDataGridCustomPaging);
                    }

                    if (View.AgencySharingData.IsNotNull() && View.AgencySharingData.Count > 0)
                    {
                        if (View.AgencySharingData[0].TotalCount > 0)
                        {
                            View.VirtualRecordCount = View.AgencySharingData[0].TotalCount;
                        }
                        View.CurrentPageIndex = View.AgencyDataGridCustomPaging.CurrentPageIndex;
                    }
                    else
                    {
                        View.VirtualRecordCount = AppConsts.NONE;
                    }
                }
                catch (Exception e)
                {
                    View.AgencySharingData = null;
                    throw e;
                }
            }
        }

        /// <summary>
        /// Getting Formatted SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetFormattedSSN(String unformattedSSN)
        {
            return ApplicationDataManager.GetFormattedSSN(unformattedSSN);
        }

        /// <summary>
        /// Getting Masked SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetMaskedSSN(String unMaskedSSN)
        {
            return ApplicationDataManager.GetMaskedSSN(unMaskedSSN);
        }



        ///// <summary>
        ///// Method to Send invitation to selected Users
        ///// </summary>
        //public void SendInvite(Dictionary<String, Object> conversionData)
        //{
        //    SysXMembershipUser currentUser = null;
        //    int selectedTenantID = 0;
        //    bool isNonScheduledInvitation = false;
        //    int currentUserId = 0;

        //    try
        //    {
        //        DateTime attestationDate;
        //        bool isRotationSharing;
        //        int rotationId;
        //        List<SharingPackageDataContract> lstSharedPkgData;
        //        Dictionary<int, bool> assignOrganizationUserIds;
        //        int selectedAgencyID;
        //        string currentAdminName;
        //        byte[] signature;
        //        string attestationReportText;
        //        bool isAdminLoggedIn;
        //        string selectedAgencyName;
        //        DateTime? invitationSchedlueDate;
        //        String institutionName;
        //        String centralLoginUrl;
        //        String profileSharingURL;
        //        String pxc_ExpireOption;
        //        String pxc_ExpirationTypeCode;
        //        Int32? pxc_MaxViews;
        //        DateTime? pxc_ExpirationDate;


        //        conversionData.TryGetValue("CurrentUser", out currentUser);
        //        conversionData.TryGetValue("AttestationDate", out attestationDate);
        //        conversionData.TryGetValue("IsRotationSharing", out isRotationSharing);
        //        conversionData.TryGetValue("RotationId", out rotationId);
        //        conversionData.TryGetValue("SelectedTenantID", out selectedTenantID);
        //        conversionData.TryGetValue("LstSharedPkgData", out lstSharedPkgData);
        //        conversionData.TryGetValue("IsNonScheduledInvitation", out isNonScheduledInvitation);
        //        conversionData.TryGetValue("AssignOrganizationUserIds", out assignOrganizationUserIds);
        //        conversionData.TryGetValue("SelectedAgencyID", out selectedAgencyID);
        //        conversionData.TryGetValue("CurrentAdminName", out currentAdminName);
        //        conversionData.TryGetValue("Signature", out signature);
        //        conversionData.TryGetValue("AttestationReportText", out attestationReportText);
        //        conversionData.TryGetValue("IsAdminLoggedIn", out isAdminLoggedIn);
        //        conversionData.TryGetValue("SelectedAgencyName", out selectedAgencyName);
        //        conversionData.TryGetValue("InvitationSchedlueDate", out invitationSchedlueDate);
        //        conversionData.TryGetValue("InstitutionName", out institutionName);
        //        conversionData.TryGetValue("InvitationSchedlueDate", out centralLoginUrl);
        //        conversionData.TryGetValue("ProfileSharingURL", out profileSharingURL);
        //        conversionData.TryGetValue("PXC_ExpireOption", out pxc_ExpireOption);
        //        conversionData.TryGetValue("PXC_ExpirationTypeCode", out pxc_ExpirationTypeCode);
        //        conversionData.TryGetValue("PXC_MaxViews", out pxc_MaxViews);
        //        conversionData.TryGetValue("PXC_ExpirationDate", out pxc_ExpirationDate);

        //        currentUserId = currentUser.OrganizationUserId;

        //        var lstClientContacts = new List<ClientContactProfileSharingData>();
        //        Int32 clientID = GetClientID();
        //        var lstApplicant = new List<Int32>();
        //        var rotationDetailsContract = new ClinicalRotationDetailContract();
        //        var lstSharedUserSnapshot = new List<SharedUserSubscriptionSnapshotContract>();
        //        var _lstPSIGroupTypes = LookupManager.GetSharedDBLookUpData<lkpProfileSharingInvitationGroupType>();

        //        if (isRotationSharing)
        //        {
        //            lstClientContacts = ClinicalRotationManager.GetRotationClientContacts(rotationId, clientID);
        //            rotationDetailsContract = ClinicalRotationManager.GetClinicalRotationById(clientID, rotationId);

        //            /* UAT 1531: WB: Instructors assigned to rotations should be included in the attestation*/
        //            if (isNonScheduledInvitation)
        //            {
        //                var _lstSharedUserTypes = LookupManager.GetLookUpData<lkpSharedUserType>(clientID).ToList();
        //                var _instructorCode = OrganizationUserType.Instructor.GetStringValue();
        //                var _preceptorCode = OrganizationUserType.Preceptor.GetStringValue();
        //                var _instructorTypeId = _lstSharedUserTypes.Where(sut => sut.SUT_Code == _instructorCode).First().SUT_ID;
        //                var _preceptorTypeId = _lstSharedUserTypes.Where(sut => sut.SUT_Code == _preceptorCode).First().SUT_ID;

        //                // Generate Snapshot for only those who have registered
        //                foreach (var clientContact in lstClientContacts.Where(cc => cc.OrgUserId != null && cc.ReqSubId != null && cc.ReqSubId != 0).ToList())
        //                {
        //                    var snapshotId = ProfileSharingManager.SaveRequirementSnapshot(currentUserId, Convert.ToInt32(clientContact.ReqSubId), clientID);
        //                    lstSharedUserSnapshot.Add(new SharedUserSubscriptionSnapshotContract
        //                    {
        //                        SnapshotId = snapshotId,
        //                        RequirementSubscriptionId = Convert.ToInt32(clientContact.ReqSubId),
        //                        SharedUserId = clientContact.ClientContactID,
        //                        SharedUserTypeId = clientContact.ClientContactTypeCode == ClientContactType.Instructor.GetStringValue() ? _instructorTypeId : _preceptorTypeId
        //                    });
        //                }
        //            }
        //        }

        //        lstApplicant = assignOrganizationUserIds.Where(cond => cond.Value == true).Select(x => x.Key).ToList();

        //        var _lstMetaData = ProfileSharingManager.GetApplicantMetaData();
        //        List<usp_GetAgencyUserData_Result> lstAgencyUsers = ProfileSharingManager.GetAgencyUserData(clientID, selectedAgencyID);

        //        // Generate new Invitation Group & Invitations
        //        if (lstAgencyUsers.IsNotNull() && lstAgencyUsers.Count > 0)
        //        {
        //            ProfileSharingInvitationGroup invitationGroup = new ProfileSharingInvitationGroup();
        //            invitationGroup.PSIG_AgencyID = selectedAgencyID;
        //            invitationGroup.PSIG_InvitationInitiatedByID = currentUserId;
        //            invitationGroup.PSIG_IsDeleted = false;
        //            invitationGroup.PSIG_CreatedByID = currentUserId;
        //            invitationGroup.PSIG_CreatedOn = DateTime.Now;
        //            invitationGroup.PSIG_TenantID = clientID;
        //            invitationGroup.PSIG_AdminName = currentAdminName;
        //            invitationGroup.PSIG_Signature = signature;
        //            invitationGroup.PSIG_AttestationDate = attestationDate;

        //            //UAT - 1486: Remove fields from Student attestation form and attestation spreadsheet for rotation and non-rotation shares
        //            if (isRotationSharing)
        //            {
        //                invitationGroup.PSIG_AssignedUnits = rotationDetailsContract.UnitFloorLoc;
        //                invitationGroup.PSIG_ClinicalFromDate = rotationDetailsContract.StartDate;
        //                invitationGroup.PSIG_ClinicalToDate = rotationDetailsContract.EndDate;
        //                invitationGroup.PSIG_ProgramName = rotationDetailsContract.Program;
        //            }
        //            //UAT:1219: Display and make editable attestation text on the Manage Agency Sharing screen.
        //            invitationGroup.PSIG_AttestationReportText = attestationReportText;

        //            var _psiGroupTypeCode = String.Empty;
        //            if (isRotationSharing)
        //            {
        //                _psiGroupTypeCode = ProfileSharingInvitationGroupTypes.ROTATION_SHARING_TYPE.GetStringValue();
        //                invitationGroup.PSIG_ClinicalRotationID = rotationId;
        //                invitationGroup.PSIG_ProfileSharingInvitationGroupTypeID = _lstPSIGroupTypes.Where(psigt => psigt.PSIGT_Code == _psiGroupTypeCode).First().PSIGT_ID;
        //            }
        //            else
        //            {
        //                _psiGroupTypeCode = ProfileSharingInvitationGroupTypes.PROFILE_SHARING_TYPE.GetStringValue();
        //                invitationGroup.PSIG_ProfileSharingInvitationGroupTypeID = _lstPSIGroupTypes.Where(psigt => psigt.PSIGT_Code == ProfileSharingInvitationGroupTypes.PROFILE_SHARING_TYPE.GetStringValue()).First().PSIGT_ID;
        //            }

        //            var invitationSourceCode = isAdminLoggedIn
        //                                      ? InvitationSourceTypes.ADMIN.GetStringValue()
        //                                      : InvitationSourceTypes.CLIENTADMIN.GetStringValue();

        //            List<InvitationSharedInfoDetails> lstInvitationSharedInfoDetails = new List<InvitationSharedInfoDetails>();

        //            var _lstInvitationsContract = new List<InvitationDetailsContract>();
        //            Dictionary<Int32, List<Int32>> dicAgencyUserMetaData = new Dictionary<Int32, List<Int32>>();
        //            Dictionary<Int32, List<Int32>> dicClientContactMetaData = new Dictionary<Int32, List<Int32>>();

        //            List<Entity.OrganizationUser> lstApplicantInfo = SecurityManager.GetOrganizationUserByIds(lstApplicant);
        //            foreach (Entity.OrganizationUser applicant in lstApplicantInfo)
        //            {
        //                //Entity.OrganizationUser applicantInfo = SecurityManager.GetOrganizationUser(applicantID);

        //                String applicantName = applicant.FirstName + " " + applicant.LastName;

        //                List<ProfileSharingPackages> applicantSharingPackages = StoredProcedureManagers.GetSharingPackages(applicant.OrganizationUserID, clientID);

        //                List<ProfileSharingPackages> compliancePackages = FilterSelectedComplianceBkgPkgs(SystemPackageTypes.COMPLIANCE_PKG.GetStringValue(), applicantSharingPackages, true, lstSharedPkgData);
        //                List<ProfileSharingPackages> bkgPackages = FilterSelectedComplianceBkgPkgs(SystemPackageTypes.BACKGROUND_PKG.GetStringValue(), applicantSharingPackages, false, lstSharedPkgData);

        //                //List<ProfileSharingPackages> compliancePackages = applicantSharingPackages.Where(cond => cond.IsCompliancePkg).ToList();
        //                //List<ProfileSharingPackages> bkgPackages = applicantSharingPackages.Where(cond => !cond.IsCompliancePkg).ToList();

        //                List<ComplianceInvitationData> compliancePkgDataList = ProfileSharingManager.GetSharingComplianceData(clientID, compliancePackages, isNonScheduledInvitation, currentUserId);
        //                List<BkgInvitationData> bkgPkgDataList = ProfileSharingManager.GetSharingBkgPkgData(bkgPackages);

        //                List<RequirementInvitationData> _lstRequirementDataList = new List<RequirementInvitationData>();

        //                if (isRotationSharing)
        //                {
        //                    List<ProfileSharingRequirementPackage> _lstRequirementPackages = StoredProcedureManagers.GetSharingRequirementPackages(applicant.OrganizationUserID, Convert.ToString(rotationId), clientID);
        //                    //var _filteredList = FilterRequirementPackages(SystemPackageTypes.REQUIREMENT_ROT_PKG.GetStringValue(), _lstRequirementPackages);
        //                    _lstRequirementDataList = ProfileSharingManager.GetSharingRequirementData(clientID, _lstRequirementPackages, isNonScheduledInvitation, currentUserId);
        //                }

        //                // If No package is available for sharing, for a user, then do not create any invitation.
        //                if (!compliancePkgDataList.IsNullOrEmpty() || !bkgPkgDataList.IsNullOrEmpty() || !_lstRequirementDataList.IsNullOrEmpty())
        //                {
        //                    Int32 agencyUserTypeID = ProfileSharingManager.GetUserTypeIdByCode(OrganizationUserType.AgencyUser.GetStringValue());

        //                    lstAgencyUsers.ForEach(agencyUser =>
        //                    {
        //                        #region Generate Invitation Contract

        //                        var _identifier = Guid.NewGuid();
        //                        var currentInvitation = GenerateInvitationDetailsInstance(currentUserId, clientID, applicant.OrganizationUserID, _identifier, agencyUser.AgencyName,
        //                                                                                  agencyUser.AgencyUserID, agencyUser.NAME, agencyUser.Phone,
        //                                                                                  agencyUser.Email, agencyUser.ApplicationInvitationMetaDataID, agencyUserTypeID, OrganizationUserType.AgencyUser.GetStringValue()
        //                                                                                  , selectedAgencyID, pxc_ExpireOption, pxc_ExpirationTypeCode, pxc_MaxViews, pxc_ExpirationDate, isNonScheduledInvitation, invitationSchedlueDate);

        //                        #endregion

        //                        //ADD COMPLIANCE PACKAGE DATA INTO CONTRACT                    
        //                        ProfileSharingManager.AddCompliancePackage(compliancePkgDataList, agencyUser.ComplianceSharedInfoTypeCode, currentInvitation);

        //                        //ADD BACKGROUND PACKAGE DATA INTO CONTRACT
        //                        ProfileSharingManager.AddBackgroundPackage(bkgPkgDataList, agencyUser.BkgSharedInfoTypeCode, currentInvitation);

        //                        var _lstSharedMataDataIds = agencyUser.ApplicationInvitationMetaDataID.Split(',').Select(id => Int32.Parse(id)).ToList(); ;

        //                        if (isRotationSharing)
        //                        {
        //                            currentInvitation.IsRotationType = true;

        //                            //Added this check for UAt-1381.
        //                            if (_lstRequirementDataList.Count > AppConsts.NONE)
        //                            {
        //                                //ADD REQUIREMENT PACKAGE DATA INTO CONTRACT
        //                                ProfileSharingManager.AddRequirementPackage(_lstRequirementDataList, agencyUser.RotationSharedInfoTypeCode, currentInvitation);
        //                            }

        //                            // UAT 1403 : Generate the Distinct list of Agency Users, in order to generate 
        //                            // Single Email Html, for all students, ONLY If it is NON-Scheduling type
        //                            if (!dicAgencyUserMetaData.ContainsKey(agencyUser.AgencyUserID.Value) && isNonScheduledInvitation)
        //                            {
        //                                dicAgencyUserMetaData.Add(Convert.ToInt32(agencyUser.AgencyUserID), _lstSharedMataDataIds);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            // Generate the Email HTML Per/Applicant & Per/Email, in case of normal Prfile Sharing, ONLY If it is NON-Scheduling type
        //                            if (isNonScheduledInvitation)
        //                            {
        //                                var applicantInfo = ProfileSharingManager.ConvertOrgUserIntoApplicantInfoContract(applicant, clientID);
        //                                currentInvitation.TemplateData = GenerateMetaData(clientID, _lstMetaData, applicantInfo, agencyUser.NAME, _lstSharedMataDataIds, institutionName, centralLoginUrl);
        //                                currentInvitation.TemplateData.Add(AppConsts.PSIEMAIL_RECIPIENTID, agencyUser.AgencyUserID.ToString());
        //                            }
        //                        }

        //                        // GENERATE CONTRACT TO STORE THE PERMISION TYPES. TO BE USED IN GENERATION OF ATTESTATION REPORTS, ONLY IF IT IS NON-SCHEDULING TYPE
        //                        if (isNonScheduledInvitation)
        //                        {
        //                            ProfileSharingManager.GenerateAttestationReportData(lstInvitationSharedInfoDetails, agencyUser.ComplianceSharedInfoTypeCode, agencyUser.RotationSharedInfoTypeCode, agencyUser.BkgSharedInfoTypeCode, _identifier, null);
        //                        }

        //                        _lstInvitationsContract.Add(currentInvitation);
        //                    });

        //                    if (isRotationSharing)
        //                    {
        //                        #region ClientContact Sharing

        //                        lstClientContacts.ForEach(clientContact =>
        //                                          {
        //                                              //UAT-1318 Restrict Invitation to be sent to the instructor if he is already an agency user.
        //                                              if (!(lstAgencyUsers.Any(cond => cond.Email.ToLower() == clientContact.Email.ToLower())))
        //                                              {
        //                                                  Int32 userTypeID = AppConsts.NONE;
        //                                                  if (clientContact.ClientContactTypeCode == ClientContactType.Instructor.GetStringValue())
        //                                                  {
        //                                                      userTypeID = ProfileSharingManager.GetUserTypeIdByCode(OrganizationUserType.Instructor.GetStringValue());
        //                                                  }
        //                                                  else
        //                                                  {
        //                                                      userTypeID = ProfileSharingManager.GetUserTypeIdByCode(OrganizationUserType.Preceptor.GetStringValue());
        //                                                  }

        //                                                  var _identifier = Guid.NewGuid();
        //                                                  var _sharedMetaDataIds = String.Empty;

        //                                                  // Client Contact default has full permissions. So use the Master List for the ID's
        //                                                  foreach (var smd in _lstMetaData)
        //                                                  {
        //                                                      _sharedMetaDataIds += smd.AIMD_ID + ",";
        //                                                  }

        //                                                  _sharedMetaDataIds = _sharedMetaDataIds.Substring(0, _sharedMetaDataIds.Length - 1);

        //                                                  // UAT 1403 : Generate the Distinct list of Client Contacts, in order to generate 
        //                                                  // Single Email Html, for all students 
        //                                                  if (!dicClientContactMetaData.ContainsKey(clientContact.ClientContactID) && isNonScheduledInvitation)
        //                                                  {
        //                                                      dicClientContactMetaData.Add(Convert.ToInt32(clientContact.ClientContactID), _lstMetaData.Select(x => x.AIMD_ID).ToList());
        //                                                  }

        //                                                  // Sending the Code of Any ClientContact type is enough as 
        //                                                  // we have the check of AgencyUser while considering change in status of Rotation
        //                                                  var currentInvitation = GenerateInvitationDetailsInstance(currentUserId, clientID, applicant.OrganizationUserID, _identifier, selectedAgencyName,
        //                                                                                                            null, clientContact.Name, clientContact.Phone,
        //                                                                                                            clientContact.Email, _sharedMetaDataIds, userTypeID, OrganizationUserType.Instructor.GetStringValue(), selectedAgencyID, pxc_ExpireOption, pxc_ExpirationTypeCode, pxc_MaxViews, pxc_ExpirationDate,
        //                                                                                                            isNonScheduledInvitation, invitationSchedlueDate, clientContact.ClientContactID);
        //                                                  currentInvitation.IsRotationType = true;

        //                                                  //ADD COMPLIANCE PACKAGE DATA INTO CONTRACT                    
        //                                                  ProfileSharingManager.AddCompliancePackage(compliancePkgDataList, clientContact.ComplianceSharedInfoTypeCode, currentInvitation);

        //                                                  //ADD BACKGROUND PACKAGE DATA INTO CONTRACT
        //                                                  ProfileSharingManager.AddBackgroundPackage(bkgPkgDataList, clientContact.BkgSharedInfoTypeCode, currentInvitation);

        //                                                  //ADD REQUIREMENT PACKAGE DATA INTO CONTRACT
        //                                                  ProfileSharingManager.AddRequirementPackage(_lstRequirementDataList, clientContact.ReqRotSharedInfoTypeCode, currentInvitation);

        //                                                  var _lstSharedMataDataIds = _sharedMetaDataIds.Split(',').Select(id => Int32.Parse(id)).ToList(); ;
        //                                                  ////currentInvitation.TemplateData = GenerateMetaData(clientID, _lstMetaData, applicant, clientContact.Name, _lstSharedMataDataIds);

        //                                                  // GENERATE CONTRACT TO STORE THE PERMISION TYPES. TO BE USED IN GENERATION OF ATTESTATION REPORTS
        //                                                  if (isNonScheduledInvitation)
        //                                                  {
        //                                                      ProfileSharingManager.GenerateAttestationReportData(lstInvitationSharedInfoDetails, clientContact.ComplianceSharedInfoTypeCode,
        //                                                                                    clientContact.ReqRotSharedInfoTypeCode, clientContact.BkgSharedInfoTypeCode, _identifier, null);
        //                                                  }
        //                                                  _lstInvitationsContract.Add(currentInvitation);
        //                                              }
        //                                          });
        //                        #endregion
        //                    }
        //                }
        //            }

        //            // STEP 1 - Save All the Invitations in Security database.
        //            var _lstInvitations = ProfileSharingManager.SaveAdminInvitations(_lstInvitationsContract, invitationGroup, invitationSourceCode, isNonScheduledInvitation);
        //            //UAT-1844:Phase 2 8: Agency User> Rotation tab and Other Tab
        //            List<Int32> distinctOrgUserId = _lstInvitations.DistinctBy(x => x.PSI_InviteeOrgUserID).Select(x => x.PSI_InviteeOrgUserID.Value).ToList();

        //            distinctOrgUserId.ForEach(orgId =>
        //            {
        //                List<Int32> invitIds = new List<Int32>();
        //                invitIds = _lstInvitations.Where(cnd => cnd.PSI_InviteeOrgUserID == orgId).Select(slct => slct.PSI_ID).ToList();
        //                ProfileSharingManager.SaveUpdateSharedUserInvitationReviewStatus(invitIds, orgId, orgId, AppConsts.NONE, String.Empty, SharedUserInvitationReviewStatus.PENDING_REVIEW.GetStringValue());
        //            });

        //            if (isNonScheduledInvitation)
        //            {
        //                // STEP 2 - Save All the Invitations in Tenant database.
        //                ProfileSharingManager.SaveAdminInvitationDetails(_lstInvitationsContract, _lstInvitations, lstSharedUserSnapshot, rotationId, clientID);

        //                // STEP 3 - Update the ProfileSharingInvitationID in the Contract used to generate the Attestation Reports
        //                UpdateInvitationSharedInfoDetailsContract(_lstInvitations, lstInvitationSharedInfoDetails);

        //                // STEP 4 - Generate the Attestation Report
        //                //TO-DO Method to Convert Profile Sharing Invitation into InvitationDetails Contract
        //                //ProfileSharingManager.ConvertProfileSharingInvitationEntityIntoContract(_lstInvitations);
        //                ProfileSharingManager.GenerateAttestationReport(lstInvitationSharedInfoDetails, _lstInvitations.First().ProfileSharingInvitationGroup.PSIG_ID, isRotationSharing, clientID, currentUserId);

        //                if (!isRotationSharing)
        //                {
        //                    // STEP 5 - Update the Link to be sent with Token, in the Email tamplate, based on the ProfileSharingInvitationID generated.
        //                    UpdateInvitationDetailsContract(_lstInvitations, _lstInvitationsContract, profileSharingURL);
        //                }

        //                // STEP 6 - Loop to Send Invitation Mail for each Invitation.
        //                #region SEND INVITATION EMAIL

        //                //var _lstInvitationsContractNew = _lstInvitationsContract;

        //                var _lstInvitationsContractResult = new List<InvitationDetailsContract>();

        //                List<Int32> lstSharedStudentIds = new List<Int32>();
        //                string agencyName = string.Empty;

        //                //Getting list of shared applicants whom profile shared to shared users
        //                lstSharedStudentIds = _lstInvitationsContract.Select(cond => cond.ApplicantId).Distinct().ToList();

        //                if (!_lstInvitationsContract.IsNullOrEmpty())
        //                    agencyName = _lstInvitationsContract.First().Agency;

        //                if (isRotationSharing)
        //                {
        //                    //Generating Email Content for Agency Users 
        //                    GeneratEmailContentForAgencyUsers(clientID, _lstMetaData, lstAgencyUsers, _lstInvitationsContract, dicAgencyUserMetaData, lstApplicantInfo, rotationDetailsContract, centralLoginUrl, institutionName);

        //                    //Generating Email Content for Client Contacts
        //                    GeneratEmailContentForClientContacts(clientID, _lstMetaData, lstClientContacts, _lstInvitationsContract, dicClientContactMetaData, lstApplicantInfo, rotationDetailsContract, centralLoginUrl, institutionName);

        //                    //Updating the Link to be sent with Token, in the Email tamplate, based on the ProfileSharingInvitationID generated. 
        //                    UpdateInvitationDetailsContract(_lstInvitations, _lstInvitationsContract, profileSharingURL);

        //                    //Get the Distinct EmailId's to send Single email to a shared user, for all Applicants
        //                    _lstInvitationsContractResult = _lstInvitationsContract.DistinctBy(x => x.EmailAddress).ToList();

        //                    //Sending Emails to shared users
        //                    foreach (var invitationContract in _lstInvitationsContractResult)
        //                    {
        //                        //invitationContract.IsEmailSentSuccessfully = ProfileSharingManager.SendInvitationEmail(View.ProfileSharingURL, invitationContract.EmailAddress,
        //                        //                                          String.Empty, String.Empty, false,
        //                        //                                          invitationContract.TemplateData, true, clientID, true);
        //                        invitationContract.IsEmailSentSuccessfully = ProfileSharingManager.SendInvitationEmailFromTemplate(invitationContract.TemplateData
        //                                                    , CommunicationSubEvents.REQUIREMENTS_SHARING_INVITATION_ROTATION_SHARING.GetStringValue()
        //                                                    , currentUserId, invitationContract.EmailAddress);

        //                        // Update the "IsEmailSentSuccessfully" flag for All the invitations for the current EmailAddress
        //                        _lstInvitationsContract.Where(s => s.EmailAddress == invitationContract.EmailAddress).ForEach(invCont =>
        //                        {
        //                            invCont.IsEmailSentSuccessfully = invitationContract.IsEmailSentSuccessfully;
        //                        });
        //                    }

        //                    //Send Confirmation Email for Invitation Sent
        //                    ProfileSharingManager.SendConfirmationForInvitationSent(string.Concat(currentUser.FirstName, " ", currentUser.LastName), currentUser.Email, currentUser.OrganizationUserId, selectedTenantID, invitationGroup.PSIG_ID, currentUser.OrganizationUserId, lstSharedStudentIds, agencyName, rotationDetailsContract.RotationName, true);
        //                }
        //                else
        //                {
        //                    //_lstInvitationsContractResult = _lstInvitationsContract.ToList();
        //                    foreach (var invitationContract in _lstInvitationsContract)
        //                    {
        //                        //invitationContract.IsEmailSentSuccessfully = ProfileSharingManager.SendInvitationEmail(View.ProfileSharingURL, invitationContract.EmailAddress,
        //                        //                                          String.Empty, String.Empty, false,
        //                        //                                          invitationContract.TemplateData, true, clientID);
        //                        invitationContract.IsEmailSentSuccessfully = ProfileSharingManager.SendInvitationEmailFromTemplate(invitationContract.TemplateData
        //                                                    , CommunicationSubEvents.REQUIREMENTS_SHARING_INVITATION_NON_ROTATION.GetStringValue()
        //                                                    , currentUserId, invitationContract.EmailAddress);
        //                    }

        //                    //Send Confirmation Email for Invitation Sent
        //                    ProfileSharingManager.SendConfirmationForInvitationSent(string.Concat(currentUser.FirstName, " ", currentUser.LastName), currentUser.Email, currentUser.OrganizationUserId, selectedTenantID, invitationGroup.PSIG_ID, currentUser.OrganizationUserId, lstSharedStudentIds, agencyName, string.Empty, false);
        //                }
        //                #endregion

        //                // STEP 7 - Update Invitation Status to 'NEW' for each invitation, for which Email has been sent successfully
        //                var _lstInvitationIDs = _lstInvitationsContract.Where(cond => cond.IsEmailSentSuccessfully).Select(col => col.PSIId).ToList();
        //                ProfileSharingManager.UpdateBulkInvitationStatus(LkpInviationStatusTypes.NEW.GetStringValue(), _lstInvitationIDs, currentUserId);

        //                if (_lstInvitationsContract.All(cond => !cond.IsEmailSentSuccessfully))
        //                {
        //                    if (!isNonScheduledInvitation)
        //                    {
        //                        View.ErrorMessage = "Some error occurred while sending invitations to shared users. Please try again Or contact System Administrator.";
        //                    }
        //                }
        //                else if (_lstInvitationsContract.Any(cond => !cond.IsEmailSentSuccessfully))
        //                {
        //                    String agencyUserEmails = String.Join(",", _lstInvitationsContract.Where(cond => !cond.IsEmailSentSuccessfully)
        //                                                                     .Select(col => col.EmailAddress).ToList());

        //                    if (!isNonScheduledInvitation)
        //                    {
        //                        View.ErrorMessage = "Some error occurred while sending invitations to below shared users." +
        //                                                 "Shared Users:" + agencyUserEmails +
        //                                                 ". Please try again Or contact System Administrator.";
        //                    }
        //                }
        //                else
        //                {
        //                    View.SuccessMessage = "Invitation(s) have been sent successfully to all Shared User(s) for selected Applicant(s).";
        //                    View.ErrorMessage = String.Empty;
        //                }
        //            }
        //            else
        //            {
        //                var _lstExcluded = lstSharedPkgData.Where(pkg => pkg.IsCompletelyExcluded == true || pkg.IsPartiallyExcluded == true).ToList();

        //                _lstExcluded.ForEach(ep =>
        //                {
        //                    ep.PSIGroupId = invitationGroup.PSIG_ID;
        //                });

        //                ProfileSharingManager.SaveScheduledExcludedPackageData(_lstExcluded, currentUserId, clientID);
        //                View.SuccessMessage = "Invitation(s) have been scheduled successfully for the selected date.";
        //                View.ErrorMessage = String.Empty;
        //            }
        //        }
        //        else
        //        {
        //            View.InfoMessage = "No Agency User exists for the selected agency.";
        //            View.SuccessMessage = String.Empty;
        //            View.ErrorMessage = String.Empty;
        //        }
        //    }
        //    catch (SysXException ex)
        //    {

        //        if (isNonScheduledInvitation)
        //        {
        //            ProfileSharingManager.SaveErrorInformationWhileInvitationSending(currentUserId);
        //        }
        //        else
        //        {
        //            View.ErrorMessage = ex.Message;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        if (isNonScheduledInvitation)
        //        {
        //            ProfileSharingManager.SaveErrorInformationWhileInvitationSending(currentUserId);
        //        }
        //        else
        //        {
        //            View.ErrorMessage = ex.Message;
        //        }
        //    }
        //}


        /// <summary>
        /// Method to Send invitation to selected Users
        /// </summary>
        public void SendInvite(Dictionary<String, Object> conversionData)
        {
            //SysXMembershipUser currentUser = null;
            //int selectedTenantID = 0;
            bool isNonScheduledInvitation = false;
            int currentUserId = 0;
            Dictionary<String, String> statusMessage = new Dictionary<string, string>();
            try
            {
                statusMessage = ProfileSharingManager.SendProfileSharingInvitation(conversionData, GetClientID());

                View.SuccessMessage = statusMessage[StatusMessages.SUCCESS_MESSAGE.GetStringValue()];
                View.InfoMessage = statusMessage[StatusMessages.INFO_MESSAGE.GetStringValue()];
                View.ErrorMessage = statusMessage[StatusMessages.ERROR_MESSAGE.GetStringValue()];
            }
            catch (SysXException ex)
            {

                if (isNonScheduledInvitation)
                {
                    ProfileSharingManager.SaveErrorInformationWhileInvitationSending(currentUserId);
                }
                else
                {
                    View.ErrorMessage = ex.Message;
                }
            }
            catch (System.Exception ex)
            {
                if (isNonScheduledInvitation)
                {
                    ProfileSharingManager.SaveErrorInformationWhileInvitationSending(currentUserId);
                }
                else
                {
                    View.ErrorMessage = ex.Message;
                }
            }
        }

        private void GeneratEmailContentForClientContacts(int clientID, List<ApplicantInvitationMetaData> _lstMetaData, List<ClientContactProfileSharingData> lstClientContacts, List<InvitationDetailsContract> _lstInvitationsContract,
                            Dictionary<int, List<int>> dicClientContactMetaData, List<Entity.OrganizationUser> lstApplicantInfo, ClinicalRotationDetailContract rotationDetailsContract, string centralLoginUrl, string institutionName)
        {
            //UAT-1403 : Add Rotation Details to Rotation Sharing Invitation Email
            var rotationDetailsHTML = ProfileSharingManager.GenerateRotationDetailsHTML(rotationDetailsContract);

            foreach (var clientContact in lstClientContacts)
            {
                if (dicClientContactMetaData.ContainsKey(clientContact.ClientContactID))
                {
                    var _clientContactMetaDataIds = dicClientContactMetaData.Where(k => k.Key == clientContact.ClientContactID).First().Value;

                    var _lstSharedMetaDataCodes = _lstMetaData.Where(amd => _clientContactMetaDataIds.Contains(amd.AIMD_ID))
                                                       .Select(amd => amd.AIMD_Code).ToList();
                    var _dicContent = new Dictionary<String, String>();
                    _dicContent.Add(AppConsts.PSIEMAIL_RECIPIENTNAME, clientContact.Name);
                    _dicContent.Add(AppConsts.PSIEMAIL_RECIPIENTID, clientContact.ClientContactID.ToString());
                    _dicContent.Add(AppConsts.PSIEMAIL_CENTRALLOGINURL, centralLoginUrl);
                    _dicContent.Add(AppConsts.PSIEMAIL_SCHOOLNAME, institutionName);

                    var applicantDataHtml = String.Empty;

                    if (!lstApplicantInfo.IsNullOrEmpty())
                    {
                        var lstApplicantInfoContract = new List<OrganizationUserContract>();
                        lstApplicantInfo.ForEach(applicant =>
                        {
                            var applicantInfo = ProfileSharingManager.ConvertOrgUserIntoApplicantInfoContract(applicant, clientID);
                            lstApplicantInfoContract.Add(applicantInfo);
                        });

                        applicantDataHtml = ProfileSharingManager.GenerateApplicantMetaDataStringRotSharing(lstApplicantInfoContract, _lstSharedMetaDataCodes, clientID);
                    }
                    _dicContent.Add(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA, applicantDataHtml);
                    _dicContent.Add(AppConsts.PSIEMAIL_RotationDetails, rotationDetailsHTML);

                    _lstInvitationsContract.Where(cond => cond.ClientContactID == clientContact.ClientContactID).ForEach(invCon =>
                    {
                        invCon.TemplateData = new Dictionary<string, string>();
                        invCon.TemplateData.AddRange(_dicContent);
                    });
                }
            }
        }

        private void GeneratEmailContentForAgencyUsers(Int32 clientID, List<ApplicantInvitationMetaData> lstMetaData, List<usp_GetAgencyUserData_Result> lstAgencyUsers, List<InvitationDetailsContract> _lstInvitationsContract,
                            Dictionary<Int32, List<Int32>> dicAgencyUserMetaData, List<Entity.OrganizationUser> lstApplicantInfo, ClinicalRotationDetailContract rotationDetailsContract, string centralLoginUrl, string institutionName)
        {
            //UAT-1403 : Add Rotation Details to Rotation Sharing Invitation Email
            var rotationDetailsHTML = ProfileSharingManager.GenerateRotationDetailsHTML(rotationDetailsContract);

            foreach (var agencyUser in lstAgencyUsers)
            {
                if (dicAgencyUserMetaData.ContainsKey(agencyUser.AgencyUserID.Value))
                {
                    var _agencyUserMetaDataIds = dicAgencyUserMetaData.Where(k => k.Key == agencyUser.AgencyUserID).First().Value;

                    var _lstSharedMetaDataCodes = lstMetaData.Where(amd => _agencyUserMetaDataIds.Contains(amd.AIMD_ID))
                                                       .Select(amd => amd.AIMD_Code).ToList();

                    var _dicContent = new Dictionary<String, String>();
                    _dicContent.Add(AppConsts.PSIEMAIL_RECIPIENTNAME, agencyUser.NAME);
                    _dicContent.Add(AppConsts.PSIEMAIL_RECIPIENTID, agencyUser.AgencyUserID.ToString());
                    _dicContent.Add(AppConsts.PSIEMAIL_CENTRALLOGINURL, centralLoginUrl);
                    _dicContent.Add(AppConsts.PSIEMAIL_SCHOOLNAME, institutionName);

                    var applicantDataHtml = String.Empty;

                    if (!lstApplicantInfo.IsNullOrEmpty())
                    {
                        var lstApplicantInfoContract = new List<OrganizationUserContract>();
                        lstApplicantInfo.ForEach(applicant =>
                        {
                            var applicantInfo = ProfileSharingManager.ConvertOrgUserIntoApplicantInfoContract(applicant, clientID);
                            lstApplicantInfoContract.Add(applicantInfo);
                        });
                        applicantDataHtml = ProfileSharingManager.GenerateApplicantMetaDataStringRotSharing(lstApplicantInfoContract, _lstSharedMetaDataCodes, clientID);
                    }
                    _dicContent.Add(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA, applicantDataHtml);
                    _dicContent.Add(AppConsts.PSIEMAIL_RotationDetails, rotationDetailsHTML);

                    _lstInvitationsContract.Where(cond => cond.AgencyUserId == agencyUser.AgencyUserID).ForEach(invCon =>
                    {
                        invCon.TemplateData = new Dictionary<string, string>();
                        invCon.TemplateData.AddRange(_dicContent);
                    });
                }
            }
        }



        private Dictionary<String, String> GenerateMetaData(Int32 clientID, List<ApplicantInvitationMetaData> _lstMetaData, OrganizationUserContract applicant,
                                                     String recepientName, List<int> _lstSharedMataDataIds, string institutionName, string centralLoginUrl)
        {
            var _lstSharedMetaDataCodes = _lstMetaData.Where(amd => _lstSharedMataDataIds.Contains(amd.AIMD_ID))
                                                      .Select(amd => amd.AIMD_Code).ToList();

            var _dicContent = new Dictionary<String, String>();
            _dicContent.Add(AppConsts.PSIEMAIL_STUDENTNAME, applicant.FirstName + " " + applicant.LastName);
            _dicContent.Add(AppConsts.PSIEMAIL_RECIPIENTNAME, recepientName);
            _dicContent.Add(AppConsts.PSIEMAIL_CENTRALLOGINURL, centralLoginUrl);
            _dicContent.Add(AppConsts.PSIEMAIL_SCHOOLNAME, institutionName);

            var applicantDataHtml = String.Empty;

            if (!applicant.IsNullOrEmpty())
            {
                applicantDataHtml = ProfileSharingManager.GenerateApplicantMetaDataString(applicant, _lstSharedMetaDataCodes, clientID,String.Empty,String.Empty);
            }
            //_dicContent.Add(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA, ProfileSharingManager.GenerateApplicantMetaDataString(applicant, _lstSharedMetaDataCodes, clientID));
            _dicContent.Add(AppConsts.PSIEMAIL_SHAREDAPPLICANTDATA, applicantDataHtml);
            return _dicContent;
        }

        private InvitationDetailsContract GenerateInvitationDetailsInstance(Int32 currentUserId, Int32 clientID, Int32 applicantID,
                                                                            Guid _identifier, String agencyName, Int32? agencyUserId,
                                                                            String name, String phone, String email, String applicantInvMetaDataIds
                                                                          , Int32 userTypeID, String userTypeCode, Int32 selectedAgencyID,
                                                                             String pxc_ExpireOption, String pxc_ExpirationTypeCode, Int32? pxc_MaxViews, DateTime? pxc_ExpirationDate
                                                                            , bool isNonScheduledInvitation, DateTime? invitationSchedlueDate,
                                                                            Int32? clientContactID = null)
        {
            var currentInvitation = new InvitationDetailsContract();

            currentInvitation.InvitationIdentifier = _identifier;
            currentInvitation.AgencyId = selectedAgencyID;
            currentInvitation.AgencyUserId = agencyUserId;
            currentInvitation.Name = name;
            currentInvitation.Phone = phone;
            currentInvitation.EmailAddress = email;
            currentInvitation.Agency = agencyName;
            currentInvitation.CurrentDateTime = DateTime.Now;
            currentInvitation.ApplicantId = applicantID;
            currentInvitation.TenantID = clientID;
            //currentInvitation.MaxViews = null;
            //currentInvitation.ExpirationDate = null;
            currentInvitation.CustomMessage = String.Empty;
            currentInvitation.CurrentUserId = currentUserId;

            //UAT 1320: Client admin expire profile shares

            if (pxc_ExpireOption.ToLower() == "yes")
            {
                if (pxc_ExpirationTypeCode == InvitationExpirationTypes.NUMBER_OF_VIEWS.GetStringValue())
                {
                    currentInvitation.MaxViews = pxc_MaxViews;
                    currentInvitation.ExpirationTypeCode = InvitationExpirationTypes.NUMBER_OF_VIEWS.GetStringValue();
                }
                else
                {
                    currentInvitation.ExpirationDate = pxc_ExpirationDate;
                    currentInvitation.ExpirationTypeCode = InvitationExpirationTypes.SPECIFIC_DATE.GetStringValue();
                }
            }
            else
            {
                currentInvitation.ExpirationTypeCode = InvitationExpirationTypes.NO_EXPIRATION_CRITERIA.GetStringValue();
            }

            //Set the ExpirationTypeId based on ExpirationTypeCode.
            currentInvitation.ExpirationTypeId = ProfileSharingManager.GetExpirationTypes().Where(x => x.Code == currentInvitation.ExpirationTypeCode).Select(x => x.ExpirationTypeID).FirstOrDefault();

            if (isNonScheduledInvitation)
            {
                currentInvitation.SharedApplicantMetaDataIds = applicantInvMetaDataIds.ToString().Split(',').Select(Int32.Parse).ToList();
            }

            currentInvitation.InviteeUserTypeID = userTypeID;
            currentInvitation.InviteeUserTypeCode = userTypeCode;
            currentInvitation.InvitationScheduleDate = invitationSchedlueDate;
            if (!clientContactID.IsNullOrEmpty())
            {
                currentInvitation.ClientContactID = clientContactID.Value;
            }
            return currentInvitation;
        }

        /// <summary>
        /// Update the Link Url to be used, with Token, in the Email Template. 
        /// This is added after the Invitation generation in database, 
        /// due to dependency on Token generated for the Invitation.
        /// </summary>
        /// <param name="lstInvitations"></param>
        /// <param name="lstEmailContract"></param>
        private void UpdateInvitationDetailsContract(List<ProfileSharingInvitation> lstInvitations, List<InvitationDetailsContract> lstInvitationContract, String profileSharingURL)
        {
            List<Entity.SharedDataEntity.lkpOrgUserType> lstOrgUserType = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpOrgUserType>();
            foreach (var invitation in lstInvitationContract)
            {
                String code = lstOrgUserType.Where(ot => ot.OrgUserTypeID == invitation.InviteeUserTypeID).FirstOrDefault().OrgUserTypeCode;

                var _invitation = lstInvitations.Where(inv => inv.InvitationIdentifier == invitation.InvitationIdentifier).First();
                var queryString = new Dictionary<String, String>
                                                                 { 
                                                                    {AppConsts.QUERY_STRING_INVITE_TOKEN, Convert.ToString(_invitation.PSI_Token)},
                                                                    {AppConsts.QUERY_STRING_USER_TYPE_CODE , code}
                                                                 };

                var url = String.Format(profileSharingURL + "?args={0}", queryString.ToEncryptedQueryString());
                invitation.TemplateData.Add(AppConsts.PSIEMAIL_PROFILEURL, url);
            }
        }

        /// <summary>
        /// Update the ProfileSharingInvitationID, based on the GUID, used in generating the Attestation report.
        /// </summary>
        /// <param name="lstInvitations"></param>
        /// <param name="lstSharedInfoDetails"></param>
        private void UpdateInvitationSharedInfoDetailsContract(List<ProfileSharingInvitation> lstInvitations, List<InvitationSharedInfoDetails> lstSharedInfoDetails)
        {
            foreach (var sharedInfoDetails in lstSharedInfoDetails)
            {
                var _invitation = lstInvitations.Where(inv => inv.InvitationIdentifier == sharedInfoDetails.InvitationIdentifier).First();
                sharedInfoDetails.SharingInvitationID = _invitation.PSI_ID;
            }
        }

        /// <summary>
        /// UAT-1201 - Method to Bind Attestation Details Grid
        /// </summary>
        public void GetAttestationDetailsData()
        {
            Int32 clientID = GetClientID();
            View.LstInvitationGroup = ProfileSharingManager.GetAttestationDetailsData(clientID, View.CurrentUserID);

            #region UAT-1313 - Setting TenantName to the extended entity column of ProfileSharingInvitationGroup
            if (!View.LstInvitationGroup.IsNullOrEmpty())
            {
                GetTenants();
            }
            foreach (var invitationGroup in View.LstInvitationGroup)
            {
                invitationGroup.TenantName = View.lstTenant.Where(cond => cond.TenantID == invitationGroup.PSIG_TenantID).Select(col => col.TenantName).FirstOrDefault();
            }
            #endregion
        }

        /// <summary>
        /// UAT-1201 - Method to Get Attestation Documents Details By InvitationGroupID 
        /// </summary>
        /// <returns></returns>
        public void GetAttestatationDocumentDetails()
        {
            View.LstInvitationDocument = ProfileSharingManager.GetAttestatationDocumentDetails(View.SelectedInvitationGroupID);
        }

        public void GetClinicalRotationById()
        {
            Int32 clientID = GetClientID();
            if (clientID > AppConsts.NONE && View.RotationId > AppConsts.NONE)
            {
                View.ClinicalRotationDetail = ClinicalRotationManager.GetClinicalRotationById(clientID, View.RotationId, null);
            }
        }


        /// <summary>
        /// Filter the type of Compliance and Background packages to be used, based on the admin selection of the categories
        /// </summary>
        /// <param name="pkgType"></param>
        /// <param name="applicantSharingPackages"></param>
        /// <param name="isCompliancePackage"></param>
        /// <returns></returns>
        private List<ProfileSharingPackages> FilterSelectedComplianceBkgPkgs(String pkgType, List<ProfileSharingPackages> applicantSharingPackages, Boolean isCompliancePackage, List<SharingPackageDataContract> lstSharedPkgData)
        {
            var _lst = new List<ProfileSharingPackages>();
            var _tempList = applicantSharingPackages.Where(pkg => pkg.IsCompliancePkg == isCompliancePackage).ToList();

            var _lstPkgSelected = lstSharedPkgData.Where(pkg => pkg.PackageType == pkgType && pkg.IsCompletelyExcluded == false).ToList();

            foreach (var _crntSelectedPkg in _lstPkgSelected)
            {
                var _lstPkgFromDB = applicantSharingPackages.Where(p => p.PackageId == _crntSelectedPkg.PackageId && p.IsCompliancePkg == isCompliancePackage).ToList();

                if (!_lstPkgFromDB.IsNullOrEmpty())
                {
                    foreach (var _pkgFromDB in _lstPkgFromDB)
                    {
                        var _pkgToAdd = new ProfileSharingPackages();
                        _pkgToAdd.PackageId = _pkgFromDB.PackageId;
                        _pkgToAdd.PackageName = _pkgFromDB.PackageName;
                        _pkgToAdd.IsCompliancePkg = _pkgFromDB.IsCompliancePkg;

                        if (isCompliancePackage)
                        {
                            _pkgToAdd.PackageSubscriptionId = _pkgFromDB.PackageSubscriptionId;
                            _pkgToAdd.CompliancePkgCategories = new List<Entity.ClientEntity.ComplianceCategory>();

                            foreach (var crntPkgCategory in _pkgFromDB.CompliancePkgCategories)
                            {
                                if (_crntSelectedPkg.lstSelectedCategoryGrpIds.Contains(crntPkgCategory.ComplianceCategoryID))
                                {
                                    _pkgToAdd.CompliancePkgCategories.Add(new Entity.ClientEntity.ComplianceCategory
                                    {
                                        ComplianceCategoryID = crntPkgCategory.ComplianceCategoryID,
                                        CategoryName = crntPkgCategory.CategoryName
                                    });
                                }
                            }
                        }
                        else
                        {
                            _pkgToAdd.BkgOrderPkgId = _pkgFromDB.BkgOrderPkgId;
                            _pkgToAdd.BkgSvcGroups = new List<BkgSvcGroup>();
                            foreach (var crntPkgSvcGrp in _pkgFromDB.BkgSvcGroups)
                            {
                                if (_crntSelectedPkg.lstSelectedCategoryGrpIds.Contains(crntPkgSvcGrp.BSG_ID))
                                {
                                    _pkgToAdd.BkgSvcGroups.Add(new BkgSvcGroup
                                    {
                                        BSG_ID = crntPkgSvcGrp.BSG_ID,
                                        BSG_Name = crntPkgSvcGrp.BSG_Name
                                    });
                                }
                            }
                        }
                        _lst.Add(_pkgToAdd);
                    }
                }
            }

            return _lst;

            //foreach (var crntPkg in _tempList)
            //{
            //    var _pkgSelected = View.lstSharedPkgData.Where(pkg => pkg.PackageId == crntPkg.PackageId && pkg.PackageType == pkgType).FirstOrDefault();

            //    if (_pkgSelected.IsNotNull())
            //    {
            //        var _pkgToAdd = new ProfileSharingPackages();
            //        _pkgToAdd.PackageId = crntPkg.PackageId;
            //        _pkgToAdd.PackageName = crntPkg.PackageName;
            //        _pkgToAdd.IsCompliancePkg = crntPkg.IsCompliancePkg; ;

            //        if (isCompliancePackage)
            //        {
            //            _pkgToAdd.PackageSubscriptionId = crntPkg.PackageSubscriptionId;
            //            _pkgToAdd.CompliancePkgCategories = new List<Entity.ClientEntity.ComplianceCategory>();

            //            foreach (var crntPkgCategory in crntPkg.CompliancePkgCategories)
            //            {
            //                if (_pkgSelected.lstCategoryGrpIds.Contains(crntPkgCategory.ComplianceCategoryID))
            //                {
            //                    _pkgToAdd.CompliancePkgCategories.Add(new Entity.ClientEntity.ComplianceCategory
            //                    {
            //                        ComplianceCategoryID = crntPkgCategory.ComplianceCategoryID,
            //                        CategoryName = crntPkgCategory.CategoryName
            //                    });
            //                }
            //            }
            //        }
            //        else
            //        {
            //            _pkgToAdd.BkgOrderPkgId = crntPkg.BkgOrderPkgId;
            //            _pkgToAdd.BkgSvcGroups = new List<BkgSvcGroup>();
            //            foreach (var crntPkgSvcGrp in crntPkg.BkgSvcGroups)
            //            {
            //                if (_pkgSelected.lstCategoryGrpIds.Contains(crntPkgSvcGrp.BSG_ID))
            //                {
            //                    _pkgToAdd.BkgSvcGroups.Add(new BkgSvcGroup
            //                    {
            //                        BSG_Name = crntPkgSvcGrp.BSG_Name,
            //                        BSG_ID = crntPkgSvcGrp.BSG_ID
            //                    });
            //                }
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Filter the type of Requirement packages to be used, based on the admin selection of the categories
        /// </summary>
        /// <param name="pkgType"></param>
        /// <param name="applicantSharingPackages"></param>
        /// <returns></returns>
        private List<ProfileSharingRequirementPackage> FilterRequirementPackages(String pkgType, List<ProfileSharingRequirementPackage> applicantSharingPackages)
        {
            var _lst = new List<ProfileSharingRequirementPackage>();
            var _lstPkgSelected = View.lstSharedPkgData.Where(pkg => pkg.PackageType == pkgType && pkg.IsCompletelyExcluded == false).ToList();

            foreach (var _crntSelectedPkg in _lstPkgSelected)
            {
                var _pkgFromDB = applicantSharingPackages.Where(p => p.RequirementPackageId == _crntSelectedPkg.PackageId).First();

                if (_pkgFromDB.IsNotNull())
                {
                    var _pkgToAdd = new ProfileSharingRequirementPackage();
                    _pkgToAdd.RequirementPackageId = _pkgFromDB.RequirementPackageId;
                    _pkgToAdd.PackageSubscriptionId = _pkgFromDB.PackageSubscriptionId;
                    _pkgToAdd.RequirementPackageName = _pkgFromDB.RequirementPackageName;
                    _pkgToAdd.PackageTypeCode = _pkgFromDB.PackageTypeCode;

                    _pkgToAdd.PackageSubscriptionId = _pkgFromDB.PackageSubscriptionId;
                    _pkgToAdd.RequirementPkgCategories = new List<Entity.ClientEntity.RequirementCategory>();

                    foreach (var crntPkgCategory in _pkgFromDB.RequirementPkgCategories)
                    {
                        if (_crntSelectedPkg.lstSelectedCategoryGrpIds.Contains(crntPkgCategory.RC_ID))
                        {
                            _pkgToAdd.RequirementPkgCategories.Add(new Entity.ClientEntity.RequirementCategory
                            {
                                RC_ID = crntPkgCategory.RC_ID,
                                RC_CategoryName = crntPkgCategory.RC_CategoryName
                            });
                        }
                    }
                    _lst.Add(_pkgToAdd);
                }
            }

            return _lst;
        }

        /// <summary>
        /// UAT 1530: WB: If sharing with an agency that does not have any users, 
        /// client admin should have to fill out a form displaying the information of the person they would like to add. 
        /// </summary>
        public void IsAgencyUserExistInAgency()
        {
            Int32 clientID = GetClientID();
            View.IsAgencyUserExistInAgency = (ProfileSharingManager.GetAgencyUserData(clientID, View.SelectedAgencyID).Count > AppConsts.NONE);
        }

        public void GetAttestationReportTextForAgency()
        {
            View.LstAttestationReportTextForAgency = ProfileSharingManager.GetAttestationReportTextForAgency(View.AgencyIDs);
        }

        public List<usp_GetAgencyUserData_Result> GetAgencyUserData(int clientID, int selectedAgencyID)
        {
            return ProfileSharingManager.GetAgencyUserData(clientID, selectedAgencyID);
        }

        public Boolean IsOnlyRotationPkgShare(List<Int32> lstAgencyIds)
        {
            //return ProfileSharingManager.IsOnlyRotationPkgShare(View.SelectedAgencyID);
            return ProfileSharingManager.IsOnlyRotationPkgShare(lstAgencyIds);
        }


        public bool IsRotationContainsRotationPkg()
        {
            return ProfileSharingManager.IsRotationContainsRotationPkg(View.RotationId, View.SelectedTenantID);
        }

        public Boolean IsPackageAvailable(String applicantIDs)
        {
            String lstApplicantIds = applicantIDs.Substring(0, applicantIDs.Length - 1);
            var _tplPackages = StoredProcedureManagers.GetSharingPackageData(lstApplicantIds, View.SelectedTenantID);
            if (_tplPackages.Item1.IsNullOrEmpty() && _tplPackages.Item2.IsNullOrEmpty())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region UAT-2529- Check Agency ProfileSharing Setting
        public void CheckAgencyPermission()
        {
            View.IsAdminProfileSharingPermission = ProfileSharingManager.CheckAgencyProfileSharingPermission(View.SelectedAgencyID, View.SelectedTenantID);
        }
        #endregion

        public List<Int32> AnyAgencyUserExists(int clientID, string agencyIds)
        {
            return ProfileSharingManager.AnyAgencyUserExists(clientID, agencyIds);
        }

        public List<ClinicalRotationAgencyContract> GetAgenciesMappedWithRotation(Int32 selectedTenantID, Int32 rotationId)
        {
            return ClinicalRotationManager.GetAgenciesMappedWithRotation(selectedTenantID, rotationId);
        }

        #region UAT-2784
        public Boolean CheckExpirationCriteria(Int32 agencyId)
        {
            String ExpirationCriterialSettingCode = AgencyHierarchySettingType.EXPIRATION_CRITERIA.GetStringValue();
            String ExpirationCriteriaSettingValue = ProfileSharingManager.GetAgencySetting(agencyId, ExpirationCriterialSettingCode);
            if (!ExpirationCriteriaSettingValue.IsNullOrEmpty())
                return ExpirationCriteriaSettingValue == "1" ? true : false;
            return true;
        }

        public Boolean CheckExpirationCriteriaForRotation()
        {
            if (!View.AgencyIDs.IsNullOrEmpty())
            {
                List<Int32> lstAgencyIds = View.AgencyIDs.Split(',').Select(Int32.Parse).ToList();
                return ProfileSharingManager.CheckExpirationCriteriaForRotation(lstAgencyIds);
            }
            return true;
        }
        #endregion

        /// <summary>
        /// This method is used to convert the list of documents into pdf document
        /// </summary>
        /// <param name="conversionData">conversionData (Data dictionary that conatins the applicantdocument table object ,tenantId,currentLoggedUserID)</param>
        /// 
        public void ConvertDocumentsIntoPdf()
        {
            if (View.AttestationDocument.IsNotNull() && View.AttestationDocument.Count > 0)
            {
                View.AttestationDocument = Business.RepoManagers.DocumentManager.ConvertAttestationDocumentToPDF(View.AttestationDocument, View.ClientTenantID, View.CurrentUserID);
            }
        }
    }
}
