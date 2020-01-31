using System;
using System.Collections.Generic;
using System.Text;
using Entity;

namespace CoreWeb.Messaging.Views
{
    public interface ITransferRulesGridView
    {
        ITransferRulesGridView CurrentViewContext { get; }
        List<MessageRule> MessageRules { get; set; }

        Int32 CurrentUserId { get; }
        Int32 RuleId { get; set; }
    }
}




