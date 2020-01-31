using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.DashBoard.Views
{
    public interface IApplicantDashboardMainView
    {
        //UAT-2302-As a student, I should see a popup on login if i have logged in using a non-preferred browser.
        Boolean IsDisplayNonPreferredOption
        {
            // get;
            set;
        }

        Int32 loggedInUserId
        {
            get;
        }

        String FilePath
        {
            get;
        }

        String OriginalFileName
        {
            get;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }

        Boolean IsAdminView
        {
            get;
            set;
        }

        //List<Entity.ClientEntity.PackageSubscription> SubscribedPackages
        //{
        //    get;
        //    set;
        //}


        List<Entity.ClientEntity.PackageSubscription> SubscribedPackages
        {
            get;
        }


        List<Entity.ClientEntity.PackageSubscription> SubscribedPackagesAll
        {
            get;
            set;
        }

        //UAT-4067
        List<Int32> selectedNodeIDs
        {
            get;
            set;
        }

        List<String> allowedFileExtensions
        {
            get;
            set;
        }

        //List<vwOrderDetail> LstCmpltBkgOrderDetail
        //{
        //    get;
        //    set;
        //}

        //List<vwOrderDetail> LstPaidBkgOrderDetail
        //{
        //    get;
        //    set;
        //}

        List<vwOrderDetail> LstCmpltBkgOrderDetail
        {
            get;
        }

        /// <summary>
        /// Count of Paid Bkg orders
        /// </summary>
        Int32 lstPaidBkgOrderDetailCount
        {
            get;
        }


        List<vwOrderDetail> LstAllBkgOrderDetail
        {
            get;
            set;
        }

        String ShowDocumentVideo
        {
            get;
            set;
        }

        String ShowDataEnteredVideo
        {
            get;
            set;
        }

        //Int32 ClientSettingBeforeExpiry
        //{
        //    get;
        //    set;
        //}
        List<SubscriptionFrequency> lstSubscriptionFrequency
        {
            get; set;
        }

        String ParentWorkQueue
        {
            get;
            set;
        }

        Int32 SelectedPkgSubscriptionId
        {
            get;
            set;
        }

        Int32 ApplicantId
        {
            get;
            set;
        }

        Int32 ApplicantTenantId
        {
            get;
            set;
        }

        List<PackageSubscription> ImmunizationTrackingPackages
        {
            get;
        }

        List<PackageSubscription> AdministrativeTrackingPackages
        {
            get;
        }


        Boolean IfUserhasImmunizationPkg
        {
            get;
            set;
        }

        Boolean IfUserHasAdministrativePkg
        {
            get;
            set;
        }

        Boolean IsApplicantClinicalRotationMember
        {
            get;
            set;
        }

        List<Int32> lstBulletinID { get; set; }
        Boolean IsNotPostBack { get; set; }

        Int32 OrgUsrID { get; }

        #region UAT-1812:Creation of an Approval/rejection summary for applicant logins
        /// <summary>
        /// Gets the current user Session id.
        /// </summary>
        /// <remarks></remarks>
        String CurrentSessionId { get; }
        #endregion

        //UAT-1834:NYU Migration 2 of 3: Applicant Complete Order Process.   
        BulkOrderUpload BulkOrderData { get; set; }
        //UAT-2003: Add ability to extend/renew when clicking "place order"
        List<vwSubscription> ListSubscription { get; set; }

        /// <summary>
        /// UAT-2218, Creation of a "Required Documents" tab on the left side of the student dashboard. 
        /// </summary>
        Boolean IsRequiredTabVisible { get; set; }

        Boolean ShowSeparateTabForApplicantPersonalDocs { get; set; }

        //UAT-2251: Ability to turn on/off the initial student videos individually and to replace the video link by institution
        Boolean ClientSettingForDisplayInitialVideo { get; set; }

        //UAT-2460
        String SelectedArchiveStateCode { get; set; }

        //UAT-2427
        Boolean IsJobBoardMenuVisible { get; set; }

        //UAT-2930
        String UserId { get; }
        Int32 CurrentLoggedInUserID { get; }

        //Code for preferred language.
        String LanguageCode { get; }
    }
}
