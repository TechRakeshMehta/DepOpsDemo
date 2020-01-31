using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    [Serializable]
    public class BkgOrderQueueNotesContract
    {
        public Int32 CreatedByID
        {
            get;
            set;
        }

        public String CreatedOnDate
        {
            get;
            set;
        }

        public String UserName
        {
            get;
            set;
        }

        public String Note
        {
            get;
            set;
        }

        public Int32 OrderID
        {
            get;
            set;
        }

        public Int32 NotesID
        {
            get;
            set;
        }

        public DateTime CreatedOn
        {
            get;
            set;
        }
        public String OrderNumber
        {
            get;
            set;
        }

    }
}
