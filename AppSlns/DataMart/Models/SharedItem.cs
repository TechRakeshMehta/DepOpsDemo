using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DataMart.Models
{
    public class SharedItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String ID { get; set; }
        [BsonElement("InvitationGroupID")]
        public Int32 InvitationGroupID { get; set; }
        [BsonElement("SharedItemType")]
        public String SharedItemType { get; set; }
        [BsonElement("TenantID")]
        public Int32 TenantID { get; set; }
        [BsonElement("TenantName")]
        public String TenantName { get; set; }
        [BsonElement("AgencyName")]
        public String AgencyName { get; set; }
        [BsonElement("StudentID")]
        public Int32 StudentID { get; set; }
        [BsonElement("FirstName")]
        public String FirstName { get; set; }
        [BsonElement("LastName")]
        public String LastName { get; set; }
        [BsonElement("RotationID")]
        public String RotationID { get; set; }
        [BsonElement("ComplioID")]
        public String ComplioID { get; set; }
        [BsonElement("RotationStartDate")]
        public DateTime? RotationStartDate { get; set; }
        [BsonElement("RotationEndDate")]
        public DateTime? RotationEndDate { get; set; }
        [BsonElement("AgencyID")]
        public Int32 AgencyID { get; set; }
        [BsonElement("CategoryID")]
        public String CategoryID { get; set; }
        [BsonElement("ItemID")]
        public String ItemID { get; set; }
        [BsonElement("FieldID")]
        public String FieldID { get; set; }
        [BsonElement("CategoryName")]
        public String CategoryName { get; set; }
        [BsonElement("ItemName")]
        public String ItemName { get; set; }
        [BsonElement("FieldName")]
        public String FieldName { get; set; }
        [BsonElement("FieldData")]
        public String FieldData { get; set; }
        [BsonElement("CategoryComplianceStatus")]
        public String CategoryComplianceStatus { get; set; }
        [BsonElement("ApprovedOverrideStatus")]
        public String ApprovedOverrideStatus { get; set; }
        [BsonElement("ItemDataID")]
        public String ItemDataID { get; set; }
        [BsonElement("FieldDataID")]
        public String FieldDataID { get; set; }
        [BsonElement("ItemDisplayOrder")]
        public Int32 ItemDisplayOrder { get; set; }
        [BsonElement("FieldDisplayOrder")]
        public Int32 FieldDisplayOrder { get; set; }
        [BsonElement("ItemSubmissionDate")]
        public DateTime? ItemSubmissionDate { get; set; }
        [BsonElement("CustomAttributeValue")]
        public String CustomAttributeValue { get; set; }
        [BsonElement("InvitationReviewStatusName")]
        public String InvitationReviewStatusName { get; set; }
        [BsonElement("ReviewCode")]
        public String ReviewCode { get; set; }
        [BsonElement("UserType")]
        public String UserType { get; set; }

        [BsonElement("SharedBy")]
        public String SharedBy { get; set; }
        [BsonElement("SharedByEmail")]
        public String SharedByEmail { get; set; }
        [BsonElement("ReviewedBy")]
        public String ReviewedBy { get; set; }
        [BsonElement("StudentEmail")]
        public String StudentEmail { get; set; }

        [BsonElement("OutOfComplianceDate")]
        public DateTime? OutOfComplianceDate { get; set; }

        [BsonElement("ReviewedDate")]
        public DateTime? ReviewedDate { get; set; }

    }
}
