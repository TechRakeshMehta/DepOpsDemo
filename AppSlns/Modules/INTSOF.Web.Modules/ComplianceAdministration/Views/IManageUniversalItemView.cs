using Entity.SharedDataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration
{
    public interface IManageUniversalItemView
    {
        IManageUniversalItemView CurrentViewContext { get; }

        List<UniversalItem> lstUniversalItems { get; set; }

        List<UniversalAttribute> lstUniversalAttribute { get; set; }

        String ItemName { get; set; }

        Int32 UniversalItemID { get; set; }

        Boolean IsAddMode { get; set; }

        Int32 CurrentUserId { get; }

        Int32 UniversalCategoryID { get; set; }

        String Messgae { get; set; }
    }
}
