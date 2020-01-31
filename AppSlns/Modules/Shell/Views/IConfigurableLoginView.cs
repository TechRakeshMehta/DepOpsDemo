#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IConfigurableLoginView.cs
// Purpose:
//

#endregion

#region Namespace

#region System Defined

using System;
using System.Collections.Generic;

#endregion

#region Application Specific

using Entity;

#endregion

#endregion

namespace CoreWeb.Shell.Views
{
    public interface IConfigurableLoginView
    {

        String CopyRightYear { get; }

        /// <summary>
        /// Sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        /// <remarks></remarks>
        String ErrorMessage { set; }
        Boolean IsAccountInActive { set; }
        Int32 WebSiteId { get; set; }

        /// <summary>
        /// List of the Website Pages
        /// </summary>
        List<WebSiteWebPage> lstWebsitePages { get; set; }
        String FooterHtml { set; }
        String LoginPageImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        /// <remarks></remarks>
        String UserName { get; set; }
        String VerificationMessage { set; }
        Boolean IsShibbolethLogin { get; set; }
        Boolean IsExistingAccount { get; set; }
        Boolean IsAutoLoginThroughShibboleth { get; set; }

        String EncPValue { get; }
    }
}
