using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;

namespace CoreWeb.ComplianceOperations.Views
{
    public class DataEntryAssignmentQueuePresenter : Presenter<IDataEntryAssignmentQueueView>
    {
        /// <summary>
        /// Check if logged in user is default tenant/ADB admin
        /// </summary>
        public Boolean IsDefaultTenant
        {
            get
            {
                return View.TenantId == SecurityManager.DefaultTenantID;
            }
        }

    }
}
