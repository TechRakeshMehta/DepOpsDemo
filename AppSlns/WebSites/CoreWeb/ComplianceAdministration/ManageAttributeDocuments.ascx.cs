using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using CuteWebUI;
using System.IO;
using System.Web.Configuration;
using INTSOF.Utils;
using INTERSOFT.WEB.UI.WebControls;
using Telerik.Web.UI;
using Entity.ClientEntity;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ManageAttributeDocuments : BaseUserControl, IManageAttributeDocumentsView
    {
        #region Variables

        #region Private Variables

        private ManageAttributeDocumentsPresenter _presenter = new ManageAttributeDocumentsPresenter();
        private String _viewType;
        private Int32 _tenantId;

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        public IManageAttributeDocumentsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        String IManageAttributeDocumentsView.ErrorMessage
        {
            get;
            set;
        }

        List<ClientSystemDocument> IManageAttributeDocumentsView.ComplianceViewDocuments
        {
            get;
            set;
        }

        Int32 IManageAttributeDocumentsView.SystemDocumentID
        {
            get;
            set;
        }

        ClientSystemDocument IManageAttributeDocumentsView.ComplianceViewDocumentToUpdate
        {
            get;
            set;
        }

        Int32 IManageAttributeDocumentsView.TenantID
        {
            get
            {
                if (!ViewState["TenantID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["TenantID"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                hdnfTenantId.Value = Convert.ToString(value);
                ViewState["TenantID"] = value;
            }
        }

        Boolean IManageAttributeDocumentsView.IsAdminLoggedIn
        {
            get;
            set;
        }

        Int32 IManageAttributeDocumentsView.ClientTenantID
        {
            get
            {
                if (_tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }

        List<Tenant> IManageAttributeDocumentsView.lstTenant
        {
            get;
            set;
        }

        public ManageAttributeDocumentsPresenter Presenter
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

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Manage Attribute Documents";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                BindControls();
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    CaptureQuerystringParameters(args);
                }
            }

            Presenter.OnViewLoaded();
            ucUploadDocuments.OnCompletedUpload -= new UploadAttributeDocuments.UploadDelegate(ucUploadDocuments_OnCompletedUpload);
            ucUploadDocuments.OnCompletedUpload += new UploadAttributeDocuments.UploadDelegate(ucUploadDocuments_OnCompletedUpload);
            base.SetPageTitle("Manage Attribute Documents");


        }

        private void CaptureQuerystringParameters(Dictionary<String, String> args)
        {
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.ContainsKey("TenantID"))
            {
                CurrentViewContext.TenantID = Convert.ToInt32(args["TenantID"]);
                ddlTenant.SelectedValue = Convert.ToString(args["TenantID"]);
            }
        }

        #endregion

        #region Grid Events

        protected void grdMapping_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetUploadedComplianceViewDocuments();
                grdMapping.DataSource = CurrentViewContext.ComplianceViewDocuments;
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

        protected void grdMapping_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.SystemDocumentID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("CSD_ID"));
                if (Presenter.IsDocumentMappedWithAttribute())
                {
                    base.ShowInfoMessage("Can not delete this document as it is mapped with an attribute.");
                }
                else if (Presenter.DeleteUploadedComplianceViewDocument(CurrentUserId))
                {
                    base.ShowSuccessMessage("Attribute Document deleted successfully.");
                }
                else
                {
                    base.ShowErrorMessage("Some error occured while deleting document. Please try again.");
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

        protected void grdMapping_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.SystemDocumentID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("CSD_ID"));

                String description = (e.Item.FindControl("txtDescription") as WclTextBox).Text;

                ClientSystemDocument sysDoc = new ClientSystemDocument();

                sysDoc.CSD_ID = CurrentViewContext.SystemDocumentID;
                sysDoc.CSD_Description = description;
                sysDoc.CSD_ModifiedByID = CurrentUserId;

                CurrentViewContext.ComplianceViewDocumentToUpdate = sysDoc;

                if (Presenter.UpdateUploadedComplianceViewDocument())
                {
                    base.ShowSuccessMessage("Attribute Document updated successfully.");
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

        protected void grdMapping_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("FieldMapping"))
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    CurrentViewContext.SystemDocumentID = Convert.ToInt32((dataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CSD_ID"]);

                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    queryString = new Dictionary<String, String>
                    { 
                        {"Child", ChildControls.AttributeDocumentFieldMapping},
                        {"CSD_ID",CurrentViewContext.SystemDocumentID.ToString()},
                        {"TenantID",CurrentViewContext.TenantID.ToString()}
                    };
                    String url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
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
                Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), false);
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

        #region Dropdown Events
        protected void ddlTenant_DataBound(object sender, EventArgs e)
        {
            ddlTenant.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
        }

        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!ddlTenant.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(ddlTenant.SelectedValue) > AppConsts.NONE)
            {
                CurrentViewContext.TenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                ucUploadDocuments.TenantID = Convert.ToInt32(ddlTenant.SelectedValue);
            }
            else
            {
                CurrentViewContext.TenantID = AppConsts.NONE;
                ucUploadDocuments.TenantID = AppConsts.NONE;
            }
            grdMapping.Rebind();
        }
        #endregion

        #endregion

        #region Methods

        #region Private Methods

        private void GetDisclosureDocumentData(string filePath)
        {

            Response.Clear();
            Response.Buffer = true;
            try
            {
                String extension = Path.GetExtension(filePath);
                //FileStream myFileStream = new FileStream(sPath, FileMode.Open);
                //long FileSize = myFileStream.Length;
                //byte[] Buffer = new byte[(int)FileSize];
                byte[] Buffer = CommonFileManager.RetrieveDocument(filePath, FileType.SystemDocumentLocation.GetStringValue());
                //myFileStream.Close();
                //myFileStream.Dispose();
                //Response.AddHeader("Content-Length", FileSize.ToString());
                Response.ContentType = GetContentType(extension);
                Response.AddHeader("Content-Disposition", "inline;");
                if (Buffer.IsNotNull())
                {
                    Response.BinaryWrite(Buffer);
                }
            }
            catch (Exception ex)
            {
                LogError("", ex);
            }

            finally
            {
                Response.End();
            }
        }

        void ucUploadDocuments_OnCompletedUpload()
        {
            grdMapping.Rebind();
        }

        private void BindControls()
        {
            Presenter.GetTenants();
            ddlTenant.DataSource = CurrentViewContext.lstTenant;
            ddlTenant.DataBind();
            Presenter.IsAdminLoggedIn();
            if (!CurrentViewContext.IsAdminLoggedIn)
            {
                ddlTenant.SelectedValue = Convert.ToString(CurrentViewContext.ClientTenantID);
                ddlTenant.Enabled = false;
            }
        }

        /// <summary>
        /// Get content type based on the file type.
        /// </summary>
        /// <param name="fileExtension">Extension of the file to download.</param>
        /// <returns>Content type for the file.</returns>
        private String GetContentType(String fileExtension)
        {
            switch (fileExtension)
            {
                case ".txt":
                    return "text/plain";
                case ".doc":
                case ".docx":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".xlsx":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".pdf":
                    return "application/pdf";
                case ".ppt":
                    return "application/mspowerpoint";
                default:
                    return "application/octet-stream";
            }
        }

        #endregion

        #region Public Methods

        #endregion

        #endregion

        #region Apply Permissions

        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();

                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Add",
                    CustomActionLabel = "Add New",
                    ScreenName = "Manage Attribute Documents"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Edit",
                    CustomActionLabel = "Edit",
                    ScreenName = "Manage Attribute Documents"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Delete",
                    CustomActionLabel = "Delete",
                    ScreenName = "Manage Attribute Documents"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Mapping",
                    CustomActionLabel = "Mapping",
                    ScreenName = "Manage Attribute Documents"
                });

                return actionCollection;
            }
        }

        public override List<String> ChildScreenPathCollection
        {
            get
            {
                List<String> childScreenPathCollection = new List<String>();
                childScreenPathCollection.Add(ChildControls.AttributeDocumentFieldMapping);
                return childScreenPathCollection;
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
                                if (x.FeatureAction.CustomActionId == "Add")
                                {
                                    //Need to be done
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    grdMapping.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdMapping.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Mapping")
                                {
                                    grdMapping.MasterTableView.GetColumn("FieldMapping").Display = false;
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

