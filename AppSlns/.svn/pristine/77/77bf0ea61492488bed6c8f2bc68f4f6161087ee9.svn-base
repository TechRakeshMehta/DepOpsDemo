using System;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using Entity.ClientEntity;
using System.Collections.Generic;
using CoreWeb.Shell;
using Telerik.Web.UI;
using INTSOF.Utils;


namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class AssignmentPropertiesDetail : BaseWebPage, IAssignmentPropertiesDetailView
    {
        private AssignmentPropertiesDetailPresenter _presenter=new AssignmentPropertiesDetailPresenter();
        private Int32 _tenantid;

        
        public AssignmentPropertiesDetailPresenter Presenter
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

        public Int32 CurrentDataID
        {
            get;
            set;
        }

        public Int32 ParentCategoryDataID
        {
            get;
            set;
        }

        public Int32 ParentPackageDataID
        {
            get;
            set;
        }

        public Int32 ParentItemDataID
        {
            get;
            set;
        }

        public String CurrentRuleSetTreeTypeCode
        {
            get;
            set;
        }

        public AssignmentProperty AssignmentPropertyDetails
        {
            get;
            set;
        }

        public List<lkpEditableBy> lstEditableBy
        {
            get;
            set;
        }

        public Int32 ApplicantEditableByID
        {
            get
            {
                return Convert.ToInt32(ViewState["ApplicantEditableByID"]);
            }
            set
            {
                ViewState["ApplicantEditableByID"] = value;
            }
        }

        public List<lkpReviewerType> lstReviewerType
        {
            get;
            set;

        }

        public List<Tenant> LstThirdPartyReviewer
        {
            get;
            set;
        }

        public int CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public int TenantId
        {
            get
            {
                if (_tenantid == 0)
                    _tenantid = Convert.ToInt32(Request.QueryString["tenantId"]);
                return _tenantid;
            }
            set { _tenantid = value; }
        }

        public IAssignmentPropertiesDetailView CurrentViewContext
        {
            get { return this; }
        }

        public Int32 selectedReviewerId
        {
            get;
            set;
        }

        public List<Entity.OrganizationUser> LstThirdPartyUser
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["CurrentDataId"] != null)
            {
                CurrentDataID = Convert.ToInt32(Request.QueryString["CurrentDataId"]);
            }

            if (Request.QueryString["PackageId"] != null)
            {
                ParentPackageDataID = Convert.ToInt32(Request.QueryString["PackageId"]);
            }

            if (Request.QueryString["CategoryId"] != null)
            {
                ParentCategoryDataID = Convert.ToInt32(Request.QueryString["CategoryId"]);
            }

            if (Request.QueryString["ItemId"] != null)
            {
                ParentItemDataID = Convert.ToInt32(Request.QueryString["ItemId"]);
            }

            if (Request.QueryString["RuleSetTreeTypeCode"] != null)
            {
                CurrentRuleSetTreeTypeCode = Convert.ToString(Request.QueryString["RuleSetTreeTypeCode"]);
                hdnRuleSetTreeTypeCode.Value = CurrentRuleSetTreeTypeCode;

            }
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                BindAssignmentPropertyDetails();
            }
            Presenter.OnViewLoaded();
            if (ViewState["AssignmentPropertyId"] != null)
            {
                fsucCmdBarAssignment.SaveButtonText = "Save";
            }
            else
            {
                fsucCmdBarAssignment.SaveButtonText = "Save";
            }
        }

        protected void fsucCmdBarAssignment_SaveClick(object sender, EventArgs e)
        {
            AssignmentProperty assignmentProperty = new AssignmentProperty();

            if (ViewState["AssignmentPropertyId"] != null)
            {
                assignmentProperty.AssignmentPropertyID = Convert.ToInt32(ViewState["AssignmentPropertyId"]);
            }

            if (CurrentRuleSetTreeTypeCode != RuleSetTreeNodeType.Package)
            {
                foreach (var item in cmbEditableBy.CheckedItems)
                {
                    AssignmentPropertiesEditableBy newitem = new AssignmentPropertiesEditableBy
                    {
                        EditableByID = Convert.ToInt32(item.Value)
                    };
                    assignmentProperty.AssignmentPropertiesEditableBies.Add(newitem);
                }
                //UAT-3806
                //Save Applicant EditableBy for Attribute But it will not to be shown on screen
                //if (CurrentRuleSetTreeTypeCode == RuleSetTreeNodeType.Attribute)
                //{
                //    AssignmentPropertiesEditableBy newApplicantEditableBy = new AssignmentPropertiesEditableBy
                //    {
                //        EditableByID = CurrentViewContext.ApplicantEditableByID
                //    };
                //    assignmentProperty.AssignmentPropertiesEditableBies.Add(newApplicantEditableBy);
                //}
            }

            if (CurrentRuleSetTreeTypeCode == RuleSetTreeNodeType.Attribute)
            {
                assignmentProperty.IsActive = chkIsActiveAttr.Checked;
            }
            else
            {
                //Enabling/disabling Exception submission on category
                if (CurrentRuleSetTreeTypeCode == RuleSetTreeNodeType.Category)
                {
                    if (rdAllowCatException.SelectedIndex != -1)
                    {
                        assignmentProperty.IsExceptionNotAllowed = !Convert.ToBoolean(rdAllowCatException.SelectedValue);
                    }
                }

                //UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
                if (CurrentRuleSetTreeTypeCode == RuleSetTreeNodeType.Item)
                {
                    if (rdbItemDataEntry.SelectedIndex != -1)
                    {
                        assignmentProperty.ItemDataEntry = Convert.ToBoolean(rdbItemDataEntry.SelectedValue);
                    }
                    //UAT-4926
                    if (rdbEnableUpdateAllTime.SelectedIndex != -1)
                    {
                        assignmentProperty.IsEnableUpdateAllTime = Convert.ToBoolean(rdbEnableUpdateAllTime.SelectedValue);
                    }
                }
                assignmentProperty.EffectiveDate = dpkrEffectiveDate.SelectedDate;

                //Set Third Party Reviewer as null when selected value is null  or empty.
                if (cmbThirdPartyReviewer.SelectedValue.IsNullOrEmpty())
                {
                    assignmentProperty.ReviewerTenantID = null;
                }
                else
                {
                    assignmentProperty.ReviewerTenantID = Convert.ToInt32(cmbThirdPartyReviewer.SelectedValue);
                }

                if (rdoApprovalReqd.SelectedIndex != -1)
                {
                    assignmentProperty.ApprovalRequired = Convert.ToBoolean(rdoApprovalReqd.SelectedValue);
                }
                assignmentProperty.IsActive = chkActive.Checked;

                foreach (var item in cmbReviewedBy.CheckedItems)
                {
                    AssignmentPropertiesReviewer newitem = new AssignmentPropertiesReviewer
                    {
                        ReviewerTypeID = Convert.ToInt16(item.Value),
                        //CreatedBy = CurrentLoggedInUserId,
                        //CreatedOn = DateTime.Now
                    };
                    assignmentProperty.AssignmentPropertiesReviewers.Add(newitem);
                }

                // For saving third Party Reviewer User Id
                if (cmbReviewedBy.CheckedItems.Count == 1)
                {
                    if (cmbReviewedBy.CheckedItems[0].Text == "Admin" && cmbThirdPartyReviewer.SelectedValue != String.Empty)
                    {
                        assignmentProperty.TPReviewerUserID = Convert.ToInt32(cmbThirdPartyUser.SelectedValue);
                    }
                }

                // For saving is Admin Data Entry Allowed
                if (rdAllowDataEntry.SelectedIndex != -1)
                {
                    assignmentProperty.IsAdminDataEntryNotAllowed = !Convert.ToBoolean(rdAllowDataEntry.SelectedValue);
                }
            } 

            Presenter.UpdateAssignmentProperties(assignmentProperty);

            if (ViewState["AssignmentPropertyId"] != null)
            {
                base.ShowSuccessMessage("Assignment Properties updated successfully.");
            }
            else
            {
                base.ShowSuccessMessage("Assignment Properties inserted successfully.");
            }
            BindAssignmentPropertyDetails();
        }

        private void BindAssignmentPropertyDetails()
        {
            //Changes for Editable By for ATR
            if (CurrentRuleSetTreeTypeCode == RuleSetTreeNodeType.Attribute)
            {
                ShowHideControls();
                BindEditableBy();
                Presenter.GetAssignmentPropertyDetails();

                if (AssignmentPropertyDetails.IsNotNull())
                {
                    foreach (var editor in AssignmentPropertyDetails.AssignmentPropertiesEditableBies)
                    {
                        if (editor.IsDeleted == false)
                        {
                            //Applicant Editable By should not be shown for Attribute
                            //if (editor.lkpEditableBy.Code != LkpEditableBy.Applicant)
                                cmbEditableBy.FindItemByText(editor.lkpEditableBy.Name).Checked = true;
                        }
                    }
                    ViewState["AssignmentPropertyId"] = AssignmentPropertyDetails.AssignmentPropertyID;
                    chkIsActiveAttr.Checked = AssignmentPropertyDetails.IsActive;
                }
            }
            else
            {
                ShowHideControls();
                BindCombobox();
                Presenter.GetAssignmentPropertyDetails();

                if (AssignmentPropertyDetails != null)
                {
                    dpkrEffectiveDate.SelectedDate = AssignmentPropertyDetails.EffectiveDate;
                    rdoApprovalReqd.SelectedValue = Convert.ToString(AssignmentPropertyDetails.ApprovalRequired);
                    chkActive.Checked = AssignmentPropertyDetails.IsActive;
                    cmbThirdPartyReviewer.SelectedValue = AssignmentPropertyDetails.ReviewerTenantID.ToString();

                    if (cmbThirdPartyReviewer.SelectedValue != String.Empty)
                    {
                        CurrentViewContext.selectedReviewerId = Convert.ToInt32(cmbThirdPartyReviewer.SelectedValue);
                        BindThirdPartyUserCombobox();
                    }

                    //Editable By will not be shown for packages.
                    if (CurrentRuleSetTreeTypeCode != RuleSetTreeNodeType.Package)
                    {
                        foreach (var editor in AssignmentPropertyDetails.AssignmentPropertiesEditableBies)
                        {
                            if (editor.IsDeleted == false)
                            {
                                cmbEditableBy.FindItemByText(editor.lkpEditableBy.Name).Checked = true;
                            }
                        }
                    }

                    foreach (var reviewer in AssignmentPropertyDetails.AssignmentPropertiesReviewers)
                    {
                        if (reviewer.IsDeleted == false)
                        {
                            cmbReviewedBy.FindItemByText(reviewer.lkpReviewerType.Name).Checked = !reviewer.IsDeleted;
                        }
                    }

                    if (AssignmentPropertyDetails.TPReviewerUserID != null)
                    {
                        CurrentViewContext.selectedReviewerId = Convert.ToInt32(cmbThirdPartyReviewer.SelectedValue);
                        BindThirdPartyUserCombobox();
                        cmbThirdPartyUser.SelectedValue = AssignmentPropertyDetails.TPReviewerUserID.ToString();
                    }
					if (CurrentRuleSetTreeTypeCode == RuleSetTreeNodeType.Category)
                    {
                        rdAllowCatException.SelectedValue = Convert.ToString(!AssignmentPropertyDetails.IsExceptionNotAllowed).ToLower();
                    }
                    //UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
                    if (CurrentRuleSetTreeTypeCode == RuleSetTreeNodeType.Item)
                    {
                        rdbItemDataEntry.SelectedValue = Convert.ToString(AssignmentPropertyDetails.ItemDataEntry).ToLower();
                        rdbEnableUpdateAllTime.SelectedValue = Convert.ToString(AssignmentPropertyDetails.IsEnableUpdateAllTime).ToLower(); //UAT-4926
                    }

                    if (CurrentRuleSetTreeTypeCode == RuleSetTreeNodeType.Item || CurrentRuleSetTreeTypeCode == RuleSetTreeNodeType.Category)
                    {
                        rdAllowDataEntry.SelectedValue = Convert.ToString(!AssignmentPropertyDetails.IsAdminDataEntryNotAllowed).ToLower();
                    }

                    ViewState["AssignmentPropertyId"] = AssignmentPropertyDetails.AssignmentPropertyID;
                }
            }
            //fsucCmdBarAssignment.SaveButtonText = "Save";
            if (ViewState["AssignmentPropertyId"] != null)
            {
                fsucCmdBarAssignment.SaveButtonText = "Update";
            }
            else
            {
                fsucCmdBarAssignment.SaveButtonText = "Save";
            }
        }

        /// <summary>
        /// To bind EditableBy ComboBox
        /// </summary>
        private void BindEditableBy()
        {
            Presenter.BindEditableBy();
            cmbEditableBy.DataSource = lstEditableBy;
            cmbEditableBy.DataBind();
        }

        private void BindCombobox()
        {
            //Editable By will not be shown for packages.
            if (CurrentRuleSetTreeTypeCode != RuleSetTreeNodeType.Package)
            {
                Presenter.BindEditableBy();
            }

            Presenter.BindReviwedBy();
            cmbEditableBy.DataSource = lstEditableBy;
            cmbEditableBy.DataBind();
            cmbReviewedBy.DataSource = lstReviewerType;
            cmbReviewedBy.DataBind();

            if (CurrentRuleSetTreeTypeCode == RuleSetTreeNodeType.Package)
            {
                Presenter.BindThirdPartyReviewer();
                cmbThirdPartyReviewer.Visible = true;
                dvLableThirdPartyReviewer.Visible = true;
                cmbThirdPartyReviewer.DataSource = LstThirdPartyReviewer;
                cmbThirdPartyReviewer.DataBind();
                //Sets the default option for Third Party Reviewer.
                cmbThirdPartyReviewer.Items.Insert(0, new RadComboBoxItem("Not Required", String.Empty));
            }
        }

        private void ShowHideControls()
        {
            //Editable By will not be shown for Packages.
            if (CurrentRuleSetTreeTypeCode == RuleSetTreeNodeType.Package)
            {
                divEffectiveDate.Visible = true;
                divReviewedBy.Visible = true;
                divThirdPartyReviewer.Visible = true;
                divEditableBy.Visible = false;
                //divEditableBy.Style["display"] = "none";
            }
            //Only Editable By will be shown for Attributes.
            else if (CurrentRuleSetTreeTypeCode == RuleSetTreeNodeType.Attribute)
            {
                divEffectiveDate.Visible = false;
                divReviewedBy.Visible = false;
                divThirdPartyReviewer.Visible = false;
                divEditableBy.Visible = true;
                //divEditableBy.Style["display"] = "block";
                //[Comments:Show Div for Is Active check for attribute Type result set tree node]
                dvIsActiveAttr.Style["display"] = "inline";
            }
            //All fields/controls will be shown for Categories and Items.
            else
            {
                if (CurrentRuleSetTreeTypeCode == RuleSetTreeNodeType.Category && !Presenter.IsDisabledBothCategoryAndItemExceptionsForTenant())
                {
                    divAllowExcep.Visible = true;
                }
                if (CurrentRuleSetTreeTypeCode == RuleSetTreeNodeType.Item)
                {
                    divItemDataEntry.Visible = true;
                    dvEnableUpdateAllTime.Visible = true; //UAT-4926
                }
                if (CurrentRuleSetTreeTypeCode == RuleSetTreeNodeType.Item || CurrentRuleSetTreeTypeCode == RuleSetTreeNodeType.Category)
                {
                    divAllowDataEntry.Visible = true;
                }
                divEffectiveDate.Visible = true;
                divReviewedBy.Visible = true;
                divThirdPartyReviewer.Visible = true;
                divEditableBy.Visible = true;
                //divEditableBy.Style["display"] = "block";
            }
            hdnIsActiveYesClientIDAttr.Value = chkIsActiveAttr.IsActiveYesClientID;
            hdnIsActiveYesPAKCAT.Value = chkActive.IsActiveYesClientID;
        }

        private void BindThirdPartyUserCombobox()
        {
            Presenter.BindThirdPartyUser();
            cmbThirdPartyUser.ClearSelection();
            cmbThirdPartyUser.DataSource = LstThirdPartyUser;
            cmbThirdPartyUser.DataBind();
            cmbThirdPartyUser.Items.Insert(0, new RadComboBoxItem("--SELECT--", String.Empty));
        }

        protected void cmbThirdPartyReviewer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbThirdPartyReviewer.SelectedValue != String.Empty)
            {
                CurrentViewContext.selectedReviewerId = Convert.ToInt32(cmbThirdPartyReviewer.SelectedValue);
            }
            BindThirdPartyUserCombobox();

        }

        protected void cmbReviewedBy_ItemChecked(object sender, EventArgs e)
        {
            if (cmbReviewedBy.CheckedItems.Count == 1)
            {
                if (cmbReviewedBy.CheckedItems[0].Text == "Admin" && cmbThirdPartyReviewer.SelectedValue != String.Empty)
                {
                    CurrentViewContext.selectedReviewerId = Convert.ToInt32(cmbThirdPartyReviewer.SelectedValue);
                    BindThirdPartyUserCombobox();
                }
            }
        }
    }
}

