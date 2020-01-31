#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ISysXCacheManager.cs
// Purpose:   ISysX Cache Manager
//

#endregion

#region Namespace

#region System Defined

using System;
using System.ServiceModel;

#endregion

#endregion

namespace SPI.Caching
{
    [ServiceContract]
    public interface ISysXCacheManager
    {
        [OperationContract]
        [FaultContract(typeof(SysXCachetException))]
        void Add(String Key, Object Value);

        [OperationContract]
        [FaultContract(typeof(SysXCachetException))]
        void Insert(String Key, Object Value, String DependencyFilePath, String Priority, DateTime AbsoluteExpiration, TimeSpan SlidingExpiration);

        [OperationContract]
        [FaultContract(typeof(SysXCachetException))]
        void Remove(String Key);

        [OperationContract]
        [FaultContract(typeof(SysXCachetException))]
        Object Get(String Key);

        [OperationContract]
        [FaultContract(typeof(SysXCachetException))]
        Boolean Exists(String Key);

        [OperationContract]
        [FaultContract(typeof(SysXCachetException))]
        Boolean SetLock(String Key);

        [OperationContract]
        [FaultContract(typeof(SysXCachetException))]
        void ReleaseLock(String Key);

    }
}
