#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;
using System.Data.Entity.Core.Objects;

using System;

#endregion

#region UserDefined

using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;

#endregion

#endregion


namespace CoreWeb.ComplianceAdministration.Views
{
    public class RuleAssociationViewerPresenter : Presenter<IRuleAssociationViewerView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }
        public void GetListOfInstanceWichCanShareRule()
        {
            //as for master database we do not need rule sharing.
            if (View.SelectedTenantId == SecurityManager.DefaultTenantID)
            {
                View.PackageListForSharingRuleInstance = null;
            }
            else
            {
                String HID = String.Empty;
                if (View.ObjectType == "ATR")
                {
                    HID = "1-" + View.PackageId + "|2-" + View.CurrentCategoryID + "|3-" + View.CurrentItemID + "|4-" + View.ObjectId;
                }
                else if (View.ObjectType == "ITM")
                {
                    HID = "1-" + View.PackageId + "|2-" + View.CurrentCategoryID + "|3-" + View.ObjectId;
                }
                else if (View.ObjectType == "CAT")
                {
                    HID = "1-" + View.PackageId + "|2-" + View.ObjectId;
                }
                View.PackageListForSharingRuleInstance = RuleManager.GetListOfInstanceWichCanShareRule(View.RuleSetId, View.SelectedTenantId, HID);
                CompliancePackage compliancePackage = GetCompliancePackageByPackageId();
                if (compliancePackage.IsNotNull())
                    View.PackageListForSharingRuleInstance.Add(compliancePackage);

            }
        }
        public Boolean DeleteRuleMapping(Int32 RuleMappingId)
        {
            RuleManager.DeleteRuleMapping(RuleMappingId, View.CurrentLoggedInUserId, View.SelectedTenantId);
            return true;
        }

        public List<RuleSetData> GetRuleSetDataByObjectId()
        {
            return RuleManager.GetRuleSetDataByObjectId(View.SelectedTenantId, View.ObjectId, View.ObjectType);
        }


        public Boolean IsRuleAssociationExists(Int32 AffectedRuleId)
        {
            return RuleManager.IsRuleAssociationExists(View.SelectedTenantId, AffectedRuleId);
        }
        public CompliancePackage GetCompliancePackageByPackageId()
        {
            return RuleManager.GetCompliancePackageByPackageId(View.SelectedTenantId, View.PackageId);
        }
    }
}
