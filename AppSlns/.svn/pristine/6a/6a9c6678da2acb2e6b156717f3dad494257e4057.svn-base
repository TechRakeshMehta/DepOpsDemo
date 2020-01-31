using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.AgencyJobBoard.Views
{
    public partial class Default : BasePage, IDefaultView
    {
        /// <summary>
        /// Page OnInitComplete event.
        /// </summary>
        /// <param name="e">Event</param>
        protected override void OnInitComplete(EventArgs e)
        {
            try
            {
                base.dynamicPlaceHolder = phDynamic;
                base.OnInitComplete(e);
                SetModuleTitle("Agency Job Board");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}