using System;
using System.Collections.Generic;
using System.Text;
using Entity;

namespace CoreWeb.BkgSetup.Views
{
    public interface IUploadDisclosureDocumentsView
    {
        List<SystemDocument> ToSaveUploadedDisclosureDocuments
        {
            get;
            set;
        }

        String ErrorMessage
        {
            get;
            set;
        }
    }
}




