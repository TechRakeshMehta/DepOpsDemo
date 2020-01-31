using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ProfileSharing.Views
{
    public interface ISchoolRepresentativeDetailsView
    {
        //ISchoolRepresentativeDetailsView CurrentViewContext { get; }
        Int32 TenantID { get; set; }
        Int32 AgencyID { get; set; } //null type?
        //List<ClientContactContract> SelectedSchoolAdminList
        //{
        //    get;
        //    set;
        //}CurrentLoggedInUserId
        //List<TenantDetailContract> lstTenant { get; set; }
        List<AgencyDetailContract> lstAgency { get; set; }
        String StrSelectedTenantIds
        {
            get;
            set;
        }
        List<TenantDetailContract> lstTenants
        {
            get;
            set;
        }
        List<TenantDetailContract> lstSelectedTenants
        {
            get;
        }
        List<AgencyClientContact> lstClientData { get; set; }

        Int32 CurrentUserId { get; }

        List<String> SharedUserTypeCodes { get; }

        Dictionary<Int32, String> SelectedUsersForEmail { get; set; }

        CustomPagingArgsContract customPagingArgsContract { get; set; }
       
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// PageSize
        /// </summary>
        /// <value> Gets the value for PageSize.</value>
        Int32 PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// VirtualPageCount
        /// </summary>
        /// <value> Sets the value for VirtualPageCount.</value>
        Int32 VirtualPageCount
        {
            set;
            get;
        }

    }
}
