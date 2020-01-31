using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using INTSOF.ServiceDataContracts.Modules.Common;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.Utils;


namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IManageAgencyNodeView
    {
        #region Properties

        Int32 SelectedTenantID
        {
            set;
            get;
        }

        Int32 CurrentUserId
        {
            get;
        }
        
        String SuccessMessage { get; set; }

        String ErrorMessage { get; set; }

        String InfoMessage { get; set; }

        List<AgencyNodeContract> lstGetNodeList
        {
            get;
            set;
        }

        
        AgencyNodeContract NodeContract
        {
            get;
            set;
        }

     
        #endregion

        //#region UAT-3652
        //#region Custom paging parameters
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
        //#endregion
        //#endregion

        String AgencyNodeName { get; }
        String Description { get;}
    }
}
