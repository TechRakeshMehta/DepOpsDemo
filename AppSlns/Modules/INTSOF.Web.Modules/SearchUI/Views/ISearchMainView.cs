using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.Search.Views
{
    public interface ISearchMainView
    {
        String SearchText
        {
            get;
            set;
        }
        String SearchType
        {
            get;
            set;
        }
    }
}




