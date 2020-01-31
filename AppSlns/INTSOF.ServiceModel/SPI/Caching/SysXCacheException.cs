#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXCacheException.cs
// Purpose:   SysX Cache Exception Class
//

#endregion

#region Namespace

#region System Defined

using System;
using System.Runtime.Serialization;

#endregion

#endregion

namespace SPI.Caching
{
    [DataContract]
    public class SysXCachetException
    {
        [DataMember]
        public String Message { get; private set; }

        [DataMember]
        public Exception InnerException { get; private set; }

        /// <summary>
        /// Cachet Exception
        /// </summary>
        /// <param name="Message">Message as String</param>
        public SysXCachetException(String Message)
        {
            this.Message = Message;
        }

        /// <summary>
        /// SysXCachetException
        /// </summary>
        /// <param name="Message">Message</param>
        /// <param name="InnerException">InnerException as Exception</param>
        public SysXCachetException(String Message, Exception InnerException)
        {
            this.Message = Message;
            this.InnerException = InnerException;
        }
    }
}
