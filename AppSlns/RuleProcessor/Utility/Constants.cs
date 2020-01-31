//------------------------------------------------------------------------------
// <copyright file="CSSqlClassFile.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace INTSOF.RuleEngine.Utility
{
    internal class Constants
    {
        public const string TOKEN_DELIMITER = "$$$";
        public const string OPERAND_EMPTY_TEXT = "$$NULL$$";
        public const string OPERAND_CURRENT_DAY = "$$TDAY$$";
        public const string OPERAND_CURRENT_MONTH = "$$TMTH$$";
        public const string OPERAND_CURRENT_YEAR = "$$TYR$$";
        public const string OPERAND_EMPTY_TEXT_EXPR = "##NULL##";
        public const string RESULT_ERROR = "##ERROR##";
    }
}
