#region Namespaces

#region System Defined

using System;

#endregion

#region Application Specific

#endregion

#endregion
namespace INTSOF.UI.Contract.SysXSecurityModel
{
    public class ManageProgramsContract
    {

        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// ProgramId</summary>
        /// <value>
        /// Gets or sets the value for ProgramId.</value>
        public Int32 ProgramId
        {
            get;
            set;
        }

        /// <summary>
        /// ProgramStudy</summary>
        /// <value>
        /// Gets or sets the value for ProgramStudy.</value>
        public String ProgramStudy
        {
            get;
            set;
        }

        /// <summary>
        /// OrganizationID</summary>
        /// <value>
        /// Gets or sets the value for OrganizationID.</value>
        public Int32 OrganizationID
        {
            get;
            set;
        }

        public Int16 GradeLevelID
        {
            get;
            set;
        }

        /// <summary>
        /// LK_ComplianceRuleGroupID</summary>
        /// <value>
        /// Gets or sets the value for LK_ComplianceRuleGroupID.</value>
        public Int32 LK_ComplianceRuleGroupID
        {
            get;
            set;
        }

        /// <summary>
        /// RenewalTerm</summary>
        /// <value>
        /// Gets or sets the value for RenewalTerm.</value>
        public Int16 RenewalTerm
        {
            get;
            set;
        }

        /// <summary>
        /// ManagementFee</summary>
        /// <value>
        /// Gets or sets the value for ManagementFee.</value>
        public Decimal? ManagementFee
        {
            get;
            set;
        }

        /// <summary>
        /// CreateDate</summary>
        /// <value>
        /// Gets or sets the value for created on date.</value>
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// ModifyDate</summary>
        /// <value>
        /// Gets or sets the value for modified on date.</value>
        public DateTime ModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// CreateUserID</summary>
        /// <value>
        /// Gets or sets the value for created by user's id.</value>
        public Int32 CreateUserID
        {
            get;
            set;
        }

        /// <summary>
        /// ModifyUserID</summary>
        /// <value>
        /// Gets or sets the value for modified by user's id.</value>
        public Int32 ModifyUserID
        {
            get;
            set;
        }

        /// <summary>
        /// DeleteFlag</summary>
        /// <value>
        /// Gets or sets the value for Deleted.</value>
        public Boolean DeleteFlag
        {
            get;
            set;
        }
        /// <summary>
        /// DurationMonth</summary>
        /// <value>
        /// Gets or sets the value for modified by user's id.</value>
        public Int32 DurationMonth
        {
            get;
            set;
        }
        /// <summary>
        /// Description</summary>
        /// <value>
        /// Gets or sets the value for Description.</value>
        public String Description
        {
            get;
            set;
        }
        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion

        #endregion


    }
}
