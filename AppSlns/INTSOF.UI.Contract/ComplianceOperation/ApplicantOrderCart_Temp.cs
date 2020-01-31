using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    //public class ApplicantOrderCart_Temp
    //{
    //    #region Properties

    //    /// <summary>
    //    /// List of all the orders in the Cart
    //    /// </summary>
    //    public List<ApplicantOrder_Temp> lstApplicantOrder { get; set; }

    //    /// <summary>
    //    /// Complete Total of All the orders
    //    /// </summary>
    //    public Decimal? GrandTotal { get; set; }

    //    /// <summary>
    //    /// Overall Invoice Number of All the Orders
    //    /// </summary>
    //    public String MasterInvoiceNumber { get; set; }

    //    /// <summary>
    //    /// Data of Custom Attributes, while order placement
    //    /// </summary>
    //    List<TypeCustomAttributes> lstCustomAttributeValues { get; set; }

    //    /// <summary>
    //    ///User Profile Data during order placement 
    //    /// </summary>
    //    public OrganizationUserProfile OrganizationUserProfile { get; set; }

    //    /// <summary>
    //    /// Property to Check whether to update the personal details
    //    /// </summary>
    //    public Boolean UpdatePersonalDetails { get; set; }

    //    /// <summary>
    //    /// Machine IP Address
    //    /// </summary>
    //    public String ClientMachineIP { get; set; }

    //    /// <summary>
    //    /// Order stage Tracking Id
    //    /// </summary>
    //    public List<Int32> lstOrderStageTrackID { get; set; }

    //    /// <summary>
    //    /// Property to Check whether Online Order is success or not
    //    /// </summary>
    //    public Boolean IsOnlineOrderFailed { get; set; }

    //    /// <summary>
    //    /// DPM-Id of the last node selected in the pending order hierarchy.
    //    /// </summary>
    //    public Int32? SelectedHierarchyNodeID { get; set; }

    //    /// <summary>
    //    /// Master Order Id
    //    /// </summary>
    //    public Int32 MasterOrderId { get; set; }


    //    #region Can remove one of these lists

    //    /// <summary>
    //    /// Store the CSV of the selected node id's on each level i.e for each combo
    //    /// </summary>
    //    public ArrayList alNodeIds { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public List<Int32> lstDepProgramMappingId { get; set; }

    //    /// <summary>
    //    /// Institution Id of the last node selected in the pending order screen. Used to get the associated Custom attributes for this institution.
    //    /// </summary>
    //    public Int32 NodeId { get; set; }

    //    #endregion

    //    #region MAY NOT BE REQUIRED

    //    /// <summary>
    //    /// Not In Use
    //    /// </summary>
    //    //public Int32 DepartmentId { get; set; }

    //    /// <summary>
    //    /// May Not be required
    //    /// </summary>
    //    //public Int32? DefaultNodeId { get; set; } 

    //    #endregion

    //    #endregion

    //    #region Methods

    //    //public ApplicantOrder_Temp GetApplicantOrder()
    //    //{
    //    //    if (this.lstApplicantOrder == null)
    //    //        this.lstApplicantOrder = new List<ApplicantOrder_Temp>();

    //    //    if (this.lstApplicantOrder.Count == 0)
    //    //        this.lstApplicantOrder.Add(new ApplicantOrder_Temp());

    //    //    return this.lstApplicantOrder[0];

    //    //}

    //    /// <summary>
    //    /// Set Organization User Profile data to Session properties, based on the settings
    //    /// </summary>
    //    /// <param name="organizationUserProfile"></param>
    //    /// <param name="updatePersonalDetails"></param>
    //    /// <param name="clientMachineIP"></param>
    //    public void AddOrganizationUserProfile(OrganizationUserProfile organizationUserProfile, Boolean updatePersonalDetails, String clientMachineIP = null)
    //    {
    //        this.OrganizationUserProfile = organizationUserProfile;
    //        this.UpdatePersonalDetails = updatePersonalDetails;

    //        if (clientMachineIP.IsNotNull())
    //        {
    //            this.ClientMachineIP = clientMachineIP;
    //        }
    //    }

    //    /// <summary>
    //    /// Set data of Custom attributes for the order
    //    /// </summary>
    //    /// <param name="lstTypeCustomAttributes"></param>
    //    public void AddCustomAttributeValues(List<TypeCustomAttributes> lstTypeCustomAttributes)
    //    {
    //        if (lstTypeCustomAttributes.IsNotNull() && lstTypeCustomAttributes.Count() > 0)
    //            this.lstCustomAttributeValues = lstTypeCustomAttributes;
    //    }

    //    /// <summary>
    //    /// Get the Custom Attribute Values
    //    /// </summary>
    //    /// <returns></returns>
    //    public List<TypeCustomAttributes> GetCustomAttributeValues()
    //    {
    //        if (this.lstCustomAttributeValues.IsNotNull() && lstCustomAttributeValues.Count() > 0)
    //            return this.lstCustomAttributeValues;
    //        else
    //            return new List<TypeCustomAttributes>();
    //    }

    //    /// <summary>
    //    /// Clear the data of Custom Attributes
    //    /// </summary>
    //    public void ClearCustomAttributeValues()
    //    {
    //        if (this.lstCustomAttributeValues.IsNotNull())
    //            lstCustomAttributeValues = null;
    //    }

    //    /// <summary>
    //    /// Add Current stage of the order
    //    /// </summary>
    //    /// <param name="orderStageID"></param>
    //    public void AddOrderStageTrackID(Int32 orderStageID)
    //    {
    //        if (this.lstOrderStageTrackID.IsNull())
    //        {
    //            this.lstOrderStageTrackID = new List<Int32>()
    //            {
    //                orderStageID
    //            };
    //        }
    //        else if (this.lstOrderStageTrackID.Last() != orderStageID)
    //        {
    //            this.lstOrderStageTrackID.Add(orderStageID);
    //        }
    //    }

    //    /// <summary>
    //    /// Clear the order cart in case of restart
    //    /// </summary>
    //    /// <param name="applicantOrderCart"></param>
    //    public void ClearOrderCart(ApplicantOrderCart_Temp applicantOrderCart)
    //    {
    //        if (applicantOrderCart.IsNotNull())
    //            applicantOrderCart = new ApplicantOrderCart_Temp();
    //    }

    //    public void ClearPackageSelectionData()
    //    {
    //        this.lstDepProgramMappingId = null;
    //        this.GrandTotal = null;
    //        //this.Amount = null;
    //        //this.RushOrderPrice = null;
    //        //this.ProgramDuration = null;
    //        this.alNodeIds = null;
    //        //this.DPP_Id = null;
    //        //this.DefaultNodeId = null;
    //        this.ClearCustomAttributeValues();
    //        if (lstApplicantOrder.IsNotNull() && lstApplicantOrder.Count > 0)
    //        {
    //            lstApplicantOrder[0].DPPS_Id = null;
    //        }
    //    }

    //    #endregion
    //}

    //public class ApplicantOrder_Temp
    //{
    //    /// <summary>
    //    /// Need to Finalize
    //    /// </summary>
    //    public Decimal SettleAmount { get; set; }

    //    /// <summary>
    //    /// Is the Background package selected is Exclusive or Not.
    //    /// </summary>
    //    public Boolean IsExclusiveBackgroundPackage { get; set; }

    //    /// <summary>
    //    /// Compliance or Background Package
    //    /// </summary>
    //    public String PackageType { get; set; }

    //    /// <summary>
    //    /// Unique Order Id
    //    /// </summary>
    //    public Int32 OrderId { get; set; }

    //    /// <summary>
    //    /// Previous Order Id of a particular order
    //    /// </summary>
    //    public Int32 PrevOrderId { get; set; }

    //    /// <summary>
    //    /// Renewal Duration of a particular Order.
    //    /// </summary>
    //    public Int32 RenewalDuration { get; set; }

    //    public String Amount { get; set; }

    //    /// <summary>
    //    /// Rush Order Price for Package, currently Compliance
    //    /// </summary>
    //    public String RushOrderPrice { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Boolean IsRushOrderIncluded { get; set; }

    //    /// <summary>
    //    /// Order level Invoice Number
    //    /// </summary>
    //    public String OrderInvoiceNumber { get; set; }

    //    public Int32? ProgramDuration { get; set; }

    //    /// <summary>
    //    /// May get removed
    //    /// </summary>
    //    // public Int32 SelectedDeptProgramId { get; set; }

    //    /// <summary>
    //    /// Price of the Package selected, currently Compliance Package
    //    /// </summary>
    //    public Decimal? CurrentPackagePrice { get; set; }

    //    /// <summary>
    //    /// Used to distinguish the Renewal, new order and change program orders
    //    /// </summary>
    //    public String OrderRequestType { get; set; }

    //    /// <summary>
    //    /// DepartmentProgramPackageId - Mapping Id of the DPP_CompliancePackageID AND DPP_DeptProgramMappingID.
    //    /// This is the Id of the selected DropDown Value, in Packages dropdown, for Compliance Packages
    //    /// </summary>
    //    public Int32? DPP_Id { get; set; }

    //    /// <summary>
    //    /// MappingId of the BkgPackageHierarchyMapping Table - Mapping of BPHM_InstitutionHierarchyNodeID & BPHM_BackgroundPackageID. 
    //    /// Will be unique for each package selected, for Background related Packages 
    //    /// </summary>
    //    public Int32? BPHM_Id { get; set; }

    //    ///// <summary>
    //    ///// DPPS_ID of selected subscription of Compliance package - Mapping ID of DPPS_DeptProgramPackageID and DPPS_SubscriptionID.
    //    ///// </summary>
    //    //public List<Int32> DPPS_Id { get; set; }

    //    /// <summary>
    //    /// DPPS_ID of selected subscription of Compliance package - Mapping ID of DPPS_DeptProgramPackageID and DPPS_SubscriptionID.
    //    /// </summary>
    //    public Int32? DPPS_Id { get; set; }

    //    /// <summary>
    //    /// Disclosure Document Id
    //    /// </summary>
    //    public Int32? DisclosureDocumentId { get; set; }

    //    /// <summary>
    //    /// Disclaimer document Id, common for all Orders
    //    /// </summary>
    //    public List<Int32?> DisclaimerDocumentId { get; set; }
    //}
}
