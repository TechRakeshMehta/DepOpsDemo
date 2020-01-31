using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IManageSeriesDoseView
    {
        IManageSeriesDoseView CurrentViewContext
        {
            get;
        }

        /// <summary>
        /// Id of the Category to which the Dose belongs to
        /// </summary>
        Int32 SelectedCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// Id of the Item for which attributes are to be fetched
        /// </summary>
        Int32 SelectedComplianceItemId
        {
            get;
            set;
        }


        /// <summary>
        /// Id of the Series to which the Dose belongs to
        /// </summary>
        Int32 SelectedSeriesId
        {
            get;
            set;
        }

        /// <summary>
        /// ComplianceItems related to selected Categories
        /// </summary>
        List<ComplianceItem> lstComplianceItems
        {
            get;
            set;
        }

        /// <summary>
        /// Id of the Selected Tenant 
        /// </summary>
        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// ComplianceAttributes related to selected Items
        /// </summary>
        List<ComplianceItemAttribute> lstCompliancItemeAttributes
        {
            get;
            set;
        }

        List<ComplianceAttribute> lstComplianceAttributes
        {
            get;
            set;
        }

        /// <summary>
        /// List of Items selected for Save
        /// </summary>
        List<Int32> lstSelectedComplianceItem
        {
            get;
            set;
        }

        /// <summary>
        /// List of Attributes selected for Save
        /// </summary>
        Dictionary<Int32, Boolean> dicAttributes
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
         
        /// <summary>
        /// Return if it is Edit Mode of the Screen
        /// </summary>
        Boolean IsEditMode
        {
            get;
            set;
        }

        /// <summary>
        /// list of selected attribute id are stored
        /// </summary>
        List<Int32> lstSelectedAttributes
        {
            get;
            set;
        }

        /// <summary>
        /// get the selected Key Attrbiute
        /// </summary>
        Int32 SelectedKeyAttribute
        {
            get;
            set;
        }
    }
}
