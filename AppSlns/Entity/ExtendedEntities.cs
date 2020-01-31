#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  UtilityProvider.cs
// Purpose:    Extended class for Utility Provider.
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Web.UI;
//using System.ComponentModel.DataAnnotations.Schema;


#endregion

#region Application Specific

#endregion

#endregion

namespace Entity
{
    public class LookupTableExtension
    {
        public String EntityName { get; set; }
        public String AssemblyQualifiedName { get; set; }
        public String EntitySetName { get; set; }
    }

    [Serializable]
    public class LookupColumnInformation
    {
        public String ColumnName { get; set; }
        public String ColumnDataType { get; set; }
        public Object MaxLength { get; set; }
        public Boolean IsForeignKey { get; set; }
        public String FKColumnName { get; set; }
        public String FKTableName { get; set; }
        public Boolean IsPrimaryKey { get; set; }
        public String FKTableSetName { get; set; }
        public Object IsRequired { get; set; }

    }

    public class EntityList
    {
        public Guid UID { get; set; }
        public EntityObject entityObject { get; set; }
    }
    /// <summary>
    /// EntityList class for Queue.
    /// </summary>
    public class EntityListNew
    {
        public Guid UID { get; set; }
        public EntityObject entityObject { get; set; }
    }

    /// <summary>
    /// Extend employee master class of entity framework.
    /// </summary>
    public partial class Employee
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Additional property for email.
        /// </summary>
        public String Email
        {
            get;
            set;
        }

        /// <summary>
        /// Additional property for userid.
        /// </summary>
        public String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// Additional property for manager name.
        /// </summary>
        public String ManagerName
        {
            get;
            set;
        }

        /// <summary>
        /// Additional property for name.
        /// </summary>
        public String Name
        {
            get;
            set;
        }

        /// <summary>
        /// Additional property for employee status.
        /// </summary>
        public String EmployeeStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Additional property for employee type.
        /// </summary>
        public String EmployeeType
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32? EmployeeHistoryOrder
        {
            get;
            set;
        }


        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public

        #endregion

        #region Private

        #endregion

        #endregion
    }

    /// <summary>
    /// Extend Tenant class of entity framework.
    /// </summary>
    public partial class Tenant
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Additional property for set/get the TenantType.
        /// </summary>
        public String TenantTypeDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Additional property for set/get the Tenant's Address.
        /// </summary>
        public String TenantAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Additional property for set/get the Tenant's  ZipCode.
        /// </summary>
        public String TenantZipCode
        {
            get;
            set;
        }

        /// <summary>
        /// Additional property for set/get the Tenant's City.
        /// </summary>
        public String TenantCity
        {
            get;
            set;
        }

        /// <summary>
        /// Additional property for set/get the Tenant's State.
        /// </summary>
        public String TenantState
        {
            get;
            set;
        }

        /// <summary>
        /// Additional property for set/get the Tenant's Phone.
        /// </summary>
        public String TenantPhone
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public

        #endregion

        #region Private

        #endregion

        #endregion
    }

    /// <summary>
    /// Extend OrganizationUser class of entity framework.
    /// </summary>
    public partial class OrganizationUser
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Additional property for set/get the CreatedByUserName.
        /// </summary>
        public String CreatedByUserName
        {
            get;
            set;
        }

        /// <summary>
        /// Additional property for set/get the IsLockedOut.
        /// </summary>
        public Boolean IsLockedOut
        {
            get;
            set;
        }

        /// <summary>
        /// Additional property for set/get the LastActivityDate.
        /// </summary>
        public String LastActivityDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the location name
        /// </summary>
        public string Location
        {
            get
            {
                if (this.OrganizationUserLocations != null && this.OrganizationUserLocations.ToList().Count > 0)
                    return this.OrganizationUserLocations.ToList()[0].OrganizationLocation.LocationName;
                return string.Empty;
            }
        }
        public String Suffix
        {
            get;
            set;
        }

        //UAT-3977
        public Dictionary<String, String> ExtraData
        {
            get;
            set;
        }


        /// <summary>
        /// Gets the program name
        /// </summary>
        //public string Program
        //{
        //    get
        //    {
        //        if (this.OrganizationUserPrograms != null && this.OrganizationUserPrograms.Count() > 0)
        //        {
        //            OrganizationUserProgram organizationUserProgram = this.OrganizationUserPrograms.ToList().FirstOrDefault(x => !x.AdminProgramStudy.DeleteFlag);
        //            if (organizationUserProgram != null)
        //                return organizationUserProgram.AdminProgramStudy.ProgramStudy;
        //        }
        //        return string.Empty;
        //    }
        //}

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public

        #endregion

        #region Private

        #endregion

        #endregion
    }

    /// <summary>
    /// Extend Organization class of entity framework.
    /// </summary>
    public partial class Organization
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Additional property for set/get the CreatedByUserName.
        /// </summary>
        public String CreatedByUserName
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public

        #endregion

        #region Private

        #endregion

        #endregion
    }

    /// <summary>
    /// Extend RoleDetail class of entity framework.
    /// </summary>
    public partial class RoleDetail
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Additional property for set/get the RoleName.
        /// </summary>
        public String RoleName
        {
            get;
            set;
        }

        /// <summary>
        /// Additional property for set/get the RoleName.
        /// </summary>
        public String CreatedByUserName
        {
            get;
            set;
        }

        /// <summary>
        /// Additional property for set/get the RoleName.
        /// </summary>
        public String SplittedRoleName
        {
            get
            {
                return Name.Split(new char[] { '_' }).FirstOrDefault();
            }
        }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public

        #endregion

        #region Private

        #endregion

        #endregion
    }


    #region AMS

    public partial class lkpSysXBlock
    {
        /// <summary>
        /// Additional property for set/get the BusinessChannelTypeName.
        /// </summary>
        public String BusinessChannelTypeName
        {
            get;
            set;
        }
    }

    #endregion

    #region Background Package Administration

    public partial class BkgSvcAttribute
    {
        public String FormatOptions
        {
            get
            {
                StringBuilder formatOptions = new StringBuilder();
                if (this.BkgSvcAttributeOptions != null && this.BkgSvcAttributeOptions.Count > 0)
                {
                    foreach (BkgSvcAttributeOption attributeOption in this.BkgSvcAttributeOptions.Where(x => !x.EBSAO_IsDeleted))
                        formatOptions.AppendFormat("{0}={1},", attributeOption.EBSAO_OptionText, attributeOption.EBSAO_OptionValue);

                    Int32 index = formatOptions.ToString().LastIndexOf(',');
                    if (index >= 0)
                        formatOptions.Remove(index, 1);
                }
                return formatOptions.ToString();
            }
        }
    }
    #endregion


    public partial class PersonAliasProfile
    {
        //    public String PAP_UniqueID { get; set; }
        public Int32 PAP_SequenceId { get; set; }
    }

    //public partial class ResidentialHistoryProfile
    //{
    //    //public String RHIP_UniqueID { get; set; }
    //    /// <summary>
    //    /// Represents the Sequence in which 
    //    /// </summary>
    //    public Int32 RHIP_SequenceId { get; set; }
    //}
    public partial class ExternalBkgSvc
    {
        public Int32 ClearStarService_ID { get; set; }
    }

    public class OrgUser
    {
        public String UserName { get; set; }
        public Int32 UserID { get; set; }
    }

    public class SystemEntityUserPermissionData
    {
        public Int32 SEUP_ID { get; set; }
        public string EntityPermissionId { get; set; }
        public String PermissionName { get; set; }
        public String PermissionTypeName { get; set; }
        public Int32 PermissionTypeID { get; set; }
        public String Permissioncode { get; set; }
        public Int32 OrganizationUserId { get; set; }
        public String UserFirstName { get; set; }
        public String UserLastName { get; set; }
        public String EmailAddress { get; set; }
        public Int32 TenantID { get; set; }
        public Int32 TotalCount { get; set; }
        public Int32 HierarchyNodeId { get; set; }
        public String HierarchyNodeLabel { get; set; }
    }


}
namespace Entity.ClientEntity
{
    public partial class ComplianceItemAttribute
    {
        public String FormatOptions
        {
            get
            {
                StringBuilder formatOptions = new StringBuilder();
                //if (this.ComplianceAttributeOptions != null && this.ComplianceAttributeOptions.Count > 0)
                //{
                //    foreach (ComplianceAttributeOption attributeOption in this.ComplianceAttributeOptions.Where(x => x.IsActive && !x.IsDeleted))
                //        formatOptions.AppendFormat("{0}={1},", attributeOption.OptionText, attributeOption.OptionValue);

                //    Int32 index = formatOptions.ToString().LastIndexOf(',');
                //    if (index >= 0)
                //        formatOptions.Remove(index, 1);
                //}
                return formatOptions.ToString();
            }
        }

    }

    public partial class ClientSetting
    {
        public String CS_SettingValueLangugaeSpecific { get; set; }
    }


    public partial class ComplianceAttribute
    {
        public String FormatOptions
        {
            get
            {
                StringBuilder formatOptions = new StringBuilder();
                if (this.ComplianceAttributeOptions != null && this.ComplianceAttributeOptions.Count > 0)
                {
                    foreach (ComplianceAttributeOption attributeOption in this.ComplianceAttributeOptions.Where(x => !x.IsDeleted))
                        formatOptions.AppendFormat("{0}={1}|", attributeOption.OptionText, attributeOption.OptionValue);

                    //UAT-3486
                    //Int32 index = formatOptions.ToString().LastIndexOf(',');
                    Int32 index = formatOptions.ToString().LastIndexOf('|');
                    if (index >= 0)
                        formatOptions.Remove(index, 1);
                }
                return formatOptions.ToString();
            }
        }

        public String TenantName
        {
            get;
            set;

        }

    }

    public partial class ComplianceCategory
    {
        public String Active
        {
            get
            {
                if (this.IsActive)
                    return "Yes";
                return "No";
            }
        }

        public String TenantName
        {
            get;
            set;
        }

        public String ExpNotes { get; set; }
        public Int32 DisplayOrder
        { get; set; }

        public string IsWithMultipleNodes { get; set; }

        //UAT-4118
        public Boolean HasMoreInfoUrls
        {
            get
            {
                if (this.ComplianceCategoryDocUrls != null && this.ComplianceCategoryDocUrls.ToList().Count > 0)
                    return true;
                return false;
            }
        }
    }
    public partial class ComplianceItem
    {
        public String TenantName
        {
            get;
            set;

        }

        public Int32 DisplayOrder
        {
            get;
            set;
        }

        public Boolean IsItemSeries
        {
            get;
            set;

        }
        public String CompItemID
        {
            get;
            set;
        }
        public String SampleDocFormURLLabel
        {
            get;
            set;
        }

        public string IsWithMultipleNodes
        {
            get;
            set;
        }

        //UAT-4118
        public Boolean HasMoreInfoUrls
        {
            get
            {
                if (this.ComplianceItemDocUrls.ToList().Count > 0)
                    return true;
                return false;
            }
        }
    }

    public partial class CompliancePackage
    {
        public String TenantName
        {
            get;
            set;
        }
    }

    public partial class ApplicantComplianceAttributeData
    {
        public String AttributeTypeCode
        {
            get;
            set;
        }

        /// <summary>
        /// ComplianceItem-Id for getting the remote attributedata in the Verification details screen, for validating UI input.
        /// </summary>
        public Int32 ComplianceItemId { get; set; }

        public Int32 DisplayOrder { get; set; }
    }

    /// <summary>
    /// Properties added to Manage validation of UI rules for the Document type attributes in Items, including incomplete items
    /// </summary>
    public partial class ApplicantComplianceItemData
    {
        public Boolean IsFileUploadApplicable { get; set; }

        /// <summary>
        /// Will be set, only if the File Upload attribute exists in the item
        /// </summary>
        public Int32? FileUploadAttributeId { get; set; }

        /// <summary>
        /// Represents the old status of the Item, BEFORE Save was clicked on the Verification Details or screen was loaded
        /// </summary>
        public String CurrentStatusCode { get; set; }

        /// <summary>
        /// Represents the New status of the Item, when Save was clicked on the Verification Details 
        /// </summary>
        public String AttemptedStatusCode { get; set; }

        /// <summary>
        /// Represents the New/Attempted status Id of the Item
        /// </summary>
        public Int32 AttemptedItemStatusId { get; set; }

        public String CurrentTenantTypeCode { get; set; }

        public String ComplianceItemName { get; set; }
        public String ComplianceCategoryName { get; set; }

        //Added to get data related to XML of 'HandleAssignment' SP in 'Queue Framework'
        public String ApplicantName { get; set; }

        /// <summary>
        /// OrganizationUserId of the Applicant
        /// </summary>
        public Int32 ApplicantId { get; set; }

        public String CompliancePackageName { get; set; }
        public String VerificationStatusText { get; set; }
        public Int32 RushOrderStatusId { get; set; }
        public String RushOrderStatusCode { get; set; }
        public String RushOrderStatusText { get; set; }
        public Int32 HierarchyNodeId { get; set; }
        //public DateTime SubmissionDate { get; set; }
        public Int16 SystemStatusId { get; set; }
        public String SystemStatusText { get; set; }

        /// <summary>
        /// Represents the Items, which were attempted for different queue, but still need another review
        /// </summary>
        public Boolean ReSetInitialReview { get; set; }

        public Boolean IsExceptionTypeItem { get; set; }

        /// <summary>
        /// Identify the record is already escalated or not 
        /// </summary>
        public Boolean IsEscalatedItem { get; set; }

        public Boolean IsIncompleteWithDocuments { get; set; }

        public String ReconciliationMatchingStatus { get; set; }
    }

    public partial class ApplicantDocument
    {
        public String MappedDocumentDetails
        {
            get { return String.Empty; }
            //{
            //if (this.ApplicantComplianceDocumentMaps == null || this.ApplicantComplianceDocumentMaps.Count == 0)
            //    return String.Empty;

            //var documentMaps = this.ApplicantComplianceDocumentMaps.Where(x => !x.IsDeleted);
            //var attributeData = documentMaps.Select(x => x.ApplicantComplianceAttributeData).Where(x => !x.IsDeleted);
            //var itemData = attributeData.Select(x => x.ApplicantComplianceItemData).Where(x => !x.IsDeleted);

            //IEnumerable<ComplianceItem> mappedItems = itemData
            //    .Select(x => x.ComplianceItem)
            //    .Where(x => !x.IsDeleted);


            //List<MappedDocumentPackageDetail> mappedDocumentPackageDetails = new List<MappedDocumentPackageDetail>();

            //foreach (ComplianceItem mappedItem in mappedItems)
            //{
            //    MappedDocumentPackageDetail packageDetail = mappedDocumentPackageDetails.FirstOrDefault(x => x.ClientCompliancePackageID == mappedItem.ClientComplianceCategory.ClientCompliancePackage.ClientCompliancePackageID);

            //    if (packageDetail == null)
            //    {
            //        packageDetail = new MappedDocumentPackageDetail();
            //        packageDetail.ClientCompliancePackageID = mappedItem.ClientComplianceCategory.ClientCompliancePackage.ClientCompliancePackageID;
            //        packageDetail.PackageName = mappedItem.ClientComplianceCategory.ClientCompliancePackage.PackageName;

            //        packageDetail.MappedDocumentCategoryDetails = new List<MappedDocumentPackageDetail.MappedDocumentCategoryDetail>();
            //        MappedDocumentPackageDetail.MappedDocumentCategoryDetail categoryDetail = new MappedDocumentPackageDetail.MappedDocumentCategoryDetail();
            //        categoryDetail.ClientComplianceCategoryID = mappedItem.ClientComplianceCategory.ClientComplianceCategoryID;
            //        categoryDetail.CategoryName = mappedItem.ClientComplianceCategory.CategoryName;
            //        categoryDetail.ItemsName = mappedItem.Name;
            //        packageDetail.MappedDocumentCategoryDetails.Add(categoryDetail);

            //        mappedDocumentPackageDetails.Add(packageDetail);
            //    }
            //    else
            //    {
            //        MappedDocumentPackageDetail.MappedDocumentCategoryDetail categoryDetail = packageDetail.MappedDocumentCategoryDetails
            //            .FirstOrDefault(x => x.ClientComplianceCategoryID == mappedItem.ClientComplianceCategory.ClientComplianceCategoryID);

            //        if (categoryDetail == null)
            //        {
            //            categoryDetail = new MappedDocumentPackageDetail.MappedDocumentCategoryDetail();
            //            categoryDetail.ClientComplianceCategoryID = mappedItem.ClientComplianceCategory.ClientComplianceCategoryID;
            //            categoryDetail.CategoryName = mappedItem.ClientComplianceCategory.CategoryName;
            //            categoryDetail.ItemsName = mappedItem.Name;
            //            packageDetail.MappedDocumentCategoryDetails.Add(categoryDetail);
            //        }
            //        else
            //            categoryDetail.ItemsName += String.Format(", {0}", mappedItem.Name);
            //    }
            //}

            //StringBuilder sb = new StringBuilder();
            //foreach (MappedDocumentPackageDetail packageDetail in mappedDocumentPackageDetails)
            //{
            //    sb.Append("<div style='padding-left: 10px'>");
            //    sb.Append(packageDetail.PackageName);
            //    foreach (MappedDocumentPackageDetail.MappedDocumentCategoryDetail categoryDetail in packageDetail.MappedDocumentCategoryDetails)
            //    {
            //        sb.AppendFormat("<div style='padding-left: 10px'>{0}<div style='padding-left: 10px'>{1}</div></div>",
            //            categoryDetail.CategoryName,
            //            categoryDetail.ItemsName);
            //    }
            //    sb.Append("</div>");
            //}

            //return sb.ToString();

            //}
        }

        public Boolean IsMapped
        {
            get;
            set;

        }

        private class MappedDocumentPackageDetail
        {
            public Int32 ClientCompliancePackageID { get; set; }
            public String PackageName { get; set; }
            public List<MappedDocumentCategoryDetail> MappedDocumentCategoryDetails { get; set; }

            public class MappedDocumentCategoryDetail
            {
                public Int32 ClientComplianceCategoryID { get; set; }
                public String CategoryName { get; set; }
                public String ItemsName { get; set; }
            }

        }
    }

    public partial class RuleMappingDetail
    {
        public Boolean IfExpressionIsvalid
        {
            get;
            set;
        }
    }

    public partial class TypeCustomAttributesSearch
    {
        public String Cvalue
        {
            get;
            set;
        }
    }
    public partial class CompliancePackageCategory
    {
        public Int32 DisplayOrder
        { get; set; }
    }

    public partial class InstHierarchyMobility
    {
        public Int32 DurationInDays { get; set; }
    }

    public partial class ApplicantComplianceDocumentMap
    {
        /// <summary>
        /// Property to differentiate Incomplete Item data in Document control on Verification Details screen
        /// </summary>
        public Boolean IsIncompleteItem { get; set; }
    }

    public partial class NodeDeadline
    {
        public Int32? ND_Frequency { get; set; }
        public Int32? ND_DaysBeforeDeadline { get; set; }
        public Int32 ND_NodeNotificationMappingId { get; set; }
    }

    public class InsContact
    {
        public String ICO_Name { get; set; }
        public Int32 ICO_ID { get; set; }
    }

    #region Background Package Administration

    public partial class BkgSvcAttribute
    {
        public String FormatOptions
        {
            get
            {
                StringBuilder formatOptions = new StringBuilder();
                if (this.BkgSvcAttributeOptions != null && this.BkgSvcAttributeOptions.Count > 0)
                {
                    foreach (BkgSvcAttributeOption attributeOption in this.BkgSvcAttributeOptions.Where(x => !x.EBSAO_IsDeleted))
                        formatOptions.AppendFormat("{0}={1}|", attributeOption.EBSAO_OptionText, attributeOption.EBSAO_OptionValue);

                    Int32 index = formatOptions.ToString().LastIndexOf('|');
                    if (index >= 0)
                        formatOptions.Remove(index, 1);
                }
                return formatOptions.ToString();
            }
        }
    }
    #endregion



    public class ClsFeatureAction
    {
        public String ScreenName { get; set; }
        public String ControlActionId { get; set; }
        public String ControlActionLabel { get; set; }
        public Control SystemControl { get; set; }

        public String CustomActionId { get; set; }
        public String CustomActionLabel { get; set; }


    }


    public class OrderDetailMain
    {
        public List<OrderDetailMenuItem> OrderDetailMenuItem { get; set; }
        public OrderDetailHeaderInfo OrderDetailHeaderInfo { get; set; }
    }


    public class OrderDetailMenuItem
    {
        public Int32 MenuID { get; set; }
        public String MenuName { get; set; }
        public String MenuToolTip { get; set; }

    }
    public class OrderDetailHeaderInfo
    {
        public Int32 OrderID { get; set; }
        public String ApplicantName { get; set; }
        public String DOB { get; set; }
        public String Gender { get; set; }
        public String PhoneNumber { get; set; }
        public String StatusType { get; set; }
        public String PaymentType { get; set; }
        public DateTime OrderDate { get; set; }
        public String InstitutionColorStatus { get; set; }
        public Int32? InstitutionColorStatusID { get; set; }
        public Decimal TotalPrice { get; set; }
        public String PaymentStatus { get; set; }
        public String OrderNumber { get; set; }
        //UAT-2439,Client Admin screen updates for text to icon
        public String InstitutionColorStatusTooltip { get; set; }
    }

    public class OrderDetailInfo
    {
        public Int32 OrderID { get; set; }
        public String ApplicantName { get; set; }
        public String DOB { get; set; }
        public String Gender { get; set; }
        public String PhoneNumber { get; set; }
        public String StatusType { get; set; }
        public String PaymentType { get; set; }
        public DateTime OrderDate { get; set; }
        public String InstitutionColorStatus { get; set; }
        public Int32? InstitutionColorStatusID { get; set; }
        public DateTime? CompletedDate { get; set; }
        public Decimal TotalPrice { get; set; }
        public String Address { get; set; }
        public String Email { get; set; }
        public String SSN { get; set; }
        public String InstitutionHierarchy { get; set; }
        public String PaymentStatus { get; set; }
        public Int32 OrganizationUserID { get; set; }
        public Boolean IsInternationalPhoneNumber { get; set; }
        public Boolean IsInternationalSecondaryPhone { get; set; }
    }

    public class ApplicantAlias
    {
        public Int32 ID { get; set; }
        public String ApplicantAliasName { get; set; }
    }

    public class ServiceGroup
    {
        public Int32 BSG_ID { get; set; }
        public String BSG_Name { get; set; }
    }

    public class ExtVendorAccount
    {
        public String EVOD_AccountNumber { get; set; }
    }

    public class ApplicantOrderDetail
    {
        public OrderDetailInfo OrderDetailInfo { get; set; }
        public List<ApplicantAlias> ApplicantAlias { get; set; }
        public List<ServiceGroup> ServiceGroup { get; set; }
        public List<ExtVendorAccount> ExtVendorAccount { get; set; }
        public List<OrderFlags> OrderFlags { get; set; }
        public Boolean IsSupplement { get; set; }
        public String CFOD_Value { get; set; }
        public String BSAD_Name { get; set; }
        public List<PaymentTypesAndStatus> PaymentTypesAndStatus { get; set; }
    }

    public class OrderFlags
    {
        public Int32 IOF_ID { get; set; }
        public String OFL_FileName { get; set; }
        public String OFL_Tooltip { get; set; }
        public String OFL_FilePath { get; set; }
    }

    public class LocalFeeItemsInfo
    {
        public Int32 PSIF_ID { get; set; }
        public String FeeItemName { get; set; }
        public String FeeItemDescription { get; set; }
        public String FeeItemType { get; set; }
        public Decimal? FeeItemAmount { get; set; }
    }

    public class LocalAttributeGroupMappedToBkgPackage
    {
        public Int32 AttributeGroupId { get; set; }
        public String AttributeGroupName { get; set; }
        public String AttributeGroupDescription { get; set; }
        public Boolean IsSystemPreConfigured { get; set; }
        public Boolean IsEditable { get; set; }
        public Int32 BackgroundPackageId { get; set; }
    }

    public class LocalFeeRecordsInfo
    {
        public Int32 LocalSFRID { get; set; }
        public Int32 LocalSFRFeeeItemId { get; set; }
        public String LocalSFRFieldValue { get; set; }
        public Decimal? LocalSFRAmount { get; set; }
        public Int32? GlobalSIFR_ID { get; set; }
        public Int32? GlobalSIFR_FeeeItemId { get; set; }
        public Decimal? GlobalFeeAmount { get; set; }
        public Boolean ISGLobal { get; set; }
        public String StateName { get; set; }
        public String CountyName { get; set; }
    }

    public class ExternalVendorAccountMappingDetails
    {
        public Int32 DPMEVAM_ID { get; set; }
        public Int32 DPMEVAM_ExternalVendorAccountID { get; set; }
        public String EVE_Name { get; set; }
        public String EVA_AccountNumber { get; set; }
        public String EVA_AccountName { get; set; }
        public Int32 EVA_VendorID { get; set; }
    }


    public class InstHierarchyRegulatoryEntityMappingDetails
    {
        public Int32 IHRE_ID { get; set; }
        public Int32 IHRE_RegulatoryEntityTypeID { get; set; }
        public String RET_Name { get; set; }
        public Int32 IHRE_InstitutionHierarchyID { get; set; }
    }


    public class PkgServiceItemCustomFormMappingDetails
    {
        public Int32 BSIFM_ID { get; set; }
        public Int32 CF_ID { get; set; }
        public String CF_Title { get; set; }
        public String CF_Name { get; set; }
        public String CF_Description { get; set; }
        public Boolean CF_IsEditable { get; set; }
        public Int32 CF_Sequence { get; set; }
        public Int32 CF_CustomFormTypeID { get; set; }
        public String CFT_Code { get; set; }
        public String CFT_Name { get; set; }
    }

    public class PaymentTypesAndStatus
    {
        public Int32 OrderPaymentDetailID { get; set; }
        public Int32 PaymentOptionID { get; set; }
        public Int32 PaymentStatusID { get; set; }
        public String PaymentOption { get; set; }
        public String PaymentStatus { get; set; }
        public String PaymentStatusCode { get; set; }
    }

    #region Order Detail for client admin

    public class OrderDetailClientAdmin
    {
        public OrderDetailInfoClientAdmin OrderDetailInfoClientAdmin { get; set; }
        public List<OrderNotesClientAdmin> OrderNotes { get; set; }
        public List<PaymentTypesAndStatus> PaymentTypesAndStatus { get; set; }
    }

    public class OrderDetailInfoClientAdmin
    {
        public Int32 OrderID { get; set; }
        public String OrderStatus { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DatePaid { get; set; }
        public DateTime? DateCompleted { get; set; }
        public String PaymentType { get; set; }
        public String Category { get; set; }
        public String ApplicantName { get; set; }
        public String DOB { get; set; }
        public String Gender { get; set; }
        public String SSN { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String Zip { get; set; }
        public String PhoneNumber { get; set; }
        public String Email { get; set; }
        public String InstitutionHierarchy { get; set; }
    }

    public class OrderNotesClientAdmin
    {
        public Int32 ONTS_ID { get; set; }
        public String ONTS_NoteText { get; set; }
    }

    [Serializable]
    public class OrderServiceGroupDetails
    {
        public Int32 BkgOrderID { get; set; }
        public Int32 MasterOrderID { get; set; }
        public Int32? OrderPackageServiceGroupID { get; set; }
        public Int32? OrderPackageServiceID { get; set; }
        public Int32? LineItemID { get; set; }
        public Int32? ServiceGroupID { get; set; }
        public String ServiceGroupName { get; set; }
        public Int32? ServiceID { get; set; }
        public String ServiceName { get; set; }
        public String VendorStatus { get; set; }
        public String Label { get; set; }
        public String CustomValue { get; set; }
        public Boolean IsOrderFlagged { get; set; }
        public Boolean IsFlagged { get; set; }
        public Boolean IsCompleted { get; set; }
        public Int32 HierarchyNodeID { get; set; }
        public String LineDescription { get; set; }
        public String ServiceTypeCode { get; set; }
        public Int32 PackageServiceGroupID { get; set; }
        public String SvcGrpReviewStatusType { get; set; }
        public String SvcGrpStatusType { get; set; }
        public Boolean IsServiceGroupStatusComplete { get; set; }
        public Boolean SvcIsReportable { get; set; }
        public Boolean IsEmployment { get; set; }
    }

    #endregion

    public class PackageService
    {
        public Int32 BkgPackageSrvcId { get; set; }
        public String ServiceName { get; set; }
    }

    public partial class BkgRuleMappingDetail
    {
        public Boolean IfExpressionIsvalid
        {
            get;
            set;
        }
    }

    #region MvrFields
    [Serializable]
    public class AttributeFieldsOfSelectedPackages
    {
        public Int32 AttributeGrpId { get; set; }
        public String AttributeGrpCode { get; set; }
        public Int32 AttributeID { get; set; }
        public String BSA_Code { get; set; }
        public Int32 AttributeGrpMapingID { get; set; }
        public Boolean IsAttributeRequired { get; set; }
        public Boolean IsAttributeDisplay { get; set; }
    }

    #endregion
    // LocalSFR

    #region Applicant Documents

    public class ApplicantDocumentDetails
    {
        public Int32 ApplicantDocumentID { get; set; }
        public String FileName { get; set; }
        public Int32 Size { get; set; }
        public String Description { get; set; }
        public String FileType { get; set; }
        public String DocumentTypeCode { get; set; }
        public String UploadedBy { get; set; }
        public DateTime? UploadedOn { get; set; }
        public String ItemName { get; set; }
        public String DocumentPath { get; set; }
        public String ApplicantDocItemAssociationID { get; set; } //UAT-2296
    }

    #endregion

    public class DashBoardMenuItem
    {
        public Int32 MenuID { get; set; }
        public String MenuName { get; set; }
        public String MenuToolTip { get; set; }
        public String ImageUrl { get; set; }
    }

    [Serializable]
    public class RuleSynchronisationData
    {
        public List<CompliancePackage> PkgListCanShareRuleInstance { get; set; }
        public Boolean IfRuleIsAlreadyShared { get; set; }
    }

    #region Manage UnArchival Requests

    public class UnArchivalRequestDetails
    {
        public Int32 UnArchiveRequestId { get; set; }
        public Int32 PackageSubscriptionID { get; set; }
        public Int32 OrderID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String PackageName { get; set; }
        public String HierarchyLabel { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ArchiveDate { get; set; }
        public DateTime? UnArchiveRequestDate { get; set; }
        public String OrderNumber { get; set; }
    }

    public class ComplianceExceptionExpiryData
    {
        public Int32 ApplicantComplianceCategoryID { get; set; }
        public Int32 ApplicantComplianceItemID { get; set; }
        public String ItemCategoryName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime ExpiryDate { get; set; }

        public Int32 OrganizationUserID { get; set; }
        public String PrimaryEmailAddress { get; set; }
        public Int32 HierarchyNodeID { get; set; }

    }
    public class RuleSetData
    {

        public Int32 RuleMappingId { get; set; }
        public Int32 RuleSetId { get; set; }
        public Int32 PackageId { get; set; }
        public Int32 CategoryId { get; set; }
        public Int32 AssignmentHierarchyID { get; set; }
        public Int32 RuleAssociationId { get; set; }
    }
    #endregion


    //#region Extended Entities for Clinical Rotation Management

    //public partial class RequirementField
    //{
    //    public Guid RequirementFieldCode
    //    {
    //        get;
    //        set;
    //    }
    //}

    //public partial class RequirementItem
    //{
    //    public Guid RequirementItemCode
    //    {
    //        get;
    //        set;
    //    }
    //}

    //public partial class RequirementCategory
    //{
    //    public Guid RequirementCategoryCode
    //    {
    //        get;
    //        set;
    //    }
    //}

    //#endregion


    public partial class PackageSubscription
    {
        public PackageSubscription()
        {
            if (this.ArchiveStateID == null)
                this.ArchiveStateID = 1;
        }
    }

    public partial class vwOrderDetail
    {
        public String CancellationStatus
        {
            get;
            set;
        }
    }

    public partial class BkgOrder
    {
        public BkgOrder()
        {
            if (this.BOR_ArchiveStateID == null)
                this.BOR_ArchiveStateID = 1;
        }
    }

    /// <summary>
    /// Partial Class to extend the 'BkgSvcGroup' table.
    /// </summary>
    public partial class BkgSvcGroup
    {
        /// <summary>
        /// Will be only for Service Groups which are Completed.
        /// </summary>
        public DateTime? SvcGrpCompletionDate { get; set; }
    }

    public partial class ItemSeriesItem
    {
        public String ISI_ItemName { get; set; }
    }

    public partial class ItemSeriesAttribute
    {
        public String ISA_AttributeName { get; set; }
    }

    public partial class UserGroup
    {
        public String HierarchyNodeIdList { get; set; }
        public String HierarchyNodeLabelList { get; set; }
    }
}

namespace Entity.SharedDataEntity
{
    #region PROFILE SHARING
    /// <summary>
    /// Extend ProfileSharingInvitation class of entity framework.
    /// </summary>
    public partial class ProfileSharingInvitation
    {
        #region Properties
        #region Public Properties
        /// <summary>
        /// Additional property for set/get the GUID
        /// </summary>
        public Guid InvitationIdentifier
        {
            get;
            set;
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// Extend ProfileSharingInvitation class of entity framework.
    /// </summary>
    public partial class ProfileSharingInvitationGroup
    {
        #region Properties
        #region Public Properties
        /// <summary>
        /// Additional property for set/get the GUID
        /// </summary>
        public String TenantName
        {
            get;
            set;
        }
        #endregion
        #endregion
    }

    public partial class RequirementObjectRuleDetail
    {
        public Boolean IfExpressionIsvalid
        {
            get;
            set;
        }
    }

    public partial class ClientContact
    {
        public Boolean IsRegistered
        {
            get;
            set;
        }
    }
    #endregion
}


