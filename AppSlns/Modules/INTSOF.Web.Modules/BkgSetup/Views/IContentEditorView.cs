using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface IContentEditorView
    {
        Int32 TenantId { get; set; }
        Int32 DeptProgramMappingID { get; set; }
        String Content { get; set; }
        Int32 CurrentUserId { get;}
        PageContent ContentData { get; set; }
        String ErrorMessage { get; set; }
        String SuccessMessage { get; set; }
    }
}
