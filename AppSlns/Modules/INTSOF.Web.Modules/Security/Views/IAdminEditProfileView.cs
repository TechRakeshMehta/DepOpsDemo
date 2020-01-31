using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public interface IAdminEditProfileView
    {
        IAdminEditProfileView CurrentViewContext { get; }
        Int32 CurrentLoggedInUserId { get; }
        String UserName { get; set; }
        String FirstName { get; set; }
        String LastName { get; set; }
        String PrimaryEmail { get; set; }
        Boolean IsAdbAdmin { get; }
    }
}




