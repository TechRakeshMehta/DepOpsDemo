using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ICustomAttributeRowControlSearchMultipleNodesView
    {
        List<TypeCustomAttributesSearch> lstTypeCustomAttributes { get; set; }
        ICustomAttributeRowControlSearchMultipleNodesView CurrentViewContext { get; }
        String previousValues { get; set; }

    }
}




