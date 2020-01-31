using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRequirementVerificationFieldControl
    {
        /// <summary>
        /// Represents the Field level data
        /// </summary>
        RequirementVerificationDetailContract FieldData
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the current Context
        /// </summary>
        IRequirementVerificationFieldControl CurrentViewContext
        {
            get;
        }

        /// <summary>
        /// Data of the Combobox Field type
        /// </summary>
        Dictionary<String, String> dicComboData
        {
            get;
            set;
        }

        /// <summary>
        /// List of Documents uplaoded in File Upload type Field. I1 is ApplicantDocumentID, It is FileName, I3 is DocumentPath.
        /// </summary>
        List<Tuple<Int32, String, String>> lstDocuments
        {
            get;
            set;
        }

        /// <summary>
        /// SelectedTenantId
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        String ControlIdGenerator
        {
            get; 
        }

        #region UAT-1470 :As a student, there should be a way to close out of the video once you open it.
        /// <summary>
        /// VideoRequiredOpenTime
        /// </summary>
        String VideoRequiredOpenTime
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Represents the screen from which the screen was opened
        /// </summary>
        String ControlUseType
        {
            get;
            set;
        }

        //UAT 2371
        String EntityPermissionName
        {
            get;
            set;
        }
        Boolean IsItemEditable
        {
            get;
            set;
        }
        #region UAT-4368
        Boolean IsClientAdminLoggedIn
        {
            get;
            set;
        }
        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }
        #endregion

    }
}
