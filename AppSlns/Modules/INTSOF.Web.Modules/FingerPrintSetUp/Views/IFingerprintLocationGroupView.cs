using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IFingerprintLocationGroupView
    {
        Int32 CurrentLoggedInUserID { get; }
        Int32 PageSize { get; set; }
        Int32 CurrentPageIndex { get; set; }
        Int32 VirtualRecordCount { get; set; }
        CustomPagingArgsContract GridCustomPaging { get; set; }
        List<FingerprintLocationGroupContract> LocationGroupLst { get; set; }
        FingerprintLocationGroupContract filterContract { get; set; }
    }
}
