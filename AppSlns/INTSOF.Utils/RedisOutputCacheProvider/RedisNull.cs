﻿using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Intsof.RedisOutputCacheProvider
{
    [Serializable]
    internal class RedisNull : ISerializable
    {
        public RedisNull() 
        {}
        protected RedisNull(SerializationInfo info, StreamingContext context)
        {}
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {}
    } 
}
