#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IPendingOrderView
    {
        #region Variables

        #region Private Variables



        #endregion

        #region Public Variables



        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties

        Int32 TenantId { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        String CurrentCompliancePackageType { get; set; }

        List<string> AvailableComplaincePackageTypes { get; set; }

        Dictionary<string, ViewCompliancePackage> CompliancePackages { get; set; }

        Int32? DPP_Id { get; set; }

        DeptProgramPackage DeptProgramPackage { get; set; }

        Int32? ProgramDuration
        {
            get;
            set;
        }

        Boolean ShowRushOrder { get; set; }

        Boolean IsPackageSubscribe { get; set; }

        List<DeptProgramPackage> DeptProgramPackages { get; set; }

        List<DeptProgramPackageSubscription> lstDeptProgramPackageSubscription { get; set; }

        List<Entity.Organization> Departments
        {
            set;
        }

        List<Program> ProgramsList { set; }

        Int32 SelectedDepartmentId { get; set; }

        List<Int32> SelectedDepProgramMappingId
        {
            get;
            set;
        }
        List<Int32> SelectedProgramIds
        {
            get;
            set;
        }
        //List<Int32> SelectedHierarchyNodeIds
        Dictionary<Int32, Int32> SelectedHierarchyNodeIds
        {
            get;
            set;
        }

        String InstitutionName
        {
            get;
            set;
        }

        /// <summary>
        /// Get and set next page path.
        /// </summary>
        String NextPagePath
        {
            get;
            set;
        }

        List<DeptProgramMapping> lstHierarchy
        {
            get;
            set;
        }

        Int32 SelectedNodeId
        {
            get;
            set;
        }

        Int32 PreviousOrderId
        {
            get;
            set;
        }

        Decimal SettlementPrice
        {
            get;
            set;
        }

        /// <summary>
        /// Institution Id of the last node selected in the pending order screen. Used to get the associated Custom attributes for this institution.
        /// </summary>
        Int32 NodeId
        {
            get;
            set;
        }

        /// <summary>
        /// Id of the node to which the current package(for which chanhe subscription is selected) belongs to.
        /// </summary>
        Int32 ChangeSubscriptionSourceNodeId { get; set; }

        Int32 ChangeSubscriptionCompliancePackageTypeId { get; set; }


        /// <summary>
        /// DPPId of the node to which the current package(for which chanhe subscription is selected) belongs to.
        /// </summary>
        Int32 ChangeSubscriptionSourceNodeDPPId { get; set; }

        /// <summary>
        /// Id of the node which can be suggested to the user
        /// </summary>
        String ChangeSubscriptionTargetNodeId { get; set; }

        //#region UAT-1214
        // UAT 1545
        //String RequiredPackageLabel
        //{
        //    set;
        //}

        //String OptionalPackageLabel
        //{
        //    set;
        //}

        #region UAT-1545

        /// <summary>
        /// Label for Immnuization Package Section
        /// </summary>
        String ImmnuizationPackageLabel
        {
            set;
        }

        /// <summary>
        /// Label for Administrative Package Section
        /// </summary>
        String AdministrativePackageLabel
        {
            set;
        }

        #endregion
        #region UAT-3601
        String ScreeningHeaderLabel { set; }
        #endregion

        //#endregion

        #region UAT-729 WB: As an applicant, if I have an active Compliance package, that package should not appear as an option in the order process.

        /// <summary>
        /// Property that used to set and get the already purchased package name
        /// </summary>
        String AlreadyPurchasedPackages { get; set; }
        #endregion

        /// <summary>
        /// Used to identify the current Order Request Type i.e. New order, Change subscription etc.
        /// </summary>
        String OrderType
        {
            get;
            set;
        }

        List<PackageBundle> lstPackageBundle
        {
            get;
            set;
        }

        //UAT-3283
        //Int32 SelectedPkgBundleId
        //{
        //    get;
        //    set;
        //}

        List<Int32> lstSelectedBundlePkgId { get; set; }

        //END UAT-3283

        /// <summary>
        /// List of Department Program Packages in a Bundle
        /// </summary>
        List<PackageBundleNodePackage> lstBundleDeptProgramPackages
        {
            get;
            set;
        }

        /// <summary>
        /// List of Background Packages in a Bundle
        /// </summary>
        List<PackageBundleNodePackage> lstBundleBkgPackages
        {
            get;
            set;
        }

        #region UAT-1560:WB: We should be able to add documents that need to be signed to the order process
        Boolean IsAdditionalDocumentExist { get; set; }
        #endregion


        //Boolean IsLocationServiceTenant { get; }
        Dictionary<Int32, Int32> lstLocationHierarchy { get; set; }
        //Int32 SelectedLocationID { get; }
        //String LocationName { get; set; }
        //String LocationAddress { get; set; }
        FingerPrintAppointmentContract FingerPrintData { get; }

        //Boolean IsLocationServiceTenant { get; set; }
        String LanguageCode { get; }
        
        String BillingCode { get; set; }

        KeyValuePair<string,string> CBIBillingCode { get; set; }

        //UAT-3850//
        Decimal BillingCodeAmount {get;}
        
        #endregion

        #endregion

        string ServiceDescription { get; set; }

        #region Methods

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion

        #endregion

        List<ServiceFeeItemRecordContract> lstAdditionalServiceFeeOption
        {
            set;
            get;
        }

    }

    public class Program
    {
        #region Variables

        #region Private Variables



        #endregion

        #region Public Variables



        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties

        public String Name { get; set; }

        public Int32 Id { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion

        #endregion
    }

    [Serializable]
    public class ViewCompliancePackage
    {
        public Int32? DPP_Id { get; set; }

        public DeptProgramPackage DeptProgramPackage { get; set; }

        public Int32? ProgramDuration
        {
            get;
            set;
        }
        public Int32 PreviousOrderId
        {
            get;
            set;
        }

        public Decimal SettlementPrice
        {
            get;
            set;
        }        
    }   
}




