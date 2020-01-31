using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.Utils;
using System.Linq;
using Entity;

namespace CoreWeb.Messaging.Views
{
    public interface IManageTemplateView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties
        #region Public Properties

        #endregion

        #region Private Properties
        /// <summary>
        /// Property to get the MessageId
        /// </summary>
        Guid MessageId
        {
            get;
        }
        
        /// <summary>
        /// 
        /// </summary>
        string TemplateName
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the ToList
        /// </summary>
        String ToList
        {
            get;
            set;
        }
        /// <summary>
        /// Property to get and set the CCList
        /// </summary>
        String CCList
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the CCList
        /// </summary>
        String BCCList
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the Subject
        /// </summary>
        String Subject
        {
            get;
            set;
        }
        /// <summary>
        /// Property to get and set the Content
        /// </summary>
        String Content
        {
            get;
            set;
        }
        /// <summary>
        /// Property to get the QueueType
        /// </summary>
        Int32 QueueType
        {
            get;
        }
        /// <summary>
        /// Property to get the current organization user id
        /// </summary>
        Int32 CurrentOrganizationUserId
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        Guid CurrentUserId
        {
            get;
        }

        /// <summary>
        /// Property to get and set the TenantTypes
        /// </summary>
        Dictionary<Int32, String> TenantTypes
        {
            get;
            set;
        }

        Int32 CommunicationTypeId
        {
            get;
            set;
        }

        String CommunicationTypeCode
        {
            get;
        }

        

        /// <summary>
        /// Property to get and set IsUserGroup
        /// </summary>
        String IsUserGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set ToListIds
        /// </summary>
        String ToListIds
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set CcListIds
        /// </summary>
        String CcListIds
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set BccListIds
        /// </summary>
        String BccListIds
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        String ToListGroupIds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        String CcListGroupIds
        {
            get;
            set;
        }
        /// <summary>
        /// Property to get and set CcIsUserGroup
        /// </summary>
        String CcIsUserGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get and set the Types of communication
        /// </summary>
        List<lkpCommunicationType> CommunicationTypeList
        {
            set;
            get;
        }

        /// <summary>
        /// Property to set the CompanyTemplates
        /// </summary>
        IQueryable<ADBMessage> CompanyTemplates
        {
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        string ADBMessageId
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        bool IsApplicant { get; }

        #endregion
        #endregion

        #region Methods
        #region Public Methods

        void BindTemplates();
        void Delete();


        #endregion
        #region Protected Methods

        #endregion
        #region Private Methods

        #endregion

        #endregion
    }
}




