#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  LookupManager.cs
// Purpose:   To get Lookup Data
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Core.Objects.DataClasses;


#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using INTSOF.AppFabricCacheServer;
using System.Data.Entity.Core.EntityClient;
using System.Configuration;
#endregion

#endregion

namespace Business.RepoManagers
{
    /// <summary>
    /// LookupManager
    /// </summary>
    public static class LookupManager
    {

        public static CommunicationSubEvents GetSubEventEnumTypeByCode(String enumCode)
        {
            switch (enumCode)
            {
                case "NTIODRCMO":
                    return CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_MONEY_ORDER;

                case "NTIODRCIN":
                    return CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_INVOICE;

                case "NTIODRA":
                    return CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL;

                case "NTIODRR":
                    return CommunicationSubEvents.NOTIFICATION_ORDER_REJECTION;

                case "NTIODRD":
                    return CommunicationSubEvents.NOTIFICATION_ORDER_COMPLETION;

                case "NTICRDT":
                    return CommunicationSubEvents.NOTIFICATION_CREDIT_CARD;

                case "NTIMNOD":
                    return CommunicationSubEvents.NOTIFICATION_MONEY_ORDER;

                case "NTIMBSP":
                    return CommunicationSubEvents.NOTIFICATION_MBS_PAYMENT;

                case "NTISBSN":
                    return CommunicationSubEvents.NOTIFICATION_NEW_SUBSCRIPTIONS;

                case "NTISBSE":
                    return CommunicationSubEvents.NOTIFICATION_EXPIRED_SUBSCRIPTIONS;

                case "NTISBSR":
                    return CommunicationSubEvents.NOTIFICATION_RENEWABLE_SUBSCRIPTION;

                case "NTIPRFC":
                    return CommunicationSubEvents.NOTIFICATION_PROFILE_CHANGE;

                case "NTIACTS":
                    return CommunicationSubEvents.NOTIFICATION_ACCOUNT_STATUS;

                case "NTIIMSG":
                    return CommunicationSubEvents.NOTIFICATION_INTERNAL_MESSAGES;

                case "NTIODRCNC":
                    return CommunicationSubEvents.NOTIFICATION_ORDER_CANCELLATION;

                case "NTIODRCNCA":
                    return CommunicationSubEvents.NOTIFICATION_ORDER_CANCELLATION_APPROVED;

                case "NTIODRCNCR":
                    return CommunicationSubEvents.NOTIFICATION_ORDER_CANCELLATION_REJECTED;

                case "NTICITME":
                    return CommunicationSubEvents.COMPLIANCE_ITEM_EXPIRED;

                case "NTIRSHOCNF":
                    return CommunicationSubEvents.NOTIFICATION_RUSH_ORDER_CONFIRMATION;

                case "ALTODRD":
                    return CommunicationSubEvents.ALERT_ORDER_DENIED;

                case "ALTSBSE":
                    return CommunicationSubEvents.ALERT_SUBSCRIPTION_EXPIRE;

                case "ALTPRFC":
                    return CommunicationSubEvents.ALERT_PROFILE_CHANGE;

                case "ALTACTS":
                    return CommunicationSubEvents.ALERT_ACCOUNT_STATUS;

                case "ALTIMSG":
                    return CommunicationSubEvents.ALERT_INTERNAL_MESSAGES;

                case "ALTSCHE":
                    return CommunicationSubEvents.ALERT_START_OF_EVENT;

                case "ALTSCHM":
                    return CommunicationSubEvents.ALERT_EVENT_MODIFICATIONS;

                case "RMRSBSE":
                    return CommunicationSubEvents.REMINDER_SUBSCRIPTION_EXPIRE;

                case "RMRIMSG":
                    return CommunicationSubEvents.REMINDER_INTERNAL_MESSAGES;

                case "RMRSBPND":
                    return CommunicationSubEvents.REMINDER_SUBSCRIPTION_PENDING;

                case "NTCACRSRCH":
                    return CommunicationSubEvents.NOTIFICATION_FOR_CANADA_NATIONAL_CRIMINAL_SEARCH;

                case "NTCOCABREG":
                    return CommunicationSubEvents.NOTIFICATION_FOR_CO_CHILD_ABUSE_REGISTRY;

                case "NTDCABRELF":
                    return CommunicationSubEvents.NOTIFICATION_FOR_DELAWARE_CHILD_ABUSE_RELEASE_FORM;

                case "NTFLAEFBIP":
                    return CommunicationSubEvents.NOTIFICATION_FOR_FL_AHCA_ELECTRONIC_FBI_PROCESSING;

                case "NTICABRELF":
                    return CommunicationSubEvents.NOTIFICATION_FOR_IOWA_CHILD_ABUSE_RELEASE_FORM;

                case "NTIDEPADAB":
                    return CommunicationSubEvents.NOTIFICATION_FOR_IOWA_DEP_ADULT_ABUSE;

                case "NTMOCRGVRF":
                    return CommunicationSubEvents.NOTIFICATION_FOR_MO_CAREGIVER_FORM;

                case "NTPACABREG":
                    return CommunicationSubEvents.NOTIFICATION_FOR_PA_CHILD_ABUSE_REGISTRY;

                case "NTSTMOCGBS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_STL_CC_MO_CAREGIVER_BACKGROUND_SCREENING;

                case "NTSCMOCANR":
                    return CommunicationSubEvents.NOTIFICATION_FOR_STL_CC_MO_REQUEST_FOR_CHILD_ABUSE_NEGLECT_RECORD;

                case "NTTNFPTINS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_TN_FINGERPRINT_INSTRUCTIONS;

                case "NTVACHDABU":
                    return CommunicationSubEvents.NOTIFICATION_FOR_VA_CHILD_ABUSE;

                case "NTCMORDRES":
                    return CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS;

                case "NTCMSGRES":
                    return CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS;

                case "NTOCMOBKGP":
                    return CommunicationSubEvents.NOTIFICATION_FOR_ORDER_CREATION_MONEY_ORDER_AMSPACKAGES;

                case "NTOCINBKGP":
                    return CommunicationSubEvents.NOTIFICATION_FOR_ORDER_CREATION_INVOICE_AMSPACKAGES;

                case "NTOCMOBTHP":
                    return CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_MONEY_ORDER_AMS_COMPLIO_PACKAGES;

                case "NTOCIBOTHP":
                    return CommunicationSubEvents.NOTIFICATION_ORDER_CREATION_INVOICE_AMS_COMPLIO_PACKAGES;

                case "NTAPPBOTHP":
                    return CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL_AMS_COMPLIO_PACKAGES;

                case "NTOAPPBKGP":
                    return CommunicationSubEvents.NOTIFICATION_ORDER_APPROVAL_AMS_PACKAGES;

                case "NTCCOABKGP":
                    return CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_AMS_PACKAGES;

                case "NTCCOABTHP":
                    return CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_AMS_COMPLIO_PACKAGES;

                case "NTCCORDACP":
                    return CommunicationSubEvents.CREDIT_CARD_ORDER_APPROVAL_FOR_COMPLIO_PACKAGES;

                case "NTCCROCNF":
                    return CommunicationSubEvents.CREDIT_CARD_RUSH_ORDER_CONFIRMATION;

                case "NTCTCABREG":
                    return CommunicationSubEvents.NOTIFICATION_FOR_CT_CHILD_ABUSE_REGISTRY_CHECK;

                case "NTCACRSRDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_CANADA_NATIONAL_CRIMINAL_SEARCH_FORM_DISPATCHED;

                case "NTDRSCALDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_DRUG_SCREENING_10_PANELS_ALCOHOL_FORM_DISPATCHED;

                case "NTDRSCPNDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_DRUG_SCREENING_10_PANELS_FORM_DISPATCHED;

                case "NTPLDRSCDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_PAPERLESS_DRUG_SCREEN_FORM_DISPATCHED;

                case "NTDRSCSPDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_DRUG_SCREENING_10_SPLIT_FORM_DISPATCHED;

                case "NTFBIFRMDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_FBI_FORM_DISPATCHED;

                case "NTFBIFPCDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_FBI_FINGERPRINT_CHECK_FORM_DISPATCHED;

                case "NTIOCHABDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_IOWA_CHILD_ABUSE_RELEASE_FORM_DISPATCHED;

                case "NTPACHABDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_PA_CHILD_ABUSE_REGISTRY_FORM_DISPATCHED;

                case "NTSMCHABDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_STL_CC_MO_REQUEST_FOR_CHILD_ABUSE_NEGLECT_RECORD_FORM_DISPATCHED;

                case "NTIOADABDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_IOWA_DEP_ADULT_ABUSE_FORM_DISPATCHED;

                case "NTDRSCPLDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_DRUG_SCREENING_10_PAPERLESS_FORM_DISPATCHED;

                case "NTDRSCPQDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_DRUG_SCREENING_10_PAPERLESS_Q_FORM_DISPATCHED;

                case "NTDLWACADS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_DELAWARE_CHILD_ABUSE_RELEASE_FORM_DISPATCHED;

                case "NTVACAFRDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_VA_CHILD_ABUSE_FORM_DISPATCHED;

                case "NTDSPLLDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_DRUG_SCREENING_10_PAPERLESS_L_FORM_DISPATCHED;

                case "NTPADPEFDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_PA_DPW_ELECTRONIC_FINGERPRINT_FORM_DISPATCHED;

                case "NTTNFNINDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_TN_FINGERPRINT_INSTRUCTIONS_FORM_DISPATCHED;

                case "NTDSMDTSDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_DRUG_SCREENING_10_PANELS_MDA_TS_FORM_DISPATCHED;

                case "NTFLFBIPDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_FL_ELECTRONIC_FBI_PROCESSING_FORM_DISPATCHED;

                case "NTPARLFPDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_PA_DPW_ROLLED_FINGERPRINT_FORM_DISPATCHED;

                case "NTDRPLQNDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_DRUG_5_PL_Q_6405N_FORM_DISPATCHED;

                case "NTMOCAFRDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_MO_CAREGIVER_FORM_DISPATCHED;

                case "NTSCMCBSDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_STL_CC_MO_CAREGIVER_BACKGROUND_SCREENING_FORM_DISPATCHED;

                case "NTDSPLQNDS":
                    return CommunicationSubEvents.Notification_For_Drug_Screen_PL_Q_21832N_Form_Dispatched;

                case "NTCOCARGDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_CO_CHILD_ABUSE_REGISTRY_FORM_DISPATCHED;

                case "NTFLEFBIDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_FL_E_FBI_NONRESIDENT_FORM_DISPATCHED;

                case "NTCTCARGDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_CT_CHILD_ABUSE_REGISTRY_CHECK_FORM_DISPATCHED;

                case "NTFLAHEFDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_FL_AHCA_ELECTRONIC_FBI_PROCESSING_FORM_DISPATCHED;

                case "NTFBIFPSF":
                    return CommunicationSubEvents.NOTIFICATION_FOR_FBI_FINGERPRINT_SERVICE_FORM;

                case "NTMOCANGSF":
                    return CommunicationSubEvents.NOTIFICATION_FOR_MO_CHILD_ABUSE_NEGLECT;

                case "NTWGFLAHSF":
                    return CommunicationSubEvents.NOTIFICATION_FOR_WGU_FL_AHCA_FORM;

                case "NTFBIFPDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_FBI_FINGERPRINT_SERVICE_FORM_DISPATCHED;

                case "NTMOCANGDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_MO_CHILD_ABUSE_NEGLECT_FORM_DISPATCHED;

                case "NTWGFLAHDS":
                    return CommunicationSubEvents.NOTIFICATION_FOR_WGU_FL_AHCA_FORM_DISPATCHED;

                case "NTFLFBIPSF":
                    return CommunicationSubEvents.NOTIFICATION_FOR_VECHS_FL_ELECTRONIC_FBI_PROCESSING_FORM;

                case "NTFLORDRES":
                    return CommunicationSubEvents.NOTIFICATION_FOR_FLAGGED_ORDER_RESULTS;

                default:
                    return CommunicationSubEvents.NONE;
            }
        }

        /// <summary>GetAllContactTypes
        /// GetContactTypes()
        /// </summary>
        /// <returns></returns>
        public static List<lkpContactType> GetAllContactTypes()
        {
            return (SysXCacheUtils.GetAddCacheLookup<lkpContactType>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()));
        }

        /// <summary>GetAllContactTypes
        /// Get list of ClientDBConfiguration from Master database using Caching Service.
        /// </summary>
        /// <returns></returns>
        public static List<ClientDBConfiguration> GetAllClientDBConfiguration()
        {
            return (SysXCacheUtils.GetAddCacheLookup<ClientDBConfiguration>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()));
        }

        /// <summary>
        /// GetOrgUserTypes()
        /// </summary>
        /// <returns></returns>
        public static List<Entity.lkpOrgUserType> GetAllOrgUserTypes()
        {
            return (SysXCacheUtils.GetAddCacheLookup<Entity.lkpOrgUserType>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()));
        }

        /// <summary>
        /// GetTenantTypes()
        /// </summary>
        /// <returns></returns>
        public static List<Entity.lkpTenantType> GetAllTenantTypes()
        {
            return (SysXCacheUtils.GetAddCacheLookup<Entity.lkpTenantType>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()));
        }

        public static List<TEntity> GetLookUpData<TEntity>() where TEntity : EntityObject
        {
            return SysXCacheUtils.GetAddCacheLookup<TEntity>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()).ToList();
        }

        /// <summary>
        /// Get Look Up data of Type TEntity
        /// </summary>
        /// <returns>List of type TEntity </returns>
        public static List<TEntity> GetLookUpData<TEntity>(Int32? tenantId) where TEntity : EntityObject
        {
            return SysXCacheUtils.GetAddCacheLookup<TEntity>(ADBDatabaseDetail.ADBDatabaseType.TENANT_DB.GetStringValue(), GetConnectionString(tenantId), tenantId).ToList();
        }

        /// <summary>
        /// Get Look Up data of Type TEntity
        /// </summary>
        /// <returns>List of type TEntity </returns>
        public static List<TEntity> GetMessagingLookUpData<TEntity>() where TEntity : EntityObject
        {
            return SysXCacheUtils.GetAddCacheLookup<TEntity>(ADBDatabaseDetail.ADBDatabaseType.MESSAGING_DB.GetStringValue()).ToList();
        }

        public static List<TEntity> GetLanguageLookUpData<TEntity>() where TEntity : EntityObject
        {
            return SysXCacheUtils.GetAddCacheLookup<TEntity>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()).ToList();
        }

        public static List<TEntity> GetSharedDBLookUpData<TEntity>() where TEntity : EntityObject
        {
            return SysXCacheUtils.GetAddCacheLookup<TEntity>(ADBDatabaseDetail.ADBDatabaseType.SHAREDDATA_DB.GetStringValue()).ToList();
        }

        /// <summary>
        /// Get Id of Look Up, in the SharedDatabase
        /// </summary>
        /// <returns>Id </returns>
        public static Int32 GetSharedLookUpIDbyCode<TEntity>(Func<TEntity, bool> predicate) where TEntity : EntityObject
        {
            TEntity Enity = SysXCacheUtils.GetAddCacheLookup<TEntity>(ADBDatabaseDetail.ADBDatabaseType.SHAREDDATA_DB.GetStringValue()).SingleOrDefault<TEntity>(predicate);
            return Convert.ToInt32(Enity.EntityKey.EntityKeyValues.FirstOrDefault().Value);
        }

        /// <summary>
        /// Get Id of Look Up
        /// </summary>
        /// <returns>Id </returns>
        public static Int32 GetLookUpIDbyCode<TEntity>(Func<TEntity, bool> predicate) where TEntity : EntityObject
        {
            TEntity Enity = SysXCacheUtils.GetAddCacheLookup<TEntity>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()).SingleOrDefault<TEntity>(predicate);
            return Convert.ToInt32(Enity.EntityKey.EntityKeyValues.FirstOrDefault().Value);
        }

        /// <summary>
        /// Get code of Look Up
        /// </summary>
        /// <returns>Id </returns>
        public static String GetLookUpCodebyID<TEntity>(Func<TEntity, bool> predicate, Func<TEntity, String> selector) where TEntity : EntityObject
        {
            return SysXCacheUtils.GetAddCacheLookup<TEntity>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()).Where(predicate).Select(selector).FirstOrDefault();
            //return Convert.ToInt32(Enity.EntityKey.EntityKeyValues.FirstOrDefault().Value);
        }

        /// <summary>
        /// Get Connection String
        /// </summary>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        private static string GetConnectionString(Int32? TenantId)
        {
            if (TenantId.IsNotNull())
            {

                EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
                //ISecurityRepository objSecurity = new SecurityRepository();
                //string tenantConnectionString = BALUtils.GetSecurityRepoInstance().GetClientConnectionString(Convert.ToInt32(TenantId));
                string tenantConnectionString = GetLookUpCodebyID<ClientDBConfiguration>(fx => fx.CDB_TenantID == TenantId, fd => fd.CDB_ConnectionString);
                entityBuilder.ProviderConnectionString = tenantConnectionString;
                entityBuilder.Provider = "System.Data.SqlClient";
                entityBuilder.Metadata = @"res://*/ClientEntity.ADBClientEntity.csdl|res://*/ClientEntity.ADBClientEntity.ssdl|res://*/ClientEntity.ADBClientEntity.msl";
                return entityBuilder.ToString();
            }
            return null;
        }

        #region Rule Lookups

        public static List<Entity.ClientEntity.lkpRuleType> GetAllRuleTypes(Int32? tenantId)
        {
            String ConnectionString = null;
            if (tenantId.IsNotNull())
            {
                ConnectionString = new DAL.Repository.RuleRepository(Convert.ToInt32(tenantId)).ClientDBContext.Connection.ConnectionString;
            }
            return (SysXCacheUtils.GetAddCacheLookup<Entity.ClientEntity.lkpRuleType>(ConnectionString));
        }

        public static List<Entity.ClientEntity.lkpRuleResultType> GetAllRuleResultTypes(Int32? tenantId)
        {
            String ConnectionString = null;
            if (tenantId.IsNotNull())
            {
                ConnectionString = new DAL.Repository.RuleRepository(Convert.ToInt32(tenantId)).ClientDBContext.Connection.ConnectionString;
            }
            return (SysXCacheUtils.GetAddCacheLookup<Entity.ClientEntity.lkpRuleResultType>(ConnectionString));
        }

        public static List<Entity.ClientEntity.lkpRuleActionType> GetAllRuleActionTypes(Int32? tenantId)
        {
            String ConnectionString = null;
            if (tenantId.IsNotNull())
            {
                ConnectionString = new DAL.Repository.RuleRepository(Convert.ToInt32(tenantId)).ClientDBContext.Connection.ConnectionString;
            }
            return (SysXCacheUtils.GetAddCacheLookup<Entity.ClientEntity.lkpRuleActionType>(ConnectionString));
        }

        public static List<Entity.ClientEntity.lkpExpressionOperator> GetAllExpressionOperators(Int32? tenantId)
        {
            String ConnectionString = null;
            if (tenantId.IsNotNull())
            {
                ConnectionString = new DAL.Repository.RuleRepository(Convert.ToInt32(tenantId)).ClientDBContext.Connection.ConnectionString;
            }
            return (SysXCacheUtils.GetAddCacheLookup<Entity.ClientEntity.lkpExpressionOperator>(ConnectionString));
        }

        #endregion

        #region AMS

        /// <summary>GetBusinessChannelTypes
        /// GetBusinessChannelTypes()
        /// </summary>
        /// <returns></returns>
        public static List<Entity.lkpBusinessChannelType> GetBusinessChannelTypes()
        {
            return (SysXCacheUtils.GetAddCacheLookup<Entity.lkpBusinessChannelType>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()));
        }

        #endregion

        #region CreateOrderService

        /// <summary>GetExternalVendorsForAMS
        /// Get list of ExternalVendors for AMS using Caching Service.
        /// </summary>
        /// <returns></returns>
        public static List<ExternalVendor> GetExternalVendorsForAMS()
        {
            return (SysXCacheUtils.GetAddCacheLookup<ExternalVendor>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()));
        }

        public static List<State> GetAllStates()
        {
            return (SysXCacheUtils.GetAddCacheLookup<State>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()));
        }

        #endregion

        #region UAT-1218
        /// <summary>
        /// Method to Get All User Type Switch Views
        /// </summary>
        /// <returns></returns>
        public static List<lkpUserTypeSwitchView> GetAllUserTypeSwitchView()
        {
            return (SysXCacheUtils.GetAddCacheLookup<lkpUserTypeSwitchView>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()));
        }
        #endregion

        #region Admin Entry Portal
        /// <summary>GetFeatureAreaType
        /// GetFeatureAreaType()
        /// </summary>
        /// <returns></returns>
        public static List<Entity.lkpFeatureAreaType> GetFeatureAreaType()
        {
            return (SysXCacheUtils.GetAddCacheLookup<Entity.lkpFeatureAreaType>(ADBDatabaseDetail.ADBDatabaseType.SECURITY_DB.GetStringValue()));
        }

        #endregion
    }
}
