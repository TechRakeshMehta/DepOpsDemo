using ExternalVendors.ClearstarGatewayDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Business.RepoManagers;
using System.IO;
using ExternalVendors.UpdateWebCCFClearStarOrder;
using System.Configuration;
using INTSOF.Utils;

namespace ExternalVendors.ClearStarVendor
{
    public class ClearStar
    {
        public Boolean IsUseClearstarTls_1_2 = !String.IsNullOrEmpty(ConfigurationManager.AppSettings["UseClearstarTls_1_2"])
                                                            ? Convert.ToBoolean(ConfigurationManager.AppSettings["UseClearstarTls_1_2"]) : false;
        public ClearStar()
        {
            if (IsUseClearstarTls_1_2)
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            }
        }
        public CreateProfile CreateClearstarProfileViaGateway(string loginName, string password, int boid,
                    string folderID, string customerID, int priorityID, string position, string accountingCode,
                    string ssn, string lastName, string firstName, string middleName, string suffix, string raceID,
                    string sex, string birthDate, string height, string weight, string scars, string eyes,
                    string addrLine1, string addrLine2, string city, string state,
                    string zipCode, string county, string fromDate, string subjectType, bool isHighlighted,
                    string comments, Int32 tenantID, Int32 bkgOrderID, Int32 bkgPackageSvcGroupID, String loggerInstance)
        {
            CreateProfile createResult = null;
            try
            {
                ClearstarGatewayProfile.Profile cgp = new ClearstarGatewayProfile.Profile();
                string profileID = string.Empty;
                cgp.EnableDecompression = true;
                DateTime methodEndTime = DateTime.Now;
                XmlNode xmlNodeCreateResult = null;
                XmlNodeReader xmlNodeReaderCreateResult = null;
                // Construct an instance of the XmlSerializer with the type
                // of object that is being deserialized.
                XmlSerializer createResultSerializer = new XmlSerializer(typeof(CreateProfile));

                List<ParamObject> parameters = new List<ParamObject>()
            {
                new ParamObject{ParameterName="loginName",ParameterValue=loginName},
                new ParamObject{ParameterName="password",ParameterValue=password},
                new ParamObject{ParameterName="boid",ParameterValue=boid},
                new ParamObject{ParameterName="bkgPackageSvcGroupID",ParameterValue=bkgPackageSvcGroupID},
                new ParamObject{ParameterName="folderID",ParameterValue=folderID},
                new ParamObject{ParameterName="priorityID",ParameterValue=priorityID},
                new ParamObject{ParameterName="position",ParameterValue=position},
                new ParamObject{ParameterName="accountingCode",ParameterValue=accountingCode},
                new ParamObject{ParameterName="ssn",ParameterValue=ssn},
                new ParamObject{ParameterName="lastName",ParameterValue=lastName},
                new ParamObject{ParameterName="firstName",ParameterValue=firstName},
                new ParamObject{ParameterName="middleName",ParameterValue=middleName},
                new ParamObject{ParameterName="suffix",ParameterValue=suffix},
                new ParamObject{ParameterName="raceID",ParameterValue=raceID},
                new ParamObject{ParameterName="sex",ParameterValue=sex},
                new ParamObject{ParameterName="birthDate",ParameterValue=birthDate},
                new ParamObject{ParameterName="height",ParameterValue=height},
                new ParamObject{ParameterName="weight",ParameterValue=weight},
                new ParamObject{ParameterName="scars",ParameterValue=scars},
                new ParamObject{ParameterName="eyes",ParameterValue=eyes},
                new ParamObject{ParameterName="addrLine1",ParameterValue=addrLine1},
                new ParamObject{ParameterName="addrLine2",ParameterValue=addrLine2},
                new ParamObject{ParameterName="city",ParameterValue=city},
                new ParamObject{ParameterName="state",ParameterValue=state},
                new ParamObject{ParameterName="zipCode",ParameterValue=zipCode},
                new ParamObject{ParameterName="county",ParameterValue=county},
                new ParamObject{ParameterName="fromDate",ParameterValue=fromDate},
                new ParamObject{ParameterName="subjectType",ParameterValue=subjectType},
                new ParamObject{ParameterName="isHighlighted",ParameterValue=isHighlighted},
                new ParamObject{ParameterName="comments",ParameterValue=comments},
            };

                DateTime methodStartTime = DateTime.Now;

                #region Call ClearStar Gateway

                xmlNodeCreateResult =
                    cgp.CreateProfile(
                        loginName,
                        password,
                        boid,
                        customerID,
                        folderID,
                        priorityID,
                        position,
                        accountingCode,
                        ssn,
                        lastName,
                        firstName,
                        middleName,
                        suffix,
                        raceID,
                        sex,
                        birthDate,
                        height,
                        weight,
                        scars,
                        eyes,
                        addrLine1,
                        addrLine2,
                        city,
                        state,
                        zipCode,
                        county,
                        fromDate,
                        subjectType,
                        isHighlighted,
                        comments);

                methodEndTime = DateTime.Now;

                xmlNodeReaderCreateResult = new XmlNodeReader(xmlNodeCreateResult);
                createResult = (CreateProfile)createResultSerializer.Deserialize(xmlNodeReaderCreateResult);

                #endregion

                SaveExtSvcInvokeHistory(tenantID, bkgOrderID, null, bkgPackageSvcGroupID, "CreateProfile", parameters, xmlNodeCreateResult.OuterXml, String.Empty, methodStartTime, methodEndTime);
            }
            catch (Exception ex)
            {
                LogExceptionMessage("CreateProfile", ex, loggerInstance);
                throw ex;
            }
            return createResult;

        }

        #region UAT-2254:Complio: Use CreateProfileForCountry API to create profile instead of CreateProfile which is being used in all three system to create profile.
        public CreateProfileForCountry CreateClearstarProfileForCountryViaGateway(string loginName, string password, int boid,
                    string folderID, string customerID, int priorityID, string position, string accountingCode,
                    string ssn, string lastName, string firstName, string middleName, string suffix, string raceID,
                    string sex, string birthDate, string height, string weight, string scars, string eyes,
                    string addrLine1, string addrLine2, string city, string state,
                    string zipCode, string county, string fromDate, string subjectType, bool isHighlighted,
                    string comments, Int32 tenantID, Int32 bkgOrderID, Int32 bkgPackageSvcGroupID, Int32 countryID, string emailAddress, string phoneNumber, String loggerInstance)
        {
            CreateProfileForCountry createResult = null;
            try
            {
                ClearstarGatewayProfile.Profile cgp = new ClearstarGatewayProfile.Profile();
                string profileID = string.Empty;
                cgp.EnableDecompression = true;
                DateTime methodEndTime = DateTime.Now;
                XmlNode xmlNodeCreateResult = null;
                XmlNodeReader xmlNodeReaderCreateResult = null;
                // Construct an instance of the XmlSerializer with the type
                // of object that is being deserialized.
                XmlSerializer createResultSerializer = new XmlSerializer(typeof(CreateProfileForCountry));

                List<ParamObject> parameters = new List<ParamObject>()
            {
                new ParamObject{ParameterName="loginName",ParameterValue=loginName},
                new ParamObject{ParameterName="password",ParameterValue=password},
                new ParamObject{ParameterName="boid",ParameterValue=boid},
                new ParamObject{ParameterName="bkgPackageSvcGroupID",ParameterValue=bkgPackageSvcGroupID},
                new ParamObject{ParameterName="folderID",ParameterValue=folderID},
                new ParamObject{ParameterName="priorityID",ParameterValue=priorityID},
                new ParamObject{ParameterName="position",ParameterValue=position},
                new ParamObject{ParameterName="accountingCode",ParameterValue=accountingCode},
                new ParamObject{ParameterName="ssn",ParameterValue=ssn},
                new ParamObject{ParameterName="lastName",ParameterValue=lastName},
                new ParamObject{ParameterName="firstName",ParameterValue=firstName},
                new ParamObject{ParameterName="middleName",ParameterValue=middleName},
                new ParamObject{ParameterName="suffix",ParameterValue=suffix},
                new ParamObject{ParameterName="raceID",ParameterValue=raceID},
                new ParamObject{ParameterName="sex",ParameterValue=sex},
                new ParamObject{ParameterName="birthDate",ParameterValue=birthDate},
                new ParamObject{ParameterName="height",ParameterValue=height},
                new ParamObject{ParameterName="weight",ParameterValue=weight},
                new ParamObject{ParameterName="scars",ParameterValue=scars},
                new ParamObject{ParameterName="eyes",ParameterValue=eyes},
                new ParamObject{ParameterName="countryID",ParameterValue=countryID},
                new ParamObject{ParameterName="addrLine1",ParameterValue=addrLine1},
                new ParamObject{ParameterName="addrLine2",ParameterValue=addrLine2},
                new ParamObject{ParameterName="city",ParameterValue=city},
                new ParamObject{ParameterName="state",ParameterValue=state},
                new ParamObject{ParameterName="zipCode",ParameterValue=zipCode},
                new ParamObject{ParameterName="county",ParameterValue=county},
                new ParamObject{ParameterName="fromDate",ParameterValue=fromDate},
                new ParamObject{ParameterName="subjectType",ParameterValue=subjectType},
                new ParamObject{ParameterName="isHighlighted",ParameterValue=isHighlighted},
                new ParamObject{ParameterName="comments",ParameterValue=comments},
                new ParamObject{ParameterName="emailAddress",ParameterValue=emailAddress},
                new ParamObject{ParameterName="phoneNumber",ParameterValue=phoneNumber},
            };

                DateTime methodStartTime = DateTime.Now;

                #region Call ClearStar Gateway

                xmlNodeCreateResult =
                    cgp.CreateProfileForCountry(
                        loginName,
                        password,
                        boid,
                        customerID,
                        folderID,
                        priorityID,
                        position,
                        accountingCode,
                        ssn,
                        lastName,
                        firstName,
                        middleName,
                        suffix,
                        raceID,
                        sex,
                        birthDate,
                        height,
                        weight,
                        scars,
                        eyes,
                        countryID,
                        addrLine1,
                        addrLine2,
                        city,
                        state,
                        zipCode,
                        county,
                        fromDate,
                        subjectType,
                        isHighlighted,
                        comments,
                        emailAddress,
                        phoneNumber
                        );

                methodEndTime = DateTime.Now;

                xmlNodeReaderCreateResult = new XmlNodeReader(xmlNodeCreateResult);
                createResult = (CreateProfileForCountry)createResultSerializer.Deserialize(xmlNodeReaderCreateResult);

                #endregion

                SaveExtSvcInvokeHistory(tenantID, bkgOrderID, null, bkgPackageSvcGroupID, "CreateProfile", parameters, xmlNodeCreateResult.OuterXml, String.Empty, methodStartTime, methodEndTime);
            }
            catch (Exception ex)
            {
                LogExceptionMessage("CreateProfile", ex, loggerInstance);
                throw ex;
            }
            return createResult;

        }

        #endregion

        public AddServiceToProfile AddServiceToClearstarProfileViaGateway(string loginName, string password, int boid,
    String customerID, String profileNumber, String serviceNumber, Int32 tenantID, Int32 bkgOrderID, String loggerInstance)
        {
            AddServiceToProfile addServiceResult = null;

            try
            {


                ClearstarGatewayProfile.Profile cgp = new ClearstarGatewayProfile.Profile();
                string profileID = string.Empty;
                cgp.EnableDecompression = true;

                XmlNode xmlNodeCreateResult = null;
                XmlNodeReader xmlNodeReaderCreateResult = null;
                // Construct an instance of the XmlSerializer with the type
                // of object that is being deserialized.
                XmlSerializer addServiceResultSerializer = new XmlSerializer(typeof(AddServiceToProfile));

                DateTime methodStartTime = DateTime.Now;

                #region Call ClearStar Gateway
                xmlNodeCreateResult =
                    cgp.AddServiceToProfile(
                        loginName,
                        password,
                        boid,
                        customerID,
                        profileNumber,
                        serviceNumber);

                DateTime methodEndTime = DateTime.Now;

                xmlNodeReaderCreateResult = new XmlNodeReader(xmlNodeCreateResult);
                addServiceResult = (AddServiceToProfile)addServiceResultSerializer.Deserialize(xmlNodeReaderCreateResult);
                #endregion

                List<ParamObject> parameters = new List<ParamObject>()
            {
                new ParamObject{ParameterName="loginName",ParameterValue=loginName},
                new ParamObject{ParameterName="password",ParameterValue=password},
                new ParamObject{ParameterName="boid",ParameterValue=boid},
                new ParamObject{ParameterName="customerID",ParameterValue=customerID},
                new ParamObject{ParameterName="profileNumber",ParameterValue=profileNumber},
                new ParamObject{ParameterName="serviceNumber",ParameterValue=serviceNumber}
            };

                SaveExtSvcInvokeHistory(tenantID, bkgOrderID, null, null, "AddServiceToProfile", parameters, xmlNodeCreateResult.OuterXml, serviceNumber,
                methodStartTime, methodEndTime);
            }
            catch (Exception ex)
            {
                LogExceptionMessage("AddServiceToProfile", ex, loggerInstance);
                throw ex;
            }
            return addServiceResult;
        }

        public AddOrderToProfile AddOrderToClearstarProfileViaGateway(String loginName, String password, Int32 boid,
            String customerID, String profileNumber, String serviceNumber, String country, String state, String county,
            String city, String zip, String instruct, String stage, Int32 aliasID, String orderFields,
            Int32 tenantID, Int32 bkgOrderID, Int32 bkgOrderPackageSvcLineItemID, Int32 bkgOrderPackageSvcGroupId, String loggerInstance)
        {
            AddOrderToProfile addOrderResult = null;

            try
            {

                ClearstarGatewayProfile.Profile cgp = new ClearstarGatewayProfile.Profile();
                cgp.EnableDecompression = true;
                String profileID = String.Empty;

                XmlNode xmlNodeAddOrderResult = null;
                XmlNodeReader xmlNodeReaderAddOrderResult = null;
                // Construct an instance of the XmlSerializer with the type
                // of object that is being deserialized.
                XmlSerializer addOrderResultSerializer = new XmlSerializer(typeof(AddOrderToProfile));

                DateTime methodStartTime = DateTime.Now;

                #region Call ClearStar Gateway
                xmlNodeAddOrderResult =
                    cgp.AddOrderToProfile(
                        loginName,
                        password,
                        boid,
                        customerID,
                        profileNumber,
                        serviceNumber,
                        country,
                        state,
                        county,
                        city,
                        zip,
                        instruct,
                        stage,
                        aliasID,
                        orderFields);
                DateTime methodEndTime = DateTime.Now;
                xmlNodeReaderAddOrderResult = new XmlNodeReader(xmlNodeAddOrderResult);
                addOrderResult = (AddOrderToProfile)addOrderResultSerializer.Deserialize(xmlNodeReaderAddOrderResult);


                #endregion

                List<ParamObject> parameters = new List<ParamObject>()
            {           
                new ParamObject{ParameterName="loginName",ParameterValue=loginName},
                new ParamObject{ParameterName="password",ParameterValue=password},
                new ParamObject{ParameterName="boid",ParameterValue=boid},
                new ParamObject{ParameterName="bkgOrderPackageSvcLineItemID",ParameterValue=bkgOrderPackageSvcLineItemID},
                new ParamObject{ParameterName="bkgOrderPackageSvcGroupId",ParameterValue=bkgOrderPackageSvcGroupId},
                new ParamObject{ParameterName="customerID",ParameterValue=customerID},
                new ParamObject{ParameterName="profileNumber",ParameterValue=profileNumber},
                new ParamObject{ParameterName="serviceNumber",ParameterValue=serviceNumber},
                new ParamObject{ParameterName="country",ParameterValue=country},
                new ParamObject{ParameterName="state",ParameterValue=state},
                new ParamObject{ParameterName="county",ParameterValue=county},
                new ParamObject{ParameterName="city",ParameterValue=city},
                new ParamObject{ParameterName="zip",ParameterValue=zip},
                new ParamObject{ParameterName="instruct",ParameterValue=instruct},
                new ParamObject{ParameterName="stage",ParameterValue=stage},
                new ParamObject{ParameterName="aliasID",ParameterValue=aliasID},
                new ParamObject{ParameterName="stage",ParameterValue=stage},
                new ParamObject{ParameterName="orderFields",ParameterValue=orderFields}
            };
                String comment = String.Empty;

                if (((AddOrderToProfileErrorStatus)addOrderResult.Items[0]).Message ==
                                                          "One or more of the Orders is a duplicate of an existing Order.")
                {
                    comment = "Error occured adding order for Service Number: " + serviceNumber +
                                           " for AMS OrderID: " + bkgOrderID + " and Profile Number: " +
                                           profileNumber + ".  The Order Data was: Country = '" +
                                           country + "' City = '" + city + "' State = '" + state + "' Zip = '" +
                                           zip + "' County = '" + county +
                                           "'.  The error message is: " + ((AddOrderToProfileErrorStatus)addOrderResult.Items[0]).Message + "<br /><br /> ";
                }
                else if (((AddOrderToProfileErrorStatus)addOrderResult.Items[0]).Message.ToUpper().Contains("LOCATION"))
                {
                    comment = "Error occured adding order for Service Number: " + serviceNumber +
                                           " for AMS OrderID: " + bkgOrderID + " and Profile Number: " +
                                           profileNumber + ".  The Order Data was: Country = '" +
                                           country + "' City = '" + city + "' State = '" + state + "' Zip = '" +
                                           zip + "' County = '" + county +
                                           "'.  The error message is: " + ((AddOrderToProfileErrorStatus)addOrderResult.Items[0]).Message + "<br /><br /> ";
                }

                SaveExtSvcInvokeHistory(tenantID, bkgOrderID, bkgOrderPackageSvcLineItemID, bkgOrderPackageSvcGroupId,
                                        "AddOrderToProfile", parameters, xmlNodeAddOrderResult.OuterXml, serviceNumber, methodStartTime, methodEndTime, comment);
            }
            catch (Exception ex)
            {
                LogExceptionMessage("AddOrderToProfile", ex, loggerInstance);
                throw ex;
            }
            return addOrderResult;
        }

        public TransmitProfile TransmitClearstarProfileViaGateway(string loginName, string password, int boid,
            string customerID, string profileNumber, Int32 tenantID, Int32 bkgOrderID, Int32 bkgOrderPackageSvcGroupId, String loggerInstance)
        {
            TransmitProfile transmitProfileResult = null;
            try
            {

                ClearstarGatewayProfile.Profile cgp = new ClearstarGatewayProfile.Profile();
                string profileID = string.Empty;
                cgp.EnableDecompression = true;

                XmlNode xmlNodeTransmitResult = null;
                XmlNodeReader xmlNodeReaderTransmitResult = null;
                // Construct an instance of the XmlSerializer with the type
                // of object that is being deserialized.
                XmlSerializer transmitProfileResultSerializer = new XmlSerializer(typeof(TransmitProfile));

                DateTime methodStartTime = DateTime.Now;

                #region Call Clearstar Gateway
                xmlNodeTransmitResult =
                    cgp.TransmitProfile(
                        loginName,
                        password,
                        boid,
                        customerID,
                        profileNumber);
                DateTime methodEndTime = DateTime.Now;
                xmlNodeReaderTransmitResult = new XmlNodeReader(xmlNodeTransmitResult);
                transmitProfileResult = (TransmitProfile)transmitProfileResultSerializer.Deserialize(xmlNodeReaderTransmitResult);
                #endregion

                #region Save ExtSvcInvokeHistory
                List<ParamObject> parameters = new List<ParamObject>()
            {        
                new ParamObject{ParameterName="loginName",ParameterValue=loginName},
                new ParamObject{ParameterName="password",ParameterValue=password},
                new ParamObject{ParameterName="boid",ParameterValue=boid},
                new ParamObject{ParameterName="bkgOrderPackageSvcGroupId",ParameterValue=bkgOrderPackageSvcGroupId},
                new ParamObject{ParameterName="customerID",ParameterValue=customerID},
                new ParamObject{ParameterName="profileNumber",ParameterValue=profileNumber}                
            };

                SaveExtSvcInvokeHistory(tenantID, bkgOrderID, null, bkgOrderPackageSvcGroupId, "TransmitProfile", parameters, xmlNodeTransmitResult.OuterXml, String.Empty, methodStartTime, methodEndTime);
                #endregion
            }
            catch (Exception ex)
            {
                LogExceptionMessage("TransmitProfile", ex, loggerInstance);
                throw ex;
            }
            return transmitProfileResult;
        }

        public GetListOfProfiles GetListofActiveProfilesViaClearstarGateway(string loginName, string password, int boid, string custID, Int32 tenantID, String loggerInstance)
        {
            GetListOfProfiles getListOfProfilesResult = null;
            try
            {
                ClearstarGatewayProfile.Profile cgp = new ClearstarGatewayProfile.Profile();
                string profileID = string.Empty;
                cgp.EnableDecompression = true;
                XmlNode xmlNodeGLOACPResult = null;
                XmlNodeReader xmlNodeReaderGLOACResult = null;
                // Construct an instance of the XmlSerializer with the type
                // of object that is being deserialized.
                XmlSerializer transmitGLOACPSerializer = new XmlSerializer(typeof(GetListOfProfiles));

                DateTime methodStartTime = DateTime.Now;

                #region Call ClearStar Gateway
                xmlNodeGLOACPResult =
                    cgp.GetListOfProfiles(
                        loginName,
                        password,
                        boid,
                        custID,
                        "",
                        false,
                        "profInProgress",
                        "profAllProfiles",
                        false,
                        false,
                        0,
                        200000);
                DateTime methodEndTime = DateTime.Now;
                xmlNodeReaderGLOACResult = new XmlNodeReader(xmlNodeGLOACPResult);
                getListOfProfilesResult = (GetListOfProfiles)transmitGLOACPSerializer.Deserialize(xmlNodeReaderGLOACResult);
                #endregion

                #region Save ExtSvcInvokeHistory
                List<ParamObject> parameters = new List<ParamObject>()
            {
                new ParamObject{ParameterName="loginName",ParameterValue=loginName},
                new ParamObject{ParameterName="password",ParameterValue=password},
                new ParamObject{ParameterName="boid",ParameterValue=boid},
                new ParamObject{ParameterName="custID",ParameterValue=custID}                               
            };

                SaveExtSvcInvokeHistory(tenantID, null, null, null, "GetListOfProfiles", parameters, xmlNodeGLOACPResult.OuterXml, String.Empty, methodStartTime, methodEndTime);
                #endregion
            }
            catch (Exception ex)
            {
                LogExceptionMessage("GetListOfProfiles", ex, loggerInstance);
                throw ex;
            }
            return getListOfProfilesResult;
        }

        public List<GetListOfProfilesProfile> GetListofAllProfilesViaClearstarGateway(string loginName,
            string password, int boid, string custID, String loggerInstance)
        {
            List<GetListOfProfilesProfile> listGLOPP = new List<GetListOfProfilesProfile>();
            try
            {
                ClearstarGatewayProfile.Profile cgp = new ClearstarGatewayProfile.Profile();
                string profileID = string.Empty;
                cgp.EnableDecompression = true;
                GetListOfProfiles getListOfProfilesResult = null;
                XmlNode xmlNodeGLOACPResult = null;
                XmlNodeReader xmlNodeReaderGLOACResult = null;
                // Construct an instance of the XmlSerializer with the type
                // of object that is being deserialized.
                XmlSerializer transmitGLOACPSerializer = new XmlSerializer(typeof(GetListOfProfiles));
                int currentRow = 100;
                int totalNumberofRows = 0;


                #region Call ClearStar Gateway
                xmlNodeGLOACPResult =
                    cgp.GetListOfProfiles(
                        loginName,
                        password,
                        boid,
                        custID,
                        "",
                        false,
                        "profNone",
                        "profAllProfiles",
                        false,
                        false,
                        1,
                        100);
                xmlNodeReaderGLOACResult = new XmlNodeReader(xmlNodeGLOACPResult);
                getListOfProfilesResult = (GetListOfProfiles)transmitGLOACPSerializer.Deserialize(xmlNodeReaderGLOACResult);
                #endregion

                if (getListOfProfilesResult != null)
                {
                    totalNumberofRows = Convert.ToInt32(getListOfProfilesResult.TotalResults);
                    listGLOPP.AddRange(getListOfProfilesResult.Profile);
                    if (Convert.ToInt32(getListOfProfilesResult.TotalResults) > 100)
                    {
                        while (currentRow < totalNumberofRows)
                        {
                            xmlNodeGLOACPResult =
                            cgp.GetListOfProfiles(
                                loginName,
                                password,
                                boid,
                                custID,
                                "",
                                false,
                                "profNone",
                                "profAllProfiles",
                                false,
                                false,
                                currentRow + 1,
                                currentRow + 100);
                            xmlNodeReaderGLOACResult = new XmlNodeReader(xmlNodeGLOACPResult);
                            getListOfProfilesResult = (GetListOfProfiles)transmitGLOACPSerializer.Deserialize(xmlNodeReaderGLOACResult);
                            currentRow += 100;
                            listGLOPP.AddRange(getListOfProfilesResult.Profile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogExceptionMessage("GetListOfProfiles", ex, loggerInstance);
                throw ex;
            }
            return listGLOPP;
        }

        public UnlockProfile UnlockClearstarProfileViaGateway(string loginName, string password, int boid,
            String customerID, String profileNumber, Int32 tenantID, Int32 bkgOrderID, Int32 bkgOrderPackageSvcGroupId, String loggerInstance)
        {
            UnlockProfile unlockProfileResult = null;
            try
            {
                ClearstarGatewayProfile.Profile cgp = new ClearstarGatewayProfile.Profile();
                string profileID = string.Empty;
                cgp.EnableDecompression = true;
                XmlNode xmlNodeUnlockResult = null;
                XmlNodeReader xmlNodeReaderUnlockResult = null;
                // Construct an instance of the XmlSerializer with the type
                // of object that is being deserialized.
                XmlSerializer transmitProfileResultSerializer = new XmlSerializer(typeof(UnlockProfile));

                DateTime methodStartTime = DateTime.Now;

                xmlNodeUnlockResult =
                    cgp.UnlockProfile(
                        loginName,
                        password,
                        boid,
                        customerID,
                        profileNumber);

                DateTime methodEndTime = DateTime.Now;
                xmlNodeReaderUnlockResult = new XmlNodeReader(xmlNodeUnlockResult);
                unlockProfileResult = (UnlockProfile)transmitProfileResultSerializer.Deserialize(xmlNodeReaderUnlockResult);

                #region Save ExtSvcInvokeHistory
                List<ParamObject> parameters = new List<ParamObject>()
            {
                new ParamObject{ParameterName="loginName",ParameterValue=loginName},
                new ParamObject{ParameterName="password",ParameterValue=password},
                new ParamObject{ParameterName="boid",ParameterValue=boid},
                new ParamObject{ParameterName="bkgOrderPackageSvcGroupId",ParameterValue=bkgOrderPackageSvcGroupId},
                new ParamObject{ParameterName="customerID",ParameterValue=customerID},
                new ParamObject{ParameterName="profileNumber",ParameterValue=profileNumber}                               
            };

                SaveExtSvcInvokeHistory(tenantID, bkgOrderID, null, bkgOrderPackageSvcGroupId, "UnlockProfile", parameters, xmlNodeUnlockResult.OuterXml, String.Empty, methodStartTime, methodEndTime);
                #endregion
            }
            catch (Exception ex)
            {
                LogExceptionMessage("UnlockProfile", ex, loggerInstance);
                throw ex;
            }
            return unlockProfileResult;
        }

        public UploadOrderDocument2 UploadProfileDocumentViaGateway(String loginName, String password, Int32 boid,
            String customerID, String profileNumber, Int32 orderID, String fileName, byte[] fileBytes, String loggerInstance, Int32 tenantID, Int32 bkgOrderID, Int32 bkgOrderPackageSvcGroupId)
        {
            UploadOrderDocument2 uploadOrderDocument2 = null;
            try
            {
                Document doc = new Document();
                string profileID = string.Empty;
                doc.EnableDecompression = true;
                XmlNode xmlNodeUploadProfileDocumentResult = null;
                XmlNodeReader xmlNodeUploadProfileDocumentResultReader = null;
                // Construct an instance of the XmlSerializer with the type
                // of object that is being deserialized.
                XmlSerializer uploadProfileDocumentResultSerializer = new XmlSerializer(typeof(UploadOrderDocument2));

                DateTime methodStartTime = DateTime.Now;

                xmlNodeUploadProfileDocumentResult =
                    doc.UploadOrderDocument2(
                        loginName,
                        password,
                        boid,
                        customerID,
                        0,
                        profileNumber,
                        orderID,
                        "Order",
                        "application/pdf",
                        1,
                        "WebCCF Registration PDF",
                        "Y",
                        fileName,
                        fileBytes
                        );
                DateTime methodEndTime = DateTime.Now;

                xmlNodeUploadProfileDocumentResultReader = new XmlNodeReader(xmlNodeUploadProfileDocumentResult);


                uploadOrderDocument2 = (UploadOrderDocument2)uploadProfileDocumentResultSerializer.Deserialize(xmlNodeUploadProfileDocumentResultReader);


                List<ParamObject> parameters = new List<ParamObject>()
            {
                new ParamObject{ParameterName="loginName",ParameterValue=loginName},
                new ParamObject{ParameterName="password",ParameterValue=password},
                new ParamObject{ParameterName="boid",ParameterValue=boid},
                new ParamObject{ParameterName="bkgOrderPackageSvcGroupId",ParameterValue=bkgOrderPackageSvcGroupId},
                new ParamObject{ParameterName="customerID",ParameterValue=customerID},
                new ParamObject{ParameterName="profileNumber",ParameterValue=profileNumber}                               
            };

                SaveExtSvcInvokeHistory(tenantID, bkgOrderID, null, bkgOrderPackageSvcGroupId, "UploadProfileDocumentViaGateway", parameters, xmlNodeUploadProfileDocumentResult.OuterXml, String.Empty, methodStartTime, methodEndTime);
            }
            catch (Exception ex)
            {
                LogExceptionMessage("UploadOrderDocument2", ex, loggerInstance);
                throw ex;
            }

            return uploadOrderDocument2;
        }

        public DeleteProfileDraft DeleteClearstarProfileDraftViaGateway(String loginName, String password, Int32 boid,
            String customerID, String profileNumber, Int32 tenantID, Int32 bkgOrderID, Int32 bkgOrderPackageSvcGroupId, String loggerInstance)
        {
            DeleteProfileDraft deleteProfileResult = null;
            try
            {
                ClearstarGatewayProfile.Profile cgp = new ClearstarGatewayProfile.Profile();
                string profileID = string.Empty;
                cgp.EnableDecompression = true;
                XmlNode xmlNodeTransmitResult = null;
                XmlNodeReader xmlNodeReaderTransmitResult = null;
                // Construct an instance of the XmlSerializer with the type
                // of object that is being deserialized.
                XmlSerializer transmitProfileResultSerializer = new XmlSerializer(typeof(DeleteProfileDraft));

                DateTime methodStartTime = DateTime.Now;
                xmlNodeTransmitResult =
                    cgp.DeleteProfileDraft(
                        loginName,
                        password,
                        boid,
                        customerID,
                        profileNumber);
                xmlNodeReaderTransmitResult = new XmlNodeReader(xmlNodeTransmitResult);
                deleteProfileResult = (DeleteProfileDraft)transmitProfileResultSerializer.Deserialize(xmlNodeReaderTransmitResult);

                DateTime methodEndTime = DateTime.Now;

                #region Save ExtSvcInvokeHistory
                List<ParamObject> parameters = new List<ParamObject>()
                {
                    new ParamObject{ParameterName="loginName",ParameterValue=loginName},
                    new ParamObject{ParameterName="password",ParameterValue=password},
                    new ParamObject{ParameterName="boid",ParameterValue=boid},
                    new ParamObject{ParameterName="bkgOrderPackageSvcGroupId",ParameterValue=bkgOrderPackageSvcGroupId},
                    new ParamObject{ParameterName="customerID",ParameterValue=customerID},
                    new ParamObject{ParameterName="profileNumber",ParameterValue=profileNumber}                               
                };

                SaveExtSvcInvokeHistory(tenantID, bkgOrderID, null, bkgOrderPackageSvcGroupId, "DeleteProfileDraft", parameters, xmlNodeTransmitResult.OuterXml, String.Empty, methodStartTime, methodEndTime);
                #endregion

            }
            catch (Exception ex)
            {
                LogExceptionMessage("DeleteProfileDraft", ex, loggerInstance);
                throw ex;
            }
            return deleteProfileResult;
        }

        public CancelProfile CancelClearstarProfileDraftViaGateway(String loginName, String password, Int32 boid,
            String customerID, String profileNumber, String authorizedBy, Boolean invoiceCustomer, Int32 tenantID, Int32 bkgOrderID, Int32 bkgOrderPackageSvcGroupId, String loggerInstance)
        {
            CancelProfile cancelProfileResult = null;
            try
            {
                ClearstarGatewayProfile.Profile cgp = new ClearstarGatewayProfile.Profile();
                cgp.EnableDecompression = true;
                XmlNode xmlNodeTransmitResult = null;
                XmlNodeReader xmlNodeReaderTransmitResult = null;
                // Construct an instance of the XmlSerializer with the type
                // of object that is being deserialized.
                XmlSerializer transmitProfileResultSerializer = new XmlSerializer(typeof(CancelProfile));

                DateTime methodStartTime = DateTime.Now;
                xmlNodeTransmitResult =
                    cgp.CancelProfile(
                        loginName,
                        password,
                        boid,
                        customerID,
                        profileNumber,
                        authorizedBy,
                        invoiceCustomer);
                xmlNodeReaderTransmitResult = new XmlNodeReader(xmlNodeTransmitResult);
                cancelProfileResult = (CancelProfile)transmitProfileResultSerializer.Deserialize(xmlNodeReaderTransmitResult);

                DateTime methodEndTime = DateTime.Now;

                #region Save ExtSvcInvokeHistory
                List<ParamObject> parameters = new List<ParamObject>()
                {
                    new ParamObject{ParameterName="loginName",ParameterValue=loginName},
                    new ParamObject{ParameterName="password",ParameterValue=password},
                    new ParamObject{ParameterName="boid",ParameterValue=boid},
                    new ParamObject{ParameterName="bkgOrderPackageSvcGroupId",ParameterValue=bkgOrderPackageSvcGroupId},
                    new ParamObject{ParameterName="customerID",ParameterValue=customerID},
                    new ParamObject{ParameterName="profileNumber",ParameterValue=profileNumber}                               
                };

                SaveExtSvcInvokeHistory(tenantID, bkgOrderID, null, bkgOrderPackageSvcGroupId, "CancelProfile", parameters, xmlNodeTransmitResult.OuterXml, String.Empty, methodStartTime, methodEndTime);
                #endregion
            }
            catch (Exception ex)
            {
                LogExceptionMessage("CancelProfile", ex, loggerInstance);
                throw ex;
            }

            return cancelProfileResult;
        }

        public EditProfileComments AddCommentToClearstarProfileDraftViaGateway(String loginName, String password, Int32 boid,
            String customerID, String profileNumber, Boolean isHighlighted, String folderID, String comments, Int32 tenantID, Int32 bkgOrderID, Int32 bkgOrderPackageSvcGroupId, String loggerInstance)
        {
            EditProfileComments editProfileCommentsResult = null;
            try
            {
                ClearstarGatewayProfile.Profile cgp = new ClearstarGatewayProfile.Profile();
                cgp.EnableDecompression = true;
                XmlNode xmlNodeEditProfileComments = null;
                XmlNodeReader xmlNodeReaderEditProfileComments = null;
                // Construct an instance of the XmlSerializer with the type
                // of object that is being deserialized.
                XmlSerializer editProfileCommentsResultSerializer = new XmlSerializer(typeof(EditProfileComments));
                DateTime methodStartTime = DateTime.Now;
                xmlNodeEditProfileComments =
                    cgp.EditProfileComments(
                        loginName,
                        password,
                        boid,
                        customerID,
                        profileNumber,
                        isHighlighted,
                        folderID,
                        comments);
                xmlNodeReaderEditProfileComments = new XmlNodeReader(xmlNodeEditProfileComments);
                editProfileCommentsResult = (EditProfileComments)editProfileCommentsResultSerializer.Deserialize(xmlNodeReaderEditProfileComments);

                DateTime methodEndTime = DateTime.Now;

                #region Save ExtSvcInvokeHistory
                List<ParamObject> parameters = new List<ParamObject>()
                {
                    new ParamObject{ParameterName="loginName",ParameterValue=loginName},
                    new ParamObject{ParameterName="password",ParameterValue=password},
                    new ParamObject{ParameterName="boid",ParameterValue=boid},
                    new ParamObject{ParameterName="bkgOrderPackageSvcGroupId",ParameterValue=bkgOrderPackageSvcGroupId},
                    new ParamObject{ParameterName="customerID",ParameterValue=customerID},
                    new ParamObject{ParameterName="profileNumber",ParameterValue=profileNumber}                               
                };

                SaveExtSvcInvokeHistory(tenantID, bkgOrderID, null, bkgOrderPackageSvcGroupId, "EditProfileComments", parameters, xmlNodeEditProfileComments.OuterXml,
                String.Empty, methodStartTime, methodEndTime);
                #endregion
            }
            catch (Exception ex)
            {
                LogExceptionMessage("EditProfileComments", ex, loggerInstance);
                throw ex;
            }

            return editProfileCommentsResult;
        }

        public GetProfileStatus GetProfileStatusByProfileID(string loginName, string password, long boid,
            string custID, string profileNumber, Int32 tenantID, Int32 bkgOrderID, Int32 bkgOrderPackageSvcGroupID, String loggerInstance)
        {
            GetProfileStatus ps = null;
            try
            {
                ClearstarGatewayProfile.Profile cgp = new ClearstarGatewayProfile.Profile();
                cgp.EnableDecompression = true;
                XmlNode xmlNodeProfileStatus = null;
                XmlNodeReader xmlNodeReaderProfileStatus = null;
                // Construct an instance of the XmlSerializer with the type
                // of object that is being deserialized.
                XmlSerializer profileStatusSerializer = new XmlSerializer(typeof(GetProfileStatus));

                DateTime methodStartTime = DateTime.Now;
                xmlNodeProfileStatus = cgp.GetProfileStatus(loginName,
                    password, Convert.ToInt32(boid), custID, profileNumber);

                DateTime methodEndTime = DateTime.Now;
                xmlNodeReaderProfileStatus = new XmlNodeReader(xmlNodeProfileStatus);
                ps = (GetProfileStatus)profileStatusSerializer.Deserialize(xmlNodeReaderProfileStatus);

                #region Save ExtSvcInvokeHistory
                List<ParamObject> parameters = new List<ParamObject>()
                {
                    new ParamObject{ParameterName="loginName",ParameterValue=loginName},
                    new ParamObject{ParameterName="password",ParameterValue=password},
                    new ParamObject{ParameterName="boid",ParameterValue=boid},
                    new ParamObject{ParameterName="bkgOrderPackageSvcGroupID",ParameterValue=bkgOrderPackageSvcGroupID},
                    new ParamObject{ParameterName="customerID",ParameterValue=custID},
                    new ParamObject{ParameterName="profileNumber",ParameterValue=profileNumber}                               
                };

                SaveExtSvcInvokeHistory(tenantID, bkgOrderID, null, bkgOrderPackageSvcGroupID, "GetProfileStatusByProfileID", parameters, xmlNodeProfileStatus.OuterXml, String.Empty, methodStartTime, methodEndTime);
                #endregion
            }
            catch (Exception ex)
            {
                LogExceptionMessage("GetProfileStatusByProfileID", ex, loggerInstance);
                throw ex;
            }
            return ps;
        }

        public AddAlias AddAliasNametoProfile(string loginName, string password,
            int boid, string custID, string profileNumber, string firstName, string middleName,
            string lastName, string nameSuffix, string aliasType, Int32 tenantID, Int32 bkgOrderID, Int32 bkgOrderPackageSvcLineItemID, Int32 bkgOrderPackageSvcGroupId)
        {
            ClearstarGatewayProfile.Profile cgp = new ClearstarGatewayProfile.Profile();
            cgp.EnableDecompression = true;
            AddAlias addAlias = null;
            XmlNode xmlNodeAddAlias = null;
            XmlNodeReader xmlNodeReaderAddAlias = null;
            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            XmlSerializer addAliasSerializer = new XmlSerializer(typeof(AddAlias));

            try
            {
                //Get Order from Clearstar Gateway
                DateTime methodStartTime = DateTime.Now;

                xmlNodeAddAlias = cgp.AddAlias(loginName, password, boid, custID, profileNumber, lastName, firstName, middleName, nameSuffix, aliasType);

                DateTime methodEndTime = DateTime.Now;

                xmlNodeReaderAddAlias = new XmlNodeReader(xmlNodeAddAlias);
                addAlias = (AddAlias)addAliasSerializer.Deserialize(xmlNodeReaderAddAlias);

                #region Save ExtSvcInvokeHistory
                List<ParamObject> parameters = new List<ParamObject>()
                {
                    new ParamObject{ParameterName="loginName",ParameterValue=loginName},
                    new ParamObject{ParameterName="password",ParameterValue=password},
                    new ParamObject{ParameterName="boid",ParameterValue=boid},
                    new ParamObject{ParameterName="bkgOrderPackageSvcLineItemID",ParameterValue=bkgOrderPackageSvcLineItemID},
                    new ParamObject{ParameterName="bkgOrderPackageSvcGroupId",ParameterValue=bkgOrderPackageSvcGroupId},
                    new ParamObject{ParameterName="customerID",ParameterValue=custID},
                    new ParamObject{ParameterName="profileNumber",ParameterValue=profileNumber},
                    new ParamObject{ParameterName="firstName",ParameterValue=firstName},
                    new ParamObject{ParameterName="middleName",ParameterValue=middleName},
                    new ParamObject{ParameterName="lastName",ParameterValue=lastName},
                    new ParamObject{ParameterName="nameSuffix",ParameterValue=nameSuffix},
                    new ParamObject{ParameterName="aliasType",ParameterValue=aliasType}                              
                };

                SaveExtSvcInvokeHistory(tenantID, bkgOrderID, bkgOrderPackageSvcLineItemID, bkgOrderPackageSvcGroupId, "AddAliasNametoProfile", parameters, xmlNodeAddAlias.OuterXml, String.Empty, methodStartTime, methodEndTime);

                #endregion
            }
            catch
            {
                throw;
            }
            return addAlias;
        }

        public GetProfileDetail GetProfileDetailForProfileByProfileID(string loginName, string password,
        int boID, string customerID, string profileNumber, Int32 tenantID, Int32 bkgOrderID, Int32 bkgOrderPackageSvcGroupID, String loggerInstance)
        {
            ClearstarGatewayProfile.Profile pro = new ClearstarGatewayProfile.Profile();
            pro.EnableDecompression = true;
            GetProfileDetail gpd = null;
            XmlNode xmlNodeCreateResult = null;
            XmlNodeReader xmlNodeReaderCreateResult = null;
            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            XmlSerializer getProfileDetailSerializer = new XmlSerializer(typeof(GetProfileDetail));

            try
            {
                DateTime methodStartTime = DateTime.Now;
                //Get Order from Clearstar Gateway
                xmlNodeCreateResult = pro.GetProfileDetail(loginName, password, boID, customerID, profileNumber);
                DateTime methodEndTime = DateTime.Now;

                xmlNodeReaderCreateResult = new XmlNodeReader(xmlNodeCreateResult);
                gpd = (GetProfileDetail)getProfileDetailSerializer.Deserialize(xmlNodeReaderCreateResult);

                #region Save ExtSvcInvokeHistory
                List<ParamObject> parameters = new List<ParamObject>()
                {
                    new ParamObject{ParameterName="loginName",ParameterValue=loginName},
                    new ParamObject{ParameterName="password",ParameterValue=password},
                    new ParamObject{ParameterName="boid",ParameterValue=boID},
                     new ParamObject{ParameterName="bkgOrderPackageSvcGroupID",ParameterValue=bkgOrderPackageSvcGroupID},
                    new ParamObject{ParameterName="customerID",ParameterValue=customerID},
                    new ParamObject{ParameterName="profileNumber",ParameterValue=profileNumber}                               
                };

                SaveExtSvcInvokeHistory(tenantID, bkgOrderID, null, bkgOrderPackageSvcGroupID, "GetProfileDetail", parameters, xmlNodeCreateResult.OuterXml, String.Empty, methodStartTime, methodEndTime);
                #endregion

            }
            catch (Exception ex)
            {
                LogExceptionMessage("GetProfileDetail", ex, loggerInstance);
                throw ex;
            }
            return gpd;
        }

        public UpdateClearStarOrderInfo UpdateClearstarWebCCFInfo(int boID, string systemAccount, string loginName, string password, string registrationID,
            CreateProfileForCountry cp, AddOrderToProfile aotp, Int32 tenantID, Int32 bkgOrderID, Int32 bkgOrderPackageSvcLineItemID, String loggerInstance)
        {
            UpdateClearStarOrderInfo updateCSOrderInfoResult = null;
            try
            {
                ClearStarOrderInfo csoi = new ClearStarOrderInfo();
                XmlNode xmlNodeUpdateClearStarOrderInfo = null;
                XmlNodeReader xmlNodeReaderUpdateClearStarOrderInfo = null;

                // Construct an instance of the XmlSerializer with the type
                // of object that is being deserialized.
                XmlSerializer editProfileCommentsResultSerializer = new XmlSerializer(typeof(UpdateClearStarOrderInfo));

                DateTime methodStartTime = DateTime.Now;
                xmlNodeUpdateClearStarOrderInfo = csoi.UpdateClearStarOrderInfo(boID, systemAccount, loginName, password, registrationID, ((CreateProfileForCountryProfile)cp.Items[1]).Prof_No,
                    ((AddOrderToProfileProfile)aotp.Items[1]).NewOrderID);
                DateTime methodEndTime = DateTime.Now;

                xmlNodeReaderUpdateClearStarOrderInfo = new XmlNodeReader(xmlNodeUpdateClearStarOrderInfo);


                #region Save ExtSvcInvokeHistory

                List<ParamObject> parameters = new List<ParamObject>()
                {
                    new ParamObject{ParameterName="loginName",ParameterValue=loginName},
                    new ParamObject{ParameterName="password",ParameterValue=password},
                    new ParamObject{ParameterName="boid",ParameterValue=boID},
                    new ParamObject{ParameterName="bkgOrderPackageSvcLineItemID",ParameterValue=bkgOrderPackageSvcLineItemID},
                    new ParamObject{ParameterName="systemAccount",ParameterValue=systemAccount},
                    new ParamObject{ParameterName="registrationID",ParameterValue=registrationID},
                    new ParamObject{ParameterName="ClearStarOrderID",ParameterValue=((AddOrderToProfileProfile)aotp.Items[1]).NewOrderID},
                    new ParamObject{ParameterName="customerID",ParameterValue=systemAccount},
                    new ParamObject{ParameterName="profileNumber",ParameterValue=((CreateProfileForCountryProfile)cp.Items[1]).Prof_No}                               
                };

                SaveExtSvcInvokeHistory(tenantID, bkgOrderID, null, bkgOrderPackageSvcLineItemID, "UpdateClearstarWebCCFInfo", parameters, xmlNodeUpdateClearStarOrderInfo.OuterXml, String.Empty, methodStartTime, methodEndTime);
                #endregion
                updateCSOrderInfoResult = (UpdateClearStarOrderInfo)editProfileCommentsResultSerializer.Deserialize(xmlNodeReaderUpdateClearStarOrderInfo);
            }
            catch (Exception ex)
            {
                LogExceptionMessage("UpdateClearstarWebCCFInfo", ex, loggerInstance);
                throw ex;
            }
            return updateCSOrderInfoResult;
        }

        public GetLocationsForZipCode GetLocationsForZipCodeFromClearstar(string loginName, string password, long boid,
            string zipCode, String loggerInstance)
        {
            ClearstarGatewayLookup.LookUp lu = new ClearstarGatewayLookup.LookUp();

            GetLocationsForZipCode glfzc = null;
            XmlNode xmlNodeProfileStatus = null;
            XmlNodeReader xmlNodeReaderProfileStatus = null;
            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            XmlSerializer profileStatusSerializer = new XmlSerializer(typeof(GetLocationsForZipCode));

            //TODO Clearstar Gateway
            xmlNodeProfileStatus = lu.GetLocationsForZipCode(loginName,
                password, Convert.ToInt32(boid), "USA", zipCode);
            xmlNodeReaderProfileStatus = new XmlNodeReader(xmlNodeProfileStatus);
            glfzc = (GetLocationsForZipCode)profileStatusSerializer.Deserialize(xmlNodeReaderProfileStatus);
            return glfzc;
        }

        public GetLocationsForCountry GetLocationsForCountryFromClearstar(string loginName, string password, long boid,
            string countryCode, String loggerInstance)
        {
            ClearstarGatewayLookup.LookUp lu = new ClearstarGatewayLookup.LookUp();

            GetLocationsForCountry glfc = null;
            XmlNode xmlNodeProfileStatus = null;
            XmlNodeReader xmlNodeReaderProfileStatus = null;
            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            XmlSerializer profileStatusSerializer = new XmlSerializer(typeof(GetLocationsForCountry));
            //TODO Clearstar Gateway
            xmlNodeProfileStatus = lu.GetLocationsForCountry(loginName,
                password, Convert.ToInt32(boid), countryCode);
            xmlNodeReaderProfileStatus = new XmlNodeReader(xmlNodeProfileStatus);
            glfc = (GetLocationsForCountry)profileStatusSerializer.Deserialize(xmlNodeReaderProfileStatus);
            return glfc;
        }

        public XmlNode GetServices(string loginName, string password, int boid, String custID)
        {
            return new ServiceGateWay.Service().GetServices(loginName, password, boid, custID);
        }

        public GetServices GetClearStarServices(string loginName, string password, int boid, String custID)
        {
            try
            {
                GetServices services = null;
                XmlNode xmlNodeGetServices = null;
                XmlNodeReader xmlNodeReaderGetServices = null;
                XmlSerializer getServicesSerializer = new XmlSerializer(typeof(GetServices));

                xmlNodeGetServices = new ServiceGateWay.Service().GetServices(loginName, password, boid, custID);
                xmlNodeReaderGetServices = new XmlNodeReader(xmlNodeGetServices);
                services = (GetServices)getServicesSerializer.Deserialize(xmlNodeReaderGetServices);
                return services;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public XmlNode GetCountryListFromClearStar(string loginName, string password, int boid, String custID)
        {
            return new ClearstarGatewayLookup.LookUp().GetISOCountryList(loginName, password, boid);
        }

        public XmlNode GetCountryLocationsFromClearStar(string loginName, string password, int boid, String country)
        {
            return new ClearstarGatewayLookup.LookUp().GetLocationsForCountry(loginName, password, boid, country);
        }

        public GetServiceFields GetServiceFields(string loginName, string password, int boid, String serviceNo)
        {
            try
            {
                GetServiceFields serviceFields = null;
                XmlNode xmlNodeServiceFields = null;
                XmlNodeReader xmlNodeReaderServiceFields = null;
                XmlSerializer serviceFieldsSerializer = new XmlSerializer(typeof(GetServiceFields));

                xmlNodeServiceFields = new ServiceGateWay.Service().GetServiceFields(loginName, password, boid, serviceNo);
                xmlNodeReaderServiceFields = new XmlNodeReader(xmlNodeServiceFields);
                serviceFields = (GetServiceFields)serviceFieldsSerializer.Deserialize(xmlNodeReaderServiceFields);
                return serviceFields;
            }
            catch (Exception ex)
            {
                throw ex;
            }



            //return new ServiceGateWay.Service().GetServiceFields(loginName, password, boid, serviceNo);
        }

        public XmlNode GetServiceDetails(string loginName, string password, int boid, String custID, String serviceNo)
        {
            return new ServiceGateWay.Service().GetServiceDetail(loginName, password, boid, custID, serviceNo, "Y", "Y");
        }

        private static void SaveExtSvcInvokeHistory(Int32 tenantID, Int32? bkgOrderID, Int32? bkgOrderPackageSvcLineItemID, Int32? bkgOrderPackageSvcGroupID, String methodName, List<ParamObject> parameterData,
                                              String response, String svcName, DateTime methodStartTime, DateTime methodEndTime, String comment = "")
        {
            try
            {
                using (StringWriter textWriter = new StringWriter())
                {
                    XmlSerializer createParameterSerializer = new XmlSerializer(typeof(List<ParamObject>));
                    createParameterSerializer.Serialize(textWriter, parameterData);

                    ExternalVendorOrderManager.SaveExtSvcInvokeHistory(tenantID, bkgOrderID, bkgOrderPackageSvcLineItemID, bkgOrderPackageSvcGroupID, methodName, textWriter.ToString(),
                                                                        response, String.Empty, methodStartTime, methodEndTime, comment);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void LogExceptionMessage(String methodName, Exception ex, String loggerInstance)
        {
            ServiceLogger.Error(String.Format("An Error has occured in " + methodName + " method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                            ex.Message, ex.InnerException, ex.StackTrace), loggerInstance);
        }

        public String IsProfileAndOrderExist(String customerID, String profileNumber, String clearStarOrderID)
        {
            String resultMessage = String.Empty;
            GetProfileDetailProfile gpdp = null;
            String loginName = AppConsts.VENDOR_LOGIN_ID;
            String password = AppConsts.VENDOR_PASSWORD;
            Int32 boID = AppConsts.VENDOR_BUSINESS_OWNER_ID;

            if (ConfigurationManager.AppSettings["ClearStarLoginID"].IsNotNull())
            {
                loginName = Convert.ToString(ConfigurationManager.AppSettings["ClearStarLoginID"]);
            }

            if (ConfigurationManager.AppSettings["ClearStarPwd"].IsNotNull())
            {
                password = Convert.ToString(ConfigurationManager.AppSettings["ClearStarPwd"]);
            }

            if (ConfigurationManager.AppSettings["BusinessOwnerID"].IsNotNull())
            {
                boID = Convert.ToInt32(ConfigurationManager.AppSettings["BusinessOwnerID"]);
            }

            ClearstarGatewayProfile.Profile pro = new ClearstarGatewayProfile.Profile();
            pro.EnableDecompression = true;
            GetProfileDetail gpd = null;
            XmlNode xmlNodeCreateResult = null;
            XmlNodeReader xmlNodeReaderCreateResult = null;
            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            XmlSerializer getProfileDetailSerializer = new XmlSerializer(typeof(GetProfileDetail));

            try
            {
                DateTime methodStartTime = DateTime.Now;
                //Get Order from Clearstar Gateway
                xmlNodeCreateResult = pro.GetProfileDetail(loginName, password, boID, customerID, profileNumber);
                DateTime methodEndTime = DateTime.Now;

                xmlNodeReaderCreateResult = new XmlNodeReader(xmlNodeCreateResult);
                gpd = (GetProfileDetail)getProfileDetailSerializer.Deserialize(xmlNodeReaderCreateResult);

                if (gpd.Items.Length > 0 && ((GetProfileDetailErrorStatus)gpd.Items[0]).Code == "0")
                {
                    gpdp = ((GetProfileDetailProfile)gpd.Items[1]);

                    if (!gpdp.Order.Any(cnd => cnd.OrderID == clearStarOrderID))
                    {
                        resultMessage = "Vendor Order Id does not exist in given profile";
                    }
                }
                else
                {
                    resultMessage = "Profile does not exist on clearstar.";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultMessage;
        }

        [Serializable]
        public class ParamObject
        {
            public String ParameterName
            {
                get;
                set;
            }

            public Object ParameterValue
            {
                get;
                set;
            }
        }


    }
}
