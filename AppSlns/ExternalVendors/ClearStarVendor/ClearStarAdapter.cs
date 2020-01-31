#region Header Comment Block

// 
// Copyright 2014 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ClearStarAdapter.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

#endregion

#region Application Specific
using Entity.ExternalVendorContracts;
using ExternalVendors.Interface;
using ExternalVendors.Utility;
using Business.RepoManagers;
using INTSOF.Utils;
using NLog;
using INTSOF.ServiceUtil;
using System.Web.Script.Serialization;
using Entity;
using System.Globalization;
using System.Configuration;
using INTSOF.Utils.PdfHelper;
using Entity.ClientEntity;

#endregion

#endregion

namespace ExternalVendors.ClearStarVendor
{
    public class ClearStarAdapter : IVendorServiceAdapter
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables
        private static String CreateOrderServiceLogger;
        private static String UpdateOrderServiceLogger;
        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public List<State> States
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Clear Star Adapter Constructor
        /// </summary>
        public ClearStarAdapter()
        {
            CreateOrderServiceLogger = "CreateOrderServiceLogger";
            UpdateOrderServiceLogger = "UpdateOrderServiceLogger";
        }

        /// <summary>
        /// Implemented the IVendorServiceAdapter DispatchOrderItemsToVendor method, this is used to dispatch the AMS Orders to Vendors
        /// </summary>
        /// <param name="evOrderContract"></param>
        /// <param name="tenantID"></param>
        /// <returns>EvCreateOrderContract</returns>
        EvCreateOrderContract IVendorServiceAdapter.DispatchOrderItemsToVendor(EvCreateOrderContract evOrderContract, Int32 tenantID, Boolean isTestModeON)
        {
            ServiceLogger.Info("Started process of disptaching order items to vendor: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
            ServiceLogger.Debug<EvCreateOrderContract>("EvCreateOrderContract: ", evOrderContract, CreateOrderServiceLogger);

            ClearStar clearStar = new ClearStar();

            evOrderContract.PackageSvcGroups.ForEach(packageSvcGroup =>
                {

                    String clearStartProfileNumber = String.Empty;
                    Boolean submitOrderFlag = true;

                    ServiceLogger.Debug<Boolean>("SubmitOrderFlag:", submitOrderFlag, CreateOrderServiceLogger);
                    submitOrderFlag = SetupSubmitOrderFlag(submitOrderFlag, false, packageSvcGroup.TransmitInd);
                    ServiceLogger.Debug<Boolean>("SubmitOrderFlag:", submitOrderFlag, CreateOrderServiceLogger);

                    String format = "MM/dd/yyyy";
                    String profileDOB = evOrderContract.DateOfBirth.HasValue ? evOrderContract.DateOfBirth.Value.ToString(format, DateTimeFormatInfo.InvariantInfo) : String.Empty;
                    String gender = evOrderContract.Gender.IsNotNull() ? evOrderContract.Gender : String.Empty;

                    String customerAccountNumber = (evOrderContract.UseADBTestAccountForAMS) ? evOrderContract.ADBTestAccountForAMS : evOrderContract.AccountNumber;

                    CreateProfileForCountry profile = null;
                    //UAT_2169:
                    String noMiddleNameDefaultText = evOrderContract.NoMiddleNameDefaultText;

                    String addedAliasInProfileName = String.Empty;
                    String firstName = evOrderContract.FirstName;
                    String middleName = evOrderContract.MiddleName.IsNullOrEmpty() ? noMiddleNameDefaultText : evOrderContract.MiddleName;
                    String lastName = evOrderContract.LastName;

                    //UAT-2254:Complio: Use CreateProfileForCountry API to create profile instead of CreateProfile which is being used in all three system to create profile.
                    Int32 countryID = evOrderContract.ClearStarCountryID;
                    String emailAddress = evOrderContract.EmailAddress;
                    String phoneNumber = String.Empty;

                    try
                    {
                        //Get the alias name from order items of service group if aliases exist for order and order is for supplement 
                        addedAliasInProfileName = packageSvcGroup.IsSupplement ? GetAliasNameForSupplementOrderProfileName(packageSvcGroup, evOrderContract.OrderProfileAliases, noMiddleNameDefaultText) : String.Empty;
                        if (packageSvcGroup.IsSupplement && !addedAliasInProfileName.IsNullOrEmpty())
                        {
                            String[] aliasName = addedAliasInProfileName.Split(' ');
                            //firstName = String.Join(" ", aliasName.Where((cond, index) => index < aliasName.Length - 1));
                            ////middleName = String.Empty;
                            ////UAT-2169:
                            //middleName = noMiddleNameDefaultText;
                            //lastName = aliasName.LastOrDefault();
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            firstName = aliasName.FirstOrDefault();
                            String middleNameTemp = String.Join(" ", aliasName.Where((cond, index) => index < aliasName.Length - 1 && index > AppConsts.NONE));
                            middleName = middleNameTemp.IsNullOrEmpty() ? noMiddleNameDefaultText : middleNameTemp;
                            lastName = aliasName.LastOrDefault();
                            firstName = isTestModeON ? String.Concat("Test_", firstName) : firstName;
                        }

                        ServiceLogger.Info("Creating clear star profile for BkgOrderPkgSvcGroupID: " + packageSvcGroup.BkgOrderPkgSvcGroupID + " at: " + DateTime.Now.ToString(), CreateOrderServiceLogger);

                        //profile = clearStar.CreateClearstarProfileViaGateway(evOrderContract.LoginName, evOrderContract.Password, evOrderContract.BusinessOwnerID, String.Empty,
                        //        customerAccountNumber, AppConsts.NONE, evOrderContract.InstitutionName, evOrderContract.AccountName, evOrderContract.SSN,
                        //    //evOrderContract.LastName, evOrderContract.FirstName,evOrderContract.MiddleName, 
                        //        lastName, firstName, middleName, String.Empty, String.Empty, gender, profileDOB,
                        //        String.Empty, String.Empty, String.Empty, String.Empty, evOrderContract.Address1, evOrderContract.Address2,
                        //        evOrderContract.City, evOrderContract.State, evOrderContract.ZipCode, evOrderContract.County,
                        //        String.Empty, "P", false, String.Empty, tenantID, evOrderContract.BkgOrderID, packageSvcGroup.BkgOrderPkgSvcGroupID, CreateOrderServiceLogger);

                        //UAT-2254:Complio: Use CreateProfileForCountry API to create profile instead of CreateProfile which is being used in all three system to create profile.
                        profile = clearStar.CreateClearstarProfileForCountryViaGateway(evOrderContract.LoginName, evOrderContract.Password, evOrderContract.BusinessOwnerID, String.Empty,
                                customerAccountNumber, AppConsts.NONE, evOrderContract.InstitutionName, evOrderContract.AccountName, evOrderContract.SSN,
                                //evOrderContract.LastName, evOrderContract.FirstName,evOrderContract.MiddleName, 
                                lastName, firstName, middleName, evOrderContract.ProfileSuffix, String.Empty, gender, profileDOB,
                                String.Empty, String.Empty, String.Empty, String.Empty, evOrderContract.Address1, evOrderContract.Address2,
                                evOrderContract.City, evOrderContract.State, evOrderContract.ZipCode, evOrderContract.County,
                                String.Empty, "P", false, String.Empty, tenantID, evOrderContract.BkgOrderID, packageSvcGroup.BkgOrderPkgSvcGroupID, countryID, emailAddress,
                                phoneNumber, CreateOrderServiceLogger);

                        if (profile != null)
                        {
                            if (((CreateProfileForCountryErrorStatus)profile.Items[AppConsts.NONE]).Code == AppConsts.ZERO)
                            {
                                clearStartProfileNumber = ((CreateProfileForCountryProfile)profile.Items[AppConsts.ONE]).Prof_No;
                                ServiceLogger.Info("Created clear star profile for BkgOrderPkgSvcGroupID: " + packageSvcGroup.BkgOrderPkgSvcGroupID + " ,ProfileNo: " + clearStartProfileNumber
                                    + " at: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
                                ServiceLogger.Debug<CreateProfileForCountry>("Profile created by the clear star: ", profile, CreateOrderServiceLogger);
                                packageSvcGroup.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                                "Profile: " + clearStartProfileNumber
                                                                + " successfully created for BkgOrderPkgSvcGroupID: "
                                                                + packageSvcGroup.BkgOrderPkgSvcGroupID
                                                                , String.Empty);
                                packageSvcGroup.BkgOrderVendorProfileID = clearStartProfileNumber;
                                packageSvcGroup.VendorResponse.IsVendorError = false;

                                submitOrderFlag = AddOrderItemsToProfile(evOrderContract.LoginName, evOrderContract.Password, evOrderContract.BusinessOwnerID,
                                                                         customerAccountNumber, evOrderContract, packageSvcGroup, clearStar, profile, tenantID, submitOrderFlag,
                                                                         addedAliasInProfileName
                                                                         );

                                if (submitOrderFlag) // Do we need to transmit the profile
                                {
                                    #region Transmit Mode
                                    TransmitClearStarProfile(evOrderContract.LoginName, evOrderContract.Password, evOrderContract.BusinessOwnerID, customerAccountNumber, clearStar, profile, tenantID, evOrderContract, packageSvcGroup);
                                    packageSvcGroup.IsTransmitted = true;
                                    #endregion
                                }

                                ServiceLogger.Info("Unlocking clear star profile for BkgOrderPkgSvcGroupID: " + packageSvcGroup.BkgOrderPkgSvcGroupID + " and Profile Number: " + clearStartProfileNumber
                                  + " at " + DateTime.Now.ToString(), CreateOrderServiceLogger);

                                //Unlock the profile
                                UnlockProfile unlockProfile = clearStar.UnlockClearstarProfileViaGateway(evOrderContract.LoginName, evOrderContract.Password, evOrderContract.BusinessOwnerID,
                                    customerAccountNumber, clearStartProfileNumber, tenantID, evOrderContract.BkgOrderID, packageSvcGroup.BkgOrderPkgSvcGroupID, CreateOrderServiceLogger);

                                ServiceLogger.Debug<UnlockProfile>("Unlock profile response:", unlockProfile, CreateOrderServiceLogger);

                                ServiceLogger.Info("Profile Number:" + clearStartProfileNumber + "Unlocked successfully for BkgOrderID: "
                                + evOrderContract.BkgOrderID + " at: " + DateTime.Now.ToString(), CreateOrderServiceLogger);

                                if (((UnlockProfileErrorStatus)unlockProfile.Items[0]).Code != AppConsts.ZERO)
                                {
                                    UnlockProfileErrorStatus unlockProfileErrorStatus = (UnlockProfileErrorStatus)unlockProfile.Items[AppConsts.NONE];
                                    packageSvcGroup.VendorResponse = CommonHelper.SetVendorResponseInContract(unlockProfileErrorStatus.Code,
                                                                     unlockProfileErrorStatus.Message, unlockProfileErrorStatus.Type);
                                    packageSvcGroup.VendorResponse.IsVendorError = true;

                                    ServiceLogger.Info("UnlockProfileErrorStatus error for ProfileNo: " + clearStartProfileNumber + " BkgOrderPkgSvcGroupID: " +
                                    packageSvcGroup.BkgOrderPkgSvcGroupID + " TenantID: " + tenantID + " VendorResponse: " + packageSvcGroup.VendorResponse.ResponseMessage.ToString()
                                    , CreateOrderServiceLogger);

                                    RemoveDraftProfile(evOrderContract.LoginName, evOrderContract.Password, evOrderContract.BusinessOwnerID, customerAccountNumber, clearStar, profile, tenantID, evOrderContract.BkgOrderID, packageSvcGroup.BkgOrderPkgSvcGroupID);
                                }
                            }
                            else
                            {
                                CreateProfileForCountryErrorStatus createProfileErrorStatus = (CreateProfileForCountryErrorStatus)profile.Items[AppConsts.NONE];
                                packageSvcGroup.VendorResponse = CommonHelper.SetVendorResponseInContract(createProfileErrorStatus.Code,
                                                                 createProfileErrorStatus.Message, createProfileErrorStatus.Type);
                                packageSvcGroup.VendorResponse.IsVendorError = true;

                                ServiceLogger.Info("CreateProfileErrorStatus error for ProfileNo: " + clearStartProfileNumber + " BkgOrderPkgSvcGroupID: " +
                                packageSvcGroup.BkgOrderPkgSvcGroupID + " TenantID: " + tenantID + " VendorResponse: " + packageSvcGroup.VendorResponse.ResponseMessage.ToString(), CreateOrderServiceLogger);

                                RemoveDraftProfile(evOrderContract.LoginName, evOrderContract.Password, evOrderContract.BusinessOwnerID, customerAccountNumber, clearStar, profile, tenantID, evOrderContract.BkgOrderID, packageSvcGroup.BkgOrderPkgSvcGroupID);
                            }
                        }
                    }
                    catch (System.Net.WebException webExc) //No network connection
                    {
                        ServiceLogger.Error(String.Format("An Error has occured in DispatchOrderItemsToVendor method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                            webExc.Message, webExc.InnerException, webExc.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString)
                                            , CreateOrderServiceLogger);
                        //RemoveDraftProfile(evOrderContract.LoginName, evOrderContract.Password, evOrderContract.BusinessOwnerID, customerAccountNumber, clearStar, profile, tenantID, evOrderContract.BkgOrderID, packageSvcGroup.BkgOrderPkgSvcGroupID);
                        throw webExc;
                        //evOrderContract.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                        //                                 webExc.Message, String.Empty);
                        //evOrderContract.VendorResponse.IsVendorError = true;

                    }
                    catch (Exception ex)  //Unable to create the profile
                    {
                        ServiceLogger.Error(String.Format("An Error has occured in DispatchOrderItemsToVendor method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                            ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString)
                                            , CreateOrderServiceLogger);
                        packageSvcGroup.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                         ex.Message, String.Empty);
                        packageSvcGroup.VendorResponse.IsVendorError = true;
                        RemoveDraftProfile(evOrderContract.LoginName, evOrderContract.Password, evOrderContract.BusinessOwnerID, customerAccountNumber, clearStar, profile, tenantID, evOrderContract.BkgOrderID, packageSvcGroup.BkgOrderPkgSvcGroupID);
                    }
                });

            ServiceLogger.Info("Ended process of disptaching order items to vendor: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
            return evOrderContract;
        }

        /// <summary>
        /// Implemented the IVendorServiceAdapter RevertExternalVendorChanges method, this method is used to revert back all the vendor changes i.e. Delete Vendor Profile 
        /// and Cancel Vendor Profile.
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="boid"></param>
        /// <param name="customerID"></param>
        /// <param name="vendorProfileNumber"></param>
        /// <param name="tenantID"></param>
        /// <param name="bkgOrderID"></param>
        void IVendorServiceAdapter.RevertExternalVendorChanges(String loginName, String password, Int32 boid, String customerID, String vendorProfileNumber,
            Int32 tenantID, Int32 bkgOrderID, Int32 bkgOrderPackageSvcGroupID)
        {
            ClearStar clearStar = new ClearStar();
            ServiceLogger.Info("Removing ClearStar Draft profile with profile number"
                                + vendorProfileNumber + " : " + DateTime.Now.ToString(), CreateOrderServiceLogger);

            //Delete Clear Star Profile based on Profile No.
            clearStar.DeleteClearstarProfileDraftViaGateway(loginName, password,
                 boid, customerID, vendorProfileNumber, tenantID, bkgOrderID, bkgOrderPackageSvcGroupID, CreateOrderServiceLogger);

            //Cancel Clear Star Profile based on Profile No.
            clearStar.CancelClearstarProfileDraftViaGateway(loginName, password,
                 boid, customerID, vendorProfileNumber, "ADBCreateOrderService", false, tenantID, bkgOrderID, bkgOrderPackageSvcGroupID, CreateOrderServiceLogger);

            ServiceLogger.Info("Removed ClearStar Draft profile with profile number"
                                + vendorProfileNumber + " : " + DateTime.Now.ToString(), CreateOrderServiceLogger);
        }


        /// <summary>
        /// Implemented the IVendorServiceAdapter DispatchOrderItemsToVendor method, this method is used to Update the AMS Order with Clear Star Vendor data.
        /// </summary>
        /// <param name="evUpdateOrderContract"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        EvUpdateOrderContract IVendorServiceAdapter.UpdateVendorBkgOrder(EvUpdateOrderContract evUpdateOrderContract, Int32 tenantID)
        {
            try
            {
                //This code was commented as per changes required by UAT-1244 ticket
                #region Old Impelementation
                //ServiceLogger.Info("Started process of Updating Clearstar AMS Order: " + DateTime.Now.ToString(), UpdateOrderServiceLogger);
                //ClearStar clearStar = new ClearStar();
                //GetProfileDetailProfile gpdp = null;
                //GetProfileDetail gpd = null;

                //try
                //{
                //    //Get all Clearstar orders for this profile
                //    ServiceLogger.Info("Started GetProfileDetail at: " + DateTime.Now.ToString() + " for ProfileNo: " + evUpdateOrderContract.BkgOrderVendorProfileID +
                //        " BkgOrderID: " + evUpdateOrderContract.BkgOrderID + "TenantID:" + tenantID, UpdateOrderServiceLogger);

                //    gpd = clearStar.GetProfileDetailForProfileByProfileID(evUpdateOrderContract.LoginName, evUpdateOrderContract.Password,
                //                                                        evUpdateOrderContract.BusinessOwnerID, evUpdateOrderContract.AccountNumber,
                //                                                        evUpdateOrderContract.BkgOrderVendorProfileID, tenantID, evUpdateOrderContract.BkgOrderID,
                //                                                        evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.BkgOrderPackageServiceGroupID,
                //                                                        UpdateOrderServiceLogger);

                //    ServiceLogger.Info("End GetProfileDetail at: " + DateTime.Now.ToString() + " for ProfileNo: " + evUpdateOrderContract.BkgOrderVendorProfileID
                //    + " BkgOrderID: " + evUpdateOrderContract.BkgOrderID + " TenantID: " + tenantID, UpdateOrderServiceLogger);

                //}
                //catch (Exception ex)
                //{
                //    //log the error
                //    ServiceLogger.Error(String.Format("An Error has occured in GetProfileDetailForProfileByProfileID method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                //                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                //                    UpdateOrderServiceLogger);

                //    evUpdateOrderContract.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                //                                         ex.Message, "System Exception");
                //    evUpdateOrderContract.VendorResponse.IsVendorError = true;

                //    ServiceLogger.Info("GetProfileDetail system exception for ProfileNo: " + evUpdateOrderContract.BkgOrderVendorProfileID + " BkgOrderID: " +
                //    evUpdateOrderContract.BkgOrderID + " TenantID: " + tenantID + " VendorResponse: " + evUpdateOrderContract.VendorResponse.ResponseMessage.ToString(),
                //    UpdateOrderServiceLogger);

                //    return evUpdateOrderContract;
                //}

                ////Status
                ////•	0:  Draft – The Order is on a Profile that is still in Draft.  It is not available to the Supplier or interface.
                ////• 1:  New – The Profile is now Open/In Progress, but the Order has not been picked up by the Supplier or sent to an interface yet.
                ////• 2:  In Progress – The Order is currently being processed by the Supplier (i.e., the batch of Orders has been sent, they’ve picked up their Orders, or an interface is processing it).
                ////• 3:  Completed – The Order has been completed by the Supplier.
                ////• 4:  Cancelled – The Order is on a Profile that has been cancelled.  The Order was not deleted because it was either In Progress or Completed when the Profile was cancelled.
                ////• 5:  Archived – The Order is on a Profile that has been Archived.

                ////Did we get anything back from Clearstar?
                //if (gpd.Items.Length > 0 && ((GetProfileDetailErrorStatus)gpd.Items[0]).Code == "0")
                //{
                //    gpdp = ((GetProfileDetailProfile)gpd.Items[1]);

                //    evUpdateOrderContract.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                //                                        "GetProfileDetail: " + evUpdateOrderContract.BkgOrderVendorProfileID
                //                                        + " successfully for BkgOrderID: "
                //                                        + evUpdateOrderContract.BkgOrderID
                //                                        , String.Empty);

                //    evUpdateOrderContract.VendorResponse.IsVendorError = false;

                //    GetProfileDetailProfileOrder(gpdp, evUpdateOrderContract);

                //    //Fetch the profile status from Clearstar
                //    //ProfileStatusID  - This is a new field that accounts for both the Stage and Status and gives single 
                //    //status for each Profile.  It takes into account the possible combinations of the Status and Stage 
                //    //to give one, definitive status.  The other fields are included for backward compatibility within our system.
                //    //•	0 – Draft 
                //    //•	1 – In Progress
                //    //•	2 - Completed
                //    //•	3 – In Review
                //    //•	4 – Cancelled 
                //    //•	5 – Archived 

                //    GetProfileStatus gps = null;
                //    try
                //    {
                //        ServiceLogger.Info("Started GetProfileStatusByProfileID at: " + DateTime.Now.ToString() + " for ProfileNo: " +
                //        evUpdateOrderContract.BkgOrderVendorProfileID +
                //        " BkgOrderID: " + evUpdateOrderContract.BkgOrderID + "TenantID:" + tenantID, UpdateOrderServiceLogger);

                //        gps = clearStar.GetProfileStatusByProfileID(evUpdateOrderContract.LoginName,
                //                                                              evUpdateOrderContract.Password, evUpdateOrderContract.BusinessOwnerID,
                //                                                              evUpdateOrderContract.AccountNumber, evUpdateOrderContract.BkgOrderVendorProfileID,
                //                                                              tenantID, evUpdateOrderContract.BkgOrderID,
                //                                                              evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.BkgOrderPackageServiceGroupID,
                //                                                              UpdateOrderServiceLogger);

                //        ServiceLogger.Info("End GetProfileStatusByProfileID at: " + DateTime.Now.ToString() + " for ProfileNo: " +
                //        evUpdateOrderContract.BkgOrderVendorProfileID +
                //        " BkgOrderID: " + evUpdateOrderContract.BkgOrderID + "TenantID:" + tenantID, UpdateOrderServiceLogger);
                //    }
                //    catch (Exception ex)
                //    {
                //        ServiceLogger.Error(String.Format("An Error has occured in 'clearStar.GetProfileStatusByProfileID' method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                //                                            ex.Message, ex.InnerException, ex.StackTrace + " current context key : "
                //                                            + ServiceContext.currentThreadContextKeyString), UpdateOrderServiceLogger);

                //        evUpdateOrderContract.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                //                                         ex.Message, "System Exception");
                //        evUpdateOrderContract.VendorResponse.IsVendorError = true;

                //        ServiceLogger.Info("GetProfileStatusByProfileID System Exception for ProfileNo: " + evUpdateOrderContract.BkgOrderVendorProfileID + " BkgOrderID: " +
                //        evUpdateOrderContract.BkgOrderID + " TenantID: " + tenantID + " VendorResponse: " + evUpdateOrderContract.VendorResponse.ResponseMessage.ToString(),
                //        UpdateOrderServiceLogger);
                //        return evUpdateOrderContract;
                //    }

                //    Boolean flaggedInd = false;
                //    GetProfileStatusProfile gpsp = null;
                //    //Update the profile status to match the Clearstar status
                //    //Did we get anything back from Clearstar?
                //    if (gps.Items.Length > 0 && ((GetProfileStatusErrorStatus)gps.Items[0]).Code == "0")
                //    {
                //        gpsp = ((GetProfileStatusProfile)gps.Items[1]);
                //        flaggedInd = Convert.ToBoolean(gpsp.HasFlaggedOrders);

                //        evUpdateOrderContract.BkgOrderFlaggedInd = flaggedInd;

                //        if (gpsp.ProfileStatusID == ClearstarProfileStatus.Cancelled)
                //        {
                //            //evUpdateOrderContract.OrderNewStatusTypeCode = OrderStatusType.CANCELLED.GetStringValue();
                //            evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupStatusCode = BkgSvcGrpStatusType.CANCELLED.GetStringValue();
                //        }
                //        else
                //        {
                //            //UpdateServiceGroupStatus(flaggedInd, evUpdateOrderContract, processedOrderItemList);
                //            UpdateServiceGroupStatus(flaggedInd, evUpdateOrderContract);
                //        }

                //        //Set the status and the hit indicator for this profile                       
                //        /*Commented Below code for UAT-844 
                //        if ((gpsp.ProfileStatusID == ClearstarProfileStatus.Draft || gpsp.ProfileStatusID == ClearstarProfileStatus.InProgress
                //            || gpsp.ProfileStatusID == ClearstarProfileStatus.InReview)
                //            && evUpdateOrderContract.OrderPreviousStatusTypeCode == OrderStatusType.INPROGRESS.GetStringValue())
                //        {
                //            evUpdateOrderContract.OrderNewStatusTypeCode = OrderStatusType.INPROGRESS.GetStringValue();
                //        }
                //        else if (gpsp.ProfileStatusID == ClearstarProfileStatus.Cancelled)
                //        {
                //            evUpdateOrderContract.OrderNewStatusTypeCode = OrderStatusType.CANCELLED.GetStringValue();
                //        }
                //        else if (gpsp.ProfileStatusID == ClearstarProfileStatus.Completed || gpsp.ProfileStatusID == ClearstarProfileStatus.Archived)
                //        {
                //            TODO: Call a method that update the service group  review status.

                //            //if (evUpdateOrderContract.OrderPreviousStatusTypeCode == OrderStatusType.INPROGRESS.GetStringValue()
                //            //    && evUpdateOrderContract.NeedsFirstReview && evUpdateOrderContract.PackageSupplementalTypeCode.IsNotNull() &&
                //            //    (evUpdateOrderContract.PackageSupplementalTypeCode == BkgPackageSupplementalType.ANY.GetStringValue()
                //            //    || (evUpdateOrderContract.PackageSupplementalTypeCode == BkgPackageSupplementalType.FLAGGED.GetStringValue() && flaggedInd)))
                //            //{
                //            //    //It was added so we dont advance the order state if there were multiple profiles on the order
                //            //    //if (amsODA.AreAllProfilesComplete(orderID)){}
                //            //    if (CommonHelper.AreAllProfilesComplete(evUpdateOrderContract, processedOrderItemList))
                //            //    {
                //            //        evUpdateOrderContract.OrderNewStatusTypeCode = OrderStatusType.FIRSTREVIEW.GetStringValue();
                //            //    }
                //            //}
                //            //else if (evUpdateOrderContract.OrderPreviousStatusTypeCode == OrderStatusType.INPROGRESS.GetStringValue())
                //            //{
                //            //    //if (amsODA.AreAllProfilesComplete(orderID))//{//orderStatus = AMSOrderStatusEnum.Completed;//}
                //            //    if (CommonHelper.AreAllProfilesComplete(evUpdateOrderContract, processedOrderItemList))
                //            //    {
                //            //        evUpdateOrderContract.OrderNewStatusTypeCode = OrderStatusType.COMPLETED.GetStringValue();
                //            //    }
                //            //}
                //            //else if (evUpdateOrderContract.OrderPreviousStatusTypeCode == OrderStatusType.ADDITIONALWORK.GetStringValue() && !flaggedInd)
                //            //{
                //            //    //This means that the order was placed in additional work status and the orders were completed without flagged results.                                
                //            //    if (CommonHelper.AreAllProfilesComplete(evUpdateOrderContract, processedOrderItemList))
                //            //    {
                //            //        if (CommonHelper.AreAllProfilesCompleteWithoutFlagged(evUpdateOrderContract, processedOrderItemList))
                //            //        {
                //            //            //This means that the order was placed in additional work status and multiple profiles in the orders 
                //            //            //and there is flagged results in the orders.
                //            //            evUpdateOrderContract.OrderNewStatusTypeCode = OrderStatusType.COMPLETED.GetStringValue();
                //            //        }
                //            //        else
                //            //        {
                //            //            evUpdateOrderContract.OrderNewStatusTypeCode = OrderStatusType.SECONDREVIEW.GetStringValue();
                //            //        }
                //            //    }
                //            //}
                //            //else if (evUpdateOrderContract.OrderPreviousStatusTypeCode == OrderStatusType.ADDITIONALWORK.GetStringValue() && flaggedInd)
                //            //{
                //            //    //This means that the order was placed in additional work status and the orders were completed with flagged results
                //            //    if (CommonHelper.AreAllProfilesComplete(evUpdateOrderContract, processedOrderItemList))
                //            //    {
                //            //        evUpdateOrderContract.OrderNewStatusTypeCode = OrderStatusType.SECONDREVIEW.GetStringValue();
                //            //    }
                //            //}
                //        }

                //         * Commented Below code for UAT-844
                //        //else if (evUpdateOrderContract.OrderPreviousStatusTypeCode == OrderStatusType.INPROGRESS.GetStringValue() &&
                //        //    evUpdateOrderContract.NeedsFirstReview && flaggedInd)
                //        //{
                //        //    if (CommonHelper.AreAllProfilesComplete(evUpdateOrderContract, processedOrderItemList))
                //        //    {
                //        //        evUpdateOrderContract.OrderNewStatusTypeCode = OrderStatusType.FIRSTREVIEW.GetStringValue();
                //        //    }
                //        //}*/
                //    }
                //    else
                //    {
                //        GetProfileStatusErrorStatus getProfileStatusErrorStatus = (GetProfileStatusErrorStatus)gps.Items[AppConsts.NONE];
                //        evUpdateOrderContract.VendorResponse = CommonHelper.SetVendorResponseInContract(getProfileStatusErrorStatus.Code,
                //                                             getProfileStatusErrorStatus.Message, getProfileStatusErrorStatus.Type);
                //        evUpdateOrderContract.VendorResponse.IsVendorError = true;

                //        ServiceLogger.Info("GetProfileStatusErrorStatus error for ProfileNo: " + evUpdateOrderContract.BkgOrderVendorProfileID + " BkgOrderID: " +
                //        evUpdateOrderContract.BkgOrderID + " TenantID: " + tenantID + " VendorResponse: " + evUpdateOrderContract.VendorResponse.ResponseMessage.ToString(),
                //        UpdateOrderServiceLogger);
                //    }
                //}
                //else
                //{
                //    GetProfileDetailErrorStatus getProfileDetailErrorStatus = (GetProfileDetailErrorStatus)gpd.Items[AppConsts.NONE];
                //    evUpdateOrderContract.VendorResponse = CommonHelper.SetVendorResponseInContract(getProfileDetailErrorStatus.Code,
                //                                         getProfileDetailErrorStatus.Message, getProfileDetailErrorStatus.Type);
                //    evUpdateOrderContract.VendorResponse.IsVendorError = true;

                //    ServiceLogger.Info("GetProfileDetailErrorStatus error for ProfileNo: " + evUpdateOrderContract.BkgOrderVendorProfileID + " BkgOrderID: " +
                //    evUpdateOrderContract.BkgOrderID + " TenantID: " + tenantID + " VendorResponse: " + evUpdateOrderContract.VendorResponse.ResponseMessage.ToString(),
                //    UpdateOrderServiceLogger);
                //}
                #endregion

                //New implementation done according to changes required by UAT-1244 ticket
                //System will get profile details and order details only if profile is completed
                //If profile is cancelled or with any other status other than Completed, nothing will be done further
                #region New Impelementation

                ServiceLogger.Info("Started process of Updating Clearstar AMS Order: " + DateTime.Now.ToString(), UpdateOrderServiceLogger);
                ClearStar clearStar = new ClearStar();

                //Fetch the profile status from Clearstar
                //ProfileStatusID  - This is a new field that accounts for both the Stage and Status and gives single 
                //status for each Profile.  It takes into account the possible combinations of the Status and Stage 
                //to give one, definitive status.  The other fields are included for backward compatibility within our system.
                //•	0 – Draft 
                //•	1 – In Progress
                //•	2 - Completed
                //•	3 – In Review
                //•	4 – Cancelled 
                //•	5 – Archived 

                GetProfileStatus gps = null;
                try
                {
                    ServiceLogger.Info("Started GetProfileStatusByProfileID at: " + DateTime.Now.ToString() + " for ProfileNo: " +
                    evUpdateOrderContract.BkgOrderVendorProfileID +
                    " BkgOrderID: " + evUpdateOrderContract.BkgOrderID + "TenantID:" + tenantID, UpdateOrderServiceLogger);

                    gps = clearStar.GetProfileStatusByProfileID(evUpdateOrderContract.LoginName,
                                                                          evUpdateOrderContract.Password, evUpdateOrderContract.BusinessOwnerID,
                                                                          evUpdateOrderContract.AccountNumber, evUpdateOrderContract.BkgOrderVendorProfileID,
                                                                          tenantID, evUpdateOrderContract.BkgOrderID,
                                                                          evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.BkgOrderPackageServiceGroupID,
                                                                          UpdateOrderServiceLogger);

                    ServiceLogger.Info("End GetProfileStatusByProfileID at: " + DateTime.Now.ToString() + " for ProfileNo: " +
                    evUpdateOrderContract.BkgOrderVendorProfileID +
                    " BkgOrderID: " + evUpdateOrderContract.BkgOrderID + "TenantID:" + tenantID, UpdateOrderServiceLogger);
                }
                catch (Exception ex)
                {
                    ServiceLogger.Error(String.Format("An Error has occured in 'clearStar.GetProfileStatusByProfileID' method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : "
                                    + ServiceContext.currentThreadContextKeyString), UpdateOrderServiceLogger);

                    evUpdateOrderContract.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                     ex.Message, "System Exception");
                    evUpdateOrderContract.VendorResponse.IsVendorError = true;

                    ServiceLogger.Info("GetProfileStatusByProfileID System Exception for ProfileNo: " + evUpdateOrderContract.BkgOrderVendorProfileID + " BkgOrderID: " +
                    evUpdateOrderContract.BkgOrderID + " TenantID: " + tenantID + " VendorResponse: " + evUpdateOrderContract.VendorResponse.ResponseMessage.ToString(),
                    UpdateOrderServiceLogger);
                    return evUpdateOrderContract;
                }

                Boolean flaggedInd = false;
                GetProfileStatusProfile gpsp = null;
                //Update the profile status to match the Clearstar status
                //Did we get anything back from Clearstar?
                if (gps.Items.Length > 0 && ((GetProfileStatusErrorStatus)gps.Items[0]).Code == "0")
                {
                    gpsp = ((GetProfileStatusProfile)gps.Items[1]);
                    flaggedInd = Convert.ToBoolean(gpsp.HasFlaggedOrders);

                    evUpdateOrderContract.BkgOrderFlaggedInd = flaggedInd;
                    evUpdateOrderContract.VendorProfileStatus = gpsp.ProfileStatusID;

                    evUpdateOrderContract.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                                "GetProfileStatusByProfileID: " + evUpdateOrderContract.BkgOrderVendorProfileID
                                                                + " successfully for ExternalVendorOrderDetailID: "
                                                                + evUpdateOrderContract.ExternalVendorBkgOrderDetailID
                                                                , String.Empty);

                    evUpdateOrderContract.VendorResponse.IsVendorError = false;


                    if (gpsp.ProfileStatusID == ClearstarProfileStatus.Cancelled)
                    {
                        evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.ServiceGroupStatusCode = BkgSvcGrpStatusType.CANCELLED.GetStringValue();
                    }
                    //UAT-4377
                    //else if (gpsp.ProfileStatusID == ClearstarProfileStatus.Completed)
                    else if (gpsp.ProfileStatusID == ClearstarProfileStatus.Completed || gpsp.ProfileStatusID == ClearstarProfileStatus.Draft || gpsp.ProfileStatusID == ClearstarProfileStatus.InProgress)
                    {
                        //UAT-4377 :- Change required here. 
                        // For status :- Draft, InProgress, Completed  

                        //evUpdateOrderContract.VendorProfileStatus = ClearstarProfileStatus.Completed; 
                        String profileStatusId = gpsp.ProfileStatusID;
                        evUpdateOrderContract.VendorProfileStatus = profileStatusId;
                        //END //UAT-4377
                        GetProfileDetailProfile gpdp = null;
                        GetProfileDetail gpd = null;

                        try
                        {
                            //Get all Clearstar orders for this profile
                            ServiceLogger.Info("Started GetProfileDetail at: " + DateTime.Now.ToString() + " for ProfileNo: " + evUpdateOrderContract.BkgOrderVendorProfileID +
                                " ExternalVendorOrderDetailID: " + evUpdateOrderContract.ExternalVendorBkgOrderDetailID + "TenantID:" + tenantID, UpdateOrderServiceLogger);

                            gpd = clearStar.GetProfileDetailForProfileByProfileID(evUpdateOrderContract.LoginName, evUpdateOrderContract.Password,
                                                                                evUpdateOrderContract.BusinessOwnerID, evUpdateOrderContract.AccountNumber,
                                                                                evUpdateOrderContract.BkgOrderVendorProfileID, tenantID, evUpdateOrderContract.BkgOrderID,
                                                                                evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.BkgOrderPackageServiceGroupID,
                                                                                UpdateOrderServiceLogger);

                            ServiceLogger.Info("End GetProfileDetail at: " + DateTime.Now.ToString() + " for ProfileNo: " + evUpdateOrderContract.BkgOrderVendorProfileID
                            + " ExternalVendorBkgOrderDetailID: " + evUpdateOrderContract.ExternalVendorBkgOrderDetailID + " TenantID: " + tenantID, UpdateOrderServiceLogger);

                        }
                        catch (Exception ex)
                        {
                            //log the error
                            ServiceLogger.Error(String.Format("An Error has occured in GetProfileDetailForProfileByProfileID method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                            ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                                            UpdateOrderServiceLogger);

                            evUpdateOrderContract.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                                 ex.Message, "System Exception");
                            evUpdateOrderContract.VendorResponse.IsVendorError = true;

                            ServiceLogger.Info("GetProfileDetail system exception for ProfileNo: " + evUpdateOrderContract.BkgOrderVendorProfileID + " ExternalVendorBkgOrderDetailID: " +
                            evUpdateOrderContract.ExternalVendorBkgOrderDetailID + " TenantID: " + tenantID + " VendorResponse: " + evUpdateOrderContract.VendorResponse.ResponseMessage.ToString(),
                            UpdateOrderServiceLogger);

                            return evUpdateOrderContract;
                        }

                        //Status
                        //•	0:  Draft – The Order is on a Profile that is still in Draft.  It is not available to the Supplier or interface.
                        //• 1:  New – The Profile is now Open/In Progress, but the Order has not been picked up by the Supplier or sent to an interface yet.
                        //• 2:  In Progress – The Order is currently being processed by the Supplier (i.e., the batch of Orders has been sent, they’ve picked up their Orders, or an interface is processing it).
                        //• 3:  Completed – The Order has been completed by the Supplier.
                        //• 4:  Cancelled – The Order is on a Profile that has been cancelled.  The Order was not deleted because it was either In Progress or Completed when the Profile was cancelled.
                        //• 5:  Archived – The Order is on a Profile that has been Archived.

                        //Did we get anything back from Clearstar?
                        if (gpd.Items.Length > 0 && ((GetProfileDetailErrorStatus)gpd.Items[0]).Code == "0")
                        {
                            gpdp = ((GetProfileDetailProfile)gpd.Items[1]);

                            evUpdateOrderContract.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                                "GetProfileDetail: " + evUpdateOrderContract.BkgOrderVendorProfileID
                                                                + " successfully for ExternalVendorBkgOrderDetailID: "
                                                                + evUpdateOrderContract.ExternalVendorBkgOrderDetailID
                                                                , String.Empty);

                            evUpdateOrderContract.VendorResponse.IsVendorError = false;

                            GetProfileDetailProfileOrder(gpdp, evUpdateOrderContract);

                            //UpdateServiceGroupStatus(flaggedInd, evUpdateOrderContract, processedOrderItemList);
                            #region Commented below code rtegarding UAt-1357
                            //UpdateServiceGroupStatus(flaggedInd, evUpdateOrderContract, tenantID);
                            #endregion

                            CommonHelper.UpdateServiceGroupStatus(flaggedInd, evUpdateOrderContract, tenantID, UpdateOrderServiceLogger);

                        }
                        else
                        {
                            GetProfileDetailErrorStatus getProfileDetailErrorStatus = (GetProfileDetailErrorStatus)gpd.Items[AppConsts.NONE];
                            evUpdateOrderContract.VendorResponse = CommonHelper.SetVendorResponseInContract(getProfileDetailErrorStatus.Code,
                                                                 getProfileDetailErrorStatus.Message, getProfileDetailErrorStatus.Type);
                            evUpdateOrderContract.VendorResponse.IsVendorError = true;

                            ServiceLogger.Info("GetProfileDetailErrorStatus error for ProfileNo: " + evUpdateOrderContract.BkgOrderVendorProfileID + " ExternalVendorBkgOrderDetailID: " +
                            evUpdateOrderContract.ExternalVendorBkgOrderDetailID + " TenantID: " + tenantID + " VendorResponse: " + evUpdateOrderContract.VendorResponse.ResponseMessage.ToString(),
                            UpdateOrderServiceLogger);
                        }

                    }
                }
                else
                {
                    GetProfileStatusErrorStatus getProfileStatusErrorStatus = (GetProfileStatusErrorStatus)gps.Items[AppConsts.NONE];
                    evUpdateOrderContract.VendorResponse = CommonHelper.SetVendorResponseInContract(getProfileStatusErrorStatus.Code,
                                                         getProfileStatusErrorStatus.Message, getProfileStatusErrorStatus.Type);
                    evUpdateOrderContract.VendorResponse.IsVendorError = true;

                    ServiceLogger.Info("GetProfileStatusErrorStatus error for ProfileNo: " + evUpdateOrderContract.BkgOrderVendorProfileID + " ExternalVendorBkgOrderDetailID: " +
                    evUpdateOrderContract.BkgOrderID + " ExternalVendorBkgOrderDetailID: " + tenantID + " VendorResponse: " + evUpdateOrderContract.VendorResponse.ResponseMessage.ToString(),
                    UpdateOrderServiceLogger);
                }
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in DispatchOrderItemsToVendor method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString),
                                    UpdateOrderServiceLogger);

                evUpdateOrderContract.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                         ex.Message, "System Exception");
                evUpdateOrderContract.VendorResponse.IsVendorError = true;

                ServiceLogger.Info("UpdateClearstarAMSOrder System Exception for ProfileNo: " + evUpdateOrderContract.BkgOrderVendorProfileID + " ExternalVendorBkgOrderDetailID: " +
                evUpdateOrderContract.ExternalVendorBkgOrderDetailID + " TenantID: " + tenantID + " VendorResponse: " + evUpdateOrderContract.VendorResponse.ResponseMessage.ToString(),
                UpdateOrderServiceLogger);
                return evUpdateOrderContract;
            }
            return evUpdateOrderContract;
            #endregion
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method is used to Transmit Clear Star Profile. Tranmitted profile will be placed as In-Progress Profile on Clear Star gateway. 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="boid"></param>
        /// <param name="customerID"></param>
        /// <param name="clearStar"></param>
        /// <param name="profile"></param>
        /// <param name="tenantID"></param>
        /// <param name="evOrderContract"></param>
        private void TransmitClearStarProfile(String loginName, String password, Int32 boid, String customerID, ClearStar clearStar, CreateProfileForCountry profile,
                                                    Int32 tenantID, EvCreateOrderContract evOrderContract, EvCreateOrderSvcGroupContract evServiceGroupContract)
        {
            try
            {
                ServiceLogger.Info("Started process of transmitting profile to clear star: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
                String clearStarProfileNumber = ((CreateProfileForCountryProfile)profile.Items[1]).Prof_No;

                //Transmit the order
                TransmitProfile transmitProfile = clearStar.TransmitClearstarProfileViaGateway(loginName, password,
                     boid, customerID, clearStarProfileNumber, tenantID, evOrderContract.BkgOrderID, evServiceGroupContract.BkgOrderPkgSvcGroupID, CreateOrderServiceLogger);

                ServiceLogger.Debug<TransmitProfile>("Transmit profile response:", transmitProfile, CreateOrderServiceLogger);

                if (((TransmitProfileErrorStatus)transmitProfile.Items[AppConsts.NONE]).Code != AppConsts.ZERO)
                {
                    TransmitProfileErrorStatus transmitProfileErrorStatus = (TransmitProfileErrorStatus)transmitProfile.Items[AppConsts.NONE];
                    evServiceGroupContract.VendorResponse = CommonHelper.SetVendorResponseInContract(transmitProfileErrorStatus.Code,
                                                     transmitProfileErrorStatus.Message, transmitProfileErrorStatus.Type);
                    evServiceGroupContract.VendorResponse.IsVendorError = true;

                    ServiceLogger.Info("TransmitProfileErrorStatus error for ProfileNo: " + clearStarProfileNumber + " BkgOrderID: " +
                    evOrderContract.BkgOrderID + " TenantID: " + tenantID + " VendorResponse: " + evServiceGroupContract.VendorResponse.ResponseMessage.ToString(), CreateOrderServiceLogger);

                    RemoveDraftProfile(loginName, password, boid, customerID, clearStar, profile, tenantID, evOrderContract.BkgOrderID, evServiceGroupContract.BkgOrderPkgSvcGroupID);
                }

                ServiceLogger.Info("Ended process of transmitting profile to clear star: " + DateTime.Now.ToString(), CreateOrderServiceLogger);

            }
            catch (Exception ex)
            {
                evServiceGroupContract.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                  ex.Message, String.Empty);
                evServiceGroupContract.VendorResponse.IsVendorError = true;
                ServiceLogger.Error(String.Format("An Error has occured in TrasmitClearStarProfile method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}"
                                   , ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString), CreateOrderServiceLogger);

                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="boid"></param>
        /// <param name="customerID"></param>
        /// <param name="evOrderContract"></param>
        /// <param name="clearStar"></param>
        /// <param name="profile"></param>
        /// <param name="tenantID"></param>
        /// <param name="submitOrderFlag"></param>
        /// <returns></returns>
        private Boolean AddOrderItemsToProfile(String loginName, String password, Int32 boid, String customerID, EvCreateOrderContract evOrderContract, EvCreateOrderSvcGroupContract evServiceGroupContract,
                                                    ClearStar clearStar, CreateProfileForCountry profile, Int32 tenantID, Boolean submitOrderFlag, String addedAliasInProfileName)
        {
            try
            {
                ServiceLogger.Info("Started process of adding orders to clear star profile: " + DateTime.Now.ToString(), CreateOrderServiceLogger);

                String clearStarProfileNumber = ((CreateProfileForCountryProfile)profile.Items[1]).Prof_No;

                Dictionary<Int32, String> existingAliases = new Dictionary<Int32, String>();

                #region UAT-1707:Supplement process, don't create new profile, only add orders to existing profile
                //Added this alias in existingAliases because this alias is already added in profile name 
                //This is working only for supplement order those contains alias.
                if (!addedAliasInProfileName.IsNullOrEmpty() && !existingAliases.Values.Contains(addedAliasInProfileName))
                {
                    addedAliasInProfileName = addedAliasInProfileName.Replace(evOrderContract.NoMiddleNameDefaultText, "");
                    existingAliases.Add(AppConsts.NONE, addedAliasInProfileName);
                }
                #endregion

                //Add orders to clear star profile based upon Order Items
                evServiceGroupContract.OrderItems.ForEach(orderItem =>
                {

                    String orderCountry = String.Empty;
                    String orderState = String.Empty;
                    String orderCounty = String.Empty;
                    String orderCity = String.Empty;
                    String orderZip = String.Empty;
                    String orderInstruction = String.Empty;
                    String orderSB = String.Empty;
                    Int32 aliasID = AppConsts.NONE;

                    #region UAT-1707:Supplement process, don't create new profile, only add orders to existing profile
                    SetAliasInOrderItemAttributesForSupplement(orderItem, addedAliasInProfileName, evOrderContract);
                    #endregion

                    if (orderItem.Alias != String.Empty)
                    {
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        String orderItemAlias = orderItem.Alias.Replace(evOrderContract.NoMiddleNameDefaultText, "");
                        String[] aliasName = orderItem.Alias.Split(' ');
                        //if (existingAliases.Values.Contains(orderItem.Alias))
                        if (existingAliases.Values.Contains(orderItemAlias))
                        {
                            //aliasID = existingAliases.FirstOrDefault(cond => cond.Value.Equals(orderItem.Alias)).Key;
                            aliasID = existingAliases.FirstOrDefault(cond => cond.Value.Equals(orderItemAlias)).Key;
                        }
                        else //if (aliasName.Count().Equals(AppConsts.THREE))
                        {
                            //String aliasFirstName = String.Join(" ", aliasName.Where((cond, index) => index < aliasName.Length - 1));
                            //String aliasMiddleName = String.Empty;
                            //String aliasLastName = aliasName.LastOrDefault();
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            //Commented Below code for Admin Entry Portal
                            //String aliasFirstName = aliasName.FirstOrDefault();
                            //String aliasMiddleName = String.Join(" ", aliasName.Where((cond, index) => index < aliasName.Length - 1 && index > AppConsts.NONE));
                            //String aliasLastName = aliasName.LastOrDefault();

                            String aliasFirstName = String.Empty;
                            String aliasMiddleName = String.Empty;
                            String aliasLastName = String.Empty;
                            String aliasSuffix = String.Empty;
                            if (aliasName.Count() > AppConsts.THREE)
                            {
                                aliasFirstName = aliasName.FirstOrDefault();
                                aliasMiddleName = String.Join(" ", aliasName.Where((cond, index) => index < aliasName.Length - 2 && index > AppConsts.NONE));
                                aliasLastName = String.Join(" ", aliasName.Where((cond, index) => index < aliasName.Length - 1 && index > AppConsts.ONE));
                                aliasSuffix = aliasName.LastOrDefault();
                            }
                            else
                            {
                                aliasFirstName = aliasName.FirstOrDefault();
                                aliasMiddleName = String.Join(" ", aliasName.Where((cond, index) => index < aliasName.Length - 1 && index > AppConsts.NONE));
                                aliasLastName = aliasName.LastOrDefault();
                            }

                            aliasMiddleName = String.IsNullOrEmpty(aliasMiddleName.TrimWhiteSpace()) ? evOrderContract.NoMiddleNameDefaultText : aliasMiddleName;

                            AddAlias addAlias = clearStar.AddAliasNametoProfile(loginName, password, boid, customerID, clearStarProfileNumber
                                                             , aliasFirstName, aliasMiddleName, aliasLastName,aliasSuffix, "A", tenantID, evOrderContract.BkgOrderID
                                                             , orderItem.BkgOrderPackageSvcLineItemID, evServiceGroupContract.BkgOrderPkgSvcGroupID);
                            //If there was an error then report it an stop processing the order.
                            if ((addAlias != null && addAlias.Items.Length == AppConsts.NONE) ||
                                (addAlias != null && addAlias.Items.Length > AppConsts.NONE && ((AddAliasErrorStatus)addAlias.Items[0]).Code != AppConsts.ZERO))
                            {
                                String orderErrorMessage = "Error while adding alias " + orderItem.Alias + " for Profile "
                                                            + clearStarProfileNumber + ": " + ((AddAliasErrorStatus)addAlias.Items[0]).Message;

                                ServiceLogger.Error(orderErrorMessage, CreateOrderServiceLogger);
                                ServiceLogger.Info("Adding Comments to draft ProfileNo: " + clearStarProfileNumber, CreateOrderServiceLogger);
                                clearStar.AddCommentToClearstarProfileDraftViaGateway(loginName, password, boid, customerID,
                                clearStarProfileNumber, false, String.Empty, orderErrorMessage, tenantID, evOrderContract.BkgOrderID, evServiceGroupContract.BkgOrderPkgSvcGroupID, CreateOrderServiceLogger);
                            }
                            else
                            {
                                ServiceLogger.Info("Succesfully added Comments to draft ProfileNo: " + clearStarProfileNumber, CreateOrderServiceLogger);
                                aliasID = Convert.ToInt32(((AddAliasAlias)addAlias.Items[AppConsts.ONE]).iNewAliasID);
                                //existingAliases.Add(aliasID, orderItem.Alias);
                                existingAliases.Add(aliasID, orderItemAlias);
                            }
                        }
                    }

                    CreateCompositeFieldsAttributes(orderItem.EvOrderItemAttributeContracts);

                    TranslateOrderItemAttributesToVendorForm(orderItem, evOrderContract);

                    EvOrderItemAttributeContract countryContract = orderItem.EvOrderItemAttributeContracts
                                                                            .FirstOrDefault(cond => cond.FieldName.ToLower() == AppConsts.COUNTRY.ToLower());
                    if (countryContract.IsNotNull())
                    {
                        orderCountry = countryContract.FieldValue.IsNullOrEmpty() ? countryContract.DefaultValue : countryContract.FieldValue;
                    }

                    EvOrderItemAttributeContract stateContract = orderItem.EvOrderItemAttributeContracts
                                                                            .FirstOrDefault(cond => cond.FieldName.ToLower() == AppConsts.STATE.ToLower());
                    if (stateContract.IsNotNull())
                    {
                        orderState = stateContract.FieldValue.IsNullOrEmpty() ? stateContract.DefaultValue : stateContract.FieldValue;
                    }

                    EvOrderItemAttributeContract countyContract = orderItem.EvOrderItemAttributeContracts
                                                                            .FirstOrDefault(cond => cond.FieldName.ToLower() == AppConsts.COUNTY.ToLower());
                    if (countyContract.IsNotNull())
                    {
                        orderCounty = countyContract.FieldValue.IsNullOrEmpty() ? countyContract.DefaultValue : countyContract.FieldValue;
                    }

                    EvOrderItemAttributeContract cityContract = orderItem.EvOrderItemAttributeContracts
                                                                            .FirstOrDefault(cond => cond.FieldName.ToLower() == AppConsts.CITY.ToLower());
                    if (cityContract.IsNotNull())
                    {
                        orderCity = cityContract.FieldValue.IsNullOrEmpty() ? cityContract.DefaultValue : cityContract.FieldValue;
                    }

                    EvOrderItemAttributeContract zipContract = orderItem.EvOrderItemAttributeContracts
                                                                            .FirstOrDefault(cond => cond.FieldName.ToLower() == AppConsts.ZIPCODE.ToLower());
                    if (zipContract.IsNotNull())
                    {
                        orderZip = zipContract.FieldValue.IsNullOrEmpty() ? zipContract.DefaultValue : zipContract.FieldValue;
                    }

                    orderSB = GenerateOrderFieldXML(orderItem);

                    ServiceLogger.Info("Started AddOrderToClearstarProfileViaGateway for OrderItemID: " + orderItem.BkgOrderPackageSvcLineItemID
                    + "to clear star profile at: " + DateTime.Now.ToString(), CreateOrderServiceLogger);

                    AddOrderToProfile clearStarOrder = clearStar.AddOrderToClearstarProfileViaGateway(loginName, password, boid, customerID,
                                        clearStarProfileNumber, orderItem.ExternalBackgroundServiceCode,
                                        orderCountry, orderState, orderCounty, orderCity, orderZip,
                                        orderInstruction, "D", aliasID, orderSB, tenantID,
                                        evOrderContract.BkgOrderID, orderItem.BkgOrderPackageSvcLineItemID, evServiceGroupContract.BkgOrderPkgSvcGroupID, CreateOrderServiceLogger);

                    ServiceLogger.Debug<AddOrderToProfile>("Order added to profile with Profile number= " + clearStarProfileNumber
                                                            + " :", clearStarOrder, CreateOrderServiceLogger);
                    ServiceLogger.Info("End AddOrderToClearstarProfileViaGateway for OrderItemID: " + orderItem.BkgOrderPackageSvcLineItemID
                    + "to clear star profile successfully at: " + DateTime.Now.ToString(), CreateOrderServiceLogger);

                    if (clearStar != null && clearStarOrder.Items.Count() > 0 && ((AddOrderToProfileErrorStatus)clearStarOrder.Items[0]).Code != AppConsts.ZERO)
                    {
                        String orderErrorMessage;
                        if (((AddOrderToProfileErrorStatus)clearStarOrder.Items[0]).Message ==
                                                           "One or more of the Orders is a duplicate of an existing Order.")
                        {
                            AddOrderToProfileErrorStatus addOrderToProfileErrorStatus = (AddOrderToProfileErrorStatus)clearStarOrder.Items[0];
                            orderItem.VendorResponse = CommonHelper.SetVendorResponseInContract(addOrderToProfileErrorStatus.Code, addOrderToProfileErrorStatus.Message,
                                                                                   addOrderToProfileErrorStatus.Type);
                            orderItem.VendorResponse.IsSpecialError = true;

                            String profileComment = "Error occured adding order for Service Number: " + orderItem.ExternalBackgroundServiceCode +
                                           " for AMS OrderID: " + evServiceGroupContract.BkgOrderPkgSvcGroupID + " and Profile Number: " +
                                           ((CreateProfileForCountryProfile)profile.Items[1]).Prof_No + ".  The Order Data was: Country = '" +
                                           orderCountry + "' City = '" + orderCity + "' State = '" + orderState + "' Zip = '" +
                                           orderZip + "' County = '" + orderCounty +
                                           "'.  The error message is: " + ((AddOrderToProfileErrorStatus)clearStarOrder.Items[0]).Message + "<br /><br /> ";

                            clearStar.AddCommentToClearstarProfileDraftViaGateway(loginName, password, boid, customerID,
                            clearStarProfileNumber, false, String.Empty, profileComment, tenantID, evOrderContract.BkgOrderID, evServiceGroupContract.BkgOrderPkgSvcGroupID, CreateOrderServiceLogger);
                            submitOrderFlag = SetupSubmitOrderFlag(submitOrderFlag, true, evServiceGroupContract.TransmitInd);
                        }
                        else if (((AddOrderToProfileErrorStatus)clearStarOrder.Items[0]).Message.ToUpper().Contains("LOCATION"))
                        {
                            AddOrderToProfileErrorStatus addOrderToProfileErrorStatus = (AddOrderToProfileErrorStatus)clearStarOrder.Items[0];
                            orderItem.VendorResponse = CommonHelper.SetVendorResponseInContract(addOrderToProfileErrorStatus.Code, addOrderToProfileErrorStatus.Message,
                                                                                   addOrderToProfileErrorStatus.Type);
                            orderItem.VendorResponse.IsSpecialError = true;

                            String profileComment = "Error occured adding order for Service Number: " + orderItem.ExternalBackgroundServiceCode +
                                           " for AMS OrderID: " + evServiceGroupContract.BkgOrderPkgSvcGroupID + " and Profile Number: " +
                                           ((CreateProfileForCountryProfile)profile.Items[1]).Prof_No + ".  The Order Data was: Country = '" +
                                           orderCountry + "' City = '" + orderCity + "' State = '" + orderState + "' Zip = '" +
                                           orderZip + "' County = '" + orderCounty +
                                           "'.  The error message is: " + ((AddOrderToProfileErrorStatus)clearStarOrder.Items[0]).Message + "<br /><br /> ";

                            clearStar.AddCommentToClearstarProfileDraftViaGateway(loginName, password, boid, customerID,
                            clearStarProfileNumber, false, String.Empty, profileComment, tenantID, evOrderContract.BkgOrderID, evServiceGroupContract.BkgOrderPkgSvcGroupID, CreateOrderServiceLogger);

                            submitOrderFlag = SetupSubmitOrderFlag(submitOrderFlag, true, evServiceGroupContract.TransmitInd);
                        }
                        else
                        {
                            AddOrderToProfileErrorStatus addOrderToProfileErrorStatus = (AddOrderToProfileErrorStatus)clearStarOrder.Items[0];
                            orderItem.VendorResponse = CommonHelper.SetVendorResponseInContract(addOrderToProfileErrorStatus.Code, addOrderToProfileErrorStatus.Message,
                                                                                   addOrderToProfileErrorStatus.Type);
                            orderItem.VendorResponse.IsVendorError = true;

                            orderErrorMessage = "There was an error while adding the order with following description:"
                                                        + ((AddOrderToProfileErrorStatus)clearStarOrder.Items[0]).Message;
                            submitOrderFlag = SetupSubmitOrderFlag(submitOrderFlag, true, evServiceGroupContract.TransmitInd);
                            throw new Exception(orderErrorMessage);
                        }

                    }
                    else
                    {
                        AddOrderToProfileProfile addOrderToProfileProfile = (AddOrderToProfileProfile)clearStarOrder.Items[1];
                        orderItem.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty, "Order Successfully added to the Profile:"
                                                                               + addOrderToProfileProfile.sProf_No, String.Empty);
                        orderItem.VendorResponse.IsVendorError = false;
                        orderItem.ExternalVendorOrderID = addOrderToProfileProfile.NewOrderID;

                        ServiceLogger.Info("AddOrderToProfileProfile successfully added for ProfileNo: " + clearStarProfileNumber + ", BkgOrderID: " +
                        evServiceGroupContract.BkgOrderPkgSvcGroupID + ", TenantID: " + tenantID + ", VendorResponse: " + orderItem.VendorResponse.ResponseMessage.ToString(), CreateOrderServiceLogger);


                        if (orderItem.BkgSvcTypeCode == BkgServiceType.ELECTRONICDRUGSCREEN.GetStringValue())
                        {
                            String orderRegistrationID = String.Empty;
                            EvOrderItemAttributeContract registrationContract = orderItem.EvOrderItemAttributeContracts
                                                                        .FirstOrDefault(cond => cond.FieldLabel == AppConsts.RegistrationID);
                            if (registrationContract.IsNotNull())
                            {
                                orderRegistrationID = registrationContract.FieldValue.IsNullOrEmpty() ? registrationContract.DefaultValue : registrationContract.FieldValue;
                            }
                            //Electronic Drug Screen needs to be tied together with the CS profile/order.
                            UpdateClearStarOrderInfo updateCSOrderInfo =
                                clearStar.UpdateClearstarWebCCFInfo(boid, customerID, evOrderContract.ClearStarCCFUserName, evOrderContract.ClearStarCCFPassword, orderRegistrationID
                                , profile, clearStarOrder, tenantID, evOrderContract.BkgOrderID, evServiceGroupContract.BkgOrderPkgSvcGroupID, CreateOrderServiceLogger);
                            if (updateCSOrderInfo.Result != "SUCCESS")
                            {
                                //We were unable to link the CS profile/order and WebCCF RegistrationID together
                                throw new Exception("Unable to update ClearStarWebCCFInfo for orderID "
                                                    + evServiceGroupContract.BkgOrderPkgSvcGroupID + ", LineItemID " + orderItem.BkgOrderPackageSvcLineItemID);
                            }

                            ////Get the current version of the registration pdf that needs to be stored in Clearstar
                            //ClearStarCCF webCCFDonor = new ClearStarCCF();
                            //GetCCF ccfPDF = null;
                            ////Get webCCFDonor PDF
                            //ccfPDF = webCCFDonor.GetClearstarDonorCCFPDF(boid,
                            //    customerID, loginName,
                            //    password, orderRegistrationID);
                            //byte[] docContents = null;
                            //if (ccfPDF != null && !string.IsNullOrEmpty(ccfPDF.PdfFile))
                            //{
                            //    docContents = System.Convert.FromBase64String(ccfPDF.PdfFile);
                            //    docContents = iTextSharpPDFWrapper.CompressPDFDocument(docContents);
                            //}
                            //Upload a copy of the registration PDF to the CS profile/order

                            String documentPath = ExternalVendorOrderManager.GetCCFDocument(evOrderContract.OrganizationUserProfileID, tenantID);
                            byte[] docContents = CommonFileManager.RetrieveDocument(documentPath, FileType.ApplicantFileLocation.GetStringValue());

                            if (docContents.IsNotNull())
                            {
                                docContents = iTextSharpPDFWrapper.CompressPDFDocument(docContents);

                                UploadOrderDocument2 upd2 = clearStar.UploadProfileDocumentViaGateway(loginName, password,
                                    boid, customerID, ((CreateProfileForCountryProfile)profile.Items[1]).Prof_No,
                                    Convert.ToInt32(addOrderToProfileProfile.NewOrderID), orderRegistrationID + ".pdf", docContents, CreateOrderServiceLogger, tenantID, evOrderContract.BkgOrderID, evServiceGroupContract.BkgOrderPkgSvcGroupID);

                                if (upd2 != null && ((UploadOrderDocument2ErrorStatus)upd2.Items[0]).Code != "0")
                                {
                                    String profileComment = " Error adding eDrug Screen PDF to Clearstar Order.  " + " RegistrationID = " + orderRegistrationID +
                                         " ProfileID = " + ((CreateProfileForCountryProfile)profile.Items[1]).Prof_No + " Clearstar OrderID = " + addOrderToProfileProfile.NewOrderID +
                                         " ServiceID: " + orderItem.ExternalBackgroundServiceCode + " AMS OrderID = " + evServiceGroupContract.BkgOrderPkgSvcGroupID;

                                    clearStar.AddCommentToClearstarProfileDraftViaGateway(loginName, password, boid, customerID,
                                      clearStarProfileNumber, false, String.Empty, profileComment, tenantID, evOrderContract.BkgOrderID, evServiceGroupContract.BkgOrderPkgSvcGroupID, CreateOrderServiceLogger);

                                    ServiceLogger.Error(profileComment, CreateOrderServiceLogger);
                                    //Dont transmit the order
                                    submitOrderFlag = SetupSubmitOrderFlag(submitOrderFlag, true, evServiceGroupContract.TransmitInd);
                                }
                            }
                        }
                    }
                });
                ServiceLogger.Info("Ended process of adding orders to clear star profile: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
            }
            catch (Exception ex)
            {
                evServiceGroupContract.VendorResponse = CommonHelper.SetVendorResponseInContract(String.Empty,
                                                 ex.Message, String.Empty);
                evServiceGroupContract.VendorResponse.IsVendorError = true;
                ServiceLogger.Error(String.Format("An Error has occured in AddOrderItemsToProfile method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}"
                    , ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString), CreateOrderServiceLogger);

                throw ex;
            }

            return submitOrderFlag;
        }

        private void CreateCompositeFieldsAttributes(List<EvOrderItemAttributeContract> orderItemAttributeContracts)
        {
            List<Int32> compositeFieldIDs = orderItemAttributeContracts.GroupBy(i => i.FieldID,
                                                (key, group) => new { key, group }).Where(cond => cond.group.Count() > AppConsts.ONE)
                                                .Select(col => col.key)
                                                .Distinct().ToList();

            compositeFieldIDs.ForEach(fieldID =>
             {
                 EvOrderItemAttributeContract compositeContract = orderItemAttributeContracts.FirstOrDefault(cond => cond.FieldID == fieldID).DeepClone();
                 String fieldDelimiter = compositeContract.FieldDelimiter;
                 List<EvOrderItemAttributeContract> fieldAttributeContracts = orderItemAttributeContracts.Where(cond => cond.FieldID == fieldID).ToList();

                 if (!fieldDelimiter.Equals(""))
                 {
                     fieldAttributeContracts.OrderBy(cond => cond.FieldSequence).ForEach(attrContract =>
                         {
                             if (attrContract.FieldFormat.Equals("AAAA"))
                             {
                                 if (!attrContract.FieldValue.IsNullOrEmpty() && String.Compare(attrContract.FieldValue, AppConsts.EMPLOYE_CURRENT_END_DATE, true) != AppConsts.NONE)
                                 {
                                     DateTime fieldDate = DateTime.Parse(attrContract.FieldValue, DateTimeFormatInfo.InvariantInfo);
                                     attrContract.FieldValue = String.Format("{0:D2}", fieldDate.Month)
                                          + "/" + Convert.ToString(fieldDate.Year).Substring(2, 2);
                                 }
                             }
                         });

                     String fieldCompositeValue = String.Join(fieldDelimiter, fieldAttributeContracts.OrderBy(cond => cond.FieldSequence).Select(col => col.FieldValue));
                     orderItemAttributeContracts.RemoveAll(cond => cond.FieldID == fieldID);
                     compositeContract.FieldValue = fieldCompositeValue;
                     orderItemAttributeContracts.Add(compositeContract);
                 }

             });
        }

        private void TranslateOrderItemAttributesToVendorForm(EvOrderItemContract orderItem, EvCreateOrderContract evOrderContract)
        {
            EvOrderItemAttributeContract stateContract = orderItem.EvOrderItemAttributeContracts
                                                                       .FirstOrDefault(cond => cond.FieldDataType == SvcAttributeDataType.STATE.GetStringValue());
            if (stateContract.IsNotNull())
            {
                if (States.IsNull())
                {
                    States = new List<State>();
                }

                State state = States.FirstOrDefault(cond => cond.StateName == stateContract.FieldValue);

                if (state.IsNotNull())
                {
                    stateContract.FieldValue = state.StateAbbreviation;
                }
                //Commented below code regarding UAT-880 : Issue with Employment form mapping to "City/State" clearstar field.
                //else if (stateContract.FieldValue.Length > AppConsts.TWO)
                //{
                //    stateContract.FieldValue = stateContract.FieldValue.Substring(0, 2);
                //}
            }

            EvOrderItemAttributeContract countryContract = orderItem.EvOrderItemAttributeContracts
                                                                                .FirstOrDefault(cond => cond.FieldDataType == SvcAttributeDataType.COUNTRY.GetStringValue());
            if (countryContract.IsNotNull())
            {
                GetCountryISO3Code(countryContract, evOrderContract.lstCountries);
            }
        }

        private void GetCountryISO3Code(EvOrderItemAttributeContract evOrderItemAttributeContract, List<EvCountryLookup> lstCountryLookup)
        {
            //UAT-2893:Update country code sent to clearstar for international criminals
            EvCountryLookup countryLookupTemp = new EvCountryLookup();
            if (!lstCountryLookup.IsNullOrEmpty())
            {
                countryLookupTemp = lstCountryLookup.FirstOrDefault(cnd => cnd.FullName.ToLower() == evOrderItemAttributeContract.FieldValue.ToLower());
            }

            if (countryLookupTemp.IsNullOrEmpty())
            {
                switch (evOrderItemAttributeContract.FieldValue)
                {
                    case "CANADA":
                        {
                            evOrderItemAttributeContract.FieldValue = CountryISO3Code.CANADA.GetStringValue();
                            break;
                        }
                    case "MEXICO":
                        {
                            evOrderItemAttributeContract.FieldValue = CountryISO3Code.MEXICO.GetStringValue();
                            break;
                        }
                    case "UNITED KINGDOM":
                        {
                            evOrderItemAttributeContract.FieldValue = CountryISO3Code.UNITED_KINGDOM.GetStringValue();
                            break;
                        }
                    case "UNITED STATES":
                        {
                            evOrderItemAttributeContract.FieldValue = CountryISO3Code.UNITED_STATES.GetStringValue();
                            break;
                        }
                    default:
                        {
                            // Commented to fix UAT 805
                            //if (evOrderItemAttributeContract.FieldValue.Length > AppConsts.THREE)
                            //{
                            //    evOrderItemAttributeContract.FieldValue = evOrderItemAttributeContract.FieldValue.Substring(0, 3);
                            //}
                            break;
                        }
                }
            }
            else
            {
                evOrderItemAttributeContract.FieldValue = countryLookupTemp.ISO3Code;
            }
        }

        private String GenerateOrderFieldXML(EvOrderItemContract orderItem)
        {
            String orderSB = String.Empty;

            IEnumerable<EvOrderItemAttributeContract> orderFields = orderItem.EvOrderItemAttributeContracts.Where(cond => cond.FieldName.IsNullOrEmpty());

            if (orderFields.IsNotNull() && orderFields.Count() > AppConsts.NONE)
            {
                XmlDocument doc = new XmlDocument();
                XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("OrderFields"));

                orderFields.ForEach(orderField =>
                {
                    XmlNode exp = el.AppendChild(doc.CreateElement("OrderField"));
                    XmlAttribute expNewOrderAttr = exp.Attributes.Append(doc.CreateAttribute("NewOrderID"));
                    expNewOrderAttr.Value = "";
                    XmlAttribute expFieldIDAttr = exp.Attributes.Append(doc.CreateAttribute("FieldID"));
                    expFieldIDAttr.Value = orderField.FieldID.ToString();
                    XmlAttribute expFieldValueAttr = exp.Attributes.Append(doc.CreateAttribute("Value"));
                    expFieldValueAttr.Value = orderField.FieldValue.IsNullOrEmpty() ? orderField.DefaultValue : orderField.FieldValue;
                });

                orderSB = doc.OuterXml;
            }

            return orderSB;
        }

        private void RemoveDraftProfile(String loginName, String password, Int32 boid, String customerID, ClearStar clearStar, CreateProfileForCountry profile,
            Int32 tenantID, Int32 bkgOrderID, Int32 bkgOrderPackageSvcGroupId)
        {
            //if (profile.IsNotNull())
            if (profile != null && (((CreateProfileForCountryErrorStatus)profile.Items[AppConsts.NONE]).Code == AppConsts.ZERO))
            {
                ServiceLogger.Info("Removing ClearStar Draft profile with profile number: "
                                    + ((CreateProfileForCountryProfile)profile.Items[1]).Prof_No + " at: " + DateTime.Now.ToString(), CreateOrderServiceLogger);

                clearStar.DeleteClearstarProfileDraftViaGateway(loginName, password,
                     boid, customerID, ((CreateProfileForCountryProfile)profile.Items[1]).Prof_No, tenantID, bkgOrderID, bkgOrderPackageSvcGroupId, CreateOrderServiceLogger);

                clearStar.CancelClearstarProfileDraftViaGateway(loginName, password,
                     boid, customerID, ((CreateProfileForCountryProfile)profile.Items[1]).Prof_No, "ADBCreateOrderService", false, tenantID, bkgOrderID, bkgOrderPackageSvcGroupId, CreateOrderServiceLogger);

                ServiceLogger.Info("Removed ClearStar Draft profile with profile number: "
                                    + ((CreateProfileForCountryProfile)profile.Items[1]).Prof_No + " at: " + DateTime.Now.ToString(), CreateOrderServiceLogger);
            }
        }

        private Boolean SetupSubmitOrderFlag(bool submitFlag, bool specialError, bool uploadType)
        {
            bool submitOrder = false;
            if (submitFlag)
            {
                if (!specialError)
                {
                    submitOrder = uploadType;
                }
            }
            else
            {
                submitOrder = submitFlag;
            }
            return submitOrder;
        }

        private void GetProfileDetailProfileOrder(GetProfileDetailProfile profileDetailProfile, EvUpdateOrderContract evUpdateOrderContract)
        {
            foreach (GetProfileDetailProfileOrder gpdpo in profileDetailProfile.Order)
            {
                //UAT-4377:- Change need here: line item order status checks. 
                // Int32 orderStatusId = Convert.ToInt32(gpdpo.OrderStatus);
                //if (orderStatusId >= AppConsts.THREE || evUpdateOrderContract.VendorProfileStatus == ClearstarProfileStatus.Completed ) //Added in UAT-4377
                //{
                ServiceLogger.Info("Started GetProfileDetailProfileOrder at" + DateTime.Now.ToString() + " for ProfileNo: " + evUpdateOrderContract.BkgOrderVendorProfileID +
                " BkgOrderID: " + evUpdateOrderContract.BkgOrderID + " ExternalVendorOrderID: " + gpdpo.OrderID, UpdateOrderServiceLogger);

                if (evUpdateOrderContract.EvUpdateOrderItemContract.Any(cond => cond.ExternalVendorOrderID == gpdpo.OrderID))
                {
                    EvUpdateOrderItemContract evUpdateOrderItemContractItem = evUpdateOrderContract.EvUpdateOrderItemContract
                                                                              .FirstOrDefault(cond => cond.ExternalVendorOrderID == gpdpo.OrderID);

                    ServiceLogger.Info("ExtVendorBkgOrderLineItemDetailsID: " + evUpdateOrderItemContractItem.ExtVendorBkgOrderLineItemDetailID +
                    "ExternalVendorOrderID: " + gpdpo.OrderID + " found in contract list", UpdateOrderServiceLogger);

                    evUpdateOrderItemContractItem.ResultText = gpdpo.OrderNote;
                    evUpdateOrderItemContractItem.ResultXML = gpdpo.OrderXML;
                    evUpdateOrderItemContractItem.SvcLineItemFlaggedInd = gpdpo.HitFlag == "Y" ? true : false;
                    evUpdateOrderItemContractItem.OrderLineItemResultStatusID = Convert.ToInt32(gpdpo.OrderStatus) + 1;

                    if (Convert.ToInt32(gpdpo.OrderStatus) >= 3)
                    {
                        //update orderitemresult record to complete
                        evUpdateOrderItemContractItem.DateCompleted = DateTime.Now;
                        ServiceLogger.Info("ExternalVendorOrderID: " + gpdpo.OrderID + " completed.", UpdateOrderServiceLogger);
                    }

                    //evUpdateOrderItemContractItem.OrderLineItemResultStatusID = Convert.ToInt32(gpdpo.OrderStatus) + 1;

                    //ServiceLogger.Info("ExtVendorBkgOrderLineItemDetailsID: " + evUpdateOrderItemContractItem.ExtVendorBkgOrderLineItemDetailID +
                    //" ExternalVendorOrderID: " + gpdpo.OrderID + " already Completed with DateCompleted:" + evUpdateOrderItemContractItem.DateCompleted,
                    //UpdateOrderServiceLogger);
                }
                else
                {
                    ServiceLogger.Info("ExternalVendorOrderID: " + gpdpo.OrderID + " does not found in contract list", UpdateOrderServiceLogger);
                }

                ServiceLogger.Info("End GetProfileDetailProfileOrder at " + DateTime.Now.ToString() + " for ProfileNo: " + evUpdateOrderContract.BkgOrderVendorProfileID +
                " BkgOrderID: " + evUpdateOrderContract.BkgOrderID + " ExternalVendorOrderID: " + gpdpo.OrderID, UpdateOrderServiceLogger);
                // }
            }
        }

        /// <summary>
        /// Method to get first or default alias from order line items of a service group.
        /// </summary>
        /// <param name="packageSvcGroup">packageSvcGroup contract</param>
        /// <returns>alias name if exist in order otherwise return empty string</returns>
        private String GetAliasNameForSupplementOrderProfileName(EvCreateOrderSvcGroupContract packageSvcGroup, List<EvOrderProfileAliases> lstOrderProfileAliases
                                                                 , String noMiddleNameDefaultText)
        {
            String alias = String.Empty;
            //implemented changes related to production issue for aliases at the time of supplementOrder[19/07/2016]
            List<String> existingOrderProfileAliases = new List<String>();
            if (!lstOrderProfileAliases.IsNullOrEmpty())
            {
                existingOrderProfileAliases = lstOrderProfileAliases.Select(slct => slct.FullName).ToList();
            }
            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
            List<EvOrderItemContract> lstOrderItems = packageSvcGroup.OrderItems.DeepClone();
            lstOrderItems.Where(x => x.Alias != String.Empty).ForEach(each => { each.Alias = each.Alias.Replace(noMiddleNameDefaultText, ""); });

            //EvOrderItemContract orderItemAliasData = packageSvcGroup.OrderItems.FirstOrDefault(orderItem => orderItem.Alias != String.Empty && !existingOrderProfileAliases.Contains(orderItem.Alias));
            EvOrderItemContract orderItemAliasData = lstOrderItems.FirstOrDefault(orderItem => orderItem.Alias != String.Empty && !existingOrderProfileAliases.Contains(orderItem.Alias));
            if (!orderItemAliasData.IsNullOrEmpty())
            {
                alias = orderItemAliasData.Alias;
            }
            return alias;
        }

        /// <summary>
        /// Method to set the Order line item alias for supplement order or add profile name as alias 
        /// </summary>
        /// <param name="orderItem">Order item contract</param>
        /// <param name="addedAliasInProfileName">added alias in profile name</param>
        /// <param name="evOrderContract">Order contract</param>
        private void SetAliasInOrderItemAttributesForSupplement(EvOrderItemContract orderItem, string addedAliasInProfileName, EvCreateOrderContract evOrderContract)
        {
            if (!addedAliasInProfileName.IsNullOrEmpty())
            {
                EvOrderItemAttributeContract aliasContract = orderItem.EvOrderItemAttributeContracts
                                                                            .FirstOrDefault(cond => cond.FieldLabel.ToLower() == AppConsts.ALIAS.ToLower());
                //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                String orderItemAlias = orderItem.Alias.Replace(evOrderContract.NoMiddleNameDefaultText, "");
                if (orderItemAlias.Contains(addedAliasInProfileName) && !aliasContract.IsNullOrEmpty())
                {
                    aliasContract.FieldValue = String.Empty;
                }

                if (orderItem.Alias.IsNullOrEmpty())
                {
                    //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                    String middleName = evOrderContract.MiddleName.IsNullOrEmpty() ? evOrderContract.NoMiddleNameDefaultText : evOrderContract.MiddleName;
                    orderItem.Alias = evOrderContract.FirstName + " " + middleName + " " + evOrderContract.LastName;
                    if (!aliasContract.IsNullOrEmpty() && aliasContract.FieldValue.IsNullOrEmpty())
                    {
                        aliasContract.FieldValue = orderItem.Alias;
                    }
                }
            }
        }
        #endregion

        #endregion
    }
}
