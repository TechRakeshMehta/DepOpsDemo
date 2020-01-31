#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXSiteMapProvider.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using INTSOF.Utils.Consts;
using Business.RepoManagers;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using INTSOF.Contracts;
using CoreWeb.IntsofLoggerModel.Interface;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
using System.Resources;
using System.Globalization;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Providers
{
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    /// <remarks></remarks>
    public class SysXSiteMapProvider : StaticSiteMapProvider
    {
        #region Constructors

        #endregion

        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private static ISysXSessionService _sysXSessionService;

        private static ISysXSecurityService _sysXSecurityService;

        private readonly object _siteMapLock = new object();

        private Boolean _mIsInitialized;

        //private ISiteMapBuilderService _mNavigationService;

        private SiteMapNode _rootNode;

        private Dictionary<String, SysXSiteMapNode> _allNodes = new Dictionary<String, SysXSiteMapNode>();

        SysXSiteMapNode newParentNode;

        private String _sysXAdminRoleName = SecurityManager.GetSysXConfigValue(SysXSecurityConst.SYSX_ADMIN_ROLE_KEY_NAME);

        private ISysXLoggerService _sysXLoggerService;
        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Get or Set SysXNavigationServcie
        /// </summary>
        /// <value>The navigation service.</value>
        /// <remarks></remarks>
        //public ISiteMapBuilderService NavigationService
        //{
        //    get
        //    {
        //        if (_mNavigationService.IsNull())
        //        {
        //            WebClientApplication applicationInstance = (WebClientApplication)HttpContext.Current.ApplicationInstance;
        //            CompositionContainer rootContainer = applicationInstance.RootContainer;
        //            _mNavigationService = rootContainer.Services.Get<ISiteMapBuilderService>(true);
        //        }

        //        return _mNavigationService;
        //    }
        //    set
        //    {
        //        _mNavigationService = value;
        //    }
        //}

        #endregion

        #region Private Properties
        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Private Methods

        private Boolean EncodeUrlIfNecessary(SiteMapNode node)
        {
            if (Uri.IsWellFormedUriString(node.Url, UriKind.Absolute))
            {
                node.Url = HttpContext.Current.Server.UrlEncode(node.Url);
                return true;
            }
            return false;
        }

        //below commented code will be remove once the menu refresh and rebuild issue has been QA certified
        //private SysXSiteMapNode CreateSiteMapNode(SysXSiteMapNodeInfo nodeDetail)
        //{
        //    SysXSiteMapNode parentNode = new SysXSiteMapNode(SiteMap.Provider, nodeDetail.Key, nodeDetail.Title,
        //                                                     nodeDetail.Url, nodeDetail.Description, nodeDetail.Icon,
        //                                                     nodeDetail.UIControlId, nodeDetail.Roles,
        //                                                     nodeDetail.Attributes,
        //                                                     nodeDetail.ExplicitResourcesKey,
        //                                                     nodeDetail.ImplicitResourceKey)
        //                                     {
        //                                         ProductFeatureId = nodeDetail.ProductFeatureId,
        //                                         UIControlId = nodeDetail.UIControlId
        //                                     };

        //    if (!String.IsNullOrEmpty(nodeDetail.Icon))
        //    {
        //        parentNode["Icon"] = nodeDetail.Icon;
        //    }

        //    if (nodeDetail.HasChildNodes)
        //    {
        //        foreach (SysXSiteMapNode node in nodeDetail.ChildNodes.Select(detail => CreateSiteMapNode1(detail)))
        //        {
        //            AddNode(node, parentNode);
        //        }
        //    }

        //    return parentNode;
        //}

        private SysXSiteMapNode CreateSiteMapNode(ProductFeature productFeature)
        {
            try
            {
                //List<String> roles = ((SysXNavigationService)NavigationService)._navigationManager.GetProductFeatureRoles(productFeature.ProductFeatureID);               
                List<RolePermissionProductFeature> roleFeatures = SecurityManager.GetProductFeatureRoles(productFeature.ProductFeatureID).ToList();
                List<String> roles = (from role in roleFeatures
                                      select role.RoleDetail.Name).ToList();

                SysXSiteMapNode parentNode = new SysXSiteMapNode(SiteMap.Provider, productFeature.ProductFeatureID.ToString(), productFeature.Name,
                                                                          productFeature.NavigationURL, productFeature.Description, productFeature.IconImageName,
                                                                          productFeature.UIControlID, roles,
                                                                          null,
                                                                          null,
                                                                          "")
                {
                    ProductFeatureId = productFeature.ProductFeatureID,
                    UIControlId = productFeature.UIControlID,
                    DisplayOrder = productFeature.DisplayOrder
                };
                return parentNode;
            }
            catch (SysXException ex)
            {
                LoggerService.GetLogger().Error(SysXUtils.GetMessage("SysXSiteMapProvider CreateSiteMapNode"), ex);
                return null;
            }
        }

        private void BuildChildSiteMapNodes(SysXSiteMapNode siteMapNode, ProductFeature productFeature)
        {
            try
            {
                foreach (ProductFeature childProductFeature in productFeature.ProductFeature1)
                {
                    if (childProductFeature.IsDeleted.Equals(true))
                    {
                        continue;
                    }

                    SysXSiteMapNode childSiteMapNode = CreateSiteMapNode(childProductFeature);
                    AddSiteMapNode(childSiteMapNode, siteMapNode.Key);

                    if (!childProductFeature.ProductFeature1.IsNull() && childProductFeature.ProductFeature1.Count > AppConsts.NONE)
                    {
                        BuildChildSiteMapNodes(childSiteMapNode, childProductFeature);
                    }
                }
            }
            catch (SysXException ex)
            {
                LoggerService.GetLogger().Error(SysXUtils.GetMessage("SysXSiteMapProvider BuildChildSiteMapNodes"), ex);
            }
        }

        private void AddNodes(SysXSiteMapNode node)
        {
            if (node.IsNullOrEmpty())
                return;

            if (node.sysXHasChildNodes)
            {
                for (Int32 index = AppConsts.NONE; index < node.SysXChildNodes.Count; index++)
                {
                    if (node.SysXChildNodes[index].sysXHasChildNodes)
                    {
                        AddNodes(node.SysXChildNodes[index]);
                    }
                    AddNode(node.SysXChildNodes[index], node);
                }
            }
        }

        private void AddSiteMapNode(SysXSiteMapNode node, String parentKey)
        {
            try
            {
                if (parentKey.IsNullOrEmpty())
                {
                    AddNode(node, _rootNode);
                    _allNodes.Add(node.Key, node);
                }
                else
                {
                    if (_allNodes.ContainsKey(parentKey))
                    {
                        newParentNode = _allNodes[parentKey];

                        if (newParentNode.IsNullOrEmpty())
                            return;

                        AddNode(node, newParentNode);
                        //commented the below as we have ordered the features by displayorder using a filter, this avoid the below loop

                        //if (newParentNode.SysXChildNodes.Count > AppConsts.NONE)
                        //{
                        //    //Add at specific location
                        //    for (Int32 index = AppConsts.NONE; index < newParentNode.SysXChildNodes.Count; index++)
                        //    {
                        //        SysXSiteMapNode tmpNode = newParentNode.SysXChildNodes[index];

                        //        if (tmpNode.DisplayOrder > node.DisplayOrder)
                        //        {
                        //            newParentNode.SysXChildNodes.Insert(index, node);
                        //            break;
                        //        }
                        //        if (index == newParentNode.SysXChildNodes.Count - AppConsts.ONE)
                        //        {
                        //            newParentNode.SysXChildNodes.Add(node);
                        //            break;
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    newParentNode.SysXChildNodes.Add(node);

                        //}
                        _allNodes.Add(node.Key, node);
                    }
                    else
                    {
                        throw new SysXException(SysXUtils.GetMessage(ResourceConst.SECURITY_PARENTNODE));
                    }
                }
            }
            catch (SysXException ex)
            {
                LoggerService.GetLogger().Error(SysXUtils.GetMessage("SysXSiteMapProvider AddSiteMapNode"), ex);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Adds the node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <remarks></remarks>
        protected override void AddNode(SiteMapNode node)
        {
            try
            {
                Boolean flag = EncodeUrlIfNecessary(node);
                base.AddNode(node);

                if (flag)
                {
                    node.Url = HttpContext.Current.Server.UrlDecode(node.Url);
                }
            }
            catch (SysXException ex)
            {
                LoggerService.GetLogger().Error(SysXUtils.GetMessage(ResourceConst.SECURITY_UNABLE_TOADD_CHILD_SITEMAP_NODE) + node.Key, ex);
            }
        }

        /// <summary>
        /// Adds the node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="parentNode">The parent node.</param>
        /// <remarks></remarks>
        protected override void AddNode(SiteMapNode node, SiteMapNode parentNode)
        {
            try
            {
                Boolean flag = EncodeUrlIfNecessary(node);
                base.AddNode(node, parentNode);

                if (flag)
                {
                    node.Url = HttpContext.Current.Server.UrlDecode(node.Url);
                }
            }
            catch (SysXException ex)
            {
                LoggerService.GetLogger().Error(SysXUtils.GetMessage(ResourceConst.SECURITY_UNABLE_TOADD_CHILD_SITEMAP_NODE) + node.Key + SysXUtils.GetMessage(ResourceConst.SECURITY_TO_SITEMAP_NODE) + parentNode.Key, ex);
            }
        }

        /// <summary>
        /// Gets the root node core.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        protected override SiteMapNode GetRootNodeCore()
        {
            return BuildSiteMap();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get instance of ISysXLoggerService
        /// </summary>
        /// <remarks></remarks>
        private ISysXLoggerService LoggerService
        {
            get
            {
                if (_sysXLoggerService.IsNull())
                {
                    _sysXLoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;

                }

                return _sysXLoggerService;
            }
        }

        /// <summary>
        /// Get instance of ISysXSessionService
        /// </summary>
        /// <remarks></remarks>
        public static ISysXSessionService SessionService
        {
            get
            {
                if (_sysXSessionService.IsNull())
                {
                    _sysXSessionService = (HttpContext.Current.ApplicationInstance as IWebApplication).SysXSessionService;
                }

                return _sysXSessionService;
            }
        }

        /// <summary>
        /// Get instance of ISysXSecurityService
        /// </summary>
        /// <remarks></remarks>
        public static ISysXSecurityService SecurityService
        {
            get
            {
                if (_sysXSecurityService.IsNull())
                {
                    _sysXSecurityService = (HttpContext.Current.ApplicationInstance as IWebApplication).SysXSecurityService;

                }

                return _sysXSecurityService;
            }
        }

        /// <summary>
        /// Builds the site map.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override SiteMapNode BuildSiteMap()
        {
            if (!_mIsInitialized)
            {
                lock (_siteMapLock)
                {
                    try
                    {
                        //List<ProductFeature> productFeatures = ((SysXNavigationService)NavigationService)._navigationManager.GetProductFeatures().OrderBy(condition => condition.DisplayOrder).ToList();
                        List<ProductFeature> productFeatures = SecurityManager.GetProductFeatures().Where(pfs => pfs.IsExplicitFeature == false).OrderBy(condition => condition.DisplayOrder).ToList();
                        //retrieving only top menus by ignoring the Security
                        List<ProductFeature> rootProductFeature = productFeatures.Where(pf => pf.ProductFeature2.IsNull() && (!pf.ProductFeatureID.Equals(1))).OrderBy(condition => condition.DisplayOrder).ToList();
                        _rootNode = new SiteMapNode(this, SysXSecurityConst.SITEMAP_ROOT_NODE_KEY, SysXSecurityConst.SITEMAP_ROOT_NODE_URL);
                        //below commented code will be remove once the menu refresh and rebuild issue has been QA certified
                        //ICollection<SysXSiteMapNodeInfo> siteMapNodeInfo = ((SysXNavigationService)NavigationService).SiteMapNodes;
                        _allNodes.Clear();
                        base.Clear();

                        foreach (ProductFeature productFeature in rootProductFeature)
                        {
                            SysXSiteMapNode node = CreateSiteMapNode(productFeature);
                            AddSiteMapNode(node, string.Empty);
                            BuildChildSiteMapNodes(node, productFeature);
                        }

                        //below commented code will be remove once the menu refresh and rebuild issue has been QA certified
                        //foreach (SysXSiteMapNodeInfo detail in siteMapNodeInfo)
                        //{
                        //    SysXSiteMapNode node = CreateSiteMapNode1(detail);
                        //    AddNode(node, _rootNode);
                        //}                        
                        _mIsInitialized = true;
                    }
                    catch (SysXException ex)
                    {
                        LoggerService.GetLogger().Error(SysXUtils.GetMessage("SysXSiteMapProvider " + ResourceConst.SECURITY_UNABLE_TO_BUILD_SITEMAP), ex);
                    }
                }
            }

            return _rootNode;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        /// <remarks></remarks>
        protected override void Clear()
        {
            base.Clear();
            _mIsInitialized = false;
        }

        /// <summary>
        /// Refreshes the site map.
        /// </summary>
        /// <remarks></remarks>
        public void RefreshSiteMap()
        {
            Clear();
        }

        /// <summary>
        /// Determines whether [is accessible to user] [the specified context].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="node">The node.</param>
        /// <returns><c>true</c> if [is accessible to user] [the specified context]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
        {
            if (node.IsNull())
            {
                throw new ArgumentNullException("node");
            }

            if (context.IsNull())
            {
                throw new ArgumentNullException("context");
            }

            if (!base.SecurityTrimmingEnabled)
            {
                return true;
            }

            if (node.Key.Equals(SysXSecurityConst.SITEMAP_ROOT_NODE_KEY))
            {
                return true;
            }

            if (!SecurityTrimmingEnabled)
            {
                return true;
            }

            //Allow SysX Admin Role for everything
            if (context.User.IsInRole(_sysXAdminRoleName))
            {
                return true;
            }

            //If there are no roles defined for the page
            //return true or false depending on your authorization scheme (when true pages with
            //no roles are visible to all users, when false no user can access these pages)
            if (node.Roles.IsNull())
            {
                return false;
            }

            return context.Session != null ? SecurityService.IsFeatureAvailableToUser(context.User.Identity.Name, ((SysXSiteMapNode)node).ProductFeatureId, SessionService.SysXBlockId) : node.Roles.Cast<String>().Any(role => context.User.IsInRole(role));
            //checks each role, if the user is in any of the roles return true
        }

        /// <summary>
        /// Finds the site map node.
        /// </summary>
        /// <param name="rawUrl">The raw URL.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override SiteMapNode FindSiteMapNode(String rawUrl)
        {
            return base.FindSiteMapNode(RemoveQuery(rawUrl));
        }

        /// <summary>
        /// Removes the query.
        /// </summary>
        /// <param name="rawUrl">The raw URL.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private String RemoveQuery(String rawUrl)
        {
            Int32 pos = rawUrl.IndexOf('?');

            if (pos > AppConsts.NONE)
            {
                Int32 ucIdPos = rawUrl.IndexOf("ucid");

                if (ucIdPos > AppConsts.NONE)
                {
                    return rawUrl.IndexOf("&", ucIdPos) > AppConsts.NONE ? rawUrl.Substring(AppConsts.NONE, rawUrl.IndexOf("&", ucIdPos)) : rawUrl;
                }
                return rawUrl.Substring(AppConsts.NONE, pos);
            }

            return rawUrl;
        }

        #region breadcrumb

        /// <summary>
        /// Adds the bread crumb node.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="nodeURL">The node URL.</param>
        /// <remarks></remarks>
        public static void AddBreadCrumbNode(String nodeName, String nodeURL)
        {
            List<BreadCrumbNode> nodeList;

            if (!HttpContext.Current.Session["BreadCrumb"].IsNull())
            {
                nodeList = (List<BreadCrumbNode>)HttpContext.Current.Session["BreadCrumb"];
                BreadCrumbNode currentNode = nodeList.Find(temp =>
                {
                    if (temp.NodeKey.IsNullOrEmpty())
                        return temp.NodeName == nodeName;
                    else return temp.NodeKey == nodeName;
                });

                //node already exist in list.. delete all levels after currentNode.
                if (!currentNode.NodeURL.IsNull())
                {
                    RemoveNodes(currentNode.Level + AppConsts.ONE, nodeList);
                }
                else
                {
                    BreadCrumbNode nod = new BreadCrumbNode { NodeName = nodeName, NodeURL = nodeURL };
                    Int32 lastLevel = AppConsts.NONE;

                    foreach (BreadCrumbNode item in nodeList)
                    {
                        lastLevel = item.Level;
                    }

                    nod.Level = lastLevel + AppConsts.ONE;
                    nodeList.Add(nod);

                    HttpContext.Current.Session["BreadCrumb"] = nodeList;
                }
            }
            else
            {
                nodeList = new List<BreadCrumbNode>();
                BreadCrumbNode breadCrumbNode = new BreadCrumbNode { NodeName = nodeName, NodeURL = nodeURL, Level = AppConsts.TWO };
                nodeList.Add(breadCrumbNode);
                HttpContext.Current.Session["BreadCrumb"] = nodeList;
            }
        }

        /// <summary>
        /// Removes the nodes.
        /// </summary>
        /// <param name="currentlevel">The current level.</param>
        /// <param name="nodes">The nodes.</param>
        /// <remarks></remarks>
        private static void RemoveNodes(Int32 currentlevel, List<BreadCrumbNode> nodes)
        {
            BreadCrumbNode node = nodes.Find(nodeInfo => nodeInfo.Level == currentlevel);

            if (node.NodeURL.IsNull())
            {
                return;
            }

            //currentlevel = node.Level;
            nodes.Remove(node);
            currentlevel = currentlevel + 1;
            RemoveNodes(currentlevel, nodes);
        }

        public static void AddAppBreadCrumbNode(String nodeName, String nodeURL, String viewPathName)
        {
            List<AppBreadCrumbNode> nodeList;

            if (!HttpContext.Current.Session["BreadCrumb"].IsNull())
            {
                nodeList = (List<AppBreadCrumbNode>)HttpContext.Current.Session["BreadCrumb"];
                AppBreadCrumbNode currentNode = nodeList.Find(temp => temp.ViewPathName == viewPathName.ToLower() || temp.NodeName == nodeName);

                //node already exist in list.. delete all levels after currentNode.
                if (!currentNode.ViewPathName.IsNull())
                {
                    RemoveAppNodes(currentNode.Level + AppConsts.ONE, nodeList);
                }
                else
                {
                    AppBreadCrumbNode nod = new AppBreadCrumbNode
                    {
                        NodeName = nodeName,
                        NodeURL = nodeURL,
                        ViewPathName = viewPathName == String.Empty ? String.Empty : viewPathName.ToLower()
                    };
                    var orderedNodeList = nodeList.OrderBy(cond => cond.Level).ToList();
                    Int32 lastLevel = AppConsts.NONE;
                    foreach (AppBreadCrumbNode item in orderedNodeList)
                    {
                        lastLevel = item.Level;
                    }

                    nod.Level = lastLevel + AppConsts.ONE;
                    nodeList.Add(nod);

                    HttpContext.Current.Session["BreadCrumb"] = nodeList;
                }
            }
            else
            {
                nodeList = new List<AppBreadCrumbNode>();
                AppBreadCrumbNode breadCrumbNode = new AppBreadCrumbNode
                {
                    NodeName = nodeName,
                    NodeURL = nodeURL,
                    Level = AppConsts.TWO,
                    ViewPathName = viewPathName == String.Empty ? String.Empty : viewPathName.ToLower()
                };
                nodeList.Add(breadCrumbNode);
                HttpContext.Current.Session["BreadCrumb"] = nodeList;
            }
        }

        private static void RemoveAppNodes(Int32 currentlevel, List<AppBreadCrumbNode> nodes)
        {
            AppBreadCrumbNode node = nodes.Find(nodeInfo => nodeInfo.Level == currentlevel);

            if (node.NodeURL.IsNull())
            {
                return;
            }

            currentlevel = node.Level;
            nodes.Remove(node);
            RemoveAppNodes(++currentlevel, nodes);
        }
        #endregion

        #endregion

        #endregion
    }

    /// <summary>
    /// This structure handles breadcrumb.
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public struct BreadCrumbNode
    {
        /// <summary>
        /// Handles Level.
        /// </summary>
        public Int32 Level;
        private String _NodeName;
        public String NodeKey { get { return _nodekey; } }
        private String _nodekey;
        private Boolean IsKeyAdded { get; set; }

        /// <summary>
        /// Handles NodeName.
        /// </summary>
        public String NodeName
        {
            set
            {
                if (!value.IsNullOrEmpty() && value.StartsWith("Key_"))
                {

                    _NodeName = value.Substring("Key_".Length);
                    IsKeyAdded = true;
                }
                else { _NodeName = value; IsKeyAdded = false; }
                _nodekey = value;
            }
            get
            {
                if (IsKeyAdded)
                {
                    INTSOF.UI.Contract.Globalization.LanguageContract currentLanguage = new INTSOF.UI.Contract.Globalization.LanguageContract();
                    if (!HttpContext.Current.Session["LanguageCulture"].IsNullOrEmpty())
                    {
                         currentLanguage = (INTSOF.UI.Contract.Globalization.LanguageContract)(HttpContext.Current.Session["LanguageCulture"]);
                        //if()language=spanish
                    }
                    //if (currentLanguage.LanguageCulture == LanguageCultures.SPANISH_CULTURE.GetStringValue())
                    //{
                    String _cultureInfo = !currentLanguage.IsNullOrEmpty() && !currentLanguage.LanguageCulture.IsNullOrEmpty() ? currentLanguage.LanguageCulture:LanguageCultures.ENGLISH_CULTURE.GetStringValue();
                    CultureInfo cultureInfo = new CultureInfo(_cultureInfo);
                    ResourceManager rm = new ResourceManager("Resources.Language", global::System.Reflection.Assembly.Load("App_GlobalResources"));
                    try
                    {
                        return rm.GetString(_NodeName, cultureInfo);
                    }
                    catch
                    {
                        return _NodeName;
                    }
                    //}
                    //else
                    //{
                    //    return _NodeName;
                    //}
                    //return from resourcefile(_NodeName)
                    //elsereturn _NodeName;
                }
                else
                {
                    return _NodeName;
                }
            }
        }

        /// <summary>
        /// Handles NodeURL.
        /// </summary>
        public String NodeURL;
    }

    /// <summary>
    /// This structure handles BreadCrumb Nodes for the Application
    /// </summary>
    [Serializable]
    public struct AppBreadCrumbNode
    {
        /// <summary>
        /// Handles Level.
        /// </summary>
        public Int32 Level;

        /// <summary>
        /// Handles NodeName.
        /// </summary>
        public String NodeName;

        /// <summary>
        /// Handles NodeURL.
        /// </summary>
        public String NodeURL;

        /// <summary>
        /// Handles the View Path Name
        /// </summary>
        public String ViewPathName;
    }
}