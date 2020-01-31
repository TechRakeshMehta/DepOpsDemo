#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXEntryPointNotFoundException.cs
// Purpose:    Entry PointNotFoundException Class
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
    public class SysXEntryPointNotFoundException
    {
        [DataMember]
        public String Message { get; private set; }

        [DataMember]
        public Exception InnerException { get; private set; }

        /// <summary>
        /// Entry PointNotFoundException
        /// </summary>
        /// <param name="Message">String</param>
        public SysXEntryPointNotFoundException(String Message)
        {
            this.Message = Message;
        }

        /// <summary>
        /// Entry PointNotFoundException
        /// </summary>
        /// <param name="Message">String</param>
        /// <param name="InnerException">Exception</param>
        public SysXEntryPointNotFoundException(String Message, Exception InnerException)
        {
            this.Message = Message;
            this.InnerException = InnerException;
        }
    }
}
