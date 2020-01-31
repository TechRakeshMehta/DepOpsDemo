using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using Entity;
using DAL.Interfaces;

namespace DAL.Repository
{
    public class SMSNotificationRepository : BaseRepository, ISMSNotificationRepository
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private SysXAppDBEntities _dbNavigation;
        private List<Int32> featureIdsToBeDeleted = new List<Int32>();

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public SMSNotificationRepository()
        {
            _dbNavigation = base.Context;
        }

        #endregion

        /// <summary>
        /// Method that return SMS notification data of applicant.
        /// </summary>
        /// <param name="organizationUserID">Organization UserId</param>
        /// <returns>ApplicantSMSSubsciption</returns>
        OrganisationUserTextMessageSetting ISMSNotificationRepository.GetSMSDataByApplicantId(Int32 organizationUserID)
        {
            return _dbNavigation.OrganisationUserTextMessageSettings.FirstOrDefault(cond => cond.OUTMS_OrganisationUserID == organizationUserID && !cond.OUTMS_IsDeleted);
        }

        /// <summary>
        /// Method to save/update SMS notification data on Amazon as well as in database.
        /// </summary>
        /// <param name="applicantID">Organization UserId</param>
        /// <param name="phoneNumber">SMS receiver phone number</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn UserId</param>
        /// <param name="receiveTextNotification">ReceiveTextNotification</param>
        /// <returns>true/false</returns>
        Boolean ISMSNotificationRepository.SaveUpdateSMSData(Int32 applicantID, String phoneNumber, Int32 currentLoggedInUserId, Boolean receiveTextNotification)
        {

            OrganisationUserTextMessageSetting objTextMessageSetting = _dbNavigation.OrganisationUserTextMessageSettings
                                                                            .Where(cond => cond.OUTMS_OrganisationUserID == applicantID
                                                                                    && !cond.OUTMS_IsDeleted)
                                                                             .FirstOrDefault();

            if (objTextMessageSetting.IsNullOrEmpty())
            {
                //Save
                OrganisationUserTextMessageSetting obj = new OrganisationUserTextMessageSetting();
                obj.OUTMS_OrganisationUserID = applicantID;
                obj.OUTMS_MobileNumber = phoneNumber;
                obj.OUTMS_ReceiveTextNotification = receiveTextNotification;
                obj.OUTMS_CreatedBy = currentLoggedInUserId;
                obj.OUTMS_CreatedOn = DateTime.Now;
                obj.OUTMS_IsDeleted = false;
                _dbNavigation.OrganisationUserTextMessageSettings.AddObject(obj);
            }
            else
            {
                //Update
                objTextMessageSetting.OUTMS_MobileNumber = phoneNumber;
                objTextMessageSetting.OUTMS_ReceiveTextNotification = receiveTextNotification;
                objTextMessageSetting.OUTMS_ModifiedBy = currentLoggedInUserId;
                objTextMessageSetting.OUTMS_ModifiedOn = DateTime.Now;
            }

            if (_dbNavigation.SaveChanges() > 0)
                return true;

            return false;
        }

        /// <summary>
        /// Save current context changes.
        /// </summary>
        /// <returns>true/false</returns>
        Boolean ISMSNotificationRepository.UpdateChanges()
        {
            if (_dbNavigation.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Check that is entered phone number is already subscribed or not.
        /// </summary>
        /// <param name="phoneNumber">SMS receiver phone number</param>
        /// <param name="applicantId">Organization UserId</param>
        /// <returns>true/false</returns>
        Boolean ISMSNotificationRepository.IsPhoneNumberExist(String phoneNumber, Int32 applicantId)
        {
            Boolean isPhoneNumberExist = false;

            OrganisationUserTextMessageSetting objTextSetting = _dbNavigation.OrganisationUserTextMessageSettings.FirstOrDefault(cond =>
                                                                         cond.OUTMS_OrganisationUserID == applicantId
                                                                         && cond.OUTMS_MobileNumber == phoneNumber
                                                                         && !cond.OUTMS_IsDeleted);
            if (!objTextSetting.IsNullOrEmpty())
            {
                isPhoneNumberExist = true;
            }

            return isPhoneNumberExist;
        }

        /// <summary>
        /// Method to delete user SMS subscription detail.
        /// </summary>
        /// <param name="applicantID">Organization UserId</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn UserId</param>
        /// <returns>true/false</returns>
        Boolean ISMSNotificationRepository.DeleteUserSMSSubscriptionDetail(Int32 applicantId, Int32 currentLoggedInUserId)
        {
            OrganisationUserTextMessageSetting organisationUserTextMessageSetting = _dbNavigation.OrganisationUserTextMessageSettings
                                                                    .FirstOrDefault(cond =>
                                                                      cond.OUTMS_OrganisationUserID == applicantId
                                                                      && !cond.OUTMS_IsDeleted);
            if (!organisationUserTextMessageSetting.IsNullOrEmpty())
            {
                organisationUserTextMessageSetting.OUTMS_IsDeleted = true;
                organisationUserTextMessageSetting.OUTMS_ModifiedBy = currentLoggedInUserId;
                organisationUserTextMessageSetting.OUTMS_ModifiedOn = DateTime.Now;

                if (_dbNavigation.SaveChanges() > 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Check applicant subscribe for sms notification or not.
        /// </summary>
        /// <param name="applicantID">Organization UserId</param>
        /// <returns>true/false</returns>
        Boolean ISMSNotificationRepository.IsSubscriptionActiveForSMS(Int32 applicantId)
        {
            Boolean isSubscribForSMS = false;

            OrganisationUserTextMessageSetting objTextMessageSetting = _dbNavigation.OrganisationUserTextMessageSettings
                                                        .FirstOrDefault(cond => cond.OUTMS_OrganisationUserID == applicantId
                                                                        && !cond.OUTMS_IsDeleted);

            if (!objTextMessageSetting.IsNullOrEmpty() && objTextMessageSetting.OUTMS_ReceiveTextNotification)
                isSubscribForSMS = true;

            return isSubscribForSMS;
        }

        /// <summary>
        /// Check applicant confirmed subscription or not.
        /// </summary>
        /// <param name="applicantID">Organization UserId</param>
        /// <returns>true/false</returns>
        Boolean ISMSNotificationRepository.IsSubscriptionConfirmedForSMS(Int32 applicantId)
        {
            Boolean isConfirmedSubscribForSMS = false;

            OrganisationUserTextMessageSetting objTextMessageSetting = _dbNavigation.OrganisationUserTextMessageSettings
                                                        .FirstOrDefault(cond => cond.OUTMS_OrganisationUserID == applicantId
                                                                        && !cond.OUTMS_IsDeleted);

            if (!objTextMessageSetting.IsNullOrEmpty() && objTextMessageSetting.OUTMS_ReceiveTextNotification)
                isConfirmedSubscribForSMS = true;

            return isConfirmedSubscribForSMS;
        }

        //private void CreateTopicAndSubscriptionOnAmazon(String topicARN, String phoneNumber, String topicName, out String newTopicARN, out String newSubscriptionARN)
        //{
        //    newTopicARN = topicARN;
        //    newSubscriptionARN = string.Empty;

        //    if (!topicARN.IsNullOrEmpty() && SMSNotification.IsTopicExist(newTopicARN))
        //    {
        //        newSubscriptionARN = SMSNotification.CreateSubscription(newTopicARN, phoneNumber);
        //    }
        //    else
        //    {
        //        newTopicARN = SMSNotification.CreateTopic(topicName);
        //        if (!newTopicARN.IsNullOrEmpty())
        //        {
        //            newSubscriptionARN = SMSNotification.CreateSubscription(newTopicARN, phoneNumber);
        //        }
        //    }
        //}
    }
}
