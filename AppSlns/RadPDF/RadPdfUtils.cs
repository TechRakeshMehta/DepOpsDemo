using RadPdfStore.Services;
using INTSOF.Utils;

namespace RadPdfStore.Utils
{
    public class RadPdfUtils
    {
        public static MongoDBPdfLiteSessionService GetMongoDBPdfLiteSessionServiceInstance()
        {
            return new MongoDBPdfLiteSessionService();
        }

        public static MongoDBPdfLiteStorageService GetMongoDBPdfLiteStorageServiceInstance()
        {
            return new MongoDBPdfLiteStorageService();
        }
    }
}
