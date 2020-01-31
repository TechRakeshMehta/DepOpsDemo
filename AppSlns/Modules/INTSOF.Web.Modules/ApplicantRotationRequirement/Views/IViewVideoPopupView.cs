using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;


namespace CoreWeb.ApplicantRotationRequirement.Views
{
    public interface IViewVideoPopupView
    {
        /// <summary>
        /// 
        /// </summary>
        Int32 RequrmntFieldVideoID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Int32 TenantID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Int32 RequrmntObjTreeID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Boolean IsEditMode { get; set; }

        IViewVideoPopupView CurrentViewContext { get; }

        /// <summary>
        /// 
        /// </summary>
        RequirementFieldVideoData RequrmntVideoData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        RequirementObjectTreeContract RequrmntObjTreePropertyContract { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ObjectAttributeContract ObjectAttrContract { get; set; }

        /// <summary>
        /// 
        /// </summary>
        RequirementPackageContract RequirementPackageContractSessionData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        String VideoPreviewURL { get; set; }

        String IsFromAdmin { get; set; }

    }
}
