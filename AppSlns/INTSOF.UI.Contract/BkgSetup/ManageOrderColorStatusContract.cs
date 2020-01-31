using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    public class ManageOrderColorStatusContract
    {
        public Int32 SelectedOrderFlagId
        {
            get;
            set;
        }

        public Boolean IsSuccessIndicator
        {
            get;
            set;
        }

        public String Description
        {
            get;
            set;
        }
    }
}
