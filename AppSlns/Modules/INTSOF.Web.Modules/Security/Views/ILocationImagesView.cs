using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CoreWeb.Security.Views
{
    public interface ILocationImagesView
    {
        List<FingerPrintLocationImagesContract> lstLocationImagesData { get; set; }
        Int32 LocationId { get; set; }
    }
}
