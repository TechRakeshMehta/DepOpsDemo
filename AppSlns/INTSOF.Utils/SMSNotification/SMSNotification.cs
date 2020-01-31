using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.Utils
{
    public class SMSNotification
    {
        #region Private Variables

        private static AmazonSimpleNotificationServiceClient _SNSClient = null;
        private static AmazonSimpleNotificationServiceConfig _SNSConfig = null;
        private static String _DisplayName = null;

        private static String _AWSSMSSenderID = null;
        private static String _AWSSMSMaxPrice = null;
        private static String _AWSSMSType = null;

        #endregion

        #region Private Class Properties

        private static AmazonSimpleNotificationServiceConfig SNSConfiguration
        {
            get
            {
                if (_SNSConfig == null)
                {
                    _SNSConfig = new AmazonSimpleNotificationServiceConfig();
                    _SNSConfig.RegionEndpoint = RegionEndpoint.USEast1;
                }
                return _SNSConfig;
            }
        }

        private static AmazonSimpleNotificationServiceClient SNSClient
        {
            get
            {
                if (_SNSClient == null)
                {
                    String AWSAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"].ToString();
                    String AWSSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"].ToString();
                    _SNSClient = new AmazonSimpleNotificationServiceClient(AWSAccessKey, AWSSecretKey, SNSConfiguration);
                }
                return _SNSClient;
            }
        }

        private static String DisplayName
        {
            get
            {
                if (_DisplayName == null)
                {
                    _DisplayName = ConfigurationManager.AppSettings["AWSTopicDiplayName"].ToString();
                }
                return _DisplayName;
            }
        }

        private static String AWSSMSSenderID
        {
            get
            {
                if (_AWSSMSSenderID == null && !ConfigurationManager.AppSettings["AWSSMSSenderID"].IsNullOrEmpty())
                    _AWSSMSSenderID = Convert.ToString(ConfigurationManager.AppSettings["AWSSMSSenderID"]);

                return _AWSSMSSenderID;
            }
        }

        private static String AWSSMSMaxPrice
        {
            get
            {
                if (_AWSSMSMaxPrice == null && !ConfigurationManager.AppSettings["AWSSMSMaxPrice"].IsNullOrEmpty())
                    _AWSSMSMaxPrice = Convert.ToString(ConfigurationManager.AppSettings["AWSSMSMaxPrice"]);

                return _AWSSMSMaxPrice;
            }
        }

        private static String AWSSMSType
        {
            get
            {
                if (_AWSSMSType == null && !ConfigurationManager.AppSettings["AWSSMSType"].IsNullOrEmpty())
                    _AWSSMSType = Convert.ToString(ConfigurationManager.AppSettings["AWSSMSType"]);

                return _AWSSMSType;
            }
        }

        #endregion

        #region Constructor
        public SMSNotification()
        {

        }
        #endregion

        #region Methods

        #region Public method

        /// <summary>
        /// Create New Topic
        /// </summary>
        /// <param name="topicName">Name of the Topic to create</param>
        /// <returns>Topic ARN code</returns>
        //public static String CreateTopic(String topicName)
        //{
        //    try
        //    {
        //        //craete the new topic and return topicARN
        //        String topicARN = SNSClient.CreateTopic(new CreateTopicRequest
        //        {
        //            Name = topicName
        //        }).TopicArn;

        //        //using TopicARN set the display Name while sending SMS
        //        SNSClient.SetTopicAttributes(new SetTopicAttributesRequest
        //        {
        //            TopicArn = topicARN,
        //            AttributeName = "DisplayName",
        //            AttributeValue = DisplayName
        //        });

        //        return topicARN;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// create the subscription
        /// and send Subscription SMS.
        /// </summary>
        /// <param name="topicARN">TopicARN (which is fetch by creating topic)</param>
        /// <param name="phoneNumber">Phone Number where subscription need to be subscribed</param>
        /// <returns>SubscriptionARN : status of the subscription</returns>
        //public static String CreateSubscription(String topicARN, String phoneNumber)
        //{
        //    try
        //    {
        //        //subscription message is send and response is stored in 
        //        //subscriptionResponse Variable
        //        var subscriptionResponse = SNSClient.Subscribe(new SubscribeRequest
        //        {
        //            TopicArn = topicARN,
        //            Protocol = "SMS",
        //            Endpoint = FormatPhoneNumber(phoneNumber)
        //        });

        //        //subscriptionARN from subscriptionResponse is returned
        //        return subscriptionResponse.SubscriptionArn;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// this method check's the status of subscription for topic
        /// </summary>
        /// <param name="topicARN">TopicARN code of that user</param>
        /// <returns>subscribed status</returns>
        //public static Boolean IsSubscriptionConfirm(String topicARN)
        //{
        //    try
        //    {
        //        var subscriptionDetails = SNSClient.ListSubscriptionsByTopic(new ListSubscriptionsByTopicRequest
        //        {
        //            TopicArn = topicARN
        //        });
        //        if (subscriptionDetails.IsNotNull() && subscriptionDetails.Subscriptions.IsNotNull() && subscriptionDetails.Subscriptions.Count > 0)
        //        {
        //            if (subscriptionDetails.Subscriptions[0].SubscriptionArn == SMSSubscriptionStatus.PENDING_CONFIRMATION.GetStringValue())
        //            {
        //                return false;
        //            }
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (NotFoundException notFountEX)
        //    {
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// send message to mobile phone
        /// </summary>
        /// <param name="topicARN">Topic ARN code</param>
        /// <param name="Message">Message to be send</param>
        //public static String SendSMS(String topicARN, String Message)
        //{
        //    try
        //    {
        //        PublishRequest request = new PublishRequest();
        //        request.TopicArn = topicARN;
        //        request.Message = Message;
        //        PublishResponse response = SNSClient.Publish(request);

        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static void SendSMS(String phoneNumber, String message)
        {
            try
            {
                PublishRequest request = new PublishRequest();
                request.Message = message;
                request.PhoneNumber = phoneNumber.Length == 10 ? string.Concat("+1", phoneNumber) : phoneNumber;

                //Adding Sender ID - Attribute with publish request
                if (!AWSSMSSenderID.IsNullOrEmpty())
                {
                    MessageAttributeValue senderIDAttribute = new MessageAttributeValue();
                    senderIDAttribute.DataType = "String";
                    senderIDAttribute.StringValue = AWSSMSSenderID;
                    request.MessageAttributes.Add("AWS.SNS.SMS.SenderID", senderIDAttribute);
                }

                //Adding Max Price - Attribute with publish request
                if (!AWSSMSMaxPrice.IsNullOrEmpty())
                {
                    MessageAttributeValue maxPriceAttribute = new MessageAttributeValue();
                    maxPriceAttribute.DataType = "Number";
                    maxPriceAttribute.StringValue = AWSSMSMaxPrice;
                    request.MessageAttributes.Add("AWS.SNS.SMS.MaxPrice", maxPriceAttribute);
                }

                //Adding SMS type - Attribute with publish request
                if (!AWSSMSType.IsNullOrEmpty())
                {
                    MessageAttributeValue smsTypeAttribute = new MessageAttributeValue();
                    smsTypeAttribute.DataType = "String";
                    smsTypeAttribute.StringValue = AWSSMSType;
                    request.MessageAttributes.Add("AWS.SNS.SMS.SMSType", smsTypeAttribute);
                }

                PublishResponse response = SNSClient.Publish(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// In this method SubscriptionARN is get from amazon using TopicARN
        /// </summary>
        /// <param name="topicARN">Topic ARN Code</param>
        /// <returns>SubscriptionARN for that particular Topic</returns>
        //public static String GetSubscriptionARN(String TopicARN)
        //{
        //    try
        //    {
        //        var subscriptionDetails = SNSClient.ListSubscriptionsByTopic(new ListSubscriptionsByTopicRequest
        //        {
        //            TopicArn = TopicARN
        //        });
        //        return subscriptionDetails.Subscriptions[0].SubscriptionArn;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Unsubscription the endpoint(Phone number) from the topic
        /// </summary>
        /// <param name="SubscriptionARN">SubscriptionARN of that endpoint(phone number) which need to be Unsubscribe</param>
        /// <returns>Succesfully unsubscribed or not</returns>
        //public static Boolean Unsubscribe(String SubscriptionARN)
        //{
        //    try
        //    {
        //        var UnsubscriptionResponse = SNSClient.Unsubscribe(new UnsubscribeRequest()
        //        {
        //            SubscriptionArn = SubscriptionARN
        //        });
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Remove the Topic from Amazon server
        /// </summary>
        /// <param name="TopicARN">TopicARN is passed which Topic has to delete</param>
        /// <returns>Succesfully Topic Deleted or not</returns>
        //public static Boolean DeleteTopic(String TopicARN)
        //{
        //    try
        //    {
        //        var UnsubscriptionResponse = SNSClient.DeleteTopic(new DeleteTopicRequest
        //        {
        //            TopicArn = TopicARN
        //        });
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Check from Amazon does Topic Exists
        /// </summary>
        /// <param name="TopicARN"></param>
        /// <returns></returns>
        //public static Boolean IsTopicExist(String TopicARN)
        //{
        //    try
        //    {
        //        var topicDetails = SNSClient.ListSubscriptionsByTopic(new ListSubscriptionsByTopicRequest()
        //        {
        //            TopicArn = TopicARN
        //        });
        //        return true;
        //    }
        //    catch (NotFoundException notFountEX)
        //    {
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        #endregion

        #region Private Method

        /// <summary>
        /// set the format of the phone number as required to send SMS
        /// </summary>
        private static String FormatPhoneNumber(String phoneNumber)
        {
            phoneNumber = "1-" + String.Format("{0}-{1}-{2}", phoneNumber.Substring(0, 3), phoneNumber.Substring(3, 3), phoneNumber.Substring(6));
            return phoneNumber;
        }

        #endregion

        #endregion
    }
}
