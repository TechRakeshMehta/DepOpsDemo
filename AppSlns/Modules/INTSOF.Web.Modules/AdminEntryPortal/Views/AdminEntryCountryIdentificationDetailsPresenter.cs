using Business.RepoManagers;
using INTSOF.UI.Contract.BkgOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AdminEntryPortal.Views
{
    public class AdminEntryCountryIdentificationDetailsPresenter
    {
        public List<CountryIdentificationDetailContract> GetCountryIdentificationDetails()
        {
            return BackgroundProcessOrderManager.GetCountryIdentificationDetails();

        }
    }
}
