using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.TicketsCentre
{
    public class DocumentsContract
    {
        public Int64? WorkItemID
        { get; set; }
        public Int64 DocumentID
        { get; set; }

        public Int64 ClientID
        { get; set; }

        public String Name
        { get; set; }

        public String OriginalName
        { get; set; }

        public String FilePath
        { get; set; }

        public Int32 DocumentTypeID //
        { get; set; }

        public String FileType
        { get; set; }

        public String FileVersion
        { get; set; }

        public Int32? FileSize
        { get; set; }

        public Int64? DocumentFolderID
        { get; set; }

        public Int32 PublishedBy
        { get; set; }

        public DateTime PublishedOn
        { get; set; }

        public Int32 CreatedBy
        { get; set; }

        public DateTime CreatedOn
        { get; set; }

        public Int32? ModifiedBy
        { get; set; }

        public DateTime? ModifiedOn
        { get; set; }

        public Int16 EntityTypeID //
        { get; set; }

        public Int64 EntityID
        { get; set; }
        public Int64 EntityAttributeMappingID
        { get; set; }
    }
}
