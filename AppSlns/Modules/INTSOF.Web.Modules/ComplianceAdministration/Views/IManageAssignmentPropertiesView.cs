#region Namespaces

#region SystemDefined

using System.Collections.Generic;
using System.Linq;
using System;

#endregion

#region UserDefined

using Entity.ClientEntity;

#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IManageAssignmentPropertiesView
    {
        #region Variables

        #region Private Variables
        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties

        List<Tenant> ListTenants
        {
            set;
            get;
        }

        List<GetRuleSetTree> LstAssignmentPropertiesTreeData
        {
            set;
            get;
        }

        Int32 SelectedTenant
        {
            set;
            get;
        }

        Int32 CurrentUserId
        {
            get;
        }

        //UAT-2717
        List<CompliancePackage> ListCompliancePackages
        {
            get;
            set;
        }

        List<Int32> LstSelectedPackageIDs
        {
            get;
            set;
        }

        //UAT-3032
        Int32 PreferredSelectedTenantID { get; set; }
        #endregion

        #endregion

        #region Methods

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}




