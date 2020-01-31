using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IAdminOrderReviewView
    {
        Int32 TenantId { get; set; }       
        List<AttributeFieldsOfSelectedPackages> LstInternationCriminalSrchAttributes
        {
            get;
            set;
        }

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

        List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }
    }
}
