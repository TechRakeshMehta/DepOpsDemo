using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.SysXSecurityModel
{
    public class ManageGradeContract
    {


        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// GradeId</summary>
        /// <value>
        /// Gets or sets the value for ProgramId.</value>
        public Int16 Id
        {
            get;
            set;
        }

        /// <summary>
        /// ProgramStudy</summary>
        /// <value>
        /// Gets or sets the value for ProgramStudy.</value>
        public String Description
        {
            get;
            set;
        }

        /// <summary>
        /// OrganizationID</summary>
        /// <value>
        /// Gets or sets the value for OrganizationID.</value>
        public Int16 GradeLevelGroupID
        {
            get;
            set;
        }

        /// <summary>
        /// LK_ComplianceRuleGroupID</summary>
        /// <value>
        /// Gets or sets the value for LK_ComplianceRuleGroupID.</value>
        public String GredeLevelGroupDescription
        {
            get;
            set;
        }

        /// <summary>
        /// RenewalTerm</summary>
        /// <value>
        /// Gets or sets the value for RenewalTerm.</value>
        public Int32 SEQ
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
