using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgSetup;
using Telerik.Web.UI;
using INTSOF.Utils;
using CoreWeb.Shell;

namespace CoreWeb.BkgSetup.Views
{
    public partial class DAndRAttributeGroupMapping : BaseUserControl, IDAndRAttributeGroupMappingView
    {
        #region Variables

        private DAndRAttributeGroupMappingPresenter _presenter = new DAndRAttributeGroupMappingPresenter();
        Int32 SystemDocumentId;
        List<DAndRAttributeGroupMappingContract> lstDAndRAttributeGroupMappingContract = null;
        private String _viewType;

        #endregion

        #region Properties

        public IDAndRAttributeGroupMappingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public DAndRAttributeGroupMappingPresenter Presenter
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

        Int32 IDAndRAttributeGroupMappingView.SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID));
            }
        }

        Int32 IDAndRAttributeGroupMappingView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        #endregion

        #region Events

        #region Page Events


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Set Module Title
                BasePage basePage = base.Page as BasePage;
                if (basePage != null)
                {
                    basePage.SetModuleTitle("Common Operations");
                }
                if (!this.IsPostBack)
                {

                    Presenter.OnViewInitialized();
                    Presenter.OnViewLoaded();

                }
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    CaptureQuerystringParameters(args);
                }
                lblMessage.Visible = false;
                base.SetPageTitle("D&A/Additional Attribute Group Mapping");
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

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "D&A Attribute Group Mapping";
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
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

        #endregion

        #region Grid Events

        protected void grdDAndRAttributeGroupMapping_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            lstDAndRAttributeGroupMappingContract = Presenter.GetDAndRAttributeGroupMapping(SystemDocumentId);
            if (lstDAndRAttributeGroupMappingContract.IsNotNull())
            {
                grdDAndRAttributeGroupMapping.DataSource = lstDAndRAttributeGroupMappingContract;
            }
            else
            {
                grdDAndRAttributeGroupMapping.DataSource = String.Empty;
            }
        }
        protected void grdDAndRAttributeGroupMapping_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 sysDocumentFieldMappingID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ID"));

                WclComboBox cmbAttributeGroup = (e.Item.FindControl("cmbAttributeGroup") as WclComboBox);
                WclComboBox cmbAttribute = (e.Item.FindControl("cmbAttribute") as WclComboBox);
                WclComboBox cmbSpecialFields = gridEditableItem.FindControl("cmbSpecialFields") as WclComboBox;
                RadioButton rbApplicantAttr = gridEditableItem.FindControl("rbApplicantAttr") as RadioButton;
                RadioButton rbSpecialField = gridEditableItem.FindControl("rbSpecialField") as RadioButton;
                RadioButton rbCustomAttribute = gridEditableItem.FindControl("rbCustomAttribute") as RadioButton;
                WclComboBox cmbInstitution = gridEditableItem.FindControl("cmbInstitution") as WclComboBox;
                WclComboBox cmbCustomAttribute = gridEditableItem.FindControl("cmbCustomAttribute") as WclComboBox;

                DAndRAttributeGroupMappingContract dAndRContract = new DAndRAttributeGroupMappingContract();
                dAndRContract.IsApplicantAttribute = rbApplicantAttr.Checked;
                dAndRContract.IsSpecialAttribute = rbSpecialField.Checked;
                dAndRContract.IsCustomAttribute = rbCustomAttribute.Checked;
                dAndRContract.SvcAttGroupID = Convert.ToInt32(cmbAttributeGroup.SelectedValue);
                dAndRContract.SvcAttrID = Convert.ToInt32(cmbAttribute.SelectedValue);
                dAndRContract.SpecialFieldTypeID = Convert.ToInt32(cmbSpecialFields.SelectedValue);
                dAndRContract.TenantID = Convert.ToInt32(cmbInstitution.SelectedValue);
                dAndRContract.CustomAttributeID = Convert.ToInt32(cmbCustomAttribute.SelectedValue);
                dAndRContract.ID = Convert.ToInt32(sysDocumentFieldMappingID);

                //Boolean status = Presenter.UpdateMapping(sysDocumentFieldMappingID
                //                                , Convert.ToInt32(cmbAttributeGroup.SelectedValue)
                //                                , Convert.ToInt32(cmbAttribute.SelectedValue)
                //                                , Convert.ToInt32(cmbSpecialFields.SelectedValue)
                //                                , rbApplicantAttr.Checked);
                Boolean status = Presenter.UpdateMapping(dAndRContract);
                if (status)
                {
                    lblMessage.Visible = true;
                    lblMessage.ShowMessage("Mapping has been updated successfully", MessageType.SuccessMessage);
                }
                else
                {
                    lblMessage.Visible = true;
                    lblMessage.ShowMessage("Some error has occured", MessageType.SuccessMessage);
                }
            }
        }

        protected void grdDAndRAttributeGroupMapping_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                WclComboBox cmbAttributeGroup = gridEditableItem.FindControl("cmbAttributeGroup") as WclComboBox;
                WclComboBox cmbAttribute = gridEditableItem.FindControl("cmbAttribute") as WclComboBox;
                WclComboBox cmbSpecialFields = gridEditableItem.FindControl("cmbSpecialFields") as WclComboBox;
                WclComboBox cmbInstitution = gridEditableItem.FindControl("cmbInstitution") as WclComboBox;
                WclComboBox cmbCustomAttribute = gridEditableItem.FindControl("cmbCustomAttribute") as WclComboBox;
                RadioButton rbSpecialField = gridEditableItem.FindControl("rbSpecialField") as RadioButton;
                RadioButton rbCustomAttribute = gridEditableItem.FindControl("rbCustomAttribute") as RadioButton;
                RadioButton rbApplicantAttr = gridEditableItem.FindControl("rbApplicantAttr") as RadioButton;
                Int32 AttributeGroupMappingID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("AttributeGroupMappingID"));
                Int32 ID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ID"));
                List<Entity.BkgSvcAttributeGroup> lstBkgSvcAttributeGroup = Presenter.GetServiceAttributeGroup();

                //For Special Fields In D and R Documents
                String specialFieldName = Convert.ToString(gridEditableItem.GetDataKeyValue("SpecialFieldTypeName"));
                List<Entity.lkpDisclosureDocumentSpecialFieldType> lstSpecialFieldType = Presenter.GetSpecialFieldType();
                List<Entity.Tenant> lstTenants = Presenter.GetTenants();
                if (lstSpecialFieldType.IsNotNull())
                {
                    cmbSpecialFields.DataSource = lstSpecialFieldType;
                    cmbSpecialFields.DataBind();
                    cmbSpecialFields.AddFirstEmptyItem();
                    String spclType = lstDAndRAttributeGroupMappingContract.Where(cond => cond.ID == ID).Select(col => col.SpecialFieldTypeID).FirstOrDefault().ToString();
                    if (!spclType.IsNullOrEmpty())
                    {
                        cmbSpecialFields.SelectedValue = spclType;
                        rbSpecialField.Checked = true;
                        rbApplicantAttr.Checked = false;
                        rbCustomAttribute.Checked = false;
                    }
                }
                else
                {
                    cmbSpecialFields.DataSource = new Entity.lkpDisclosureDocumentSpecialFieldType();
                    cmbSpecialFields.DataBind();
                }

                if (lstTenants.IsNotNull())
                {
                    cmbInstitution.DataSource = lstTenants;
                    cmbInstitution.DataBind();
                    cmbInstitution.AddFirstEmptyItem();
                    Int32 tenantID = lstDAndRAttributeGroupMappingContract.Where(cond => cond.ID == ID).Select(col => col.TenantID).FirstOrDefault();
                    if (tenantID > AppConsts.NONE)
                    {
                        cmbInstitution.SelectedValue = tenantID.ToString();
                        rbCustomAttribute.Checked = true;
                        rbApplicantAttr.Checked = false;
                        rbSpecialField.Checked = false;
                    }
                }
                else
                {
                    cmbInstitution.DataSource = new List<Entity.Tenant>();
                    cmbInstitution.DataBind();
                    cmbCustomAttribute.DataSource = new List<CustomAttribute>();
                    cmbCustomAttribute.DataBind();
                }


                if (lstBkgSvcAttributeGroup.IsNotNull())
                {
                    cmbAttributeGroup.DataSource = lstBkgSvcAttributeGroup;
                    cmbAttributeGroup.DataBind();
                    cmbAttributeGroup.AddFirstEmptyItem();
                    cmbAttributeGroup.SelectedValue = lstDAndRAttributeGroupMappingContract.Where(cond => cond.ID == ID).Select(col => col.SvcAttGroupID).FirstOrDefault().ToString();

                }
                else
                {
                    cmbAttributeGroup.DataSource = new Entity.BkgSvcAttributeGroup();
                    cmbAttributeGroup.DataBind();

                }
                if (lstDAndRAttributeGroupMappingContract.Select(col => col.SvcAttGroupID).FirstOrDefault().IsNotNull())
                {
                    List<Entity.BkgSvcAttribute> lstBkgSvcAttribute = Presenter.GetServiceAttributeByServiceGroupID(Convert.ToInt32(cmbAttributeGroup.SelectedValue));
                    cmbAttribute.DataSource = lstBkgSvcAttribute;
                    cmbAttribute.DataBind();
                    cmbAttribute.AddFirstEmptyItem();
                    cmbAttribute.SelectedValue = lstDAndRAttributeGroupMappingContract.Where(cond => cond.ID == ID).Select(col => col.SvcAttrID).FirstOrDefault().ToString();

                    List<CustomAttribute> lstCustomAttribute = Presenter.GetCustomAttributesByTenantID(Convert.ToInt32(cmbInstitution.SelectedValue));
                    if (!lstCustomAttribute.IsNullOrEmpty())
                    {
                        lstCustomAttribute.RemoveAll(x => x.lkpCustomAttributeDataType.Code.ToLower().Trim() == CustomAttributeDatatype.User_Group.GetStringValue().ToLower().Trim());
                    }
                    cmbCustomAttribute.DataSource = lstCustomAttribute;
                    cmbCustomAttribute.DataBind();
                    cmbCustomAttribute.AddFirstEmptyItem();
                    Int32 customAttributeID = lstDAndRAttributeGroupMappingContract.Where(cond => cond.ID == ID).Select(col => col.CustomAttributeID).FirstOrDefault();
                    cmbCustomAttribute.SelectedValue = customAttributeID.ToString();
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "Enable();", true);
            }
        }


        #endregion

        #region Button Events
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    
                                                                    { "Child", ChildControls.DisclosureDocument}
                                                                 
                                                                   
                                                                 };
                string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
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
        #endregion

        #region ComboBox Events

        protected void cmbAttributeGroup_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            WclComboBox cmbAttributeGroup = sender as WclComboBox;
            WclComboBox cmbAttribute = cmbAttributeGroup.Parent.NamingContainer.FindControl("cmbAttribute") as WclComboBox;
            if (cmbAttribute.IsNotNull())
            {
                List<Entity.BkgSvcAttribute> lstBkgSvcAttribute = Presenter.GetServiceAttributeByServiceGroupID(Convert.ToInt32(cmbAttributeGroup.SelectedValue));
                cmbAttribute.DataSource = lstBkgSvcAttribute;
                cmbAttribute.DataBind();
            }
            else
            {
                cmbAttribute.DataSource = new Entity.BkgSvcAttribute();
                cmbAttribute.DataBind();
            }
            cmbAttribute.AddFirstEmptyItem();
        }

        protected void cmbInstitution_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            WclComboBox cmbInstitution = sender as WclComboBox;
            WclComboBox cmbCustomAttribute = cmbInstitution.Parent.NamingContainer.FindControl("cmbCustomAttribute") as WclComboBox;
            if (cmbCustomAttribute.IsNotNull())
            {
                List<CustomAttribute> lstCustomAttribute = Presenter.GetCustomAttributesByTenantID(Convert.ToInt32(cmbInstitution.SelectedValue));
                if (!lstCustomAttribute.IsNullOrEmpty())
                {
                    lstCustomAttribute.RemoveAll(x => x.lkpCustomAttributeDataType.Code.ToLower().Trim() == CustomAttributeDatatype.User_Group.GetStringValue().ToLower().Trim());
                }
                cmbCustomAttribute.DataSource = lstCustomAttribute;
                cmbCustomAttribute.DataBind();
            }
            else
            {
                cmbCustomAttribute.DataSource = new List<CustomAttribute>();
                cmbCustomAttribute.DataBind();
            }
            cmbCustomAttribute.AddFirstEmptyItem();
        }

        #endregion

        #endregion

        #region Methods

        private void CaptureQuerystringParameters(Dictionary<String, String> args)
        {
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.ContainsKey("SystemDocumentID"))
            {
                SystemDocumentId = Convert.ToInt32(args["SystemDocumentID"]);
            }
        }



        #endregion

        #region Apply Permissions

        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();

                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Edit",
                    CustomActionLabel = "Edit",
                    ScreenName = "D&A Attribute Group Mapping"
                });

                return actionCollection;
            }
        }

        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
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
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    grdDAndRAttributeGroupMapping.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                }
                                break;
                            }
                    }
                });
            }
        }

        #endregion
    }
}