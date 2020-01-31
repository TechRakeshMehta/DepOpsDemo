using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ExternalVendors.ClearStarVendor
{
    public static class ClearStarInvokeDetails
    {
        public static String GetCreateProfileErrorMessage(String responseXML)
        {
            try
            {
                //UAT-2254: Complio: Use CreateProfileForCountry API to create profile instead of CreateProfile which is being used in all three system to create profile.
                XmlDocument xml = null;
                XmlNode nodeCreateProfile = null;
                XmlNode nodeCreateProfileForCountry = null;
                if (!responseXML.IsNullOrEmpty())
                {
                    xml = new XmlDocument();
                    xml.LoadXml(responseXML);
                }
                if (!xml.IsNullOrEmpty())
                {
                    nodeCreateProfile = xml.SelectSingleNode("CreateProfile");
                    nodeCreateProfileForCountry = xml.SelectSingleNode("CreateProfileForCountry");
                }

                if (!nodeCreateProfile.IsNullOrEmpty())
                {
                    XmlSerializer createResultSerializer = new XmlSerializer(typeof(CreateProfile));
                    CreateProfile result;
                    using (TextReader reader = new StringReader(responseXML))
                    {
                        result = (CreateProfile)createResultSerializer.Deserialize(reader);
                    }

                    if (((CreateProfileErrorStatus)result.Items[AppConsts.NONE]).Code != AppConsts.ZERO)
                    {
                        CreateProfileErrorStatus createProfileErrorStatus = (CreateProfileErrorStatus)result.Items[AppConsts.NONE];
                        return createProfileErrorStatus.Message;
                    }
                }
                else if (!nodeCreateProfileForCountry.IsNullOrEmpty())
                {
                    XmlSerializer createForCountryResultSerializer = new XmlSerializer(typeof(CreateProfileForCountry));
                    CreateProfileForCountry result;
                    using (TextReader reader = new StringReader(responseXML))
                    {
                        result = (CreateProfileForCountry)createForCountryResultSerializer.Deserialize(reader);
                    }

                    if (((CreateProfileForCountryErrorStatus)result.Items[AppConsts.NONE]).Code != AppConsts.ZERO)
                    {
                        CreateProfileForCountryErrorStatus createProfileForCountryErrorStatus = (CreateProfileForCountryErrorStatus)result.Items[AppConsts.NONE];
                        return createProfileForCountryErrorStatus.Message;
                    }
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        public static String GetAddOrderToProfileErrorMessage(String responseXML)
        {
            try
            {
                XmlSerializer addOrderResultSerializer = new XmlSerializer(typeof(AddOrderToProfile));
                AddOrderToProfile result;
                using (TextReader reader = new StringReader(responseXML))
                {
                    result = (AddOrderToProfile)addOrderResultSerializer.Deserialize(reader);
                }

                if (result != null && result.Items.Count() > 0 && ((AddOrderToProfileErrorStatus)result.Items[0]).Code != AppConsts.ZERO)
                {
                    AddOrderToProfileErrorStatus addOrderToProfileErrorStatus = (AddOrderToProfileErrorStatus)result.Items[AppConsts.NONE];
                    return addOrderToProfileErrorStatus.Message;
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }


    }
}
