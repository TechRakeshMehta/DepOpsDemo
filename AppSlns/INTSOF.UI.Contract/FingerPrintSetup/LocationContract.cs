using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace INTSOF.UI.Contract.FingerPrintSetup
{
    [Serializable]
    [DataContract]
    public class LocationContract
    {
        [DataMember]
        public Int32 LocationID { get; set; }
        [DataMember]
        public String LocationName { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public String Zipcode { get; set; }
        [DataMember]
        public String Address { get; set; }
        [DataMember]
        public String State { get; set; }
        [DataMember]
        public String City { get; set; }
        [DataMember]
        public String Country { get; set; }
        [DataMember]
        public String County { get; set; }
        [DataMember]
        public String FullAddress { get; set; }
        [DataMember]
        public String Longitude { get; set; }
        [DataMember]
        public String Latitude { get; set; }
        [DataMember]
        public Int32? ScheduleMasterID { get; set; }
        [DataMember]
        public String PlaceID { get; set; }
        [DataMember]
        public Boolean IsEnroller { get; set; }
        [DataMember]
        public Int32 TotalCount { get; set; }
        [DataMember]
        public string TenantName { get; set; }
        [DataMember]
        public string PermissionCode { get; set; }
        [DataMember]
        public String GeoCode { get; set; }
        [DataMember]
        public String SlotTime { get; set; }


        [DataMember]
        public DateTime SlotDate { get; set; }



        [DataMember]
        public DateTime EventFromDateTime { get; set; }

        [DataMember]
        public String SlotDescription { get; set; }

        [DataMember]
        public String EventDescription { get; set; }


        [DataMember]
        public String EventName { get; set; }
        
        [DataMember]
        public Boolean IsInUse { get; set; }
        [DataMember]
        public Int32 LocationGroupID { get; set; }
        [DataMember]
        public String LocationGroupName { get; set; }
        [DataMember]
        public Int32 CurrentPageIndex { get; set; }
        [DataMember]
        public Int32 CurrentPageSize { get; set; }
        [DataMember]
        public Boolean IsContractLocation { get; set; }
        [DataMember]
        public string FailureMessage { get; set; }
        [DataMember]
        public String IsContractLocationText { get { return IsContractLocation ? "Yes" : "No"; } }
        [DataMember]
        public Int32 EventSlotId { get; set; }
        [DataMember]
        public Int32? TimeFrame { get; set; }
        [DataMember]
        public Boolean IsSkipABIReview { get; set; }
        [DataMember]
        public Boolean IsPrinterAvailable { get; set; }
        [DataMember]
        public Boolean IsPhotoService { get; set; }
    }
}

