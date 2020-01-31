//using INTSOF.Utils;
//using RadPdf.Lite;
//using RadPdfStore.Utils;

//// Not in use by default
//// Uncomment the approprate line in CustomPdfIntegrationProvider.cs
//public class MongoDBPdfLiteStorageProvider : PdfLiteStorageProvider
//{
//    public override void DeleteData(PdfLiteSession session)
//    {
//        RadPdfUtils.GetMongoDBPdfLiteStorageServiceInstance().Remove(session.ID.ToString());
//    }

//    public override byte[] GetData(PdfLiteSession session, int subtype)
//    {
//        var storage = RadPdfUtils.GetMongoDBPdfLiteStorageServiceInstance().Get(session.ID.ToString(), subtype);
//        if (storage.IsNotNull())
//        {
//            return storage.Value;
//        }
//        return null;
//    }

//    public override void SetData(PdfLiteSession session, int subtype, byte[] value)
//    {
//        RadPdfUtils.GetMongoDBPdfLiteStorageServiceInstance().Create(new RadPdfStore.Models.MongoDBPdfLiteStorage { Key = session.ID.ToString(), SubType = subtype, Value = value });
//    }
//}

