using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.Templates;
using INTSOF.Utils;
using System.Linq;

namespace CoreWeb.Messaging.Views
{
    public interface ICommunicationArchiveSummaryView
    {
        //commented by :Charanjot for UAT-1427: WB:  
        //IQueryable<CommunicationTemplateContract> CommunicationSummaryList
        //{
        //    get;
        //    set;
        //}

        List<CommunicationTemplateContract> CommunicationSummaryList
        {
            get;
            set;
        }

        ICommunicationArchiveSummaryView CurrentViewContext
        {
            get;
        }

        List<Int32> SystemCommunicationDeliveryIds
        {
            get;
            set;
        }

        Int32 CurrentUserId
        {
            get;
        }

        SearchCommunicationTemplateContract SearchContract { get; set; }

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

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        Int32 PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        //Int32 VirtualPageCount
        //{
        //    set;
        //}

        Int32 VirtualRecordCount
        {
            get;
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

        #endregion
    }
}




