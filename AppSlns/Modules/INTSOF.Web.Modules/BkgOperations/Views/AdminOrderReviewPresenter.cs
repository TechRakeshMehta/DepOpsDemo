using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public class AdminOrderReviewPresenter : Presenter<IAdminOrderReviewView>
    {
        public void GetAttributeFieldsOfSelectedPackages(String packageIds)
        {
            List<Entity.ClientEntity.AttributeFieldsOfSelectedPackages> lstAttributeFields = BackgroundProcessOrderManager.GetAttributeFieldsOfSelectedPackages(packageIds, View.TenantId);
            if (!lstAttributeFields.IsNullOrEmpty())
            {
                View.LstInternationCriminalSrchAttributes = lstAttributeFields.Where(cond => (cond.BSA_Code.ToUpper().Equals("3DA8912A-6337-4B8F-93C4-88BFC3032D2D")
                                                                        || cond.BSA_Code.ToUpper().Equals("AAB51E52-2A9B-42AB-9A9D-D1AFFC18E211")
                                                                        || cond.BSA_Code.ToUpper().Equals("515BEF57-9072-4D2A-A97A-0C248BB045F9"))).ToList();
            }
            else
            {
                View.LstInternationCriminalSrchAttributes = new List<AttributeFieldsOfSelectedPackages>();
            }
        }

        #region E Drug Screening
        public void GetEDrugAttributeGroupIdAndFormId()
        {
            String eDrugScrnAttributeGrpCode = AppConsts.ELECTRONIC_DRUG_SCREEN_ATT_GROUP_CODE;
            String result = BackgroundProcessOrderManager.GetEDrugAttributeGroupId(View.TenantId, eDrugScrnAttributeGrpCode);
            if (!result.IsNullOrEmpty())
            {
                String[] separator = { "," };
                String[] splitIds = result.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                View.EDrugScreenCustomFormId = Convert.ToInt32(splitIds[0]);
                View.EDrugScreenAttributeGroupId = Convert.ToInt32(splitIds[1]);
            }
        }  
              
        #endregion

        public void GetAttributesForTheCustomForm(String packageId, Int32 customFormId)
        {
            CustomFormDataContract customFormDataContract = BackgroundProcessOrderManager.GetAttributesForTheCustomForm(View.TenantId, packageId, customFormId);
            View.lstCustomFormAttributes = customFormDataContract.lstCustomFormAttributes;
        }
    }
}
