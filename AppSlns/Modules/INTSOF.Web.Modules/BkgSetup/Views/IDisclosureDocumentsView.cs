
#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Text;
using Entity;

#endregion

#region UserDefined

#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public interface IDisclosureDocumentsView
    {
        String ErrorMessage
        {
            get;
            set;
        }

        List<SystemDocument> DisclosureDocuments
        {
            get;
            set;
        }

        Int32 SystemDocumentID
        {
            get;
            set;
        }

        SystemDocument DisclosureDocumentToUpdate
        {
            get;
            set;
        }

        List<lkpDocumentType> DocumentTypeList { get; set; }

        //UAT-2625:
        List<lkpDisclosureDocumentAgeGroup> DisclosureDocAgeGroupTypeList
        {
            get;
            set;
        }

        //UAT-3745
        List<ExternalBkgSvc> lstExtBkgSvc
        { get; set; }

        Int32 SelectedExtBkgSvcID { get; set; }
    }
}




