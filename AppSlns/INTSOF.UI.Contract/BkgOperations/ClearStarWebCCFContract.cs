using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.ClientEntity;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract
{
    public class ClearStarWebCCFContract
    {
       public  List<CustomFormOrderData> ListCustomFormOrderData { get; set; }
       public ApplicantDocument ApplicantDocumentCCFData { get; set; }
    }
}
