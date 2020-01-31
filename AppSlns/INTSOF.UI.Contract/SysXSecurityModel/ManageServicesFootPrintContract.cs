using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;

namespace INTSOF.UI.Contract.IntsofSecurityModel
{
    public class ManageServicesFootPrintContract
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        public Int32 TenantId
        {
            get;
            set;
        }

        public Int32 CurrentUserID
        {
            get;
            set;
        }

        public List<ServiceFootprint> ServiceFootprint
        {
            get;
            set;
        }

        public String ErrorMessage
        {
            get;
            set;
        }
        public List<ServiceFootprint> ServiceFootprintTreeListState
        {
            get;
            set;
        }
        public String SuccessMessage
        {
            get;
            set;
        }

        #endregion
    }
}
