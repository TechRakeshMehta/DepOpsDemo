//using INTSOF.Utils;
//using RadPdf.Lite;
//using RadPdfStore.Utils;
//using System.Data.SqlClient;

//// Not in use by default
//// Uncomment the approprate line in CustomPdfIntegrationProvider.cs
//public class MongoDBPdfLiteSessionProvider : PdfLiteSessionProvider
//{
//    public override string AddSession(PdfLiteSession session)
//    {
//        // Generate a session key using RAD PDF's default generator.
//        // Your own generator can be used here instead.
//        string key = GenerateKey();

//        RadPdfUtils.GetMongoDBPdfLiteSessionServiceInstance().Create(new RadPdfStore.Models.MongoDBPdfLiteSession { Key = key, Value = session.Serialize() });

//        return key;
//    }

//    public override PdfLiteSession GetSession(string key)
//    {
//        var sessionData = RadPdfUtils.GetMongoDBPdfLiteSessionServiceInstance().Get(key);
//        if (sessionData.IsNotNull() && sessionData.Value.IsNotNull())
//        {
//            return PdfLiteSession.Deserialize(sessionData.Value);
//        }
//        else
//        {
//            return null;
//        }
//    }
//}

