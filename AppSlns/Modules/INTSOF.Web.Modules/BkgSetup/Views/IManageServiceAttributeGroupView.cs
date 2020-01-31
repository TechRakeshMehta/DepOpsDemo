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
    public interface IManageServiceAttributeGroupView
    {

        Int32 SelectedTenantId
        {
            get;
            set;
        }

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

        ManageServiceAttributeGrpContract ViewContract
        {
            get;
          
        }

        String ErrorMessage
        {
            get;
            set;
        }
        List<ManageServiceAttributeGrpContract> SvcAttributeGrpLst
        {
            get;
            set;
        }
        Int32 DefaultTenantId
        {
            get;
            set;
        }
        List<BkgSvcAttributeGroup> ListAttributeGrps
        {
            get;
            set;
        }
        Int32 SelectedAttributeGrp
        {
            get;
            set;
        }
        List<BkgSvcAttribute> ListAttributes
        {
            get;
            set;
        }
        List<Int32> SelectedAttributes
        {
            get;
            set;
        }
    }
}
