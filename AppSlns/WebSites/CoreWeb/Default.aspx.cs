#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  Default.aspx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;

#endregion

#region Application Specific

using INTSOF.Utils;
using CoreWeb.Shell;
using CoreWeb.Shell.Views;
using CoreWeb.Shell.MasterPages;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using System.Web.Services;
using Telerik.Web.UI;
using Business.RepoManagers;
using Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Text;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Collections.Specialized;
using System.Web;
using System.Configuration;
using CoreWeb;

#endregion

#endregion

/// <summary>
/// This class handles the operations default page.
/// </summary>
public partial class ShellDefault : System.Web.UI.Page, IDefaultView
{
    #region Variables

    #region Public Variables


    public DefaultViewPresenter Presenter
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

    #region Private Variables

    private DefaultViewPresenter _presenter = new DefaultViewPresenter();
    private ISysXSessionService _sessionService = SysXWebSiteUtils.SessionService;

    #endregion

    #endregion

    #region Properties
    private SysXPageViewStatePersister _pageStatePersister;
    #endregion

    #region Events

    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            if (_pageStatePersister == null)
            {
                _pageStatePersister = new SysXPageViewStatePersister(this);
            }
            return _pageStatePersister;
        }
    }

    /// <summary>
    /// Handles the operation related to SavePageStateToPersistenceMedium.
    /// </summary>
    /// <param name="state">value for state.</param>
    //protected override void SavePageStateToPersistenceMedium(Object state)
    //{
    //    LosFormatter losFormatter = new LosFormatter();
    //    StringWriter stringWriter = new StringWriter();

    //    losFormatter.Serialize(stringWriter, state);
    //    var vsPageState = Convert.ToString(stringWriter);
    //    var bytesViewState = Convert.FromBase64String(vsPageState);
    //    bytesViewState = _sessionService.CompressViewState(bytesViewState);
    //    Session[Session.SessionID] = Convert.ToBase64String(bytesViewState);
    //}

    /// <summary>
    /// Handles the operations related to LoadPageStateFromPersistenceMedium.
    /// </summary>
    /// <returns>
    /// The page state from persistence medium.
    /// </returns>
    //protected override Object LoadPageStateFromPersistenceMedium()
    //{
    //    Byte[] bytes = Convert.FromBase64String((String)Session[Session.SessionID]);
    //    bytes = _sessionService.DecompressViewState(bytes);
    //    LosFormatter formatter = new LosFormatter();

    //    return formatter.Deserialize(Convert.ToBase64String(bytes));
    //}

    /// <summary>
    /// Raises the initialize complete event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
    protected override void OnInitComplete(EventArgs e)
    {
        if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
            {
                queryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
            }
            if (queryString.ContainsKey(SysXSearchConsts.VIEW_MODE))
            {
                ViewMode pageView = (ViewMode)Enum.Parse(typeof(ViewMode), queryString[SysXSearchConsts.VIEW_MODE], true);
                UserControl userControl = null;

                switch (pageView)
                {
                    case ViewMode.Search:
                        if (queryString.ContainsKey(SysXSearchConsts.SEARCH_MODE))
                        {
                            SearchViewMode viewMode = (SearchViewMode)Enum.Parse(typeof(SearchViewMode), queryString[SysXSearchConsts.SEARCH_MODE], true);

                            switch (viewMode)
                            {
                                case SearchViewMode.QuickSearch: //Load Quick Search Result
                                    userControl = (UserControl)LoadControl("~/shared/controls/SearchResult.ascx");
                                    userControl.ID = "ucSearchResults";
                                    plcDynamic.Controls.Add(userControl);
                                    break;
                                case SearchViewMode.AdvanceSearch: // Load Respective Advance Search with Search Result if any
                                    QuickSearchOption quickSearchOption = (QuickSearchOption)Enum.Parse(typeof(QuickSearchOption), queryString[SysXSearchConsts.QUICK_SEARCH_OPTION], true);

                                    switch (quickSearchOption)
                                    {
                                        case QuickSearchOption.Asset:
                                            userControl = (UserControl)LoadControl("~/shared/controls/AssetSearch.ascx");
                                            break;
                                        case QuickSearchOption.Client:
                                            userControl = (UserControl)LoadControl("~/shared/controls/ClientSearch.ascx");
                                            break;
                                        case QuickSearchOption.Supplier:
                                            userControl = (UserControl)LoadControl("~/shared/controls/SupplierSearch.ascx");
                                            break;
                                        case QuickSearchOption.User:
                                            userControl = (UserControl)LoadControl("~/shared/controls/UserSearch.ascx");
                                            break;
                                        case QuickSearchOption.Investor:
                                            userControl = (UserControl)LoadControl("~/shared/controls/InvestorSearch.ascx");
                                            break;
                                        case QuickSearchOption.Insurer:
                                            userControl = (UserControl)LoadControl("~/shared/controls/InsurerSearch.ascx");
                                            break;
                                        case QuickSearchOption.Organization:
                                            userControl = (UserControl)LoadControl("~/shared/controls/OrganizationSearch.ascx");
                                            break;
                                        case QuickSearchOption.Product:
                                            userControl = (UserControl)LoadControl("~/shared/controls/ProductSearch.ascx");
                                            break;
                                        case QuickSearchOption.Item:
                                            userControl = (UserControl)LoadControl("~/shared/controls/ItemSearch.ascx");
                                            break;
                                        case QuickSearchOption.Employee:
                                            userControl = (UserControl)LoadControl("~/shared/controls/EmployeeSearch.ascx");
                                            break;
                                    }

                                    userControl.ID = AppConsts.UC_DYNAMIC_CONTROL;
                                    plcDynamic.Controls.Add(userControl);

                                    if (queryString.ContainsKey(SysXSearchConsts.LOAD_RESULTS)) // Load Search Result If Any
                                    {
                                        userControl = (UserControl)LoadControl("~/shared/controls/SearchResult.ascx");
                                        userControl.ID = "ucSearchResult";
                                        plcResult.Controls.Add(userControl);
                                    }
                                    break;
                            }
                        }
                        break;
                    case ViewMode.Notes:
                        if (!queryString["CurrentContext"].IsNullOrEmpty() && !queryString["NotesOwnerID"].IsNullOrEmpty())
                        {
                            userControl = (UserControl)LoadControl("~/CommonControls/NotesList.ascx");
                            userControl.ID = "ucNotes";
                            plcResult.Controls.Add(userControl);
                        }
                        else
                        {
                            (Master as ISysXDefaultMasterView).ShowErrorMessage(SysXUtils.GetMessage(ResourceConst.SECURITY_NO_NOTES_AVAILABLE));
                        }
                        break;

                    case ViewMode.Hoa:
                        userControl = (UserControl)LoadControl("~/AssetHOAFunctions/HOAManagement/HOAManagement.ascx");
                        userControl.ID = "ucHoa";
                        plcResult.Controls.Add(userControl);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Page load.
    /// </summary>
    /// <param name="sender">Sender value.</param>
    /// <param name="e">     An <see cref="T:System.EventArgs"></see> object that contains the event
    ///  data.</param>
    protected void Page_Load(Object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();

            //The Below code is commented for QA Bug-10013
            //UAT-1122
            //String args = (String)SysXWebSiteUtils.SessionService.GetCustomData("INVITE_TOKEN");
            //if (!args.IsNullOrEmpty())
            //{
            //    String url = ifrPage.Src + "?args=" + args;
            //    ifrPage.Attributes["src"] = url;
            //    //SysXWebSiteUtils.SessionService.ClearSession(false);
            //    return;
            //}

            //AD: Added code to control page refresh, Reading last URI from the session
            String uri = (String)SysXWebSiteUtils.SessionService.GetCustomData("CURRENT_URL");
            if (!String.IsNullOrWhiteSpace(uri))
            {
                ifrPage.Attributes["src"] = uri;
            }
        }
        catch (Exception ex)
        {
            ErrorLog logFile = new ErrorLog("Problem in sending data from Default page" + ex);
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get message content according to messageid, currentUserId and queuetype
    /// </summary>
    /// <param name="messageId">messageId</param>
    /// <param name="currentUserId">currentUserId</param>
    /// <param name="queueTypeID">queueTypeID</param>
    /// <returns></returns>
    [WebMethod]
    public static String GetMessageContent(String messageId, Int32 currentUserId, Int32 queueTypeID)
    {
        Guid currentMessageId = new Guid(messageId);
        ComplexObject message = MessageManager.GetCommonMessageContents(queueTypeID, currentMessageId);
        StringBuilder content = new StringBuilder();
        content.Append("<br/> ");
        content.Append("<br/> ");
        String eVaultDocumentID = String.Empty;
        String documentName = String.Empty;

        switch ((TenantTypeEnum)queueTypeID)
        {

            case TenantTypeEnum.Company:
                GetADBMessageContentResult adbMessageContent = message as GetADBMessageContentResult;
                content.Append("<br/> " + adbMessageContent.MessageBody);
                content.Append("<br/><div class='sxseparator'></div>");
                eVaultDocumentID = adbMessageContent.EVaultDocumentID;
                documentName = adbMessageContent.DocumentName;
                break;

        }
        String[] arrEVaultDocumentID = { };
        String[] arrDocumentName = { };
        arrEVaultDocumentID = eVaultDocumentID.Split(new char[] { ',' });
        arrDocumentName = documentName.Split(new char[] { ',' });
        if (!eVaultDocumentID.IsNullOrEmpty())
        {
            content.Append("<b>Attached Documents:</b><br/><br/>");

            for (Int32 i = AppConsts.NONE; i < arrEVaultDocumentID.Length; i++)
            {
                //if (isRowDoubleClick)
                //    content.Append((i + 1) + ") " + "<a href=\"" + "../../EVaultDocumentView.aspx?EvaultDocumentId=" + arrEVaultDocumentID[i]
                //         + "&DocumentName=" + arrDocumentName[i] + "\">" + arrDocumentName[i] + "</a><br/>");
                //else
                content.Append((i + 1) + ") " + "<a href=\"" + "../EVaultDocumentView.aspx?EvaultDocumentId=" + arrEVaultDocumentID[i]
                    + "&DocumentName=" + arrDocumentName[i] + "\">" + arrDocumentName[i] + "</a><br/>");
            }

        }
        return content.ToString();
    }

    // <summary>
    // Invoked to delete the message and update the respective folder.
    // </summary>
    // <param name="folderID">folderID</param>
    // <param name="messageId">messageId</param>
    // <param name="queueOwnerId">queueOwnerId</param>
    // <param name="queueTypeID">queueTypeID</param>
    // <returns></returns>
    //[WebMethod]
    //public static List<MessageDetail> DeleteMessageAndUpdateResult(Int32 startIndex, Int32 maximumRows, String sortExpressions, List<GridFilterExpression> filterExpressions, Int32 folderID, String messageId, Int32 queueOwnerId, Int32 queueTypeID)
    //public static List<MessageDetail> DeleteMessageAndUpdateResult(Int32 startIndex, Int32 maximumRows, String sortExpressions, List<GridFilterExpression> filterExpressions, Int32 folderID, String folderCode, String messageId, Int32 queueOwnerId, Int32 userGroup, Int32 queueTypeID)
    //{
    //    Guid currentMessageId = new Guid(messageId);
    //    Boolean isDeleted = MessageManager.DeleteMesssage(currentMessageId, queueOwnerId, folderCode, queueTypeID);
    //    List<MessageDetail> mails = MessageManager.GetQueue(QueueConstants.MESSAGEQUEUE, folderID, folderCode, queueTypeID, queueOwnerId, userGroup, false, DateTime.Now);
    //    List<MessageDetail> page = new List<MessageDetail>();
    //    if (!mails.IsNull() && mails.Count > 0)
    //    {
    //        if (sortExpressions.IsNullOrEmpty())
    //        {
    //            page = mails.Skip(startIndex).Take(maximumRows).ToList();
    //            page[0].TotalRecords = mails.Count;

    //        }
    //        else
    //        {
    //            page = SortedResults(startIndex, maximumRows, sortExpressions, mails);
    //        }

    //        if (filterExpressions.Count > 0)
    //        {
    //            foreach (GridFilterExpression expression in filterExpressions)
    //            {
    //                page = page.FindAll(new CoreWeb.AppUtils.GridGenericFilterer<MessageDetail>(expression).Filter);
    //            }
    //        }
    //        return page;
    //    }
    //    else
    //    {
    //        return new List<MessageDetail>();
    //    }
    //}

    public static List<MessageDetail> SortedResults(Int32 startIndex, Int32 maximumRows, String sortExpressions, List<MessageDetail> mails)
    {
        IOrderedEnumerable<MessageDetail> orderedMails = null;
        List<MessageDetail> filteredMessages = new List<MessageDetail>();
        String[] header = sortExpressions.Split(new char[] { ' ' });

        if (header[0].Equals("From", StringComparison.OrdinalIgnoreCase))
        {
            if (header[1].Equals("Desc", StringComparison.OrdinalIgnoreCase))
            {
                orderedMails = mails.OrderByDescending(md => md.From);
            }
            else
            {
                orderedMails = mails.OrderBy(md => md.From);
            }
        }
        else if (header[0].Equals("Subject", StringComparison.OrdinalIgnoreCase))
        {
            if (header[1].Equals("Desc", StringComparison.OrdinalIgnoreCase))
            {
                orderedMails = mails.OrderByDescending(md => md.Subject);
            }
            else
            {
                orderedMails = mails.OrderBy(md => md.Subject);
            }
        }
        else if (header[0].Equals("ReceivedDate", StringComparison.OrdinalIgnoreCase))
        {
            if (header[1].Equals("Desc", StringComparison.OrdinalIgnoreCase))
            {
                orderedMails = mails.OrderByDescending(md => md.ReceivedDate);
            }
            else
            {
                orderedMails = mails.OrderBy(md => md.ReceivedDate);
            }
        }
        else if (header[0].Equals("Size", StringComparison.OrdinalIgnoreCase))
        {
            if (header[1].Equals("Desc", StringComparison.OrdinalIgnoreCase))
            {
                orderedMails = mails.OrderByDescending(md => md.Size);
            }
            else
            {
                orderedMails = mails.OrderBy(md => md.Size);
            }
        }
        filteredMessages = orderedMails.Skip(startIndex).Take(maximumRows).ToList();
        filteredMessages[0].TotalRecords = mails.Count;
        return filteredMessages;
    }

    /// <summary>
    /// Invoked to add new folder for a particular user.
    /// </summary>
    /// <param name="nodeText"></param>
    /// <param name="currentUserId"></param>
    /// <returns></returns>
    //[WebMethod]
    //public static int AddNewFolder(String nodeText, Int32 currentUserId, Int32 userGroup)
    //{
    //    return MessageManager.AddNewFolder(nodeText.Trim(), currentUserId, userGroup);
    //}



    [WebMethod(EnableSession = true)]
    public static Boolean ResetSessionTimeOut()
    {
        var user = SysXWebSiteUtils.SessionService.SysXMembershipUser;
        if (user != null)
            return true;
        else
            return false;
    }

    [WebMethod(EnableSession = true)]
    public static String ClearBreadCrumb()
    {
        SysXWebSiteUtils.SessionService.SetCustomData("BreadCrumb", null);

        /*UAT-2998*/
        HttpContext.Current.Session[AppConsts.SHARED_REQUIREMENT_SHARE_SESSION_KEY] = null;
        HttpContext.Current.Session.Remove(AppConsts.SESSION_TENANTID_REQUIREMENT_SHARE);
        HttpContext.Current.Session[AppConsts.SHARED_USER_ROTATION_SESSION_KEY] = null;
        HttpContext.Current.Session[AppConsts.SESSION_TENANTID_ROTATION_SHARE] = null;
        HttpContext.Current.Session[AppConsts.SESSION_IS_RETURN_FROM_AGENCY_APPLICANT] = null;
        /*End UAT-2998*/
        return "true";
    }

    #endregion

    protected override void InitializeCulture()
    {
        Boolean isLanguageTransaltionEnable = ConfigurationManager.AppSettings["IsLanguageTranslation"].IsNullOrEmpty() ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["IsLanguageTranslation"]);
        Boolean IsLocationTenant = false;
        if (!Session["IsLocationTenant"].IsNullOrEmpty())
            IsLocationTenant = Convert.ToBoolean(SysXWebSiteUtils.SessionService.GetCustomData("IsLocationTenant"));

        if (isLanguageTransaltionEnable && IsLocationTenant)
        {
            LanguageTranslateUtils.LanguageTranslateInit();
            base.InitializeCulture();
        }
    }

    protected virtual String GetLanguageCulture()
    {
        return LanguageTranslateUtils.GetCurrentLanguageCultureFromSession();
    }
}