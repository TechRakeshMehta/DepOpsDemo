using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using CuteWebUI;
using System.IO;
using System.Web.Configuration;
using INTSOF.Utils;
using Entity;
using INTERSOFT.WEB.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.Views
{
    public partial class DisclosureDocuments : BaseUserControl, IDisclosureDocumentsView
    {
        #region Variables

        #region Private Variables

        private DisclosureDocumentsPresenter _presenter = new DisclosureDocumentsPresenter();
        private String _viewType;

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        public IDisclosureDocumentsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        String IDisclosureDocumentsView.ErrorMessage
        {
            get;
            set;
        }

        List<SystemDocument> IDisclosureDocumentsView.DisclosureDocuments
        {
            get;
            set;
        }

        Int32 IDisclosureDocumentsView.SystemDocumentID
        {
            get;
            set;
        }

        SystemDocument IDisclosureDocumentsView.DisclosureDocumentToUpdate
        {
            get;
            set;
        }

        public DisclosureDocumentsPresenter Presenter
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

        List<lkpDocumentType> IDisclosureDocumentsView.DocumentTypeList
        {
            get;
            set;
        }

        //UAT-2625:
        List<lkpDisclosureDocumentAgeGroup> IDisclosureDocumentsView.DisclosureDocAgeGroupTypeList
        {
            get;
            set;
        }

        //UAT-3745
        List<ExternalBkgSvc> IDisclosureDocumentsView.lstExtBkgSvc
        {
            get;
            set;
        }

        Int32 IDisclosureDocumentsView.SelectedExtBkgSvcID { get; set; }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Manage D&A/Additional Documents";

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
            //Set Module Title
            BasePage basePage = base.Page as BasePage;
            if (basePage != null)
            {
                basePage.SetModuleTitle("Common Operations");
            }
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                BindDocumentType();
            }

            Presenter.OnViewLoaded();
            ucUploadDocuments.OnCompletedUpload -= new UploadDisclosureDocuments.UploadDelegate(ucUploadDocuments_OnCompletedUpload);
            ucUploadDocuments.OnCompletedUpload += new UploadDisclosureDocuments.UploadDelegate(ucUploadDocuments_OnCompletedUpload);
            base.SetPageTitle("Manage D&A/Additional Documents");

            ucUploadDocuments.DocumentType = cmbDocumentType.SelectedValue;
            ucUploadDocuments.IsOperational = null;
            ucUploadDocuments.SendToStudent = null;
            ucUploadDocuments.SelectedAgeGroup = null;
            if (cmbDocumentType.SelectedValue == DislkpDocumentType.ADDITIONAL_DOCUMENTS.GetStringValue())
            {
                ucUploadDocuments.IsOperational = chkIsOperational.Checked;
                ucUploadDocuments.SendToStudent = chkSendToStudent.Checked;
                //UAT-3745
                if (!cmbExtBkService.SelectedValue.IsNullOrEmpty())
                    ucUploadDocuments.SelectedExtBkgSvcID = Convert.ToInt32(cmbExtBkService.SelectedValue);
            }
            //UAT-2625
            else if (cmbDocumentType.SelectedValue == DislkpDocumentType.DISCLOSURE_AND_RELEASE_TEMPLATE.GetStringValue() && !cmbAgeGroups.SelectedValue.IsNullOrEmpty())
            {
                ucUploadDocuments.SelectedAgeGroup = Convert.ToInt32(cmbAgeGroups.SelectedValue);
            }

        }



        #endregion

        #region Grid Events

        protected void grdMapping_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                // Presenter.GetUploadedDisclosureDocuments();
                Presenter.GetBothUploadedDisclosureDocuments();
                grdMapping.DataSource = CurrentViewContext.DisclosureDocuments;
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
                CurrentViewContext.SystemDocumentID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("SystemDocumentID"));
                if (Presenter.DeleteUploadedDisclosureDocument(CurrentUserId))
                {
                    base.ShowSuccessMessage("D&A Document deleted successfully.");
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
                CurrentViewContext.SystemDocumentID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("SystemDocumentID"));

                String description = (e.Item.FindControl("txtDescription") as WclTextBox).Text;

                SystemDocument sysDoc = new SystemDocument();

                sysDoc.SystemDocumentID = CurrentViewContext.SystemDocumentID;
                sysDoc.Description = description;
                sysDoc.ModifiedBy = CurrentUserId;

                //UAT-3745
                WclComboBox cmbExtBkService = (e.Item.FindControl("cmbExtBkService") as WclComboBox);
                if (!cmbExtBkService.IsNullOrEmpty() && !cmbExtBkService.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(cmbExtBkService.SelectedValue) > AppConsts.NONE)
                {
                    Int32 extBkgSvcID = Convert.ToInt32(cmbExtBkService.SelectedValue);
                    //Entity.ExtBkgSvcDocumentMapping extBkgSvcDocMapping = new ExtBkgSvcDocumentMapping();
                    //extBkgSvcDocMapping.EBSDM_ExtBkgSvcID = extBkgSvcID;
                    //extBkgSvcDocMapping.EBSDM_SystemDocumentID = CurrentViewContext.SystemDocumentID;

                    //sysDoc.ExtBkgSvcDocumentMappings.Add(extBkgSvcDocMapping);

                    CurrentViewContext.SelectedExtBkgSvcID = extBkgSvcID;
                }
                //End
                CurrentViewContext.DisclosureDocumentToUpdate = sysDoc;

                if (Presenter.UpdateUploadedDisclosureDocument())
                {
                    base.ShowSuccessMessage("D&A Document updated successfully.");
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
                    CurrentViewContext.SystemDocumentID = Convert.ToInt32((dataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SystemDocumentID"]);

                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    queryString = new Dictionary<String, String>
                    { 
                        {"Child", ChildControls.DisclosureDocumentFieldMapping},
                        {"SystemDocumentID",CurrentViewContext.SystemDocumentID.ToString()}
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

        #region Combobox Event
        protected void cmbDocumentType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!cmbDocumentType.SelectedValue.IsNullOrEmpty())
            {
                if (cmbDocumentType.SelectedValue == DislkpDocumentType.ADDITIONAL_DOCUMENTS.GetStringValue())
                {
                    dvIsOperational.Visible = true;
                    //UAT-2625
                    dvAgeGroupType.Visible = false;
                }
                else if (cmbDocumentType.SelectedValue == DislkpDocumentType.DISCLOSURE_AND_RELEASE_TEMPLATE.GetStringValue())
                {
                    //UAT-2625
                    dvAgeGroupType.Visible = true;
                    dvIsOperational.Visible = false;
                    BindAgeGroupType();
                }
                else
                {
                    dvIsOperational.Visible = false;
                    //UAT-2625
                    dvAgeGroupType.Visible = false;
                }
                chkIsOperational.Checked = false;
                chkSendToStudent.Checked = false;
                //UAT-3745:- 
                ManageExtBkgSvcDivVisibility();
            }
            else
            {
                dvIsOperational.Visible = false;
                //UAT-2625
                dvAgeGroupType.Visible = false;
                //UAT-3745
                dvExternalBkgSvc.Visible = false;
            }
        }

        protected void cmbDocumentType_DataBound(object sender, EventArgs e)
        {
            cmbDocumentType.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--"));
        }
        #endregion

        #endregion

        #region Methods

        #region Private Methods

        private void BindDocumentType()
        {
            Presenter.GetDocumentTypes();
            cmbDocumentType.DataTextField = "DT_Name";
            cmbDocumentType.DataValueField = "DT_Code";
            cmbDocumentType.DataSource = CurrentViewContext.DocumentTypeList;
            cmbDocumentType.DataBind();
        }

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

        #region UAT-2625:
        private void BindAgeGroupType()
        {
            Presenter.GetAgeGroupTypes();
            cmbAgeGroups.DataTextField = "LDDAG_Name";
            cmbAgeGroups.DataValueField = "LDDAG_ID";
            cmbAgeGroups.DataSource = CurrentViewContext.DisclosureDocAgeGroupTypeList;
            cmbAgeGroups.DataBind();
        }
        #endregion

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
                    ScreenName = "Manage D&A Documents"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Edit",
                    CustomActionLabel = "Edit",
                    ScreenName = "Manage D&A Documents"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Delete",
                    CustomActionLabel = "Delete",
                    ScreenName = "Manage D&A Documents"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Mapping",
                    CustomActionLabel = "Mapping",
                    ScreenName = "Manage D&A Documents"
                });

                return actionCollection;
            }
        }

        public override List<String> ChildScreenPathCollection
        {
            get
            {
                List<String> childScreenPathCollection = new List<String>();
                childScreenPathCollection.Add(ChildControls.DisclosureDocumentFieldMapping);
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

        protected void grdMapping_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    SystemDocument systemDocument = (SystemDocument)(e.Item.DataItem);
                    if (systemDocument.lkpDocumentType.DT_Code == DislkpDocumentType.DISCLOSURE_AND_RELEASE_TEMPLATE.GetStringValue())
                    {

                        dataItem["DocType"].Text = "D & A Document";
                    }
                    if (!systemDocument.IsNullOrEmpty())
                    {
                        dataItem["Size"].Text = systemDocument.Size.IsNullOrEmpty() ? AppConsts.ZERO : Convert.ToString(systemDocument.Size / 1024);
                    }

                    //UAT-3745
                    if (systemDocument.lkpDocumentType.DT_Code == DislkpDocumentType.ADDITIONAL_DOCUMENTS.GetStringValue())
                    {
                        if (systemDocument.ExtBkgSvcDocumentMappings.Count > AppConsts.NONE)
                        {
                            ExtBkgSvcDocumentMapping extBkgSvcDocumentMapping = systemDocument.ExtBkgSvcDocumentMappings.WhereSelect(c => !c.EBSDM_IsDeleted).FirstOrNew();
                            if (!extBkgSvcDocumentMapping.IsNullOrEmpty() && extBkgSvcDocumentMapping.EBSDM_ID > AppConsts.NONE)
                            {
                                dataItem["Service"].Text = extBkgSvcDocumentMapping.ExternalBkgSvc.EBS_ExternalCode + " - " + extBkgSvcDocumentMapping.ExternalBkgSvc.EBS_Name;
                                dataItem["ExternalServiceID"].Text = extBkgSvcDocumentMapping.EBSDM_ExtBkgSvcID.ToString();
                            }
                        }
                    }
                }
            }

            //UAT-3745
            if ((e.Item is GridEditableItem) && (e.Item.IsInEditMode))
            {
                WclComboBox cmbExtBkService = e.Item.FindControl("cmbExtBkServiceEdit") as WclComboBox;
                SystemDocument systemDocument = (SystemDocument)(e.Item.DataItem);

                if (!cmbExtBkService.IsNullOrEmpty())
                {
                    if (systemDocument.lkpDocumentType.DT_Code == DislkpDocumentType.ADDITIONAL_DOCUMENTS.GetStringValue())
                    {
                        cmbExtBkService.Visible = true;

                        Presenter.GetExternalBkgSvc();
                        cmbExtBkService.DataSource = CurrentViewContext.lstExtBkgSvc;
                        cmbExtBkService.DataValueField = "EBS_ID";
                        cmbExtBkService.DataTextField = "EBS_Name";
                        cmbExtBkService.DataBind();
                        cmbExtBkService.Items.Insert(0, new RadComboBoxItem { Text = "--Select--", Value = "0" });

                        Int32 selectedExtBkgSvcID = AppConsts.NONE;
                        if (systemDocument.ExtBkgSvcDocumentMappings.Count > AppConsts.NONE)
                        {
                            selectedExtBkgSvcID = systemDocument.ExtBkgSvcDocumentMappings.FirstOrNew().EBSDM_ExtBkgSvcID;
                        }
                        cmbExtBkService.SelectedValue = selectedExtBkgSvcID.ToString();
                    }
                    else
                    {
                        cmbExtBkService.Visible = false;
                    }
                }
            }

        }

        #region UAT-3745:- Add additional document mapping with External Bkg service.

        #region Methods

        public void BindExternalBkgSvcs()
        {
            Presenter.GetExternalBkgSvc();
            cmbExtBkService.DataSource = CurrentViewContext.lstExtBkgSvc;
            cmbExtBkService.DataValueField = "EBS_ID";
            cmbExtBkService.DataTextField = "EBS_Name";
            cmbExtBkService.DataBind();
        }

        public void ManageExtBkgSvcDivVisibility()
        {
            if (cmbDocumentType.SelectedValue == DislkpDocumentType.ADDITIONAL_DOCUMENTS.GetStringValue())
            {
                dvExternalBkgSvc.Visible = true;
                BindExternalBkgSvcs();
            }
            else
            {
                dvExternalBkgSvc.Visible = false;
            }
        }

        #endregion

        #region Dropdown Events

        protected void cmbExtBkService_DataBound(object sender, EventArgs e)
        {
            cmbExtBkService.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--"));
        }

        #endregion

        #endregion
    }
}

