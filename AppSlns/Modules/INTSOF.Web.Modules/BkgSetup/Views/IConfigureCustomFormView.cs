using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public interface IConfigureCustomFormView
    {
        IConfigureCustomFormView CurrentViewContext
        {
            get;
        }

        CustomFormConfigurationContract ViewContract
        {
            get;
        }

        Int32 SelectedAttributeGroup
        {
            get;
            set;
        }

        List<BkgSvcAttributeGroup> lstAttributeGroup
        {
            get;
            set;
        }

        List<CustomFormAttributeGroup> CustomFormAttributeGroups
        {
            get;
            set;
        }

        String CustomFormTitle
        {
            get;
            set;
        }

        String CustomFormName
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }


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

        INTSOF.Utils.DisplayColumn DisplayColumnData
        {
            get;
            set;
        }

    }
}
