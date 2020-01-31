using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreWeb.ApplicantModule.Views
{
    public interface IBasePageView
    {
        string PageName { get; }
        string HtmlMarkup { set; }        
    }
}
