#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ISysXSessionService.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web.Security;
using System.Web.UI;

#endregion

#region Application Specific

using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Interface.Services
{
    /// <summary>
    /// This interface handles session services.
    /// </summary>
    /// <remarks></remarks>
    public interface IAllClientSessionService
    {
        /// <summary>
        /// Set custom data in Session
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <remarks></remarks>
        void SetCustomData(String key, Object data);

        /// <summary>
        /// Get custom data from Session
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        Object GetCustomData(String key);

        /// <summary>
        /// 
        /// </summary>
        Boolean IsAlumniRedirectionDue
        {
            get;
            set;
        }

        /// <summary>
        /// Clears the data in session.
        /// </summary>
        /// <param name="doAbandon">if set to <c>true</c> [do abandon].</param>
        /// <remarks></remarks>
        void ClearSession(Boolean doAbandon);
    }
}