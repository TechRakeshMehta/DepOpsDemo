using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using System;
using System.Collections.Generic;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRequirementPackageCategoryDetail
    { 
        List<RequirementCategoryContract> CategoryData { get; set; }
        Int32 RotationID { get; set; }
        Int32 TenantID { get; set; }
        Boolean IsStudentPackage { get; set; }
    }
}
