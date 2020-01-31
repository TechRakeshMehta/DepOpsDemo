#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXExceptionConsts.cs
// Purpose:   Exception Consts
//

#endregion

#region Namespaces

#region System Defined

using System;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Utils.Consts
{
    /// <summary>
    /// Handles the constant for Exception handling.
    /// </summary>
    /// <remarks></remarks>
    public static class SysXExceptionConsts
    {
        #region Constant for Exception Formating

        /// <summary>
        /// Constant for ApplicationDomain:
        /// </summary>
        public const String APPLICATION_DOMAIN = "ApplicationDomain:";

        /// <summary>
        /// Constant for TrustLevel:
        /// </summary>
        public const String TRUST_LEVEL = "TrustLevel:";

        /// <summary>
        /// Constant for ApplicationVirtualPath:
        /// </summary>
        public const String APPLICATION_VIRTUALPATH = "ApplicationVirtualPath:";

        /// <summary>
        /// Constant for AppliactionPath:
        /// </summary>
        public const String APPLIACTION_PATH = "AppliactionPath:";

        /// <summary>
        /// Constant for MachineName:
        /// </summary>
        public const String MACHINE_NAME = "MachineName:";

        /// <summary>
        /// Constant for EventCode:
        /// </summary>
        public const String EVENT_CODE = "EventCode:";

        /// <summary>
        /// Constant for EventMessage:
        /// </summary>
        public const String EVENT_MESSAGE = "EventMessage:";

        /// <summary>
        /// Constant for EventTime:
        /// </summary>
        public const String EVENT_TIME = "EventTime:";

        /// <summary>
        /// Constant for EventSequence:
        /// </summary>
        public const String EVENT_SEQUENCE = "EventSequence:";

        /// <summary>
        /// Constant for EventOccurrence:
        /// </summary>
        public const String EVENT_OCCURRENCE = "EventOccurrence:";

        /// <summary>
        /// Constant for EventDetailCode:
        /// </summary>
        public const String EVENT_DETAIL_CODE = "EventDetailCode:";

        /// <summary>
        /// Constant for ProcessId:
        /// </summary>
        public const String PROCESS_ID = "ProcessId:";

        /// <summary>
        /// Constant for ProcessName:
        /// </summary>
        public const String PROCESS_NAME = "ProcessName:";

        /// <summary>
        /// Constant for AccountName:
        /// </summary>
        public const String ACCOUNT_NAME = "AccountName:";

        /// <summary>
        /// Constant for ExceptionType:
        /// </summary>
        public const String EXCEPTION_TYPE = "ExceptionType:";

        /// <summary>
        /// Constant for ExceptionMessage:
        /// </summary>
        public const String EXCEPTION_MESSAGE = "ExceptionMessage:";

        /// <summary>
        /// Constant for ExceptionMessage:
        /// </summary>
        public const String INNEREXCEPTION_MESSAGE = "InnerExceptionMessage:";


        /// <summary>
        /// Constant for Severity:
        /// </summary>
        public const String SEVERITY = "Severity:";

        /// <summary>
        /// Constant for Module/Project:
        /// </summary>
        public const String MODULE_PROJECT = "Module/Project:";

        /// <summary>
        /// Constant for Class File Name:
        /// </summary>
        public const String CLASS_FILE_NAME = "Class File Name:";

        /// <summary>
        /// Constant for Class Name:
        /// </summary>
        public const String CLASS_NAME = "Class Name:";

        /// <summary>
        /// Constant for Method Name:
        /// </summary>
        public const String METHOD_NAME = "Method Name:";

        /// <summary>
        /// Constant for Line: # 
        /// </summary>
        public const String LINE_NO = "Line: # ";

        /// <summary>
        /// Constant for RequestUrl:
        /// </summary>
        public const String REQUEST_URL = "RequestUrl:";

        /// <summary>
        /// Constant for RequestPath:
        /// </summary>
        public const String REQUEST_PATH = "RequestPath:";

        /// <summary>
        /// Constant for UserHostAddress:
        /// </summary>
        public const String USERHOST_ADDRESS = "UserHostAddress:";

        /// <summary>
        /// Constant for IsAuthenticated:
        /// </summary>
        public const String IS_AUTHENTICATED = "IsAuthenticated:";

        /// <summary>
        /// Constant for AuthenticationType:
        /// </summary>
        public const String AUTHENTICATION_TYPE = "AuthenticationType:";

        /// <summary>
        /// Constant for ThreadId:
        /// </summary>
        public const String THREAD_ID = "ThreadId:";

        /// <summary>
        /// Constant for ThreadAccountName:
        /// </summary>
        public const String THREAD_ACCOUNT_NAME = "ThreadAccountName:";

        /// <summary>
        /// Constant for 
        /// </summary>
        public const String IS_IMPERSONATING = "IsImpersonating:";

        /// <summary>
        /// Constant for StackTrace:
        /// </summary>
        public const String STACK_TRACE = "StackTrace:";

        /// <summary>
        /// Constant for StackTraceFunctionOrEvent:
        /// </summary>
        public const String STACK_TRACE_FUNCTION_OR_EVENT = "StackTraceFunctionOrEvent:";

        /// <summary>
        /// Constant for http://:
        /// </summary>
        public const String HTTP_INITIAL = "http://:";

        /// <summary>
        /// Constant for Source:
        /// </summary>
        public const String SOURCE = "Source:";

        /// <summary>
        /// Constant for \n
        /// </summary>
        public const String NEWLINE = "\n";

        /// <summary>
        /// Constant for Application Information
        /// </summary>
        public const String APPLICATION_INFORMATION = "Application Information";

        /// <summary>
        /// Constant for =====================
        /// </summary>
        public const String ERROR_FORMATTER = "=====================";

        /// <summary>
        /// Constant for Event Information
        /// </summary>
        public const String EVENT_INFORMATION = "Event Information";

        /// <summary>
        /// Constant for Process Information
        /// </summary>
        public const String PROCESS_INFORMATION = "Process Information";

        /// <summary>
        /// Constant for Exception Information
        /// </summary>
        public const String EXCEPTION_INFORMATION = "Exception Information";

        /// <summary>
        /// Constant for 
        /// </summary>
        public const String REQUEST_INFORMATION = "Request Information";

        /// <summary>
        /// Constant for Thread Information
        /// </summary>
        public const String THREAD_INFORMATION = "Thread Information";

        /// <summary>
        /// Constant for NA
        /// </summary>
        public const String Exception_NA = "NA";

        /// <summary>
        /// Constant for Custom Message : 
        /// </summary>
        public const String CUSTOM_MESSAGE = "Custom Message : ";

        /// <summary>
        /// Constant for Some error has occurred the description of which is : 
        /// </summary>
        public const String SYSX_EXCEPTION_GENERIC_ERROR_MESSAGE = "Some error has occurred the description of which is: ";

        /// <summary>
        /// External Exception
        /// </summary>
        public const String EXTERNAL_EXCEPTION = "ExternalException";

        /// <summary>
        /// Access Exception 
        /// </summary>
        public const String ACCESS_EXCEPTION = "AccessException";

        /// <summary>
        /// ARGUMENT EXCEPTION Const
        /// </summary>
        public const String ARGUMENT_EXCEPTION = "ArgumentException";

        /// <summary>
        /// ARGUMENT NULL EXCEPTION
        /// </summary>
        public const String ARGUMENT_NULL_EXCEPTION = "ArgumentNullException";

        /// <summary>
        /// Arithmetic Exception
        /// </summary>
        public const String ARITHMETIC_EXCEPTION = "ArithmeticException";

        /// <summary>
        /// Array Type Mismatch Exception
        /// </summary>
        public const String ARRAY_TYPE_MISMATCH_EXCEPTION = "ArrayTypeMismatchException";

        /// <summary>
        /// Bad Image Format Exception const
        /// </summary>
        public const String BAD_IMAGE_FORMAT_EXCEPTION = "BadImageFormatException";

        /// <summary>
        /// Divide By Zero Exception const
        /// </summary>
        public const String DIVIDE_BY_ZERO_EXCEPTION = "DivideByZeroException";

        /// <summary>
        /// Format Exception const
        /// </summary>
        public const String FORMAT_EXCEPTION = "FormatException";

        /// <summary>
        /// Index Out Of Range Exception const
        /// </summary>
        public const String INDEX_OUTOFRANGE_EXCEPTION = "IndexOutOfRangeException";

        /// <summary>
        /// Invalid Operation Exception const
        /// </summary>
        public const String INVALID_OPERATION_EXCEPTION = "InvalidOperationException";

        /// <summary>
        /// Core Exception const
        /// </summary>
        public const String CORE_EXCEPTION = "CoreException";

        /// <summary>
        /// Missing Member Exception const
        /// </summary>
        public const String MISSING_MEMBER_EXCEPTION =  "MissingMemberException";

        /// <summary>
        /// Not Finite Number Exception const
        /// </summary>
        public const String NOT_FINITE_NUMBER_EXCEPTION = "NotFiniteNumberException";

        /// <summary>
        /// Not Supported Exception const
        /// </summary>
        public const String NOT_SUPPORTED_EXCEPTION = "NotSupportedException";

        /// <summary>
        /// Null Reference Exception const
        /// </summary>
        public const String NULL_REFERENCE_EXCEPTION = "NullReferenceException";

        /// <summary>
        /// Out Of Memory Exception const
        /// </summary>
        public const String OUT_OF_MEMORY_EXCEPTION = "OutOfMemoryException";

        /// <summary>
        /// Stack Over flow Exception const
        /// </summary>
        public const String STACK_OVER_FLOW_EXCEPTION = "StackOverflowException";

        /// <summary>
        /// Optimistic Concurrency Exception const
        /// </summary>
        public const String OPTIMISTIC_CONCURRENCY_EXCEPTION = "OptimisticConcurrencyException";

        /// <summary>
        /// Update Exception const
        /// </summary>
        public const String UPDATE_EXCEPTION =  "UpdateException";

        /// <summary>
        /// Invalid Cast Exception
        /// </summary>
        public const String  INVALID_CAST_EXCEPTION = "InvalidCastException";

        /// <summary>
        /// Com Exception
        /// </summary>
        public const String  COM_EXCEPTION = "ComException";

        /// <summary>
        /// SHEException const
        /// </summary>
        public const String SHE_EXCEPTION = "SHEException";

        /// <summary>
        /// Access Violation Exception
        /// </summary>
        public const String ACCESS_VIOLATION_EXCEPTION = "AccessViolationException";

        /// <summary>
        /// HTTP HOST const
        /// </summary>
        public const String HTTP_HOST = "HTTP_HOST";

        /// <summary>
        /// LINE
        /// </summary>
        public const String  LINE = "line";

        
        /// <summary>
        /// File Extension DOT CS
        /// </summary>
        public const String DOT_CS = ".cs";



        #endregion
    }
}