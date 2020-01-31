using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AdminEntryPortal.Views
{
    public class AdminEntryCustomFormLoadPresenter : Presenter<IAdminEntryCustomFormLoadView>
    {
        public void GetCustomFormsForThePackage(String packageId)
        {
            View.lstCustomForm = BackgroundProcessOrderManager.GetCustomFormsForThePackage(View.TenantId, packageId);
        }

        public void GetAttributesForTheCustomForm(String packageId, Int32 customFormId, string _languageCode)
        {
            CustomFormDataContract customFormDataContract = BackgroundProcessOrderManager.GetAttributesForTheCustomForm(View.TenantId, packageId, customFormId, _languageCode);
            View.lstCustomFormAttributes = customFormDataContract.lstCustomFormAttributes;

        }

        public Int32 GetTenant()
        {
            Entity.Organization _org = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization;
            // View.InstitutionName = _org.OrganizationName;
            return _org.TenantID.Value;
        }

        public String GetNextPagePathByOrderStageID(ApplicantOrderCart applicantOrderCart)
        {
            return ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.CustomForms);
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

        public String ValidatePageData(List<BackgroundOrderData> listData, Boolean IsCustomFormScreen, string languageCode)
        {
            StringBuilder xmlStringData = new StringBuilder();

            xmlStringData.Append("<Attributes>");
            foreach (BackgroundOrderData item in listData)
            {
                foreach (var dic in item.CustomFormData)
                {
                    xmlStringData.Append("<Attribute><AttributeID>" + dic.Key + "</AttributeID><AttributeValue>" + System.Security.SecurityElement.Escape(dic.Value) + "</AttributeValue></Attribute>");
                }
            }
            xmlStringData.Append("</Attributes>");
            return BackgroundProcessOrderManager.ValidatePageData(View.TenantId, xmlStringData, IsCustomFormScreen, languageCode);
        }

        #region Order Flow
        public AppointmentSlotContract ReserveSlotForEventCodeType(int reservedSlotID, int selectedSlotID)
        {

            return FingerPrintSetUpManager.ReserveSlotForEventCodeType(reservedSlotID, selectedSlotID, View.CurrentLoggedInUserId);
        }
        #endregion

        public List<CustomFormAutoFillDataContract> GetConditionsforAttributes(StringBuilder xmlStringData)
        {
            return BackgroundProcessOrderManager.GetConditionsforAttributes(View.TenantId, xmlStringData, View.LanguageCode);
        }

        public void SaveCustomFormApplicantData(string dataXmlString, int applicantUserId)
        {
            BackgroundProcessOrderManager.SaveCustomFormApplicantData(View.TenantId, dataXmlString, applicantUserId, View.CurrentLoggedInUserId);
        }
    }
}
