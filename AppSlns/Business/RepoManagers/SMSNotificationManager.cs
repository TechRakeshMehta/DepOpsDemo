using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.Utils;

namespace Business.RepoManagers
{
    public static class SMSNotificationManager
    {
        /// <summary>
        /// Method that return SMS notification data of applicant.
        /// </summary>
        /// <param name="organizationUserID">Organization UserId</param>
        /// <returns>ApplicantSMSSubsciption</returns>
        public static OrganisationUserTextMessageSetting GetSMSDataByApplicantId(Int32 organizationUserID)
        {
            try
            {
                return BALUtils.GetSMSNotificationRepoInstance().GetSMSDataByApplicantId(organizationUserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to save/update SMS notification data on Amazon as well as in database.
        /// </summary>
        /// <param name="applicantID">Organization UserId</param>
        /// <param name="phoneNumber">SMS receiver phone number</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn UserId</param>
        /// <param name="receiveTextNotification">ReceiveTextNotification</param>
        /// <returns>true/false</returns>
        public static Boolean SaveUpdateSMSData(Int32 applicantID, String phoneNumber, Int32 currentLoggedInUserId, Boolean receiveTextNotification)
        {
            try
            {
                //if (!receiveTextNotification || !IsPhoneNumberExist(phoneNumber, applicantID))
                //{
                return BALUtils.GetSMSNotificationRepoInstance().SaveUpdateSMSData(applicantID, phoneNumber, currentLoggedInUserId, receiveTextNotification);
                //}
                //return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Check that is entered phone number is already subscribed or not.
        /// </summary>
        /// <param name="phoneNumber">SMS receiver phone number</param>
        /// <param name="applicantId">Organization UserId</param>
        /// <returns>true/false</returns>
        public static Boolean IsPhoneNumberExist(String phoneNumber, Int32 applicantId)
        {
            try
            {
                return BALUtils.GetSMSNotificationRepoInstance().IsPhoneNumberExist(phoneNumber, applicantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Convert SMS notification data in entity object.
        /// </summary>
        /// <param name="applicantID">Organization UserId</param>
        /// <param name="phoneNumber">SMS receiver phone number</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn UserId</param>
        /// <returns>ApplicantSMSSubsciption</returns>
        //private static ApplicantSMSSubsciption GetSMSDataObject(Int32 applicantID, String phoneNumber, Int32 currentLoggedInUserId)
        //{
        //    try
        //    {
        //        ApplicantSMSSubsciption appSMSSubscription = new ApplicantSMSSubsciption();
        //        //appSMSSubscription.ASSB_TopicARN = topicARN;
        //        //appSMSSubscription.ASSB_TopicName = topicName;
        //        appSMSSubscription.ASSB_OrganisationUserId = applicantID;
        //        appSMSSubscription.ASSB_IsDeleted = false;
        //        appSMSSubscription.ASSB_CreatedById = currentLoggedInUserId;
        //        appSMSSubscription.ASSB_CreatedOn = DateTime.Now;

        //        ApplicantSMSSubsciptionDetail appSMSSubDetail = new ApplicantSMSSubsciptionDetail();
        //        // appSMSSubDetail.ASSD_SubscriptionARN = subscriptionARN;
        //        appSMSSubDetail.ASSD_MobileNo = phoneNumber;
        //        appSMSSubDetail.ASSD_IsSubscriptionConfirm = false;
        //        appSMSSubDetail.ASSD_CreatedById = currentLoggedInUserId;
        //        appSMSSubDetail.ASSD_CreatedOn = DateTime.Now;

        //        appSMSSubscription.ApplicantSMSSubsciptionDetails.Add(appSMSSubDetail);
        //        return appSMSSubscription;
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        /// <summary>
        /// Save current context changes.
        /// </summary>
        /// <returns>true/false</returns>
        public static Boolean UpdateChanges()
        {
            try
            {
                return BALUtils.GetSMSNotificationRepoInstance().UpdateChanges();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to delete user SMS subscription detail.
        /// </summary>
        /// <param name="applicantID">Organization UserId</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn UserId</param>
        /// <returns>true/false</returns>
        public static Boolean DeleteUserSMSSubscriptionDetail(Int32 applicantID, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetSMSNotificationRepoInstance().DeleteUserSMSSubscriptionDetail(applicantID, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Check applicant subscribe for sms notification or not.
        /// </summary>
        /// <param name="applicantID">Organization UserId</param>
        /// <returns>true/false</returns>
        public static Boolean IsSubscriptionActiveForSMS(Int32 applicantID)
        {
            try
            {
                return BALUtils.GetSMSNotificationRepoInstance().IsSubscriptionActiveForSMS(applicantID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Check applicant confirmed subscription or not.
        /// </summary>
        /// <param name="applicantID">Organization UserId</param>
        /// <returns>true/false</returns>
        public static Boolean IsSubscriptionConfirmedForSMS(Int32 applicantID)
        {
            try
            {
                return BALUtils.GetSMSNotificationRepoInstance().IsSubscriptionConfirmedForSMS(applicantID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to update subscription Details(Subscription ARN and Subscription Confirmation status)
        /// </summary>
        /// <param name="applicantSMSSub">ApplicantSMSSubsciption Object</param>
        /// <param name="isSubscriptionConfirmed">Status of confirmation</param>
        /// <param name="subscriptionARN">Subscription ARN</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn UserId</param>
        /// <returns>true/false</returns>
        //public static Boolean UpdateSubscriptionDetail(ApplicantSMSSubsciption applicantSMSSub, Boolean isSubscriptionConfirmed, String subscriptionARN, Int32 currentLoggedInUserId)
        //{
        //    try
        //    {
        //        if (applicantSMSSub.IsNotNull())
        //        {
        //            ApplicantSMSSubsciptionDetail appSMSSubDetail = applicantSMSSub.ApplicantSMSSubsciptionDetails.FirstOrDefault(cond => !cond.ASSD_IsDeleted);
        //            appSMSSubDetail.ASSD_SubscriptionARN = subscriptionARN;
        //            appSMSSubDetail.ASSD_IsSubscriptionConfirm = isSubscriptionConfirmed;
        //            appSMSSubDetail.ASSD_ModifiedById = currentLoggedInUserId;
        //            appSMSSubDetail.ASSD_ModifiedOn = DateTime.Now;
        //            return UpdateChanges();
        //        }
        //        return false;
        //    }
        //    catch (SysXException ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //        throw (new SysXException(ex.Message, ex));
        //    }
        //}

        /// <summary>
        /// This method get the status of subscription from amazon 
        /// and update the database for that subscription details
        /// </summary>
        /// <param name="applicantID"></param>
        /// <param name="currentLoggedInUserID"></param>
        /// <returns></returns>
        //public static Boolean UpdateSubscriptionStatusFromAmazon(Int32 applicantID, Int32 currentLoggedInUserID)
        //{
        //try
        //{
        //    ApplicantSMSSubsciption appSMSSubscription = GetSMSDataByApplicantId(applicantID);
        //    if (!appSMSSubscription.IsNullOrEmpty())
        //    {
        //        ApplicantSMSSubsciptionDetail appSMSSubDetails = appSMSSubscription.ApplicantSMSSubsciptionDetails.FirstOrDefault(cond => !cond.ASSD_IsDeleted);
        //        if (appSMSSubDetails.IsNotNull())
        //        {
        //            if (SMSNotification.IsTopicExist(appSMSSubscription.ASSB_TopicARN))
        //            {
        //                appSMSSubDetails.ASSD_IsSubscriptionConfirm = SMSNotification.IsSubscriptionConfirm(appSMSSubscription.ASSB_TopicARN);
        //                if (appSMSSubDetails.ASSD_IsSubscriptionConfirm)
        //                {
        //                    appSMSSubDetails.ASSD_SubscriptionARN = SMSNotification.GetSubscriptionARN(appSMSSubscription.ASSB_TopicARN);
        //                }
        //                else
        //                {
        //                    appSMSSubDetails.ASSD_SubscriptionARN = SMSSubscriptionStatus.PENDING_CONFIRMATION.GetStringValue();
        //                }
        //            }
        //            else
        //            {
        //                appSMSSubDetails.ASSD_IsSubscriptionConfirm = false;
        //                appSMSSubDetails.ASSD_SubscriptionARN = SMSSubscriptionStatus.PENDING_CONFIRMATION.GetStringValue();
        //            }
        //            UpdateSubscriptionDetail(appSMSSubscription, appSMSSubDetails.ASSD_IsSubscriptionConfirm, appSMSSubDetails.ASSD_SubscriptionARN, currentLoggedInUserID);
        //            return appSMSSubDetails.ASSD_IsSubscriptionConfirm;
        //        }
        //    }
        //    return false;
        //}
        //catch (SysXException ex)
        //{
        //    BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //    throw ex;
        //}
        //catch (Exception ex)
        //{
        //    BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
        //    throw (new SysXException(ex.Message, ex));
        //}
        //}
    }
}
