using Entity.ClientEntity;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.QueueManagement.Views
{
     public interface IAssignmentConfigurationQueueView
    {
         /// <summary>
        /// Get and Set QueueType
         /// </summary>
         QueueConfirgurationType QueueType
         {
             get;
             set;
         }
         /// <summary>
         ///  to get and set Tenant List.
         /// </summary>
         List<Tenant> lstTenant
         {
             get;
             set;
         }
         /// <summary>
         /// to get the Assignment Configuration record for grid 
         /// </summary>
         List<QueueAssignmentConfRecord> AssignmentConfigRecord 
         { 
             get; 
             set; 
         }
         /// <summary>
         /// to Get and Set the Selected Tenant ID
         /// </summary>
         Int32 SelectedTenantId
         {
             get;
             set;
         }
         /// <summary>
         /// to get and set QueueNamelist
         /// </summary>
         List<QueueMetaData> lstQueueData
         {
             get;
             set;
         }
         /// <summary>
         /// To get and Set Meta data list
         /// </summary>
         List<QueueFieldsMetaData> lstQueueFieldsMetaData
         {
             get;
             set;
         }
         /// <summary>
         /// to get or set Selected QueueID
         /// </summary>
         Int32 SelectedQueueId
         {
             get;
             set;
         }
         /// <summary>
         /// to get code for the QueueType
         /// </summary>
         String QueueTypeCode 
         { 
             get;
         }
        
         Int32 CurrentLoggedInUserId
         {
             get;
         }
         /// <summary>
         ///  to get and set Description for the Queue
         /// </summary>
         String Description 
         {
             get;
             set;
         }
         /// <summary>
         /// To get the TenantID
         /// </summary>
         Int32 TenantId 
         {
             get;
             set;
         }
         /// <summary>
         ///  to get or set Selected QueueID
         /// </summary>
         Int32 SelectedQueueFieldID 
         {
             get;
             set;
         }
         /// <summary>
         /// to get or set Specialized FieldValue List
         /// </summary>
         List<GetQueueSpecilizationCriterion> LstQueueSpecilizationCriterion
         {
             get;
             set;
         }
         /// <summary>
         /// to get or set the QueueFieldValue//MetaDataValue
         /// </summary>
         String SelectedQueueFieldValue 
         {
             get;
             set;
         }

         String ErrorMessage { get; set; }

         String SuccessMessage { get; set; }
         String InfoMessage { get; set; }

         #region Custom Paging Parameters

         /// <summary>
         /// CurrentPageIndex</summary>
         /// <value>
         /// Gets or sets the value for CurrentPageIndex.</value>
         Int32 CurrentPageIndex
         {
             get;
             set;
         }

         /// <summary>
         /// PageSize</summary>
         /// <value>
         /// Gets the value for PageSize.</value>
         Int32 PageSize
         {
             get;
             set;
         }

         /// <summary>
         /// VirtualPageCount</summary>
         /// <value>
         /// Sets the value for VirtualPageCount.</value>
         Int32 VirtualRecordCount
         {
             set;
         }

         /// <summary>
         /// get object of shared class of custom paging
         /// </summary>
         CustomPagingArgsContract GridCustomPaging
         {
             get;
             set;
         }

         #endregion
    }
}
