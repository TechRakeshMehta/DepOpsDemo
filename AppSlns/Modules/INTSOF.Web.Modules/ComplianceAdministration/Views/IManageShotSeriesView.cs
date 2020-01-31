using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IManageShotSeriesView
    {
        IManageShotSeriesView CurrentViewContext
        {
            get;
        }

        ComplianceCategoryContract ViewContract
        {
            get;
        }

        ComplianceCategory ComplianceCategory
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 CurrentCategoryId
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

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        Int32 DefaultTenantId
        {
            get;
            set;
        }

        String SeriesName
        {
            get;
        }

        String SeriesLabel
        {
            get;
        }

        String SeriesDetails
        {
            get;
        }

        String SeriesDescription
        {
            get;
        }

        Boolean SeriesIsActive
        {
            get;
        }

        Boolean IsAvailablePostApproval
        {
            get;
        }

        Int32 RuleExecutionOrder
        {
            get;
        }
    }
}
