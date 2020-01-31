using System;
using System.Collections.Generic;
using System.Text;

using INTSOF.RuleEngine.Expression;

namespace INTSOF.RuleEngine.Model
{
    internal class Expression
    {
        public string Name { get; set; }
        public string Definition { get; set; }
        public string UILabel 
        {
            get
            {
                return Operators.GetUILabel(Definition);
            }
        }
    }
}
