using System;

namespace WidgetDataWebService.Utils
{
    public static class WidgetDataConsts
    {
        #region Numbers

        /// <summary>
        /// Constant for numeric 0 (zero).
        /// </summary>
        public const Int32 NONE = 0;

        /// <summary>
        /// Constant for one.
        /// </summary>
        public const Int32 ONE = 1;

        /// <summary>
        /// Constant for two.
        /// </summary>
        public const Int32 TWO = 2;

        /// <summary>
        /// Constant for eight.
        /// </summary>
        public const Int32 EIGHT = 8;

        /// <summary>
        /// Constant for numeric value sixteen.
        /// </summary>
        public const Int32 SIXTEEN = 16;

        #endregion

        #region ChildControls

        public const String ORDER_PAYMENT_DETAILS = @"~\ComplianceOperations\UserControl\OrderPaymentDetails.ascx";

        public const String DASHBOARD = @"~/Dashboard/Default.aspx";

        public const String SUBSCRIPTION_DETAIL = @"UserControl\CompliancePackageDetails.ascx";

        public static String RenewalOrder = @"~\ComplianceOperations\UserControl\RenewalOrder.ascx";

        public static String PackageSubscription = @"~\ComplianceOperations\PackageSubscription.ascx";

        public static String ApplicantPendingOrder = @"~/ComplianceOperations/UserControl/PendingOrder.ascx";


        #endregion
    }
}