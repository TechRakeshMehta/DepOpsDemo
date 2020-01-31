#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXExceptionSeverity.cs
// Purpose:   Exception Severity class
//

#endregion

#region Namespaces

#region System Defined

using System;

#endregion

#region Application Specific

using INTSOF.Utils;
using INTSOF.Utils.Consts;

#endregion

#endregion

namespace CoreWeb.IntsofExceptionModel
{
    /// <summary>
    /// Class to get the Severity of Exception
    /// </summary>
    public class SysXExceptionSeverity
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private static String _severityStatus = String.Empty;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        #region Function to Get the Severity of Exception

        /// <summary>
        /// Get the Severity of Exception
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>String</returns>
        public static String GetSeverity(Type type)
        {
            switch (type.Name)
            {
                case SysXExceptionConsts.EXTERNAL_EXCEPTION:
                    _severityStatus = Severity.High.ToString();
                    break;
                case SysXExceptionConsts.ACCESS_EXCEPTION:
                    _severityStatus = Severity.High.ToString();
                    break;
                case SysXExceptionConsts.ARGUMENT_EXCEPTION:
                    _severityStatus = Severity.Low.ToString();
                    break;
                case SysXExceptionConsts.ARGUMENT_NULL_EXCEPTION:
                    _severityStatus = Severity.High.ToString();
                    break;
                case SysXExceptionConsts.ARITHMETIC_EXCEPTION:
                    _severityStatus = Severity.Medium.ToString();
                    break;
                case SysXExceptionConsts.ARRAY_TYPE_MISMATCH_EXCEPTION:
                    _severityStatus = Severity.High.ToString();
                    break;
                case SysXExceptionConsts.BAD_IMAGE_FORMAT_EXCEPTION:
                    _severityStatus = Severity.Warn.ToString();
                    break;
                case SysXExceptionConsts.DIVIDE_BY_ZERO_EXCEPTION:
                    _severityStatus = Severity.High.ToString();
                    break;
                case SysXExceptionConsts.FORMAT_EXCEPTION:
                    _severityStatus = Severity.Medium.ToString();
                    break;
                case SysXExceptionConsts.INDEX_OUTOFRANGE_EXCEPTION:
                    _severityStatus = Severity.High.ToString();
                    break;
                case SysXExceptionConsts.INVALID_OPERATION_EXCEPTION:
                    _severityStatus = Severity.High.ToString();
                    break;
                case SysXExceptionConsts.CORE_EXCEPTION:
                    _severityStatus = Severity.Low.ToString();
                    break;
                case SysXExceptionConsts.MISSING_MEMBER_EXCEPTION:
                    _severityStatus = Severity.Low.ToString();
                    break;
                case SysXExceptionConsts.NOT_FINITE_NUMBER_EXCEPTION:
                    _severityStatus = Severity.High.ToString();
                    break;
                case SysXExceptionConsts.NOT_SUPPORTED_EXCEPTION:
                    _severityStatus = Severity.Low.ToString();
                    break;
                case SysXExceptionConsts.NULL_REFERENCE_EXCEPTION:
                    _severityStatus = Severity.High.ToString();
                    break;
                case SysXExceptionConsts.OUT_OF_MEMORY_EXCEPTION:
                    _severityStatus = Severity.High.ToString();
                    break;
                case SysXExceptionConsts.STACK_OVER_FLOW_EXCEPTION:
                    _severityStatus = Severity.Medium.ToString();
                    break;
                case SysXExceptionConsts.OPTIMISTIC_CONCURRENCY_EXCEPTION:
                    _severityStatus = Severity.High.ToString();
                    break;
                case SysXExceptionConsts.UPDATE_EXCEPTION:
                    _severityStatus = Severity.High.ToString();
                    break;
                case SysXExceptionConsts.INVALID_CAST_EXCEPTION:
                    _severityStatus = Severity.Medium.ToString();
                    break;
                case SysXExceptionConsts.COM_EXCEPTION:
                    _severityStatus = Severity.Medium.ToString();
                    break;
                case  SysXExceptionConsts.SHE_EXCEPTION:
                    _severityStatus = Severity.Medium.ToString();
                    break;
                case  SysXExceptionConsts.ACCESS_VIOLATION_EXCEPTION:
                    _severityStatus = Severity.Medium.ToString();
                    break;
                default:
                    _severityStatus = Severity.Medium.ToString();
                    break;
            }

            return _severityStatus;
        }

        #endregion

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}