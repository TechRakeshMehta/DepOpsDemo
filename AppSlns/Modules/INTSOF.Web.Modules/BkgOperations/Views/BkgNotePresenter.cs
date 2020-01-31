using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using CoreWeb.BkgOperations.Views;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;

namespace CoreWeb.BkgOperations.Views
{
    public class BkgNotePresenter: Presenter<IBkgNoteView>
    {


        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        /// <summary>
        /// Called when viwe is initialized.
        /// </summary>
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }


        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public DataTable GetNotesByOrderId()
        {
            return BackgroundProcessOrderManager.GetNotesByOrderId(View.SelectedTenantId, View.OrderID);
        }
        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public Boolean AddNote()
        {
            Int16 BusinessChannelTypeID = ModuleUtility.ModuleUtils.SessionService.BusinessChannelType == null ? (Int16)1 : ModuleUtility.ModuleUtils.SessionService.BusinessChannelType.BusinessChannelTypeID;
            OrderNote orderNote = new OrderNote();
            orderNote.ONTS_Order_ID = View.OrderID;
            orderNote.ONTS_NoteText = View.NewNote;
            orderNote.ONTS_CreatedByID = View.CurrentLoggedInUserId;
            orderNote.ONTS_CreatedOn = DateTime.Now;
            orderNote.ONTS_BusinessChannelTypeID = BusinessChannelTypeID;
            return BackgroundProcessOrderManager.AddNote(View.SelectedTenantId, orderNote);
        }

        public Boolean SaveBkgOrderNote()
        {
            return BackgroundProcessOrderManager.SaveBkgOrderNote(View.SelectedTenantId, View.OrderID, View.NewNote, View.CurrentLoggedInUserId);
        }

        public void GetBkgOrderNote()
        {
            List<BkgOrderQueueNotesContract> lstNotes = BackgroundProcessOrderManager.GetBkgOrderNotes(View.SelectedTenantId, View.OrderID).OrderByDescending(col => col.CreatedOnDate).ToList();
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
    }
}
