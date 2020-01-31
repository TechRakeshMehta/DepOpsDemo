using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;

using Business.RepoManagers;
using INTSOF.Utils;

namespace CoreWeb.Templates.Views
{
    public class ReadOnlyTemplatesGridPresenter : Presenter<IReadOnlyTemplatesGridView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }
        public void GetOtherTemplates()
        {
            View.OtherTemplates = TemplatesManager.GetOtherTemplates();
        }
    }
}
