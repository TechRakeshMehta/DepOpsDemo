using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DataMart.Models
{
    public class RotationDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String ID { get; set; }
        [BsonElement("InvitationGroupID")]
        public Int32 InvitationGroupID { get; set; }
        [BsonElement("TenantID")]
        public Int32 TenantID { get; set; }
        [BsonElement("TenantName")]
        public String TenantName { get; set; }
        [BsonElement("AgencyName")]
        public String AgencyName { get; set; }
        [BsonElement("StudentID")]
        public Int32 StudentID { get; set; }
        [BsonElement("StudentFirstName")]
        public String StudentFirstName { get; set; }
        [BsonElement("StudentLastName")]
        public String StudentLastName { get; set; }
        [BsonElement("StudentEmailAddress")]
        public String StudentEmailAddress { get; set; }
        [BsonElement("StudentPhoneNumber")]
        public String StudentPhoneNumber { get; set; }
        [BsonElement("StudentAddress")]
        public String StudentAddress { get; set; }
        [BsonElement("StudentDOB")]
        public String StudentDOB { get; set; }

        [BsonElement("RotationID")]
        public String RotationID { get; set; }
        [BsonElement("ComplioID")]
        public String ComplioID { get; set; }
        [BsonElement("RotationName")]
        public String RotationName { get; set; }
        [BsonElement("TypeSpecialty")]
        public String TypeSpecialty { get; set; }
        [BsonElement("Department")]
        public String Department { get; set; }
        [BsonElement("Program")]
        public String Program { get; set; }
        [BsonElement("Course")]
        public String Course { get; set; }
        [BsonElement("Term")]
        public String Term { get; set; }
        [BsonElement("UnitFloorLoc")]
        public String UnitFloorLoc { get; set; }
        [BsonElement("RotationShift")]
        public String RotationShift { get; set; }
        [BsonElement("Times")]
        public String Times { get; set; }
        [BsonElement("RotationStartDate")]
        public DateTime? RotationStartDate { get; set; }
        [BsonElement("RotationEndDate")]
        public DateTime? RotationEndDate { get; set; }
        [BsonElement("AgencyID")]
        public Int32 AgencyID { get; set; }
        [BsonElement("InvitationReviewStatusName")]
        public String InvitationReviewStatusName { get; set; }
        [BsonElement("InvitationSourceType")]
        public String InvitationSourceType { get; set; }
        [BsonElement("InvitationSourceCode")]
        public String InvitationSourceCode { get; set; }
        [BsonElement("Days")]
        public String Days { get; set; }
        [BsonElement("CustomAttributes")]
        public String CustomAttributes { get; set; }
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
        [BsonElement("ReviewedDate")]
        public DateTime? ReviewedDate { get; set; }
    }
}
