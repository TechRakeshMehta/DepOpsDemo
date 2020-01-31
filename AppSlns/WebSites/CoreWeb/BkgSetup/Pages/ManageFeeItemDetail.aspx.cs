using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageFeeItemDetail : BaseWebPage
    {
        #region Public Properties

        public Int32 FeeItemId { get; set; }

        public Int32 SelectedTenantId
        {
            get;
            set;
        }

        public String ErrorMessage
        {
            set { base.ShowErrorMessage(value); }

        }

        public String InfoMessage
        {
            set { base.ShowInfoMessage(value); }
        }

        public String SuccessMessage
        {
            set { base.ShowSuccessMessage(value); }
        }

        #endregion

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
              try
            {
                if (!this.IsPostBack)
                {
                    SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                    FeeItemId = Convert.ToInt32(Request.QueryString["Id"]);
                    SetEditFeeItemUCProperties();
                    SetManageFeeItemFeeRecordsUCProperties();
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
               base.ShowErrorMessage(ex.Message);
            }
        }


        #endregion

        #region Methods
        /// <summary>
        /// Method to set Edit Fee Item user control properties.
        /// </summary>
        private void SetEditFeeItemUCProperties()
        {
            ucEditFeeItemDetail.TenantId = SelectedTenantId;
            ucEditFeeItemDetail.FeeItemId = FeeItemId;
        }

        private void SetManageFeeItemFeeRecordsUCProperties()
        {
            ucManageFeeItemFeeRecords.TenantId = SelectedTenantId;
            ucManageFeeItemFeeRecords.SelectedFeeItemId = FeeItemId;
        }
        #endregion
    }
}