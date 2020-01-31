
#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ILoginView.cs
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
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.Shell.Views
{
    /// <summary>
    /// Interface for Login page.
    /// </summary>
    /// <remarks></remarks>
    public interface ILoginCenterPanelView
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        /// <remarks></remarks>
        String UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <remarks></remarks>
        String Password
        {
            get;
        }

        #region New Region

        Boolean IsRedirectToMobileSite { get; }

        String CurrentSessionId { get; }

        String SiteUrl { get; set; }

        String SharedUserLoginURL { get; set; }

        Boolean needToShowEmploymentResultReport { get; set; }

        Boolean CheckWebsiteURL { get; }
        
        #region UAT-1218
        List<OrganizationUserTypeMapping> OrganizationUserTypeMapping { get; set; }
        Boolean IsSharedUserHasOtherRoles { get; set; }
        #endregion

        /// <summary>
        /// Property to decide whether the User is from Correct Tenant Url or not. 
        /// Will be set TRUE even if it is Non-Central login type.
        /// NOTE - WILL BE OVERRIDEED, WHEN THE APPLICANT IS VALIDATED FOR AUTOLOGIN PROCESS
        /// </summary>
        Boolean IsIncorrectLoginUrl { get; set; }

        String ErrorMessage {  set; }

        Boolean IsAccountInActive { set; }

        String EncPValue { get; set; }

        /// <summary>
        /// Gets the selected block id.
        /// </summary>
        /// <remarks></remarks>
        Int32 SelectedBlockId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the selected block.
        /// </summary>
        /// <remarks></remarks>
        String SelectedBlockName
        {
            get;
            set;
        }

        /// <summary>
        /// UAT-2494, New Account verification enhancements (additional verification step)
        /// </summary>
        Boolean ShowAdditionalAccountVerificationPage { get; set; }

        String VerificationMessage {  set; }

        #region UAT-2792
        Boolean IsShibbolethLogin { get; set; }
        String ShibbolethUniqueIdentifier { get; set; }
        Int32 IntegrationClientId { get; set; }

        Boolean IsAutoLoginThroughShibboleth { get; set; }
        Int32 ShibbolethHostID { get; set; }
        String HostName { get; set; }
        Boolean IsExistingAccount { get; set; }
        Boolean IsShibbolethApplicant { get; set; }
        #endregion

        Boolean IsTwoFactorAuthenticationRequired { get; set; }//UAT-2930

        #endregion

        Boolean IsRequestExteranlLogin { get; set; }
        Int32 StoreExternalLoginTenantId { get; set; }
        String InvalidUsernamePswd { get; }

        String AccountLockedMessage { get; }

        IPersistViewState ViewStateProvider { get; }
    }
}