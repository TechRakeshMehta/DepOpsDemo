using Entity;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface ILanguageTranslateView
    {
        Int32 TenantId { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        List<Tenant> lstTenant { get; set; }
        Int32 SelectedTenantID { get; set; }
        String EnglishText { get; set; }
        String SpanishText { get; set; }
        LanguageTranslateContract filterContract { get; set; }
        List<LanguageTranslateContract> lstLanguageRef { get; set; }

        List<lkpLanguage> lstlanguage { get; set; }
        Int32 SelectedLanguageID { get; set; }

        Int32 PageSize { get; set; }
        Int32 CurrentPageIndex { get; set; }
        Int32 VirtualRecordCount { get; set; }
        CustomPagingArgsContract GridCustomPaging { get; set; }

       
    }
}
