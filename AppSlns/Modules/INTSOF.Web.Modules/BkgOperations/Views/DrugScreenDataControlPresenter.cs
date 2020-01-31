#region NameSpaces

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Project Specific
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.Utils;
#endregion

#endregion

namespace CoreWeb.BkgOperations.Views
{
    public class DrugScreenDataControlPresenter : Presenter<IDrugScreenDataControlView>
    {

        public void GetAttributeListByGroupId()
        {
            View.LstBkgAttributeGroupMapping = BackgroundProcessOrderManager.GetAttributeListByGroupId(View.TenantId, View.AttributeGroupId);
        }

        public void GetCustomHtmlContent()
        {
            Entity.CustomFormAttributeGroup eDrugCustomFormHtml = BackgroundProcessOrderManager.GetEDrugScreeningHtml(View.TenantId, View.CustomFormId, View.AttributeGroupId);
            if (!eDrugCustomFormHtml.IsNullOrEmpty())
                View.CustomHtml = eDrugCustomFormHtml.CFAG_CustomHTML;
            else
                View.CustomHtml = String.Empty;

        }

        public string GetRegistrationIdAttributeName()
        {
            Guid attGrpMappingCode = new Guid(AppConsts.DRUG_SCREEN_REGISTRATION_ID);
            return BackgroundProcessOrderManager.GetRegistrationIdAttributeName(View.TenantId, attGrpMappingCode, View.AttributeGroupId);
        }

        public Boolean ProceedDrugScreeningOrderWithoutRegisterationId(Int32 tenantId)
        {
            Entity.ClientEntity.ClientSetting clientsettings = ComplianceDataManager.GetClientSetting(tenantId, Setting.DRUG_SCREENING_ORDER_PROCEED.GetStringValue());
            if (clientsettings.IsNotNull())
            {
                return (!String.IsNullOrEmpty(clientsettings.CS_SettingValue) && clientsettings.CS_SettingValue == AppConsts.STR_ONE) ? true : false;
            }
            else
            {
                return false;
            }
        }

        #region UAT-3669
        public Boolean UpdateBlockedOrdersHistoryData(CoreWeb.IntsofSecurityModel.SysXMembershipUser user, Int32 selectedHierarchyNodeID, String selectedPackageIds)
        {
            if (View.TenantId > AppConsts.NONE)
            {
                Int32 applicantOrgUserId = user.OrganizationUserId;
                String firstName = user.FirstName;
                String lastName = user.LastName;
                String blockedReasonCode = lkpBlockedOrderReason.WebCCF_Didnot_Supplied_Registration_Id.GetStringValue();


                SecurityManager.UpdateBlockedOrdersHistoryData(View.TenantId, selectedHierarchyNodeID, selectedPackageIds, applicantOrgUserId, firstName, lastName, blockedReasonCode);

                return true;
            }
            return false;
        }


        #endregion


    }
}
