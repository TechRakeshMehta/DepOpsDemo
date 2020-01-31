using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public class ApplicantDetailsPresenter: Presenter<IApplicantDetails>
    {
        /// <summary>
        /// Gets Applicant details, related to the Current Background Order
        /// </summary>
        public SupplementOrderApplicantDataContract GetApplicantData(Int32 masterOrderId, Int32 tenantId)
        {
            return StoredProcedureManagers.GetApplicantData(masterOrderId, tenantId);
        }
    }
}
