using Entity.SharedDataEntity;
using INTSOF.UI.Contract.RotationPackages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public interface IExpressionObjectView
    {
        Int32 TenantId { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        //List<RequirementPackageCategory> lstRequirementPackageCategories
        //{
        //    get;
        //    set;
        //}

        List<RequirementExpressionData> lstRequirementCategoryItems
        {
            get;
            set;
        }

        List<RequirementExpressionData> lstRequirementItemAttributes
        {
            get;
            set;
        }

        List<lkpRuleObjectMappingType> lstRuleObjectMappingType { get; set; }

        List<lkpObjectType> lstObjectType { get; set; }

        String SelectedMappingTypeCode { get; set; }

        List<lkpConstantType> lstConstantType { get; set; }

        List<lkpConstantType> lstConstantGroup { get; set; }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 SelectedTenantId { get; set; }

        List<RequirementExpressionData> RequirementCategories { get; set; }
    }
}
