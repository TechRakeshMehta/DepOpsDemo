using Entity.SharedDataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IManageUniversalAttributesView
    {
        List<lkpUniversalAttributeDataType> lstAttributeDatatype { get; set; }
        List<UniversalField> lstUniversalField { get; set; }

        IManageUniversalAttributesView CurrentViewContext { get; }

        Int32 CurrentUserId { get; }

        Int32 UniversalFieldID { get; set; }

        String FieldName { get; set; }

        Int32 AttributeDataTypeID {

            get; set;

        }

        String OptionDataTypeValue { get; set; }
    }
}
