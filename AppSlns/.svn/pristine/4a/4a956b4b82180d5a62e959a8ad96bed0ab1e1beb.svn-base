#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ISysXUIRulesManager.cs
// Purpose:   Interface UI Rules Manager
//

#endregion


#region Namespace

#region System Defined
using System.ServiceModel;



#endregion

#endregion

namespace SPI.Rule.UI
{
    
    [ServiceContract]

    
    public interface ISysXUIRulesManager
    {
        [OperationContract]
        [FaultContract(typeof(SysXArgumentException))]
        [FaultContract(typeof(SysXServiceNotFoundException))]
        [FaultContract(typeof(SysXEntryPointNotFoundException))]
        SysXUIRuleResponse InvokeRule(SysXUIRuleRequest ruleRequest);

        [OperationContract]
        [FaultContract(typeof(SysXArgumentException))]
        [FaultContract(typeof(SysXServiceNotFoundException))]
        [FaultContract(typeof(SysXEntryPointNotFoundException))]
        void InvokeRuleWithReference(ref SysXUIRuleRequest ruleRequest);
    }
}
