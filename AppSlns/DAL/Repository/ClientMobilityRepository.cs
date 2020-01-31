#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ClientMobilityRepository.cs
// Purpose:
//

#endregion

using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.Mobility;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Data;

namespace DAL.Repository
{
    public class ClientMobilityRepository : ClientBaseRepository, IClientMobilityRepository
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables
        private ADB_LibertyUniversity_ReviewEntities _ClientDBContext;
        #endregion

        #endregion

        #region Constructors

        ///// <summary>
        ///// Default constructor to initialize class level variables.
        ///// </summary>
        public ClientMobilityRepository(Int32 tenantId)
            : base(tenantId)
        {
            _ClientDBContext = base.ClientDBContext;
        }

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Get the hierarchical tree data for mapping screen
        /// </summary>
        /// <returns></returns>
        public ObjectResult<GetRuleSetTree> GetRuleSetTreeForPackage(String packageID)
        {
            return _ClientDBContext.usp_GetRuleSetTreeForPackage(packageID);
        }



        Boolean IClientMobilityRepository.UpdateOrderSkippedMapping(Entity.PkgMappingMaster pkgMappingMaster)
        {
            _ClientDBContext.usp_UpdateOrderSkippedMapping(pkgMappingMaster.PMM_ToNodeID,
                    pkgMappingMaster.PMM_FromNodeID, pkgMappingMaster.PMM_FromPackageID, pkgMappingMaster.PMM_ToPackageID);
            return true;
        }

        String IClientMobilityRepository.GetNodesDetails(Int32 nodeID)
        {
            var deptProgramMapping = _ClientDBContext.DeptProgramMappings.Where(cond => cond.DPM_ID == nodeID).FirstOrDefault();
            if (deptProgramMapping.IsNotNull())
            {
                return deptProgramMapping.DPM_Label;
            }
            return String.Empty;
        }

        #region Institute Hierarchy Mobility

        public Boolean CreateMobilityInstance(Int32 backgroundProcessUserId, Int32 chunkSize)
        {
            Int32 recCount = Convert.ToInt32(((ObjectResult<int?>)_ClientDBContext.CreateMobilityInstance(backgroundProcessUserId, chunkSize)).FirstOrDefault());
            if (recCount == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public MobilityInstance GetNodeMobilityInstance(Int32 dpmID)
        {
            var instHierarchyMobility = _ClientDBContext.InstHierarchyMobilities.Include("MobilityInstances").Where(cond => cond.IHM_HierarchyID == dpmID && !cond.IHM_IsDeleted && cond.IHM_IsActive).FirstOrDefault();
            MobilityInstance mobilityInstance = new MobilityInstance();
            if (instHierarchyMobility.IsNotNull())
            {
                mobilityInstance = instHierarchyMobility.MobilityInstances.FirstOrDefault(cond => cond.MI_IsActive && !cond.MI_IsDeleted);
            }
            return mobilityInstance;
        }

        public Boolean InsertNodeTranistionQueue(Int32 backgroundProcessUserId, Int32 daysDueBeforeTransition)
        {
            Int32 recCount = Convert.ToInt32(((ObjectResult<int?>)_ClientDBContext.InsertNodeTranistionQueue(backgroundProcessUserId, daysDueBeforeTransition)).FirstOrDefault());
            if (recCount == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// To get InstHierarchyMobility data
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <returns></returns>
        public InstHierarchyMobility GetInstHierarchyMobility(Int32 deptProgramMappingID)
        {
            return _ClientDBContext.InstHierarchyMobilities.FirstOrDefault(cond => cond.IHM_HierarchyID == deptProgramMappingID &&
                                                            cond.IHM_IsDeleted == false && cond.IHM_IsActive == true);
        }

        /// <summary>
        /// To save mobility data
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="firstStartDate"></param>
        /// <param name="durationTypeID"></param>
        /// <param name="duration"></param>
        /// <param name="successorNodeID"></param>
        /// <param name="listMobilityPackageRelation"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public Boolean SaveMobilityData(Int32 deptProgramMappingID, DateTime firstStartDate, Int16 durationTypeID, Int32 duration, Int32? instanceInterval, Int32? successorNodeID, List<MobilityPackageRelation> listMobilityPackageRelation, Int32 currentUserId)
        {
            InstHierarchyMobility newMapping = new InstHierarchyMobility();
            var instHierarchyMobility = _ClientDBContext.InstHierarchyMobilities.FirstOrDefault(x => x.IHM_HierarchyID == deptProgramMappingID && x.IHM_IsDeleted == false);

            //If mobility is not defined then insert the records else update the old mobility record.
            if (instHierarchyMobility.IsNullOrEmpty())
            {
                newMapping.IHM_HierarchyID = deptProgramMappingID;
                newMapping.IHM_FirstStartDate = firstStartDate;
                newMapping.IHM_DurationTypeID = durationTypeID;
                newMapping.IHM_Duration = duration;
                newMapping.IHM_InstanceInterval = instanceInterval;
                newMapping.IHM_SuccessorID = successorNodeID;
                newMapping.IHM_IsActive = true;
                newMapping.IHM_IsDeleted = false;
                newMapping.IHM_CreatedByID = currentUserId;
                newMapping.IHM_CreatedOn = DateTime.Now;

                MobilityPackageRelation tempMobilityPackageRelation = null;
                if (listMobilityPackageRelation.IsNotNull() && listMobilityPackageRelation.Count > 0)
                {
                    foreach (var item in listMobilityPackageRelation)
                    {
                        tempMobilityPackageRelation = new MobilityPackageRelation();
                        tempMobilityPackageRelation.MPR_PackageID = item.MPR_PackageID;
                        tempMobilityPackageRelation.MPR_SuccessorPackageID = item.MPR_SuccessorPackageID;
                        tempMobilityPackageRelation.MPR_IsDeleted = false;
                        tempMobilityPackageRelation.MPR_CreatedByID = currentUserId;
                        tempMobilityPackageRelation.MPR_CreatedOn = DateTime.Now;
                        newMapping.MobilityPackageRelations.Add(tempMobilityPackageRelation);
                    }
                }
                _ClientDBContext.InstHierarchyMobilities.AddObject(newMapping);
            }
            else
            {
                MobilityPackageRelation tempMobilityPackageRelation = null;
                List<MobilityPackageRelation> selectedMobilityPackageRelationList = new List<MobilityPackageRelation>();

                instHierarchyMobility.IHM_HierarchyID = deptProgramMappingID;
                instHierarchyMobility.IHM_FirstStartDate = firstStartDate;
                instHierarchyMobility.IHM_DurationTypeID = durationTypeID;
                instHierarchyMobility.IHM_Duration = duration;
                instHierarchyMobility.IHM_InstanceInterval = instanceInterval;
                instHierarchyMobility.IHM_SuccessorID = successorNodeID;
                instHierarchyMobility.IHM_IsActive = true;
                instHierarchyMobility.IHM_IsDeleted = false;
                instHierarchyMobility.IHM_ModifiedByID = currentUserId;
                instHierarchyMobility.IHM_ModifiedOn = DateTime.Now;


                List<MobilityPackageRelation> _lstTemp = instHierarchyMobility.MobilityPackageRelations.Where(x => x.MPR_IsDeleted == false).ToList();
                if (listMobilityPackageRelation.IsNotNull() && listMobilityPackageRelation.Count > 0)
                {
                    foreach (var mobPackageRelation in listMobilityPackageRelation)
                    {
                        MobilityPackageRelation _mpr = _lstTemp.Where(mpr => mpr.MPR_PackageID == mobPackageRelation.MPR_PackageID
                                                       && mpr.MPR_HierarchyMobilityID == instHierarchyMobility.IHM_ID)
                                                       .FirstOrDefault();
                        if (_mpr.IsNotNull())
                        {
                            _mpr.MPR_SuccessorPackageID = mobPackageRelation.MPR_SuccessorPackageID;
                            _mpr.MPR_ModifiedByID = currentUserId;
                            _mpr.MPR_ModifiedOn = DateTime.Now;
                        }
                        else
                        {
                            tempMobilityPackageRelation = new MobilityPackageRelation();
                            tempMobilityPackageRelation.MPR_HierarchyMobilityID = instHierarchyMobility.IHM_ID;
                            tempMobilityPackageRelation.MPR_PackageID = mobPackageRelation.MPR_PackageID;
                            tempMobilityPackageRelation.MPR_SuccessorPackageID = mobPackageRelation.MPR_SuccessorPackageID;
                            tempMobilityPackageRelation.MPR_IsDeleted = false;
                            tempMobilityPackageRelation.MPR_CreatedByID = currentUserId;
                            tempMobilityPackageRelation.MPR_CreatedOn = DateTime.Now;

                            _ClientDBContext.MobilityPackageRelations.AddObject(tempMobilityPackageRelation);
                        }
                    }
                }
            }
            if (_ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// To delete Mobility Data
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public Boolean DeleteMobilityData(Int32 deptProgramMappingID, Int32 currentUserId)
        {
            var instHierarchyMobility = _ClientDBContext.InstHierarchyMobilities.FirstOrDefault(x => x.IHM_HierarchyID == deptProgramMappingID && x.IHM_IsDeleted == false);

            if (instHierarchyMobility.IsNotNull())
            {
                instHierarchyMobility.IHM_IsDeleted = true;
                instHierarchyMobility.IHM_ModifiedByID = currentUserId;
                instHierarchyMobility.IHM_ModifiedOn = DateTime.Now;

                if (_ClientDBContext.SaveChanges() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }


        #endregion

        #region Applicant Node transition Status
        List<ApplicantTransitionStatus> IClientMobilityRepository.GetApplicantNodeTransitionStatus(CustomPagingArgsContract gridCustomPaging, MobilitySearchDataContract mobilitySearchData)
        {
            try
            {
                string orderBy = QueueConstants.APPLICANT_SEARCH_DEFAULT_SORTING_FIELDS;
                string ordDirection = null;

                orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? orderBy : gridCustomPaging.SortExpression;
                ordDirection = gridCustomPaging.SortDirectionDescending == false ? null : "desc";

                List<ApplicantTransitionStatus> applicantTransitionStatus = _ClientDBContext.GetApplicantTransitionStatus(orderBy, ordDirection, gridCustomPaging.CurrentPageIndex, gridCustomPaging.PageSize, mobilitySearchData.ApplicantFirstName, mobilitySearchData.ApplicantLastName
                                                                            , mobilitySearchData.SourceNodeIds, mobilitySearchData.Status, mobilitySearchData.TransitionDate, mobilitySearchData.UserGroupID).ToList();
                return applicantTransitionStatus;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        public Boolean UpdateNodeTransitionStatus(List<Int32> mobilityNodeTransitionIds, Int32 currentLoggedInUserId, Int16 approvalStatusID)
        {
            MobilityNodeTransition mobilityNodeTransition = null;
            foreach (Int32 mobilityNodeTransitionId in mobilityNodeTransitionIds)
            {
                mobilityNodeTransition = _ClientDBContext.MobilityNodeTransitions.FirstOrDefault(obj => obj.MNT_ID == mobilityNodeTransitionId && obj.MNT_IsDeleted == false);
                if (mobilityNodeTransition.IsNotNull())
                {
                    mobilityNodeTransition.MNT_ApprovalStatusID = approvalStatusID;
                    mobilityNodeTransition.MNT_ModifiedByID = currentLoggedInUserId;
                    mobilityNodeTransition.MNT_ModifiedOn = DateTime.Now;
                }
            }
            if (_ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }



        #region Institution Change Request Details


        /// <summary>
        /// Delete the applicant orders placed in the current institution.
        /// </summary>
        /// <param name="organisationUserId">Organisation User Id</param>
        /// <param name="currentloggedInUserId">Current Logged In User Id</param>
        /// <returns>OrganizationUser Object</returns>
        public Address DeleteAppicantOrders(Int32 organisationUserId, Int32 currentloggedInUserId)
        {
            Int16 SubscriptionMobilityStatusID = 0;
            //Get the list of applicant orders.
            List<Order> lstOrder = ClientDBContext.Orders
                .Where(obj => obj.OrganizationUserProfile.OrganizationUserID == organisationUserId && obj.IsDeleted == false).ToList();

            List<PackageSubscription> lstPackageSubscription = ClientDBContext.PackageSubscriptions
                .Where(obj => obj.Order.OrganizationUserProfile.OrganizationUserID == organisationUserId && obj.IsDeleted == false).ToList();

            if (lstPackageSubscription.Count > 0)
            {
                String statusCode = LkpSubscriptionMobilityStatus.InstituteSwitched;
                SubscriptionMobilityStatusID = ClientDBContext.lkpSubscriptionMobilityStatus.FirstOrDefault(obj => obj.Code.Equals(statusCode) && obj.IsDeleted == false).SubscriptionMobilityStatusID;
            }
            //Update package subscriptions and deleted all the orders placed by the applicant.
            foreach (PackageSubscription packageSubscription in lstPackageSubscription)
            {
                packageSubscription.SubscriptionMobilityStatusID = SubscriptionMobilityStatusID;
                packageSubscription.ModifiedByID = currentloggedInUserId;
                packageSubscription.ModifiedOn = DateTime.Now;
            }

            foreach (Order order in lstOrder)
            {
                order.IsDeleted = true;
                order.ModifiedByID = currentloggedInUserId;
                order.ModifiedOn = DateTime.Now;
            }

            //Delete the applicant profile from the current institution
            OrganizationUser organizationUser = ClientDBContext.OrganizationUsers.FirstOrDefault(ou => ou.OrganizationUserID == organisationUserId
                && ou.IsDeleted == false && ou.IsActive == true);
            if (!organizationUser.IsNull())
            {
                organizationUser.IsActive = false;
                organizationUser.IsDeleted = true;
            }
            ClientDBContext.SaveChanges();
            if (organizationUser.IsNotNull() && organizationUser.AddressHandle.IsNotNull())
                return organizationUser.AddressHandle.Addresses.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Creates applicant account in the new institution.
        /// </summary>
        /// <param name="organizationUser">OrganizationUser Entity</param>
        /// <param name="currentloggedInUserId">Current Logged In User Id</param>
        public void CreateApplicantAccount(Entity.OrganizationUser organizationUser, Address address, Int32 currentloggedInUserId)
        {
            if (organizationUser.IsNotNull())
            {
                OrganizationUser newOrganizationUser = new OrganizationUser();
                if (address.IsNotNull())
                {
                    AddressHandle addressHandle = new AddressHandle();
                    addressHandle.AddressHandleID = address.AddressHandleID;
                    addressHandle.OrganizationUsers.Add(newOrganizationUser);

                    Address addressNew = new Address();
                    addressNew.AddressID = address.AddressID;
                    addressNew.AddressTypeID = address.AddressTypeID;
                    addressNew.ZipCodeID = address.ZipCodeID;
                    addressNew.GeoCodeID = address.GeoCodeID;
                    addressNew.LegalDescriptionID = address.LegalDescriptionID;
                    addressNew.NeighborhoodTypeID = address.NeighborhoodTypeID;
                    addressNew.TimeZoneID = address.TimeZoneID;
                    addressNew.UnitNumber = address.UnitNumber;
                    addressNew.Address1 = address.Address1;
                    addressNew.Address2 = address.Address2;
                    addressNew.IsActive = address.IsActive;
                    addressNew.ExpireDate = address.ExpireDate;
                    addressNew.CreatedByID = currentloggedInUserId;
                    addressNew.CreatedOn = address.CreatedOn;
                    newOrganizationUser.AddressHandle.Addresses.Add(addressNew);
                }
                newOrganizationUser.OrganizationUserID = organizationUser.OrganizationUserID;
                newOrganizationUser.UserID = organizationUser.UserID;
                newOrganizationUser.OrganizationID = organizationUser.OrganizationID;
                newOrganizationUser.BillingAddressID = organizationUser.BillingAddressID;
                newOrganizationUser.ContactID = organizationUser.ContactID;
                newOrganizationUser.UserTypeID = organizationUser.UserTypeID;
                newOrganizationUser.DepartmentID = organizationUser.DepartmentID;
                newOrganizationUser.SysXBlockID = organizationUser.SysXBlockID;
                newOrganizationUser.FirstName = organizationUser.FirstName;
                newOrganizationUser.LastName = organizationUser.LastName;
                newOrganizationUser.VerificationCode = organizationUser.VerificationCode;
                newOrganizationUser.OfficeReturnDateTime = organizationUser.OfficeReturnDateTime;
                newOrganizationUser.IsOutOfOffice = organizationUser.IsOutOfOffice;
                newOrganizationUser.IsNewPassword = organizationUser.IsNewPassword;
                newOrganizationUser.IgnoreIPRestriction = organizationUser.IgnoreIPRestriction;
                newOrganizationUser.IsMessagingUser = organizationUser.IsMessagingUser;
                newOrganizationUser.IsSystem = organizationUser.IsSystem;
                newOrganizationUser.IsDeleted = organizationUser.IsDeleted;
                newOrganizationUser.IsActive = organizationUser.IsActive;
                newOrganizationUser.ExpireDate = organizationUser.ExpireDate;
                newOrganizationUser.CreatedByID = organizationUser.CreatedByID;
                newOrganizationUser.CreatedOn = organizationUser.CreatedOn;
                newOrganizationUser.IsSubscribeToEmail = organizationUser.IsSubscribeToEmail;
                newOrganizationUser.IsApplicant = organizationUser.IsApplicant;
                newOrganizationUser.PhotoName = organizationUser.PhotoName;
                newOrganizationUser.OriginalPhotoName = organizationUser.OriginalPhotoName;
                newOrganizationUser.DOB = organizationUser.DOB;
                newOrganizationUser.SSN = organizationUser.SSN;
                newOrganizationUser.Gender = organizationUser.Gender;
                newOrganizationUser.PhoneNumber = organizationUser.PhoneNumber;
                newOrganizationUser.MiddleName = organizationUser.MiddleName;
                newOrganizationUser.Alias1 = organizationUser.Alias1;
                newOrganizationUser.Alias2 = organizationUser.Alias2;
                newOrganizationUser.Alias3 = organizationUser.Alias3;
                newOrganizationUser.PrimaryEmailAddress = organizationUser.PrimaryEmailAddress;
                newOrganizationUser.SecondaryEmailAddress = organizationUser.SecondaryEmailAddress;
                newOrganizationUser.SecondaryPhone = organizationUser.SecondaryPhone;
                newOrganizationUser.UserVerificationCode = organizationUser.UserVerificationCode;

                ClientDBContext.OrganizationUsers.AddObject(newOrganizationUser);
                ClientDBContext.SaveChanges();
            }
        }

        public PackageSubscription GetPackageSubscriptionById(Int32 packageSubscriptionId, Boolean checkDeletedSubscriptions = true)
        {
            if (checkDeletedSubscriptions == true)
            {
                return ClientDBContext.PackageSubscriptions.FirstOrDefault(ps => ps.PackageSubscriptionID == packageSubscriptionId && ps.IsDeleted == false);
            }
            else
            {
                return ClientDBContext.PackageSubscriptions.FirstOrDefault(ps => ps.PackageSubscriptionID == packageSubscriptionId);
            }
        }

        public DeptProgramMapping GetDeptProgramMappingById(Int32 deptProgramMappingId)
        {
            return ClientDBContext.DeptProgramMappings.FirstOrDefault(dpm => dpm.DPM_ID == deptProgramMappingId && dpm.DPM_IsDeleted == false);
        }
        #endregion

        #endregion

        #region Applicant Balance Payment

        /// <summary>
        /// Get the Order for an Applicant whose Balance is due.
        /// </summary>
        /// <param name="applicantID">ID of applicant.</param>
        /// <returns>Order</returns>
        public Order GetApplicantBalanceDueOrder(Int32 applicantID)
        {
            Order _orderToProcess = new Order();

            String paymentDueCode = ApplicantOrderStatus.Payment_Due.GetStringValue();
            String sentOnlinePaymentStatusCode = ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue();
            String onlinePaymentNotCompletedStatusCode = ApplicantOrderStatus.Online_Payment_Not_Completed.GetStringValue();

            //return _ClientDBContext.Orders.Include("OrganizationUserProfile").Include("OrganizationUserProfile.AddressHandle")
            //    .Include("OrganizationUserProfile.AddressHandle.Addresses").Include("DeptProgramPackage.DeptProgramPackageSubscriptions")
            //    .Include("DeptProgramPackage.CompliancePackage").Include("DeptProgramMapping").Include("lkpOrderStatu")
            //    .FirstOrDefault(x => x.IsDeleted == false && (x.lkpOrderStatu.Code == paymentDueCode || x.lkpOrderStatu.Code == sentOnlinePaymentStatusCode) &&
            //        x.OrganizationUserProfile.OrganizationUserID == applicantID && x.OrganizationUserProfile.IsDeleted == false &&
            //        x.DuePayment != null && x.DuePayment != Decimal.Zero);

            var _opd = _ClientDBContext.OrderPaymentDetails.Include("Order").Include("Order.OrganizationUserProfile").Include("Order.OrganizationUserProfile.AddressHandle")
             .Include("Order.OrganizationUserProfile.AddressHandle.Addresses").Include("Order.DeptProgramPackage.DeptProgramPackageSubscriptions")
             .Include("Order.DeptProgramPackage.CompliancePackage").Include("Order.DeptProgramMapping").Include("lkpOrderStatu")
             .Where(x => x.OPD_IsDeleted == false && x.Order.IsDeleted == false && x.Order.OrganizationUserProfile.OrganizationUserID == applicantID &&
               (x.lkpOrderStatu.Code == sentOnlinePaymentStatusCode
               || x.lkpOrderStatu.Code == paymentDueCode
               || x.lkpOrderStatu.Code == onlinePaymentNotCompletedStatusCode)
               && x.Order.DuePayment != null && x.Order.DuePayment != Decimal.Zero
               && x.Order.OrganizationUserProfile.IsDeleted == false).FirstOrDefault();

            return _opd.IsNotNull() ? _opd.Order : null;
        }

        public Order GetApplicantBalanceDuePreviousOrder(Int32 applicantID, Int32? orderID)
        {
            return _ClientDBContext.Orders.Include("DeptProgramPackage.CompliancePackage")
                .Include("DeptProgramMapping").FirstOrDefault(x => x.OrderID == orderID && !x.IsDeleted);
        }

        Boolean IClientMobilityRepository.IsOrderPaymtDueAndChangeByAdmin(Int32 orderID)
        {
            String paymentDueCode = ApplicantOrderStatus.Payment_Due.GetStringValue();
            String orderChangeByAdminCode = OrderRequestType.ChangeSubscriptionByAdmin.GetStringValue();
            String compliancePackageTypeCode = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();
            Order order = _ClientDBContext.Orders.Where(x => x.OrderID == orderID && !x.IsDeleted).FirstOrDefault();

            //if (order.lkpOrderStatu.Code == paymentDueCode && order.lkpOrderRequestType.ORT_Code == orderChangeByAdminCode)
            //    return true;

            if (order.OrderPaymentDetails
                .Any(opd => opd.OrderPkgPaymentDetails
                .Any(oppd => oppd.lkpOrderPackageType.OPT_Code == compliancePackageTypeCode && !oppd.OPPD_IsDeleted)
                  && !opd.OPD_IsDeleted)
                  && order.lkpOrderRequestType.ORT_Code == orderChangeByAdminCode)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region "Get List of Active Subscriptions"
        List<AdminChangeSubscription> IClientMobilityRepository.GetAdminChangeSubscriptionList(SearchItemDataContract searchDataContract, CustomPagingArgsContract gridCustomPaging)
        {
            string orderBy = QueueConstants.APPLICANT_SEARCH_DEFAULT_SORTING_FIELDS;
            string ordDirection = null;

            orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? orderBy : gridCustomPaging.SortExpression;
            ordDirection = gridCustomPaging.SortDirectionDescending == false ? null : "desc";

            return _ClientDBContext.GetAdminChangeSubscriptionList(searchDataContract.DPM_Id, searchDataContract.ApplicantFirstName, searchDataContract.ApplicantLastName, searchDataContract.FilterUserGroupID, orderBy, ordDirection, gridCustomPaging.CurrentPageIndex, gridCustomPaging.PageSize).ToList();
        }

        #region "Get List of Active Subscriptions for Rollback Change Subscription"
        List<ActiveSubscriptionsForRollback> IClientMobilityRepository.GetActiveSubscriptionsForRollback(string _applicantFirstName, string _applicantLastName, Int32? _userGroupID, Int32? _sourceNodeID, Int32? _targetNodeID, DateTime? _fromDate, DateTime? _toDate, CustomPagingArgsContract gridCustomPaging)
        {
            string orderBy = QueueConstants.APPLICANT_SEARCH_DEFAULT_SORTING_FIELDS;
            string ordDirection = null;

            orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? orderBy : gridCustomPaging.SortExpression;
            ordDirection = gridCustomPaging.SortDirectionDescending == false ? null : "desc";

            return _ClientDBContext.GetActiveSubscriptionsForRollback(_applicantFirstName, _applicantLastName, _userGroupID, _sourceNodeID, _targetNodeID, _fromDate, _toDate, orderBy, ordDirection, gridCustomPaging.CurrentPageIndex, gridCustomPaging.PageSize).ToList();
        }

        #region update mapping instance  package subscription
        void IClientMobilityRepository.UpdateMappingInstanceforPendingSubscription(Int32 mappingMasterId, Int32 mappingInstanceId, Int32 currentLoggedInUserId)
        {
            _ClientDBContext.usp_UpdateMappinginstanceIdForPendingSubscription(mappingMasterId, mappingInstanceId, currentLoggedInUserId);
        }
        #endregion

        #endregion

        #endregion

        #region Private Methods

        #endregion

        #endregion

        #region Compiled Queries

        #endregion

        #region Institution Change Request Queue

        ObjectResult<GetPreviousValuesForSubscription> IClientMobilityRepository.GetPreviousDataForSubscription(int subscriptionnId)
        {
            return _ClientDBContext.GetPreviousValuesForSubscription(subscriptionnId);
        }




        List<PackageSubscriptionList> IClientMobilityRepository.CopyPackageData(List<SourceTargetSubscriptionList> subscriptionID, Int32 currentLoggedInUserID)
        {
            string str = "<Subscriptions>";
            foreach (SourceTargetSubscriptionList obj in subscriptionID)
            {
                str += "<Subscription>";
                str += "<SourceSubscriptionID>" + obj.SourceSubscriptionID.ToString() + "</SourceSubscriptionID>";
                str += "<TargetSubscriptionID>" + obj.TargetSubscriptionID.ToString() + "</TargetSubscriptionID>";
                str += "</Subscription>";
            }
            str += "</Subscriptions>";
            return _ClientDBContext.CopyPackageData(str, currentLoggedInUserID).ToList();
        }

        Int32 IClientMobilityRepository.RollbackSubscriptions(Int32 LoginUserID, List<Int32> subscriptionID)
        {


            string str = "<root><Orders>";
            foreach (Int32 obj in subscriptionID)
            {
                str += "<Order Value=\"" + obj.ToString() + "\">" + obj.ToString() + "</Order>";
            }
            str += "</Orders>";
            str += "<CurrentLoggedInUserId>" + LoginUserID.ToString() + "</CurrentLoggedInUserId></root>";
            return _ClientDBContext.RollbackSubscriptions(str).FirstOrDefault().Value;
        }


        ObjectResult<GetSourceNodeDeatils> IClientMobilityRepository.GetSourceNodeDeatils(Int32 sourceNodeId, Int32 sourcePackageId)
        {
            return _ClientDBContext.GetSourceNodeDeatils(sourceNodeId, sourcePackageId);
        }
        public String GetTargetNodeHierarchyLabel(Int32 departmentProgramMappingId)
        {
            return _ClientDBContext.DeptProgramMappings.Where(obj => obj.DPM_ID == departmentProgramMappingId).FirstOrDefault().DPM_Label;
        }
        public List<PackageSubscription> GetSourceSubscriptionDetails(List<Int32> subscriptionIds)
        {
            return _ClientDBContext.PackageSubscriptions.Where(obj => subscriptionIds.Contains(obj.PackageSubscriptionID)).ToList();
        }
        #endregion

        public List<SourceTargetSubscriptionList> GetSourceTargetSubscriptionList(Int32 chunkSize)
        {
            return _ClientDBContext.GetSourceTargetSubscriptionIds(chunkSize).ToList();
        }
        //public List<usp_SubscriptionChange_Result> CreateNewSubscriptionForMobilityNode(String xml)
        //{
        //    return _ClientDBContext.MobilitySubscriptionChange(xml).ToList();
        //}

        public List<Entity.ClientEntity.CompliancePkgMappingDependency> GetPkgMappingDependencyList(CustomPagingArgsContract gridCustomPaging, Int32 mappingMasterID)
        {
            string orderBy = null;
            string ordDirection = null;
            ordDirection = gridCustomPaging.SortDirectionDescending == false ? null : "desc";


            List<Entity.ClientEntity.CompliancePkgMappingDependency> lstPkgMappingDependenciesList = _ClientDBContext.GetCompliancePkgMappingDependencies(orderBy, ordDirection, gridCustomPaging.CurrentPageIndex, gridCustomPaging.PageSize, mappingMasterID).ToList();
            return lstPkgMappingDependenciesList;
        }

        Boolean IClientMobilityRepository.IsSubscriptionExist(Int32 pkgMappingMasterId)
        {
            return _ClientDBContext.PackageSubscriptions.Any(x => x.MappingMasterID == pkgMappingMasterId && !x.IsDeleted);
        }


        public List<ApplicantsNodeTransitions> GetApplicantsNodeTransitionsDue(Int32 chunkSize)
        {
            return _ClientDBContext.GetApplicantsNodeTransitionsDue(chunkSize).ToList();
        }

        public List<AutomaticChangedSubscriptions> AutomaticChangeSubscription(String sourceXML)
        {
            return _ClientDBContext.AutomaticChangeSubscription(sourceXML).ToList();
        }

        #region UAT-1395:Change Subscription/Data sync bugs found by QA
        //Insert Subscription Detail in Data Sunc History
        void IClientMobilityRepository.SaveDataSyncHistory(String subscriptionXml, Int32 currentLoggedInUSerID,Int32 tenantId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("ams.usp_InsertRecordInDataSyncHistory", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SubscriptionIdXML", subscriptionXml);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUSerID);
                command.Parameters.AddWithValue("@TenantID", tenantId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
            }

        }

        List<Int32> IClientMobilityRepository.GetPackageSubscriptionIDForChangeSub(List<Int32> pkgSubscriptionIds, Int32 ordChangeReqTypeID)
        {
            return _ClientDBContext.PackageSubscriptions.Where(x => pkgSubscriptionIds.Contains(x.PackageSubscriptionID) && x.IsDeleted == false
                                                         && x.Order.OrderRequestTypeID == ordChangeReqTypeID).Select(y => y.PackageSubscriptionID).ToList();
        }
        #endregion

        #endregion

        #region UAT-1476:WB: When a tracking package is ordered and there was already a previous package with entered data,
        //then there would be data movement as if there were a subscription change.
        CompliancePackageCopyDataMapping IClientMobilityRepository.GetCompliancePackageCopyDataMapping(Int32 tenantId, Int32 targetPackageId, Int32 currentLoggedInUserId,
                                                                                                         Int32 SelectedNodeId)
        {
            return _ClientDBContext.GetCompPackageCopyDataMapping(targetPackageId, tenantId, currentLoggedInUserId, SelectedNodeId).FirstOrDefault();
        }

        #endregion


        #region UAT-2387
        List<usp_SubscriptionChange_Result> IClientMobilityRepository.ChangePackageAndSubscription(String sourceXml, Boolean isOnlyPackageChange)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            List<usp_SubscriptionChange_Result> lstSubscriptionChangeData = new List<usp_SubscriptionChange_Result>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@SourceXML", sourceXml),
                             new SqlParameter("@IsOnlyPackageChange", isOnlyPackageChange)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_SubscriptionChange", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())                        {
                            usp_SubscriptionChange_Result usp_SubscriptionChange_Result = new usp_SubscriptionChange_Result();
                            usp_SubscriptionChange_Result.FirstName = Convert.ToString(dr["FirstName"]);
                            usp_SubscriptionChange_Result.LastName = Convert.ToString(dr["LastName"]);
                            usp_SubscriptionChange_Result.HierarchyNodeId = dr["HierarchyNodeId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["HierarchyNodeId"]);
                            usp_SubscriptionChange_Result.SelectedNodeId = dr["SelectedNodeId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["SelectedNodeId"]);
                            usp_SubscriptionChange_Result.SourceSubscriptionId = dr["SourceSubscriptionId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["SourceSubscriptionId"]);
                            usp_SubscriptionChange_Result.NewSubscriptionId = dr["NewSubscriptionId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["NewSubscriptionId"]);
                            usp_SubscriptionChange_Result.NewOrderId = dr["NewOrderId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["NewOrderId"]);
                            usp_SubscriptionChange_Result.NewOrderStatusId = dr["NewOrderStatusId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["NewOrderStatusId"]);
                            usp_SubscriptionChange_Result.DuePayment = dr["DuePayment"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["DuePayment"]);
                            usp_SubscriptionChange_Result.OranizationUserId = dr["OranizationUserId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["OranizationUserId"]);
                            usp_SubscriptionChange_Result.PrimaryEmailAddress = Convert.ToString(dr["PrimaryEmailAddress"]);
                            lstSubscriptionChangeData.Add(usp_SubscriptionChange_Result);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);

            }
            return lstSubscriptionChangeData;
           
        }

        #endregion
    }
}
