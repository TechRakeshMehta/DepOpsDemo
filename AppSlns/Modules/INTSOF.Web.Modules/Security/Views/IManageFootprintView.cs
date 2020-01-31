#region Header Comment Block
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IManageFootprintView.cs
// Purpose:   
//
// 
// Comment
// -------------------------------------------------
//  Initial.
//
#endregion

#region Namespaces

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using INTSOF.UI.Contract.IntsofSecurityModel;
#endregion

#region Application Specific
#endregion

#endregion
namespace CoreWeb.IntsofSecurityModel.Views
{
    public interface IManageFootprintView
    { 
        #region Variables

        #endregion

        #region Properties

        IEnumerable<ServiceFootprint> ServiceFootprint
        {
            set;
        }
        List<ServiceFootprint> ServiceFootprintTreeListState
        {
            get;    
            set;
        }
        ManageServicesFootPrintContract ViewContract
        {
            get;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }
        
        #endregion

        #region Events

        #endregion

        #region Methods

        #endregion
    }
}
