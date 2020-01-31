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
    public class FingerPrintArchivedOrderContract
    {
        [DataMember]
        public Int32 OrderId { get; set; }
        [DataMember]
        public String OrderNumber { get; set; }
        [DataMember]
        public String FirstName { get; set; }
        [DataMember]
        public String LastName { get; set; }
        [DataMember]
        public DateTime? OrderDate
        {
            get;
            set;
        }
        [DataMember]
        public String OrderDetails { get; set; }
        [DataMember]
        public Int32 ArchivedOrgUserProfileId { get; set; }
        [DataMember]
        public String PrimaryEmail { get; set; }
        [DataMember]
        public Int32 DowntownLocationId { get; set; }
        [DataMember]
        public String DowntownLocationName { get; set; }
        [DataMember]
        public String DowntownLocationAddress { get; set; }
        [DataMember]
        public bool IsPrinterAvailable { get; set; }
        [DataMember]
        public String DowntownLocationDescription { get; set; }
        [DataMember]
        public bool IsPassportPhotoService { get; set; }
        [DataMember]
        public Int32 ArchivedOrgUserId { get; set; }
    }
}
