using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.BkgSetup.Views
{
    public interface IManageSvcItemCustomFormsView
    {
        IManageSvcItemCustomFormsView CurrentViewContext
        {
            get;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 currentLoggedInUserId
        {
            get;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        String InfoMessage
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }

        Int32 PackageServiceItemID { get; set; }

        List<Entity.CustomForm> lstCustomFormSupplementary { get; set; }

        Int32? SelectedCustomFormId { get; set; }

        List<PkgServiceItemCustomFormMappingDetails> lstPkgServiceItemCustomFormMappingDetails { get; set; }
    }
}
