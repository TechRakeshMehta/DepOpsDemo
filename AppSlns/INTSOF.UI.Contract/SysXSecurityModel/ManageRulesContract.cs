using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.IntsofSecurityModel
{
    public class ManageRulesContract
    {

        /// <summary>
        /// StateID
        /// </summary>
        /// <value>Gets or sets the value for State's id.</value>
        /// <remarks></remarks>
        public Int32 StateID
        {
            get;
            set;
        }

        /// <summary>
        /// TenantID
        /// </summary>
        /// <value>Gets or sets the value for Tenant's id.</value>
        /// <remarks></remarks>
        public Int32 TenantID
        {
            get;
            set;
        }

        /// <summary>
        /// CountyID
        /// </summary>
        /// <value>Gets or sets the value for County's id.</value>
        /// <remarks></remarks>
        public Int32 CountyID
        {
            get;
            set;
        }

        /// <summary>
        /// JudgeID
        /// </summary>
        /// <value>Gets or sets the value for Judge's id.</value>
        /// <remarks></remarks>
        public Int32 JudgeID
        {
            get;
            set;
        }

        /// Name</summary>
        /// <value>
        /// Gets or sets the value for rule's name.</value>
        public String RuleName
        {
            get;
            set;
        }

        /// <summary>
        /// CategoryID
        /// </summary>
        /// <value>Gets or sets the value for CategoryID.</value>
        /// <remarks></remarks>
        public Int32 CategoryID
        {
            get;
            set;
        }


        /// <summary>
        /// CategoryName
        /// </summary>
        /// <value>Gets or sets the value for Category Name.</value>
        /// <remarks></remarks>
        public String CategoryName
        {
            get;
            set;
        }

        /// <summary>
        /// StateID
        /// </summary>
        /// <value>Gets or sets the value for Category Description.</value>
        /// <remarks></remarks>
        public String CategoryDescription
        {
            get;
            set;
        }

        public String RuleDescription
        {
            get;
            set;
        }
        public Int32 RuleID
        {
            get;
            set;
        }
    }
}
