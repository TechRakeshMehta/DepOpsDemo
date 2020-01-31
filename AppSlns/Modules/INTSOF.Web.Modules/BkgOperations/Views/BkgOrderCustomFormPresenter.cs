using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace CoreWeb.BkgOperations.Views
{
    public class BkgOrderCustomFormPresenter : Presenter<IBkgOrderCustomFormView>
    {
        public void GetAttributesCustomFormIdOrderId()
        {
            BkgOrderDetailCustomFormDataContract bkgOrderDetailCustomFormDataContract = BackgroundProcessOrderManager.GetBkgOrderCustomFormAttributesData(View.SelectedTenantID, View.MasterOrderID, View.CustomFormID);
            if (bkgOrderDetailCustomFormDataContract.IsNotNull())
            {
                if (bkgOrderDetailCustomFormDataContract.lstCustomFormAttributes.IsNotNull())
                    View.lstCustomFormAttributes = bkgOrderDetailCustomFormDataContract.lstCustomFormAttributes;
                else
                    View.lstCustomFormAttributes = new List<AttributesForCustomFormContract>();
                if (bkgOrderDetailCustomFormDataContract.lstDataForCustomForm.IsNotNull())
                    View.lstDataForCustomForm = bkgOrderDetailCustomFormDataContract.lstDataForCustomForm;
                else
                    View.lstDataForCustomForm = new List<BkgOrderDetailCustomFormUserData>();
            }
            else
            {
                View.lstCustomFormAttributes = new List<AttributesForCustomFormContract>();
                View.lstDataForCustomForm = new List<BkgOrderDetailCustomFormUserData>();
            }

        }

        public Boolean IsEdsServiceExitForOrder(Int32 groupId)
        {
            String eDrugScrnServiceTypeCode = BkgServiceType.ELECTRONICDRUGSCREEN.GetStringValue();
            String eDrugScrnAttributeGrpCode = AppConsts.ELECTRONIC_DRUG_SCREEN_ATT_GROUP_CODE;
            if (View.SelectedTenantID > AppConsts.NONE && View.MasterOrderID > AppConsts.NONE)
            {
                if (BackgroundProcessOrderManager.IsEdsServiceExitForOrder(View.SelectedTenantID, View.MasterOrderID, eDrugScrnServiceTypeCode))
                {

                    if (BackgroundProcessOrderManager.GetSvcAttributeGroupIdByCode(View.SelectedTenantID, eDrugScrnAttributeGrpCode) == groupId)
                        return true;
                    return false;
                }
            }
            return false;
        }
    }
}
