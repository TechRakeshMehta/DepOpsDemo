using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRotationCustomAttribute
    {
        /// <summary>
        /// Represensts the Current Context
        /// </summary>
        IRotationCustomAttribute CurrentViewContext
        {
            get;
        }

        /// <summary>
        /// Represents the Selected TenantID
        /// </summary>
        Int32 TenantID
        {
            get;
            set;
        }

        List<CustomAttribteContract> CustomAttributeList { get; set; }

        List<TenantDetailContract> lstTenantDetail
        {
            get;
            set;
        }
    }
}
