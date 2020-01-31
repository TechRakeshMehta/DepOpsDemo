#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXUIRuleRequest.cs
// Purpose:   UI Rule Request class
//

#endregion

#region Namespace

#region System defined

using System;
using System.Runtime.Serialization;

#endregion

#endregion

namespace SPI.Rule.UI
{
    [DataContract]
    public class SysXUIRuleRequest
    {
        [DataMember]
        public Guid InstanceId { get; private set; }

        [DataMember]
        public String ServiceName { get; set; }

        [DataMember]
        public String EntryPoint { get; set; }

        [DataMember]
        public Object[] Args { get; set; }

        /// <summary>
        /// UI Rule Request
        /// </summary>
        public SysXUIRuleRequest()
        {
            this.InstanceId = Guid.NewGuid();
        }

        /// <summary>
        /// UI Rule Request
        /// </summary>
        /// <param name="InstanceId">Guid</param>
        public SysXUIRuleRequest(Guid InstanceId)
        {
            this.InstanceId = InstanceId;
        }


    }
}
