using Entity.SharedDataEntity;
using INTSOF.UI.Contract.PlacementMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.PlacementMatching.Views
{
    public interface ILocationDepartmentView
    {
        Int32 CurrentLoggedInUserId { get; }
        List<Department> lstDepartment { get; set; }
        List<StudentType> lstStudentType { get; set; }
        List<AgencyLocationDepartmentContract> lstAgencyLocationDepartment { get; set; }
        Int32 AgencyLocationID { get; set; }
        AgencyLocationDepartmentContract LocationDepartment { get; set; }
    }
}
