#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ISecurityRepository.cs
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

namespace DataMart.DAL.Interfaces
{
    /// <summary>
    /// This interface has the method declaration for the security repository.
    /// </summary>

    public interface ISecurityRepository
    {

        String GetClientConnectionString(Int32 tenantId);
    }
}