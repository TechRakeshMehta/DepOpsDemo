using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using Business.RepoManagers;

namespace CoreWeb.BkgSetup.Views
{
    public class CascadingAttributeDetailPresenter : Presenter<ICascadingAttributeDetailView>
    {
        public void GetAttributeOptions()
        {
            View.AttributeOptions = BackgroundSetupManager.GetCascadingAttributeOptions(View.SelectedTenantId, View.SelectedAttributeId);
            //var attributeOptions = new List<CascadingAttributeOptionsContract>();
            //for(var i = 0; i < 10; i++)
            //{
            //    for (var j = 0; j < 10; j++)
            //    {
            //        attributeOptions.Add(new CascadingAttributeOptionsContract
            //        {
            //            Id = i + j,
            //            AttributeId = View.SelectedAttributeId,
            //            DisplaySequence = j,
            //            Value = "Value" + j,
            //            SourceValue = "Source" + i
            //        });
            //    }
            //}

            //View.AttributeOptions = attributeOptions;
        } 

        public Boolean AddUpdateAttributeOption()
        {
            Boolean bResult = false;
            if (View.CurrentOption.IsNotNull() 
                && View.CurrentOption.AttributeId > Convert.ToInt32(DefaultNumbers.None)
                && !string.IsNullOrWhiteSpace(View.CurrentOption.Value))
            {
                bResult = BackgroundSetupManager.SaveCascadingAttributeOption(View.SelectedTenantId, View.CurrentOption, View.CurrentLoggedInUserId);
            }
            return bResult;
        }

        public Boolean DeleteOption()
        {
            Boolean bResult = false;
            if(View.CurrentOption.IsNotNull()
                && View.CurrentOption.Id > Convert.ToInt32(DefaultNumbers.None))
            {
                bResult = BackgroundSetupManager.DeleteCascadingAttributeOption(View.SelectedTenantId, View.CurrentOption.Id, View.CurrentLoggedInUserId);
            }
            return bResult;
        }
    }
}
