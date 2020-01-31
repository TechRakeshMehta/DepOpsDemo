using System;
using System.Web;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using INTERSOFT.WEB.UI.WebControls;
using Telerik.Web.UI;
using System.ComponentModel;
using CoreWeb.Shell;
using Business.RepoManagers;
using Entity;
using System.Linq;
using INTSOF.UI.Contract.SysXSecurityModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using CoreWeb.IntsofSecurityModel;
using System.Web.Security;
using INTSOF.Utils.Consts;
using System.Text.RegularExpressions;


namespace CoreWeb
{
    public static class SessionSharingManagement
    {
        #region Methods

        #region Public Methods

        /// <summary>
        /// Method to store the current logged in user session in DB against a unique identifier.
        /// </summary>
        public static Guid? SetSessionForSharing(String destUrl)
        {
            String sessionData = String.Empty;
            CrossApplicationData applicationData = new CrossApplicationData();

            Guid sessionToken = Guid.NewGuid();
            if (sessionToken != null)
            {
                //Get the entity object data for session.
                byte[] sessionObjdata = CreateSessionObjectData();

                if (sessionObjdata != null)
                {
                    applicationData.CAD_Token = sessionToken;
                    applicationData.CAD_ObjectData = sessionObjdata;
                    applicationData.CAD_IsActive = true;
                    applicationData.CAD_Timespan = 10;
                    applicationData.CAD_CreatedOn = DateTime.Now;
                    applicationData.CAD_TargetURL = destUrl;
                    applicationData.CAD_TypeID = GetRedirectTokenTypeID(RedirectTokenType.MVPTOREACT.GetStringValue());
                }
            }

            if (!applicationData.IsNullOrEmpty() && SecurityManager.SetSessionData(applicationData))
            {
                return sessionToken;
            }
            return null;
        }

        /// <summary>
        /// Method to return target url after creating the session for the user.
        /// </summary>
        /// <param name="queryStringArgs"></param>
        /// <returns></returns>
        public static String GetTargetUrl(NameValueCollection queryString)
        {
            String targetUrl = String.Empty;
            if (queryString != null && !queryString.IsNullOrEmptyCollection())
            {
                //Dictionary<String, String> args = new Dictionary<String, String>();
                //args.ToDecryptedQueryString(queryString["args"]);

                Guid? token = null;

                //if (args.ContainsKey("Token") && !args["Token"].IsNull())
                //{
                //    token = new Guid(args["Token"]);
                //}
                if (!queryString["RMTokenKey"].IsNull())
                {
                    token = new Guid(queryString["RMTokenKey"]);
                }

                CrossApplicationData appData = GetSessionFromDB(token);

                if (!appData.IsNullOrEmpty())
                {
                    CreateUserSession(appData);
                    targetUrl = appData.CAD_TargetURL;
                    UpdateSessionActiveState(token);
                }
            }

            return targetUrl;
        }

        /// <summary>
        /// Method to update the active state of session. Use whenever the session expires.
        /// </summary>
        public static void UpdateSessionActiveState(Guid? token)
        {
            if (token != null)
                SecurityManager.UpdateSessionActiveState(token);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method TO get Session from DB on the basis of token.
        /// </summary>
        private static CrossApplicationData GetSessionFromDB(Guid? token)
        {
            if (!token.IsNullOrEmpty())
            {
                //Get data for session from DB using the token.
                return SecurityManager.GetSessionUsingToken(token);
            }
            return new CrossApplicationData();
        }

        /// <summary>
        /// Method to create the session of user on redirection.
        /// </summary>
        /// <param name="appData"></param>
        private static void CreateUserSession(CrossApplicationData appData)
        {
            if (!appData.IsNullOrEmpty())
            {
                //Dictionary<String, Object> sessionData = new Dictionary<String, Object>();
                //Object sessionObj = ByteArrayToObject(appData.CAD_ObjectData);
                //if (sessionObj != null)
                //    sessionData = DeserializeDictionaryValues<Object>(sessionObj);

                //if (!sessionData.IsNullOrEmpty())
                //{
                //    var sysBlockId = sessionData.Where(con => con.Key == RedirectionSessionData.SYSX_BLOCK_ID).FirstOrDefault().Value;
                //    var sysXBlockName = sessionData.Where(con => con.Key == RedirectionSessionData.SYSX_BLOCK_NAME).FirstOrDefault().Value;
                //    var user = sessionData.Where(con => con.Key == RedirectionSessionData.SYSX_MEMBERSHIP_USER).FirstOrDefault().Value;
                //    SysXWebSiteUtils.SessionService.SetSysXBlockId(Convert.ToInt32(sysBlockId));
                //    SysXWebSiteUtils.SessionService.SetSysXBlockName(Convert.ToString(sysXBlockName));
                //    SysXWebSiteUtils.SessionService.SetSysXMembershipUser((MembershipUser)user);

                //}

                SessionSharingContract sessionData = new SessionSharingContract();
                Object sessionObj = ByteArrayToObject(appData.CAD_ObjectData);
                if (sessionObj != null)
                    sessionData = DeserializeSessionData(sessionObj);

                if (!sessionData.IsNullOrEmpty())
                {
                    String userName = String.Empty;
                    var sysBlockId = sessionData.SysXBlockId;
                    var sysXBlockName = sessionData.SysXBlockName;

                    OrganizationUser orgUser;
                    if (sessionData.TenantId == 0 || sessionData.TenantId == null)
                        orgUser = SecurityManager.GetOrganizationUserInfoByUserId(sessionData.UserId).FirstOrDefault();
                    else
                        orgUser = SecurityManager.GetOrganizationUserInfoByUserId(sessionData.UserId).Where(con => con.Organization.TenantID == sessionData.TenantId).FirstOrDefault();
                    userName = orgUser.aspnet_Users.UserName;
                    SysXMembershipUser user = System.Web.Security.Membership.GetUser(Regex.Replace(userName, @"(?<=^\s*)\s|\s(?=\s*$)", SysXSecurityConst.ASCIISPACE)) as SysXMembershipUser;

                    SysXWebSiteUtils.SessionService.SetSysXBlockId(Convert.ToInt32(sysBlockId));
                    SysXWebSiteUtils.SessionService.SetSysXBlockName(Convert.ToString(sysXBlockName));
                    SysXWebSiteUtils.SessionService.SetSysXMembershipUser((MembershipUser)user);
                    SysXWebSiteUtils.SessionService.BusinessChannelType = sessionData.BusinessChannelType;
                }
            }
        }

        /// <summary>
        /// Method to create session object and return it in the form of byte array.
        /// </summary>
        /// <returns></returns>
        private static byte[] CreateSessionObjectData()
        {
            // Dictionary<String, Object> sessionData = new Dictionary<String, Object>();
            //Dictionary<String, String> serializedData = new Dictionary<String, String>();

            SessionSharingContract sessionData = new SessionSharingContract();
            String serializedData = String.Empty;

            //Bind Key-Value pair for session Data//
            //sessionData = BindSessionData();
            sessionData = BindSessionData();

            if (sessionData != null)
                //serializedData = SerializeDictionaryValues<Object>(sessionData);
                serializedData = SerializeSessionData(sessionData);

            //if (serializedData != null)
            //    return ObjectToByteArray(serializedData);

            if (serializedData != null)
                return ObjectToByteArray(serializedData);

            return null;
        }

        /// <summary>
        /// Method to bind the session data as key,value pair.
        /// </summary>
        /// <returns></returns>
        //private static Dictionary<String, Object> BindSessionData()
        private static SessionSharingContract BindSessionData()
        {
            //Dictionary<String, Object> sessionData = new Dictionary<String, Object>();

            //SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;

            //sessionData.Add(RedirectionSessionData.ORGANIZATION_USER_ID, SysXWebSiteUtils.SessionService.OrganizationUserId);
            //sessionData.Add(RedirectionSessionData.USER_ID, SysXWebSiteUtils.SessionService.UserId);
            //sessionData.Add(RedirectionSessionData.SYSX_BLOCK_ID, SysXWebSiteUtils.SessionService.SysXBlockId);
            //sessionData.Add(RedirectionSessionData.SYSX_BLOCK_NAME, SysXWebSiteUtils.SessionService.SysXBlockName);
            //sessionData.Add(RedirectionSessionData.USER_GOOGLE_AUTHENTICATED, SysXWebSiteUtils.SessionService.UserGoogleAuthenticated);
            //sessionData.Add(RedirectionSessionData.IS_SYSX_ADMIN, SysXWebSiteUtils.SessionService.IsSysXAdmin);
            //sessionData.Add(RedirectionSessionData.BUSINESS_CHANNEL_TYPE, SysXWebSiteUtils.SessionService.BusinessChannelType);
            //sessionData.Add(RedirectionSessionData.SYSX_MEMBERSHIP_USER, user);

            ////Session["PreferredSelectedTenant"];
            //return sessionData;


            SessionSharingContract sessionData = new SessionSharingContract();
            Entity.WebSite website = new Entity.WebSite();


            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;

            if (!user.IsNullOrEmpty())
            {

                if (!user.TenantId.IsNullOrEmpty() && user.TenantId > AppConsts.NONE)
                {
                    Int32 tenantId = Convert.ToInt32(user.TenantId);
                    website = WebSiteManager.GetWebSiteDetail(tenantId);
                }

                sessionData.OrganizationUserId = SysXWebSiteUtils.SessionService.OrganizationUserId;
                sessionData.UserId = SysXWebSiteUtils.SessionService.UserId;
                sessionData.SysXBlockId = SysXWebSiteUtils.SessionService.SysXBlockId;
                sessionData.SysXBlockName = SysXWebSiteUtils.SessionService.SysXBlockName;
                sessionData.UserGoogleAuthenticated = SysXWebSiteUtils.SessionService.UserGoogleAuthenticated;
                sessionData.IsSysXAdmin = SysXWebSiteUtils.SessionService.IsSysXAdmin;
                sessionData.BusinessChannelType = SysXWebSiteUtils.SessionService.BusinessChannelType;
                sessionData.TenantId = user.TenantId;
                sessionData.WebsiteId = !website.IsNullOrEmpty() && !website.WebSiteID.IsNullOrEmpty() && website.WebSiteID > AppConsts.NONE ? website.WebSiteID : AppConsts.NONE;
            }
            return sessionData;
        }

        /// <summary>
        /// Method to get redirect token type ID on the basis of redirect token type code.
        /// </summary>
        /// <param name="redirectTokenTypeCode"></param>
        /// <returns></returns>
        private static Int32 GetRedirectTokenTypeID(String redirectTokenTypeCode)
        {
            return SecurityManager.GetRedirectTokenTypeID(redirectTokenTypeCode);
        }

        /// <summary>
        /// Method to convert object into a byte array.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static byte[] ObjectToByteArray(Object obj)
        {
            // convert byte array to memory stream
            using (MemoryStream memStream = new MemoryStream())
            {
                // set memory stream position to starting point
                memStream.Position = 0;

                // create new BinaryFormatter
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                // Serializes an object graph into binary stream and copy to memory stream.
                binaryFormatter.Serialize(memStream, obj);

                return memStream.GetBuffer();
            }
        }

        /// <summary>
        /// Function to get object from byte array
        /// </summary>
        /// <param name="_ByteArray">byte array to get object</param>
        /// <returns>object</returns>
        private static Object ByteArrayToObject(byte[] byteArray)
        {
            // convert byte array to memory stream
            using (MemoryStream memStream = new MemoryStream(byteArray))
            {
                // create new BinaryFormatter
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                // set memory stream position to starting point
                memStream.Position = 0;

                // Deserializes a stream into an object graph and return as a object.
                return binaryFormatter.Deserialize(memStream);
            }
        }

        /// <summary>
        /// Method to serialize the key value pair and return serialized key value pair. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionaryData"></param>
        /// <returns></returns>
        //private static Dictionary<String, String> SerializeDictionaryValues<T>(Dictionary<String, T> dictionaryData)
        //{
        //    Dictionary<String, String> serializedValueDictionaryData = new Dictionary<String, String>();

        //    foreach (var data in dictionaryData)
        //    {
        //        var serializer = new XmlSerializer(typeof(T));
        //        var sb = new StringBuilder();

        //        using (TextWriter writer = new StringWriter(sb))
        //        {
        //            serializer.Serialize(writer, data.Value);
        //        }
        //        serializedValueDictionaryData.Add(data.Key, Convert.ToString(sb));
        //    }
        //    return serializedValueDictionaryData;
        //}

        private static String SerializeSessionData(SessionSharingContract sessionData)
        {
            var serializer = new XmlSerializer(typeof(SessionSharingContract));
            var sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, sessionData);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Method to deserialize the object and return deserialized key value pair.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedValueObject"></param>
        /// <returns></returns>
        //private static Dictionary<String, T> DeserializeDictionaryValues<T>(Object serializedValueObject)
        //{
        //    if (serializedValueObject.IsNotNull())
        //    {
        //        Dictionary<String, String> serializedValueDictionaryData = (Dictionary<String, String>)serializedValueObject;
        //        Dictionary<String, T> dictionaryData = new Dictionary<String, T>();

        //        foreach (var data in serializedValueDictionaryData)
        //        {
        //            var serializer = new XmlSerializer(typeof(T));
        //            TextReader reader = new StringReader(Convert.ToString(data.Value));
        //            dictionaryData[data.Key] = (T)serializer.Deserialize(reader);
        //        }
        //        return dictionaryData;
        //    }
        //    return null;
        //}
        private static SessionSharingContract DeserializeSessionData(Object serializedValueObject)
        {
            SessionSharingContract sessionData = new SessionSharingContract();
            if (!serializedValueObject.IsNull())
            {
                var serializer = new XmlSerializer(typeof(SessionSharingContract));
                TextReader reader = new StringReader(Convert.ToString(serializedValueObject));
                sessionData = (SessionSharingContract)serializer.Deserialize(reader);
            }
            return sessionData;
        }

        #endregion

        #endregion
    }
}
