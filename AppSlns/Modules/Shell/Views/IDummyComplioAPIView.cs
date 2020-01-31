using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace CoreWeb.Shell.Views
{
    public interface IDummyComplioAPIView
    {
        //Int32 TenantId
        //{
        //    get;
        //    set;
        //}
        List<Tenant> ListTenants
        {
            get;
            set;
        }
        List<APIMetaData> ListEntityType
        {
            get;
            set;
        }
        Int32 SelectedTenantID
        {
            get;
        }
        String SelectedEntityTypeCode
        {
            get;
        }
        String InputData
        {
            get;
        }
    }
}
