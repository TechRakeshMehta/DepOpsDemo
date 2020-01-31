using INTSOF.UI.Contract.PlacementMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.PlacementMatching.Views
{
    public interface IDepartmentView
    {
        List<DepartmentContract> lstDepartments { get; set; }
        DepartmentContract departmentContract { get; set; }
        Int32 CurrentUserId
        {
            get;
        }
        String SuccessMsg { get; set; }
        String ErrorMsg { get; set; }
        String InfoMsg { get; set; }
    }
}
