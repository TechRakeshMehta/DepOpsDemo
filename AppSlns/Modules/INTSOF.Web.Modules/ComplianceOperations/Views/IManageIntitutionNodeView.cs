#region Namespaces

#region SystemDefined

using System.Collections.Generic;
using System.Linq;
using System;

#endregion

#region UserDefined

using Entity.ClientEntity;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IManageIntitutionNodeView
    {
        #region Variables

        #region Private Variables
        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties

        List<Tenant> ListTenants
        {
            set;
            get;
        }

        Int32 SelectedTenantID
        {
            set;
            get;
        }

        Int32 CurrentUserId
        {
            get;
        }

        IQueryable<InstitutionNode> GetNodeList
        {
            get;
            set;
        }

        List<InstitutionNodeType> GetNodeTypeList
        {
            get;
            set;
        }

        Int32 NodeId
        {
            get;
            set;
        }

        Int32 NodeTypeId
        {
            get;
            set;
        }

        String Name
        { get; set; }

        Int32? Duration
        { get; set; }

        String Label
        { get; set; }

        String Description
        { get; set; }

        String ErrorMessage { get; set; }

        String SuccessMessage { get; set; }
        String InfoMessage { get; set; }
        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IManageIntitutionNodeView CurrentViewContext
        {
            get;

        }

        String LastCode
        {
            get;
            set;
        }

        String GeneratedCode
        {
            get;
            set;
        }

        /// <summary>
        /// Get and set custom attribute list of type hierarchy.
        /// </summary>
        IQueryable<CustomAttribute> GetCustomAttributeListTypeHierarchy
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set List of Custom Attribute for mapping
        /// </summary>
        List<CustomAttributeMapping> ListToAddCustomAttributeMapping
        {
            get;
            set;
        }

        List<CustomAttributeMapping> GetCustomAttributeMappedList
        {
            get;
            set;
        }

        List<Int32> MappedIdsWithCustomAttributeValue
        {
            get;
            set;
        }

        //UAT-3032
        Int32 PreferredSelectedTenantID { get; set; }
        #endregion

        #endregion

        #region Methods

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}




