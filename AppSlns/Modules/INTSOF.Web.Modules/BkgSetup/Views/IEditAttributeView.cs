using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public interface IEditAttributeView
    {

        Int32 TenantId
        {
            get;
            set;
        }

        ManageServiceAttributeData ManageServiceAttributeData
        {
            get;
            set;
        }

        ServiceAttributeParameter serviceAttributeParameter
        {
            get;
            set;
        }



        Int32 AttributeId
        {
            get;
        }

        List<lkpSvcAttributeDataType> listAttributeDataType
        {
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        String IsSystemConfigured
        {
            get;
        }

        String IsCompleteEdit
        {
            get;
        }

        String IsEditable
        {
            get;
        }

    }
}
