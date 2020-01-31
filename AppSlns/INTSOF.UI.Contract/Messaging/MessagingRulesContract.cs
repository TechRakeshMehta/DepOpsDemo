using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DB = Entity;
namespace INTSOF.UI.Contract.Messaging
{
    public class MessagingRulesContract
    {
        public Int32 RuleId { get; set; }
        public String RuleDescription { get; set; }
        public Int32 FolderId { get; set; }
        public Int32 MessageRuleLocationID { get; set; }
        public Int32? InstitutionID { get; set; }
        public Int32? LocationID { get; set; }
        public Int32? ProgramID { get; set; }
        public Int32 UserID { get; set; }
        public Dictionary<Int32, String> MessageFromUsers { get; set; }        
        public DB.MessageRuleUserLocation TransactionalObject { get; set; }
        public String FirstName { get; set; }
        //public List<DB.MessageRuleUserLocation> GridData { get; set; }
    }
}
