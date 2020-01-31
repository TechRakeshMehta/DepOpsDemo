#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXMembershipUser.cs
// Purpose:   
// 

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Web.Security;

#endregion

#region Application Specific

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel
{
    /// <summary>
    ///This class handles the operations related to Security module membership user.
    /// </summary>
    [Serializable]
    public class SysXMembershipUser : MembershipUser
    {
        #region Variables

        #region Private Varibales

        private String _firstName;
        private Boolean _ignoreIPRestriction;
        private Boolean _isNewPassword;
        private Boolean _isOldPassword;
        private Boolean _isOutOfOffice;
        private String _lastName;
        private DateTime? _officeReturnDateTime;
        private Int32 _organizationId;
        private Int32 _organizationUserId;
        private Boolean _passwordReset;
        private Guid _userId = Guid.Empty;
        private Boolean _isApplicant;
        private Boolean _isSystem;
        private Boolean _isSharedUser;
        private Int32 _userLoginHistoryID;
        private Dictionary<String, Boolean> _checkPermissionAssignmentQueue = new Dictionary<string, bool>();
        private Dictionary<String, Boolean> _checkPermissionUserQueue = new Dictionary<string, bool>();
        private Dictionary<String,Boolean> _checkPermissionFeatureBookmark=  new Dictionary<string, bool>();
        private Boolean? _isEnroller=null;
        //private Int32 _checkPermissionAssignmentQueue_SysXBlock ;
        //private Int32 _checkPermissionUserQueue_SysXBlock;
        //private Int32 _checkPermissionFeatureBookmark_SysXBlock;


        /// <summary>
        /// Variable to store whether the Applicant has logged in from an IncorrectUrl
        /// </summary>
        private Boolean _isIncorrectLoginUrl = false;

        private List<String> _sharedUserTypesCode = new List<String>();
      

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor SysXMembershipUser 
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="name"></param>
        /// <param name="providerUserKey"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="comment"></param>
        /// <param name="isApproved"></param>
        /// <param name="isLockedOut"></param>
        /// <param name="creationDate"></param>
        /// <param name="lastLoginDate"></param>
        /// <param name="lastActivityDate"></param>
        /// <param name="lastPasswordChangedDate"></param>
        /// <param name="lastLockoutDate"></param>
        /// <param name="organizationUserId"></param>
        /// <param name="userId"></param>
        /// <param name="organizationId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="isOutOfOffice"></param>
        /// <param name="officeReturnDateTime"></param>
        /// <param name="isNewPassword"></param>
        /// <param name="isOldPassword"></param>
        /// <param name="passwordReset"></param>
        /// <param name="ignoreIPRestriction"></param>
        /// <param name="clientId"></param>
        /// <param name="tenantTypeId"></param>
        /// <param name="tenantTypeCode"></param>
        /// <param name="productId"></param>
        /// <param name="isApplicant"></param>
        /// <param name="_isSystem"></param>
        public SysXMembershipUser(String providerName,
                                  String name,
                                  object providerUserKey,
                                  String email,
                                  String passwordQuestion,
                                  String comment,
                                  Boolean isApproved,
                                  Boolean isLockedOut,
                                  DateTime creationDate,
                                  DateTime lastLoginDate,
                                  DateTime lastActivityDate,
                                  DateTime lastPasswordChangedDate,
                                  DateTime lastLockoutDate,
                                  Int32 organizationUserId,
                                  Guid userId,
                                  Int32 organizationId,
                                  String firstName,
                                  String lastName,
                                  Boolean isOutOfOffice,
                                  DateTime? officeReturnDateTime,
                                  Boolean isNewPassword,
                                  Boolean isOldPassword,
                                  Boolean passwordReset,
                                  Boolean ignoreIPRestriction,
                                  Int32? clientId,
                                  Int32? tenantTypeId,
                                  String tenantTypeCode,
                                  Int32? productId,
                                  Boolean IsApplicant,
                                  Boolean IsSystem,
                                  Boolean IsSharedUser,
                                  List<String> SharedUserTypesCodes)
            : base(providerName,
                   name,
                   providerUserKey,
                   email,
                   passwordQuestion,
                   comment,
                   isApproved,
                   isLockedOut,
                   creationDate,
                   lastLoginDate,
                   lastActivityDate,
                   lastPasswordChangedDate,
                   lastLockoutDate)
        {
            IsSysXAdmin = false;
            _organizationUserId = organizationUserId;
            _userId = userId;
            _organizationId = organizationId;
            _firstName = firstName;
            _lastName = lastName;
            _isOutOfOffice = isOutOfOffice;
            _officeReturnDateTime = officeReturnDateTime;
            _isNewPassword = isNewPassword;
            _isOldPassword = isOldPassword;
            _passwordReset = passwordReset;
            _ignoreIPRestriction = ignoreIPRestriction;
            TenantId = clientId;
            TenantTypeId = tenantTypeId;
            TenantTypeCode = tenantTypeCode;
            ProductId = productId;
            _isApplicant = IsApplicant;
            _isSystem = IsSystem;
            _isSharedUser = IsSharedUser;
            _sharedUserTypesCode = SharedUserTypesCodes;
            _checkPermissionAssignmentQueue = new Dictionary<string, bool>();
            _checkPermissionFeatureBookmark = new Dictionary<string, bool>();
            _checkPermissionUserQueue = new Dictionary<string, bool>();
            _isEnroller = null;
        }

        #endregion

        #region Properties

        #region Public Properties

        public Boolean IsSysXAdmin
        {
            get;
            private set;
        }

        public Int32? TenantId
        {
            get;
            private set;
        }

        public Int32? DetailExtId
        {
            get;
            private set;
        }
        public Int32? TrackingNumber
        {
            get;
            private set;
        }

        public Int32? ServiceStatusId
        {
            get;
            private set;
        }



        public Int32? TenantTypeId
        {
            get;
            private set;
        }

        public String TenantTypeCode
        {
            get;
            private set;
        }

        public Int32? ProductId
        {
            get;
            private set;
        }

        /// <summary>
        /// Get set Organization User ID
        /// </summary>
        public Int32 OrganizationUserId
        {
            get
            {
                return _organizationUserId;
            }
            set
            {
                _organizationUserId = value;
            }
        }

        /// <summary>
        /// Get set User id
        /// </summary>
        public Guid UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
            }
        }

        /// <summary>
        /// Get set Organization id.
        /// </summary>
        public Int32 OrganizationId
        {
            get
            {
                return _organizationId;
            }
            set
            {
                _organizationId = value;
            }
        }

        /// <summary>
        /// Get set first name.
        /// </summary>
        public String FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
            }
        }

        /// <summary>
        /// Get set last name.
        /// </summary>
        public String LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
            }
        }

        /// <summary>
        /// Get set out of office.
        /// </summary>
        public Boolean IsOutOfOffice
        {
            get
            {
                return _isOutOfOffice;
            }
            set
            {
                _isOutOfOffice = value;
            }
        }

        /// <summary>
        /// Get set office return date time.
        /// </summary>
        public DateTime? OfficeReturnDateTime
        {
            get
            {
                return _officeReturnDateTime;
            }
            set
            {
                _officeReturnDateTime = value;
            }
        }

        /// <summary>
        /// Get set is new password.
        /// </summary>
        public Boolean IsNewPassword
        {
            get
            {
                return _isNewPassword;
            }
            set
            {
                _isNewPassword = value;
            }
        }

        /// <summary>
        /// Get set is old password.
        /// </summary>
        public Boolean IsOldPassword
        {
            get
            {
                return _isOldPassword;
            }
            set
            {
                _isOldPassword = value;
            }
        }

        /// <summary>
        /// Get set password reset.
        /// </summary>
        public Boolean PasswordReset
        {
            get
            {
                return _passwordReset;
            }
            set
            {
                _passwordReset = value;
            }
        }

        /// <summary>
        /// Get set Is Applicant.
        /// </summary>
        public Boolean IsApplicant
        {
            get
            {
                return _isApplicant;
            }
            set
            {
                _isApplicant = value;
            }
        }

        /// <summary>
        /// Get set Is System.
        /// </summary>
        public Boolean IsSystem
        {
            get
            {
                return _isSystem;
            }
            set
            {
                _isSystem = value;
            }
        }

        /// <summary>
        /// Get set ignore IP restriction.
        /// </summary>
        public Boolean IgnoreIPRestriction
        {
            get
            {
                return _ignoreIPRestriction;
            }
            set
            {
                _ignoreIPRestriction = value;
            }
        }

        /// <summary>
        /// Property to store whether the Applicant tried to log in from an IncorrectUrl
        /// </summary>
        public Boolean IncorrectLoginUrlUsed
        {
            get
            {
                return _isIncorrectLoginUrl;
            }
            set
            {
                _isIncorrectLoginUrl = value;
            }
        }

        #region UAT-1110
        /// <summary>
        /// Get set Is Shared User.
        /// </summary>
        public Boolean IsSharedUser
        {
            get
            {
                return _isSharedUser;
            }
            set
            {
                _isSharedUser = value;
            }
        }
        #endregion

        #region UAT-1218

        /// <summary>
        /// Get set Is Shared User.
        /// </summary>
        public List<String> SharedUserTypesCode
        {
            get
            {
                return _sharedUserTypesCode;
            }
            set
            {
                _sharedUserTypesCode = value;
            }
        }

        #endregion

        public Int32 UserLoginHistoryID
        {
            get
            {
                return _userLoginHistoryID;
            }
            set
            {
                _userLoginHistoryID = value;
            }
        }

        public Dictionary<String, Boolean> CheckPermissionAssignmentQueue
        {
            get
            {
                return _checkPermissionAssignmentQueue;
            }
            set
            {
                _checkPermissionAssignmentQueue = value;
            }
        }
        public Dictionary<String, Boolean> CheckPermissionUserQueue
        {
            get
            {
                return _checkPermissionUserQueue;
            }
            set
            {
                _checkPermissionUserQueue = value;
            }
        }
        public Dictionary<String,Boolean> CheckPermissionFeatureBookmark
        {
            get
            {
                return _checkPermissionFeatureBookmark;
            }
            set
            {
                _checkPermissionFeatureBookmark = value;
            }
        }

        //public Int32 CheckPermissionAssignmentQueue_SysXBlock
        //{
        //    get
        //    {
        //        return _checkPermissionAssignmentQueue_SysXBlock;
        //    }
        //    set
        //    {
        //        _checkPermissionAssignmentQueue_SysXBlock = value;
        //    }
        //}
        //public Int32 CheckPermissionUserQueue_SysXBlock
        //{
        //    get
        //    {
        //        return _checkPermissionUserQueue_SysXBlock;
        //    }
        //    set
        //    {
        //        _checkPermissionUserQueue_SysXBlock = value;
        //    }
        //}
        //public Int32 CheckPermissionFeatureBookmark_SysXBlock
        //{
        //    get
        //    {
        //        return _checkPermissionFeatureBookmark_SysXBlock;
        //    }
        //    set
        //    {
        //        _checkPermissionFeatureBookmark_SysXBlock = value;
        //    }
        //}
        public Boolean? IsEnroller
        {
            get
            {
                return _isEnroller;
            }
            set
            {
                _isEnroller = value;
            }
        }


        #endregion

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