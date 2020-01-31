using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INTSOF.Complio.API.Core
{
    public class ComplioAPIConstants
    {
        public const String ASCIISPACE = "&#32";

        public const String CredentialValidationExpression = @"(?<=^\s*)\s|\s(?=\s*$)";

        public const String AuthorizationHeaderName = "Authorization";

        public const String InvalidUserNamePassword = "invalid username or password";

        public const String SuccessStatusMessge = "Request completed successfully.";

        public const String FailureStatusMessge = "Request could not be completed.";

        public const String XmlDataTag = "xmldata";

        public const String XmlDataOpenTag = "<xmldata>";

        public const String XmlDataCloseTag = "</xmldata>";

        public const String ReportName = "OrderCompletion";

        public const String StatusSuccessType = "Success";

        public const String StatusErrorType = "Error";
        public const String InvalidTokenMessge = "Token could not be validated.";
    }
}