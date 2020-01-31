using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public interface IManageBkgServiceCustomFormView
    {
        Int32 ServiceId
        {
            get;
            set;
        }

        String ServiceName
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;

        }
        List<ManageServiceCustomFormContract> MappedCustomFormList
        {
            get;
            set;
        }
        List<CustomForm> CustomFormList
        {
            get;
            set;
        }
        String ErrorMessage
        {
            get;
            set;
        }
    }
}
