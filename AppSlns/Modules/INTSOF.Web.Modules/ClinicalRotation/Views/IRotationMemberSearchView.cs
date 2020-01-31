#region NameSpace

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Project Specific
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using Entity.ClientEntity;
#endregion

#endregion

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRotationMemberSearchView
    {
        List<lkpArchiveState> lstArchiveState { set; }//UAT-2545
        List<String> SelectedArchiveStatusCode { get; set; } //UAT-2545
        List<RotationMemberSearchDetailContract> LstRotationMemberSearchData
        {
            get;
            set;
        }

        Int32 TenantID
        {
            get;
            set;
        }

        Int32 SelectedTenantID
        {
            get;

            set;

        }
        Boolean IsReset
        {
            get;
            set;
        }
        List<TenantDetailContract> lstTenant
        {
            get;
            set;
        }

        List<AgencyDetailContract> lstAgency
        {
            get;
            set;
        }

        Int32 SelectedAgencyID
        {
            get;
            set;
        }

        List<ClientContactContract> ClientContactList
        {
            get;
            set;
        }

        List<WeekDayContract> WeekDayList
        {
            get;
            set;
        }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        RotationMemberSearchDetailContract SearchContract
        {
            get;
            set;
        }

        List<ApplicantDocumentContract> LstApplicantDocumentToExport { get; set; }
        List<RotationMemberSearchDetailContract> LstApplicantRotationToExport { get; set; }

        //Represents ErrorMessage
        String ErrorMessage
        {

            get;
            set;
        }

        //Represents SuccessMessage
        String SuccessMessage
        {
            get;
            set;
        }


        List<Entity.ClientEntity.UserGroup> lstUserGroup { get; set; }
       
        Int32 SelectedUserGroupID { get; set; }

        //UAT-3749
        //Int32 SelectedUserTypeID { get; set; }
        Dictionary<String, String> dicUserTypes {get;set;}
            
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

        //UAT-1881
        Int32 CurrentUserID { get; }

        /// <summary>
        /// UAT-3549
        /// </summary>
        Int32 CurrentLoggedInUserId
        {
            get;
        }
    }
}
