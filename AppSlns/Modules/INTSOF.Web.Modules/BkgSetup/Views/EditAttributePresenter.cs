using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public class EditAttributePresenter : Presenter<IEditAttributeView>
    {
        #region Methods

        /// <summary>
        /// Get Attribute Data type list.
        /// </summary>
        public void GetAttributeDataType()
        {
            List<lkpSvcAttributeDataType> tempServiceAttributeDtatType = new List<lkpSvcAttributeDataType>();
            if (View.TenantId > 0)
            {
                tempServiceAttributeDtatType = BackgroundSetupManager.GetAttributeDataType(View.TenantId);
            }
            tempServiceAttributeDtatType.Insert(0, new lkpSvcAttributeDataType { SADT_ID = 0, SADT_Name = "--SELECT--" });

            View.listAttributeDataType = tempServiceAttributeDtatType;
        }

        /// <summary>
        /// Get background service attribute data
        /// </summary>
        /// <returns></returns>
        public BkgSvcAttribute GetBkgSvcAttributeData()
        {
            if (View.TenantId > 0 && View.AttributeId > 0)
            {
                return BackgroundSetupManager.GetBkgSvcAttribute(View.TenantId, View.AttributeId);
            }
            return null;
        }

        /// <summary>
        /// Get background service attribute data
        /// </summary>
        /// <returns></returns>
        public void GetBkgSvcAttributeDataForEdit()
        {

            View.ManageServiceAttributeData = BackgroundSetupManager.GetBkgSvcAttributeData(View.serviceAttributeParameter, View.TenantId);

        }

        /// <summary>
        /// update service attribute.
        /// </summary>
        /// <param name="serviceAttributeContract">serviceAttributeContract</param>
        /// <returns></returns>
        public Boolean UpdateServiceAttribute(ServiceAttributeContract serviceAttributeContract, Boolean _editLocally)
        {
            return BackgroundSetupManager.UpdateBkgSvcAttribute(serviceAttributeContract,View.TenantId, View.CurrentLoggedInUserId, _editLocally);
        }

        #endregion
    }
}
