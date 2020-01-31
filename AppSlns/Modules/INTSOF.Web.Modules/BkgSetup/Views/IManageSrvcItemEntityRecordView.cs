using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.BkgSetup.Views
{
    public interface IManageSrvcItemEntityRecordView
    {
        IManageSrvcItemEntityRecordView CurrentViewContext
        {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        Int32 PackageServiceItemId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        List<GetServiceItemEntityList> ServiceItemEntityList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        List<GetAttributeListForServiceItemEntity> AttributeList { get; set; }

        List<Entity.State> StateList
        {
            get;
            set;
        }

        List<Entity.County> CountyList
        {
            get;
            set;
        }

        Int32 SelectedStateId
        {
            get;
        }

        Int32 SelectedAttributeId
        {
            get;
        }

        String SelectedStateValue
        {
            get;
        }

        String SelectedCountyValue
        {
            get;
        }

        Boolean ifAllOccurenceChecked
        {
            get;
        }

        String AttributeType
        {
            get;
            set;
        }

        Int32 ServiceItemEntityId
        {
            get;
            set;
        }

        String AttributeValue
        {
            get;
        }

         String ErrorMessage
        {
            get;
            set;
        }
    }
}
