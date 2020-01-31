using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public interface IManageCustomFormView
    {
        IManageCustomFormView CurrentViewContext
        {
            get;
        }

        CustomFormContract ViewContract
        {
            get;
        }

        List<CustomForm> CustomForms
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
        List<lkpCustomFormType> LstCutomFormType
        {
            get;
            set;
        }
        Int32 SelectedCustomFormTypeId
        {
            get;
            set;
        }
       

    }
}
