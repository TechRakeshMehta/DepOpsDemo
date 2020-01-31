#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ILookupRepository.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System.Collections.Generic;


#endregion

#region Application Defined



#endregion

#endregion

namespace DAL.Interfaces
{
    /// <summary>
    /// ILookupRepository : LookRepository interface 
    /// </summary>
    public interface ILookupRepository
    {
        List<TEntity> GetLookupData<TEntity>();

    }
}
