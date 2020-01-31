using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class DocumentUrlInfo : BaseUserControl, IDocumentUrlInfoView
    {
        #region Variables
        private Boolean _isFalsePostBack = false;
        #endregion

        #region Properties

        public Boolean HasDuplicateNames
        {
            get
            {
                List<String> nameList = new List<String>();

                nameList.AddRange(DocumentUrlTempList.Select(x => x.SampleDocFormURL.ToLower().Trim() + "#" + x.SampleDocFormURLLabel.ToLower().Trim()));

                nameList.Add(SampleDocFormURL.ToLower().Trim() + "#" + SampleDocFormURLLabel.ToLower().Trim());
                return !(nameList.Count() == nameList.Distinct().Count());

            }
        }

        public List<DocumentUrlContract> DocumentUrlList
        {
            get
            {
                if (IsEditModeOn)
                {
                    IsEditModeOn = false;
                    rptrDocumentUrl.DataSource = DocumentUrlTempList;
                    rptrDocumentUrl.DataBind();
                }
                if (divErrorMessage.Visible)
                    divErrorMessage.Visible = false;
                if (DocumentUrlTempList.IsNotNull())
                {
                    //  No point in adding anything if empty
                    if (!String.IsNullOrWhiteSpace(txtNewSampleDocFormURL.Text))
                    {

                        int docUrlID = 0;
                        int.TryParse(hdnDocUrlId.Text, out docUrlID);
                        DocumentUrlContract documentUrlContract = new DocumentUrlContract()
                        {
                            SampleDocFormURL = NewSampleDocFormURL,
                            SampleDocFormURLLabel = NewSampleDocFormURLLabel,
                            SampleDocFormUrlDisplayLabel = NewSampleDocFormUrlDisplayLabel,
                            ID = docUrlID
                        };
                        if (!HasDuplicateNames)
                        {
                            // Add a new Document Url
                            DocumentUrlTempList.Add(documentUrlContract);

                            //TODO: Renamme DocumentUrlTempListMaxOcc
                            rptrDocumentUrl.DataSource = DocumentUrlTempListMaxOcc;
                            rptrDocumentUrl.DataBind();

                        }
                        txtNewSampleDocFormURL.Text = String.Empty;
                        txtNewSampleDocFormUrlDisplayLabel.Text = String.Empty;
                        txtNewSampleDocFormURLLabel.Text = string.Empty;

                        divErrorMessage.Visible = false;
                    }
                    return DocumentUrlTempList;
                }
                return new List<DocumentUrlContract>();
            }
            set
            {
                DocumentUrlTempList = value;
            }
        }

        public List<DocumentUrlContract> DocumentUrlTempList
        {
            get
            {
                if (ViewState["DocumentUrlList"] != null)
                {
                    return ViewState["DocumentUrlList"] as List<DocumentUrlContract>;
                }
                return null;
            }
            set
            {
                ViewState["DocumentUrlList"] = value;
            }
        }

        public String SampleDocFormURL
        {
            get
            {
                if (ViewState["SampleDocFormURL"] != null)
                {
                    return Convert.ToString(ViewState["SampleDocFormURL"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["SampleDocFormURL"] = value;
            }
        }

        public String SampleDocFormURLLabel
        {
            get
            {
                if (ViewState["SampleDocFormURLLabel"] != null)
                {
                    return Convert.ToString(ViewState["SampleDocFormURLLabel"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["SampleDocFormURLLabel"] = value;
            }
        }


        //TODO: Check 
        public List<DocumentUrlContract> DocumentUrlTempListMaxOcc
        {
            get
            {
                return ViewState["DocumentUrlList"] as List<DocumentUrlContract>;
                //List<DocumentUrlContract> tempPersonAliasList = ViewState["DocumentUrlList"] as List<DocumentUrlContract>;
                //if (IsEditProfile || (MaxOccurance == AppConsts.NONE) || MaxOccurance.IsNullOrEmpty())
                //{
                //    return tempPersonAliasList;
                //}
                //else
                //{
                //    return tempPersonAliasList.OrderBy(cond => cond.AliasSequenceId).Take(MaxOccurance.Value).ToList();
                //}
            }
        }

        public Boolean IsEditModeOn
        {
            get
            {
                if (ViewState["IsEditModeOn"] != null)
                {
                    return Convert.ToBoolean(ViewState["IsEditModeOn"]);
                }
                return false;
            }
            set
            {
                txtNewSampleDocFormURL.Enabled = !value;
                txtNewSampleDocFormUrlDisplayLabel.Enabled = !value;
                txtNewSampleDocFormURLLabel.Enabled = !value;
                btnAddNewRecord.Enabled = !value;

                ViewState["IsEditModeOn"] = value;
            }
        }

        //TODO Check -to use it
        public Boolean IsEditProfile
        {
            get
            {
                if ((ViewState["IsEditProfile"].IsNullOrEmpty()))
                {
                    ViewState["IsEditProfile"] = false;
                }
                return (Boolean)ViewState["IsEditProfile"];
            }
            set
            {
                ViewState["IsEditProfile"] = value;
            }
        }

        public Boolean IsReadOnly
        {
            get;
            set;
        }
        //if Document Url  is to show in the label mark this property as true,else Disabled Textbox will show the Document Url in Repeater 
        public Boolean IsLabelMode
        {
            get;
            set;
        }

        public Int32? MaxOccurance
        {
            get
            {
                if (!(ViewState["MaxOccurance"].IsNull()))
                {
                    return (Int32)ViewState["MaxOccurance"];
                }
                return null;
            }
            set
            {
                ViewState["MaxOccurance"] = value;
            }
        }

        public String NewSampleDocFormURL
        {
            get
            {
                return txtNewSampleDocFormURL.Text.Trim();
            }
        }
        #region UAT - 3740

        public String NewSampleDocFormUrlDisplayLabel
        {
            get
            {
                return txtNewSampleDocFormUrlDisplayLabel.Text.Trim();
            }
        }
        #endregion
        public String NewSampleDocFormURLLabel
        {
            get
            {
                return txtNewSampleDocFormURLLabel.Text.Trim();
            }
        }

        public bool IsUcOnComplianceCategoryScreen { get; set; }

        public Boolean IsFalsePostBack
        {
            get
            {
                return _isFalsePostBack;
            }
            set
            {
                _isFalsePostBack = value;
            }
        }

        #endregion

        #region Events

        #region Page Event
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsUcOnComplianceCategoryScreen)
                {
                    this.footerDisplayUrlLabelSection.Visible = false;
                    this.divFooterLable1.Text = this.divFooterLable1.Text = "Sample Document Form URL";
                }
                else
                {
                    this.footerURLDisplaylabel.Visible = false;
                }


                if (!IsPostBack || IsFalsePostBack)
                {
                    if (IsReadOnly)
                    {
                        divFooter.Visible = false;
                    }
                    divErrorMessage.Visible = false;

                    rptrDocumentUrl.DataSource = DocumentUrlTempList;
                    rptrDocumentUrl.DataBind();

                    foreach (RepeaterItem ri in rptrDocumentUrl.Items)
                    {
                        Label lblSampleDocFormURL = ri.FindControl("lblSampleDocFormURL") as Label;
                        Label lblSampleDocFormURLDisplayLabel = ri.FindControl("lblSampleDocFormURLDisplayLabel") as Label;
                        WclTextBox txtNewSampleDocFormURL1 = ri.FindControl("txtNewSampleDocFormURL1") as WclTextBox;
                        Label lblSampleDocFormURLLabel = ri.FindControl("lblSampleDocFormURLLabel") as Label;
                        if (IsLabelMode)
                        {
                            if (lblSampleDocFormURL.IsNotNull())
                                lblSampleDocFormURL.Visible = true;
                            if (lblSampleDocFormURLDisplayLabel.IsNotNull())
                                lblSampleDocFormURLDisplayLabel.Visible = true;
                            if (txtNewSampleDocFormURL1.IsNotNull())
                                txtNewSampleDocFormURL1.Visible = false;
                            if (lblSampleDocFormURLLabel.IsNotNull())
                                lblSampleDocFormURLLabel.Visible = true;

                        }
                        else
                        {
                            if (lblSampleDocFormURL.IsNotNull())
                                lblSampleDocFormURL.Visible = false;
                            if (lblSampleDocFormURLDisplayLabel.IsNotNull())
                                lblSampleDocFormURLDisplayLabel.Visible = false;
                            if (txtNewSampleDocFormURL1.IsNotNull())
                                txtNewSampleDocFormURL1.Visible = true;
                            if (lblSampleDocFormURLLabel.IsNotNull())
                                lblSampleDocFormURLLabel.Visible = false;

                        }
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
        #endregion

        #region Button Events
        protected void OnAddRecord(object sender, EventArgs e)
        {
            try
            {
                if (DocumentUrlTempList.IsNull())
                    DocumentUrlTempList = new List<DocumentUrlContract>();
                //  No point in adding anything if empty
                if (!String.IsNullOrWhiteSpace(txtNewSampleDocFormURL.Text))
                {
                    DocumentUrlContract documentUrlContract = new DocumentUrlContract()
                    {
                        SampleDocFormURL = NewSampleDocFormURL,
                        SampleDocFormUrlDisplayLabel = NewSampleDocFormUrlDisplayLabel,
                        SampleDocFormURLLabel = NewSampleDocFormURLLabel

                    };
                    if (IsUcOnComplianceCategoryScreen)
                    {
                        if ((!DocumentUrlTempList.Any(cond => cond.SampleDocFormURL.ToLower() == documentUrlContract.SampleDocFormURL.ToLower())))
                        {
                            OnAddSetControls(documentUrlContract);
                        }
                        else
                        {
                            DisplayMessageOnDuplicateRecordAdd();
                        }
                    }
                    else
                    {
                        if ((!DocumentUrlTempList.Any(cond => cond.SampleDocFormURL.ToLower() == documentUrlContract.SampleDocFormURL.ToLower())))
                        {
                            OnAddSetControls(documentUrlContract);
                        }
                        else
                        {
                            DisplayMessageOnDuplicateRecordAdd();
                        }

                    }

                }
                else
                {
                    divErrorMessage.Visible = true;
                    if (!IsUcOnComplianceCategoryScreen)
                    {
                        lblErrorMsg.Text = "Sample Document Form URL is required.";
                    }
                    else
                    {
                        lblErrorMsg.Text = "URL For More Information is required.";
                    }

                }
                IsEditModeOn = false;
                txtNewSampleDocFormURL.Focus();
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

        private void DisplayMessageOnDuplicateRecordAdd()
        {
            divErrorMessage.Visible = true;
            lblErrorMsg.Text = "Duplicate document url cannot be added.";
        }

        private void OnAddSetControls(DocumentUrlContract documentUrlContract)
        {
            // Add a document url
            DocumentUrlTempList.Add(documentUrlContract);
            txtNewSampleDocFormURL.Text = String.Empty;
            txtNewSampleDocFormUrlDisplayLabel.Text = String.Empty;
            txtNewSampleDocFormURLLabel.Text = String.Empty;

            divErrorMessage.Visible = false;
            rptrDocumentUrl.DataSource = DocumentUrlTempList;
            rptrDocumentUrl.DataBind();
            foreach (RepeaterItem ri in rptrDocumentUrl.Items)
            {
                Label lblSampleDocFormURL = ri.FindControl("lblSampleDocFormURL") as Label;
                Label lblSampleDocFormURLDisplayLabel = ri.FindControl("lblSampleDocFormURLDisplayLabel") as Label;
                WclTextBox txtNewSampleDocFormURL1 = ri.FindControl("txtNewSampleDocFormURL1") as WclTextBox;
                Label lblSampleDocFormURLLabel = ri.FindControl("lblSampleDocFormURLLabel") as Label;

                if (IsLabelMode)
                {
                    if (lblSampleDocFormURL.IsNotNull())
                        lblSampleDocFormURL.Visible = true;
                    if (lblSampleDocFormURLDisplayLabel.IsNotNull())
                        lblSampleDocFormURLDisplayLabel.Visible = true;
                    if (txtNewSampleDocFormURL1.IsNotNull())
                        txtNewSampleDocFormURL1.Visible = false;
                    if (lblSampleDocFormURLLabel.IsNotNull())
                        lblSampleDocFormURLLabel.Visible = true;
                }
                else
                {
                    if (lblSampleDocFormURL.IsNotNull())
                        lblSampleDocFormURL.Visible = false;
                    if (lblSampleDocFormURLDisplayLabel.IsNotNull())
                        lblSampleDocFormURLDisplayLabel.Visible = false;
                    if (txtNewSampleDocFormURL1.IsNotNull())
                        txtNewSampleDocFormURL1.Visible = true;
                    if (lblSampleDocFormURLLabel.IsNotNull())
                        lblSampleDocFormURLLabel.Visible = false;
                }
            }

        }
        #endregion

        #region Repeater Event
        protected void rptrDocumentUrl_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                DocumentUrlContract documentUrlDetail = null;

                if (e.CommandName == "delete")
                {
                    DocumentUrlTempList.Remove(DocumentUrlTempList[e.Item.ItemIndex]);
                    divErrorMessage.Visible = false;
                    IsEditModeOn = false;
                    rptrDocumentUrl.DataSource = DocumentUrlTempList;
                    rptrDocumentUrl.DataBind();
                    //hide or show the add new button 
                    if (DocumentUrlTempList.Count() == 0)
                    {
                        divFooter.Visible = true;
                    }
                }
                if (e.CommandName == "cancel")
                {

                    WclTextBox txtSampleDocFormURL = e.Item.FindControl("txtSampleDocFormURL") as WclTextBox;
                    if (txtSampleDocFormURL.IsNotNull())
                        txtSampleDocFormURL.Visible = false;
                    WclTextBox txtSampleDocFormURLDisplayLabel = e.Item.FindControl("txtSampleDocFormURLDisplayLabel") as WclTextBox;
                    if (txtSampleDocFormURLDisplayLabel.IsNotNull())
                        txtSampleDocFormURLDisplayLabel.Visible = false;
                    WclTextBox txtSampleDocFormURLLabel = e.Item.FindControl("txtSampleDocFormURLLabel") as WclTextBox;
                    if (txtSampleDocFormURLLabel.IsNotNull())
                        txtSampleDocFormURLLabel.Visible = false;

                    if (IsLabelMode)
                    {
                        Label lblSampleDocFormURL = e.Item.FindControl("lblSampleDocFormURL") as Label;
                        Label lblSampleDocFormURLDisplayLabel = e.Item.FindControl("lblSampleDocFormURLDisplayLabel") as Label;
                        Label lblSampleDocFormURLLabel = e.Item.FindControl("lblSampleDocFormURLLabel") as Label;
                        if (lblSampleDocFormURL.IsNotNull())
                            lblSampleDocFormURL.Visible = true;
                        if (lblSampleDocFormURLDisplayLabel.IsNotNull())
                            lblSampleDocFormURLDisplayLabel.Visible = true;
                        if (lblSampleDocFormURLLabel.IsNotNull())
                            lblSampleDocFormURLLabel.Visible = true;
                    }



                    LinkButton btnEdit = e.Item.FindControl("btnEdit") as LinkButton;
                    if (btnEdit.IsNotNull())
                    {
                        btnEdit.Text = "Edit";
                        btnEdit.CommandName = "edit";

                    }
                    LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
                    if (btnDelete.IsNotNull())
                    {
                        btnDelete.Text = "Delete";
                        btnDelete.CommandName = "delete";
                        btnDelete.OnClientClick = "return confirm('Are you sure you want to delete the document url')";

                    }
                    divErrorMessage.Visible = false;
                    IsEditModeOn = false;
                    rptrDocumentUrl.DataSource = DocumentUrlTempList;
                    rptrDocumentUrl.DataBind();
                }
                else if (e.CommandName == "edit")
                {
                    if (IsEditModeOn)
                    {
                        divErrorMessage.Visible = true;
                        lblErrorMsg.Text = "Only one record can be updated at a time.";
                    }
                    else
                    {
                        WclTextBox txtSampleDocFormURL = e.Item.FindControl("txtSampleDocFormURL") as WclTextBox;
                        if (txtSampleDocFormURL.IsNotNull())
                            txtSampleDocFormURL.Visible = true;
                        WclTextBox txtSampleDocFormURLDisplayLabel = e.Item.FindControl("txtSampleDocFormURLDisplayLabel") as WclTextBox;
                        if (txtSampleDocFormURLDisplayLabel.IsNotNull())
                            txtSampleDocFormURLDisplayLabel.Visible = true;
                        WclTextBox txtSampleDocFormURLLabel = e.Item.FindControl("txtSampleDocFormURLLabel") as WclTextBox;
                        if (txtSampleDocFormURLLabel.IsNotNull())
                            txtSampleDocFormURLLabel.Visible = true;

                        if (IsLabelMode)
                        {
                            Label lblSampleDocFormUR = e.Item.FindControl("lblSampleDocFormURL") as Label;
                            Label lblSampleDocFormURDisplayLabel = e.Item.FindControl("lblSampleDocFormURLDisplayLabel") as Label;
                            Label lblSampleDocFormURLLabel = e.Item.FindControl("lblSampleDocFormURLLabel") as Label;

                            if (lblSampleDocFormUR.IsNotNull())
                                lblSampleDocFormUR.Visible = false;
                            if (lblSampleDocFormURDisplayLabel.IsNotNull())
                                lblSampleDocFormURDisplayLabel.Visible = false;
                            if (lblSampleDocFormURLLabel.IsNotNull())
                                lblSampleDocFormURLLabel.Visible = false;


                        }
                        else
                        {
                            WclTextBox txtNewSampleDocFormURL1 = e.Item.FindControl("txtSampleDocFormURL1") as WclTextBox;
                            WclTextBox txtNewSampleDocFormURLDisplayLabel1 = e.Item.FindControl("txtSampleDocFormURLDisplayLabel1") as WclTextBox;
                            WclTextBox txtSampleDocFormURLLabel1 = e.Item.FindControl("txtSampleDocFormURLLabel1") as WclTextBox;

                            if (txtNewSampleDocFormURL1.IsNotNull())
                                txtNewSampleDocFormURL1.Visible = true;
                            if (txtNewSampleDocFormURLDisplayLabel1.IsNotNull())
                                txtNewSampleDocFormURLDisplayLabel1.Visible = true;
                            if (txtSampleDocFormURLLabel1.IsNotNull())
                                txtSampleDocFormURLLabel1.Visible = true;

                        }

                        WclTextBox sampleDocFormURL = e.Item.FindControl("txtSampleDocFormURL") as WclTextBox;

                        sampleDocFormURL.Focus();

                        SampleDocFormURL = sampleDocFormURL.Text.ToString();

                        LinkButton btnEdit = e.Item.FindControl("btnEdit") as LinkButton;
                        if (btnEdit.IsNotNull())
                        {
                            btnEdit.Text = "Save";
                            btnEdit.CommandName = "save";
                        }
                        LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
                        if (btnDelete.IsNotNull())
                        {
                            btnDelete.Text = "Cancel";
                            btnDelete.CommandName = "cancel";
                            btnDelete.OnClientClick = "";
                        }
                        divErrorMessage.Visible = false;
                        IsEditModeOn = true;
                    }
                }
                else if (e.CommandName == "save")
                {
                    documentUrlDetail = DocumentUrlTempList[e.Item.ItemIndex];
                    WclTextBox txtSampleDocFormURL = e.Item.FindControl("txtSampleDocFormURL") as WclTextBox;
                    WclTextBox txtSampleDocFormURLDisplayLabel = e.Item.FindControl("txtSampleDocFormURLDisplayLabel") as WclTextBox;
                    WclTextBox txtSampleDocFormURLLabel = e.Item.FindControl("txtSampleDocFormURLLabel") as WclTextBox;
                    HtmlControl rptrDivErrorMessage = e.Item.FindControl("rptrDivErrorMessage") as HtmlControl;
                    Label rptrLblErrorMsg = e.Item.FindControl("rptrLblErrorMsg") as Label;

                    if (txtSampleDocFormURL.IsNotNull() && txtSampleDocFormURLLabel.IsNotNull())
                    {
                        //if ((DocumentUrlTempList.Any(cond => cond.SampleDocFormURL.ToLower() == txtSampleDocFormURL.Text.ToLower())
                        //    //&& (DocumentUrlTempList.Any(cond => cond.SampleDocFormURLLabel.IsNull() || cond.SampleDocFormURLLabel.ToLower() == txtSampleDocFormURLLabel.Text.ToLower()))
                        //    //&& (DocumentUrlTempList.Any(cond => cond.SampleDocFormUrlDisplayLabel.IsNull() || cond.SampleDocFormUrlDisplayLabel.ToLower() == txtSampleDocFormURLDisplayLabel.Text.ToLower())) 
                        //    && !IsEditModeOn)
                        //    )

                        if ((DocumentUrlTempList.Any(cond => cond.SampleDocFormURL.ToLower() == txtSampleDocFormURL.Text.ToLower()) && !IsEditModeOn)
                            || (IsEditModeOn && DocumentUrlTempList.Where(cond=>cond.SampleDocFormURL.ToLower() != SampleDocFormURL.ToLower()).Any(cond=>cond.SampleDocFormURL.ToLower() == txtSampleDocFormURL.Text.ToLower())))
                        {
                            IsEditModeOn = true;
                            rptrDivErrorMessage.Visible = true;
                            rptrLblErrorMsg.Text = "Duplicate document url cannot be added.";
                            WclTextBox sampleDocFormURL = e.Item.FindControl("txtSampleDocFormURL") as WclTextBox;
                            sampleDocFormURL.Focus();
                            return;
                        }
                        //if (SampleDocFormURL.ToLower().Equals(txtSampleDocFormURL.Text.ToLower()) && SampleDocFormURLLabel.ToLower().Equals(txtSampleDocFormURLLabel.Text.ToLower()))
                        //{
                        //    IsEditModeOn = true;
                        //    divErrorMessage.Visible = true;
                        //    lblErrorMsg.Text = "Duplicate document url cannot be added.";
                        //    WclTextBox sampleDocFormURL = e.Item.FindControl("txtSampleDocFormURL") as WclTextBox;
                        //    sampleDocFormURL.Focus();
                        //    return;
                        //}

                        if (
                           (documentUrlDetail.SampleDocFormURLLabel.IsNullOrEmpty())
                           ||
                           !(documentUrlDetail.SampleDocFormURL.ToLower() == txtSampleDocFormURL.Text.ToLower().Trim() && documentUrlDetail.SampleDocFormURLLabel.ToLower() == txtSampleDocFormURLLabel.Text.ToLower().Trim())
                           )
                        {
                            if (
                                 DocumentUrlTempList.Any(cond => cond.SampleDocFormURLLabel.IsNullOrEmpty())
                                 ||
                                 !DocumentUrlTempList.Any(cond => cond.SampleDocFormURL.ToLower() == txtSampleDocFormURL.Text.ToLower().Trim() && cond.SampleDocFormURLLabel.ToLower() == txtSampleDocFormURLLabel.Text.ToLower().Trim())
                               )
                            {
                                documentUrlDetail.SampleDocFormURL = txtSampleDocFormURL.Text.Trim();
                                documentUrlDetail.SampleDocFormURLLabel = txtSampleDocFormURLLabel.Text.Trim();
                                documentUrlDetail.SampleDocFormUrlDisplayLabel = txtSampleDocFormURLDisplayLabel.Text.Trim();
                                txtSampleDocFormURL.Visible = false;
                                txtSampleDocFormURLDisplayLabel.Visible = false;
                                txtSampleDocFormURLLabel.Visible = false;

                                if (IsLabelMode)
                                {
                                    Label lblSampleDocFormURL = e.Item.FindControl("lblSampleDocFormURL") as Label;
                                    Label lblSampleDocFormURLDisplayLabel = e.Item.FindControl("lblSampleDocFormURLDisplayLabel") as Label;
                                    if (lblSampleDocFormURL.IsNotNull())
                                        lblSampleDocFormURL.Visible = true;
                                    if (lblSampleDocFormURLDisplayLabel.IsNotNull())
                                        lblSampleDocFormURLDisplayLabel.Visible = true;
                                    Label lblSampleDocFormURLLabel = e.Item.FindControl("lblSampleDocFormURLLabel") as Label;
                                    if (lblSampleDocFormURLLabel.IsNotNull())
                                        lblSampleDocFormURLLabel.Visible = true;
                                }
                                else
                                {
                                    WclTextBox txtSampleDocFormURL1 = e.Item.FindControl("txtSampleDocFormURL1") as WclTextBox;
                                    if (txtSampleDocFormURL1.IsNotNull())
                                        txtSampleDocFormURL1.Visible = true;
                                    WclTextBox txtSampleDocFormURLDisplayLabel1 = e.Item.FindControl("txtSampleDocFormURLDisplayLabel1") as WclTextBox;//UAT - 3740
                                    if (txtSampleDocFormURLDisplayLabel1.IsNotNull())
                                        txtSampleDocFormURLDisplayLabel1.Visible = true;
                                    WclTextBox txtSampleDocFormURLLabel1 = e.Item.FindControl("txtSampleDocFormURLLabel1") as WclTextBox;
                                    if (txtSampleDocFormURLLabel1.IsNotNull())
                                        txtSampleDocFormURLLabel1.Visible = true;
                                }

                                LinkButton btnEdit = e.Item.FindControl("btnEdit") as LinkButton;
                                if (btnEdit.IsNotNull())
                                {
                                    btnEdit.Text = "Edit";
                                    btnEdit.CommandName = "edit";
                                }
                                LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
                                if (btnDelete.IsNotNull())
                                {
                                    btnDelete.Text = "Delete";
                                    btnDelete.CommandName = "delete";
                                    btnDelete.OnClientClick = "return confirm('Are you sure you want to delete document url ?')";
                                }
                                rptrDivErrorMessage.Visible = false;

                                divErrorMessage.Visible = false;
                                IsEditModeOn = false;
                                rptrDocumentUrl.DataSource = DocumentUrlTempList;
                                rptrDocumentUrl.DataBind();

                            }
                            else
                            {
                                IsEditModeOn = true;

                                divErrorMessage.Visible = true;
                                lblErrorMsg.Text = "Duplicate document url cannot be added.";
                            }
                        }
                        else
                        {
                            txtSampleDocFormURL.Visible = false;
                            txtSampleDocFormURLDisplayLabel.Visible = false;
                            txtSampleDocFormURLLabel.Visible = false;

                            if (IsLabelMode)
                            {
                                Label lblSampleDocFormURL = e.Item.FindControl("lblSampleDocFormURL") as Label;
                                Label lblSampleDocFormURLDisplayLabel = e.Item.FindControl("lblSampleDocFormURLDisplayLabel") as Label;
                                if (lblSampleDocFormURL.IsNotNull())
                                    lblSampleDocFormURL.Visible = true;
                                if (lblSampleDocFormURLDisplayLabel.IsNotNull())
                                    lblSampleDocFormURLDisplayLabel.Visible = true;
                                Label lblSampleDocFormURLLabel = e.Item.FindControl("lblSampleDocFormURLLabel") as Label;
                                if (lblSampleDocFormURLLabel.IsNotNull())
                                    lblSampleDocFormURLLabel.Visible = true;

                            }
                            else
                            {
                                WclTextBox txtSampleDocFormURL1 = e.Item.FindControl("txtSampleDocFormURL1") as WclTextBox;
                                WclTextBox txtSampleDocFormURLDisplayLabel1 = e.Item.FindControl("txtSampleDocFormURLDisplayLabel1") as WclTextBox;
                                if (txtSampleDocFormURL1.IsNotNull())
                                    txtSampleDocFormURL1.Visible = true;
                                if (txtSampleDocFormURLDisplayLabel1.IsNotNull())
                                    txtSampleDocFormURLDisplayLabel1.Visible = true;
                                WclTextBox txtSampleDocFormURLLabel1 = e.Item.FindControl("txtSampleDocFormURLLabel1") as WclTextBox;
                                if (txtSampleDocFormURLLabel1.IsNotNull())
                                    txtSampleDocFormURLLabel1.Visible = true;


                            }
                            LinkButton btnEdit = e.Item.FindControl("btnEdit") as LinkButton;
                            if (btnEdit.IsNotNull())
                            {
                                btnEdit.Text = "Edit";
                                btnEdit.CommandName = "edit";
                            }
                            LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
                            if (btnDelete.IsNotNull())
                            {
                                btnDelete.Text = "Delete";
                                btnDelete.CommandName = "delete";
                                btnDelete.OnClientClick = "return confirm('Are you sure you want to delete the document url ?')";
                            }
                            rptrDivErrorMessage.Visible = false;

                            divErrorMessage.Visible = false;
                            IsEditModeOn = false;

                        }
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

        protected void rptrDocumentUrl_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (!IsUcOnComplianceCategoryScreen)
                {
                    Control rptrDisplayUrlLabelSection = e.Item.FindControl("rptrDisplayUrlLabelSection") as Control;
                    if (rptrDisplayUrlLabelSection.IsNotNull())
                        rptrDisplayUrlLabelSection.Visible = false;
                    footerDisplayUrlLabelSection.Visible = false;

                    Label rptrLable1 = e.Item.FindControl("rptrLable1") as Label;
                    if (rptrLable1.IsNotNull())
                    {
                        rptrLable1.Text = "Sample Document Form URL";
                    }
                    Label rptrLable2 = e.Item.FindControl("rptrLable2") as Label;
                    if (rptrLable2.IsNotNull())
                    {
                        rptrLable2.Text = "Sample Document Form URL";
                    }
                }
                else
                {

                    Control dvrptrURLDisplaylabel = e.Item.FindControl("dvrptrURLDisplaylabel") as Control;
                    if (dvrptrURLDisplaylabel.IsNotNull())
                        dvrptrURLDisplaylabel.Visible = false;
                    footerURLDisplaylabel.Visible = false;

                }
                if (IsReadOnly)
                {
                    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                    {
                        Control divsbutton = e.Item.FindControl("divButtons") as Control;
                        if (divsbutton.IsNotNull())
                            divsbutton.Visible = false;
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
        #endregion

        #endregion

        #region Methods
        #region Private Methods

        #endregion

        #endregion

        #region Public Methods


        public Boolean RebindRptrDocumentUrl()
        {
            rptrDocumentUrl.DataSource = null;
            if (!DocumentUrlTempList.IsNullOrEmpty() && DocumentUrlTempList.Count() > 0)
            {
                rptrDocumentUrl.DataSource = DocumentUrlTempList;
                rptrDocumentUrl.DataBind();
                divDocumentUrl.Style["display"] = "block";
                return true;
            }
            else
            {
                rptrDocumentUrl.DataBind();
                divDocumentUrl.Style["display"] = "block";
                return true;
            }
        }
        #endregion


    }
}
