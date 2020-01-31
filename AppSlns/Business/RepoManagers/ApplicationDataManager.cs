using Entity;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Linq;

namespace Business.RepoManagers
{
    public static class ApplicationDataManager
    {
        #region Variables

        #region public Variables



        #endregion

        #region Private Variables



        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static ApplicationDataManager()
        {
            BALUtils.ClassModule = "ApplicationDataManager";
        }

        #endregion

        #region Properties

        #region public Properties



        #endregion

        #region Private Properties



        #endregion

        #endregion

        #region Methods

        #region Public Methods

        public static Object GetObjectDataByKey(String key)
        {
            try
            {
                return BALUtils.GetApplicationDataRepoInstance().GetObjectDataByKey(key);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void AddWebApplicationData(String key, Object data, Int32 validtimespan)
        {
            try
            {
                BALUtils.GetApplicationDataRepoInstance().AddWebApplicationData(key, data, validtimespan);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void RemoveWebApplicationData(String key)
        {
            try
            {
                BALUtils.GetApplicationDataRepoInstance().RemoveWebApplicationData(key);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static void UpdateWebApplicationData(String key, Object data)
        {
            try
            {
                BALUtils.GetApplicationDataRepoInstance().UpdateWebApplicationData(key, data);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Dictionary<String, String> SerializeDictionaryValues<T>(Dictionary<String, T> dictionaryData)
        {
            Dictionary<String, String> serializedValueDictionaryData = new Dictionary<String, String>();

            foreach (var data in dictionaryData)
            {
                var serializer = new XmlSerializer(typeof(T));
                var sb = new StringBuilder();

                using (TextWriter writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, data.Value);
                }
                serializedValueDictionaryData.Add(data.Key, Convert.ToString(sb));
            }
            return serializedValueDictionaryData;
        }

        public static Dictionary<String, T> DeserializeDictionaryValues<T>(Object serializedValueObject)
        {
            if (serializedValueObject.IsNotNull())
            {
                Dictionary<String, String> serializedValueDictionaryData = (Dictionary<String, String>)serializedValueObject;
                Dictionary<String, T> dictionaryData = new Dictionary<String, T>();

                foreach (var data in serializedValueDictionaryData)
                {
                    var serializer = new XmlSerializer(typeof(T));
                    TextReader reader = new StringReader(Convert.ToString(data.Value));
                    dictionaryData[data.Key] = (T)serializer.Deserialize(reader);
                }
                return dictionaryData;
            }
            return null;
        }

        /// <summary>
        /// Method to format SSN as ###-##-####
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public static String GetFormattedSSN(String unformattedSSN)
        {
            try
            {
                if (unformattedSSN == null)
                {
                    return String.Empty;
                }
                string value = unformattedSSN;
                Regex re = new Regex(@"(\d\d\d)(\d\d)(\d\d\d\d)");
                if (re.IsMatch(unformattedSSN))
                {
                    Match match = re.Match(unformattedSSN);
                    value = match.Groups[1] + "-" + match.Groups[2] + "-" + match.Groups[3];
                }
                return value;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static string GetFormattedPhoneNumber(string unformattedPhoneNumber)
        {
            try
            {
                if (unformattedPhoneNumber == null)
                {
                    return String.Empty;
                }
                string value = Regex.Replace(unformattedPhoneNumber, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3");
                return value;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to Masked SSN as ###-##-0000
        /// </summary>
        /// <param name="unMaskedSSN"></param>
        /// <returns></returns>
        public static String GetMaskedSSN(String unMaskedSSN)
        {
            try
            {
                if (unMaskedSSN == null)
                {
                    return String.Empty;
                }
                string value = unMaskedSSN;
                Regex re = new Regex(@"(\d\d\d)(\d\d)(\d\d\d\d)");
                if (re.IsMatch(unMaskedSSN))
                {
                    Match match = re.Match(unMaskedSSN);
                    value = "###-##-" + match.Groups[3];
                }
                return value;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to return SSN as "__3_5____"
        /// </summary>
        /// <param name="inputSSN"></param>
        /// <returns></returns>
        public static String GetSSNForFilters(String inputSSN)
        {
            try
            {
                if (!inputSSN.IsNullOrEmpty() && inputSSN != AppConsts.EMPTY_SSN && inputSSN != AppConsts.EMPTY_SSN_MASKED)
                {
                    if (inputSSN.Length == AppConsts.FOUR)
                    {
                        return "_____" + inputSSN;
                    }
                    else
                    {
                        return inputSSN;
                    }
                }
                return null;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to Masked SSN as MM-DD-####
        /// </summary>
        /// <param name="unMaskedDOB"></param>
        /// <returns></returns>
        public static string GetMaskDOB(string unMaskedDOB)
        {
            try
            {
                if (unMaskedDOB.IsNullOrEmpty())
                {
                    return String.Empty;
                }
                String[] arr = unMaskedDOB.Split('/');
                return arr[0] + "/" + arr[1] + "/####";
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Assign default roles to the agency shared user
        /// </summary>
        /// <param name="organizationUser"></param>
        public static void AssignDefaultRolesToAgencyUser(OrganizationUser organizationUser)
        {
            List<Int32> productId = SecurityManager.GetProductsForTenant(organizationUser.Organization.TenantID.Value).Select(obj => obj.TenantProductID).ToList();
            List<String> defaultRoledetailIds = SecurityManager.GetDefaultRoleDetailIdsForSharedUser(productId, organizationUser.UserID.ToString());
            SecurityManager.SaveMappingOfRolesForSharedUser(organizationUser, defaultRoledetailIds);
            SecurityManager.SetSharedUserDefaultBusinessChannel(organizationUser.OrganizationUserID);
        }

        #endregion

        #region Private Methods



        #endregion

        #endregion



    }
}
