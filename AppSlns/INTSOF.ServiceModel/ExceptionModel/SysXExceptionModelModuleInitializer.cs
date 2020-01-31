#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXExceptionModelModuleInitializer.cs
// Purpose:   Exception Model Module Initializer class
//

#endregion

#region Namespaces

#region System Defined

using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;


#endregion

#region Application Specific

#endregion

#endregion

namespace CoreWeb.IntsofExceptionModel
{
    /// <summary>
    /// Exception Model ModuleInitializer
    /// </summary>
    /// <remarks></remarks>
    public class SysXExceptionModelModuleInitializer : ModuleInitializer
    {
        #region Variables

        #endregion

        #region Properties

        #endregion

        #region Events

        #endregion

        #region Methods

        #region public methods

        /// <summary>
        /// Loads the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <remarks></remarks>
        public override void Load(CompositionContainer container)
        {
            base.Load(container);

            AddGlobalServices(container.Services);
        }

        /// <summary>
        /// Configures the specified services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="moduleConfiguration">The module configuration.</param>
        /// <remarks></remarks>
        public override void Configure(IServiceCollection services, System.Configuration.Configuration moduleConfiguration)
        {
            // TODO: Used in future
        }

        #endregion

        #region protected Methods

        /// <summary>
        /// Adds the global services.
        /// </summary>
        /// <param name="globalServices">The global services.</param>
        /// <remarks></remarks>
        protected virtual void AddGlobalServices(IServiceCollection globalServices)
        {
            // TODO: add a service that will be visible to any module
        }

        #endregion

        #endregion
    }
}