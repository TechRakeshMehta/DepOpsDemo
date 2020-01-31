using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class CompliancePackageContract
    {

        public Int32 CompliancePackageId { get; set; }
        public String PackageName { get; set; }
        public String Description { get; set; }
        public String PackageLabel { get; set; }
        public String ScreenLabel { get; set; }
        public Boolean State { get; set; }
        public Boolean ViewDetails { get; set; }
        public Int32 CompliancePackageTypeID { get; set; }
        public String ChecklistDocumentURL { get; set; }
        public Int32 NotesDisplayPositionId { get; set; }


        /// <summary>
        /// TenantID
        /// </summary>
        /// <value>Gets or sets the value for Tenant ID.</value>
        /// <remarks></remarks> 
        public Int32 TenantID
        {
            get;
            set;
        }

        /// <summary>
        /// TenantName
        /// </summary>
        /// <value>Gets or sets the value for Tenant Name.</value>
        /// <remarks></remarks> 
        public String TenantName
        {
            get;
            set;
        }

        /// <summary>
        /// ExplanatoryNotes
        /// </summary>
        /// <value>Gets or sets the value for Explanatory Notes.</value>
        /// <remarks></remarks>
        public String ExplanatoryNotes
        {
            get;
            set;
        }

        /// <summary>
        /// ExceptionDescription
        /// </summary>
        /// <value>Gets or sets the value for Exception Description.</value>
        /// <remarks></remarks>
        public String ExceptionDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Program Id for the Master compliance Packages
        /// </summary>
        public Int32 AssignToProgramId { get; set; }

        /// <summary>
        /// Package Detail //UAT 1006
        /// </summary>
        /// <value>Gets or sets the value for Package Detail.</value>
        /// <remarks></remarks>
        public String PackageDetail
        {
            get;
            set;
        } 
    }
}
