using System;

namespace CoreWeb.Shell.Views
{
    public interface IIsActiveToggle
    {
        Boolean Checked { get; set; }
        String IsActiveToggleYesLabel { set; }
        String IsActiveToggleNoLabel { set; }
        Boolean IsActiveEnable { get; set; }
        Boolean IsAutoPostBack { get; set; }
    }
}
