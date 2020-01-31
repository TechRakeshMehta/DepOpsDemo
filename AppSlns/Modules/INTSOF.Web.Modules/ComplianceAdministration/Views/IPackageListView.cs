using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IPackageListView
    {
        List<CompliancePackage> CompliancePackages { get; set; }


        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        CompliancePackageContract ViewContract
        {
            get;
        }

        /// <summary>
        /// Gets the current UserId
        /// </summary>
        Int32 CurrentLoggedInUserId
        {
            get;
        }

        /// <summary>
        /// Gets the TenantId
        /// </summary>
        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 DefaultTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the Error Message
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

        //UAT-1116: Package selection combo box on package screens
        /// <summary>
        /// To set and get CompliancePackageID
        /// </summary>
        Int32 CompliancePackageID
        {
            get;
            set;
        }

        Int32 NotesPositionId { get; set; }
    }
}




