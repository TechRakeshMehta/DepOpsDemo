using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public static class DelegateClass
    {
        //Delegate Declared for the function.
        public delegate void delBindData(Boolean IsCategorySpecificData);

        /// <summary>
        /// Object of the delegate
        /// </summary>
        public static delBindData bindData;
    }
}
