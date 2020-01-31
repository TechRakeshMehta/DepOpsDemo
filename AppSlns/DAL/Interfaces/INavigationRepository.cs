#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  INavigationRepository.cs
// Purpose:
//

#endregion

using Entity;
using System;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    /// <summary>

    // Saurav Roy - Date - 22 /06/2011
    /// </summary>
    public interface INavigationRepository
    {
        List<ProductFeature> GetProductFeatures();
        List<String> GetProductFeatureRoles(Int32 productFeatureId);
    }
}
