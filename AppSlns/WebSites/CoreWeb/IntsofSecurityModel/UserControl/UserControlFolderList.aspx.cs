#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  UserControlFolderList.aspx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;

#endregion

#region Application Specific

using INTSOF.Utils;
using Telerik.Web.UI;
using System.Xml;

#endregion

#endregion

namespace CoreWeb
{
    /// <summary>
    /// List of system x coordinate security model user control folders.
    /// </summary>
    public partial class SysXSecurityModel_UserControlFolderList : Page
    {
        #region Internal Class

        /// <summary>
        /// This class handles the operations related to handling user control folder listing.
        /// </summary>
        internal class SiteDataItem
        {
            #region Properties

            /// <summary>
            /// Gets or sets and sets the value for Text.
            /// </summary>
            /// <value>
            /// The text.
            /// </value>
            public String Text
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets and sets the value for id.
            /// </summary>
            /// <value>
            /// The identifier.
            /// </value>
            public Int32 Id
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets and sets the value for parent's id.
            /// </summary>
            /// <value>
            /// The identifier of the parent.
            /// </value>
            public Int32 ParentId
            {
                get;
                set;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Initializes the vale for the properties.
            /// </summary>
            /// <param name="id">      id.</param>
            /// <param name="parentId">parent's id value.</param>
            /// <param name="text">    text.</param>
            public SiteDataItem(Int32 id, Int32 parentId, String text)
            {
                Id = id;
                ParentId = parentId;
                Text = text;
            }

            #endregion
        }

        #endregion

        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties.

        /// <summary>
        /// Gets or sets the current web page name.
        /// </summary>
        /// <value>
        /// The name of the current web page.
        /// </value>
        public String CurrentWebPageName
        {
            get;
            set;
        }

        public String CurrentWebPageFullPath
        {
            get;
            set;
        }

        public String BusinessChannelName
        {
            get;
            set;
        }

        #endregion

        #region Events

        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString.AllKeys.Length > AppConsts.NONE)
                {
                    CurrentWebPageName = Convert.ToString(Request.QueryString["selectedWePageName"]);
                    CurrentWebPageFullPath = Convert.ToString(Request.QueryString["controlFullPath"]);
                    BusinessChannelName = Convert.ToString(Request.QueryString["businessChannel"]);
                    //CurrentWebPageName = Convert.ToString(Request.QueryString[0]);
                    BindToIEnumerable(treeControlID);
                }

            }
            catch (SysXException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        private void BindToIEnumerable(RadTreeView treeView)
        {
            const String radiobutton = "<input type=\"radio\" name=\"rdbtnControlName\" ";
            List<SiteDataItem> siteData = new List<SiteDataItem>();

            CurrentWebPageName = BaseUserControl.FormatControlName(CurrentWebPageName);
            //var data = "~/Apple/Ball/Cat/Accounting/default.aspx?ucid=AccountingQueue.ascx";
            String[] returnData = CurrentWebPageFullPath.Split('/');

            if (!returnData[0].IsNullOrWhiteSpace())
            {
                CurrentWebPageFullPath = returnData[returnData.Length - AppConsts.TWO];
            }

            String virtualPath = Server.MapPath("~");


            String[] directoriesNotToInclude = { "App_Code", "App_Data", "Bin", "App_Themes", ".svn", "App_GlobalResources", "Alerts", "AMS", "ApplicantModule", 
            "CommunicationEventMockUp", "MapForce", "Queues", "Student" };

            XmlDocument doc = new XmlDocument();
            String fileName = Server.MapPath("~/FolderList.xml");
            List<String> directoryList = new List<String>();
            if (File.Exists(fileName))
            {
                doc.Load(fileName);
                XmlNodeList commonNodes = doc.DocumentElement.SelectNodes("//FolderDetail[BusinessChannelType='Common']/@Name");
                foreach (XmlNode node in commonNodes)
                {
                    directoryList.Add(node.Value);
                }

                XmlNodeList nodeList = doc.DocumentElement.SelectNodes("//FolderDetail[BusinessChannelType='" + BusinessChannelName + "']/@Name");

                foreach (XmlNode node in nodeList)
                {
                    directoryList.Add(node.Value);
                }

            }


            DirectoryInfo directoryInfo = new DirectoryInfo(virtualPath);
            //DirectoryInfo[] subFolders = directoryInfo.GetDirectories().Where(x => !directoriesNotToInclude.Contains(x.Name)).ToArray();

            DirectoryInfo[] subFolders = directoryInfo.GetDirectories().Where(x => directoryList.Count > AppConsts.NONE ? directoryList.Contains(x.Name) : !directoriesNotToInclude.Contains(x.Name)).ToArray();

            Int32 currentSubFolder = AppConsts.ONE;
            Int32 currentFileNode = AppConsts.ONE;
            Int32 parentNode = AppConsts.NONE;






            siteData.Add(new SiteDataItem(currentSubFolder, parentNode, "American Databank"));
            parentNode = currentSubFolder;
            String[] filesNotToInclude = { "AdminExceptionItemSearch.ascx", "AssigneeExceptionItemSearch.ascx", "ChangePassword.ascx", "ForgotPassword.ascx", 
                                             "ManageGrade.ascx", "ManagePermission.ascx", "ManagePermissionType.ascx", "ManageSubClientTenant.ascx", "ManageSubTenant.ascx",
                                         "ManageUserGroup.ascx", "PolicyRegisterControlMappings.ascx", "SelectBuisnessChannel.ascx", "MessageTypeUserGroup.ascx", 
                                         "SubscriptionSetting.ascx", "SearchMain.ascx", "Markup.ascx", "WebSiteSetUp.ascx", "ManageDepartment.ascx", "ManagePrograms.ascx",
                                         "SetupRuleSet.ascx","ManageInvitationExpiration.ascx","AgencyNodeMapping.ascx","ApplicantSearch.ascx"};

            String fileTrack = String.Empty;
            foreach (DirectoryInfo subFolder in subFolders)
            {
                FileInfo[] fileInfo = subFolder.GetFiles("*.ascx").Where(x => !filesNotToInclude.Contains(x.Name)).ToArray();
                Boolean isSubFolderAdded = false;

                foreach (FileInfo file in fileInfo)
                {
                    if (!isSubFolderAdded)
                    {
                        currentSubFolder = ++currentFileNode;
                        siteData.Add(new SiteDataItem(currentSubFolder, parentNode,
                                                      BaseUserControl.FormatControlName(subFolder.Name)));

                    }

                    isSubFolderAdded = true;
                    currentFileNode++;

                    siteData.Add(BaseUserControl.FormatControlName(file.Name).Equals(CurrentWebPageName) && subFolder.Name.Equals(CurrentWebPageFullPath, StringComparison.InvariantCultureIgnoreCase)
                                     ? new SiteDataItem(currentFileNode, currentSubFolder,
                                         radiobutton + " checked=\"checked\" value= \"" +
                                         BaseUserControl.FormatControlName(file.Name) + "\"  textid='" + currentFileNode + "' />" +
                                         BaseUserControl.FormatControlName(file.Name))
                                         : new SiteDataItem(currentFileNode, currentSubFolder,
                                                        radiobutton + " value= \"" +
                                                      BaseUserControl.FormatControlName(file.Name) + "\" textid='" + currentFileNode + "' />" +
                                                        BaseUserControl.FormatControlName(file.Name)));

                    if (BaseUserControl.FormatControlName(file.Name).Equals(CurrentWebPageName))
                    {
                        fileTrack = file.Name + "," + subFolder.Name;
                    }
                }
            }

            IEnumerable<SiteDataItem> stdata = siteData.Where(fx => CurrentWebPageName.Length > 0 && fx.Text.Contains(CurrentWebPageName));

            treeView.Attributes.Add("SkipTreeViewEncoding", "true");
            treeView.DataTextField = "Text";
            treeView.DataValueField = "ID";
            treeView.DataFieldID = "ID";
            treeView.DataFieldParentID = "ParentID";
            treeView.DataSource = siteData;
            treeView.DataBind();

            if (!stdata.IsNull())
            {
                foreach (var siteDataItem in stdata)
                {
                    if (!treeView.FindNodeByText(siteDataItem.Text).IsNull() && Regex.Replace(treeView.FindNodeByText(siteDataItem.Text).ParentNode.Text, @"\s+", "").Equals(CurrentWebPageFullPath, StringComparison.InvariantCultureIgnoreCase))
                    {
                        treeView.FindNodeByText(siteDataItem.Text).Selected = true;
                        hdnCurrentNode.Value = fileTrack;
                    }
                }
                //RadTreeNode radTreeNode = treeView.FindNodeByText(stdata.Text);
                //RadTreeNode radParentTreeNode = radTreeNode.ParentNode;

                //if (!radTreeNode.IsNull() && radParentTreeNode.Text.Equals(CurrentWebPageFullPath))
                //{
                //    radTreeNode.Selected = true;
                //    hdnCurrentNode.Value = fileTrack;
                //}
            }
        }

        #endregion

        #endregion
    }
}