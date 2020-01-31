#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXException.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Runtime.Serialization;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Utils
{
    [Serializable]
    public class SysXException : ApplicationException, ISerializable
    {
        #region Variables

        #endregion

        #region Properties

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <remarks></remarks>
        public string ErrorMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the error description.
        /// </summary>
        /// <remarks></remarks>
        public string ErrorDescription
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the sys X error value.
        /// </summary>
        /// <remarks></remarks>
        public int SysXErrorValue
        {
            get;
            private set;
        }

        /// <summary>
        /// Message Property
        /// </summary>
        /// <returns>
        /// The error message that explains the reason for the exception, or an empty string("").
        ///   </returns>
        /// <remarks></remarks>
        public new String Message
        {
            get
            {
                return base.Message;                
            }
        }

        #endregion

        #region Events

        #endregion

        #region Methods

        // Normal 3 constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public SysXException()
        {
            //TODO: needs to be implemented.
        }

        /// <summary>
        /// Pameratrised Constructor
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public SysXException(String message)
            : base(message)
        {
            //TODO: needs to be implemented.
        }

        /// <summary>
        /// Pameratrised Constructor with two parameters
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        /// <remarks></remarks>
        public SysXException(String message, Exception inner)
            : base(message, inner)
        {
            //TODO: needs to be implemented.
        }

        /// <summary>
        /// constructors that take the added value
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="theValue">The value.</param>
        /// <remarks></remarks>
        public SysXException(String message, Int32 theValue)
            : base(message)
        {
            this.SysXErrorValue = theValue;
        }

        /// <summary>
        /// Constructors that take the added value
        /// </summary>
        /// <param name="detailErrorMessage">The detail error message.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="description">The description.</param>
        /// <remarks></remarks>
        public SysXException(String detailErrorMessage, String errorMessage, String description)
            : base(detailErrorMessage)
        {
            ErrorMessage = errorMessage;
            ErrorDescription = description;
        }

        /// <summary>
        /// deserialization constructor
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <remarks></remarks>
        public SysXException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.SysXErrorValue = info.GetInt32("SysXErrorValue");
        }

        /// <summary>
        /// Called by the frameworks during serialization, to fetch the data from an object.
        /// </summary>
        /// <param name="info">Serialization Info</param>
        /// <param name="context">Streaming Context</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("SysXErrorValue", this.SysXErrorValue);
        }

        /// <summary>
        /// Returns the method name.
        /// </summary>
        /// <returns>String</returns>
        /// <remarks></remarks>
        public static String ShowTrace()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1);
            System.Diagnostics.StackFrame sf = st.GetFrame(0);
            String str = sf.GetMethod().DeclaringType.FullName + "." + sf.GetMethod().Name;
            return str;
        }

        #endregion
    }
}