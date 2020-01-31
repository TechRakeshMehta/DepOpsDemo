using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;

namespace CoreWeb.ProfileSharing.Views
{
    public class SharedUserInvitationDetailPanelPresenter : Presenter<ISharedUserInvitationDetailPanelView>
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Get Subscription Package details and package detail
        /// </summary>
        public void GetClientCompliancePackageBySubscription()
        {
            View.Subscription = ComplianceDataManager.GetPackageSubscriptionByID(View.SelectedTenantID, View.PackageSubscriptionId);
        }
        public String GetCatIncompleteStatusName()
        {
            lkpCategoryComplianceStatu lkpCategoryComplianceStatus = ComplianceDataManager.GetCategoryComplianceStatusByCode(ApplicantCategoryComplianceStatus.Incomplete.GetStringValue(), View.SelectedTenantID);
            return lkpCategoryComplianceStatus.Name;
        }

        /// <summary>
        /// Get Immunization data from snapshot.
        /// </summary>
        /// <param name="incompleteStatusName"></param>
        public void GetImmunizationDataFromSnapshot(String incompleteStatusName, Boolean optionalCategoryClientSettingValue)
        {
            if (View.SelectedTenantID > AppConsts.NONE && View.SnapShotID > AppConsts.NONE)
            {
                DataSet immunizationSnapShotData = ProfileSharingManager.GetImmunizationDataFromSnapshot(View.SelectedTenantID, View.SnapShotID);
                if (immunizationSnapShotData.Tables.Count > AppConsts.NONE)
                {
                    List<Int32> lstCategoryId = new List<Int32>();

                   List<lkpCategoryExceptionStatu> lstCategoryExceptionStatus = LookupManager.GetLookUpData<lkpCategoryExceptionStatu>(View.SelectedTenantID).Where(cnd => cnd.CES_IsDeleted == false).ToList();
                   List<lkpCategoryComplianceStatu> lstCategoryComplianceStatus = LookupManager.GetLookUpData<lkpCategoryComplianceStatu>(View.SelectedTenantID).Where(cnd => cnd.IsDeleted == false).ToList();
                   List<lkpPackageComplianceStatu> lstPackageCompStatus = LookupManager.GetLookUpData<lkpPackageComplianceStatu>(View.SelectedTenantID).ToList();
                   List<lkpItemComplianceStatu> lstItemComplianceStatus = LookupManager.GetLookUpData<lkpItemComplianceStatu>(View.SelectedTenantID).Where(cnd => cnd.IsDeleted == false).ToList();
                   List<lkpComplianceAttributeDatatype> lstComplianceAttributeDatatType = LookupManager.GetLookUpData<lkpComplianceAttributeDatatype>(View.SelectedTenantID).ToList();

                    View.SharedInvitationComplianceSnapShotDetail = new SharedUserInvitationDetailContarct(immunizationSnapShotData, incompleteStatusName, lstCategoryExceptionStatus,
                                                                         lstCategoryComplianceStatus, lstPackageCompStatus, lstItemComplianceStatus, lstComplianceAttributeDatatType, optionalCategoryClientSettingValue);

                    View.PackageName = immunizationSnapShotData.Tables[0].Rows[0]["PackageLabel"].ToString().IsNullOrEmpty() ? immunizationSnapShotData.Tables[0].Rows[0]["PackageName"].ToString() : immunizationSnapShotData.Tables[0].Rows[0]["PackageLabel"].ToString();
                    #region Set categoryIds
                    DataTable categoryData = immunizationSnapShotData.Tables[1];

                    lstCategoryId = categoryData.AsEnumerable().DistinctBy(x => Convert.ToString(x["CategoryID"])).Select(y => Convert.ToInt32(y["CategoryID"])).ToList();

                    View.SnapShotcategoryIds = GetConcatCategoryIds(lstCategoryId);
                    #endregion
                }
            }
        }

        public String GetConcatCategoryIds(List<Int32> lstCategoryId)
        {
            StringBuilder strBuilder = new StringBuilder();
            String sharedCategories = String.Empty;
            lstCategoryId.ForEach(cond =>
            {
                strBuilder.Append(Convert.ToString(cond) + ",");
            });
            if (!strBuilder.IsNullOrEmpty())
                sharedCategories = strBuilder.ToString().Remove(strBuilder.Length - 1).TrimEnd();
            return sharedCategories;
        }

        /// <summary>
        /// UAT 3106
        /// </summary>
        /// <returns></returns>
        public Boolean GetClientSettingByCode()
        {
            ClientSetting optionalCategoryClientSetting = ComplianceDataManager.GetClientSetting(View.SelectedTenantID, Setting.EXECUTE_COMPLIANCE_RULE_WHEN_OPTIONAL_CATEGORY_COMPLIANCE_RULE_MET.GetStringValue());
            if (!optionalCategoryClientSetting.IsNullOrEmpty())
            {
                return optionalCategoryClientSetting.CS_SettingValue == AppConsts.STR_ONE ? true : false;
            }
            else
            {
                return false;
            } 
        }

        /// <summary>
        /// UAT 3683
        /// </summary>
        /// <returns></returns>
        public Boolean GetOptionalCategorySettingForNode()
        {
            return ComplianceDataManager.GetOptionalCategorySettingForNode(View.SelectedTenantID, AppConsts.NONE, View.PackageSubscriptionId, SubscriptionTypeCategorySetting.COMPLIANCE_PACKAGE.GetStringValue());
        }
        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}
