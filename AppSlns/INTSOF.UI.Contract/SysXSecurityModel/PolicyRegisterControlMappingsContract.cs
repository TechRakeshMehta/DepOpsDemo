#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  PolicyRegisterControlMappings.cs
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

namespace INTSOF.UI.Contract.IntsofSecurityModel
{
    /// <summary>
    /// This contract gets or sets the properties for Policy Register Control Mappings section.
    /// </summary>
    public class PolicyRegisterControlMappingsContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// RegisterControlID </summary>
        /// <value>
        /// Handles the Register Control ID.</value>
        public Int32 RegisterControlId
        {
            get;
            set;
        }

        /// <summary>
        /// ParentControlID </summary>
        /// <value>
        /// Handles the Parent Control ID.</value>
        public Int32 ParentControlId
        {
            get;
            set;
        }

        /// <summary>
        /// ControlName </summary>
        /// <value>
        /// Handles the Control Name.</value>
        public String ControlName
        {
            get;
            set;
        }

        /// <summary>
        /// DisplayName </summary>
        /// <value>
        /// Handles the Display Name.</value>
        public String DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// ControlPath </summary>
        /// <value>
        /// Handles the Control Path.</value>
        public String ControlPath
        {
            get;
            set;
        }

        /// <summary>
        /// PermissionTypeID </summary>
        /// <value>
        /// Handles the Permission Type ID.</value>
        public Int32 PermissionTypeId
        {
            set;
            get;
        }

        /// <summary>
        /// PolicyRegisterControlID </summary>
        /// <value>
        /// Handles the Policy Register Control ID.</value>
        public Int32 PolicyRegisterControlId
        {
            get;
            set;
        }

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}