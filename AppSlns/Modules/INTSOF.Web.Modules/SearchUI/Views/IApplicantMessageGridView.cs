using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.Search.Views
{
    public interface IApplicantMessageGridView
    {
        Int32 PageSize { get; }
        Int32 OrganizationUserId { get; }
        List<ApplicantMessageDetails> MessageDetailData { get; set; }
        Int32 GridTotalCount {set; }
        Int32 CurrentPageIndex { get;set; }
    }
}
