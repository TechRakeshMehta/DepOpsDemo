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
    public interface ITemplatesGridView
    {
        /// <summary>
        /// List of Templates to display in the grid
        /// </summary>
        List<SystemEventTemplatesContract> EventSpecificTemplates
        {
            get;
            set;
        }

        /// <summary>
        /// List of Templates to display in the grid
        /// </summary>
        List<CommunicationTemplate> OtherTemplates
        {
            get;
            set;
        }


        /// <summary>
        /// Property used to get the type of templates
        /// </summary>
        Boolean IsDeleted
        {
            get;
            set;
        }

        /// <summary>
        /// Instance of the current View Context
        /// </summary>
        ITemplatesGridView CurrentContext
        {
            get;
        }

        /// <summary>
        /// Id of the Template to delete
        /// </summary>
        Int32 TemplateId
        {
            get;
            set;
        }

        Boolean IsEventSpecific { get; set; }

        Int32 CurrentUserId { get; }

        Int32 SelectedTenantId { get; }

        List<SystemEventTemplatesContract> lstAgencyHierarchySpecificTemplates
        {
            get;
            set;
        }

        List<Entity.SharedDataEntity.AgencyHierarchy> lstAgencyHierarchyRootNodes
        {
            get;
            set;
        }
    }
}




