using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using INTSOF.RuleEngine.Utility;

namespace INTSOF.RuleEngine.Expression
{
    internal static class Operand
    {
        public static OperandType GetOperandTypeByValue(string operand)
        {
            try
            {
                Decimal tempD = Decimal.Parse(operand);
                return OperandType.Numeric;
            }
            catch
            {
                try
                {
                    Boolean tempB = Boolean.Parse(operand);
                    return OperandType.Boolean;
                }
                catch
                {
                    try
                    {
                        DateTime tempD = DateTime.Parse(operand);
                        return OperandType.Date;
                    }
                    catch { }
                }
            }
            OperandType timespanType = GetTimepanDataTypeByValue(operand);
            if (timespanType == OperandType.Undefined)
                return OperandType.Text;
            else
                return timespanType;
        }

        public static string GetDummyValue(OperandType type)
        {
            Random rdm = new Random();
            switch (type)
            {
                case OperandType.Numeric: return rdm.Next(1, 12).ToString();
                case OperandType.Text: return ((char)(65+rdm.Next(26))).ToString();
                case OperandType.Date: return DateTime.Today.AddDays(rdm.Next(365)).ToString("MM/dd/yyyy");
                case OperandType.Boolean: return (Boolean.Parse((rdm.Next(100)%2==1).ToString())).ToString();
                case OperandType.TDay: return (rdm.Next(60).ToString());
                case OperandType.TMth: return (rdm.Next(12).ToString());
                case OperandType.TYr: return (DateTime.Today.Year + rdm.Next(70).ToString());
                default: throw new Exception("Invalid Operand Type");
            }
        }

        public static OperandType GetOperandTypeByTypeString(string dataType)
        {
            if (dataType.Equals(OperandType.Boolean.ToString(), StringComparison.OrdinalIgnoreCase))
                return OperandType.Boolean;
            if (dataType.Equals(OperandType.Numeric.ToString(), StringComparison.OrdinalIgnoreCase))
                return OperandType.Numeric;
            if (dataType.Equals(OperandType.Text.ToString(), StringComparison.OrdinalIgnoreCase))
                return OperandType.Text;
            if (dataType.Equals(OperandType.Date.ToString(), StringComparison.OrdinalIgnoreCase))
                return OperandType.Date;
            if (dataType.Equals(OperandType.TDay.ToString(), StringComparison.OrdinalIgnoreCase))
                return OperandType.TDay;
            if (dataType.Equals(OperandType.TMth.ToString(), StringComparison.OrdinalIgnoreCase))
                return OperandType.TMth;
            if (dataType.Equals(OperandType.TYr.ToString(), StringComparison.OrdinalIgnoreCase))
                return OperandType.TYr;

            throw new Exception("Invalid Data Type provided in mapping");
        }
        public static OperandType GetTimepanDataTypeByValue(string value)
        {
            value = value.ToUpper();

            if (value.Contains(Helper.ToDescriptionString(OperandType.TDay)))
                return OperandType.TDay;
            if (value.Contains(Helper.ToDescriptionString(OperandType.TMth)))
                return OperandType.TMth;
            if (value.Contains(Helper.ToDescriptionString(OperandType.TYr)))
                return OperandType.TYr;

            return OperandType.Undefined;
        }
 
    }

    internal enum OperandType
    {
        Text,
        Numeric,
        Date,
        Boolean,
        [Description("##TDAY##")]
        TDay,
        [Description("##TMTH##")]
        TMth,
        [Description("##TYR##")]
        TYr,
        Undefined
    }
}
