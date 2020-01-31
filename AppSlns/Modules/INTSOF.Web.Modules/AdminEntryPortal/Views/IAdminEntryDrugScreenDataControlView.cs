using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AdminEntryPortal.Views
{
    public interface IAdminEntryDrugScreenDataControlView
    {
        /// <summary>
        /// Gets and sets Logged In User TenantId
        /// </summary>
        Int32 TenantId { get; set; }
        Int32 AttributeGroupId { get; set; }
        List<BkgAttributeGroupMapping> LstBkgAttributeGroupMapping { get; set; }
        String CustomHtml { set; }
        Int32 CustomFormId { get; set; }
        List<AttributesForCustomFormContract> LstAttributeForCustomFormContract { get; set; }
    }
}
