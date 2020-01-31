using System;
using System.Collections.Generic;

namespace Intsof.RedisOutputCacheProvider
{
    internal sealed class RedisUtility
    {
        private readonly ProviderConfiguration _configuration;
        internal readonly ISerializer _serializer;

        public RedisUtility(ProviderConfiguration configuration)
        {
            _configuration = configuration;
            _serializer = GetSerializer();
        }

        private ISerializer GetSerializer()
        {
            string serializerTypeName = _configuration.RedisSerializerType;
            if (!string.IsNullOrWhiteSpace(serializerTypeName))
            {
                var serializerType = Type.GetType(serializerTypeName, true);
                if (serializerType != null)
                {
                    return (ISerializer)Activator.CreateInstance(serializerType);
                }
            }
            return new BinarySerializer();
        }

        internal byte[] GetBytesFromObject(object data)
        {
            return _serializer.Serialize(data);
        }

        internal object GetObjectFromBytes(byte[] dataAsBytes)
        {
            return _serializer.Deserialize(dataAsBytes);
        }
    }
}
