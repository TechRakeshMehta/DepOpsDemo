using INTSOF.UI.Contract.ComplianceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ISeriesDoseView
    {
        List<SeriesItemContract> lstSeriesItemContract
        {
            get;
            set;
        }

        List<SeriesAttributeContract> lstSeriesAttributeContract
        {
            get;
            set;
        }

        Int32 SeriesId { get; set; }

        Int32 SelectedTenantId { get; set; }

        ISeriesDoseView CurrentViewContext { get; }

        /// <summary>
        /// List of Possible status types after shuffling of items
        /// </summary>
        List<lkpItemStatusPostDataShuffle> lstStatusTypes
        {
            get;
            set;
        }

        /// <summary>
        /// List to Save/Update the data in database
        /// </summary>
        List<SeriesItemContract> lstSeriesItemContractSaveUpdate
        {
            get;
            set;
        }

        /// <summary>
        /// Id of the Current LoggedInUser
        /// </summary>
        Int32 CurrentUserId
        {
            get;
        }
    }
}
