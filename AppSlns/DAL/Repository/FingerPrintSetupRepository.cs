using DAL.Interfaces;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using Entity.LocationEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Configuration;
//using System.Data.Entity.Spatial; /// Dependency on Microsoft.SqlServer.types.dll.

namespace DAL.Repository
{
    public class FingerPrintSetupRepository : BaseRepository, IFingerPrintSetupRepository
    {
        private ADB_LocationDataEntities _locationDBContext;
      

        #region Default Constructor to initilize DB Context
        ///// <summary>
        ///// Default constructor to initialize the context
        ///// </summary>
        public FingerPrintSetupRepository()
            : base()
        {
            _locationDBContext = base.LocationDBContext;
        }

        #endregion

        #region Methods

        Int32 IFingerPrintSetupRepository.SaveFingerPrintLocations(Int32 currentLoggedIdUserID, LocationContract locationContract)
        {
            FingerPrintLocation fingerPrintLoc = new FingerPrintLocation();
            if (!locationContract.LocationID.IsNullOrEmpty() && locationContract.LocationID > AppConsts.NONE)
            {
                fingerPrintLoc = _locationDBContext.FingerPrintLocations.Where(cond => cond.FPL_ID == locationContract.LocationID && !cond.FPL_IsDeleted).FirstOrDefault();
                fingerPrintLoc.FPL_Name = locationContract.LocationName;
                fingerPrintLoc.FPL_Description = locationContract.Description;
                fingerPrintLoc.FPL_ZipCode = locationContract.Zipcode;
                fingerPrintLoc.FPL_Address = locationContract.Address;
                fingerPrintLoc.FPL_State = locationContract.State;
                fingerPrintLoc.FPL_City = locationContract.City;
                fingerPrintLoc.FPL_Country = locationContract.Country;
                fingerPrintLoc.FPL_County = locationContract.County;
                fingerPrintLoc.FPL_Longitude = locationContract.Longitude;
                fingerPrintLoc.FPL_Latitude = locationContract.Latitude;
                fingerPrintLoc.FPL_FullAddress = locationContract.FullAddress;
                fingerPrintLoc.FPL_ModifiedBy = currentLoggedIdUserID;
                fingerPrintLoc.FPL_ModifiedOn = DateTime.Now;
                fingerPrintLoc.FPL_PlaceID = locationContract.PlaceID;
                fingerPrintLoc.FPL_LocationGroupID = locationContract.LocationGroupID > AppConsts.NONE ? locationContract.LocationGroupID : (int?)null;
                fingerPrintLoc.FPL_IsContractLocation = locationContract.IsContractLocation; //UAT 3675
                fingerPrintLoc.FPL_SkipABIReview = locationContract.IsSkipABIReview;
                fingerPrintLoc.FPL_IsPassPhotoService = true;// locationContract.IsPhotoService;
                fingerPrintLoc.FPL_IsPrinterAvailable = false; // locationContract.IsPrinterAvailable;

                //fingerPrintLoc.FPL_GeoCode = DbGeography.FromText(locationContract.GeoCode);   ///// Dependency on Microsoft.SqlServer.types.dll.

            }
            else
            {
                fingerPrintLoc.FPL_Name = locationContract.LocationName;
                fingerPrintLoc.FPL_Description = locationContract.Description;
                fingerPrintLoc.FPL_Country = locationContract.Country;
                fingerPrintLoc.FPL_State = locationContract.State;
                fingerPrintLoc.FPL_City = locationContract.City;
                fingerPrintLoc.FPL_ZipCode = locationContract.Zipcode;
                fingerPrintLoc.FPL_Address = locationContract.Address;
                fingerPrintLoc.FPL_County = locationContract.County;
                fingerPrintLoc.FPL_Longitude = locationContract.Longitude;
                fingerPrintLoc.FPL_Latitude = locationContract.Latitude;
                fingerPrintLoc.FPL_FullAddress = locationContract.FullAddress;
                fingerPrintLoc.FPL_CreatedOn = DateTime.Now;
                fingerPrintLoc.FPL_CreatedBy = currentLoggedIdUserID;
                fingerPrintLoc.FPL_PlaceID = locationContract.PlaceID;
                fingerPrintLoc.FPL_LocationGroupID = locationContract.LocationGroupID > AppConsts.NONE ? locationContract.LocationGroupID : (int?)null;
                fingerPrintLoc.FPL_IsContractLocation = locationContract.IsContractLocation; //UAT 3675
                fingerPrintLoc.FPL_SkipABIReview = locationContract.IsSkipABIReview;
                fingerPrintLoc.FPL_IsPrinterAvailable = locationContract.IsPrinterAvailable;
                fingerPrintLoc.FPL_IsPassPhotoService = locationContract.IsPhotoService;
                // fingerPrintLoc.FPL_GeoCode = DbGeography.FromText(locationContract.GeoCode);    /// Dependency on Microsoft.SqlServer.types.dll.

                _locationDBContext.FingerPrintLocations.AddObject(fingerPrintLoc);
            }

            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
                return fingerPrintLoc.FPL_ID;
            return AppConsts.NONE;
        }

        Boolean IFingerPrintSetupRepository.DeleteFingerPrintLocations(Int32 currentLoggedIdUserID, Int32 selectedLocationID)
        {
            FingerPrintLocation fingerprintLoc = _locationDBContext.FingerPrintLocations.Where(cond => !cond.FPL_IsDeleted && cond.FPL_ID == selectedLocationID).FirstOrDefault();
            if (!fingerprintLoc.IsNullOrEmpty())
            {
                fingerprintLoc.FPL_IsDeleted = true;
                fingerprintLoc.FPL_ModifiedBy = currentLoggedIdUserID;
                fingerprintLoc.FPL_ModifiedOn = DateTime.Now;

                //Below code is commented as the location is deleted, no other will be retreive from database as depending on joins.

                //List<EnrollerLocationPermission> lstEnrollerLocationPermissions = _locationDBContext.EnrollerLocationPermissions.Where(cond => !cond.ELP_IsDeleted && cond.ELP_LocationID == fingerprintLoc.FPL_ID && cond.ELP_LocationID == selectedLocationID).ToList();

                //if (!lstEnrollerLocationPermissions.IsNullOrEmpty() && lstEnrollerLocationPermissions.Count() > AppConsts.NONE)
                //{
                //    foreach (EnrollerLocationPermission enrollerLocPermission in lstEnrollerLocationPermissions)
                //    {
                //        enrollerLocPermission.ELP_IsDeleted = true;
                //        enrollerLocPermission.ELP_ModifiedByID = currentLoggedIdUserID;
                //        enrollerLocPermission.ELP_ModifiedOn = DateTime.Now;
                //    }
                //}

                ////Delete location tenant Mapping.

                //List<LocationTenantMapping> lstLocTenantMapping = _locationDBContext.LocationTenantMappings.Where(cond=>!cond.LTM_IsDeleted && cond.LTM_LocationID == selectedLocationID).ToList();
                //if (!lstLocTenantMapping.IsNullOrEmpty() && lstLocTenantMapping.Count > AppConsts.NONE)
                //{

                //}
            }

            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        List<LocationContract> IFingerPrintSetupRepository.GetFingerprintLocations(Int32 orgUserId, Boolean isEnroller, Boolean getFullAccessOnly = false)
        {
            List<LocationContract> lstFingerprintLocContract = new List<LocationContract>();
            List<FingerPrintLocation> lstFingerPrintLoc = new List<FingerPrintLocation>();
            if (!orgUserId.IsNullOrEmpty() && orgUserId > AppConsts.NONE && isEnroller)
            {
                String noAccessCode = LkpPermission.NoAccess.GetStringValue().ToLower();
                String readOnlyAccessCode = LkpPermission.ReadOnly.GetStringValue().ToLower();
                lstFingerPrintLoc = _locationDBContext.EnrollerLocationPermissions.Where(cond => !cond.ELP_IsDeleted && !cond.FingerPrintLocation.FPL_IsDeleted
                                                                                     && cond.ELP_OrganizationUserID == orgUserId
                                                                                     && cond.lkpPermission.PER_Code.ToLower() != noAccessCode
                                                                                     && (!getFullAccessOnly
                                                                                          || (getFullAccessOnly
                                                                                               && cond.lkpPermission.PER_Code.ToLower() != readOnlyAccessCode
                                                                                              )
                                                                                         )
                                                                                     ).Select(sel => sel.FingerPrintLocation).ToList();
            }
            else
            {
                lstFingerPrintLoc = _locationDBContext.FingerPrintLocations.Where(cond => !cond.FPL_IsDeleted).ToList();
            }

            if (!lstFingerPrintLoc.IsNullOrEmpty() && lstFingerPrintLoc.Count > AppConsts.NONE)
            {
                foreach (FingerPrintLocation fingerPrintloc in lstFingerPrintLoc)
                {
                    LocationContract fingerPrintLocContractObj = new LocationContract();
                    fingerPrintLocContractObj.LocationID = fingerPrintloc.FPL_ID;
                    fingerPrintLocContractObj.FullAddress = fingerPrintloc.FPL_FullAddress.IsNullOrEmpty() ? String.Empty : fingerPrintloc.FPL_FullAddress;
                    fingerPrintLocContractObj.Zipcode = fingerPrintloc.FPL_ZipCode.IsNullOrEmpty() ? String.Empty : fingerPrintloc.FPL_ZipCode;
                    fingerPrintLocContractObj.Address = fingerPrintloc.FPL_Address.IsNullOrEmpty() ? String.Empty : fingerPrintloc.FPL_Address;
                    fingerPrintLocContractObj.State = fingerPrintloc.FPL_State.IsNullOrEmpty() ? String.Empty : fingerPrintloc.FPL_State;
                    fingerPrintLocContractObj.City = fingerPrintloc.FPL_City.IsNullOrEmpty() ? String.Empty : fingerPrintloc.FPL_City;
                    fingerPrintLocContractObj.Country = fingerPrintloc.FPL_Country.IsNullOrEmpty() ? String.Empty : fingerPrintloc.FPL_Country;
                    fingerPrintLocContractObj.County = fingerPrintloc.FPL_County.IsNullOrEmpty() ? String.Empty : fingerPrintloc.FPL_County;
                    fingerPrintLocContractObj.Longitude = fingerPrintloc.FPL_Longitude.IsNullOrEmpty() ? String.Empty : fingerPrintloc.FPL_Longitude;
                    fingerPrintLocContractObj.Latitude = fingerPrintloc.FPL_Latitude.IsNullOrEmpty() ? String.Empty : fingerPrintloc.FPL_Latitude;
                    fingerPrintLocContractObj.LocationName = fingerPrintloc.FPL_Name.IsNullOrEmpty() ? String.Empty : fingerPrintloc.FPL_Name;
                    fingerPrintLocContractObj.Description = fingerPrintloc.FPL_Description.IsNullOrEmpty() ? String.Empty : fingerPrintloc.FPL_Description;
                    fingerPrintLocContractObj.ScheduleMasterID = fingerPrintloc.FPL_ScheduleMasterID.IsNullOrEmpty() ? AppConsts.NONE : fingerPrintloc.FPL_ScheduleMasterID;
                    fingerPrintLocContractObj.PlaceID = fingerPrintloc.FPL_PlaceID.IsNullOrEmpty() ? String.Empty : fingerPrintloc.FPL_PlaceID;
                    fingerPrintLocContractObj.LocationGroupName = fingerPrintloc.FingerprintLocationGroup.IsNullOrEmpty() ? String.Empty : fingerPrintloc.FingerprintLocationGroup.FLG_Name;
                    fingerPrintLocContractObj.IsContractLocation = fingerPrintloc.FPL_IsContractLocation ?? false;
                    fingerPrintLocContractObj.IsSkipABIReview = fingerPrintloc.FPL_SkipABIReview;
                    fingerPrintLocContractObj.IsPrinterAvailable = fingerPrintloc.FPL_IsPrinterAvailable;
                    fingerPrintLocContractObj.IsPhotoService = fingerPrintloc.FPL_IsPassPhotoService;
                    //fingerPrintLocContractObj.GeoCode = fingerPrintloc.FPL_GeoCode.IsNullOrEmpty() ? String.Empty : fingerPrintloc.FPL_GeoCode.ToString();
                    fingerPrintLocContractObj.IsEnroller = isEnroller;
                    lstFingerprintLocContract.Add(fingerPrintLocContractObj);
                }
            }

            return lstFingerprintLocContract;
        }

        List<LocationContract> IFingerPrintSetupRepository.GetAssignedFingerprintLocations(string selectedTenantIds, Int32? organizationUserId, CustomPagingArgsContract customPagingArgsContract)
        {
            List<LocationContract> lstFingerprintLocContract = new List<LocationContract>();

            String orderBy = "LocationName";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = _locationDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@organizationUserId",organizationUserId),
                            new SqlParameter("@selectedTenantIds",selectedTenantIds),
                            new SqlParameter("@OrderBy", orderBy),
                            new SqlParameter("@OrderDirection", ordDirection),
                            new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                            new SqlParameter("@PageSize", customPagingArgsContract.PageSize)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetAssignedFingerprintLocations", sqlParameterCollection))
                {
                    int intParseResult;
                    if (dr.HasRows)
                    {
                        Guid guidParseResult = Guid.Empty;
                        while (dr.Read())
                        {
                            LocationContract locationContract = new LocationContract();
                            locationContract.LocationID = Convert.ToInt32(dr["FPL_ID"]);
                            locationContract.LocationName = dr["FPL_Name"].ToString();
                            locationContract.FullAddress = dr["FPL_FullAddress"].ToString();
                            locationContract.TotalCount = Convert.ToInt32(dr["TotalCount"].ToString());
                            locationContract.TenantName = dr["TenantName"].ToString();
                            if (int.TryParse(dr["FPL_ScheduleMasterID"].ToString(), out intParseResult))
                            {
                                locationContract.ScheduleMasterID = intParseResult;
                            }
                            locationContract.PermissionCode = dr["PER_Code"].ToString();
                            lstFingerprintLocContract.Add(locationContract);
                        }
                    }
                }
            }

            return lstFingerprintLocContract;
        }

        List<AppointmentSlotContract> IFingerPrintSetupRepository.GetLocationAppointmentSlots(AppointmentSlotContract searchContract, Int32 locationId)
        {
            List<AppointmentSlotContract> result = new List<AppointmentSlotContract>();

            EntityConnection connection = _locationDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                List<SqlParameter> sqlParameterCollection = new List<SqlParameter>
                        {
                            new SqlParameter("@locationId",locationId)
                        };
                if (searchContract.IsNotNull())
                {
                    sqlParameterCollection.Add(new SqlParameter("@slotDate", searchContract.SlotDate));
                }

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetFingerprintLocationSlots", sqlParameterCollection.ToArray()))
                {
                    int intParseResult;
                    DateTime dateTimeParseResult;
                    string timeFormat = "hh:mm tt";
                    if (dr.HasRows)
                    {
                        Guid guidParseResult = Guid.Empty;
                        while (dr.Read())
                        {
                            AppointmentSlotContract appointmentSlot = new AppointmentSlotContract();
                            appointmentSlot.SlotID = Convert.ToInt32(dr["FPSS_SlotID"]);
                            if (DateTime.TryParse(dr["FPSS_SlotDate"].ToString(), out dateTimeParseResult))
                            {
                                appointmentSlot.SlotDate = dateTimeParseResult;
                            }
                            if (DateTime.TryParse(dr["FPSS_SlotStartTime"].ToString(), out dateTimeParseResult))
                            {
                                appointmentSlot.SlotStartTime = dateTimeParseResult.ToString(timeFormat);
                            }
                            if (DateTime.TryParse(dr["FPSS_SlotEndTime"].ToString(), out dateTimeParseResult))
                            {
                                appointmentSlot.SlotEndTime = dateTimeParseResult.ToString(timeFormat);
                            }
                            if (int.TryParse(dr["FPSS_SlotAppointment"].ToString(), out intParseResult))
                            {
                                appointmentSlot.SlotAppointment = intParseResult;
                            }
                            if (int.TryParse(dr["FPL_ID"].ToString(), out intParseResult))
                            {
                                appointmentSlot.LocationId = intParseResult;
                            }
                            result.Add(appointmentSlot);
                        }
                    }
                }
            }

            return result;
        }

        Boolean IFingerPrintSetupRepository.UpdateAppointmentSlot(AppointmentSlotContract appointmentSlotContract, Int32 currentLoggedInUserId)
        {
            Boolean result = false;
            var slotAppointmentCoun = _locationDBContext.FingerPrintApplicantAppointments.Count(fpA => !fpA.FPAA_IsDeleted
            && fpA.FPAA_ScheduleSlotID == appointmentSlotContract.SlotID);

            if (slotAppointmentCoun > appointmentSlotContract.SlotAppointment)
            {
                throw new SysXException(slotAppointmentCoun + " Appointment(s) are already ordered for the selected slot. Please select a greater value.");
            }

            var entity = _locationDBContext.FingerPrintScheduleSlots.FirstOrDefault(fps => !fps.FPSS_IsDeleted
                && fps.FPSS_SlotID == appointmentSlotContract.SlotID);

            if (entity != null)
            {
                entity.FPSS_SlotAppointment = appointmentSlotContract.SlotAppointment;
                entity.FPSS_ModifiedBy = currentLoggedInUserId;
                entity.FPSS_ModifiedOn = DateTime.Now;
                _locationDBContext.SaveChanges();
                result = true;
            }
            else
            {
                throw new SysXException("Appointment slot not found. Can't Update.");
            }
            return result;
        }

        LocationContract IFingerPrintSetupRepository.GetSelectedLocationDetails(Int32 selectedLocationId)
        {
            LocationContract locationDetails = new LocationContract();
            if (!selectedLocationId.IsNullOrEmpty() && selectedLocationId > AppConsts.NONE)
            {
                FingerPrintLocation fingerPrintLocation = new FingerPrintLocation();
                fingerPrintLocation = _locationDBContext.FingerPrintLocations.Where(cond => !cond.FPL_IsDeleted && cond.FPL_ID == selectedLocationId).FirstOrDefault();
                locationDetails.LocationID = fingerPrintLocation.FPL_ID;
                locationDetails.FullAddress = fingerPrintLocation.FPL_FullAddress.IsNullOrEmpty() ? String.Empty : fingerPrintLocation.FPL_FullAddress;
                locationDetails.Zipcode = fingerPrintLocation.FPL_ZipCode.IsNullOrEmpty() ? String.Empty : fingerPrintLocation.FPL_ZipCode;
                locationDetails.Address = fingerPrintLocation.FPL_Address.IsNullOrEmpty() ? String.Empty : fingerPrintLocation.FPL_Address;
                locationDetails.State = fingerPrintLocation.FPL_State.IsNullOrEmpty() ? String.Empty : fingerPrintLocation.FPL_State;
                locationDetails.City = fingerPrintLocation.FPL_City.IsNullOrEmpty() ? String.Empty : fingerPrintLocation.FPL_City;
                locationDetails.Country = fingerPrintLocation.FPL_Country.IsNullOrEmpty() ? String.Empty : fingerPrintLocation.FPL_Country;
                locationDetails.County = fingerPrintLocation.FPL_County.IsNullOrEmpty() ? String.Empty : fingerPrintLocation.FPL_County;
                locationDetails.Longitude = fingerPrintLocation.FPL_Longitude.IsNullOrEmpty() ? String.Empty : fingerPrintLocation.FPL_Longitude;
                locationDetails.Latitude = fingerPrintLocation.FPL_Latitude.IsNullOrEmpty() ? String.Empty : fingerPrintLocation.FPL_Latitude;
                locationDetails.LocationName = fingerPrintLocation.FPL_Name.IsNullOrEmpty() ? String.Empty : fingerPrintLocation.FPL_Name;
                locationDetails.Description = fingerPrintLocation.FPL_Description.IsNullOrEmpty() ? String.Empty : fingerPrintLocation.FPL_Description;
                locationDetails.PlaceID = fingerPrintLocation.FPL_PlaceID.IsNullOrEmpty() ? String.Empty : fingerPrintLocation.FPL_PlaceID;
                locationDetails.LocationGroupID = fingerPrintLocation.FPL_LocationGroupID.HasValue ? fingerPrintLocation.FPL_LocationGroupID.Value : 0;
                locationDetails.IsContractLocation = fingerPrintLocation.FPL_IsContractLocation.HasValue ? fingerPrintLocation.FPL_IsContractLocation.Value : false; //UAT 3675
                locationDetails.IsSkipABIReview = fingerPrintLocation.FPL_SkipABIReview;
                locationDetails.IsPrinterAvailable = fingerPrintLocation.FPL_IsPrinterAvailable;
                locationDetails.IsPhotoService = fingerPrintLocation.FPL_IsPassPhotoService;
                //  locationDetails.GeoCode = fingerPrintLocation.FPL_GeoCode.IsNullOrEmpty() ? String.Empty : fingerPrintLocation.FPL_GeoCode.ToString();
                locationDetails.TimeFrame = fingerPrintLocation.FPL_TimeFrameHours;
            }
            return locationDetails;
        }

        #endregion

        List<LocationContract> IFingerPrintSetupRepository.GetLocationAvailable(Int32 tenantId)
        {
            List<LocationContract> lstlocationDetails = new List<LocationContract>();
            lstlocationDetails = _locationDBContext.LocationTenantMappings.Where(cond => cond.LTM_TenantID == tenantId && !cond.LTM_IsDeleted && !cond.FingerPrintLocation.FPL_IsDeleted)
                                 .Select(sel => new LocationContract
                                 {
                                     LocationID = sel.FingerPrintLocation.FPL_ID,
                                     Description = sel.FingerPrintLocation.FPL_Description,
                                     LocationName = sel.FingerPrintLocation.FPL_Name,
                                     Zipcode = sel.FingerPrintLocation.FPL_ZipCode == null || sel.FingerPrintLocation.FPL_ZipCode == "" ? String.Empty : sel.FingerPrintLocation.FPL_ZipCode,
                                     Address = sel.FingerPrintLocation.FPL_Address == null || sel.FingerPrintLocation.FPL_Address == "" ? String.Empty : sel.FingerPrintLocation.FPL_Address,
                                     State = sel.FingerPrintLocation.FPL_State == null || sel.FingerPrintLocation.FPL_State == "" ? String.Empty : sel.FingerPrintLocation.FPL_State,
                                     City = sel.FingerPrintLocation.FPL_City == null || sel.FingerPrintLocation.FPL_City == "" ? String.Empty : sel.FingerPrintLocation.FPL_City,
                                     Country = sel.FingerPrintLocation.FPL_Country == null || sel.FingerPrintLocation.FPL_Country == "" ? String.Empty : sel.FingerPrintLocation.FPL_Country,
                                     County = sel.FingerPrintLocation.FPL_County == null || sel.FingerPrintLocation.FPL_County == "" ? String.Empty : sel.FingerPrintLocation.FPL_County,
                                     FullAddress = sel.FingerPrintLocation.FPL_FullAddress == null || sel.FingerPrintLocation.FPL_FullAddress == "" ? String.Empty : sel.FingerPrintLocation.FPL_FullAddress,
                                     Longitude = sel.FingerPrintLocation.FPL_Longitude == null || sel.FingerPrintLocation.FPL_Latitude == "" ? String.Empty : sel.FingerPrintLocation.FPL_Longitude,
                                     Latitude = sel.FingerPrintLocation.FPL_Latitude == null || sel.FingerPrintLocation.FPL_Latitude == "" ? String.Empty : sel.FingerPrintLocation.FPL_Latitude,
                                     PlaceID = sel.FingerPrintLocation.FPL_PlaceID == null || sel.FingerPrintLocation.FPL_PlaceID == "" ? String.Empty : sel.FingerPrintLocation.FPL_PlaceID
                                     // GeoCode = sel.FingerPrintLocation.FPL_GeoCode == null ? String.Empty : sel.FingerPrintLocation.FPL_GeoCode.ToString()
                                 }).ToList();
            return lstlocationDetails;

        }

        Boolean IFingerPrintSetupRepository.SaveEnrollerMapping(ManageEnrollerMappingContract enrollerPermissionContract, Int32 currentLoggedInUserId)
        {
            if (!enrollerPermissionContract.IsNullOrEmpty())
            {
                Int16 permissionId = _locationDBContext.lkpPermissions.Where(cond => cond.PER_IsDeleted == false
                                                    && cond.PER_Code == enrollerPermissionContract.PermissionCode).Select(col => col.PER_ID).FirstOrDefault();

                EnrollerLocationPermission EnrollerMapping = new EnrollerLocationPermission();
                if (!enrollerPermissionContract.EnrollerMappingID.IsNullOrEmpty() && enrollerPermissionContract.EnrollerMappingID > AppConsts.NONE)
                {
                    //Update enroller permission mappings
                    EnrollerMapping = _locationDBContext.EnrollerLocationPermissions.Where(cond => !cond.ELP_IsDeleted && cond.ELP_ID == enrollerPermissionContract.EnrollerMappingID && cond.ELP_LocationID == enrollerPermissionContract.LocationId).FirstOrDefault();
                    EnrollerMapping.ELP_PermissionID = permissionId;
                    EnrollerMapping.ELP_ModifiedByID = currentLoggedInUserId;
                    EnrollerMapping.ELP_ModifiedOn = DateTime.Now;
                }
                else
                {
                    //Save enroller permission mappings
                    EnrollerMapping.ELP_PermissionID = permissionId;
                    EnrollerMapping.ELP_OrganizationUserID = enrollerPermissionContract.OrganizationUserID;
                    EnrollerMapping.ELP_LocationID = enrollerPermissionContract.LocationId;
                    EnrollerMapping.ELP_CreatedOn = DateTime.Now;
                    EnrollerMapping.ELP_CreatedByID = currentLoggedInUserId;

                    _locationDBContext.EnrollerLocationPermissions.AddObject(EnrollerMapping);
                }
            }

            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        #region Order Flow and Location off
        Boolean IFingerPrintSetupRepository.SaveAndUpdateTimeOff(ManageScheduleTimeContract manageScheduleTimeContract, Int32 currentLoggedInUserID)
        {
            if (!manageScheduleTimeContract.IsNullOrEmpty())
            {
                FingerPrintLocationTimeoff fingerPrintLocationTimeoff = _locationDBContext.FingerPrintLocationTimeoffs.Where(cond => cond.FLT_ID == manageScheduleTimeContract.FingureLocationTimeOffId).FirstOrDefault();
                if (fingerPrintLocationTimeoff != null && fingerPrintLocationTimeoff.FLT_ID > AppConsts.ONE)
                {
                    //Update here
                    fingerPrintLocationTimeoff.FLT_FromTime = manageScheduleTimeContract.StartDateTime;
                    fingerPrintLocationTimeoff.FLT_ToTime = manageScheduleTimeContract.EndDateTime;
                    fingerPrintLocationTimeoff.FLT_Reason = manageScheduleTimeContract.OffReason;
                    fingerPrintLocationTimeoff.FLT_ModifiedBy = currentLoggedInUserID;
                    fingerPrintLocationTimeoff.FLT_ModifiedOn = DateTime.Now;
                }
                else
                {
                    fingerPrintLocationTimeoff = new FingerPrintLocationTimeoff();
                    fingerPrintLocationTimeoff.FLT_Reason = manageScheduleTimeContract.OffReason;
                    fingerPrintLocationTimeoff.FLT_FromTime = manageScheduleTimeContract.StartDateTime;
                    fingerPrintLocationTimeoff.FLT_ToTime = manageScheduleTimeContract.EndDateTime;
                    fingerPrintLocationTimeoff.FLT_CreatedBy = currentLoggedInUserID;
                    fingerPrintLocationTimeoff.FLT_CreatedOn = DateTime.Now;
                    fingerPrintLocationTimeoff.FLT_LocationID = manageScheduleTimeContract.LocationId;
                    fingerPrintLocationTimeoff.FLT_IsDeleted = false;
                    _locationDBContext.FingerPrintLocationTimeoffs.AddObject(fingerPrintLocationTimeoff);

                }
            }
            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;

        }
        Boolean IFingerPrintSetupRepository.DeleteTimeOff(ManageScheduleTimeContract manageScheduleTimeContract, Int32 currentLoggedInUserID)
        {

            if (!manageScheduleTimeContract.IsNullOrEmpty())
            {

                FingerPrintLocationTimeoff fingerPrintLocationTimeoff = _locationDBContext.FingerPrintLocationTimeoffs.Where(cond => cond.FLT_ID == manageScheduleTimeContract.FingureLocationTimeOffId).FirstOrDefault();
                if (!fingerPrintLocationTimeoff.IsNullOrEmpty())
                {
                    fingerPrintLocationTimeoff.FLT_IsDeleted = true;
                    fingerPrintLocationTimeoff.FLT_ModifiedBy = currentLoggedInUserID;
                    fingerPrintLocationTimeoff.FLT_ModifiedOn = DateTime.Now;
                }
                if (_locationDBContext.SaveChanges() > AppConsts.NONE)
                    return true;
                return false;
            }
            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;

        }
        Int32 IFingerPrintSetupRepository.PublishTimeOff(ManageScheduleTimeContract manageScheduleTimeContract, Int32 currentLoggedInUserID)
        {
            int IsSuccess = 0;
            if (!manageScheduleTimeContract.IsNullOrEmpty())
            {

                EntityConnection connection = _locationDBContext.Connection as EntityConnection;

                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand command = new SqlCommand("ams.usp_PublishTimeOff", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FingerPrintLocationTimeoffID", manageScheduleTimeContract.FingureLocationTimeOffId);
                    command.Parameters.AddWithValue("@LocationID", manageScheduleTimeContract.LocationId);
                    command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserID);
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    IsSuccess = command.ExecuteNonQuery();
                    con.Close();
                }
                return IsSuccess;
            }
            return IsSuccess;
        }
        List<ManageScheduleTimeContract> IFingerPrintSetupRepository.GetLocationDataForScheduleOff(Int32 locationId)
        {
            try
            {
                List<ManageScheduleTimeContract> lstEnrollerPermissionsMappingContract = new List<ManageScheduleTimeContract>();
                EntityConnection connection = this._locationDBContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@LocationId", locationId)

                };
                    base.OpenSQLDataReaderConnection(con);
                    using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetLocationSchedlueTimeData", sqlParameterCollection))
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                ManageScheduleTimeContract manageScheduleTime = new ManageScheduleTimeContract();
                                manageScheduleTime.LocationId = dr["LocationId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LocationId"]);
                                manageScheduleTime.FingureLocationTimeOffId = dr["FingureLocationTimeOffId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FingureLocationTimeOffId"]);
                                manageScheduleTime.Published = Convert.ToBoolean(dr["Published"]);
                                manageScheduleTime.OffReason = Convert.ToString(dr["OffReason"]);
                                manageScheduleTime.StartDateTime = Convert.ToDateTime(dr["StartDate"]);
                                manageScheduleTime.EndDateTime = Convert.ToDateTime(dr["EndDate"]);
                                manageScheduleTime.StartTime = dr["StartTime"] != DBNull.Value ? Convert.ToString(dr["StartTime"]) : null; dr["StartTime"].ToString();
                                manageScheduleTime.EndTime = dr["EndTime"] != DBNull.Value ? Convert.ToString(dr["EndTime"]) : null; dr["EndTime"].ToString();
                                lstEnrollerPermissionsMappingContract.Add(manageScheduleTime);
                            }
                        }
                    }
                    base.CloseSQLDataReaderConnection(con);
                }
                return lstEnrollerPermissionsMappingContract;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }
        List<LocationContract> IFingerPrintSetupRepository.GetValidateEventCodeStatusAndEventDetails(FingerPrintAppointmentContract fingerPrintAppointmentContract, Int32 TenantId)
        {
            List<LocationContract> lstFingerprintLocContract = new List<LocationContract>();
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                List<SqlParameter> sqlParameterCollection = new List<SqlParameter>
                        {
                            //new SqlParameter("@TenantId",TenantId),
                            //new SqlParameter("@EventCode_Id",fingerPrintAppointmentContract.EventID),
                            //new SqlParameter("@Location_Id",fingerPrintAppointmentContract.LocationId),
                            //new SqlParameter("@Slotid",fingerPrintAppointmentContract.SlotID),
                            new SqlParameter("@Event_Code",fingerPrintAppointmentContract.TempEventCode)
                        };
                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetValidateEventCodeStatusAndEventDetails", sqlParameterCollection.ToArray()))
                {
                    if (dr.HasRows)
                    {
                        Guid guidParseResult = Guid.Empty;
                        while (dr.Read())
                        {
                            LocationContract locationContract = new LocationContract();
                            locationContract.LocationName = dr["LocationName"] != DBNull.Value ? Convert.ToString(dr["LocationName"]) : null;
                            locationContract.FullAddress = dr["LocationAddress"] != DBNull.Value ? Convert.ToString(dr["LocationAddress"]) : null;
                            locationContract.Description = dr["LocationDescription"] != DBNull.Value ? Convert.ToString(dr["LocationDescription"]) : null;
                            locationContract.SlotTime = dr["SlotTime"] != DBNull.Value ? Convert.ToString(dr["SlotTime"]) : null;
                            locationContract.EventDescription = dr["EventDescription"] != DBNull.Value ? Convert.ToString(dr["EventDescription"]) : null;
                            locationContract.EventName = dr["EventName"] != DBNull.Value ? Convert.ToString(dr["EventName"]) : null;
                            locationContract.EventFromDateTime = Convert.ToDateTime(dr["EventFromDateTime"]);
                            locationContract.SlotDate = Convert.ToDateTime(dr["SlotDate"]);
                            locationContract.EventSlotId = Convert.ToInt32(dr["EventSlotId"]);
                            locationContract.LocationID = Convert.ToInt32(dr["LocationId"]);
                            lstFingerprintLocContract.Add(locationContract);
                        }
                    }
                }
            }
            return lstFingerprintLocContract;
        }
        
        List<LocationContract> IFingerPrintSetupRepository.GetLocationDataForOutOfState(int locationId, Int32 tenantId)
        {
            List<LocationContract> lstlocationDetails = new List<LocationContract>();
            lstlocationDetails = _locationDBContext.FingerPrintLocations.Where(cond => cond.FPL_ID == locationId && !cond.FPL_IsDeleted)
                .Select(sel => new LocationContract()
                {
                    LocationID = sel.FPL_ID,
                    LocationName = sel.FPL_Name,
                    FullAddress = sel.FPL_FullAddress,
                    Description = sel.FPL_Description
                }

                ).ToList();
            return lstlocationDetails;
        }
        #endregion

        #region CBI Result Files
        Boolean IFingerPrintSetupRepository.InsertDataCBIResultFile(FingerPrintOrderContract fingerPrintOrderContract, Int32 bkgProcessUserID)
        {
            DateTime _currentTimeStamp = DateTime.Now;

            CBIResultFile cbiResultFiles = new CBIResultFile();
            cbiResultFiles.CRF_CreatedByID = bkgProcessUserID;
            cbiResultFiles.CRF_EXT = fingerPrintOrderContract.Extension;
            cbiResultFiles.CRF_FileContent = fingerPrintOrderContract.FileContent;
            cbiResultFiles.CRF_Name = fingerPrintOrderContract.Name;
            cbiResultFiles.CRF_PCNNumber = fingerPrintOrderContract.PCNNumber;
            cbiResultFiles.CRF_RejectionReason = fingerPrintOrderContract.RejectionReason;
            cbiResultFiles.CRF_Result = fingerPrintOrderContract.Result;
            cbiResultFiles.CRF_ResultStatus = fingerPrintOrderContract.ResultStatus;
            cbiResultFiles.CRF_CreatedOn = _currentTimeStamp;
            cbiResultFiles.CRF_CBITenantId = fingerPrintOrderContract.CBI_TentantId;
            cbiResultFiles.CRF_IsDeleted = true;
            _locationDBContext.CBIResultFiles.AddObject(cbiResultFiles);
            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean IFingerPrintSetupRepository.UpdateDataCBIResultFile(string PCNNumber, Int32 bkgProcessUserID)
        {
            DateTime _currentTimeStamp = DateTime.Now;
            var GetDataforUpdate = _locationDBContext.CBIResultFiles.Where(x => x.CRF_PCNNumber == PCNNumber && x.CRF_IsDeleted == true).FirstOrDefault();
            if (!GetDataforUpdate.IsNullOrEmpty())
            {
                GetDataforUpdate.CRF_IsDeleted = false;
                GetDataforUpdate.CRF_ModifiedByID = bkgProcessUserID;
                GetDataforUpdate.CRF_ModifiedOn = _currentTimeStamp;
            }
            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        #endregion


        List<ManageEnrollerMappingContract> IFingerPrintSetupRepository.GetEnrollerMappings(CustomPagingArgsContract customPagingArgsContract, Int32 locationId)
        {
            try
            {

                //List<EnrollerLocationPermission> lstEnrollerMappings = new List<EnrollerLocationPermission>();
                //lstEnrollerMappings = _ClientDBContext.EnrollerLocationPermissions.Where(cond => !cond.ELP_IsDeleted && cond.ELP_LocationID == locationId).ToList();
                //return lstEnrollerMappings;

                List<ManageEnrollerMappingContract> lstEnrollerPermissionsMappingContract = new List<ManageEnrollerMappingContract>();

                String orderBy = "EnrollerMappingID";
                String ordDirection = null;

                orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
                ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

                EntityConnection connection = this._locationDBContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@LocationId", locationId),
                    new SqlParameter("@OrderBy", orderBy),
                    new SqlParameter("@OrderDirection", ordDirection),
                    new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                    new SqlParameter("@PageSize", customPagingArgsContract.PageSize),
                };

                    base.OpenSQLDataReaderConnection(con);

                    using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetEnrollerPermissionMapping", sqlParameterCollection))
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                ManageEnrollerMappingContract enrollerPermissionMapping = new ManageEnrollerMappingContract();
                                enrollerPermissionMapping.EnrollerMappingID = dr["ELP_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ELP_ID"]);
                                enrollerPermissionMapping.LocationId = dr["ELP_LocationID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ELP_LocationID"]);
                                enrollerPermissionMapping.PermissionId = dr["ELP_PermissionID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ELP_PermissionID"]);
                                enrollerPermissionMapping.OrganizationUserID = dr["ELP_OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ELP_OrganizationUserID"]);
                                enrollerPermissionMapping.OrganizationID = dr["OrganizationID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["OrganizationID"]);
                                enrollerPermissionMapping.PermissionCode = dr["PER_Code"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PER_Code"]);
                                enrollerPermissionMapping.FirstName = dr["FirstName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["FirstName"]);
                                enrollerPermissionMapping.LastName = dr["LastName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["LastName"]);
                                enrollerPermissionMapping.Permission = dr["PER_Name"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PER_Name"]);
                                enrollerPermissionMapping.TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0;
                                lstEnrollerPermissionsMappingContract.Add(enrollerPermissionMapping);
                            }
                        }
                    }
                    base.CloseSQLDataReaderConnection(con);
                }
                return lstEnrollerPermissionsMappingContract;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        List<Int32> IFingerPrintSetupRepository.GetEnrollerMappedWithLocation(Int32 locationId)
        {
            try
            {
                return _locationDBContext.EnrollerLocationPermissions.Where(cond => !cond.ELP_IsDeleted && cond.ELP_LocationID == locationId).Select(sel => sel.ELP_OrganizationUserID).ToList();
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        Boolean IFingerPrintSetupRepository.DeleteEnrollerMapping(Int32 currentLoggedInUserID, Int32 enrollerPermissionMappingId)
        {
            try
            {
                EnrollerLocationPermission EnrollerMapping = _locationDBContext.EnrollerLocationPermissions.Where(cond => cond.ELP_ID == enrollerPermissionMappingId && !cond.ELP_IsDeleted).FirstOrDefault();
                if (!EnrollerMapping.IsNullOrEmpty())
                {
                    EnrollerMapping.ELP_IsDeleted = true;
                    EnrollerMapping.ELP_ModifiedByID = currentLoggedInUserID;
                    EnrollerMapping.ELP_ModifiedOn = DateTime.Now;
                }
                if (_locationDBContext.SaveChanges() > AppConsts.NONE)
                    return true;
                return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region AppointmentScheduleSetUp

        List<AppointmentContract> IFingerPrintSetupRepository.GetFingerPrintAppointmentScheduleData(Int32 locationID, Int32 ScheduleMasterID)
        {
            List<AppointmentContract> lstAppointmentContract = new List<AppointmentContract>();

            if (locationID.IsNotNull() && (ScheduleMasterID.IsNull() || ScheduleMasterID == AppConsts.NONE))
            {
                FingerPrintLocation location = _locationDBContext.FingerPrintLocations.Where(con => con.FPL_ID == locationID).FirstOrDefault();
                ScheduleMasterID = location.FPL_ScheduleMasterID.HasValue ? location.FPL_ScheduleMasterID.Value : AppConsts.NONE;
            }

            if (ScheduleMasterID.IsNotNull() && ScheduleMasterID > AppConsts.NONE)
            {
                List<Schedule> schedules = _locationDBContext.Schedules.Where(con => !con.S_IsDeleted && con.S_ScheduleMasterID == ScheduleMasterID).ToList();
                lstAppointmentContract = schedules.Select(x => new AppointmentContract
                {
                    Description = "",
                    End = x.S_EndTime,
                    ID = x.S_ID,
                    RecurrenceParentID = x.S_RecurrenceParentID,
                    RecurrenceRule = x.S_RecurrenceRule,
                    RecurrenceState = x.S_RecurrenceState,
                    Start = x.S_StartTime,
                    Subject = x.S_Increment + " Minute(s) X " + x.S_TotalAppointment,
                    //Increment = x.S_Increment.Value,
                    Increment = x.S_Increment,
                    TotalAppointment = x.S_TotalAppointment,
                    OldScheduleMasterID = x.S_ScheduleMasterID,
                }).ToList();

            }

            return lstAppointmentContract;
        }
        Int32 IFingerPrintSetupRepository.ScheduleAppointment(AppointmentContract appointmentContract, Int32 currentLoggedInUserId)
        {
            if (!appointmentContract.IsNullOrEmpty())
            {

                EntityConnection connection = _locationDBContext.Connection as EntityConnection;

                SqlParameter outputParam = new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.Output };
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand command = new SqlCommand("ams.usp_CreateUpdateAppointmentSchedule", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", appointmentContract.ID);
                    command.Parameters.AddWithValue("@StartDate", appointmentContract.Start);
                    command.Parameters.AddWithValue("@EndDate", appointmentContract.End);
                    command.Parameters.AddWithValue("@RecurrenceRule", appointmentContract.RecurrenceRule);
                    command.Parameters.AddWithValue("@RecurrenceParentID", appointmentContract.RecurrenceParentID);
                    command.Parameters.AddWithValue("@RecurrenceState", appointmentContract.RecurrenceState);
                    command.Parameters.AddWithValue("@Increment", appointmentContract.Increment);
                    command.Parameters.AddWithValue("@TotalAppointment", appointmentContract.TotalAppointment);
                    command.Parameters.AddWithValue("@LocationID", appointmentContract.LocationID);
                    command.Parameters.AddWithValue("@OldScheduleMasterID", appointmentContract.OldScheduleMasterID);
                    command.Parameters.AddWithValue("@NewScheduleMasterID", appointmentContract.NewScheduleMasterID);
                    command.Parameters.AddWithValue("@CommandName", "Insert");
                    command.Parameters.AddWithValue("@IsDeleted", 0);
                    command.Parameters.AddWithValue("@CurrentLoggedInUserId", currentLoggedInUserId);
                    command.Parameters.Add(outputParam);
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    command.ExecuteNonQuery();
                    con.Close();
                }
                return Convert.ToInt32(outputParam.Value);
            }
            return 0;
        }

        Int32 IFingerPrintSetupRepository.UpdateScheduleAppointment(AppointmentContract appointmentContract, Int32 currentLoggedInUserId)
        {
            if (!appointmentContract.IsNullOrEmpty())
            {

                EntityConnection connection = _locationDBContext.Connection as EntityConnection;

                SqlParameter outputParam = new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.Output };
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand command = new SqlCommand("ams.usp_CreateUpdateAppointmentSchedule", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", appointmentContract.ID);
                    command.Parameters.AddWithValue("@StartDate", appointmentContract.Start);
                    command.Parameters.AddWithValue("@EndDate", appointmentContract.End);
                    command.Parameters.AddWithValue("@RecurrenceRule", appointmentContract.RecurrenceRule);
                    command.Parameters.AddWithValue("@RecurrenceParentID", appointmentContract.RecurrenceParentID);
                    command.Parameters.AddWithValue("@RecurrenceState", appointmentContract.RecurrenceState);
                    command.Parameters.AddWithValue("@Increment", appointmentContract.Increment);
                    command.Parameters.AddWithValue("@TotalAppointment", appointmentContract.TotalAppointment);
                    command.Parameters.AddWithValue("@LocationID", appointmentContract.LocationID);
                    command.Parameters.AddWithValue("@OldScheduleMasterID", appointmentContract.OldScheduleMasterID);
                    command.Parameters.AddWithValue("@NewScheduleMasterID", appointmentContract.NewScheduleMasterID);
                    command.Parameters.AddWithValue("@CommandName", "Update");
                    command.Parameters.AddWithValue("@IsDeleted", 0);
                    command.Parameters.AddWithValue("@CurrentLoggedInUserId", currentLoggedInUserId);
                    command.Parameters.AddWithValue("@IsExceptionEdit", appointmentContract.IsExceptionEdit);
                    command.Parameters.Add(outputParam);
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    command.ExecuteNonQuery();
                    con.Close();

                }
                return Convert.ToInt32(outputParam.Value);
            }
            return 0;
        }

        Int32 IFingerPrintSetupRepository.DeleteScheduleAppointment(AppointmentContract appointmentContract, Int32 currentLoggedInUserID)
        {
            if (!appointmentContract.IsNullOrEmpty())
            {

                EntityConnection connection = _locationDBContext.Connection as EntityConnection;

                SqlParameter outputParam = new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.Output };
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand command = new SqlCommand("ams.usp_CreateUpdateAppointmentSchedule", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", appointmentContract.ID);
                    command.Parameters.AddWithValue("@StartDate", appointmentContract.Start);
                    command.Parameters.AddWithValue("@EndDate", appointmentContract.End);
                    command.Parameters.AddWithValue("@RecurrenceRule", appointmentContract.RecurrenceRule);
                    command.Parameters.AddWithValue("@RecurrenceParentID", appointmentContract.RecurrenceParentID);
                    command.Parameters.AddWithValue("@RecurrenceState", appointmentContract.RecurrenceState);
                    command.Parameters.AddWithValue("@Increment", appointmentContract.Increment);
                    command.Parameters.AddWithValue("@TotalAppointment", appointmentContract.TotalAppointment);
                    command.Parameters.AddWithValue("@LocationID", appointmentContract.LocationID);
                    command.Parameters.AddWithValue("@OldScheduleMasterID", appointmentContract.OldScheduleMasterID);
                    command.Parameters.AddWithValue("@NewScheduleMasterID", appointmentContract.NewScheduleMasterID);
                    command.Parameters.AddWithValue("@CommandName", "Delete");
                    command.Parameters.AddWithValue("@IsDeleted", 1);
                    command.Parameters.AddWithValue("@CurrentLoggedInUserId", currentLoggedInUserID);
                    command.Parameters.AddWithValue("@IsExceptionEdit", appointmentContract.IsExceptionEdit);
                    command.Parameters.Add(outputParam);
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    command.ExecuteNonQuery();
                    con.Close();

                }
                return Convert.ToInt32(outputParam.Value);
            }
            return 0;
        }

        String IFingerPrintSetupRepository.CommitScheduleAppointment(Int32 LocationID, Int32 ScheduleMasterID, Int32 NewScheduleMasterID, Int32 CurrentLoggedInUserID)
        {
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;

            SqlParameter outputParam = new SqlParameter("@ErrorMessageOut", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_AppointmentScheduleSlotValidation", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LocationID", LocationID);
                command.Parameters.AddWithValue("@OldScheduleMasterID", ScheduleMasterID);
                command.Parameters.AddWithValue("@NewScheduleMasterID", NewScheduleMasterID);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", CurrentLoggedInUserID);
                command.Parameters.Add(outputParam);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }
            return Convert.ToString(outputParam.Value);
        }

        void IFingerPrintSetupRepository.DiscardScheduleAppointment(Int32 NewScheduleMasterID)
        {
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_DiscardAppointmentSchedules", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@NewScheduleMasterID", NewScheduleMasterID);

                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }
        }

        void IFingerPrintSetupRepository.CallDigestionProcedure(Dictionary<String, Object> param)
        {

            //call digestion SP
            String NewScheduleMasterID;
            Int32 CurrentLoggedInUserID;
            String oldScheduleMasterId;
            param.TryGetValue("ScheduleMasterID", out NewScheduleMasterID);
            param.TryGetValue("CurrentLoggedInUserID", out CurrentLoggedInUserID);
            param.TryGetValue("oldScheduleMasterId", out oldScheduleMasterId);

            EntityConnection connection = _locationDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_AppointmentScheduleDigestion", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ScheduleMasterID", NewScheduleMasterID);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", CurrentLoggedInUserID);
                command.Parameters.AddWithValue("@oldScheduleMasterId", oldScheduleMasterId);
                //command.CommandTimeout = 300;
                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }
        }


        void IFingerPrintSetupRepository.CallBackgroundDigestionProcedure(Int32 backgroundProcessUserId, String ScheduleMasterIds)
        {

            //call digestion SP


            EntityConnection connection = _locationDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_BkgAppointmentScheduleDigestion", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", backgroundProcessUserId);
                command.Parameters.AddWithValue("@ScheduleMasterIds", ScheduleMasterIds);

                if (!ConfigurationManager.AppSettings["CommandTimeout"].IsNullOrEmpty())
                {
                    command.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);
                }
                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }
        }

        List<Int32> IFingerPrintSetupRepository.GetLocationMasterSchedules()
        {
            return _locationDBContext.FingerPrintLocations.Where(a => !a.FPL_IsDeleted && a.FPL_ScheduleMasterID.HasValue).Select(a => a.FPL_ScheduleMasterID.Value).ToList();

        }

        #endregion

        AppointmentSlotContract IFingerPrintSetupRepository.GetAppointmentScheduleBySlotID(Int32 slotId)
        {
            AppointmentSlotContract appointmentSlotContract = new AppointmentSlotContract();
            FingerPrintScheduleSlot fingerPrintScheduleSlot = _locationDBContext.FingerPrintScheduleSlots.Where(con => con.FPSS_SlotID == slotId && !con.FPSS_IsDeleted).FirstOrDefault();
            if (!fingerPrintScheduleSlot.IsNullOrEmpty())
            {
                appointmentSlotContract.SlotID = fingerPrintScheduleSlot.FPSS_SlotID;
                appointmentSlotContract.SlotDate = fingerPrintScheduleSlot.FPSS_SlotDate;
                appointmentSlotContract.SlotStartTime = Convert.ToString(fingerPrintScheduleSlot.FPSS_SlotStartTime);
                appointmentSlotContract.SlotEndTime = Convert.ToString(fingerPrintScheduleSlot.FPSS_SlotEndTime);
            }
            return appointmentSlotContract;
        }
        #region Event Scheduler UAT-3849


        AppointmentSlotContract IFingerPrintSetupRepository.GetEventAppointmentScheduleBySlotID(Int32 slotId)
        {
            var appointmentSlotContract = new AppointmentSlotContract();
            FingerPrintScheduleSlot fingerPrintScheduleSlot = _locationDBContext.FingerPrintScheduleSlots.Where(con => con.FPSS_SlotID == slotId && !con.FPSS_IsDeleted).FirstOrDefault();
            if (!fingerPrintScheduleSlot.IsNullOrEmpty())
            {
                appointmentSlotContract.SlotID = fingerPrintScheduleSlot.FPSS_SlotID;
                appointmentSlotContract.SlotDate = fingerPrintScheduleSlot.FPSS_SlotDate;
                appointmentSlotContract.SlotStartTime = Convert.ToString(fingerPrintScheduleSlot.FPSS_SlotStartTime);
                appointmentSlotContract.SlotEndTime = Convert.ToString(fingerPrintScheduleSlot.FPSS_SlotEndTime);
            }
            return appointmentSlotContract;
        }

        List<AppointmentSlotContract> IFingerPrintSetupRepository.GetEventAppointmentSlotsAvailable(Int32 EventSlotId)
        {
            List<AppointmentSlotContract> lstAppointmentSlotContract = new List<AppointmentSlotContract>();

            EntityConnection connection = this._locationDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@EventSlotId", EventSlotId),
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetAvailableEventAppointmentSchedule", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            AppointmentSlotContract eventAppointmentSlotContract = new AppointmentSlotContract();
                            eventAppointmentSlotContract.SlotID = dr["FPSS_SlotID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FPSS_SlotID"]);
                            eventAppointmentSlotContract.SlotDate = dr["FPSS_SlotDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FPSS_SlotDate"]);
                            eventAppointmentSlotContract.SlotStartTime = dr["FPSS_SlotStartTime"] == DBNull.Value ? null : Convert.ToString(dr["FPSS_SlotStartTime"]);
                            eventAppointmentSlotContract.SlotEndTime = dr["FPSS_SlotEndTime"] == DBNull.Value ? null : Convert.ToString(dr["FPSS_SlotEndTime"]);
                            eventAppointmentSlotContract.SlotAppointment = dr["FPSS_SlotAppointment"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FPSS_SlotAppointment"]);
                            eventAppointmentSlotContract.IsAvailable = Convert.ToBoolean(dr["IsAvailable"]);
                            lstAppointmentSlotContract.Add(eventAppointmentSlotContract);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstAppointmentSlotContract;
        }
        #endregion

        List<AppointmentSlotContract> IFingerPrintSetupRepository.GetAppointmentSlotsAvailable(Int32 locationId, Boolean IsApplicant)
        {
            List<AppointmentSlotContract> lstAppointmentSlotContract = new List<AppointmentSlotContract>();

            EntityConnection connection = this._locationDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@LocationId", locationId),
                    new SqlParameter("@IsApplicant",IsApplicant)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetAvailableAppointmentSchedule", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            AppointmentSlotContract appointmentSlotContract = new AppointmentSlotContract();
                            appointmentSlotContract.SlotID = dr["FPSS_SlotID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FPSS_SlotID"]);
                            appointmentSlotContract.SlotDate = dr["FPSS_SlotDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FPSS_SlotDate"]);
                            appointmentSlotContract.SlotStartTime = dr["FPSS_SlotStartTime"] == DBNull.Value ? null : Convert.ToString(dr["FPSS_SlotStartTime"]);
                            appointmentSlotContract.SlotEndTime = dr["FPSS_SlotEndTime"] == DBNull.Value ? null : Convert.ToString(dr["FPSS_SlotEndTime"]);
                            appointmentSlotContract.SlotAppointment = dr["FPSS_SlotAppointment"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FPSS_SlotAppointment"]);
                            appointmentSlotContract.LocationId = dr["FPL_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FPL_ID"]);
                            appointmentSlotContract.IsAvailable = Convert.ToBoolean(dr["IsAvailable"]);
                            lstAppointmentSlotContract.Add(appointmentSlotContract);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstAppointmentSlotContract;
        }

        List<AppointmentOrderScheduleContract> IFingerPrintSetupRepository.GetAppointmentOrders(Boolean IsAdminLogin, Int32 UserId, AppointmentOrderScheduleContract filterContract, CustomPagingArgsContract GridCustomPagingArgs, String TenantIds)
        {
            List<AppointmentOrderScheduleContract> appointmentOrderLst = new List<AppointmentOrderScheduleContract>();
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;

            String orderBy = "OrderDate";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(GridCustomPagingArgs.SortExpression) ? orderBy : GridCustomPagingArgs.SortExpression;
            ordDirection = !GridCustomPagingArgs.SortDirectionDescending && !GridCustomPagingArgs.SortExpression.IsNullOrEmpty() ? "asc" : "desc";



            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_FingerprintAppointmentOrderList", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrgUserID", UserId);
                command.Parameters.AddWithValue("@IsAdminLogin", IsAdminLogin);
                command.Parameters.AddWithValue("@FirstName", filterContract != null ? filterContract.AppFirstNameFilter : string.Empty);
                command.Parameters.AddWithValue("@LastName", filterContract != null ? filterContract.AppLastNameFilter : string.Empty);
                command.Parameters.AddWithValue("@OrderNumber", filterContract != null ? filterContract.OrderIdFilter : string.Empty);
                command.Parameters.AddWithValue("@CBIPCNNumber", filterContract != null ? filterContract.CBIPCNNumber : string.Empty);
                command.Parameters.AddWithValue("@AppointmentDateFrom", filterContract != null ? filterContract.AppointmentDateFrom : null);
                command.Parameters.AddWithValue("@AppointmentDateTo", filterContract != null ? filterContract.AppointmentDateTo : null);
                command.Parameters.AddWithValue("@LocationIds", filterContract != null ? filterContract.LocationIds : null);
                command.Parameters.AddWithValue("@FingerPrintingSite", filterContract != null ? filterContract.FingerPrintingLocation : AppConsts.NONE);
                command.Parameters.AddWithValue("@AppointmentStatusIds", filterContract != null ? filterContract.AppointmentStatusIds : null);
                command.Parameters.AddWithValue("@OrderBy", orderBy.IsNullOrEmpty() ? null : orderBy);
                command.Parameters.AddWithValue("@OrderDirection", ordDirection.IsNullOrEmpty() ? null : ordDirection);
                command.Parameters.AddWithValue("@PageSize", GridCustomPagingArgs.PageSize);
                command.Parameters.AddWithValue("@PageIndex", GridCustomPagingArgs.CurrentPageIndex);
                command.Parameters.AddWithValue("@TenantIDs", TenantIds.IsNullOrEmpty() ? null : TenantIds);

                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    AppointmentOrderScheduleContract AppointmentOrder = new AppointmentOrderScheduleContract();
                    AppointmentOrder.AppointmentDate = dr["AppointmentDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["AppointmentDate"]);
                    AppointmentOrder.EndTime = dr["EndTime"] == DBNull.Value ? null : Convert.ToString(dr["EndTime"]);
                    AppointmentOrder.FirstName = dr["FirstName"] == DBNull.Value ? null : Convert.ToString(dr["FirstName"]);
                    AppointmentOrder.LastName = dr["LastName"] == DBNull.Value ? null : Convert.ToString(dr["LastName"]);
                    AppointmentOrder.LocationDescription = dr["LocationDescription"] == DBNull.Value ? null : Convert.ToString(dr["LocationDescription"]);
                    AppointmentOrder.LocationId = dr["LocationId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["LocationId"]);
                    AppointmentOrder.LocationName = dr["LocationName"] == DBNull.Value ? null : Convert.ToString(dr["LocationName"]);
                    AppointmentOrder.OrderId = dr["OrderId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrderId"]);
                    AppointmentOrder.OrderNumber = dr["OrderNumber"] == DBNull.Value ? null : Convert.ToString(dr["OrderNumber"]);
                    AppointmentOrder.SlotId = dr["SlotId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SlotId"]);
                    AppointmentOrder.StartTime = dr["StartTime"] == DBNull.Value ? null : Convert.ToString(dr["StartTime"]);
                    AppointmentOrder.ApplicantAppointmentId = dr["ApplicantAppointmentId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ApplicantAppointmentId"]);
                    AppointmentOrder.ApplicantOrgUserId = dr["OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrganizationUserID"]);
                    AppointmentOrder.PermissionName = dr["PermissionName"] == DBNull.Value ? null : Convert.ToString(dr["PermissionName"]);
                    AppointmentOrder.TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0;
                    AppointmentOrder.TenantID = dr["TenantID"] != DBNull.Value ? Convert.ToString(dr["TenantID"]) : null;
                    AppointmentOrder.PackageName = dr["PackageName"] != DBNull.Value ? Convert.ToString(dr["PackageName"]) : null;
                    AppointmentOrder.LocationAddress = Convert.ToString(dr["LocationAddress"]);
                    AppointmentOrder.HeirarchyNodeId = dr["HeirarchyNodeId"] != DBNull.Value ? Convert.ToInt32(dr["HeirarchyNodeId"]) : 0;
                    AppointmentOrder.ApplicantEmail = Convert.ToString(dr["ApplicantEmail"]);
                    AppointmentOrder.OrderDate = Convert.ToDateTime(dr["OrderDate"]);
                    AppointmentOrder.TotalOrderPrice = dr["TotalOrderPrice"] != DBNull.Value ? Convert.ToDecimal(dr["TotalOrderPrice"]) : 0;
                    AppointmentOrder.PaymentStatus = Convert.ToString(dr["PaymentStatus"]);
                    AppointmentOrder.OrderStatus = Convert.ToString(dr["OrderStatus"]);
                    AppointmentOrder.IsContractLocation = Convert.ToBoolean(dr["IsContractLocation"]);
                    AppointmentOrder.FingerPrintingSite = Convert.ToString(dr["FingerPrintingSite"]);
                    AppointmentOrder.CBIPCNNumber = Convert.ToString(dr["CBIPCNNumber"]);
                    AppointmentOrder.OrderStatusCode = dr["OrderStatusCode"].ToString();
                    AppointmentOrder.AppointmentStatusCode = dr["AppointmentStatusCode"].ToString();
                    // AppointmentOrder.AppointmentStatus = dr["AppointmentStatus"] == DBNull.Value ? null : Convert.ToString(dr["AppointmentStatus"]);
                    // AppointmentOrder.AppointmentStatus = dr["AppointmentStatus"] == DBNull.Value ? null : Convert.ToString(dr["AppointmentStatus"]);
                    appointmentOrderLst.Add(AppointmentOrder);
                }
                con.Close();
            }
            return appointmentOrderLst;
        }        

        Boolean IFingerPrintSetupRepository.SaveUpdateApplicantAppointment(AppointmentSlotContract scheduleAppointmentContract, Int32 currentLoggedInUserID, out Int32 oldStatusId)
        {
            FingerPrintApplicantAppointment fingerPrintApplicantAppointment = new FingerPrintApplicantAppointment();
            oldStatusId = 0;
            lkpAppointmentStatu AppointmentStatus = _locationDBContext.lkpAppointmentStatus.Where(x => x.AS_Code == "AAAA").FirstOrDefault();

            if (!scheduleAppointmentContract.ApplicantAppointmentId.IsNullOrEmpty() && scheduleAppointmentContract.ApplicantAppointmentId > AppConsts.NONE)
            {
                //update case
                fingerPrintApplicantAppointment = _locationDBContext.FingerPrintApplicantAppointments.Where(cond => !cond.FPAA_IsDeleted && cond.FPAA_ID == scheduleAppointmentContract.ApplicantAppointmentId)
                    .FirstOrDefault();
                oldStatusId = fingerPrintApplicantAppointment.FPAA_Status.HasValue ? fingerPrintApplicantAppointment.FPAA_Status.Value : 0;
                bool IsFPAAStatusNotUpdate = IsCabsserviceInProgress(oldStatusId);
                if (scheduleAppointmentContract.IsLocationUpdate
                    && scheduleAppointmentContract.LocationId > 0)
                {
                    fingerPrintApplicantAppointment.FPAA_LocationID = scheduleAppointmentContract.LocationId;
                }
                fingerPrintApplicantAppointment.FPAA_IsOnsiteAppointment = scheduleAppointmentContract.IsEventType;
                fingerPrintApplicantAppointment.FPAA_ScheduleSlotID = scheduleAppointmentContract.SlotID;
                fingerPrintApplicantAppointment.FPAA_Status = IsFPAAStatusNotUpdate ? oldStatusId : AppointmentStatus.AS_ID;
                fingerPrintApplicantAppointment.FPAA_ModifiedBy = currentLoggedInUserID;
                fingerPrintApplicantAppointment.FPAA_ModifiedOn = DateTime.Now;
            }
            else
            {
                //Save case
                fingerPrintApplicantAppointment.FPAA_OrganizationUserID = scheduleAppointmentContract.ApplicantOrgUserId;
                fingerPrintApplicantAppointment.FPAA_OrderID = scheduleAppointmentContract.OrderId;
                fingerPrintApplicantAppointment.FPAA_LocationID = scheduleAppointmentContract.LocationId;
                fingerPrintApplicantAppointment.FPAA_ScheduleSlotID = scheduleAppointmentContract.SlotID;
                fingerPrintApplicantAppointment.FPAA_Status = AppointmentStatus.AS_ID;
                fingerPrintApplicantAppointment.FPAA_CreatedBy = currentLoggedInUserID;
                fingerPrintApplicantAppointment.FPAA_CreatedOn = DateTime.Now;

                _locationDBContext.FingerPrintApplicantAppointments.AddObject(fingerPrintApplicantAppointment);
            }
            if(scheduleAppointmentContract.ReservedSlotID > 0)
            {
                var reservedSlot = _locationDBContext.ReservedSlots.FirstOrDefault(rs => rs.RS_ID == scheduleAppointmentContract.ReservedSlotID
                && !rs.RS_IsDeleted);
                if(reservedSlot != null)
                {
                    reservedSlot.RS_IsDeleted = true;
                    reservedSlot.RS_ModifiedBy = currentLoggedInUserID;
                    reservedSlot.RS_ModifiedOn = DateTime.Now;
                }
            }

            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        Boolean IFingerPrintSetupRepository.ResetOutOfStateApplicantAppointment(AppointmentSlotContract scheduleAppointmentContract, Int32 currentLoggedInUserID, out Int32 oldStatusId)
        {
            FingerPrintApplicantAppointment fingerPrintApplicantAppointment = new FingerPrintApplicantAppointment();
            oldStatusId = 0;
            lkpAppointmentStatu AppointmentStatus = _locationDBContext.lkpAppointmentStatus.Where(x => x.AS_Code == "AAAA").FirstOrDefault();

            if (scheduleAppointmentContract.IsOutOfStateAppointment)
            {
                var fileErrorStatus = FingerPrintAppointmentStatus.FINGERPRINT_FILE_ERROR.GetStringValue();
                var fileRejectedStatus = FingerPrintAppointmentStatus.FINGERPRINT_FILE_REJECTED.GetStringValue();
                var cbiFileRejectedStatus = FingerPrintAppointmentStatus.CBI_FINGERPRINT_FILE_REJECTED.GetStringValue();
                var manuallyRejectedStatus = FingerPrintAppointmentStatus.MANUALLY_REJECTED_ORDER.GetStringValue();
                var errorStatus = _locationDBContext.lkpAppointmentStatus.Where(x => x.AS_Code == fileErrorStatus
                || x.AS_Code == fileRejectedStatus || x.AS_Code == cbiFileRejectedStatus || x.AS_Code == manuallyRejectedStatus).Select(s => s.AS_ID).ToList();
                //update case
                fingerPrintApplicantAppointment = _locationDBContext.FingerPrintApplicantAppointments
                    .Where(cond => !cond.FPAA_IsDeleted
                    && cond.FPAA_OrderID == scheduleAppointmentContract.OrderId
                    && errorStatus.Any(e => e == cond.FPAA_Status)
                    && cond.FPAA_IsOutOfState)
                    .FirstOrDefault();
                if (fingerPrintApplicantAppointment != null)
                {
                    oldStatusId = fingerPrintApplicantAppointment.FPAA_Status.HasValue ? fingerPrintApplicantAppointment.FPAA_Status.Value : 0;

                    fingerPrintApplicantAppointment.FPAA_Status = AppointmentStatus.AS_ID;
                    fingerPrintApplicantAppointment.FPAA_ModifiedBy = currentLoggedInUserID;
                    fingerPrintApplicantAppointment.FPAA_ModifiedOn = DateTime.Now;
                    if (_locationDBContext.SaveChanges() > AppConsts.NONE)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public IQueryable<Entity.LocationEntity.lkpPaymentOption> GetAllPaymentOption()
        {
            return _locationDBContext.lkpPaymentOptions.Where(cond => !cond.PO_IsDeleted);
        }

        public IQueryable<Entity.LocationEntity.LocationPaymentOption> GetMappedLoactionPaymentOption(Int32 SelectedLocationID)
        {
            return _locationDBContext.LocationPaymentOptions.Where(cond => cond.LPO_LocationID == SelectedLocationID && !cond.LPO_IsDeleted);
        }

        /// <summary>
        /// To save mapped Payment Options
        /// 
        public Boolean SaveMappedPaymentOptions(Int32 SelectedLocationID, List<Int32> paymentOptions, Int32 currentUserId)
        {

            LocationPaymentOption tempPaymentOption = null;
            List<LocationPaymentOption> selectedPaymentOptionList = new List<LocationPaymentOption>();

            var _locPaymentMapping = _locationDBContext.LocationPaymentOptions.Where(dpm => dpm.LPO_LocationID == SelectedLocationID).FirstOrDefault();


            if (!_locPaymentMapping.IsNullOrEmpty())
            {

                _locPaymentMapping.LPO_ModifiedBy = currentUserId;
                _locPaymentMapping.LPO_ModifiedOn = DateTime.Now;


            }

            //Get list of selected mapped payment options
            foreach (var paymentOptionID in paymentOptions)
            {
                tempPaymentOption = new LocationPaymentOption();
                tempPaymentOption.LPO_LocationID = SelectedLocationID;
                tempPaymentOption.LPO_PaymentOptionID = paymentOptionID;
                tempPaymentOption.LPO_IsDeleted = false;
                tempPaymentOption.LPO_CreatedOn = DateTime.Now;
                tempPaymentOption.LPO_CreatedBy = currentUserId;
                selectedPaymentOptionList.Add(tempPaymentOption);
            }

            List<LocationPaymentOption> mapPaymentoptionList = _locationDBContext.LocationPaymentOptions.Where(cond => cond.LPO_LocationID == SelectedLocationID && !cond.LPO_IsDeleted).ToList();
            List<LocationPaymentOption> paymentOptionsToDelete = mapPaymentoptionList.Where(x => !selectedPaymentOptionList.Any(cnd => cnd.LPO_PaymentOptionID == x.LPO_PaymentOptionID)).ToList();
            List<LocationPaymentOption> paymentOptionsToInsert = selectedPaymentOptionList.Where(y => !mapPaymentoptionList.Any(cd => cd.LPO_PaymentOptionID == y.LPO_PaymentOptionID)).ToList();

            //To delete already saved Payment Options
            paymentOptionsToDelete.ForEach(cond =>
            {
                cond.LPO_IsDeleted = true;
                cond.LPO_ModifiedBy = currentUserId;
                cond.LPO_ModifiedOn = DateTime.Now;
            });

            //To insert Payment Options
            paymentOptionsToInsert.ForEach(con =>
            {
                _locationDBContext.LocationPaymentOptions.AddObject(con);
            });

            if (_locationDBContext.SaveChanges() > 0)
            {

                return true;
            }

            return false;
        }


        ManageEnrollerMappingContract IFingerPrintSetupRepository.GetEnrollerPermission(Int32 orgUserId, Int32 selectedLocationID)
        {
            ManageEnrollerMappingContract enrollerPermission = new ManageEnrollerMappingContract();

            enrollerPermission = _locationDBContext.EnrollerLocationPermissions.Where(cond => !cond.ELP_IsDeleted && cond.ELP_LocationID == selectedLocationID && cond.ELP_OrganizationUserID == orgUserId)
                .Select(x => new ManageEnrollerMappingContract
                {
                    EnrollerMappingID = x.ELP_ID,
                    OrganizationUserID = x.ELP_OrganizationUserID,
                    PermissionCode = x.lkpPermission.PER_Code,
                    PermissionId = x.ELP_PermissionID,
                    Permission = x.lkpPermission.PER_Name
                }).FirstOrDefault();

            return enrollerPermission;
        }


        Boolean IFingerPrintSetupRepository.IsLocationInUse(int TenantId, List<Int32> lstLocationIds)
        {
            return _locationDBContext.FingerPrintApplicantAppointments.Where(a => a.FPAA_TenantID == TenantId && lstLocationIds.Contains(a.FPAA_LocationID) && a.FPAA_IsDeleted == false).Any();

        }
        Boolean IFingerPrintSetupRepository.IsAnyScheduleForLocation(Int32 selectedLocationID)
        {
            var currentDate = DateTime.Now.Date;
            //return _locationDBContext.FingerPrintApplicantAppointments.Where(cond => cond.FPAA_LocationID == selectedLocationID && !cond.FPAA_IsDeleted && cond.FingerPrintScheduleSlot.FPSS_SlotDate > currentDate).Any();
            return _locationDBContext.FingerPrintApplicantAppointments.Where(cond => cond.FPAA_LocationID == selectedLocationID && !cond.FPAA_IsDeleted).Any();
        }


        List<LocationContract> IFingerPrintSetupRepository.GetApplicantAvailableLocation(Int32 TenantId, string lng, string lat,String orderRequestType)
        {
            List<LocationContract> lstFingerprintLocContract = new List<LocationContract>();

            ///@todo: Need to Implement pagination
         
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                List<SqlParameter> sqlParameterCollection = new List<SqlParameter>
                        {
                            new SqlParameter("@TenantId",TenantId),
                            new SqlParameter("@lng",lng),
                            new SqlParameter("@lat",lat)

                        };


                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetApplicantAvailableLocation", sqlParameterCollection.ToArray()))
                {
                    int intParseResult;
                    if (dr.HasRows)
                    {
                        Guid guidParseResult = Guid.Empty;
                        while (dr.Read())
                        {
                            LocationContract locationContract = new LocationContract();
                            locationContract.LocationID = Convert.ToInt32(dr["FPL_ID"]);
                            locationContract.LocationName = dr["FPL_Name"].ToString();
                            locationContract.FullAddress = dr["FPL_FullAddress"].ToString();
                            locationContract.PlaceID = dr["FPL_PlaceID"].ToString();
                            locationContract.Longitude = dr["FPL_Longitude"].ToString();
                            locationContract.Latitude = dr["FPL_Latitude"].ToString();
                            locationContract.Zipcode = dr["FPL_ZipCode"].ToString();
                            locationContract.Description = dr["FPL_Description"].ToString();
                            locationContract.IsPrinterAvailable = Convert.ToBoolean(dr["FPL_IsPrinterAvailable"]);
                            locationContract.IsPhotoService = Convert.ToBoolean(dr["FPL_IsPassPhotoService"]);
                            if (int.TryParse(dr["FPL_ScheduleMasterID"].ToString(), out intParseResult))
                            {
                                locationContract.ScheduleMasterID = intParseResult;
                            }

                            lstFingerprintLocContract.Add(locationContract);
                        }
                    }
                }
            }
           

            return lstFingerprintLocContract;
        }







        //AppointmentSlotContract IFingerPrintSetupRepository.GetBkgOrderWithAppointmentData(Int32 tenantId, Int32 OrderId, Int32 ApplicantOrgUserID)
        //{
        //    return _locationDBContext.FingerPrintApplicantAppointments.Where(cond => !cond.FPAA_IsDeleted && cond.FPAA_TenantID == tenantId && cond.FPAA_OrganizationUserID == ApplicantOrgUserID
        //                  && cond.FPAA_OrderID == OrderId && !cond.FingerPrintLocation.FPL_IsDeleted && !cond.FingerPrintScheduleSlot.FPSS_IsDeleted).Select(sel => new AppointmentSlotContract
        //                  {
        //                      LocationId = sel.FingerPrintLocation.FPL_ID,
        //                      LocationName = sel.FingerPrintLocation.FPL_Name,
        //                      LocationAddress = sel.FingerPrintLocation.FPL_FullAddress,
        //                      SlotDate = sel.FingerPrintScheduleSlot.FPSS_SlotDate,
        //                      SlotID = sel.FPAA_ScheduleSlotID,
        //                      ApplicantAppointmentId = sel.FPAA_ID,
        //                      ApplicantOrgUserId = sel.FPAA_OrganizationUserID,
        //                      OrderId = sel.FPAA_OrderID,
        //                      SlotStartTimeTimeSpanFormat = sel.FingerPrintScheduleSlot.FPSS_SlotStartTime,
        //                      SlotEndTimeTimeSpanFormat = sel.FingerPrintScheduleSlot.FPSS_SlotEndTime
        //                  }).FirstOrDefault();
        //}

        Boolean IFingerPrintSetupRepository.IsBkgOrderWithAppointment(Int32 tenantId, Int32 orderId, Int32 applicantOrgUserId)
        {
            return _locationDBContext.FingerPrintApplicantAppointments.Where(cond =>
            !cond.FPAA_IsDeleted
            && cond.FPAA_TenantID == tenantId
            && cond.FPAA_OrganizationUserID == applicantOrgUserId
            && cond.FPAA_OrderID == orderId).Any();
        }

        AppointmentOrderScheduleContract IFingerPrintSetupRepository.GetAppointmentOrderDetailData(Int32 UserID, Boolean IsAdmin, String TenantId, Int32 ApplicantAppointmentId)
        {
            AppointmentOrderScheduleContract AppointmentOrder = new AppointmentOrderScheduleContract();
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                 SqlCommand command = new SqlCommand("ams.usp_FingerPrintApplicantAppointmentDetail", con);
               
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrganizationUserID", UserID);
                command.Parameters.AddWithValue("@IsAdmin", IsAdmin);
                command.Parameters.AddWithValue("@TenantID", TenantId);
                command.Parameters.AddWithValue("@ApplicantAppointmentID", Convert.ToString(ApplicantAppointmentId));
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    AppointmentOrder.TenantID = dr["TenantID"] != DBNull.Value ? Convert.ToString(dr["TenantID"]) : null;
                    AppointmentOrder.AppointmentDate = dr["AppointmentDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["AppointmentDate"]);
                    AppointmentOrder.EndTime = dr["EndTime"] == DBNull.Value ? null : Convert.ToString(dr["EndTime"]);
                    AppointmentOrder.StartTime = dr["StartTime"] == DBNull.Value ? null : Convert.ToString(dr["StartTime"]);
                    AppointmentOrder.FirstName = dr["FirstName"] == DBNull.Value ? null : Convert.ToString(dr["FirstName"]);
                    AppointmentOrder.LastName = dr["LastName"] == DBNull.Value ? null : Convert.ToString(dr["LastName"]);
                    AppointmentOrder.SlotId = dr["SlotId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SlotId"]);
                    AppointmentOrder.LocationDescription = dr["LocationDescription"] == DBNull.Value ? null : Convert.ToString(dr["LocationDescription"]);
                    AppointmentOrder.LocationName = dr["LocationName"] == DBNull.Value ? null : Convert.ToString(dr["LocationName"]);
                    AppointmentOrder.LocationId = dr["LocationId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["LocationId"]);
                    AppointmentOrder.OrderId = dr["OrderId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrderId"]);
                    AppointmentOrder.OrderNumber = dr["OrderNumber"] == DBNull.Value ? null : Convert.ToString(dr["OrderNumber"]);
                    AppointmentOrder.ApplicantOrgUserId = dr["OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrganizationUserID"]);
                    AppointmentOrder.LocationAddress = Convert.ToString(dr["LocationAddress"]);
                    AppointmentOrder.HeirarchyNodeId = dr["HeirarchyNodeId"] != DBNull.Value ? Convert.ToInt32(dr["HeirarchyNodeId"]) : 0;
                    AppointmentOrder.ApplicantEmail = Convert.ToString(dr["ApplicantEmail"]);
                    AppointmentOrder.TotalOrderPrice = dr["TotalOrderPrice"] != DBNull.Value ? Convert.ToDecimal(dr["TotalOrderPrice"]) : 0;
                    AppointmentOrder.OrderDate = Convert.ToDateTime(dr["OrderDate"]);
                    AppointmentOrder.OrderStatus = Convert.ToString(dr["OrderStatus"]);
                    AppointmentOrder.PaymentStatus = Convert.ToString(dr["PaymentStatus"]);
                    AppointmentOrder.PaymentType = Convert.ToString(dr["PaymentType"]);
                    AppointmentOrder.PackageName = dr["PackageName"] != DBNull.Value ? Convert.ToString(dr["PackageName"]) : null;
                    AppointmentOrder.CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : (DateTime?)null;
                    AppointmentOrder.FingerPrintTech = Convert.ToString(dr["FingerPrintTech"]);
                    AppointmentOrder.CBI_FBI_Status = Convert.ToString(dr["CBI_FBI_Status"]);
                    // AppointmentOrder.OPD_ID = dr["OPD_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OPD_ID"]);
                    AppointmentOrder.PaymentStatusCode = dr["PaymentStatusCode"] != DBNull.Value ? Convert.ToString(dr["PaymentStatusCode"]) : null;
                    AppointmentOrder.FingerPrintDocumentId = dr["FingerPrintDocumentId"] != DBNull.Value ? Convert.ToInt32(dr["FingerPrintDocumentId"]) : 0;
                    AppointmentOrder.IsContractLocation = Convert.ToBoolean(dr["IsContractLocation"]);
                    AppointmentOrder.AppointmentStatus = dr["AppointmentStatus"] == DBNull.Value ? null : Convert.ToString(dr["AppointmentStatus"]);
                    AppointmentOrder.OrderStatusCode = dr["OrderStatusCode"] == DBNull.Value ? null : Convert.ToString(dr["OrderStatusCode"]);
                    AppointmentOrder.FingerPrintingSite = Convert.ToString(dr["FingerPrintingSite"]);
                    AppointmentOrder.AppointmentStatusCode = dr["AppointmentStatusCode"].ToString();
                    AppointmentOrder.CBIPCNNumber = dr["CBIPCNNumber"].ToString();
                    AppointmentOrder.ResonedFingerprinting = dr["ResonedFingerprinting"].ToString();
                    AppointmentOrder.ApplicantPhone = dr["ApplicantPhone"].ToString();
                    AppointmentOrder.CbiUniqueId = dr["CBIUniqueID"].ToString();
                    AppointmentOrder.ApplicantAppointmentDetailID = dr["ApplicantAppointmentDetailID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ApplicantAppointmentDetailID"]);
                }
                con.Close();
            }
            return AppointmentOrder;
        }

        AppointmentOrderScheduleContract IFingerPrintSetupRepository.GetAppointmentFulFillmentQueueOrderDetailData(Int32 UserID, Boolean IsAdmin, String TenantId, Int32 ApplicantAppointmentId)
        {
            AppointmentOrderScheduleContract AppointmentOrder = new AppointmentOrderScheduleContract();
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_FingerPrintFulfillmentDetail", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrganizationUserID", UserID);
                command.Parameters.AddWithValue("@IsAdmin", IsAdmin);
                command.Parameters.AddWithValue("@TenantID", TenantId);
                command.Parameters.AddWithValue("@ApplicantAppointmentID", Convert.ToString(ApplicantAppointmentId));
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    AppointmentOrder.TenantID = dr["TenantID"] != DBNull.Value ? Convert.ToString(dr["TenantID"]) : null;
                    AppointmentOrder.AppointmentDate = dr["AppointmentDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["AppointmentDate"]);
                    AppointmentOrder.EndTime = dr["EndTime"] == DBNull.Value ? null : Convert.ToString(dr["EndTime"]);
                    AppointmentOrder.StartTime = dr["StartTime"] == DBNull.Value ? null : Convert.ToString(dr["StartTime"]);
                    AppointmentOrder.FirstName = dr["FirstName"] == DBNull.Value ? null : Convert.ToString(dr["FirstName"]);
                    AppointmentOrder.LastName = dr["LastName"] == DBNull.Value ? null : Convert.ToString(dr["LastName"]);
                    AppointmentOrder.SlotId = dr["SlotId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SlotId"]);
                    AppointmentOrder.LocationDescription = dr["LocationDescription"] == DBNull.Value ? null : Convert.ToString(dr["LocationDescription"]);
                    AppointmentOrder.LocationName = dr["LocationName"] == DBNull.Value ? null : Convert.ToString(dr["LocationName"]);
                    AppointmentOrder.LocationId = dr["LocationId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["LocationId"]);
                    AppointmentOrder.OrderId = dr["OrderId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrderId"]);
                    AppointmentOrder.OrderNumber = dr["OrderNumber"] == DBNull.Value ? null : Convert.ToString(dr["OrderNumber"]);
                    AppointmentOrder.ApplicantOrgUserId = dr["OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrganizationUserID"]);
                    AppointmentOrder.LocationAddress = Convert.ToString(dr["LocationAddress"]);
                    AppointmentOrder.HeirarchyNodeId = dr["HeirarchyNodeId"] != DBNull.Value ? Convert.ToInt32(dr["HeirarchyNodeId"]) : 0;
                    AppointmentOrder.ApplicantEmail = Convert.ToString(dr["ApplicantEmail"]);
                    AppointmentOrder.TotalOrderPrice = dr["TotalOrderPrice"] != DBNull.Value ? Convert.ToDecimal(dr["TotalOrderPrice"]) : 0;
                    AppointmentOrder.OrderDate = Convert.ToDateTime(dr["OrderDate"]);
                    AppointmentOrder.OrderStatus = Convert.ToString(dr["OrderStatus"]);
                    AppointmentOrder.PaymentStatus = Convert.ToString(dr["PaymentStatus"]);
                    AppointmentOrder.PaymentType = Convert.ToString(dr["PaymentType"]);
                    AppointmentOrder.PackageName = dr["PackageName"] != DBNull.Value ? Convert.ToString(dr["PackageName"]) : null;
                    AppointmentOrder.CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : (DateTime?)null;
                    AppointmentOrder.FingerPrintTech = Convert.ToString(dr["FingerPrintTech"]);
                    AppointmentOrder.CBI_FBI_Status = Convert.ToString(dr["CBI_FBI_Status"]);
                    // AppointmentOrder.OPD_ID = dr["OPD_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OPD_ID"]);
                    AppointmentOrder.PaymentStatusCode = dr["PaymentStatusCode"] != DBNull.Value ? Convert.ToString(dr["PaymentStatusCode"]) : null;
                    AppointmentOrder.FingerPrintDocumentId = dr["FingerPrintDocumentId"] != DBNull.Value ? Convert.ToInt32(dr["FingerPrintDocumentId"]) : 0;
                    AppointmentOrder.IsContractLocation = Convert.ToBoolean(dr["IsContractLocation"]);
                    AppointmentOrder.AppointmentStatus = dr["AppointmentStatus"] == DBNull.Value ? null : Convert.ToString(dr["AppointmentStatus"]);
                    AppointmentOrder.OrderStatusCode = dr["OrderStatusCode"] == DBNull.Value ? null : Convert.ToString(dr["OrderStatusCode"]);
                    AppointmentOrder.FingerPrintingSite = Convert.ToString(dr["FingerPrintingSite"]);
                    AppointmentOrder.AppointmentStatusCode = dr["AppointmentStatusCode"].ToString();
                    AppointmentOrder.CBIPCNNumber = dr["CBIPCNNumber"].ToString();
                    AppointmentOrder.ABIPCNNumber = dr["ABIPCNNumber"].ToString();
                    AppointmentOrder.ResonedFingerprinting = dr["ResonedFingerprinting"].ToString();
                    AppointmentOrder.ApplicantPhone = dr["ApplicantPhone"].ToString();
                    AppointmentOrder.CbiUniqueId = dr["CBIUniqueID"].ToString();
                    AppointmentOrder.ApplicantAppointmentDetailID = dr["ApplicantAppointmentDetailID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ApplicantAppointmentDetailID"]);
                }
                con.Close();
            }
            return AppointmentOrder;
        }

        ReserveSlotContract IFingerPrintSetupRepository.ReserveSlot(Int32 reservedSlotID, Int32 selectedSlotID, Int32 currentLoggedInUserId)
        {
            ReserveSlotContract reserveSlotContract = new ReserveSlotContract();
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_CheckReservedSlot", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Slot_ID", selectedSlotID);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                command.Parameters.AddWithValue("@ReservedSlotID", reservedSlotID);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    reserveSlotContract.IsAvailable = dr["IsAvailable"] != DBNull.Value ? Convert.ToBoolean(dr["IsAvailable"]) : false;
                    reserveSlotContract.ReservedSlotID = dr["RS_ID"] != DBNull.Value ? Convert.ToInt32(dr["RS_ID"]) : AppConsts.NONE;
                }
                con.Close();
            }
            return reserveSlotContract;
        }

        public Boolean IsPrinterAvailableNewLoc(Int32 LocationId)
        {
            if(LocationId>0) {
                var fingerPrintLocationDetail = _locationDBContext.FingerPrintLocations.Where(y => y.FPL_ID== LocationId).FirstOrDefault();
                if (!fingerPrintLocationDetail.IsNullOrEmpty())
                {
                    return fingerPrintLocationDetail.FPL_IsPrinterAvailable;
                }
            }            
            return false;
        }

        public AppointmentSlotContract ReserveSlotForEventCodeType(Int32 reservedSlotID, Int32 selectedSlotID, Int32 currentLoggedInUserId)
        {
            AppointmentSlotContract AppointmentreserveSlotContract = new AppointmentSlotContract();
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_CheckReservedSlot", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Slot_ID", selectedSlotID);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                command.Parameters.AddWithValue("@ReservedSlotID", reservedSlotID);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    AppointmentreserveSlotContract.IsAvailable = dr["IsAvailable"] != DBNull.Value ? Convert.ToBoolean(dr["IsAvailable"]) : false;
                    AppointmentreserveSlotContract.ReservedSlotID = dr["RS_ID"] != DBNull.Value ? Convert.ToInt32(dr["RS_ID"]) : AppConsts.NONE;
                }
                con.Close();
            }
            return AppointmentreserveSlotContract;
        }
        public List<FingerprintLocationGroupContract> GetLocationGroupList(CustomPagingArgsContract GridCustomPagingArgs, FingerprintLocationGroupContract filterContract)
        {
            Int32 pagenumber = GridCustomPagingArgs.CurrentPageIndex - 1;
            Boolean nameFilterHasValue = filterContract.NameFilter.IsNullOrEmpty() ? false : true;
            var assignedLocationGroupIds = _locationDBContext.FingerPrintLocations.Where(x => !x.FPL_IsDeleted && x.FPL_LocationGroupID > AppConsts.NONE).Select(x => x.FPL_LocationGroupID).ToList();
            List<FingerprintLocationGroupContract> locationGroupList = new List<FingerprintLocationGroupContract>();
            locationGroupList = _locationDBContext.FingerprintLocationGroups.Where(x => !x.FLG_IsDeleted && (!nameFilterHasValue || x.FLG_Name.Contains(filterContract.NameFilter))).Select(x => new FingerprintLocationGroupContract()
            {
                LocationGroupID = x.FLG_ID,
                Name = x.FLG_Name,
                Description = x.FLG_Description,
                IsAssigned = assignedLocationGroupIds.Count > AppConsts.NONE && assignedLocationGroupIds.Contains(x.FLG_ID) ? true : false,
            }).ToList();
            if (locationGroupList.Count > AppConsts.NONE)
                locationGroupList.FirstOrDefault().TotalCount = locationGroupList.Count;
            //locationGroupList.Select(x => { x.TotalCount = locationGroupList.Count(); return x; });
            return locationGroupList.Skip(pagenumber * GridCustomPagingArgs.PageSize).Take(GridCustomPagingArgs.PageSize).ToList();

        }
        public Boolean SaveUpdateLocationGroup(FingerprintLocationGroupContract LocationGroupData, Int32 organisationUserId)
        {
            if (LocationGroupData.LocationGroupID > AppConsts.NONE)
            {
                //update code
                FingerprintLocationGroup fngrprntLocGrp = _locationDBContext.FingerprintLocationGroups.Where(x => x.FLG_ID == LocationGroupData.LocationGroupID).FirstOrDefault();
                fngrprntLocGrp.FLG_Name = LocationGroupData.Name;
                fngrprntLocGrp.FLG_Description = LocationGroupData.Description;
                fngrprntLocGrp.FLG_ModifiedBy = organisationUserId;
                fngrprntLocGrp.FLG_ModifiedOn = DateTime.Now;
            }
            else
            {
                //add Code
                FingerprintLocationGroup fngrprntLocGrp = new FingerprintLocationGroup();
                fngrprntLocGrp.FLG_Name = LocationGroupData.Name;
                fngrprntLocGrp.FLG_Description = LocationGroupData.Description;
                fngrprntLocGrp.FLG_CreatedBy = organisationUserId;
                fngrprntLocGrp.FLG_CreatedOn = DateTime.Now;
                _locationDBContext.FingerprintLocationGroups.AddObject(fngrprntLocGrp);
            }
            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            else
                return false;
        }
        public Boolean DeleteLocationGroup(Int32 LocationGroupId, Int32 OrganizationUserID)
        {
            // Add check for assigned groups
            if (_locationDBContext.FingerPrintLocations.Where(x => x.FPL_LocationGroupID == LocationGroupId && !x.FPL_IsDeleted).Count() > AppConsts.NONE)
            {
                return false;
            }
            FingerprintLocationGroup fngrprntLocGrp = _locationDBContext.FingerprintLocationGroups.Where(x => x.FLG_ID == LocationGroupId).FirstOrDefault();
            fngrprntLocGrp.FLG_IsDeleted = true;
            fngrprntLocGrp.FLG_ModifiedBy = OrganizationUserID;
            fngrprntLocGrp.FLG_ModifiedOn = DateTime.Now;
            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }
        public List<FingerprintLocationGroupContract> GetLocationGroupCompleteList()
        {
            List<FingerprintLocationGroupContract> locationGroupList = new List<FingerprintLocationGroupContract>();
            locationGroupList = _locationDBContext.FingerprintLocationGroups.Where(x => !x.FLG_IsDeleted).Select(x => new FingerprintLocationGroupContract()
            {
                LocationGroupID = x.FLG_ID,
                Name = x.FLG_Name,
                Description = x.FLG_Description,
            }).ToList();
            return locationGroupList;
        }
        public Boolean AddLocationImages(List<FingerPrintLocationImagesContract> AddedLocationImagesContract, Int32 LocationId, Int32 CurrentUserId)
        {
            //List<FingerPrintLocationImage> FingerPrintLocationImages = new List<FingerPrintLocationImage>();
            foreach (var ImageData in AddedLocationImagesContract)
            {
                FingerPrintLocationImage LocationImageData = new FingerPrintLocationImage();
                LocationImageData.FPLI_CreatedBy = CurrentUserId;
                LocationImageData.FPLI_CreatedOn = DateTime.Now;
                LocationImageData.FPLI_FileName = ImageData.FPLI_FileName;
                LocationImageData.FPLI_FilePath = ImageData.FPLI_FilePath;
                LocationImageData.FPLI_LocationId = LocationId;
                _locationDBContext.FingerPrintLocationImages.AddObject(LocationImageData);
            }
            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }
        public List<FingerPrintLocationImagesContract> GetLocationImages(CustomPagingArgsContract CustomGridArg, Int32 LocationId)
        {
            var LocationImagesData = _locationDBContext.FingerPrintLocationImages.Where(x => !x.FPLI_IsDeleted && x.FPLI_LocationId == LocationId).ToList();
            var totalCount = LocationImagesData.Count();
            List<FingerPrintLocationImagesContract> imageList = new List<FingerPrintLocationImagesContract>();
            if (CustomGridArg.PageSize > AppConsts.NONE)
            {
                imageList = LocationImagesData.Skip((CustomGridArg.CurrentPageIndex - 1) * CustomGridArg.PageSize).Take(CustomGridArg.PageSize).Select(x => new FingerPrintLocationImagesContract()
                {
                    FPLI_FileName = x.FPLI_FileName,
                    FPLI_FilePath = x.FPLI_FilePath,
                    FPLI_ID = x.FPLI_ID,
                    FPLI_LocationId = x.FPLI_LocationId,
                    TotalCount = totalCount
                }).ToList();
            }
            else
            {
                imageList = LocationImagesData.Select(x => new FingerPrintLocationImagesContract()
                {
                    FPLI_FileName = x.FPLI_FileName,
                    FPLI_FilePath = x.FPLI_FilePath,
                    FPLI_ID = x.FPLI_ID,
                    FPLI_LocationId = x.FPLI_LocationId,
                    TotalCount = totalCount
                }).ToList();
            }
            return imageList;
        }
        public Boolean DeleteLocationImage(Int32 ImageId, Int32 CurrentUserId)
        {
            FingerPrintLocationImage FPLocationImage = _locationDBContext.FingerPrintLocationImages.Where(x => x.FPLI_ID == ImageId).FirstOrDefault();
            FPLocationImage.FPLI_IsDeleted = true;
            FPLocationImage.FPLI_ModifiedBy = CurrentUserId;
            FPLocationImage.FPLI_ModifiedOn = DateTime.Now;
            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;

        }
        #region UAT-3734

        public List<AppointmentOrderScheduleContract> GetApplicantsMissedAppointments(String LocationTenantIds, Int32 chunkSize, Boolean IsMissedNotify)
        {
            List<AppointmentOrderScheduleContract> appointmentOrderLst = new List<AppointmentOrderScheduleContract>();
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetApplicantMissedAppointments", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantIDs", LocationTenantIds);
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                command.Parameters.AddWithValue("@IsMissedNotify", IsMissedNotify);

                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    AppointmentOrderScheduleContract AppointmentOrder = new AppointmentOrderScheduleContract();
                    AppointmentOrder.TenantID = dr["TenantID"] != DBNull.Value ? Convert.ToString(dr["TenantID"]) : null;
                    AppointmentOrder.AppointmentDate = dr["AppointmentDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["AppointmentDate"]);
                    AppointmentOrder.EndTime = dr["EndTime"] == DBNull.Value ? null : Convert.ToString(dr["EndTime"]);
                    AppointmentOrder.StartTime = dr["StartTime"] == DBNull.Value ? null : Convert.ToString(dr["StartTime"]);
                    AppointmentOrder.FirstName = dr["FirstName"] == DBNull.Value ? null : Convert.ToString(dr["FirstName"]);
                    AppointmentOrder.LastName = dr["LastName"] == DBNull.Value ? null : Convert.ToString(dr["LastName"]);
                    AppointmentOrder.SlotId = dr["SlotId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SlotId"]);
                    AppointmentOrder.LocationDescription = dr["LocationDescription"] == DBNull.Value ? null : Convert.ToString(dr["LocationDescription"]);
                    AppointmentOrder.LocationName = dr["LocationName"] == DBNull.Value ? null : Convert.ToString(dr["LocationName"]);
                    AppointmentOrder.LocationId = dr["LocationId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["LocationId"]);
                    AppointmentOrder.OrderId = dr["OrderId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrderId"]);
                    AppointmentOrder.OrderNumber = dr["OrderNumber"] == DBNull.Value ? null : Convert.ToString(dr["OrderNumber"]);
                    AppointmentOrder.ApplicantAppointmentId = dr["ApplicantAppointmentId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ApplicantAppointmentId"]);
                    AppointmentOrder.ApplicantOrgUserId = dr["OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrganizationUserID"]);
                    AppointmentOrder.LocationAddress = Convert.ToString(dr["LocationAddress"]);
                    AppointmentOrder.HeirarchyNodeId = dr["HeirarchyNodeId"] != DBNull.Value ? Convert.ToInt32(dr["HeirarchyNodeId"]) : 0;
                    AppointmentOrder.ApplicantEmail = Convert.ToString(dr["ApplicantEmail"]);
                    AppointmentOrder.TotalOrderPrice = dr["TotalOrderPrice"] != DBNull.Value ? Convert.ToDecimal(dr["TotalOrderPrice"]) : 0;
                    AppointmentOrder.OrderDate = Convert.ToDateTime(dr["OrderDate"]);
                    AppointmentOrder.OrderStatus = Convert.ToString(dr["OrderStatus"]);
                    AppointmentOrder.IsOnsiteAppointment = dr["IsOnsiteAppointment"] != DBNull.Value ? Convert.ToBoolean(dr["IsOnsiteAppointment"]) : false;
                    AppointmentOrder.IsOutOfStateAppointment = dr["IsOutOfStateAppointment"] != DBNull.Value ? Convert.ToBoolean(dr["IsOutOfStateAppointment"]) : false;
                    appointmentOrderLst.Add(AppointmentOrder);
                }
                con.Close();
            }
            return appointmentOrderLst;
        }
        public Boolean UpdateAppointmentStatus(Int32 ApplicantAppointmentID, String StatusCode, Int32 BackgroundProcessId, out Int32 OldStatusId)
        {
            OldStatusId = 0;
            lkpAppointmentStatu AppointmentStatus = _locationDBContext.lkpAppointmentStatus.Where(x => x.AS_Code == StatusCode).FirstOrDefault();
            FingerPrintApplicantAppointment FPAAppointment = _locationDBContext.FingerPrintApplicantAppointments.Where(x => x.FPAA_ID == ApplicantAppointmentID && !x.FPAA_IsDeleted).FirstOrDefault();
            OldStatusId = FPAAppointment.FPAA_Status.HasValue ? FPAAppointment.FPAA_Status.Value : 0;
            if (FPAAppointment != null)
            {
                FPAAppointment.FPAA_ModifiedOn = DateTime.Now;
                FPAAppointment.FPAA_Status = AppointmentStatus.AS_ID;
                FPAAppointment.FPAA_ModifiedBy = BackgroundProcessId;
            }
            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }
        public List<AppointmentOrderScheduleContract> GetApplicantsOffTimeRevokedAppointments(String LocationTenantIds, Int32 chunkSize)
        {
            List<AppointmentOrderScheduleContract> appointmentOrderLst = new List<AppointmentOrderScheduleContract>();
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetApplicantRevokedAppointments", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantIDs", LocationTenantIds);
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);

                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    AppointmentOrderScheduleContract AppointmentOrder = new AppointmentOrderScheduleContract();
                    AppointmentOrder.TenantID = dr["TenantID"] != DBNull.Value ? Convert.ToString(dr["TenantID"]) : null;
                    AppointmentOrder.AppointmentDate = dr["AppointmentDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["AppointmentDate"]);
                    AppointmentOrder.EndTime = dr["EndTime"] == DBNull.Value ? null : Convert.ToString(dr["EndTime"]);
                    AppointmentOrder.StartTime = dr["StartTime"] == DBNull.Value ? null : Convert.ToString(dr["StartTime"]);
                    AppointmentOrder.FirstName = dr["FirstName"] == DBNull.Value ? null : Convert.ToString(dr["FirstName"]);
                    AppointmentOrder.LastName = dr["LastName"] == DBNull.Value ? null : Convert.ToString(dr["LastName"]);
                    AppointmentOrder.SlotId = dr["SlotId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SlotId"]);
                    AppointmentOrder.LocationDescription = dr["LocationDescription"] == DBNull.Value ? null : Convert.ToString(dr["LocationDescription"]);
                    AppointmentOrder.LocationName = dr["LocationName"] == DBNull.Value ? null : Convert.ToString(dr["LocationName"]);
                    AppointmentOrder.LocationId = dr["LocationId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["LocationId"]);
                    AppointmentOrder.OrderId = dr["OrderId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrderId"]);
                    AppointmentOrder.OrderNumber = dr["OrderNumber"] == DBNull.Value ? null : Convert.ToString(dr["OrderNumber"]);
                    AppointmentOrder.ApplicantAppointmentId = dr["ApplicantAppointmentId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ApplicantAppointmentId"]);
                    AppointmentOrder.ApplicantOrgUserId = dr["OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrganizationUserID"]);
                    AppointmentOrder.LocationAddress = Convert.ToString(dr["LocationAddress"]);
                    AppointmentOrder.HeirarchyNodeId = dr["HeirarchyNodeId"] != DBNull.Value ? Convert.ToInt32(dr["HeirarchyNodeId"]) : 0;
                    AppointmentOrder.ApplicantEmail = Convert.ToString(dr["ApplicantEmail"]);
                    AppointmentOrder.TotalOrderPrice = dr["TotalOrderPrice"] != DBNull.Value ? Convert.ToDecimal(dr["TotalOrderPrice"]) : 0;
                    AppointmentOrder.OrderDate = Convert.ToDateTime(dr["OrderDate"]);
                    AppointmentOrder.OrderStatus = Convert.ToString(dr["OrderStatus"]);
                    AppointmentOrder.IsOnsiteAppointment = dr["IsOnsiteAppointment"] != DBNull.Value ? Convert.ToBoolean(dr["IsOnsiteAppointment"]) : false;
                    AppointmentOrder.IsOutOfStateAppointment = dr["IsOutOfStateAppointment"] != DBNull.Value ? Convert.ToBoolean(dr["IsOutOfStateAppointment"]) : false;
                    appointmentOrderLst.Add(AppointmentOrder);
                }
                con.Close();
            }
            return appointmentOrderLst;
        }


        #endregion
        public IQueryable<Entity.LocationEntity.lkpAppointmentStatu> GetAllAppointmentStatus()
        {
            return _locationDBContext.lkpAppointmentStatus.Where(cond => !cond.AS_IsDeleted);
        }

        List<LocationServiceAppointmentAuditContract> IFingerPrintSetupRepository.GetAuditHistoryList(String tenantIds, CustomPagingArgsContract GridCustomPagingArgs, Int32 organizationUserId, LocationServiceAppointmentAuditContract filterContract)
        {
            List<LocationServiceAppointmentAuditContract> appointmentAuditHistoryLst = new List<LocationServiceAppointmentAuditContract>();
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;

            String orderBy = "UpdationDate";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(GridCustomPagingArgs.SortExpression) ? orderBy : GridCustomPagingArgs.SortExpression;
            ordDirection = !GridCustomPagingArgs.SortDirectionDescending && !GridCustomPagingArgs.SortExpression.IsNullOrEmpty() ? "asc" : "desc";



            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_AppointmentAuditHistoryList", con);
                command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@OrgUserID", organizationUserId);
                command.Parameters.AddWithValue("@EventName", filterContract != null ? filterContract.EventNameFilter : string.Empty);
                command.Parameters.AddWithValue("@OrderNumber", filterContract != null ? filterContract.OrderNumberFilter : string.Empty);
                command.Parameters.AddWithValue("@IsEvent", filterContract != null ? filterContract.IsEventFilter : false);
                command.Parameters.AddWithValue("@IsAllAppointment", filterContract != null ? filterContract.IsAllAppointment : false);
                command.Parameters.AddWithValue("@AuditDateFrom", filterContract != null ? filterContract.AppointmentAuditHistoryFrom : null);
                command.Parameters.AddWithValue("@AuditDateTo", filterContract != null ? filterContract.AppointmentAuditHistoryTo : null);
                command.Parameters.AddWithValue("@LocationIds", filterContract != null ? filterContract.LocationIds : null);
                command.Parameters.AddWithValue("@OrderBy", orderBy.IsNullOrEmpty() ? null : orderBy);
                command.Parameters.AddWithValue("@OrderDirection", ordDirection.IsNullOrEmpty() ? null : ordDirection);
                command.Parameters.AddWithValue("@PageSize", GridCustomPagingArgs.PageSize);
                command.Parameters.AddWithValue("@PageIndex", GridCustomPagingArgs.CurrentPageIndex);
                command.Parameters.AddWithValue("@TenantIDs", tenantIds.IsNullOrEmpty() ? null : tenantIds);
                command.Parameters.AddWithValue("@ApplicantName", filterContract != null ? filterContract.ApplicantnameFilter : string.Empty);
                command.Parameters.AddWithValue("@IsOutOfState", filterContract != null ? filterContract.IsOutOfState : false);

                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    LocationServiceAppointmentAuditContract AppointmentAuditHistory = new LocationServiceAppointmentAuditContract();
                    AppointmentAuditHistory.TenantId = dr["TenantID"] != DBNull.Value ? Convert.ToInt32(dr["TenantID"]) : 0;
                    AppointmentAuditHistory.AppointmentAuditId = dr["AppointmentAuditId"] != DBNull.Value ? Convert.ToInt32(dr["AppointmentAuditId"]) : 0;
                    AppointmentAuditHistory.OrderNumber = dr["OrderNumber"] != DBNull.Value ? Convert.ToString(dr["OrderNumber"]) : String.Empty;
                    AppointmentAuditHistory.Description = Convert.ToString(dr["Description"]);
                    AppointmentAuditHistory.TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0;
                    AppointmentAuditHistory.UpdatedBy = Convert.ToString(dr["UpdatedBy"]);
                    AppointmentAuditHistory.UpdationDate = Convert.ToDateTime(dr["UpdationDate"]);
                    AppointmentAuditHistory.ApplicantName = Convert.ToString(dr["ApplicantName"]);
                    AppointmentAuditHistory.LocationName = Convert.ToString(dr["LocationName"]);
                    appointmentAuditHistoryLst.Add(AppointmentAuditHistory);
                }
                con.Close();
            }
            return appointmentAuditHistoryLst;
        }


        #region Onsite Events
        public List<ManageOnsiteEventsContract> GetOnsiteEvents(CustomPagingArgsContract customPagingArgsContract, Int32 locationId)
        {
            List<ManageOnsiteEventsContract> lstOnsiteEvents = new List<ManageOnsiteEventsContract>();
            Int32 currentPageIndex = 0;
            Int32 virtualPageCount = 0;
            EntityConnection connection = this._locationDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetOnsiteEventsList", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LocationId", locationId);
                command.Parameters.AddWithValue("@filteringSortingData", customPagingArgsContract.XML);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        currentPageIndex = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["CurrentPageIndex"]));
                        virtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["VirtualCount"]));
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            ManageOnsiteEventsContract onsiteEvents = new ManageOnsiteEventsContract();
                            onsiteEvents.LocationEventId = ds.Tables[1].Rows[i]["LocationEventId"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[1].Rows[i]["LocationEventId"]);
                            onsiteEvents.LocationId = ds.Tables[1].Rows[i]["LocationId"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[1].Rows[i]["LocationId"]);
                            onsiteEvents.LocationEventName = ds.Tables[1].Rows[i]["LocationEventName"] == DBNull.Value ? String.Empty : Convert.ToString(ds.Tables[1].Rows[i]["LocationEventName"]);
                            onsiteEvents.LocationEventDesc = ds.Tables[1].Rows[i]["LocationEventDesc"] == DBNull.Value ? String.Empty : Convert.ToString(ds.Tables[1].Rows[i]["LocationEventDesc"]);
                            onsiteEvents.LocationEventFromDate = Convert.ToDateTime(ds.Tables[1].Rows[i]["LocationEventFromDate"]);
                            onsiteEvents.LocationEventToDate = Convert.ToDateTime(ds.Tables[1].Rows[i]["LocationEventToDate"]);
                            onsiteEvents.LocationEventPublishedDate = ds.Tables[1].Rows[i]["LocationEventPublishedDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[1].Rows[i]["LocationEventPublishedDate"]) : (DateTime?)null;
                            onsiteEvents.LocationEventIsPublished = Convert.ToBoolean(ds.Tables[1].Rows[i]["LocationEventIsPublished"]);
                            onsiteEvents.TotalCount = virtualPageCount;
                            onsiteEvents.CurrentPageIndex = currentPageIndex;
                            lstOnsiteEvents.Add(onsiteEvents);
                        }
                    }
                }
            }
            return lstOnsiteEvents;
        }


        public int SaveOnsiteEvent(ManageOnsiteEventsContract onsiteEventContract, Int32 currentLoggedInUserId)
        {
            if (!onsiteEventContract.IsNullOrEmpty())
            {

                FingerPrintLocationEvent fingerprintLocationEvent = new FingerPrintLocationEvent();
                if (!onsiteEventContract.LocationEventId.IsNullOrEmpty() && onsiteEventContract.LocationEventId > AppConsts.NONE)
                {
                    //Update Onsite Events
                    fingerprintLocationEvent = _locationDBContext.FingerPrintLocationEvents.Where(cond => !cond.FPLE_IsDeleted && cond.FPLE_ID == onsiteEventContract.LocationEventId && cond.FPLE_LocationID == onsiteEventContract.LocationId).FirstOrDefault();
                    fingerprintLocationEvent.FPLE_Name = onsiteEventContract.LocationEventName;
                    fingerprintLocationEvent.FPLE_Description = onsiteEventContract.LocationEventDesc;
                    fingerprintLocationEvent.FPLE_FromDate = onsiteEventContract.LocationEventFromDate;
                    fingerprintLocationEvent.FPLE_ToDate = onsiteEventContract.LocationEventToDate;
                    fingerprintLocationEvent.FPLE_ModifiedBy = currentLoggedInUserId;
                    fingerprintLocationEvent.FPLE_ModifiedOn = DateTime.Now;
                }
                else
                {
                    //Save onsite events
                    fingerprintLocationEvent.FPLE_Name = onsiteEventContract.LocationEventName;
                    fingerprintLocationEvent.FPLE_Description = onsiteEventContract.LocationEventDesc;
                    fingerprintLocationEvent.FPLE_FromDate = onsiteEventContract.LocationEventFromDate;
                    fingerprintLocationEvent.FPLE_ToDate = onsiteEventContract.LocationEventToDate;
                    fingerprintLocationEvent.FPLE_IsPublished = false;
                    fingerprintLocationEvent.FPLE_LocationID = onsiteEventContract.LocationId;
                    fingerprintLocationEvent.FPLE_IsDeleted = false;
                    fingerprintLocationEvent.FPLE_CreatedBy = currentLoggedInUserId;
                    fingerprintLocationEvent.FPLE_CreatedOn = DateTime.Now;

                    _locationDBContext.FingerPrintLocationEvents.AddObject(fingerprintLocationEvent);
                }
                if (_locationDBContext.SaveChanges() > AppConsts.NONE)
                {
                    return fingerprintLocationEvent.FPLE_ID;
                }
                else
                {
                    return AppConsts.NONE;
                }
            }
            else
            {
                return AppConsts.NONE;
            }
        }
        public Boolean DeleteOnsiteEvent(Int32 eventId, Int32 locationId, Int32 currentLoggedInUserId)
        {
            FingerPrintLocationEvent fingerprintLocationEvent = new FingerPrintLocationEvent();
            fingerprintLocationEvent = _locationDBContext.FingerPrintLocationEvents.Where(cond => !cond.FPLE_IsDeleted && cond.FPLE_ID == eventId && cond.FPLE_LocationID == locationId).FirstOrDefault();
            if (!fingerprintLocationEvent.IsNullOrEmpty())
            {
                fingerprintLocationEvent.FPLE_IsDeleted = true;
                fingerprintLocationEvent.FPLE_ModifiedBy = currentLoggedInUserId;
                fingerprintLocationEvent.FPLE_ModifiedOn = DateTime.Now;
            }
            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }
        public ManageOnsiteEventsContract GetSelectedEventDetails(Int32 eventId)
        {
            ManageOnsiteEventsContract eventDetails = new ManageOnsiteEventsContract();
            if (!eventId.IsNullOrEmpty() && eventId > AppConsts.NONE)
            {
                FingerPrintLocationEvent fingerPrintLocationEvent = new FingerPrintLocationEvent();
                fingerPrintLocationEvent = _locationDBContext.FingerPrintLocationEvents.Where(cond => !cond.FPLE_IsDeleted && cond.FPLE_ID == eventId).FirstOrDefault();
                eventDetails.LocationEventId = fingerPrintLocationEvent.FPLE_ID;
                eventDetails.LocationId = Convert.ToInt32(fingerPrintLocationEvent.FPLE_LocationID);
                eventDetails.LocationEventName = fingerPrintLocationEvent.FPLE_Name;
                eventDetails.LocationEventDesc = fingerPrintLocationEvent.FPLE_Description.IsNullOrEmpty() ? String.Empty : fingerPrintLocationEvent.FPLE_Description;
                eventDetails.LocationEventFromDate = fingerPrintLocationEvent.FPLE_FromDate;
                eventDetails.LocationEventToDate = fingerPrintLocationEvent.FPLE_ToDate;
                eventDetails.LocationEventIsPublished = Convert.ToBoolean(fingerPrintLocationEvent.FPLE_IsPublished);
                eventDetails.LocationEventPublishedDate = fingerPrintLocationEvent.FPLE_PublishedDate.IsNullOrEmpty() ? (DateTime?)null : fingerPrintLocationEvent.FPLE_PublishedDate;
            }
            return eventDetails;
        }

        public List<FingerPrintEventSlotContract> GetEventSlots(CustomPagingArgsContract CustomGridArg, Int32 eventId)
        {
            try
            {
                var lstFingerPrintEventSlots = _locationDBContext.FingerPrintEventsSlots.Where(cond => !cond.FPES_IsDeleted && cond.FPES_EventID == eventId).ToList();
                var totalCount = lstFingerPrintEventSlots.Count();
                List<FingerPrintEventSlotContract> lstEventSlots = new List<FingerPrintEventSlotContract>();
                lstEventSlots = lstFingerPrintEventSlots.Select(x => new FingerPrintEventSlotContract()
                {
                    EventSlotId = x.FPES_ID,
                    EventSlot_Description = x.FPES_Description,
                    EventSlot_EventsCode = x.FPES_EventsCode,
                    EventSlot_FromTime = x.FPES_FromTime,
                    EventSlot_ToTime = x.FPES_ToTime,
                    EventId = x.FPES_EventID,
                    Increment = x.FPES_Increment,
                    TotalAppointment = x.FPES_TotalAppointment,
                    TotalCount = totalCount
                }).ToList();

                return lstEventSlots;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }


        }

        public bool SaveOnsiteEventSlot(FingerPrintEventSlotContract eventSlotContract, int currentLoggedInUserId)
        {
            if (!eventSlotContract.IsNullOrEmpty())
            {

                FingerPrintEventsSlot fingerprintLocationEventSlot = new FingerPrintEventsSlot();
                if (!eventSlotContract.EventSlotId.IsNullOrEmpty() && eventSlotContract.EventSlotId > AppConsts.NONE)
                {
                    //Update Onsite Event slots
                    fingerprintLocationEventSlot = _locationDBContext.FingerPrintEventsSlots.Where(cond => !cond.FPES_IsDeleted && cond.FPES_ID == eventSlotContract.EventSlotId && cond.FPES_EventID == eventSlotContract.EventId).FirstOrDefault();
                    fingerprintLocationEventSlot.FPES_Description = eventSlotContract.EventSlot_Description;
                    fingerprintLocationEventSlot.FPES_FromTime = eventSlotContract.EventSlot_FromTime;
                    fingerprintLocationEventSlot.FPES_ToTime = eventSlotContract.EventSlot_ToTime;
                    fingerprintLocationEventSlot.FPES_Increment = eventSlotContract.Increment;
                    fingerprintLocationEventSlot.FPES_TotalAppointment = eventSlotContract.TotalAppointment;
                    fingerprintLocationEventSlot.FPES_ModifiedBy = currentLoggedInUserId;
                    fingerprintLocationEventSlot.FPES_ModifiedOn = DateTime.Now;
                }
                else
                {
                    //Save onsite event slots
                    fingerprintLocationEventSlot.FPES_Description = eventSlotContract.EventSlot_Description;
                    fingerprintLocationEventSlot.FPES_FromTime = eventSlotContract.EventSlot_FromTime;
                    fingerprintLocationEventSlot.FPES_ToTime = eventSlotContract.EventSlot_ToTime;
                    fingerprintLocationEventSlot.FPES_EventID = eventSlotContract.EventId;
                    fingerprintLocationEventSlot.FPES_IsDeleted = false;
                    fingerprintLocationEventSlot.FPES_Increment = eventSlotContract.Increment;
                    fingerprintLocationEventSlot.FPES_TotalAppointment = eventSlotContract.TotalAppointment;
                    fingerprintLocationEventSlot.FPES_CreatedBy = currentLoggedInUserId;
                    fingerprintLocationEventSlot.FPES_CreatedOn = DateTime.Now;

                    _locationDBContext.FingerPrintEventsSlots.AddObject(fingerprintLocationEventSlot);
                }
                if (_locationDBContext.SaveChanges() > AppConsts.NONE)
                {
                    return true;
                }
            }

            return false;
        }
        public bool DeleteOnsiteEventSlot(int eventSlotId, Int32 currentLoggedInUserId)
        {
            FingerPrintEventsSlot fingerprintEventSlot = new FingerPrintEventsSlot();
            fingerprintEventSlot = _locationDBContext.FingerPrintEventsSlots.Where(cond => !cond.FPES_IsDeleted && cond.FPES_ID == eventSlotId).FirstOrDefault();
            if (!fingerprintEventSlot.IsNullOrEmpty())
            {
                fingerprintEventSlot.FPES_IsDeleted = true;
                fingerprintEventSlot.FPES_ModifiedBy = currentLoggedInUserId;
                fingerprintEventSlot.FPES_ModifiedOn = DateTime.Now;
            }
            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }
        public bool PublishEvent(int eventId, Int32 currentLoggedInUserId)
        {
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_PublishFingerPrintEvent", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@eventId", eventId);
                command.Parameters.AddWithValue("@userId", currentLoggedInUserId);

                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }

            return true;
        }

        public bool IsLocationMapped(int locationId)
        {
            var isMapped = _locationDBContext.LocationTenantMappings.Where(cond => !cond.LTM_IsDeleted && cond.LTM_LocationID == locationId);
            if (!isMapped.IsNullOrEmpty())
                return true;
            else
                return false;
        }

        #endregion

        public FingerPrintLocation GetLocationById(Int32 LocationId)
        {
            if (LocationId > AppConsts.NONE)
            {
                return _locationDBContext.FingerPrintLocations.Where(x => x.FPL_ID == LocationId && !x.FPL_IsDeleted).FirstOrDefault();
            }
            return null;
        }
        public FingerPrintApplicantLocationImageContract GetImagesWithLocationId(Int32 LocationId)
        {
            FingerPrintApplicantLocationImageContract fingerprintLocImagesData = new FingerPrintApplicantLocationImageContract();
            var location = _locationDBContext.FingerPrintLocations.Where(x => x.FPL_ID == LocationId && !x.FPL_IsDeleted).FirstOrDefault();
            if (!location.IsNullOrEmpty())
            {
                fingerprintLocImagesData.LocationName = location.FPL_Name;
            }
            var LocationImagesData = _locationDBContext.FingerPrintLocationImages.Where(x => !x.FPLI_IsDeleted && x.FPLI_LocationId == LocationId).ToList();
            var totalCount = LocationImagesData.Count();
            fingerprintLocImagesData.imageDataList = new List<FingerPrintLocationImagesContract>();
            fingerprintLocImagesData.imageDataList = LocationImagesData.Select(x => new FingerPrintLocationImagesContract()
            {
                FPLI_FileName = x.FPLI_FileName,
                FPLI_FilePath = x.FPLI_FilePath,
                FPLI_ID = x.FPLI_ID,
                FPLI_LocationId = x.FPLI_LocationId,
                TotalCount = totalCount
            }).ToList();

            return fingerprintLocImagesData;
        }

        Boolean IFingerPrintSetupRepository.ChangeAppointmentStatus(Int32 orderID, Int32 currentLoggedInUserID, Boolean IsDeleted, Int32 reservedSlotId, Boolean reservedSlotNewStatus)
        {
            FingerPrintApplicantAppointment fingerPrintAppointment = new FingerPrintApplicantAppointment();
            fingerPrintAppointment = _locationDBContext.FingerPrintApplicantAppointments.Where(cond => cond.FPAA_OrderID == orderID && cond.FPAA_IsDeleted != IsDeleted).OrderByDescending(x=>x.FPAA_ID).FirstOrDefault();

            if (!fingerPrintAppointment.IsNullOrEmpty())
            {
                fingerPrintAppointment.FPAA_IsDeleted = IsDeleted;
            }

            if(reservedSlotId > 0)
            {
                var reservedSlot = _locationDBContext.ReservedSlots.FirstOrDefault(rs => rs.RS_ID == reservedSlotId 
                && rs.RS_IsDeleted != reservedSlotNewStatus);

                if(reservedSlot != null)
                {
                    reservedSlot.RS_IsDeleted = reservedSlotNewStatus;
                }
            }
            else if(!fingerPrintAppointment.IsNull() && reservedSlotId == -1 && !IsDeleted)
            {
                var reservedSlot = _locationDBContext.ReservedSlots.Where(rs => rs.RS_CreatedBy == currentLoggedInUserID
                && rs.RS_SlotID == fingerPrintAppointment.FPAA_ScheduleSlotID).OrderByDescending(rs=>rs.RS_ID).FirstOrDefault();
                if(reservedSlot != null 
                    && reservedSlot.RS_IsDeleted != reservedSlotNewStatus)
                {
                    reservedSlot.RS_IsDeleted = reservedSlotNewStatus;
                }
            }
            return _locationDBContext.SaveChanges() > AppConsts.NONE;
        }

        #region UAT - 4242
        List<AppointmentOrderScheduleContract> IFingerPrintSetupRepository.GetLocationEnrollerList(Int32 LocationId)
        {
            List<AppointmentOrderScheduleContract> EnrollerDataList = new List<AppointmentOrderScheduleContract>();
            if (LocationId > AppConsts.NONE)
            {
                EntityConnection connection = _locationDBContext.Connection as EntityConnection;

                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand command = new SqlCommand("ams.usp_GetLocationEnrollerData", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@locationId", LocationId);

                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        AppointmentOrderScheduleContract EnrollerData = new AppointmentOrderScheduleContract();
                        EnrollerData.ApplicantOrgUserId = dr["USERID"] != DBNull.Value ? Convert.ToInt32(dr["USERID"]) : 0;
                        EnrollerData.UserFullName = dr["UserName"].ToString();
                        EnrollerData.ApplicantEmail = dr["EmailId"].ToString();
                        EnrollerDataList.Add(EnrollerData);
                    }
                    con.Close();
                }
            }
            return EnrollerDataList;
        }
        #endregion

        #region UAT-4230
        Boolean IFingerPrintSetupRepository.UpdateManualRevokedAppointment(String OrderID, String NewStatusCode, Int32 CurrentLoggedInUserId, Boolean IsManualRevokedStatus)
        {
            Boolean result = false;
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@orderIds",OrderID),
                    new SqlParameter("@statusCode",NewStatusCode),
                    new SqlParameter("@currentLoggedInUserId",CurrentLoggedInUserId),
                    new SqlParameter("@IsManualRevokedStatus",IsManualRevokedStatus)
                };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_UpdateAndAuditManualRevokedOrder", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            result = dr["result"] == DBNull.Value ? false : Convert.ToBoolean(dr["result"]);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
                return result;
            }
        }
        #endregion

        ScheduleInformationContract IFingerPrintSetupRepository.GetLastBookedAppointmentDate(ScheduleInformationContract scheduleInformationContract)
        {
            ScheduleInformationContract result = new ScheduleInformationContract();
            if (scheduleInformationContract != null && scheduleInformationContract.ScheduleID > AppConsts.NONE)
            {
                EntityConnection connection = _locationDBContext.Connection as EntityConnection;

                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand command = new SqlCommand("ams.usp_GetLastBookedSlot", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ScheduleID", scheduleInformationContract.ScheduleID);
                    command.Parameters.AddWithValue("@LocationID", scheduleInformationContract.LocationID);

                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    SqlDataReader dr = command.ExecuteReader();
                    //bool bTemp = false;
                    while (dr.Read())
                    {
                        result.LastBookedAppointmentDate = dr["lastBookedDate"] != DBNull.Value ? Convert.ToDateTime(dr["lastBookedDate"]) : new Nullable<DateTime>();
                        //bool.TryParse(dr["isPendingChange"].ToString(), out bTemp);
                        result.IsPendingChanges = dr["isPendingChange"].ToString() == "1";
                    }
                    con.Close();
                }
            }
            return result;
        }
        Boolean IFingerPrintSetupRepository.IsAppointmentBookedForTheSelectedDate(DateTime AppointmentDate, Int32 ScheduleID)
        {
            var schedules = _locationDBContext.Schedules.Where(a => a.S_ID == ScheduleID
                    || a.S_RecurrenceParentID == ScheduleID)
                .Select(a => a.S_ID).ToList();
            AppointmentDate = AppointmentDate.Date;
            return _locationDBContext.FingerPrintApplicantAppointments.Any(fpaa => schedules.Any(s => s == fpaa.FingerPrintScheduleSlot.FPSS_ScheduleID)
                         && !fpaa.FPAA_IsDeleted
                         && fpaa.FingerPrintScheduleSlot.FPSS_SlotDate == AppointmentDate && fpaa.lkpAppointmentStatu.AS_Code == "AAAA");
        }
        
        List<LookupContract> IFingerPrintSetupRepository.GetAllFingerPrintAppointmentStatus(Int32 tenantID)
        {
            List<LookupContract> lkpDataLst = new List<LookupContract>();
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetAllAppointmentStatus", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantID", tenantID);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    LookupContract lkpData = new LookupContract();

                    lkpData.Name = Convert.ToString(dr["Name"]);
                    lkpData.Code = Convert.ToString(dr["Code"]);
                    lkpDataLst.Add(lkpData);
                }
                con.Close();
            }
            return lkpDataLst;
        }       

        Int32 IFingerPrintSetupRepository.SaveFingerPrintLocationTimeFrame(Int32 cuurentLoggedInUserId, Int32 locationID, Int32? TimeFrame)
        {
            FingerPrintLocation fingerPrintLocation = _locationDBContext.FingerPrintLocations.Where(cond => cond.FPL_ID == locationID && !cond.FPL_IsDeleted).FirstOrDefault();
            fingerPrintLocation.FPL_TimeFrameHours = TimeFrame;
            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
                return fingerPrintLocation.FPL_ID;
            return AppConsts.NONE;
        }

        #region UAT - 4025

        Dictionary<Int32, String> IFingerPrintSetupRepository.GetCABSUsers()
        {
            Dictionary<Int32, String> lstCABSUsers = new Dictionary<Int32, String>();
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;


            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.GetCABSUsers", con);
                command.CommandType = CommandType.StoredProcedure;

                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {

                    String UserName = Convert.ToString(dr["UserName"]);
                    Int32 OrganizationUserID = Convert.ToInt32(dr["OrganizationUserID"]);
                    lstCABSUsers.Add(OrganizationUserID, UserName);
                }
                con.Close();
            }
            return lstCABSUsers;

        }

        List<String> IFingerPrintSetupRepository.GetPermittedCBIId(Int32 CurrentLoggedInUserID)
        {
            List<String> lstCbiUniqueIds = new List<String>();
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetPermittedCbiUniqueIds", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrganizationUserId", CurrentLoggedInUserID);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    String CbiUniqueId = dr["CbiUniqueId"] != DBNull.Value ? Convert.ToString(dr["CbiUniqueId"]) : null;
                    lstCbiUniqueIds.Add(CbiUniqueId);
                }
                con.Close();
            }
            return lstCbiUniqueIds;
        }

        List<AppointmentOrderScheduleContract> IFingerPrintSetupRepository.GetHrAdminAppointmentOrders(Int32 UserId, AppointmentOrderScheduleContract filterContract, CustomPagingArgsContract GridCustomPagingArgs, String TenantIds, Boolean IsHrAdminEnroller)
        {
            List<AppointmentOrderScheduleContract> appointmentOrderLst = new List<AppointmentOrderScheduleContract>();
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;

            String orderBy = "OrderDate";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(GridCustomPagingArgs.SortExpression) ? orderBy : GridCustomPagingArgs.SortExpression;
            ordDirection = !GridCustomPagingArgs.SortDirectionDescending && !GridCustomPagingArgs.SortExpression.IsNullOrEmpty() ? "asc" : "desc";



            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_FingerprintHrAdminOrderList", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrgUserID", UserId);
                command.Parameters.AddWithValue("@IsAdminLogin", !IsHrAdminEnroller);
                command.Parameters.AddWithValue("@FirstName", filterContract != null ? filterContract.AppFirstNameFilter : string.Empty);
                command.Parameters.AddWithValue("@LastName", filterContract != null ? filterContract.AppLastNameFilter : string.Empty);
                command.Parameters.AddWithValue("@OrderNumber", filterContract != null ? filterContract.OrderIdFilter : string.Empty);
                command.Parameters.AddWithValue("@CBIPCNNumber", filterContract != null ? filterContract.CBIPCNNumber : string.Empty);
                command.Parameters.AddWithValue("@AppointmentDateFrom", filterContract != null ? filterContract.AppointmentDateFrom : null);
                command.Parameters.AddWithValue("@AppointmentDateTo", filterContract != null ? filterContract.AppointmentDateTo : null);
                command.Parameters.AddWithValue("@FingerPrintingSite", filterContract != null ? filterContract.FingerPrintingLocation : AppConsts.NONE);
                command.Parameters.AddWithValue("@AppointmentStatusIds", filterContract != null ? filterContract.AppointmentStatusIds : null);
                command.Parameters.AddWithValue("@OrderBy", orderBy.IsNullOrEmpty() ? null : orderBy);
                command.Parameters.AddWithValue("@OrderDirection", ordDirection.IsNullOrEmpty() ? null : ordDirection);
                command.Parameters.AddWithValue("@PageSize", GridCustomPagingArgs.PageSize);
                command.Parameters.AddWithValue("@PageIndex", GridCustomPagingArgs.CurrentPageIndex);
                command.Parameters.AddWithValue("@TenantIDs", TenantIds.IsNullOrEmpty() ? null : TenantIds);
                command.Parameters.AddWithValue("@IsHrAdminEnroller", IsHrAdminEnroller);
                command.Parameters.AddWithValue("@CbiUniqueIds", filterContract != null ? filterContract.CbiUniqueids : null);

                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    AppointmentOrderScheduleContract AppointmentOrder = new AppointmentOrderScheduleContract();
                    AppointmentOrder.AppointmentDate = dr["AppointmentDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["AppointmentDate"]);
                    AppointmentOrder.EndTime = dr["EndTime"] == DBNull.Value ? null : Convert.ToString(dr["EndTime"]);
                    AppointmentOrder.FirstName = dr["FirstName"] == DBNull.Value ? null : Convert.ToString(dr["FirstName"]);
                    AppointmentOrder.LastName = dr["LastName"] == DBNull.Value ? null : Convert.ToString(dr["LastName"]);
                    AppointmentOrder.LocationDescription = dr["LocationDescription"] == DBNull.Value ? null : Convert.ToString(dr["LocationDescription"]);
                    AppointmentOrder.LocationId = dr["LocationId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["LocationId"]);
                    AppointmentOrder.LocationName = dr["LocationName"] == DBNull.Value ? null : Convert.ToString(dr["LocationName"]);
                    AppointmentOrder.OrderId = dr["OrderId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrderId"]);
                    AppointmentOrder.OrderNumber = dr["OrderNumber"] == DBNull.Value ? null : Convert.ToString(dr["OrderNumber"]);
                    AppointmentOrder.SlotId = dr["SlotId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SlotId"]);
                    AppointmentOrder.StartTime = dr["StartTime"] == DBNull.Value ? null : Convert.ToString(dr["StartTime"]);
                    AppointmentOrder.ApplicantAppointmentId = dr["ApplicantAppointmentId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ApplicantAppointmentId"]);
                    AppointmentOrder.ApplicantOrgUserId = dr["OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrganizationUserID"]);
                    AppointmentOrder.PermissionName = dr["PermissionName"] == DBNull.Value ? null : Convert.ToString(dr["PermissionName"]);
                    AppointmentOrder.TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0;
                    AppointmentOrder.TenantID = dr["TenantID"] != DBNull.Value ? Convert.ToString(dr["TenantID"]) : null;
                    AppointmentOrder.PackageName = dr["PackageName"] != DBNull.Value ? Convert.ToString(dr["PackageName"]) : null;
                    AppointmentOrder.LocationAddress = Convert.ToString(dr["LocationAddress"]);
                    AppointmentOrder.HeirarchyNodeId = dr["HeirarchyNodeId"] != DBNull.Value ? Convert.ToInt32(dr["HeirarchyNodeId"]) : 0;
                    AppointmentOrder.ApplicantEmail = Convert.ToString(dr["ApplicantEmail"]);
                    AppointmentOrder.OrderDate = Convert.ToDateTime(dr["OrderDate"]);
                    AppointmentOrder.TotalOrderPrice = dr["TotalOrderPrice"] != DBNull.Value ? Convert.ToDecimal(dr["TotalOrderPrice"]) : 0;
                    AppointmentOrder.PaymentStatus = Convert.ToString(dr["PaymentStatus"]);
                    AppointmentOrder.OrderStatus = Convert.ToString(dr["OrderStatus"]);
                    AppointmentOrder.IsContractLocation = Convert.ToBoolean(dr["IsContractLocation"]);
                    AppointmentOrder.FingerPrintingSite = Convert.ToString(dr["FingerPrintingSite"]);
                    AppointmentOrder.CBIPCNNumber = Convert.ToString(dr["CBIPCNNumber"]);
                    AppointmentOrder.OrderStatusCode = dr["OrderStatusCode"].ToString();
                    AppointmentOrder.AppointmentStatusCode = dr["AppointmentStatusCode"].ToString();
                    AppointmentOrder.CbiUniqueId = dr["CbiUniqueId"].ToString();
                    // AppointmentOrder.AppointmentStatus = dr["AppointmentStatus"] == DBNull.Value ? null : Convert.ToString(dr["AppointmentStatus"]);
                    // AppointmentOrder.AppointmentStatus = dr["AppointmentStatus"] == DBNull.Value ? null : Convert.ToString(dr["AppointmentStatus"]);
                    appointmentOrderLst.Add(AppointmentOrder);
                }
                con.Close();
            }
            return appointmentOrderLst;
        }

        List<HrAdminPermissionContract> IFingerPrintSetupRepository.GetAllHrAdmins(HrAdminPermissionContract filterContract, CustomPagingArgsContract GridCustomPagingArgs)
        {
            List<HrAdminPermissionContract> lstHrAdminPermission = new List<HrAdminPermissionContract>();
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;
            String orderBy = "FirstName";
            String ordDirection = null;
            orderBy = String.IsNullOrEmpty(GridCustomPagingArgs.SortExpression) ? orderBy : GridCustomPagingArgs.SortExpression;
            ordDirection = !GridCustomPagingArgs.SortDirectionDescending && !GridCustomPagingArgs.SortExpression.IsNullOrEmpty() ? "asc" : "desc";

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetAllPermittedHrAdminsList", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FirstName", filterContract != null ? filterContract.FirstNameFilter : string.Empty);
                command.Parameters.AddWithValue("@LastName", filterContract != null ? filterContract.LastNameFilter : string.Empty);
                command.Parameters.AddWithValue("@OrderBy", orderBy.IsNullOrEmpty() ? null : orderBy);
                command.Parameters.AddWithValue("@OrderDirection", ordDirection.IsNullOrEmpty() ? null : ordDirection);
                command.Parameters.AddWithValue("@PageSize", GridCustomPagingArgs.PageSize);
                command.Parameters.AddWithValue("@PageIndex", GridCustomPagingArgs.CurrentPageIndex);

                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    HrAdminPermissionContract HrAdminPermission = new HrAdminPermissionContract();
                    HrAdminPermission.FirstName = Convert.ToString(dr["FirstName"]);
                    HrAdminPermission.LastName = Convert.ToString(dr["LastName"]);
                    HrAdminPermission.OrganizationUserID = dr["OrgUserId"] != DBNull.Value ? Convert.ToInt32(dr["OrgUserId"]) : AppConsts.NONE;
                    lstHrAdminPermission.Add(HrAdminPermission);
                }
                con.Close();
            }
            return lstHrAdminPermission;

        }


        Boolean IFingerPrintSetupRepository.SaveHrAdminPermission(HrAdminPermissionContract PermissionContract, Int32 currentLoggedInUserId)
        {
            int permissionId = _locationDBContext.lkpCABSPermissionTypes.Where(cond => cond.LCPT_IsDeleted == false
            && cond.LCPT_Code == "AAAC").Select(col => col.LCPT_ID).FirstOrDefault();
            //Save  permission Permission
            UserCABSPermissionMapping UserMapping = new UserCABSPermissionMapping();
            UserMapping.UCPM_OrganizationUserID = PermissionContract.OrganizationUserID;
            UserMapping.UCPM_CreatedBy = currentLoggedInUserId;
            UserMapping.UCPM_CreatedOn = DateTime.Now;
            UserMapping.UCPM_PermissionTypeID = permissionId;
            UserMapping.UCPM_IsDeleted = false;



            _locationDBContext.UserCABSPermissionMappings.AddObject(UserMapping);



            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }


        Boolean IFingerPrintSetupRepository.DeleteHrAdminPermission(Int32 currentLoggedInUserID, Int32 UserId)
        {
            try
            {
                //Delete  permission Permission
                List<UserCABSPermissionMapping> UserMapping = _locationDBContext.UserCABSPermissionMappings
                    .Where(cond => cond.UCPM_OrganizationUserID == UserId 
                    && !cond.UCPM_IsDeleted)
                    .ToList();
                foreach (var item in UserMapping)
                {
                    item.UCPM_IsDeleted = true;
                    item.UCPM_ModifiedBy = currentLoggedInUserID;
                    item.UCPM_ModifiedOn = DateTime.Now;
                }

                if (_locationDBContext.SaveChanges() > AppConsts.NONE)
                    return true;
                return false;
            }
            catch (SysXException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw (new SysXException(ex.Message, ex));
            }
        }

        List<String> IFingerPrintSetupRepository.FilterCBIUniqueIdsToSave(Int32 selectedOrganizationUserID, List<String> lstCBIUniqueId)
        {
            var cbiUniqueIdsToIgnore = _locationDBContext.UserCABSPermissionMappings.Where(um => um.UCPM_OrganizationUserID == selectedOrganizationUserID
                && um.lkpCABSPermissionType.LCPT_Code == "AAAA"
                && !um.UCPM_IsDeleted)
                .Select(um => um.UCPM_Value)
                .ToList();

            return lstCBIUniqueId.Except(cbiUniqueIdsToIgnore, StringComparer.InvariantCultureIgnoreCase).ToList();
        }


        Boolean IFingerPrintSetupRepository.AssignCBIUniqueIds(Int32 currentLoggedInUserID, Int32 selectedOrganizationUserID, List<String> lstCBIUniqueId)
        {            
            Int32 CBIUniqueIdPermissionTypeID = _locationDBContext.lkpCABSPermissionTypes.Where(cond => cond.LCPT_Code == "AAAA" && !cond.LCPT_IsDeleted).Select(sel => sel.LCPT_ID).FirstOrDefault();
            if (!CBIUniqueIdPermissionTypeID.IsNullOrEmpty())
            {
                foreach (var item in lstCBIUniqueId)
                {
                    UserCABSPermissionMapping _permissionMapping = new UserCABSPermissionMapping();
                    _permissionMapping.UCPM_OrganizationUserID = selectedOrganizationUserID;
                    _permissionMapping.UCPM_PermissionTypeID = CBIUniqueIdPermissionTypeID;
                    _permissionMapping.UCPM_Value = item;
                    _permissionMapping.UCPM_IsDeleted = false;
                    _permissionMapping.UCPM_CreatedBy = currentLoggedInUserID;
                    _permissionMapping.UCPM_CreatedOn = DateTime.Now;
                    _locationDBContext.UserCABSPermissionMappings.AddObject(_permissionMapping);
                }
            }

            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        List<String> IFingerPrintSetupRepository.FilterAccountNamesToSave(Int32 selectedOrganizationUserID, List<String> lstAccountName)
        {
            var accountNamesToIgnore = _locationDBContext.UserCABSPermissionMappings.Where(um => um.UCPM_OrganizationUserID == selectedOrganizationUserID
                && um.lkpCABSPermissionType.LCPT_Code == "AAAB"
                && !um.UCPM_IsDeleted)
                .Select(um => um.UCPM_Value)
                .ToList();

            return lstAccountName.Except(accountNamesToIgnore, StringComparer.InvariantCultureIgnoreCase).ToList();
        }

        Boolean IFingerPrintSetupRepository.AssignAccountNames(Int32 currentLoggedInUserID, Int32 selectedOrganizationUserID, List<String> lstAccountName)
        {
            Int32 accountNamePermissionTypeID = _locationDBContext.lkpCABSPermissionTypes.Where(cond => cond.LCPT_Code == "AAAB" && !cond.LCPT_IsDeleted).Select(sel => sel.LCPT_ID).FirstOrDefault();
            if (!accountNamePermissionTypeID.IsNullOrEmpty())
            { 
                foreach (var item in lstAccountName)
                {
                    UserCABSPermissionMapping _permissionMapping = new UserCABSPermissionMapping();
                    _permissionMapping.UCPM_OrganizationUserID = selectedOrganizationUserID;
                    _permissionMapping.UCPM_PermissionTypeID = accountNamePermissionTypeID;
                    _permissionMapping.UCPM_Value = item;
                    _permissionMapping.UCPM_IsDeleted = false;
                    _permissionMapping.UCPM_CreatedBy = currentLoggedInUserID;
                    _permissionMapping.UCPM_CreatedOn = DateTime.Now;
                    _locationDBContext.UserCABSPermissionMappings.AddObject(_permissionMapping);
                }
            }

            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        Boolean IFingerPrintSetupRepository.UnAssignHRAdminPermission(Int32 currentLoggedInUserID, Int32 permissionID)
        {
            UserCABSPermissionMapping _permissionMapping = _locationDBContext.UserCABSPermissionMappings
                .Where(cond => cond.UCPM_ID == permissionID
                && !cond.UCPM_IsDeleted)
                .FirstOrDefault();
            if (!_permissionMapping.IsNullOrEmpty())
            {
                _permissionMapping.UCPM_IsDeleted = true;
                _permissionMapping.UCPM_ModifiedBy = currentLoggedInUserID;
                _permissionMapping.UCPM_ModifiedOn = DateTime.Now;
            }

            if (_locationDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        List<UserCABSPermissionMapping> IFingerPrintSetupRepository.GetHRAdminPermissions(Int32 OrgUserID)
        {
            return _locationDBContext.UserCABSPermissionMappings.Where(cond => cond.UCPM_OrganizationUserID == OrgUserID 
            && !cond.UCPM_IsDeleted)
            .ToList();
        }

        #endregion

        //UAT-4360
        bool IFingerPrintSetupRepository.SaveFingerPrintOrderKeyData(Int32 TenantID,Int32 currentLoggedIdUserID, List<LookupContract> lstLookupContract, Int32 orderId)
        {

            FingerPrintOrderKeyData fingerPrintOrderKeyData = new FingerPrintOrderKeyData {
                FPOKD_OrderID = orderId,
                FPOKD_TenantID=TenantID,
                FPOKD_CreatedBy = currentLoggedIdUserID,
                FPOKD_CreatedOn = DateTime.Now
            };
            lstLookupContract.ForEach(lc =>
            {
                switch (lc.Name)
                {
                    case AppConsts.CBIUniqueID:
                        fingerPrintOrderKeyData.FPOKD_CBIUniqueID = lc.Code;
                        break;
                    case AppConsts.ReasonFingerprinted:
                        fingerPrintOrderKeyData.FPOKD_ReasonFingerprinted = lc.Code;
                        break;
                    case AppConsts.AcctName:
                        fingerPrintOrderKeyData.FPOKD_AccountName = lc.Code;
                        break;
                    case AppConsts.BillingORI:
                        fingerPrintOrderKeyData.FPOKD_BillingAccount = lc.Code;
                        break;
                }
            });

            var existingOrder = _locationDBContext.FingerPrintOrderKeyDatas
                .FirstOrDefault(fp => fp.FPOKD_TenantID == TenantID
                && fp.FPOKD_OrderID == orderId
                && !fp.FPOKD_IsDeleted);

            if(existingOrder != null)
            {
                existingOrder.FPOKD_IsDeleted = true;
                existingOrder.FPOKD_ModifiedBy = currentLoggedIdUserID;
                existingOrder.FPOKD_ModifiedOn = DateTime.Now;
            }

            _locationDBContext.FingerPrintOrderKeyDatas.AddObject(fingerPrintOrderKeyData);
            return _locationDBContext.SaveChanges() > 0;
        }

        FingerPrintOrderKeyDataContract IFingerPrintSetupRepository.GetFingerPrintOrderKeydata(Int32 OrderID)
        {

            FingerPrintOrderKeyDataContract lstfingerprintdata = LocationDBContext.FingerPrintOrderKeyDatas
                .Where(Fpd => Fpd.FPOKD_OrderID == OrderID && !Fpd.FPOKD_IsDeleted)
                 .Select(Fpd => new FingerPrintOrderKeyDataContract
                 {
                     CBIUniqueID = Fpd.FPOKD_CBIUniqueID,
                     ReasonFingerprinted = Fpd.FPOKD_ReasonFingerprinted,
                     BillingORI = Fpd.FPOKD_BillingAccount,
                     AcctName = Fpd.FPOKD_AccountName
                 }).FirstOrDefault(); 

           return lstfingerprintdata;
        }

        bool IFingerPrintSetupRepository.IsReservedSlotExpired(Int32 reservedSlotId, Int32 currentLoggedIdUserID,bool IsCCPayment)
        {
            var connection = LocationDBContext.Connection as EntityConnection;
            var isexpired = true;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[ams].[usp_IsReservedSlotExpired]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@reservedSlotId", reservedSlotId);
                command.Parameters.AddWithValue("@currentLoggenInUser", currentLoggedIdUserID);
                command.Parameters.AddWithValue("@ccPayment", IsCCPayment);
                command.Parameters.Add("@rs_expired", SqlDbType.Bit);
                command.Parameters["@rs_expired"].Direction = ParameterDirection.Output;

                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();
                isexpired = (bool)command.Parameters["@rs_expired"].Value;
                con.Close();
            }
            return isexpired;
        }

        List<AppointmentOrderScheduleContract> IFingerPrintSetupRepository.GetOrderFulFillment(Boolean IsAdminLogin, Int32 UserId, AppointmentOrderScheduleContract filterContract, CustomPagingArgsContract GridCustomPagingArgs, String TenantIds)
        {
            List<AppointmentOrderScheduleContract> orderFulFillmentLst = new List<AppointmentOrderScheduleContract>();
            EntityConnection connection = _locationDBContext.Connection as EntityConnection;

            String orderBy = "OrderDate";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(GridCustomPagingArgs.SortExpression) ? orderBy : GridCustomPagingArgs.SortExpression;
            ordDirection = !GridCustomPagingArgs.SortDirectionDescending && !GridCustomPagingArgs.SortExpression.IsNullOrEmpty() ? "asc" : "desc";

            String addtionalSearchStatus = string.Empty;
            String addtionalSearchotherStatus = string.Empty;
            if (!filterContract.AppointmentStatusIds.IsNullOrEmpty())
            {
                addtionalSearchStatus = GetAdditionalStatus(filterContract.AppointmentStatusIds);
                addtionalSearchotherStatus = GetAdditionalOrderStatus(filterContract.AppointmentStatusIds);
            }

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_FingerprintAppointmentFulFillmentQueueList", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrgUserID", UserId);
                command.Parameters.AddWithValue("@IsAdminLogin", IsAdminLogin);
                command.Parameters.AddWithValue("@FirstName", filterContract != null ? filterContract.AppFirstNameFilter : string.Empty);
                command.Parameters.AddWithValue("@LastName", filterContract != null ? filterContract.AppLastNameFilter : string.Empty);
                command.Parameters.AddWithValue("@OrderNumber", filterContract != null ? filterContract.OrderIdFilter : string.Empty);
                command.Parameters.AddWithValue("@CBIPCNNumber", filterContract != null ? filterContract.CBIPCNNumber : string.Empty);
                command.Parameters.AddWithValue("@ABIPCNNumber", filterContract != null ? filterContract.ABIPCNNumber : string.Empty);
                command.Parameters.AddWithValue("@AppointmentDateFrom", filterContract != null ? filterContract.AppointmentDateFrom : null);
                command.Parameters.AddWithValue("@AppointmentDateTo", filterContract != null ? filterContract.AppointmentDateTo : null);
                command.Parameters.AddWithValue("@LocationIds", filterContract != null ? filterContract.LocationIds : null);
                command.Parameters.AddWithValue("@FingerPrintingSite", filterContract != null ? filterContract.FingerPrintingLocation : AppConsts.NONE);
                command.Parameters.AddWithValue("@AppointmentStatusIds", filterContract != null ? filterContract.AppointmentStatusIds : null);
                command.Parameters.AddWithValue("@OrderBy", orderBy.IsNullOrEmpty() ? null : orderBy);
                command.Parameters.AddWithValue("@OrderDirection", ordDirection.IsNullOrEmpty() ? null : ordDirection);
                command.Parameters.AddWithValue("@PageSize", GridCustomPagingArgs.PageSize);
                command.Parameters.AddWithValue("@PageIndex", GridCustomPagingArgs.CurrentPageIndex);
                command.Parameters.AddWithValue("@TenantIDs", TenantIds.IsNullOrEmpty() ? null : TenantIds);
                command.Parameters.AddWithValue("@ShipmentStatusIds", filterContract != null ? filterContract.ShipmentStatusIds : null);
                command.Parameters.AddWithValue("@addtionalSearchIds", addtionalSearchStatus != null ? addtionalSearchStatus : null);
                command.Parameters.AddWithValue("@addtionalSearchotherids", addtionalSearchotherStatus != null ? addtionalSearchotherStatus : null);
                
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    AppointmentOrderScheduleContract orderFulFillment = new AppointmentOrderScheduleContract();
                    orderFulFillment.AppointmentDate = dr["AppointmentDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["AppointmentDate"]);
                    orderFulFillment.EndTime = dr["EndTime"] == DBNull.Value ? null : Convert.ToString(dr["EndTime"]);
                    orderFulFillment.FirstName = dr["FirstName"] == DBNull.Value ? null : Convert.ToString(dr["FirstName"]);
                    orderFulFillment.LastName = dr["LastName"] == DBNull.Value ? null : Convert.ToString(dr["LastName"]);
                    orderFulFillment.LocationDescription = dr["LocationDescription"] == DBNull.Value ? null : Convert.ToString(dr["LocationDescription"]);
                    orderFulFillment.LocationId = dr["LocationId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["LocationId"]);
                    orderFulFillment.LocationName = dr["LocationName"] == DBNull.Value ? null : Convert.ToString(dr["LocationName"]);
                    orderFulFillment.OrderId = dr["OrderId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrderId"]);
                    orderFulFillment.OrderNumber = dr["OrderNumber"] == DBNull.Value ? null : Convert.ToString(dr["OrderNumber"]);
                    orderFulFillment.SlotId = dr["SlotId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SlotId"]);
                    orderFulFillment.StartTime = dr["StartTime"] == DBNull.Value ? null : Convert.ToString(dr["StartTime"]);
                    orderFulFillment.ApplicantAppointmentId = dr["ApplicantAppointmentId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ApplicantAppointmentId"]);
                    orderFulFillment.ApplicantOrgUserId = dr["OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["OrganizationUserID"]);
                    orderFulFillment.PermissionName = dr["PermissionName"] == DBNull.Value ? null : Convert.ToString(dr["PermissionName"]);
                    orderFulFillment.TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0;
                    orderFulFillment.TenantID = dr["TenantID"] != DBNull.Value ? Convert.ToString(dr["TenantID"]) : null;
                    orderFulFillment.PackageName = dr["PackageName"] != DBNull.Value ? Convert.ToString(dr["PackageName"]) : null;
                    orderFulFillment.LocationAddress = Convert.ToString(dr["LocationAddress"]);
                    orderFulFillment.HeirarchyNodeId = dr["HeirarchyNodeId"] != DBNull.Value ? Convert.ToInt32(dr["HeirarchyNodeId"]) : 0;
                    orderFulFillment.ApplicantEmail = Convert.ToString(dr["ApplicantEmail"]);
                    orderFulFillment.OrderDate = Convert.ToDateTime(dr["OrderDate"]);
                    orderFulFillment.TotalOrderPrice = dr["TotalOrderPrice"] != DBNull.Value ? Convert.ToDecimal(dr["TotalOrderPrice"]) : 0;
                    orderFulFillment.PaymentStatus = Convert.ToString(dr["PaymentStatus"]);
                    orderFulFillment.OrderStatus = Convert.ToString(dr["OrderStatus"]);                    
                    orderFulFillment.IsContractLocation = Convert.ToBoolean(dr["IsContractLocation"]);
                    orderFulFillment.FingerPrintingSite = Convert.ToString(dr["FingerPrintingSite"]);
                    orderFulFillment.CBIPCNNumber = Convert.ToString(dr["CBIPCNNumber"]);
                    orderFulFillment.ABIPCNNumber=Convert.ToString(dr["ABIPCNNumber"]);
                    orderFulFillment.OrderStatusCode = dr["OrderStatusCode"].ToString();
                    orderFulFillment.AppointmentStatusCode = dr["AppointmentStatusCode"].ToString();
                    orderFulFillment.FingerPrintCardFileName = dr["FingerprintCardFilename"] != DBNull.Value ? dr["FingerprintCardFilename"].ToString() : null;
                    orderFulFillment.FingerPrintCardDocID = dr["FingerprintCardDocID"] != DBNull.Value ? dr["FingerprintCardDocID"].ToString() : null;
                    orderFulFillment.PassportPhotoFileName = dr["PassportPhotoFilename"] != DBNull.Value ? dr["PassportPhotoFilename"].ToString() : null;
                    orderFulFillment.PassportPhotoDocID = dr["PassportPhotoDocID"] != DBNull.Value ? dr["PassportPhotoDocID"].ToString() : null;
                    orderFulFillment.ShipmentStatus = Convert.ToString(dr["ShipmentStatus"]);
                    orderFulFillmentLst.Add(orderFulFillment);
                }
                con.Close();
            }
            return orderFulFillmentLst;
        }


       private string GetAdditionalStatus(string OrderIds)
        {
            List<string> listStrLineElements = OrderIds.Split(',').ToList();
            String returnValue = string.Empty;
            List<string> Retrunresult = new List<string>();
            if (listStrLineElements.Contains("AAAB_1"))
            {
                
                Retrunresult.Add(CABSServiceStatus.PENDING_SHIPMENT.GetStringValue());
                Retrunresult.Add(CABSServiceStatus.RETURNED_TO_SENDER.GetStringValue());
            }
            if (listStrLineElements.Contains("AAAG_0"))
            {
                Retrunresult.Add(CABSServiceStatus.SHIPPED.GetStringValue());
            }
            if (listStrLineElements.Contains("AAAQ_0"))
            {
                Retrunresult.Add(CABSServiceStatus.REJECTSERVICE.GetStringValue());
            }
            if (listStrLineElements.Contains("AAAA_1"))
            {
                Retrunresult.Add(CABSServiceStatus.NEW.GetStringValue());
            }
            if(Retrunresult != null)
            {
                 returnValue = String.Join(",", Retrunresult);
                return returnValue;
            }
            return string.Empty;
        }

        private string GetAdditionalOrderStatus(string OrderIds)
        {
            List<string> listStrLineElements = OrderIds.Split(',').ToList();
            String returnValue = string.Empty;
            List<string> Retrunresult = new List<string>();
            foreach(var item in listStrLineElements)
            {

                if(!(item == "AAAB_1" || item == "AAAG_0" || item == "AAAQ_0" || item == "AAAA_1"))
                {
                    Retrunresult.Add(item);
                }
               
            }
            if(Retrunresult.Count >0)
                returnValue = String.Join(",", Retrunresult);

            return returnValue;
        }


        bool IsCabsserviceInProgress(int? StatusCode)
        {
            if (StatusCode != null)
            {
                String Status = base.LocationDBContext.lkpAppointmentStatus.Where(x => x.AS_ID == StatusCode.Value).FirstOrDefault().AS_Code;
                List<String> AppointmentStatusList = new List<String>();
                AppointmentStatusList.Add(FingerPrintAppointmentStatus.ACTIVE.GetStringValue());
                AppointmentStatusList.Add(FingerPrintAppointmentStatus.MISSED.GetStringValue());
                AppointmentStatusList.Add(FingerPrintAppointmentStatus.MISSED_AND_NOTIFIED.GetStringValue());
                AppointmentStatusList.Add(FingerPrintAppointmentStatus.REJECTED_BY_ABI.GetStringValue());
                AppointmentStatusList.Add(FingerPrintAppointmentStatus.REVOKED.GetStringValue());
                AppointmentStatusList.Add(FingerPrintAppointmentStatus.REVOKED_AND_NOTIFIED.GetStringValue());
                AppointmentStatusList.Add(FingerPrintAppointmentStatus.REJECTED_BY_CBI.GetStringValue());
                AppointmentStatusList.Add(FingerPrintAppointmentStatus.REJECTED_BY_FBI.GetStringValue());
                AppointmentStatusList.Add(FingerPrintAppointmentStatus.COMPLETED.GetStringValue());
                if (!AppointmentStatusList.Contains(Status))
                {
                    return true;
                }
            }

            return false;
        }


    }
}