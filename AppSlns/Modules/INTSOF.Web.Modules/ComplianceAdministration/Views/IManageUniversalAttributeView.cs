using Entity.SharedDataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IManageUniversalAttributeView
    {
        IManageUniversalAttributeView CurrentViewContext { get; }

        Int32 UniversalItemID { get; set; }

        Boolean IsAddMode { get; set; }

        Int32 CurrentUserId { get; }

        Int32 UniversalAttributeID { get; set; }

        String AttributeName { get; set; }

        String AttributeDataTypeID { get; set; }

        String OptionDataTypeValue { get; set; }

        List<UniversalAttribute> lstUniversalAttribute { get; set; }

        List<lkpUniversalAttributeDataType> lstAttributeDatatype { get; set; }

        String Messgae { get; set; }
    }
}
