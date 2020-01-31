using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.WebSite.Views
{
    public interface ICustomPageContentView
    {
        ICustomPageContentView CurrentViewContext
        {
            get;
        }

        Int32 WebPageId
        {
            get;
            set;
        }

        String PageHTML
        {
            get;
            set;
        }
    }
}




