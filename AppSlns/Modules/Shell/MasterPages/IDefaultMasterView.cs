#region Header Comment

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IDefaultMasterView.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using System.Collections.Generic;

#endregion

#endregion

namespace CoreWeb.Shell.MasterPages
{
    /// <summary>
    /// This is an interface handling master page operations.
    /// </summary>
    /// <remarks></remarks>
    public interface IDefaultMasterView
    {
        #region Variables

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets the aspnet_Membership.
        /// </summary>
        aspnet_Membership aspnet_Membership
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current Session ID.
        /// </summary>
        /// <remarks></remarks>
        String CurrentSessionId
        {
            get;
        }

        /// <summary>
        /// Set Queue History control visibility
        /// </summary>
        Boolean ShowQueueHistory
        {
            get;
            set;
        }

        /// <summary>
        /// Set Entity ID
        /// </summary>
        Int32 EntityId
        {
            get;
            set;
        }

        IPersistViewState ViewStateProvider { get; }

        /// <summary>
        /// Set Context information
        /// </summary>
        Dictionary<Int32, Dictionary<String, String>> ContextInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Load History Control
        /// </summary>
        void LoadQueueHistory();

        /// <summary>
        ///  Admin Queue Context ID
        /// </summary>
        AdminQueueContext AdminQueueContextId
        {
            get;
            set;
        }

        /// <summary>
        /// To set Timeout minutes
        /// </summary>
        Int32 TimeoutMinutes
        {
            get;
        }

        //Boolean IsApplicant
        //{
        //    get;
        //    set;
        //}

        #endregion

        #region UAT-3077
        Boolean UseAsPopUpWindow
        {
            get;
            set;
        }
        #endregion

        #region Events

        #endregion

        #region Methods

        /// <summary>
        /// Shows the error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <remarks></remarks>
        void ShowSearchErrorMessage(String errorMessage);

        /// <summary>
        /// Shows the error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <remarks></remarks>
        void ShowErrorMessage(String errorMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="messageType"></param>
        void ShowMessageOnPage(String errorMessage, MessageType messageType);

        /// <summary>
        /// Shows the info message.
        /// </summary>
        /// <param name="infoMessage">The info message.</param>
        /// <remarks></remarks>
        void ShowInfoMessage(String infoMessage);

        void ShowSuccessMessage(String successMessage);

        /// <summary>
        /// Hides the error message.
        /// </summary>
        /// <remarks></remarks>
        void HideErrorMessage();

        /// <summary>
        /// Hides the title bar UI component from the rendered page content.
        /// </summary>
        /// <param name="IncludeCssClass">Includes 'no_error_panel' css class in the form element</param>
        void HideTitleBars(bool IncludeCssClass = false);

        /// <summary>
        /// Sets Module title
        /// </summary>
        /// <param name="title"></param>
        void SetModuleTitle(string title);

        /// <summary>
        /// Sets Page / Screen Title
        /// </summary>
        /// <param name="title"></param>
        void SetPageTitle(string title);


        //// <summary>
        /// Shows the error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <remarks></remarks>
        void ShowErrorInfoMessage(String errorMessage);

        #endregion
    }
}