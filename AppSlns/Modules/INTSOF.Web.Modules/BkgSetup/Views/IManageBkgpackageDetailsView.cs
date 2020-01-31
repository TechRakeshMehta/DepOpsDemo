#region NameSpaces

#region system defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Project Specific
using Entity.ClientEntity;
#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public interface IManageBkgpackageDetailsView
    {
        Int32 TenantId { get; set; }
        Int32 BkgPackageHierarchyMappingId { get; set; }
        Int32 BkgPackageId { get; set; }
        Int32 ParentNodeId { get; set; }

        String PackageName { get; set; }

        /// <summary>
        /// Represents the Background Package Label
        /// </summary>
        String PackageLabel { get; set; }
        Int32? PackageTypeId { get; set; } //UAT-3525
        Decimal? BasePrice { get; set; }
        Boolean IsPackageExclusive { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Boolean? TransmitToVendor { get; set; }
        Boolean? RequireFirstReview { get; set; }
        List<lkpPackageSupplementalType> LstSupplemantalType { get; set; }
        Int16? SelectedSupplemantalTypeID { get; set; }
        String Instruction { get; set; }
        String PriceText { get; set; }
        Int32? MaxNumberOfYearforResidence { get; set; }
        String ErrorMessage
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }
        String InfoMessage
        {
            get;
            set;
        }

        Boolean IsBackgroundPkgAvailableForOrder
        {
            get;
            set;
        }

        Boolean IsBackgroundPkgAvailableForHRPortal { get; set; }

        /// <summary>
        /// Stores the list of Payment OptionIds selected at the Package Level
        /// </summary>
        List<Int32> lstPaymentOptionIds
        {
            get;
            set;
        }
        //UAT-2073
        Int32 PaymentApprovalID
        {
            get;
            set;
        }
        #region Package DropDown :UAT-2388
        List<BackgroundPackage> lstBackgroundPackage
        {
            set;
            get;
        }

        List<Int32> SelectedBkgPackageIdList
        {
            get;
            set;
        }
        String AutomaticInvitationMonth
        {
            get;
            set;
        }
        BackgroundPackage BackgroundPackage { get; set; }

        Boolean isAutomaticPackageInvitationActive { get; set; }
        #endregion

        //UAT-3268
        Boolean IsReqToQualifyInRotation { get; set; }
        Boolean? IsAdditionalPriceAvailable { get; set; }
        Decimal? AdditionalPrice { get; set; }
        Int32? SelectedAdditonalPaymentOptionID { get; set; }
        List<lkpPaymentOption> AdditionalPaymentOptions { get; set; }


        String Passcode { get; set; }  //UAT-3771

    }
}
