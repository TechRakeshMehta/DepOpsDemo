using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface ISharedUserCustomAttributeDisplayControlView
    {
        CustomAttribteContract TypeCustomtAttribute { get; set; }
        ISharedUserCustomAttributeDisplayControlView CurrentViewContext { get; }
        Int32 SelectedRecordId
        {
            get;
            set;
        }
    }
}




