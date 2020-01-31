#region NameSpaces

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Project Specific
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
#endregion

#endregion

namespace CoreWeb.BkgOperations.Views
{
    public interface IDrugScreenDataControlView
    {
        /// <summary>
        /// Gets and sets Logged In User TenantId
        /// </summary>
        Int32 TenantId
        {
            get;
            set;
        }

        Int32 AttributeGroupId
        {
            get;
            set;

        }

        List<BkgAttributeGroupMapping> LstBkgAttributeGroupMapping
        {
            get;
            set;
        }

        String  CustomHtml
        {
            set;

        }
        Int32 CustomFormId
        {
            get;
            set;

        }
        List<AttributesForCustomFormContract> LstAttributeForCustomFormContract
        {
            get;
            set;
        }
    }
}
