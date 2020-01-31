using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IFingerPrintLocationMapView
    {
        Int32 TenantId { get; set; }
        LocationContract LocationDetails { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        String SelectedLocationAddress { get; set; }
        Boolean IsEditMode { get; set; }
        Boolean IsSaveMode { get; set; }
        Int32 SelectedLocationId { get; set; }
        Boolean IsEditClicked { get; set; }
        List<LocationContract> lstAvailableLocations { get; set; }
        Boolean IsApplicant { get; }
        Boolean IsEnroller { get; set; }       
        bool IsReadOnly { get; set; }
        List<FingerprintLocationGroupContract> lstLocationGroup { get; set; }
        List<FingerPrintLocationImagesContract> AddedLocationImagesData { get; set; }
        Int32 PageSize { get; set; }
        Int32 CurrentPageIndex { get; set; }
        Int32 VirtualRecordCount { get; set; }
        CustomPagingArgsContract GridCustomPaging { get; set; }
    }
}
