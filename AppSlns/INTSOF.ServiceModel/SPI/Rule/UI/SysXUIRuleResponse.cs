#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXServiceNotFoundException.cs
// Purpose:   Service Not Found Exception class
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
    public class SysXUIRuleResponse
    {
        [DataMember]
        public Guid InstanceId { get; private set; }

        [DataMember]
        public String ServiceName { get; private set; }

        [DataMember]
        public String EntryPoint { get; private set; }

        [DataMember]
        public Object Output { get; set; }

        /// <summary>
        /// UI Rule Response
        /// </summary>
        /// <param name="request">SysXUIRuleRequest</param>
        public SysXUIRuleResponse(SysXUIRuleRequest request)
        {
            this.ServiceName = request.ServiceName;
            this.EntryPoint = request.EntryPoint;
            this.InstanceId = request.InstanceId;
        }


    }
}
