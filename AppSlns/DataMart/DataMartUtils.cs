using DataMart.Services;
using INTSOF.Utils;

namespace DataMart.Utils
{
    public class DataMartUtils
    {
        public static AgencyUserService GetAgencyUserServiceInstance(AccessIntent intent = AccessIntent.Read)
        {
            return new AgencyUserService(intent);
        }

        public static CollectionVersionService GetCollectionVersionServiceInstance()
        {
            return new CollectionVersionService();
        }

        public static SharedItemService GetSharedItemServiceInstance(AccessIntent intent = AccessIntent.Read)
        {
            return new SharedItemService(intent);
        }

        public static RotationDetailService GetRotationDetailServiceInstance(AccessIntent intent = AccessIntent.Read)
        {
            return new RotationDetailService(intent);
        }

        public static SavedSearchService GetSavedSearchServiceInstance()
        {
            return new SavedSearchService();
        }
    }

    public enum AccessIntent
    {
        Read,
        Write
    }

    public enum ScheduledTasks
    {
        [StringValue("AgencyUsers")]
        AgencyUsers,
        [StringValue("InvitationGroups")]
        InvitationGroups,
        [StringValue("InitializeCollections")]
        InitializeCollections,
        [StringValue("RotationDetails")]
        RotationDetails
    }

    public enum DataMartCollections
    {
        [StringValue("AgencyUsers")]
        AgencyUsers,
        [StringValue("SharedItems")]
        SharedItems,
        [StringValue("RotationDetails")]
        RotationDetails
    }

    public enum AgencyUserItemAccess
    {
        [StringValue("Tracking")]
        Tracking,
        [StringValue("Rotation")]
        Rotation
    }

    public enum SearchType
    {
        [StringValue("CategoryDataReport")]
        CategoryDataReport,
        [StringValue("CategoryDataReportByComplioID")]
        CategoryDataReportByComplioID,
        [StringValue("ItemDataCountReport")]
        ItemDataCountReport,
        [StringValue("RotationStudentsOverallNonComplianceStatus")]
        RotationStudentsOverallNonComplianceStatus,
        [StringValue("RotationStudentDetails")]
        RotationStudentDetails,
        [StringValue("RotationStudentsByDay")]
        RotationStudentsByDay,
        [StringValue("ProfileCount")]
        ProfileCount
    }

    public class DataMartConsts
    {
        public const string COLLECTION_REPLICA_1 = "_Replica1";
        public const string COLLECTION_REPLICA_2 = "_Replica1";
        public const string INITIAL_SYNC_DATE = "01/01/1900";
        public const string DATA_MART_MANAGER = "DataMartManager";
    }
}
