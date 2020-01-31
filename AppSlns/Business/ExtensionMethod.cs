#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ExtensionMethod.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;

#endregion

#endregion

namespace Business.RepoManagers
{
    /// <summary>
    /// This class handles all the operations related to Extension methods.
    /// </summary>
    /// <remarks></remarks>
    public static class ExtensionMethod
    {
        #region Variables

        #endregion

        #region Properties

        #endregion

        #region Events

        #endregion

        #region Methods

        /// <summary>
        /// Adds the contact detail.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="contactType">Type of the contact.</param>
        /// <param name="contactTypeValue">The contact type value.</param>
        /// <param name="userId">The user ID.</param>
        /// <remarks></remarks>
        //public static void AddContactDetail(this Contact contact, String contactType, String contactTypeValue, Int32 userId)
        //{
        //    ContactDetail contactDetail = new ContactDetail();
        //    contactDetail.lkpContactType = SupplierManager.GetContactTypeId(contactType);
        //    contactDetail.ContactTypeValue = contactTypeValue;
        //    contactDetail.ExpireDate = DateTime.Now;
        //    contactDetail.IsPreferred = true;
        //    contactDetail.CreatedByID = userId;
        //    contactDetail.ModifiedByID = userId;
        //    contactDetail.ModifiedOn = DateTime.Now;
        //    contactDetail.CreatedOn = DateTime.Now;
        //    contactDetail.IsActive = true;

        //    //TBD :Fill default value for contactdetail table as per new DB
        //    if (!contactDetail.lkpContactType.IsNull())
        //    {
        //        contactDetail.ContactTypeID = contactDetail.lkpContactType.ContactTypeID;
        //    }

        //    contact.ContactDetails.Add(contactDetail);
        //}

        /// <summary>
        /// Adds the contact detail.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="contactTypeId">Type of the contact.</param>
        /// <param name="contactTypeValue">The contact type value.</param>
        /// <param name="userId">The user ID.</param>
        /// <remarks></remarks>
        public static void AddContactDetail(this Contact contact, Int32 contactTypeId, String contactTypeValue, Int32 userId)
        {
            ContactDetail contactDetail = new ContactDetail();

            contactDetail.ContactTypeID = contactTypeId;
            contactDetail.ContactTypeValue = contactTypeValue;
            contactDetail.ExpireDate = DateTime.Now;
            contactDetail.IsPreferred = true;
            contactDetail.CreatedByID = userId;
            contactDetail.ModifiedByID = userId;
            contactDetail.ModifiedOn = DateTime.Now;
            contactDetail.CreatedOn = DateTime.Now;
            contactDetail.IsActive = true;

            contact.ContactDetails.Add(contactDetail);
        }              

        /// <summary>
        /// Adds the contact detail.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="contactTypeId">Type of the contact.</param>
        /// <param name="contactTypeValue">The contact type value.</param>
        /// <param name="userId">The user ID.</param>
        /// <param name="isPreferred">Is preferred</param>
        public static void AddContactDetail(this Contact contact, Int32 contactTypeId, String contactTypeValue, Int32 userId, Boolean isPreferred)
        {
            ContactDetail contactDetail = new ContactDetail();

            contactDetail.ContactTypeID = contactTypeId;
            contactDetail.ContactTypeValue = contactTypeValue;
            contactDetail.ExpireDate = DateTime.Now;
            contactDetail.IsPreferred = isPreferred;
            contactDetail.CreatedByID = userId;
            contactDetail.ModifiedByID = userId;
            contactDetail.ModifiedOn = DateTime.Now;
            contactDetail.CreatedOn = DateTime.Now;
            contactDetail.IsActive = true;

            contact.ContactDetails.Add(contactDetail);
        }

        /// <summary>
        /// Updates the contact detail.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="contactType">Type of the contact.</param>
        /// <param name="contactTypeValue">The contact type value.</param>
        /// <param name="userId">The user ID.</param>
        /// <remarks></remarks>
        //public static void UpdateContactDetail(this Contact contact, String contactType, String contactTypeValue, Int32 userId)
        //{
        //    ContactDetail contactDetail = contact.ContactDetails.FirstOrDefault(contactInfo => contactInfo.ContactTypeID.Equals(SupplierManager.GetContactTypeId(contactType).ContactTypeID));

        //    if (!contactDetail.IsNull())
        //    {
        //        contactDetail.ContactTypeValue = contactTypeValue;
        //        contactDetail.ExpireDate = DateTime.Now;
        //        contactDetail.IsPreferred = true;
        //        contactDetail.ModifiedByID = userId;
        //        contactDetail.ModifiedOn = DateTime.Now;
        //    }
        //}

        /// <summary>
        /// Updates the contact detail.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="contactTypeId">Type of the contact.</param>
        /// <param name="contactTypeValue">The contact type value.</param>
        /// <param name="userId">The user ID.</param>
        /// <param name="isPreferred">Is preferred</param>
        public static void UpdateContactDetail(this Contact contact, Int32 contactTypeId, String contactTypeValue, Int32 userId, Boolean isPreferred)
        {
            ContactDetail contactDetail = contact.ContactDetails.FirstOrDefault(contactInfo => contactInfo.ContactTypeID.Equals(contactTypeId));

            if (!contactDetail.IsNull())
            {
                contactDetail.ContactTypeValue = contactTypeValue;
                contactDetail.ExpireDate = DateTime.Now;
                contactDetail.IsPreferred = isPreferred;
                contactDetail.ModifiedByID = userId;
                contactDetail.ModifiedOn = DateTime.Now;
            }
            else
            {
                AddContactDetail(contact, contactTypeId, contactTypeValue, userId, isPreferred);
            }
        }

        /// <summary>
        /// Override Updates the contact detail without using SupplierManager.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="contactTypeId">Type of the contact.</param>
        /// <param name="contactTypeValue">The contact type value.</param>
        /// <param name="userId">The user ID.</param>
        /// <remarks></remarks>
        public static void UpdateContactDetail(this Contact contact, Int32 contactTypeId, String contactTypeValue, Int32 userId)
        {
            ContactDetail contactDetail = contact.ContactDetails.FirstOrDefault(contactInfo => contactInfo.ContactTypeID.Equals(contactTypeId));

            if (!contactDetail.IsNull())
            {
                contactDetail.ContactTypeValue = contactTypeValue;
                contactDetail.ExpireDate = DateTime.Now;
                contactDetail.IsPreferred = true;
                contactDetail.ModifiedByID = userId;
                contactDetail.ModifiedOn = DateTime.Now;
            }
        }

        ///// <summary>
        ///// Gets the contact detail value.
        ///// </summary>
        ///// <param name="contact">The contact.</param>
        ///// <param name="contactType">Type of the contact.</param>
        ///// <returns></returns>
        ///// <remarks></remarks>
        public static String GetContactDetailValue(this Contact contact, String contactType)
        {
            String contactDetailValue = String.Empty;
            ContactDetail contactDetail = contact.ContactDetails.FirstOrDefault(contactInfo => contactInfo.ContactTypeID.Equals(LookupManager.GetLookUpIDbyCode<lkpContactType>(cond => cond.ContactCode.Trim().Equals(contactType))));

            if (!contactDetail.IsNull())
            {
                contactDetailValue = contactDetail.ContactTypeValue;
            }
            return contactDetailValue;
        }

        /// <summary>
        /// Gets the contact detail value.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="contactType">Type of the contact.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        //public static String GetContactDetailValueandId(this Contact contact, String contactType)
        //{
        //    String contactDetailValue = String.Empty;
        //    ContactDetail contactDetail = contact.ContactDetails.FirstOrDefault(contactInfo => contactInfo.ContactTypeID.Equals(SupplierManager.GetContactTypeId(contactType).ContactTypeID));

        //    if (!contactDetail.IsNull())
        //    {
        //        contactDetailValue = contactDetail.ContactTypeValue + ',' + contactDetail.ContactID;
        //    }
        //    return contactDetailValue;
        //}

        /// <summary>
        /// Gets the contact detail value.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="contactType">Type of the contact.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Int32 GetContactDetailValueforPreferred(this Contact contact)
        {
            Int32 contactTypeID = 0;
            ContactDetail contactDetail = contact.ContactDetails.FirstOrDefault(contactInfo => contactInfo.IsPreferred);

            if (!contactDetail.IsNull())
            {
                contactTypeID = contactDetail.ContactTypeID;
            }
            return contactTypeID;
        }



        #endregion
    }
}