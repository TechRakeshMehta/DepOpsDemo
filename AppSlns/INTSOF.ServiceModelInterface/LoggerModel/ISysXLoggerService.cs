#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ISysXLoggerService.cs
// Purpose:   ISysXLoggerService Interface
//

#endregion

#region Namespaces

#region System Defined

#endregion

#region Application Specific

using INTSOF.Logger;

#endregion

#endregion

namespace CoreWeb.IntsofLoggerModel.Interface
{
    /// <summary>
    /// ISysXLoggerService interface
    /// </summary>
    /// <remarks></remarks>
    public interface ISysXLoggerService
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods


        /// <summary>
        /// Loggs the messages.
        /// </summary>
        /// <returns>ILogger</returns>
        /// <remarks></remarks>
        ILogger GetLogger();

        #endregion

        #endregion
    }
}
