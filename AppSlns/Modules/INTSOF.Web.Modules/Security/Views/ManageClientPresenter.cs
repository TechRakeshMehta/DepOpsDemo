#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageClientPresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using System.Linq;

#endregion

#region Application Specific

using Entity;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.Utils.Consts;
using System.Data.Entity.Core.EntityClient;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class has the method's implementation which performs all the CRUD(Create/ Read/ Update/ Delete) operation for managing tenants with its details.
    /// </summary>
    public class ManageTenantPresenter : Presenter<IManageTenantView>
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

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// This method is invoked by the view every time it loads.
        /// </summary>
        public override void OnViewLoaded()
        {
            View.TenantTypes = SecurityManager.GetTenantTypes().ToList();
        }

        /// <summary>
        /// Retrieves a list of all cities.
        /// </summary>
        public void GetAllCities()
        {
            View.Cities = SecurityManager.GetCities().ToList();
        }

        /// <summary>
        /// Performs binding for states.
        /// </summary>
        public void BindState()
        {
            View.States = SecurityManager.GetStates().ToList();
        }

        /// <summary>
        /// Performs binding for cities.
        /// </summary>
        public void BindCity()
        {
            IQueryable<ZipCode> zipInformation = SecurityManager.GetZipCodesByZipCodeNumber(View.ViewContract.ZipCode).Take(AppConsts.ONE);

            if (!zipInformation.Any())
            {
                return;
            }

            View.Cities = new List<City>();
            View.Cities.AddRange(zipInformation.Select(selectCity => selectCity.City).OrderBy(orderBy => orderBy.CityName));
        }

        /// <summary>
        /// Retrieves a list of all tenants with it's details.
        /// </summary>
        public void RetrievingTenants()
        {
            Boolean SortByName = false;
            //Extended properties like description, phone, address, zipcode, city and state for Tenants.  
            Boolean GetExtendedProperties = true;
            View.Tenants = SecurityManager.GetTenants(SortByName, GetExtendedProperties).ToList();
        }

        /// <summary>
        /// Retrieves a list of all tenant's products.
        /// </summary>
        public void RetrievingTenantProducts()
        {
            View.TenantProducts = SecurityManager.GetProductsForTenant(View.ViewContract.TenantId).ToList();
        }

        /// <summary>
        /// Retrieves a list of all organizations.
        /// </summary>
        public void RetrievingOrganization()
        {
            Organization organization = SecurityManager.GetOrganizationForTenant(View.ViewContract.TenantId);
            List<Organization> allOrganization = new List<Organization>();

            if (!organization.IsNull())
            {
                allOrganization.Add(organization);
            }

            View.Organizations = allOrganization;
        }

        /// <summary>
        /// Performs an insert operation for tenant.
        /// </summary>
        public void TenantSave()
        {
            if (View.ViewContract.TenantTypeId == Convert.ToInt32(TenantType.Institution))
            {
                if (View.ViewContract.TenantDBServer == String.Empty)
                {
                    View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_DATABASE_SERVER_REQUIRED);
                    return;
                }
                if (View.ViewContract.TenantDBName == String.Empty)
                {
                    View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_DATABASE_NAME_REQUIRED);
                    return;
                }
                if (View.ViewContract.TenantDBUserName == String.Empty)
                {
                    View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_DATABASE_USER_NAME_REQUIRED);
                    return;
                }
                if (View.ViewContract.TenantDBPassword == String.Empty)
                {
                    View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_DATABASE_PASSWORD_REQUIRED);
                    return;
                }
                if (View.ViewContract.TenantConnectinString == String.Empty)
                {
                    View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_TENANT_CONN_STRING);
                    return;
                }
                DatabaseExistsStatus dbexist = IsValidTenantConnectionString(View.ViewContract.TenantConnectinString);
                if (dbexist == DatabaseExistsStatus.NO_CONNECTION)
                {
                    View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_TENANT_CONN_STRING_NOT_VALID);
                    return;
                }
                if (dbexist == DatabaseExistsStatus.NO_DB)
                {
                    View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_TENANT_DB_NOT_EXISTS);
                    return;
                }
            }
            if (SecurityManager.IsTenantExists(View.ViewContract.TenantName))
            {
                View.ErrorMessage = View.ViewContract.TenantName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_TENANT_EXISTS);
            }
            else if (SecurityManager.IsProductExists(View.ViewContract.ProductName))
            {
                View.ErrorMessage = View.ViewContract.ProductName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_PRODUCT_EXISTS);
            }
            else if (SecurityManager.IsOrganizationExists(View.ViewContract.OrganizationName))
            {
                View.ErrorMessage = View.ViewContract.OrganizationName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_ORGANIZATION_EXISTS);
            }
            else
            {
                View.ErrorMessage = String.Empty;

                // Tenant Section.
                Tenant tenant = new Tenant
                                    {
                                        TenantName = View.ViewContract.TenantName,
                                        TenantDesc = View.ViewContract.TenantDesc,
                                        TenantTypeID = View.ViewContract.TenantTypeId
                                    };

                if (View.ViewContract.TenantTypeId.Equals(AppConsts.NONE))
                {
                    tenant.TenantTypeID = null;
                }

                if (View.ViewContract.TenantTypeId == Convert.ToInt32(TenantType.Institution))
                {
                    DatabaseExistsStatus dbexist = IsTenantDBExists(View.ViewContract.TenantConnectinString, null);
                    if (dbexist == DatabaseExistsStatus.EXISTING)
                    {
                        View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_DATABASE_ALREADY_EXISTS);
                        return;
                    }

                    //ClientDBConfiguration Section
                    ClientDBConfiguration clientDBConfiguration = new ClientDBConfiguration
                    {
                        CDB_DBName = View.ViewContract.TenantDBName,
                        CDB_DBServer = View.ViewContract.TenantDBServer,
                        CDB_UserName = View.ViewContract.TenantDBUserName,
                        CDB_Password = View.ViewContract.TenantDBPassword,
                        CDB_ConnectionString = View.ViewContract.TenantConnectinString
                    };
                    tenant.ClientDBConfigurations.Add(clientDBConfiguration);
                }

                Address tenantAddress = new Address
                                            {
                                                Address1 = View.ViewContract.TenantAddress,
                                                //AddressTypeID = GetAddressTypeIdByCode(AddressType.Primary.GetStringValue()),
                                                ZipCodeID = View.ViewContract.TenantZipId,
                                                CreatedByID = View.CurrentUserId,
                                                CreatedOn = DateTime.Now,
                                                IsActive = true,
                                                ZipCode = SecurityManager.GetZip(View.ViewContract.TenantZipId)
                                            };

                AddressHandle tenantAddressHandle = new AddressHandle { AddressHandleID = Guid.NewGuid() };

                tenant.AddressHandle = tenantAddressHandle;

                Contact contact = new Contact
                                      {
                                          FirstName = String.Empty,
                                          LastName = String.Empty,
                                          CreatedByID = View.CurrentUserId,
                                          CreatedOn = DateTime.Now
                                      };

                contact.AddContactDetail(GetContactTypeIdByCode(ContactType.PrimaryPhone.GetStringValue()), View.ViewContract.TenantPhone, View.CurrentUserId);
                tenant.CreatedByID = View.CurrentUserId;
                tenant.CreatedOn = DateTime.Now;
                tenant.IsActive = true;
                tenant.IsDeleted = false;

                tenantAddressHandle.Addresses.Add(tenantAddress);
                tenant.Contact = contact;

                // Tenant product section.
                TenantProduct tenantProduct = new TenantProduct
                {
                    Description = View.ViewContract.ProductDescription,
                    Name = View.ViewContract.ProductName,
                    CreatedOn = DateTime.Now,
                    CreatedByID = View.CurrentUserId,
                    IsActive = true,
                    IsDeleted = false
                };
                tenant.TenantProducts.Add(tenantProduct);

                // Organization section.
                Organization organization = new Organization
                {
                    OrganizationName = View.ViewContract.OrganizationName,
                    OrganizationDesc = View.ViewContract.OrganizationDescription,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByID = View.CurrentUserId,
                    CreatedOn = DateTime.Now
                };

                Address organizationAddress = new Address
                                                  {
                                                      Address1 = View.ViewContract.TenantAddress,
                                                      //AddressTypeID = GetAddressTypeIdByCode(AddressType.Primary.GetStringValue()),
                                                      ZipCodeID = View.ViewContract.OrganizationZipId,
                                                      CreatedByID = View.CurrentUserId,
                                                      CreatedOn = DateTime.Now,
                                                      IsActive = true,
                                                      ZipCode = SecurityManager.GetZip(View.ViewContract.OrganizationZipId)
                                                  };

                AddressHandle organizationAddressHandle = new AddressHandle { AddressHandleID = Guid.NewGuid() };
                organizationAddressHandle.Addresses.Add(organizationAddress);

                Contact organizationContact = new Contact { FirstName = String.Empty, LastName = String.Empty };

                organizationContact.AddContactDetail(GetContactTypeIdByCode(ContactType.PrimaryPhone.GetStringValue()), View.ViewContract.OrganizationPhone, View.CurrentUserId);
                organizationContact.CreatedByID = View.CurrentUserId;
                organizationContact.CreatedOn = DateTime.Now;
                organization.AddressHandle = organizationAddressHandle;
                organization.Contact = organizationContact;
                tenant.Organizations.Add(organization);
                if (!View.OrganizationPrefixes.IsNull())
                {
                    foreach (OrganizationUserNamePrefix prefix in View.OrganizationPrefixes)
                    {
                        if (prefix.IsActive == true)
                            tenant.Organizations.FirstOrDefault().OrganizationUserNamePrefixes.Add(prefix);
                    }
                }

                Tenant newTenant = SecurityManager.AddTenant(contact, tenant);
                SecurityManager.AddDefaultRole(tenant);
                if (View.ViewContract.TenantTypeId == Convert.ToInt32(TenantType.Institution))
                {
                    //Also pass new Tenant's ConnectionString
                    ClientSecurityManager.UpdateDefaultEntryForNewClient(newTenant.TenantID, View.ViewContract.TenantConnectinString);
                }
                View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.TENANT) + SysXUtils.GetMessage(ResourceConst.SPACE) + tenant.TenantName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SAVED_SUCCESSFULLY);
            }
        }

        /// <summary>
        ///  Performs an update operation for tenant.
        /// </summary>
        public void TenantUpdate()
        {
            if (View.ViewContract.TenantTypeId == Convert.ToInt32(TenantType.Institution))
            {
                if (View.ViewContract.TenantDBServer == String.Empty)
                {
                    View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_DATABASE_SERVER_REQUIRED);
                    return;
                }
                if (View.ViewContract.TenantDBName == String.Empty)
                {
                    View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_DATABASE_NAME_REQUIRED);
                    return;
                }
                if (View.ViewContract.TenantDBUserName == String.Empty)
                {
                    View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_DATABASE_USER_NAME_REQUIRED);
                    return;
                }
                if (View.ViewContract.TenantDBPassword == String.Empty)
                {
                    View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_DATABASE_PASSWORD_REQUIRED);
                    return;
                }
                if (View.ViewContract.TenantConnectinString == String.Empty)
                {
                    View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_TENANT_CONN_STRING);
                    return;
                }
                DatabaseExistsStatus dbexist = IsValidTenantConnectionString(View.ViewContract.TenantConnectinString);
                if (dbexist == DatabaseExistsStatus.NO_CONNECTION)
                {
                    View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_TENANT_CONN_STRING_NOT_VALID);
                    return;
                }
                if (dbexist == DatabaseExistsStatus.NO_DB)
                {
                    View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_TENANT_DB_NOT_EXISTS);
                    return;
                }
            }
            Tenant tenant = SecurityManager.GetTenant(View.ViewContract.TenantId);
            TenantProduct tenantProduct = SecurityManager.GetProductsForTenant(View.ViewContract.TenantId).FirstOrDefault();
            Organization organizationUpdate = SecurityManager.GetOrganizationForTenant(View.ViewContract.TenantId);

            if (SecurityManager.IsTenantExists(View.ViewContract.TenantName, tenant.TenantName))
            {
                View.ErrorMessage = View.ViewContract.TenantName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_TENANT_EXISTS);
            }
            else if (!tenantProduct.IsNull() && SecurityManager.IsProductExists(View.ViewContract.ProductName, tenantProduct.Name))
            {
                View.ErrorMessage = View.ViewContract.ProductName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_PRODUCT_EXISTS);
            }
            else if (SecurityManager.IsOrganizationExists(View.ViewContract.OrganizationName, organizationUpdate.OrganizationName))
            {
                View.ErrorMessage = View.ViewContract.OrganizationName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_ORGANIZATION_EXISTS);
            }
            else
            {
                View.ErrorMessage = String.Empty;

                tenant.TenantName = View.ViewContract.TenantName;
                tenant.TenantDesc = View.ViewContract.TenantDesc;
                tenant.TenantTypeID = View.ViewContract.TenantTypeId;

                if (View.ViewContract.TenantTypeId.Equals(AppConsts.NONE))
                {
                    tenant.TenantTypeID = null;
                }

                if (View.ViewContract.TenantTypeId == Convert.ToInt32(TenantType.Institution))
                {
                    //ClientDBConfiguration Section
                    ClientDBConfiguration clientDBConfiguration = tenant.ClientDBConfigurations.FirstOrDefault();

                    if (!clientDBConfiguration.IsNull())
                    {
                        DatabaseExistsStatus dbexist = IsTenantDBExists(View.ViewContract.TenantConnectinString, View.ViewContract.TenantId);
                        if (dbexist == DatabaseExistsStatus.EXISTING)
                        {
                            View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_DATABASE_ALREADY_EXISTS);
                            return;
                        }

                        clientDBConfiguration.CDB_DBName = View.ViewContract.TenantDBName;
                        clientDBConfiguration.CDB_DBServer = View.ViewContract.TenantDBServer;
                        clientDBConfiguration.CDB_UserName = View.ViewContract.TenantDBUserName;
                        clientDBConfiguration.CDB_Password = View.ViewContract.TenantDBPassword;
                        clientDBConfiguration.CDB_ConnectionString = View.ViewContract.TenantConnectinString;

                        tenant.ClientDBConfigurations.Add(clientDBConfiguration);
                    }
                    else
                    {
                        DatabaseExistsStatus dbexist = IsTenantDBExists(View.ViewContract.TenantConnectinString, null);
                        if (dbexist == DatabaseExistsStatus.EXISTING)
                        {
                            View.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_DATABASE_ALREADY_EXISTS);
                            return;
                        }

                        clientDBConfiguration = new ClientDBConfiguration();

                        clientDBConfiguration.CDB_DBName = View.ViewContract.TenantDBName;
                        clientDBConfiguration.CDB_DBServer = View.ViewContract.TenantDBServer;
                        clientDBConfiguration.CDB_UserName = View.ViewContract.TenantDBUserName;
                        clientDBConfiguration.CDB_Password = View.ViewContract.TenantDBPassword;
                        clientDBConfiguration.CDB_ConnectionString = View.ViewContract.TenantConnectinString;

                        tenant.ClientDBConfigurations.Add(clientDBConfiguration);
                    }
                }

                Address tenantAddress = tenant.AddressHandle.Addresses.FirstOrDefault();

                if (!tenantAddress.IsNull())
                {
                    tenantAddress.Address1 = View.ViewContract.TenantAddress;
                    tenantAddress.ZipCodeID = View.ViewContract.TenantZipId;
                    tenantAddress.ModifiedByID = View.CurrentUserId;
                    tenantAddress.ModifiedOn = DateTime.Now;
                }

                tenant.ModifiedByID = View.CurrentUserId;
                tenant.ModifiedOn = DateTime.Now;

                tenant.Contact.FirstName = String.Empty;
                tenant.Contact.LastName = String.Empty;
                tenant.Contact.UpdateContactDetail(GetContactTypeIdByCode(ContactType.PrimaryPhone.GetStringValue()), View.ViewContract.TenantPhone, View.CurrentUserId);
                tenant.Contact.ModifiedByID = View.CurrentUserId;
                tenant.Contact.ModifiedOn = DateTime.Now;

                // Tenant product section.
                if (!tenantProduct.IsNull())
                {
                    tenantProduct.Description = View.ViewContract.ProductDescription;
                    tenantProduct.Name = View.ViewContract.ProductName;
                    tenantProduct.ModifiedByID = View.CurrentUserId;
                    tenantProduct.ModifiedOn = DateTime.Now;

                    tenant.TenantProducts.Add(tenantProduct);
                }

                //Organization section.
                Organization organization = SecurityManager.GetOrganizationForTenant(View.ViewContract.TenantId);
                organization.OrganizationName = View.ViewContract.OrganizationName;
                organization.OrganizationDesc = View.ViewContract.OrganizationDescription;
                organization.ModifiedByID = View.CurrentUserId;
                organization.ModifiedOn = DateTime.Now;

                Address organizationAddress = organization.AddressHandle.Addresses.FirstOrDefault();

                if (!organizationAddress.IsNull())
                {
                    organizationAddress.Address1 = View.ViewContract.OrganizationAddress;
                    organizationAddress.ZipCodeID = View.ViewContract.OrganizationZipId;
                    organizationAddress.ModifiedByID = View.CurrentUserId;
                    organizationAddress.ModifiedOn = DateTime.Now;
                }

                organization.Contact.FirstName = String.Empty;
                organization.Contact.LastName = String.Empty;
                organization.Contact.UpdateContactDetail(GetContactTypeIdByCode(ContactType.PrimaryPhone.GetStringValue()), View.ViewContract.OrganizationPhone, View.CurrentUserId);
                organization.Contact.ModifiedByID = View.CurrentUserId;
                organization.Contact.ModifiedOn = DateTime.Now;

                tenant.Organizations.Add(organization);

                // code for updated prefixed
                foreach (OrganizationUserNamePrefix prefix in View.OrganizationPrefixes)
                {
                    OrganizationUserNamePrefix userprefix = tenant.Organizations.FirstOrDefault().OrganizationUserNamePrefixes.Where(condtion => condtion.OrganizationUserNamePrefixID == prefix.OrganizationUserNamePrefixID).FirstOrDefault();
                    if (!userprefix.IsNull())
                    {
                        userprefix.UserNamePrefix = prefix.UserNamePrefix;
                        userprefix.Description = prefix.Description;
                        userprefix.ModifiedByID = prefix.ModifiedByID;
                        userprefix.ModifiedOn = prefix.ModifiedOn;
                        userprefix.IsActive = prefix.IsActive;
                    }
                }
                //end code here of prefix

                // code for newly added organization user prefix
                List<OrganizationUserNamePrefix> NewAddedPrefix = View.OrganizationPrefixes.Where(cond => cond.OrganizationUserNamePrefixID == AppConsts.NONE && cond.IsActive == true).ToList();
                foreach (OrganizationUserNamePrefix prefix in NewAddedPrefix)
                {
                    prefix.OrganizationID = tenant.Organizations.FirstOrDefault().OrganizationID;
                    SecurityManager.AddOrganizationUserPrefix(prefix);
                }

                SecurityManager.UpdateTenant(tenant);
                if (View.ViewContract.TenantTypeId == Convert.ToInt32(TenantType.Institution))
                {
                    //Also pass new Tenant's ConnectionString
                    ClientSecurityManager.UpdateDefaultEntryForNewClient(tenant.TenantID, View.ViewContract.TenantConnectinString);
                }
                View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.TENANT) + SysXUtils.GetMessage(ResourceConst.SPACE) + tenant.TenantName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.UPDATED_SUCCESSFULLY);
            }
        }

        public DatabaseExistsStatus IsValidTenantConnectionString(String ConnectionString)
        {
            try
            {
                EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
                entityBuilder.ProviderConnectionString = ConnectionString;
                //entityBuilder.Provider = AppConsts.SQL_DATA_PROVIDER;
                //entityBuilder.Metadata = AppConsts.TENANT_ENTITY_METADATA;
                entityBuilder.Provider = "System.Data.SqlClient";
                entityBuilder.Metadata = @"res://*/ClientEntity.ADBClientEntity.csdl|res://*/ClientEntity.ADBClientEntity.ssdl|res://*/ClientEntity.ADBClientEntity.msl";
                using (var db = new Entity.ClientEntity.ADB_LibertyUniversity_ReviewEntities(entityBuilder.ToString()))
                {
                    bool DbExists = db.DatabaseExists();
                    if (DbExists)
                    {
                        // database is existing
                        return DatabaseExistsStatus.EXISTING;
                    }
                    else
                    {
                        // config is working, but database does not exist
                        return DatabaseExistsStatus.NO_DB;
                    }
                }
            }
            catch (Exception)
            {
                // no working config
                return DatabaseExistsStatus.NO_CONNECTION;
            }
        }

        public DatabaseExistsStatus IsTenantDBExists(String ConnectionString, Int32? TenantId)
        {
            try
            {
                Boolean DbExists = SecurityManager.GetTenantConnectionStringByConnectionStringAndUserId(ConnectionString, TenantId);

                if (DbExists)
                {
                    // database is existing
                    return DatabaseExistsStatus.EXISTING;
                }
                else
                {
                    // config is working, but database does not exist
                    return DatabaseExistsStatus.NO_DB;
                }
            }
            catch (Exception)
            {
                // no working config
                return DatabaseExistsStatus.NO_CONNECTION;
            }
        }

        /// <summary>
        /// Performa a delete operation for tenant.
        /// </summary>
        public void TenantDelete()
        {
            Tenant tenant = SecurityManager.GetTenant(View.ViewContract.TenantId);
            tenant.IsDeleted = true;
            tenant.ModifiedByID = View.CurrentUserId;
            View.SuccessMessage = SysXUtils.GetMessage(ResourceConst.TENANT) + SysXUtils.GetMessage(ResourceConst.SPACE) + tenant.TenantName + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.DELETED_SUCCESSFULLY);
            SecurityManager.DeleteTenantWithAllDependent(tenant);

            // Resolved Bug #6603 - ADB Manage Assignments – List of deleted Third Party organizations appears in the Third Party Reviewer drop down list in Assignment Properties screen.
            // Delete all its relationship in ClientRelation
            if (tenant.TenantTypeID == 5)
            {
                List<ClientRelation> clientRelations = SecurityManager.GetClientRelationBasedonRelatedID(tenant.TenantID);
                foreach (var clientRelation in clientRelations)
                {
                    clientRelation.IsDeleted = true;
                    clientRelation.IsActive = false;
                    clientRelation.ModifiedByID = View.CurrentUserId;
                    clientRelation.ModifiedOn = DateTime.Now;
                    SecurityManager.DeleteSubTenant(clientRelation);
                    ComplianceDataManager.DeleteSubTenant(clientRelation.TenantID, clientRelation.RelatedTenantID, View.CurrentUserId);
                }
            }



        }

        /// <summary>
        /// Retrieves a list of all Zip Code.
        /// </summary>
        public List<ZipCode> GetZipCodesByZipCodeNumber(String zipCode)
        {
            return SecurityManager.GetZipCodesByZipCodeNumber(zipCode).ToList();
        }

        /// <summary>
        /// Retrieves the application configuration value based on SysXKey.
        /// </summary>
        /// <param name="sysXKey">The sys X key.</param>
        public String GetSysXConfigValue(String sysXKey)
        {
            return SecurityManager.GetSysXConfigValue(sysXKey);
        }

        public Boolean IsUserNamePrefixExist(String prefixName, Int32 prefixid)
        {
            Boolean IsExist = false;
            OrganizationUserNamePrefix userPrefix = SecurityManager.getAllUserNamePrefix().Where(cond => cond.UserNamePrefix.Equals(prefixName) && cond.OrganizationUserNamePrefixID != prefixid).FirstOrDefault();
            if (!userPrefix.IsNull())
            {
                IsExist = true;
            }
            return IsExist;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Get AddressTypeId By AddressTypeCode
        /// </summary>
        /// <param name="addressTypeCode"></param>
        /// <returns></returns>
        //private Int32 GetAddressTypeIdByCode(String addressTypeCode)
        //{
        //    return LookupManager.GetLookUpIDbyCode<lkpAddressType>(action => action.AddressTypeCode.Equals(addressTypeCode));
        //}

        /// <summary>
        /// Get ContactTypeId By ContactTypeCode
        /// </summary>
        /// <param name="contactTypeCode"></param>
        /// <returns></returns>
        private Int32 GetContactTypeIdByCode(String contactTypeCode)
        {
            return LookupManager.GetLookUpIDbyCode<lkpContactType>(cond => cond.ContactCode.Equals(contactTypeCode));
        }

        #endregion

        #endregion



        public String GetFormattedPhoneNumber(String phone)
        {
            return ApplicationDataManager.GetFormattedPhoneNumber(phone);
        }
    }
}