using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ProfileSharing.Views
{

    public interface IAgencyApplicantShareHistroy
    {

        Int32 ApplicantId { get; set; }
        Int32 AgencyOrgUserID { get; set; }
        Int32 TenantId { get; set; }
        String ApplicantName { get; set; }
        //public String AgencyName { get; set; }
        //public String InvitationDate { get; set; }
        //public String ReviewStatus { get; set; }
        //public String SharingType { get; set; }
        //Int32 TotalCount { get; set; }
        List<AgencyApplicantShareHistoryContract> lstAgencyApplicantShareHistory { get; set; }
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
            set;
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
