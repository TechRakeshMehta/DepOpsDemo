using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.BkgOperations;
using Entity.ClientEntity;

namespace CoreWeb.BkgOperations.Views
{
    public interface ICustomFormLoadView
    {
        Int32 CurrentLoggedInUserId { get; }
        Int32 TenantId { get; set; }
        //UAT-2842
        Boolean IsAdminOrderScreen { get; set; }
        List<CustomFormDataContract> lstCustomForm { get; set; }
        List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }
        List<CustomFormUserData> lstDataForCustomForm { get; set; }
        #region E DRUG SCREENING PROPERTIES
        Int32 EDrugScreenCustomFormId
        {
            get;
            set;
        }
        Int32 EDrugScreenAttributeGroupId
        {
            get;
            set;
        }
        #endregion

        String ValidationMessage { get; set; } //UAT 3573
        String LanguageCode { get; }
    }
}
