﻿#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ICompressor.cs
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

namespace CoreWeb.IntsofSecurityModel
{
    /// <summary>
    /// Handles the operations for Compressor.
    /// </summary>
    public interface ICompressor
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

        Byte[] CompressViewState(Byte[] uncompData);

        Byte[] DecompressViewState(Byte[] compData);

        #endregion

        #endregion
    }
}