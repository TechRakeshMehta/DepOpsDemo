using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.BkgSetup.Views
{
    public interface ISetupPackagesForInsitiuteView
    {
        List<BackgroundPackage> lstPackages { get; set; }
        Int32 tenantId { get; set; }
        String ErrorMessage { get; set; }
        Int32 NotesPositionId { get; set; }
        String Passcode { get; set; }
    }
}
