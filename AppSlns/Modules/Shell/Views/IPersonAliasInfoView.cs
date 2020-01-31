using System;
using System.Collections.Generic;
using System.Linq;
using Entity.ClientEntity;
using INTSOF.UI.Contract.FingerPrintSetup;
namespace CoreWeb.Shell.Views
{
    public interface IPersonAliasInfoView
    {
        Boolean IsLocationServiceTenant { get; }
        List<Entity.lkpSuffix> lstAliasSuffixes { get; set; }
        Boolean IsSuffixDropDownType { get; set; }
    }
}
