using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.CommonControls.Views
{
    public class AgencySearchPresenter : Presenter<IAgencySearch>
    {
        public void SearchAgency()
        {
            View.LstAgencies = ProfileSharingManager.SearchAgency(AgencySearchStausTypes.AVAILABLE.GetStringValue());
        }
    }
}
