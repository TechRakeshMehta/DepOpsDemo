using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace RadPdfStore.Models
{
    public class MongoDBPdfLiteStorage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String ID { get; set; }
        [BsonElement("Key")]
        public String Key { get; set; }
        [BsonElement("SubType")]
        public Int32 SubType { get; set; }
        [BsonElement("Value")]
        public Byte[] Value { get; set; }
    }
}
