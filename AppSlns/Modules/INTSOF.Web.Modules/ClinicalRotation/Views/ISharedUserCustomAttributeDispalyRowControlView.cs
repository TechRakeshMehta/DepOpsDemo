using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface ISharedUserCustomAttributeDispalyRowControlView
    {
        List<CustomAttribteContract> lstTypeCustomAttributes { get; set; }
        ISharedUserCustomAttributeDispalyRowControlView CurrentViewContext { get; }

        /// <summary>
        /// Id of the PK of any module, used to set the validation groups. Will be null from the database
        /// </summary>
        Int32 SelectedRecordId { get; set; }
    }
}




