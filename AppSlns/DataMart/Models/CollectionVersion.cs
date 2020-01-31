using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DataMart.Models
{
    public class CollectionVersion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String ID { get; set; }
        [BsonElement("CollectionName")]
        public String CollectionName { get; set; }
        [BsonElement("ReadVersion")]
        public String ReadVersion { get; set; }
        [BsonElement("WriteVersion")]
        public String WriteVersion { get; set; }
        [BsonElement("LastSyncDate")]
        public DateTime LastSyncDate { get; set; }
        [BsonElement("WasLastSyncSuccess")]
        public Boolean WasLastSyncSuccess { get; set; }
    }
}
