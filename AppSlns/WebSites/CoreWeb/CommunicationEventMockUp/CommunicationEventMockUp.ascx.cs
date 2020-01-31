using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.Services.Observer;
using Business.Observer;
using Business.RepoManagers;
using System.Collections;
using INTSOF.UI.Contract.Templates;
using System.Collections.Generic;
using INTSOF.Utils;
using Entity;
using System.Configuration;

namespace CoreWeb.CommunicationEventMockUp.Views
{
    public partial class CommunicationEventMockUp : BaseUserControl, ICommunicationEventMockUpView
    {
        #region Private Variables
        private String _viewType;
        private CommunicationEventMockUpPresenter _presenter=new CommunicationEventMockUpPresenter();
        #endregion

        protected override void OnInit(EventArgs e)
        {
            try
            {
               // _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = "Communication Event MockUp";

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
            base.SetPageTitle("Communication Events");
        }

        
        public CommunicationEventMockUpPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	

        protected void btnNotificationOrdeCreationr_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();


            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_MONEY_ORDER.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("OrderNo", mockData.OrderNo);
            a.Add("OrderDate", mockData.OrderDate);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_MONEY_ORDER, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Order created successfully.");
                }
                else
                 {
                    base.ShowSuccessMessage("User is not subscribed to receive this notification.");
                }
            }
    ));
            subject.Notify();


        }

        protected void btnNotificationOrderApproval_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();


            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("OrderNo", mockData.OrderNo);
            a.Add("OrderDate", mockData.OrderDate);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Order approved successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this notification.");
                }
            }
    ));
            subject.Notify();

        }

        protected void btnNotificationOrderCompletion_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();


            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.NOTIFICATION_ORDER_COMPLETION.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("OrderNo", mockData.OrderNo);
            a.Add("OrderDate", mockData.OrderDate);
            a.Add("OrderContent", mockData.OrderContent);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.NOTIFICATION_ORDER_COMPLETION, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Order completed successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this notification.");
                }

            }
    ));
            subject.Notify();

        }

        protected void btnNotificationNewSubscription_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();

            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.NOTIFICATION_NEW_SUBSCRIPTIONS.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("SubscriptionName", mockData.SubscriptionName);
            a.Add("SubscriptionStartDate", mockData.SubscriptionStartDate);
            a.Add("SubscriptionEndDate", mockData.SubscriptionEndDate);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.NOTIFICATION_NEW_SUBSCRIPTIONS, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("New subscription notification sent successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this notification.");
                }

            }
    ));
            subject.Notify();
        }

        protected void btnNotificationExpiredSubscription_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();

            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.NOTIFICATION_EXPIRED_SUBSCRIPTIONS.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("SubscriptionName", mockData.SubscriptionName);
            a.Add("SubscriptionEndDate", mockData.SubscriptionEndDate);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.NOTIFICATION_EXPIRED_SUBSCRIPTIONS, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {

                    base.ShowSuccessMessage("Expired subscription notification sent successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this notification.");
                }
            }
    ));
            subject.Notify();
        }

        protected void btnNotificationRenewableSubscription_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();

            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.NOTIFICATION_RENEWABLE_SUBSCRIPTION.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("SubscriptionName", mockData.SubscriptionName);
            a.Add("SubscriptionStartDate", mockData.SubscriptionStartDate);
            a.Add("SubscriptionEndDate", mockData.SubscriptionEndDate);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.NOTIFICATION_RENEWABLE_SUBSCRIPTION, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Subscription expiring notification sent successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this notification.");
                }
            }
    ));
            subject.Notify();

        }

        protected void btnNotificationProfileChanges_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();

            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.NOTIFICATION_PROFILE_CHANGE.GetStringValue());

            a.Add("UserFullName", mockData.FullName);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.NOTIFICATION_PROFILE_CHANGE, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Profile changes notification sent successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this notification.");
                }
            }
    ));
            subject.Notify();

        }

        protected void btnNotificationAccountStatus_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();

            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.NOTIFICATION_ACCOUNT_STATUS.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("AccountStatus", Convert.ToBoolean(mockData.AccountStatus) ? "Active" : "In-Active");

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.NOTIFICATION_ACCOUNT_STATUS, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Account status notification sent successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this notification.");
                }

            }
    ));
            subject.Notify();

        }

        protected void btnNotificationCreditCard_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();

            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.NOTIFICATION_CREDIT_CARD.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("CreditCardDetails", mockData.CreditCardDetails);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.NOTIFICATION_CREDIT_CARD, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Notifications for credit card sent successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this notification.");
                }

            }));
            subject.Notify();

        }

        protected void btnNotificationMoneyOrder_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();

            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.NOTIFICATION_MONEY_ORDER.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("MoneyOrderDetails", mockData.MoneyOrderDetails);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.NOTIFICATION_MONEY_ORDER, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Notifications for money order sent successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this notification.");
                }
            }));
            subject.Notify();

        }

        protected void btnNotificationMBSpayment_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();

            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.NOTIFICATION_MBS_PAYMENT.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("MBSPaymentDetails", mockData.MBSPaymentDetails);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.NOTIFICATION_MBS_PAYMENT, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Notifications for MBS Payment sent successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this notification.");
                }
            }));
            subject.Notify();

        }

        protected void btnAlertOrderDenied_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();

            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.ALERT_ORDER_DENIED.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("OrderNo", mockData.OrderNo);
            a.Add("OrderStatus", Convert.ToBoolean(mockData.OrderStatus) ? "Accepted" : "Denied");

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.ALERT_ORDER_DENIED, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Order denied alert sent successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this alert.");
                }

            }
    ));
            subject.Notify();

        }

        protected void btnAlertSubscriptionExpire_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();

            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.ALERT_SUBSCRIPTION_EXPIRE.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("SubscriptionName", mockData.SubscriptionName);
            a.Add("SubscriptionEndDate", mockData.SubscriptionEndDate);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.ALERT_SUBSCRIPTION_EXPIRE, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Subscription expiring alert sent successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this alert.");
                }
            }
    ));
            subject.Notify();

        }

        protected void btnAlertProfileChangese_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();

            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.ALERT_PROFILE_CHANGE.GetStringValue());

            a.Add("UserFullName", mockData.FullName);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.ALERT_PROFILE_CHANGE, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Profile changes alert sent successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this alert.");
                }
            }));
            subject.Notify();

        }

        protected void btnAlertAccountStatus_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();

            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.ALERT_ACCOUNT_STATUS.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("Password", mockData.Password);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.ALERT_ACCOUNT_STATUS, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Account Status alert sent successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this alert.");
                }
            }));
            subject.Notify();

        }

        protected void btnAlertApplicantStatus_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();

            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.ALERT_INTERNAL_MESSAGES.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("ComplianceStatus", mockData.ComplianceStatus);
            a.Add("ComplianceName", mockData.ComplianceName);
            a.Add("ComplianceExpiryDate", mockData.ComplianceExpiryDate);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.ALERT_INTERNAL_MESSAGES, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Compliance status alert sent successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this alert.");
                }

            }));
            subject.Notify();

        }

        protected void btnAlertStartEvent_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();

            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.ALERT_START_OF_EVENT.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("EventStartDate", mockData.EventStartDate);
            a.Add("EventEndDate", mockData.EventEndDate);
            a.Add("EventName", mockData.EventName);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.ALERT_START_OF_EVENT, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Event start alert sent successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this alert.");
                }
            }));
            subject.Notify();

        }

        protected void btnAlertEventModifications_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();

            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.ALERT_EVENT_MODIFICATIONS.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("EventNewStartDate", mockData.EventNewStartDate);
            a.Add("EventNewEndDate", mockData.EventNewEndDate);
            a.Add("EventName", mockData.EventName);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.ALERT_EVENT_MODIFICATIONS, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Event modification alert sent successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this alert.");
                }
            }));
            subject.Notify();

        }

        protected void btnReminderSubscriptionExpire_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();

            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.REMINDER_SUBSCRIPTION_EXPIRE.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("SubscriptionName", mockData.SubscriptionName);
            a.Add("SubscriptionEndDate", mockData.SubscriptionEndDate);
            a.Add("ProgramName", mockData.ProgramName);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.REMINDER_SUBSCRIPTION_EXPIRE, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Subscription expiring reminder sent successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this reminder.");
                }
            }));
            subject.Notify();

        }

        protected void btnReminderApplicantComplianceItemExpire_Click(object sender, EventArgs e)
        {
            ISubject subject = new Subject();
            var a = new Dictionary<String, object>();

            CommunicationMockUpData mockData = CommunicationManager.GetMockUpData(CommunicationSubEvents.REMINDER_INTERNAL_MESSAGES.GetStringValue());

            a.Add("UserFullName", mockData.FullName);
            a.Add("ComplianceItemName", mockData.ComplianceItemName);
            a.Add("ComplianceItemExpiryDate", mockData.ComplianceItemExpiryDate);

            var b = SetCommunicationContract(mockData);

            subject.Attach(
            new Observer<object>(subject, new { dictionary = a, details = b }, (theSubject, theElement) =>
            {
                //Send mail
                // frameworkElement.Visibility = subject.A.Equals(subject.SubjectState);
                if (CommunicationManager.SaveMailContentBasisOnSubscription(CommunicationSubEvents.REMINDER_INTERNAL_MESSAGES, ((dynamic)theElement).dictionary, ((dynamic)theElement).details, 0))
                {
                    base.ShowSuccessMessage("Compliance item expiring reminder sent successfully.");
                }
                else
                {
                    base.ShowSuccessMessage("User is not subscribed to receive this reminder.");
                }
            }));
            subject.Notify();

        }

        private CommunicationTemplateContract SetCommunicationContract(CommunicationMockUpData mockData)
        {

            CommunicationTemplateContract communicationTemplateContract = new CommunicationTemplateContract();
            communicationTemplateContract.CurrentUserId = base.CurrentUserId;
            communicationTemplateContract.SenderName = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_NAME]);
            communicationTemplateContract.SenderEmailID = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SENDER_EMAIL_ID]);
            communicationTemplateContract.RecieverName = mockData.UserName;
            communicationTemplateContract.RecieverEmailID = mockData.EmailID;
            communicationTemplateContract.ReceiverOrganizationUserId = mockData.ReceiverOrganizationUserID;
            return communicationTemplateContract;
        }
    }
}

