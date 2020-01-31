using CoreWeb.Shell;
using Entity.SharedDataEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
//using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class AgencyHierarchyUserPermission : BaseUserControl, IAgencyHierarchyUserPermissionView
    {
        #region Handlers
        public delegate bool ShowMessageHandler(object sender, StatusMessages messageType, String message);
        public event ShowMessageHandler eventShowMessage;
        #endregion

        #region [Private Variables]

        private AgencyHierarchyUserPermissionPresenter _presenter = new AgencyHierarchyUserPermissionPresenter();

        #endregion

        #region [Properties]

        #region [Public Properties]

        public AgencyHierarchyUserPermissionPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public IAgencyHierarchyUserPermissionView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IAgencyHierarchyUserPermissionView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IAgencyHierarchyUserPermissionView.AgencyHierarchyId
        {
            get
            {
                return Convert.ToInt32(ViewState["AgencyHierarchyId"]);
            }
            set
            {
                ViewState["AgencyHierarchyId"] = value;
            }
        }

        List<AgencyHierarchyUserContract> IAgencyHierarchyUserPermissionView.lstAgencyUsers
        {
            get;
            set;
        }

        public List<Entity.SharedDataEntity.lkpInvitationSharedInfoType> LstSharedInfoType
        {
            get;
            set;
        }

        public List<Entity.SharedDataEntity.ApplicantInvitationMetaData> LstSharedInfo
        {
            get;
            set;
        }

        List<AgencyHierarchyUserContract> IAgencyHierarchyUserPermissionView.lstAgencyHierarchyUsers
        {
            get;
            set;
        }

        AgencyHierarchyUserContract IAgencyHierarchyUserPermissionView.agencyHierarchyUserContract
        {
            get;
            set;
        }
        public Int32 NodeId { get; set; }

        //UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
        List<lkpAgencyUserNotification> IAgencyHierarchyUserPermissionView.lstAgencyUserNotification
        {
            get;
            set;
        }

        List<AgencyUserPermissionTemplateContract> IAgencyHierarchyUserPermissionView.lstAgencyUserPerTemplates
        {
            get;
            set;
        }


        #region UAT-3316
        List<AgencyUserPermissionTemplateMapping> IAgencyHierarchyUserPermissionView.lstAgencyUserPerTemplatesMappings
        {
            get;
            set;
        }
        List<AgencyUserPerTemplateNotificationMapping> IAgencyHierarchyUserPermissionView.lstAgencyUserPerTemplatesNotificationMappings
        {
            get;
            set;
        }
        List<lkpAgencyUserPermissionType> IAgencyHierarchyUserPermissionView.lstAgencyUserPermisisonType
        {
            get;
            set;
        }

        public List<Int32> InvitationSharedInfoTypeIDs
        {
            get;
            set;
        }

        public List<Int32?> ApplicantInvMetaDataIDs
        {
            get;
            set;
        }
        #endregion
        #region UAT-3664 Properties
        List<lkpAgencyUserReport> IAgencyHierarchyUserPermissionView.lstAgencyUserReports
        {
            get
            {
                if (!ViewState["lstAgencyUserReports"].IsNullOrEmpty())
                    return (List<lkpAgencyUserReport>)ViewState["lstAgencyUserReports"];
                return new List<lkpAgencyUserReport>();
            }
            set
            {
                ViewState["lstAgencyUserReports"] = value;
            }
        }

        List<AgencyUserReportPermissionContract> IAgencyHierarchyUserPermissionView.lstAgencyUserReportPermission
        {
            get
            {
                if (!ViewState["lstAgencyUserReportPermission"].IsNullOrEmpty())
                    return (List<AgencyUserReportPermissionContract>)ViewState["lstAgencyUserReportPermission"];
                return new List<AgencyUserReportPermissionContract>();
            }
            set
            {
                ViewState["lstAgencyUserReportPermission"] = value;
            }
        }

        List<AgencyUserPermissionTemplateMapping> IAgencyHierarchyUserPermissionView.lstTemplateReportPermissions
        {
            get
            {
                if (!ViewState["lstTemplateReportPermissions"].IsNullOrEmpty())
                    return (List<AgencyUserPermissionTemplateMapping>)ViewState["lstTemplateReportPermissions"];
                return new List<AgencyUserPermissionTemplateMapping>();
            }
            set
            {
                ViewState["lstTemplateReportPermissions"] = value;
            }
        }

        #endregion
        #endregion

        #region [Private Properties]

        #endregion

        #endregion

        #region [Page Events]
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    CurrentViewContext.AgencyHierarchyId = NodeId;
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }
        #endregion

        #region [Controls Events]

        #region [Combobox Events]

        #endregion

        #region [Grid Events]

        protected void grdAgencyHirarchyUserPermission_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    ViewState["SortExpression"] = e.SortExpression;
                    ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    ViewState["SortExpression"] = String.Empty;
                    ViewState["SortDirection"] = false;
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdAgencyHirarchyUserPermission_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetAgencyHirarchyAgencyUsers();
                grdAgencyHirarchyUserPermission.DataSource = CurrentViewContext.lstAgencyHierarchyUsers;

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdAgencyHirarchyUserPermission_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                AgencyHierarchyUserContract agencyHierarchyUserContract = new AgencyHierarchyUserContract();
                agencyHierarchyUserContract.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserId;
                agencyHierarchyUserContract.AgencyHierarchyID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AgencyHierarchyID"]);
                agencyHierarchyUserContract.AGU_ID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AGU_ID"]);
                CurrentViewContext.agencyHierarchyUserContract = agencyHierarchyUserContract;
                if (Presenter.DeleteAgencyHierarchyUserMapping())
                    eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, "User Successfully Deleted.");
                else
                    eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, "Unable to delete user.");
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdAgencyHirarchyUserPermission_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox cmbAgencyUser = editform.FindControl("cmbAgencyUser") as WclComboBox;
                    System.Web.UI.HtmlControls.HtmlContainerControl dvAgencyUserPermission = editform.FindControl("dvAgencyUserPermission") as System.Web.UI.HtmlControls.HtmlContainerControl;

                    AgencyHierarchyUserContract agencyHierarchyUserContract = e.Item.DataItem as AgencyHierarchyUserContract;
                    if (e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem)
                    {
                        var divpermissionTemplate = e.Item.FindControl("divpermissionTemplate");
                        divpermissionTemplate.Visible = false;

                    }
                    if (agencyHierarchyUserContract != null)
                    {
                        Presenter.GetSharedInfo();
                        Presenter.GetApplicantInvitationMetaData();
                        Presenter.GetAgencyUserPerTemplates();
                        //Presenter.GetApplicantInvitationMetaData();


                        #region UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives

                        Presenter.GetAgencyUserNotifications();
                        CheckBoxList chkAgencUserNotifications = editform.FindControl("chkAgencUserNotifications") as CheckBoxList;
                        chkAgencUserNotifications.DataSource = CurrentViewContext.lstAgencyUserNotification;
                        chkAgencUserNotifications.DataTextField = "AUN_Name";
                        chkAgencUserNotifications.DataValueField = "AUN_ID";
                        chkAgencUserNotifications.DataBind();

                        Int32 RequirementSharingNonRotationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.REQUIREMENTS_SHARING_INVITATION_NON_ROTATION.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());
                        Int32 RequirementSharingRotationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.REQUIREMENTS_SHARING_INVITATION_ROTATION_SHARING.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());
                        Int32 RotationInvitationApprovalRejectionID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_ROTATION_INVITATION_APPROVAL_REJECTION.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());
                        Int32 IndividualProfileSharingWithEmailID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_INDIVIDUAL_PROFILE_SHARING_WITH_EMAIL.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());
                        Int32 ProfileSharingWithEmailID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_PROFILE_SHARING_WITH_AGENCY_APPROVED.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());//UAT-2942
                        Int32 outOfComplianceNotificationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_UPON_STUDENT_FALL_OUT_OF_COMPLIANCE.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());//UAT-2977
                        Int32 updatedAppReqNotificationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_UPDATED_APPLICANT_REQUIREMENTS.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());//UAT-3059
                        Int32 updatedRotationDetailsNotificationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_UPDATED_ROTATION_DETAILS.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());//UAT-3108
                        Int32 studentDroppedFromRotationNotificationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_STUDENT_DROPPED_FROM_ROTATION.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());//UAT-3222
                        Int32 itSystemAccessFormNotificationId = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_IT_SYSTEM_ACCESS_FORM.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());//UAT-3998
                        Int32 rotationEndDateChangeNotificationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_ROTATION_END_DATE_CHANGE.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault()); //UAT-4561

                        foreach (ListItem item in chkAgencUserNotifications.Items)
                        {
                            if ((Convert.ToInt32(item.Value) == RequirementSharingNonRotationID) && agencyHierarchyUserContract.IsRequirementSharingNonRotationNotification)
                            {
                                item.Selected = true;
                            }
                            else if ((Convert.ToInt32(item.Value) == RequirementSharingRotationID) && agencyHierarchyUserContract.IsRequirementSharingRotationNotification)
                            {
                                item.Selected = true;
                            }
                            else if ((Convert.ToInt32(item.Value) == RotationInvitationApprovalRejectionID) && agencyHierarchyUserContract.IsRotationInvitationApprovalRejectionNotification)
                            {
                                item.Selected = true;
                            }
                            else if ((Convert.ToInt32(item.Value) == IndividualProfileSharingWithEmailID) && agencyHierarchyUserContract.IsIndividualProfileSharingWithEmailNotification)
                            {
                                item.Selected = true;
                            }
                            //UAT-2942
                            else if ((Convert.ToInt32(item.Value) == ProfileSharingWithEmailID) && agencyHierarchyUserContract.IsProfileSharingWithEmailNotification)
                            {
                                item.Selected = true;
                            }
                            //UAT-2977
                            else if ((Convert.ToInt32(item.Value) == outOfComplianceNotificationID) && agencyHierarchyUserContract.SendOutOfComplianceNotification)
                            {
                                item.Selected = true;
                            }
                            else if ((Convert.ToInt32(item.Value) == updatedAppReqNotificationID) && agencyHierarchyUserContract.SendUpdatedApplicantRequirementNotification)
                            {
                                item.Selected = true;
                            }
                            //UAT-3108
                            else if ((Convert.ToInt32(item.Value) == updatedRotationDetailsNotificationID) && agencyHierarchyUserContract.SendUpdatedRotationDetailsNotification)
                            {
                                item.Selected = true;
                            }
                            //UAT-3222
                            else if ((Convert.ToInt32(item.Value) == studentDroppedFromRotationNotificationID) && agencyHierarchyUserContract.SendStudentDroppedFromRotationNotification)
                            {
                                item.Selected = true;
                            }
                            //UAT-3998
                            else if ((Convert.ToInt32(item.Value) == itSystemAccessFormNotificationId) && agencyHierarchyUserContract.SendItSystemAccessFormNotification)
                            {
                                item.Selected = true;
                            }
                            //UAT-4561
                            else if ((Convert.ToInt32(item.Value) == rotationEndDateChangeNotificationID) && agencyHierarchyUserContract.SendRotationEndDateChangeNotification)
                            {
                                item.Selected = true;
                            }
                        }
                        #endregion

                        WclComboBox cmbAgencyPermissionTemplate = editform.FindControl("cmbAgencyPermissionTemplate") as WclComboBox;
                        cmbAgencyPermissionTemplate.DataSource = CurrentViewContext.lstAgencyUserPerTemplates;
                        cmbAgencyPermissionTemplate.DataTextField = "AGUPT_Name";
                        cmbAgencyPermissionTemplate.DataValueField = "AGUPT_ID";
                        cmbAgencyPermissionTemplate.DataBind();
                        cmbAgencyPermissionTemplate.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("Custom Permissions"));

                        WclComboBox cmbCompliancePermissions = editform.FindControl("cmbCompliancePermissions") as WclComboBox;
                        cmbCompliancePermissions.DataSource = CurrentViewContext.LstSharedInfoType.Where(x => x.MasterInfoTypeCode == SharedInfoMasterType.MASTERTYPE_COMPLIANCE.GetStringValue());
                        cmbCompliancePermissions.DataTextField = "SharedInfoType";
                        cmbCompliancePermissions.DataValueField = "SharedInfoTypeID";
                        cmbCompliancePermissions.DataBind();

                        WclComboBox cmbProfileSharedInformation = editform.FindControl("cmbProfileSharedInformation") as WclComboBox;
                        cmbProfileSharedInformation.DataSource = CurrentViewContext.LstSharedInfo;
                        cmbProfileSharedInformation.DataBind();

                        WclComboBox permissionTypeBkg = editform.FindControl("cmbBackgroundPermission") as WclComboBox;
                        permissionTypeBkg.DataSource = CurrentViewContext.LstSharedInfoType.Where(x => x.MasterInfoTypeCode == SharedInfoMasterType.MASTERTYPE_BACKGROUND.GetStringValue());
                        permissionTypeBkg.DataBind();

                        cmbAgencyUser.SelectedValue = agencyHierarchyUserContract.AGU_ID.ToString();
                        cmbAgencyUser.Enabled = false;

                        WclComboBox cmbRotationPermissions = editform.FindControl("cmbRotationPermissions") as WclComboBox;
                        cmbRotationPermissions.DataSource = CurrentViewContext.LstSharedInfoType.Where(x => x.MasterInfoTypeCode == SharedInfoMasterType.MASTERTYPE_REQUIREMENT_ROTATION.GetStringValue());
                        cmbRotationPermissions.DataTextField = "SharedInfoType";
                        cmbRotationPermissions.DataValueField = "SharedInfoTypeID";
                        cmbRotationPermissions.DataBind();

                        RadioButtonList agencyUserPermission = editform.FindControl("rblMngAgencyUserPermsn") as RadioButtonList;
                        RadioButtonList rotationPackagePermission = editform.FindControl("rblMngRotationPckgPermsn") as RadioButtonList;
                        RadioButtonList rblAttestationTxtPermsn = editform.FindControl("rblAttestationTxtPermsn") as RadioButtonList;
                        RadioButtonList rblSSNpermission = editform.FindControl("rblSSNpermission") as RadioButtonList; //UAT-2510
                        RadioButtonList rblAgencyPortalDetailLinkpermission = editform.FindControl("rblAgencyPortalDetailLink") as RadioButtonList; //UAT-3220
                        RadioButtonList rblEmailNeedtoSend = editform.FindControl("rblEmailNeedtoSend") as RadioButtonList;
                        //UAT-2706
                        RadioButtonList rblRequirementPackage = editform.FindControl("rblRequirementPackage") as RadioButtonList;


                        //UAT-2427
                        RadioButtonList rblAllowJobPosting = editform.FindControl("rblAllowJobPosting") as RadioButtonList;

                        //UAT-2427
                        RadioButtonList rblDoNotShowNonAgencyShares = editform.FindControl("rblDoNotShowNonAgencyShares") as RadioButtonList;

                        foreach (RadComboBoxItem item in cmbProfileSharedInformation.Items)
                        {
                            if (item.Value == AppConsts.STR_ONE)
                            {
                                item.Checked = true;
                                item.Enabled = false;
                            }
                            break;
                        }

                        //Start UAT-3664
                        WclComboBox cmbReportPermissions = editform.FindControl("cmbReportPermissions") as WclComboBox;
                        if (!cmbReportPermissions.IsNullOrEmpty())
                        {
                            Presenter.GetAgencyUserReports();
                            cmbReportPermissions.DataSource = CurrentViewContext.lstAgencyUserReports;
                            cmbReportPermissions.DataBind();
                        }
                        //end UAT-3664

                        //Set Permissions
                        if (e.Item.IsInEditMode)
                        {
                            Int32 AUG_ID = Convert.ToInt32(editform.GetDataKeyValue("AGU_ID"));
                            Presenter.GetInvitationSharedInfoTypeID(agencyHierarchyUserContract.AGU_TemplateId);
                            Presenter.GetApplicationInvitationMetaDataID(agencyHierarchyUserContract.AGU_TemplateId);
                            List<Int32> lstApplicationInvitationMetaDataID = agencyHierarchyUserContract.lstApplicationInvitationMetaDataID; // agencyHierarchyUserContract.lstApplicationInvitationMetaDataID;
                            foreach (RadComboBoxItem item in cmbProfileSharedInformation.Items)
                            {
                                if (lstApplicationInvitationMetaDataID.Contains(Convert.ToInt32(item.Value)))
                                    item.Checked = true;
                                else
                                    item.Checked = false;
                            }

                            List<Int32> lstInvitationSharedInfoTypeID = agencyHierarchyUserContract.lstInvitationSharedInfoTypeID; //agencyHierarchyUserContract.lstInvitationSharedInfoTypeID;
                            foreach (RadComboBoxItem item in permissionTypeBkg.Items)
                            {
                                if (lstInvitationSharedInfoTypeID.IsNotNull() && lstInvitationSharedInfoTypeID.Any()
                                        && lstInvitationSharedInfoTypeID.Contains(Convert.ToInt32(item.Value)))
                                    item.Checked = true;
                                else
                                    item.Checked = false;
                            }

                            cmbAgencyPermissionTemplate.SelectedValue = Convert.ToString(agencyHierarchyUserContract.AGU_TemplateId);
                            //Bind Radiobox List
                            cmbCompliancePermissions.SelectedValue = Convert.ToString(agencyHierarchyUserContract.AGU_ComplianceSharedInfoTypeID);
                            cmbRotationPermissions.SelectedValue = Convert.ToString(agencyHierarchyUserContract.AGU_ReqRotationSharedInfoTypeID);
                            //if (!CurrentViewContext.lstAgencyUserPerTemplates.IsNullOrEmpty())
                            //{

                            //    cmbCompliancePermissions.SelectedValue =  Convert.ToString(agencyHierarchyUserContract.AGU_ComplianceSharedInfoTypeID; //Convert.ToString(CurrentViewContext.lstAgencyUserPerTemplates.Where(x => x.AGUPT_ID == agencyHierarchyUserContract.AGU_TemplateId).FirstOrDefault().AGUPT_ComplianceSharedInfoTypeID); //Convert.ToString(agencyHierarchyUserContract.AGU_ComplianceSharedInfoTypeID);
                            //    cmbRotationPermissions.SelectedValue = Convert.ToString(CurrentViewContext.lstAgencyUserPerTemplates.Where(x => x.AGUPT_ID == agencyHierarchyUserContract.AGU_TemplateId).FirstOrDefault().AGUPT_ReqRotationSharedInfoTypeID);
                            //}
                            agencyUserPermission.SelectedValue = Convert.ToString(agencyHierarchyUserContract.AGU_AgencyUserPermission);
                            rotationPackagePermission.SelectedValue = Convert.ToString(agencyHierarchyUserContract.AGU_RotationPackagePermission);
                            rblAttestationTxtPermsn.SelectedValue = Convert.ToString(agencyHierarchyUserContract.AttestationRptPermission);

                            rblRequirementPackage.SelectedValue = Convert.ToString(agencyHierarchyUserContract.AGU_RotationPackageViewPermission);
                            //UAT-2427
                            rblAllowJobPosting.SelectedValue = Convert.ToString(agencyHierarchyUserContract.AGU_AllowJobPosting);

                            rblDoNotShowNonAgencyShares.SelectedValue = Convert.ToString(agencyHierarchyUserContract.AGU_DoNotShowNonAgencyShares);

                            rblSSNpermission.SelectedValue = Convert.ToString(agencyHierarchyUserContract.SSN_Permission); //UAT-2510   
                            rblAgencyPortalDetailLinkpermission.SelectedValue = Convert.ToString(agencyHierarchyUserContract.HideAgencyPortalDetailLink); //UAT-3220   

                            //UAT-3664 //
                            List<Int32> lstNotAccessTypeReportIds = new List<Int32>();
                            List<AgencyUserReportPermissionContract> lstNotAccessTypeReport = Presenter.GetAgencyUserReportsWithNoAccess(AUG_ID); //Get on the basis of Agency user id and template if exists(priority).
                            if (!lstNotAccessTypeReport.IsNullOrEmpty())
                                lstNotAccessTypeReportIds = lstNotAccessTypeReport.Select(sel => sel.AgencyUserReportID).ToList();

                            foreach (RadComboBoxItem item in cmbReportPermissions.Items)
                            {
                                if (!lstNotAccessTypeReportIds.IsNullOrEmpty() && lstNotAccessTypeReportIds.Contains(Convert.ToInt32(item.Value)))
                                    item.Checked = true;
                                else
                                    item.Checked = false;
                            }

                            //End UAT-3664

                            if (agencyHierarchyUserContract.AGU_TemplateId != 0)
                            {
                                cmbCompliancePermissions.Enabled = false;
                                cmbRotationPermissions.Enabled = false;
                                permissionTypeBkg.Enabled = false;
                                cmbProfileSharedInformation.Enabled = false;
                                rotationPackagePermission.Enabled = false;
                                agencyUserPermission.Enabled = false;
                                rblAttestationTxtPermsn.Enabled = false;
                                rblRequirementPackage.Enabled = false;
                                rblAllowJobPosting.Enabled = false;
                                rblSSNpermission.Enabled = false;
                                rblDoNotShowNonAgencyShares.Enabled = false;
                                rblAgencyPortalDetailLinkpermission.Enabled = false;
                                chkAgencUserNotifications.Enabled = false;
                                cmbReportPermissions.Enabled = false;
                            }

                        }
                    }
                    else
                    {
                        dvAgencyUserPermission.Visible = false;
                    }
                    BindAgencyUsersForAddEditForm(cmbAgencyUser, agencyHierarchyUserContract);
                }

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }

        protected void grdAgencyHirarchyUserPermission_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    WclComboBox cmbAgencyUser = e.Item.FindControl("cmbAgencyUser") as WclComboBox;
                    AgencyHierarchyUserContract agencyHierarchyUserContract = new AgencyHierarchyUserContract();
                    agencyHierarchyUserContract.CurrentLoggedInUser = CurrentViewContext.CurrentLoggedInUserId;


                    if (!cmbAgencyUser.SelectedValue.IsNullOrEmpty())
                    {
                        String DisplayMessage = String.Empty;
                        agencyHierarchyUserContract.AGU_ID = Convert.ToInt32(cmbAgencyUser.SelectedValue);
                        agencyHierarchyUserContract.AgencyHierarchyID = CurrentViewContext.AgencyHierarchyId;
                        CurrentViewContext.agencyHierarchyUserContract = agencyHierarchyUserContract;

                        if (e.CommandName == RadGrid.UpdateCommandName)
                        {
                            agencyHierarchyUserContract.IsUpdateFlag = true;
                            DisplayMessage = "User Permission(s) Successfully Updated.";



                            WclComboBox cmbAgencyPermissionTemplate = (e.Item.FindControl("cmbAgencyPermissionTemplate") as WclComboBox);
                            agencyHierarchyUserContract.AGU_TemplateId = cmbAgencyPermissionTemplate.SelectedValue.IsNullOrEmpty() ? 0 : Convert.ToInt32(cmbAgencyPermissionTemplate.SelectedValue); //UAT-3316

                            if (cmbAgencyPermissionTemplate.SelectedValue.IsNullOrEmpty()) ///save values to Agency User Permission table
                            {
                                WclComboBox cmbProfileSharedInformation = (e.Item.FindControl("cmbProfileSharedInformation") as WclComboBox);
                                agencyHierarchyUserContract.lstApplicationInvitationMetaDataID = cmbProfileSharedInformation.CheckedItems.Select(x => Convert.ToInt32(x.Value)).ToList();

                                WclComboBox cmbCompliancePermissions = (e.Item.FindControl("cmbCompliancePermissions") as WclComboBox);
                                agencyHierarchyUserContract.AGU_ComplianceSharedInfoTypeID = Convert.ToInt32(cmbCompliancePermissions.SelectedValue);

                                WclComboBox cmbRotationPermissions = (e.Item.FindControl("cmbRotationPermissions") as WclComboBox);
                                agencyHierarchyUserContract.AGU_ReqRotationSharedInfoTypeID = Convert.ToInt32(cmbRotationPermissions.SelectedValue);

                                WclComboBox cmbBackgroundPermission = (e.Item.FindControl("cmbBackgroundPermission") as WclComboBox);
                                agencyHierarchyUserContract.lstInvitationSharedInfoTypeID = cmbBackgroundPermission.CheckedItems.Select(x => Convert.ToInt32(x.Value)).ToList();

                                RadioButtonList rblManageAgencyUserPermission = (e.Item.FindControl("rblMngAgencyUserPermsn") as RadioButtonList);
                                agencyHierarchyUserContract.AGU_AgencyUserPermission = Convert.ToBoolean(rblManageAgencyUserPermission.SelectedValue);

                                RadioButtonList rblManageRotationPackagePermission = (e.Item.FindControl("rblMngRotationPckgPermsn") as RadioButtonList);
                                agencyHierarchyUserContract.AGU_RotationPackagePermission = Convert.ToBoolean(rblManageRotationPackagePermission.SelectedValue);

                                RadioButtonList rblAttestationTxtPermsn = (e.Item.FindControl("rblAttestationTxtPermsn") as RadioButtonList);
                                agencyHierarchyUserContract.IsManageAttestationPermission = Convert.ToBoolean(rblAttestationTxtPermsn.SelectedValue);

                                //Code commented for UAT-2803
                                //RadioButtonList rblEmailNeedtoSend = (e.Item.FindControl("rblEmailNeedtoSend") as RadioButtonList);
                                //agencyHierarchyUserContract.IsEmailNeedToSend = Convert.ToBoolean(rblEmailNeedtoSend.SelectedValue);

                                //UAT-2706
                                RadioButtonList rblRequirementPackage = (e.Item.FindControl("rblRequirementPackage") as RadioButtonList);
                                agencyHierarchyUserContract.AGU_RotationPackageViewPermission = Convert.ToBoolean(rblRequirementPackage.SelectedValue);

                                //UAT-2427
                                RadioButtonList rblAllowJobPosting = (e.Item.FindControl("rblAllowJobPosting") as RadioButtonList);
                                agencyHierarchyUserContract.AGU_AllowJobPosting = Convert.ToBoolean(rblAllowJobPosting.SelectedValue);

                                //UAT-2510
                                RadioButtonList rblSSNpermission = (e.Item.FindControl("rblSSNpermission") as RadioButtonList);
                                agencyHierarchyUserContract.SSN_Permission = Convert.ToBoolean(rblSSNpermission.SelectedValue);

                                //UAT-3220
                                RadioButtonList rblAgencyPortalDetailLink = (e.Item.FindControl("rblAgencyPortalDetailLink") as RadioButtonList);
                                agencyHierarchyUserContract.HideAgencyPortalDetailLink = Convert.ToBoolean(rblAgencyPortalDetailLink.SelectedValue);

                                //UAT-2840
                                RadioButtonList rblDoNotShowNonAgencyShares = (e.Item.FindControl("rblDoNotShowNonAgencyShares") as RadioButtonList);
                                agencyHierarchyUserContract.AGU_DoNotShowNonAgencyShares = Convert.ToBoolean(rblDoNotShowNonAgencyShares.SelectedValue);

                                //UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
                                CheckBoxList chkAgencUserNotifications = (e.Item.FindControl("chkAgencUserNotifications") as CheckBoxList);
                                Dictionary<Int32, Boolean> dicNotificationData = new Dictionary<Int32, Boolean>();
                                for (int i = 0; i < chkAgencUserNotifications.Items.Count; i++)
                                {
                                    Boolean isChecked = chkAgencUserNotifications.Items[i].Selected;
                                    Int32 selectedNotificationID = Convert.ToInt32(chkAgencUserNotifications.Items[i].Value);
                                    dicNotificationData.Add(selectedNotificationID, isChecked);
                                }
                                agencyHierarchyUserContract.dicNotificationData = dicNotificationData;

                                #region Reports Permissions UAT-3664, checked Items will be added with no access permission.

                                List<AgencyUserPermission> lstReportsPermissions = new List<AgencyUserPermission>();
                                //Int32 agencyUserPermissionTypeId = Presenter.GetAgencyUserPermissionTypeID(AgencyUserPermissionType.REPORTS_PERMISSION.GetStringValue());
                                //Int32 agencyUserPermissionAccessTypeId = Presenter.GetAgencyUserPermissionAccessTypeID(AgencyUserPermissionAccessType.NO.GetStringValue());  //Checked means no access.

                                WclComboBox cmbReportPermissions = (e.Item.FindControl("cmbReportPermissions") as WclComboBox);
                                agencyHierarchyUserContract.lstCheckedReportsTypeID = new List<Int32>();
                                agencyHierarchyUserContract.lstCheckedReportsTypeID = cmbReportPermissions.CheckedItems.Select(x => Convert.ToInt32(x.Value)).ToList();
                                #endregion
                            }

                        }
                        else
                        {
                            agencyHierarchyUserContract.IsUpdateFlag = false;
                            DisplayMessage = "User Successfully Added.";
                        }
                        if (Presenter.SaveUpdateAgencyHierarchyUserMapping())
                            eventShowMessage(sender, StatusMessages.SUCCESS_MESSAGE, DisplayMessage);
                        else
                            eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, "Error while adding user to selected node.");
                    }

                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                eventShowMessage(sender, StatusMessages.ERROR_MESSAGE, ex.Message);
            }
        }
        #endregion

        #endregion

        #region [Private Methods]
        private void BindAgencyUsersForAddEditForm(RadComboBox cmbAgencyUserOnAddEditForm, AgencyHierarchyUserContract agencyUserContract)
        {
            Presenter.GetAgencyUsers(agencyUserContract);
            cmbAgencyUserOnAddEditForm.DataSource = CurrentViewContext.lstAgencyUsers;
            cmbAgencyUserOnAddEditForm.DataBind();
            cmbAgencyUserOnAddEditForm.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
            cmbAgencyUserOnAddEditForm.Focus();
        }
        #endregion

        protected void cmbAgencyPermissionTemplate_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

            WclComboBox cmbAgencyPermissionTemplate = sender as WclComboBox;
            var divPermissionSection = cmbAgencyPermissionTemplate.Parent.NamingContainer.FindControl("divPermissions");

            #region GetControls
            WclComboBox cmbProfileSharedInformation = cmbAgencyPermissionTemplate.Parent.NamingContainer.FindControl("cmbProfileSharedInformation") as WclComboBox;

            WclComboBox cmbCompliancePermissions = cmbAgencyPermissionTemplate.Parent.NamingContainer.FindControl("cmbCompliancePermissions") as WclComboBox;
            WclComboBox cmbBackgroundPermission = cmbAgencyPermissionTemplate.Parent.NamingContainer.FindControl("cmbBackgroundPermission") as WclComboBox;
            WclComboBox cmbRotationPermissions = cmbAgencyPermissionTemplate.Parent.NamingContainer.FindControl("cmbRotationPermissions") as WclComboBox;

            RadioButtonList rblMngRotationPckgPermsn = cmbAgencyPermissionTemplate.Parent.NamingContainer.FindControl("rblMngRotationPckgPermsn") as RadioButtonList;
            RadioButtonList rblMngAgencyUserPermsn = cmbAgencyPermissionTemplate.Parent.NamingContainer.FindControl("rblMngAgencyUserPermsn") as RadioButtonList;
            RadioButtonList rblAttestationTxtPermsn = cmbAgencyPermissionTemplate.Parent.NamingContainer.FindControl("rblAttestationTxtPermsn") as RadioButtonList;

            RadioButtonList rblRequirementPackage = cmbAgencyPermissionTemplate.Parent.NamingContainer.FindControl("rblRequirementPackage") as RadioButtonList;
            RadioButtonList rblAllowJobPosting = cmbAgencyPermissionTemplate.Parent.NamingContainer.FindControl("rblAllowJobPosting") as RadioButtonList;
            RadioButtonList rblSSNpermission = cmbAgencyPermissionTemplate.Parent.NamingContainer.FindControl("rblSSNpermission") as RadioButtonList;
            RadioButtonList rblDoNotShowNonAgencyShares = cmbAgencyPermissionTemplate.Parent.NamingContainer.FindControl("rblDoNotShowNonAgencyShares") as RadioButtonList;
            RadioButtonList rblAgencyPortalDetailLink = cmbAgencyPermissionTemplate.Parent.NamingContainer.FindControl("rblAgencyPortalDetailLink") as RadioButtonList;
            WclComboBox cmbAgencyUser = cmbAgencyPermissionTemplate.Parent.NamingContainer.FindControl("cmbAgencyUser") as WclComboBox;
            CheckBoxList chkAgencUserNotifications = cmbAgencyPermissionTemplate.Parent.NamingContainer.FindControl("chkAgencUserNotifications") as CheckBoxList;
            WclComboBox cmbReportPermissions = cmbAgencyPermissionTemplate.Parent.NamingContainer.FindControl("cmbReportPermissions") as WclComboBox; //UAT-3664
            #endregion


            //divPermissionSection.Visible = true;
            if (!cmbAgencyPermissionTemplate.SelectedValue.IsNullOrEmpty() && !(cmbAgencyPermissionTemplate.SelectedItem.Text == "Custom Permissions"))
            {
                Int32 permissionTemplateId = cmbAgencyPermissionTemplate.SelectedValue.IsNullOrEmpty() ? 0 : Convert.ToInt32(cmbAgencyPermissionTemplate.SelectedValue);
                Presenter.GetAgencyUsrPerTemplateMappings(Convert.ToInt32(cmbAgencyPermissionTemplate.SelectedValue));
                Presenter.GetAgencyUsrPerTemplateNotificationsMappings(Convert.ToInt32(cmbAgencyPermissionTemplate.SelectedValue));
                Presenter.GetAgencyUserNotifications();
                Presenter.GetAgencyUserPermissionTypes();


                #region UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
                Int32 RequirementSharingNonRotationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.REQUIREMENTS_SHARING_INVITATION_NON_ROTATION.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());
                Int32 RequirementSharingRotationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.REQUIREMENTS_SHARING_INVITATION_ROTATION_SHARING.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());
                Int32 RotationInvitationApprovalRejectionID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_ROTATION_INVITATION_APPROVAL_REJECTION.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());
                Int32 IndividualProfileSharingWithEmailID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_INDIVIDUAL_PROFILE_SHARING_WITH_EMAIL.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());
                Int32 ProfileSharingWithEmailID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_PROFILE_SHARING_WITH_AGENCY_APPROVED.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());//UAT-2942
                Int32 outOfComplianceNotificationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_UPON_STUDENT_FALL_OUT_OF_COMPLIANCE.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());//UAT-2977
                Int32 updatedAppReqNotificationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_UPDATED_APPLICANT_REQUIREMENTS.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());//UAT-3059
                Int32 updatedRotationDetailsNotificationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_UPDATED_ROTATION_DETAILS.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());//UAT-3108
                Int32 studentDroppedFromROtationNotificationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_STUDENT_DROPPED_FROM_ROTATION.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());//UAT-3222

                foreach (ListItem item in chkAgencUserNotifications.Items)
                {
                    item.Selected = false;
                }

                foreach (ListItem item in chkAgencUserNotifications.Items)
                {
                    foreach (var notificationIds in CurrentViewContext.lstAgencyUserPerTemplatesNotificationMappings)
                    {

                        if ((Convert.ToInt32(item.Value) == RequirementSharingNonRotationID) && (RequirementSharingNonRotationID == notificationIds.AGUPTNM_NotificationTypeID && notificationIds.AGUPTNM_IsMailToBeSend))
                        {
                            item.Selected = true;
                            break;
                        }
                        else if ((Convert.ToInt32(item.Value) == RequirementSharingRotationID) && (RequirementSharingRotationID == notificationIds.AGUPTNM_NotificationTypeID && notificationIds.AGUPTNM_IsMailToBeSend))
                        {
                            item.Selected = true;
                            break;
                        }
                        else if ((Convert.ToInt32(item.Value) == RotationInvitationApprovalRejectionID) && (RotationInvitationApprovalRejectionID == notificationIds.AGUPTNM_NotificationTypeID && notificationIds.AGUPTNM_IsMailToBeSend))
                        {
                            item.Selected = true;
                            break;
                        }
                        else if ((Convert.ToInt32(item.Value) == IndividualProfileSharingWithEmailID) && (IndividualProfileSharingWithEmailID == notificationIds.AGUPTNM_NotificationTypeID && notificationIds.AGUPTNM_IsMailToBeSend))
                        {
                            item.Selected = true;
                            break;
                        }
                        else if ((Convert.ToInt32(item.Value) == ProfileSharingWithEmailID) && (ProfileSharingWithEmailID == notificationIds.AGUPTNM_NotificationTypeID && notificationIds.AGUPTNM_IsMailToBeSend))
                        {
                            item.Selected = true;
                            break;
                        }
                        else if ((Convert.ToInt32(item.Value) == outOfComplianceNotificationID) && (outOfComplianceNotificationID == notificationIds.AGUPTNM_NotificationTypeID && notificationIds.AGUPTNM_IsMailToBeSend))
                        {
                            item.Selected = true;
                            break;
                        }
                        else if ((Convert.ToInt32(item.Value) == updatedAppReqNotificationID) && (updatedAppReqNotificationID == notificationIds.AGUPTNM_NotificationTypeID && notificationIds.AGUPTNM_IsMailToBeSend))
                        {
                            item.Selected = true;
                            break;
                        }
                        else if ((Convert.ToInt32(item.Value) == updatedRotationDetailsNotificationID) && (updatedRotationDetailsNotificationID == notificationIds.AGUPTNM_NotificationTypeID && notificationIds.AGUPTNM_IsMailToBeSend))
                        {
                            item.Selected = true;
                            break;
                        }
                        else if ((Convert.ToInt32(item.Value) == studentDroppedFromROtationNotificationID) && (studentDroppedFromROtationNotificationID == notificationIds.AGUPTNM_NotificationTypeID && notificationIds.AGUPTNM_IsMailToBeSend))
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }

                #endregion

                #region BindPermissionValues
                Int32 rblMngRotationPckgPermsnId = Convert.ToInt32(CurrentViewContext.lstAgencyUserPermisisonType.Where(con => con.AUPT_Code == AgencyUserPermissionType.ROTATION_PACKAGE_PERMISSION.GetStringValue() && !con.AUPT_IsDeleted).Select(sel => sel.AUPT_ID).FirstOrDefault());
                Int32 rblMngAgencyUserPermsnId = Convert.ToInt32(CurrentViewContext.lstAgencyUserPermisisonType.Where(con => con.AUPT_Code == AgencyUserPermissionType.AGENCY_USER_PERMISSION.GetStringValue() && !con.AUPT_IsDeleted).Select(sel => sel.AUPT_ID).FirstOrDefault());
                Int32 rblAttestationTxtPermsnId = Convert.ToInt32(CurrentViewContext.lstAgencyUserPermisisonType.Where(con => con.AUPT_Code == AgencyUserPermissionType.ATTESTATION_REPORT_TEXT_PERMISSION.GetStringValue() && !con.AUPT_IsDeleted).Select(sel => sel.AUPT_ID).FirstOrDefault());
                Int32 rblSSNpermissionId = Convert.ToInt32(CurrentViewContext.lstAgencyUserPermisisonType.Where(con => con.AUPT_Code == AgencyUserPermissionType.GRANULAR_SSN_PERMISSION.GetStringValue() && !con.AUPT_IsDeleted).Select(sel => sel.AUPT_ID).FirstOrDefault());
                Int32 rblAgencyPortalDetailLinkId = Convert.ToInt32(CurrentViewContext.lstAgencyUserPermisisonType.Where(con => con.AUPT_Code == AgencyUserPermissionType.AGENCY_PORTAL_DETAIL_LINK_PERMISSION.GetStringValue() && !con.AUPT_IsDeleted).Select(sel => sel.AUPT_ID).FirstOrDefault());//UAT-2942

                Int32 rblRequirementPackageID = Convert.ToInt32(CurrentViewContext.lstAgencyUserPermisisonType.Where(con => con.AUPT_Code == AgencyUserPermissionType.ROTATION_PACKAGE_VIEW_PERMISSION.GetStringValue() && !con.AUPT_IsDeleted).Select(sel => sel.AUPT_ID).FirstOrDefault());//UAT-3059
                Int32 rblAllowJobPostingID = Convert.ToInt32(CurrentViewContext.lstAgencyUserPermisisonType.Where(con => con.AUPT_Code == AgencyUserPermissionType.ALLOW_JOB_POSTING_PERMISSION.GetStringValue() && !con.AUPT_IsDeleted).Select(sel => sel.AUPT_ID).FirstOrDefault());//UAT-3108
                Int32 rblDoNotShowNonAgencySharesID = Convert.ToInt32(CurrentViewContext.lstAgencyUserPermisisonType.Where(con => con.AUPT_Code == AgencyUserPermissionType.DONOT_SHOW_NON_AGENCY_SHARES_PERMISSION.GetStringValue() && !con.AUPT_IsDeleted).Select(sel => sel.AUPT_ID).FirstOrDefault());//UAT-3222

                foreach (var agencyUserPerTemp in CurrentViewContext.lstAgencyUserPerTemplatesMappings)
                {
                    if (agencyUserPerTemp.AGUPTM_PermissionTypeID == rblMngRotationPckgPermsnId)
                        rblMngRotationPckgPermsn.SelectedValue = agencyUserPerTemp.AGUPTM_PermissionAccessTypeID == 2 ? "False" : Convert.ToString(Convert.ToBoolean(agencyUserPerTemp.AGUPTM_PermissionAccessTypeID));

                    if (agencyUserPerTemp.AGUPTM_PermissionTypeID == rblMngAgencyUserPermsnId)
                        rblMngAgencyUserPermsn.SelectedValue = agencyUserPerTemp.AGUPTM_PermissionAccessTypeID == 2 ? "False" : Convert.ToString(Convert.ToBoolean(agencyUserPerTemp.AGUPTM_PermissionAccessTypeID));

                    if (agencyUserPerTemp.AGUPTM_PermissionTypeID == rblAttestationTxtPermsnId)
                        rblAttestationTxtPermsn.SelectedValue = agencyUserPerTemp.AGUPTM_PermissionAccessTypeID == 2 ? "False" : Convert.ToString(Convert.ToBoolean(agencyUserPerTemp.AGUPTM_PermissionAccessTypeID));

                    if (agencyUserPerTemp.AGUPTM_PermissionTypeID == rblSSNpermissionId)
                        rblSSNpermission.SelectedValue = agencyUserPerTemp.AGUPTM_PermissionAccessTypeID == 2 ? "False" : Convert.ToString(Convert.ToBoolean(agencyUserPerTemp.AGUPTM_PermissionAccessTypeID));
                    //Convert.ToString(agencyUserContract.SSN_Permission);

                    if (agencyUserPerTemp.AGUPTM_PermissionTypeID == rblAgencyPortalDetailLinkId)
                        rblAgencyPortalDetailLink.SelectedValue = agencyUserPerTemp.AGUPTM_PermissionAccessTypeID == 2 ? "False" : Convert.ToString(Convert.ToBoolean(agencyUserPerTemp.AGUPTM_PermissionAccessTypeID));

                    if (agencyUserPerTemp.AGUPTM_PermissionTypeID == rblRequirementPackageID)
                        rblRequirementPackage.SelectedValue = agencyUserPerTemp.AGUPTM_PermissionAccessTypeID == 2 ? "False" : Convert.ToString(Convert.ToBoolean(agencyUserPerTemp.AGUPTM_PermissionAccessTypeID));

                    if (agencyUserPerTemp.AGUPTM_PermissionTypeID == rblAllowJobPostingID)
                        rblAllowJobPosting.SelectedValue = agencyUserPerTemp.AGUPTM_PermissionAccessTypeID == 2 ? "False" : Convert.ToString(Convert.ToBoolean(agencyUserPerTemp.AGUPTM_PermissionAccessTypeID));

                    if (agencyUserPerTemp.AGUPTM_PermissionTypeID == rblDoNotShowNonAgencySharesID)
                        rblDoNotShowNonAgencyShares.SelectedValue = agencyUserPerTemp.AGUPTM_PermissionAccessTypeID == 2 ? "False" : Convert.ToString(Convert.ToBoolean(agencyUserPerTemp.AGUPTM_PermissionAccessTypeID));
                }
                #endregion

                Presenter.GetAgencyUserPerTemplates();
                //cmbCompliancePermissions.SelectedValue = Convert.ToString(CurrentViewContext.lstAgencyUserPerTemplates.Where(x => x.AGUPT_ID == permissionTemplateId && !x.AGUPT_IsDeleted).FirstOrDefault().AGUPT_ComplianceSharedInfoTypeID);
                //cmbRotationPermissions.SelectedValue = Convert.ToString(CurrentViewContext.lstAgencyUserPerTemplates.Where(x => x.AGUPT_ID == permissionTemplateId && !x.AGUPT_IsDeleted).FirstOrDefault().AGUPT_ReqRotationSharedInfoTypeID);

                AgencyUserPermissionTemplateContract agencyUserPermissionTemplateContract = CurrentViewContext.lstAgencyUserPerTemplates.Where(x => x.AGUPT_ID == permissionTemplateId && !x.AGUPT_IsDeleted).FirstOrDefault();


                cmbCompliancePermissions.SelectedValue = Convert.ToString(agencyUserPermissionTemplateContract.AGUPT_ComplianceSharedInfoTypeID);
                cmbRotationPermissions.SelectedValue = Convert.ToString(agencyUserPermissionTemplateContract.AGUPT_ReqRotationSharedInfoTypeID);
                Presenter.GetInvitationSharedInfoTypeID(agencyUserPermissionTemplateContract.AGUPT_ID);
                Presenter.GetApplicationInvitationMetaDataID(agencyUserPermissionTemplateContract.AGUPT_ID);
                List<Int32> lstInvitationSharedInfoTypeID = CurrentViewContext.InvitationSharedInfoTypeIDs;
                foreach (RadComboBoxItem item in cmbBackgroundPermission.Items)
                {
                    if (lstInvitationSharedInfoTypeID.IsNotNull() && lstInvitationSharedInfoTypeID.Any()
                            && lstInvitationSharedInfoTypeID.Contains(Convert.ToInt32(item.Value)))
                        item.Checked = true;
                    else
                        item.Checked = false;
                }

                List<Int32?> lstApplicationInvitationMetaDataID = CurrentViewContext.ApplicantInvMetaDataIDs;
                foreach (RadComboBoxItem item in cmbProfileSharedInformation.Items)
                {
                    if (lstApplicationInvitationMetaDataID.Contains(Convert.ToInt32(item.Value)))
                        item.Checked = true;
                    else
                        item.Checked = false;
                }

                //Start UAT-3664
                Int32 templateId = Convert.ToInt32(cmbAgencyPermissionTemplate.SelectedValue);
                Presenter.GetAgencyUserTemplateReportPermissions(templateId);

                List<AgencyUserPermissionTemplateMapping> lstAgencyUserReportsWithNoAccess = new List<AgencyUserPermissionTemplateMapping>();
                List<Int32?> lstAgencyUserReportsWithNoAccessID = new List<Int32?>();
                if (!CurrentViewContext.lstTemplateReportPermissions.IsNullOrEmpty())
                {
                    String noAccessTypeCode = AgencyUserPermissionAccessType.NO.GetStringValue();
                    lstAgencyUserReportsWithNoAccess = CurrentViewContext.lstTemplateReportPermissions.Where(cond => cond.lkpAgencyUserPermissionAccessType.AUPAT_Code == noAccessTypeCode).ToList();
                }

                if (!CurrentViewContext.lstTemplateReportPermissions.IsNullOrEmpty())
                    lstAgencyUserReportsWithNoAccessID = lstAgencyUserReportsWithNoAccess.Select(Sel => Sel.AGUPTM_RecordTypeID).ToList();

                foreach (RadComboBoxItem item in cmbReportPermissions.Items)
                {
                    if (!lstAgencyUserReportsWithNoAccessID.IsNullOrEmpty() && lstAgencyUserReportsWithNoAccessID.Contains(Convert.ToInt32(item.Value)))
                        item.Checked = true;
                    else
                        item.Checked = false;
                }

                //end UAT-3664

                #region Disable controls
                rblMngRotationPckgPermsn.Enabled = false;
                rblMngAgencyUserPermsn.Enabled = false;
                rblAttestationTxtPermsn.Enabled = false;
                rblRequirementPackage.Enabled = false;
                rblAllowJobPosting.Enabled = false;
                rblSSNpermission.Enabled = false;
                rblDoNotShowNonAgencyShares.Enabled = false;
                rblAgencyPortalDetailLink.Enabled = false;
                chkAgencUserNotifications.Enabled = false;
                cmbProfileSharedInformation.Enabled = false;
                cmbCompliancePermissions.Enabled = false;
                cmbRotationPermissions.Enabled = false;
                cmbBackgroundPermission.Enabled = false;
                cmbReportPermissions.Enabled = false;
                #endregion


            }
            else
            {
                Int32 AgencyUsrId = Convert.ToInt32(cmbAgencyUser.SelectedValue);
                Presenter.GetAgencyHirarchyAgencyUsers();
                AgencyHierarchyUserContract agencyHierarchyUserContract = CurrentViewContext.lstAgencyHierarchyUsers.Where(x => x.AGU_ID == AgencyUsrId).FirstOrDefault(); //e.Item.DataItem as AgencyHierarchyUserContract;

                if (agencyHierarchyUserContract != null)
                {
                    Presenter.GetSharedInfo();
                    Presenter.GetApplicantInvitationMetaData();
                    Presenter.GetAgencyUserPerTemplates();
                    Presenter.GetApplicantInvitationMetaData();


                    #region UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives

                    Presenter.GetAgencyUserNotifications();
                    chkAgencUserNotifications.DataSource = CurrentViewContext.lstAgencyUserNotification;
                    chkAgencUserNotifications.DataTextField = "AUN_Name";
                    chkAgencUserNotifications.DataValueField = "AUN_ID";
                    chkAgencUserNotifications.DataBind();

                    Int32 RequirementSharingNonRotationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.REQUIREMENTS_SHARING_INVITATION_NON_ROTATION.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());
                    Int32 RequirementSharingRotationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.REQUIREMENTS_SHARING_INVITATION_ROTATION_SHARING.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());
                    Int32 RotationInvitationApprovalRejectionID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_ROTATION_INVITATION_APPROVAL_REJECTION.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());
                    Int32 IndividualProfileSharingWithEmailID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_INDIVIDUAL_PROFILE_SHARING_WITH_EMAIL.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());
                    Int32 ProfileSharingWithEmailID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_PROFILE_SHARING_WITH_AGENCY_APPROVED.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());//UAT-2942
                    Int32 outOfComplianceNotificationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_UPON_STUDENT_FALL_OUT_OF_COMPLIANCE.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());//UAT-2977
                    Int32 updatedAppReqNotificationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_UPDATED_APPLICANT_REQUIREMENTS.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());//UAT-3059
                    Int32 updatedRotationDetailsNotificationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_UPDATED_ROTATION_DETAILS.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());//UAT-3108
                    Int32 studentDroppedFromRotationNotificationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_STUDENT_DROPPED_FROM_ROTATION.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault());//UAT-3222
                    Int32 rotationEndDateChangeNotificationID = Convert.ToInt32(CurrentViewContext.lstAgencyUserNotification.Where(con => con.AUN_Code == AgencyUserNotificationLookup.NOTIFICATION_FOR_ROTATION_END_DATE_CHANGE.GetStringValue() && !con.AUN_IsDeleted).Select(sel => sel.AUN_ID).FirstOrDefault()); //UAT-4561

                    foreach (ListItem item in chkAgencUserNotifications.Items)
                    {
                        if ((Convert.ToInt32(item.Value) == RequirementSharingNonRotationID) && agencyHierarchyUserContract.IsRequirementSharingNonRotationNotification)
                        {
                            item.Selected = true;
                        }
                        else if ((Convert.ToInt32(item.Value) == RequirementSharingRotationID) && agencyHierarchyUserContract.IsRequirementSharingRotationNotification)
                        {
                            item.Selected = true;
                        }
                        else if ((Convert.ToInt32(item.Value) == RotationInvitationApprovalRejectionID) && agencyHierarchyUserContract.IsRotationInvitationApprovalRejectionNotification)
                        {
                            item.Selected = true;
                        }
                        else if ((Convert.ToInt32(item.Value) == IndividualProfileSharingWithEmailID) && agencyHierarchyUserContract.IsIndividualProfileSharingWithEmailNotification)
                        {
                            item.Selected = true;
                        }
                        else if ((Convert.ToInt32(item.Value) == ProfileSharingWithEmailID) && agencyHierarchyUserContract.IsProfileSharingWithEmailNotification)
                        {
                            item.Selected = true;
                        }
                        else if ((Convert.ToInt32(item.Value) == outOfComplianceNotificationID) && agencyHierarchyUserContract.SendOutOfComplianceNotification)
                        {
                            item.Selected = true;
                        }
                        else if ((Convert.ToInt32(item.Value) == updatedAppReqNotificationID) && agencyHierarchyUserContract.SendUpdatedApplicantRequirementNotification)
                        {
                            item.Selected = true;
                        }
                        else if ((Convert.ToInt32(item.Value) == updatedRotationDetailsNotificationID) && agencyHierarchyUserContract.SendUpdatedRotationDetailsNotification)
                        {
                            item.Selected = true;
                        }
                        else if ((Convert.ToInt32(item.Value) == studentDroppedFromRotationNotificationID) && agencyHierarchyUserContract.SendStudentDroppedFromRotationNotification)
                        {
                            item.Selected = true;
                        }
                        //UAT-4561
                        else if ((Convert.ToInt32(item.Value) == rotationEndDateChangeNotificationID) && agencyHierarchyUserContract.SendRotationEndDateChangeNotification)
                        {
                            item.Selected = true;
                        }
                    }
                    #endregion


                    cmbAgencyPermissionTemplate.DataSource = CurrentViewContext.lstAgencyUserPerTemplates;
                    cmbAgencyPermissionTemplate.DataTextField = "AGUPT_Name";
                    cmbAgencyPermissionTemplate.DataValueField = "AGUPT_ID";
                    cmbAgencyPermissionTemplate.DataBind();
                    cmbAgencyPermissionTemplate.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("Custom Permissions"));


                    cmbCompliancePermissions.DataSource = CurrentViewContext.LstSharedInfoType.Where(x => x.MasterInfoTypeCode == SharedInfoMasterType.MASTERTYPE_COMPLIANCE.GetStringValue());
                    cmbCompliancePermissions.DataTextField = "SharedInfoType";
                    cmbCompliancePermissions.DataValueField = "SharedInfoTypeID";
                    cmbCompliancePermissions.DataBind();



                    cmbProfileSharedInformation.DataSource = CurrentViewContext.LstSharedInfo;
                    cmbProfileSharedInformation.DataBind();


                    cmbBackgroundPermission.DataSource = CurrentViewContext.LstSharedInfoType.Where(x => x.MasterInfoTypeCode == SharedInfoMasterType.MASTERTYPE_BACKGROUND.GetStringValue());
                    cmbBackgroundPermission.DataBind();

                    cmbAgencyUser.SelectedValue = agencyHierarchyUserContract.AGU_ID.ToString();
                    cmbAgencyUser.Enabled = false;


                    cmbRotationPermissions.DataSource = CurrentViewContext.LstSharedInfoType.Where(x => x.MasterInfoTypeCode == SharedInfoMasterType.MASTERTYPE_REQUIREMENT_ROTATION.GetStringValue());
                    cmbRotationPermissions.DataTextField = "SharedInfoType";
                    cmbRotationPermissions.DataValueField = "SharedInfoTypeID";
                    cmbRotationPermissions.DataBind();

                    List<Int32> lstInvitationSharedInfoTypeID = agencyHierarchyUserContract.lstInvitationSharedInfoTypeID;
                    foreach (RadComboBoxItem item in cmbBackgroundPermission.Items)
                    {
                        if (lstInvitationSharedInfoTypeID.IsNotNull() && lstInvitationSharedInfoTypeID.Any()
                                && lstInvitationSharedInfoTypeID.Contains(Convert.ToInt32(item.Value)))
                            item.Checked = true;
                    }
                    List<Int32> lstApplicationInvitationMetaDataID = agencyHierarchyUserContract.lstApplicationInvitationMetaDataID;
                    foreach (RadComboBoxItem item in cmbProfileSharedInformation.Items)
                    {
                        if (lstApplicationInvitationMetaDataID.Contains(Convert.ToInt32(item.Value)))
                            item.Checked = true;
                    }

                }


                Presenter.GetAgencyUserPerTemplates();
                cmbCompliancePermissions.SelectedValue = Convert.ToString(agencyHierarchyUserContract.AGU_ComplianceSharedInfoTypeID); //Convert.ToString(CurrentViewContext.lstAgencyUserPerTemplates.Where(x => x.AGUPT_ID == permissionTemplateId && !x.AGUPT_IsDeleted).FirstOrDefault().AGUPT_ComplianceSharedInfoTypeID);
                cmbRotationPermissions.SelectedValue = Convert.ToString(agencyHierarchyUserContract.AGU_ReqRotationSharedInfoTypeID);//Convert.ToString(CurrentViewContext.lstAgencyUserPerTemplates.Where(x => x.AGUPT_ID == permissionTemplateId && !x.AGUPT_IsDeleted).FirstOrDefault().AGUPT_ReqRotationSharedInfoTypeID);


                rblAttestationTxtPermsn.SelectedValue = Convert.ToString(agencyHierarchyUserContract.AttestationRptPermission);
                rblRequirementPackage.SelectedValue = Convert.ToString(agencyHierarchyUserContract.AGU_RotationPackageViewPermission);

                rblAllowJobPosting.SelectedValue = Convert.ToString(agencyHierarchyUserContract.AGU_AllowJobPosting);

                rblDoNotShowNonAgencyShares.SelectedValue = Convert.ToString(agencyHierarchyUserContract.AGU_DoNotShowNonAgencyShares);

                rblSSNpermission.SelectedValue = Convert.ToString(agencyHierarchyUserContract.SSN_Permission);
                rblAgencyPortalDetailLink.SelectedValue = Convert.ToString(agencyHierarchyUserContract.HideAgencyPortalDetailLink); //UAT-3220   
                rblMngAgencyUserPermsn.SelectedValue = Convert.ToString(agencyHierarchyUserContract.AGU_AgencyUserPermission);
                rblMngRotationPckgPermsn.SelectedValue = Convert.ToString(agencyHierarchyUserContract.AGU_RotationPackagePermission);


                //UAT-3664
                Presenter.GetAgencyUserReports();
                cmbReportPermissions.DataSource = CurrentViewContext.lstAgencyUserReports;
                cmbReportPermissions.DataTextField = "AUR_Name";
                cmbReportPermissions.DataValueField = "AUR_ID";
                cmbReportPermissions.DataBind();

                cmbReportPermissions.Enabled = true;
                cmbReportPermissions.SelectedValue = AppConsts.ZERO;
                cmbReportPermissions.SelectedIndex = 0;
                //End UAT-3664

                rblMngRotationPckgPermsn.Enabled = true;
                rblMngAgencyUserPermsn.Enabled = true;
                rblAttestationTxtPermsn.Enabled = true;
                rblRequirementPackage.Enabled = true;
                rblAllowJobPosting.Enabled = true;
                rblSSNpermission.Enabled = true;
                rblDoNotShowNonAgencyShares.Enabled = true;
                rblAgencyPortalDetailLink.Enabled = true;
                chkAgencUserNotifications.Enabled = true;
                cmbProfileSharedInformation.Enabled = true;
                cmbCompliancePermissions.Enabled = true;
                cmbRotationPermissions.Enabled = true;
                cmbBackgroundPermission.Enabled = true;


            }

        }

    }
}