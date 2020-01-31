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
    public interface ISetupServiceAttributeGroupView
    {

        String ErrorMessage
        {
            get;
            set;
        }

        List<BkgSvcAttributeGroup> ServiceAttributeGroupList
        {
            get;
            set;
        }


        ServiceAttributeGroupContract ViewContract
        {
            get;
        }
    }
}
