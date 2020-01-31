using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class EmailDetails
    {
        #region Private Variables

        private Int32 _systemcommId;
        private String _emailType;
        private String _subject;
        private String _dispatchedDate;
        private Int32 _totalRecords;
        #endregion

        #region Properties
        public Int32 SystemCommunicationId { get { return _systemcommId; } set { _systemcommId = value; } }
        public String EmailType { get { return _emailType; } set { _emailType = value; } }
        public String Subject { get { return _subject; } set { _subject = value; } }
        public String DispatchedDate { get { return _dispatchedDate; } set { _dispatchedDate = value; } }
        public Int32 TotalRecords { get { return _totalRecords; } set { _totalRecords = value; } }
        #endregion
    }
}
