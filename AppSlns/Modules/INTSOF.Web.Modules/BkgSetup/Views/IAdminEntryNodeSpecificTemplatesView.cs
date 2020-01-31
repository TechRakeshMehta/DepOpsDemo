using INTSOF.UI.Contract.BkgSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface IAdminEntryNodeSpecificTemplatesView
    {
        Int32 TenantId { get; set; }
        Int32 DeptProgramMappingID { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        List<Entity.CommunicationTemplatePlaceHolder> lstTemplatePlaceHolders { get; set; }
        AdminEntryNodeTemplate AdminEntryNodeTemplate { get; set; }
    }
}
