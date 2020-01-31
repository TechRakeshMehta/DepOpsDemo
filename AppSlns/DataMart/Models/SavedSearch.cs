using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace DataMart.Models
{
    public class SavedSearch
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String ID { get; set; }
        [BsonElement("UserID")]
        public String UserID { get; set; }
        [BsonElement("SearchType")]
        public String SearchType { get; set; }
        [BsonElement("SearchName")]
        public String SearchName { get; set; }
        [BsonElement("SearchDescription")]
        public String SearchDescription { get; set; }
        [BsonElement("Institutes")]
        public List<Int32> Institutes { get; set; }
        [BsonElement("Agencies")]
        public List<Int32> Agencies { get; set; }
        [BsonElement("Categories")]
        public List<String> Categories { get; set; }
        [BsonElement("Days")]
        public List<String> Days { get; set; }
        [BsonElement("Items")]
        public List<String> Items { get; set; }
        [BsonElement("ReviewStatus")]
        public List<String> ReviewStatus { get; set; }
        [BsonElement("UserTypes")]
        public List<String> UserTypes { get; set; }
        [BsonElement("ComplioID")]
        public String ComplioID { get; set; }
        [BsonElement("CustomAttribute")]
        public String CustomAttribute { get; set; }
        [BsonElement("RotationStartDate")]
        public String RotationStartDate { get; set; }
        [BsonElement("RotationEndDate")]
        public String RotationEndDate { get; set; }

        [BsonElement("IncludeUndefinedDataShares")]
        public String IncludeUndefinedDataShares { get; set; }

        [BsonElement("IsUniqueResultsOnly")]
        public String IsUniqueResultsOnly { get; set; }
    }
}
