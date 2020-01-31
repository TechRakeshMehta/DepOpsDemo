#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXExceptionMessageFormatter.cs
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
    public class DeleteEntityEventArgs : EventArgs
    {
        #region variables

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the entity GUID.
        /// </summary>
        /// <value>The entity GUID.</value>
        /// <remarks></remarks>
        public String EntityGuid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the entity ID.
        /// </summary>
        /// <value>The entity ID.</value>
        /// <remarks></remarks>
        public Int32 EntityID
        {
            get;
            set;
        }

        #endregion

        #region Events

        #endregion

        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteEntityEventArgs"/> class.
        /// </summary>
        /// <param name="entityId">The entity ID.</param>
        /// <remarks></remarks>
        public DeleteEntityEventArgs(Int32 entityId)
        {
            EntityID = entityId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteEntityEventArgs"/> class.
        /// </summary>
        /// <param name="entityGuid">The entity GUID.</param>
        /// <remarks></remarks>
        public DeleteEntityEventArgs(String entityGuid)
        {
            EntityGuid = entityGuid;
        }

        #endregion
    }
}