using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClientContact
{
    [Serializable]
    [DataContract]
    public class ClientContactNotesContract
    {
        [DataMember]
        public Int32 NoteId { get; set; }
        [DataMember]
        public String Notes { get; set; }
        [DataMember]
        public String NotesCreatedBy { get; set; }
        [DataMember]
        public DateTime NoteCreatedOn { get; set; }
        [DataMember]
        public Boolean IsDeleted { get; set; }
        [DataMember]
        public Int32 InstructorOrgUserId { get; set; }
        [DataMember]
        public Int32 ClientContactId { get; set; }
    }
}
