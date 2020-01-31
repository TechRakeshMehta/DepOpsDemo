#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#region Application Specific

using Entity;
using INTSOF.UI.Contract.Templates;

#endregion

#endregion

namespace CoreWeb.Templates.Views
{
    public interface ITemplatesMarkupView
    {
        /// <summary>
        /// Id of the template selected
        /// </summary>
        Int32 TemplateId
        {
            get;
            set;
        }
        /// <summary>
        /// Name of the Template
        /// </summary>
        String TemplateName
        {
            get;
            set;
        }
        /// <summary>
        /// Content of the Template
        /// </summary>
        String TemplateContent
        {
            get;
            set;
        }
        /// <summary>
        /// Represents the current context of the Template Maintenance Form
        /// </summary>
        ITemplatesMarkupView CurrentContext
        {
            get;
        }
        /// <summary>
        /// Template details in edit mode
        /// </summary>
        SystemEventTemplatesContract SystemEventTemplate { get; set; }
    }
}
