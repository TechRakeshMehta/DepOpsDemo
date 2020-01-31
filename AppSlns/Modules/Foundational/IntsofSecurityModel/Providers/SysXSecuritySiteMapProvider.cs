#region Header Comment Block

// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXSecuritySiteMapProvider.cs
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

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Providers
{
    /// <summary>
    /// Security Menu custom siteMapProvider class.
    /// </summary>
    /// <remarks></remarks>
    public class SysXSecuritySiteMapProvider : StaticSiteMapProvider
    {

        #region Variables

        #region Private Variables

        static ISysXSessionService _sysXSessionService;

        static ISysXSecurityService _sysXSecurityService;

        ISysXLoggerService _sysXLoggerService;

        readonly object _siteMapLock = new object();

        Boolean _mIsInitialized;

        SiteMapNode _rootNode;

        SysXSiteMapNode newParentNode;

        Dictionary<String, SysXSiteMapNode> _allNodes = new Dictionary<String, SysXSiteMapNode>();

        String _sysXAdminRoleName = SecurityManager.GetSysXConfigValue(SysXSecurityConst.SYSX_ADMIN_ROLE_KEY_NAME);

        #endregion

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

        private SysXSiteMapNode CreateSiteMapNode(ProductFeature productFeature)
        {
            try
            {
                //List<String> roles = ((SysXNavigationService)NavigationService)._navigationManager.GetProductFeatureRoles(productFeature.ProductFeatureID);
                List<RolePermissionProductFeature> roleFeatures = SecurityManager.GetProductFeatureRoles(productFeature.ProductFeatureID).ToList();
                List<String> roles = (from role in roleFeatures
                                      select role.RoleDetail.Name).ToList();
                SysXSiteMapNode parentNode = new SysXSiteMapNode(this, productFeature.ProductFeatureID.ToString(), productFeature.Name,
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
                LoggerService.GetLogger().Error(SysXUtils.GetMessage("SysXSecuritySiteMapProvider CreateSiteMapNode"), ex);
                return null;
            }
        }

        private void BuildChildSiteMapNodes(SysXSiteMapNode siteMapNode, ProductFeature productFeature)
        {
            try
            {
                //the display order is taken care at this level to avoid the 2 loops currently running while building the menu 
                foreach (ProductFeature childProductFeature in productFeature.ProductFeature1.OrderBy(condition => condition.DisplayOrder))
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
                LoggerService.GetLogger().Error(SysXUtils.GetMessage("SysXSecuritySiteMapProvider BuildChildSiteMapNodes"), ex);
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
                        _allNodes.Add(node.Key, node);
                    }
                    else
                    {
                        throw new SysXException(SysXUtils.GetMessage(ResourceConst.SECURITY_PARENTNODE));
                    }
                }
            }
            catch (System.Exception ex)
            {
                LoggerService.GetLogger().Error(SysXUtils.GetMessage("SysXSecuritySiteMapProvider AddSiteMapNode"), ex);
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
                        List<ProductFeature> productFeatures = SecurityManager.GetProductFeatures().Where(pfs => pfs.IsExplicitFeature == false).OrderBy(condition => condition.DisplayOrder).ToList();
                        //retrieving only Security by ignoring the top menus
                        List<ProductFeature> rootProductFeature = productFeatures.Where(pf => pf.ProductFeature2.IsNull() && pf.ProductFeatureID.Equals(1)).OrderBy(condition => condition.DisplayOrder).ToList();
                        _rootNode = new SiteMapNode(this, SysXSecurityConst.SITEMAP_ROOT_NODE_KEY, SysXSecurityConst.SITEMAP_ROOT_NODE_URL);
                        _allNodes.Clear();
                        base.Clear();

                        foreach (ProductFeature productFeature in rootProductFeature)
                        {
                            SysXSiteMapNode node = CreateSiteMapNode(productFeature);
                            AddSiteMapNode(node, string.Empty);
                            BuildChildSiteMapNodes(node, productFeature);
                        }

                        _mIsInitialized = true;
                    }
                    catch (SysXException ex)
                    {
                        LoggerService.GetLogger().Error(SysXUtils.GetMessage("SysXSecuritySiteMapProvider" + ResourceConst.SECURITY_UNABLE_TO_BUILD_SITEMAP), ex);
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
        public static void AddSecurityBreadCrumbNode(String nodeName, String nodeURL)
        {
            List<SecurityBreadCrumbNode> nodeList;

            if (!HttpContext.Current.Session["BreadCrumb"].IsNull())
            {
                nodeList = (List<SecurityBreadCrumbNode>)HttpContext.Current.Session["BreadCrumb"];
                SecurityBreadCrumbNode currentNode = nodeList.Find(temp => temp.NodeName == nodeName);

                //node already exist in list.. delete all levels after currentNode.
                if (!currentNode.NodeURL.IsNull())
                {
                    RemoveNodes(currentNode.Level + AppConsts.ONE, nodeList);
                }
                else
                {
                    SecurityBreadCrumbNode nod = new SecurityBreadCrumbNode { NodeName = nodeName, NodeURL = nodeURL };
                    Int32 lastLevel = AppConsts.NONE;

                    foreach (SecurityBreadCrumbNode item in nodeList)
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
                nodeList = new List<SecurityBreadCrumbNode>();
                SecurityBreadCrumbNode breadCrumbNode = new SecurityBreadCrumbNode { NodeName = nodeName, NodeURL = nodeURL, Level = AppConsts.TWO };
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
        private static void RemoveNodes(Int32 currentlevel, List<SecurityBreadCrumbNode> nodes)
        {
            SecurityBreadCrumbNode node = nodes.Find(nodeInfo => nodeInfo.Level == currentlevel);

            if (node.NodeURL.IsNull())
            {
                return;
            }

            currentlevel = node.Level;
            nodes.Remove(node);
            RemoveNodes(currentlevel++, nodes);
        }

        #endregion

        #endregion

        #endregion
    }

    public struct SecurityBreadCrumbNode
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
    }
}