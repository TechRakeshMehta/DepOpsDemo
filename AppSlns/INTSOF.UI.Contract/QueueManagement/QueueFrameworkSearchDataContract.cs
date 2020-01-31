using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.QueueManagement
{
   public class QueueFrameworkSearchDataContract
    {
       public CustomPagingArgsContract GridCustomPagingArguments
       {
           get;
           set;
       }

       public Int32 TenantID 
       {
           get;
           set;
       }
       public String Description
       {
           get; 
           set; 
       }
       public Int32 SelectedQueueId
       {
           get;
           set;
       }
       public Int32 SelectedQueueFieldID
       {
           get;
           set; 
       }
       public String SelectedQueueFieldValue { get; set; }

       public Int32 SelectedBusinessProcessId
       {
           get;
           set;
       }

       public Int32 RecordId
       {
           get;
           set;
       }

        #region QUEUEAUDIT
       public DateTime? FromDate
       {
           get;
           set;
       }

       public DateTime? ToDate
       {
           get;
           set;
       }

       public Int32 LoggedInUserId
       {
           get;
           set;
       }
       public Int32? SelectedUserId
       {
           get;
           set;
       }
       public Int32 LoggedInUserTenantId
       {
           get;
           set;
       }
        #endregion
    }
}
