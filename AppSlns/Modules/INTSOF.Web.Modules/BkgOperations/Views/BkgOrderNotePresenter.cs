using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public class BkgOrderNotePresenter : Presenter<IBkgOrderNoteView>
    {
        public override void OnViewLoaded()
        {
            //GetBkgOrderNote();
        }

        public override void OnViewInitialized()
        {

        }

        public void GetBkgOrderNote()
        {
            List<BkgOrderQueueNotesContract> lstNotes = BackgroundProcessOrderManager.GetBkgOrderNotes(View.TenantID, View.OrderID).OrderByDescending(col=>col.CreatedOnDate).ToList();
            if (!lstNotes.IsNullOrEmpty())
            {
                lstNotes.ForEach(
                    col =>
                    {
                        col.CreatedOnDate = col.CreatedByID == AppConsts.NONE ? "Unknown" : col.CreatedOnDate;
                    }
                    );
            }
            View.LstNotes = lstNotes;
        }

        public Boolean SaveBkgOrderNote()
        {
            return BackgroundProcessOrderManager.SaveBkgOrderNote(View.TenantID, View.OrderID, View.Notes, View.CurrentUserId);
        }
    }
}
