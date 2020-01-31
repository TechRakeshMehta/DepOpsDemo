using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public interface IManageFeeItemFeeRecordView
    {
        IManageFeeItemFeeRecordView CurrentViewContext
        {
            get;
        }

        FeeRecordContract ViewContract
        {
            get;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 currentLoggedInUserId
        {
            get;
        }

        String ErrorMessage
        {
            get;
            set;
        }
        
        String SuccessMessage
        {
            get;
            set;
        }
        String InfoMessage
        {
            get;
            set;
        }

        
        /// <summary>
        /// Gets and set List of ServiceItemFeeType
        /// </summary>
        List<LocalFeeRecordsInfo> ListServiceItemFeeRecord
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and set List of All States 
        /// </summary>
        List<Entity.State> ListAllState
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and set List of County corresponding to stateid. 
        /// </summary>
        List<Entity.County> ListCountyByStateId
        {
            set;
            get;
        }

        Int32 SelectedFeeItemId
        {
            get;
            set;
        }

    }
}
