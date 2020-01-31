#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IBaseRepository.cs
// Purpose: 
//

#endregion

using System;
using System.Data.Entity.Core.Objects.DataClasses;

namespace DAL.Interfaces
{
    public interface IBaseRepository
    {
        T AddObjectEntity<T>(T entity) where T : class;
        Boolean UpdateObjectEntity(EntityObject updatedEntityObject);
        Boolean DeleteObjectEntity(EntityObject entityObject);
        Boolean DeleteObjectEntity(EntityObject entityObject, Boolean isHardDelete);

        #region Notes

        //IQueryable<Note> GetCurrentContextNotesByEntityId(Int32 entityId, Int32 contextId);
        //IQueryable<Note> GetNotesByUID(Guid uniqueId);
        //IQueryable<lkpContext> GetNoteContexts(Int32 contextId);

        #endregion        
    }
}
