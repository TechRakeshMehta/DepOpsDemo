using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DAL.Interfaces
{
    public interface ISMSNotificationRepository
    {
        /// <summary>
        /// Method to save/update SMS notification data on Amazon as well as in database.
        /// </summary>
        /// <param name="applicantID">Organization UserId</param>
        /// <param name="phoneNumber">SMS receiver phone number</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn UserId</param>
        /// <param name="receiveTextNotification">ReceiveTextNotification</param>
        /// <returns>true/false</returns>
        Boolean SaveUpdateSMSData(Int32 applicantID, String phoneNumber, Int32 currentLoggedInUserId, Boolean receiveTextNotification);
        /// <summary>
        /// Method that return SMS notification data of applicant.
        /// </summary>
        /// <param name="organizationUserID">Organization UserId</param>
        /// <returns>ApplicantSMSSubsciption</returns>
        OrganisationUserTextMessageSetting GetSMSDataByApplicantId(Int32 organizationUserID);
        /// <summary>
        /// Save current context changes.
        /// </summary>
        /// <returns>true/false</returns>
        Boolean UpdateChanges();
        /// <summary>
        /// Check that is entered phone number is already subscribed or not.
        /// </summary>
        /// <param name="phoneNumber">SMS receiver phone number</param>
        /// <param name="applicantId">Organization UserId</param>
        /// <returns>true/false</returns>
        Boolean IsPhoneNumberExist(String phoneNumber, Int32 applicantId);
        /// <summary>
        /// Method to delete user SMS subscription detail.
        /// </summary>
        /// <param name="applicantID">Organization UserId</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn UserId</param>
        /// <returns>true/false</returns>
        Boolean DeleteUserSMSSubscriptionDetail(Int32 applicantId, Int32 currentLoggedInUserId);
        /// <summary>
        /// Check applicant subscribe for sms notification or not.
        /// </summary>
        /// <param name="applicantID">Organization UserId</param>
        /// <returns>true/false</returns>
        Boolean IsSubscriptionActiveForSMS(Int32 applicantID);
        /// <summary>
        /// Check applicant confirmed subscription or not.
        /// </summary>
        /// <param name="applicantID">Organization UserId</param>
        /// <returns>true/false</returns>
        Boolean IsSubscriptionConfirmedForSMS(Int32 applicantID);
    }
}
