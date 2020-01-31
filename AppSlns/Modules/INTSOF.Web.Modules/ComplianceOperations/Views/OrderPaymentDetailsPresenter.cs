using System;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using System.Linq;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using ExternalVendors.ClearStarVendor;
using INTSOF.UI.Contract;
using System.Web;
using INTSOF.Contracts;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.ServiceUtil;
using System.Data;
using System.Text;
using System.Collections.Specialized;
using INTSOF.UI.Contract.ComplianceRuleEngine;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.FingerPrintSetup;

namespace CoreWeb.ComplianceOperations.Views
{
    public class OrderPaymentDetailsPresenter : Presenter<IOrderPaymentDetailsView>
    {
        public override void OnViewLoaded()
        {

        }

        public override void OnViewInitialized()
        {
            GetGranularPermissionForDOBandSSN();//UAt-806
            GetOrderDetailsAndSetControls();
            //UAT-1245
            IsCompPkgCancelDueToChangeSubs();
        }

        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        //UAT-4490
        public Entity.OrganizationUser GetOrganizationUserByUserID(int userId, Int32 tenantId)
        {
            return SecurityManager.GetOrganizationUser(userId);
        }

        /// <summary>
        /// Fetches the details for the order Id and sets the corresponding values in th controls present on the page.
        /// </summary>


        public List<OrderDetailContract> OrderSerivceDetail(int OrderId)
        {
            return FingerPrintDataManager.OrderSerivceDetail(View.SelectedTenantId, OrderId, View.CurrentLoggedInUserId);

        }
        public string GetBkgOrderServiceDetails(int orderID)
        {
            return BackgroundProcessOrderManager.GetBkgOrderServiceDetails(View.SelectedTenantId, orderID);
        }
        public string GetPackageNameForCompleteOrder(Int32 orderId, string serviceType,bool isIdRequired)
        {
            return FingerPrintDataManager.GetPackageNameForCompleteOrder(View.SelectedTenantId, orderId, serviceType, isIdRequired);
        }
        public string GetShippingLineItemName(string serviceType)
        {
            return FingerPrintDataManager.GetShippingLineItemName(View.SelectedTenantId, serviceType);
        }
        public void GetOrderDetailsAndSetControls()
                                {
            Entity.ZipCode zipcode = null;
            DateTime expirydate = DateTime.Now;
            Int32 durationMonths = AppConsts.NONE;

            // UAT 1067 - Not required as DPMId should be the SelectedNodeId now
            //Int32 dpmdId = AppConsts.NONE;
            //UAT-916
            // OrderPaymentDetail orderPaymentDetail = null;
            List<OrderPaymentDetail> orderPaymentDetailList = null;
            Order orderFromOrderPaymentDetail = null;
            String compOrderPkgTypeCode = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();
            OrderPaymentDetail orderPaymentDetailForCompPkg = null;
            OrderPkgPaymentDetail tempOrderPkgPaymentDetail = null;
            String paymOptInvcWithApprovalCode = PaymentOptions.InvoiceWithApproval.GetStringValue();
            String paymOptInvcWithOutApprovalCode = PaymentOptions.InvoiceWithOutApproval.GetStringValue();
            //UAT-1790 related changes.
            //View.TextOrderId = View.OrderId.ToString();
            View.TextOrderId = View.OrderNumber.ToString();

            //UAT-3335
            View.Gender = null;
            View.State = null;
            View.City = null;
            View.Zip = null;
            View.DateOfBirth = null;
            View.Address1 = null;
            View.Address2 = null;
            //End
            //UAT916
            //orderPaymentDetail = ComplianceDataManager.GetOrderDetailById(View.SelectedTenantId, View.OrderId);
            orderPaymentDetailList = ComplianceDataManager.GetAllPaymentDetailsOfOrderByOrderID(View.SelectedTenantId, View.OrderId);

            if (View.IsLocationServiceTenant && orderPaymentDetailList.Any()) //UAT-4272
            {
                if (orderPaymentDetailList.Any(opd => opd.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue()
                 && opd.lkpOrderStatu.Code == ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue()))
                {
                    View.TextOrderId = "";
                }
            }

            GetOrderPkgPaymentDetail();//Method to get OrderPkgPaymentDetail.
            tempOrderPkgPaymentDetail = View.OrderPkgPaymentDetailList.FirstOrDefault(x => x.lkpOrderPackageType.OPT_Code == compOrderPkgTypeCode);
            if (tempOrderPkgPaymentDetail.IsNotNull())
            {
                orderPaymentDetailForCompPkg = View.OrderPkgPaymentDetailList.FirstOrDefault(x => x.lkpOrderPackageType.OPT_Code == compOrderPkgTypeCode).OrderPaymentDetail;
            }
            orderFromOrderPaymentDetail = orderPaymentDetailList.Select(x => x.Order).FirstOrDefault();

            if (!orderFromOrderPaymentDetail.PackageCancellationHistories.IsNullOrEmpty())
            {
                var user = SecurityManager.GetOrganizationUser(orderFromOrderPaymentDetail.PackageCancellationHistories.FirstOrDefault().PCH_CancelledByUserID);
                View.TrackingPkgCancelledBy = user.IsNullOrEmpty() ? null : (user.FirstName + " " + user.LastName);
            }

            //UAT916
            //View.BkgOrderID = orderPaymentDetail.Order.OrderID;
            View.BkgOrderID = orderFromOrderPaymentDetail.OrderID;

            //UAT-1683 : Implementation of Archive WRT Graduated and Un-Graduated Archive State
            if (!orderFromOrderPaymentDetail.IsNullOrEmpty() && !orderFromOrderPaymentDetail.BkgOrders.Where(x => !x.BOR_IsDeleted).IsNullOrEmpty())
            {
                var bkgOrder = orderFromOrderPaymentDetail.BkgOrders.First(cnd => !cnd.BOR_IsDeleted);
                View.BkgArchiveStateCode = bkgOrder.lkpArchiveState.IsNull() ? String.Empty : bkgOrder.lkpArchiveState.AS_Code;
            }
            //UAT916
            View.OrderPackageTypeCode = orderFromOrderPaymentDetail.lkpOrderPackageType.OPT_Code;
            ////View.PartialOrderCancellationTypeID = orderPaymentDetail.Order.PartialOrderCancellationTypeID;
            //UAT-916
            //View.PartialOrderCancellationTypeCode = orderPaymentDetail.Order.lkpPartialOrderCancellationType.IsNotNull()
            //                                        ? orderPaymentDetail.Order.lkpPartialOrderCancellationType.Code
            //                                        : String.Empty;
            View.PartialOrderCancellationTypeCode = orderFromOrderPaymentDetail.lkpPartialOrderCancellationType.IsNotNull()
                                                   ? orderFromOrderPaymentDetail.lkpPartialOrderCancellationType.Code
                                                 : String.Empty;

            //if (View.OrderPackageType == AppConsts.TWO)
            if (View.OrderPackageTypeCode == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue())
            {
                //UAT-916
                //View.InstituteHierarchy = orderPaymentDetail.Order.DeptProgramMapping.DPM_Label;

                //View.InstituteHierarchy = orderFromOrderPaymentDetail.DeptProgramMapping.DPM_Label;
                if (orderFromOrderPaymentDetail.DeptProgramMapping1.IsNotNull())
                    View.InstituteHierarchy = orderFromOrderPaymentDetail.DeptProgramMapping1.DPM_Label;
            }
            //UAT-916
            // if (orderPaymentDetail.IsNotNull())
            if (orderPaymentDetailList.IsNotNull())
            {
                //UAT-916
                //View.OrderPaymentDetail = orderPaymentDetail;
                View.OrderPaymentDetailList = orderPaymentDetailList;
                //Assign value to the read only controls
                //UAT-916
                // View.OrderDate = orderPaymentDetail.Order.OrderDate.HasValue ? Convert.ToDateTime(orderPaymentDetail.Order.OrderDate).ToShortDateString() : String.Empty;
                View.OrderDate = orderFromOrderPaymentDetail.OrderDate.HasValue ? Convert.ToDateTime(orderFromOrderPaymentDetail.OrderDate).ToShortDateString() : String.Empty;
                //UAT-916
                //if (orderPaymentDetail.Order.ApprovalDate.HasValue)
                if (orderPaymentDetailForCompPkg.IsNotNull() && orderPaymentDetailForCompPkg.OPD_ApprovalDate.HasValue)
                {
                    //UAT-916
                    //View.SubscriptionStartDate = orderPaymentDetail.Order.ApprovalDate.Value.ToShortDateString();
                    View.SubscriptionStartDate = orderPaymentDetailForCompPkg.OPD_ApprovalDate.Value.ToShortDateString();
                }
                //UAT-916
                //if (orderPaymentDetail.Order.IsNotNull() && orderPaymentDetail.Order.lkpPartialOrderCancellationType.IsNotNull()
                //       && orderPaymentDetail.Order.lkpPartialOrderCancellationType.Code != PartialOrderCancellationType.BACKGROUND_PACKAGE.GetStringValue())
                if (orderFromOrderPaymentDetail.IsNotNull() && orderFromOrderPaymentDetail.lkpPartialOrderCancellationType.IsNotNull()
                       && orderFromOrderPaymentDetail.lkpPartialOrderCancellationType.Code != PartialOrderCancellationType.BACKGROUND_PACKAGE.GetStringValue())
                {
                    View.IsCompliancePartialOrderCancelled = true;
                }
                else
                {
                    View.IsCompliancePartialOrderCancelled = false;
                }
                //UAT-916
                //var packageSubscription = orderPaymentDetail.Order.PackageSubscriptions.FirstOrDefault(x => x.IsDeleted == false);
                var packageSubscription = orderFromOrderPaymentDetail.PackageSubscriptions.FirstOrDefault(x => x.IsDeleted == false);

                //UAT-1558 As a Student, I should be able to mark when I have "Graduated" from a tracking and/or screening package's corresponding program
                if (packageSubscription.IsNotNull() && !packageSubscription.ArchiveStateID.IsNullOrEmpty())
                {
                    View.ArchiveStateCode = packageSubscription.lkpArchiveState.AS_Code;
                }
                else
                {
                    //UAT-2217
                    View.ArchiveStateCode = ArchiveState.Package_Subscription_Cancelled.GetStringValue();
                }

                if (packageSubscription.IsNotNull() && packageSubscription.ExpiryDate.HasValue)
                {
                    View.SubscriptionExpirationDate = packageSubscription.ExpiryDate.Value.ToShortDateString();
                }

                //if (orderPaymentDetail.Order.TotalPrice.HasValue)
                if (orderFromOrderPaymentDetail.TotalPrice.HasValue)
                {
                    //UAT-916
                    //View.TotalOrderValue = orderPaymentDetail.Order.TotalPrice.Value.ToString();
                    View.TotalOrderValue = orderFromOrderPaymentDetail.TotalPrice.Value.ToString();
                }
                //if (orderPaymentDetail.Order.DuePayment.HasValue)
                if (orderFromOrderPaymentDetail.DuePayment.HasValue)
                {
                    //UAT-916
                    //View.DuePayment = orderPaymentDetail.Order.DuePayment.Value.ToString();
                    View.DuePayment = orderFromOrderPaymentDetail.DuePayment.Value.ToString();
                }
                if (orderFromOrderPaymentDetail.RushOrderPrice.HasValue)
                {
                    View.RushOrderPrice = orderFromOrderPaymentDetail.RushOrderPrice.Value.ToString();
                }
                if (View.IsLocationServiceTenant)
                {
                    if (orderPaymentDetailList.IsNotNull())
                    {
                        View.GrandTotalPrice = orderPaymentDetailList.Sum(x => x.OPD_Amount).ToString();
                    }
                }
                else
                {
                    if (orderFromOrderPaymentDetail.GrandTotal.HasValue)
                    {
                        View.GrandTotalPrice = orderFromOrderPaymentDetail.GrandTotal.Value.ToString();
                    }
                }
                

                /* UAT 1067 - Not required as DPMId should be the SelectedNodeId now
                 * if (orderFromOrderPaymentDetail.SelectedNodeID.HasValue && orderFromOrderPaymentDetail.lkpOrderPackageType.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue())
                {
                    dpmdId = orderFromOrderPaymentDetail.SelectedNodeID.Value;
                    //Set DPM_ID If only Background packages UAT-916
                    View.DPM_ID = dpmdId;
                }
                else
                {
                    if (orderFromOrderPaymentDetail.HierarchyNodeID.HasValue)
                        dpmdId = orderFromOrderPaymentDetail.HierarchyNodeID.Value;
                }*/

                //UAT 832 : Package price is displaying in order history when invoice options were used for payment

                //if (dpmdId > AppConsts.NONE)
                //{
                //    //View.IsInvoiceOnly = ComplianceDataManager.CheckIsInvoiceOnly(View.SelectedTenantId, dpmdId);
                //}
                //10/03/2014 | Used LookupManager for lkp tables
                /*UAT-916
                //String orderStatusName = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderStatu>(View.TenantId)
                //                                    .FirstOrDefault(con => con.OrderStatusID == orderPaymentDetail.Order.OrderStatusID && con.IsDeleted == false).Name;

                //String orderStatusCode = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderStatu>(View.TenantId)
                //                                    .FirstOrDefault(con => con.OrderStatusID == orderPaymentDetail.Order.OrderStatusID && con.IsDeleted == false).Code;*/

                String orderRequestTypeCode = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderRequestType>(View.TenantId)
                                                .FirstOrDefault(con => con.ORT_ID == orderFromOrderPaymentDetail.OrderRequestTypeID).ORT_Code;

                /*UAT-916
                //String orderPaymentType = String.Empty;
                //String orderPaymentTypeCode = String.Empty;
                //if (!orderPaymentDetail.Order.PaymentOptionID.IsNullOrEmpty())
                //{
                //    orderPaymentType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpPaymentOption>(View.SelectedTenantId)
                //                                     .FirstOrDefault(con => con.PaymentOptionID == orderPaymentDetail.Order.PaymentOptionID && con.IsDeleted == false).Name;

                //    orderPaymentTypeCode = LookupManager.GetLookUpData<Entity.ClientEntity.lkpPaymentOption>(View.SelectedTenantId)
                //                                      .FirstOrDefault(con => con.PaymentOptionID == orderPaymentDetail.Order.PaymentOptionID && con.IsDeleted == false).Code;
                //}

                View.OrderStatus = orderStatusName; //orderPaymentDetail.Order.lkpOrderStatu.Name; //Used LookupManager for lkp tables*/
                if (orderFromOrderPaymentDetail.RushOrderStatusID.IsNotNull())
                    View.RushOrderStatus = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderStatu>(View.TenantId)
                                                    .FirstOrDefault(con => con.OrderStatusID == orderFromOrderPaymentDetail.RushOrderStatusID && con.IsDeleted == false).Name; //orderPaymentDetail.Order.lkpOrderStatu1.Name; //Used LookupManager for lkp tables

                #region UAT 1067 - Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on.

                //if (orderFromOrderPaymentDetail.DeptProgramPackage != null)
                //{
                //    View.InstituteHierarchy = orderFromOrderPaymentDetail.DeptProgramPackage.DeptProgramMapping.DPM_Label;
                //}
                //DPM_ID
                //if (orderFromOrderPaymentDetail.DeptProgramPackage != null)
                //{
                //    View.DPM_ID = orderFromOrderPaymentDetail.DeptProgramPackage.DeptProgramMapping.DPM_ID;
                //}

                if (orderFromOrderPaymentDetail.DeptProgramMapping1.IsNotNull())
                {
                    View.InstituteHierarchy = orderFromOrderPaymentDetail.DeptProgramMapping1.DPM_Label;
                    View.DPM_ID = orderFromOrderPaymentDetail.DeptProgramMapping1.DPM_ID;
                }
                # endregion

                //OrganizationUserProfile
                View.OrganizationUserProfile = orderFromOrderPaymentDetail.OrganizationUserProfile;

                //DPPSId
                //Removed y.DPPS_IsDeleted == false check so that user can view order if subscription is deleted
                if (orderFromOrderPaymentDetail.DeptProgramPackage != null)
                {
                    var Dpps_IDs = orderFromOrderPaymentDetail.DeptProgramPackage.DeptProgramPackageSubscriptions
                                    .FirstOrDefault(y => y.SubscriptionOption.Label.ToLower() == orderFromOrderPaymentDetail.SubscriptionLabel.ToLower());
                    if (Dpps_IDs.IsNotNull())
                        View.DPPSId = Dpps_IDs.DPPS_ID;
                }
                //View.DPPSId = orderPaymentDetail.Order.DeptProgramPackage.DeptProgramPackageSubscriptions.FirstOrDefault().DPPS_ID;
                // FirstName
                View.FirstName = orderFromOrderPaymentDetail.OrganizationUserProfile.FirstName;
                // MiddleName
                //View.MiddleName = orderFromOrderPaymentDetail.OrganizationUserProfile.MiddleName;
                //UAT-2212: Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                View.MiddleName = orderFromOrderPaymentDetail.OrganizationUserProfile.MiddleName.IsNullOrEmpty() ? View.NoMiddleNameText
                                                                                                                 : orderFromOrderPaymentDetail.OrganizationUserProfile.MiddleName;
                // LastName
                View.LastName = orderFromOrderPaymentDetail.OrganizationUserProfile.LastName;
                //CBI || CABS || Add Suffix in Last Name
                if (View.IsLocationServiceTenant)
                {
                    if (!View.lstSuffixes.IsNullOrEmpty())
                    {
                        View.LastName = (orderFromOrderPaymentDetail.OrganizationUserProfile.UserTypeID.IsNullOrEmpty() || !View.lstSuffixes.Any(cond => cond.SuffixID == orderFromOrderPaymentDetail.OrganizationUserProfile.UserTypeID)) ? orderFromOrderPaymentDetail.OrganizationUserProfile.LastName : orderFromOrderPaymentDetail.OrganizationUserProfile.LastName + " - " + View.lstSuffixes.Where(cond => cond.SuffixID == orderFromOrderPaymentDetail.OrganizationUserProfile.UserTypeID).FirstOrDefault().Suffix;
                    }
                }

                //Bind the Alias List
                if (orderFromOrderPaymentDetail.OrganizationUserProfile.OrganizationUser.PersonAlias.IsNotNull()
                    && orderFromOrderPaymentDetail.OrganizationUserProfile.OrganizationUser.PersonAlias.Where(x => !x.PA_IsDeleted).Any())
                {
                    View.PersonAliasList = orderFromOrderPaymentDetail.OrganizationUserProfile.OrganizationUser.PersonAlias.Where(x => !x.PA_IsDeleted).Select(cond => new PersonAliasContract
                    {
                        FirstName = cond.PA_FirstName,
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        MiddleName = cond.PA_MiddleName,
                        LastName = cond.PA_LastName,
                        ID = cond.PA_ID,
                        //CBI|| CABS ||
                        Suffix = View.IsLocationServiceTenant ? (!cond.PersonAliasExtensions.IsNullOrEmpty() && !cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).IsNullOrEmpty() && !cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).PAE_Suffix.IsNullOrEmpty() ? cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).PAE_Suffix : String.Empty) : String.Empty,
                        //SuffixID = View.IsLocationServiceTenant ? (!cond.PersonAliasExtensions.IsNullOrEmpty() && !cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).IsNullOrEmpty() && !cond.PersonAliasExtensions.FirstOrDefault(con => !con.PAE_IsDeleted).PAE_Suffix.IsNullOrEmpty() ? View.lstSuffixes.Where(con => con.Suffix == cond.PersonAliasExtensions.FirstOrDefault(x => !x.PAE_IsDeleted).PAE_Suffix).FirstOrDefault().SuffixID : (Int32?)null) : (Int32?)null,
                    }).ToList();
                }
                else
                    View.PersonAliasList = new List<PersonAliasContract>();

                // Alias1
                //View.Alias1 = orderPaymentDetail.Order.OrganizationUserProfile.Alias1;
                //// Alias2
                //View.Alias2 = orderPaymentDetail.Order.OrganizationUserProfile.Alias2;
                //// Alias3
                //View.Alias3 = orderPaymentDetail.Order.OrganizationUserProfile.Alias3;
                // Gender
                //View.Gender = orderFromOrderPaymentDetail.OrganizationUserProfile.lkpGender.GenderName;

                if (!orderFromOrderPaymentDetail.IsNullOrEmpty() && !orderFromOrderPaymentDetail.OrganizationUserProfile.IsNullOrEmpty() && !orderFromOrderPaymentDetail.OrganizationUserProfile.lkpGender.IsNullOrEmpty())
                {
                    if (!View.LanguageCode.IsNullOrEmpty() && View.LanguageCode == Languages.SPANISH.GetStringValue())
                    {
                        int _genderId = orderFromOrderPaymentDetail.OrganizationUserProfile.lkpGender.DefaultLanguageKeyID.Value;
                        List<Entity.ClientEntity.lkpGender> lstGender = ComplianceDataManager.GetGenderList(View.TenantId);
                        View.Gender = lstGender.Where(cond => cond.DefaultLanguageKeyID == _genderId && cond.lkpLanguage.LAN_Code == View.LanguageCode).FirstOrDefault().GenderName;
                    }
                    else
                    {
                        View.Gender = orderFromOrderPaymentDetail.OrganizationUserProfile.lkpGender.GenderName;
                    }
                }

                // DateOfBirth
                if (orderFromOrderPaymentDetail.OrganizationUserProfile.DOB.HasValue)
                    View.DateOfBirth = orderFromOrderPaymentDetail.OrganizationUserProfile.DOB.Value.ToShortDateString();



                // SocialSecurityNumber
                //UAT-806

                String unFormattedSSN = ComplianceSetupManager.GetFormattedString(orderFromOrderPaymentDetail.OrganizationUserProfile.OrganizationUserProfileID, true, View.SelectedTenantId);

                if (View.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue())
                {
                    //View.SocialSecurityNumberMaskedOnly = GetMaskedSSN(orderFromOrderPaymentDetail.OrganizationUserProfile.SSN);
                    View.SocialSecurityNumberMaskedOnly = GetMaskedSSN(unFormattedSSN);
                }
                //View.SocialSecurityNumber = orderFromOrderPaymentDetail.OrganizationUserProfile.SSN;
                View.SocialSecurityNumber = unFormattedSSN;
                // PrimaryEmail
                View.PrimaryEmail = orderFromOrderPaymentDetail.OrganizationUserProfile.PrimaryEmailAddress;
                // SecondaryEmail
                View.SecondaryEmail = orderFromOrderPaymentDetail.OrganizationUserProfile.SecondaryEmailAddress;
                // Phone 
                if (IsInstructorPreceptor(orderFromOrderPaymentDetail.OrganizationUserProfile.OrganizationUser.UserID)) // in UAT-3335
                {
                    Guid userId = orderFromOrderPaymentDetail.OrganizationUserProfile.OrganizationUser.UserID;
                    if (!userId.IsNullOrEmpty())
                    {
                        View.SharedUserDetails = ProfileSharingManager.GetSharedUserDashboardDetails(userId);
                        if (!View.SharedUserDetails.IsNullOrEmpty())
                            View.Phone = View.SharedUserDetails.SharedUserPhone;
                    }
                }
                else
                {
                    if (orderFromOrderPaymentDetail.OrganizationUserProfile.IsInternationalPhoneNumber) //UAT-2447
                        View.PhoneUnMasking = orderFromOrderPaymentDetail.OrganizationUserProfile.PhoneNumber;
                    else
                        View.Phone = orderFromOrderPaymentDetail.OrganizationUserProfile.PhoneNumber;
                }

                // SecondaryPhone
                if (orderFromOrderPaymentDetail.OrganizationUserProfile.IsInternationalSecondaryPhone)  //UAT-2447
                    View.SecondaryPhoneUnMasking = orderFromOrderPaymentDetail.OrganizationUserProfile.SecondaryPhone;
                else
                    View.SecondaryPhone = orderFromOrderPaymentDetail.OrganizationUserProfile.SecondaryPhone;

                View.OrganizationUserId = orderFromOrderPaymentDetail.OrganizationUserProfile.OrganizationUserID;

                View.UserId = orderFromOrderPaymentDetail.OrganizationUserProfile.OrganizationUser.UserID;

                //UAT-2893: Update country code sent to clearstar for international criminals
                Entity.ClientEntity.AddressExt addressExt = new Entity.ClientEntity.AddressExt();

                if (orderFromOrderPaymentDetail.OrganizationUserProfile.AddressHandle.IsNotNull())
                {
                    if (orderFromOrderPaymentDetail.OrganizationUserProfile.AddressHandle.Addresses.IsNotNull())
                    {
                        View.Address1 = orderFromOrderPaymentDetail.OrganizationUserProfile.AddressHandle.Addresses.FirstOrDefault().Address1;
                        View.Address2 = orderFromOrderPaymentDetail.OrganizationUserProfile.AddressHandle.Addresses.FirstOrDefault().Address2;
                        var zipId = orderFromOrderPaymentDetail.OrganizationUserProfile.AddressHandle.Addresses.FirstOrDefault().ZipCodeID;
                        zipcode = SecurityManager.GetZip(zipId.Value);
                        if (zipcode.ZipCodeID == 0)
                        {
                            addressExt = orderFromOrderPaymentDetail.OrganizationUserProfile.AddressHandle.Addresses.FirstOrDefault().AddressExts.FirstOrDefault();
                        }
                    }
                }
                if (!zipcode.IsNullOrEmpty())
                {
                    //UAT-2893: Update country code sent to clearstar for international criminals
                    if (zipcode.ZipCodeID == 0 && !addressExt.IsNullOrEmpty())
                    {
                        View.State = addressExt.AE_StateName;
                        View.City = addressExt.AE_CityName;
                        View.Zip = addressExt.AE_ZipCode;
                    }
                    else
                    {
                        View.City = zipcode.City.CityName; ;
                        View.State = zipcode.County.State.StateName;
                        View.Zip = zipcode.ZipCode1;
                    }
                }


                /* UAT-916
                 if (orderPaymentDetail.Order.GrandTotal != (Decimal)AppConsts.NONE)
                {
                    View.PaymentType = orderPaymentDetail.Order.lkpPaymentOption.IsNullOrEmpty() ? String.Empty : orderPaymentType; //orderPaymentDetail.Order.lkpPaymentOption.Name; //Used LookupManager for lkp tables
                }
                if (orderPaymentDetail.Order.lkpPaymentOption.IsNotNull())
                {
                    View.PaymentTypeCode = orderPaymentTypeCode; //orderPaymentDetail.Order.lkpPaymentOption.Code; //Used LookupManager for lkp tables
                }*/
                if (orderFromOrderPaymentDetail.DeptProgramPackage != null)
                {
                    var compliancePackage = orderFromOrderPaymentDetail.DeptProgramPackage.CompliancePackage;
                    View.PackageHeading = compliancePackage.lkpCompliancePackageType.CPT_Label.IsNullOrEmpty()
                                                    ? compliancePackage.lkpCompliancePackageType.CPT_Name
                                                    : compliancePackage.lkpCompliancePackageType.CPT_Label;
                    View.Package = String.IsNullOrEmpty(compliancePackage.PackageLabel)
                        ? orderFromOrderPaymentDetail.DeptProgramPackage.CompliancePackage.PackageName
                        : orderFromOrderPaymentDetail.DeptProgramPackage.CompliancePackage.PackageLabel;
                    //UAT-4490
                    int cancelledById = 0;
                    string cancelledBy = string.Empty;
                    DateTime cancelledOn;
                    OrderPkgPaymentDetail compOrderPkgPaymentDetail = View.OrderPkgPaymentDetailList.FirstOrDefault(x => x.lkpOrderPackageType.OPT_Code == compOrderPkgTypeCode);

                    List<PackageCancellationHistory> lstPCH = orderFromOrderPaymentDetail.PackageCancellationHistories.Where(x => x.PCH_OrderId == orderFromOrderPaymentDetail.OrderID && !x.PCH_IsDeleted
                            && x.PCH_PkgID == orderFromOrderPaymentDetail.DeptProgramPackage.CompliancePackage.CompliancePackageID &&
                            x.PCH_OPPDID == compOrderPkgPaymentDetail.OPPD_ID).ToList();

                    if (orderFromOrderPaymentDetail.PackageCancellationHistories.Count() > 0 && lstPCH.Count > 0)
                    {
                        cancelledOn = orderFromOrderPaymentDetail.PackageCancellationHistories.Where(x => x.PCH_OrderId == orderFromOrderPaymentDetail.OrderID && !x.PCH_IsDeleted
                            && x.PCH_PkgID == orderFromOrderPaymentDetail.DeptProgramPackage.CompliancePackage.CompliancePackageID).FirstOrDefault().PCH_CreatedOn;
                        cancelledById = orderFromOrderPaymentDetail.PackageCancellationHistories.Where(x => x.PCH_OrderId == orderFromOrderPaymentDetail.OrderID && !x.PCH_IsDeleted
                            && x.PCH_PkgID == orderFromOrderPaymentDetail.DeptProgramPackage.CompliancePackage.CompliancePackageID).FirstOrDefault().PCH_CancelledByUserID;
                        Entity.OrganizationUser objOrgUser = GetOrganizationUserByUserID(cancelledById, View.TenantId);
                        cancelledBy = objOrgUser.FirstName + " " + objOrgUser.LastName;
                        View.CompliancePkgCancelledBy = cancelledBy;
                        View.CompliancePkgCancelledOn = (cancelledOn).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        View.CompliancePkgCancelledBy = string.Empty;
                        View.CompliancePkgCancelledOn = string.Empty;
                    }
                    //UAT-4490
                }

                if (orderFromOrderPaymentDetail.DeptProgramPackage != null)
                {
                    View.PackageId = orderFromOrderPaymentDetail.DeptProgramPackage.DPP_CompliancePackageID;
                }

                if (orderFromOrderPaymentDetail.SubscriptionYear.HasValue)
                {
                    expirydate = expirydate.AddYears(orderFromOrderPaymentDetail.SubscriptionYear.Value);
                    durationMonths = orderFromOrderPaymentDetail.SubscriptionYear.Value * 12;
                }
                if (orderFromOrderPaymentDetail.SubscriptionMonth.HasValue)
                {
                    expirydate = expirydate.AddMonths(orderFromOrderPaymentDetail.SubscriptionMonth.Value);
                    durationMonths += orderFromOrderPaymentDetail.SubscriptionMonth.Value;
                }
                View.ExpiryDate = expirydate;
                View.DurationMonths = durationMonths.ToString();
                /* UAT-916View.OrderStatusCode = orderStatusCode;*/
                //orderPaymentDetail.Order.lkpOrderStatu.Code; //Used LookupManager for lkp tables
                /* UAT-916
                if (View.OrderStatusCode.Equals(ApplicantOrderStatus.Paid.GetStringValue()) &&
                    (orderPaymentDetail.Order.DuePayment.IsNull() || (orderPaymentDetail.Order.DuePayment.HasValue && orderPaymentDetail.Order.DuePayment.Value == 0)) &&
                   ( //orderPaymentDetail.Order.lkpOrderRequestType.IsNotNull() && orderPaymentDetail.Order.lkpOrderRequestType.ORT_Code //Used LookupManager for lkp tables
                    orderRequestTypeCode.Equals(OrderRequestType.ChangeSubscriptionByAdmin.GetStringValue())))
                {
                    View.ReferenceNumber = "Subscription changed by Admin- Previous Subscription Id: " + orderPaymentDetail.Order.PreviousSubscriptionID + "";
                }
                else
                {
                    View.ReferenceNumber = orderPaymentDetail.OPD_ReferenceNo;
                }
                if (View.OrderStatusCode.Equals(ApplicantOrderStatus.Cancellation_Requested.GetStringValue()))
                    View.ShowApproveCancellation = true;
                else if (View.OrderStatusCode.Equals(ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue()))
                    View.ShowApprovePayment = true;*/

                if (View.OrderPaymentDetailList.IsNotNull() && View.OrderPaymentDetailList.Count > AppConsts.NONE && View.OrderPaymentDetailList.Any(cnd => cnd.lkpOrderStatu != null && cnd.lkpOrderStatu.Code == ApplicantOrderStatus.Cancellation_Requested.GetStringValue()))
                {
                    View.ShowApproveCancellation = true;
                }

                //else if (View.OrderStatusCode.Equals(ApplicantOrderStatus.Paid.GetStringValue())
                //    || View.OrderStatusCode.Equals(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue())
                //    || View.OrderStatusCode.Equals(ApplicantOrderStatus.Payment_Rejected.GetStringValue())
                //    || View.OrderStatusCode.Equals(ApplicantOrderStatus.Payment_Due.GetStringValue())
                //    )
                //{
                View.ShowOfflineSettlement = true;
                // }
                /* UAT-916
                if (View.PaymentTypeCode.Equals(PaymentOptions.InvoiceWithApproval.GetStringValue()) || View.PaymentTypeCode.Equals(PaymentOptions.InvoiceWithOutApproval.GetStringValue()))
                {
                    View.IsInvoiceOnly = true;
                }
                else
                {
                    View.IsInvoiceOnly = false;
                }*/
                if (orderPaymentDetailList.IsNotNull() && orderPaymentDetailList.Any(cond => cond.lkpPaymentOption != null && (cond.lkpPaymentOption.Code == paymOptInvcWithApprovalCode || cond.lkpPaymentOption.Code == paymOptInvcWithOutApprovalCode)))
                //if (orderPaymentDetailList.IsNotNull() && orderPaymentDetailList.Any(cond => cond.lkpPaymentOption != null && (cond.lkpPaymentOption.Code == paymOptInvcWithOutApprovalCode)))
                {
                    View.IsInvoiceOnly = true;
                }
                else
                {
                    View.IsInvoiceOnly = false;
                }

                /* UAT-916
                if (orderStatusCode == ApplicantOrderStatus.Cancellation_Requested.GetStringValue() ||
                    orderStatusCode == ApplicantOrderStatus.Cancelled.GetStringValue())
                {
                    OnlinePaymentTransaction _paymentTransaction = ComplianceDataManager.GetSuccessfullOrderPaymentDetails(View.OrderId, View.SelectedTenantId);
                    View.TransactionId = _paymentTransaction.Trans_id;
                    View.CCNumber = _paymentTransaction.CCNumber;
                    View.InvoiceNumber = _paymentTransaction.Invoice_num;
                }
                else if (!String.IsNullOrEmpty(View.PartialOrderCancellationTypeCode))
                {
                    OnlinePaymentTransaction _paymentTransaction = ComplianceDataManager.GetSuccessfullOrderPaymentDetails(View.OrderId, View.SelectedTenantId);
                    View.TransactionId = _paymentTransaction.Trans_id;
                    View.CCNumber = _paymentTransaction.CCNumber;
                    View.InvoiceNumber = _paymentTransaction.Invoice_num;
                }*/

            }

            View.AutomaticRenewalTurnedOff = orderFromOrderPaymentDetail.AutomaticRenewalTurnedOff.IsNull() || orderFromOrderPaymentDetail.AutomaticRenewalTurnedOff == false ? false : true;

            #region UAT-796

            //case when ps.PackageSubscriptionID is not null and ps.IsDeleted=0 and  (isnull(LSMS.Code,'') <>'AAAA') then 1 else 0 end
            Boolean hasActiveComplianceSubscription = false;

            if (orderFromOrderPaymentDetail.PackageSubscriptions.IsNotNull())
            {
                var subscription = orderFromOrderPaymentDetail.PackageSubscriptions.FirstOrDefault();
                if (subscription != null && !subscription.IsDeleted
                  && !(subscription.lkpSubscriptionMobilityStatu.IsNotNull() && subscription.lkpSubscriptionMobilityStatu.Code.Equals("AAAA")))
                {
                    hasActiveComplianceSubscription = true;
                }
            }
            /*UAT-916
            if ((orderPaymentDetail.IsNotNull() && orderPaymentDetail.Order.IsNotNull() && orderPaymentDetail.Order.lkpPaymentOption.IsNotNull() && orderPaymentDetail.Order.lkpOrderStatu.IsNotNull())
                && ((orderPaymentDetail.Order.lkpPaymentOption.Name.Equals(OrderPaymentType.InvoicetoInstitution.GetStringValue())
                || orderPaymentDetail.Order.lkpPaymentOption.Name.Equals(OrderPaymentType.InvoiceWithApproval.GetStringValue())) &&
                ((orderPaymentDetail.Order.lkpOrderStatu.Code.Equals(ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue())) || hasActiveComplianceSubscription)
                && (orderPaymentDetail.Order.DeptProgramPackageID.IsNotNull()
                && (orderPaymentDetail.Order.DeptProgramPackageID > 0))))
            {
                View.ShowAutoRenewalControl = true;
                if (orderPaymentDetail.Order.DeptProgramPackage.DPP_IsAutoRenewInvoiceOrder == false || orderPaymentDetail.Order.DeptProgramPackage.DPP_IsAutoRenewInvoiceOrder.IsNull())
                {
                    View.DisableAutoRenewalControl = true;
                }
            }*/
            //UAT-916 
            if ((orderPaymentDetailForCompPkg.IsNotNull() && orderPaymentDetailForCompPkg.Order.IsNotNull() && orderPaymentDetailForCompPkg.lkpPaymentOption.IsNotNull() && orderPaymentDetailForCompPkg.lkpOrderStatu.IsNotNull())
               && ((orderPaymentDetailForCompPkg.lkpPaymentOption.Code.Equals(PaymentOptions.InvoiceWithOutApproval.GetStringValue())
               || orderPaymentDetailForCompPkg.lkpPaymentOption.Code.Equals(PaymentOptions.InvoiceWithApproval.GetStringValue())) &&
               ((orderPaymentDetailForCompPkg.lkpOrderStatu.Code.Equals(ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue())) || hasActiveComplianceSubscription)
               && (orderPaymentDetailForCompPkg.Order.DeptProgramPackageID.IsNotNull()
               && (orderPaymentDetailForCompPkg.Order.DeptProgramPackageID > 0))))
            {
                View.ShowAutoRenewalControl = true;
                if (orderPaymentDetailForCompPkg.Order.DeptProgramPackage.DPP_IsAutoRenewInvoiceOrder == false || orderPaymentDetailForCompPkg.Order.DeptProgramPackage.DPP_IsAutoRenewInvoiceOrder.IsNull())
                {
                    View.DisableAutoRenewalControl = true;
                }
            }

            //UAT-916
            if (orderPaymentDetailForCompPkg.IsNotNull())
            {
                View.CompPkgOrderPaymentDetail = orderPaymentDetailForCompPkg;

                if (orderPaymentDetailForCompPkg.lkpPaymentOption.IsNotNull() && orderPaymentDetailForCompPkg.Order.IsNotNull() && orderPaymentDetailForCompPkg.Order.TotalPrice > AppConsts.NONE)
                    View.SetCompliancePkgPaymentType = orderPaymentDetailForCompPkg.lkpPaymentOption.Name;
                else
                    View.SetCompliancePkgPaymentType = String.Empty;
            }
            #endregion

            //UAT-3077
            if (String.Compare(orderFromOrderPaymentDetail.lkpOrderRequestType.ORT_Code, OrderRequestType.ItemPayment.GetStringValue(), true) == AppConsts.NONE)
            {
                View.IsItemPaymentOrder = true;

                #region UAT-3632

                Dictionary<String, String> dicDataItemPayment = ComplianceDataManager.GetItemPaymentOrderData(View.SelectedTenantId, View.OrderId);
                if (!dicDataItemPayment.IsNullOrEmpty())
                {
                    if (dicDataItemPayment.ContainsKey("ItemName"))
                    {
                        View.PaymentItemName = Convert.ToString(dicDataItemPayment["ItemName"]);
                    }
                    if (dicDataItemPayment.ContainsKey("IsRequirementItemPayment"))
                    {
                        View.IsRequirementItemPayment = Convert.ToBoolean(dicDataItemPayment["IsRequirementItemPayment"]);
                    }
                    if (dicDataItemPayment.ContainsKey("CIDORorderID"))
                    {
                        View.ItemPaymentCIDOROrderID = Convert.ToString(dicDataItemPayment["CIDORorderID"]);
                    }
                }
                #endregion
            }
            else
            {
                View.IsItemPaymentOrder = false;
            }
        }

        ///// <summary>
        ///// Fetch order cancellation details 
        ///// </summary>
        //public void GetOrderCancellationDetails() 
        //{
        //    List<OrderPaymentDetail> orderPaymentDetailList = null;
        //    Order orderFromOrderPaymentDetail = null;
        //    orderPaymentDetailList = ComplianceDataManager.GetAllPaymentDetailsOfOrderByOrderID(View.SelectedTenantId, View.OrderId);

        //    orderFromOrderPaymentDetail = orderPaymentDetailList.Select(x => x.Order).FirstOrDefault();

        //    if (!orderFromOrderPaymentDetail.PackageCancellationHistories.IsNullOrEmpty())
        //    {
        //        var user = SecurityManager.GetOrganizationUser(orderFromOrderPaymentDetail.PackageCancellationHistories.FirstOrDefault().PCH_CancelledByUserID);
        //        View.CompliancePkgCancelledBy = user.IsNullOrEmpty() ? null : (user.FirstName + " " + user.LastName);
        //    }
        //    else 
        //    { 

        //    }
        //}

        /// <summary>
        /// Method to approve orders with status pending payment approval. .
        /// </summary>
        /// <returns></returns>
        public Boolean ApprovePendingOrders()
        {
            //UAT-916 
            OrderPaymentDetail testOrderPayment = new OrderPaymentDetail();
            testOrderPayment = View.OrderPaymentDetailList.FirstOrDefault(x => x.OPD_ID == View.OrderPaymentDetailID);
            //Added below check to ignore the package subscription creation if no compliance package is included in payment deatail to approve.
            if (!View.IsCompliancePackageInclude)
                View.PackageId = 0;
            var orderStatusCode = ApplicantOrderStatus.Paid.GetStringValue();
            if (View.OrderPaymentDetailStatusCode.Equals(ApplicantOrderStatus.Payment_Due.GetStringValue()) ||
              (View.OrderPaymentDetailStatusCode.Equals(ApplicantOrderStatus.Payment_Rejected.GetStringValue()) && testOrderPayment.Order.DuePayment.IsNotNull())) //View.OrderPaymentDetail.Order.DuePayment.IsNotNull()))
            {
                return ComplianceDataManager.UpdatePaymentDueOrders(View.OrderId, View.SelectedTenantId, View.ReferenceNumber, View.OrderPaymentAmount, orderStatusCode, View.OrderPaymentDetailID, View.CurrentLoggedInUserId);
            }
            return ComplianceDataManager.UpdateOrderStatus(View.SelectedTenantId, View.OrderId, orderStatusCode, View.PackageId,
                 View.CurrentLoggedInUserId, View.OrganizationUserId, View.ReferenceNumber, View.ExpiryDate, View.OrderPaymentDetailID);
        }

        /// <summary>
        /// Method to Send Order Approval Mail and Message.
        /// </summary>
        /// <returns></returns>
        public void SendOrderApprovalNotification()
        {
            //PackageSubscription packageSubscription = ComplianceDataManager.GetPackageSubscriptionDetailByOrderId(View.SelectedTenantId, View.OrderId);
            //if (packageSubscription != null || View.OrderPackageTypeCode.Equals(OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()))
            //{


            ComplianceDataManager.SendOrderNotification(View.SelectedTenantId, View.CurrentLoggedInUserId, View.OrderId, View.OrderPaymentDetailID);


            //View.OrderPaymentDetail = ComplianceDataManager.GetOrderDetailById(View.SelectedTenantId, View.OrderId);

            //if (View.OrderPaymentDetail.IsNotNull())
            //{
            //    Int32? systemCommunicationID = null;
            //    Guid? messageID = null;
            //    String orderPackageTypeCode = View.OrderPaymentDetail.Order.lkpOrderPackageType.OPT_Code;
            //    //systemCommunicationID = CommunicationManager.SendOrderApprovalMail(packageSubscription, View.CurrentLoggedInUserId, View.SelectedTenantId);
            //    //messageID = CommunicationManager.SendOrderApprovalMessage(packageSubscription, View.CurrentLoggedInUserId, View.SelectedTenantId);
            //    systemCommunicationID = CommunicationManager.SendOrderApprovalMail(View.OrderPaymentDetail, View.CurrentLoggedInUserId, View.SelectedTenantId);
            //    messageID = CommunicationManager.SendOrderApprovalMessage(View.OrderPaymentDetail, View.CurrentLoggedInUserId, View.SelectedTenantId);

            //    //enter data in order notification table
            //    if (orderPackageTypeCode.Equals(OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()) || orderPackageTypeCode.Equals(OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue()))
            //    {
            //        if (systemCommunicationID != null && messageID != null)
            //        {
            //            List<Entity.ClientEntity.lkpBusinessChannelType> businessChannelType = BackgroundProcessOrderManager.GetBusinessChannelType(View.SelectedTenantId);
            //            List<Entity.ClientEntity.lkpOrderNotificationType> orderNotificationType = BackgroundProcessOrderManager.GetOrderNotificationType(View.SelectedTenantId);

            //            String amsBusinessChannelCode = BusinessChannelType.AMS.GetStringValue();
            //            Int16 businessChannelTypeID = businessChannelType.IsNotNull() && businessChannelType.Count > 0 ?
            //                                          Convert.ToInt16(businessChannelType.FirstOrDefault(cond => cond.Code == amsBusinessChannelCode).BusinessChannelTypeID)
            //                                          : Convert.ToInt16(0);

            //            String ordPaidNotificationTypeCode = OrderNotificationType.ORDER_PAID_NOTIFICATION.GetStringValue();
            //            Int32 ordPaidNotificationTypeID = orderNotificationType.IsNotNull() && orderNotificationType.Count > 0 ?
            //                Convert.ToInt32(orderNotificationType.FirstOrDefault(cond => cond.ONT_Code == ordPaidNotificationTypeCode).ONT_ID) : Convert.ToInt32(0);

            //            OrderNotification ordNotification = new OrderNotification();
            //            ordNotification.ONTF_OrderID = View.OrderId;
            //            ordNotification.ONTF_MSG_SystemCommunicationID = systemCommunicationID;
            //            ordNotification.ONTF_MSG_MessageID = messageID;
            //            ordNotification.ONTF_BusinessChannelTypeID = businessChannelTypeID;
            //            ordNotification.ONTF_IsPostal = false;
            //            ordNotification.ONTF_CreatedByID = View.CurrentLoggedInUserId;
            //            ordNotification.ONTF_CreatedOn = DateTime.Now;
            //            ordNotification.ONTF_ModifiedByID = null;
            //            ordNotification.ONTF_ModifiedDate = null;
            //            ordNotification.ONTF_ParentNotificationID = null;
            //            ordNotification.ONTF_OrderNotificationTypeID = ordPaidNotificationTypeID;
            //            Int32 ordNotificationID = BackgroundProcessOrderManager.CreateOrderNotification(View.SelectedTenantId, ordNotification);
            //        }
            //    }
            //}
        }

        /// <summary>
        ///  Method to Send Order Rejection Mail and Message.
        /// </summary>
        public void SendOrderRejectionNotification()
        {
            OrderPaymentDetail OrderPaymentDetail = View.OrderPaymentDetailList.Where(cond => cond.OPD_ID == View.OrderPaymentDetailID).FirstOrDefault();
            if (OrderPaymentDetail != null)
            {
                CommunicationManager.SendOrderRejectionMail(OrderPaymentDetail, View.SelectedTenantId, View.IsCompliancePackageInclude);
                CommunicationManager.SendOrderRejectionMessage(OrderPaymentDetail, View.SelectedTenantId, View.IsCompliancePackageInclude);
            }
        }

        /// <summary>
        /// Method to Send Order Cancellation Approval Mail and Message.
        /// </summary>
        /// <returns></returns>
        public void SendOrderCancellationApprovalNotification()
        {
            //UAT-926 changed
            if (View.OrderPaymentDetailList != null)
            {
                CommunicationManager.SendOrderCancellationApprovedMail(View.OrderPaymentDetailList.FirstOrDefault(), View.SelectedTenantId);
                CommunicationManager.SendOrderCancellationApprovedMessage(View.OrderPaymentDetailList.FirstOrDefault(), View.SelectedTenantId);
            }
        }

        /// <summary>
        /// Method to Send Order Cancellation Approval Mail and Message.
        /// </summary>
        /// <returns></returns>
        public void SendOrderCancellationRejectionNotification()
        {
            if (View.OrderPaymentDetailList != null)
            {
                CommunicationManager.SendOrderCancellationRejectedMail(View.OrderPaymentDetailList.FirstOrDefault(), View.SelectedTenantId);
                CommunicationManager.SendOrderCancellationRejectedMessage(View.OrderPaymentDetailList.FirstOrDefault(), View.SelectedTenantId);
            }
        }

        /// <summary>
        /// Method to approve cancellations of orders with status cancellation requested.
        /// </summary>
        /// <returns></returns>
        public Boolean ApproveCancellations(Boolean isCancelledByApplicant)
        {
            var orderStatusCode = ApplicantOrderStatus.Cancelled.GetStringValue();
            return ComplianceDataManager.CancelPlacedOrder(View.SelectedTenantId, View.OrderId, orderStatusCode, View.CurrentLoggedInUserId, isCancelledByApplicant);
        }

        /// <summary>
        /// Method to Reject cancellations of orders with status cancellation requested.
        /// </summary>
        /// <returns></returns>
        public Boolean RejectCancellationRequest()
        {
            return ComplianceDataManager.RejectCancellationRequest(View.SelectedTenantId, View.OrderId, View.CurrentLoggedInUserId, View.RejectionReason);
        }

        /// <summary>
        /// Method to Reject Payment of orders with status Pending Payment Approval.
        /// </summary>
        /// <returns></returns>
        public Boolean RejectPaymentRequest()
        {
            String orderStatusCode = ApplicantOrderStatus.Payment_Rejected.GetStringValue();

            //UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.
            if (View.OrderPaymentDetailStatusCode == ApplicantOrderStatus.Pending_School_Approval.GetStringValue())
            {
                orderStatusCode = ApplicantOrderStatus.Online_Payment_Not_Completed.GetStringValue();
            }
            //pass paremeter orderPaymentDetailID to Cancel only selected payment deatil [UAT-916:WB: As an application admin, I should be able to define payment options at the package level in addition to the node level]
            return ComplianceDataManager.CancelPlacedOrder(View.SelectedTenantId, View.OrderId, orderStatusCode, View.CurrentLoggedInUserId, false, View.RejectionPaymentReason, View.OrderPaymentDetailID, View.IsCompliancePackageInclude);
        }

        /// <summary>
        /// Gets the data from table ApplicantComplianceDataItems.
        /// </summary>
        //public void GetNextRecordData()
        //{
        //    List<vwOrderDetail> lstOrderQueueData = null;
        //    IQueryable<vwOrderDetail> resultQuery = null;
        //    SearchItemDataContract searchItemDataContract = View.GridSearchContract;
        //    if (searchItemDataContract.LstStatusCode.Count == AppConsts.NONE)
        //    {
        //        searchItemDataContract.LstStatusCode = new List<String>();
        //        searchItemDataContract.LstStatusCode.Add(ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue());
        //        searchItemDataContract.LstStatusCode.Add(ApplicantOrderStatus.Cancellation_Requested.GetStringValue());
        //        searchItemDataContract.LstStatusCode.Add(ApplicantOrderStatus.Cancelled.GetStringValue());
        //        searchItemDataContract.LstStatusCode.Add(ApplicantOrderStatus.Paid.GetStringValue());
        //        searchItemDataContract.LstStatusCode.Add(ApplicantOrderStatus.Payment_Rejected.GetStringValue());
        //        searchItemDataContract.LstStatusCode.Add(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue());
        //    }
        //    View.GridCustomPaging.DefaultSortExpression = QueueConstants.ORDER_QUEUE_DEFAULT_SORTING_FIELDS;
        //    View.GridCustomPaging.SecondarySortExpression = QueueConstants.ORDER_QUEUE_SECONDARY_SORTING_FIELDS;
        //    View.GridCustomPaging.SortDirectionDescending = true;
        //    if (View.TenantId != SecurityManager.DefaultTenantID)
        //    {
        //        //UAT-1181: Ability to restrict additional nodes to the order queue
        //        //ShowApproveRejectButtons true means parent page is Order Queue
        //        if (View.ShowApproveRejectButtons == true)
        //        {
        //            searchItemDataContract.LstUserNodePermissions = ComplianceDataManager.GetUserNodeOrderPermissions(searchItemDataContract.ClientID, View.CurrentLoggedInUserId).Select(col => col.DPM_ID).ToList();
        //        }
        //        else
        //        {
        //            searchItemDataContract.LstUserNodePermissions = ComplianceDataManager.GetUserNodePermissions(searchItemDataContract.ClientID, View.CurrentLoggedInUserId, searchItemDataContract.ClientID).Select(col => col.DPM_ID).ToList();
        //        }
        //    }
        //    else
        //    {
        //        searchItemDataContract.LstUserNodePermissions = null;
        //    }
        //    resultQuery = ComplianceDataManager.PerformSearch<vwOrderDetail>(searchItemDataContract, View.GridCustomPaging);
        //    lstOrderQueueData = resultQuery.ToList();
        //    if (lstOrderQueueData != null && lstOrderQueueData.Count > AppConsts.NONE)
        //    {
        //        if (lstOrderQueueData.SkipWhile(x => x.OrderId != View.OrderId).Skip(1).FirstOrDefault() != null)
        //        {
        //            View.NextOrderId = lstOrderQueueData.SkipWhile(x => x.OrderId != View.OrderId).Skip(1).FirstOrDefault().OrderId;
        //            if (View.FirstOrderId == View.NextOrderId)
        //                View.NextOrderId = AppConsts.NONE;
        //        }
        //        else if (lstOrderQueueData.Count() == AppConsts.NONE)
        //        {
        //            View.NextOrderId = AppConsts.NONE;
        //        }
        //        else
        //        {
        //            View.NextOrderId = lstOrderQueueData.FirstOrDefault().OrderId;
        //            if (View.FirstOrderId == View.NextOrderId)
        //                View.NextOrderId = AppConsts.NONE;
        //        }
        //    }

        //}

        public void GetNextRecordData()
        {
            List<OrderContract> lstOrderQueueData = null;
            List<OrderContract> resultQuery = null;
            OrderApprovalQueueContract orderApprovalQueueContract = View.GridSearchContract;
            if (orderApprovalQueueContract.LstStatusCode.Count == AppConsts.NONE)
            {
                List<String> lstStatusCode = new List<String>();
                lstStatusCode.Add(ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue());
                lstStatusCode.Add(ApplicantOrderStatus.Cancellation_Requested.GetStringValue());
                lstStatusCode.Add(ApplicantOrderStatus.Cancelled.GetStringValue());
                lstStatusCode.Add(ApplicantOrderStatus.Paid.GetStringValue());
                lstStatusCode.Add(ApplicantOrderStatus.Payment_Rejected.GetStringValue());
                lstStatusCode.Add(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue());
                lstStatusCode.Add(ApplicantOrderStatus.Online_Payment_Not_Completed.GetStringValue());
                lstStatusCode.Add(ApplicantOrderStatus.Pending_School_Approval.GetStringValue());
                orderApprovalQueueContract.OrderStatusCode = String.Join(","
                                                                    , lstStatusCode.ToArray());
            }
            View.GridCustomPaging.DefaultSortExpression = QueueConstants.ORDER_QUEUE_DEFAULT_SORTING_FIELDS;
            View.GridCustomPaging.SecondarySortExpression = QueueConstants.ORDER_QUEUE_SECONDARY_SORTING_FIELDS;
            View.GridCustomPaging.SortDirectionDescending = true;
            if (View.TenantId != SecurityManager.DefaultTenantID)
            {
                //UAT-1181: Ability to restrict additional nodes to the order queue
                orderApprovalQueueContract.LoggedInUserId = View.CurrentLoggedInUserId;
            }
            else
            {
                orderApprovalQueueContract.LoggedInUserId = null;
            }

            resultQuery = ComplianceDataManager.GetOrderApprovalQueueData(orderApprovalQueueContract, View.GridCustomPaging);
            lstOrderQueueData = resultQuery.ToList();
            if (lstOrderQueueData != null && lstOrderQueueData.Count > AppConsts.NONE)
            {
                if (lstOrderQueueData.SkipWhile(x => x.OrderId != View.OrderId).Skip(1).FirstOrDefault() != null)
                {
                    View.NextOrderId = lstOrderQueueData.SkipWhile(x => x.OrderId != View.OrderId).Skip(1).FirstOrDefault().OrderId;
                    View.OrderNumber = lstOrderQueueData.SkipWhile(x => x.OrderId != View.OrderId).Skip(1).FirstOrDefault().OrderNumber;
                    if (View.FirstOrderId == View.NextOrderId)
                        View.NextOrderId = AppConsts.NONE;
                }
                else if (lstOrderQueueData.Count() == AppConsts.NONE)
                {
                    View.NextOrderId = AppConsts.NONE;
                    View.OrderNumber = String.Empty;
                }
                else
                {
                    View.NextOrderId = lstOrderQueueData.FirstOrDefault().OrderId;
                    View.OrderNumber = lstOrderQueueData.FirstOrDefault().OrderNumber;
                    if (View.FirstOrderId == View.NextOrderId)
                        View.NextOrderId = AppConsts.NONE;
                }
            }

        }

        /// <summary>
        /// To get Payment Options/types
        /// </summary>
        //public void GetPaymentOptions()
        //{
        //    var tempPaymentOptions = ComplianceDataManager.GetPaymentOptionsByDPMId(View.TenantId, View.DPM_ID);
        //    View.ListPaymentOptions = tempPaymentOptions.Where(x => x.Code != View.PaymentTypeCode).ToList();
        //    List<lkpPaymentOption> lstPaymntOptn = ComplianceDataManager.GetPaymentTypeList(View.TenantId);
        //    if (!lstPaymntOptn.IsNullOrEmpty())
        //    {
        //        View.PaymentMode_InvoicetoInstitutionId = lstPaymntOptn.Where(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()).FirstOrDefault().PaymentOptionID;
        //        View.PaymentMode_InvoiceWithoutApprovalId = lstPaymntOptn.Where(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()).FirstOrDefault().PaymentOptionID;
        //    }
        //}

        /// <summary>
        /// To get Order by order Id
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public Order GetOrderById(Int32 orderID)
        {
            //UAT-2073: Added IsAdminLoggedIn condition
            if (IsAdminLoggedIn())
                return ComplianceDataManager.GetOrderById(View.SelectedTenantId, orderID);
            else
                return ComplianceDataManager.GetOrderById(View.TenantId, orderID);
        }

        public void GetOrderBkgPackageDetails(List<int> bkgHierarchyMappingIds)
        {
            View.lstExternalPackages = BackgroundSetupManager.GetOrderBkgPackageDetails(View.TenantId, bkgHierarchyMappingIds);
        }

        public void GetBkgOrderData()
        {
            View.BkgPackagesList = ComplianceDataManager.GetBkgOrderById(View.SelectedTenantId, View.BkgOrderID);
        }

        public void GetCancelledBkgOrderData()
        {
            View.BkgPackagesList = ComplianceDataManager.GetCancelledBkgOrderData(View.SelectedTenantId, View.OrderId);
        }


        /// <summary>
        /// Method to update the EDS Related Data For customformdata and also update the external vendor dispatch status.
        /// </summary>
        /// <param name="applicantOrderDataContract">applicantOrderDataContract</param>
        /// <param name="orderId">orderId</param>
        /// <param name="userOrder">userOrder</param>
        public void UpdateEDSStatus()
        {
            #region E-DRUG SCREENING

            BkgOrder bkgOrderObj = BackgroundProcessOrderManager.GetBkgOrderByOrderID(View.SelectedTenantId, View.OrderId);
            if (!bkgOrderObj.IsNullOrEmpty())
            {
                List<BkgOrderPackage> lstBkgOrderPackage = BackgroundProcessOrderManager.GetBackgroundPackageIdListByBkgOrderId(View.SelectedTenantId, bkgOrderObj.BOR_ID);
                if (!lstBkgOrderPackage.IsNullOrEmpty() && (lstBkgOrderPackage.Count() > 0))
                {
                    List<Int32> lstBackgroundPackageId = lstBkgOrderPackage.Select(slct => slct.BkgPackageHierarchyMapping.BackgroundPackage.BPA_ID).ToList();
                    List<Int32> lstBPHM_Id = lstBkgOrderPackage.Select(slt => slt.BOP_BkgPackageHierarchyMappingID.Value).ToList();
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
                        //Create dictionary for parallel task parameter.
                        Dictionary<String, Object> dicParam = new Dictionary<String, Object>();
                        dicParam.Add("BkgOrderId", bkgOrderObj.BOR_ID);
                        dicParam.Add("TenantId", View.SelectedTenantId);
                        dicParam.Add("ExtVendorId", Convert.ToInt32(extVendorId));
                        dicParam.Add("BPHMId_List", lstBPHM_Id);
                        dicParam.Add("RegistrationId", String.Empty);
                        dicParam.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);
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
            }
            #endregion

        }

        /// <summary>
        /// Check whether Order is fresh and has Compliance Package
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean IsComplianceAndFreshOrder()
        {
            return ComplianceDataManager.IsComplianceAndFreshOrder(View.OrderId, View.SelectedTenantId);
        }

        public void CopyBkgDataToCompliancePackage()
        {
            Int32 packageSubscriptionID = ComplianceDataManager.GetPackageSubscriptionID(View.OrderId, View.SelectedTenantId);
            if (packageSubscriptionID > 0)
            {
                Dictionary<String, Object> dataDict = new Dictionary<String, Object>();
                dataDict.Add("packageSubscriptionID", packageSubscriptionID);
                dataDict.Add("tenantId", View.SelectedTenantId);
                dataDict.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);
                dataDict.Add("OrderId", View.OrderId);

                var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                ParallelTaskContext.PerformParallelTask(CopyData, dataDict, LoggerService, ExceptiomService);
            }
            else
            {
                CopyCompPackageDataForNewOrder();
            }
        }

        private void CopyData(Dictionary<String, Object> data)
        {
            Int32 packageSubscriptionID = Convert.ToInt32(data.GetValue("packageSubscriptionID"));
            Int32 tenantId = Convert.ToInt32(data.GetValue("tenantId"));
            Int32 currentLoggedInUserId = Convert.ToInt32(data.GetValue("CurrentLoggedInUserId"));
            //Prod ISsue
            Int32 OrderId = Convert.ToInt32(data.GetValue("OrderId"));
            MobilityManager.CopyCompPackageDataForNewOrder(tenantId, OrderId, currentLoggedInUserId);
            ComplianceDataManager.CopyData(packageSubscriptionID, tenantId, currentLoggedInUserId, null);
        }

        public void GetAutomaticServiceFormForOrder()
        {
            View.lstServiceForm = BackgroundProcessOrderManager.GetAutomaticServiceFormForOrder(View.SelectedTenantId, View.OrderId);
        }

        /// <summary>
        /// Get CustomerProfileId by UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Entity.AuthNetCustomerProfile GetCustomerProfile(Guid userId)
        {
            return ComplianceDataManager.GetCustomerProfile(userId, View.SelectedTenantId);
        }


        /// <summary>
        /// Get Order  by order id.
        /// </summary>
        /// <param name="orderId">orderId</param>
        /// <returns></returns>
        public Order GetOrderByOrderId()
        {
            return ComplianceDataManager.GetOrderById(View.SelectedTenantId, View.OrderId);
        }

        public vw_AddressLookUp GetAddressLookupByHandlerId(String addressHandleId)
        {
            Guid addHandleId = new Guid(addressHandleId);
            return ComplianceDataManager.GetAddressLookupByHandlerId(addHandleId, View.SelectedTenantId);
        }


        public List<OrderPaymentDetail> GetOrderPaymentDetails(Order currentOrder)
        {
            List<OrderPaymentDetail> PaymentDetail = new List<OrderPaymentDetail>();
            if (!currentOrder.IsNullOrEmpty() && !currentOrder.OrderPaymentDetails.IsNullOrEmpty())
            {
                String ordrPkgTypeComplianceRushOrderCode = OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue();
                //var sentForOnlinePaymentDetailTemp = currentOrder.OrderPaymentDetails.Where(cnd => cnd.lkpOrderStatu != null
                //                                                                     && cnd.lkpOrderStatu.Code == sentForOnlinePaymentCode && !cnd.OPD_IsDeleted).ToList();

                var PaymentDetailTemp = currentOrder.OrderPaymentDetails.Where(cnd => cnd.lkpOrderStatu != null
                                                                     && !cnd.OPD_IsDeleted).ToList();

                foreach (var opd in PaymentDetailTemp)
                {
                    if (!opd.OrderPkgPaymentDetails.Any(OPPD => !OPPD.OPPD_IsDeleted && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeComplianceRushOrderCode))
                    {
                        PaymentDetail.Add(opd);
                    }
                }
            }

            return PaymentDetail;
        }

        public Boolean IsBackgroundPackageIncluded(List<OrderPaymentDetail> sentForOnlinePaymentDetailList)
        {
            String ordrPkgTypeBkgCode = OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue();
            Boolean isBackgroundPackageIncluded = false;
            if (!sentForOnlinePaymentDetailList.IsNullOrEmpty())
            {
                foreach (var opd in sentForOnlinePaymentDetailList)
                {
                    isBackgroundPackageIncluded = opd.OrderPkgPaymentDetails.Any(OPPD => OPPD.OPPD_BkgOrderPackageID != null && !OPPD.OPPD_IsDeleted
                                                                          && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeBkgCode);
                    if (isBackgroundPackageIncluded)
                    {
                        break;
                    }
                }
            }
            return isBackgroundPackageIncluded;
        }



        public List<BkgOrderPackage> GetBkgOrderPackageDetail(List<OrderPaymentDetail> sentForOnlinePaymentDetailList)
        {
            String ordrPkgTypeBkgCode = OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue();
            List<Int32> lstBopIDs = new List<Int32>();
            List<BkgOrderPackage> lstBkgOrderPackageList = new List<BkgOrderPackage>();
            foreach (var opd in sentForOnlinePaymentDetailList)
            {
                var bopIds = opd.OrderPkgPaymentDetails.Where(OPPD => OPPD.OPPD_BkgOrderPackageID != null && !OPPD.OPPD_IsDeleted
                                                               && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeBkgCode).Select(slct => slct.OPPD_BkgOrderPackageID.Value)
                                                               .ToList();
                lstBopIDs.AddRange(bopIds);
            }
            View.BopIds = lstBopIDs;
            lstBkgOrderPackageList = BackgroundProcessOrderManager.GetBackgroundOrderPackageListById(View.TenantId, lstBopIDs);
            return lstBkgOrderPackageList;

        }



        /// <summary>
        /// Generates an entry for the Refund request in Order Details screen
        /// </summary>
        /// <param name="refundHistory"></param>
        public void AddRefundHistory(RefundHistory refundHistory)
        {
            ComplianceDataManager.AddRefundHistory(refundHistory, View.SelectedTenantId);
            if (View.IsLocationServiceTenant)
            {
                FingerPrintDataManager.SaveUpdateApointmentRefundAudit(refundHistory, View.SelectedTenantId, View.AppointSlotContract.ApplicantOrgUserId, View.CurrentLoggedInUserId);
            }
        }

        /// <summary>
        /// Returns the list of Refunds associated with the current order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<RefundHistory> GetRefundHistory()
        {
            return ComplianceDataManager.GetRefundHistory(View.OrderId, View.SelectedTenantId);
        }


        public String GenerateDescription()
        {
            String _tenantName = ComplianceDataManager.GetTenantList(SecurityManager.DefaultTenantID, true).Where(col => col.TenantID == View.SelectedTenantId).FirstOrDefault().TenantName;
            //02/17/2015 [SG]: UAT-1023 - Complio: Update Credit Card Spreadsheet
            //return ("Complio: " + _tenantName + " (Order #:  " + View.OrderId + ")");
            return _tenantName;
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        public void GetGranularPermissionForDOBandSSN()
        {
            View.IsDOBDisable = false;
            View.SSNPermissionCode = EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue();
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            if (SecurityManager.GetUserGranularPermission(View.CurrentLoggedInUserId, out dicPermissions))
            {
                if (dicPermissions.ContainsKey(EnumSystemEntity.DOB.GetStringValue()) && dicPermissions[EnumSystemEntity.DOB.GetStringValue()].ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
                {
                    View.IsDOBDisable = true;
                }
                if (dicPermissions.ContainsKey(EnumSystemEntity.SSN.GetStringValue()))
                {
                    View.SSNPermissionCode = dicPermissions[EnumSystemEntity.SSN.GetStringValue()];
                }
            }
        }

        /// <summary>
        /// Getting Masked SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetMaskedSSN(String unMaskedSSN)
        {
            return ApplicationDataManager.GetMaskedSSN(unMaskedSSN);
        }

        #endregion


        /// <summary>
        /// Gets the next page path if Order Stage is incorrect.
        /// </summary>
        /// <param name="applicantOrderCart">applicantOrderCart session variable in which order staging is present.</param>
        public void GetNextPagePathByOrderStageID(ApplicantOrderCart applicantOrderCart)
        {
            View.NextPagePath = ComplianceDataManager.GetNextPagePathByOrderStageID(applicantOrderCart, OrderStages.OrderPaymentDetails);
        }

        public void SavePartialOrderCancellation()
        {
            ComplianceDataManager.SavePartialOrderCancellation(View.SelectedTenantId, View.PartialOrderCancellationXML, View.OrderId,
                                                               View.PartialOrderCancellationTypeCode, View.CurrentLoggedInUserId);
        }

        /// <summary>
        /// Method to Send Partial Order Cancellation Mail and Message.
        /// </summary>
        /// <returns></returns>
        public void SendPartialOrderCancellationNotification(String packageNames)
        {
            //UAT-916
            if (View.OrderPaymentDetailList != null)
            {
                CommunicationManager.SendPartialOrderCancellationMailAndMessage(View.OrderPaymentDetailList.FirstOrDefault(), View.SelectedTenantId, packageNames);
            }
        }

        #region UAT-916:WB: As an application admin, I should be able to define payment options at the package level in addition to the node level
        public void GetOrderPkgPaymentDetail()
        {
            if (View.OrderId > AppConsts.NONE)
            {
                View.OrderPkgPaymentDetailList = ComplianceDataManager.GetOrderPkgPaymentDetailsByOrderID(View.SelectedTenantId, View.OrderId);
            }
        }



        /// <summary>
        /// To get Payment Options/types
        /// </summary>
        public List<lkpPaymentOption> GetPaymentOptionsForChangePayment(String paymentTypeCode)
        {
            String bkgOrderPackageType = OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue();

            String BPHM_IDs = String.Empty;
            List<Int32> listBopId = View.OrderPkgPaymentDetailList.Where(x => x.lkpOrderPackageType.OPT_Code == bkgOrderPackageType && x.OPPD_OrderPaymentDetailID == View.OrderPaymentDetailID && !x.OPPD_IsDeleted).Select(slct => slct.OPPD_BkgOrderPackageID.Value).ToList();
            List<BkgOrderPackage> listBkgOrderPackage = ComplianceDataManager.GetBkgOrderPackageListByBphmIds(View.SelectedTenantId, listBopId);
            BPHM_IDs = GetConcatinatedBphmIds(listBkgOrderPackage);

            Int32 DPP_ID = AppConsts.NONE;
            var _complianceOPD = View.OrderPaymentDetailList.Where(x => x.OPD_ID == View.OrderPaymentDetailID).FirstOrDefault();
            if (_complianceOPD.IsNotNull() && IsComplianceOPD(_complianceOPD))
                DPP_ID = _complianceOPD.Order.DeptProgramPackageID.IsNotNull() ? _complianceOPD.Order.DeptProgramPackageID.Value : 0;

            DataSet tempdataSet = StoredProcedureManagers.GetPaymentOptions(View.SelectedTenantId, DPP_ID, BPHM_IDs, View.DPM_ID);
            View.lstPaymentOption = tempdataSet;
            List<lkpPaymentOption> lstPaymntOptn = ComplianceDataManager.GetPaymentTypeList(View.TenantId);
            if (!lstPaymntOptn.IsNullOrEmpty())
            {
                View.PaymentMode_InvoicetoInstitutionId = lstPaymntOptn.Where(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()).FirstOrDefault().PaymentOptionID;
                View.PaymentMode_InvoiceWithoutApprovalId = lstPaymntOptn.Where(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()).FirstOrDefault().PaymentOptionID;
            }
            return GetCombinedPaymentOption(tempdataSet, paymentTypeCode);
        }

        /// <summary>
        /// Returnes whether the current OrderPaymentDetails object is of Compliance package type or not
        /// </summary>
        /// <param name="orderPaymentDetails"></param>
        /// <returns></returns>
        private Boolean IsComplianceOPD(OrderPaymentDetail orderPaymentDetails)
        {
            var compliancePackageType = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();

            foreach (var oppd in orderPaymentDetails.OrderPkgPaymentDetails)
            {
                if (oppd.lkpOrderPackageType.OPT_Code == compliancePackageType)
                {
                    return true;
                }
            }
            return false;
        }

        private String GetConcatinatedBphmIds(List<BkgOrderPackage> listBkgOrderPackage)
        {
            StringBuilder tempBphmIDs = new StringBuilder();
            listBkgOrderPackage.ForEach(cond =>
            {
                tempBphmIDs.Append(Convert.ToString(cond.BOP_BkgPackageHierarchyMappingID) + ",");
            });
            if (tempBphmIDs.Length > 0)
                return tempBphmIDs.ToString().Remove(tempBphmIDs.Length - 1).TrimEnd();
            return tempBphmIDs.ToString();
        }

        private List<lkpPaymentOption> GetCombinedPaymentOption(DataSet dataSet, String paymentTypeCode)
        {
            List<lkpPaymentOption> tempNodePaymentOption = new List<lkpPaymentOption>();
            List<lkpPaymentOption> tempPackagePaymentOption = new List<lkpPaymentOption>();
            List<lkpPaymentOption> finalPaymentOptions = new List<lkpPaymentOption>();
            if (dataSet.Tables.Count > 0)
            {
                IEnumerable<DataRow> rowsPkgLevel = dataSet.Tables[0].AsEnumerable().Where(cnd => Convert.ToBoolean(cnd["IsPkgLevel"]));
                IEnumerable<DataRow> rowsPkgTemp = dataSet.Tables[0].AsEnumerable().Where(cnd => Convert.ToBoolean(cnd["IsPkgLevel"]));
                IEnumerable<DataRow> rowsNodeLevel = dataSet.Tables[1].AsEnumerable();

                tempPackagePaymentOption = rowsPkgLevel.Select(x => new lkpPaymentOption
                {
                    PaymentOptionID = x["PaymentOptionId"].IsNotNull() ? Convert.ToInt32(Convert.ToString(x["PaymentOptionId"])) : 0,
                    Name = Convert.ToString(x["PaymentOptionName"]),
                    Code = Convert.ToString(x["PaymentOptionCode"]),
                }).ToList();
                Boolean isAnyNodeLevelPaymentOption = dataSet.Tables[0].AsEnumerable().Any(cnd => !Convert.ToBoolean(cnd["IsPkgLevel"]));

                if (isAnyNodeLevelPaymentOption)
                {
                    tempNodePaymentOption = rowsNodeLevel.Select(x => new lkpPaymentOption
                    {
                        PaymentOptionID = x["POId"].IsNotNull() ? Convert.ToInt32(Convert.ToString(x["POId"])) : 0,
                        Name = Convert.ToString(x["POName"]),
                        Code = Convert.ToString(x["POCode"]),
                    }).ToList();
                    if (tempPackagePaymentOption.IsNotNull() && tempPackagePaymentOption.Count() > 0)
                    {
                        List<String> tempPkgPaymCodeLst = tempPackagePaymentOption.Select(x => x.Code).ToList();
                        finalPaymentOptions = tempNodePaymentOption.Where(x => tempPkgPaymCodeLst.Contains(x.Code)).ToList();
                    }
                    else
                        finalPaymentOptions = tempNodePaymentOption;
                }
                else
                {
                    finalPaymentOptions = tempPackagePaymentOption;
                }

            }
            finalPaymentOptions = finalPaymentOptions.Where(cnd => cnd.Code != paymentTypeCode).DistinctBy(x => x.Code).ToList();
            return finalPaymentOptions;

        }

        public Boolean IsOrderPaymentIncludeEDSService()
        {
            if (View.OrderPaymentDetailID > 0)
                return ComplianceDataManager.IsOrderPaymentIncludeEDSService(View.SelectedTenantId, View.OrderPaymentDetailID);
            return false;
        }

        #endregion

        #region UAT-1189:If payment method is the same, both tracking and screening are getting cancelled when the applicant attempts to cancel the tracking order.
        private void IsCompPkgCancelDueToChangeSubs()
        {
            List<String> orderRequestTypeList = new List<String>();
            orderRequestTypeList.Add(OrderRequestType.ChangeSubscription.GetStringValue());
            orderRequestTypeList.Add(OrderRequestType.ChangeSubscriptionByAdmin.GetStringValue());
            var ordr = ComplianceDataManager.GetOrderByPreviousOrderID(View.SelectedTenantId, View.OrderId, orderRequestTypeList);
            if (!ordr.IsNullOrEmpty())
            {
                View.IsCompliancePackageCancelledByChangeSubs = true;
            }
            else
            {
                View.IsCompliancePackageCancelledByChangeSubs = false;
            }
        }
        #endregion

        #region UAT-1358:Complio Notification to applicant for PrintScan

        public Boolean IsPrintScanServiceExistInOrder()
        {
            if (View.OrderPaymentDetailID > 0 && View.SelectedTenantId > 0)
            {
                String bkgServiceTypeCode = BkgServiceType.PRINT_SCAN.GetStringValue();
                return BackgroundProcessOrderManager.IsBkgServiceExistInOrder(View.SelectedTenantId, View.OrderPaymentDetailID, bkgServiceTypeCode);
            }
            return false;
        }

        public void SendNotificationForPrintScanService()
        {
            if (View.OrderPaymentDetailID > 0 && View.SelectedTenantId > 0)
            {
                OrderPaymentDetail OrderPaymentDetail = View.OrderPaymentDetailList.Where(cond => cond.OPD_ID == View.OrderPaymentDetailID).FirstOrDefault();
                CommunicationManager.SendNotificationForPrintScan(OrderPaymentDetail, View.SelectedTenantId);
            }
        }
        #endregion

        #region UAT-1476:When a tracking package is ordered and there was already a previous package with entered data,
        //then there would be data movement as if there were a subscription change.

        public void CopyCompPackageDataForNewOrder()
        {
            Dictionary<String, Object> dataDict = new Dictionary<String, Object>();
            dataDict.Add("OrderId", View.OrderId);
            dataDict.Add("tenantId", View.SelectedTenantId);
            dataDict.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);

            var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
            var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
            ParallelTaskContext.PerformParallelTask(CopyCompPackageDataForNewOrderParallelTask, dataDict, LoggerService, ExceptiomService);

        }
        public void CopyCompPackageDataForNewOrderParallelTask(Dictionary<String, Object> data)
        {
            Int32 OrderId = Convert.ToInt32(data.GetValue("OrderId"));
            Int32 tenantId = Convert.ToInt32(data.GetValue("tenantId"));
            Int32 currentLoggedInUserId = Convert.ToInt32(data.GetValue("CurrentLoggedInUserId"));

            MobilityManager.CopyCompPackageDataForNewOrder(tenantId, OrderId, currentLoggedInUserId);
        }

        #endregion

        #region UAT-1558
        public Boolean UpdateIsGraduatedCompPkg(Boolean isGraduated)
        {
            String GraduatedCode = String.Empty;
            if (isGraduated)
            {
                if (View.ArchiveStateCode == ArchiveState.Active.GetStringValue() || View.ArchiveStateCode.IsNullOrEmpty())
                {
                    GraduatedCode = ArchiveState.Graduated.GetStringValue();
                }
                else if (View.ArchiveStateCode == ArchiveState.Archived.GetStringValue())
                {
                    GraduatedCode = ArchiveState.Archived_and_Graduated.GetStringValue();
                }
            }
            else
            {
                if (View.ArchiveStateCode == ArchiveState.Graduated.GetStringValue())
                {
                    GraduatedCode = ArchiveState.Active.GetStringValue();
                }
                else if (View.ArchiveStateCode == ArchiveState.Archived_and_Graduated.GetStringValue())
                {
                    GraduatedCode = ArchiveState.Archived.GetStringValue();
                }
            }
            //Boolean result = ComplianceDataManager.UpdateCompPkgGraduationStatus(View.TenantId, View.OrderId, GraduatedCode, View.CurrentLoggedInUserId);
            Boolean result = ComplianceDataManager.UpdateCompPkgGraduationStatus(View.TenantId, View.OrderId, GraduatedCode, View.CurrentLoggedInUserId, View.OrgUsrID);

            if (result)
            {
                View.ArchiveStateCode = GraduatedCode;

                //UAT-1827: Notification (Comm Copy setting) for when a student has marked themselves graduated. 
                if (GraduatedCode == ArchiveState.Graduated.GetStringValue() || GraduatedCode == ArchiveState.Archived_and_Graduated.GetStringValue())
                {
                    //var lstClientAdminUsers = SecurityManager.GetClientAdminUsersByTanentId(View.TenantId);

                    //Create Dictionary
                    Dictionary<String, object> dictMailData = new Dictionary<String, object>();
                    dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, "Admin");
                    dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, View.FirstName + " " + View.LastName);

                    //Create MockUp Data
                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                    mockData.UserName = "Admin";
                    mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                    mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                    //Send mail
                    CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_STUDENT_GRADUATED, dictMailData, mockData, View.TenantId, View.DPM_ID, null, null, true);

                    //Send Message
                    CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_STUDENT_GRADUATED, dictMailData, AppConsts.BACKGROUND_PROCESS_USER_VALUE, View.TenantId);
                }
                return result;
            }
            return result;
        }
        /// <summary>
        /// UAT-1683
        /// Implementation of Archive WRT Graduated and Un-Graduated (Archive State).
        /// </summary>
        /// <param name="IsGraduated"></param>
        /// <returns></returns>
        public Boolean UpdateIsGraduatedBkgPkg(Boolean IsGraduated)
        {
            String BkgGraduatedCode = String.Empty;
            if (IsGraduated)
            {
                if (View.BkgArchiveStateCode == ArchiveState.Active.GetStringValue() || View.BkgArchiveStateCode.IsNullOrEmpty())
                {
                    BkgGraduatedCode = ArchiveState.Graduated.GetStringValue();
                }
                else if (View.BkgArchiveStateCode == ArchiveState.Archived.GetStringValue())
                {
                    BkgGraduatedCode = ArchiveState.Archived_and_Graduated.GetStringValue();
                }
            }
            else
            {
                if (View.BkgArchiveStateCode == ArchiveState.Graduated.GetStringValue())
                {
                    BkgGraduatedCode = ArchiveState.Active.GetStringValue();
                }
                else if (View.BkgArchiveStateCode == ArchiveState.Archived_and_Graduated.GetStringValue())
                {
                    BkgGraduatedCode = ArchiveState.Archived.GetStringValue();
                }
            }

            Boolean result = ComplianceDataManager.UpdateIsGraduatedBkgPkg(View.TenantId, View.BkgOrderID, BkgGraduatedCode, View.CurrentLoggedInUserId);
            if (result)
            {
                View.BkgArchiveStateCode = BkgGraduatedCode;

                //UAT-1827: Notification (Comm Copy setting) for when a student has marked themselves graduated. 
                if (BkgGraduatedCode == ArchiveState.Graduated.GetStringValue() || BkgGraduatedCode == ArchiveState.Archived_and_Graduated.GetStringValue())
                {
                    //Create Dictionary
                    Dictionary<String, object> dictMailData = new Dictionary<String, object>();
                    dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, "Admin");
                    dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, View.FirstName + " " + View.LastName);

                    //Create MockUp Data
                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                    mockData.UserName = "Admin";
                    mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                    mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                    //Send mail
                    CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_STUDENT_GRADUATED, dictMailData, mockData, View.TenantId, View.DPM_ID, null, null, true);

                    //Send Message
                    CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_STUDENT_GRADUATED, dictMailData, AppConsts.BACKGROUND_PROCESS_USER_VALUE, View.TenantId);
                }
                return result;
            }
            return result;
        }
        #endregion

        #region UAT-1648:As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
        public Boolean IsCompliancePackageAlreadyPurchased(OrderPaymentDetail ordPaymentDetail)
        {
            String ordrPkgTypeComplianceCode = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();
            String sentForOnlinePaymentStatus = ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue();
            Boolean isCompliancePackageAlreadyPurchased = false;
            if (!ordPaymentDetail.IsNullOrEmpty()
                //Added order status code check to hide approve button for 'Sent For online payment' status if package already purchased.
                && !ordPaymentDetail.lkpOrderStatu.IsNullOrEmpty()
                && String.Compare(ordPaymentDetail.lkpOrderStatu.Code, sentForOnlinePaymentStatus, true) == AppConsts.NONE
                && ordPaymentDetail.OrderPkgPaymentDetails.Any(OPPD => OPPD.OPPD_BkgOrderPackageID == null && !OPPD.OPPD_IsDeleted
                                                                      && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeComplianceCode))
            {
                var compPkgSubscription = ComplianceDataManager.GetPackageSubscriptionByPackageID(ordPaymentDetail.Order.DeptProgramPackage.DPP_CompliancePackageID,
                                                                                                ordPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID,
                                                                                                 View.SelectedTenantId);
                if (!compPkgSubscription.IsNullOrEmpty())
                {
                    isCompliancePackageAlreadyPurchased = true;
                }
            }
            return isCompliancePackageAlreadyPurchased;
        }
        #endregion

        #region UAT-1560:WB: We should be able to add documents that need to be signed to the order process
        public void UpdateAdditionalDocStatus()
        {
            if (ComplianceDataManager.IsSubscriptionExistForApplicant(View.OrganizationUserId, View.SelectedTenantId))
            {
                List<ApplicantDocument> updatedAppDocument = ComplianceDataManager.UpdateAdditionalDocumentStatusForApproveOrder(View.SelectedTenantId, View.OrderId,
                                                               View.CurrentLoggedInUserId, View.OrganizationUserId);
                if (!updatedAppDocument.IsNullOrEmpty())
                {
                    DocumentManager.CallParallelTaskPdfConversionMergingForAppDoc(updatedAppDocument, View.SelectedTenantId, View.OrganizationUserId, View.CurrentLoggedInUserId);
                }
            }
        }
        #endregion
        public void SendAdditionalDocumentToStudent()
        {
            OrderPaymentDetail OrderPaymentDetail = View.OrderPaymentDetailList.Where(cond => cond.OPD_ID == View.OrderPaymentDetailID).FirstOrDefault();
            #region UAT-1759:Create the ability to mark an "Additional Documents" that the students complete in the order flow as "Send to student"
            ComplianceDataManager.SendAdditionalDocumentsToStudent(View.SelectedTenantId, OrderPaymentDetail.Order, View.CurrentLoggedInUserId);
            #endregion
        }
        #region UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.

        /// <summary>
        /// Get Ordr Payment Detail By ID
        /// </summary>
        /// <returns></returns>
        public OrderPaymentDetail GetOrdrPaymentDetailByID()
        {
            if (IsAdminLoggedIn())
                return ComplianceDataManager.GetOrdrPaymentDetailByID(View.SelectedTenantId, View.OrderPaymentDetailID);
            else
                return ComplianceDataManager.GetOrdrPaymentDetailByID(View.TenantId, View.OrderPaymentDetailID);
        }

        /// <summary>
        /// For UAT-2073: Update the Status of the OPD for the Credit Card, to the status specified
        /// </summary>
        public void UpdateOPDStatus()
        {
            //Int32 tenatId = View.SelectedTenantId > 0 ? View.SelectedTenantId : View.TenantId;
            ComplianceDataManager.UpdateOPDStatus(View.SelectedTenantId, ApplicantOrderStatus.Online_Payment_Not_Completed.GetStringValue(), View.OrgUsrID, View.OrderPaymentDetailID);
        }

        /// <summary>
        /// Save transaction details
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <param name="transactionDetails"></param>
        public void SaveTransactionDetails(String invoiceNumber, NameValueCollection transactionDetails)
        {
            if (transactionDetails.IsNotNull())
            {
                Int32 authorizeDotNetUserId = GetAuthorizeDotNetUserID();
                ComplianceDataManager.UpdateOnlineTransactionResults(invoiceNumber, transactionDetails, View.SelectedTenantId, authorizeDotNetUserId);
            }
        }

        /// <summary>
        /// Get Authorize.Net User ID
        /// </summary>
        /// <returns></returns>
        private static Int32 GetAuthorizeDotNetUserID()
        {
            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.AUTHORIZE_DOT_NET_USER_ID);
            Int32 authorizeDotNetUserId = AppConsts.AUTHORIZE_DOT_NET_USER_VALUE;

            if (appConfiguration.IsNotNull())
            {
                authorizeDotNetUserId = Convert.ToInt32(appConfiguration.AC_Value);
            }
            return authorizeDotNetUserId;
        }

        /// <summary>
        /// Checked if logged user is ADB admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
            //Checked if logged user is client admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        public void IsClientAdmin()
        {
            if (IntegrityManager.IsClientAdmin(View.TenantId))
            {
                View.IsClientAdmin = true;
            }
        }

        #region UAT-2422
        public void SetQueueImaging()
        {
            Dictionary<String, Object> dataDictionary = new Dictionary<String, Object>();
            dataDictionary.Add("TenantID", View.SelectedTenantId);
            QueueImagingManager.AsignQueueImagingRepoInstance(dataDictionary);
        }
        #endregion

        #region UAT-2618:If a document has ever been associated with any item within a tracking subscription, the refund functionality on the order detail screen should be grayed out
        public Boolean IsGrayedOutRefundFunctionality(Int32 OPD_ID)
        {
            return ComplianceDataManager.IsGrayedOutRefundFunctionality(View.SelectedTenantId, View.OrderId, OPD_ID);
        }
        #endregion

        #region UAT-3077
        public void ApprovePaymentItem()
        {
            ItemPaymentContract itemPaymentContract = ComplianceDataManager.GetItempaymentDetailsByOrderId(View.OrderId, View.SelectedTenantId);
            if (!itemPaymentContract.IsNullOrEmpty() && itemPaymentContract.orderID > AppConsts.NONE)
            {
                itemPaymentContract.TenantID = View.SelectedTenantId;
                itemPaymentContract.IsPaid = true;
                ComplianceDataManager.CreateItemPaymentOrder(itemPaymentContract);
            }

            Tuple<Int32, Int32, Int32> itemResult = StoredProcedureManagers.ApprovedPaymentItem(View.OrderId, View.SelectedTenantId, View.CurrentLoggedInUserId);

            if (itemResult.Item1 > 0 && itemResult.Item2 > 0 && itemResult.Item3 > 0)
            {
                if (itemPaymentContract.IsRequirementPackage)
                    EvaluateRequirementBuisnessRules(itemResult.Item2, itemResult.Item3, itemPaymentContract.PkgSubscriptionId);
                else
                    evaluatePostSubmitRules(itemResult.Item1, itemResult.Item2, itemResult.Item3);
            }

        }
        private void EvaluateRequirementBuisnessRules(Int32 reqCategoryId, Int32 reqItemId, Int32 pkgSubscriptionId)
        {
            List<RequirementRuleObject> ruleObjectMappingList = new List<RequirementRuleObject>();

            RequirementRuleObject ruleObjectMappingForCategory = new RequirementRuleObject
            {
                RuleObjectTypeCode = ObjectType.Compliance_Category.GetStringValue(),
                RuleObjectId = Convert.ToString(reqCategoryId),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            };

            RequirementRuleObject ruleObjectMappingForItem = new RequirementRuleObject
            {
                RuleObjectTypeCode = ObjectType.Compliance_Item.GetStringValue(),
                RuleObjectId = Convert.ToString(reqItemId),
                RuleObjectParentId = Convert.ToString(reqCategoryId)
            };

            ruleObjectMappingList.Add(ruleObjectMappingForCategory);
            ruleObjectMappingList.Add(ruleObjectMappingForItem);

            RequirementRuleManager.ExecuteRequirementObjectBuisnessRules(ruleObjectMappingList, pkgSubscriptionId, View.CurrentLoggedInUserId, View.TenantId);
        }
        public void evaluatePostSubmitRules(Int32 compliancePackageID, Int32 complianceCategoryId, Int32 complianceItemId)
        {

            List<RuleObjectMapping> ruleObjectMappingList = new List<RuleObjectMapping>();
            RuleObjectMapping ruleObjectMappingForPackage = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Package.GetStringValue(), View.SelectedTenantId).OT_ID),
                RuleObjectId = Convert.ToString(compliancePackageID),
                RuleObjectParentId = Convert.ToString(AppConsts.NONE)
            };

            RuleObjectMapping ruleObjectMappingForCategory = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Category.GetStringValue(), View.SelectedTenantId).OT_ID),
                //RuleObjectId = Convert.ToString(View.ComplianceCategoryId),
                RuleObjectId = Convert.ToString(complianceCategoryId),
                RuleObjectParentId = Convert.ToString(compliancePackageID)
            };

            RuleObjectMapping ruleObjectMappingForItem = new RuleObjectMapping
            {
                RuleObjectTypeId = Convert.ToString(RuleManager.GetObjectType(ObjectType.Compliance_Item.GetStringValue(), View.SelectedTenantId).OT_ID),
                RuleObjectId = Convert.ToString(complianceItemId),
                //RuleObjectParentId = Convert.ToString(View.ComplianceCategoryId)
                RuleObjectParentId = Convert.ToString(complianceCategoryId)
            };


            ruleObjectMappingList.Add(ruleObjectMappingForPackage);
            ruleObjectMappingList.Add(ruleObjectMappingForCategory);
            ruleObjectMappingList.Add(ruleObjectMappingForItem);
            RuleManager.evaluatePostSubmitRules(ruleObjectMappingList, View.OrganizationUserId, View.OrgUsrID, View.SelectedTenantId);
        }
        #endregion

        #region UAT-3268

        public List<ServiceLevelDetailsForOrderContract> GetServiceLevelDetailsForOrder(Int32 tenantId)
        {
            if (tenantId > 0)
            {
                return BackgroundProcessOrderManager.GetServiceLevelDetailsForOrder(tenantId, View.OrderId);
            }
            return new List<ServiceLevelDetailsForOrderContract>();
        }

        #endregion

        #region UAT-3521 || CBI || CABS

        public void IsLocationServiceTenant()
        {
            if (View.TenantId > AppConsts.NONE)
                View.IsLocationServiceTenant = SecurityManager.IsLocationServiceTenant(View.TenantId);
        }

        public void IsBkgOrderWithAppointment()
        {
            if (View.TenantId > AppConsts.NONE && View.OrderId > AppConsts.NONE && View.CurrentLoggedInUserId > AppConsts.NONE)
            {
                View.IsBkgOrderWithAppointment = FingerPrintSetUpManager.IsBkgOrderWithAppointment(View.TenantId, View.OrderId, View.CurrentLoggedInUserId);
            }
        }

        public void GetBkgOrderWithAppointmentData()
        {
            View.AppointSlotContract = new AppointmentSlotContract();
            if (View.TenantId > AppConsts.NONE && View.OrderId > AppConsts.NONE && View.CurrentLoggedInUserId > AppConsts.NONE)
                View.AppointSlotContract = FingerPrintDataManager.GetBkgOrderWithAppointmentData(View.TenantId, View.OrderId, View.CurrentLoggedInUserId);
        }


        public bool HideReschedule(AppointmentSlotContract appointmentSlotContract)
        {
            return FingerPrintDataManager.HideReschedule(appointmentSlotContract, View.SelectedTenantId);
        }

            

        public ReserveSlotContract ReserveSlot()
        {
            return FingerPrintSetUpManager.ReserveSlot(0, View.SelectedSlotID, View.CurrentLoggedInUserId);
        }

        //public ReserveSlotContract SubmitApplicantAppointment()
        //{
        //    return SubmitApplicantAppointment(false, false);
        //}

        public ReserveSlotContract SubmitApplicantAppointment(Boolean isLocationUpdate, Boolean IsOnsiteAppointment, Boolean IsRejectedReschedule)
        {
            ReserveSlotContract reserveSlotContract = new ReserveSlotContract();
            reserveSlotContract.SlotID = View.AppointSlotContract.SlotID;
            reserveSlotContract.TenantID = View.TenantId;
            reserveSlotContract.AppOrgUserID = View.AppointSlotContract.ApplicantOrgUserId;
            reserveSlotContract.OrderID = View.AppointSlotContract.OrderId;
            reserveSlotContract.LocationID = View.AppointSlotContract.LocationId;
            reserveSlotContract.ReservedSlotID = View.AppointSlotContract.ReservedSlotID;
            reserveSlotContract.IsEventTypeCode = IsOnsiteAppointment;
            reserveSlotContract.IsLocationUpdate = isLocationUpdate;
            reserveSlotContract.IsRejectedReschedule = IsRejectedReschedule;

            if (!reserveSlotContract.IsNullOrEmpty())
                return FingerPrintDataManager.SubmitApplicantAppointment(reserveSlotContract, View.CurrentLoggedInUserId);
            else
                return new ReserveSlotContract();
        }

        public Boolean CancelBkgOrder(Decimal? RefundAmount = null)
        {
            if (View.IsLocationServiceTenant)
            {
                var orderStatusCode = ApplicantOrderStatus.Cancelled.GetStringValue();
                Boolean isCancelledByApplicant = true;
                return FingerPrintDataManager.CancelBkgOrder(View.SelectedTenantId, View.OrderId, orderStatusCode, View.CurrentLoggedInUserId, isCancelledByApplicant, View.AppointSlotContract.ApplicantAppointmentId, RefundAmount, View.AppointSlotContract.ApplicantOrgUserId);
            }
            return false;
            //return ComplianceDataManager.CancelPlacedOrder();
        }

        public void SendAppointmentRescheduleNotification(Boolean isLocationUpdateAllowed)
        {
            Boolean isAdmin = false;
            AppointmentOrderScheduleContract appOrdSchdContract = FingerPrintSetUpManager.GetAppointmentOrderDetailData(View.CurrentLoggedInUserId, isAdmin, View.SelectedTenantId.ToString(), View.AppointSlotContract.ApplicantAppointmentId);
            if (!appOrdSchdContract.IsNullOrEmpty() && !View.AppointSlotContract.IsNullOrEmpty())
            {
                AppointmentSlotContract appointmentSlotContract = new AppointmentSlotContract();
                appointmentSlotContract.IsLocationUpdate = isLocationUpdateAllowed;
                appointmentSlotContract.LocationId = appOrdSchdContract.LocationId;
                appointmentSlotContract.SlotDate = appOrdSchdContract.AppointmentDate;
                appointmentSlotContract.SlotStartTime = appOrdSchdContract.StartTime;
                appointmentSlotContract.SlotEndTime = appOrdSchdContract.EndTime;
                appointmentSlotContract.ApplicantOrgUserId = appOrdSchdContract.ApplicantOrgUserId;
                appointmentSlotContract.IsEventType = View.AppointSlotContract.IsEventType;
                appointmentSlotContract.EventName = View.AppointSlotContract.EventName;
                appointmentSlotContract.EventDescription = View.AppointSlotContract.EventDescription;
                var res = FingerPrintSetUpManager.SendFingerPrintAppointmentMailNotification(appOrdSchdContract, appointmentSlotContract, true);
            }
        }

        //UAT-3734
        public string GetLocTenMaxAllowedDays()
        {
            return SecurityManager.GetLocTenMaxAllowedDays();
        }

        //CBI || CABS || Get Suffixes
        public void GetSuffixes()
        {
            View.lstSuffixes = new List<Entity.lkpSuffix>();
            View.lstSuffixes = SecurityManager.GetSuffixes();
        }
        public void IsFileSentToCbi()
        {
            if (!View.OrderId.IsNullOrEmpty())
                View.IsFileSentToCBI = FingerPrintDataManager.IsFileSentToCbi(View.TenantId, View.OrderId);
        }

        #endregion

        #region UAT-3850

        //UAT-3817
        public String GetPaymentApprovedByUsingId(Int32 pymntApprovedById)
        {
            String approvedByName = "";
            Entity.OrganizationUser orgUser = SecurityManager.GetOrganizationUser(pymntApprovedById);
            if (!orgUser.IsNullOrEmpty() && !orgUser.FirstName.IsNullOrEmpty())
                return approvedByName = orgUser.FirstName + " " + orgUser.LastName;

            else return approvedByName;
        }
        public Entity.ClientEntity.OrderBillingCodeMapping GetOrderBillingCode(Int32 orderId)
        {
            return FingerPrintDataManager.GetOrderBillingCode(View.SelectedTenantId, orderId);
        }
        #endregion

        public Boolean IsInstructorPreceptor(Guid userId)
        {
            Boolean _isInstructor = false;
            if (!userId.IsNullOrEmpty())
            {
                List<Entity.OrganizationUserTypeMapping> listOrganizationUserTypeMapping = SecurityManager.GetOrganizationUserTypeMapping(userId);
                _isInstructor = listOrganizationUserTypeMapping.Any(outm => (outm.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Instructor.GetStringValue()) || (outm.lkpOrgUserType.OrgUserTypeCode == OrganizationUserType.Preceptor.GetStringValue()));
            }
            return _isInstructor;
        }
        #region UAT-3954
        public List<String> IsOrderExistForCurrentYear(String orderIds)
        {
            if (IsDuplicateAlertEnabled())
            {
                return ComplianceDataManager.IsOrderExistForCurrentYear(orderIds, View.SelectedTenantId);
            }
            else
                return new List<String>();
        }

        public void GetOrderDetails(String OrderID)
        {
            View.lstOrderQueue = ComplianceDataManager.GetOrderDetails(OrderID, View.SelectedTenantId);
        }

        private Boolean IsDuplicateAlertEnabled()
        {
            var Code = Setting.ORDER_QUEUE_DUPLICATE_ORDER_POPUP_ALERT.GetStringValue();
            return ComplianceDataManager.GetBkgOrderNoteSetting(View.SelectedTenantId, Code);
        }
        #endregion

        public void GetFingerPrintOrderKeydata()
        {

            View.lstFingerPrintData = FingerPrintSetUpManager.GetFingerPrintOrderKeydata(View.OrderId);

        }

        /// <summary>
        /// To Get mailing detail.
        /// 
        /// </summary>
        /// 
        public void GetMailingDetail(int OrderId)
        {
            View.MailingAddressData = FingerPrintDataManager.GetMailingDetail(OrderId, View.TenantId);
        }


        #region UAT-4498
        public void CopyDataForDummyLineItem()
        {
            ComplianceDataManager.CopyDataForDummyLineItem(View.OrderId, View.SelectedTenantId, View.CurrentLoggedInUserId);
        }
        #endregion

        public string GetCountryByCountryId(int CountryId)
        {
            var countryName = "";
            countryName = SecurityManager.GetCountryByCountryId(CountryId);
            return countryName;
        }


        public PreviousAddressContract GetAddressThroughAddressHandleID(string MailingAddressHandleId)
        {
            return FingerPrintDataManager.GetAddressThroughAddressHandleID(View.TenantId, MailingAddressHandleId);
        }
    }
}