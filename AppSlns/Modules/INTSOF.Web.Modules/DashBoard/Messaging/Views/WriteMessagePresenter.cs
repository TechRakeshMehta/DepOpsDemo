using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using INTSOF.Utils;
using System.Linq;
using System.Configuration;
using INTSOF.UI.Contract.Messaging;

namespace CoreWeb.Messaging.Views
{
    public class WriteMessagePresenter : Presenter<IWriteMessageView>
    {
        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IMessagingController _controller;
        // public ReplyPresenter([CreateNew] IMessagingController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        // TODO: Handle other view events and set state in the view

        public void GetTenantTypes()
        {
            Dictionary<Int32, String> tenantTypes = new Dictionary<Int32, String>();
            {
                switch ((TenantTypeEnum)View.QueueType)
                {
                    case TenantTypeEnum.Client:
                        tenantTypes.Clear();
                        tenantTypes.Add(Convert.ToInt32(TenantTypeEnum.Company), "Company");
                        View.TenantTypes = tenantTypes;
                        break;

                    case TenantTypeEnum.Company:
                        tenantTypes.Clear();
                        tenantTypes.Add(0, "--SELECT--");
                        tenantTypes.Add(Convert.ToInt32(TenantTypeEnum.Client), "Client");
                        tenantTypes.Add(Convert.ToInt32(TenantTypeEnum.Company), "Company");
                        tenantTypes.Add(Convert.ToInt32(TenantTypeEnum.Supplier), "Supplier");
                        View.TenantTypes = tenantTypes;
                        break;

                    case TenantTypeEnum.Supplier:
                        tenantTypes.Clear();
                        tenantTypes.Add(Convert.ToInt32(TenantTypeEnum.Company), "Company");
                        View.TenantTypes = tenantTypes;
                        break;
                }
            }
        }


        public List<MessagingRulesContract> RetrieveUsers(Int32 organizationUserId, lkpCommunicationTypeContext communicationTypeContext)
        {
            IQueryable<OrganizationUser> organizationUsersList = MessageManager.RetrieveUsers(null, organizationUserId, communicationTypeContext);
            return organizationUsersList.Select(x => new MessagingRulesContract
            {
                FirstName = x.FirstName,
                UserID = x.OrganizationUserID

            }).ToList();
        }


        public void GetTemplates()
        {
            View.CompanyTemplates = MessageManager.GetTemplates(View.QueueType, View.CurrentUserId, View.CommunicationType).OfType<ADBMessage>(); ;
            #region Commented For queue type issue by Deepika
            //switch ((TenantTypeEnum)View.QueueType)
            //{
            //    case TenantTypeEnum.Client:
            //        // View.ClientTemplates = MessageManager.GetTemplates(View.QueueType, View.CurrentUserId).OfType<ClientMessage>(); ;
            //        break;

            //    case TenantTypeEnum.Company:
            //        View.CompanyTemplates = MessageManager.GetTemplates(View.QueueType, View.CurrentUserId).OfType<ADBMessage>(); ;
            //        break;

            //    case TenantTypeEnum.Supplier:
            //        //View.SupplierTemplates = MessageManager.GetTemplates(View.QueueType, View.CurrentUserId).OfType<SupplierMessage>(); ;
            //        break;
            //} 
            #endregion

        }

        public void GetTemplateMessage(string messageId)
        {
            if (!string.IsNullOrEmpty(messageId))
            {
                EntityObject obj;

                if (View.IsApplicant)
                    obj = MessageManager.GetMessageContentsForApplicant(View.QueueType, new Guid(messageId));
                else
                    obj = MessageManager.GetMessageContents(View.QueueType, new Guid(messageId));

                MapMessageXMLToViewProperties(obj);
            }
            else
            {
                View.Subject
                    = View.Content
                    = View.ToListGroupIds
                    = View.CcListGroupIds
                    = View.ToListIds
                    = View.CcListIds = string.Empty;
            }
        }


        ///// <summary>
        ///// Get Template Message.
        ///// </summary>
        ///// <param name="MessageId"></param>
        //public void GetTemplateMessage(Guid MessageId)
        //{
        //    EntityObject obj = MessageManager.GetMessageContents(View.QueueType, MessageId);

        //    List<Int32> toList = new List<Int32>();
        //    List<Int32> ccList = new List<Int32>();
        //    String templateName = String.Empty;
        //    String subject = String.Empty;
        //    String toUsersList = String.Empty;
        //    String ccUsersList = String.Empty;
        //    String toUsersListid = String.Empty;
        //    String ccUsersListid = String.Empty;
        //    Int32 tenantType = 1;
        //    Int32 messagetype = 0;
        //    StringBuilder messageContent = new StringBuilder();
        //    MessageDetail.MakeADBTemplateMessageHistoryContent(obj, toList, ccList, out templateName, out subject, View.CurrentUserId, messageContent, out tenantType, out messagetype, out toUsersList, out ccUsersList, out toUsersListid, out ccUsersListid);

        //    View.Content = messageContent.ToString();
        //    View.Subject = subject;
        //    if (!String.IsNullOrEmpty(toUsersListid))
        //    {
        //        String[] arrToList = toUsersListid.Split(';');
        //        View.ToListIds = MessageManager.GetUserNamesIdsString(arrToList.Where(x => !String.IsNullOrEmpty(x.Trim())).Select(x => Int32.Parse(x)).ToList());
        //    }

        //    if (!String.IsNullOrEmpty(ccUsersListid))
        //    {
        //        String[] arrCcList = ccUsersListid.Split(';');
        //        View.CcListIds = MessageManager.GetUserNamesIdsString(arrCcList.Where(x => !String.IsNullOrEmpty(x.Trim())).Select(x => Int32.Parse(x)).ToList());
        //    }
        //    View.MessageType = messagetype;
        //    View.MessageTypeId = messagetype;
        //    View.ReceiverTypeId = tenantType;
        //}

        /// <summary>
        /// Invoked to get the message for drafting.
        /// </summary>
        public void BindMessageType()
        {
            //View.BindMessageType = MessageManager.GetCurrentUserMessageTypes(View.CurrentUserId).OrderBy(x=>x.MessageTypeDesc);
            View.BindMessageType = MessageManager.GetMessageTypesByReceiver((TenantTypeEnum)View.QueueType).OrderBy(x => x.Description).AsQueryable();
        }

        public Dictionary<string, string> SaveDocumentAndGetDocumentId(List<ADBMessageDocument> documents)
        {
            return MessageManager.SaveDocumentAndGetDocumentId(documents);
        }

        /// <summary>
        /// Invoked to send message.
        /// </summary>
        /// <param name="messageMode"></param>
        public void SendMessage(String messageMode, Boolean isHighImportance)
        {

            UserGroup userGroup = null;

            List<String> toUserEmailIds;
            List<int> toIds;
            if (View.IsApplicant && View.Action != MessagingAction.Reply && View.Action != MessagingAction.ReplyAll)
            {
                toIds = View.ToListGroupIds.Split(';').Where(userId => userId.Trim() != String.Empty).Select(userId => int.Parse(userId)).ToList();
                toUserEmailIds = MessageManager.GetEmailIdsByGroupIds(toIds).ToList();
                View.ViewContract.ToUserGroupIds = View.ToListGroupIds;

                toIds.Clear();

                toIds = View.ToListUsersForApplicant.Split(';').Where(userId => userId.Trim() != String.Empty).Select(userId => int.Parse(userId)).ToList();
                toUserEmailIds = MessageManager.GetEmailId(toIds).ToList();
                View.ViewContract.ToUserIds = View.ToListUsersForApplicant;
            }
            else
            {
                toIds = View.ToListIds.Split(';').Where(userId => userId.Trim() != String.Empty).Select(userId => int.Parse(userId)).ToList();
                toUserEmailIds = MessageManager.GetEmailId(toIds);
                View.ViewContract.ToUserIds = View.ToListIds;
            }
            View.ViewContract.toUserList = String.Join(";", toUserEmailIds.ToArray());


            List<int> ccIds;
            List<String> ccUserEmailIds;

            if (View.IsApplicant)
            {
                ccIds = View.CcListGroupIds.Split(';').Where(userId => userId.Trim() != String.Empty).Select(userId => int.Parse(userId)).ToList();
                ccUserEmailIds = MessageManager.GetEmailIdsByGroupIds(ccIds).ToList();
                View.ViewContract.CCUserGroupIds = View.CcListGroupIds;

                ccIds.Clear();

                ccIds = View.CcListOfUserForApplicant.Split(';').Where(userId => userId.Trim() != String.Empty).Select(userId => int.Parse(userId)).ToList();
                ccUserEmailIds = MessageManager.GetEmailId(ccIds);
                View.ViewContract.CcUserIds = View.CcListOfUserForApplicant;
            }
            else
            {
                ccIds = View.CcListIds.Split(';').Where(userId => userId.Trim() != String.Empty).Select(userId => int.Parse(userId)).ToList();
                ccUserEmailIds = MessageManager.GetEmailId(ccIds);
                View.ViewContract.CcUserIds = View.CcListIds;
            }
            View.ViewContract.CcUserList = String.Join(";", ccUserEmailIds.ToArray());

            List<int> bccIds;
            List<String> bccUserEmailIds;

            if (!View.IsApplicant)
            {
                bccIds = View.BccListIds.Split(';').Where(userId => userId.Trim() != String.Empty).Select(userId => int.Parse(userId)).ToList();
                //UAT-4179
                if (View.IsNededToShowCopyMeInMailCheckBox && View.IsCopyOfMailToSender)
                {
                    bccIds.Add(View.CurrentUserId);
                    String newBccListIds = String.Concat(View.BccListIds, View.CurrentUserId.ToString() + ";");

                    View.ViewContract.BccUserIds = newBccListIds;
                }
                else
                {
                    View.ViewContract.BccUserIds = View.BccListIds;
                }

                bccUserEmailIds = MessageManager.GetEmailId(bccIds);
               
            }
            View.ViewContract.BccUserList = String.Join(";", ccUserEmailIds.ToArray());
            View.ViewContract.CommunicationType = View.CommunicationType;

            View.ViewContract.ToList = View.ToList;
            {
                userGroup = MessageManager.GetUserGroupByCurrentUserIdAndMessageType(View.CurrentUserId, View.MessageType);
                if (!userGroup.IsNull())
                {
                    View.ViewContract.CurrentUserId = userGroup.UserGroupID;
                    View.ViewContract.From = userGroup.UserGroupName;
                }
                else
                {
                    View.ViewContract.CurrentUserId = View.CurrentUserId;
                    View.ViewContract.From = MessageManager.GetEmailId(View.CurrentUserId);
                }
            }

            //check if we can pull the Id from lkpMessageFolder entity by passing "(Int32)lkpMessageFolderContext.DRAFTS : (Int32)lkpMessageFolderContext.INBOX;"
            View.ViewContract.CcList = View.CCList;
            View.ViewContract.MessageType = View.MessageType;

            // Draft based folder code will be set here. Else it will be set in the database SP, based on the type of rule applicable on it.
            View.ViewContract.FolderId = messageMode.Equals(MessageMode.DRAFTMESSAGE.GetStringValue()) ? MessageManager.GetFolderIdByCode(lkpMessageFolderContext.DRAFTS.GetStringValue()) : MessageManager.GetFolderIdByCode(lkpMessageFolderContext.INBOX.GetStringValue());

            View.ViewContract.MessageMode = messageMode;
            View.ViewContract.IsHighImportance = isHighImportance;
            View.ViewContract.Action = View.Action;
            View.ViewContract.QueueType = View.QueueType;
            View.ViewContract.MessageId = View.MessageId;
            View.ViewContract.Subject = View.Subject.Trim();
            View.ViewContract.TenantTypes = View.TenantTypes;
            View.ViewContract.Content = View.Content;
            View.ViewContract.DocumentID = View.DocumentID;
            View.ViewContract.EVaultDocumentID = View.EVaultDocumentID;
            View.ViewContract.DocumentName = View.DocumentName;
            View.ViewContract.OriginalDocumentName = View.OriginalDocumentName;

            //Database Name
            View.ViewContract.ApplicationDatabaseName = MessageManager.GetDatabaseName(ConfigurationManager.ConnectionStrings[AppConsts.APPLICATION_CONNECTION_STRING].ConnectionString);

            Boolean sendMessage = MessageManager.SendMessage(View.ViewContract);
            View.IsSendMessageSuccess = sendMessage;            
            View.MessageId = View.ViewContract.MessageId;

        }

        /// <summary>
        /// Invoked to get the selected message details
        /// </summary>
        public void GetMessage()
        {
            EntityObject obj = MessageManager.GetMessageContents(View.QueueType, View.MessageId);

            List<Int32> toList = new List<Int32>();
            List<Int32> ccList = new List<Int32>();
            String subject = String.Empty;
            String toListName = String.Empty;
            Int32 messageType = AppConsts.NONE;
            Int32 tenantType = 1;

            StringBuilder messageContent = new StringBuilder();
            Dictionary<String, String> dicAttachments = new Dictionary<String, String>();

            MessageDetail.MakeReplyMessageHistoryContent(obj, toList, ccList, out subject, View.CurrentUserId, View.Action, messageContent, out tenantType, out messageType, out toListName, out dicAttachments);

            ADBMessage adbMessage = obj as ADBMessage;
            View.IsHighImportant = adbMessage.IsHighImportant;

            View.ToList = MessageManager.GetEmailId(toList).ConvertEmailList();
            View.ToList = toListName;

            if (View.Action == MessagingAction.ReplyAll)
            {
                View.CcListIds = Convert.ToString(MessageManager.GetUserNamesIdsString(ccList));
                View.ToListIds = Convert.ToString(MessageManager.GetUserNamesIdsString(toList));
            }
            else
                View.ToListIds = Convert.ToString(MessageManager.GetUserNamesIdsString(toList).Split(';')[0]);

            View.Content = messageContent.ToString();
            View.Subject = "RE: " + subject;

            View.MessageType = messageType;
            View.ReceiverTypeId = tenantType;
            View.MessageTypeId = messageType;
        }

        /// <summary>
        /// Invoked to get the message to forward.
        /// </summary>
        public void GetForwardMessage()
        {
            EntityObject obj = MessageManager.GetMessageContents(View.QueueType, View.MessageId);

            List<Int32> toList = new List<Int32>();
            List<Int32> ccList = new List<Int32>();
            String subject = String.Empty;
            String toListName = String.Empty;
            Int32 tenantType = 1;
            Int32 messageType = AppConsts.NONE;
            StringBuilder messageContent = new StringBuilder();
            Dictionary<String, String> dicAttachments = new Dictionary<String, String>();

            MessageDetail.MakeReplyMessageHistoryContent(obj, toList, ccList, out subject, View.CurrentUserId, View.Action, messageContent, out tenantType, out messageType, out toListName, out dicAttachments);

            ADBMessage adbMessage = obj as ADBMessage;
            View.IsHighImportant = adbMessage.IsHighImportant;

            View.Content = messageContent.ToString();
            View.Subject = "FW: " + subject;
            View.ToList = "";
            View.CCList = "";
            View.MessageType = messageType;
            View.MessageTypeId = messageType;
            GetTenantTypes();
            View.ReceiverTypeId = tenantType;
            GetTheRelatedAttachments(adbMessage);

        }

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idsWithSemicolonSeparated"></param>
        /// <returns></returns>
        private string FormatGroupIds(string idsWithSemicolonSeparated)
        {
            if (!string.IsNullOrEmpty(idsWithSemicolonSeparated))
            {
                StringBuilder sb = new StringBuilder();
                String[] arrIds = idsWithSemicolonSeparated.Split(';');
                List<Int32> intParseIds = arrIds.Where(x => !String.IsNullOrEmpty(x.Trim())).Select(x => Int32.Parse(x)).ToList();
                IEnumerable<MessagingGroup> messagingGroups = MessageManager.GetMessagingGroups(intParseIds);

                foreach (MessagingGroup messagingGroup in messagingGroups)
                    sb.Append(string.Format("{0}:{1}", messagingGroup.GroupName, messagingGroup.MessagingGroupID));

                return sb.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        private string FormatUserIds(string userIds)
        {
            if (!string.IsNullOrEmpty(userIds))
            {
                String[] arrToList = userIds.Split(';');
                return MessageManager.GetUserNamesIdsString(arrToList.Where(x => !String.IsNullOrEmpty(x.Trim())).Select(x => Int32.Parse(x)).ToList());
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void MapMessageXMLToViewProperties(EntityObject message)
        {
            ADBMessage item = (message as ADBMessage);
            View.Subject = item.Subject;
            View.IsHighImportant = item.IsHighImportant;
            //View.CommunicationTypeId = Convert.ToInt32(item.Message.GetNodeContent("ToMessageType"));
            View.Content = SysXUtils.GetXmlDecodedString(item.Message.GetMessageBody());
            GetTheRelatedAttachments(item);

            if (View.IsApplicant)
            {
                View.ToListGroupIds = FormatGroupIds(item.Message.GetNodeContent("ToUserGroup"));
                View.CcListGroupIds = FormatGroupIds(item.Message.GetNodeContent("CCReceipients"));
            }
            else
            {
                View.ToListIds = FormatUserIds(item.Message.GetNodeContent("MessageReceipients"));
                View.CcListIds = FormatUserIds(item.Message.GetNodeContent("CCUserReceipients"));
                View.BccListIds = FormatUserIds(item.Message.GetNodeContent("BCCUserReceipients"));
            }
        }

        private void GetTheRelatedAttachments(ADBMessage item)
        {
            List<ADBMessageDocument> attachmentDocument = MessageManager.GetMessageAttachment(item.ADBMessageID);
            Int32 size = 0;
            if (attachmentDocument.IsNullOrEmpty())
            {
                View.AttachedFiles = null;
            }
            else
            {
                Dictionary<string, string> attachedFiles = new Dictionary<string, string>();
                foreach (var documentDetail in attachmentDocument)
                {
                    attachedFiles.Add(Convert.ToString(documentDetail.DocumentId), documentDetail.OriginalDocumentName);
                    size += documentDetail.DocumentSize;
                }
                View.AttachedFiles = attachedFiles;
            }

            View.FileSize = Convert.ToString(size);
        }
        #endregion

        public void SendEmail(List<SystemCommunicationAttachment> lstSystemCommunicationAttachment)
        {
            List<int> toIds, ccIds, bccIds = new List<int>();

            //Getting To Ids
            toIds = View.ToListIds.Split(';').Where(userId => userId.Trim() != String.Empty).Select(userId => int.Parse(userId)).ToList();

            //Getting CC Ids
            ccIds = View.CcListIds.Split(';').Where(userId => userId.Trim() != String.Empty).Select(userId => int.Parse(userId)).ToList();

            //Getting BCC Ids
            bccIds = View.BccListIds.Split(';').Where(userId => userId.Trim() != String.Empty).Select(userId => int.Parse(userId)).ToList();

            //UAT-4179
            if (View.IsNededToShowCopyMeInMailCheckBox && View.IsCopyOfMailToSender)
            {
                bccIds.Add(View.CurrentUserId);
            }
            CommunicationManager.SendEmail(toIds, ccIds, bccIds, View.CurrentUserId, View.Content, View.Subject, lstSystemCommunicationAttachment);
        }
        public Boolean CheckApplicantClientSettings()
        {
            Int32 _tenantId = SecurityManager.GetOrganizationUser(View.CurrentUserId).Organization.TenantID.Value;
            Entity.ClientEntity.ClientSetting clientsettings = ComplianceDataManager.GetClientSetting(_tenantId, Setting.ALLOW_APPLICANT_TO_SEND_MESSAGE.GetStringValue());
            if (clientsettings.IsNotNull())
            {
                return (!String.IsNullOrEmpty(clientsettings.CS_SettingValue) && clientsettings.CS_SettingValue == AppConsts.STR_ONE) ? true : false;
            }
            else
            {
                return true;
            }
        }

        #region UAT-3215
        public String GetRotationDetails(INTSOF.ServiceDataContracts.Modules.ClinicalRotation.ClinicalRotationDetailContract clinicalRotationDetailContract)
        {
            if (clinicalRotationDetailContract.IsNullOrEmpty())
            {
                return String.Empty;
            }
            StringBuilder _sbRotationDetails = new StringBuilder();
            _sbRotationDetails.Append("<h4><i>Rotation Details:</i></h4>");
            _sbRotationDetails.Append("<div style='line-height:21px'>");
            _sbRotationDetails.Append("<ul style='list-style-type: disc'>");

            if (!clinicalRotationDetailContract.AgencyName.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Agency Name: </b>" + clinicalRotationDetailContract.AgencyName + "</li>");
            }
            if (!clinicalRotationDetailContract.ComplioID.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Complio ID: </b>" + clinicalRotationDetailContract.ComplioID + "</li>");
            }
            if (!clinicalRotationDetailContract.RotationName.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Rotation Name: </b>" + clinicalRotationDetailContract.RotationName + "</li>");
            }
            if (!clinicalRotationDetailContract.Department.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Department: </b>" + clinicalRotationDetailContract.Department + "</li>");
            }
            if (!clinicalRotationDetailContract.Program.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Program: </b>" + clinicalRotationDetailContract.Program + "</li>");
            }
            if (!clinicalRotationDetailContract.Course.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Course: </b>" + clinicalRotationDetailContract.Course + "</li>");
            }
            if (!clinicalRotationDetailContract.Term.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Term: </b>" + clinicalRotationDetailContract.Term + "</li>");
            }
            if (!clinicalRotationDetailContract.TypeSpecialty.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Type/Specialty: </b>" + clinicalRotationDetailContract.TypeSpecialty + "</li>");
            }
            if (!clinicalRotationDetailContract.UnitFloorLoc.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Unit/Floor or Location: </b>" + clinicalRotationDetailContract.UnitFloorLoc + "</li>");
            }
            if (!clinicalRotationDetailContract.RecommendedHours.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "# of Recommended Hours: </b>" + clinicalRotationDetailContract.RecommendedHours + "</li>");
            }
            if (!clinicalRotationDetailContract.DaysName.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Days: </b>" + clinicalRotationDetailContract.DaysName + "</li>");
            }
            if (!clinicalRotationDetailContract.Shift.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Shift: </b>" + clinicalRotationDetailContract.Shift + "</li>");
            }
            if (!clinicalRotationDetailContract.Time.IsNullOrEmpty() && clinicalRotationDetailContract.Time != "-")
            {
                _sbRotationDetails.Append("<li><b>" + "Time: </b>" + clinicalRotationDetailContract.Time + "</li>");
            }
            if (!clinicalRotationDetailContract.StartDate.IsNullOrEmpty() && !clinicalRotationDetailContract.EndDate.IsNullOrEmpty())
            {
                _sbRotationDetails.Append("<li><b>" + "Dates: </b>" + Convert.ToDateTime(clinicalRotationDetailContract.StartDate).ToString("MM/dd/yyyy") + " - " + Convert.ToDateTime(clinicalRotationDetailContract.EndDate).ToString("MM/dd/yyyy") + "</li>");
            }
            _sbRotationDetails.Append("</ul>");
            _sbRotationDetails.Append("</div>");
            return Convert.ToString(_sbRotationDetails);
        }
        #endregion
    }
}




