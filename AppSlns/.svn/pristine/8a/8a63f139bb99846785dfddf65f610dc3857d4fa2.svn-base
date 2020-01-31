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

namespace CoreWeb.IntsofSecurityModel.Pages
{
    public partial class AdminPortalComponentList : System.Web.UI.Page
    {
        #region Internal Class

        /// <summary>
        /// This class handles the operations related to handling user control folder listing.
        /// </summary>
        internal class SiteDataItem
        {
            #region Properties

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
            /// Gets or sets the value for path of the component.
            /// </summary>
            public String Path
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
            public SiteDataItem(Int32 id, Int32 parentId, String text, String path)
            {
                Id = id;
                ParentId = parentId;
                Text = text;
                Path = path;
            }

            #endregion
        }

        #endregion

        #region Variables

        #endregion

        #region Properties

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

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {   //this is test one.
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
            String[] returnData = CurrentWebPageFullPath.Split('/');

            if (!returnData[0].IsNullOrWhiteSpace())
            {
                CurrentWebPageFullPath = returnData[returnData.Length - AppConsts.TWO];
            }

            XmlDocument doc = new XmlDocument();
            String fileName = Server.MapPath("~/RComponentList.xml");
            Dictionary<String, String> componentList = new Dictionary<String, String>();
            if (File.Exists(fileName))
            {
                doc.Load(fileName);
                XmlNodeList nodeList = doc.DocumentElement.SelectNodes("//FeatureDetail[BusinessChannelType='" + BusinessChannelName + "']");
                foreach (XmlNode node in nodeList)
                {
                    XmlAttributeCollection nodeAttr = node.Attributes;
                    //nodeAttr = node.Attributes;
                    if (nodeAttr != null && nodeAttr.Count > AppConsts.NONE)
                        componentList.Add(nodeAttr[0].Value, nodeAttr[1].Value);
                }
            }

            Int32 currentSubFolder = AppConsts.ONE;
            Int32 currentFileNode = AppConsts.ONE;
            Int32 parentNode = AppConsts.NONE;

            siteData.Add(new SiteDataItem(currentSubFolder, parentNode, "Admin Entry Portal", String.Empty));
            parentNode = currentSubFolder;
            String fileTrack = String.Empty;

            if (componentList != null && componentList.Count > AppConsts.NONE)
            {
                foreach (KeyValuePair<String, String> component in componentList)
                {
                    currentFileNode++;

                    siteData.Add(component.Key.Equals(CurrentWebPageName) && component.Value.Equals(CurrentWebPageFullPath, StringComparison.InvariantCultureIgnoreCase)
                                    ? new SiteDataItem(currentFileNode
                                                       , currentSubFolder
                                                       , radiobutton + " checked=\"checked\" value= \"" + component.Key + "\"  textid='" + currentFileNode + "' ComponentPath=\"" + component.Value + "\"" + "/>" + component.Key
                                                       , component.Value)
                                    : new SiteDataItem(currentFileNode
                                                       , currentSubFolder
                                                       , radiobutton + " value= \"" + component.Key + "\" textid='" + currentFileNode + "' ComponentPath=\"" + component.Value + "\"" + "/>" + component.Key
                                                       , component.Value));

                    if (component.Key.Equals(CurrentWebPageName))
                    {
                        fileTrack = component.Key + "," + component.Value;
                    }
                }
            }

            IEnumerable<SiteDataItem> stdata = siteData.Where(fx => CurrentWebPageName.Length > 0 && fx.Text.Contains(CurrentWebPageName));

            treeView.DataTextField = "Text";
            treeView.DataValueField = "ID";
            treeView.DataFieldID = "ID";
            treeView.DataFieldParentID = "ParentID";
            //treeView.DataNavigateUrlField = "Path";
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
            }
        }

        #endregion

        #endregion
    }
}