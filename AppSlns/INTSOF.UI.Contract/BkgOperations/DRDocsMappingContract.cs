using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    public class DRDocsMappingContract
    {
        public Int32 DisclosureDocumentMappingId { get; set; }
        public Int32 CountryId { get; set; }
        public String CountryName { get; set; }
        public Int32 StateId { get; set; }
        public String StateName { get; set; }
        public Int32 ServiceId { get; set; }
        public String ServiceName { get; set; }
        public Int32 InstitutionHierarchyId { get; set; }
        public String InstitutionHierarchy { get; set; }
        public Int16 RegulatoryEntityTypeId { get; set; }
        public String RegulatoryEntityType { get; set; }
        public Int32 DocumentId { get; set; }
        public String DocumentName { get; set; }
        public Int32 TenantId { get; set; }
        public String TenantName { get; set; }
    }

    public class DRDocsMappingObjectIds
    {
        public Int32 DisclosureDocumentMappingId { get; set; }
        public Int32 CountryId { get; set; }
        public Int32 StateId { get; set; }
        public Int32 ServiceId { get; set; }
        public Int32 InstitutionHierarchyId { get; set; }
        public Int16 RegulatoryEntityTypeId { get; set; }
        public Int32 DocumentId { get; set; }
        public Int32 TenantId { get; set; }
    }
}
