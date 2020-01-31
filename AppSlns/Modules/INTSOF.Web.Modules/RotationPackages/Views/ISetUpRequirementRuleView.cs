using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.RotationPackages.Views
{
    public interface ISetUpRequirementRuleView
    {
        Int32 RequirementObjectTreeID
        {
            get;
            set;
        }

        List<RequirementRuleContract> lstItemRules
        {
            get;
            set;
        }

        Boolean IsVersionRequired
        {
            get;
            set;
        }

        Boolean IsViewMode
        {
            get;
            set;
        }

        Boolean IsEditMode
        {
            get;
            set;
        }

        List<RulesConstantTypeContract> LstRulesConstantTypeContract
        {
            get;
            set;
        }

        List<RequirementFieldContract> lstItemFields
        {
            get;
            set;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        Int32 RequirementCategoryID { get; set; }

        Int32 RequirementItemID { get; set; }


        List<RequirementItemContract> lstCategoryItems { get; set; }

        String Hpath
        {
            get;
            set;
        }

        //UAT-3342 
        Boolean IsCalculatedAttribute
        {
            get;
            set;
        }

        Int32 fieldID
        {
            get;
            set;
        }
        
    }
}
