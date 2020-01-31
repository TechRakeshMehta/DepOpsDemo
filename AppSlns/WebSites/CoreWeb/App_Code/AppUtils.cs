using System;
using System.Web;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using INTERSOFT.WEB.UI.WebControls;
using Telerik.Web.UI;
using System.ComponentModel;
using CoreWeb.Shell;
using Business.RepoManagers;
using Entity;
using System.Linq;
using INTSOF.UI.Contract.SysXSecurityModel;


namespace CoreWeb
{
    /// <summary>
    /// Summary description for AppUtils
    /// </summary>
    public static class AppUtils
    {

        #region Extension Methods
        /// <summary>
        /// Display message on label
        /// </summary>
        /// <param name="label"></param>
        /// <param name="message"></param>
        /// <param name="messageType"></param>

        public static void ShowMessage(this Label label, String message, MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Error:
                    label.Text = message;
                    label.CssClass = "error";
                    break;
                case MessageType.Information:
                    label.Text = message;
                    label.CssClass = "info";
                    break;
                case MessageType.SuccessMessage:
                    label.Text = message;
                    label.CssClass = "sucs";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Adds the first empty item.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        /// <remarks></remarks>
        public static void AddFirstEmptyItem(this WclComboBox comboBox)
        {
            comboBox.Items.Insert(AppConsts.NONE, new RadComboBoxItem { Selected = true, Text = AppConsts.COMBOBOX_ITEM_SELECT, Value = AppConsts.NONE.ToString() });
        }


        /// <summary>
        /// Extension method to clear the text of text box.
        /// </summary>
        /// <param name="WclTextBox"></param>
        public static void ClearText(this WclTextBox WclTextBox)
        {
            WclTextBox.Text = String.Empty;
        }


        /// <summary>
        /// Extension method to clear the text of text box.
        /// </summary>
        /// <param name="WclTextBox"></param>
        public static void ClearText(this WclNumericTextBox sysXNumericTextBox)
        {
            sysXNumericTextBox.Text = String.Empty;
        }

        #endregion

        public class GridGenericFilterer<T>
        {
            GridFilterExpression _expression = null;
            PropertyDescriptor _descriptor = null;
            Type _propertyType = null;
            GridKnownFunction _filterFunction;

            public GridGenericFilterer(GridFilterExpression expression)
            {
                _expression = expression;
                _descriptor = TypeDescriptor.GetProperties(GetType().GetGenericArguments()[0]).Find(expression.FieldName, true);
                _propertyType = _descriptor.PropertyType;
                _filterFunction = (GridKnownFunction)Enum.Parse(typeof(GridKnownFunction), _expression.FilterFunction);
            }

            public bool Filter(T item)
            {
                object value = _descriptor.GetValue(item);
                string stringValue = value.ToString();

                if (_propertyType == typeof(String))
                {
                    if (_filterFunction == GridKnownFunction.Contains)
                    {
                        return stringValue.Contains(_expression.FieldValue);
                    }
                    else if (_filterFunction == GridKnownFunction.DoesNotContain)
                    {
                        return !stringValue.Contains(_expression.FieldValue);
                    }
                    else if (_filterFunction == GridKnownFunction.StartsWith)
                    {
                        return stringValue.StartsWith(_expression.FieldValue);
                    }
                    else if (_filterFunction == GridKnownFunction.EndsWith)
                    {
                        return stringValue.EndsWith(_expression.FieldValue);
                    }
                    else if (_filterFunction == GridKnownFunction.EqualTo)
                    {
                        return stringValue.Equals(_expression.FieldValue);
                    }
                    else if (_filterFunction == GridKnownFunction.NotEqualTo)
                    {
                        return !stringValue.Equals(_expression.FieldValue);
                    }
                    else if (_filterFunction == GridKnownFunction.IsEmpty)
                    {
                        return stringValue.Equals(String.Empty);
                    }
                    else if (_filterFunction == GridKnownFunction.NotIsEmpty)
                    {
                        return !stringValue.Equals(String.Empty);
                    }
                    else if (_filterFunction == GridKnownFunction.GreaterThan)
                    {
                        return stringValue.CompareTo(_expression.FieldValue) > 0;
                    }
                    else if (_filterFunction == GridKnownFunction.GreaterThanOrEqualTo)
                    {
                        return stringValue.CompareTo(_expression.FieldValue) >= 0;
                    }
                    else if (_filterFunction == GridKnownFunction.LessThan)
                    {
                        return stringValue.CompareTo(_expression.FieldValue) < 0;
                    }
                    else if (_filterFunction == GridKnownFunction.LessThanOrEqualTo)
                    {
                        return stringValue.CompareTo(_expression.FieldValue) <= 0;
                    }
                }
                else if (_propertyType == typeof(Boolean))
                {
                    if (_filterFunction == GridKnownFunction.EqualTo)
                    {
                        return Boolean.Parse(stringValue).Equals(Boolean.Parse(_expression.FieldValue));
                    }
                    else if (_filterFunction == GridKnownFunction.NotEqualTo)
                    {
                        return !Boolean.Parse(stringValue).Equals(Boolean.Parse(_expression.FieldValue));
                    }
                }
                else if (_propertyType == typeof(DateTime))
                {
                    if (_filterFunction == GridKnownFunction.EqualTo)
                    {
                        return DateTime.Parse(stringValue).Equals(DateTime.Parse(_expression.FieldValue));
                    }
                    else if (_filterFunction == GridKnownFunction.NotEqualTo)
                    {
                        return !DateTime.Parse(stringValue).Equals(DateTime.Parse(_expression.FieldValue));
                    }
                    else if (_filterFunction == GridKnownFunction.GreaterThan)
                    {
                        return DateTime.Parse(stringValue).CompareTo(DateTime.Parse(_expression.FieldValue)) > 0;
                    }
                    else if (_filterFunction == GridKnownFunction.GreaterThanOrEqualTo)
                    {
                        return DateTime.Parse(stringValue).CompareTo(DateTime.Parse(_expression.FieldValue)) >= 0;
                    }
                    else if (_filterFunction == GridKnownFunction.LessThan)
                    {
                        return DateTime.Parse(stringValue).CompareTo(DateTime.Parse(_expression.FieldValue)) < 0;
                    }
                    else if (_filterFunction == GridKnownFunction.LessThanOrEqualTo)
                    {
                        return DateTime.Parse(stringValue).CompareTo(DateTime.Parse(_expression.FieldValue)) <= 0;
                    }
                }
                else if (IsNumeric(_propertyType))
                {
                    if (_filterFunction == GridKnownFunction.EqualTo)
                    {
                        return Decimal.Parse(stringValue).Equals(Decimal.Parse(_expression.FieldValue));
                    }
                    else if (_filterFunction == GridKnownFunction.NotEqualTo)
                    {
                        return !Decimal.Parse(stringValue).Equals(Decimal.Parse(_expression.FieldValue));
                    }
                    else if (_filterFunction == GridKnownFunction.GreaterThan)
                    {
                        return Decimal.Parse(stringValue).CompareTo(Decimal.Parse(_expression.FieldValue)) > 0;
                    }
                    else if (_filterFunction == GridKnownFunction.GreaterThanOrEqualTo)
                    {
                        return Decimal.Parse(stringValue).CompareTo(Decimal.Parse(_expression.FieldValue)) >= 0;
                    }
                    else if (_filterFunction == GridKnownFunction.LessThan)
                    {
                        return Decimal.Parse(stringValue).CompareTo(Decimal.Parse(_expression.FieldValue)) < 0;
                    }
                    else if (_filterFunction == GridKnownFunction.LessThanOrEqualTo)
                    {
                        return Decimal.Parse(stringValue).CompareTo(Decimal.Parse(_expression.FieldValue)) <= 0;
                    }
                }

                if (_filterFunction == GridKnownFunction.IsNull)
                {
                    return value.Equals(null);
                }
                else if (_filterFunction == GridKnownFunction.NotIsNull)
                {
                    return !value.Equals(null);
                }

                return true;
            }

            static bool IsNumeric(Type type)
            {
                if (!type.IsEnum)
                {
                    switch (Type.GetTypeCode(type))
                    {
                        case TypeCode.Char:
                        case TypeCode.SByte:
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                }
                return false;
            }
        }

        #region Properties

        public static String GetInstitutionName
        {
            get
            {
                InstitutionDataContract currentInstitutionData = GetInstitutionData();
                if (currentInstitutionData.IsNotNull())
                    return currentInstitutionData.InstitutionName;
                return String.Empty;
            }
        }

        public static String GetInstitutionTypeCode
        {
            get
            {
                InstitutionDataContract currentInstitutionData = GetInstitutionData();
                if (currentInstitutionData.IsNotNull())
                    return currentInstitutionData.InstitutionTypeCode;
                return String.Empty;
            }
        }

        #endregion


        #region Private Methods

        private static InstitutionDataContract GetInstitutionData()
        {
            String SiteUrl = HttpContext.Current.Request.Url.Host;
            InstitutionDataContract currentInstitutionData = null;
            //InstitutionData currentInstitutionData = null;
            currentInstitutionData = (InstitutionDataContract)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_KEY_INSTITUTIONDATA);

            if (currentInstitutionData.IsNull())
            {
                var website = WebSiteManager.GetWebSite(SiteUrl);
                if (website.IsNotNull())
                {
                    Tenant tenant = website.TenantWebsiteMappings.FirstOrDefault(twm => twm.TWM_IsDeleted == false).Tenant;
                    if (tenant.IsNotNull())
                    {
                        currentInstitutionData = new InstitutionDataContract();
                        currentInstitutionData.InstitutionName = tenant.TenantName;
                        if (tenant.lkpTenantType.IsNotNull())
                        {
                            currentInstitutionData.InstitutionTypeCode = tenant.lkpTenantType.TenantTypeCode;
                        }
                        else
                        {
                            currentInstitutionData.InstitutionTypeCode = String.Empty;
                        }
                        currentInstitutionData.InstitutionURL = SiteUrl.ToLower();
                        SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_KEY_INSTITUTIONDATA, currentInstitutionData);
                    }
                }
            }
            return currentInstitutionData;
        }

        #endregion

        #region Public methods

        public static String GetIdForHierarchy(String nodeId, String type)
        {
            String[] ids = nodeId.Split('_');
            switch (type)
            {
                case "PKG":
                    return ids[1];
                case "SVCG":
                    return ids[2];
                case "SVC":
                    {
                        if (ids[4] == "NA")
                            return ids[3];
                        else
                            return ids[4];
                    }
                case "ATTG":
                    return ids[5];
                case "ATT":
                    return ids[6];
                default:
                    return String.Empty;
            }
        }

        #endregion
    }

   
}
