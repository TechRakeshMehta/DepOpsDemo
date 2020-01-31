using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.Contracts;
using Business.RepoManagers;
using Entity.ClientEntity;
using System.IO;
using System.Web;
using INTSOF.ServiceUtil;
using CoreWeb.IntsofLoggerModel.Interface;
using CoreWeb.IntsofExceptionModel.Interface;
using INTSOF.SharedObjects;
using INTSOF.Utils.Consts;
using INTSOF.Utils.CommonPocoClasses;
using INTSOF.UI.Contract.ClinicalRotation;
using System;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using System.Collections.Generic;
using INTSOF.UI.Contract.ClinicalRotation;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace CoreWeb.ClinicalRotation.Views
{
    public class RotationStudentDetailsPopupPresenter : Presenter<IRotationStudentDetailsPopupView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
        }

        public List<RotationMemberDetailContract> GetClinicalRotationMembersDetails()
        {
          return View.lstRotationMemberDetail =  ClinicalRotationManager.GetClinicalRotationMembers(View.TenantId, View.clinicalRotationId, View.AgencyID, View.CurrentLoggedInUserId);
        }

    }
}
