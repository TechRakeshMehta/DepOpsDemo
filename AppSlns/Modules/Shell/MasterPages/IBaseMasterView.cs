#region Header Comment Master

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IBaseMasterView.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined



#endregion

#region Application Specific



#endregion

#endregion

namespace CoreWeb.Shell.MasterPages
{
    /// <summary>
    /// Interface for base master view.
    /// </summary>
    public interface IBaseMasterView
    {
        #region Variables

        #endregion

        #region Properties

        string SiteTitle { set; }
        string HeaderHtml { set; }
        string FooterHtml { set; }
        bool IsSharedUser { get; set; }
        #endregion

        #region Events

        #endregion

        #region Methods

        #endregion
    }
}