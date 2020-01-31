#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXArgumentException.cs
// Purpose:   Argument Exception
//

#endregion

#region Namespace

#region System Defined
using System;
using System.Runtime.Serialization;
#endregion

#endregion

namespace SPI.Rule.UI
{
    [DataContract]
    public class SysXArgumentException
    {
        [DataMember]
        public String Message { get; private set; }

        [DataMember]
        public Exception InnerException { get; private set; }

        /// <summary>
        ///  Argument Exception constructor
        /// </summary>
        /// <param name="Message">Message as String</param>
        public SysXArgumentException(String Message)
        {
            this.Message = Message;
        }

        /// <summary>
        /// Argument Exception constructor
        /// </summary>
        /// <param name="Message">String</param>
        /// <param name="InnerException">Exception</param>
        public SysXArgumentException(String Message, Exception InnerException)
        {
            this.Message = Message;
            this.InnerException = InnerException;
        }
    }
}
