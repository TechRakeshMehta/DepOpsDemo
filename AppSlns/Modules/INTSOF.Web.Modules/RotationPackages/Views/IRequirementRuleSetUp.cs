using Entity.SharedDataEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using System;
using System.Collections.Generic;

namespace CoreWeb.RotationPackages.Views
{
    public interface IRequirementRuleSetUp
    {

        Int32 CurrentLoggedInUserId { get; }

        IRequirementRuleSetUp CurrentViewContext { get; }
          
        String ErrorMessage
        {
            get;
            set;
        }
 

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 DefaultTenantId
        {
            get;
            set;
        } 

        String ErrMsg
        {
            get;
            set;
        }

        Int32 CurrentCategoryID { get; set; }

        Int32 ParentObjectTreeID { get; set; }

        Boolean IsInEditMode { get; set; }

        Int32 ObjectRuleID { get; set; }

        List<RequirementObjectRule> lstRuleMapping { get; set; }

        //UAT-4657
        Boolean IsDetailsEditable { get; set; }
    }
}

