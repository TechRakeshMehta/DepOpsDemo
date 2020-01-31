using INTSOF.ServiceDataContracts.Modules.AgencyJobBoard;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyJobBoard.Views
{
    public interface IViewAgencyJobPostingView
    {
        IViewAgencyJobPostingView CurrentViewContext { get; }

        Int32 CurrentLoggedInUserId { get; }

        Int32 OrganisationUserID { get; }

        String JobTitle { get; set; }

        String Company {get; set;}

        String Location { get; set; }

        String JobTypeCode { get; set; }

        List<AgencyJobContract> LstAgencyJobPosting { get; set; }

        Boolean IsAppliacnt { get; set; }

        Int32 TenantId { get; set; }

        List<TenantDetailContract> LstTenant { set; }

        List<Int32> SelectedTenantIds { get; set; }

        Boolean IsAdminLoggedIn { get; set; }

        List<DefinedRequirementContract> LstJobFieldType { get; set; }

        Int32 SelectedJobFieldTypeID { get; set; }

        #region Custom paging parameters
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        Int32 PageSize
        {
            get;
            set;
        }

        Int32 VirtualRecordCount
        {
            get;
            set
           ;
        }

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }
        #endregion
    }
}
