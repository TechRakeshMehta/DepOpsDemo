using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class LanguageTranslatePresenter : Presenter<ILanguageTranslateView>
    {
        public override void OnViewInitialized()
        {
            GetTenants();
            GetLanguages();
        }
        private void GetTenants()
        {
            View.lstTenant = new List<Tenant>();
            View.lstTenant = SecurityManager.GetListOfTenantWithLocationService();
        }

        private void GetLanguages()
        {

            View.lstlanguage = SecurityManager.GetCommLang().Where(X => X.LAN_Code == Languages.SPANISH.GetStringValue()).ToList();
        }

        public void GetLanguageTranslationData()
        {
            if (View.SelectedTenantID > AppConsts.NONE)
            {
                View.lstLanguageRef = FingerPrintSetUpManager.GetLanguageTranslationData(View.SelectedTenantID, View.GridCustomPaging, View.CurrentLoggedInUserID, View.filterContract);
                if (View.lstLanguageRef.IsNotNull() && View.lstLanguageRef.Count > 0)
                {
                    if (View.lstLanguageRef[0].TotalCount > 0)
                    {
                        View.VirtualRecordCount = View.lstLanguageRef[0].TotalCount;
                    }
                    View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                }
                else
                {
                    View.VirtualRecordCount = 0;
                    View.CurrentPageIndex = 1;
                }
            }

        }

        /// <summary>
        /// Method to save and update categories
        /// </summary>
        /// <returns></returns>
        public bool SaveLanguageTranslateDetails(LanguageTranslateContract languageTranslateContract)
        {
            return FingerPrintSetUpManager.SaveLanguageTranslateDetails(languageTranslateContract, View.CurrentLoggedInUserID,View.SelectedTenantID);
         //   return serviceResponse.Result;
        }
        //public void GetAuditHistory()
        //{
        //    View.lstAppAuditHistory = new List<LocationServiceAppointmentAuditContract>();
        //    if (!View.TenantIDs.IsNullOrEmpty())
        //        View.lstAppAuditHistory = FingerPrintSetUpManager.GetAuditHistoryList(View.TenantIDs, View.GridCustomPaging, View.CurrentLoggedInUserID, View.filterContract);
        //    if (View.lstAppAuditHistory.Count > 0)
        //        View.VirtualRecordCount = View.lstAppAuditHistory.FirstOrDefault().TotalCount;
        //    else
        //        View.VirtualRecordCount = AppConsts.NONE;
        //}
    }
}
