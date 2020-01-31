using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.BkgOperations;
using Entity.ClientEntity;

namespace CoreWeb.BkgOperations.Views
{
    public interface ICustomFormHtlmView
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
