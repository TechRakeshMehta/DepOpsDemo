using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using System;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ClinicalRotation;
using Entity.ClientEntity;
using INTSOF.Utils;


namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRotationStudentDetailsPopupView
    {

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 TenantId
        {
            set;
            get;
        }

        Int32 clinicalRotationId
        {
            get;
        }

        Int32 AgencyID
        {
            set;
            get;
        }

        List<RotationMemberDetailContract> lstRotationMemberDetail
        {
            get;
            set;
        }

        IRotationStudentDetailsPopupView CurrentViewContext { get; }

    }
}
