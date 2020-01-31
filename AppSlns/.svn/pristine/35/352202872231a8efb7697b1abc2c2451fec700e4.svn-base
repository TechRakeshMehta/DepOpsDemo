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
    public class SysXServiceNotFoundException
    {
        [DataMember]
        public String Message { get; private set; }

        [DataMember]
        public Exception InnerException { get; private set; }

        /// <summary>
        /// Service NotFoundException
        /// </summary>
        /// <param name="Message"></param>
        public SysXServiceNotFoundException(String Message)
        {
            this.Message = Message;
        }

        /// <summary>
        /// Service NotFoundException
        /// </summary>
        /// <param name="Message">String</param>
        /// <param name="InnerException">Exception</param>
        public SysXServiceNotFoundException(String Message, Exception InnerException)
        {
            this.Message = Message;
            this.InnerException = InnerException;
        }
    }
}
