using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.Shell.Views;
using Entity.ClientEntity;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.Views
{
    public partial class SetupPackagesForInsitiute : BaseWebPage, ISetupPackagesForInsitiuteView
    {
        #region Variables

        #region Private variables
        private SetupPackagesForInsitiutePresenter _presenter = new SetupPackagesForInsitiutePresenter();
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public SetupPackagesForInsitiutePresenter Presenter
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

        public String ErrorMessage
        {
            get;
            set;
        }

        public Int32 tenantId
        {
            get;
            set;
        }

        public List<BackgroundPackage> lstPackages
        {
            get;
            set;
        }

        public Int32 NotesPositionId
        {
            get;
            set;
        }

        #region UAT-3771
        public String Passcode
        {
            get
            {
                return txtPasscode.Text.Trim();
            }
            set
            {
                txtPasscode.Text = value;
            }
        }
        #endregion
        #endregion



        #region Private Properties

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.QueryString["tenantId"].IsNullOrEmpty())
            {
                Int32 TenantId = 0;
                if (Int32.TryParse(Request.QueryString["tenantId"], out TenantId))
                    tenantId = TenantId;
                // tenantId = Convert.ToInt32(Request.QueryString["tenantId"]);
            }
            if (!this.IsPostBack)
            {
                ApplyActionLevelPermission(ActionCollection, "Manage Package Service SetUp");
            }
        }

        #endregion

        #region Grid Events

        protected void grdPackage_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (tenantId > 0)
            {
                Presenter.GetPackageData();
                grdPackage.DataSource = lstPackages;
                //grdPackage.DataBind();
            }
        }


        protected void grdPackage_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Delete"))
            {
                Int32 packageID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BPA_ID"].ToString());
                //Presenter.DeletePackageMapping(packageID);
                if (Presenter.DeletePackageMapping(packageID))
                {
                    base.ShowSuccessMessage("Package mapping deleted successfully.");
                    divAddForm.Visible = false;
                    //ResetControls();
                    grdPackage.Rebind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "parent.UpdatePackageInDropDown('" + Convert.ToString(packageID) + "','" + "" + "','" + "true" + "');", true);
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                }
                else
                {
                    base.ShowInfoMessage(ErrorMessage);
                    divAddForm.Visible = false;
                }

            }
        }
        #endregion

        #region Button Event
        /// <summary>
        /// Add button Click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            divAddForm.Visible = true;
            ResetControls();
        }

        protected void fsucCmdBarPackage_CancelClick(object sender, EventArgs e)
        {
            divAddForm.Visible = false;
            ResetControls();
        }

        /// <summary>
        /// Save button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarPackage_SaveClick(object sender, EventArgs e)
        {
            BackgroundPackage backgroundPackage = new BackgroundPackage();
            backgroundPackage.BPA_Name = txtPackageName.Text;
            backgroundPackage.BPA_Description = txtPkgDescription.Text;
            backgroundPackage.BPA_IsActive = (chkActive as IsActiveToggle).Checked;
            backgroundPackage.BPA_IsViewDetailsInOrderEnabled = chkViewdetails.Checked;
            backgroundPackage.BPA_IsDeleted = false;
            backgroundPackage.BPA_Archived = 0;
            backgroundPackage.BPA_CreatedById = 1;
            backgroundPackage.BPA_CreatedDate = DateTime.Now;
            backgroundPackage.BPA_PackageDetail = rdEditorPackageDetail.Content;
            backgroundPackage.BPA_Label = txtPkgLabel.Text;
            Presenter.GetPackageNotesPosition(rbtnDisplayPosition.SelectedValue);
            backgroundPackage.BPA_NotesDisplayPositionId = NotesPositionId;
            //UAT-2194Invite only packages
            backgroundPackage.BPA_IsInviteOnlyPackage = Convert.ToBoolean(rdbInviteOnlyPackage.SelectedValue);
            backgroundPackage.BPA_IsAvailableForApplicantOrder = Convert.ToBoolean(rblAvalblForApplicant.SelectedValue);
            backgroundPackage.BPA_IsAvailableForAdminOrder = Convert.ToBoolean(rblAvalblForClientAdmin.SelectedValue);
            //UAT-3268
            backgroundPackage.BPA_IsReqToQualifyInRotation = Convert.ToBoolean(rblIsReqToQualifyInRotation.SelectedValue);
            backgroundPackage.BPA_BkgPackageTypeId = (cmbBkgPackageType.SelectedValue) != "0" ? Convert.ToInt32(cmbBkgPackageType.SelectedValue)  : (int?)(null); //UAT-3525
            backgroundPackage.BPA_Passcode = txtPasscode.Text;


            if (rdEditorPackageDetail.Content.Length <= 5000)
            {
                Presenter.SavePackagedetail(backgroundPackage);
            }
            else
            {
                ErrorMessage = "Some error has occurred. Please try again.";
            }
            if (String.IsNullOrEmpty(ErrorMessage))
            {
                String packageName = backgroundPackage.BPA_Name;
                base.ShowSuccessMessage("Package saved successfully.");
                divAddForm.Visible = false;
                ResetControls();
                grdPackage.Rebind();
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "parent.AddNewPackageInDropDown('" + Convert.ToString(backgroundPackage.BPA_ID) + "','" + packageName + "');", true);
                //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
            }
            else
            {
                base.ShowInfoMessage(ErrorMessage);
                divAddForm.Visible = true;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// To reset Controls
        /// </summary>
        private void ResetControls()
        {
            txtPackageName.Text = String.Empty;
            txtPkgLabel.Text = String.Empty;
            txtPkgDescription.Text = String.Empty;
            (chkActive as IsActiveToggle).Checked = true;
            chkViewdetails.Checked = true;
            rbtnDisplayPosition.SelectedValue = PkgNotesDisplayPosition.DISPLAY_BELOW.GetStringValue();
            cmbBkgPackageType.DataSource = Presenter.GetBkgPackageType();  //UAT-3525
            cmbBkgPackageType.DataBind();  //UAT-3525
        }


        #endregion

        #region Public Methods



        #endregion

        #endregion

        #region Apply Permissions


        protected override void ApplyActionLevelPermission(List<ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            List<Entity.FeatureRoleAction> permission = base.ActionPermission;
            if (permission.IsNotNull())
            {
                permission.ForEach(x =>
                {
                    switch (x.PermissionID)
                    {
                        case AppConsts.ONE:
                            {
                                break;
                            }
                        case AppConsts.THREE:
                            {
                                if (x.FeatureAction.CustomActionId == "Add Package")
                                {
                                    btnAdd.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete Package")
                                {
                                    grdPackage.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                break;
                            }
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "Add Package")
                                {
                                    btnAdd.Visible = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete Package")
                                {
                                    grdPackage.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                break;
                            }
                    }

                });
            }
        }

        #endregion

        protected void cmbBkgPackageType_DataBound(object sender, EventArgs e)
        {
            cmbBkgPackageType.Items.Insert(0, new RadComboBoxItem("Default",AppConsts.ZERO));
        }

    }
}