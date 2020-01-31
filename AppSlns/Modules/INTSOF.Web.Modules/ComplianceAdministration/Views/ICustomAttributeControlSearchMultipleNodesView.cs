using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface ICustomAttributeControlSearchMultipleNodesView
    {
        TypeCustomAttributesSearch TypeCustomtAttribute { get; set; }
        ICustomAttributeControlSearchMultipleNodesView CurrentViewContext { get; }
        String previousValues { get; set; }
        
    }
}




