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
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

namespace CoreWeb.ProfileSharing.Views
{
    public class SharedRotationInvitationDetailPanelPresenter : Presenter<ISharedRotationInvitationDetailPanelView>
    {
        /// <summary>
        /// Get Subscription Package details and package detail
        /// </summary>
        public void GetRotationRequirementPackageBySubscription()
        {
            View.Subscription = ProfileSharingManager.GetRequirementPackageSubscription(View.RequirementPackageSubscriptionId, View.SelectedTenantID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="incompleteStatusName"></param>
        public void GetRequirementDataFromSnapshot(String incompleteStatusName, bool extractDataFromSnapshot)
        {
            if (View.SelectedTenantID > AppConsts.NONE && View.SnapShotID > AppConsts.NONE || View.IsInstructorPreceptorPackage)//UAT-3338
            {
                DataSet requirementSnapShotData = new DataSet();
                if (extractDataFromSnapshot)
                    requirementSnapShotData = ProfileSharingManager.GetRequirementDataFromSnapshot(View.SelectedTenantID, View.SnapShotID);
                else
                    requirementSnapShotData = ProfileSharingManager.GetRequirementLiveData(View.SelectedTenantID, View.RequirementPackageSubscriptionId);

                if (requirementSnapShotData.Tables.Count > AppConsts.NONE)
                {

                    //UAT-2164
                    if (!requirementSnapShotData.Tables[0].IsNullOrEmpty() && requirementSnapShotData.Tables[0].Rows.Count > 0)
                        View.RequirementPackageID = Convert.ToInt32(requirementSnapShotData.Tables[0].Rows[0]["RequirementPackageID"]);

                    GetBackgroundDocumentPermissionByReqPkgID();

                    List<Int32> lstCategoryId = new List<Int32>();


                    List<lkpRequirementCategoryStatu> lstCategoryComplianceStatus = LookupManager.GetLookUpData<lkpRequirementCategoryStatu>(View.SelectedTenantID).Where(cnd => cnd.RCS_IsDeleted == false).ToList();
                    List<lkpRequirementPackageStatu> lstPackageCompStatus = LookupManager.GetLookUpData<lkpRequirementPackageStatu>(View.SelectedTenantID).ToList();
                    List<lkpRequirementItemStatu> lstItemComplianceStatus = LookupManager.GetLookUpData<lkpRequirementItemStatu>(View.SelectedTenantID).Where(cnd => cnd.RIS_IsDeleted == false).ToList();
                    List<lkpRequirementFieldDataType> lstComplianceAttributeDatatType = LookupManager.GetLookUpData<lkpRequirementFieldDataType>(View.SelectedTenantID).ToList();

                    View.SharedInvitationRequirementSnapShotDetail = new SharedRotationInvitationDetailContarct(requirementSnapShotData, incompleteStatusName,
                                                                         lstCategoryComplianceStatus, lstPackageCompStatus, lstItemComplianceStatus, lstComplianceAttributeDatatType, View.LstIsBackgroundDocumentContract);

                    if (!requirementSnapShotData.Tables[0].IsNullOrEmpty() && requirementSnapShotData.Tables[0].Rows.Count > 0)
                        View.RequirementPackageName = requirementSnapShotData.Tables[0].Rows[0]["RequirementPackageLabel"].ToString().IsNullOrEmpty() ? requirementSnapShotData.Tables[0].Rows[0]["RequirementPackageName"].ToString()
                                                                                                                                : requirementSnapShotData.Tables[0].Rows[0]["RequirementPackageLabel"].ToString();



                    #region Set categoryIds
                    DataTable categoryData = requirementSnapShotData.Tables[1];

                    lstCategoryId = categoryData.AsEnumerable().DistinctBy(x => Convert.ToString(x["RequirementCategoryID"])).Select(y => Convert.ToInt32(y["RequirementCategoryID"])).ToList();

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
            if (!strBuilder.IsNullOrEmpty() && strBuilder.ToString().Length > 0)
                sharedCategories = strBuilder.ToString().Remove(strBuilder.Length - 1).TrimEnd();
            return sharedCategories;
        }

        /// <summary>
        /// UAT-2164
        /// </summary>
        public void GetBackgroundDocumentPermissionByReqPkgID()
        {
            View.LstIsBackgroundDocumentContract = ProfileSharingManager.GetBackgroundDocumentPermissionByReqPkgID(View.SelectedTenantID, View.RequirementPackageID, View.CurrentLoggedInUserId);
        }

        /// <summary>
        /// UAT-2414, Create a snapshot on Rotation End Date 
        /// </summary>
        public void ShowLiveRotationData()
        {
            ClinicalRotationDetailContract clinicalRotationDatail = ClinicalRotationManager.GetClinicalRotationById(View.SelectedTenantID, View.ClinicalRotationID,null);
            //UAT-2544:
            Boolean isApplicantDropped = ClinicalRotationManager.IsApplicantDroppedFromRotation(View.SelectedTenantID, View.ClinicalRotationID, View.RequirementPackageSubscriptionId
                                                                                               , View.CurrentLoggedInUserId);
            if (!clinicalRotationDatail.IsNullOrEmpty())
            {
                if ((clinicalRotationDatail.EndDate.HasValue && clinicalRotationDatail.EndDate.Value.Date <= DateTime.Now.Date) || isApplicantDropped)
                {
                    View.ShowLiveRotationData = false;
                }
                else
                {
                    View.ShowLiveRotationData = true;
                }
            }
        }
    }
}
