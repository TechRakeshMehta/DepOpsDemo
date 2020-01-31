using DAL.Interfaces;
using Entity;
using INTSOF.UI.Contract.Messaging;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace DAL.Repository
{
    public class MessageRulesRepository : BaseQueueRepository,  IMessageRulesRepository
    {
        private ObjectContext _messageRulesEntityContext;
        private SysXAppDBEntities _appDBContext;
        private ADBMessageDB_DevEntities _appMessageDBContext;
 

        public MessageRulesRepository()
        {
            _appDBContext = base.AppDBContext;
          
        }

        /// <summary>
        /// Save new rule in the database
        /// </summary>
        /// <param name="messagingRulesContract">Contract which gets data from the UI</param>
        /// <returns>Save status</returns>
        public void SaveMessageRules(MessagingRulesContract messagingRulesContract,String dataBasename)
        {
            if (messagingRulesContract.RuleId > 0)
            {
                MessageRule msgRule = _appDBContext.MessageRules.Where(x => x.MessageRuleID == messagingRulesContract.RuleId).FirstOrDefault();
                msgRule.Description = messagingRulesContract.RuleDescription;
                msgRule.MessageFolderID = messagingRulesContract.FolderId;
                msgRule.ModifiedByID = messagingRulesContract.UserID;
                msgRule.ModifiedOn = DateTime.Now;

                MessageRuleLocation msgRuleLocation = _appDBContext.MessageRuleLocations.Where(x => x.MessageRuleLocationID == messagingRulesContract.RuleId).FirstOrDefault();
                msgRuleLocation.InstitutionID = messagingRulesContract.InstitutionID;
                msgRuleLocation.LocationID = messagingRulesContract.LocationID;
                msgRuleLocation.ProgramID = messagingRulesContract.ProgramID;
                msgRuleLocation.ModifiedByID = messagingRulesContract.UserID;
                msgRuleLocation.ModifiedOn = DateTime.Now;

                List<MessageRuleUserLocation> lstRuleUserLocation = _appDBContext.MessageRuleUserLocations.Where(mrul => mrul.MessageRuleLocationID == msgRuleLocation.MessageRuleLocationID).ToList();

                foreach (var mrul in lstRuleUserLocation)
                {
                    _appDBContext.MessageRuleUserLocations.DeleteObject(mrul);
                }
                foreach (var user in messagingRulesContract.MessageFromUsers)
                {
                    MessageRuleUserLocation messageRuleUserLocation = new MessageRuleUserLocation();
                    messageRuleUserLocation.UserID = user.Key;
                    messageRuleUserLocation.MessageRuleLocationID = msgRuleLocation.MessageRuleLocationID;
                    _appDBContext.MessageRuleUserLocations.AddObject(messageRuleUserLocation);
                }
            }
            else
            {

                MessageRule rule = new MessageRule()
                {
                    Description = messagingRulesContract.RuleDescription,
                    MessageFolderID = messagingRulesContract.FolderId,
                    UserID = messagingRulesContract.UserID,
                    CreatedByID = messagingRulesContract.UserID,
                    ModifiedByID = messagingRulesContract.UserID
                };

                MessageRuleLocation ruleLocation = new MessageRuleLocation()
                 {
                     InstitutionID = messagingRulesContract.InstitutionID,
                     ProgramID = messagingRulesContract.ProgramID,
                     LocationID = messagingRulesContract.LocationID,
                     CreatedByID = messagingRulesContract.UserID,
                     ModifiedByID = messagingRulesContract.UserID,
                     MessageRule = rule
                 };
                foreach (var user in messagingRulesContract.MessageFromUsers)
                {
                    MessageRuleUserLocation messageRuleUserLocation = new MessageRuleUserLocation();
                    messageRuleUserLocation.UserID = user.Key;
                    messageRuleUserLocation.MessageRuleLocation = ruleLocation;
                    _appDBContext.MessageRuleUserLocations.AddObject(messageRuleUserLocation);
                }
                if (messagingRulesContract.MessageFromUsers.Count == AppConsts.NONE)
                {
                    _appDBContext.MessageRuleLocations.AddObject(ruleLocation);
                }
            }
            _appDBContext.SaveChanges();

            ApplyMessageRule(messagingRulesContract.UserID,dataBasename);
        }

        private void ApplyMessageRule(Int32 ruleOwnerId, String dataBasename)
        {
            _appMessageDBContext = base.ADB_MessageQueueContext;
            _appMessageDBContext.ApplyMessageRulesOnCreation(dataBasename, ruleOwnerId);
        }

        /// <summary>
        /// Get the list of all the message rules for a user
        /// </summary>
        /// <param name="userId">Current user id</param>
        /// <returns>List of the rules</returns>
        public List<MessageRule> GetMessageRules(Int32 userId)
        {
            return _appDBContext.MessageRules.Include(SysXEntityConstants.TABLE_MESSAGERULELOCATIONS_DOT_MESSAGERULEUSERLOCATIONS).Where(mrl => mrl.IsDeleted == false && mrl.UserID == userId).ToList();
        }

        /// <summary>
        /// Delete the selected Rule
        /// </summary>
        /// <param name="ruleId">Id of the rule to delete</param>
        public void DeleteMessageRule(Int32 ruleId)
        {
            MessageRuleUserLocation mr = _appDBContext.MessageRuleUserLocations.Where(mrul => mrul.MessageRuleLocation.MessageRule.MessageRuleID.Equals(ruleId)).FirstOrDefault();
            MessageRule messageRule = new MessageRule();

            if (mr.IsNotNull())
                messageRule = _appDBContext.MessageRuleUserLocations.Where(mrul => mrul.MessageRuleLocation.MessageRule.MessageRuleID == ruleId).FirstOrDefault().MessageRuleLocation.MessageRule;
            else
                messageRule = _appDBContext.MessageRuleLocations.Where(mruleloc => mruleloc.MessageRule.MessageRuleID == ruleId).FirstOrDefault().MessageRule;

            messageRule.IsDeleted = true;
            _appDBContext.SaveChanges();
        }
    }
}
