using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.Main.Views
{
    public interface IDefaultView
    {
        Int32 TenantId
        {
            get;
            set;
        }
    }
}
