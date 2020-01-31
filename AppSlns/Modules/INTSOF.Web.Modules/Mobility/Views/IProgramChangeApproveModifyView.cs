using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using System.Data.Entity.Core.Objects;
using INTSOF.UI.Contract.ComplianceOperation;
namespace CoreWeb.Mobility.Views
{
    public interface IProgramChangeApproveModifyView
    {
        #region Public Properties

        Int32 TenantId { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 UserId { get; set; }
        Int32 PreviousSubscriptionId { get; set; }
        //List<DeptProgramPackage> DeptProgramPackages { get; set; }
        List<AvailablePackageContarct> DeptProgramPackages { get; set; }
        List<Int32> SelectedHierarchyNodeIds { get; }
        Boolean IsPackageSubscribe { get; set; }
        Int32 NodeId { get; set; }
        DeptProgramPackage DeptProgramPackage { get; set; }
        String PreviousHierarchyNode { get; set; }
        String PreviousPackage { get; set; }           
        List<DeptProgramPackageSubscription> lstDeptProgramPackageSubscription { get; set; }
        DeptProgramPackageSubscription SelectedDeptProgramPackageSubscription { get; }        
        List<GetSourceNodeDeatils> SourceNodeDeatils { get; set; }
        String TargetNodeHierarchyLabel { get; set; }
        List<PackageSubscription> SourceSubscriptionList { get; set; }
        List<usp_SubscriptionChange_Result> MobilitySubscriptionChangeDetails { get; set; }
        String TargetHierarchyLabel { get; set; }
        String ErrorMessage {get;set;}
        String SuccessMessage { get;set;}
        Int32 TargetNodeID { get; set; }
        Boolean IsOnlyPackageChange{ get; set; } //UAT-2387
        #endregion
    }
}




