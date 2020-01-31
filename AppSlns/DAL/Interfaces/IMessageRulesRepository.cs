using Entity;
using INTSOF.UI.Contract.Messaging;
using System;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    /// <summary>
    /// Interface for exposing methods for the Message Rules
    /// </summary>
    public interface IMessageRulesRepository
    {
        void SaveMessageRules(MessagingRulesContract messagingRulesContract,String dataBasename);
        List<MessageRule> GetMessageRules(Int32 userId);
        void DeleteMessageRule(Int32 ruleId);
    }
}
