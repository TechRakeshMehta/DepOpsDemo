using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MobileWebApi.Models
{
    [DataContract]
    public class PackageContract
    {
        [DataMember]
        public Int32 PackageSubscriptionId { get; set; }
        [DataMember]
        public Int32 PackageId { get; set; }
        [DataMember]
        public String PackageName { get; set; }

        [DataMember]
        public String PackageStatus { get; set; }
        [DataMember]
        public List<CategoryContract> lstCategory
        {
            get;
            set;
        }
    }


    [DataContract]
    public class CategoryContract
    {
        [DataMember]
        public Int32 CategoryId { get; set; }
        [DataMember]
        public String CategoryName { get; set; }
        [DataMember]
        public String CategoryStatus { get; set; }
        [DataMember]
        public String CategoryNonCompliancedate { get; set; }
        [DataMember]
        public List<ItemContract> lstItem
        {
            get;

            set;
        }
    }


    [DataContract]
    public class ItemContract
    {
        [DataMember]
        public Int32 ItemId { get; set; }
        [DataMember]
        public String ItemName { get; set; }
        [DataMember]
        public String ItemStatus { get; set; }
        [DataMember]
        public List<AttributeContract> lstAttribute { get; set; }
    }


    [DataContract]
    public class AttributeContract
    {
        [DataMember]
        public Int32 AttributeId { get; set; }
        [DataMember]
        public String AttributeName { get; set; }
        [DataMember]
        public String AttributeValue { get; set; }
    }

}