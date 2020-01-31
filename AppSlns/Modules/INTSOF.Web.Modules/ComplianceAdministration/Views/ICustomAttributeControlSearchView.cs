using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ICustomAttributeControlSearchView
    {
        TypeCustomAttributesSearch TypeCustomtAttribute { get; set; }
        ICustomAttributeControlSearchView CurrentViewContext { get; }
        String previousValues { get; set; }
        
    }
}




