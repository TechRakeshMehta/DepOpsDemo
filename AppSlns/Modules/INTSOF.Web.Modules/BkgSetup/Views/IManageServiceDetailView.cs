using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface IManageServiceDetailView
    {
         Int32 BkgPackageSvcId
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }
    }
}
