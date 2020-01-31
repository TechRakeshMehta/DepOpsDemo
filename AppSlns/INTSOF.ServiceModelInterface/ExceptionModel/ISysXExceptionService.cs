#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ISysXExceptionService.cs
// Purpose:   ISysXExceptionService Interface
//

#endregion

#region Namespaces

#region System Defined

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;


#endregion

#region Application Specific

#endregion

#endregion

namespace CoreWeb.IntsofExceptionModel.Interface
{
    /// <summary>
    /// ISysXExceptionService Interface
    /// </summary>
    /// <remarks></remarks>
    public interface ISysXExceptionService
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
        /// Gets the exception handler.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        IExceptionHandler GetExceptionHandler();

        //void HandleInfo(string infoMessage);

        /// <summary>
        /// Handles the error.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <remarks></remarks>
        void HandleError(String errorMessage);

        //void HandleDebugInfo(string debugMessgae); // TODO: not sure either to keep it or remove.

        //void HandleInfoWithException(string infoMessage, Exception e); // TODO: not sure either to keep it or remove.

        /// <summary>
        /// Handles the error.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="e">The e.</param>
        /// <remarks></remarks>
        void HandleError(String errorMessage, Exception e);

        //StringBuilder FormatErrorMessage(Exception ex); // TODO: not sure either to keep it or remove.
        void HandleDebug(string message);
        #endregion

        #endregion
    }
}