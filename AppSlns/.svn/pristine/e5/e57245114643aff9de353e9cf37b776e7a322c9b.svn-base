using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreWeb.ComplianceAdministration.Views;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceAdministration.UserControl.Views
{
    public partial class Permissions : BaseUserControl, IPermissionView
    {
        private PermissionPresenter _presenter = new PermissionPresenter();



        public PermissionPresenter Presenter
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

        public IPermissionView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Boolean IPermissionView.IsIncludeAnotherHierarchyPermissionType { get; set; }

        List<vwHierarchyPermission> IPermissionView.BackgroundHierarchyPermissionList
        {
            get;
            set;
        }

        Int16? IPermissionView.ProfilePermissionId { get; set; }
        Int16? IPermissionView.VerificationPermissionId { get; set; }
        Int16? IPermissionView.OrderPermissionId { get; set; }
        Int16? IPermissionView.PackagePermissionID { get; set; } //UAT - 2834       
        List<lkpPermission> IPermissionView.UserPacakgePermissionList { get; set; } //UAT - 2834
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                Presenter.OnViewInitialized();
                CurrentViewContext.DeptProgramMappingID = Convert.ToInt32(Request.QueryString["Id"]);
                CurrentViewContext.PermissionCode = Request.QueryString["PermissionCode"];
            }
        }

        #region Map Hierarchy Permission

        protected void grdUsrPermission_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetHierarchyPermission();
                //grdUsrPermission.DataSource = CurrentViewContext.HierarchyPermissionList;
                grdUsrPermission.DataSource = CurrentViewContext.BackgroundHierarchyPermissionList;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdUsrPermission_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.OrganizationUserID = Convert.ToInt32(((e.Item.FindControl("ddlHierPerUser") as WclComboBox).SelectedValue));
                CurrentViewContext.PermissionId = Convert.ToInt16(((e.Item.FindControl("rblPermissions") as RadioButtonList).SelectedValue));
                CurrentViewContext.IsIncludeAnotherHierarchyPermissionType = ((e.Item.FindControl("chkApplyOnBoth") as RadButton).Checked);
                if (CurrentViewContext.IsIncludeAnotherHierarchyPermissionType)
                {
                    CurrentViewContext.ProfilePermissionId = Convert.ToInt16(((e.Item.FindControl("rblProfilePermission") as RadioButtonList).SelectedValue));
                    CurrentViewContext.VerificationPermissionId = Convert.ToInt16(((e.Item.FindControl("rblVerificationPermission") as RadioButtonList).SelectedValue));
                    CurrentViewContext.OrderPermissionId = Convert.ToInt16(((e.Item.FindControl("rblOrderPermission") as RadioButtonList).SelectedValue));
                    CurrentViewContext.PackagePermissionID = Convert.ToInt16(((e.Item.FindControl("rblPackagePermission") as RadioButtonList).SelectedValue)); // UAT 2834
                }

                Presenter.SaveHierarchyPermission();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdUsrPermission_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.PermissionId = Convert.ToInt16(((e.Item.FindControl("rblPermissions") as RadioButtonList).SelectedValue));
                CurrentViewContext.HierarchyPermissionID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyPermissionID"]);
                CurrentViewContext.IsIncludeAnotherHierarchyPermissionType = ((e.Item.FindControl("chkApplyOnBoth") as RadButton).Checked);
                Presenter.UpdateHierarchyPermission();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }

            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }

        }

        protected void grdUsrPermission_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.HierarchyPermissionID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyPermissionID"]);
                Presenter.DeleteHierarchyPermission();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!String.IsNullOrEmpty(CurrentViewContext.SuccessMessage))
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdUsrPermission_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem gridEditFormItem = (GridEditFormItem)e.Item;
                    WclComboBox ddlHierPerUser = (WclComboBox)gridEditFormItem.FindControl("ddlHierPerUser");
                    RadButton chkApplyOnBoth = (RadButton)gridEditFormItem.FindControl("chkApplyOnBoth");
                    HtmlGenericControl divOrderPermission = (HtmlGenericControl)gridEditFormItem.FindControl("divOrderPermission");
                    HtmlGenericControl divOtherPermissions = (HtmlGenericControl)gridEditFormItem.FindControl("divOtherPermissions");
                    HtmlGenericControl dvPackagePermission = (HtmlGenericControl)gridEditFormItem.FindControl("dvPackagePermission");
                    String complianceHierarchyPermissionType = HierarchyPermissionTypes.COMPLIANCE.GetStringValue();

                    Presenter.GetOrganizationUserList();

                    if (CurrentViewContext.OrganizationUserList.IsNotNull())
                    {
                        //if (CurrentViewContext.OrganizationUserList.Count > SysXClientConsts.Zero)
                        //{
                        Entity.OrganizationUser organizationUser = new Entity.OrganizationUser
                        {
                            OrganizationUserID = AppConsts.NONE,
                            FirstName = AppConsts.COMBOBOX_ITEM_SELECT
                        };

                        CurrentViewContext.OrganizationUserList.Insert(AppConsts.NONE, organizationUser);
                        //}

                        ddlHierPerUser.DataSource = CurrentViewContext.OrganizationUserList;
                        ddlHierPerUser.DataBind();
                    }

                    RadioButtonList rblPermissions = (RadioButtonList)gridEditFormItem.FindControl("rblPermissions");
                    Presenter.GetPermissionList(false);
                    if (CurrentViewContext.UserPermissionList.IsNotNull())
                    {
                        rblPermissions.DataSource = CurrentViewContext.UserPermissionList;
                        rblPermissions.DataBind();
                    }
                    if (e.Item is GridEditFormInsertItem)
                    {
                        ShowCompliancePermissionControls(chkApplyOnBoth, divOrderPermission, divOtherPermissions, dvPackagePermission);
                    }
                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        vwHierarchyPermission hierarchyPermission = (vwHierarchyPermission)e.Item.DataItem;
                        if (!hierarchyPermission.IsNull())
                        {
                            if (CurrentViewContext.OrganizationUserList.IsNotNull())
                            {
                                WclTextBox txtHierUser = (WclTextBox)gridEditFormItem.FindControl("txtHierUser");
                                txtHierUser.Text = Convert.ToString(hierarchyPermission.UserFirstName + " " + hierarchyPermission.UserLastName);
                                txtHierUser.Enabled = false;
                                txtHierUser.Visible = true;
                                ddlHierPerUser.Visible = false;

                                Label lblUserName = (Label)gridEditFormItem.FindControl("lblUserName");
                                //Label lblUserName = (Label)gridEditFormItem.FindControl("lblUserName");

                                lblUserName.Text = "User";

                                //HtmlGenericControl spnUserNameReq = (HtmlGenericControl)gridEditFormItem.FindControl("spnUserNameReq");
                                //spnUserNameReq.Visible = false;
                                ////lblUserName.Visible = false;
                            }
                            if (CurrentViewContext.UserPermissionList.IsNotNull())
                            {
                                rblPermissions.SelectedValue = Convert.ToString(hierarchyPermission.PermissionID);
                            }
                            //if (!CurrentViewContext.HierarchyPermissionList.Any(cond => cond.HierarchyPermissionTypeCode.Equals(complianceHierarchyPermissionType)
                            //                          && cond.OrganizationUserID == hierarchyPermission.OrganizationUserID))
                            //{
                            //    chkApplyOnBoth.Enabled = true;
                            //    chkApplyOnBoth.Checked = false;
                            //}
                            //else
                            //{
                            //    chkApplyOnBoth.Enabled = false;
                            //    chkApplyOnBoth.Checked = false;
                            //}
                            chkApplyOnBoth.Enabled = false;
                            chkApplyOnBoth.Checked = false;
                            divOrderPermission.Visible = false;
                            divOtherPermissions.Visible = false;
                            dvPackagePermission.Visible = false;
                        }
                    }
                    //View.lstArchiveState = ComplianceDataManager.GetArchiveStateList(View.SelectedTenantId);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region PROPERTIES

        public List<lkpPermission> UserPermissionList
        {
            get;
            set;
        }

        public short PermissionId
        {
            get;
            set;
        }

        public int HierarchyPermissionID
        {
            get;
            set;
        }

        public List<vwHierarchyPermission> HierarchyPermissionList
        {
            get;
            set;
        }

        public string PermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["PermissionCode"]);
            }
            set
            {
                ViewState["PermissionCode"] = value;
            }
        }

        public int OrganizationUserID
        {
            get;
            set;
        }

        public List<Entity.OrganizationUser> OrganizationUserList
        {
            get;
            set;
        }

        public string ErrorMessage
        {
            get;
            set;
        }

        public string SuccessMessage
        {
            get;
            set;
        }

        public string InfoMessage
        {
            get;
            set;
        }

        public string PageType
        {
            get
            {
                return Convert.ToString(ViewState["PageType"]);
            }
            set
            {
                ViewState["PageType"] = value;
            }
        }

        public int CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public int TenantId
        {
            get
            {
                return (Int32)(ViewState["TenantId"]);
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        public int DefaultTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["DefaultTenantId"]);
            }
            set
            {
                ViewState["DefaultTenantId"] = value;
            }
        }

        public int DeptProgramMappingID
        {
            get
            {
                return (Int32)(ViewState["DepProgramMappingID"]);
            }
            set
            {
                ViewState["DepProgramMappingID"] = value;
            }
        }
        #endregion

        protected void ddlHierPerUser_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                Presenter.GetHierarchyPermission();
                String complianceHierarchyPermissionType = HierarchyPermissionTypes.COMPLIANCE.GetStringValue();
                WclComboBox cmbHierPerUser = sender as WclComboBox;
                Int32 selectedOrgUserID = Convert.ToInt32(cmbHierPerUser.SelectedValue);
                RadButton chkApplyOnBoth = cmbHierPerUser.Parent.NamingContainer.FindControl("chkApplyOnBoth") as RadButton;
                HtmlGenericControl divOrderPermission = cmbHierPerUser.Parent.NamingContainer.FindControl("divOrderPermission") as HtmlGenericControl;
                HtmlGenericControl divOtherPermissions = cmbHierPerUser.Parent.NamingContainer.FindControl("divOtherPermissions") as HtmlGenericControl;
                HtmlGenericControl dvPackagePermission = cmbHierPerUser.Parent.NamingContainer.FindControl("dvPackagePermission") as HtmlGenericControl; // UAT 2834
                if (CurrentViewContext.HierarchyPermissionList.IsNullOrEmpty()
                   || !CurrentViewContext.HierarchyPermissionList.Any(cond => cond.HierarchyPermissionTypeCode.Equals(complianceHierarchyPermissionType)
                                                     && cond.OrganizationUserID == selectedOrgUserID))
                {
                    chkApplyOnBoth.Enabled = true;
                    chkApplyOnBoth.Checked = true;
                    ShowCompliancePermissionControls(chkApplyOnBoth, divOrderPermission, divOtherPermissions, dvPackagePermission);
                }
                else
                {
                    chkApplyOnBoth.Enabled = false;
                    chkApplyOnBoth.Checked = false;
                    divOtherPermissions.Visible = false;
                    divOrderPermission.Visible = false;
                    dvPackagePermission.Visible = false;
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }

        }

        protected void chkApplyOnBoth_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadButton chkApplyOnBoth = sender as RadButton;
                HtmlGenericControl divOrderPermission = chkApplyOnBoth.Parent.NamingContainer.FindControl("divOrderPermission") as HtmlGenericControl;
                HtmlGenericControl divOtherPermissions = chkApplyOnBoth.Parent.NamingContainer.FindControl("divOtherPermissions") as HtmlGenericControl;
                HtmlGenericControl dvPackagePermissions = chkApplyOnBoth.Parent.NamingContainer.FindControl("dvPackagePermission") as HtmlGenericControl;
                if (chkApplyOnBoth.Checked)
                {
                    ShowCompliancePermissionControls(chkApplyOnBoth, divOrderPermission, divOtherPermissions, dvPackagePermissions);
                }
                else
                {
                    divOtherPermissions.Visible = false;
                    divOrderPermission.Visible = false;
                    dvPackagePermissions.Visible = false;
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        private void ShowCompliancePermissionControls(RadButton chkApplyOnBoth, HtmlGenericControl divOrderPermission, HtmlGenericControl divOtherPermissions, HtmlGenericControl dvPackagePermissions)
        {
            divOtherPermissions.Visible = true;
            divOrderPermission.Visible = true;
            dvPackagePermissions.Visible = true;
            Presenter.GetPermissionList(false);
            Presenter.GetPermissionList(true);
            if (CurrentViewContext.UserPermissionList.IsNotNull())
            {
                RadioButtonList rblVerificationPermission = chkApplyOnBoth.Parent.NamingContainer.FindControl("rblVerificationPermission") as RadioButtonList;
                RadioButtonList rblProfilePermission = chkApplyOnBoth.Parent.NamingContainer.FindControl("rblProfilePermission") as RadioButtonList;
                RadioButtonList rblOrderPermission = chkApplyOnBoth.Parent.NamingContainer.FindControl("rblOrderPermission") as RadioButtonList;
                RadioButtonList rblPackagePermission = chkApplyOnBoth.Parent.NamingContainer.FindControl("rblPackagePermission") as RadioButtonList; //UAT 2834
                String noAccessPermissionCode = LkpPermission.NoAccess.GetStringValue();
                String fullAccessPermissionCode = LkpPermission.FullAccess.GetStringValue();
                List<lkpPermission> allPermissionsExceptNoAccess = CurrentViewContext.UserPermissionList.Where(cond => !cond.PER_Code.Equals(noAccessPermissionCode)).ToList();
                String fullAccessPermissionValue = Convert.ToString(CurrentViewContext.UserPermissionList.Where(cond => cond.PER_Code.Equals(fullAccessPermissionCode)).Select(col => col.PER_ID).FirstOrDefault());
                if (rblVerificationPermission.IsNotNull())
                {
                    rblVerificationPermission.DataSource = allPermissionsExceptNoAccess;
                    rblVerificationPermission.DataBind();
                    rblVerificationPermission.SelectedValue = fullAccessPermissionValue;
                }
                if (rblProfilePermission.IsNotNull())
                {
                    rblProfilePermission.DataSource = allPermissionsExceptNoAccess;
                    rblProfilePermission.DataBind();
                    rblProfilePermission.SelectedValue = fullAccessPermissionValue;
                }

                if (rblOrderPermission.IsNotNull())
                {
                    rblOrderPermission.DataSource = CurrentViewContext.UserPermissionList;
                    rblOrderPermission.DataBind();
                    rblOrderPermission.SelectedValue = fullAccessPermissionValue;
                }
                if (rblPackagePermission.IsNotNull())
                {
                    String BothPackagePermission = LkpPermission.BothPackagePermission.GetStringValue();
                    String BothPackagePermissionValue = Convert.ToString(CurrentViewContext.UserPacakgePermissionList.Where(cond => cond.PER_Code.Equals(BothPackagePermission)).Select(col => col.PER_ID).FirstOrDefault());
                    rblPackagePermission.DataSource = CurrentViewContext.UserPacakgePermissionList;
                    rblPackagePermission.DataBind();
                    rblPackagePermission.SelectedValue = BothPackagePermissionValue;
                }
            }
        }
    }
}