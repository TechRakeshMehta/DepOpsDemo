using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AdminEntryPortal.Views
{
    public interface IAdminEntryCustomFormHtmlView
    {
        Int32 DisplayColumns { get; set; }
        String SectionTitle { get; set; }
        String CustomHtml { get; set; }
        List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }
        Int32 InstanceId { get; set; }
        Int32 tenantId { get; set; }

        List<SystemSpecificLanguageText> SystemSpecificLanguageTextList { get; set; }
    }
}
