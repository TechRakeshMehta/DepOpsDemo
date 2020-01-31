using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace RadPdfStore.Models
{
    public class MongoDBPdfLiteSession
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String ID { get; set; }
        [BsonElement("Key")]
        public String Key { get; set; }
        [BsonElement("Value")]
        public Byte[] Value { get; set; }
    }
}
