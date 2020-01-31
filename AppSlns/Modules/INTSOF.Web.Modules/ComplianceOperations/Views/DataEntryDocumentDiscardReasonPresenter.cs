using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public class DataEntryDocumentDiscardReasonPresenter : Presenter<IDataEntryDocumentDiscardReasonView>
    {

        #region Methods
        public void GetDocumentDiscardReasonList()
        {
           var tempList = ComplianceDataManager.GetDocumentDiscardReasonList(View.TenantId);

           if (tempList!=null)
           {
               tempList.Insert(0, new lkpDocumentDiscardReason { DDR_Name = "--SELECT--", DDR_ID = 0 });
           }
           View.LstDocumentDiscradReason = tempList;
        }

        public short GetDocumentStatusIdByCode(String documentStatusCode)
        {
            return ComplianceDataManager.GetDocumentStatusIdByCode(documentStatusCode);
        }
        #endregion
    }
}
