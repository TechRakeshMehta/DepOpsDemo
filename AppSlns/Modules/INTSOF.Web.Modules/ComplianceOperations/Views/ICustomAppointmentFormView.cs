using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface ICustomAppointmentFormView
    {
       
        DateTime? LastBookedDate { get; set; }
        bool PendingChangesToCommit { get; set; }
    }
}
