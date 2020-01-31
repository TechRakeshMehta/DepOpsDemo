#region NameSpaces

#region system defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Project Specific
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public class ManageBkgpackageDetailsPresenter : Presenter<IManageBkgpackageDetailsView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            GetBackgroundPackageDetail();
        }

        public void GetBackgroundPackageDetail()
        {
            var packageDetails = BackgroundSetupManager.GetBackgroundPackageDetail(View.BkgPackageHierarchyMappingId, View.TenantId);
            if (packageDetails.IsNotNull())
            {
                String code = PackageAvailability.AVAILABLE_FOR_ORDER.GetStringValue();
                View.PackageName = packageDetails.BackgroundPackage.BPA_Name;
                View.PackageLabel = packageDetails.BackgroundPackage.BPA_Label;
                View.PackageTypeId = Convert.ToInt32(packageDetails.BackgroundPackage.BPA_BkgPackageTypeId); //UAT-3525
                View.BkgPackageId = packageDetails.BPHM_BackgroundPackageID;
                View.BasePrice = packageDetails.BPHM_PackageBasePrice;
                View.IsPackageExclusive = packageDetails.BPHM_IsExclusive;
                View.TransmitToVendor = packageDetails.BPHM_TransmitToVendor;
                View.RequireFirstReview = packageDetails.BPHM_NeedFirstReview;
                View.Instruction = packageDetails.BPHM_Instructions;
                View.PriceText = packageDetails.BPHM_CustomPriceText;
                View.MaxNumberOfYearforResidence = packageDetails.BPHM_MaxNumberOfYearforResidence;
                View.Passcode = packageDetails.BackgroundPackage.BPA_Passcode; //UAT-3771
                ////UAT-3268
                View.IsReqToQualifyInRotation = packageDetails.BackgroundPackage.BPA_IsReqToQualifyInRotation;

                if (packageDetails.BackgroundPackage.BPA_IsReqToQualifyInRotation)
                {
                    View.IsAdditionalPriceAvailable = packageDetails.BPHM_IsAdditionalPriceAvailable;
                    if (!packageDetails.BPHM_IsAdditionalPriceAvailable.IsNullOrEmpty() && Convert.ToBoolean(packageDetails.BPHM_IsAdditionalPriceAvailable))
                    {
                        View.AdditionalPrice = packageDetails.BPHM_AdditionalPrice;
                        View.SelectedAdditonalPaymentOptionID = packageDetails.BPHM_AdditionalPricePaymentOptionID;
                    }
                }

                // View.SelectedSupplemantalTypeID = (packageDetails.BPHM_PkgSupplementalTypeID.IsNotNull()) ? packageDetails.BPHM_PkgSupplementalTypeID :;
                if ((packageDetails.BPHM_PkgSupplementalTypeID.IsNotNull()))
                {
                    View.SelectedSupplemantalTypeID = packageDetails.BPHM_PkgSupplementalTypeID;
                }
                if (packageDetails.lkpPackageAvailability.IsNotNull() && packageDetails.lkpPackageAvailability.PA_Code == code)
                {
                    View.IsBackgroundPkgAvailableForOrder = true;
                }
                else
                {
                    View.IsBackgroundPkgAvailableForOrder = false;
                }
                View.BackgroundPackage = packageDetails.BackgroundPackage;
                View.IsBackgroundPkgAvailableForHRPortal = packageDetails.BPHM_IsAvailableForAdminEntry;
            }
        }

        public Boolean CheckIfResidentialHistoryAttributeGroupsMappedWithPkg()
        {
            List<MappedResidentialHistoryAttributeGroupsWithPkg> listAttrGrp = BackgroundSetupManager.GetMappedResidentialHistoryAttributeGroupsWithPkg(View.TenantId, View.BkgPackageId);
            if (listAttrGrp.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SupplementalTypeList()
        {
            var temSupplementList = BackgroundSetupManager.SupplementalTypeList(View.TenantId);
            //temSupplementList.Insert(0, new lkpPackageSupplementalType { PST_ID = 0, PST_Name = "--SELECT--" });
            View.LstSupplemantalType = temSupplementList;
        }
        public void UpdatePackageDetails()
        {
            List<lkpPackageAvailability> pkgAvailability = ComplianceSetupManager.GetPackageAvailablity(View.TenantId);
            Int32 paID = 0;
            String code = String.Empty;
            if (View.IsBackgroundPkgAvailableForOrder)
            {
                code = PackageAvailability.AVAILABLE_FOR_ORDER.GetStringValue();
                paID = pkgAvailability.FirstOrDefault(x => x.PA_Code == code).PA_ID;
            }
            else
            {
                code = PackageAvailability.NOT_AVAILABLE_FOR_ORDER.GetStringValue();
                paID = pkgAvailability.FirstOrDefault(x => x.PA_Code == code).PA_ID;
            }

            BkgPackageHierarchyMapping bkgPackageHierarchyMapping = new BkgPackageHierarchyMapping
            {
                BPHM_PackageBasePrice = View.BasePrice,
                BPHM_IsExclusive = View.IsPackageExclusive,
                BPHM_TransmitToVendor = View.TransmitToVendor,
                BPHM_NeedFirstReview = View.RequireFirstReview,
                // BPHM_PkgSupplementalTypeID = (View.SelectedSupplemantalTypeID != AppConsts.NONE) ? View.SelectedSupplemantalTypeID : (short?)null,
                BPHM_PkgSupplementalTypeID = View.SelectedSupplemantalTypeID,
                BPHM_Instructions = View.Instruction,
                BPHM_CustomPriceText = View.PriceText,
                BPHM_MaxNumberOfYearforResidence = View.MaxNumberOfYearforResidence,
                BPHM_ModifiedByID = View.CurrentLoggedInUserId,
                BPHM_ModifiedOn = DateTime.Now,
                BPHM_PackageAvailabilityID = paID,
                BPHM_IsAvailableForAdminEntry=View.IsBackgroundPkgAvailableForHRPortal,
                //UAT-2073
                BPHM_PaymentApprovalID = View.PaymentApprovalID,
                //UAT-3268
                BPHM_IsAdditionalPriceAvailable = View.IsAdditionalPriceAvailable,
                BPHM_AdditionalPrice = View.AdditionalPrice,
                BPHM_AdditionalPricePaymentOptionID = View.SelectedAdditonalPaymentOptionID,
            };
            //UAT-2777

            AutomaticPackageInvitation objAutomaticPackageInvitation = new AutomaticPackageInvitation();

            bkgPackageHierarchyMapping.BackgroundPackage = new BackgroundPackage()
            {
                BPA_Name = View.PackageName,
                BPA_Label = View.PackageLabel,
                BPA_BkgPackageTypeId = View.PackageTypeId,
                BPA_Passcode = View.Passcode, //UAT-3771
            };
            if (BackgroundSetupManager.UpdatePackageHirarchyDetails(bkgPackageHierarchyMapping, View.BkgPackageHierarchyMappingId, View.CurrentLoggedInUserId, View.lstPaymentOptionIds, View.TenantId, View.SelectedBkgPackageIdList, Convert.ToInt32(View.AutomaticInvitationMonth), View.isAutomaticPackageInvitationActive)) //UAT-2388: Save Package Invitation Setting on hierarchy
            {
                View.SuccessMessage = "Package Details and Package Payment Options saved successfully.";
            }
            else
            {
                View.ErrorMessage = "Some error occurred. Please try again.";
            }
        }

        #region UAT:2388
        public void GetBkgPackages()
        {
            List<BackgroundPackage> tempBkgPackages = new List<BackgroundPackage>();
            if (View.TenantId > AppConsts.NONE && View.BkgPackageId > AppConsts.NONE)
            {
                tempBkgPackages = BackgroundSetupManager.GetAutomaticInvitationBackgroundPackages(View.TenantId, View.BkgPackageId);
            }
            if (!tempBkgPackages.IsNullOrEmpty())
            {
                tempBkgPackages = tempBkgPackages.OrderBy(col => col.BPA_Name).ToList();
            }
            View.lstBackgroundPackage = tempBkgPackages;
        }

        public void GetAutomaticPackageInvitationSetting()
        {
            View.isAutomaticPackageInvitationActive = BackgroundSetupManager.GetAutomaticPackageInvitationSetting(View.TenantId, View.BkgPackageId);
        }
        #endregion

        #region UAT-3268
        public void BindAdditionalPricePaymentOption()
        {
            var lstPaymentOptions = LookupManager.GetLookUpData<Entity.ClientEntity.lkpPaymentOption>(View.TenantId).Where(con => con.IsDeleted == false).ToList();
            if (!lstPaymentOptions.IsNullOrEmpty())
            {
                View.AdditionalPaymentOptions = lstPaymentOptions.Where(cond => cond.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()
                                                                             || cond.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()).ToList();
            }
        }
        #endregion

        #region UAT-3525
        public List<BkgPackageType> GetBkgPackageType()
        {
            return BackgroundSetupManager.GetAllBkgPackageTypes(View.TenantId).ToList();
        }
        #endregion
    }
}
