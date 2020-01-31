using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ICustomAttributeRowControlSearchView
    {
        List<TypeCustomAttributesSearch> lstTypeCustomAttributes { get; set; }
        ICustomAttributeRowControlSearchView CurrentViewContext { get; }
        String previousValues { get; set; }

    }
}




