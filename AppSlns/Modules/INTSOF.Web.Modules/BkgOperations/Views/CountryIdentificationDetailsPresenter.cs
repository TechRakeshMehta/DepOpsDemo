using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.BkgOperations;
using Business.RepoManagers;

namespace CoreWeb.BkgOperations.Views
{
    public class CountryIdentificationDetailsPresenter
    {
        public List<CountryIdentificationDetailContract> GetCountryIdentificationDetails()
        {
            return BackgroundProcessOrderManager.GetCountryIdentificationDetails();

        }

    }
}
