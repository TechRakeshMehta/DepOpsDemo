using INTSOF.SharedObjects;
using System;
using System.Linq;
using System.Collections.Generic;
using Business.RepoManagers;
using Entity;
using INTSOF.Utils;

namespace CoreWeb.Shell.Views
{
    public class PersonAliasInfoPresenter : Presenter<IPersonAliasInfoView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }
        public void GetSuffixes()
        {
            View.lstAliasSuffixes = new List<Entity.lkpSuffix>();
            View.lstAliasSuffixes = SecurityManager.GetSuffixes();
        }

        public void IsDropDownSuffixType()
        {
            AppConfiguration appConfig = SecurityManager.GetAppConfiguration(AppConsts.SUFFIX_TYPE.ToString());
            View.IsSuffixDropDownType = !appConfig.IsNullOrEmpty() && appConfig.AC_Value == "1" ? true : false;
        }
    }
}

