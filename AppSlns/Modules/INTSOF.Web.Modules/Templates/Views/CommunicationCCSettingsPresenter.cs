using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.Templates;
using INTSOF.Utils;

namespace CoreWeb.Templates.Views
{
    public partial class CommunicationCCSettingsPresenter : Presenter<ICommunicationCCSettingsView>
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// This method is invoked by the view every time it loads.
        /// </summary>
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed the first time the view loads.
        }

        /// <summary>
        /// This method is invoked by the view the first time it loads.
        /// </summary>
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads.
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;

            }
        }
        public int GetDefaultTenant()
        {
            return SecurityManager.DefaultTenantID;
        }
        public void BindCommunicationTypes()
        {
            View.CommunicationTypes = CommunicationManager.GetCommunicationTypes().OrderBy(x => x.Name).ToList(); //UAT- sort dropdowns by Name;
        }

        public void BindEvents()
        {
            View.CommunicationEvents = CommunicationManager.GetCommunicationEvents(Convert.ToInt32(View.CommunicationTypeId)).OrderBy(x => x.Name).ToList(); //UAT- sort dropdowns by Name
        }

        public void BindUniqueSubEvents()
        {
            List<CommunicationCCMaster> communicationCCMaster = new List<CommunicationCCMaster>();
            if (View.SelectedTenantId.IsNotNull() && View.SelectedTenantId > AppConsts.NONE)
            {
                //View.CommunicationCcExistingSubEvent = CommunicationManager.GetTenantSpecificCommunicationCCMaster(Convert.ToInt32(View.SelectedTenantId));
                //OLD CODE COMMENTED- UAT 1043
                //communicationCCMaster = CommunicationManager.GetTenantSpecificCommunicationCCMaster(Convert.ToInt32(View.SelectedTenantId), SecurityManager.DefaultTenantID, View.CurrentUserId);

                //UAT 1043 - WB: Admin Comm Copy Settings should have super admin behavior for all ADB admins. 
                if (!IsDefaultTenant) //Client Admin
                {
                    communicationCCMaster = CommunicationManager.GetTenantSpecificCommunicationCCMaster(Convert.ToInt32(View.SelectedTenantId), SecurityManager.DefaultTenantID, View.CurrentUserId, AppConsts.USERTYPE_CLIENTADMIN);
                }
                else if (IsDefaultTenant && View.LoggedInUser.IsNotNull() && View.LoggedInUser.IsSystem == false) //ADB user
                {
                    communicationCCMaster = CommunicationManager.GetTenantSpecificCommunicationCCMaster(Convert.ToInt32(View.SelectedTenantId), SecurityManager.DefaultTenantID, View.CurrentUserId, AppConsts.USERTYPE_ADBUSER);
                }
                else if (IsDefaultTenant && View.LoggedInUser.IsNotNull() && View.LoggedInUser.IsSystem == true && View.SelectedUserType.Equals("Institute") || View.SelectedUserType.Equals("Master")) //ADB Super Admin
                {
                    communicationCCMaster = CommunicationManager.GetTenantSpecificCommunicationCCMaster(Convert.ToInt32(View.SelectedTenantId), SecurityManager.DefaultTenantID, View.CurrentUserId, AppConsts.USERTYPE_SUPERADMIN);
                }
            }

            List<lkpCommunicationSubEvent> communicationSubEvent = CommunicationManager.GetCommunicationTypeSubEvents(Convert.ToInt32(View.CommunicationTypeId),
                                                                   Convert.ToInt32(View.EventId)).Where(cond =>
                                                                   cond.Code != CommunicationSubEvents.EXTERNAL_EMAIL_NOTIFICATION.GetStringValue()
                                                                   && cond.Code != CommunicationSubEvents.NOTIFICATION_FOR_FAILED_APPLICANT_DOCUMENT_MERGING.GetStringValue()//UAT-2628
                                                                   ).ToList();
            var uniqueSubEventList = communicationSubEvent.Where(x => !communicationCCMaster.Any(p => p.CommunicationSubEventID == x.CommunicationSubEventID));
            View.CommunicationSubEvents = uniqueSubEventList.OrderBy(x => x.Name).ToList(); //UAT- sort dropdowns by Name
        }

        public void BindSubEvents()
        {
            View.CommunicationSubEvents = CommunicationManager.GetCommunicationTypeSubEvents(Convert.ToInt32(View.CommunicationTypeId), Convert.ToInt32(View.EventId)).Where(cond =>
                                                                                             cond.Code != CommunicationSubEvents.EXTERNAL_EMAIL_NOTIFICATION.GetStringValue()
                                                                                             && cond.Code != CommunicationSubEvents.NOTIFICATION_FOR_FAILED_APPLICANT_DOCUMENT_MERGING.GetStringValue()//UAT-2628     
                                                                                             ).OrderBy(x => x.Name).ToList(); //UAT- sort dropdowns by Name
        }

        public void GetTenantSpecificCommunicationCCMaster()
        {
            //UAT 1043 - WB: Admin Comm Copy Settings should have super admin behavior for all ADB admins. 
            if (View.SelectedTenantId.IsNotNull() && View.SelectedTenantId > AppConsts.NONE)
            {
                if (!IsDefaultTenant) //Client Admin
                {
                    View.CommunicationCCMasterList = CommunicationManager.GetTenantSpecificCommunicationCCMaster(View.SelectedTenantId, SecurityManager.DefaultTenantID, View.CurrentUserId, AppConsts.USERTYPE_CLIENTADMIN);
                }
                else if (IsDefaultTenant && View.LoggedInUser.IsNotNull() && View.LoggedInUser.IsSystem == false) //ADB user
                {
                    View.CommunicationCCMasterList = CommunicationManager.GetTenantSpecificCommunicationCCMaster(View.SelectedTenantId, SecurityManager.DefaultTenantID, View.CurrentUserId, AppConsts.USERTYPE_ADBUSER);
                }
                else if (IsDefaultTenant && View.LoggedInUser.IsNotNull() && View.LoggedInUser.IsSystem == true && View.SelectedUserType.Equals("Institute") || View.SelectedUserType.Equals("Master")) //ADB Super Admin
                {
                    View.CommunicationCCMasterList = CommunicationManager.GetTenantSpecificCommunicationCCMaster(View.SelectedTenantId, SecurityManager.DefaultTenantID, View.CurrentUserId, AppConsts.USERTYPE_SUPERADMIN);
                }
            }
        }

        public void GetCommunicationCCUser()
        {
            if (View.CommunicationCCMasterID.IsNotNull() && View.CommunicationCCMasterID > AppConsts.NONE)
            {
                if (View.IsNeedToGetCCUsersSettings)
                {
                    //UAT-1072
                    //UAT 1043 - WB: Admin Comm Copy Settings should have super admin behavior for all ADB admins. 
                    if (!IsDefaultTenant) //Client Admin
                    {
                        View.CommunicationCCUserContract = CommunicationManager.GetCommunicationCCUserAndSettings(View.CommunicationCCMasterID, View.CurrentUserId, AppConsts.USERTYPE_CLIENTADMIN).ToList();
                    }
                    else if (IsDefaultTenant && View.LoggedInUser.IsNotNull() && View.LoggedInUser.IsSystem == false) //ADB user
                    {
                        View.CommunicationCCUserContract = CommunicationManager.GetCommunicationCCUserAndSettings(View.CommunicationCCMasterID, View.CurrentUserId, AppConsts.USERTYPE_ADBUSER).ToList();
                    }
                    else if (IsDefaultTenant && View.LoggedInUser.IsNotNull() && View.LoggedInUser.IsSystem == true && View.SelectedUserType.Equals("Institute") || View.SelectedUserType.Equals("Master")) //ADB Super Admin
                    {
                        View.CommunicationCCUserContract = CommunicationManager.GetCommunicationCCUserAndSettings(View.CommunicationCCMasterID, View.CurrentUserId, AppConsts.USERTYPE_SUPERADMIN).ToList();
                    }

                    if (!View.CommunicationCCUserContract.IsNullOrEmpty())
                    {
                        foreach (CommunicationCCUserContract item in View.CommunicationCCUserContract)
                        {
                            if (!string.IsNullOrEmpty(item.SelectedRecordIdsStr))
                            {
                                if (String.Compare(item.SelectedRecordTypeCode, LCObjectType.ComplianceCategory.GetStringValue()) == 0)
                                {
                                    item.SelectedRecordNames = ComplianceSetupManager.GetCategoryNamesByCategoryIds(item.SelectedRecordIdsStr, View.SelectedTenantId);
                                }
                                else if (String.Compare(item.SelectedRecordTypeCode, LCObjectType.ComplianceItem.GetStringValue()) == 0)
                                {
                                    item.SelectedRecordNames = ComplianceSetupManager.GetItemNamesByItemIds(item.SelectedRecordIdsStr, View.SelectedTenantId);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //OLD CODE COMMENTED UAT 1043
                    //List<CommunicationCCUser> communicationCCUser = CommunicationManager.GetCommunicationCCUser(View.CommunicationCCMasterID, SecurityManager.DefaultTenantID, View.CurrentUserId).ToList();
                    List<CommunicationCCUser> communicationCCUser = new List<CommunicationCCUser>();


                    //UAT 1043 - WB: Admin Comm Copy Settings should have super admin behavior for all ADB admins. 
                    if (!IsDefaultTenant) //Client Admin
                    {
                        communicationCCUser = CommunicationManager.GetCommunicationCCUser(View.CommunicationCCMasterID, SecurityManager.DefaultTenantID, View.CurrentUserId, AppConsts.USERTYPE_CLIENTADMIN).ToList();
                    }
                    else if (IsDefaultTenant && View.LoggedInUser.IsNotNull() && View.LoggedInUser.IsSystem == false) //ADB user
                    {
                        communicationCCUser = CommunicationManager.GetCommunicationCCUser(View.CommunicationCCMasterID, SecurityManager.DefaultTenantID, View.CurrentUserId, AppConsts.USERTYPE_ADBUSER).ToList();
                    }
                    else if (IsDefaultTenant && View.LoggedInUser.IsNotNull() && View.LoggedInUser.IsSystem == true && View.SelectedUserType.Equals("Institute") || View.SelectedUserType.Equals("Master")) //ADB Super Admin
                    {
                        communicationCCUser = CommunicationManager.GetCommunicationCCUser(View.CommunicationCCMasterID, SecurityManager.DefaultTenantID, View.CurrentUserId, AppConsts.USERTYPE_SUPERADMIN).ToList();
                    }

                    List<CommunicationCCUserContract> communicationCCUserContractList = new List<CommunicationCCUserContract>();

                    List<Int32?> organizationUserIDs = communicationCCUser.Select(x => x.OrganizationUserID).ToList();
                    if (organizationUserIDs.IsNotNull())
                    {
                        List<OrganizationUser> organizationUser = SecurityManager.getOrganizationUserByIdList(organizationUserIDs);
                        foreach (var item in organizationUser)
                        {

                            CommunicationCCUserContract communicationCCUserContract = new CommunicationCCUserContract();
                            communicationCCUserContract.IsForEmail = communicationCCUser.Where(x => x.OrganizationUserID == item.OrganizationUserID).Select(x => x.IsEmail ?? false).FirstOrDefault();
                            communicationCCUserContract.IsOnlyRotationCreatedNotification = communicationCCUser.Where(x => x.OrganizationUserID == item.OrganizationUserID).Select(x => x.IsOnlyRotationCreatedNotification).FirstOrDefault();
                            communicationCCUserContract.IsForCommunicationCentre = communicationCCUser.Where(x => x.OrganizationUserID == item.OrganizationUserID).Select(x => x.IsCommunicationCentre ?? false).FirstOrDefault();
                            communicationCCUserContract.CopyTypeName = communicationCCUser.FirstOrDefault(x => x.OrganizationUserID == item.OrganizationUserID).lkpCopyType.IsNull() ? String.Empty : communicationCCUser.FirstOrDefault(x => x.OrganizationUserID == item.OrganizationUserID).lkpCopyType.CT_Name;
                            communicationCCUserContract.CopyTypeCode = communicationCCUser.FirstOrDefault(x => x.OrganizationUserID == item.OrganizationUserID).lkpCopyType.IsNull() ? String.Empty : communicationCCUser.FirstOrDefault(x => x.OrganizationUserID == item.OrganizationUserID).lkpCopyType.CT_Code;
                            //View.SelectedCopyTypeCode = communicationCCUser.FirstOrDefault(x => x.OrganizationUserID == item.OrganizationUserID).lkpCopyType.IsNull() ? String.Empty : communicationCCUser.FirstOrDefault(x => x.OrganizationUserID == item.OrganizationUserID).lkpCopyType.CT_Code;
                            communicationCCUserContract.UserFirstName = item.FirstName;
                            communicationCCUserContract.CommunicationCCMasterID = View.CommunicationCCMasterID;
                            communicationCCUserContract.UserLastName = item.LastName;
                            communicationCCUserContract.UserMiddleName = item.FirstName + " " + item.MiddleName + " " + item.LastName + " ("
                                                                         + item.aspnet_Users.aspnet_Membership.Email + ") ";
                            communicationCCUserContract.UserEmailAddress = item.aspnet_Users.aspnet_Membership.Email;
                            communicationCCUserContract.OrganizationUserID = item.OrganizationUserID;

                            var communicationCCUsers = communicationCCUser.Where(x => x.OrganizationUserID == item.OrganizationUserID
                                                       && x.IsDeleted == false).FirstOrDefault().CommunicationCCUsersID;

                            if (communicationCCUsers.IsNotNull())
                            {
                                communicationCCUserContract.CommunicationCCUsersID = communicationCCUsers;
                            }
                            communicationCCUserContractList.Add(communicationCCUserContract);
                        }
                    }

                    View.CommunicationCCUserContract = communicationCCUserContractList;
                }
            }
            else
            {
                View.CommunicationCCUserContract = new List<CommunicationCCUserContract>();
            }
        }

        public void UpdateCommunicationCcUsers()
        {
            List<CommunicationCCUser> communicationCcUserList = new List<CommunicationCCUser>();
            if (View.CommunicationCCUserContract.IsNotNull())
            {
                List<CommunicationCCUserContract> communicationCcUserToSaveUpd = View.CommunicationCCUserContract.Where(x => x.IsSaveUpdateRequired == true).ToList();
                foreach (var item in communicationCcUserToSaveUpd)
                {
                    CommunicationCCUser communicationCcUser = new CommunicationCCUser();
                    communicationCcUser.CommunicationCCUsersID = item.CommunicationCCUsersID;
                    communicationCcUser.CommunicationCCMasterID = View.CommunicationCCMasterID;
                    communicationCcUser.OrganizationUserID = item.OrganizationUserID;
                    communicationCcUser.CopyTypeID = item.SelectedCopyTypeID;
                    communicationCcUser.IsCommunicationCentre = item.IsForCommunicationCentre;
                    communicationCcUser.IsEmail = item.IsForEmail;
                    communicationCcUser.IsOnlyRotationCreatedNotification = item.IsOnlyRotationCreatedNotification.HasValue ? item.IsOnlyRotationCreatedNotification.Value : false;
                    communicationCcUser.IsDeleted = item.IsDeleted;
                    communicationCcUser.CreatedOn = DateTime.Now;
                    communicationCcUser.CreatedBy = View.CurrentUserId;

                    if (item.SelectedRecordTypeId > AppConsts.NONE && !item.IsDeleted && item.SelectedRecordIds != null)
                    {
                        foreach (var recordId in item.SelectedRecordIds)
                        {
                            CommunicationCCUsersSetting CommunicationCCUsersSetting = new CommunicationCCUsersSetting();
                            CommunicationCCUsersSetting.CCUS_ObjectTypeId = item.SelectedRecordTypeId;
                            CommunicationCCUsersSetting.CCUS_RecordId = recordId;
                            CommunicationCCUsersSetting.CCUS_CreatedBy = View.CurrentUserId;
                            CommunicationCCUsersSetting.CCUS_CreatedOn = DateTime.Now;
                            communicationCcUser.CommunicationCCUsersSettings.Add(CommunicationCCUsersSetting);
                        }
                    }

                    communicationCcUserList.Add(communicationCcUser);
                }

                if (CommunicationManager.SaveCommunicationCcUsers(communicationCcUserList, View.CurrentUserId, View.CommunicationCCMasterID))
                {
                    View.SuccessMessage = "Communication Copy Settings for Event updated successfully.";
                }
                else
                {
                    View.ErrorMessage = "An error occurred while updating Communication Copy Settings for Event.";
                }

            }
        }

        public void SaveCommunicationCCMaster()
        {
            if (View.SelectedTenantId > AppConsts.NONE)
            {
                CommunicationCCMaster communicationCCMasterObj = new CommunicationCCMaster();
                communicationCCMasterObj.CommunicationSubEventID = View.SubEventId;
                communicationCCMasterObj.CreatedBy = View.CurrentUserId;
                communicationCCMasterObj.CreatedOn = DateTime.Now;
                communicationCCMasterObj.TenantID = View.SelectedTenantId;
                communicationCCMasterObj.IsDeleted = false;

                if (View.CommunicationCCUserContract.IsNotNull())
                {
                    foreach (var item in View.CommunicationCCUserContract)
                    {
                        if (item.CommunicationCCUsersID < AppConsts.NONE)     //by sahil   < instead of =
                        {
                            CommunicationCCUser communicationCCUser = new CommunicationCCUser();
                            communicationCCUser.OrganizationUserID = item.OrganizationUserID;
                            communicationCCUser.CopyTypeID = item.SelectedCopyTypeID;
                            communicationCCUser.IsCommunicationCentre = item.IsForCommunicationCentre;
                            communicationCCUser.IsEmail = item.IsForEmail;
                            communicationCCUser.IsOnlyRotationCreatedNotification = item.IsOnlyRotationCreatedNotification.HasValue ? item.IsOnlyRotationCreatedNotification.Value : false;
                            communicationCCUser.IsDeleted = false;
                            communicationCCUser.CreatedOn = DateTime.Now;
                            communicationCCUser.CreatedBy = View.CurrentUserId;

                            if (item.SelectedRecordTypeId > AppConsts.NONE)
                            {
                                foreach (var recordId in item.SelectedRecordIds)
                                {
                                    CommunicationCCUsersSetting CommunicationCCUsersSetting = new CommunicationCCUsersSetting();
                                    CommunicationCCUsersSetting.CCUS_ObjectTypeId = item.SelectedRecordTypeId;
                                    CommunicationCCUsersSetting.CCUS_RecordId = recordId;
                                    CommunicationCCUsersSetting.CCUS_CreatedBy = View.CurrentUserId;
                                    CommunicationCCUsersSetting.CCUS_CreatedOn = DateTime.Now;
                                    communicationCCUser.CommunicationCCUsersSettings.Add(CommunicationCCUsersSetting);
                                }
                            }

                            communicationCCMasterObj.CommunicationCCUsers.Add(communicationCCUser);
                        }
                    }
                }
                if (View.CommunicationCCUserContract.IsNotNull())
                {
                    if (CommunicationManager.SaveCommunicationCCMaster(communicationCCMasterObj))
                    {
                        View.SuccessMessage = "Communication Copy Settings for Event added successfully.";
                    }
                    else
                    {
                        View.ErrorMessage = "An error occurred while adding Communication Copy Settings for Event.";
                    }
                }
                else
                {
                    //UAT 1043 - WB: Admin Comm Copy Settings should have super admin behavior for all ADB admins. 
                    if (IsDefaultTenant)   //For ADB USERS and SUPERADMINS --if (View.CurrentUserId == AppConsts.ONE)
                    {
                        View.ErrorMessage = "Please Add User Preference.";
                    }
                    else
                    {
                        View.ErrorMessage = "Please Add My Preference.";
                    }
                }

            }
        }

        public void DeleteCommunicationCcMaster()
        {
            if (View.SelectedTenantId > AppConsts.NONE && View.CommunicationCCMasterID > AppConsts.NONE)
            {
                if (CommunicationManager.DeleteCommunicationCcMaster(View.SelectedTenantId, View.CommunicationCCMasterID, View.CurrentUserId))
                {
                    View.SuccessMessage = "Communication Copy Settings for the Event deleted successfully.";
                }
                else
                {
                    View.ErrorMessage = "An error occurred while deleting Communication Copy Settings for the Event.";
                }

            }
        }

        public void GetOrganizationUserList(Int32 tenantIDForUsersType)
        {
            var CommunicationCCUserContractList = View.CommunicationCCUserContract;

            var OrganizationUserList = SecurityManager.GetOganisationUsersByTanentId(tenantIDForUsersType).Where(cond => cond.IsActive == true
                                       && cond.IsDeleted == false);

            if (CommunicationCCUserContractList.IsNotNull())
            {
                var uniqueOrganizationUserList = OrganizationUserList.Where(p => !CommunicationCCUserContractList.Any(p2 => p2.OrganizationUserID == p.OrganizationUserID));
                View.OrganizationUserList = uniqueOrganizationUserList.Select(x => new Entity.OrganizationUser
                {
                    FirstName = x.FirstName,
                    OrganizationUserID = x.OrganizationUserID,
                    LastName = x.LastName,
                    PrimaryEmailAddress = x.aspnet_Users.aspnet_Membership.Email,
                    MiddleName = x.FirstName + " " + x.MiddleName + " " + x.LastName + " (" + x.aspnet_Users.aspnet_Membership.Email + ") "
                }).OrderBy(x => x.MiddleName).ToList(); //UAT- sort dropdowns by Name
            }
            else
            {
                View.OrganizationUserList = OrganizationUserList.Select(x => new Entity.OrganizationUser
                {
                    FirstName = x.FirstName,
                    OrganizationUserID = x.OrganizationUserID,
                    LastName = x.LastName,
                    PrimaryEmailAddress = x.aspnet_Users.aspnet_Membership.Email,
                    MiddleName = x.FirstName + " " + x.MiddleName + " " + x.LastName + " (" + x.aspnet_Users.aspnet_Membership.Email + ") "
                }).OrderBy(x => x.MiddleName).ToList(); //UAT- sort dropdowns by Name
            }
        }

        public void BindCopyTypes()
        {
            View.lstCopyType = CommunicationManager.GetCopyTypes();
        }

        public void GetOganisationUsersByTanentIdOfLoggedInUser()
        {
            var OrganizationUserList = SecurityManager.GetOganisationUsersByTanentIdOfLoggedInUser(View.TenantId, View.CurrentUserId);
            View.OrganizationUserList = OrganizationUserList.Select(x => new Entity.OrganizationUser
            {
                FirstName = x.FirstName,
                OrganizationUserID = x.OrganizationUserID,
                LastName = x.LastName,
                PrimaryEmailAddress = x.aspnet_Users.aspnet_Membership.Email,
                MiddleName = x.FirstName + " " + x.MiddleName + " " + x.LastName + " (" + x.aspnet_Users.aspnet_Membership.Email + ") "
            }).ToList();

        }

        public lkpCopyType GetCopyTypeIDByCode(String code)
        {
            var lkpCopyType = LookupManager.GetMessagingLookUpData<lkpCopyType>().Where(cond => cond.CT_IsDeleted == false && cond.CT_Code == code).FirstOrDefault();
            if (lkpCopyType.IsNotNull())
            {
                return lkpCopyType;
            }
            return null;
        }

        public void GetOganisationUsersByUserID(Int32 organizationUserID)
        {
            var OrganizationUserList = SecurityManager.GetOganisationUsersByUserID(organizationUserID);
            View.OrganizationUserList = OrganizationUserList.Select(x => new Entity.OrganizationUser
            {
                FirstName = x.FirstName,
                OrganizationUserID = x.OrganizationUserID,
                LastName = x.LastName,
                PrimaryEmailAddress = x.aspnet_Users.aspnet_Membership.Email,
                MiddleName = x.FirstName + " " + x.MiddleName + " " + x.LastName + " (" + x.aspnet_Users.aspnet_Membership.Email + ") "
            }).ToList();

        }
        //Int32 communicationCCuserID,Int32 currentUserID,Int32 subEventID
        public Boolean IsUserAlreadyMappedToSubEvent(Int32 communicationCCMasterID)
        {
            return CommunicationManager.IsUserAlreadyMappedToSubEvent(communicationCCMasterID, View.CurrentUserId, View.SubEventId);
        }

        public void BindRecordObjectTypes()
        {
            View.RecordObjectTypes = LookupManager.GetMessagingLookUpData<lkpRecordObjectType>().Where(cond => cond.OT_IsDeleted == false
                    && (cond.OT_Code == LCObjectType.ComplianceCategory.GetStringValue() || cond.OT_Code == LCObjectType.ComplianceItem.GetStringValue())).ToList();
        }

        public void BindCategories()
        {
            View.LstComplianceCategory = ComplianceSetupManager.GetComplianceCategories(View.SelectedTenantId, false);
            if (View.LstComplianceCategory.IsNullOrEmpty())
            {
                View.LstComplianceCategory = new List<Entity.ClientEntity.ComplianceCategory>();
            }
        }

        public void BindItems()
        {
            View.LstComplianceItems = ComplianceSetupManager.GetComplianceItems(View.SelectedTenantId, false);
        }
        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
