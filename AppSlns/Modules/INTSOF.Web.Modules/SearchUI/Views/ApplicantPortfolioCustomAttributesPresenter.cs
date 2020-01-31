#region Namespaces

#region SystemDefined

using System;
using INTSOF.SharedObjects;
using System.Collections.Generic;
using System.Linq;

#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.Utils;
using Entity.ClientEntity;

#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public class ApplicantPortfolioCustomAttributesPresenter : Presenter<IApplicantPortfolioCustomAttributesView>
    {

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads               
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// Get DepartmentProgramMappingIds from ApplicantHierarchyMapping Table
        /// </summary>
        public void GetDepartmentMappingId()
        {
            View.ListDepartmentProgramIds = ComplianceDataManager.GetDepartmentProgramMappingId(View.TenantId, View.CurrentUserId);
        }
       
        /// <summary>
        /// Get the DepartmentProgramMapping Record based on DepartmentProgramMappingIds
        /// </summary>
        public void GetDepartmentProgramMappingRecord()
        {
            if (View.ListDepartmentProgramIds.IsNotNull())
            {
                View.DepartmentProgramMapping = ComplianceDataManager.GetDepartmentProgramMappingRecord(View.TenantId, View.ListDepartmentProgramIds);
            }
        }
    }
}




