using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IManualServiceFormsView
    {

        Int32 TenantId
        {
            get;
            set;
        }

        List<ManualServiceFormContract> lstManualServiceForm
        {
            get;
            set;
        }


        Int32 SelectedServiceFormStatusId
        {
            get;
            set;
        }


        /// <summary>
        /// Gets and set List of ServiceItemFeeType
        /// </summary>
        List<Entity.ClientEntity.lkpServiceFormStatu> ListServiceFormStatus
        {
            set;
            get;
        }

        ManualServiceFormsSearchContract SetManualServiceFormsSearchContract
        {
            set;
        }

        IManualServiceFormsView CurrentViewContext
        {
            get;
        }

        //Int32? DeptProgramMappingID
        //{
        //    get;
        //}

        String SelectedDeptProgramMappingID
        {
            get;
        }

        Int32? ServiceFormStatusID
        {
            get;
            set;
        }


        Int32 CurrentLoggedInUserId
        {
            get;
        }

        List<Entity.ClientEntity.Tenant> lstTenant
        {
            set;
        }

        List<Entity.ClientEntity.BackgroundService> lstBackroundServices
        {
            set;
        }

        ManualServiceFormContract lstManualServiceFormSearchContract
        {
            get;
            set;
        }
        List<BackroundServicesContract> lstBackroundServicesContract
        {
            get;
            set;
        }

        /// <summary>
        /// Error Message
        /// </summary>
        String ErrorMessage
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }

        Boolean IsAdminUser
        {
            get;
            set;
        }

        String FirstNameSearch
        {
            get;
        }

        String LastNameSearch
        {
            get;
        }

        Int32? ServiceID
        {
            get;
            set;
        }

        #region Custom Paging Parameters

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        ///// <summary>
        ///// PageSize</summary>
        ///// <value>
        ///// Gets the value for PageSize.</value>
        Int32 PageSize
        {
            get;
            set;
        }

        ///// <summary>
        ///// VirtualPageCount</summary>
        ///// <value>
        ///// Sets the value for VirtualPageCount.</value>
        Int32 VirtualPageCount
        {
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }

        /// <summary>
        /// View Contract
        /// </summary>
        //OrderContract ViewContract
        //{
        //    get;
        //}

        #endregion
    }
}

