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
    public interface IReadOnlyTemplatesGridView
    {
        /// <summary>
        /// List of Templates to display in the grid
        /// </summary>        
       List<CommunicationTemplate> OtherTemplates
        {
            get;
            set;
        }

       IReadOnlyTemplatesGridView CurrentContext
        {
            get;
        }
    }
}
