using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.Utils.PdfHelper
{
    public class DocumentPDFMapper
    {

        #region members
        private long _documentPDFMapperID;
        private long _documentID;
        private string _documentField;
        private string _documentTableValue;
        private string _documentFieldValue;
        private string _defaultValue;
        private string _documentPDFOrderData;
        #endregion

        #region constructors
        public DocumentPDFMapper() { }
        #endregion

        #region accessors
        public long DocumentPDFMapperID
        {
            get
            {
                return _documentPDFMapperID;
            }
            set
            {
                _documentPDFMapperID = value;
            }
        }
        public long DocumentID
        {
            get
            {
                return _documentID;
            }
            set
            {
                _documentID = value;
            }
        }
        public string DocumentField
        {
            get
            {
                return _documentField;
            }
            set
            {
                _documentField = value;
            }
        }
        public string DocumentTableValue
        {
            get
            {
                return _documentTableValue;
            }
            set
            {
                _documentTableValue = value;
            }
        }

        public string DocumentFieldValue
        {
            get
            {
                return _documentFieldValue;
            }
            set
            {
                _documentFieldValue = value;
            }
        }
        public string DefaultValue
        {
            get
            {
                return _defaultValue;
            }
            set
            {
                _defaultValue = value;
            }
        }
        public string DocumentPDFOrderData
        {
            get
            {
                return _documentPDFOrderData;
            }
            set
            {
                _documentPDFOrderData = value;
            }
        }

        #endregion

    }
}
