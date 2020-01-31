#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IPersistViewState.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Utils
{
    /// <summary>
    /// This interface handles view state.
    /// </summary>
    /// <remarks></remarks>
    public interface IPersistViewState
    {
        /// <summary>
        /// Loads the specified session id.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="pageUrl">The page URL.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        String Load(String sessionId, String pageUrl);

        // <summary>
        /// Saves the viewstate for the specified session id and page url.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="content">The content.</param>
        /// <param name="isOverWritable">True to overwrite if session part matches, False to insert a new entry (if session and page url does not matches)</param>
        /// <remarks></remarks>
        void Save(String sessionId, String pageUrl, String content, Boolean isOverWritable);

        void Delete(String sessionId);
    }
}