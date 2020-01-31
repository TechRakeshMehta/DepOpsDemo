using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.Shell.Views
{
    public interface IUserAttestationDisclosure
    {
        Int32 TenantID { get; set; }
        Int32 OrganizationUserID { get; set; }
        String DocumentTypeCode { get; set; }
        Entity.UserAttestationDetail UserAttestationDetails { get; set; }
        byte[] MergedSignedDocumentBuffer { get; set; }

        List<Entity.OrganizationUserTypeMapping> OrganizationUserTypeMapping { get; set; }

        Guid UserId { get;}
    }
}
