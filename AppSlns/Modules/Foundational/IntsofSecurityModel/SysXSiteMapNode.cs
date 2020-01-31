#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXSiteMapNode.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
#endregion

#region Application Specific
using INTSOF.Utils;
#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel
{
    /// <summary>
    /// System x coordinate site map node.
    /// </summary>
    public class SysXSiteMapNode : SiteMapNode
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SysXSiteMapNode"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="key">The key.</param>
        /// <param name="productFeatureName">Name of the product feature.</param>
        /// <param name="navigationURL">The navigation URL.</param>
        /// <param name="description">The description.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="uiControlId">The UI control id.</param>
        /// <remarks></remarks>
        public SysXSiteMapNode(SiteMapProvider provider,
                               String key,
                               String productFeatureName,
                               String navigationURL,
                               String description,
                               String icon,
                               String uiControlId)
            : base(provider,
                   key,
                   navigationURL,
                   productFeatureName,
                   description)
        {
            Icon = icon;
            UIControlId = uiControlId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SysXSiteMapNode"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="key">The key.</param>
        /// <param name="productFeatureName">Name of the product feature.</param>
        /// <param name="navigationURL">The navigation URL.</param>
        /// <param name="description">The description.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="uiControlId">The UI control id.</param>
        /// <param name="roles">The roles.</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="explicitResourceKeys">The explicit resource keys.</param>
        /// <param name="implicitResourceKey">The implicit resource key.</param>
        /// <remarks></remarks>
        public SysXSiteMapNode(SiteMapProvider provider,
                               String key,
                               String productFeatureName,
                               String navigationURL,
                               String description,
                               String icon,
                               String uiControlId,
                               IList roles,
                               NameValueCollection attributes,
                               NameValueCollection explicitResourceKeys,
                               String implicitResourceKey)
            : base(provider,
                   key,
                   navigationURL,
                   productFeatureName,
                   description,
                   roles,
                   attributes,
                   explicitResourceKeys,
                   implicitResourceKey)
        {
            Icon = icon;
            UIControlId = uiControlId;
        }

        #endregion

        #region Variables

        #endregion

        #region Properties
        #region Private Properties
        private List<SysXSiteMapNode> _childNodes = new List<SysXSiteMapNode>();
        #endregion
        #region Public Properties

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        /// <remarks></remarks>
        public String Icon
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the UI control id.
        /// </summary>
        /// <value>The UI control id.</value>
        /// <remarks></remarks>
        public String UIControlId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the product feature id.
        /// </summary>
        /// <value>The product feature id.</value>
        /// <remarks></remarks>
        public Int32 ProductFeatureId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <remarks></remarks>
        public String Key
        {
            get
            {
                return base.Key;
            }
        }       

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>The display order.</value>
        /// <remarks></remarks>
        public Int32 DisplayOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has child nodes.
        /// </summary>
        /// <remarks></remarks>
        public Boolean sysXHasChildNodes
        {
            get
            {
                return (SysXChildNodes.Count > AppConsts.NONE);
            }
        }
        

        /// <summary>
        /// Gets the child nodes.
        /// </summary>
        /// <remarks></remarks>
        public List<SysXSiteMapNode> SysXChildNodes
        {
            get
            {
                return _childNodes;
            }
        }

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        /// <value>The parent node.</value>
        /// <remarks></remarks>
        public SysXSiteMapNode SysXParentNode
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #endregion
    }
}