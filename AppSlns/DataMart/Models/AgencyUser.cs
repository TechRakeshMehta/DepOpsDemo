using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace DataMart.Models
{
    public class AgencyUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String ID { get; set; }
        [BsonElement("AgencyUserID")]
        public Int32 AgencyUserID { get; set; }
        [BsonElement("AgencyUserName")]
        public String AgencyUserName { get; set; }
        [BsonElement("AgencyUserEmail")]
        public String AgencyUserEmail { get; set; }
        [BsonElement("UserID")]
        public String UserID { get; set; }
        [BsonElement("HasRotationAccess")]
        public Boolean HasRotationAccess { get; set; }
        [BsonElement("HasComplianceAccess")]
        public Boolean HasComplianceAccess { get; set; }

        [BsonElement("Agencies")]
        public List<Agency> Agencies { get; set; }

        [BsonElement("SharingInvitations")]
        public List<SharingInvitation> SharingInvitations { get; set; }

        [BsonElement("SharedRotations")]
        public List<SharedRotation> SharedRotations { get; set; }
    }

    public class SharedRotation
    {
    }

    public class SharingInvitation
    {
        [BsonElement("InvitationID")]
        public Int32 InvitationID { get; set; }

        [BsonElement("InvitationGroupID")]
        public Int32 InvitationGroupID { get; set; }
    }

    public class Agency
    {
        [BsonElement("TenantID")]
        public Int32 TenantID { get; set; }

        [BsonElement("TenantName")]
        public String TenantName { get; set; }

        [BsonElement("NodeID")]
        public Int32 NodeID { get; set; }

        [BsonElement("NodeName")]
        public String NodeName { get; set; }

        [BsonElement("AgencyID")]
        public Int32 AgencyID { get; set; }

        [BsonElement("AgencyName")]
        public String AgencyName { get; set; }
    }
}
