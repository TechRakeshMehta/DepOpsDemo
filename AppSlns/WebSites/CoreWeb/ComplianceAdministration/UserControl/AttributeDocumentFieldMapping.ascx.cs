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

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class AttributeDocumentFieldMapping : BaseUserControl, IAttributeDocumentFieldMappingView
    {
        #region Variables

        private AttributeDocumentFieldMappingPresenter _presenter = new AttributeDocumentFieldMappingPresenter();
        private String _viewType;

        #endregion

        #region Properties

        public IAttributeDocumentFieldMappingView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public AttributeDocumentFieldMappingPresenter Presenter
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

        Int32 IAttributeDocumentFieldMappingView.SelectedTenantId
        {
            get;
            set;
        }

        Int32 IAttributeDocumentFieldMappingView.SystemDocumentId
        {
            get;
            set;
        }

        List<DocumentFieldMapping> IAttributeDocumentFieldMappingView.lstDocumentFieldMapping
        {
            get
            {
                return (List<DocumentFieldMapping>)ViewState["lstDocumentFieldMapping"];
            }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    value = new List<DocumentFieldMapping>();
                }
                ViewState["lstDocumentFieldMapping"] = value;
            }
        }

        Int32 IAttributeDocumentFieldMappingView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public List<lkpDocumentFieldType_> lstDocumentFieldType
        {
            get;
            set;
        }

        #endregion

        #region Events

        #region Page Events


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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
                base.SetPageTitle("Attribute Document Field Mapping");
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
                base.Title = "Attribute Document Field Mapping";
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

        protected void grdAttributeFieldMapping_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetDocumentFieldMapping();
                if (CurrentViewContext.lstDocumentFieldMapping.IsNotNull())
                {
                    grdAttributeFieldMapping.DataSource = CurrentViewContext.lstDocumentFieldMapping;
                }
                else
                {
                    grdAttributeFieldMapping.DataSource = new List<DocumentFieldMapping>();
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

        protected void grdAttributeFieldMapping_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                    WclComboBox cmbFieldType = gridEditableItem.FindControl("cmbFieldType") as WclComboBox;
                    Presenter.GetDocumentFieldTypes();
                    cmbFieldType.DataSource = CurrentViewContext.lstDocumentFieldType;
                    cmbFieldType.DataBind();
                    Int32 DFM_ID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("DFM_ID"));
                    Int32? fieldTypeID = CurrentViewContext.lstDocumentFieldMapping.Where(x => x.DFM_ID == DFM_ID).Select(col => col.DFM_DocumentFieldTypeID).FirstOrDefault();
                    if (fieldTypeID.IsNotNull())
                    {
                        cmbFieldType.SelectedValue = Convert.ToString(fieldTypeID);
                    }
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

        protected void grdAttributeFieldMapping_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 documentFieldMappingID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("DFM_ID"));
                WclComboBox cmbFieldType = (e.Item.FindControl("cmbFieldType") as WclComboBox);
                Boolean status = Presenter.UpdateDocumentFieldMapping(documentFieldMappingID, Convert.ToInt32(cmbFieldType.SelectedValue));
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

        #region Button Events
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "Child", ChildControls.ManageAttributeDocument},
                                                                    { "TenantID",CurrentViewContext.SelectedTenantId.ToString()}
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



        #endregion

        #region Methods

        private void CaptureQuerystringParameters(Dictionary<String, String> args)
        {
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.ContainsKey("CSD_ID"))
            {
                CurrentViewContext.SystemDocumentId = Convert.ToInt32(args["CSD_ID"]);
            }
            if (args.ContainsKey("TenantID"))
            {
                CurrentViewContext.SelectedTenantId = Convert.ToInt32(args["TenantID"]);
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
                    ScreenName = "Attribute Document Field Mapping"
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
                                    grdAttributeFieldMapping.MasterTableView.GetColumn("EditCommandColumn").Display = false;
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