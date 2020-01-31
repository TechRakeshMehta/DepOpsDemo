using System;
using Microsoft.Practices.ObjectBuilder;
using Entity.ClientEntity;
using System.Collections.Generic;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using CoreWeb.Shell;


namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ManageUploadDocuments :System.Web.UI.Page, IManageUploadDocumentsView
    {
        private ManageUploadDocumentsPresenter _presenter=new ManageUploadDocumentsPresenter();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();

            ucUploadDocuments.OnCompletedUpload -= new UploadDocuments.UploadDelegate(ucUploadDocuments_OnCompletedUpload);
            ucUploadDocuments.OnCompletedUpload += new UploadDocuments.UploadDelegate(ucUploadDocuments_OnCompletedUpload);
            ucUploadDocuments.TenantID = CurrentViewContext.TenantID = Request.QueryString["TenantId"] != null ? Convert.ToInt32(Request.QueryString["TenantId"]) : 0;
        }

        void ucUploadDocuments_OnCompletedUpload()
        {
            grdMapping.Rebind();
        }

        
        public ManageUploadDocumentsPresenter Presenter
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

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	

        public IManageUploadDocumentsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<ApplicantDocument> ToSaveApplicantUploadedDocuments
        {
            get;
            set;
        }

        public Int32 CurrentUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public List<ApplicantDocument> ApplicantUploadedDocuments
        {
            get;
            set;
        }

        public Int32 TenantID
        {
            get;
            set;
        }

        public Int32 ApplicantUploadedDocumentID
        {
            get;
            set;
        }

        ApplicantDocument toUpdateUploadedDocument = new ApplicantDocument();
        public ApplicantDocument ToUpdateUploadedDocument
        {
            get { return toUpdateUploadedDocument; }
            set { toUpdateUploadedDocument = value; }
        }

        protected void grdMapping_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Presenter.GetApplicantUploadedDocuments();
            grdMapping.DataSource = ApplicantUploadedDocuments;
        }

        protected void grdMapping_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem gridEditableItem = e.Item as GridEditableItem;
            ApplicantUploadedDocumentID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ApplicantDocumentID"));
            Presenter.DeleteApplicantUploadedDocument();
        }

        protected void grdMapping_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem gridEditableItem = e.Item as GridEditableItem;
            ApplicantUploadedDocumentID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ApplicantDocumentID"));

            String description = (e.Item.FindControl("txtDescription") as WclTextBox).Text;

            ToUpdateUploadedDocument.ApplicantDocumentID = ApplicantUploadedDocumentID;
            ToUpdateUploadedDocument.Description = description;
            ToUpdateUploadedDocument.ModifiedByID = CurrentUserID;
            ToUpdateUploadedDocument.ModifiedOn = DateTime.Now;

            Presenter.UpdateApplicantUploadedDocument();

        }

        protected void grdMapping_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "ViewDoccument")
            {
                //ApplicantDocument currentDoccument = (e.Item).DataItem as ApplicantDocument;
                //String filename = "abc";
                //Response.Clear();
                //Response.ContentType = "application/octet-stream";
                //Response.AddHeader("Content-Disposition",
                //  "attachment; filename=abc");
                //Response.Flush();
                //Response.WriteFile("~//9b3ecae5-0009-4044-bbf9-140a492746c1.jpg");
                GridDataItem dataItem = (GridDataItem)e.Item;
                String filename = dataItem["FileName"].Text;
                //String fileName = "abc";
                String filepath = "~/MessageAttachments/ea85140a-67bb-49ee-986b-a0f26c2174dc.jpg";
                String originalFileName = "images.jpg";
                Response.Clear();
                Response.ClearHeaders();
                System.IO.Stream stream = null;
                stream = new System.IO.FileStream(Server.MapPath(filepath), System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                long bytesToRead = stream.Length;
                Response.ContentType = GetContentType(originalFileName.Substring(originalFileName.LastIndexOf(".")));
                //Response.BufferOutput = true; 
                Response.AddHeader("Content-Disposition", "attachment; filename=" + originalFileName.Replace(" ", "_"));

                while (bytesToRead > 0)
                {
                    if (Response.IsClientConnected)
                    {
                        byte[] buffer = new Byte[10000];
                        int length = stream.Read(buffer, 0, 10000);
                        Response.OutputStream.Write(buffer, 0, length);
                        Response.Flush();
                        bytesToRead = bytesToRead - length;
                    }
                    else
                    {
                        bytesToRead = -1;
                    }
                }
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
    }
}

