using INTSOF.Utils;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects.DataClasses;
using System.ServiceModel;
using System.ServiceModel.Activation;
using QueueInterface = INTSOF.UnityHelper.Interfaces;
//using INTSOF.NewQueues.Interfaces.BusinessFacadeInterface.WorkOrderService;



namespace INTSOF.UnityHelper.Host 
{
    public class QueueServiceHostFactory : ServiceHostFactory
    {
        private readonly IUnityContainer _serviceContainer;

        public QueueServiceHostFactory()
        {

        }

        public QueueServiceHostFactory(String configSectionName)
        {
            _serviceContainer = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection(configSectionName); //QueueContainer
            section.Configure(_serviceContainer);
        }


        //public QueueServiceHostFactory(IConfigurationStore serviceHostConfiguration, String serviceHostConfigSectionName, String serviceContainerName)
        //{
        //    //   _serviceContainer = QueueServiceContainerResolver.Resolve(serviceHostConfiguration, serviceHostConfigSectionName, serviceContainerName);
        //}

        public QueueServiceHostFactory(IUnityContainer serviceContainer)
        {
            if (serviceContainer.IsNull())
                //queueServiceBehaviorLogger.Error ("Queue Container - Incorrect Parameter Value (Null)", new ArgumentNullException ("Service Component Error Source Guid" + _serviceComponentGuid.ToString ()));
                throw new ArgumentNullException("Queue Container - Incorrect Parameter Value (Null)");

            _serviceContainer = serviceContainer;
        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new QueueServiceHost(serviceType, _serviceContainer, baseAddresses);
        }

        #region IMessageQueue's Implementation

        public Boolean SendMessage(String queueName, Int32 intiator, String message, String applicationDatabaseName, Int32 senderId, String subject,out Guid adbMessageID)
        {
            QueueInterface.IMessageQueue obj = _serviceContainer.Resolve<QueueInterface.IMessageQueue>(queueName);
            return obj.SendMessage(intiator, message, applicationDatabaseName, senderId,subject,out adbMessageID);
        }

       

        public Boolean SendMessageInBatch(String queueName, Int32 intiator, String[] messages)
        {
            QueueInterface.IMessageQueue obj = _serviceContainer.Resolve<QueueInterface.IMessageQueue>(queueName);
            return obj.SendMessageInBatch(intiator, messages);
        }

        public Boolean ReplyMesssage(String queueName, Guid messageId, Int32 initiator, String message, String applicationDatabaseName, Int32 senderId, String subject)
        {
            QueueInterface.IMessageQueue obj = _serviceContainer.Resolve<QueueInterface.IMessageQueue>(queueName);
            return obj.ReplyMessage(messageId, initiator, message, applicationDatabaseName, senderId,  subject);
        }

        /// <summary>
        /// Use to get a message by  messageID
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="folderId"></param>
        /// <param name="queueType"></param>
        /// <param name="queueOwnerId"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public EntityObject GetMessage(String queueName, Int32 folderId, Int32 queueType, Int32 queueOwnerId, Guid messageId)
        {
            QueueInterface.IMessageQueue obj = _serviceContainer.Resolve<QueueInterface.IMessageQueue>(queueName);
            return obj.GetMessage(folderId, queueType, queueOwnerId, messageId);
        }

        public IEnumerable<EntityObject> GetQueue(String queueName, Int32 folderId, String folderCode, Int32 queueType, Int32 queueOwnerId, Int32 userGroup, Int32 communicationTypeId)
        {
            QueueInterface.IMessageQueue obj = _serviceContainer.Resolve<QueueInterface.IMessageQueue>(queueName);
            return obj.GetQueue(folderId, folderCode, queueType, queueOwnerId, userGroup, communicationTypeId);
        }

        public IEnumerable<EntityObject> GetQueue(String queueName, Int32 queueOwnerId, Int32 communicationTypeId)
        {
            QueueInterface.IMessageQueue obj = _serviceContainer.Resolve<QueueInterface.IMessageQueue>(queueName);
            return obj.GetQueue(queueOwnerId,communicationTypeId);
        }

        public IEnumerable<EntityObject> GetUpdateQueue(String queueName, Int32 folderId, Int32 queueType, Int32 queueOwnerId, DateTime lastDateTime)
        {
            QueueInterface.IMessageQueue obj = _serviceContainer.Resolve<QueueInterface.IMessageQueue>(queueName);
            return obj.GetUpdateQueue(folderId, queueType, queueOwnerId, lastDateTime);
        }

        #endregion

        public IEnumerable<EntityObject> GetRecentMessages(String queueName, String folderCode, Int32 queueOwnerId, Int32 communicationTypeId)
        {
            QueueInterface.IMessageQueue obj = _serviceContainer.Resolve<QueueInterface.IMessageQueue>(queueName);
            return obj.GetRecentMessages(folderCode, queueOwnerId, communicationTypeId);
        }
    }
}
