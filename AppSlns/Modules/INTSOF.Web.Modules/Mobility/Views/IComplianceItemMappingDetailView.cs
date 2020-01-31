using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.Mobility;


namespace CoreWeb.Mobility.Views
{
    public interface IComplianceItemMappingDetailView
    {
        /// <summary>
        /// List to bind the From Treeview
        /// </summary>
        List<GetRuleSetTree> lstFromTreeData { get; set; }

        /// <summary>
        /// List to bind the To Treeview
        /// </summary>
        List<GetRuleSetTree> lstToTreeData { get; set; }

        String FromTenantName { get; set; }

        String ToTenantName { get; set; }

        Int32 CurrentUserId { get; }

        Int32 PackageMappingMasterId { get; set; }

        Int32 FromTenantId { get; set; }

        Int32 FromPackageId { get; set; }

        Int32 ToTenantId { get; set; }

        Int32 ToPackageId { get; set; }

        Int16 MappingStatusID { get; set; }

        Int32 FromAttrributeID { get; set; }

        Int32 ToAttrributeID { get; set; }

        Int32 FromItemID { get; set; }

        Int32 ToItemID { get; set; }

        String ComplianceItemMappingXML { get; set; }

        List<Entity.GetComplianceItemMappingDetails_Result> MappedItemList { get; set; }

        Boolean IsMappingDetailsSaved { get; set; }

        List<CmplnceItemMappingDetailContract> MappedItemsDetailsList { get; }

        CmplnceItemMappingDetailContract SetMappedItemsDetailsList
        {
            set;
        }

        /// <summary>
        /// Error Messsge property
        /// </summary>
        String ErrorMessage
        {
            get;
            set;
        }
        
        /// <summary>
        /// Success Message property
        /// </summary>
        String SuccessMessage
        {
            get;
            set;
        }

        List<Entity.lkpPkgMappingStatu> lstMappingStatus
        {
            set;
        }

        /// <summary>
        /// Mapping Name
        /// </summary>
        String MappingName
        {
            get;
            set;
        }

        /// <summary>
        /// Property to differentiate the screen from which user navigates to this screen
        /// </summary>
        String FromQueueType
        {
            get;
            set;
        }

        /// <summary>
        /// Target Package Name
        /// </summary>
        String ToPackageName
        {
            get;
            set;
        }

        /// <summary>
        /// Source Package Name
        /// </summary>
        String FromPackageName
        {
            get;
            set;
        }

        /// <summary>
        /// Skip Mapping, if check box is checked, property value will be set as true else false.
        /// </summary>
        Boolean IsSkipMapping
        {
            get;
            set;
        }

        Int32 FromNodeID
        {
            get;
            set;
        }

        Int32 ToNodeID
        {
            get;
            set;
        }


        String FromNodeName
        {
            get;
            set;
        }

        String ToNodeName
        {
            get;
            set;
        }

    }
}




