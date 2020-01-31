using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public interface IMapServiceAttributeToGroupView
    {
        Int32 ServiceGroupId
        {
            get;
            set;
        }

        String ServiceGroupName
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;

        }

        MapServiceAttributeToGroupContract ViewContract
        {
            get;

        }

        String ErrorMessage
        {
            get;
            set;
        }
        List<MapServiceAttributeToGroupContract> MappedSvcAttributeList
        {
            get;
            set;
        }
        Int32 DefaultTenantId
        {
            get;
            set;
        }
        List<BkgSvcAttributeGroup> ListAttributeGrps
        {
            get;
            set;
        }
        Int32 SelectedAttributeGrp
        {
            get;
            set;
        }
        List<BkgSvcAttribute> UnmappedSvcAttributeList
        {
            get;
            set;
        }
        List<Int32> SelectedAttributes
        {
            get;
            set;
        }

        Int32 SelectedAttributeId { get; set; }

        List<BkgSvcAttribute> SourceSvcAttributeList
        {
            get;
            set;
        }
    }
}
