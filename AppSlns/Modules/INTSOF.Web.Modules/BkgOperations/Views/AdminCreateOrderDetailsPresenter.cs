using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract;
using INTSOF.Contracts;
using System.Web;
using ExternalVendors.ClearStarVendor;


namespace CoreWeb.BkgOperations.Views
{
    public class AdminCreateOrderDetailsPresenter : Presenter<IAdminCreateOrderDetailsView>
    {
        /// <summary>
        /// 
        /// </summary>
        public override void OnViewInitialized()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnViewLoaded()
        {
            View.GenderList = GetGenderList();
        }

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DPMIds"></param>
        public void GetBkgPackageDetailsForAdminOrder(String DPMIds)
        {
            if (!DPMIds.IsNullOrEmpty())
                View.lstBkgPackage = BackgroundProcessOrderManager.GetBkgPackageDetailsForAdminOrder(DPMIds, View.SelectedTenantId);
            else
                View.lstBkgPackage = new List<BackgroundPackagesContract>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetAdminOrderDetails()
        {
            if (View.OrderID > AppConsts.NONE)
            {
                var createOrderData = BackgroundProcessOrderManager.GetAdminOrderDetailsByOrderId(View.OrderID, View.SelectedTenantId);
                List<AdminCreateOrderContract> lstAdminOrderDetails = createOrderData.Item1;
                View.lstBkgOrderData = createOrderData.Item2;
                if (!lstAdminOrderDetails.IsNullOrEmpty())
                {
                    View.AdminCreateOrderContract = lstAdminOrderDetails;
                    if (!lstAdminOrderDetails.FirstOrDefault().IsNullOrEmpty())
                    {
                        View.BkgOrderId = lstAdminOrderDetails.FirstOrDefault().BKgOrderID;
                        Guid UserId = new Guid(lstAdminOrderDetails.FirstOrDefault().UserID);
                        GetOrganizationUserDetailsByUserID(UserId);
                    }
                }
                View.IsOrderReadyForTransmit = View.AdminCreateOrderContract.Any() ? View.AdminCreateOrderContract.FirstOrDefault().AdminOrderStatusType.Equals(AdminOrderStatusTypeOptions.ReadyForTransmit.GetStringValue()) ? true : false : false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean AddUpdateUser()
        {
            if (!View.OrganizationUser.IsNullOrEmpty() && View.OrganizationUser.OrganizationUserID > AppConsts.NONE)
            {
                //Update Organization User
                SaveUserData();
            }
            else
            {
                //Add Organization User
                aspnet_Applications application = SecurityManager.GetApplication();
                aspnet_Users aspnetUsers = new aspnet_Users();
                OrganizationUser organizationUser = new OrganizationUser();

                aspnetUsers.MobileAlias = View.PhoneNumber;
                aspnetUsers.LastActivityDate = DateTime.MaxValue;
                aspnetUsers.UserName = "UN" + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Millisecond;
                aspnetUsers.LoweredUserName = aspnetUsers.UserName.ToLower();
                aspnetUsers.ApplicationId = application.ApplicationId;
                aspnetUsers.UserId = Guid.NewGuid();

                organizationUser.Organization = SecurityManager.GetOrganizationForTenantID(View.SelectedTenantId); ;
                organizationUser.aspnet_Users = aspnetUsers;
                organizationUser.FirstName = View.FirstName;
                organizationUser.MiddleName = View.MiddleName;
                organizationUser.LastName = View.LastName;
                organizationUser.PrimaryEmailAddress = View.Email;
                organizationUser.IsNewPassword = false;
                organizationUser.CreatedOn = DateTime.Now;
                organizationUser.CreatedByID = View.CurrentLoggedInUserId;
                organizationUser.Gender = View.Gender;
                organizationUser.ModifiedOn = null;
                organizationUser.ModifiedByID = null;
                organizationUser.IsDeleted = true;
                organizationUser.IsActive = true;
                organizationUser.ActiveDate = DateTime.Now;
                organizationUser.IsApplicant = true;
                organizationUser.IsOutOfOffice = false;
                organizationUser.IgnoreIPRestriction = true;
                organizationUser.IsMessagingUser = true;
                organizationUser.IsSystem = false;
                organizationUser.IsMessagingUser = false;
                organizationUser.DOB = View.DOB;
                organizationUser.SSN = View.SSN;
                organizationUser.PhoneNumber = View.PhoneNumber;
                organizationUser.AddressHandle = GetAddressHandle();
                organizationUser.IsInternationalPhoneNumber = View.IsInternationalPhoneNumber;

                //Adds and updates the Person Alias.
                AddUpdatePersonAlias(organizationUser);

                // AddCurrentResidentialHistory(organizationUser);
                SecurityManager.AddOrganizationUser(organizationUser, aspnetUsers);

                //Add Current Resident History
                AddCurrentResidentialHistory(organizationUser);

                organizationUser.IsDeleted = false;
                SecurityManager.AddOrganizationUserProfile(organizationUser);

                if (organizationUser.OrganizationUserID > AppConsts.NONE)
                {
                    SecurityManager.DeleteAdminOrganizationUser(organizationUser, View.CurrentLoggedInUserId);
                    GetOrganizationUserDetailsByUserID(organizationUser.UserID);
                    SetDefaultSubscription(organizationUser.OrganizationUserID);
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean AddUpdateOrderDetails()
        {
            Entity.ClientEntity.Order order = null;

            Int32 OrderId = View.AdminCreateOrderContract.FirstOrDefault().IsNullOrEmpty() ? AppConsts.NONE : View.AdminCreateOrderContract.FirstOrDefault().OrderID;

            List<Int32> previousPackageHIerarchyIds = new List<Int32>();
            List<Int32> lstBkgOrderPackageIdsToRemove = new List<Int32>();

            Boolean IsEditMode = false;

            if (OrderId > AppConsts.NONE)
            {
                order = BackgroundProcessOrderManager.GetAdminOrderDataByOrderId(OrderId, View.SelectedTenantId);
                IsEditMode = true;
            }
            else
            {
                order = new Entity.ClientEntity.Order();
            }

            if (!order.IsNullOrEmpty() && order.OrderID > AppConsts.NONE)
            {
                order.OrganizationUserProfileID = View.OrganizationUserProfile.OrganizationUserProfileID;

                if (View.SelectedNodeId > AppConsts.NONE)
                    order.SelectedNodeID = View.SelectedNodeId;
                if (View.HierarchyNodeID > AppConsts.NONE)
                    order.HierarchyNodeID = View.HierarchyNodeID;
                order.OrderDate = DateTime.Now;
                order.OrderMachineIP = View.ClientMachineIP;
                order.ModifiedByID = View.CurrentLoggedInUserId;
                order.ModifiedOn = DateTime.Now;
            }
            else
            {
                order.OrganizationUserProfileID = View.OrganizationUserProfile.OrganizationUserProfileID;
                order.IsDeleted = true;
                order.CreatedByID = View.CurrentLoggedInUserId;
                order.CreatedOn = DateTime.Now;
                order.SelectedNodeID = View.SelectedNodeId;
                order.HierarchyNodeID = View.HierarchyNodeID;
                order.ArchiveStateID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpArchiveState>(View.SelectedTenantId).Where(cond => cond.AS_Code == ArchiveState.Active.GetStringValue()).FirstOrDefault().AS_ID;
                order.OrderStatusID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderStatu>(View.SelectedTenantId).Where(cond => cond.Code == ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue()).FirstOrDefault().OrderStatusID;
                order.OrderRequestTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderRequestType>(View.SelectedTenantId).Where(cond => cond.ORT_Code == OrderRequestType.NewOrder.GetStringValue()).FirstOrDefault().ORT_ID;
                order.OrderPackageType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderPackageType>(View.SelectedTenantId).Where(cond => cond.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;
                order.OrderDate = DateTime.Now;
                order.OrderMachineIP = View.ClientMachineIP;
            }

            if (!View.lstSelectedPackageIds.IsNullOrEmpty())
            {

                Entity.ClientEntity.BkgOrder BkgOrder = order.BkgOrders.FirstOrDefault();

                if (BkgOrder.IsNullOrEmpty())
                {
                    BkgOrder = new Entity.ClientEntity.BkgOrder();

                    BkgOrder.BOR_IsAdminOrder = true;
                    BkgOrder.BOR_IsDeleted = true;
                    BkgOrder.BOR_CreatedByID = View.CurrentLoggedInUserId;
                    BkgOrder.BOR_CreatedOn = DateTime.Now;
                    BkgOrder.BOR_ArchiveStateID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpArchiveState>(View.SelectedTenantId).Where(cond => cond.AS_Code == ArchiveState.Active.GetStringValue()).FirstOrDefault().AS_ID;
                    BkgOrder.BOR_OrderStatusTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderStatusType>(View.SelectedTenantId).Where(cond => cond.Code == OrderStatusType.NEW.GetStringValue()).FirstOrDefault().OrderStatusTypeID;
                    BkgOrder.BOR_IsArchived = false;

                    if (BkgOrder.BkgOrderPackages.Count <= AppConsts.NONE || BkgOrder.BkgOrderPackages.Where(cond => !cond.BOP_IsDeleted).ToList().Count <= AppConsts.NONE)
                    {
                        BkgOrder.BkgOrderPackages = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.ClientEntity.BkgOrderPackage>();
                    }

                    if (BkgOrder.BkgAdminOrderDetails.Count <= AppConsts.NONE || BkgOrder.BkgAdminOrderDetails.Where(cond => !cond.BAOD_IsDeleted).ToList().Count <= AppConsts.NONE)
                    {
                        BkgOrder.BkgAdminOrderDetails = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.ClientEntity.BkgAdminOrderDetail>();

                        //AdminOrderStatusTable
                        Entity.ClientEntity.BkgAdminOrderDetail adminOrderDetail = new Entity.ClientEntity.BkgAdminOrderDetail();
                        adminOrderDetail.BAOD_OrderStatusID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpAdminOrderStatu>(View.SelectedTenantId).Where(cond => cond.LAOS_Code == AdminOrderStatus.DRAFT.GetStringValue()).FirstOrDefault().LAOS_ID;
                        adminOrderDetail.BAOD_AttestFCRAPrevisions = View.AttestFCRAPrevisions;
                        adminOrderDetail.BAOD_IsDeleted = false;
                        adminOrderDetail.BAOD_CreatedBy = View.CurrentLoggedInUserId;
                        adminOrderDetail.BAOD_CreatedOn = DateTime.Now;
                        adminOrderDetail.BAOD_AttestFCRACheckedBy = View.CurrentLoggedInUserId;
                        adminOrderDetail.BAOD_AttestFCRACheckedOn = DateTime.Now;

                        BkgOrder.BkgAdminOrderDetails.Add(adminOrderDetail);
                    }
                    else
                    {
                        Entity.ClientEntity.BkgAdminOrderDetail adminOrderDetail = BkgOrder.BkgAdminOrderDetails.Where(cond => !cond.BAOD_IsDeleted).FirstOrDefault();
                        adminOrderDetail.BAOD_AttestFCRAPrevisions = View.AttestFCRAPrevisions;
                        adminOrderDetail.BAOD_ModifiedBy = View.CurrentLoggedInUserId;
                        adminOrderDetail.BAOD_ModifiedOn = DateTime.Now;
                        adminOrderDetail.BAOD_AttestFCRACheckedBy = View.CurrentLoggedInUserId;
                        adminOrderDetail.BAOD_AttestFCRACheckedOn = DateTime.Now;
                        adminOrderDetail.BAOD_AttestFCRACheckedBy = View.CurrentLoggedInUserId;
                        adminOrderDetail.BAOD_AttestFCRACheckedOn = DateTime.Now;
                    }
                }
                else
                {
                    Entity.ClientEntity.BkgAdminOrderDetail adminOrderDetail = BkgOrder.BkgAdminOrderDetails.Where(cond => !cond.BAOD_IsDeleted).FirstOrDefault();
                    if (!adminOrderDetail.IsNullOrEmpty())
                    {
                        adminOrderDetail.BAOD_AttestFCRAPrevisions = View.AttestFCRAPrevisions;
                        adminOrderDetail.BAOD_ModifiedBy = View.CurrentLoggedInUserId;
                        adminOrderDetail.BAOD_ModifiedOn = DateTime.Now;
                        adminOrderDetail.BAOD_AttestFCRACheckedBy = View.CurrentLoggedInUserId;
                        adminOrderDetail.BAOD_AttestFCRACheckedOn = DateTime.Now;
                    }
                }

                BkgOrder.BOR_OrganizationUserProfileID = View.OrganizationUserProfile.OrganizationUserProfileID;

                if (BkgOrder.BkgOrderPackages.Where(cond => !cond.BOP_IsDeleted).Any())
                {
                    previousPackageHIerarchyIds = BkgOrder.BkgOrderPackages.Where(cond => !cond.BOP_IsDeleted).Select(sel => sel.BOP_BkgPackageHierarchyMappingID.Value).ToList();
                    lstBkgOrderPackageIdsToRemove = previousPackageHIerarchyIds.Except(View.lstSelectedPackageIds).ToList();
                }

                //Remove Unchecked BkgOrderPackage data
                BkgOrder.BkgOrderPackages.Where(cond => !cond.BOP_IsDeleted && lstBkgOrderPackageIdsToRemove.Contains(cond.BOP_BkgPackageHierarchyMappingID.Value)).ToList().ForEach(x =>
                {
                    x.BOP_IsDeleted = true;
                    x.BOP_ModifiedByID = View.CurrentLoggedInUserId;
                    x.BOP_ModifiedOn = DateTime.Now;
                });

                foreach (Int32 pkgHierarchyId in View.lstSelectedPackageIds)
                {
                    Int32 pkgId = View.lstBkgPackage.Where(cond => cond.BPHMId == pkgHierarchyId).FirstOrDefault().BPAId;
                    List<Int32> lkpSvcGroupIds = BackgroundProcessOrderManager.GetBkgSvcGroupDetailsByBkgPkgId(pkgId, View.SelectedTenantId);

                    if (!previousPackageHIerarchyIds.Contains(pkgHierarchyId))
                    {
                        //BkgOrderPackge
                        Entity.ClientEntity.BkgOrderPackage bkgOrderPackage = new Entity.ClientEntity.BkgOrderPackage();

                        bkgOrderPackage.BOP_CreatedByID = View.CurrentLoggedInUserId;
                        bkgOrderPackage.BOP_CreatedOn = DateTime.Now;
                        bkgOrderPackage.BOP_IsDeleted = false;
                        bkgOrderPackage.BOP_BkgPackageHierarchyMappingID = pkgHierarchyId;


                        //BkgOrderpackageSvcGroup
                        bkgOrderPackage.BkgOrderPackageSvcGroups = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.ClientEntity.BkgOrderPackageSvcGroup>();
                        foreach (Int32 svcGrpId in lkpSvcGroupIds)
                        {
                            Entity.ClientEntity.BkgOrderPackageSvcGroup bkgOrderPkgSvcGroup = new Entity.ClientEntity.BkgOrderPackageSvcGroup();

                            bkgOrderPkgSvcGroup.OPSG_IsDeleted = false;
                            bkgOrderPkgSvcGroup.OPSG_CreatedByID = View.CurrentLoggedInUserId;
                            bkgOrderPkgSvcGroup.OPSG_CreatedOn = DateTime.Now;
                            bkgOrderPkgSvcGroup.OPSG_BkgSvcGroupID = svcGrpId;
                            bkgOrderPkgSvcGroup.OPSG_SvcGrpStatusTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpBkgSvcGrpStatusType>(View.SelectedTenantId)
                                                    .Where(cond => cond.BSGS_StatusCode == BkgSvcGrpStatusType.NEW.GetStringValue()).FirstOrDefault().BSGS_ID;
                            bkgOrderPkgSvcGroup.OPSG_SvcGrpReviewStatusTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpBkgSvcGrpReviewStatusType>(View.SelectedTenantId)
                                                    .Where(cond => cond.BSGRS_ReviewCode == BkgSvcGrpReviewStatusType.NEW.GetStringValue()).FirstOrDefault().BSGRS_ID;

                            bkgOrderPkgSvcGroup.BkgOrderPackageSvcs = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.ClientEntity.BkgOrderPackageSvc>();

                            List<Int32> lkpSvcIds = BackgroundProcessOrderManager.GetBackgroundServiceIdsBysvcGrpId(svcGrpId, pkgId, View.SelectedTenantId);

                            foreach (Int32 SvcId in lkpSvcIds)
                            {
                                Entity.ClientEntity.BkgOrderPackageSvc bkgOrderPkgSvc = new Entity.ClientEntity.BkgOrderPackageSvc();
                                bkgOrderPkgSvc.BOPS_IsDeleted = false;
                                bkgOrderPkgSvc.BOPS_CreatedByID = View.CurrentLoggedInUserId;
                                bkgOrderPkgSvc.BOPS_CreatedOn = DateTime.Now;
                                bkgOrderPkgSvc.BOPS_BackgroundServiceID = SvcId;

                                bkgOrderPkgSvcGroup.BkgOrderPackageSvcs.Add(bkgOrderPkgSvc);
                            }

                            bkgOrderPackage.BkgOrderPackageSvcGroups.Add(bkgOrderPkgSvcGroup);
                        }

                        BkgOrder.BkgOrderPackages.Add(bkgOrderPackage);
                    }
                }

                if (!IsEditMode)
                {
                    order.BkgOrders.Add(BkgOrder);
                }
            }
            else
            {
                if (!order.BkgOrders.FirstOrDefault().IsNullOrEmpty())
                {
                    if (!order.BkgOrders.FirstOrDefault().IsNullOrEmpty())
                    {
                        order.BkgOrders.FirstOrDefault().BkgOrderPackages.Where(cond => !cond.BOP_IsDeleted).ForEach(x =>
                        {
                            x.BOP_IsDeleted = true;
                            x.BOP_ModifiedByID = View.CurrentLoggedInUserId;
                            x.BOP_ModifiedOn = DateTime.Now;
                        });
                    }

                    order.BkgOrders.FirstOrDefault().BOR_OrganizationUserProfileID = View.OrganizationUserProfile.OrganizationUserProfileID;
                    order.BkgOrders.FirstOrDefault().BOR_ModifiedByID = View.CurrentLoggedInUserId;
                    order.BkgOrders.FirstOrDefault().BOR_ModifiedOn = DateTime.Now;
                }
                else
                {
                    order.BkgOrders.Add(SaveDefaultBkgOrderDetails());
                }
            }

            order = BackgroundProcessOrderManager.SaveAdminOrderDetails(order, View.SelectedTenantId);
            if (order.OrderID > AppConsts.NONE)
            {
                //Remove custom form details if any changes in package
                List<Int32> CurrentPackageHIerarchyIds = order.BkgOrders.FirstOrDefault().BkgOrderPackages.Where(cond => !cond.BOP_IsDeleted).Select(sel => sel.BOP_BkgPackageHierarchyMappingID.Value).ToList();
                if (lstBkgOrderPackageIdsToRemove.Count > AppConsts.NONE
                    || (previousPackageHIerarchyIds.Count == AppConsts.NONE && CurrentPackageHIerarchyIds.Count > AppConsts.NONE)
                    || (previousPackageHIerarchyIds.Count != CurrentPackageHIerarchyIds.Count))
                {
                    //UAT-2914:Add ability to update/change packages in HR module once order has been created
                    String packageIDs = String.Empty;
                    var packageHierarchyList = order.BkgOrders.FirstOrDefault().BkgOrderPackages.Where(cond => !cond.BOP_IsDeleted)
                                                                                                .Select(sel => sel.BkgPackageHierarchyMapping).ToList();
                    List<Int32> lstPackageIDs = packageHierarchyList.IsNullOrEmpty() ? new List<Int32>() : packageHierarchyList.Select(slct => slct.BPHM_BackgroundPackageID).ToList();
                    if (!lstPackageIDs.IsNullOrEmpty())
                    {
                        packageIDs = String.Join(",", lstPackageIDs);
                    }
                    BackgroundProcessOrderManager.DeleteCustomFormData(order.BkgOrders.FirstOrDefault().BOR_ID, View.CurrentLoggedInUserId, View.SelectedTenantId, packageIDs);
                }

                View.OrderID = order.OrderID;
                //var createOrderData = BackgroundProcessOrderManager.GetAdminOrderDetailsByOrderId(View.OrderID, View.SelectedTenantId);
                //List<AdminCreateOrderContract> lstAdminOrderDetails = createOrderData.Item1;
                //View.lstBkgOrderData = createOrderData.Item2;

                //if (!lstAdminOrderDetails.IsNullOrEmpty())
                //{
                //    View.AdminCreateOrderContract = lstAdminOrderDetails;
                //    if (!lstAdminOrderDetails.FirstOrDefault().IsNullOrEmpty())
                //    {
                //        View.BkgOrderId = lstAdminOrderDetails.FirstOrDefault().BKgOrderID;
                //        Guid UserId = new Guid(lstAdminOrderDetails.FirstOrDefault().UserID);
                //        GetOrganizationUserDetailsByUserID(UserId);
                //    }
                //}
                //View.IsOrderReadyForTransmit = View.AdminCreateOrderContract.Any() ? View.AdminCreateOrderContract.FirstOrDefault().AdminOrderStatusType.Equals(AdminOrderStatusTypeOptions.ReadyForTransmit.GetStringValue()) ? true : false : false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveDefaultOrderDetail()
        {
            Entity.ClientEntity.Order order = new Entity.ClientEntity.Order();

            order.OrganizationUserProfileID = View.OrganizationUserProfile.OrganizationUserProfileID;
            order.IsDeleted = true;
            order.CreatedByID = View.CurrentLoggedInUserId;
            order.CreatedOn = DateTime.Now;
            order.ArchiveStateID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpArchiveState>(View.SelectedTenantId).Where(cond => cond.AS_Code == ArchiveState.Active.GetStringValue()).FirstOrDefault().AS_ID;
            order.OrderStatusID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderStatu>(View.SelectedTenantId).Where(cond => cond.Code == ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue()).FirstOrDefault().OrderStatusID;
            order.OrderRequestTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderRequestType>(View.SelectedTenantId).Where(cond => cond.ORT_Code == OrderRequestType.NewOrder.GetStringValue()).FirstOrDefault().ORT_ID;
            order.OrderDate = DateTime.Now;
            order.OrderMachineIP = View.ClientMachineIP;

            Entity.ClientEntity.BkgOrder BkgOrder = SaveDefaultBkgOrderDetails();

            order.BkgOrders.Add(BkgOrder);

            order = BackgroundProcessOrderManager.SaveAdminOrderDetails(order, View.SelectedTenantId);

            View.OrderID = order.OrderID;
            //var createOrderDetails = BackgroundProcessOrderManager.GetAdminOrderDetailsByOrderId(View.OrderID, View.SelectedTenantId);
            //List<AdminCreateOrderContract> lstAdminOrderDetails = createOrderDetails.Item1;
            //View.lstBkgOrderData = createOrderDetails.Item2;
            //if (!lstAdminOrderDetails.IsNullOrEmpty())
            //{
            //    View.AdminCreateOrderContract = lstAdminOrderDetails;
            //    if (!lstAdminOrderDetails.FirstOrDefault().IsNullOrEmpty())
            //    {
            //        View.BkgOrderId = lstAdminOrderDetails.FirstOrDefault().BKgOrderID;
            //        Guid UserId = new Guid(lstAdminOrderDetails.FirstOrDefault().UserID);
            //        GetOrganizationUserDetailsByUserID(UserId);
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Entity.ClientEntity.BkgOrder SaveDefaultBkgOrderDetails()
        {
            Entity.ClientEntity.BkgOrder BkgOrder = new Entity.ClientEntity.BkgOrder();
            BkgOrder.BOR_IsAdminOrder = true;
            BkgOrder.BOR_IsDeleted = true;
            BkgOrder.BOR_CreatedByID = View.CurrentLoggedInUserId;
            BkgOrder.BOR_CreatedOn = DateTime.Now;
            BkgOrder.BOR_ArchiveStateID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpArchiveState>(View.SelectedTenantId).Where(cond => cond.AS_Code == ArchiveState.Active.GetStringValue()).FirstOrDefault().AS_ID;
            BkgOrder.BOR_IsArchived = false;
            BkgOrder.BOR_OrganizationUserProfileID = View.OrganizationUserProfile.OrganizationUserProfileID;

            //BkgOrderPackge
            //BkgOrder.BkgOrderPackages = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.ClientEntity.BkgOrderPackage>();
            //Entity.ClientEntity.BkgOrderPackage bkgOrderPackage = new Entity.ClientEntity.BkgOrderPackage();

            //bkgOrderPackage.BOP_CreatedByID = View.CurrentLoggedInUserId;
            //bkgOrderPackage.BOP_CreatedOn = DateTime.Now;
            //bkgOrderPackage.BOP_IsDeleted = false;


            //BkgOrderpackageSvcGroup
            //bkgOrderPackage.BkgOrderPackageSvcGroups = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.ClientEntity.BkgOrderPackageSvcGroup>();
            //Entity.ClientEntity.BkgOrderPackageSvcGroup bkgOrderPkgSvcGroup = new Entity.ClientEntity.BkgOrderPackageSvcGroup();

            //bkgOrderPkgSvcGroup.OPSG_IsDeleted = false;
            //bkgOrderPkgSvcGroup.OPSG_CreatedByID = View.CurrentLoggedInUserId;
            //bkgOrderPkgSvcGroup.OPSG_CreatedOn = DateTime.Now;
            //bkgOrderPkgSvcGroup.OPSG_SvcGrpStatusTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpBkgSvcGrpStatusType>(View.SelectedTenantId)
            //                        .Where(cond => cond.BSGS_StatusCode == BkgSvcGrpStatusType.NEW.GetStringValue()).FirstOrDefault().BSGS_ID;
            //bkgOrderPkgSvcGroup.OPSG_SvcGrpReviewStatusTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpBkgSvcGrpReviewStatusType>(View.SelectedTenantId)
            //                        .Where(cond => cond.BSGRS_ReviewCode == BkgSvcGrpReviewStatusType.NEW.GetStringValue()).FirstOrDefault().BSGRS_ID;

            //bkgOrderPkgSvcGroup.BkgOrderPackageSvcs = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.ClientEntity.BkgOrderPackageSvc>();
            //Entity.ClientEntity.BkgOrderPackageSvc bkgOrderPkgSvc = new Entity.ClientEntity.BkgOrderPackageSvc();

            //bkgOrderPkgSvc.BOPS_IsDeleted = false;
            //bkgOrderPkgSvc.BOPS_CreatedByID = View.CurrentLoggedInUserId;
            //bkgOrderPkgSvc.BOPS_CreatedOn = DateTime.Now;

            //bkgOrderPkgSvcGroup.BkgOrderPackageSvcs.Add(bkgOrderPkgSvc);
            //bkgOrderPackage.BkgOrderPackageSvcGroups.Add(bkgOrderPkgSvcGroup);
            //BkgOrder.BkgOrderPackages.Add(bkgOrderPackage);

            //AdminOrderStatusTable
            BkgOrder.BkgAdminOrderDetails = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.ClientEntity.BkgAdminOrderDetail>();
            Entity.ClientEntity.BkgAdminOrderDetail adminOrderDetail = new Entity.ClientEntity.BkgAdminOrderDetail();

            adminOrderDetail.BAOD_OrderStatusID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpAdminOrderStatu>(View.SelectedTenantId).Where(cond => cond.LAOS_Code == AdminOrderStatus.NEW.GetStringValue()).FirstOrDefault().LAOS_ID;
            adminOrderDetail.BAOD_AttestFCRAPrevisions = View.AttestFCRAPrevisions;
            adminOrderDetail.BAOD_IsDeleted = false;
            adminOrderDetail.BAOD_CreatedBy = View.CurrentLoggedInUserId;
            adminOrderDetail.BAOD_CreatedOn = DateTime.Now;
            adminOrderDetail.BAOD_AttestFCRACheckedBy = View.CurrentLoggedInUserId;
            adminOrderDetail.BAOD_AttestFCRACheckedOn = DateTime.Now;

            BkgOrder.BkgAdminOrderDetails.Add(adminOrderDetail);

            return BkgOrder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean DeleteAdminOrderDetails()
        {
            return BackgroundProcessOrderManager.DeleteAdminOrderDetails(View.SelectedTenantId, View.CurrentLoggedInUserId, View.OrderID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="docTypeCode"></param>
        /// <returns></returns>
        public Int32 GetlkpDocTypeMapping(String docTypeCode)
        {
            return BackgroundProcessOrderManager.GetDocumentTypeIdByCode(docTypeCode, View.SelectedTenantId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentName"></param>
        /// <param name="documentSize"></param>
        /// <param name="documentUploadedBytes"></param>
        /// <param name="docTypeCode"></param>
        /// <returns></returns>
        public String IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, byte[] documentUploadedBytes, String docTypeCode)
        {
            Int32 ApplicantOrgUserID = View.OrganizationUser.OrganizationUserID;

            if (BackgroundProcessOrderManager.IsDocumentAlreadyUploaded(documentName, documentSize, ApplicantOrgUserID, docTypeCode, View.SelectedTenantId))
                return documentName;

            List<ApplicantDocumentContract> lstApplicantDocumentContract = View.lstApplicantDocumentContract;
            String md5Hash = GetMd5Hash(documentUploadedBytes);

            //if (!lstApplicantDocumentContract.IsNullOrEmpty())
            //{
            //    ApplicantDocumentContract docDetails = lstApplicantDocumentContract.Where(cond => cond.md == md5Hash).FirstOrDefault();

            //    if (!docDetails.IsNullOrEmpty())
            //        return docDetails.FileName;
            //}

            return String.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ApplicantDocTypeID"></param>
        /// <returns></returns>
        public Boolean IsDandAAlreadyUploaded(Int32 ApplicantDocTypeID)
        {
            return BackgroundProcessOrderManager.IsDandAAlreadyUploaded(View.OrganizationUser.OrganizationUserID, ApplicantDocTypeID, View.SelectedTenantId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentUploadedBytes"></param>
        /// <returns></returns>
        public String GetMd5Hash(byte[] documentUploadedBytes)
        {
            return CommonFileManager.GetMd5Hash(documentUploadedBytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean SaveApplicantDocumentDetails()
        {
            return BackgroundProcessOrderManager.SaveApplicantDocumentDetails(View.SelectedTenantId, View.lstApplicantDocument, View.CurrentLoggedInUserId, View.OrganizationUserProfile.OrganizationUserProfileID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean SaveGenericDocumentMapping()
        {
            List<Int32> applicantDocIds = View.lstApplicantDocumentContract.Select(x => x.ApplicantDocumentID).ToList();
            return BackgroundProcessOrderManager.SaveUpdateGenericDocumentmapping(View.SelectedTenantId, applicantDocIds, View.CurrentLoggedInUserId, View.OrganizationUserProfile.OrganizationUserProfileID);
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetApplicantDocuments()
        {
            List<ApplicantDocumentContract> lstApplicantDocumentContract = new List<ApplicantDocumentContract>();
            View.lstApplicantDocumentContract = BackgroundProcessOrderManager.GetApplicantDocuments(View.SelectedTenantId, View.OrganizationUser.OrganizationUserID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean DeleteApplicantDocuments()
        {
            return BackgroundProcessOrderManager.DeleteApplicantDocuments(View.SelectedTenantId, View.OrganizationUser.OrganizationUserID, View.CurrentLoggedInUserId);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        private void GetOrganizationUserDetailsByUserID(Guid UserID)
        {
            View.OrganizationUser = BackgroundProcessOrderManager.GetOrganisationUserByUserID(UserID, View.SelectedTenantId);
            if (!View.OrganizationUser.OrganizationUserProfiles.IsNullOrEmpty())
            {
                View.OrganizationUserProfile = View.OrganizationUser.OrganizationUserProfiles.Where(cond => !cond.IsDeleted).FirstOrDefault();
            }
            View.ResidentialHistoryListAll = GetResidentialHistories(View.OrganizationUser.UserID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgUser"></param>
        private void AddOrganizationUserProfile(Entity.ClientEntity.OrganizationUser orgUser)
        {
            Boolean isAddMode = false;
            Entity.ClientEntity.OrganizationUserProfile orgUserProfile = orgUser.OrganizationUserProfiles.FirstOrDefault();

            if (orgUserProfile.IsNullOrEmpty())
            {
                orgUserProfile = new Entity.ClientEntity.OrganizationUserProfile();
                isAddMode = true;
            }

            orgUserProfile.AddressHandleID = orgUser.AddressHandleID;
            orgUserProfile.FirstName = orgUser.FirstName;
            orgUserProfile.MiddleName = orgUser.MiddleName;
            orgUserProfile.LastName = orgUser.LastName;
            orgUserProfile.IsDeleted = false;
            orgUserProfile.IsActive = orgUser.IsActive;
            orgUserProfile.ExpireDate = orgUser.ExpireDate;
            orgUserProfile.CreatedByID = orgUser.CreatedByID;
            orgUserProfile.CreatedOn = orgUser.CreatedOn;
            orgUserProfile.ModifiedByID = orgUser.ModifiedByID;
            orgUserProfile.ModifiedOn = orgUser.ModifiedOn;
            orgUserProfile.DOB = orgUser.DOB;
            orgUserProfile.SSN = orgUser.SSN;
            orgUserProfile.Gender = orgUser.Gender;
            orgUserProfile.PhoneNumber = orgUser.PhoneNumber;
            orgUserProfile.PrimaryEmailAddress = orgUser.PrimaryEmailAddress;

            //if (orgUser.PersonAlias.IsNotNull())
            //{
            //    List<Entity.ClientEntity.PersonAlia> currentAliasList = orgUser.PersonAlias.Where(x => x.PA_IsDeleted == false).ToList();
            //    foreach (Entity.ClientEntity.PersonAlia tempPersonAlias in currentAliasList)
            //    {
            //        Entity.ClientEntity.PersonAliasProfile personAliasProfile = new Entity.ClientEntity.PersonAliasProfile();
            //        personAliasProfile.PAP_FirstName = tempPersonAlias.PA_FirstName;
            //        personAliasProfile.PAP_LastName = tempPersonAlias.PA_LastName;
            //        personAliasProfile.PAP_MiddleName = tempPersonAlias.PA_MiddleName;
            //        personAliasProfile.PAP_IsDeleted = false;
            //        personAliasProfile.PAP_CreatedBy = tempPersonAlias.PA_CreatedBy;
            //        personAliasProfile.PAP_CreatedOn = DateTime.Now;
            //        orgUserProfile.PersonAliasProfiles.Add(personAliasProfile);
            //    }
            //}
            if (isAddMode)
            {
                orgUser.OrganizationUserProfiles.Add(orgUserProfile);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUser"></param>
        private void AddCurrentResidentialHistory(OrganizationUser organizationUser)
        {
            ResidentialHistory currentResedentialHistory = new ResidentialHistory();
            currentResedentialHistory.RHI_IsCurrentAddress = true;
            currentResedentialHistory.RHI_IsPrimaryResidence = false;
            currentResedentialHistory.RHI_IsDeleted = false;
            currentResedentialHistory.RHI_CreatedByID = AppConsts.NONE;
            currentResedentialHistory.RHI_CreatedOn = DateTime.Now;
            currentResedentialHistory.RHI_OrganizationUserID = organizationUser.OrganizationUserID;
            currentResedentialHistory.RHI_AddressId = organizationUser.AddressHandle.Addresses.FirstOrDefault().AddressID;
            currentResedentialHistory.RHI_SequenceOrder = AppConsts.ONE;
            organizationUser.ResidentialHistories.Add(currentResedentialHistory);

            AddUpdateResidentialHistories(organizationUser);

            SecurityManager.UpdateOrganizationUser(organizationUser);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUser"></param>
        private void AddUpdatePersonAlias(OrganizationUser organizationUser)
        {
            if (View.PersonAliasList.IsNotNull())
            {
                List<PersonAlia> currentAliasList = organizationUser.PersonAlias.Where(x => x.PA_IsDeleted == false).ToList();
                foreach (PersonAliasContract tempPersonAlias in View.PersonAliasList)
                {
                    if (tempPersonAlias.ID > 0)
                    {
                        PersonAlia personAlias = currentAliasList.FirstOrDefault(x => x.PA_ID == tempPersonAlias.ID);
                        if (personAlias.IsNotNull())
                        {
                            personAlias.PA_FirstName = tempPersonAlias.FirstName;
                            personAlias.PA_LastName = tempPersonAlias.LastName;
                            personAlias.PA_MiddleName = tempPersonAlias.MiddleName.IsNullOrEmpty() ? "----" : tempPersonAlias.MiddleName;
                            personAlias.PA_ModifiedBy = AppConsts.NONE;
                            personAlias.PA_ModifiedOn = DateTime.Now;
                        }
                    }
                    else
                    {
                        PersonAlia personAlias = new PersonAlia();
                        personAlias.PA_FirstName = tempPersonAlias.FirstName;
                        personAlias.PA_LastName = tempPersonAlias.LastName;
                        personAlias.PA_MiddleName = tempPersonAlias.MiddleName.IsNullOrEmpty() ? "----" : tempPersonAlias.MiddleName;
                        personAlias.PA_IsDeleted = false;
                        personAlias.PA_CreatedBy = AppConsts.NONE;
                        personAlias.PA_CreatedOn = DateTime.Now;
                        personAlias.PA_AliasIdentifier = Guid.NewGuid();
                        organizationUser.PersonAlias.Add(personAlias);
                    }
                }
                List<Int32> aliasIDToBeDeleted = currentAliasList.Select(x => x.PA_ID).Except(View.PersonAliasList.Select(y => y.ID)).ToList();
                foreach (Int32 delAliasID in aliasIDToBeDeleted)
                {
                    PersonAlia delAlias = currentAliasList.FirstOrDefault(x => x.PA_IsDeleted == false && x.PA_ID == delAliasID);
                    delAlias.PA_IsDeleted = true;
                    delAlias.PA_ModifiedBy = AppConsts.NONE;
                    delAlias.PA_ModifiedOn = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private AddressHandle GetAddressHandle()
        {
            Guid addressHandleId = Guid.NewGuid();

            AddressHandle addressHandle = new AddressHandle();
            addressHandle.AddressHandleID = addressHandleId;
            addressHandle.Addresses = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Address>();
            addressHandle.Addresses.Add(new Address()
            {

                AddressHandleID = addressHandleId,
                Address1 = View.Address1,
                Address2 = View.Address2,
                ZipCodeID = View.ZipId,
                CreatedByID = 0,
                IsActive = true,
                CreatedOn = DateTime.Now
            });
            if (View.ZipId == 0)
            {
                addressHandle.Addresses.FirstOrDefault().AddressExts = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<AddressExt>();
                addressHandle.Addresses.FirstOrDefault().AddressExts.Add(new AddressExt()
                {
                    AE_CountryID = View.CountryId,
                    AE_StateName = View.StateName,
                    AE_CityName = View.CityName,
                    AE_ZipCode = View.PostalCode,
                });
            }
            return addressHandle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<lkpGender> GetGenderList()
        {
            return SecurityManager.GetGender().ToList();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adress"></param>
        public void UpdateAddress(Address adress)
        {
            adress.Address1 = View.Address1;
            adress.Address2 = View.Address2;
            adress.ZipCodeID = View.ZipId;
            adress.CreatedByID = 0;
            adress.IsActive = true;
            adress.CreatedOn = DateTime.Now;
        }

        /// <summary>
        /// To save user profile data
        /// </summary>
        /// <returns></returns>
        public Boolean SaveUserData()
        {
            OrganizationUser organizationUser = SecurityManager.GetOrganizationUserOfAdminOrder(View.OrganizationUser.UserID);

            organizationUser.aspnet_Users.MobileAlias = View.PhoneNumber;
            organizationUser.Organization = SecurityManager.GetOrganizationForTenantID(View.SelectedTenantId);
            organizationUser.FirstName = View.FirstName;
            organizationUser.LastName = View.LastName;
            organizationUser.MiddleName = View.MiddleName;
            organizationUser.IsApplicant = true;
            organizationUser.ModifiedByID = View.CurrentLoggedInUserId;
            organizationUser.ModifiedOn = DateTime.Now;
            organizationUser.DOB = View.DOB;
            organizationUser.Gender = View.Gender;
            organizationUser.PhoneNumber = View.PhoneNumber;
            organizationUser.PrimaryEmailAddress = View.Email;
            organizationUser.SSN = View.SSN;
            organizationUser.IsInternationalPhoneNumber = View.IsInternationalPhoneNumber;

            //Adds and updates the Person Alias.
            AddUpdatePersonAlias(organizationUser);

            Address addressNew = null;
            AddressExt addressExtNew = null;
            //Check if Address Handle is not null
            if (organizationUser.AddressHandle.IsNotNull())
            {
                var address = organizationUser.AddressHandle.Addresses.FirstOrDefault();
                //Check if Address not null
                if (address.IsNotNull())
                {
                    //Check if current address has been modified.
                    if (CheckIfAddressUpdated(address))
                    {
                        addressNew = new Address();
                        Dictionary<String, Object> dicAddressData = GetAddressDataDictionary();
                        if (View.ZipId == 0)
                        {
                            addressExtNew = new AddressExt();
                            addressExtNew.AE_CountryID = View.CountryId;
                            addressExtNew.AE_StateName = View.StateName;
                            addressExtNew.AE_CityName = View.CityName;
                            addressExtNew.AE_ZipCode = View.PostalCode;
                        }
                        Guid addressHandleId = Guid.NewGuid();
                        SecurityManager.AddAddressHandle(addressHandleId);
                        SecurityManager.AddAddress(dicAddressData, addressHandleId, View.CurrentLoggedInUserId, addressNew, addressExtNew);

                        SecurityManager.UpdateChanges();
                        ClientSecurityManager.AddAddressHandle(View.SelectedTenantId, addressHandleId);
                        ClientSecurityManager.AddAddress(View.SelectedTenantId, dicAddressData, addressHandleId, View.CurrentLoggedInUserId, addressNew, addressExtNew);
                        organizationUser.AddressHandleID = addressHandleId;
                        organizationUser.AddressHandle.Addresses.Add(addressNew);
                    }
                }
            }
            ////Adds and updates the residential histories.
            AddUpdateResidentialHistory(organizationUser, addressNew);

            organizationUser.IsDeleted = false;

            BackgroundProcessOrderManager.DeleteOldOrganizationUserProfileData(View.OrganizationUser.UserID, View.SelectedTenantId);

            OrganizationUserProfile organizationUserProfile = SecurityManager.AddOrganizationUserProfile(organizationUser);

            organizationUser.IsDeleted = true;

            if (SecurityManager.UpdateOrganizationUser(organizationUser))
            {
                SecurityManager.DeleteAdminOrganizationUser(organizationUser, View.CurrentLoggedInUserId);
                if (organizationUser.OrganizationUserID > AppConsts.NONE)
                {
                    SecurityManager.DeleteAdminOrganizationUser(organizationUser, View.CurrentLoggedInUserId);
                    GetOrganizationUserDetailsByUserID(organizationUser.UserID);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objAddress"></param>
        /// <returns></returns>
        private Boolean CheckIfAddressUpdated(Address objAddress)
        {
            if (objAddress.Address1.ToLower().Trim() != View.Address1.ToLower().Trim()
                 || objAddress.Address2.ToLower().Trim() != View.Address2.ToLower().Trim()
                 || objAddress.ZipCodeID != View.ZipId)
            {
                return true;
            }
            var addressExt = objAddress.AddressExts.FirstOrDefault();
            if (addressExt.IsNotNull())
            {
                if (addressExt.AE_CountryID != View.CountryId
                 || addressExt.AE_StateName.ToLower().Trim() != View.StateName.ToLower().Trim()
                 || addressExt.AE_CityName.ToLower().Trim() != View.CityName.ToLower().Trim()
                 || addressExt.AE_ZipCode.ToLower().Trim() != View.PostalCode.ToLower().Trim())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUser"></param>
        /// <param name="addressNew"></param>
        private void AddUpdateResidentialHistory(OrganizationUser organizationUser, Address addressNew)
        {
            //Current residential Address
            ResidentialHistory currentResedentialHistory = organizationUser.ResidentialHistories.FirstOrDefault(x => x.RHI_IsCurrentAddress == true && x.RHI_IsDeleted == false);
            if (currentResedentialHistory.IsNotNull())
            {
                //Update current residential address.
                currentResedentialHistory.Address = addressNew == null ? organizationUser.AddressHandle.Addresses.FirstOrDefault() : addressNew;
                currentResedentialHistory.RHI_ResidenceStartDate = DateTime.Now;
                currentResedentialHistory.RHI_ModifiedByID = View.CurrentLoggedInUserId;
                currentResedentialHistory.RHI_CreatedByID = View.CurrentLoggedInUserId;
                currentResedentialHistory.RHI_SequenceOrder = AppConsts.ONE;
                currentResedentialHistory.RHI_ModifiedOn = DateTime.Now;
            }
            else
            {
                currentResedentialHistory = new ResidentialHistory();
                currentResedentialHistory.RHI_IsCurrentAddress = true;
                currentResedentialHistory.RHI_IsPrimaryResidence = false;
                //currentResedentialHistory.RHI_ResidenceStartDate = View.DateResidentFrom;
                currentResedentialHistory.RHI_IsDeleted = false;
                currentResedentialHistory.RHI_CreatedByID = View.CurrentLoggedInUserId;
                currentResedentialHistory.RHI_CreatedOn = DateTime.Now;
                currentResedentialHistory.RHI_OrganizationUserID = organizationUser.OrganizationUserID;
                currentResedentialHistory.RHI_SequenceOrder = AppConsts.ONE;
                currentResedentialHistory.Address = addressNew == null ? organizationUser.AddressHandle.Addresses.FirstOrDefault() : addressNew;
                organizationUser.ResidentialHistories.Add(currentResedentialHistory);
            }

            //else
            //{
            //    //ADd current residential address.
            //    currentResedentialHistory = new ResidentialHistory();
            //    currentResedentialHistory.RHI_IsCurrentAddress = true;
            //    currentResedentialHistory.RHI_IsPrimaryResidence = false;
            //    currentResedentialHistory.RHI_IsDeleted = false;
            //    currentResedentialHistory.RHI_CreatedByID = AppConsts.NONE;
            //    currentResedentialHistory.RHI_CreatedOn = DateTime.Now;
            //    currentResedentialHistory.RHI_OrganizationUserID = organizationUser.OrganizationUserID;
            //    currentResedentialHistory.RHI_AddressId = organizationUser.AddressHandle.Addresses.FirstOrDefault().AddressID;
            //    currentResedentialHistory.RHI_SequenceOrder = AppConsts.ONE;
            //    organizationUser.ResidentialHistories.Add(currentResedentialHistory);


            //    SecurityManager.UpdateOrganizationUser(organizationUser);
            //}

            AddUpdateResidentialHistories(organizationUser);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUser"></param>
        private void AddUpdateResidentialHistories(OrganizationUser organizationUser)
        {
            //List of residential histories.
            List<PreviousAddressContract> previousAddressList = View.ResidentialHistoryList;
            if (previousAddressList.IsNotNull())
            {
                previousAddressList = previousAddressList.Where(x => x.isDeleted == true || x.isNew == true || x.isUpdated == true).ToList();
                if (previousAddressList.Count > 0)
                {
                    // List of Resedential Histories associated with the organisaion User ID.
                    List<ResidentialHistory> lstResedentialHistory = organizationUser.ResidentialHistories.Where(x => x.RHI_IsDeleted == false).ToList();
                    // List of Resedential Histories to be deleted.
                    List<Int32> lstResHisIDToBeDel = previousAddressList.Where(x => x.isDeleted == true).Select(y => y.ID).ToList();
                    List<ResidentialHistory> lstResHisToBeDel = lstResedentialHistory.Where(x => lstResHisIDToBeDel.Contains(x.RHI_ID)).ToList();
                    foreach (var prevAddress in lstResHisToBeDel)
                    {
                        prevAddress.RHI_IsDeleted = true;
                        prevAddress.RHI_ModifiedByID = View.CurrentLoggedInUserId;
                        prevAddress.RHI_ModifiedOn = DateTime.Now;
                    }

                    // List of Resedential Histories to be added.
                    List<PreviousAddressContract> lstResHisIDToBeAdded = previousAddressList.Where(x => x.isNew == true).ToList();
                    foreach (var prevAddress in lstResHisIDToBeAdded)
                    {
                        Address addressPervious = AddNewPreviousAddress(prevAddress);

                        ResidentialHistory newResidentialHistory = new ResidentialHistory();
                        newResidentialHistory.RHI_AddressId = addressPervious.AddressID;
                        newResidentialHistory.RHI_ResidenceStartDate = prevAddress.ResidenceStartDate;
                        newResidentialHistory.RHI_ResidenceEndDate = prevAddress.ResidenceEndDate;
                        newResidentialHistory.RHI_IsCurrentAddress = false;
                        newResidentialHistory.RHI_IsDeleted = false;
                        newResidentialHistory.RHI_CreatedByID = View.CurrentLoggedInUserId;
                        newResidentialHistory.RHI_CreatedOn = DateTime.Now;
                        newResidentialHistory.RHI_SequenceOrder = prevAddress.ResHistorySeqOrdID;
                        organizationUser.ResidentialHistories.Add(newResidentialHistory);
                    }

                    // List of Resedential Histories to be updated.
                    List<PreviousAddressContract> lstResHisToBeUpdated = previousAddressList.Where(x => x.isUpdated == true).ToList();
                    foreach (var prevAddress in lstResHisToBeUpdated)
                    {
                        ResidentialHistory resHistory = lstResedentialHistory.FirstOrDefault(x => x.RHI_ID == prevAddress.ID && x.RHI_IsDeleted == false);
                        if (resHistory.IsNotNull())
                        {
                            if (CheckIfPreviousAddressUpdated(resHistory.Address, prevAddress))
                            {
                                Address addressPervious = AddNewPreviousAddress(prevAddress);
                                resHistory.RHI_AddressId = addressPervious.AddressID;
                            }
                            resHistory.RHI_ResidenceStartDate = prevAddress.ResidenceStartDate;
                            resHistory.RHI_ResidenceEndDate = prevAddress.ResidenceEndDate;
                            resHistory.Address.ModifiedByID = View.CurrentLoggedInUserId;
                            resHistory.RHI_ModifiedByID = View.CurrentLoggedInUserId;
                            resHistory.RHI_CreatedByID = View.CurrentLoggedInUserId;
                            resHistory.Address.ModifiedOn = DateTime.Now;
                            resHistory.RHI_ModifiedOn = DateTime.Now;
                            resHistory.RHI_SequenceOrder = prevAddress.ResHistorySeqOrdID;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objAddress"></param>
        /// <param name="prevAddress"></param>
        /// <returns></returns>
        private Boolean CheckIfPreviousAddressUpdated(Address objAddress, PreviousAddressContract prevAddress)
        {
            if (objAddress.Address1.ToLower().Trim() != prevAddress.Address1.ToLower().Trim()
                 || objAddress.Address2.ToLower().Trim() != prevAddress.Address2.ToLower().Trim()
                 || objAddress.ZipCodeID != prevAddress.ZipCodeID)
            {
                return true;
            }
            var addressExt = objAddress.AddressExts.FirstOrDefault();
            if (addressExt.IsNotNull())
            {
                if (addressExt.AE_CountryID != prevAddress.CountryId
                 || addressExt.AE_StateName.ToLower().Trim() != prevAddress.StateName.ToLower().Trim()
                 || addressExt.AE_CityName.ToLower().Trim() != prevAddress.CityName.ToLower().Trim()
                 || addressExt.AE_ZipCode.ToLower().Trim() != prevAddress.Zipcode.ToLower().Trim())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prevAddress"></param>
        /// <returns></returns>
        private Address AddNewPreviousAddress(PreviousAddressContract prevAddress)
        {
            Address addressPervious = new Address();
            AddressExt addressExtPervious = null;
            Dictionary<String, Object> dicAddressData = GetPrevAddressDataDictionary(prevAddress);
            if (prevAddress.ZipCodeID == 0)
            {
                addressExtPervious = new AddressExt();
                addressExtPervious.AE_CountryID = prevAddress.CountryId;
                addressExtPervious.AE_StateName = prevAddress.StateName;
                addressExtPervious.AE_CityName = prevAddress.CityName;
                addressExtPervious.AE_ZipCode = prevAddress.Zipcode;
            }
            Guid addressHandleId = Guid.NewGuid();
            SecurityManager.AddAddressHandle(addressHandleId);
            SecurityManager.AddAddress(dicAddressData, addressHandleId, View.CurrentLoggedInUserId, addressPervious, addressExtPervious);
            SecurityManager.UpdateChanges();
            ClientSecurityManager.AddAddressHandle(View.SelectedTenantId, addressHandleId);
            ClientSecurityManager.AddAddress(View.SelectedTenantId, dicAddressData, addressHandleId, View.CurrentLoggedInUserId, addressPervious, addressExtPervious);
            return addressPervious;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, Object> GetAddressDataDictionary()
        {
            Dictionary<String, Object> dicAddressData = new Dictionary<String, Object>();
            dicAddressData.Add("address1", View.Address1);
            dicAddressData.Add("address2", View.Address2);
            dicAddressData.Add("zipcodeid", View.ZipId);
            return dicAddressData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prevAddress"></param>
        /// <returns></returns>
        public Dictionary<String, Object> GetPrevAddressDataDictionary(PreviousAddressContract prevAddress)
        {
            Dictionary<String, Object> dicAddressData = new Dictionary<String, Object>();
            dicAddressData.Add("address1", prevAddress.Address1);
            dicAddressData.Add("address2", prevAddress.Address2);
            dicAddressData.Add("zipcodeid", prevAddress.ZipCodeID);
            return dicAddressData;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SaveCustomFormDetails()
        {
            //Entity.ClientEntity.Order order = BackgroundProcessOrderManager.GetAdminOrderDataByOrderId(View.OrderID, View.SelectedTenantId);
            //List<Int32> lstBkgOrderId = new List<Int32>();
            //order.BkgOrders.ForEach(x =>
            //{
            //    x.BkgAdminOrderDetails.Where(cond => !cond.BAOD_IsDeleted).ForEach(y =>
            //    {
            //        lstBkgOrderId.Add(y.BAOD_BkgOrderID);
            //    });
            //});
            //foreach (var bkgOrderId in lstBkgOrderId.Distinct())
            //{
            BackgroundProcessOrderManager.SaveCustomFormDetails(View.lstBkgOrderData, View.BkgOrderId, View.SelectedTenantId, View.CurrentLoggedInUserId);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        private List<PreviousAddressContract> GetResidentialHistories(Guid UserId)
        {
            List<ResidentialHistory> tempResidentialAddress = SecurityManager.GetUserResidentialHistoriesForAdminOrder(UserId).ToList();
            List<PreviousAddressContract> tempList = tempResidentialAddress.Where(cond => cond.Address.ZipCodeID > 0).Select(x => new PreviousAddressContract
            {
                ID = x.RHI_ID,
                Address1 = x.Address.Address1,
                Address2 = x.Address.Address2,
                ResidenceStartDate = x.RHI_ResidenceStartDate,
                ResidenceEndDate = x.RHI_ResidenceEndDate,
                isNew = false,
                isDeleted = false,
                isUpdated = false,
                isCurrent = x.RHI_IsCurrentAddress.IsNullOrEmpty() ? false : x.RHI_IsCurrentAddress.Value,
                ZipCodeID = x.Address.ZipCodeID,
                CityName = x.Address.ZipCode.City.CityName,
                StateName = x.Address.ZipCode.County.State.StateName,
                Country = x.Address.ZipCode.County.State.Country.FullName,
                CountryId = x.Address.ZipCode.County.State.CountryID.Value,
                Zipcode = x.Address.ZipCode.ZipCode1,
                ResHistorySeqOrdID = x.RHI_SequenceOrder.IsNotNull() ? x.RHI_SequenceOrder.Value : AppConsts.NONE
            }).ToList();

            tempList.AddRange(tempResidentialAddress.Where(cond => cond.Address.ZipCodeID == 0).Select(x => new PreviousAddressContract
            {
                ID = x.RHI_ID,
                Address1 = x.Address.Address1,
                Address2 = x.Address.Address2,
                ZipCodeID = x.Address.ZipCodeID,
                ResidenceStartDate = x.RHI_ResidenceStartDate,
                ResidenceEndDate = x.RHI_ResidenceEndDate,
                isCurrent = x.RHI_IsCurrentAddress.IsNullOrEmpty() ? false : x.RHI_IsCurrentAddress.Value,
                isNew = false,
                isDeleted = false,
                isUpdated = false,
                CityName = x.Address.AddressExts.FirstOrDefault().AE_CityName,
                StateName = x.Address.AddressExts.FirstOrDefault().AE_StateName,
                Country = x.Address.AddressExts.FirstOrDefault().Country.FullName,
                Zipcode = x.Address.AddressExts.FirstOrDefault().AE_ZipCode,
                CountryId = x.Address.AddressExts.FirstOrDefault().Country.CountryID,
                ResHistorySeqOrdID = x.RHI_SequenceOrder.IsNotNull() ? x.RHI_SequenceOrder.Value : AppConsts.NONE
            }).ToList());
            return tempList;
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetAttributeFieldsOfSelectedPackages()
        {
            List<Int32> lstSelectedPackageIds = View.lstBkgPackage.Where(cond => View.lstSelectedPackageIds.Contains(cond.BPHMId)).Select(sel => sel.BPAId).ToList();
            if (!lstSelectedPackageIds.IsNullOrEmpty())
            {
                String packageIds = String.Join(",", lstSelectedPackageIds);
                List<Entity.ClientEntity.AttributeFieldsOfSelectedPackages> lstAttributeFields = BackgroundProcessOrderManager.GetAttributeFieldsOfSelectedPackages(packageIds, View.SelectedTenantId);
                if (!lstAttributeFields.IsNullOrEmpty())
                {
                    View.lstMvrAttGrp = lstAttributeFields.Where(cond => (cond.BSA_Code.ToUpper().Equals("1ADA97AE-9100-4BE6-B829-C914B7FA8750")
                                                                            || cond.BSA_Code.ToUpper().Equals("515BEF57-9072-4D2A-A97A-0C248BB045F9"))
                                                                           && cond.AttributeGrpCode.ToUpper().Equals("CF76960D-2120-46FE-9E03-01C218F8A336")).ToList();
                    View.LstInternationCriminalSrchAttributes = lstAttributeFields.Where(cond => (cond.BSA_Code.ToUpper().Equals("3DA8912A-6337-4B8F-93C4-88BFC3032D2D")
                                                                            || cond.BSA_Code.ToUpper().Equals("AAB51E52-2A9B-42AB-9A9D-D1AFFC18E211")
                                                                            || cond.BSA_Code.ToUpper().Equals("515BEF57-9072-4D2A-A97A-0C248BB045F9"))).ToList();
                }
                else
                {
                    View.lstMvrAttGrp = new List<Entity.ClientEntity.AttributeFieldsOfSelectedPackages>();
                }
            }
            else
            {
                View.lstMvrAttGrp = new List<Entity.ClientEntity.AttributeFieldsOfSelectedPackages>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetAllStates()
        {
            List<Entity.State> lstStateTemp = BackgroundSetupManager.GetAllStates().Where(x => !x.StateAbbreviation.IsNullOrEmpty()).ToList();
            View.ListStates = lstStateTemp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Int32 GetCustomFormIDBYCode()
        {
            return BackgroundProcessOrderManager.GetCustomFormIDBYCode(BkgCustomForm.Personal_and_residential_Information.GetStringValue());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean TransmmitAdminOrders()
        {
            List<Int32> OrderIDs = new List<Int32>();
            OrderIDs.Add(View.OrderID);
            if (BackgroundProcessOrderManager.TransmmitAdminOrders(View.SelectedTenantId, View.CurrentLoggedInUserId, OrderIDs))
            {
                UpdateEDSStatus();
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean AdminOrderIsReadyToTransmit()
        {
            return BackgroundProcessOrderManager.AdminOrderIsReadyToTransmit(View.SelectedTenantId, View.CurrentLoggedInUserId, Convert.ToString(View.OrderID));
        }

        /// <summary>
        /// Method to update the EDS Related Data For customformdata and also update the external vendor dispatch status.
        /// </summary>
        /// <param name="applicantOrderDataContract">applicantOrderDataContract</param>
        /// <param name="orderId">orderId</param>
        /// <param name="userOrder">userOrder</param>
        public void UpdateEDSStatus()
        {
            if (!View.OrderID.IsNullOrEmpty())
            {
                Entity.ClientEntity.Order order = new Entity.ClientEntity.Order();
                order = ComplianceDataManager.GetOrderById(View.SelectedTenantId, View.OrderID);
                if (!order.IsNullOrEmpty())
                {
                    Entity.ClientEntity.OrderPaymentDetail _orderPaymentDetailEDS = null;
                    Entity.ClientEntity.OrderPaymentDetail orderPaymentDetail = order.OrderPaymentDetails.FirstOrDefault(x => !x.OPD_IsDeleted);


                    if (!orderPaymentDetail.IsNullOrEmpty() && ComplianceDataManager.IsOrderPaymentIncludeEDSService(View.SelectedTenantId, orderPaymentDetail.OPD_ID))
                    {
                        _orderPaymentDetailEDS = orderPaymentDetail;
                    }


                    if (View.OrderID > 0 && !_orderPaymentDetailEDS.IsNullOrEmpty())
                    {
                        String _prevStatus = ApplicantOrderStatus.Paid.GetStringValue();
                        Int32 orderStatusId = ComplianceDataManager.GetOrderStatusList(View.SelectedTenantId).Where(orderSts => orderSts.Code.ToLower() == _prevStatus.ToLower() && !orderSts.IsDeleted)
                                     .FirstOrDefault().OrderStatusID;
                        #region E-DRUG SCREENING
                        Entity.ClientEntity.BkgOrder bkgOrderObj = BackgroundProcessOrderManager.GetBkgOrderByOrderID(View.SelectedTenantId, View.OrderID);
                        if (!bkgOrderObj.IsNullOrEmpty())
                        {
                            List<Int32> lstBackgroundPackageId = bkgOrderObj.BkgOrderPackages.Where(x => !x.BOP_IsDeleted).Select(x => x.BkgPackageHierarchyMapping.BPHM_BackgroundPackageID).ToList();
                            String extVendorId = String.Empty;
                            ClearStarCCF objClearstarCCf = new ClearStarCCF();

                            ClearStarWebCCFContract clearStarWebCCFContract = new ClearStarWebCCFContract();
                            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                            String result = BackgroundProcessOrderManager.GetClearStarServiceId(View.SelectedTenantId, lstBackgroundPackageId, BkgServiceType.ELECTRONICDRUGSCREEN.GetStringValue());
                            if (!result.IsNullOrEmpty())
                            {
                                String[] separator = { "," };
                                String[] splitIds = result.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                extVendorId = splitIds[1];
                            }

                            //Update BkgOrderSvcLineItem Status to DisptachOnHold_WaitingForEDSData for background package that contains EDS service.
                            if (!extVendorId.IsNullOrEmpty())
                            {
                                BackgroundProcessOrderManager.UpdateBkgOrderSvcLineItem(View.SelectedTenantId, Convert.ToInt32(extVendorId), bkgOrderObj.BOR_ID, SvcLineItemDispatchStatus.DISPTACH_ON_HOLD_WAITING_FOR_EDS_DATA.GetStringValue(), order.CreatedByID);
                            }
                            //Update status PSLI_DispatchedExternalVendor from DisptachOnHold_WaitingForEDSData to Dispatched
                            if (_orderPaymentDetailEDS.IsNotNull() && _orderPaymentDetailEDS.OPD_OrderStatusID.IsNotNull() && _orderPaymentDetailEDS.OPD_OrderStatusID == orderStatusId && !extVendorId.IsNullOrEmpty())
                            {
                                //Create dictionary for parallel task parameter.
                                Dictionary<String, Object> dicParam = new Dictionary<String, Object>();
                                dicParam.Add("BkgOrderId", bkgOrderObj.BOR_ID);
                                dicParam.Add("TenantId", View.SelectedTenantId);
                                dicParam.Add("ExtVendorId", Convert.ToInt32(extVendorId));
                                dicParam.Add("BPHMId_List", bkgOrderObj.BkgOrderPackages.Where(x => !x.BOP_IsDeleted).Select(x => x.BOP_BkgPackageHierarchyMappingID).ToList());
                                dicParam.Add("RegistrationId", String.Empty);
                                dicParam.Add("CurrentLoggedInUserId", order.CreatedByID);
                                dicParam.Add("OrganizationUserId", bkgOrderObj.OrganizationUserProfile.OrganizationUserID);
                                dicParam.Add("OrganizationUserProfileId", bkgOrderObj.BOR_OrganizationUserProfileID);
                                dicParam.Add("ApplicantName", string.Concat(bkgOrderObj.OrganizationUserProfile.FirstName, " ", bkgOrderObj.OrganizationUserProfile.LastName));
                                dicParam.Add("PrimaryEmailAddress", bkgOrderObj.OrganizationUserProfile.PrimaryEmailAddress);
                                //Pass selectedNodeId in place of HierarchyId [UAT-1067]
                                //dicParam.Add("HierarchyNodeId", bkgOrderObj.Order.HierarchyNodeID.Value);
                                dicParam.Add("HierarchyNodeId", bkgOrderObj.Order.SelectedNodeID.Value);
                                BackgroundProcessOrderManager.RunParallelTaskSaveCCFDataAndPDF(objClearstarCCf.SaveCCFDataAndPDF, dicParam, LoggerService, ExceptiomService, View.SelectedTenantId);
                            }
                        }
                        #endregion
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean IsOnlyMVRPackage()
        {
            List<Int32> lstSelectedPackageIds = View.lstBkgPackage.Where(cond => View.lstSelectedPackageIds.Contains(cond.BPHMId)).Select(sel => sel.BPAId).ToList();
            if (!lstSelectedPackageIds.IsNullOrEmpty())
            {
                String SelectedPackageIds = String.Join(",", lstSelectedPackageIds);
                var lstCustomForm = BackgroundProcessOrderManager.GetCustomFormsForThePackage(View.SelectedTenantId, SelectedPackageIds);
                if (!lstCustomForm.IsNullOrEmpty() && lstCustomForm.Count > AppConsts.NONE)
                {
                    Int32 MVRCustomFormId = GetCustomFormIDBYCode() > 0 ? GetCustomFormIDBYCode() : AppConsts.NONE;
                    if (lstCustomForm.Where(cond => cond.customFormId == MVRCustomFormId).Any())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="backgroundPackagesID"></param>
        /// <returns></returns>
        public List<PackageGroupContract> CheckShowResidentialHistory(Int32 tenantId, List<Int32> backgroundPackagesID)
        {
            Guid resHistory_ServiceAttributeGroup = new Guid(ServiceAttributeGroup.RESIDENTIAL_HISTORY.GetStringValue());
            Guid persInformation_SvcAttributeGroup = new Guid(ServiceAttributeGroup.PERSONAL_INFORMATION.GetStringValue());
            List<PackageGroupContract> tempList = BackgroundProcessOrderManager.CheckShowResidentialHistory(tenantId, backgroundPackagesID);
            //View.IsPersonalInformationGroupExist = tempList.Any(fx => fx.Code.Equals(persInformation_SvcAttributeGroup));
            return tempList.Where(yx => yx.Code.Equals(resHistory_ServiceAttributeGroup)).DistinctBy(y => y.PackageId).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="backgroundPackagesID"></param>
        public void GetMinMaxResidentailHistoryOccurances(Int32 tenantId, List<Int32> backgroundPackagesID)
        {
            Dictionary<String, Int32?> minMaxResidentailHistoryOccurances = BackgroundProcessOrderManager.GetMinMaxOccurancesForAttributeGroup(tenantId, backgroundPackagesID, new Guid(ServiceAttributeGroup.RESIDENTIAL_HISTORY.GetStringValue()));

            if (minMaxResidentailHistoryOccurances.ContainsKey("MaxOccurrence") && minMaxResidentailHistoryOccurances.ContainsKey("MinOccurrence"))
            {
                if (!minMaxResidentailHistoryOccurances["MaxOccurrence"].IsNullOrEmpty())
                {
                    View.MaxResidentailHistoryOccurances = Convert.ToInt32(minMaxResidentailHistoryOccurances["MaxOccurrence"]);
                }
                View.MinResidentailHistoryOccurances = Convert.ToInt32(minMaxResidentailHistoryOccurances["MinOccurrence"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="backgroundPackagesID"></param>
        public void GetMinMaxPaersonalAliasOccurances(Int32 tenantId, List<Int32> backgroundPackagesID)
        {
            Dictionary<String, Int32?> minMaxPaersonalAliasOccurances = BackgroundProcessOrderManager.GetMinMaxOccurancesForAttributeGroup(tenantId, backgroundPackagesID, new Guid(ServiceAttributeGroup.PERSONAL_ALIAS.GetStringValue()));
            if (minMaxPaersonalAliasOccurances.ContainsKey("MaxOccurrence") && minMaxPaersonalAliasOccurances.ContainsKey("MinOccurrence"))
            {
                //View.MaxPersonalAliasOccurances = Convert.ToInt32(minMaxPaersonalAliasOccurances["MaxOccurrence"]);
                //Added a check for nullable value UAT-605
                if (!minMaxPaersonalAliasOccurances["MaxOccurrence"].IsNullOrEmpty())
                {
                    View.MaxPersonalAliasOccurances = Convert.ToInt32(minMaxPaersonalAliasOccurances["MaxOccurrence"]);
                }
                View.MinPersonalAliasOccurances = Convert.ToInt32(minMaxPaersonalAliasOccurances["MinOccurrence"]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String CheckOrderAvailabilityForTrasmit()
        {
            if (View.OrderID > AppConsts.NONE)
            {
                Dictionary<String, Boolean> result = BackgroundProcessOrderManager.CheckOrderAvailabilityForTrasmit(View.OrderID.ToString(), View.SelectedTenantId);
                if (!result.IsNullOrEmpty() && !result.FirstOrDefault().IsNullOrEmpty() && !result.FirstOrDefault().Value)
                {
                    return "This Order cannot be transmitted or modified because added package(s) is no longer available";
                }
            }
            return String.Empty;
        }

        #region Set Default Subscription

        /// <summary>
        /// 
        /// </summary>
        Int32 notificationCommunicationTypeId = 0;
        private Int32 NotificationCommunicationTypeId
        {
            get
            {
                if (notificationCommunicationTypeId == 0)
                    notificationCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.NOTIFICATION.GetStringValue()).CommunicationTypeID;
                return notificationCommunicationTypeId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        Int32 alertCommunicationTypeId = 0;
        private Int32 AlertCommunicationTypeId
        {
            get
            {
                if (alertCommunicationTypeId == 0)
                    alertCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.ALERTS.GetStringValue()).CommunicationTypeID;
                return alertCommunicationTypeId;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        Int32 reminderCommunicationTypeId = 0;
        private Int32 ReminderCommunicationTypeId
        {
            get
            {
                if (reminderCommunicationTypeId == 0)
                    reminderCommunicationTypeId = MessageManager.GetCommuncationTypeByCode(lkpCommunicationTypeContext.REMINDERS.GetStringValue()).CommunicationTypeID;
                return reminderCommunicationTypeId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        private void SetDefaultSubscription(Int32 organizationUserId)
        {

            List<UserCommunicationSubscriptionSetting> userCommunicationSubscriptionSettings = new List<UserCommunicationSubscriptionSetting>();
            List<UserCommunicationSubscriptionSetting> mappedSubscriptionSettings = null;
            IEnumerable<lkpCommunicationEvent> communicationEvents = null;

            communicationEvents = CommunicationManager.GetCommunicationEvents(AlertCommunicationTypeId);
            mappedSubscriptionSettings = GetMappedUserCommunicationSubscriptionSettings(organizationUserId, View.CurrentLoggedInUserId, AlertCommunicationTypeId, communicationEvents);
            if (mappedSubscriptionSettings != null && mappedSubscriptionSettings.Count > 0)
                userCommunicationSubscriptionSettings.AddRange(mappedSubscriptionSettings);

            communicationEvents = CommunicationManager.GetCommunicationEvents(NotificationCommunicationTypeId);
            mappedSubscriptionSettings = GetMappedUserCommunicationSubscriptionSettings(organizationUserId, View.CurrentLoggedInUserId, NotificationCommunicationTypeId, communicationEvents);
            if (mappedSubscriptionSettings != null && mappedSubscriptionSettings.Count > 0)
                userCommunicationSubscriptionSettings.AddRange(mappedSubscriptionSettings);

            communicationEvents = CommunicationManager.GetCommunicationEvents(ReminderCommunicationTypeId);
            mappedSubscriptionSettings = GetMappedUserCommunicationSubscriptionSettings(organizationUserId, View.CurrentLoggedInUserId, ReminderCommunicationTypeId, communicationEvents);
            if (mappedSubscriptionSettings != null && mappedSubscriptionSettings.Count > 0)
                userCommunicationSubscriptionSettings.AddRange(mappedSubscriptionSettings);

            if (userCommunicationSubscriptionSettings != null && userCommunicationSubscriptionSettings.Count > 0)
                CommunicationManager.AddUserCommunicationSubscriptionSettings(userCommunicationSubscriptionSettings);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="ById"></param>
        /// <param name="communicationTypeId"></param>
        /// <param name="communicationEvents"></param>
        /// <returns></returns>
        private List<UserCommunicationSubscriptionSetting> GetMappedUserCommunicationSubscriptionSettings(
            Int32 organizationUserId,
            Int32 ById,
            Int32 communicationTypeId,
            IEnumerable<lkpCommunicationEvent> communicationEvents)
        {
            List<UserCommunicationSubscriptionSetting> userCommunicationSubscriptionSettings = null;
            if (communicationEvents != null && communicationEvents.Count() > 0)
            {
                userCommunicationSubscriptionSettings = new List<UserCommunicationSubscriptionSetting>();
                foreach (lkpCommunicationEvent communicationEvent in communicationEvents)
                {
                    userCommunicationSubscriptionSettings.Add(new UserCommunicationSubscriptionSetting()
                    {
                        OrganizationUserID = organizationUserId,
                        CommunicationTypeID = communicationTypeId,
                        CommunicationEventID = communicationEvent.CommunicationEventID,
                        IsSubscribedToAdmin = true,
                        IsSubscribedToUser = true,
                        CreatedByID = ById,
                        CreatedOn = DateTime.Now,
                        ModifiedByID = ById,
                        ModifiedOn = DateTime.Now
                    });
                }
            }
            return userCommunicationSubscriptionSettings;
        }
        #endregion

    }
}
