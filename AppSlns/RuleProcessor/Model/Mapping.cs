using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using INTSOF.RuleEngine.Expression;
using INTSOF.RuleEngine.Utility;

namespace INTSOF.RuleEngine.Model
{
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    internal class ObjectMapping//:List<Mapping>
    {
        public List<Mapping> Mappings { get; set; }
        public string getMappedName(string objectName)
        {
            string mappedName = string.Empty;

            Mapping mapping = Mappings.Find(m => m.Key.Equals(objectName));
            if (mapping != null)
            {
                if (!(string.IsNullOrEmpty(mapping.MappedName)))
                    mappedName = "["+mapping.MappedName+"]";
                else
                    mappedName = mapping.Key;
            }

            return mappedName;
        }

        public bool SetHappyValue()
        {
            bool hasImpact = false;
            foreach (Mapping map in Mappings)
            {
                if (!string.IsNullOrEmpty(map.HappyValue))
                {
                    map.Value = map.HappyValue;
                    hasImpact = true;
                }
            }
            return hasImpact;
        }
    }

    internal class Mapping
    {
        private string _value;

        public int ID { get; set; }
        public string Key { get; set; }
        public string MappedName { get; set; }
        public string Value
        {
            get { return _value; }
            set
            {
                if (!String.IsNullOrEmpty(DataType))
                {
                    OperandType type = Operand.GetOperandTypeByTypeString(DataType);
                    string typeSuffix = Helper.ToDescriptionString(type);
                    if ((!string.IsNullOrEmpty(typeSuffix)) && typeSuffix.Contains("##") && (!value.Contains(typeSuffix)))
                        value = value + typeSuffix;
                }
                if (value.Equals(Constants.OPERAND_EMPTY_TEXT, StringComparison.OrdinalIgnoreCase))
                    value = Constants.OPERAND_EMPTY_TEXT_EXPR;
                else if (value.Equals(Constants.OPERAND_CURRENT_DAY, StringComparison.OrdinalIgnoreCase))
                    value = DateTime.Today.ToShortDateString();
                else if (value.Equals(Constants.OPERAND_CURRENT_MONTH, StringComparison.OrdinalIgnoreCase))
                    value = DateTime.Today.Month.ToString();
                else if (value.Equals(Constants.OPERAND_CURRENT_YEAR, StringComparison.OrdinalIgnoreCase))
                    value = DateTime.Today.Year.ToString();
                _value = value;
            }
        }
        public string DataType { get; set; }
        public string HappyValue { get; set; }
    }
}
