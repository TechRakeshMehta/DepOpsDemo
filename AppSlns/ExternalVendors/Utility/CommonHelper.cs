using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using Entity.ExternalVendorContracts;
using INTSOF.Utils;

namespace ExternalVendors.Utility
{
    public static class CommonHelper
    {
        public static VendorResponse SetVendorResponseInContract(String vendorCode, String vendorMessage, String vendorMessageType)
        {
            VendorResponse profileVendorResponse = new VendorResponse();
            profileVendorResponse.ResponseCode = vendorCode;
            profileVendorResponse.ResponseMessage = vendorMessage;
            profileVendorResponse.ResponseType = vendorMessageType;
            return profileVendorResponse;
        }

        public static Boolean AreAllProfilesComplete(EvUpdateOrderContract evUpdateOrderContract,
                                                    IEnumerable<usp_GetOrdersToBeUpdatedVendorData_Result> processedOrderItemList)
        {
            //4-Completed
            if (processedOrderItemList.Count() > AppConsts.NONE)
            {
                if (evUpdateOrderContract.EvUpdateOrderItemContract.Any(cond => cond.OrderLineItemResultStatusID != 4) ||
                    processedOrderItemList.Any(cond => cond.OrderLineItemResultStatusID != 4))
                {
                    return false;
                }
                return true;
            }
            else
            {
                if (evUpdateOrderContract.EvUpdateOrderItemContract.Any(cond => cond.OrderLineItemResultStatusID != 4))
                {
                    return false;
                }
                return true;
            }
        }

        public static Boolean AreAllProfilesCompleteWithoutFlagged(EvUpdateOrderContract evUpdateOrderContract,
                                                                   IEnumerable<usp_GetOrdersToBeUpdatedVendorData_Result> processedOrderItemList)
        {
            if (processedOrderItemList.Count() > AppConsts.NONE)
            {
                if ((evUpdateOrderContract.EvUpdateOrderItemContract.Any(cond => cond.SvcLineItemFlaggedInd)
                    && processedOrderItemList.Any(cond => cond.OrderLineItemFlaggedInd.HasValue ? (cond.OrderLineItemFlaggedInd.Value) : false)))
                {
                    return false;
                }
                return true;
                //return (!(evUpdateOrderContract.EvUpdateOrderItemContract.Any(cond => cond.SvcLineItemFlaggedInd)
                //    && processedOrderItemList.Any(cond => cond.OrderLineItemFlaggedInd.HasValue ? (cond.OrderLineItemFlaggedInd.Value) : false)));
            }
            else
            {
                if (evUpdateOrderContract.EvUpdateOrderItemContract.Any(cond => cond.SvcLineItemFlaggedInd))
                {
                    return false;
                }
                return true;
            }
        }


        #region UAT-844

        /// <summary>
        /// Method that check is all service line item of a service group is completed or not.
        /// </summary>
        /// <param name="evUpdateOrderContract"></param>
        /// <param name="processedOrderItemList"></param>
        /// <param name="opsgID"></param>
        /// <returns></returns>
        public static Boolean IsAllSvcGroupLineItemsCompleted(EvUpdateOrderContract evUpdateOrderContract, Int32 externalVendorBkgOrderDetailID, Int32 tenantID)
        {

            //Get other Service group line items from DB other than current contract line items
            //Check if other svc groups ALL line items are completed and Contract line items are Completed-> 
            //if yes, then return TRUE
            //if NO, then return FALSE

            Boolean areOtherItemsPending = ExternalVendorOrderManager.CheckPendingSvcGroupItems(tenantID, externalVendorBkgOrderDetailID);

            if (areOtherItemsPending)
            {
                return false;
            }
            else
            {
                if (evUpdateOrderContract.EvUpdateOrderItemContract.All(cond => cond.DateCompleted.HasValue))
                {
                    return true;
                }
                return false;
            }
        }
        #endregion

        #region UAT-1357
        /// <summary>
        /// Method to update new order status in 'UpdateOrderContract' for each service group completion and depends on first Review and Second Review trigger of service group.
        /// </summary>
        /// <param name="profileDetailProfile"></param>
        /// <param name="evUpdateOrderContract"></param>
        //private void UpdateServiceGroupStatus(Boolean clearStarFlaggedInd, EvUpdateOrderContract evUpdateOrderContract, IEnumerable<usp_GetOrdersToBeUpdatedVendorData_Result> processedOrderItemList)
        public static void UpdateServiceGroupStatus(Boolean clearStarFlaggedInd, EvUpdateOrderContract evUpdateOrderContract, Int32 tenantID, String UpdateOrderServiceLogger)
        {
            //evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.Select(slct => slct.BkgOrderPackageServiceGroupID).ForEach(opsgID =>
            //{

            //UAT-1244
            //EvUpdateOrderPackageSvcGroup serviceGroupToUpdate = evUpdateOrderContract.EvUpdateOrderPackageSvcGroup
            //                                                                               .FirstOrDefault(cnd => cnd.BkgOrderPackageServiceGroupID == opsgID);

            ServiceLogger.Info("SetBkgOrderStatus: Started processing for BkgSvcGroupID:" + evUpdateOrderContract.EvUpdateOrderPackageSvcGroup.BkgOrderPackageServiceGroupID
                                                                                           , UpdateOrderServiceLogger);
            EvUpdateOrderPackageSvcGroup serviceGroupToUpdate = evUpdateOrderContract.EvUpdateOrderPackageSvcGroup;
            Boolean _firstReviewTrigger = serviceGroupToUpdate.FirstReviewTrigger;
            Boolean _secondReviewTrigger = serviceGroupToUpdate.SecondReviewTrigger;
            Boolean _isAutoReviewComplete = evUpdateOrderContract.IsAutoReviewComplete;
            Boolean existingSvcGrpFlaggedInd = serviceGroupToUpdate.ServiceGroupFlaggedInd;

            //var isAllSvcGroupLineItemsCompleted = evUpdateOrderContract.EvUpdateOrderItemContract.Where(cond => cond.BkgOrderPackageServiceGroupID == opsgID)
            //                                                                                      .All(cond => cond.DateCompleted.HasValue);

            //UAT-1244
            var isAllSvcGroupLineItemsCompleted = CommonHelper.IsAllSvcGroupLineItemsCompleted(evUpdateOrderContract
                                                                                               , evUpdateOrderContract.ExternalVendorBkgOrderDetailID, tenantID);

            //var isAllSvcGroupLineItemsCompleted = evUpdateOrderContract.EvUpdateOrderItemContract.All(cond => cond.DateCompleted.HasValue);

            //check if All service line item of a service group is completed then update the service group review status.
            //if (isAllSvcGroupLineItemsCompleted) //Commented in UAT-4377
            if (isAllSvcGroupLineItemsCompleted && !evUpdateOrderContract.VendorProfileStatus.IsNullOrEmpty()
                && evUpdateOrderContract.VendorProfileStatus == ClearstarProfileStatus.Completed)//Added in UAT-4377
            {

                //UAT-1244
                //Boolean isSvcGroupFlaggedInd = evUpdateOrderContract.EvUpdateOrderItemContract.Where(cond => cond.BkgOrderPackageServiceGroupID == opsgID)
                //                                                                                 .Any(cond => cond.SvcLineItemFlaggedInd);

                Boolean isSvcGroupFlaggedInd = evUpdateOrderContract.EvUpdateOrderItemContract.Any(cond => cond.SvcLineItemFlaggedInd);

                serviceGroupToUpdate.ServiceGroupFlaggedInd = (existingSvcGrpFlaggedInd) ? existingSvcGrpFlaggedInd : isSvcGroupFlaggedInd;
                //if firstreview trigger is true for service group and service group is In-Progress and supplemental type code is any or 
                //flagged and clear star flaggedInd is true then set service group review status to First Review.
                if (_firstReviewTrigger
                     && serviceGroupToUpdate.ServiceGroupStatusCode == BkgSvcGrpStatusType.IN_PROGRESS.GetStringValue()
                     && !String.IsNullOrEmpty(serviceGroupToUpdate.PackageSupplementalTypeCode)
                     && serviceGroupToUpdate.ServiceGroupPreviousReviewStatusCode == BkgSvcGrpReviewStatusType.NEW.GetStringValue()
                     && (serviceGroupToUpdate.PackageSupplementalTypeCode == BkgPackageSupplementalType.ANY.GetStringValue()
                       || (serviceGroupToUpdate.PackageSupplementalTypeCode == BkgPackageSupplementalType.NONE.GetStringValue()
                       )))
                {
                    serviceGroupToUpdate.ServiceGroupNewReviewStatusCode = BkgSvcGrpReviewStatusType.FIRST_REVIEW.GetStringValue();
                }
                //if firstreview trigger is true for service group and service group is In-Progress and supplemental type code is 
                //flagged and clear star flaggedInd is false then set service group review status to AutoReview Completed.
                else if (_firstReviewTrigger
                          && serviceGroupToUpdate.ServiceGroupStatusCode == BkgSvcGrpStatusType.IN_PROGRESS.GetStringValue()
                          && serviceGroupToUpdate.ServiceGroupPreviousReviewStatusCode == BkgSvcGrpReviewStatusType.NEW.GetStringValue()
                          && !String.IsNullOrEmpty(serviceGroupToUpdate.PackageSupplementalTypeCode)
                          && (serviceGroupToUpdate.PackageSupplementalTypeCode == BkgPackageSupplementalType.FLAGGED.GetStringValue()
                          ))
                {
                    if (serviceGroupToUpdate.ServiceGroupFlaggedInd)
                    {
                        serviceGroupToUpdate.ServiceGroupNewReviewStatusCode = BkgSvcGrpReviewStatusType.FIRST_REVIEW.GetStringValue();
                    }
                    else
                    {
                        serviceGroupToUpdate.ServiceGroupNewReviewStatusCode = BkgSvcGrpReviewStatusType.AUTO_REVIEW_COMPLETED.GetStringValue();
                    }
                }
                //If first review for service group is completed and second review trigger is needed and service group status is In-Progress then service group review status 
                // set to Auto Review Completed or Second Review.
                else if (_secondReviewTrigger && serviceGroupToUpdate.ServiceGroupStatusCode == BkgSvcGrpStatusType.IN_PROGRESS.GetStringValue()
                         && serviceGroupToUpdate.ServiceGroupPreviousReviewStatusCode == BkgSvcGrpReviewStatusType.FIRST_REVIEW_COMPLETED.GetStringValue())
                {
                    //If isAutoReviewComplete is set to true and Clear Star FlaggedInd is false then service group review status set to Auto Review Completed 
                    //else Second Review.
                    if (_isAutoReviewComplete && !(serviceGroupToUpdate.ServiceGroupFlaggedInd))
                    {
                        serviceGroupToUpdate.ServiceGroupNewReviewStatusCode = BkgSvcGrpReviewStatusType.AUTO_REVIEW_COMPLETED.GetStringValue();
                    }
                    else
                    {
                        serviceGroupToUpdate.ServiceGroupNewReviewStatusCode = BkgSvcGrpReviewStatusType.SECOND_REVIEW.GetStringValue();
                    }
                }
                //If first review for service group is completed and second review trigger is false and service group status is In-Progress then service group review status 
                // set to Second Review.
                //else if (!_secondReviewTrigger && serviceGroupToUpdate.ServiceGroupStatusCode == BkgSvcGrpStatusType.IN_PROGRESS.GetStringValue()
                //         && serviceGroupToUpdate.ServiceGroupPreviousReviewStatusCode == BkgSvcGrpReviewStatusType.FIRST_REVIEW_COMPLETED.GetStringValue())
                //{
                //    serviceGroupToUpdate.ServiceGroupNewReviewStatusCode = BkgSvcGrpReviewStatusType.SECOND_REVIEW.GetStringValue();
                //}
                //If First Review is not required then Service group review Status set to Auto Review Completed.s
                else if (!_firstReviewTrigger && serviceGroupToUpdate.ServiceGroupStatusCode == BkgSvcGrpStatusType.IN_PROGRESS.GetStringValue())
                {
                    serviceGroupToUpdate.ServiceGroupNewReviewStatusCode = BkgSvcGrpReviewStatusType.AUTO_REVIEW_COMPLETED.GetStringValue();
                }
            }
            ServiceLogger.Info("SetBkgOrderStatus: End foreach loop for BkgSvcGroupID:" + DateTime.Now.ToString(), UpdateOrderServiceLogger);
        }
        #endregion


    }
}
