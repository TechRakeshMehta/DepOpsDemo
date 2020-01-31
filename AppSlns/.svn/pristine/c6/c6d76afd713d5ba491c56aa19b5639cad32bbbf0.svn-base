using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class RequiredDocuments : BaseUserControl, IRequiredDocumentsView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variable

        private RequiredDocumentsPresenter _presenter = new RequiredDocumentsPresenter();
        private Int32 _tenantid;

        #endregion

        #endregion

        #region Properties

        public RequiredDocumentsPresenter Presenter
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

        public IRequiredDocumentsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Boolean Visiblity
        {
            get
            {
                if (ViewState["Visiblity"] != null)
                    return (Convert.ToBoolean(ViewState["Visiblity"]));
                return true;
            }
            set
            {
                ViewState["Visiblity"] = value;
            }
        }

        public String ControlUseType
        {
            get
            {
                if (ViewState["ControlUseType"] != null)
                    return (Convert.ToString(ViewState["ControlUseType"]));
                return String.Empty;
            }
            set
            {
                ViewState["ControlUseType"] = value;
            }
        }

        public Int32 TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantid;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// UAT 1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
        /// </summary>
        public Int32 OrgUsrID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                }
                else
                {
                    return CurrentLoggedInUserId;
                }
            }
        }

        //to check whether applicant has complio buisness channel or not.
        public Boolean IsComplioBuisnessChannelTypeAvlbl
        {
            get;
            set;
        }

        public List<ApplicantRequiredDocumentsContract> lstApplicantRequiredDocumentsContract
        { get; set; }

        #region UAT-2306
        //Applicantid Get and Set when screen opens from Admin(Applicant View from Compliance Search Screen)
        public Int32 FromAdminApplicantID
        {
            get
            {
                if (ViewState["FromAdminApplicantID"] != null)
                    return (Convert.ToInt32(ViewState["FromAdminApplicantID"]));
                return AppConsts.NONE;
            }
            set
            {
                ViewState["FromAdminApplicantID"] = value;
            }
        }

        //ApplicantTenantid Get and Set when screen opens from Admin(Applicant View from Compliance Search Screen)
        public Int32 FromAdminTenantID
        {
            get
            {
                if (ViewState["FromAdminTenantID"] != null)
                    return (Convert.ToInt32(ViewState["FromAdminTenantID"]));
                return AppConsts.NONE;
            }
            set
            {
                ViewState["FromAdminTenantID"] = value;
            }
        }

        //Get and Set IsAdminView from Compliance Search Screen
        public Boolean IsAdminView
        {
            get
            {
                if (ViewState["IsAdminView"] != null)
                    return (Convert.ToBoolean(ViewState["IsAdminView"]));
                return false;
            }
            set
            {
                ViewState["IsAdminView"] = value;
            }
        }
        #endregion

        #region UAT-3161

        public List<ApplicantRequiredDocumentsContract> lstApplicantRotReqdDocumentsContract { get; set; }

        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tlstRotationReqDocuments.Rebind();
                treeListRequiredDocuments.Rebind();
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

        protected void treeListRequiredDocuments_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetRequirementDocumentsDetails();
                ShowHideControls();
                treeListRequiredDocuments.DataSource = lstApplicantRequiredDocumentsContract;
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

        protected void treeListRequiredDocuments_PreRender(object sender, EventArgs e)
        {
            try
            {
                treeListRequiredDocuments.ExpandAllItems();
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

        protected void treeListRequiredDocuments_ItemCreated(object sender, Telerik.Web.UI.TreeListItemCreatedEventArgs e)
        {
            try
            {
                // Below code disables the expand button.
                if (e.Item is TreeListDataItem)
                {
                    Control expandButton = e.Item.FindControl("ExpandCollapseButton");
                    if (expandButton != null)
                    {
                        ((Button)expandButton).Enabled = false;
                        ((Button)expandButton).Visible = false;
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

        protected void treeListRequiredDocuments_ItemDataBound(object sender, TreeListItemDataBoundEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(TreeListItemType.Item) || e.Item.ItemType.Equals(TreeListItemType.AlternatingItem))
                {
                    Int32 DataID = Convert.ToInt32((e.Item as TreeListDataItem).GetDataKeyValue("DataID"));
                    TreeListDataItem item = (TreeListDataItem)e.Item;
                    ApplicantRequiredDocumentsContract reqDocContract = (ApplicantRequiredDocumentsContract)item.DataItem;
                    //LinkButton lnkVisitRequiredDocument = (e.Item.FindControl("lnkVisitRequiredDocument") as LinkButton);
                    //lnkVisitRequiredDocument.OnClientClick = "VisitRequiredDocument('" + reqDocContract.NavigationURL + "');";
                    //lnkVisitRequiredDocument.Attributes.Add("onclick", "return false;");
                    //lnkVisitRequiredDocument.Visible = reqDocContract.ParentID == AppConsts.NONE ? false : (reqDocContract.NavigationURL.IsNullOrEmpty() ? false : true);

                    System.Web.UI.HtmlControls.HtmlGenericControl divLnkVisitRequiredDocument = (e.Item.FindControl("divLnkVisitRequiredDocument") as System.Web.UI.HtmlControls.HtmlGenericControl);
                    divLnkVisitRequiredDocument.Visible = reqDocContract.NavigationURL == string.Empty ? false : (reqDocContract.NavigationURL.IsNullOrEmpty() ? false : true);
                    divLnkVisitRequiredDocument.InnerHtml = CreateDocumentUrlLink(reqDocContract.NavigationURL, reqDocContract.NavigationLabel);

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

        private string CreateDocumentUrlLink(string sampleDocFormURL, string sampleDocFormLabel)
        {
            string[] sampleDocFormURLs = sampleDocFormURL.Split(',');
            string[] sampleDocFormLabels = sampleDocFormLabel.Split(new string[] { "##|| " }, StringSplitOptions.None);
            string sampleDocLink = string.Empty;
            int i = 0;
            foreach (var sampleDocURL in sampleDocFormURLs)
            {
                //Only display hyperlink if sampleDocFromUrl available 
                if (!sampleDocURL.IsNullOrEmpty())
                {
                    if (sampleDocFormLabels[i].IsNullOrEmpty())
                        sampleDocFormLabels[i] = "More Information";

                    sampleDocLink += "<a href=\"" + sampleDocURL + "\" onclick=\"\" target=\"_blank\");'>" + sampleDocFormLabels[i].Trim() + "</a></br>";
                    if(i < sampleDocFormLabels.Length-1)
                    i++;
                }
            }
            return sampleDocLink;
        }
        //Commented Method in UAT-3161.
        //private void ShowHideControls()
        //{
        //    if (lstApplicantRequiredDocumentsContract.IsNullOrEmpty())
        //    {
        //        dvMessage.Visible = true;
        //        dvRequiredDocs.Visible = false;
        //    }
        //    else
        //    {
        //        //If doucment exist
        //        dvMessage.Visible = false;
        //        dvRequiredDocs.Visible = true;
        //    }

        //}

        #region UAT-3161

        protected void tlstRotationReqDocuments_ItemCreated(object sender, TreeListItemCreatedEventArgs e)
        {
            try
            {
                // Below code disables the expand button.
                if (e.Item is TreeListDataItem)
                {
                    Control expandButton = e.Item.FindControl("ExpandCollapseButton");
                    if (expandButton != null)
                    {
                        ((Button)expandButton).Enabled = false;
                        ((Button)expandButton).Visible = false;
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

        protected void tlstRotationReqDocuments_PreRender(object sender, EventArgs e)
        {
            try
            {
                tlstRotationReqDocuments.ExpandAllItems();
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

        protected void tlstRotationReqDocuments_ItemDataBound(object sender, TreeListItemDataBoundEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(TreeListItemType.Item) || e.Item.ItemType.Equals(TreeListItemType.AlternatingItem))
                {
                    Int32 DataID = Convert.ToInt32((e.Item as TreeListDataItem).GetDataKeyValue("DataID"));
                    TreeListDataItem item = (TreeListDataItem)e.Item;
                    ApplicantRequiredDocumentsContract rotReqDocContract = (ApplicantRequiredDocumentsContract)item.DataItem;
                    //LinkButton lnkVisitRotationReqDocuments = (e.Item.FindControl("lnkVisitRotationReqDocuments") as LinkButton);
                    //lnkVisitRotationReqDocuments.OnClientClick = "VisitRotRequiredDocument('" + rotReqDocContract.NavigationURL + "');";
                    //lnkVisitRotationReqDocuments.Attributes.Add("onclick", "return false;");
                    //lnkVisitRotationReqDocuments.Visible = rotReqDocContract.ParentID == AppConsts.NONE ? false : (rotReqDocContract.NavigationURL.IsNullOrEmpty() ? false : true);
                    //lnkVisitRotationReqDocuments.Text = rotReqDocContract.NavigationLabel.IsNullOrEmpty() ? "More Information" : rotReqDocContract.NavigationLabel;


                    System.Web.UI.HtmlControls.HtmlGenericControl divLnkVisitRotRequiredDocument = (e.Item.FindControl("divLnkVisitRotRequiredDocument") as System.Web.UI.HtmlControls.HtmlGenericControl);
                    divLnkVisitRotRequiredDocument.Visible = rotReqDocContract.NavigationURL == string.Empty ? false : (rotReqDocContract.NavigationURL.IsNullOrEmpty() ? false : true);
                    divLnkVisitRotRequiredDocument.InnerHtml = CreateDocumentUrlLink(rotReqDocContract.NavigationURL, rotReqDocContract.NavigationLabel);
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

        protected void tlstRotationReqDocuments_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetRotReqDocumentsDetails();
                //ShowHideRotationControls();
                ShowHideControls();
                tlstRotationReqDocuments.DataSource = lstApplicantRotReqdDocumentsContract;
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

        //New Method to show hide controls, UAT-3161.
        private void ShowHideControls()
        {
            if (lstApplicantRequiredDocumentsContract.IsNullOrEmpty() && lstApplicantRotReqdDocumentsContract.IsNullOrEmpty())
            {
                dvMessage.Visible = true;
                dvRequiredDocs.Visible = false;
                dvRotReqdDocs.Visible = false;
            }
            else if (lstApplicantRequiredDocumentsContract.IsNullOrEmpty() && !lstApplicantRotReqdDocumentsContract.IsNullOrEmpty())
            {
                dvMessage.Visible = false;
                dvRequiredDocs.Visible = false;
                dvRotReqdDocs.Visible = true;
            }
            else if (!lstApplicantRequiredDocumentsContract.IsNullOrEmpty() && lstApplicantRotReqdDocumentsContract.IsNullOrEmpty())
            {
                dvMessage.Visible = false;
                dvRequiredDocs.Visible = true;
                dvRotReqdDocs.Visible = false;
            }
            else
            {
                //If doucment exist
                dvMessage.Visible = false;
                dvRequiredDocs.Visible = true;
                dvRotReqdDocs.Visible = true;
            }
        }
        #endregion

        //#region UAT-4254 

        //private String CreateRotRequiredDocumentUrl(string sampleDocFormURL, string sampleDocFormLabel)
        //{
        //    //LinkButton lnkVisitRotationReqDocuments = (e.Item.FindControl("lnkVisitRotationReqDocuments") as LinkButton);
        //    //lnkVisitRotationReqDocuments.OnClientClick = "VisitRotRequiredDocument('" + rotReqDocContract.NavigationURL + "');";
        //    //lnkVisitRotationReqDocuments.Attributes.Add("onclick", "return false;");
        //    //lnkVisitRotationReqDocuments.Visible = rotReqDocContract.ParentID == AppConsts.NONE ? false : (rotReqDocContract.NavigationURL.IsNullOrEmpty() ? false : true);
        //    //lnkVisitRotationReqDocuments.Text = rotReqDocContract.NavigationLabel.IsNullOrEmpty() ? "More Information" : rotReqDocContract.NavigationLabel;


        //    string[] sampleDocFormURLs = sampleDocFormURL.Split(',');
        //    string[] sampleDocFormLabels = sampleDocFormLabel.Split(new string[] { "##|| " }, StringSplitOptions.None);
        //    string sampleDocLink = string.Empty;
        //    int i = 0;
        //    foreach (var sampleDocURL in sampleDocFormURLs)
        //    {
        //        //Only display hyperlink if sampleDocFromUrl available 
        //        if (!sampleDocURL.IsNullOrEmpty())
        //        {
        //            if (sampleDocFormLabels[i].IsNullOrEmpty())
        //                sampleDocFormLabels[i] = "More Information";

        //            sampleDocLink += "<a href=\"" + sampleDocURL + "\" onclick=\"\" target=\"_blank\");'>" + sampleDocFormLabels[i].Trim() + "</a></br>";
        //            if (i < sampleDocFormLabels.Length - 1)
        //                i++;
        //        }
        //    }
        //    return sampleDocLink;
        //}
        //#endregion
    }
}