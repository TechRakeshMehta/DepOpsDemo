using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.SystemSetUp;
//using Entity;
using INTSOF.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;


namespace DAL.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class ComplianceSetupRepository : ClientBaseRepository, IComplianceSetupRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _ClientDBContext;


        ///// <summary>
        ///// Default constructor to initialize class level variables.
        ///// </summary>
        public ComplianceSetupRepository(Int32 tenantId)
            : base(tenantId)
        {
            _ClientDBContext = base.ClientDBContext;
        }

        ///// <summary>
        ///// Obsolete constructor. Will be removed once all the calls to parameterless constructor are removed.
        ///// </summary>
        //public ComplianceSetupRepository()
        //    : base(1)
        //{
        //    _ClientAdminContext = base.Context;
        //}

        #region Admin Setup Screens


        #region 3871
        public List<CompliancePackage> GetCompliancePackagesForTrackingRequired(Int32 TenantId)
        {
            List<CompliancePackage> compliancePackages = ClientDBContext.CompliancePackages.Where(obj => obj.IsDeleted == false).ToList();
            return compliancePackages;
        }
        public List<TrackingPackageRequiredContract> GetTrackingPackageRequired(Int32 tenantId, String SelectedPackageIDs)
        {
            EntityConnection connection = null;
            if (tenantId == AppConsts.ONE)
            {
                connection = base.SecurityContext.Connection as EntityConnection;
            }
            else
                if (tenantId > AppConsts.ONE)
            {
                connection = base.ClientDBContext.Connection as EntityConnection;
            }
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[dbo].[GetTrackingPackageRequired]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantId", tenantId);
                command.Parameters.AddWithValue("@PackageIDs", SelectedPackageIDs);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                List<TrackingPackageRequiredContract> trackingPackageRequiredContract = new List<TrackingPackageRequiredContract>();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    trackingPackageRequiredContract = ds.Tables[0].AsEnumerable().Select(col =>
                         new TrackingPackageRequiredContract
                         {
                             CountOfAssociated = Convert.ToString(col["CountOfPackage"]),
                             TempCountOfAssociated = Convert.ToString(col["CountOfPackage"]),
                             trackingPackageRequiredDOCURLId = Convert.ToInt32(col["ID"]),
                             SampleDocFormURL = col["Link"] == DBNull.Value ? null : Convert.ToString(col["Link"]),
                             PackageIds = Convert.ToString(col["PackagesIds"]),
                             ScreenName = Convert.ToString(col["internalName"]),
                             Label = Convert.ToString(col["dispalayLabel"])

                         }).ToList();
                }
                return trackingPackageRequiredContract;
            }
        }
        //<summary>
        //Save/Update the compliance item
        //</summary>
        //<param name="complianceItem">Details of the compliance item to save/update</param>
        public Boolean SaveComplianceItem(TrackingPackageRequiredContract trackingPackageRequiredContract, Int32 currentloggedInUserId)
        {
            Boolean IsSuccessfullyDataInsertdOrUpdate = false;
            #region Update Data
            var packageUrlId = 0;
            if (trackingPackageRequiredContract.trackingPackageRequiredDOCURLId > 0)
            {
                if (trackingPackageRequiredContract.TenantId == AppConsts.ONE)
                {
                    Entity.TrackingPackageRequiredDocURL _UpdateTrackingPackageRequiredDocURL = null;
                    _UpdateTrackingPackageRequiredDocURL = base.SecurityContext.TrackingPackageRequiredDocURLs.Where(GetValue => GetValue.TPRDU_ID == trackingPackageRequiredContract.trackingPackageRequiredDOCURLId && GetValue.TPRDU_IsDeleted == false).FirstOrDefault();
                    _UpdateTrackingPackageRequiredDocURL.TPRDU_DisplayLabel = trackingPackageRequiredContract.Label;
                    _UpdateTrackingPackageRequiredDocURL.TPRDU_InternalName = trackingPackageRequiredContract.Name;
                    _UpdateTrackingPackageRequiredDocURL.TPRDU_Link = trackingPackageRequiredContract.SampleDocFormURL;
                    _UpdateTrackingPackageRequiredDocURL.TPRDU_ModifiedByID = currentloggedInUserId;
                    _UpdateTrackingPackageRequiredDocURL.TPRDU_ModifiedOn = DateTime.Now;

                    List<Entity.TrackingPackageRequiredDocURLMapping> listTrackingPackageRequiredDocURLMapping = base.SecurityContext.TrackingPackageRequiredDocURLMappings.Where(GetValue => GetValue.TPRPM_URLID == trackingPackageRequiredContract.trackingPackageRequiredDOCURLId && GetValue.TPRPM_IsDeleted == false).ToList();
                    string[] TempPackageIds = trackingPackageRequiredContract.PackageIds.Split(',');
                    if (TempPackageIds.Count() > 0)
                    {
                        for (int i = 0; i < TempPackageIds.Length - 1; i++)
                        {
                            //fresh Insert
                            var TempPackageRequiredDOC = listTrackingPackageRequiredDocURLMapping.Where(x => x.TPRPM_CompliancePackageId == Convert.ToInt32(TempPackageIds[i])).FirstOrDefault();
                            if (TempPackageRequiredDOC == null)
                            {
                                Entity.TrackingPackageRequiredDocURLMapping objTrackingPackageRequiredDocURLMapping = new Entity.TrackingPackageRequiredDocURLMapping();

                                objTrackingPackageRequiredDocURLMapping.TPRPM_URLID = trackingPackageRequiredContract.trackingPackageRequiredDOCURLId;
                                objTrackingPackageRequiredDocURLMapping.TPRPM_CompliancePackageId = Convert.ToInt32(TempPackageIds[i]);
                                objTrackingPackageRequiredDocURLMapping.TPRPM_CreatedByID = currentloggedInUserId;
                                objTrackingPackageRequiredDocURLMapping.TPRPM_CreatedOn = DateTime.Now;
                                objTrackingPackageRequiredDocURLMapping.TPRPM_IsActive = true;
                                objTrackingPackageRequiredDocURLMapping.TPRPM_IsDeleted = false;
                                base.SecurityContext.TrackingPackageRequiredDocURLMappings.AddObject(objTrackingPackageRequiredDocURLMapping);
                            }
                        }
                    }
                    if (listTrackingPackageRequiredDocURLMapping.Count > 0)
                    {
                        foreach (var item in listTrackingPackageRequiredDocURLMapping)
                        {
                            //Delete here data
                            string[] GetTempData = trackingPackageRequiredContract.PackageIds.Split(',');
                            if (Array.IndexOf(GetTempData, item.TPRPM_CompliancePackageId.ToString()) == -1)
                            {
                                Entity.TrackingPackageRequiredDocURLMapping objTrackingPackageRequiredDocURLMapping = base.SecurityContext.TrackingPackageRequiredDocURLMappings.Where(GetValue => GetValue.TPRPM_ID == item.TPRPM_ID && GetValue.TPRPM_IsDeleted == false).FirstOrDefault();
                                objTrackingPackageRequiredDocURLMapping.TPRPM_ModifiedByID = currentloggedInUserId;
                                objTrackingPackageRequiredDocURLMapping.TPRPM_ModifiedOn = DateTime.Now;
                                objTrackingPackageRequiredDocURLMapping.TPRPM_IsActive = false;
                                objTrackingPackageRequiredDocURLMapping.TPRPM_IsDeleted = true;

                            }
                        }
                    }
                    if (base.SecurityContext.SaveChanges() > 0)
                        IsSuccessfullyDataInsertdOrUpdate = true;
                    else
                        IsSuccessfullyDataInsertdOrUpdate = false;

                }
                else if (trackingPackageRequiredContract.TenantId > AppConsts.ONE)
                {
                    Entity.ClientEntity.TrackingPackageRequiredDocURL EntityUpdateTrackingPackageRequiredDocURL = null;
                    EntityUpdateTrackingPackageRequiredDocURL = base.ClientDBContext.TrackingPackageRequiredDocURLs.Where(GetValue => GetValue.TPRDU_ID == trackingPackageRequiredContract.trackingPackageRequiredDOCURLId && GetValue.TPRDU_IsDeleted == false).FirstOrDefault();
                    EntityUpdateTrackingPackageRequiredDocURL.TPRDU_DisplayLabel = trackingPackageRequiredContract.Label;
                    EntityUpdateTrackingPackageRequiredDocURL.TPRDU_InternalName = trackingPackageRequiredContract.Name;
                    EntityUpdateTrackingPackageRequiredDocURL.TPRDU_Link = trackingPackageRequiredContract.SampleDocFormURL;
                    EntityUpdateTrackingPackageRequiredDocURL.TPRDU_ModifiedByID = currentloggedInUserId;
                    EntityUpdateTrackingPackageRequiredDocURL.TPRDU_ModifiedOn = DateTime.Now;

                    List<Entity.ClientEntity.TrackingPackageRequiredDocURLMapping> listTrackingPackageRequiredDocURLMapping = base.ClientDBContext.TrackingPackageRequiredDocURLMappings.Where(GetValue => GetValue.TPRPM_URLID == trackingPackageRequiredContract.trackingPackageRequiredDOCURLId && GetValue.TPRPM_IsDeleted == false).ToList();
                    string[] TempPackageIds = trackingPackageRequiredContract.PackageIds.Split(',');
                    if (TempPackageIds.Count() > 0)
                    {
                        for (int i = 0; i < TempPackageIds.Length - 1; i++)
                        {
                            //fresh Insert
                            var TempPackageRequiredDOC = listTrackingPackageRequiredDocURLMapping.Where(x => x.TPRPM_CompliancePackageId == Convert.ToInt32(TempPackageIds[i])).FirstOrDefault();
                            if (TempPackageRequiredDOC == null)
                            {
                                Entity.ClientEntity.TrackingPackageRequiredDocURLMapping objTrackingPackageRequiredDocURLMapping = new Entity.ClientEntity.TrackingPackageRequiredDocURLMapping();

                                objTrackingPackageRequiredDocURLMapping.TPRPM_URLID = trackingPackageRequiredContract.trackingPackageRequiredDOCURLId;
                                objTrackingPackageRequiredDocURLMapping.TPRPM_CompliancePackageId = Convert.ToInt32(TempPackageIds[i]);
                                objTrackingPackageRequiredDocURLMapping.TPRPM_CreatedByID = currentloggedInUserId;
                                objTrackingPackageRequiredDocURLMapping.TPRPM_CreatedOn = DateTime.Now;
                                objTrackingPackageRequiredDocURLMapping.TPRPM_IsActive = true;
                                objTrackingPackageRequiredDocURLMapping.TPRPM_IsDeleted = false;
                                base.ClientDBContext.TrackingPackageRequiredDocURLMappings.AddObject(objTrackingPackageRequiredDocURLMapping);
                            }
                        }
                    }
                    if (listTrackingPackageRequiredDocURLMapping.Count > 0)
                    {
                        foreach (var item in listTrackingPackageRequiredDocURLMapping)
                        {
                            //Delete here data
                            string[] GetTempData = trackingPackageRequiredContract.PackageIds.Split(',');
                            if (Array.IndexOf(GetTempData, item.TPRPM_CompliancePackageId.ToString()) == -1)
                            {
                                Entity.ClientEntity.TrackingPackageRequiredDocURLMapping objTrackingPackageRequiredDocURLMapping = base.ClientDBContext.TrackingPackageRequiredDocURLMappings.Where(GetValue => GetValue.TPRPM_ID == item.TPRPM_ID && GetValue.TPRPM_IsDeleted == false).FirstOrDefault();
                                objTrackingPackageRequiredDocURLMapping.TPRPM_ModifiedByID = currentloggedInUserId;
                                objTrackingPackageRequiredDocURLMapping.TPRPM_ModifiedOn = DateTime.Now;
                                objTrackingPackageRequiredDocURLMapping.TPRPM_IsActive = false;
                                objTrackingPackageRequiredDocURLMapping.TPRPM_IsDeleted = true;

                            }
                        }
                    }
                    if (base.ClientDBContext.SaveChanges() > 0)
                        IsSuccessfullyDataInsertdOrUpdate = true;
                    else
                        IsSuccessfullyDataInsertdOrUpdate = false;

                }



            }
            #endregion
            #region Insert Data
            else
            {
                if (trackingPackageRequiredContract.TenantId == AppConsts.ONE)
                {
                    Entity.TrackingPackageRequiredDocURL objTrackingPackageRequiredDocURL = new Entity.TrackingPackageRequiredDocURL();
                    objTrackingPackageRequiredDocURL.TPRDU_DisplayLabel = trackingPackageRequiredContract.Label;
                    objTrackingPackageRequiredDocURL.TPRDU_InternalName = trackingPackageRequiredContract.Name;
                    objTrackingPackageRequiredDocURL.TPRDU_Link = trackingPackageRequiredContract.SampleDocFormURL;
                    objTrackingPackageRequiredDocURL.TPRDU_CreatedByID = currentloggedInUserId;
                    objTrackingPackageRequiredDocURL.TPRDU_CreatedOn = DateTime.Now;
                    base.SecurityContext.TrackingPackageRequiredDocURLs.AddObject(objTrackingPackageRequiredDocURL);
                    if (base.SecurityContext.SaveChanges() > 0)
                    {
                        packageUrlId = objTrackingPackageRequiredDocURL.TPRDU_ID;
                        if (trackingPackageRequiredContract.PackageIds != null && packageUrlId > 0)
                        {
                            string[] TempPackageIds = trackingPackageRequiredContract.PackageIds.Split(',');
                            if (TempPackageIds.Count() > 0)
                            {

                                for (int i = 0; i < TempPackageIds.Length - 1; i++)
                                {
                                    Entity.TrackingPackageRequiredDocURLMapping objTrackingPackageRequiredDocURLMapping = new Entity.TrackingPackageRequiredDocURLMapping();
                                    objTrackingPackageRequiredDocURLMapping.TPRPM_URLID = packageUrlId;
                                    objTrackingPackageRequiredDocURLMapping.TPRPM_CompliancePackageId = Convert.ToInt32(TempPackageIds[i]);
                                    objTrackingPackageRequiredDocURLMapping.TPRPM_CreatedByID = currentloggedInUserId;
                                    objTrackingPackageRequiredDocURLMapping.TPRPM_CreatedOn = DateTime.Now;
                                    objTrackingPackageRequiredDocURLMapping.TPRPM_IsActive = true;
                                    objTrackingPackageRequiredDocURLMapping.TPRPM_IsDeleted = false;
                                    base.SecurityContext.TrackingPackageRequiredDocURLMappings.AddObject(objTrackingPackageRequiredDocURLMapping);

                                }
                                if (base.SecurityContext.SaveChanges() > 0)
                                    IsSuccessfullyDataInsertdOrUpdate = true;
                                else
                                    IsSuccessfullyDataInsertdOrUpdate = false;
                            }
                        }
                        else
                            IsSuccessfullyDataInsertdOrUpdate = false;
                    }
                }
                else if (trackingPackageRequiredContract.TenantId > AppConsts.ONE)
                {
                    Entity.ClientEntity.TrackingPackageRequiredDocURL objTrackingPackageRequiredDocURL = new Entity.ClientEntity.TrackingPackageRequiredDocURL();
                    objTrackingPackageRequiredDocURL.TPRDU_DisplayLabel = trackingPackageRequiredContract.Label;
                    objTrackingPackageRequiredDocURL.TPRDU_InternalName = trackingPackageRequiredContract.Name;
                    objTrackingPackageRequiredDocURL.TPRDU_Link = trackingPackageRequiredContract.SampleDocFormURL;
                    objTrackingPackageRequiredDocURL.TPRDU_CreatedByID = currentloggedInUserId;
                    objTrackingPackageRequiredDocURL.TPRDU_CreatedOn = DateTime.Now;
                    base.ClientDBContext.TrackingPackageRequiredDocURLs.AddObject(objTrackingPackageRequiredDocURL);
                    if (base.ClientDBContext.SaveChanges() > 0)
                    {
                        packageUrlId = objTrackingPackageRequiredDocURL.TPRDU_ID;
                        if (trackingPackageRequiredContract.PackageIds != null && packageUrlId > 0)
                        {
                            string[] TempPackageIds = trackingPackageRequiredContract.PackageIds.Split(',');
                            if (TempPackageIds.Count() > 0)
                            {

                                for (int i = 0; i < TempPackageIds.Length - 1; i++)
                                {
                                    Entity.ClientEntity.TrackingPackageRequiredDocURLMapping objTrackingPackageRequiredDocURLMapping = new Entity.ClientEntity.TrackingPackageRequiredDocURLMapping();
                                    objTrackingPackageRequiredDocURLMapping.TPRPM_URLID = packageUrlId;
                                    objTrackingPackageRequiredDocURLMapping.TPRPM_CompliancePackageId = Convert.ToInt32(TempPackageIds[i]);
                                    objTrackingPackageRequiredDocURLMapping.TPRPM_CreatedByID = currentloggedInUserId;
                                    objTrackingPackageRequiredDocURLMapping.TPRPM_CreatedOn = DateTime.Now;
                                    objTrackingPackageRequiredDocURLMapping.TPRPM_IsActive = true;
                                    objTrackingPackageRequiredDocURLMapping.TPRPM_IsDeleted = false;
                                    base.ClientDBContext.TrackingPackageRequiredDocURLMappings.AddObject(objTrackingPackageRequiredDocURLMapping);

                                }
                                if (base.ClientDBContext.SaveChanges() > 0)
                                    IsSuccessfullyDataInsertdOrUpdate = true;
                                else
                                    IsSuccessfullyDataInsertdOrUpdate = false;
                            }
                        }
                        else
                            IsSuccessfullyDataInsertdOrUpdate = false;
                    }

                }
                #endregion
            }

            return IsSuccessfullyDataInsertdOrUpdate;
        }


        public bool CheckDuplicateRecords(TrackingPackageRequiredContract trackingPackageRequiredContract, Int32 currentloggedInUserId)
        {
            Boolean IsRecordDuplicateFound = false;
            //string Tempstring = trackingPackageRequiredContract.PackageIds.Remove(trackingPackageRequiredContract.PackageIds.Length - 1);

            //string[] GetTempData = Tempstring.Split(',');
            if (trackingPackageRequiredContract.TenantId == AppConsts.ONE)
            {
                if (trackingPackageRequiredContract.trackingPackageRequiredDOCURLId > 0)
                {
                    Entity.TrackingPackageRequiredDocURL CheckDuplicatePackageRequiredDocURL = base.SecurityContext.TrackingPackageRequiredDocURLs.Where(GetValue => GetValue.TPRDU_Link == trackingPackageRequiredContract.SampleDocFormURL && GetValue.TPRDU_InternalName == trackingPackageRequiredContract.Name && GetValue.TPRDU_IsDeleted == false && GetValue.TPRDU_ID != trackingPackageRequiredContract.trackingPackageRequiredDOCURLId).FirstOrDefault();
                    if (!CheckDuplicatePackageRequiredDocURL.IsNullOrEmpty())
                    {
                        IsRecordDuplicateFound = true;
                    }
                }
                else
                {
                    Entity.TrackingPackageRequiredDocURL CheckDuplicatePackageRequiredDocURL = base.SecurityContext.TrackingPackageRequiredDocURLs.Where(GetValue => GetValue.TPRDU_Link == trackingPackageRequiredContract.SampleDocFormURL && GetValue.TPRDU_InternalName == trackingPackageRequiredContract.Name && GetValue.TPRDU_IsDeleted == false).FirstOrDefault();
                    if (!CheckDuplicatePackageRequiredDocURL.IsNullOrEmpty())
                    {
                        IsRecordDuplicateFound = true;
                    }
                }
                //Entity.TrackingPackageRequiredDocURL CheckDuplicatePackageRequiredDocURL = base.SecurityContext.TrackingPackageRequiredDocURLs.Where(GetValue => GetValue.TPRDU_Link == trackingPackageRequiredContract.SampleDocFormURL && GetValue.TPRDU_InternalName == trackingPackageRequiredContract.Name && GetValue.TPRDU_IsDeleted==false).FirstOrDefault();
                //if (CheckDuplicatePackageRequiredDocURL != null)
                //{
                //    var TempList = base.SecurityContext.TrackingPackageRequiredDocURLMappings.Where(x => x.TPRPM_URLID == CheckDuplicatePackageRequiredDocURL.TPRDU_ID && x.TPRPM_IsDeleted==false).ToList();
                //    if (TempList != null)
                //    {
                //        foreach (var item in TempList)
                //        {
                //            if (GetTempData.Contains(item.TPRPM_CompliancePackageId.ToString()))
                //            {

                //                IsRecordDuplicateFound = true;
                //                break;
                //            }
                //        }
                //    }
                //}
            }
            else if (trackingPackageRequiredContract.TenantId > AppConsts.ONE)
            {
                if (trackingPackageRequiredContract.trackingPackageRequiredDOCURLId > 0)
                {
                    Entity.ClientEntity.TrackingPackageRequiredDocURL CheckDuplicatePackageRequiredDocURL = base.ClientDBContext.TrackingPackageRequiredDocURLs.Where(GetValue => GetValue.TPRDU_Link == trackingPackageRequiredContract.SampleDocFormURL && GetValue.TPRDU_InternalName == trackingPackageRequiredContract.Name && GetValue.TPRDU_IsDeleted == false && GetValue.TPRDU_ID != trackingPackageRequiredContract.trackingPackageRequiredDOCURLId).FirstOrDefault();
                    if (!CheckDuplicatePackageRequiredDocURL.IsNullOrEmpty())
                    {
                        IsRecordDuplicateFound = true;
                    }
                }
                else
                {
                    Entity.ClientEntity.TrackingPackageRequiredDocURL CheckDuplicatePackageRequiredDocURL = base.ClientDBContext.TrackingPackageRequiredDocURLs.Where(GetValue => GetValue.TPRDU_Link == trackingPackageRequiredContract.SampleDocFormURL && GetValue.TPRDU_InternalName == trackingPackageRequiredContract.Name && GetValue.TPRDU_IsDeleted == false).FirstOrDefault();
                    if (!CheckDuplicatePackageRequiredDocURL.IsNullOrEmpty())
                    {
                        IsRecordDuplicateFound = true;
                    }
                }


            }

            return IsRecordDuplicateFound;
        }
        public bool DeleteComplianceItem(TrackingPackageRequiredContract trackingPackageRequiredContract, Int32 currentloggedInUserId)
        {
            Boolean IsSuccessfullyDataInsertdOrUpdate = false;
            if (trackingPackageRequiredContract.trackingPackageRequiredDOCURLId > 0)
            {
                if (trackingPackageRequiredContract.TenantId == AppConsts.ONE)
                {
                    Entity.TrackingPackageRequiredDocURL DeleteTrackingPackageRequiredDocURL = base.SecurityContext.TrackingPackageRequiredDocURLs.Where(GetValue => GetValue.TPRDU_ID == trackingPackageRequiredContract.trackingPackageRequiredDOCURLId && GetValue.TPRDU_IsDeleted == false).FirstOrDefault();
                    DeleteTrackingPackageRequiredDocURL.TPRDU_IsDeleted = true;
                    DeleteTrackingPackageRequiredDocURL.TPRDU_ModifiedByID = currentloggedInUserId;
                    DeleteTrackingPackageRequiredDocURL.TPRDU_ModifiedOn = DateTime.Now;
                    base.SecurityContext.TrackingPackageRequiredDocURLMappings.Where(x => x.TPRPM_URLID == trackingPackageRequiredContract.trackingPackageRequiredDOCURLId).ToList().ForEach(a => { a.TPRPM_IsDeleted = true; a.TPRPM_ModifiedByID = currentloggedInUserId; a.TPRPM_ModifiedOn = DateTime.Now; });
                    if (base.SecurityContext.SaveChanges() > 0)
                    {
                        IsSuccessfullyDataInsertdOrUpdate = true;
                    }
                }
                else if (trackingPackageRequiredContract.TenantId > AppConsts.ONE)
                {
                    Entity.ClientEntity.TrackingPackageRequiredDocURL DeleteTrackingPackageRequiredDocURL = base.ClientDBContext.TrackingPackageRequiredDocURLs.Where(GetValue => GetValue.TPRDU_ID == trackingPackageRequiredContract.trackingPackageRequiredDOCURLId && GetValue.TPRDU_IsDeleted == false).FirstOrDefault();
                    DeleteTrackingPackageRequiredDocURL.TPRDU_IsDeleted = true;
                    DeleteTrackingPackageRequiredDocURL.TPRDU_ModifiedByID = currentloggedInUserId;
                    DeleteTrackingPackageRequiredDocURL.TPRDU_ModifiedOn = DateTime.Now;
                    base.ClientDBContext.TrackingPackageRequiredDocURLMappings.Where(x => x.TPRPM_URLID == trackingPackageRequiredContract.trackingPackageRequiredDOCURLId).ToList().ForEach(a => { a.TPRPM_IsDeleted = true; a.TPRPM_ModifiedByID = currentloggedInUserId; a.TPRPM_ModifiedOn = DateTime.Now; });
                    if (base.ClientDBContext.SaveChanges() > 0)
                    {
                        IsSuccessfullyDataInsertdOrUpdate = true;
                    }
                }
            }
            return IsSuccessfullyDataInsertdOrUpdate;
        }

        #endregion
        #region Manage Compliance Packages


        #region //3871
        public List<TrackingPackageRequiredDocURLMapping> GetTrackingPackageRequiredDOCURLMapping()
        {
            List<TrackingPackageRequiredDocURLMapping> listTrackingPackageRequiredDocURLMapping = ClientDBContext.TrackingPackageRequiredDocURLMappings.Where(obj => obj.TPRPM_IsDeleted == false).ToList();

            return listTrackingPackageRequiredDocURLMapping;
        }

        public List<TrackingPackageRequiredDocURL> GetTrackingPackageRequiredDOCURL()
        {
            List<TrackingPackageRequiredDocURL> listTrackingPackageRequiredDocURL = ClientDBContext.TrackingPackageRequiredDocURLs.Where(obj => obj.TPRDU_IsDeleted == false).ToList();

            return listTrackingPackageRequiredDocURL;
        }
        #endregion


        public List<CompliancePackage> GetCompliancePackages(Boolean getTenantName, List<Entity.Tenant> tenantList)
        {
            List<CompliancePackage> compliancePackages = ClientDBContext.CompliancePackages.Where(obj => obj.IsDeleted == false).ToList();
            if (getTenantName)
            {
                compliancePackages.Where(t => t.TenantID != null)
                                  .ForEach(t =>
                                  {
                                      var tenant = tenantList.FirstOrDefault(item => item.TenantID == t.TenantID.Value);
                                      if (tenant.IsNotNull())
                                      {
                                          t.TenantName = tenant.TenantName;
                                      }
                                      else
                                      {
                                          t.TenantName = String.Empty;
                                      }
                                  });
            }

            return compliancePackages;
        }

        public List<NodesContract> GetListofNodes(Int32 CategoryId, Int32? ItemId)
        {
            List<NodesContract> lstNodesContract = new List<NodesContract>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@CategoryId", CategoryId),
                             new SqlParameter("@ItemId", ItemId)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAllNodesForCategoriesItems", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            NodesContract nodesContract = new NodesContract();
                            nodesContract.DPM_ID = dr["DPM_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["DPM_ID"]);
                            nodesContract.DPM_Label = dr["DPM_Label"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DPM_Label"]);
                            nodesContract.PackageName = dr["PackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PackageName"]);

                            lstNodesContract.Add(nodesContract);
                        }
                    }
                    else
                    {
                        NodesContract nodesContract = new NodesContract();
                        nodesContract.DPM_Label = "None";
                        lstNodesContract.Add(nodesContract);
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstNodesContract;
        }

        public CompliancePackage SaveCompliancePackageDetail(CompliancePackage package, Int32 currentUserId)
        {
            package.Code = package.Code.Equals(Guid.Empty) ? Guid.NewGuid() : package.Code;
            package.TenantID = package.TenantID;
            package.CreatedByID = currentUserId;
            package.CreatedOn = DateTime.Now;
            ClientDBContext.CompliancePackages.AddObject(package);
            ClientDBContext.SaveChanges();
            return package;
        }

        public void UpdateCompliancePackageDetail(CompliancePackage package, Int32 currentUserId)
        {
            CompliancePackage compliancePackage = _ClientDBContext.CompliancePackages.FirstOrDefault(obj => obj.CompliancePackageID == package.CompliancePackageID && obj.IsDeleted == false);
            compliancePackage.PackageName = package.PackageName;
            compliancePackage.Description = package.Description;
            compliancePackage.IsActive = package.IsActive;
            compliancePackage.PackageLabel = package.PackageLabel;
            compliancePackage.ScreenLabel = package.ScreenLabel;
            compliancePackage.IsViewDetailsInOrderEnabled = package.IsViewDetailsInOrderEnabled;
            compliancePackage.PackageDetail = package.PackageDetail; //UAT 1006
            compliancePackage.ModifiedByID = currentUserId;
            compliancePackage.ModifiedOn = DateTime.Now;
            compliancePackage.CompliancePackageTypeID = package.CompliancePackageTypeID;
            compliancePackage.ChecklistURL = package.ChecklistURL;
            compliancePackage.NotesDisplayPositionId = package.NotesDisplayPositionId; //UAT-2219
            ClientDBContext.SaveChanges();
            //get values from database which may be useful in operations afterwards.
            package.Code = compliancePackage.Code;
        }

        public Boolean DeleteCompliancePackage(Int32 packageID, Int32 currentUserId)
        {
            Boolean categoryDependency = false;
            categoryDependency = _ClientDBContext.CompliancePackageCategories.Any(obj => obj.CPC_PackageID == packageID && obj.CPC_IsDeleted == false);

            if (!categoryDependency)
            {
                CompliancePackage compliancePackage = ClientDBContext.CompliancePackages.FirstOrDefault(obj => obj.CompliancePackageID == packageID && obj.IsDeleted == false);
                compliancePackage.IsDeleted = true;
                compliancePackage.ModifiedOn = DateTime.Now;
                compliancePackage.ModifiedByID = currentUserId;
                _ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        public Boolean CheckIfPackageNameAlreadyExist(String packageName, Int32 compliancePackageId, Int32 tenantId)
        {
            return ClientDBContext.CompliancePackages.Any(obj => obj.PackageName.ToUpper() == packageName.ToUpper() && obj.CompliancePackageID != compliancePackageId && obj.IsDeleted == false && obj.TenantID == tenantId);
        }

        public Boolean CheckIfPackageNameAlreadyExist(String packageName)
        {
            return ClientDBContext.CompliancePackages.Any(obj => obj.PackageName.ToUpper() == packageName.ToUpper() && obj.IsDeleted == false);
        }

        /// <summary>
        /// To get existing package
        /// </summary>
        /// <param name="packageName">Package Name</param>
        /// <returns>Compliance Package object</returns>
        public CompliancePackage GetExistingPackage(String packageName)
        {
            String delimiter = "-DUP";
            var compliancePackages = ClientDBContext.CompliancePackages.Where(obj => obj.PackageName.Trim().ToUpper().Contains(packageName.Trim().ToUpper() + delimiter) && obj.IsDeleted == false).OrderByDescending(obj => obj.CreatedOn).FirstOrDefault();

            if (compliancePackages.IsNull())
            {
                return ClientDBContext.CompliancePackages.Where(obj => obj.PackageName.Trim().ToUpper() == packageName.Trim().ToUpper() && obj.IsDeleted == false).OrderByDescending(obj => obj.CreatedOn).FirstOrDefault();
            }
            return compliancePackages;
            //return complianceCategories.OrderByDescending(obj => obj.CreatedOn).Select(x => x.CategoryName).FirstOrDefault();
        }

        public CompliancePackage GetCopiedCompliancePackage(Int32 parentPackageId, String packageName)
        {
            return ClientDBContext.CompliancePackages.FirstOrDefault(obj => obj.IsDeleted == false && obj.PackageName == packageName && obj.ParentPackageID == parentPackageId);
        }

        #endregion

        #region Manage Compliance Categories

        /// <summary>
        /// Returns all the compliance categories viewable to the current logged in user. 
        /// </summary>
        /// <returns>List of Compliance Categories</returns>
        public List<ComplianceCategory> GetComplianceCategories(Boolean getTenantName, List<Entity.Tenant> tenantList)
        {
            List<ComplianceCategory> complianceCategories = ClientDBContext.ComplianceCategories.Where(obj => obj.IsDeleted == false).ToList();
            if (getTenantName)
            {
                complianceCategories.Where(t => t.TenantID != null)
                   .ForEach(t =>
                   {
                       var tenant = tenantList.FirstOrDefault(item => item.TenantID == t.TenantID.Value);
                       if (tenant.IsNotNull())
                       {
                           t.TenantName = tenant.TenantName;
                       }
                       else
                       {
                           t.TenantName = String.Empty;
                       }
                   });
            }
            return complianceCategories;
        }

        public List<ComplianceCategory> GetComplianceCategoriesForNodes(string dpmIds)
        {
            List<ComplianceCategory> complianceCategories = new List<ComplianceCategory>();

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("dbo.usp_GetComplianceCatagoriesOfNode", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@dpmIds", dpmIds);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);


                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    complianceCategories = ds.Tables[0].AsEnumerable().Select(col =>
                         new ComplianceCategory
                         {
                             ComplianceCategoryID = Convert.ToInt32(col["ComplianceCategoryID"]),
                             CategoryName = col["CategoryName"].ToString(),
                             Description = col["Description"].ToString(),
                             CategoryLabel = col["CategoryLabel"].ToString(),
                             ScreenLabel = col["ScreenLabel"].ToString(),
                             IsActive = col["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(col["IsActive"]),
                             TenantID = col["TenantID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["TenantID"]),
                             IsCreatedByAdmin = col["IsCreatedByAdmin"] == DBNull.Value ? false : Convert.ToBoolean(col["IsCreatedByAdmin"]),
                             IsDissociated = col["IsDissociated"] == DBNull.Value ? false : Convert.ToBoolean(col["IsDissociated"]),
                             DissociatedFrom = col["DissociatedFrom"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["DissociatedFrom"]),
                             SampleDocFormURL = col["SampleDocFormURL"].ToString(),
                             TriggerOtherCategoryRules = col["TriggerOtherCategoryRules"] == DBNull.Value ? false : Convert.ToBoolean(col["TriggerOtherCategoryRules"]),
                             SampleDocFormURLLabel = col["SampleDocFormURLLabel"].ToString(),
                             SendItemDocOnApproval = col["SendItemDocOnApproval"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(col["SendItemDocOnApproval"]),
                             IsWithMultipleNodes = col["IsWithMultipleNodes"].ToString()
                         }).ToList();

                    List<ComplianceCategoryDocUrl> ComplianceCategoryDocUrls = ds.Tables[1].AsEnumerable().Select(col =>
                         new ComplianceCategoryDocUrl
                         {
                             ComplianceCategoryDocUrlID = Convert.ToInt32(col["ComplianceCategoryDocUrlID"]),
                             ComplianceCategoryID = Convert.ToInt32(col["ComplianceCategoryID"]),
                             SampleDocFormURL = col["SampleDocFormURL"].ToString(),
                             SampleDocFormURLLabel = col["SampleDocFormURLLabel"].ToString()
                         }).ToList();

                    if (ComplianceCategoryDocUrls != null && ComplianceCategoryDocUrls.Any())
                    {
                        complianceCategories
                            .Where(cc =>
                            ComplianceCategoryDocUrls.Any(ccd => ccd.ComplianceCategoryID == cc.ComplianceCategoryID))
                            .ForEach(cc =>
                        {
                            cc.ComplianceCategoryDocUrls = new EntityCollection<ComplianceCategoryDocUrl>();
                            ComplianceCategoryDocUrls.Where(ccd => ccd.ComplianceCategoryID == cc.ComplianceCategoryID).ForEach(doc =>
                            {
                                cc.ComplianceCategoryDocUrls.Add(doc);
                            });
                        });
                    }

                }
            }

            return complianceCategories;
        }

        /// <summary>
        /// Saves the Compliance Category.
        /// </summary>
        /// <param name="category">ComplianceCategory Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <returns>ComplianceCategory Entity</returns>
        public ComplianceCategory SaveCategoryDetail(ComplianceCategory category, Int32 currentLoggedInUserId)
        {
            category.Code = category.Code.Equals(Guid.Empty) ? Guid.NewGuid() : category.Code;
            category.TenantID = category.TenantID;
            category.CreatedByID = currentLoggedInUserId;
            category.CreatedOn = DateTime.Now;
            category.DisplayOrder = category.DisplayOrder;
            category.SampleDocFormURL = category.SampleDocFormURL;
            category.SendItemDocOnApproval = category.SendItemDocOnApproval; //UAT-3805
            _ClientDBContext.ComplianceCategories.AddObject(category);
            _ClientDBContext.SaveChanges();
            return category;
        }

        /// <summary>
        /// Updates the Compliance Category.
        /// </summary>
        /// <param name="category">ComplianceCategory Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        public void UpdateCategoryDetail(ComplianceCategory category, Int32 currentLoggedInUserId, Boolean isMasterScreen = false)
        {
            ComplianceCategory complianceCategory = _ClientDBContext.ComplianceCategories.FirstOrDefault(obj => obj.ComplianceCategoryID == category.ComplianceCategoryID && obj.IsDeleted == false);
            complianceCategory.CategoryName = category.CategoryName;
            complianceCategory.CategoryLabel = category.CategoryLabel;
            complianceCategory.ScreenLabel = category.ScreenLabel;
            complianceCategory.Description = category.Description;
            complianceCategory.TriggerOtherCategoryRules = category.TriggerOtherCategoryRules;
            complianceCategory.SendItemDocOnApproval = category.SendItemDocOnApproval; //UAT-3805
            // TODO Discusses on commented code
            //if (isMasterScreen)
            //    complianceCategory.SampleDocFormURL = category.SampleDocFormURL;

            complianceCategory.IsActive = category.IsActive;
            complianceCategory.CopiedFromCode = category.Code;
            complianceCategory.ModifiedByID = currentLoggedInUserId;
            complianceCategory.ModifiedOn = DateTime.Now;
            //complianceCategory.SampleDocFormURLLabel = category.SampleDocFormURLLabel; //UAT-3161

            var existingDocUrlList = complianceCategory.ComplianceCategoryDocUrls.Where(obj => obj.IsDeleted == false).ToList();
            if (category.ComplianceCategoryDocUrls.Any())
            {
                foreach (var tempDocUrl in category.ComplianceCategoryDocUrls)
                {
                    if (tempDocUrl.ComplianceCategoryDocUrlID > 0)
                    {
                        ComplianceCategoryDocUrl existingDocUrl = existingDocUrlList.FirstOrDefault(obj => obj.ComplianceCategoryDocUrlID == tempDocUrl.ComplianceCategoryDocUrlID && obj.IsDeleted == false);
                        if (existingDocUrl.IsNotNull())
                        {
                            existingDocUrl.SampleDocFormURL = tempDocUrl.SampleDocFormURL;
                            existingDocUrl.SampleDocFormURLLabel = tempDocUrl.SampleDocFormURLLabel;
                            existingDocUrl.ModifiedByID = currentLoggedInUserId;
                            existingDocUrl.ModifiedOn = DateTime.Now;
                        }
                    }
                    else
                    {
                        ComplianceCategoryDocUrl complianceCategoryDocUrl = new ComplianceCategoryDocUrl();
                        complianceCategoryDocUrl.SampleDocFormURL = tempDocUrl.SampleDocFormURL;
                        complianceCategoryDocUrl.SampleDocFormURLLabel = tempDocUrl.SampleDocFormURLLabel;
                        complianceCategoryDocUrl.ModifiedByID = AppConsts.NONE;
                        complianceCategoryDocUrl.CreatedByID = currentLoggedInUserId;
                        complianceCategoryDocUrl.CreatedOn = DateTime.Now;
                        complianceCategory.ComplianceCategoryDocUrls.Add(complianceCategoryDocUrl);
                    }
                }
                ComplianceCategoryToDelete(category, currentLoggedInUserId, existingDocUrlList);
            }
            else
            {
                if (isMasterScreen) // Added for UAT-4103
                    ComplianceCategoryToDelete(category, currentLoggedInUserId, existingDocUrlList);
            }


            _ClientDBContext.SaveChanges();
            //get values from database which may be useful in operations afterwards.
            category.Code = complianceCategory.Code;
        }

        private static void ComplianceCategoryToDelete(ComplianceCategory category, int currentLoggedInUserId, List<ComplianceCategoryDocUrl> existingDocUrlList)
        {
            List<Int32> docUrlsIDToBeDeleted = existingDocUrlList.Select(x => x.ComplianceCategoryDocUrlID).Except(category.ComplianceCategoryDocUrls.Select(y => y.ComplianceCategoryDocUrlID)).ToList();
            foreach (Int32 docUrlsID in docUrlsIDToBeDeleted)
            {
                ComplianceCategoryDocUrl delDocUrl = existingDocUrlList.FirstOrDefault(x => x.IsDeleted == false && x.ComplianceCategoryDocUrlID == docUrlsID);
                delDocUrl.IsDeleted = true;
                delDocUrl.ModifiedByID = currentLoggedInUserId;
                delDocUrl.ModifiedOn = DateTime.Now;
            }
        }

        /// <summary>
        /// Deletes the Compliance Category.
        /// </summary>
        /// <param name="category">ComplianceCategory Entity</param>
        /// <param name="currentLoggedInUserId">Current LoggedIn User Id</param>
        /// <returns>True or False</returns>
        public Boolean DeleteComplianceCategory(Int32 categoryID, Int32 currentUserId)
        {
            Boolean packageDependency = false;
            Boolean itemDependency = false;
            String objectTypeCode = LCObjectType.ComplianceCategory.GetStringValue();
            Boolean isCategoryMapRuleSet = false;
            packageDependency = _ClientDBContext.CompliancePackageCategories.Any(obj => obj.CPC_CategoryID == categoryID && obj.CPC_IsDeleted == false);
            itemDependency = _ClientDBContext.ComplianceCategoryItems.Any(obj => obj.CCI_CategoryID == categoryID && obj.CCI_IsDeleted == false);
            if (!(packageDependency || itemDependency))
            {
                ComplianceCategory complianceCategory = ClientDBContext.ComplianceCategories.FirstOrDefault(obj => obj.ComplianceCategoryID == categoryID && obj.IsDeleted == false);
                complianceCategory.IsDeleted = true;
                complianceCategory.ModifiedOn = DateTime.Now;
                complianceCategory.ModifiedByID = currentUserId;

                //doc url changes
                if (complianceCategory.ComplianceCategoryDocUrls.Any(obj => obj.IsDeleted == false))
                {
                    foreach (var docUrl in complianceCategory.ComplianceCategoryDocUrls.Where(obj => obj.IsDeleted == false))
                    {
                        docUrl.IsDeleted = true;
                        docUrl.ModifiedOn = DateTime.Now;
                        docUrl.ModifiedByID = currentUserId;
                    }
                }
                _ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the category name already exists.
        /// </summary>
        /// <param name="categoryName">Category Name</param>
        /// <param name="complianceCategoryId">Compliance Category Id</param>
        /// <param name="tenantId">Tenant Id</param>
        /// <returns>True or false</returns>
        public Boolean CheckIfCategoryNameAlreadyExist(String categoryName, Int32 complianceCategoryId, Int32 tenantId)
        {
            return ClientDBContext.ComplianceCategories.Any(obj => obj.CategoryName.Trim().ToUpper() == categoryName.Trim().ToUpper() && obj.ComplianceCategoryID != complianceCategoryId && obj.IsDeleted == false && obj.TenantID == tenantId);
        }

        /// <summary>
        /// To get Not Mapped Compliance Categories
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public List<ComplianceCategory> GetNotMappedComplianceCategories(Int32 packageId)
        {
            //UAT-2069 : Passing multiple package ids to GetcomplianceCategoriesByPackage() method
            List<Int32> lstPackageIds = new List<Int32>();
            lstPackageIds.Add(packageId);
            List<Int32> categoryIds = GetcomplianceCategoriesByPackage(lstPackageIds, false, null).Select(x => x.CPC_CategoryID).ToList();

            return _ClientDBContext.ComplianceCategories
                .Where(cond =>
                    !cond.IsDeleted
                    && !categoryIds.Contains(cond.ComplianceCategoryID)).ToList();

        }

        /// <summary>
        /// Update the Display order in Compliance Package Categories 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="packageId"></param>
        /// <param name="displayOrder"></param>
        /// <param name="currentloggedInUserId"></param>
        /// <returns></returns>
        public Boolean UpdateCompliancePackageCategoryDisplayOrder(CompliancePackageCategory compliancePackageCategory, Int32 currentloggedInUserId)
        {
            CompliancePackageCategory compliancePackageCategoryInDb = _ClientDBContext.CompliancePackageCategories.Where(c => c.CPC_CategoryID == compliancePackageCategory.CPC_CategoryID
                                                                                                                           && c.CPC_PackageID == compliancePackageCategory.CPC_PackageID
                                                                                                                           && c.CPC_IsActive == true && c.CPC_IsDeleted == false).FirstOrDefault();

            if (compliancePackageCategoryInDb.IsNotNull())
            {
                compliancePackageCategoryInDb.CPC_DisplayOrder = compliancePackageCategory.CPC_DisplayOrder;
                compliancePackageCategoryInDb.CPC_ComplianceRequired = compliancePackageCategory.CPC_ComplianceRequired;
                compliancePackageCategoryInDb.CPC_ComplianceRqdStartDate = compliancePackageCategory.CPC_ComplianceRqdStartDate;
                compliancePackageCategoryInDb.CPC_ComplianceRqdEndDate = compliancePackageCategory.CPC_ComplianceRqdEndDate;
                compliancePackageCategoryInDb.CPC_ModifiedByID = currentloggedInUserId;
                compliancePackageCategoryInDb.CPC_ModifiedOn = DateTime.Now;
                _ClientDBContext.SaveChanges();
                compliancePackageCategory.CPC_ID = compliancePackageCategoryInDb.CPC_ID;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get Compliance Package Category object
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public CompliancePackageCategory GetCompliancePackageCategory(Int32 categoryId, Int32 packageId)
        {
            var CompliancePackageCategory = _ClientDBContext.CompliancePackageCategories.Where(c => c.CPC_CategoryID == categoryId && c.CPC_PackageID == packageId && c.CPC_IsActive == true && c.CPC_IsDeleted == false).FirstOrDefault();

            if (CompliancePackageCategory.IsNotNull())
            {
                return CompliancePackageCategory;
            }
            return null;
        }
        #endregion

        #region Manage Compliance Items

        public List<ComplianceItem> GetComplianceItems(Boolean getTenantName, List<Entity.Tenant> tenantList)
        {
            List<ComplianceItem> complianceItems = _ClientDBContext.ComplianceItems.Where(cmpItem => cmpItem.IsDeleted == false).OrderBy(x => x.Name).ToList();
            if (getTenantName)
            {
                //SecurityRepository securityRepository = new SecurityRepository();
                //var tenantList = ((ISecurityRepository)securityRepository).GetTenants(false, false).ToList();
                complianceItems.Where(t => t.TenantID != null)
                    .ForEach(t =>
                    {
                        var tenant = tenantList.FirstOrDefault(item => item.TenantID == t.TenantID.Value);
                        if (tenant.IsNotNull())
                        {
                            t.TenantName = tenant.TenantName;
                        }
                        else
                        {
                            t.TenantName = String.Empty;
                        }
                    });
            }

            return complianceItems;
        }

        public List<ComplianceItem> GetComplianceItemsForNodes(string dpmIds)
        {
            List<ComplianceItem> complianceItems = new List<ComplianceItem>();

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("dbo.usp_GetComplianceItemsOfNode", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@dpmIds", dpmIds);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);


                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    complianceItems = ds.Tables[0].AsEnumerable().Select(col =>
                         new ComplianceItem
                         {
                             ComplianceItemID = Convert.ToInt32(col["ComplianceItemID"]),
                             ComplianceItemTypeID = Convert.ToInt32(col["ComplianceItemTypeID"].ToString()),
                             Name = col["Name"].ToString(),
                             Description = col["Description"].ToString(),
                             EffectiveDate = col["EffectiveDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["EffectiveDate"]),
                             ItemLabel = col["ItemLabel"].ToString(),
                             ScreenLabel = col["ScreenLabel"].ToString(),
                             IsActive = col["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(col["IsActive"]),
                             TenantID = col["TenantID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["TenantID"]),
                             IsCreatedByAdmin = col["IsCreatedByAdmin"] == DBNull.Value ? false : Convert.ToBoolean(col["IsCreatedByAdmin"]),
                             IsDissociated = col["IsDissociated"] == DBNull.Value ? false : Convert.ToBoolean(col["IsDissociated"]),
                             DissociatedFrom = col["DissociatedFrom"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["DissociatedFrom"]),
                             SampleDocFormURL = col["SampleDocFormURL"].ToString(),
                             Details = col["Details"].ToString(),
                             Amount = col["Amount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(col["Amount"]),
                             //SampleDocFormURLLabel = col["SampleDocFormURLLabel"].ToString(),
                             IsPaymentType = col["IsPaymentType"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(col["IsPaymentType"]),
                             IsWithMultipleNodes = col["IsWithMultipleNodes"].ToString()
                         }).ToList();

                    List<ComplianceItemDocUrl> ComplianceItemDocUrls = ds.Tables[1].AsEnumerable().Select(col =>
                         new ComplianceItemDocUrl
                         {
                             ComplianceItemDocUrlID = Convert.ToInt32(col["ComplianceItemDocUrlID"]),
                             ComplianceItemID = Convert.ToInt32(col["ComplianceItemID"]),
                             SampleDocFormURL = col["SampleDocFormURL"].ToString(),
                             SampleDocFormDisplayURLLabel = col["SampleDocFormDisplayURLLabel"].ToString()
                         }).ToList();

                    if (ComplianceItemDocUrls != null && ComplianceItemDocUrls.Any())
                    {
                        complianceItems
                            .Where(cc =>
                            ComplianceItemDocUrls.Any(ccd => ccd.ComplianceItemID == cc.ComplianceItemID))
                            .ForEach(cc =>
                            {
                                cc.ComplianceItemDocUrls = new EntityCollection<ComplianceItemDocUrl>();
                                ComplianceItemDocUrls.Where(ccd => ccd.ComplianceItemID == cc.ComplianceItemID).ForEach(doc =>
                                {
                                    cc.ComplianceItemDocUrls.Add(doc);
                                });
                            });
                    }

                }
            }

            return complianceItems;
        }

        /// <summary>
        /// Get the list of Items not associated with the current Category in the mapping screen.
        /// </summary>
        /// <param name="currentCategoryId">Id of the category not associated with</param>
        /// <returns>Lit of the compliance items.</returns>
        public List<ComplianceItem> GetAvailableComplianceItems(Int32 currentCategoryId)
        {
            List<Int32> lstAssociatedItems = _ClientDBContext.ComplianceCategoryItems.Where(cci => cci.CCI_CategoryID == currentCategoryId && cci.CCI_IsDeleted == false).Select(cci => cci.CCI_ItemID).ToList();
            return _ClientDBContext.ComplianceItems.Where(cmpItem => !lstAssociatedItems.Contains(cmpItem.ComplianceItemID) && cmpItem.IsDeleted == false).ToList();
        }

        /// <summary>
        /// Delete the selected compliance item
        /// </summary>
        /// <param name="complianceItemId">Id of the item to delete</param>
        /// <returns>Status of deletion, if it was success or association exists</returns>
        public Boolean DeleteComplianceItem(Int32 complianceItemId, Int32 currentUserId)
        {
            Boolean _isAttributeLinked = false;
            Boolean _isCategoryLinked = ClientDBContext.ComplianceCategoryItems.Where(cci => cci.CCI_ItemID == complianceItemId && cci.CCI_IsDeleted == false).Any();

            if (_isCategoryLinked)
                return false;
            else
            {
                _isAttributeLinked = ClientDBContext.ComplianceItemAttributes.Where(cia => cia.CIA_ItemID == complianceItemId && cia.CIA_IsDeleted == false).Any();
                if (_isAttributeLinked)
                    return false;
                else
                {
                    ComplianceItem complianceItem = ClientDBContext.ComplianceItems.Where(cmpItem => cmpItem.ComplianceItemID == complianceItemId).FirstOrDefault();

                    complianceItem.IsDeleted = true;
                    complianceItem.ModifiedOn = DateTime.Now;
                    complianceItem.ModifiedBy = currentUserId;

                    //doc url changes
                    if (complianceItem.ComplianceItemDocUrls.Any(obj => obj.IsDeleted == false))
                    {
                        foreach (var docUrl in complianceItem.ComplianceItemDocUrls.Where(obj => obj.IsDeleted == false))
                        {
                            docUrl.IsDeleted = true;
                            docUrl.ModifiedOn = DateTime.Now;
                            docUrl.ModifiedByID = currentUserId;
                        }
                    }

                    ClientDBContext.SaveChanges();
                    return true;
                }
            }
        }

        /// <summary>
        /// Save/Update the compliance item
        /// </summary>
        /// <param name="complianceItem">Details of the compliance item to save/update</param>
        public ComplianceItem SaveComplianceItem(ComplianceItem complianceItem, Int32 tenantId)
        {
            complianceItem.Code = complianceItem.Code.Equals(Guid.Empty) ? Guid.NewGuid() : complianceItem.Code;
            complianceItem.CreatedOn = DateTime.Now;
            complianceItem.IsDeleted = false;
            complianceItem.ComplianceItemTypeID = 1;
            //if (complianceItem.ComplianceCategoryItems.Count > 0)
            //{
            //    Int32 _lastDisplayOrder = _ClientDBContext.ComplianceCategoryItems.OrderByDescending(order => order.CCI_ID).Select(order => order.CCI_DisplayOrder).FirstOrDefault();
            //    complianceItem.ComplianceCategoryItems.FirstOrDefault().CCI_DisplayOrder = _lastDisplayOrder + 1;
            //}

            ClientDBContext.ComplianceItems.AddObject(complianceItem);
            ClientDBContext.SaveChanges();
            return complianceItem;
        }


        /// <summary>
        /// Gets the unique name availability
        /// </summary>
        /// <param name="complianceItem"></param>
        /// <returns></returns>
        public Boolean CheckIfItemAlreadyExist(String complianceItem, Int32 complianceItemId, Int32 tenantId)
        {
            return _ClientDBContext.ComplianceItems.Any(x => x.Name.ToLower().Equals(complianceItem.ToLower()) && x.ComplianceItemID != complianceItemId && x.IsDeleted == false && x.TenantID == tenantId);
        }



        /// <summary>
        /// Update the compliance item
        /// </summary>
        /// <param name="complianceItem">Details of the compliance item to update</param>
        public void UpdateComplianceItem(ComplianceItem complianceItem, Int32 tenantId, Boolean isMasterScreen = false)
        {

            ComplianceItem cmpItemToUpdate = _ClientDBContext.ComplianceItems.Where(cmpItem => cmpItem.ComplianceItemID == complianceItem.ComplianceItemID).FirstOrDefault();
            cmpItemToUpdate.Name = complianceItem.Name;
            cmpItemToUpdate.Description = complianceItem.Description;
            cmpItemToUpdate.Details = complianceItem.Details;
            cmpItemToUpdate.EffectiveDate = complianceItem.EffectiveDate;
            cmpItemToUpdate.ItemLabel = complianceItem.ItemLabel;
            cmpItemToUpdate.ScreenLabel = complianceItem.ScreenLabel;
            cmpItemToUpdate.IsActive = complianceItem.IsActive;
            cmpItemToUpdate.SampleDocFormURL = complianceItem.SampleDocFormURL;
            cmpItemToUpdate.ModifiedBy = complianceItem.ModifiedBy;
            cmpItemToUpdate.ModifiedOn = DateTime.Now;
            cmpItemToUpdate.ComplianceItemTypeID = cmpItemToUpdate.ComplianceItemTypeID; // To be removed if column not required. Currently added to handle foreign ket constraint.
            //previousComplianceItem.ExplanatoryNotes = complianceItem.ExplanatoryNotes;
            //UAT-3077
            cmpItemToUpdate.IsPaymentType = complianceItem.IsPaymentType;
            cmpItemToUpdate.Amount = complianceItem.Amount;
            var existingDocUrlList = cmpItemToUpdate.ComplianceItemDocUrls.Where(obj => obj.IsDeleted == false).ToList();
            if (complianceItem.ComplianceItemDocUrls.Any())
            {
                foreach (var tempDocUrl in complianceItem.ComplianceItemDocUrls)
                {
                    ComplianceItemDocUrl existingDocUrl = cmpItemToUpdate.ComplianceItemDocUrls.FirstOrDefault(obj => obj.ComplianceItemDocUrlID == tempDocUrl.ComplianceItemDocUrlID && obj.IsDeleted == false);
                    if (tempDocUrl.ComplianceItemDocUrlID > 0)
                    {
                        if (existingDocUrl.IsNotNull())
                        {
                            existingDocUrl.SampleDocFormURL = tempDocUrl.SampleDocFormURL;
                            existingDocUrl.SampleDocFormDisplayURLLabel = tempDocUrl.SampleDocFormDisplayURLLabel;
                            existingDocUrl.ModifiedByID = complianceItem.ModifiedBy;
                            existingDocUrl.ModifiedOn = DateTime.Now;
                        }
                    }
                    else
                    {
                        ComplianceItemDocUrl ComplianceItemDocUrl = new ComplianceItemDocUrl();
                        ComplianceItemDocUrl.SampleDocFormURL = tempDocUrl.SampleDocFormURL;
                        ComplianceItemDocUrl.SampleDocFormDisplayURLLabel = tempDocUrl.SampleDocFormDisplayURLLabel;
                        ComplianceItemDocUrl.ModifiedByID = AppConsts.NONE;
                        ComplianceItemDocUrl.CreatedByID = complianceItem.ModifiedBy;
                        ComplianceItemDocUrl.CreatedOn = DateTime.Now;
                        cmpItemToUpdate.ComplianceItemDocUrls.Add(ComplianceItemDocUrl);
                    }
                }
                CpmplianceItemsToDelete(complianceItem, existingDocUrlList);
            }
            else
            {
                if (isMasterScreen) // Added for UAT-4103
                    CpmplianceItemsToDelete(complianceItem, existingDocUrlList);
            }

            ClientDBContext.SaveChanges();

            //get values from database which may be useful in operations afterwards.
            complianceItem.Code = cmpItemToUpdate.Code;
        }

        private static void CpmplianceItemsToDelete(ComplianceItem complianceItem, List<ComplianceItemDocUrl> existingDocUrlList)
        {
            List<Int32> docUrlsIDToBeDeleted = existingDocUrlList.Select(x => x.ComplianceItemDocUrlID).Except(complianceItem.ComplianceItemDocUrls.Select(y => y.ComplianceItemDocUrlID)).ToList();
            foreach (Int32 docUrlsID in docUrlsIDToBeDeleted)
            {
                ComplianceItemDocUrl delDocUrl = existingDocUrlList.FirstOrDefault(x => x.IsDeleted == false && x.ComplianceItemDocUrlID == docUrlsID);
                delDocUrl.IsDeleted = true;
                delDocUrl.ModifiedByID = complianceItem.ModifiedBy;
                delDocUrl.ModifiedOn = DateTime.Now;
            }
        }

        public Boolean UpdateComplianceCategoryItemDisplayOrder(Int32 itemId, Int32 categoryId, Int32 displayOrder, Int32 currentloggedInUserId)
        {
            ComplianceCategoryItem complianceCategoryItem = _ClientDBContext.ComplianceCategoryItems.Where(c => c.CCI_CategoryID == categoryId && c.CCI_ItemID == itemId && c.CCI_IsActive && !c.CCI_IsDeleted).FirstOrDefault();

            if (complianceCategoryItem.IsNotNull())
            {
                complianceCategoryItem.CCI_DisplayOrder = displayOrder;
                complianceCategoryItem.CCI_ModifiedByID = currentloggedInUserId;
                complianceCategoryItem.CCI_ModifiedOn = DateTime.Now;
                _ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        #endregion

        #region Manage Compliance Attributes

        public List<ComplianceAttribute> GetComplianceAttributes(Boolean getTenantName, List<Entity.Tenant> tenantList)
        {
            List<ComplianceAttribute> complianceAttributes =
            _ClientDBContext.ComplianceAttributes.Include("ComplianceAttributeGroup")
                .Where(complianceAttribute =>
                    !complianceAttribute.IsDeleted).ToList();

            if (getTenantName)
            {
                complianceAttributes.Where(t => t.TenantID != null)
                     .ForEach(t =>
                     {
                         var tenant = tenantList.FirstOrDefault(item => item.TenantID == t.TenantID.Value);
                         if (tenant.IsNotNull())
                         {
                             t.TenantName = tenant.TenantName;
                         }
                         else
                         {
                             t.TenantName = String.Empty;
                         }
                     });
            }

            return complianceAttributes;
        }

        //public List<ComplianceAttribute> GetNotMappedComplianceAttributes(Int32 itemId, Int32 fileUploadAttributeDatatypeId, Int32 viewDocAttributeDataTypeId)
        //{
        //    List<Int32> attributeIds = GetComplianceItemAttribute(itemId, false, null).Select(x => x.CIA_AttributeID).ToList();
        //    if (_ClientDBContext.ComplianceAttributes.Any(complianceAttribute => !complianceAttribute.IsDeleted
        //            && (attributeIds.Contains(complianceAttribute.ComplianceAttributeID)
        //            && complianceAttribute.ComplianceAttributeDatatypeID == fileUploadAttributeDatatypeId)
        //            ))
        //    {
        //        return _ClientDBContext.ComplianceAttributes.Where(complianceAttribute => !complianceAttribute.IsDeleted
        //                && (!attributeIds.Contains(complianceAttribute.ComplianceAttributeID) && complianceAttribute.ComplianceAttributeDatatypeID != fileUploadAttributeDatatypeId)
        //                ).ToList();
        //    }
        //    return _ClientDBContext.ComplianceAttributes.Where(complianceAttribute => !complianceAttribute.IsDeleted
        //               && !attributeIds.Contains(complianceAttribute.ComplianceAttributeID)
        //               ).ToList();
        //}


        //UAT - 1559: As an admin, I should be able to attach a form to be completed as a immunization package attribute
        public List<ComplianceAttribute> GetNotMappedComplianceAttributes(Int32 itemId, Int32 fileUploadAttributeDatatypeId, Int32 viewDocAttributeDataTypeId)
        {
            List<Int32> attributeIds = GetComplianceItemAttribute(itemId, false, null).Select(x => x.CIA_AttributeID).ToList();
            List<ComplianceAttribute> lstAllAttributesNotMappedInItem = _ClientDBContext.ComplianceAttributes.Where(complianceAttribute => !complianceAttribute.IsDeleted
                                                                                     && !attributeIds.Contains(complianceAttribute.ComplianceAttributeID)).ToList();
            List<ComplianceAttribute> lstAllComplianceAttributeMappedInItem = _ClientDBContext.ComplianceAttributes
                                                                                    .Where(complianceAttribute => !complianceAttribute.IsDeleted
                                                                                          && attributeIds.Contains(complianceAttribute.ComplianceAttributeID)).ToList();
            if (lstAllComplianceAttributeMappedInItem.Any(complianceAttribute => complianceAttribute.ComplianceAttributeDatatypeID == fileUploadAttributeDatatypeId))
            {
                lstAllAttributesNotMappedInItem = lstAllAttributesNotMappedInItem
                                                        .Where(cond => cond.ComplianceAttributeDatatypeID != fileUploadAttributeDatatypeId).ToList();
            }
            if (lstAllComplianceAttributeMappedInItem.Any(complianceAttribute => complianceAttribute.ComplianceAttributeDatatypeID == viewDocAttributeDataTypeId))
            {
                lstAllAttributesNotMappedInItem = lstAllAttributesNotMappedInItem
                                                        .Where(cond => cond.ComplianceAttributeDatatypeID != viewDocAttributeDataTypeId).ToList();
            }
            return lstAllAttributesNotMappedInItem;
        }

        /// <summary>
        /// To get the Presence of FileUpload Type Attribute in item
        /// </summary>
        /// <param name="itemId">ItemId</param>
        /// <returns></returns>
        public Boolean IsFileUploadAttributePresent(Int32 itemId, Int32 fileUploadAttributeDatatypeId)
        {
            List<Int32> attributeIds = GetComplianceItemAttribute(itemId, false, null).Select(x => x.CIA_AttributeID).ToList();
            String complianceAttributeDatatypeCode = ComplianceAttributeDatatypes.FileUpload.GetStringValue();
            if (_ClientDBContext.ComplianceAttributes.Any(complianceAttribute => !complianceAttribute.IsDeleted
                    && (attributeIds.Contains(complianceAttribute.ComplianceAttributeID)
                    && complianceAttribute.ComplianceAttributeDatatypeID == fileUploadAttributeDatatypeId)
                    ))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// To get the Presence of View Document Type Attribute in item
        /// </summary>
        /// <param name="itemId">ItemId</param>
        /// <returns></returns>
        public Boolean IsViewDocAttributePresent(Int32 itemId, Int32 viewDocAttributeDatatypeId)
        {
            List<Int32> attributeIds = GetComplianceItemAttribute(itemId, false, null).Select(x => x.CIA_AttributeID).ToList();
            String complianceAttributeDatatypeCode = ComplianceAttributeDatatypes.FileUpload.GetStringValue();
            if (_ClientDBContext.ComplianceAttributes.Any(complianceAttribute => !complianceAttribute.IsDeleted
                    && (attributeIds.Contains(complianceAttribute.ComplianceAttributeID)
                    && complianceAttribute.ComplianceAttributeDatatypeID == viewDocAttributeDatatypeId)
                    ))
            {
                return false;
            }
            return true;
        }

        public ComplianceAttribute GetComplianceAttribute(Int32 complianceAttributeID)
        {
            return ClientDBContext.ComplianceAttributes
                .FirstOrDefault(complianceAttribute => complianceAttribute.ComplianceAttributeID.Equals(complianceAttributeID)
                && !complianceAttribute.IsDeleted);
        }

        /// <summary>
        /// To get compliance attribute group list
        /// </summary>
        /// <returns></returns>
        public List<ComplianceAttributeGroup> GetComplianceAttributeGroup()
        {
            return _ClientDBContext.ComplianceAttributeGroups.Where(x => x.CAG_IsDeleted == false).ToList();
        }

        public Boolean AddComplianceAttribute(ComplianceAttribute complianceAttribute)
        {
            complianceAttribute.Code = complianceAttribute.Code.Equals(Guid.Empty) ? Guid.NewGuid() : complianceAttribute.Code;
            ClientDBContext.ComplianceAttributes.AddObject(complianceAttribute);
            ClientDBContext.SaveChanges();
            return true;
        }

        public Boolean UpdateComplianceAttribute(ComplianceAttribute complianceAttribute)
        {
            ComplianceAttribute complianceAttributeInDb = GetComplianceAttribute(complianceAttribute.ComplianceAttributeID);

            if (complianceAttributeInDb != null)
            {
                complianceAttributeInDb.ComplianceAttributeTypeID = complianceAttribute.ComplianceAttributeTypeID;
                complianceAttributeInDb.ComplianceAttributeDatatypeID = complianceAttribute.ComplianceAttributeDatatypeID;
                complianceAttributeInDb.Name = complianceAttribute.Name;
                complianceAttributeInDb.AttributeLabel = complianceAttribute.AttributeLabel;
                complianceAttributeInDb.ScreenLabel = complianceAttribute.ScreenLabel;
                complianceAttributeInDb.Description = complianceAttribute.Description;
                complianceAttributeInDb.MaximumCharacters = complianceAttribute.MaximumCharacters;
                complianceAttributeInDb.IsActive = complianceAttribute.IsActive;
                //complianceAttributeInDb.CopiedFromCode = complianceAttribute.Code;
                complianceAttributeInDb.ModifiedByID = complianceAttribute.ModifiedByID;
                complianceAttributeInDb.ModifiedOn = complianceAttribute.ModifiedOn;
                complianceAttributeInDb.ComplianceAttributeGroupID = complianceAttribute.ComplianceAttributeGroupID;

                //UAT-2023: Reconciliation: Addition of attribute-level setting to enable/disable trigger for reconciliation queue.
                complianceAttributeInDb.IsTriggersReconciliation = complianceAttribute.IsTriggersReconciliation;
                ///Commit4383
                complianceAttributeInDb.IsSendForintegration = complianceAttribute.IsSendForintegration;

                UpdateComplianceAttributeOption(complianceAttributeInDb, complianceAttribute);

                UpdateComplianceAttributeDocument(complianceAttribute, complianceAttributeInDb);
                UpdateComplianceFileAttributeDocs(complianceAttribute, complianceAttributeInDb);//Added In UAT-4554
                _ClientDBContext.SaveChanges();
                //get values from database which may be useful in operations afterwards.
                complianceAttribute.Code = complianceAttributeInDb.Code;
                return true;
            }
            return false;
        }

        private static void UpdateComplianceAttributeDocument(ComplianceAttribute complianceAttribute, ComplianceAttribute complianceAttributeInDb)
        {
            ComplianceAttributeDocument doc = complianceAttribute.ComplianceAttributeDocuments.FirstOrDefault();
            ComplianceAttributeDocument docInDB = complianceAttributeInDb.ComplianceAttributeDocuments.Where(cond => !cond.CAD_IsDeleted).FirstOrDefault();
            if (!docInDB.IsNullOrEmpty())
            {
                if (doc.IsNullOrEmpty())
                {
                    docInDB.CAD_IsDeleted = true;
                    docInDB.CAD_ModifiedBy = complianceAttribute.ModifiedByID;
                    docInDB.CAD_ModifiedOn = complianceAttribute.ModifiedOn;
                }
                else
                {

                    docInDB.CAD_ModifiedBy = doc.CAD_CreatedBy;
                    docInDB.CAD_ModifiedOn = doc.CAD_CreatedOn;
                    docInDB.CAD_DocumentID = doc.CAD_DocumentID;
                }
            }
            else if (!doc.IsNullOrEmpty())
            {
                if (complianceAttributeInDb.ComplianceAttributeDocuments.IsNull())
                {
                    complianceAttributeInDb.ComplianceAttributeDocuments = new EntityCollection<ComplianceAttributeDocument>();
                }
                complianceAttributeInDb.ComplianceAttributeDocuments.Add(new ComplianceAttributeDocument()
                {
                    CAD_CreatedBy = doc.CAD_CreatedBy,
                    CAD_CreatedOn = doc.CAD_CreatedOn,
                    CAD_IsDeleted = false,
                    CAD_DocumentID = doc.CAD_DocumentID
                });
            }
        }


        public Boolean DeleteComplianceAttribute(Int32 complianceAttributeID, Int32 modifiedByID)
        {
            if (ClientDBContext.RuleSetObjects.Any(x => x.RLSO_ObjectID == complianceAttributeID && !x.RLSO_IsDeleted))
                return false;
            ComplianceAttribute complianceAttribute = GetComplianceAttribute(complianceAttributeID);
            if (complianceAttribute != null)
            {
                complianceAttribute.IsDeleted = true;
                complianceAttribute.ModifiedByID = modifiedByID;
                complianceAttribute.ModifiedOn = DateTime.Now;
                _ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }


        #region Private

        private Boolean UpdateComplianceAttributeOption(ComplianceAttribute complianceAttributeInDb, ComplianceAttribute complianceAttribute)
        {
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<ComplianceAttributeOption> attributeOptions = complianceAttribute.ComplianceAttributeOptions;

            IEnumerable<ComplianceAttributeOption> attributeOptionsInDb = complianceAttributeInDb.ComplianceAttributeOptions
                .Where(x => x.IsActive && !x.IsDeleted);

            // Deletes attribute options
            foreach (ComplianceAttributeOption attributeOptionIndb in attributeOptionsInDb)
            {
                if (attributeOptions.Any(x => x.OptionText == attributeOptionIndb.OptionText
                    && x.OptionValue == attributeOptionIndb.OptionValue))
                    continue;

                attributeOptionIndb.IsDeleted = true;
                attributeOptionIndb.ModifiedOn = DateTime.Now;
                attributeOptionIndb.ModifiedByID = complianceAttribute.ModifiedByID;
            }

            List<ComplianceAttributeOption> needToAddAttributeOptions = new List<ComplianceAttributeOption>();
            // Adds attribute options
            foreach (ComplianceAttributeOption attributeOption in attributeOptions)
            {
                if (attributeOptionsInDb.Any(x => x.OptionText == attributeOption.OptionText
                    && x.OptionValue == attributeOption.OptionValue))
                    continue;

                needToAddAttributeOptions.Add(attributeOption);
            }

            foreach (ComplianceAttributeOption addAttributeOption in needToAddAttributeOptions)
                complianceAttributeInDb.ComplianceAttributeOptions.Add(addAttributeOption);

            return true;
        }

        #endregion
        #endregion

        #region Manage Compliance RuleSet

        public RuleSet SaveComplianceRuleSetDetail(RuleSet ruleSet, Int32 currentUserId, Int32 tenantId)
        {
            ruleSet.RLS_Code = ruleSet.RLS_Code.Equals(Guid.Empty) ? Guid.NewGuid() : ruleSet.RLS_Code;
            ruleSet.RLS_IsDeleted = false;
            ruleSet.RLS_CreatedByID = currentUserId;
            ruleSet.RLS_CreatedOn = DateTime.Now;
            ClientDBContext.RuleSets.AddObject(ruleSet);
            ClientDBContext.SaveChanges();
            return ruleSet;
        }

        public RuleSet UpdateComplianceRuleSetDetail(RuleSet ruleSet, Int32 currentUserId, Int32 tenantId)
        {
            RuleSet complianceRuleSet = _ClientDBContext.RuleSets.FirstOrDefault(obj => obj.RLS_ID == ruleSet.RLS_ID && obj.RLS_IsDeleted == false);
            complianceRuleSet.RLS_ID = ruleSet.RLS_ID;
            complianceRuleSet.RLS_Name = ruleSet.RLS_Name;
            complianceRuleSet.RLS_Description = ruleSet.RLS_Description;
            complianceRuleSet.RLS_StartDate = ruleSet.RLS_StartDate;
            //complianceRuleSet.RLS_RuleSetType = ruleSet.RLS_RuleSetType;
            complianceRuleSet.RLS_IsActive = ruleSet.RLS_IsActive;
            complianceRuleSet.RLS_IsDeleted = false;
            complianceRuleSet.RLS_ModifiedByID = currentUserId;
            complianceRuleSet.RLS_ModifiedOn = DateTime.Now;
            _ClientDBContext.SaveChanges();
            return complianceRuleSet;
        }

        public Boolean DeleteComplianceRuleSetDetail(Int32 ruleSetId, Int32 currentUserId, Int32 tenantId)
        {
            if (ClientDBContext.RuleSetObjects.Any(x => x.RLSO_RuleSetID == ruleSetId && !x.RLSO_IsDeleted))
                return false;

            RuleSet complianceRuleSet = _ClientDBContext.RuleSets.FirstOrDefault(obj => obj.RLS_ID == ruleSetId && obj.RLS_IsDeleted == false);
            if (complianceRuleSet != null)
            {
                complianceRuleSet.RLS_IsDeleted = true;
                complianceRuleSet.RLS_ModifiedOn = DateTime.Now;
                complianceRuleSet.RLS_ModifiedByID = currentUserId;
                _ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        #endregion

        #region Manage Comlianc Rules

        /// <summary>
        /// Gets all the rows from table RuleMappings.
        /// </summary>
        /// <returns>List of type RuleMapping</returns>
        public List<RuleMapping> GetAllRuleMappings()
        {
            return ClientDBContext.RuleMappings.Where(obj => obj.RLM_IsDeleted == false).ToList(); ;
        }

        /// <summary>
        /// Gets all the rows from table RuleTemplateExpression.
        /// </summary>
        /// <returns>List of type RuleTemplateExpression</returns>
        public List<RuleTemplateExpression> GetAllRuleTemplateExpression()
        {
            return ClientDBContext.RuleTemplateExpressions.Where(obj => obj.RLE_IsDeleted == false).ToList(); ;
        }

        /// <summary>
        /// Gets all the rows from table RuleMappingDetail.
        /// </summary>
        /// <returns>List of type RuleMappingDetail</returns>
        public List<RuleMappingDetail> GetAllRuleMappingDetails()
        {
            return ClientDBContext.RuleMappingDetails.Where(obj => obj.RLMD_IsDeleted == false).ToList(); ;
        }

        /// <summary>
        /// Gets all the rows from table RuleMappingObjectTree.
        /// </summary>
        /// <returns>List of type RuleMappingObjectTree</returns>
        public List<RuleMappingObjectTree> GetAllRuleMappingObjectTree()
        {
            return ClientDBContext.RuleMappingObjectTrees.Select(x => x).ToList(); ;
        }

        #endregion

        #endregion

        #region Admin Mapping Screens

        #region Package-Category Mapping

        public List<CompliancePackage> GetCompliancePackagesByPermissionList(Int32 tenantId, String dpmIds, Int32 OrganisationId, Boolean IsAdminLoggedIn)
        {
            List<CompliancePackage> compliancePackages = new List<CompliancePackage>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("dbo.usp_GetCompliancePackagesByPermission", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrganisationUserID", OrganisationId);
                command.Parameters.AddWithValue("@dpmIds", dpmIds);
                command.Parameters.AddWithValue("@IsAdmin", IsAdminLoggedIn);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    compliancePackages = ds.Tables[0].AsEnumerable().Select(col =>
                         new CompliancePackage
                         {
                             CompliancePackageID = Convert.ToInt32(col["CompliancePackageID"]),
                             PackageName = col["PackageName"].ToString(),
                         }).ToList();
                }
            }
            return compliancePackages;
        }

        public List<ComplianceCategory> GetComplianceCategoriesByPermissionList(List<Entity.Tenant> tenentIds, Int32 tenantId, List<Int32> packageIds, String dpmIds, Int32? OrganisationId, Boolean IsAdminLoggedIn)
        {
            List<ComplianceCategory> complianceCategories = new List<ComplianceCategory>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            if (packageIds.IsNullOrEmpty())
            {
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {

                    SqlCommand command = new SqlCommand("dbo.usp_GetComplianceCategoryiesByPermission", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrganisationUserID", OrganisationId);
                    command.Parameters.AddWithValue("@dpmIds", dpmIds);
                    command.Parameters.AddWithValue("@IsAdmin", IsAdminLoggedIn);
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = command;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > AppConsts.NONE)
                    {
                        complianceCategories = ds.Tables[0].AsEnumerable().Select(col =>
                             new ComplianceCategory
                             {
                                 ComplianceCategoryID = Convert.ToInt32(col["ComplianceCategoryID"]),
                                 CategoryName = col["CategoryName"].ToString(),
                             }).ToList();
                    }
                }
            }
            else
            {
                complianceCategories = _ClientDBContext.CompliancePackageCategories
                                    .Include("ComplianceCategory")
                                    .Where(obj => packageIds.Contains(obj.CPC_PackageID) && obj.CPC_IsDeleted == false && obj.ComplianceCategory.IsDeleted == false).Select(x => x.ComplianceCategory).Distinct().ToList();
            }
            return complianceCategories;
        }


        public List<ComplianceCategory> GetcomplianceCategoriesByPackageList(Int32 packageId, List<Entity.Tenant> tenantList, Int32 tenantId = 0)
        {
            var categoryList = _ClientDBContext.CompliancePackageCategories
                                                          .Where(obj => obj.CPC_PackageID == packageId
                                                          && obj.CPC_IsDeleted == false).ToList();
            List<ComplianceCategory> mappedCategories = categoryList.Select(obj => obj.ComplianceCategory).ToList();
            mappedCategories.Where(t => t.TenantID != null)
                                   .ForEach(t =>
                                   {
                                       var tenant = tenantList.FirstOrDefault(item => item.TenantID == t.TenantID.Value);
                                       if (tenant.IsNotNull())
                                       {
                                           t.TenantName = tenant.TenantName;
                                       }
                                       else
                                       {
                                           t.TenantName = String.Empty;
                                       }
                                   });
            ComplianceSetupRepository complianceSetupRepository = new ComplianceSetupRepository(tenantId);

            //13-02-2014  Changes Done for -"Category listing screen save performance improvement". 
            mappedCategories.ForEach(d =>
                {
                    var category = categoryList.FirstOrDefault(cat => cat.CPC_CategoryID == d.ComplianceCategoryID);
                    d.DisplayOrder = (category.IsNotNull()) ? category.CPC_DisplayOrder : 0;
                });

            return mappedCategories;
        }

        public List<CompliancePackageCategory> GetcomplianceCategoriesByPackage(List<Int32> packageIds, Boolean getTenantName, List<Entity.Tenant> tenantList)
        {

            List<CompliancePackageCategory> mappedCategories = new List<CompliancePackageCategory>();
            //UAT-2069 : - Passing null or more than one package id to this method.
            if (packageIds.IsNullOrEmpty())
            {
                mappedCategories = _ClientDBContext.CompliancePackageCategories
                                                    .Include("ComplianceCategory")
                                                    .Where(obj => obj.CPC_IsDeleted == false && obj.ComplianceCategory.IsDeleted == false).ToList();

            }
            else
            {
                mappedCategories = _ClientDBContext.CompliancePackageCategories
                                    .Include("ComplianceCategory")
                                    .Where(obj => packageIds.Contains(obj.CPC_PackageID) && obj.CPC_IsDeleted == false && obj.ComplianceCategory.IsDeleted == false).ToList();
            }

            if (getTenantName)
            {
                mappedCategories.Where(t => t.ComplianceCategory.TenantID != null)
                     .ForEach(t =>
                     {
                         var tenant = tenantList.FirstOrDefault(item => item.TenantID == t.ComplianceCategory.TenantID.Value);
                         if (tenant.IsNotNull())
                         {
                             t.ComplianceCategory.TenantName = tenant.TenantName;
                         }
                         else
                         {
                             t.ComplianceCategory.TenantName = String.Empty;
                         }
                     });
            }

            return mappedCategories;
        }

        public Boolean SaveCompliancePackageCategoryMapping(CompliancePackageCategory compliancePackageCategory, Int32 currentUserId, Boolean IsCreatedByAdmin)
        {
            compliancePackageCategory.CPC_IsActive = true;
            compliancePackageCategory.CPC_CreatedByID = currentUserId;
            compliancePackageCategory.CPC_CreatedOn = DateTime.Now;
            compliancePackageCategory.CPC_IsCreatedByAdmin = IsCreatedByAdmin;
            _ClientDBContext.CompliancePackageCategories.AddObject(compliancePackageCategory);
            _ClientDBContext.SaveChanges();
            return true;
        }

        public ComplianceCategory getCurrentCategoryInfo(Int32 categoryId)
        {
            return _ClientDBContext.ComplianceCategories.Where(obj => obj.ComplianceCategoryID == categoryId && obj.IsDeleted == false).FirstOrDefault();
        }

        public CompliancePackage GetCurrentPackageInfo(Int32 packageId)
        {
            return _ClientDBContext.CompliancePackages.Where(obj => obj.CompliancePackageID == packageId && obj.IsDeleted == false).FirstOrDefault();
        }

        public List<dynamic> GetPackageBundleNodeHierarchy(Int32 packageId)
        {
            var lstBundleIds = _ClientDBContext.DeptProgramPackages
                .Where(dpp => dpp.DPP_CompliancePackageID == packageId
                && !dpp.DPP_IsDeleted)
                .SelectMany(dpp => dpp.PackageBundleNodePackages
                .Where(PBNP => !PBNP.PBNP_IsDeleted)
                .Select(PBNP => PBNP.PBNP_PackageBundleID))
                .AsQueryable<int>();

            List<dynamic> lstHiearchy = _ClientDBContext.PackageBundleNodeMappings
                .Where(pbnm => lstBundleIds
                .Any(b => b == pbnm.PBNM_PackageBundleID)
                && !pbnm.PBNM_IsDeleted)
                .Select(pbnm => pbnm.DeptProgramMapping)
                .Where(dpm => !dpm.DPM_IsDeleted)
                .Select(dpm => new { DpmId = dpm.DPM_ID, DpmLabel = dpm.DPM_Label })
                .ToList<dynamic>();

            return lstHiearchy;
        }

        public Boolean DeleteCompliancePackageCategoryMapping(Int32 packageId, Int32 categoryId, Int32 currentUserId)
        {
            CompliancePackageCategory existingMapping = _ClientDBContext.CompliancePackageCategories.Where(obj => obj.CPC_CategoryID == categoryId && obj.CPC_PackageID == packageId && obj.CPC_IsDeleted == false).FirstOrDefault();
            existingMapping.CPC_IsDeleted = true;
            existingMapping.CPC_ModifiedByID = currentUserId;
            existingMapping.CPC_ModifiedOn = DateTime.Now;
            _ClientDBContext.SaveChanges();
            return true;
        }

        public Boolean DeletePackageCategoryMappingAndAssociatedData(Int32 packageId, Int32 categoryId, Int32 currentUserId, Int32 tenantId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_RemoveCategorymapping", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageId", packageId);
                command.Parameters.AddWithValue("@CategoryId", categoryId);
                command.Parameters.AddWithValue("@UserId", currentUserId);
                command.Parameters.AddWithValue("@TenantId", tenantId);
                command.Parameters.Add("@isScheduleActionRecordInserted", SqlDbType.Bit);
                command.Parameters["@isScheduleActionRecordInserted"].Direction = ParameterDirection.Output;
                if (con.State == ConnectionState.Closed)
                    con.Open();
                command.ExecuteNonQuery();
                con.Close();

                return Convert.ToBoolean(command.Parameters["@isScheduleActionRecordInserted"].Value);
            }
            return false;
        }

        /// <summary>
        /// To process Optional Category and insert/update data in ApplicantComplianceCategoryData and ScheduledAction tables
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="categoryId"></param>
        /// <param name="currentUserId"></param>
        public void ProcessOptionalCategory(Int32 packageId, Int32 categoryId, Int32 currentUserId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_ProcessOptionalCategory", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageID", packageId);
                command.Parameters.AddWithValue("@CategoryID", categoryId);
                command.Parameters.AddWithValue("@SystemUserID", currentUserId);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }
        }

        /// <summary>
        /// To process Required Category and insert data in ScheduledAction table
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="categoryId"></param>
        /// <param name="currentUserId"></param>
        public void ProcessRequiredCategory(Int32 packageId, Int32 categoryId, Int32 currentUserId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_ProcessRequiredCategory", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageID", packageId);
                command.Parameters.AddWithValue("@CategoryID", categoryId);
                command.Parameters.AddWithValue("@SystemUserID", currentUserId);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }
        }

        #endregion

        #region Category-Item Mapping

        /// <summary>
        /// Gets the list of Items related to a category
        /// </summary>
        /// <param name="categoryId">Id of the selected category</param>
        /// <returns>List of the items of that category</returns>
        public List<ComplianceCategoryItem> GetComplianceCategoryItems(Int32 categoryId, Boolean getTenantName, List<Entity.Tenant> tenantList)
        {
            List<ComplianceCategoryItem> complianceCategoryItems = ClientDBContext.ComplianceCategoryItems
                                                                   .Include("ComplianceItem")
                                                                   .Where(cci => cci.CCI_CategoryID == categoryId && cci.CCI_IsDeleted == false &&
                                                                    cci.ComplianceItem.IsDeleted == false).ToList();

            if (getTenantName)
            {
                //13-02-2014  Changes Done for -"Item listing screen save performance improvement".        
                complianceCategoryItems.Where(t => t.ComplianceCategory.TenantID != null)
                     .ForEach(t =>
                     {
                         var tenant = tenantList.FirstOrDefault(item => item.TenantID == t.ComplianceCategory.TenantID.Value);
                         if (tenant.IsNotNull())
                         {
                             t.ComplianceItem.TenantName = tenant.TenantName;
                         }
                         else
                         {
                             t.ComplianceItem.TenantName = String.Empty;
                         }
                     });
            }
            return complianceCategoryItems;
        }

        /// <summary>
        /// Save category-item mapping
        /// </summary>
        /// <param name="complianceItem">Details of the compliance item with mapping information to save/update</param>
        /// <param name="tenantId">Id of the tenant to which the current user belongs to</param>
        public void SaveCategoryItemMapping(ComplianceCategoryItem complianceCategoryItem)
        {
            //Int32 _lastDisplayOrder = _ClientDBContext.ComplianceCategoryItems.OrderByDescending(order => order.CCI_ID).Select(order => order.CCI_DisplayOrder).FirstOrDefault();
            //complianceCategoryItem.CCI_DisplayOrder = _lastDisplayOrder + 1;
            _ClientDBContext.ComplianceCategoryItems.AddObject(complianceCategoryItem);
            _ClientDBContext.SaveChanges();
        }

        public List<ComplianceItem> GetComplianceItemsByCategory(List<Int32> lstCategoryIds)
        {
            //UAT-2069 :- Passing multiple Categories Id.
            List<Int32> lstItemIds = _ClientDBContext.ComplianceCategoryItems.Where(cci => lstCategoryIds.Contains(cci.CCI_CategoryID) && cci.CCI_IsDeleted == false && cci.CCI_IsActive).Select(cci => cci.CCI_ItemID).ToList();
            return _ClientDBContext.ComplianceItems.Where(ci => lstItemIds.Contains(ci.ComplianceItemID) && ci.IsActive && ci.IsDeleted == false).ToList();
        }

        public void DeleteCategoryItemMapping(Int32 complianceCategoryItemId, Int32 currentLoggedInUserId)
        {
            ComplianceCategoryItem cmpCatItem = _ClientDBContext.ComplianceCategoryItems.Where(cci => cci.CCI_ID == complianceCategoryItemId).FirstOrDefault();
            cmpCatItem.CCI_IsDeleted = true;
            cmpCatItem.CCI_ModifiedByID = currentLoggedInUserId;
            cmpCatItem.CCI_ModifiedOn = DateTime.Now;
            _ClientDBContext.SaveChanges();
        }

        public Boolean DeleteCategoryItemMappingAndAssociatedData(Int32 categoryId, Int32 itemId, Int32 currentLoggedInUserId, Int32 tenantId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_RemoveItemMapping", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CategoryId", categoryId);
                command.Parameters.AddWithValue("@ItemId", itemId);
                command.Parameters.AddWithValue("@UserId", currentLoggedInUserId);
                command.Parameters.AddWithValue("@TenantId", tenantId);
                command.CommandTimeout = 120;
                command.Parameters.Add("@isScheduleActionRecordInserted", SqlDbType.Bit);
                command.Parameters["@isScheduleActionRecordInserted"].Direction = ParameterDirection.Output;
                if (con.State == ConnectionState.Closed)
                    con.Open();
                command.ExecuteNonQuery();
                con.Close();
                return Convert.ToBoolean(command.Parameters["@isScheduleActionRecordInserted"].Value);
            }
            return false;
        }

        public ComplianceItem getCurrentItemInfo(Int32 itemId)
        {
            return _ClientDBContext.ComplianceItems.Where(obj => obj.ComplianceItemID == itemId && obj.IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// Get Series Un-Mapped Attributes
        /// </summary>
        /// <param name="itemSeriesID"></param>
        /// <returns></returns>
        public List<SeriesAttributeContract> GetUnMappedAttributes(Int32 itemSeriesID)
        {
            List<SeriesAttributeContract> lstSeriesAttributeContract = new List<SeriesAttributeContract>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@ItemSeriesID", itemSeriesID)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetUnMapSeriesAttribute", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SeriesAttributeContract seriesAttributeContract = new SeriesAttributeContract();
                            seriesAttributeContract.CmpItemSeriesId = dr["ItemSeriesID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ItemSeriesID"]);
                            seriesAttributeContract.CmpItemId = dr["ItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ItemID"]);
                            seriesAttributeContract.CmpItemName = dr["ItemName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ItemName"]);
                            seriesAttributeContract.CmpAttributeId = dr["AttributeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["AttributeID"]);
                            seriesAttributeContract.CmpAttributeName = dr["AttributeName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AttributeName"]);
                            seriesAttributeContract.CmpAttributeDataType = dr["AttributeDatatypeName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AttributeDatatypeName"]);
                            seriesAttributeContract.CmpAttributeValue = dr["AttributeValue"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AttributeValue"]);
                            seriesAttributeContract.CmpAttributeDatatypeCode = dr["AttributeDatatypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AttributeDatatypeCode"]);
                            seriesAttributeContract.CmpItemSeriesItemId = dr["ItemSeriesItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ItemSeriesItemID"]);
                            seriesAttributeContract.CmpItemSeriesItemAttributeValueId = dr["ItemSeriesItemAttributeValueID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ItemSeriesItemAttributeValueID"]);
                            seriesAttributeContract.AttributeOptionList = dr["AttributeOptionList"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AttributeOptionList"]);

                            lstSeriesAttributeContract.Add(seriesAttributeContract);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstSeriesAttributeContract;
        }

        /// <summary>
        /// Save Series Un-Mapped Attributes
        /// </summary>
        /// <param name="currentUserID"></param>
        /// <param name="lstSeriesAttributeContract"></param>
        /// <returns></returns>
        public Boolean SaveUnMappedAttributes(Int32 currentUserID, List<SeriesAttributeContract> lstSeriesAttributeContract)
        {
            List<Int32> lstItemSeriesItemAttributeValueID = lstSeriesAttributeContract.Select(x => x.CmpItemSeriesItemAttributeValueId.Value).ToList();
            List<ItemSeriesItemAttributeValue> lstItemSeriesItemAttributeValueToUpdate = _ClientDBContext.ItemSeriesItemAttributeValues.Where(x => lstItemSeriesItemAttributeValueID.Contains(x.ISAV_ID)
                                                                                    && x.ISAV_IsDeleted == false).ToList();
            foreach (var seriesAttributeContract in lstSeriesAttributeContract)
            {
                //Update data
                if (!lstItemSeriesItemAttributeValueToUpdate.IsNullOrEmpty())
                {
                    lstItemSeriesItemAttributeValueToUpdate.ForEach(x =>
                        {
                            if (x.ISAV_ID == seriesAttributeContract.CmpItemSeriesItemAttributeValueId)
                            {
                                x.ISAV_AttributeValue = seriesAttributeContract.CmpAttributeValue;
                                x.ISAV_ModifiedByID = currentUserID;
                                x.ISAV_ModifiedOn = DateTime.Now;
                            }
                        });
                }

                if (seriesAttributeContract.CmpItemSeriesItemAttributeValueId == AppConsts.NONE)
                {
                    //Add new data
                    ItemSeriesItemAttributeValue itemSeriesItemAttributeValue = new ItemSeriesItemAttributeValue();
                    itemSeriesItemAttributeValue.ISAV_ItemSeriesItemID = seriesAttributeContract.CmpItemSeriesItemId.Value;
                    itemSeriesItemAttributeValue.ISAV_AttributeID = seriesAttributeContract.CmpAttributeId;
                    itemSeriesItemAttributeValue.ISAV_AttributeValue = seriesAttributeContract.CmpAttributeValue;
                    itemSeriesItemAttributeValue.ISAV_IsDeleted = false;
                    itemSeriesItemAttributeValue.ISAV_CreatedByID = currentUserID;
                    itemSeriesItemAttributeValue.ISAV_CreatedOn = DateTime.Now;

                    _ClientDBContext.ItemSeriesItemAttributeValues.AddObject(itemSeriesItemAttributeValue);
                }
            }

            if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        #endregion

        #region Item-Attributes Mapping

        public List<ComplianceItemAttribute> GetComplianceItemAttribute(Int32 itemID, Boolean getTenantName, List<Entity.Tenant> tenantList)
        {
            List<ComplianceItemAttribute> complianceItemAttributes =
            _ClientDBContext.ComplianceItemAttributes
                .Include("ComplianceAttribute")
                .Where(x =>
                    x.CIA_ItemID.Equals(itemID)
                    && !x.ComplianceAttribute.IsDeleted
                    && !x.CIA_IsDeleted).ToList();
            if (getTenantName)
            {
                complianceItemAttributes.Where(t => t.ComplianceAttribute.TenantID != null)
                    .ForEach(t =>
                    {
                        var tenant = tenantList.FirstOrDefault(item => item.TenantID == t.ComplianceAttribute.TenantID.Value);
                        if (tenant.IsNotNull())
                        {
                            t.ComplianceAttribute.TenantName = tenant.TenantName;
                        }
                        else
                        {
                            t.ComplianceAttribute.TenantName = String.Empty;
                        }
                    });
            }
            return complianceItemAttributes;
        }

        public ComplianceItemAttribute GetComplianceItemAttributeByID(Int32 cia_ID)
        {
            return _ClientDBContext.ComplianceItemAttributes
                .FirstOrDefault(x => x.CIA_ID.Equals(cia_ID) && !x.CIA_IsDeleted);
        }

        public Boolean AddComplianceItemAttribute(ComplianceItemAttribute complianceItemAttribute)
        {
            ClientDBContext.ComplianceItemAttributes.AddObject(complianceItemAttribute);
            ClientDBContext.SaveChanges();
            return true;
        }

        public Boolean DeleteComplianceItemAttribute(Int32 cia_ID, Int32 modifiedByID)
        {
            ComplianceItemAttribute complianceItemAttribute = GetComplianceItemAttributeByID(cia_ID);
            complianceItemAttribute.CIA_IsDeleted = true;
            complianceItemAttribute.CIA_ModifiedByID = modifiedByID;
            complianceItemAttribute.CIA_ModifiedOn = DateTime.Now;
            _ClientDBContext.SaveChanges();
            return true;
        }

        public Boolean DeleteComplianceItemAttributeAndAssociatedData(Int32 itemId, Int32 attributeId, Int32 modifiedByID, Int32 tenantId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_RemoveAttributeMapping", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ItemId", itemId);
                command.Parameters.AddWithValue("@AttributeId", attributeId);
                command.Parameters.AddWithValue("@UserId", modifiedByID);
                command.Parameters.AddWithValue("@TenantId", tenantId);
                command.CommandTimeout = 120;
                command.Parameters.Add("@isScheduleActionRecordInserted", SqlDbType.Bit);
                command.Parameters["@isScheduleActionRecordInserted"].Direction = ParameterDirection.Output;
                if (con.State == ConnectionState.Closed)
                    con.Open();
                command.ExecuteNonQuery();
                con.Close();

                return Convert.ToBoolean(command.Parameters["@isScheduleActionRecordInserted"].Value);
            }
            return false;
        }
        #endregion

        #region Rule Set Mapping

        /// <summary>
        /// Gets all the admin rule set objects.
        /// </summary>
        /// <returns>List<RuleSetObject></returns>
        public List<RuleSet> GetRuleSet()
        {
            return ClientDBContext.RuleSets.Where(obj => obj.RLS_IsDeleted == false && obj.RLS_AssignmentHierarchyID != null).ToList();
        }

        /// <summary>
        /// Gets the rule set for the given Rule Set ID.
        /// </summary>
        /// <param name="ruleSetId">Rule Set Id</param>
        /// <returns>RuleSet entity</returns>
        public RuleSet GetRuleSetInfoByID(Int32 ruleSetId)
        {

            return ClientDBContext.RuleSets.FirstOrDefault(obj => obj.RLS_ID == ruleSetId && obj.RLS_IsDeleted == false);
        }

        /// <summary>
        /// Gets the list of rule set for the selected object.
        /// </summary>
        /// <param name="associationHierarchyId">Object Id</param>
        /// <param name="objectTypeId">Object Type Id</param>
        /// <returns>List of RuleSet</returns>
        public List<RuleSet> GetRuleSetForObject(Int32 associationHierarchyId, Int32 objectTypeId)
        {
            //return ClientDBContext.RuleSetObjects.Where(obj => obj.RLSO_ObjectID == objectId && obj.RLSO_ObjectTypeID == objectTypeId &&
            //    obj.RLSO_IsDeleted == false && obj.RuleSet.RLS_IsDeleted == false).Select(obj => obj.RuleSet).ToList();
            return ClientDBContext.RuleSets.Where(obj => obj.RLS_AssignmentHierarchyID == associationHierarchyId && obj.RLS_IsDeleted == false).ToList();
        }

        /// <summary>
        /// Gets the list of rule set which are not associated with the selected object.
        /// </summary>
        /// <param name="objectId">Object Id</param>
        /// <param name="objectTypeId">Object Type Id</param>
        /// <returns>List of RuleSet</returns>
        public List<RuleSet> GetMasterRuleSetsForObject(Int32 objectId, Int32 objectTypeId)
        {
            List<Int32> ruleSetIds = ClientDBContext.RuleSetObjects.Where(obj => obj.RLSO_ObjectID == objectId && obj.RLSO_ObjectTypeID == objectTypeId &&
                obj.RLSO_IsDeleted == false).Select(obj => obj.RLSO_RuleSetID).ToList();
            return ClientDBContext.RuleSets.Where(obj => !(ruleSetIds.Contains(obj.RLS_ID)) && obj.RLS_IsDeleted == false).ToList();
        }

        /// <summary>
        /// Saves the rule set and object mapping.
        /// </summary>
        /// <param name="ruleSetObject">RuleSet Object</param>
        /// <param name="currentUserId">Current User Id</param>
        public void SaveRuleSetObjectMapping(RuleSetObject ruleSetObject, Int32 currentUserId)
        {
            ruleSetObject.RLS_CreatedById = currentUserId;
            ruleSetObject.RLS_CreatedOn = DateTime.Now;
            ClientDBContext.RuleSetObjects.AddObject(ruleSetObject);
            ClientDBContext.SaveChanges();
        }

        /// <summary>
        /// Deletes the rule set and object mapping.
        /// </summary>
        /// <param name="ruleSetId">RuleSet Id</param>
        /// <param name="objectId">Object Id</param>
        /// <param name="objectTypeId">Object Type Id</param>
        /// <param name="currentUserId">Current User Id</param>
        public void DeleteRuleSet(Int32 ruleSetId, Int32 objectId, Int32 objectTypeId, Int32 currentUserId)
        {
            RuleSet existingRuleSet = _ClientDBContext.RuleSets.Where(obj => obj.RLS_ID == ruleSetId && obj.RLS_IsDeleted == false).FirstOrDefault();
            existingRuleSet.RLS_IsDeleted = true;
            existingRuleSet.RLS_ModifiedByID = currentUserId;
            existingRuleSet.RLS_ModifiedOn = DateTime.Now;
            _ClientDBContext.SaveChanges();
        }

        #endregion

        #region Common Methods

        /// <summary>
        /// Get the hierarchical tree data for mapping screen
        /// </summary>
        /// <param name="packageIds"></param>
        /// <returns></returns>
        public ObjectResult<GetRuleSetTree> GetRuleSetTree(String packageIds)
        {
            return _ClientDBContext.GetRuleSetTree(packageIds);
        }

        public Int32 GetLargeContentTypeIdByCode(String code)
        {
            return ClientDBContext.lkpLargeContentTypes.Where(obj => obj.LCT_Code == code && obj.LCT_IsDeleted == false).FirstOrDefault().LCT_ID;
        }

        public Int32 GetLargeObjectTypeIdByCode(String code)
        {
            return ClientDBContext.lkpObjectTypes.Where(obj => obj.OT_Code == code && obj.OT_IsDeleted == false).FirstOrDefault().OT_ID;
        }

        public void SaveLargeContentRecord(LargeContent largeContent, Int32 objectTypeID, Int32 contentTypeID, Int32 currentUserId)
        {
            LargeContent existingContent = ClientDBContext.LargeContents.Where(obj => obj.LC_ObjectID == largeContent.LC_ObjectID && obj.LC_ObjectTypeID == objectTypeID &&
           obj.LC_LargeContentTypeID == contentTypeID && obj.LC_IsDeleted == false).FirstOrDefault();
            if (existingContent != null)
            {
                existingContent.LC_Content = largeContent.LC_Content;
                existingContent.LC_ModifiedByID = currentUserId;
                existingContent.LC_ModifiedOn = DateTime.Now;
                ClientDBContext.SaveChanges();
            }
            else
            {
                if (largeContent.LC_Content == String.Empty)
                    return;
                else
                {
                    largeContent.LC_LargeContentTypeID = contentTypeID;
                    largeContent.LC_ObjectTypeID = objectTypeID;
                    largeContent.LC_CreatedByID = currentUserId;
                    largeContent.LC_CreatedOn = DateTime.Now;
                    ClientDBContext.LargeContents.AddObject(largeContent);
                    ClientDBContext.SaveChanges();
                }
            }

        }

        public LargeContent getLargeContentRecord(Int32 objectId, Int32 objectTypeID, Int32 contentTypeID)
        {
            return ClientDBContext.LargeContents.Where(obj => obj.LC_ObjectID == objectId && obj.LC_LargeContentTypeID == contentTypeID && obj.LC_ObjectTypeID == objectTypeID && obj.LC_IsDeleted == false).FirstOrDefault();
        }
        #endregion

        /// <summary>
        /// Get the hierarchical tree data for copy to client screen
        /// </summary>
        /// <returns></returns>
        public ObjectResult<GetRuleSetTree> GetComplianceTree()
        {
            return _ClientDBContext.GetComplianceTree();
        }

        /// <summary>
        /// Get the package hierarchical tree data for package detail screen
        /// </summary>
        /// <returns>ObjectResult<GetPackageDetail></returns>
        public ObjectResult<GetPackageDetail> GetPackageDetailTree(Int32 packageID)
        {
            return _ClientDBContext.GetPackageDetailTree(packageID);
        }

        /// <summary>
        /// To get the Portfolio Subscription tree hierarchical data
        /// </summary>
        /// <param name="packageID"></param>
        /// <returns></returns>
        public ObjectResult<GetPortfolioSubscriptionTree> GetPortfolioSubscriptionTree(Int32 organizationUserId)
        {
            return _ClientDBContext.GetPortfolioSubscriptionTree(organizationUserId);
        }

        /// <summary>
        /// Get the hierarchical Department tree data
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public ObjectResult<GetDepartmentTree> GetDepartmentTree(Int32 departmentId)
        {
            //return _ClientDBContext.GetDepartmentTree(departmentId);
            return null;
        }

        //public List<GetInstituteHierarchyTreeData> GetInstituteHierarchyTree(int? orgUserID, Boolean fetchNoAccessNodes = false)
        //{

        //    List<GetInstituteHierarchyTreeData> list = _ClientDBContext.usp_GetInstituteHierarchyTreeData(orgUserID, fetchNoAccessNodes).ToList();

        //    return list;
        //    // return _ClientDBContext.usp_GetInstituteHierarchyTreeData(orgUserID, fetchNoAccessNodes).ToList;
        //}
        public List<InstituteHierarchyTreeDataContract> GetInstituteHierarchyTree(int? orgUserID, Boolean fetchNoAccessNodes = false)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[dbo].[usp_GetInstituteHierarchyTree]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrgUserID", orgUserID);
                command.Parameters.AddWithValue("@FetchNoAccessNodes", fetchNoAccessNodes);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                List<InstituteHierarchyTreeDataContract> instituteHierarchyTreeData = new List<InstituteHierarchyTreeDataContract>();


                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    instituteHierarchyTreeData = ds.Tables[0].AsEnumerable().Select(col =>
                         new InstituteHierarchyTreeDataContract
                         {
                             TreeNodeTypeID = Convert.ToInt32(col["TreeNodeTypeID"]),
                             NodeID = Convert.ToString(col["NodeID"]),
                             ParentNodeID = col["ParentNodeID"] == DBNull.Value ? null : Convert.ToString(col["ParentNodeID"]),
                             Level = Convert.ToInt32(col["Level"]),
                             DataID = Convert.ToInt32(col["DataID"]),
                             Value = Convert.ToString(col["Value"]),
                             ParentDataID = col["ParentDataID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["ParentDataID"]),
                             UICode = Convert.ToString(col["UICode"]),
                             IsLabel = Convert.ToBoolean(col["IsLabel"]),
                             NodeCode = Convert.ToString(col["NodeCode"]),
                             ParentNodeCode = Convert.ToString(col["ParentNodeCode"]),
                             Associated = Convert.ToBoolean(col["Associated"]),
                             MappingID = Convert.ToInt32(col["MappingID"]),
                             EntityID = Convert.ToInt32(col["EntityID"]),
                             PermissionCode = Convert.ToString(col["PermissionCode"]),
                             //Used = Convert.ToBoolean(col["Used"]),
                             PermissionName = Convert.ToString(col["PermissionName"]),
                             ProfilePermissionCode = Convert.ToString(col["ProfilePermissionCode"]),
                             ProfilePermissionName = Convert.ToString(col["ProfilePermissionName"]),
                             VerificationPermissionCode = Convert.ToString(col["VerificationPermissionCode"]),
                             VerificationPermissionName = Convert.ToString(col["VerificationPermissionName"]),
                             OrderPermissionCode = Convert.ToString(col["OrderPermissionCode"]),
                             OrderPermissionName = Convert.ToString(col["OrderPermissionName"]),
                             DPM_DisplayOrder = col["DPM_DisplayOrder"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["DPM_DisplayOrder"]),
                             IsPackageAvailableForOrder = col["IsPackageAvailableForOrder"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(col["IsPackageAvailableForOrder"]),
                             IsPackageBundleAvailableForOrder = col["IsPackageBundleAvailableForOrder"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(col["IsPackageBundleAvailableForOrder"]),
                         }).ToList();
                }
                return instituteHierarchyTreeData;
            }
            //return new List<InstituteHierarchyTreeDataContract>();
        }

        public ObjectResult<InstituteHierarchyNodesList> GetInstituteHierarchyNodes(int? orgUserID, Boolean fetchNoAccessNodes = false) //UAT-3369
        {
            return _ClientDBContext.usp_GetInstituteHierarchyNodes(orgUserID, fetchNoAccessNodes);
        }

        public ObjectResult<GetDepartmentTree> GetInstituteHierarchyTreeForBackgroundHierarchyPermissionType(Int32? orgUserID, Boolean fetchNoAccessNodes = false)
        {
            return _ClientDBContext.GetInstituteHierarchyTreeForBackgroundHierarchyPermissionType(orgUserID, fetchNoAccessNodes);
        }

        public ObjectResult<GetDepartmentTree> GetInstituteHierarchyTreeForConfiguration(int? orgUserID)
        {
            return _ClientDBContext.GetInstituteHierarchyTreeForConfiguration(orgUserID);
        }

        /// <summary>
        /// Get Institute Hierarchy Order Tree data
        /// </summary>
        /// <param name="orgUserID"></param>
        /// <returns></returns>
        public ObjectResult<GetInstituteHierarchyOrderTree> GetInstituteHierarchyOrderTree(int? orgUserID)
        {
            //return null;
            return _ClientDBContext.GetInstituteHierarchyOrderTree(orgUserID);
        }

        public DataTable GetFeaturePermissionTree(Guid? userID, Int32 tenantId)
        {
            EntityConnection connection = SecurityContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[dbo].[usp_GetFeatureActionPermissionsForClientAdmin]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", userID);
                command.Parameters.AddWithValue("@TenantID", tenantId);
                //command.Parameters.AddWithValue("@filteringSortingData", verificationGridCustomPaging.XML);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        /// <summary>
        /// Get the hierarchical tree data for copy to client screen
        /// </summary>
        /// <returns></returns>
        public ObjectResult<ComplianceAssociations> GetComplianceAssociations()
        {
            return _ClientDBContext.GetComplianceAssociations();
        }

        /// <summary>
        /// Get Department Program Mapping object.
        /// </summary>
        /// <param name="DepPrgMappingId">DepPrgMappingId</param>
        /// <returns>DeptProgramMapping</returns>
        public DeptProgramMapping GetDepartmentProgMapping(Int32 DepPrgMappingId)
        {
            return _ClientDBContext.DeptProgramMappings.Where(cond => !cond.DPM_IsDeleted && cond.DPM_ID == DepPrgMappingId).FirstOrDefault();
        }

        /// <summary>
        /// Get Department Program Mapping object.
        /// </summary>
        /// <param name="delimittedDepPrgMappingId">delimittedDepPrgMappingId</param>
        /// <returns>List of DeptProgramMapping</returns>
        public List<DeptProgramMapping> GetDepartmentProgMappingList(String delimittedDepPrgMappingId)
        {
            List<Int32> arrDeptPrgMappingID = delimittedDepPrgMappingId.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            return _ClientDBContext.DeptProgramMappings.Where(cond => arrDeptPrgMappingID.Contains(cond.DPM_ID) && !cond.DPM_IsDeleted).ToList();
        }
        #endregion

        #region Copy to Client

        public ComplianceAttribute SaveComplianceAttribute(ComplianceAttribute complianceAttribute, Int32 currentUserId)
        {
            complianceAttribute.CreatedByID = currentUserId;
            complianceAttribute.CreatedOn = DateTime.Now;
            ClientDBContext.ComplianceAttributes.AddObject(complianceAttribute);
            ClientDBContext.SaveChanges();
            return complianceAttribute;
        }

        void IComplianceSetupRepository.CopyComplianceToClient(String complianceDetails, Int32 currentUserId, Int32 tenantID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_CopyComplianceToClient", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantID", tenantID);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentUserId);
                command.Parameters.AddWithValue("@ComplianceListXML", complianceDetails);
                command.CommandTimeout = 120;
                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }
        }


        private void SaveCompliancePksUrlMappings(Int32 PackageId, Int32 currentUserId, Guid? nodeCode, List<CompliancePackage> lstAdminPackages, List<TrackingPackageRequiredDocURL> listAdminTrackingURL, List<TrackingPackageRequiredDocURLMapping> listadminTrackingURLMapping, Int32 tenantID)
        {
            CompliancePackage adminPackage = lstAdminPackages.Where(p => p.Code == nodeCode).FirstOrDefault();
            List<TrackingPackageRequiredDocURLMapping> listadminTrackingPackageRequiredDocURLMapping = listadminTrackingURLMapping.Where(getValue => getValue.TPRPM_CompliancePackageId == adminPackage.CompliancePackageID && getValue.TPRPM_IsDeleted == false).ToList();
            if (listadminTrackingPackageRequiredDocURLMapping.Count > AppConsts.NONE)
            {
                List<TrackingPackageRequiredDocURL> lstadminTrackingRequiredDOCURL = (from a in listAdminTrackingURL
                                                                                      join b in listadminTrackingPackageRequiredDocURLMapping on a.TPRDU_ID equals b.TPRPM_URLID
                                                                                      where a.TPRDU_IsDeleted == false
                                                                                      select a).ToList();

                List<TrackingPackageRequiredDocURL> lstnewadminTrackingRequiredDOCURL = new List<TrackingPackageRequiredDocURL>();
                foreach (var adminTrackingRequiredDOCURL in lstadminTrackingRequiredDOCURL)
                {
                    TrackingPackageRequiredDocURL DOCURL = new TrackingPackageRequiredDocURL();

                    DOCURL.TPRDU_Link = adminTrackingRequiredDOCURL.TPRDU_Link;
                    DOCURL.TPRDU_InternalName = adminTrackingRequiredDOCURL.TPRDU_InternalName;
                    DOCURL.TPRDU_DisplayLabel = adminTrackingRequiredDOCURL.TPRDU_DisplayLabel;
                    DOCURL.TPRDU_CreatedByID = currentUserId;
                    DOCURL.TPRDU_CreatedOn = DateTime.Now;
                    lstnewadminTrackingRequiredDOCURL.Add(DOCURL);
                    ClientDBContext.TrackingPackageRequiredDocURLs.AddObject(DOCURL);
                }

                ClientDBContext.SaveChanges();

                foreach (var adminTrackingRequiredDOCURL in lstnewadminTrackingRequiredDOCURL)
                {
                    TrackingPackageRequiredDocURLMapping clientURLMappingToCreate = new TrackingPackageRequiredDocURLMapping
                    {
                        TPRPM_URLID = adminTrackingRequiredDOCURL.TPRDU_ID,
                        TPRPM_CompliancePackageId = PackageId,
                        TPRPM_IsDeleted = false,
                        TPRPM_CreatedByID = currentUserId,
                        TPRPM_IsActive = true,
                        TPRPM_CreatedOn = DateTime.Now
                    };
                    ClientDBContext.TrackingPackageRequiredDocURLMappings.AddObject(clientURLMappingToCreate);

                }
                ClientDBContext.SaveChanges();
            }
        }
        /// <summary>
        /// Copies/Removes the elements from Admin to Client
        /// </summary>
        /// <param name="lstElementsToAdd">Elements that are selected by admin for copying to client</param>
        /// <param name="lstElementsToRemove">Elements de-selected by admin to remove from the client</param>
        /// <param name="lstAdminPackages">List of Master packages of Admin, to get the details of particular package, when required.</param>
        /// <param name="lstAdminCategories">List of Master categories of Admin, to get the details of particular category, when required.</param>
        /// <param name="lstAdminItems">List of Master items of Admin, to get the details of particular item, when required.</param>
        /// <param name="lstAdminAttributes">List of Master attributes of Admin, to get the details of particular attribute, when required.</param>
        /// <param name="currentUserId">Id of the currently logged in user.</param>
        public void CopyToClient(List<GetRuleSetTree> lstElementsToAdd, List<GetRuleSetTree> lstElementsToRemove, ComplianceSetUpContract adminData, Int32 currentUserId, Int32 tenantID)
        {
            Int32 newObjectTypeId = 0;
            String newObjectTypeCode = String.Empty;
            Int32 parentObjectTypeId = 0;
            String parentObjectTypeCode = String.Empty;
            Int32 _categoryId = 0;
            Int32 _packageId = 0;
            Int32 _itemId = 0;
            Int32 _attributeId = 0;
            Guid? _nodeUICode;
            Guid? _nodeParentUICode;
            adminData.RuleSetToBeCopied = new List<RuleSet>();
            ComplianceSetUpContract clientData = new ComplianceSetUpContract();
            clientData.CompliancePackageList = GetCompliancePackages(false, null);
            clientData.lstTrackingPackageRequiredDocURL = GetTrackingPackageRequiredDOCURL();
            clientData.lstTrackingPackageRequiredDocURLMapping = GetTrackingPackageRequiredDOCURLMapping();
            clientData.ComplianceCategoryList = GetComplianceCategories(false, null);
            clientData.ComplianceItemList = GetComplianceItems(false, null);
            clientData.ComplianceAttributeList = GetComplianceAttributes(false, null);
            clientData.LkpObjectTypeList = GetlkpObjectType();
            clientData.LkpRuleObjectMappingTypeList = GetlkpRuleObjMapType();

            // UAT-2985
            /*List<UniversalCategoryMapping> lstUniversalCategoryMapping = GetUniversalCategoryMappings("AAAA");
            List<UniversalItemMapping> lstUniversalItemMapping = GetUniversalItemMappings(lstUniversalCategoryMapping.Select(sel => sel.UCM_ID).ToList());
            List<UniversalAttributeMapping> lstUniversalAttrMapping = GetUniversalAttributeMappings(lstUniversalItemMapping.Select(sel => sel.UIM_ID).ToList());
            clientData.lstUniversalCategorymapping = lstUniversalCategoryMapping;
            clientData.lstUniversalItemMapping = lstUniversalItemMapping;
             */
            List<UniversalFieldMapping> lstUniversalFieldMapping = GetUniversalFieldMappings("AAAA");
            clientData.lstUniversalAttrMapping = lstUniversalFieldMapping;

            List<CompliancePackageCategory> lstClientPackageCategory = new List<CompliancePackageCategory>();
            List<ComplianceCategoryItem> lstClientCategoryItem = new List<ComplianceCategoryItem>();
            List<ComplianceItemAttribute> lstClientItemAttribute = new List<ComplianceItemAttribute>();

            #region ADD NEW ELEMENTS

            foreach (var element in lstElementsToAdd)
            {
                _nodeUICode = element.NodeCode;
                _nodeParentUICode = element.ParentNodeCode;

                if (element.UICode.ToLower() == RuleSetTreeNodeType.Package.ToLower())
                {
                    CompliancePackage cmpPackage = GetExistingPackage(clientData.CompliancePackageList, _nodeUICode);
                    if (cmpPackage.IsNull())
                    {
                        // Get the package from Admin and add to client
                        CompliancePackage packageCreated = AddGetPackageId(currentUserId, _nodeUICode, adminData.CompliancePackageList, tenantID);
                        clientData.CompliancePackageList.Add(packageCreated);
                        _packageId = packageCreated.CompliancePackageID;
                        //UAT-3781
                        SaveCompliancePksUrlMappings(_packageId, currentUserId, _nodeUICode, adminData.CompliancePackageList, adminData.lstTrackingPackageRequiredDocURL, adminData.lstTrackingPackageRequiredDocURLMapping, tenantID);
                        // END Here
                        List<LargeContent> contentListToBeCopied = GetLargeContentByObjectId(adminData.LargeContentList, element.DataID, LCObjectType.CompliancePackage.GetStringValue());

                        foreach (LargeContent contentToBeCopied in contentListToBeCopied)
                        {
                            CopylargeContentForClient(contentToBeCopied, packageCreated.CompliancePackageID, currentUserId);
                        }
                        CopyInstitutionPackageWebpage(_packageId, currentUserId, tenantID, false, adminData.InstitutionWebPageList, element.DataID);
                    }
                    newObjectTypeCode = LCObjectType.CompliancePackage.GetStringValue();
                    newObjectTypeId = clientData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == newObjectTypeCode && x.OT_IsDeleted == false).OT_ID;
                    AddAssociationHierarchyNode(null, null, newObjectTypeId, _packageId, currentUserId, true);
                }
                else if (element.UICode.ToLower() == RuleSetTreeNodeType.Category.ToLower())
                {
                    CompliancePackage cmpPackage = GetExistingPackage(clientData.CompliancePackageList, _nodeParentUICode);

                    if (cmpPackage.IsNull())
                    {
                        // Get the package from Admin and add to client
                        CompliancePackage packageCreated = AddGetPackageId(currentUserId, _nodeParentUICode, adminData.CompliancePackageList, tenantID);
                        clientData.CompliancePackageList.Add(packageCreated);
                        _packageId = packageCreated.CompliancePackageID;
                        List<LargeContent> contentListToBeCopied = GetLargeContentByObjectId(adminData.LargeContentList, element.ParentDataID, LCObjectType.CompliancePackage.GetStringValue());
                        foreach (LargeContent contentToBeCopied in contentListToBeCopied)
                        {
                            CopylargeContentForClient(contentToBeCopied, _packageId, currentUserId);
                        }
                        CopyInstitutionPackageWebpage(_packageId, currentUserId, tenantID, false, adminData.InstitutionWebPageList, element.DataID);
                    }
                    else
                        _packageId = cmpPackage.CompliancePackageID;

                    ComplianceCategory cmpCategory = GetExistingCategory(clientData.ComplianceCategoryList, _nodeUICode);

                    if (cmpCategory.IsNull())
                    {
                        // Get the Category from Admin and add to client
                        ComplianceCategory categoryCreated = AddGetCategoryId(currentUserId, _nodeUICode, adminData.ComplianceCategoryList, tenantID);
                        clientData.ComplianceCategoryList.Add(categoryCreated);
                        _categoryId = categoryCreated.ComplianceCategoryID;
                        List<LargeContent> contentListToBeCopied = GetLargeContentByObjectId(adminData.LargeContentList, element.DataID, LCObjectType.ComplianceCategory.GetStringValue());
                        foreach (LargeContent contentToBeCopied in contentListToBeCopied)
                        {
                            CopylargeContentForClient(contentToBeCopied, _categoryId, currentUserId);
                        }
                        //Int32 adminCategoryId = adminData.ComplianceCategoryList.FirstOrDefault(a => a.Code == _nodeUICode).ComplianceCategoryID;
                        //CopyRuleSetToClient(adminData, clientData, LCObjectType.ComplianceCategory.GetStringValue(), adminCategoryId, _categoryId, tenantID, currentUserId);
                    }
                    else
                        _categoryId = cmpCategory.ComplianceCategoryID;

                    //UAT-2985: Universal Mapping Design Changes (1 of 2)
                    /*UniversalCategoryMapping universalCategoryMappingInMaster = adminData.lstUniversalCategorymapping.FirstOrDefault(cond => cond.UCM_CategoryID == element.DataID);
                    if (!universalCategoryMappingInMaster.IsNullOrEmpty())
                    {
                        CopyUniversalCategorMaapingyToClient(_categoryId, universalCategoryMappingInMaster, currentUserId);
                    }*/

                    if (!CheckPackageCategoryMappingExist(lstClientPackageCategory, _categoryId, _packageId))
                    {
                        Int32 displayOrder = 0;
                        Boolean complianceRequired = false;
                        CompliancePackage adminPackage = adminData.CompliancePackageList.Where(p => p.Code == _nodeParentUICode).FirstOrDefault();
                        ComplianceCategory adminCategory = adminData.ComplianceCategoryList.Where(c => c.Code == _nodeUICode).FirstOrDefault();
                        CompliancePackageCategory adminCompliancePackageCategory = adminData.CompliancePackageCategoryList.FirstOrDefault(obj => obj.CPC_CategoryID == adminCategory.ComplianceCategoryID && obj.CPC_PackageID == adminPackage.CompliancePackageID);
                        if (adminCompliancePackageCategory.IsNotNull())
                        {
                            displayOrder = adminCompliancePackageCategory.CPC_DisplayOrder;
                            complianceRequired = adminCompliancePackageCategory.CPC_ComplianceRequired;
                        }
                        SaveClientPackageCategoryMapping(_packageId, _categoryId, displayOrder, complianceRequired, currentUserId, lstClientPackageCategory);

                        parentObjectTypeCode = LCObjectType.CompliancePackage.GetStringValue();
                        newObjectTypeCode = LCObjectType.ComplianceCategory.GetStringValue();
                        parentObjectTypeId = clientData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == parentObjectTypeCode && x.OT_IsDeleted == false).OT_ID;
                        newObjectTypeId = clientData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == newObjectTypeCode && x.OT_IsDeleted == false).OT_ID;
                        AddAssociationHierarchyNode(parentObjectTypeId, _packageId, newObjectTypeId, _categoryId, currentUserId, true);
                    }
                    Int32 parentPackageId = adminData.CompliancePackageList.FirstOrDefault(a => a.Code == _nodeParentUICode).CompliancePackageID;
                    Int32 adminCategoryId = adminData.ComplianceCategoryList.FirstOrDefault(a => a.Code == _nodeUICode).ComplianceCategoryID;
                    CopyRuleSetToClient(adminData, ComplainceObjectType.Category, adminCategoryId, ComplainceObjectType.Package, parentPackageId, tenantID, currentUserId);
                }
                else if (element.UICode.ToLower() == RuleSetTreeNodeType.Item.ToLower())
                {
                    ComplianceCategory cmpCategory = GetExistingCategory(clientData.ComplianceCategoryList, _nodeParentUICode);
                    if (cmpCategory.IsNull())
                    {
                        // Get the Category from Admin and add to client
                        ComplianceCategory categoryCreated = AddGetCategoryId(currentUserId, _nodeParentUICode, adminData.ComplianceCategoryList, tenantID);
                        clientData.ComplianceCategoryList.Add(categoryCreated);
                        _categoryId = categoryCreated.ComplianceCategoryID;
                        List<LargeContent> contentListToBeCopied = GetLargeContentByObjectId(adminData.LargeContentList, element.ParentDataID, LCObjectType.ComplianceCategory.GetStringValue());
                        foreach (LargeContent contentToBeCopied in contentListToBeCopied)
                        {
                            CopylargeContentForClient(contentToBeCopied, _categoryId, currentUserId);
                        }
                        //Int32 adminCategoryId = adminData.ComplianceCategoryList.FirstOrDefault(a => a.Code == _nodeUICode).ComplianceCategoryID;
                        //CopyRuleSetToClient(adminData, clientData, LCObjectType.ComplianceCategory.GetStringValue(), adminCategoryId, _categoryId, tenantID, currentUserId);
                    }
                    else
                        _categoryId = cmpCategory.ComplianceCategoryID;

                    ComplianceItem cmpItem = GetExistingItem(clientData.ComplianceItemList, _nodeUICode);

                    if (cmpItem.IsNull())
                    {
                        // Get the Item from Admin and add to client
                        ComplianceItem itemCreated = AddGetItemId(currentUserId, _nodeUICode, adminData.ComplianceItemList, tenantID);
                        clientData.ComplianceItemList.Add(itemCreated);
                        _itemId = itemCreated.ComplianceItemID;
                        List<LargeContent> contentListToBeCopied = GetLargeContentByObjectId(adminData.LargeContentList, element.DataID, LCObjectType.ComplianceItem.GetStringValue());
                        foreach (LargeContent contentToBeCopied in contentListToBeCopied)
                        {
                            CopylargeContentForClient(contentToBeCopied, _itemId, currentUserId);
                        }
                        //Int32 adminItemId = adminData.ComplianceItemList.FirstOrDefault(a => a.Code == _nodeUICode).ComplianceItemID;
                        //CopyRuleSetToClient(adminData, clientData, LCObjectType.ComplianceItem.GetStringValue(), adminItemId, _itemId, tenantID, currentUserId);
                    }
                    else
                        _itemId = cmpItem.ComplianceItemID;

                    if (!CheckCategoryItemMappingExist(lstClientCategoryItem, _categoryId, _itemId))
                    {
                        SaveClientCategoryItemMapping(_categoryId, _itemId, currentUserId, lstClientCategoryItem);

                        parentObjectTypeCode = LCObjectType.ComplianceCategory.GetStringValue();
                        newObjectTypeCode = LCObjectType.ComplianceItem.GetStringValue();
                        parentObjectTypeId = clientData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == parentObjectTypeCode && x.OT_IsDeleted == false).OT_ID;
                        newObjectTypeId = clientData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == newObjectTypeCode && x.OT_IsDeleted == false).OT_ID;
                        AddAssociationHierarchyNode(parentObjectTypeId, _categoryId, newObjectTypeId, _itemId, currentUserId, false);
                    }
                    if (cmpCategory.IsNull())
                    {
                        Int32 adminCategoryId = adminData.ComplianceCategoryList.FirstOrDefault(a => a.Code == _nodeUICode).ComplianceCategoryID;
                        CopyRuleSetToClient(adminData, ComplainceObjectType.Category, adminCategoryId, ComplainceObjectType.Package, null, tenantID, currentUserId);
                    }

                    Int32 parentCategoryId = adminData.ComplianceCategoryList.FirstOrDefault(a => a.Code == _nodeParentUICode).ComplianceCategoryID;
                    Int32 adminItemId = adminData.ComplianceItemList.FirstOrDefault(a => a.Code == _nodeUICode).ComplianceItemID;
                    CopyRuleSetToClient(adminData, ComplainceObjectType.Item, adminItemId, ComplainceObjectType.Category, parentCategoryId, tenantID, currentUserId);

                    #region UAt-2305: universalMapping

                    //UAT-2985: Universal Mapping Design Changes (1 of 2)
                    /*
                    UniversalCategoryMapping universalCategoryMappingInMaster = adminData.lstUniversalCategorymapping.FirstOrDefault(cond => cond.UCM_CategoryID == element.ParentDataID);
                    if (!universalCategoryMappingInMaster.IsNullOrEmpty())
                    {
                        UniversalCategoryMapping universalCategoryMappingInClient = CopyUniversalCategorMaapingyToClient(_categoryId, universalCategoryMappingInMaster, currentUserId);

                        ComplianceCategory adminCategory = adminData.ComplianceCategoryList.Where(c => c.Code == _nodeParentUICode).FirstOrDefault();
                        ComplianceItem adminItem = adminData.ComplianceItemList.Where(c => c.Code == _nodeUICode).FirstOrDefault();
                        ComplianceCategoryItem categoryItem = adminCategory.ComplianceCategoryItems.FirstOrDefault(sel => !sel.CCI_IsDeleted && sel.CCI_ItemID == adminItem.ComplianceItemID);

                        UniversalItemMapping universalItemMappingInMaster = adminData.lstUniversalItemMapping.FirstOrDefault(cond => cond.UIM_UniversalCategoryMappingID == universalCategoryMappingInMaster.UCM_ID
                                                                                                                                 && cond.UIM_CategoryItemMappingID == categoryItem.CCI_ID);
                        if (!universalItemMappingInMaster.IsNullOrEmpty())
                        {
                            ComplianceCategoryItem categoryItemClient = lstClientCategoryItem.FirstOrDefault(x => x.CCI_CategoryID == _categoryId && x.CCI_ItemID == _itemId);
                            CopyUniversalItemMaapingToClient(categoryItemClient.CCI_ID, universalCategoryMappingInClient.UCM_ID, universalItemMappingInMaster, currentUserId);
                        }
                    }
                     */
                    #endregion
                }
                else if (element.UICode.ToLower() == RuleSetTreeNodeType.Attribute.ToLower())
                {
                    ComplianceItem cmpItem = GetExistingItem(clientData.ComplianceItemList, _nodeParentUICode);

                    if (cmpItem.IsNull())
                    {
                        // Get the Item from Admin and add to client
                        ComplianceItem itemCreated = AddGetItemId(currentUserId, _nodeParentUICode, adminData.ComplianceItemList, tenantID);
                        clientData.ComplianceItemList.Add(itemCreated);
                        _itemId = itemCreated.ComplianceItemID;
                        List<LargeContent> contentListToBeCopied = GetLargeContentByObjectId(adminData.LargeContentList, element.ParentDataID, LCObjectType.ComplianceItem.GetStringValue());
                        foreach (LargeContent contentToBeCopied in contentListToBeCopied)
                        {
                            CopylargeContentForClient(contentToBeCopied, _itemId, currentUserId);
                        }
                        //Int32 adminItemId = adminData.ComplianceItemList.FirstOrDefault(a => a.Code == _nodeUICode).ComplianceItemID;
                        //CopyRuleSetToClient(adminData, clientData, LCObjectType.ComplianceItem.GetStringValue(), adminItemId, _itemId, tenantID, currentUserId);
                    }
                    else
                        _itemId = cmpItem.ComplianceItemID;


                    ComplianceAttribute cmpAttribute = GetExistingAttribute(clientData.ComplianceAttributeList, _nodeUICode);
                    ComplianceAttribute attributeInClientDb = null;
                    if (cmpAttribute.IsNull())
                    {
                        // Get the Item from Admin and add to client
                        attributeInClientDb = AddGetAttributeId(currentUserId, _nodeUICode, adminData.ComplianceAttributeList, tenantID);
                        clientData.ComplianceAttributeList.Add(attributeInClientDb);
                        _attributeId = attributeInClientDb.ComplianceAttributeID;
                        List<LargeContent> contentListToBeCopied = GetLargeContentByObjectId(adminData.LargeContentList, element.DataID, LCObjectType.ComplianceATR.GetStringValue());
                        foreach (LargeContent contentToBeCopied in contentListToBeCopied)
                        {
                            CopylargeContentForClient(contentToBeCopied, _attributeId, currentUserId);
                        }
                        //Int32 adminAttributeId = adminData.ComplianceAttributeList.FirstOrDefault(a => a.Code == _nodeUICode).ComplianceAttributeID;
                        //CopyRuleSetToClient(adminData, clientData, LCObjectType.ComplianceATR.GetStringValue(), adminAttributeId, _attributeId, tenantID, currentUserId);

                    }
                    else
                    {
                        attributeInClientDb = cmpAttribute;
                        _attributeId = cmpAttribute.ComplianceAttributeID;
                    }

                    if (!CheckItemAttributeMappingExist(lstClientItemAttribute, _attributeId, _itemId))
                    {
                        Int32 newCia_Id = SaveItemAttributeMapping(_itemId, _attributeId, currentUserId, lstClientItemAttribute);

                        parentObjectTypeCode = LCObjectType.ComplianceItem.GetStringValue();
                        newObjectTypeCode = LCObjectType.ComplianceATR.GetStringValue();
                        parentObjectTypeId = clientData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == parentObjectTypeCode && x.OT_IsDeleted == false).OT_ID;
                        newObjectTypeId = clientData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == newObjectTypeCode && x.OT_IsDeleted == false).OT_ID;
                        AddAssociationHierarchyNode(parentObjectTypeId, _itemId, newObjectTypeId, _attributeId, currentUserId, false);
                        Int32 parentAdminItemID = adminData.ComplianceItemList.FirstOrDefault(a => a.Code == _nodeParentUICode).ComplianceItemID;
                        Int32 adminAttributeID = adminData.ComplianceAttributeList.FirstOrDefault(a => a.Code == _nodeUICode).ComplianceAttributeID;
                        CopyAttributeInstructionToClient(adminData, ComplainceObjectType.Attribute, adminAttributeID, ComplainceObjectType.Item, parentAdminItemID, tenantID, currentUserId, newCia_Id);
                    }
                    if (cmpItem.IsNull())
                    {
                        Int32 adminItemId = adminData.ComplianceItemList.FirstOrDefault(a => a.Code == _nodeUICode).ComplianceItemID;
                        CopyRuleSetToClient(adminData, ComplainceObjectType.Item, adminItemId, ComplainceObjectType.Category, null, tenantID, currentUserId);
                    }
                    Int32 parentItemId = adminData.ComplianceItemList.FirstOrDefault(a => a.Code == _nodeParentUICode).ComplianceItemID;
                    Int32 adminAttributeId = adminData.ComplianceAttributeList.FirstOrDefault(a => a.Code == _nodeUICode).ComplianceAttributeID;
                    CopyRuleSetToClient(adminData, ComplainceObjectType.Attribute, adminAttributeId, ComplainceObjectType.Item, parentItemId, tenantID, currentUserId);

                    #region UAt-2305- UniversalMapping
                    List<ComplianceCategory> lstMasterCategoryMappedWithItem = adminData.ComplianceCategoryList.Where(cond => cond.ComplianceCategoryItems.Any(cc => !cc.CCI_IsDeleted && cc.CCI_ItemID == parentItemId)).ToList();

                    foreach (var masterCategoryMappedWithItem in lstMasterCategoryMappedWithItem)
                    {

                        ComplianceCategory CmpCategoryInClient = ClientDBContext.ComplianceCategories.FirstOrDefault(con => con.CopiedFromCode == masterCategoryMappedWithItem.Code);
                        if (!CmpCategoryInClient.IsNullOrEmpty())
                        {
                            // UniversalCategoryMapping universalCategoryMappingInMaster = adminData.lstUniversalCategorymapping.FirstOrDefault(cond => cond.UCM_CategoryID == masterCategoryMappedWithItem.ComplianceCategoryID);
                            //if (!universalCategoryMappingInMaster.IsNullOrEmpty())
                            // {
                            //UniversalCategoryMapping universalCategoryMappingInClient = CopyUniversalCategorMaapingyToClient(CmpCategoryInClient.ComplianceCategoryID, universalCategoryMappingInMaster, currentUserId);

                            //if (!universalCategoryMappingInClient.IsNullOrEmpty())
                            //{
                            ComplianceItem masterItem = adminData.ComplianceItemList.Where(c => c.Code == _nodeParentUICode).FirstOrDefault();
                            ComplianceCategoryItem masterCategoryItem = masterCategoryMappedWithItem.ComplianceCategoryItems.FirstOrDefault(sel => !sel.CCI_IsDeleted
                                                                                                                        && sel.CCI_ItemID == masterItem.ComplianceItemID);

                            //UniversalItemMapping universalItemMappingInMaster = adminData.lstUniversalItemMapping.FirstOrDefault(cond => cond.UIM_UniversalCategoryMappingID == universalCategoryMappingInMaster.UCM_ID
                            //                                                                                              && cond.UIM_CategoryItemMappingID == masterCategoryItem.CCI_ID);
                            //if (!universalItemMappingInMaster.IsNullOrEmpty())
                            //{
                            ComplianceCategoryItem categoryItemClient = lstClientCategoryItem.FirstOrDefault(x => x.CCI_CategoryID == _categoryId && x.CCI_ItemID == _itemId);
                            //UniversalItemMapping universalItemMappingInClient = CopyUniversalItemMaapingToClient(categoryItemClient.CCI_ID, universalCategoryMappingInClient.UCM_ID, universalItemMappingInMaster, currentUserId);

                            //if (!universalItemMappingInClient.IsNullOrEmpty())
                            //{
                            ComplianceAttribute masterAttribute = adminData.ComplianceAttributeList.Where(c => c.Code == _nodeUICode).FirstOrDefault();

                            ComplianceItemAttribute masterItemAttribute = masterItem.ComplianceItemAttributes.FirstOrDefault(sel => !sel.CIA_IsDeleted
                                                                                                                    && sel.CIA_AttributeID == masterAttribute.ComplianceAttributeID);
                            UniversalFieldMapping universalFieldMappingInMaster = adminData.lstUniversalAttrMapping.FirstOrDefault(cond => cond.UFM_CategoryItemMappingID == masterCategoryItem.CCI_ID
                                                                                                                     && cond.UFM_ItemAttributeMappingID == masterItemAttribute.CIA_ID);

                            if (!universalFieldMappingInMaster.IsNullOrEmpty())
                            {
                                ComplianceItemAttribute itemAttributeClient = lstClientItemAttribute.FirstOrDefault(x => x.CIA_ItemID == _itemId && x.CIA_AttributeID == _attributeId);
                                CopyUniversalFieldMaapingToClient(itemAttributeClient.CIA_ID, categoryItemClient.CCI_ID, universalFieldMappingInMaster.UFM_UniversalFieldID, universalFieldMappingInMaster, currentUserId
                                                                  , adminData.lstUniversalAttrOptionMapping, masterAttribute, attributeInClientDb);
                            }

                            //}
                            //}
                            //}
                            //}
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region REMOVE OLD ELEMENTS

            var attributesToRemove = lstElementsToRemove.Where(x => x.UICode == RuleSetTreeNodeType.Attribute).ToList();

            foreach (var element in attributesToRemove)
            {
                ComplianceItem cmpItem = GetExistingItem(clientData.ComplianceItemList, element.ParentNodeCode);
                ComplianceAttribute cmpAttribute = GetExistingAttribute(clientData.ComplianceAttributeList, element.NodeCode);
                if (cmpItem.IsNotNull() && cmpAttribute.IsNotNull())
                {
                    RemoveClientItemAttributeMapping(cmpItem.ComplianceItemID, cmpAttribute.ComplianceAttributeID, currentUserId);

                    parentObjectTypeCode = LCObjectType.ComplianceItem.GetStringValue();
                    newObjectTypeCode = LCObjectType.ComplianceATR.GetStringValue();

                    parentObjectTypeId = clientData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == parentObjectTypeCode && x.OT_IsDeleted == false).OT_ID;
                    newObjectTypeId = clientData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == newObjectTypeCode && x.OT_IsDeleted == false).OT_ID;
                    DeleteAssociationHierarchyNode(parentObjectTypeId, cmpItem.ComplianceItemID, newObjectTypeId, cmpAttribute.ComplianceAttributeID, currentUserId);
                }
            }


            var itemToRemove = lstElementsToRemove.Where(x => x.UICode == RuleSetTreeNodeType.Item).ToList();

            foreach (var element in itemToRemove)
            {
                ComplianceCategory cmpCategory = GetExistingCategory(clientData.ComplianceCategoryList, element.ParentNodeCode);
                ComplianceItem cmpItem = GetExistingItem(clientData.ComplianceItemList, element.NodeCode);
                if (cmpCategory.IsNotNull() && cmpItem.IsNotNull())
                {
                    RemoveClientCategoryItemMapping(cmpCategory.ComplianceCategoryID, cmpItem.ComplianceItemID, currentUserId);

                    parentObjectTypeCode = LCObjectType.ComplianceCategory.GetStringValue();
                    newObjectTypeCode = LCObjectType.ComplianceItem.GetStringValue();

                    parentObjectTypeId = clientData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == parentObjectTypeCode && x.OT_IsDeleted == false).OT_ID;
                    newObjectTypeId = clientData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == newObjectTypeCode && x.OT_IsDeleted == false).OT_ID;
                    DeleteAssociationHierarchyNode(parentObjectTypeId, cmpCategory.ComplianceCategoryID, newObjectTypeId, cmpItem.ComplianceItemID, currentUserId);
                }
            }


            var categoryToRemove = lstElementsToRemove.Where(x => x.UICode == RuleSetTreeNodeType.Category).ToList();

            foreach (var element in categoryToRemove)
            {
                ComplianceCategory cmpCategory = GetExistingCategory(clientData.ComplianceCategoryList, element.NodeCode);
                CompliancePackage cmpPackage = GetExistingPackage(clientData.CompliancePackageList, element.ParentNodeCode);
                if (cmpCategory.IsNotNull() && cmpPackage.IsNotNull())
                {
                    RemoveClientPackageCategoryMapping(cmpPackage.CompliancePackageID, cmpCategory.ComplianceCategoryID, currentUserId);

                    parentObjectTypeCode = LCObjectType.CompliancePackage.GetStringValue();
                    newObjectTypeCode = LCObjectType.ComplianceCategory.GetStringValue();

                    parentObjectTypeId = clientData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == parentObjectTypeCode && x.OT_IsDeleted == false).OT_ID;
                    newObjectTypeId = clientData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == newObjectTypeCode && x.OT_IsDeleted == false).OT_ID;
                    DeleteAssociationHierarchyNode(parentObjectTypeId, cmpPackage.CompliancePackageID, newObjectTypeId, cmpCategory.ComplianceCategoryID, currentUserId);
                }
            }


            var packageToRemove = lstElementsToRemove.Where(x => x.UICode == RuleSetTreeNodeType.Package).ToList();

            foreach (var element in packageToRemove)
            {
                CompliancePackage cmpPackage = GetExistingPackage(clientData.CompliancePackageList, element.NodeCode);

                if (cmpPackage.IsNotNull())
                {
                    RemoveClientPackage(cmpPackage.CompliancePackageID, currentUserId);
                    RemoveDocumentURLMappingAndURL(cmpPackage.CompliancePackageID, currentUserId);
                    CopyInstitutionPackageWebpage(cmpPackage.CompliancePackageID, currentUserId, tenantID, true);

                    newObjectTypeCode = LCObjectType.CompliancePackage.GetStringValue();
                    newObjectTypeId = clientData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == newObjectTypeCode && x.OT_IsDeleted == false).OT_ID;
                    DeleteAssociationHierarchyNode(null, null, newObjectTypeId, cmpPackage.CompliancePackageID, currentUserId);
                }
            }
            #endregion

            _ClientDBContext.SaveChanges();

            //Saves all the rules associated with the ruleset added in the client database.
            CopyRulesToClient(adminData, clientData, currentUserId, tenantID);
        }

        /// <summary>
        /// Checks if Category Item mapping already existxs in database or added in transaction while copying data to client.
        /// </summary>
        /// <param name="lstClientCategoryItem">transaction category item maintained list.</param>
        /// <param name="categoryId">categoryId</param>
        /// <param name="itemId">itemId</param>
        /// <returns>True if mapping exists</returns>
        private Boolean CheckCategoryItemMappingExist(List<ComplianceCategoryItem> lstClientCategoryItem, Int32 categoryId, Int32 itemId)
        {
            if (lstClientCategoryItem.Count > 0 && lstClientCategoryItem.Any(ci => ci.CCI_IsDeleted == false && ci.CCI_ItemID == itemId && ci.CCI_CategoryID == categoryId))
            {
                return true;
            }
            return _ClientDBContext.ComplianceCategoryItems.Any(ci => ci.CCI_IsDeleted == false && ci.CCI_ItemID == itemId && ci.CCI_CategoryID == categoryId);
        }

        /// <summary>
        /// Checks if Item Attribute mapping already existxs in database or added in transaction while copying data to client.
        /// </summary>
        /// <param name="lstClientItemAttribute">transaction Item Attribute maintained list.</param>
        /// <param name="attributeId">attributeId</param>
        /// <param name="itemId">itemId</param>
        /// <returns>True if mapping exists</returns>
        private Boolean CheckItemAttributeMappingExist(List<ComplianceItemAttribute> lstClientItemAttribute, Int32 attributeId, Int32 itemId)
        {
            if (lstClientItemAttribute.Count > 0 && lstClientItemAttribute.Any(ia => ia.CIA_IsDeleted == false && ia.CIA_AttributeID == attributeId && ia.CIA_ItemID == itemId))
            {
                return true;
            }
            return _ClientDBContext.ComplianceItemAttributes.Any(ia => ia.CIA_IsDeleted == false && ia.CIA_AttributeID == attributeId && ia.CIA_ItemID == itemId);
        }

        /// <summary>
        /// Checks if Package Category mapping already existxs in database or added in transaction while copying data to client.
        /// </summary>
        /// <param name="lstClientPackageCategory">transaction Package Category maintained list.</param>
        /// <param name="categoryId">categoryId</param>
        /// <param name="packageId">packageId</param>
        /// <returns>True if mapping exists</returns>
        private Boolean CheckPackageCategoryMappingExist(List<CompliancePackageCategory> lstClientPackageCategory, Int32 categoryId, Int32 packageId)
        {
            if (lstClientPackageCategory.Count > 0 && lstClientPackageCategory.Any(pc => pc.CPC_IsDeleted == false && pc.CPC_PackageID == packageId &&
                pc.CPC_CategoryID == categoryId))
            {
                return true;
            }
            return _ClientDBContext.CompliancePackageCategories.Any(pc => pc.CPC_IsDeleted == false && pc.CPC_PackageID == packageId &&
                pc.CPC_CategoryID == categoryId);
        }

        public CompliancePackage GetPackageDetailsByCode(Guid? copiedFromCode)
        {
            return ClientDBContext.CompliancePackages.Where(cp => cp.CopiedFromCode == copiedFromCode && cp.IsDeleted == false && cp.IsCreatedByAdmin == true).FirstOrDefault();
        }

        public ComplianceCategory GetCategoryDetailsByCode(Guid? copiedFromCode)
        {
            return ClientDBContext.ComplianceCategories.Where(cc => cc.CopiedFromCode == copiedFromCode && cc.IsDeleted == false && cc.IsCreatedByAdmin == true).FirstOrDefault();
        }

        public ComplianceItem GetItemDetailsByCode(Guid? copiedFromCode)
        {
            return ClientDBContext.ComplianceItems.Where(ci => ci.CopiedFromCode == copiedFromCode && ci.IsDeleted == false && ci.IsCreatedByAdmin == true).FirstOrDefault();
        }
        public ComplianceAttribute GetAttributeDetailsByCode(Guid? copiedFromCode)
        {
            return ClientDBContext.ComplianceAttributes.Where(ca => ca.CopiedFromCode == copiedFromCode && ca.IsDeleted == false && ca.IsCreatedByAdmin == true).FirstOrDefault();
        }

        public Boolean UpdateComplianceItemAttributeDisplayOrder(Int32 complianceAttributeID, Int32 itemID, Int32 displayOrder, Int32 currentloggedInUserId)
        {
            ComplianceItemAttribute complianceItemAttribute = _ClientDBContext.ComplianceItemAttributes.Where(c => c.CIA_AttributeID == complianceAttributeID && c.CIA_ItemID == itemID && c.CIA_IsActive && !c.CIA_IsDeleted).FirstOrDefault();

            if (complianceItemAttribute.IsNotNull())
            {
                complianceItemAttribute.CIA_DisplayOrder = displayOrder;
                complianceItemAttribute.CIA_CreatedByID = currentloggedInUserId;
                complianceItemAttribute.CIA_ModifiedOn = DateTime.Now;
                _ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        #region COPY TO CLIENT - SPECIFIC METHODS

        /// <summary>
        /// Adds the new package to the client, if not exists, and returns its id for mapping storage
        /// </summary>
        /// <param name="currentUserId">Id of the currently logged in user</param>
        /// <param name="nodeCode">Code of the package in the admin list</param>
        /// <param name="lstAdminPackages">Master List of Admin packages</param>
        /// <returns>Id of the newly added element</returns>
        private CompliancePackage AddGetPackageId(Int32 currentUserId, Guid? nodeCode, List<CompliancePackage> lstAdminPackages, Int32 tenantID)
        {
            CompliancePackage adminPackage = lstAdminPackages.Where(p => p.Code == nodeCode).FirstOrDefault();
            CompliancePackage clientPackageToCreate = new CompliancePackage
            {
                PackageName = adminPackage.PackageName,
                PackageLabel = adminPackage.PackageLabel,
                ScreenLabel = adminPackage.ScreenLabel,
                IsActive = true,
                IsDeleted = false,
                CopiedFromCode = adminPackage.Code,
                Code = Guid.NewGuid(),
                Description = adminPackage.Description,
                IsCreatedByAdmin = true,
                TenantID = tenantID,
                IsViewDetailsInOrderEnabled = adminPackage.IsViewDetailsInOrderEnabled,
                PackageDetail = adminPackage.PackageDetail,
                CompliancePackageTypeID = adminPackage.CompliancePackageTypeID,
                ChecklistURL = adminPackage.ChecklistURL,
                NotesDisplayPositionId = adminPackage.NotesDisplayPositionId //UAT-2219 Added New Column in Compliance Package
            };
            return SaveCompliancePackageDetail(clientPackageToCreate, currentUserId);
        }

        /// <summary>
        /// Adds the new category to the client, if not exists, and returns its id for mapping storage
        /// </summary>
        /// <param name="currentUserId">Id of the currently logged in user</param>
        /// <param name="nodeCode">Code of the category in the admin list</param>
        /// <param name="lstAdminCategories">Master List of Admin categories</param>
        /// <returns>Id of the newly added element</returns>
        private ComplianceCategory AddGetCategoryId(Int32 currentUserId, Guid? nodeCode, List<ComplianceCategory> lstAdminCategories, Int32 tenanatId)
        {
            ComplianceCategory adminCategory = lstAdminCategories.Where(c => c.Code == nodeCode).FirstOrDefault();
            ComplianceCategory clientCategoryToCreate = new ComplianceCategory
            {
                CategoryName = adminCategory.CategoryName,
                CategoryLabel = adminCategory.CategoryLabel,
                ScreenLabel = adminCategory.ScreenLabel,
                Description = adminCategory.Description,
                IsDeleted = false,
                IsActive = true,
                IsCreatedByAdmin = true,
                CopiedFromCode = adminCategory.Code,
                Code = Guid.NewGuid(),
                SampleDocFormURL = adminCategory.SampleDocFormURL,
                SampleDocFormURLLabel = adminCategory.SampleDocFormURLLabel,  //UAT-3161
                SendItemDocOnApproval = adminCategory.SendItemDocOnApproval,//UAT-3805
                TenantID = tenanatId
            };
            return SaveCategoryDetail(clientCategoryToCreate, currentUserId);
        }

        /// <summary>
        /// Adds the new item to the client, if not exists, and returns its id for mapping storage
        /// </summary>
        /// <param name="currentUserId">Id of the currently logged in user</param>
        /// <param name="nodeCode">Code of the item in the admin list</param>
        /// <param name="lstAdminItems">Master List of Admin items</param>
        /// <returns>Id of the newly added element</returns>
        private ComplianceItem AddGetItemId(Int32 currentUserId, Guid? nodeCode, List<ComplianceItem> lstAdminItems, Int32 tenantId)
        {
            ComplianceItem adminItem = lstAdminItems.Where(i => i.Code == nodeCode).FirstOrDefault();
            ComplianceItem clientItemToCreate = new ComplianceItem
            {
                Name = adminItem.Name,
                ItemLabel = adminItem.ItemLabel,
                ScreenLabel = adminItem.ScreenLabel,
                Description = adminItem.ScreenLabel,
                SampleDocFormURL = adminItem.SampleDocFormURL,
                IsDeleted = false,
                IsActive = true,
                IsCreatedByAdmin = true,
                CopiedFromCode = adminItem.Code,
                Code = Guid.NewGuid(),
                TenantID = tenantId,
                //UAT-3077
                IsPaymentType = adminItem.IsPaymentType,
                Amount = adminItem.Amount
            };
            return SaveComplianceItem(clientItemToCreate, currentUserId);
        }

        /// <summary>
        /// Adds the new attribute to the client, if not exists, and returns its id for mapping storage
        /// </summary>
        /// <param name="currentUserId">Id of the currently logged in user</param>
        /// <param name="nodeCode">Code of the attribute in the admin list</param>
        /// <param name="lstAdminAttributes">Master List of Admin attributes</param>
        /// <returns>Id of the newly added element</returns>
        private ComplianceAttribute AddGetAttributeId(Int32 currentUserId, Guid? nodeCode, List<ComplianceAttribute> lstAdminAttributes, Int32 tenantId)
        {
            ComplianceAttribute adminAttribute = lstAdminAttributes.Where(a => a.Code == nodeCode).FirstOrDefault();
            ComplianceAttributeGroup adminAttributeGroup = null;
            ComplianceAttributeGroup clientAttributeGroup = null;
            if (adminAttribute.ComplianceAttributeGroup.IsNotNull())
            {
                adminAttributeGroup = adminAttribute.ComplianceAttributeGroup;
                clientAttributeGroup = _ClientDBContext.ComplianceAttributeGroups.Where(cnd => cnd.CAG_CopiedFromCode == adminAttribute.ComplianceAttributeGroup.CAG_Code).FirstOrDefault();
            }
            ComplianceAttribute clientAttributeToCreate = new ComplianceAttribute
            {
                Name = adminAttribute.Name,
                AttributeLabel = adminAttribute.AttributeLabel,
                ScreenLabel = adminAttribute.ScreenLabel,
                Description = adminAttribute.Description,
                IsDeleted = false,
                IsActive = true,
                IsCreatedByAdmin = true,
                CopiedFromCode = adminAttribute.Code,
                Code = Guid.NewGuid(),
                MaximumCharacters = adminAttribute.MaximumCharacters,
                ComplianceAttributeDatatypeID = adminAttribute.ComplianceAttributeDatatypeID,
                ComplianceAttributeTypeID = adminAttribute.ComplianceAttributeTypeID,
                TenantID = tenantId,
                //UAT-2023: Reconciliation: Addition of attribute-level setting to enable/disable trigger for reconciliation queue.
                IsTriggersReconciliation = adminAttribute.IsTriggersReconciliation,
                ///Commit4383
                // IsSendForintegration=adminAttribute.IsSendForintegration
            };

            List<ComplianceAttributeOption> lstTempOptions = adminAttribute.ComplianceAttributeOptions.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
            foreach (var option in lstTempOptions)
            {
                clientAttributeToCreate.ComplianceAttributeOptions.Add(new ComplianceAttributeOption
                {
                    ComplianceItemAttributeID = option.ComplianceItemAttributeID,
                    OptionValue = option.OptionValue,
                    OptionText = option.OptionText,
                    IsDeleted = false,
                    IsActive = true,
                    CreatedByID = currentUserId,
                    CreatedOn = DateTime.Now
                });
            }
            //Check for clientAttributeGroup and adminAttributeGroup is null or not.
            if (clientAttributeGroup.IsNullOrEmpty() && adminAttributeGroup.IsNotNull())
            {
                clientAttributeToCreate.ComplianceAttributeGroup = new ComplianceAttributeGroup
                {
                    CAG_Name = adminAttributeGroup.CAG_Name,
                    CAG_Label = adminAttributeGroup.CAG_Label,
                    CAG_TenantID = adminAttributeGroup.CAG_TenantID,
                    CAG_Code = Guid.NewGuid(),
                    CAG_CopiedFromCode = adminAttributeGroup.CAG_Code,
                    CAG_IsDeleted = false,
                    CAG_IsCreatedByAdmin = true,
                    CAG_CreatedByID = currentUserId,
                    CAG_CreatedOn = DateTime.Now
                };
            }
            else if (!clientAttributeGroup.IsNullOrEmpty())
            {
                clientAttributeToCreate.ComplianceAttributeGroupID = clientAttributeGroup.CAG_ID;
            }

            //UAT-4558
            List<ComplianceAttributeDocMapping> lstComplianceAttrDocMappings = adminAttribute.ComplianceAttributeDocMappings.Where(x => x.CADM_IsDeleted == false).ToList();
            foreach (var docMapping in lstComplianceAttrDocMappings)
            {
                clientAttributeToCreate.ComplianceAttributeDocMappings.Add(new ComplianceAttributeDocMapping
                {
                    CADM_SystemDocumentID = docMapping.CADM_SystemDocumentID,
                    CADM_CreatedOn = DateTime.Now,
                    CADM_CreatedBy = currentUserId,
                    CADM_IsDeleted = false,
                });
            }
            //END

            return SaveComplianceAttribute(clientAttributeToCreate, currentUserId);
        }

        private CompliancePackage GetExistingPackage(List<CompliancePackage> lstPackages, Guid? copiedFromCode)
        {
            return lstPackages.Where(cp => cp.CopiedFromCode.IsNotNull() && cp.CopiedFromCode == copiedFromCode && cp.IsDeleted == false).FirstOrDefault();
        }
        private ComplianceCategory GetExistingCategory(List<ComplianceCategory> lstCategories, Guid? copiedFromCode)
        {
            return lstCategories.Where(cc => cc.CopiedFromCode.IsNotNull() && cc.CopiedFromCode == copiedFromCode && cc.IsDeleted == false).FirstOrDefault();
        }
        private ComplianceItem GetExistingItem(List<ComplianceItem> lstComplianceItems, Guid? copiedFromCode)
        {
            return lstComplianceItems.Where(ci => ci.CopiedFromCode.IsNotNull() && ci.CopiedFromCode == copiedFromCode && ci.IsDeleted == false).FirstOrDefault();
        }
        private ComplianceAttribute GetExistingAttribute(List<ComplianceAttribute> lstComplianceAttributes, Guid? copiedFromCode)
        {
            return lstComplianceAttributes.Where(ca => ca.CopiedFromCode.IsNotNull() && ca.CopiedFromCode == copiedFromCode && ca.IsDeleted == false).FirstOrDefault();
        }

        public List<CompliancePackageCategory> GetCompliancePackageCategoryList()
        {
            return _ClientDBContext.CompliancePackageCategories.Where(obj => obj.CPC_IsActive == true && obj.CPC_IsDeleted == false).ToList();
        }

        public List<Entity.InstitutionWebPage> GetAdminInstitutionWebPageList(List<GetRuleSetTree> lstComplainceElements, String recordTypeCode, Int32 tenantId)
        {
            Int16 recordTypeId = Context.lkpRecordTypes.FirstOrDefault(x => x.Code.Equals(recordTypeCode)).RecordTypeID;
            List<Entity.InstitutionWebPage> lstInstitutionWebPage = new List<Entity.InstitutionWebPage>();
            foreach (var element in lstComplainceElements)
            {
                Entity.InstitutionWebPage institutionWebPage = Context.InstitutionWebPages.FirstOrDefault(x => x.RecordID == element.DataID && x.RecordTypeID == recordTypeId
                            && x.IsDeleted == false && x.IsActive == true && x.TenantID == tenantId);
                if (institutionWebPage.IsNotNull())
                {
                    lstInstitutionWebPage.Add(institutionWebPage);
                }
            }
            return lstInstitutionWebPage;
        }


        /// <summary>
        /// Removes the Package
        /// </summary>
        /// <param name="packageId">Id of the package</param>
        /// <param name="currentUserId">Id of the currently logged in user</param>
        private void RemoveClientPackage(Int32 packageId, Int32 currentUserId)
        {
            var cmpPackage = _ClientDBContext.CompliancePackages.Where(cp => cp.CompliancePackageID == packageId && cp.IsDeleted == false);
            foreach (var item in cmpPackage)
            {
                item.ModifiedByID = currentUserId;
                item.ModifiedOn = DateTime.Now;
                item.IsDeleted = true;
            }
        }

        private void RemoveDocumentURLMappingAndURL(Int32 PackageId, Int32 currentUserId)
        {
            var TRPackage = _ClientDBContext.TrackingPackageRequiredDocURLMappings.Where(TR => TR.TPRPM_CompliancePackageId == PackageId && TR.TPRPM_IsDeleted == false);
            List<int> TEMPValues = new List<int>();

            foreach (var item in TRPackage)
            {
                item.TPRPM_ModifiedByID = currentUserId;
                item.TPRPM_ModifiedOn = DateTime.Now;
                item.TPRPM_IsDeleted = true;
                TEMPValues.Add(item.TPRPM_URLID);


            }
            foreach (var URLID in TEMPValues)
            {
                var TRPackageURl = _ClientDBContext.TrackingPackageRequiredDocURLs.Where(TR => TR.TPRDU_ID == URLID && TR.TPRDU_IsDeleted == false);
                foreach (var item in TRPackageURl)
                {
                    item.TPRDU_ModifiedByID = currentUserId;
                    item.TPRDU_ModifiedOn = DateTime.Now;
                    item.TPRDU_IsDeleted = true;
                }

            }


            _ClientDBContext.SaveChanges();
        }


        /// <summary>
        /// Removes the Package-Category Mapping
        /// </summary>
        /// <param name="packageId">Id of the package</param>
        /// <param name="categoryId">Id of the category</param>
        /// <param name="currentUserId">Id of the currently logged in user</param>
        private void RemoveClientPackageCategoryMapping(Int32 packageId, Int32 categoryId, Int32 currentUserId)
        {
            var cmpPackageCategory = _ClientDBContext.CompliancePackageCategories.Where(cpc => cpc.CPC_PackageID == packageId && cpc.CPC_CategoryID == categoryId && cpc.CPC_IsDeleted == false);
            foreach (var item in cmpPackageCategory)
            {
                item.CPC_ModifiedByID = currentUserId;
                item.CPC_ModifiedOn = DateTime.Now;
                item.CPC_IsDeleted = true;
            }
        }

        /// <summary>
        /// Removes the Category-Item Mapping
        /// </summary>
        /// <param name="itemId">Id of the Item</param>
        /// <param name="categoryId">Id of the category</param>
        /// <param name="currentUserId">Id of the currently logged in user</param>
        private void RemoveClientCategoryItemMapping(Int32 categoryId, Int32 itemId, Int32 currentUserId)
        {
            var cmpCategoryItem = _ClientDBContext.ComplianceCategoryItems.Where(cci => cci.CCI_CategoryID == categoryId && cci.CCI_ItemID == itemId && cci.CCI_IsDeleted == false);
            foreach (var item in cmpCategoryItem)
            {
                item.CCI_ModifiedByID = currentUserId;
                item.CCI_ModifiedOn = DateTime.Now;
                item.CCI_IsDeleted = true;
            }
        }

        /// <summary>
        /// Removes the Item-Attribtue Mapping
        /// </summary>
        /// <param name="attributeId">Id of the Attribute</param>
        /// <param name="itemId">Id of the Item</param>
        /// <param name="currentUserId">Id of the currently logged in user</param>
        private void RemoveClientItemAttributeMapping(Int32 itemId, Int32 attributeId, Int32 currentUserId)
        {
            var cmpItemAttribute = _ClientDBContext.ComplianceItemAttributes.Where(cia => cia.CIA_ItemID == itemId && cia.CIA_AttributeID == attributeId && cia.CIA_IsDeleted == false);
            foreach (var item in cmpItemAttribute)
            {
                item.CIA_ModifiedByID = currentUserId;
                item.CIA_ModifiedOn = DateTime.Now;
                item.CIA_IsDeleted = true;
            }
        }


        private void SaveClientPackageCategoryMapping(Int32 packageId, Int32 categoryId, Int32 displayOrder, Boolean complianceRequired, Int32 currentUserId, List<CompliancePackageCategory> lstClientPackageCategory)
        {
            CompliancePackageCategory compliancePackageCategory = new CompliancePackageCategory
            {
                CPC_CategoryID = categoryId,
                CPC_PackageID = packageId,
                CPC_IsActive = true,
                CPC_CreatedByID = currentUserId,
                CPC_CreatedOn = DateTime.Now,
                CPC_IsCreatedByAdmin = true,
                CPC_DisplayOrder = displayOrder,
                CPC_ComplianceRequired = complianceRequired
            };

            //Int32 _lastDisplayOrder = _ClientDBContext.CompliancePackageCategories.Where(pc => pc.CPC_CategoryID == categoryId && pc.CPC_PackageID == packageId).
            //    OrderByDescending(pc => pc.CPC_ID).Select(pc => pc.CPC_DisplayOrder).FirstOrDefault();

            //compliancePackageCategory.CPC_DisplayOrder = _lastDisplayOrder + 1;
            _ClientDBContext.CompliancePackageCategories.AddObject(compliancePackageCategory);
            lstClientPackageCategory.Add(compliancePackageCategory);
        }

        public void SaveClientCategoryItemMapping(Int32 categoryId, Int32 itemId, Int32 currentUserId, List<ComplianceCategoryItem> lstClientCategoryItem)
        {
            ComplianceCategoryItem complianceCategoryItem = new ComplianceCategoryItem
            {
                CCI_CategoryID = categoryId,
                CCI_ItemID = itemId,
                CCI_IsActive = true,
                CCI_IsDeleted = false,
                CCI_CreatedByID = currentUserId,
                CCI_CreatedOn = DateTime.Now,
                CCI_IsCreatedByAdmin = true
            };
            Int32 _lastDisplayOrder = _ClientDBContext.ComplianceCategoryItems.Where(ci => ci.CCI_CategoryID == categoryId && ci.CCI_ItemID == itemId)
                .OrderByDescending(ci => ci.CCI_ID).Select(ci => ci.CCI_DisplayOrder).FirstOrDefault();

            complianceCategoryItem.CCI_DisplayOrder = _lastDisplayOrder + 1;
            _ClientDBContext.ComplianceCategoryItems.AddObject(complianceCategoryItem);
            ClientDBContext.SaveChanges();
            lstClientCategoryItem.Add(complianceCategoryItem);
        }

        public Int32 SaveItemAttributeMapping(Int32 itemId, Int32 attributeId, Int32 currentUserId, List<ComplianceItemAttribute> lstClientItemAttribute)
        {
            ComplianceItemAttribute cmpAttribute = new ComplianceItemAttribute
            {
                CIA_ItemID = itemId,
                CIA_AttributeID = attributeId,
                CIA_IsActive = true,
                CIA_IsDeleted = false,
                CIA_CreatedByID = currentUserId,
                CIA_CreatedOn = DateTime.Now,
                CIA_IsCreatedByAdmin = true
            };
            Int32 _lastDisplayOrder = _ClientDBContext.ComplianceItemAttributes.Where(ia => ia.CIA_ItemID == itemId && ia.CIA_AttributeID == attributeId)
                .OrderByDescending(order => order.CIA_ID).Select(order => order.CIA_DisplayOrder).FirstOrDefault();

            cmpAttribute.CIA_DisplayOrder = _lastDisplayOrder + 1;
            ClientDBContext.ComplianceItemAttributes.AddObject(cmpAttribute);
            lstClientItemAttribute.Add(cmpAttribute);
            ClientDBContext.SaveChanges();
            return cmpAttribute.CIA_ID;
        }

        public List<LargeContent> GetLargeContent()
        {
            return _ClientDBContext.LargeContents.Where(obj => obj.LC_IsDeleted == false).ToList();
        }

        public List<LargeContent> GetLargeContentByObjectId(List<LargeContent> lstAdminLargeContent, Int32? objectId, String objectTypeCode)
        {
            Int32 objectTypeID = GetLargeObjectTypeIdByCode(objectTypeCode);
            return lstAdminLargeContent.Where(obj => obj.LC_ObjectID == objectId && obj.LC_ObjectTypeID == objectTypeID).ToList();
        }

        public void CopylargeContentForClient(LargeContent contentToCopy, Int32 objectId, Int32 currentUserId)
        {
            LargeContent newContent = new LargeContent
            {
                LC_ObjectID = objectId,
                LC_ObjectTypeID = contentToCopy.LC_ObjectTypeID,
                LC_LargeContentTypeID = contentToCopy.LC_LargeContentTypeID,
                LC_Content = contentToCopy.LC_Content,
                LC_IsDeleted = false,
                LC_CreatedByID = currentUserId,
                LC_CreatedOn = DateTime.Now
            };
            _ClientDBContext.LargeContents.AddObject(newContent);
        }

        //private void CopyRuleSetToClientNew(ComplianceSetUpContract adminData, ComplianceSetUpContract clientData, String objectType, Int32 adminObjectId, Int32 objectId, Int32 tenantId, Int32 currentLoggedInUser)
        //{
        //    RuleSet newRuleSet = null;
        //    Int32 assignmentHierarchyID = 0;
        //    if (adminData.RuleSetObjectList.IsNotNull())
        //    {
        //        //Finds the Object Type ID from lkpObjectType in Client DB.
        //        var lkpObjectType = clientData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == objectType);

        //        //Finds the  Ruleset row from Master DB need to be Copied to the Client DB.
        //        List<RuleSet> ruleSetToBeCopy = adminData.RuleSetObjectList.Where(x => x.RLSO_IsDeleted == false && x.RLSO_ObjectID == adminObjectId &&
        //            lkpObjectType.IsNotNull() && x.RLSO_ObjectTypeID == lkpObjectType.OT_ID).Select(obj => obj.RuleSet).ToList();

        //        foreach (var ruleSet in ruleSetToBeCopy)
        //        {
        //            assignmentHierarchyID = GetClientAssignmentHierarchyForRuleset(adminData, ruleSet.RLS_AssignmentHierarchyID);
        //            //Finds the  Ruleset row in Client DB.
        //            var clientRuleSet = ClientDBContext.RuleSets.FirstOrDefault(x => x.RLS_CopiedFromCode == ruleSet.RLS_Code);
        //            RuleSetObject adminRuleSetObject = adminData.RuleSetObjectList.FirstOrDefault(x => x.RLSO_IsDeleted == false && x.RLSO_ObjectID == adminObjectId &&
        //                    lkpObjectType.IsNotNull() && x.RLSO_ObjectTypeID == lkpObjectType.OT_ID && x.RuleSet.RLS_ID == ruleSet.RLS_ID);

        //            if (clientRuleSet.IsNullOrEmpty())
        //            {
        //                adminData.RuleSetList.Add(ruleSet);
        //                newRuleSet = AssignValueToRuleset(tenantId, currentLoggedInUser, ruleSet, assignmentHierarchyID);
        //                ClientDBContext.RuleSets.AddObject(newRuleSet);
        //                newRuleSet.RuleSetObjects.Add(AssignValueToRulesetObject(adminRuleSetObject, currentLoggedInUser, objectId, null));
        //            }
        //            else
        //            {
        //                ClientDBContext.RuleSetObjects.AddObject(AssignValueToRulesetObject(adminRuleSetObject, currentLoggedInUser, objectId, clientRuleSet.RLS_ID));
        //            }
        //            ClientDBContext.SaveChanges();
        //        }
        //    }
        //}

        private void CopyRuleSetToClient(ComplianceSetUpContract adminData, String objectType, Int32 objectId, String parentObjectType, Int32? parentObjectId, Int32 tenantId, Int32 currentLoggedInUser)
        {
            RuleSet newRuleSet = null;
            List<Int32> lstAssignmentHierarchyID = null;
            //Finds the Object Type ID from lkpObjectType in Admin DB
            var objectTypeID = adminData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == objectType).OT_ID;
            var parentObjectTypeID = adminData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == parentObjectType).OT_ID;

            if (parentObjectId.IsNull())
            {
                lstAssignmentHierarchyID = adminData.AssignmentHierarchyList.Where(ah => ah.ObjectTypeID == objectTypeID && ah.ObjectID == objectId && ah.IsDeleted == false
                                && ah.AssignmentHierarchy2.ObjectTypeID == parentObjectTypeID).Select(x => x.AssignmentHierarchyID).ToList();
            }
            else
            {
                lstAssignmentHierarchyID = adminData.AssignmentHierarchyList.Where(ah => ah.ObjectTypeID == objectTypeID && ah.ObjectID == objectId && ah.IsDeleted == false
                                && ah.AssignmentHierarchy2.ObjectTypeID == parentObjectTypeID && ah.AssignmentHierarchy2.ObjectID == parentObjectId.Value).Select(x => x.AssignmentHierarchyID).ToList();
            }
            //Finds the  Ruleset row from Master DB need to be Copied to the Client DB.
            List<RuleSet> ruleSetToBeCopy = adminData.RuleSetList.Where(x => x.RLS_IsDeleted == false && lstAssignmentHierarchyID.Contains(x.RLS_AssignmentHierarchyID.Value)).ToList();

            Int32? assignmentHierarchyID = 0;
            foreach (var ruleSet in ruleSetToBeCopy)
            {
                //Finds the  Ruleset row in Client DB.
                var clientRuleSet = ClientDBContext.RuleSets.FirstOrDefault(x => x.RLS_CopiedFromCode == ruleSet.RLS_Code && x.RLS_IsDeleted == false);

                if (clientRuleSet.IsNullOrEmpty())
                {
                    assignmentHierarchyID = GetClientAssignmentHierarchyForRuleset(adminData, ruleSet.RLS_AssignmentHierarchyID, tenantId);
                    //adminData.RuleSetList.Add(ruleSet);
                    if (assignmentHierarchyID.IsNotNull())
                    {
                        adminData.RuleSetToBeCopied.Add(ruleSet);
                        newRuleSet = AssignValueToRuleset(tenantId, currentLoggedInUser, ruleSet, assignmentHierarchyID.Value);
                        ClientDBContext.RuleSets.AddObject(newRuleSet);
                    }
                }
                ClientDBContext.SaveChanges();
            }
        }


        private void CopyRulesToClient(ComplianceSetUpContract adminData, ComplianceSetUpContract clientData, Int32 currentLoggedInUser, Int32 tenantId)
        {
            //Loop executes for the ruleset added in client Db.
            foreach (var ruleSet in adminData.RuleSetToBeCopied)
            {
                //Gets all the rows of RuleMapping Table for Ruleset Object from Master DB.
                List<RuleMapping> ruleMappingToCopy = adminData.RuleMappingList.Where(x => x.RLM_IsDeleted == false && x.RLM_RuleSetID == ruleSet.RLS_ID).Select(x => x).ToList();
                //Finds the ruleset present in Client DB using ruleset code from Master DB.
                RuleSet clientRuleset = ClientDBContext.RuleSets.FirstOrDefault(x => x.RLS_CopiedFromCode == ruleSet.RLS_Code && x.RLS_IsDeleted == false);
                foreach (var ruleMapping in ruleMappingToCopy)
                {
                    // RLT_CopiedFromCode,RLT_IsCreatedByAdmin
                    RuleTemplate clientRuleTemplate = ClientDBContext.RuleTemplates.FirstOrDefault(x => x.RLT_CopiedFromCode == ruleMapping.RuleTemplate.RLT_Code);
                    RuleTemplate newRuleTemplate = null;
                    Int32 ruleTemplateId = 0;
                    //Checks if Rule Template already exist in Client DB
                    if (clientRuleTemplate.IsNullOrEmpty())
                    {
                        newRuleTemplate = AssignValueToRuleTemplate(currentLoggedInUser, ruleMapping);
                        ClientDBContext.RuleTemplates.AddObject(newRuleTemplate);

                        //Gets all the rows of RuleTemplateExpression table for RuleTemplate Object from Master DB.
                        List<RuleTemplateExpression> lstRuleTempExpRows = adminData.RuleTemplateExpressionList.Where(x => x.RLE_RuleID == ruleMapping.RuleTemplate.RLT_ID &&
                            x.RLE_IsDeleted == false).ToList();

                        foreach (RuleTemplateExpression ruleTempExp in lstRuleTempExpRows)
                        {
                            Expression newExpression = AssignValueToExpression(currentLoggedInUser, ruleTempExp);
                            RuleTemplateExpression RuleTemplateExpression = AssignValueToRuleTempExpresn(currentLoggedInUser, newRuleTemplate, ruleTempExp, newExpression);
                            ClientDBContext.RuleTemplateExpressions.AddObject(RuleTemplateExpression);
                        }
                    }
                    else
                    {
                        newRuleTemplate = clientRuleTemplate;
                        ruleTemplateId = clientRuleTemplate.RLT_ID;
                    }

                    RuleMapping newRuleMapping = AssignValueToRuleMapping(currentLoggedInUser, clientRuleset.RLS_ID, ruleMapping, ruleTemplateId);
                    newRuleTemplate.RuleMappings.Add(newRuleMapping);

                    //Gets all the rows of RuleMappingDetail table for RuleMapping Object from Master DB.
                    List<RuleMappingDetail> lstRuleMappingDetailRows = adminData.RuleMappingDetailList.Where(x => x.RLMD_RuleMappingID == ruleMapping.RLM_ID &&
                        x.RLMD_IsDeleted == false).ToList();

                    foreach (RuleMappingDetail ruleMappingDetail in lstRuleMappingDetailRows)
                    {
                        RuleMappingDetail newRuleMappingDetail = AssignValueToRuleMappingDetail(currentLoggedInUser, ruleMappingDetail, adminData);
                        newRuleMapping.RuleMappingDetails.Add(newRuleMappingDetail);

                        //Gets all the rows of RuleMappingObjectTree table for RuleMappingDetail Object from Master DB.
                        List<RuleMappingObjectTree> lstRuleMappingObjectTreeRows = adminData.RuleMappingObjectTreeList.Where(x => x.RMOT_RuleMappingDetailID ==
                            ruleMappingDetail.RLMD_ID).ToList();

                        foreach (RuleMappingObjectTree ruleMappingObjectTree in lstRuleMappingObjectTreeRows)
                        {
                            RuleMappingObjectTree newRuleMappingObjectTree = AssignValueToRuleMapObjTree(adminData, ruleMappingObjectTree, ruleSet.RLS_ID);
                            newRuleMappingDetail.RuleMappingObjectTrees.Add(newRuleMappingObjectTree);
                        }
                    }
                }
                try
                {
                    ClientDBContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    DALUtils.LoggerService.GetLogger().Error("DAL ComplianceSetUpRepository CopyRulesToClient : Rules under RuleSet ID = " + ruleSet.RLS_ID + " and Ruleset Name = " + ruleSet.RLS_Name
                        + " cannot be saved in client database.", ex);
                }
            }
        }

        private void CopyAttributeInstructionToClient(ComplianceSetUpContract adminData, String objectType, Int32 objectId, String parentObjectType, Int32? parentObjectId, Int32 tenantId, Int32 currentLoggedInUser, Int32 newCia_Id)
        {
            AttributeInstruction newAttributeInstruction = null;
            List<Int32> lstAssignmentHierarchyID = null;
            //Finds the Object Type ID from lkpObjectType in Admin DB
            var objectTypeID = adminData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == objectType).OT_ID;
            var parentObjectTypeID = adminData.LkpObjectTypeList.FirstOrDefault(x => x.OT_Code == parentObjectType).OT_ID;

            if (parentObjectId.IsNull())
            {
                lstAssignmentHierarchyID = adminData.AssignmentHierarchyList.Where(ah => ah.ObjectTypeID == objectTypeID && ah.ObjectID == objectId && ah.IsDeleted == false
                                && ah.AssignmentHierarchy2.ObjectTypeID == parentObjectTypeID).Select(x => x.AssignmentHierarchyID).ToList();
            }
            else
            {
                lstAssignmentHierarchyID = adminData.AssignmentHierarchyList.Where(ah => ah.ObjectTypeID == objectTypeID && ah.ObjectID == objectId && ah.IsDeleted == false
                                && ah.AssignmentHierarchy2.ObjectTypeID == parentObjectTypeID && ah.AssignmentHierarchy2.ObjectID == parentObjectId.Value).Select(x => x.AssignmentHierarchyID).ToList();
            }
            //Finds the  AttributeInstruction row from Master DB need to be Copied to the Client DB.
            List<AttributeInstruction> attributeInstructionToBeCopy = adminData.AttributeInstructionList.Where(x => x.AI_IsDeleted == false && lstAssignmentHierarchyID.Contains(x.AI_AssignmentHierarchyID.Value)).ToList();

            Int32? assignmentHierarchyID = 0;
            foreach (var attributeInstruction in attributeInstructionToBeCopy)
            {
                assignmentHierarchyID = GetClientAssignmentHierarchyForRuleset(adminData, attributeInstruction.AI_AssignmentHierarchyID, tenantId);
                //adminData.RuleSetList.Add(ruleSet);
                if (assignmentHierarchyID.IsNotNull())
                {

                    newAttributeInstruction = new AttributeInstruction();
                    newAttributeInstruction.AI_AssignmentHierarchyID = assignmentHierarchyID;
                    newAttributeInstruction.AI_ComplianceItemAttributeID = newCia_Id;
                    newAttributeInstruction.AI_InstructionText = attributeInstruction.AI_InstructionText;
                    newAttributeInstruction.AI_IsDeleted = false;
                    newAttributeInstruction.AI_CreatedBy = currentLoggedInUser;
                    newAttributeInstruction.AI_CreatedOn = DateTime.Now;
                    ClientDBContext.AttributeInstructions.AddObject(newAttributeInstruction);
                }
                ClientDBContext.SaveChanges();
            }
        }

        private Int32? GetClientAssignmentHierarchyForRuleset(ComplianceSetUpContract adminData, Int32? assignmentHierarchyId, Int32 tenantId)
        {
            AssignmentHierarchy assignmentHierarchy = null;
            List<Int32> lstAssignmentHierarchy = new List<Int32>();
            while (assignmentHierarchyId != null)
            {
                lstAssignmentHierarchy.Add(assignmentHierarchyId.Value);
                assignmentHierarchy = adminData.AssignmentHierarchyList.FirstOrDefault(obj => obj.AssignmentHierarchyID == assignmentHierarchyId && obj.IsDeleted == false);
                assignmentHierarchyId = assignmentHierarchy.ParentID;
            }
            lstAssignmentHierarchy.Reverse();
            Int32? parentId = null;
            AssignmentHierarchy clientAssignmentHierarchy = null;
            foreach (Int32 id in lstAssignmentHierarchy)
            {
                assignmentHierarchy = adminData.AssignmentHierarchyList.FirstOrDefault(obj => obj.AssignmentHierarchyID == id && obj.IsDeleted == false);

                String objTypeCode = adminData.LkpObjectTypeList.FirstOrDefault(obj => obj.OT_ID == assignmentHierarchy.ObjectTypeID && obj.OT_IsDeleted == false).OT_Code;
                Int32? objectId = GetClientComplianceObjectID(adminData, objTypeCode, assignmentHierarchy.ObjectID, tenantId);

                clientAssignmentHierarchy = ClientDBContext.AssignmentHierarchies.FirstOrDefault(obj => obj.ObjectID == objectId && obj.IsDeleted == false &&
                    obj.ObjectTypeID == assignmentHierarchy.ObjectTypeID && (obj.ParentID == null && null == parentId || (obj.ParentID == parentId)));

                if (clientAssignmentHierarchy.IsNotNull())
                {
                    parentId = clientAssignmentHierarchy.AssignmentHierarchyID;
                }
                else
                {
                    return null;
                }
            }
            return parentId;
        }

        public void CopyInstitutionPackageWebpage(Int32 clientPackageId, Int32 currentUserId, Int32 tenantId, Boolean isDeleted, List<Entity.InstitutionWebPage> adminInstitutionWebPage = null, Int32 adminPackageId = 0)
        {
            String recordTypeCode = RecordType.Package.GetStringValue();
            Int16 recordTypeId = ClientDBContext.lkpRecordTypes.FirstOrDefault(x => x.Code.Equals(recordTypeCode)).RecordTypeID;
            if (isDeleted)
            {
                InstitutionWebPage institutionWebPage = ClientDBContext.InstitutionWebPages.FirstOrDefault(x => x.RecordID == clientPackageId && x.RecordTypeID == recordTypeId
                         && x.IsDeleted == false && x.IsActive == true && x.TenantID == tenantId);
                if (institutionWebPage.IsNotNull())
                {
                    institutionWebPage.IsDeleted = isDeleted;
                    institutionWebPage.IsActive = false;
                    institutionWebPage.ModifiedByID = currentUserId;
                    institutionWebPage.ModifiedOn = DateTime.Now;
                }
            }
            else
            {
                Entity.InstitutionWebPage institutionWebPage = adminInstitutionWebPage.FirstOrDefault(x => x.RecordID == adminPackageId && x.RecordTypeID == recordTypeId
                         && x.IsDeleted == false && x.IsActive == true);
                if (institutionWebPage.IsNotNull())
                {
                    InstitutionWebPage newInstitutionWebPage = new InstitutionWebPage();
                    newInstitutionWebPage.WebSiteWebPageTypeID = institutionWebPage.WebSiteWebPageTypeID;
                    newInstitutionWebPage.PageName = institutionWebPage.PageName;
                    newInstitutionWebPage.HtmlMarkup = institutionWebPage.HtmlMarkup;
                    newInstitutionWebPage.IsActive = true;
                    newInstitutionWebPage.IsDeleted = isDeleted;
                    newInstitutionWebPage.CreatedByID = currentUserId;
                    newInstitutionWebPage.CreatedOn = DateTime.Now;
                    newInstitutionWebPage.RecordID = clientPackageId;
                    newInstitutionWebPage.RecordTypeID = recordTypeId;
                    newInstitutionWebPage.TenantID = tenantId;
                    ClientDBContext.InstitutionWebPages.AddObject(newInstitutionWebPage);
                }
            }
        }

        #endregion

        #endregion

        #region Methods to get look up data

        /// <summary>
        /// Gets all the rows from lkpObjectType
        /// </summary>
        /// <returns>List</returns>
        public List<lkpObjectType> GetlkpObjectType()
        {

            return ClientDBContext.lkpObjectTypes.Where(x => x.OT_IsDeleted == false).ToList();
        }

        /// <summary>
        /// Gets all the rows from lkpRuleObjectMappingType
        /// </summary>
        /// <returns>List</returns>
        public List<lkpRuleObjectMappingType> GetlkpRuleObjMapType()
        {

            return ClientDBContext.lkpRuleObjectMappingTypes.Where(x => x.RMT_IsDeleted == false).ToList();
        }

        /// <summary>
        /// Gets all the rows from lkpRuleObjectMappingType
        /// </summary>
        /// <returns>List</returns>
        public List<RuleSetTree> GetAllRuleSetTree()
        {

            return ClientDBContext.RuleSetTrees.Where(x => x.RST_IsDeleted == false).ToList();
        }

        /// <summary>
        /// Gets all the rows from Assignment Hierarchy
        /// </summary>
        /// <returns>List</returns>
        public List<AssignmentHierarchy> GetAssignmentHierarchy()
        {

            return ClientDBContext.AssignmentHierarchies.Where(x => x.IsDeleted == false).ToList();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method assigns values to the entity of table RuleMappingObjectTree
        /// </summary>
        /// <param name="adminData">ComplianceSetUpContract Object</param>
        /// <param name="ruleMappingObjectTree">RuleMappingObjectTree Row from Master DB</param>
        /// <returns>RuleMappingObjectTree Entity</returns>
        private RuleMappingObjectTree AssignValueToRuleMapObjTree(ComplianceSetUpContract adminData, RuleMappingObjectTree ruleMappingObjectTree, Int32 ruleId)
        {
            RuleMappingObjectTree newRuleMappingObjectTree = new RuleMappingObjectTree();

            newRuleMappingObjectTree.RMOT_RuleSetTreeID = GetRuleSetTreeID(ruleMappingObjectTree.RMOT_RuleSetTreeID, adminData);
            Int32? ComplianceId = GetComplianceObjectID(adminData, ruleMappingObjectTree.RuleSetTree.RST_UICode, ruleMappingObjectTree.RMOT_ObjectID, ruleId);
            if (ComplianceId.HasValue)
            {
                newRuleMappingObjectTree.RMOT_ObjectID = ComplianceId.Value;
            }
            return newRuleMappingObjectTree;
        }

        /// <summary>
        /// Method Gets the Compliance Object ID from client DB using Code and Object Id in master DB.
        /// </summary>
        /// <param name="adminData">adminData</param>
        /// <param name="objTypeCode">Object Type Code</param>
        /// <param name="objectId">Object Id</param>
        /// <returns>Compliance Object Id</returns>
        private Int32? GetComplianceObjectID(ComplianceSetUpContract adminData, String objTypeCode, Int32? objectId, Int32 tenantId, Int32? ruleId = null)
        {
            if (objectId.HasValue)
            {
                Guid complianceCode = new Guid();
                switch (objTypeCode)
                {
                    case RuleSetTreeNodeType.Package:
                        complianceCode = adminData.CompliancePackageList.FirstOrDefault(x => x.CompliancePackageID == objectId).Code;
                        var compliancePackage = ClientDBContext.CompliancePackages.FirstOrDefault(x => x.CopiedFromCode == complianceCode && x.IsDeleted == false);
                        if (compliancePackage.IsNotNull())
                        {
                            return compliancePackage.CompliancePackageID;
                        }
                        break;

                    case RuleSetTreeNodeType.Category:
                        complianceCode = adminData.ComplianceCategoryList.FirstOrDefault(x => x.ComplianceCategoryID == objectId).Code;
                        var complianceCategory = ClientDBContext.ComplianceCategories.FirstOrDefault(x => x.CopiedFromCode == complianceCode && x.IsDeleted == false);
                        if (complianceCategory.IsNotNull())
                        {
                            return complianceCategory.ComplianceCategoryID;
                        }
                        break;

                    case RuleSetTreeNodeType.Item:
                        complianceCode = adminData.ComplianceItemList.FirstOrDefault(x => x.ComplianceItemID == objectId).Code;
                        var complianceItem = ClientDBContext.ComplianceItems.FirstOrDefault(x => x.CopiedFromCode == complianceCode && x.IsDeleted == false);
                        if (complianceItem.IsNotNull())
                        {
                            return complianceItem.ComplianceItemID;
                        }
                        break;

                    case RuleSetTreeNodeType.Attribute:
                        complianceCode = adminData.ComplianceAttributeList.FirstOrDefault(x => x.ComplianceAttributeID == objectId).Code;
                        var complianceAttribute = ClientDBContext.ComplianceAttributes.FirstOrDefault(x => x.CopiedFromCode == complianceCode && x.IsDeleted == false);
                        if (complianceAttribute.IsNotNull())
                        {
                            return complianceAttribute.ComplianceAttributeID;
                        }
                        break;
                }

                if (ruleId.IsNotNull())
                {
                    DALUtils.LoggerService.GetLogger().Info("DAL ComplianceSetUpRepository GetComplianceObjectID: Tenant ID = " + tenantId + ", Object ID = " + objectId + ", Object Type Code = " + objTypeCode
                        + " and Object Code = " + complianceCode + " in RuleMapping ID :" + ruleId + " is not found in client database.");
                }
                else
                {
                    DALUtils.LoggerService.GetLogger().Info("DAL ComplianceSetUpRepository GetComplianceObjectID: Tenant ID = " + tenantId + ", Object ID = " + objectId + ", Object Type Code = " + objTypeCode
                        + " and Object Code = " + complianceCode + " is not found in client database while copying Assignment Hierarchy.");
                }
            }
            return null;
        }

        /// <summary>
        /// Method Gets the Compliance Object ID from client DB using Code and Object Id in master DB.
        /// </summary>
        /// <param name="adminData">adminData</param>
        /// <param name="objTypeCode">Object Type Code</param>
        /// <param name="objectId">Object Id</param>
        /// <returns>Compliance Object Id</returns>
        private Int32? GetClientComplianceObjectID(ComplianceSetUpContract adminData, String objTypeCode, Int32? objectId, Int32 tenantId)
        {
            if (objectId.HasValue)
            {
                Guid complianceCode = new Guid();
                switch (objTypeCode)
                {
                    case ComplainceObjectType.Package:
                        complianceCode = adminData.CompliancePackageList.FirstOrDefault(x => x.CompliancePackageID == objectId).Code;
                        var compliancePackage = ClientDBContext.CompliancePackages.FirstOrDefault(x => x.CopiedFromCode == complianceCode && x.IsDeleted == false);
                        if (compliancePackage.IsNotNull())
                        {
                            return compliancePackage.CompliancePackageID;
                        }
                        break;

                    case ComplainceObjectType.Category:
                        complianceCode = adminData.ComplianceCategoryList.FirstOrDefault(x => x.ComplianceCategoryID == objectId).Code;
                        var complianceCategory = ClientDBContext.ComplianceCategories.FirstOrDefault(x => x.CopiedFromCode == complianceCode && x.IsDeleted == false);
                        if (complianceCategory.IsNotNull())
                        {
                            return complianceCategory.ComplianceCategoryID;
                        }
                        break;

                    case ComplainceObjectType.Item:
                        complianceCode = adminData.ComplianceItemList.FirstOrDefault(x => x.ComplianceItemID == objectId).Code;
                        var complianceItem = ClientDBContext.ComplianceItems.FirstOrDefault(x => x.CopiedFromCode == complianceCode && x.IsDeleted == false);
                        if (complianceItem.IsNotNull())
                        {
                            return complianceItem.ComplianceItemID;
                        }
                        break;

                    case ComplainceObjectType.Attribute:
                        complianceCode = adminData.ComplianceAttributeList.FirstOrDefault(x => x.ComplianceAttributeID == objectId).Code;
                        var complianceAttribute = ClientDBContext.ComplianceAttributes.FirstOrDefault(x => x.CopiedFromCode == complianceCode && x.IsDeleted == false);
                        if (complianceAttribute.IsNotNull())
                        {
                            return complianceAttribute.ComplianceAttributeID;
                        }
                        break;
                }

                DALUtils.LoggerService.GetLogger().Info("DAL ComplianceSetUpRepository GetComplianceObjectID: Tenant ID = " + tenantId + ", Object ID = " + objectId + ", Object Type Code = " + objTypeCode
                      + " and Object Code = " + complianceCode + " is not found in client database while copying Assignment Hierarchy.");
            }
            return null;
        }

        /// <summary>
        /// Method assigns values to the entity of table RuleTemplateExpression
        /// </summary>
        /// <param name="currentLoggedInUser">currentLoggedInUser</param>
        /// <param name="newRuleTemplate">RuleTemplate Entity</param>
        /// <param name="ruleTempExp">RuleTemplateExpression Entity</param>
        /// <param name="newExpression">Expression Entity</param>
        /// <returns>RuleTemplateExpression Entity</returns>
        private RuleTemplateExpression AssignValueToRuleTempExpresn(Int32 currentLoggedInUser, RuleTemplate newRuleTemplate, RuleTemplateExpression ruleTempExp, Expression newExpression)
        {
            return new RuleTemplateExpression()
            {
                RLE_ExpressionID = 0,
                RLE_RuleID = 0,
                Expression = newExpression,
                RuleTemplate = newRuleTemplate,
                RLE_ExpressionOrder = ruleTempExp.RLE_ExpressionOrder,
                RLE_IsActive = ruleTempExp.RLE_IsActive,
                RLE_IsDeleted = false,
                RLE_CreatedByID = currentLoggedInUser,
                RLE_CreatedOn = DateTime.Now
            };
        }

        /// <summary>
        /// Method assigns values to the entity of table Expression
        /// </summary>
        /// <param name="currentLoggedInUser">CurrentLoggedInUser</param>
        /// <param name="adminRuleTempExp">RuleTemplateExpression Entity</param>
        /// <returns>Expression Entity</returns>
        private Expression AssignValueToExpression(Int32 currentLoggedInUser, RuleTemplateExpression adminRuleTempExp)
        {
            return new Expression()
            {
                EX_Name = adminRuleTempExp.Expression.EX_Name,
                EX_Description = adminRuleTempExp.Expression.EX_Description,
                EX_Expression = adminRuleTempExp.Expression.EX_Expression,
                EX_ResultType = adminRuleTempExp.Expression.EX_ResultType,
                EX_IsActive = adminRuleTempExp.Expression.EX_IsActive,
                EX_IsDeleted = false,
                EX_CreatedByID = currentLoggedInUser,
                EX_CreatedOn = DateTime.Now,
                EX_Code = adminRuleTempExp.Expression.EX_Code
            };
        }

        /// <summary>
        /// Method assigns values to the entity of table RuleMappingDetail
        /// </summary>
        /// <param name="currentLoggedInUser">currentLoggedInUser</param>
        /// <param name="ruleMappingDetail">RuleMappingDetail Entity</param>
        /// <param name="adminData">ComplianceSetUpContract Object</param>
        /// <returns>RuleMappingDetail Entity</returns>
        private RuleMappingDetail AssignValueToRuleMappingDetail(Int32 currentLoggedInUser, RuleMappingDetail ruleMappingDetail, ComplianceSetUpContract adminData)
        {
            String objectTypeCode = String.Empty;
            if (ruleMappingDetail.lkpObjectType.IsNotNull())
            {
                objectTypeCode = ruleMappingDetail.lkpObjectType.OT_Code;
            }

            RuleMappingDetail newRuleMappingDetail = new RuleMappingDetail();
            newRuleMappingDetail.RLMD_PlaceHolderName = ruleMappingDetail.RLMD_PlaceHolderName;
            newRuleMappingDetail.RLMD_ObjectID = GetComplianceObjectID(adminData, objectTypeCode, ruleMappingDetail.RLMD_ObjectID, ruleMappingDetail.RuleMapping.RLM_ID);
            newRuleMappingDetail.RLMD_ConstantValue = ruleMappingDetail.RLMD_ConstantValue;
            newRuleMappingDetail.RLMD_IsDeleted = false;
            newRuleMappingDetail.RLMD_CreatedByID = currentLoggedInUser;
            newRuleMappingDetail.RLMD_CreatedOn = DateTime.Now;
            newRuleMappingDetail.RLMD_UILabel = ruleMappingDetail.RLMD_UILabel;
            newRuleMappingDetail.RLMD_ObjectTypeID = GetObjctTypeId(ruleMappingDetail.RLMD_ObjectTypeID, adminData);
            newRuleMappingDetail.RLMD_RuleObjectMappingTypeID = GetObjctTypeId(ruleMappingDetail.RLMD_RuleObjectMappingTypeID, adminData);
            newRuleMappingDetail.RLMD_ConstantType = ruleMappingDetail.RLMD_ConstantType;
            return newRuleMappingDetail;
        }

        /// <summary>
        ///Method assigns values to the entity of table RuleMapping
        /// </summary>
        /// <param name="currentLoggedInUser">CurrentLoggedInUser</param>
        /// <param name="ruleSetId">RuleSet Id</param>
        /// <param name="ruleMapping">RuleMapping Entity</param>
        /// <param name="ruleTemplateId">RuleTemplate Id</param>
        /// <returns>RuleMapping Entity</returns>
        private RuleMapping AssignValueToRuleMapping(Int32 currentLoggedInUser, Int32 ruleSetId, RuleMapping ruleMapping, Int32 ruleTemplateId)
        {
            RuleMapping tempRuleMapping = new RuleMapping();
            {
                tempRuleMapping.RLM_RuleTemplateID = ruleTemplateId;
                tempRuleMapping.RLM_RuleSetID = ruleSetId;
                tempRuleMapping.RLM_Code = ruleMapping.RLM_Code.Equals(Guid.Empty) ? Guid.NewGuid() : ruleMapping.RLM_Code;
                tempRuleMapping.RLM_Name = ruleMapping.RLM_Name;
                tempRuleMapping.RLM_Version = ruleMapping.RLM_Version;
                tempRuleMapping.RLM_ActionBlock = ruleMapping.RLM_ActionBlock;
                tempRuleMapping.RLM_UIExpression = ruleMapping.RLM_UIExpression;
                tempRuleMapping.RLM_IsActive = ruleMapping.RLM_IsActive;
                tempRuleMapping.RLM_IsDeleted = false;
                tempRuleMapping.RLM_CreatedByID = currentLoggedInUser;
                tempRuleMapping.RLM_CreatedOn = DateTime.Now;
                tempRuleMapping.RLM_SuccessMessage = ruleMapping.RLM_SuccessMessage;
                tempRuleMapping.RLM_ErrorMessage = ruleMapping.RLM_ErrorMessage;
                tempRuleMapping.RLM_IsCurrent = ruleMapping.RLM_IsCurrent;
                //tempRuleMapping.RLM_FirstVersionID = 12;
            };
            return tempRuleMapping;
        }

        /// <summary>
        /// Method assigns values to the entity of table RuleTemplate
        /// </summary>
        /// <param name="currentLoggedInUser">currentLoggedInUser</param>
        /// <param name="ruleMapping">RuleMapping Entity</param>
        /// <returns>RuleTemplate Entity</returns>
        private RuleTemplate AssignValueToRuleTemplate(Int32 currentLoggedInUser, RuleMapping ruleMapping)
        {
            return new RuleTemplate()
            {
                RLT_Name = ruleMapping.RuleTemplate.RLT_Name,
                RLT_Description = ruleMapping.RuleTemplate.RLT_Description,
                RLT_SQLExpression = ruleMapping.RuleTemplate.RLT_SQLExpression,
                RLT_UIExpression = ruleMapping.RuleTemplate.RLT_UIExpression,
                RLT_UserExpression = ruleMapping.RuleTemplate.RLT_UserExpression,
                RLT_ResultType = ruleMapping.RuleTemplate.RLT_ResultType,
                RLT_IsActive = ruleMapping.RuleTemplate.RLT_IsActive,
                RLT_IsDeleted = false,
                RLT_CreatedByID = currentLoggedInUser,
                RLT_CreatedOn = DateTime.Now,
                RLT_CopiedFromCode = ruleMapping.RuleTemplate.RLT_Code,
                RLT_Code = Guid.NewGuid(),
                RLT_Type = ruleMapping.RuleTemplate.RLT_Type,
                RLT_ActionType = ruleMapping.RuleTemplate.RLT_ActionType,
                RLT_ObjectCount = ruleMapping.RuleTemplate.RLT_ObjectCount,
                RLT_Notes = ruleMapping.RuleTemplate.RLT_Notes
            };
        }

        /// <summary>
        /// Method assigns values to the entity of table RuleSetObject
        /// </summary>
        /// <param name="objectId">Object Id</param>
        /// <param name="currentLoggedInUser">currentLoggedInUser</param>
        /// <param name="objectTypeId">Object Type Id</param>
        /// <param name="clientRuleSetId">Client RuleSet Id</param>
        /// <returns>RuleSetObject Entity</returns>
        private RuleSetObject AssignValueToRulesetObject(RuleSetObject adminRuleSetObject, Int32 currentLoggedInUser, Int32 clientObjectID, Int32? clientRuleSetId)
        {
            RuleSetObject tempRuleSetObject = new RuleSetObject();

            if (clientRuleSetId.HasValue)
            {
                tempRuleSetObject.RLSO_RuleSetID = clientRuleSetId.Value;
            }
            tempRuleSetObject.RLSO_ObjectTypeID = adminRuleSetObject.RLSO_ObjectTypeID;
            tempRuleSetObject.RLSO_ObjectID = clientObjectID;
            tempRuleSetObject.RLSO_ExecutionOrder = 1;
            tempRuleSetObject.RLSO_IsActive = adminRuleSetObject.RLSO_IsActive;
            tempRuleSetObject.RLSO_IsDeleted = false;
            tempRuleSetObject.RLS_CreatedById = currentLoggedInUser;
            tempRuleSetObject.RLS_CreatedOn = DateTime.Now;
            tempRuleSetObject.RLSO_IsCreatedByAdmin = true;
            return tempRuleSetObject;
        }

        /// <summary>
        /// Method assigns values to the entity of table RuleSet
        /// </summary>
        /// <param name="tenantId">tenant Id</param>
        /// <param name="currentLoggedInUser">currentLoggedInUser</param>
        /// <param name="ruleSet">ruleSet Entity</param>
        /// <returns>RuleSet Entity</returns>
        private RuleSet AssignValueToRuleset(Int32 tenantId, Int32 currentLoggedInUser, RuleSet ruleSet, Int32 assignmentHierarchyID)
        {
            return new RuleSet()
            {
                RLS_Name = ruleSet.RLS_Name,
                RLS_Description = ruleSet.RLS_Description,
                RLS_StartDate = ruleSet.RLS_StartDate,
                RLS_IsActive = ruleSet.RLS_IsActive,
                RLS_IsDeleted = false,
                RLS_CreatedByID = currentLoggedInUser,
                RLS_CreatedOn = DateTime.Now,
                RLS_CopiedFromCode = ruleSet.RLS_Code,
                RLS_Code = Guid.NewGuid(),
                RLS_TenantID = tenantId,
                RLS_IsCreatedByAdmin = true,
                RLS_AssignmentHierarchyID = assignmentHierarchyID
            };
        }

        /// <summary>
        /// Gets the Object Type ID from Client Db using Object type ID from Master DB
        /// </summary>
        /// <param name="adminObjTypeId">Master Objct type ID</param>
        /// <param name="adminData">ComplianceSetUpContract Object</param>
        /// <returns>Client Object Type Id</returns>
        private Int32? GetObjctTypeId(Int32? adminObjTypeId, ComplianceSetUpContract adminData)
        {
            if (adminObjTypeId.HasValue)
            {
                String adminCode = adminData.LkpObjectTypeList.FirstOrDefault(obj => obj.OT_ID == adminObjTypeId && obj.OT_IsDeleted == false).OT_Code;
                return ClientDBContext.lkpObjectTypes.FirstOrDefault(obj => obj.OT_Code.Equals(adminCode) && obj.OT_IsDeleted == false).OT_ID;
            }
            return null;
        }

        /// <summary>
        /// Gets the Rule Object Mapping Type ID from Client Db using Rule Object Mapping Type ID from Master DB
        /// </summary>
        /// <param name="adminRuleObjMapTypeId">Master RuleObjMapTypeId</param>
        /// <param name="adminData">ComplianceSetUpContract Object</param>
        /// <returns>Client RuleObjMapTypeId</returns>
        private Int32? GetRuleObjMappingTypeID(Int32? adminRuleObjMapTypeId, ComplianceSetUpContract adminData)
        {
            if (adminRuleObjMapTypeId.HasValue)
            {
                String adminCode = adminData.LkpRuleObjectMappingTypeList.FirstOrDefault(obj => obj.RMT_ID == adminRuleObjMapTypeId && obj.RMT_IsDeleted == false).RMT_Code;
                return ClientDBContext.lkpRuleObjectMappingTypes.FirstOrDefault(obj => obj.RMT_Code.Equals(adminCode) && obj.RMT_IsDeleted == false).RMT_ID;
            }
            return null;
        }

        /// <summary>
        /// Gets the RuleSetTreeId from Client Db using RuleSetTreeId from Master DB
        /// </summary>
        /// <param name="adminRuleSetTreeId">Master RuleSetTreeId</param>
        /// <param name="adminData">ComplianceSetUpContract Object</param>
        /// <returns>Client RuleSetTreeId</returns>
        private Int32 GetRuleSetTreeID(Int32 adminRuleSetTreeId, ComplianceSetUpContract adminData)
        {
            String adminCode = adminData.RuleSetTreeList.FirstOrDefault(obj => obj.RST_ID == adminRuleSetTreeId && obj.RST_IsDeleted == false).RST_UICode;
            return ClientDBContext.RuleSetTrees.FirstOrDefault(obj => obj.RST_UICode.Equals(adminCode) && obj.RST_IsDeleted == false).RST_ID;
        }
        #endregion

        #region Assignment Properties

        ///// <summary>
        ///// Gets the master list of the User types who can edit the compliance item being created/updated
        ///// </summary>
        ///// <returns>Master list of the users</returns>
        public List<lkpEditableBy> GetComplianceEditableBy()
        {
            return _ClientDBContext.lkpEditableBies.Where(editableBy => editableBy.IsDeleted == false).ToList();
        }

        ///// <summary>
        ///// Gets the master list of the User types who can review the compliance item being created/updated
        ///// </summary>
        ///// <returns>Master list of the users</returns>
        public List<lkpReviewerType> GetComplianceReviwedBy()
        {
            return _ClientDBContext.lkpReviewerTypes.Where(reviwedBy => reviwedBy.IsDeleted == false).ToList();
        }

        public List<Tenant> GetThirdPartyReviewers(Int32 packageID)
        {
            var compliancePackages = _ClientDBContext.CompliancePackages.FirstOrDefault(x => x.CompliancePackageID == packageID && x.IsDeleted == false);

            if (compliancePackages.IsNotNull())
            {
                return _ClientDBContext.ClientRelations.Where(x => x.TenantID == compliancePackages.TenantID &&
                    x.IsDeleted == false && x.Tenant.IsDeleted == false).Select(x => x.Tenant).ToList();
            }
            return null;
        }

        /// <summary>
        /// Gest the Assignment Properties.
        /// </summary>
        /// <param name="currentDataID">currentDataID</param>
        /// <param name="parentCategoryDataID">parentCategoryDataID</param>
        /// <param name="parentPackageDataID">parentPackageDataID</param>
        /// <param name="currentObjectTypeCode">currentObjectTypeCode</param>
        /// <returns>AssignmentProperty</returns>
        public AssignmentProperty GetAssignmentPropertyDetails(Int32 currentDataID, Int32 parentCategoryDataID, Int32 parentPackageDataID, Int32 parentItemDataID, String currentObjectTypeCode, Int32 currentObjectTypeID, List<lkpObjectType> lkpObjectType)
        {
            //Gets AssignmentProperty for a package.
            if (currentObjectTypeCode == LCObjectType.CompliancePackage.GetStringValue())
            {
                var packageHierarchy = GetExistingPackageHierarchy(currentDataID, currentObjectTypeID);
                return GetsAssignmentPropertiesByHierarchy(packageHierarchy);
            }
            //Gets AssignmentProperty for a category.
            else if (currentObjectTypeCode == LCObjectType.ComplianceCategory.GetStringValue())
            {
                Int32 packageObjectTypeID = FetchObjectTypeIDByCode(lkpObjectType, LCObjectType.CompliancePackage.GetStringValue());
                var packageHierarchy = GetExistingPackageHierarchy(parentPackageDataID, packageObjectTypeID);

                //Gets AssignmentProperty for a category if package hierarchy is found.
                if (packageHierarchy.IsNotNull())
                {
                    var categoryHierarchy = GetExistingCategoryHierarchy(currentDataID, packageHierarchy.AssignmentHierarchyID, currentObjectTypeID);
                    return GetsAssignmentPropertiesByHierarchy(categoryHierarchy);
                }
            }
            //Gets AssignmentProperty for a item.
            else if (currentObjectTypeCode == LCObjectType.ComplianceItem.GetStringValue())
            {
                Int32 packageObjectTypeID = FetchObjectTypeIDByCode(lkpObjectType, LCObjectType.CompliancePackage.GetStringValue());
                var packageHierarchy = GetExistingPackageHierarchy(parentPackageDataID, packageObjectTypeID);

                //Gets AssignmentProperty for a item if package hierarchy is found.
                if (packageHierarchy.IsNotNull())
                {
                    Int32 categoryObjectTypeID = FetchObjectTypeIDByCode(lkpObjectType, LCObjectType.ComplianceCategory.GetStringValue());
                    var categoryHierarchy = GetExistingCategoryHierarchy(parentCategoryDataID, packageHierarchy.AssignmentHierarchyID, categoryObjectTypeID);

                    //Gets AssignmentProperty for a item if category hierarchy is found.
                    if (categoryHierarchy.IsNotNull())
                    {
                        var itemHierarchy = GetExistingItemHierarchy(currentDataID, categoryHierarchy.AssignmentHierarchyID, currentObjectTypeID);
                        return GetsAssignmentPropertiesByHierarchy(itemHierarchy);
                    }
                }
            }
            //Changes for Editable By for ATR
            else if (currentObjectTypeCode == LCObjectType.ComplianceATR.GetStringValue())
            {
                Int32 packageObjectTypeID = FetchObjectTypeIDByCode(lkpObjectType, LCObjectType.CompliancePackage.GetStringValue());
                var packageHierarchy = GetExistingPackageHierarchy(parentPackageDataID, packageObjectTypeID);

                //Gets AssignmentProperty for a item if package hierarchy is found.
                if (packageHierarchy.IsNotNull())
                {
                    Int32 categoryObjectTypeID = FetchObjectTypeIDByCode(lkpObjectType, LCObjectType.ComplianceCategory.GetStringValue());
                    var categoryHierarchy = GetExistingCategoryHierarchy(parentCategoryDataID, packageHierarchy.AssignmentHierarchyID, categoryObjectTypeID);

                    //Gets AssignmentProperty for a item if category hierarchy is found.
                    if (categoryHierarchy.IsNotNull())
                    {
                        Int32 itemObjectTypeID = FetchObjectTypeIDByCode(lkpObjectType, LCObjectType.ComplianceItem.GetStringValue());
                        var itemHierarchy = GetExistingItemHierarchy(parentItemDataID, categoryHierarchy.AssignmentHierarchyID, itemObjectTypeID);
                        //return GetsAssignmentPropertiesByHierarchy(itemHierarchy);

                        //Gets AssignmentProperty for an attribute if item hierarchy is found.
                        if (itemHierarchy.IsNotNull())
                        {
                            var attributeHierarchy = GetExistingAttributeHierarchy(currentDataID, itemHierarchy.AssignmentHierarchyID, currentObjectTypeID);
                            return GetsAssignmentPropertiesByHierarchy(attributeHierarchy);
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Fetch the Assignment Options starting from leaf node to parent. If no assignment options find for any level then returns default values.
        /// </summary>
        /// <param name="parentPackageDataID">package id</param>
        /// <param name="parentCategoryDataID">category id</param>
        /// <param name="itemDataID">item id</param>
        /// <returns>AssignmentProperty</returns>
        public AssignmentProperty FetchAssignmentOptions(Int32 parentPackageDataID, List<lkpObjectType> lkpObjectType, Int32 parentCategoryDataID = 0, Int32 itemDataID = 0)
        {
            AssignmentProperty assignmentOptions = new AssignmentProperty();
            AssignmentProperty packageAssignmentOptions = new AssignmentProperty();

            Int32 packageObjectTypeID = FetchObjectTypeIDByCode(lkpObjectType, LCObjectType.CompliancePackage.GetStringValue());
            Int32 categoryObjectTypeID = FetchObjectTypeIDByCode(lkpObjectType, LCObjectType.ComplianceCategory.GetStringValue());
            Int32 itemObjectTypeID = FetchObjectTypeIDByCode(lkpObjectType, LCObjectType.ComplianceItem.GetStringValue());

            var packageHierarchy = GetExistingPackageHierarchy(parentPackageDataID, packageObjectTypeID);

            //Checks if package hierarchy exists.
            if (packageHierarchy.IsNotNull())
            {
                var categoryHierarchy = GetExistingCategoryHierarchy(parentCategoryDataID, packageHierarchy.AssignmentHierarchyID, categoryObjectTypeID);

                //Checks if category hierarchy exists.
                if (categoryHierarchy.IsNotNull())
                {
                    var itemHierarchy = GetExistingItemHierarchy(itemDataID, categoryHierarchy.AssignmentHierarchyID, itemObjectTypeID);

                    //Checks if item hierarchy exists and gets Assignment Options for Item.
                    if (itemHierarchy.IsNotNull())
                    {
                        assignmentOptions = GetAssignmentOptionsByHierarchy(itemHierarchy);
                    }
                    //Gets Assignment Options for category.
                    if (assignmentOptions.IsNull() || assignmentOptions.AssignmentPropertyID == 0)
                    {
                        assignmentOptions = GetAssignmentOptionsByHierarchy(categoryHierarchy);
                    }
                }

                packageAssignmentOptions = GetAssignmentOptionsByHierarchy(packageHierarchy);
                //Gets Assignment Options for Package.
                if (assignmentOptions.IsNull() || assignmentOptions.AssignmentPropertyID == 0)
                {
                    assignmentOptions = packageAssignmentOptions;
                }
            }
            //Gets default Assignment Options.
            if (assignmentOptions.IsNull() || assignmentOptions.AssignmentPropertyID == 0)
            {
                assignmentOptions = GetDefaultAssignmentOptions();
            }
            else
            {
                assignmentOptions = ModifyAssignmentOptions(assignmentOptions, packageAssignmentOptions);
            }
            return assignmentOptions;
        }

        private AssignmentProperty ModifyAssignmentOptions(AssignmentProperty assignmentOptionsInDB, AssignmentProperty packageAssignmentOptions)
        {
            AssignmentProperty assignmentProperty = new AssignmentProperty();
            assignmentProperty.EffectiveDate = assignmentOptionsInDB.EffectiveDate;
            assignmentProperty.ApprovalRequired = assignmentOptionsInDB.ApprovalRequired;
            assignmentProperty.IsActive = assignmentOptionsInDB.IsActive;

            //Sets ReviewerTenantID from package.
            if (packageAssignmentOptions.IsNotNull() && packageAssignmentOptions.ReviewerTenantID.IsNotNull())
            {
                assignmentProperty.ReviewerTenantID = packageAssignmentOptions.ReviewerTenantID;
                assignmentProperty.Tenant = new Tenant
                {
                    TenantID = packageAssignmentOptions.Tenant.TenantID,
                    TenantTypeID = packageAssignmentOptions.Tenant.TenantTypeID,
                    TenantName = packageAssignmentOptions.Tenant.TenantName
                };
            }

            List<AssignmentPropertiesReviewer> assignmentPropertiesReviewers = assignmentOptionsInDB.AssignmentPropertiesReviewers.Where(x => x.IsDeleted == false).ToList();

            foreach (var reviewer in assignmentPropertiesReviewers)
            {
                assignmentProperty.AssignmentPropertiesReviewers.Add(new AssignmentPropertiesReviewer()
                {
                    AssignmentPropertyReviewerID = reviewer.AssignmentPropertyReviewerID,
                    ReviewerTypeID = reviewer.ReviewerTypeID,
                    IsDeleted = false,
                    lkpReviewerType = new lkpReviewerType
                    {
                        ReviewerTypeID = reviewer.lkpReviewerType.ReviewerTypeID,
                        Code = reviewer.lkpReviewerType.Code,
                        Name = reviewer.lkpReviewerType.Name
                    }
                });
            }

            List<AssignmentPropertiesEditableBy> assignmentPropertiesEditableBy = assignmentOptionsInDB.AssignmentPropertiesEditableBies.Where(x => x.IsDeleted == false).ToList();

            foreach (var editor in assignmentPropertiesEditableBy)
            {
                assignmentProperty.AssignmentPropertiesEditableBies.Add(new AssignmentPropertiesEditableBy()
                {
                    AssignmentPropertyEditableByID = editor.AssignmentPropertyEditableByID,
                    EditableByID = editor.EditableByID,
                    IsDeleted = false,
                    lkpEditableBy = new lkpEditableBy
                    {
                        ComplianceItemEditableByID = editor.lkpEditableBy.ComplianceItemEditableByID,
                        Code = editor.lkpEditableBy.Code,
                        Name = editor.lkpEditableBy.Name
                    }
                });
            }

            //check if reviewer type is admin and reviwer tenant is selected then add thirdpartyuser id to assignment property
            if (assignmentPropertiesReviewers.Count == 1)
            {
                if (assignmentPropertiesReviewers.FirstOrDefault().lkpReviewerType.Code == LkpReviewerType.Admin && assignmentProperty.ReviewerTenantID != null)
                {
                    assignmentProperty.TPReviewerUserID = packageAssignmentOptions.TPReviewerUserID;
                }

            }

            return assignmentProperty;
        }

        public List<ListItemAssignmentProperties> GetAssignmentPropertiesByCategoryId(Int32 packageId, Int32 categoryId)
        {
            return _ClientDBContext.GetAssignmentPropertiesByCategory(packageId, categoryId).ToList();
        }

        /// <summary>
        ///  Gets the list of Editable Bies for all the attributes in all the items in a category
        /// </summary>
        /// <param name="parentPackageDataID"></param>
        /// <param name="parentCategoryDataID"></param>
        /// <param name="isAdmin"></param>
        /// <param name="itemDataID"></param>
        /// <returns></returns>
        public List<ListItemEditableBies> GetEditableBiesByCategoryId(Int32 packageId, Int32 categoryId)
        {
            return _ClientDBContext.GetEditableBiesByCategory(packageId, categoryId).ToList();
        }

        /// <summary>
        /// To fetch Assignment Options
        /// </summary>
        /// <param name="parentPackageDataID"></param>
        /// <param name="parentCategoryDataID"></param>
        /// <param name="itemDataID"></param>
        /// <returns></returns>
        public List<AssignmentHierarchyEditableByContract> GetEditableBies(Int32 parentPackageDataID, Int32 parentCategoryDataID, Boolean isApplicant, List<lkpObjectType> lkpObjectType, List<lkpEditableBy> lstEditableBy, Int32 itemDataID = 0)
        {
            //Changes for Editable By for ATR
            //AssignmentHierarchyEditableByContract
            AssignmentProperty assignmentOptions = new AssignmentProperty();
            AssignmentProperty packageAssignmentOptions = new AssignmentProperty();
            List<AssignmentHierarchyEditableByContract> lstAHEditableByContract = new List<AssignmentHierarchyEditableByContract>();
            List<AssignmentHierarchyEditableByContract> _finalAHEditableByContract = new List<AssignmentHierarchyEditableByContract>();

            Int32 packageObjectTypeID = FetchObjectTypeIDByCode(lkpObjectType, LCObjectType.CompliancePackage.GetStringValue());
            Int32 categoryObjectTypeID = FetchObjectTypeIDByCode(lkpObjectType, LCObjectType.ComplianceCategory.GetStringValue());
            Int32 itemObjectTypeID = FetchObjectTypeIDByCode(lkpObjectType, LCObjectType.ComplianceItem.GetStringValue());
            Int32 attributeObjectTypeID = FetchObjectTypeIDByCode(lkpObjectType, LCObjectType.ComplianceATR.GetStringValue());

            //var packageHierarchy = GetExistingPackageHierarchy(parentPackageDataID, packageObjectTypeID);
            AssignmentHierarchy packageHierarchy = null;

            if (parentPackageDataID > 0 && parentCategoryDataID > 0)
            {

                packageHierarchy = GetExistingPackageHierarchy(parentPackageDataID, packageObjectTypeID);

                //Checks if package hierarchy exists.
                if (packageHierarchy.IsNotNull())
                {
                    //Gets Assignment Options for Package.
                    assignmentOptions = GetAssignmentOptionsByHierarchy(packageHierarchy);

                    //Gets default Assignment Options.
                    if (assignmentOptions.IsNull() || assignmentOptions.AssignmentPropertyID == 0 || assignmentOptions.AssignmentPropertyID.IsNullOrEmpty())
                    {
                        //    assignmentOptions = GetDefaultAssignmentOptions();
                        lstAHEditableByContract.Add(new AssignmentHierarchyEditableByContract
                        {
                            ParentObjectId = null,
                            ParentObjectTypeCode = null,
                            ObjectId = parentPackageDataID,
                            ObjectTypeCode = LCObjectType.CompliancePackage.GetStringValue(),
                            AssignmentHierarchyId = packageHierarchy.AssignmentHierarchyID,
                            AssignmentPropertyId = null,
                            //lstEditableBy = GetDefaultEditableBy(isApplicant)
                        });
                    }
                    else
                    {
                        lstAHEditableByContract.Add(AddAssignmentOptionsContractList(assignmentOptions));
                    }
                    //Gets Category Hierarchy
                    var categoryHierarchy = GetExistingCategoryHierarchy(parentCategoryDataID, packageHierarchy.AssignmentHierarchyID, categoryObjectTypeID);
                    //Checks if category hierarchy exists.
                    if (categoryHierarchy.IsNotNull())
                    {
                        //Gets Assignment Options for category.
                        assignmentOptions = null;
                        assignmentOptions = GetAssignmentOptionsByHierarchy(categoryHierarchy);

                        //Gets default Assignment Options.
                        if (assignmentOptions.IsNull() || assignmentOptions.AssignmentPropertyID == 0 || assignmentOptions.AssignmentPropertyID.IsNullOrEmpty())
                        {
                            lstAHEditableByContract.Add(new AssignmentHierarchyEditableByContract
                            {
                                ParentObjectId = parentPackageDataID,
                                ParentObjectTypeCode = LCObjectType.CompliancePackage.GetStringValue(),
                                ObjectId = parentCategoryDataID,
                                ObjectTypeCode = LCObjectType.ComplianceCategory.GetStringValue(),
                                AssignmentHierarchyId = categoryHierarchy.AssignmentHierarchyID,
                                AssignmentPropertyId = null,
                                // lstEditableBy = GetDefaultEditableBy(isApplicant)
                            });
                        }
                        else
                        {
                            lstAHEditableByContract.Add(AddAssignmentOptionsContractList(assignmentOptions));
                        }

                        if (itemDataID == 0)
                        {
                            //Gets Assignment Options for all Items in a category.
                            List<Int32> AHCatIds = new List<Int32>();
                            AHCatIds.Add(categoryHierarchy.AssignmentHierarchyID);

                            var itemsHierarchyInCategory = GetHierarchyChild(AHCatIds);

                            if (itemsHierarchyInCategory.IsNotNull())
                            {
                                //Items
                                var assignmentOptionsItemList = GetAssignmentOptionsByHierarchyIds(itemsHierarchyInCategory.Select(x => x.AssignmentHierarchyID).ToList());

                                foreach (var ahItem in itemsHierarchyInCategory)
                                {
                                    AssignmentProperty _assignmentProperty = assignmentOptionsItemList.Where(x => x.AssignmentHierarchy.AssignmentHierarchyID == ahItem.AssignmentHierarchyID
                                                && x.AssignmentHierarchy.ObjectTypeID == itemObjectTypeID).FirstOrDefault();

                                    if (!_assignmentProperty.IsNullOrEmpty())
                                    {
                                        lstAHEditableByContract.Add(AddAssignmentOptionsContractList(_assignmentProperty));
                                    }
                                    else
                                    {
                                        lstAHEditableByContract.Add(new AssignmentHierarchyEditableByContract
                                        {
                                            ParentObjectId = parentCategoryDataID,
                                            ParentObjectTypeCode = LCObjectType.ComplianceCategory.GetStringValue(),
                                            ObjectId = ahItem.ObjectID,
                                            ObjectTypeCode = LCObjectType.ComplianceItem.GetStringValue(),
                                            AssignmentHierarchyId = ahItem.AssignmentHierarchyID,
                                            AssignmentPropertyId = null,
                                            // lstEditableBy = GetDefaultEditableBy(isApplicant)
                                        });
                                    }
                                }

                                //Attribues
                                var attributeHierarchyInItem = GetHierarchyChild(itemsHierarchyInCategory.Select(x => x.AssignmentHierarchyID).ToList());
                                if (attributeHierarchyInItem.IsNotNull())
                                {
                                    var assignmentOptionsAttributesList = GetAssignmentOptionsByHierarchyIds(attributeHierarchyInItem.Select(x => x.AssignmentHierarchyID).ToList());

                                    foreach (var ahAttribute in attributeHierarchyInItem)
                                    {
                                        AssignmentProperty _assignmentProperty = assignmentOptionsAttributesList.Where(x => x.AssignmentHierarchy.AssignmentHierarchyID == ahAttribute.AssignmentHierarchyID
                                                    && x.AssignmentHierarchy.ObjectTypeID == attributeObjectTypeID).FirstOrDefault();

                                        if (!_assignmentProperty.IsNullOrEmpty())
                                        {
                                            lstAHEditableByContract.Add(AddAssignmentOptionsContractList(_assignmentProperty));
                                        }
                                        else
                                        {
                                            lstAHEditableByContract.Add(new AssignmentHierarchyEditableByContract
                                            {
                                                ParentObjectId = ahAttribute.AssignmentHierarchy2.ObjectID,
                                                ParentObjectTypeCode = LCObjectType.ComplianceItem.GetStringValue(),
                                                ObjectId = ahAttribute.ObjectID,
                                                ObjectTypeCode = LCObjectType.ComplianceATR.GetStringValue(),
                                                AssignmentHierarchyId = ahAttribute.AssignmentHierarchyID,
                                                AssignmentPropertyId = null,
                                                //   lstEditableBy = GetDefaultEditableBy(isApplicant)
                                            });
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Gets Assignment Options for item.
                            var itemHierarchy = GetExistingItemHierarchy(itemDataID, categoryHierarchy.AssignmentHierarchyID, itemObjectTypeID);
                            //Checks if item hierarchy exists
                            if (itemHierarchy.IsNotNull())
                            {
                                assignmentOptions = null;
                                assignmentOptions = GetAssignmentOptionsByHierarchy(itemHierarchy);

                                //Gets default Assignment Options.
                                if (assignmentOptions.IsNull() || assignmentOptions.AssignmentPropertyID == 0 || assignmentOptions.AssignmentPropertyID.IsNullOrEmpty())
                                {
                                    lstAHEditableByContract.Add(new AssignmentHierarchyEditableByContract
                                    {
                                        ParentObjectId = parentCategoryDataID,
                                        ParentObjectTypeCode = LCObjectType.ComplianceCategory.GetStringValue(),
                                        ObjectId = itemHierarchy.ObjectID,
                                        ObjectTypeCode = LCObjectType.ComplianceItem.GetStringValue(),
                                        AssignmentHierarchyId = itemHierarchy.AssignmentHierarchyID,
                                        AssignmentPropertyId = null,
                                        //lstEditableBy = GetDefaultEditableBy(isApplicant)
                                    });
                                }
                                else
                                {
                                    lstAHEditableByContract.Add(AddAssignmentOptionsContractList(assignmentOptions));
                                }

                                //Attribues
                                List<Int32> AHItemIds = new List<Int32>();
                                AHItemIds.Add(itemHierarchy.AssignmentHierarchyID);

                                var attributeHierarchyForItems = GetHierarchyChild(AHItemIds);

                                var assignmentOptionsAttributesList = GetAssignmentOptionsByHierarchyIds(attributeHierarchyForItems.Select(x => x.AssignmentHierarchyID).ToList());

                                foreach (var ahAttribute in attributeHierarchyForItems)
                                {
                                    AssignmentProperty _assignmentProperty = assignmentOptionsAttributesList
                                                .Where(x => x.AssignmentHierarchy.AssignmentHierarchyID == ahAttribute.AssignmentHierarchyID
                                                && x.AssignmentHierarchy.ObjectTypeID == attributeObjectTypeID).FirstOrDefault();

                                    if (!_assignmentProperty.IsNullOrEmpty())
                                    {
                                        lstAHEditableByContract.Add(AddAssignmentOptionsContractList(_assignmentProperty));
                                    }
                                    else
                                    {
                                        lstAHEditableByContract.Add(new AssignmentHierarchyEditableByContract
                                        {
                                            ParentObjectId = ahAttribute.AssignmentHierarchy2.ObjectID,
                                            ParentObjectTypeCode = LCObjectType.ComplianceItem.GetStringValue(),
                                            ObjectId = ahAttribute.ObjectID,
                                            ObjectTypeCode = LCObjectType.ComplianceATR.GetStringValue(),
                                            AssignmentHierarchyId = ahAttribute.AssignmentHierarchyID,
                                            AssignmentPropertyId = null,
                                            //  lstEditableBy = GetDefaultEditableBy(isApplicant)
                                        });
                                    }
                                }
                            }
                        }

                        List<AssignmentPropertiesEditableBy> assignmentPropertiesEditableBy = null;

                        // Set the Editable bies, as the above code sets them to NULL for all
                        foreach (var record in lstAHEditableByContract)
                        {
                            assignmentPropertiesEditableBy = new List<AssignmentPropertiesEditableBy>();
                            // If AssignmentProperties does not Exist for the record being parsed, then get its parent
                            if (record.AssignmentPropertyId.IsNullOrEmpty() || record.AssignmentPropertyId == 0)
                            {
                                AssignmentHierarchyEditableByContract _parent = GetParent(record, lstAHEditableByContract);

                                if (!_parent.IsNullOrEmpty())
                                {
                                    // Loop till the Assignment property is not null & editable by is not null
                                    // while (!_parent.IsNullOrEmpty() && (_parent.AssignmentPropertyId.IsNullOrEmpty() || _parent.lstEditableBy.IsNullOrEmpty()))
                                    while (!_parent.IsNullOrEmpty() && _parent.AssignmentPropertyId.IsNullOrEmpty())
                                    {
                                        // GetParentAssignmentProperty(_parent, lstAHEditableByContract);
                                        _parent = GetParent(_parent, lstAHEditableByContract);
                                    }

                                    // Get the list of editable by using Propertyid
                                    if (!_parent.IsNullOrEmpty() && !_parent.AssignmentPropertyId.IsNullOrEmpty())
                                    {
                                        Int32? _propertyId = _parent.AssignmentPropertyId;
                                        assignmentPropertiesEditableBy = GetAssignmentPropertiesEditableBy(_propertyId);
                                    }
                                    else if (_parent.IsNullOrEmpty() || (!_parent.IsNullOrEmpty() && _parent.AssignmentPropertyId.IsNullOrEmpty()))
                                    {
                                        // 2 Conditions above :
                                        // 1. Parent is not NULL and its PropertyId is null. So set default. 
                                        // 2. Parent is NULL Can occur when Package level is done but no hierarchy is defined
                                        assignmentPropertiesEditableBy = GetDefaultEditableBy(isApplicant, lstEditableBy);
                                    }
                                }
                                else
                                {
                                    // Set Default for the PACKAGE Level, as PARENT is NULL
                                    assignmentPropertiesEditableBy = GetDefaultEditableBy(isApplicant, lstEditableBy);
                                }
                            }
                            else
                            {
                                // Get the list of editable by using Propertyid
                                assignmentPropertiesEditableBy = GetAssignmentPropertiesEditableBy(record.AssignmentPropertyId);
                            }

                            _finalAHEditableByContract.Add(new AssignmentHierarchyEditableByContract
                            {
                                lstEditableBy = assignmentPropertiesEditableBy.IsNullOrEmpty() ? GetDefaultEditableBy(isApplicant, lstEditableBy) : assignmentPropertiesEditableBy,

                                ObjectId = record.ObjectId,
                                ObjectTypeCode = record.ObjectTypeCode,
                                AssignmentPropertyId = record.AssignmentPropertyId,
                                AssignmentHierarchyId = record.AssignmentHierarchyId,
                                ParentObjectId = record.ParentObjectId,
                                ParentObjectTypeCode = record.ParentObjectTypeCode
                            });
                        }
                    }
                }
            }
            return _finalAHEditableByContract;
        }

        private List<AssignmentPropertiesEditableBy> GetDefaultEditableBy(Boolean isApplicant, List<lkpEditableBy> lstEditableBy)
        {
            List<AssignmentPropertiesEditableBy> _lstEditableBy = new List<AssignmentPropertiesEditableBy>();

            List<lkpEditableBy> lstEditableBies = new List<lkpEditableBy>();

            if (isApplicant) // If Admin, then NO default rights available
                lstEditableBies = _ClientDBContext.lkpEditableBies.Where(x => x.IsDeleted == false).ToList();
            else
                lstEditableBies = _ClientDBContext.lkpEditableBies.Where(x => x.IsDeleted == false
                                                  && x.Code != LkpEditableBy.InstitutionAdmin && x.Code != LkpEditableBy.Admin).ToList();

            foreach (var editor in lstEditableBies)
            {
                _lstEditableBy.Add(new AssignmentPropertiesEditableBy()
                {
                    EditableByID = editor.ComplianceItemEditableByID,
                    IsDeleted = false,
                    lkpEditableBy = new lkpEditableBy
                    {
                        Code = editor.Code,
                        ComplianceItemEditableByID = editor.ComplianceItemEditableByID,
                        Name = editor.Name
                    }
                });
            }
            return _lstEditableBy;
        }

        /// <summary>
        /// To get Assignment Properties Editable By
        /// </summary>
        /// <param name="assignmentPropertyID"></param>
        /// <returns></returns>
        private List<AssignmentPropertiesEditableBy> GetAssignmentPropertiesEditableBy(Int32? assignmentPropertyID)
        {
            return _ClientDBContext.AssignmentPropertiesEditableBies.Where(obj => obj.AssignmentPropertyID == assignmentPropertyID
                   && obj.IsDeleted == false && obj.AssignmentProperty.IsActive == true).ToList();
        }

        /// <summary>
        /// To get Parent Assignment Property
        /// </summary>
        /// <param name="currentRecord"></param>
        /// <param name="lstAHEditableByContract"></param>
        /// <returns></returns>
        private AssignmentHierarchyEditableByContract GetParent(AssignmentHierarchyEditableByContract currentRecord, List<AssignmentHierarchyEditableByContract> lstAHEditableByContract)
        {
            Int32? _parentId = currentRecord.ParentObjectId;
            AssignmentHierarchyEditableByContract _parent = lstAHEditableByContract.Where(x => x.ObjectId == _parentId && x.ObjectTypeCode == currentRecord.ParentObjectTypeCode).FirstOrDefault();
            return _parent;
        }

        /// <summary>
        /// To add Assignment Options Contract List
        /// </summary>
        /// <param name="assignmentOptions"></param>
        /// <returns></returns>
        private AssignmentHierarchyEditableByContract AddAssignmentOptionsContractList(AssignmentProperty assignmentOptions)
        {
            AssignmentHierarchyEditableByContract AHEditableByContract = new AssignmentHierarchyEditableByContract();

            AHEditableByContract.ObjectId = assignmentOptions.AssignmentHierarchy.ObjectID.HasValue ? assignmentOptions.AssignmentHierarchy.ObjectID.Value : 0;
            AHEditableByContract.ObjectTypeCode = assignmentOptions.AssignmentHierarchy.lkpObjectType.OT_Code;
            AHEditableByContract.AssignmentPropertyId = assignmentOptions.AssignmentPropertyID;
            AHEditableByContract.AssignmentHierarchyId = assignmentOptions.AssignmentHierarchyID;

            // No Parent for the Package Type
            if (assignmentOptions.AssignmentHierarchy.lkpObjectType.OT_Code == LCObjectType.CompliancePackage.GetStringValue())
            {
                AHEditableByContract.ParentObjectId = null;
                AHEditableByContract.ParentObjectTypeCode = null;
            }
            else
            {
                AHEditableByContract.ParentObjectId = assignmentOptions.AssignmentHierarchy.AssignmentHierarchy2.ObjectID.HasValue ? assignmentOptions.AssignmentHierarchy.AssignmentHierarchy2.ObjectID.Value : 0;
                AHEditableByContract.ParentObjectTypeCode = assignmentOptions.AssignmentHierarchy.AssignmentHierarchy2.lkpObjectType.OT_Code;
            }
            return AHEditableByContract;
        }

        /// <summary>
        /// To get Hierarchy Child
        /// </summary>
        /// <param name="assignmentHierarchyIDs"></param>
        /// <returns></returns>
        private List<AssignmentHierarchy> GetHierarchyChild(List<Int32> assignmentHierarchyIDs)
        {
            return _ClientDBContext.AssignmentHierarchies.Where(obj => assignmentHierarchyIDs.Contains(obj.ParentID.Value)
                   && obj.IsDeleted == false).ToList();
        }

        /// <summary>
        /// Gets Assignment Options By Assignment Hierarchy Ids
        /// </summary>
        /// <param name="assignmentHierarchyIds"></param>
        /// <returns></returns>
        private List<AssignmentProperty> GetAssignmentOptionsByHierarchyIds(List<Int32> assignmentHierarchyIds)
        {
            return _ClientDBContext.AssignmentProperties.Where(x => x.IsDeleted == false && x.IsActive == true &&
                assignmentHierarchyIds.Contains(x.AssignmentHierarchyID)).ToList();
        }

        /// <summary>
        /// Insert or update the assignment property for currently selected node.
        /// </summary>
        /// <param name="assignmentProperty">object Of assignment property</param>
        /// <param name="currentDataId">Currently selected node id</param>
        /// <param name="parentPackageId">Parent Package Id</param>
        /// <param name="parentCategoryId">Parent Category Id</param>
        /// <param name="parentItemId">Parent Item Id</param>
        /// <param name="currentRuleSetTreeTypeCode"> Ui Code for currently selected node</param>
        /// <param name="loggedInUserId"></param>
        public void UpdateAssignmentProperties(AssignmentProperty assignmentProperty, Int32 currentDataId, Int32 parentPackageId, Int32 parentCategoryId, Int32 parentItemId, String currentRuleSetTreeTypeCode, Int32 loggedInUserId)
        {
            AssignmentProperty assignmentPropertyIndb = _ClientDBContext.AssignmentProperties.Where(obj => obj.AssignmentPropertyID == assignmentProperty.AssignmentPropertyID).FirstOrDefault();

            //Update
            if (assignmentProperty.AssignmentPropertyID > 0)
            {
                //Editable By will not be shown for Packages
                if (currentRuleSetTreeTypeCode != RuleSetTreeType.Packages.GetStringValue())
                {
                    //Check whether selected editable by record exist in db if not exist then insert it
                    foreach (var editableBy in assignmentProperty.AssignmentPropertiesEditableBies)
                    {
                        if (!(assignmentPropertyIndb.AssignmentPropertiesEditableBies.Any(obj => obj.EditableByID == editableBy.EditableByID && obj.IsDeleted == false)))
                        {
                            AssignmentPropertiesEditableBy newEditableBy = new AssignmentPropertiesEditableBy()
                            {
                                EditableByID = editableBy.EditableByID,
                                IsDeleted = false,
                                CreatedBy = loggedInUserId,
                                CreatedOn = DateTime.Now
                            };
                            assignmentPropertyIndb.AssignmentPropertiesEditableBies.Add(newEditableBy);
                        }
                    }
                    //Check whether all editable by record in db are currently selected if not  then delete record from db
                    foreach (var editableBy in assignmentPropertyIndb.AssignmentPropertiesEditableBies.Where(obj => obj.IsDeleted == false))
                    {
                        //Not to update record for Attribute and LkpEditableBy code is Applicant
                        //UAT-3806 Update record for Attribute and lkpEditableBy code is Applicant
                        if (!(assignmentProperty.AssignmentPropertiesEditableBies.Any(obj => obj.EditableByID == editableBy.EditableByID)))
                        {
                            editableBy.IsDeleted = true;
                            editableBy.ModifiedBy = loggedInUserId;
                            editableBy.ModifiedOn = DateTime.Now;
                        }
                    }
                    //Assigning whether to allow exception submission in the category
                    if (currentRuleSetTreeTypeCode == RuleSetTreeType.Categories.GetStringValue())
                    {
                        assignmentPropertyIndb.IsExceptionNotAllowed = assignmentProperty.IsExceptionNotAllowed;
                    }

                    //UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
                    if (currentRuleSetTreeTypeCode == RuleSetTreeType.Items.GetStringValue())
                    {
                        assignmentPropertyIndb.ItemDataEntry = assignmentProperty.ItemDataEntry;
                        assignmentPropertyIndb.IsEnableUpdateAllTime = assignmentProperty.IsEnableUpdateAllTime; //UAT-4926
                    }
                }
                if (currentRuleSetTreeTypeCode != RuleSetTreeNodeType.Attribute)
                {
                    //Check whether selected Reviewer record exist in db if not exist then insert it
                    foreach (var reviewBy in assignmentProperty.AssignmentPropertiesReviewers)
                    {
                        if (!(assignmentPropertyIndb.AssignmentPropertiesReviewers.Any(obj => obj.ReviewerTypeID == reviewBy.ReviewerTypeID && obj.IsDeleted == false)))
                        {
                            AssignmentPropertiesReviewer newReviewer = new AssignmentPropertiesReviewer()
                            {
                                ReviewerTypeID = reviewBy.ReviewerTypeID,
                                IsDeleted = false,
                                CreatedBy = loggedInUserId,
                                CreatedOn = DateTime.Now
                            };
                            assignmentPropertyIndb.AssignmentPropertiesReviewers.Add(newReviewer);
                        }
                    }

                    //Check whether all Reviewer record in db are currently selected if not  then delete record from db
                    foreach (var reviewBy in assignmentPropertyIndb.AssignmentPropertiesReviewers.Where(obj => obj.IsDeleted == false))
                    {
                        if (!(assignmentProperty.AssignmentPropertiesReviewers.Any(obj => obj.ReviewerTypeID == reviewBy.ReviewerTypeID)))
                        {
                            reviewBy.IsDeleted = true;
                            reviewBy.ModifiedBy = loggedInUserId;
                            reviewBy.ModifiedOn = DateTime.Now;
                        }
                    }
                    assignmentPropertyIndb.EffectiveDate = assignmentProperty.EffectiveDate;
                    assignmentPropertyIndb.ReviewerTenantID = assignmentProperty.ReviewerTenantID;
                    assignmentPropertyIndb.ApprovalRequired = assignmentProperty.ApprovalRequired;
                    assignmentPropertyIndb.TPReviewerUserID = assignmentProperty.TPReviewerUserID;
                }

                assignmentPropertyIndb.IsActive = assignmentProperty.IsActive;
                assignmentPropertyIndb.ModifiedBy = loggedInUserId;
                assignmentPropertyIndb.ModifiedOn = DateTime.Now;
                assignmentPropertyIndb.IsAdminDataEntryNotAllowed = assignmentProperty.IsAdminDataEntryNotAllowed;
            }
            else //Insert
            {
                Int32 assignmentHierarchyId = new Int32();

                if (currentRuleSetTreeTypeCode == RuleSetTreeType.Packages.GetStringValue())
                {
                    assignmentHierarchyId = InsertUpdateAssignmentHierarchyForPackage(currentDataId, loggedInUserId);
                }
                else if (currentRuleSetTreeTypeCode == RuleSetTreeType.Categories.GetStringValue())
                {
                    assignmentHierarchyId = InsertUpdateAssignmentHierarchyForCategory(parentPackageId, currentDataId, loggedInUserId);
                }
                else if (currentRuleSetTreeTypeCode == RuleSetTreeType.Items.GetStringValue())
                {
                    assignmentHierarchyId = InsertUpdateAssignmentHierarchyForItem(parentPackageId, parentCategoryId, currentDataId, loggedInUserId);
                }
                else if (currentRuleSetTreeTypeCode == RuleSetTreeType.Attributes.GetStringValue())
                {
                    assignmentHierarchyId = InsertUpdateAssignmentHierarchyForAttribute(parentPackageId, parentCategoryId, parentItemId, currentDataId, loggedInUserId);
                }

                //Editable By will not be shown for Packages
                if (currentRuleSetTreeTypeCode != RuleSetTreeType.Packages.GetStringValue())
                {
                    foreach (var editableBy in assignmentProperty.AssignmentPropertiesEditableBies)
                    {
                        editableBy.CreatedBy = loggedInUserId;
                        editableBy.CreatedOn = DateTime.Now;
                    }
                    //Assigning whether to allow exception submission in the category
                    if (currentRuleSetTreeTypeCode == RuleSetTreeType.Categories.GetStringValue())
                    {
                        assignmentProperty.IsExceptionNotAllowed = assignmentProperty.IsExceptionNotAllowed;
                    }
                }

                if (currentRuleSetTreeTypeCode != RuleSetTreeType.Attributes.GetStringValue())
                {
                    foreach (var reviewBy in assignmentProperty.AssignmentPropertiesReviewers)
                    {
                        reviewBy.CreatedBy = loggedInUserId;
                        reviewBy.CreatedOn = DateTime.Now;
                    }
                }
                assignmentProperty.AssignmentHierarchyID = assignmentHierarchyId;
                assignmentProperty.CreatedBy = loggedInUserId;
                assignmentProperty.CreatedOn = DateTime.Now;
                _ClientDBContext.AddToAssignmentProperties(assignmentProperty);
            }
            _ClientDBContext.SaveChanges();
        }

        /// <summary>
        /// Insert the assignment hierarchy for package node
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public Int32 InsertUpdateAssignmentHierarchyForPackage(Int32 packageId, Int32 loggedInUserId)
        {
            if (packageId != null)
            {
                Int32 objectTypeIdForPackage = GetLargeObjectTypeIdByCode(ObjectType.Compliance_Package.GetStringValue());
                AssignmentHierarchy assignmentHierarchyForPackage = getAssignmentHierarchyIdForPackageNode(packageId, objectTypeIdForPackage, loggedInUserId);
                if (assignmentHierarchyForPackage.AssignmentHierarchyID == AppConsts.NONE)
                {
                    _ClientDBContext.AddToAssignmentHierarchies(assignmentHierarchyForPackage);
                }
                _ClientDBContext.SaveChanges();
                return assignmentHierarchyForPackage.AssignmentHierarchyID;
            }
            return AppConsts.NONE;
        }

        /// <summary>
        /// Insert the assignment hierarchy for category node
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="CategoryId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public Int32 InsertUpdateAssignmentHierarchyForCategory(Int32 packageId, Int32 CategoryId, Int32 loggedInUserId)
        {
            if (packageId != null)
            {
                Int32 objectTypeIdForPackage = GetLargeObjectTypeIdByCode(ObjectType.Compliance_Package.GetStringValue());
                AssignmentHierarchy assignmentHierarchyForPackage = getAssignmentHierarchyIdForPackageNode(packageId, objectTypeIdForPackage, loggedInUserId);
                if (assignmentHierarchyForPackage.AssignmentHierarchyID == AppConsts.NONE)
                {
                    _ClientDBContext.AddToAssignmentHierarchies(assignmentHierarchyForPackage);
                    _ClientDBContext.SaveChanges();
                }
                if (CategoryId != null && assignmentHierarchyForPackage != null)
                {
                    Int32 objectTypeIdForCategory = GetLargeObjectTypeIdByCode(ObjectType.Compliance_Category.GetStringValue());
                    AssignmentHierarchy assignmentHierarchyForCategory = getAssignmentHierarchyIdForCategoryNode(CategoryId, objectTypeIdForCategory, assignmentHierarchyForPackage.AssignmentHierarchyID, loggedInUserId);
                    if (assignmentHierarchyForCategory.AssignmentHierarchyID == AppConsts.NONE)
                    {
                        _ClientDBContext.AddToAssignmentHierarchies(assignmentHierarchyForCategory);
                    }
                    _ClientDBContext.SaveChanges();
                    return assignmentHierarchyForCategory.AssignmentHierarchyID;
                }
            }
            return AppConsts.NONE;
        }

        /// <summary>
        /// Insert the assignment hierarchy for item node
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="CategoryId"></param>
        /// <param name="itemId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public Int32 InsertUpdateAssignmentHierarchyForItem(Int32 packageId, Int32 CategoryId, Int32 itemId, Int32 loggedInUserId)
        {
            if (packageId != null)
            {
                Int32 objectTypeIdForPackage = GetLargeObjectTypeIdByCode(ObjectType.Compliance_Package.GetStringValue());
                AssignmentHierarchy assignmentHierarchyForPackage = getAssignmentHierarchyIdForPackageNode(packageId, objectTypeIdForPackage, loggedInUserId);
                if (assignmentHierarchyForPackage.AssignmentHierarchyID == AppConsts.NONE)
                {
                    _ClientDBContext.AddToAssignmentHierarchies(assignmentHierarchyForPackage);
                    _ClientDBContext.SaveChanges();
                }
                if (CategoryId != null && assignmentHierarchyForPackage != null)
                {
                    Int32 objectTypeIdForCategory = GetLargeObjectTypeIdByCode(ObjectType.Compliance_Category.GetStringValue());
                    AssignmentHierarchy assignmentHierarchyForCategory = getAssignmentHierarchyIdForCategoryNode(CategoryId, objectTypeIdForCategory, assignmentHierarchyForPackage.AssignmentHierarchyID, loggedInUserId);
                    if (assignmentHierarchyForCategory.AssignmentHierarchyID == AppConsts.NONE)
                    {
                        _ClientDBContext.AddToAssignmentHierarchies(assignmentHierarchyForCategory);
                        _ClientDBContext.SaveChanges();
                    }
                    if (itemId != null && assignmentHierarchyForCategory != null)
                    {
                        Int32 objectTypeIdForItem = GetLargeObjectTypeIdByCode(ObjectType.Compliance_Item.GetStringValue());
                        AssignmentHierarchy assignmentHierarchyForItem = getAssignmentHierarchyIdForItemNode(itemId, objectTypeIdForItem, assignmentHierarchyForCategory.AssignmentHierarchyID, loggedInUserId);
                        if (assignmentHierarchyForItem.AssignmentHierarchyID == AppConsts.NONE)
                        {
                            _ClientDBContext.AddToAssignmentHierarchies(assignmentHierarchyForItem);
                        }
                        _ClientDBContext.SaveChanges();
                        return assignmentHierarchyForItem.AssignmentHierarchyID;
                    }
                }
            }
            return AppConsts.NONE;
        }

        /// <summary>
        /// Insert the assignment hierarchy for Attribute node
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="CategoryId"></param>
        /// <param name="itemId"></param>
        /// <param name="attributeId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public Int32 InsertUpdateAssignmentHierarchyForAttribute(Int32 packageId, Int32 CategoryId, Int32 itemId, Int32 attributeId, Int32 loggedInUserId)
        {
            if (packageId != null)
            {
                Int32 objectTypeIdForPackage = GetLargeObjectTypeIdByCode(ObjectType.Compliance_Package.GetStringValue());
                AssignmentHierarchy assignmentHierarchyForPackage = getAssignmentHierarchyIdForPackageNode(packageId, objectTypeIdForPackage, loggedInUserId);
                if (assignmentHierarchyForPackage.AssignmentHierarchyID == AppConsts.NONE)
                {
                    _ClientDBContext.AddToAssignmentHierarchies(assignmentHierarchyForPackage);
                    _ClientDBContext.SaveChanges();
                }
                if (CategoryId != null && assignmentHierarchyForPackage != null)
                {
                    Int32 objectTypeIdForCategory = GetLargeObjectTypeIdByCode(ObjectType.Compliance_Category.GetStringValue());
                    AssignmentHierarchy assignmentHierarchyForCategory = getAssignmentHierarchyIdForCategoryNode(CategoryId, objectTypeIdForCategory, assignmentHierarchyForPackage.AssignmentHierarchyID, loggedInUserId);
                    if (assignmentHierarchyForCategory.AssignmentHierarchyID == AppConsts.NONE)
                    {
                        _ClientDBContext.AddToAssignmentHierarchies(assignmentHierarchyForCategory);
                        _ClientDBContext.SaveChanges();
                    }
                    if (itemId != null && assignmentHierarchyForCategory != null)
                    {
                        Int32 objectTypeIdForItem = GetLargeObjectTypeIdByCode(ObjectType.Compliance_Item.GetStringValue());
                        AssignmentHierarchy assignmentHierarchyForItem = getAssignmentHierarchyIdForItemNode(itemId, objectTypeIdForItem, assignmentHierarchyForCategory.AssignmentHierarchyID, loggedInUserId);
                        if (assignmentHierarchyForItem.AssignmentHierarchyID == AppConsts.NONE)
                        {
                            _ClientDBContext.AddToAssignmentHierarchies(assignmentHierarchyForItem);
                            _ClientDBContext.SaveChanges();
                        }
                        if (attributeId != null && assignmentHierarchyForItem != null)
                        {
                            Int32 objectTypeIdForAttribute = GetLargeObjectTypeIdByCode(ObjectType.Compliance_ATR.GetStringValue());

                            AssignmentHierarchy assignmentHierarchyForAttribute = GetAssignmentHierarchyIdForAttrNode(attributeId, objectTypeIdForAttribute, assignmentHierarchyForItem.AssignmentHierarchyID, loggedInUserId);
                            if (assignmentHierarchyForAttribute.AssignmentHierarchyID == AppConsts.NONE)
                            {
                                _ClientDBContext.AddToAssignmentHierarchies(assignmentHierarchyForAttribute);
                            }

                            _ClientDBContext.SaveChanges();
                            return assignmentHierarchyForAttribute.AssignmentHierarchyID;
                        }
                    }
                }
            }
            return AppConsts.NONE;
        }

        /// <summary>
        /// gets the assignment hierarchy for package node
        /// </summary>
        /// <param name="packageId">Package Id</param>
        /// <param name="objectTypeId">Object Type Id</param>
        /// <param name="loggedInUserId"> current User Id</param>
        /// <returns>Assignment Hierarchy</returns>
        private AssignmentHierarchy getAssignmentHierarchyIdForPackageNode(Int32 packageId, Int32 objectTypeId, Int32 loggedInUserId)
        {
            //Check whether assignment hierarchy already exist
            AssignmentHierarchy existingHierarchy = GetExistingPackageHierarchy(packageId, objectTypeId);
            if (existingHierarchy != null)
            {
                return existingHierarchy;
            }
            else
            {
                AssignmentHierarchy newHierarchy = new AssignmentHierarchy
                {
                    ObjectID = packageId,
                    ObjectTypeID = objectTypeId,
                    CreatedBy = loggedInUserId,
                    CreatedOn = DateTime.Now
                };

                return newHierarchy;
            }
        }

        private AssignmentHierarchy GetExistingPackageHierarchy(Int32 packageId, Int32 objectTypeId)
        {
            return _ClientDBContext.AssignmentHierarchies.Where(obj => obj.ObjectID == packageId
                   && obj.ObjectTypeID == objectTypeId
                   && obj.IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// gets the assignment hierarchy for category node
        /// </summary>
        /// <param name="categoryId"> Category Id</param>
        /// <param name="objectTypeId">Object Type Id</param>
        /// <param name="loggedInUserId"> current User Id</param>
        /// <param name="parentId">Assignment hierachy Id of parent</param>
        /// <returns>Assignment Hierarchy</returns>
        private AssignmentHierarchy getAssignmentHierarchyIdForCategoryNode(Int32 categoryId, Int32 objectTypeId, Int32 parentId, Int32 loggedInUserId)
        {
            //Check whether assignment hierarchy already exist
            AssignmentHierarchy existingHierarchy = GetExistingCategoryHierarchy(categoryId, parentId, objectTypeId);
            if (existingHierarchy != null)
            {
                return existingHierarchy;
            }
            else
            {
                AssignmentHierarchy newHierarchy = new AssignmentHierarchy
                {
                    ObjectID = categoryId,
                    ObjectTypeID = objectTypeId,
                    ParentID = parentId,
                    CreatedBy = loggedInUserId,
                    CreatedOn = DateTime.Now
                };
                return newHierarchy;
            }
        }

        private AssignmentHierarchy GetExistingCategoryHierarchy(Int32 categoryId, Int32 parentId, Int32 objectTypeId)
        {
            return _ClientDBContext.AssignmentHierarchies.Where(obj => obj.ObjectID == categoryId
               && obj.ObjectTypeID == objectTypeId
               && obj.ParentID == parentId
               && obj.IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// gets the assignment hierarchy for item node
        /// </summary>
        /// <param name="itemId">Item id</param>
        /// <param name="objectTypeId">Object Type Id</param>
        /// <param name="loggedInUserId"> current User Id</param>
        /// <param name="parentId">Assignment hierachy Id of parent</param>
        /// <returns>Assignment Hierarchy</returns>
        private AssignmentHierarchy getAssignmentHierarchyIdForItemNode(Int32 itemId, Int32 objectTypeId, Int32 parentId, Int32 loggedInUserId)
        {
            //Check whether assignment hierarchy already exist
            AssignmentHierarchy existingHierarchy = GetExistingItemHierarchy(itemId, parentId, objectTypeId);
            if (existingHierarchy != null)
            {
                return existingHierarchy;
            }
            else
            {
                AssignmentHierarchy newHierarchy = new AssignmentHierarchy
                {
                    ObjectID = itemId,
                    ObjectTypeID = objectTypeId,
                    ParentID = parentId,
                    CreatedBy = loggedInUserId,
                    CreatedOn = DateTime.Now
                };
                return newHierarchy;
            }
        }

        /// <summary>
        /// gets the assignment hierarchy for Attribute node
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="objectTypeId"></param>
        /// <param name="parentId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        private AssignmentHierarchy GetAssignmentHierarchyIdForAttrNode(Int32 attributeId, Int32 objectTypeId, Int32 parentId, Int32 loggedInUserId)
        {
            //Check whether assignment hierarchy already exist
            AssignmentHierarchy existingHierarchy = GetExistingAttributeHierarchy(attributeId, parentId, objectTypeId);
            if (existingHierarchy != null)
            {
                return existingHierarchy;
            }
            else
            {
                AssignmentHierarchy newHierarchy = new AssignmentHierarchy
                {
                    ObjectID = attributeId,
                    ObjectTypeID = objectTypeId,
                    ParentID = parentId,
                    CreatedBy = loggedInUserId,
                    CreatedOn = DateTime.Now
                };
                return newHierarchy;
            }
        }

        private AssignmentHierarchy GetExistingItemHierarchy(Int32 itemId, Int32 parentId, Int32 objectTypeId)
        {
            return _ClientDBContext.AssignmentHierarchies.Where(obj => obj.ObjectID == itemId
              && obj.ObjectTypeID == objectTypeId
              && obj.ParentID == parentId
              && obj.IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// Gets the AssignmentProperty by AssignmentHierarchy
        /// </summary>
        /// <param name="assignmentHierarchy">assignmentHierarchy</param>
        /// <returns>AssignmentProperty</returns>
        private AssignmentProperty GetsAssignmentPropertiesByHierarchy(AssignmentHierarchy assignmentHierarchy)
        {
            if (assignmentHierarchy.IsNotNull())
            {
                return _ClientDBContext.AssignmentProperties.FirstOrDefault(x => x.IsDeleted == false && x.AssignmentHierarchyID == assignmentHierarchy.AssignmentHierarchyID);
            }
            return null;
        }

        /// <summary>
        /// Gets Assignment Options By Hierarchy
        /// </summary>
        /// <param name="assignmentHierarchy">assignmentHierarchy</param>
        /// <returns>AssignmentProperty</returns>
        private AssignmentProperty GetAssignmentOptionsByHierarchy(AssignmentHierarchy assignmentHierarchy)
        {
            return _ClientDBContext.AssignmentProperties.FirstOrDefault(x => x.IsDeleted == false && x.IsActive == true &&
                x.AssignmentHierarchyID == assignmentHierarchy.AssignmentHierarchyID);
        }

        /// <summary>
        /// Get the following default values of Assignment Properties options.
        /// Effective Date = Today Date, Approval Required = true, Is Active = True, Editable By = All items of lkpEditableBies and Reviewd By = ADB Admin.
        /// </summary>
        /// <returns>AssignmentProperty</returns>
        private AssignmentProperty GetDefaultAssignmentOptions()
        {
            AssignmentProperty assignmentOptions = new AssignmentProperty();
            assignmentOptions.EffectiveDate = DateTime.Now;
            assignmentOptions.ApprovalRequired = true;
            assignmentOptions.IsActive = true;

            var lkpEditableBies = _ClientDBContext.lkpEditableBies.Where(x => x.IsDeleted == false);
            foreach (var editor in lkpEditableBies)
            {
                assignmentOptions.AssignmentPropertiesEditableBies.Add(new AssignmentPropertiesEditableBy()
                {
                    EditableByID = editor.ComplianceItemEditableByID,
                    IsDeleted = false,
                    lkpEditableBy = new lkpEditableBy
                    {
                        Code = editor.Code,
                        ComplianceItemEditableByID = editor.ComplianceItemEditableByID,
                        Name = editor.Name
                    }
                });
            }

            var adminReviewer = _ClientDBContext.lkpReviewerTypes.FirstOrDefault(x => x.Code == LkpReviewerType.Admin && x.IsDeleted == false);
            assignmentOptions.AssignmentPropertiesReviewers.Add(new AssignmentPropertiesReviewer()
            {
                ReviewerTypeID = adminReviewer.ReviewerTypeID,
                IsDeleted = false,
                lkpReviewerType = new lkpReviewerType
                {
                    ReviewerTypeID = adminReviewer.ReviewerTypeID,
                    Code = adminReviewer.Code,
                    Name = adminReviewer.Name,
                }
            });
            return assignmentOptions;
        }

        /// <summary>
        /// Fetch the object type id as per given code from lkpObjectType.
        /// </summary>
        /// <param name="lkpObjectType">lkpObjectType</param>
        /// <param name="objectCode">objectCode</param>
        /// <returns>Object ID</returns>
        private Int32 FetchObjectTypeIDByCode(List<lkpObjectType> lkpObjectType, String objectCode)
        {
            return lkpObjectType.FirstOrDefault(x => x.OT_Code == objectCode).OT_ID;
        }

        private AssignmentHierarchy GetExistingAttributeHierarchy(Int32 attributeId, Int32 parentId, Int32 objectTypeId)
        {
            return _ClientDBContext.AssignmentHierarchies.Where(obj => obj.ObjectID == attributeId
              && obj.ObjectTypeID == objectTypeId
              && obj.ParentID == parentId
              && obj.IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentObjectTypeID"></param>
        /// <param name="parentObjectID"></param>
        /// <param name="newObjectTypeID"></param>
        /// <param name="newObjectID"></param>
        /// <param name="loggedInUserId"></param>
        public void AddAssociationHierarchyNode(Int32? parentObjectTypeID, Int32? parentObjectID, Int32 newObjectTypeID, Int32 newObjectID, Int32 loggedInUserId, Boolean isDefaultAssgnmntRqud)
        {
            _ClientDBContext.usp_AddAssociationHierarchyNode(parentObjectTypeID, parentObjectID, newObjectTypeID, newObjectID, loggedInUserId, isDefaultAssgnmntRqud);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentObjectTypeID"></param>
        /// <param name="parentObjectID"></param>
        /// <param name="newObjectTypeID"></param>
        /// <param name="newObjectID"></param>
        /// <param name="loggedInUserId"></param>
        public void DeleteAssociationHierarchyNode(Int32? parentObjectTypeID, Int32? parentObjectID, Int32 newObjectTypeID, Int32 newObjectID, Int32 loggedInUserId)
        {
            _ClientDBContext.usp_DeleteAssociationHierarchyNode(parentObjectTypeID, parentObjectID, newObjectTypeID, newObjectID, loggedInUserId);
        }


        /// <summary>
        /// method to get association hierarchy id
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="categoryId"></param>
        /// <param name="itemId"></param>
        /// <param name="attributeId"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public Int32? getAssociationHierarchyIdForObject(Int32 loggedInUserId, Int32 packageId, Int32 categoryId, Int32 itemId, Int32 attributeId)
        {
            Int32 objectTypeIdForPackage = GetLargeObjectTypeIdByCode(ObjectType.Compliance_Package.GetStringValue());
            AssignmentHierarchy assignmentHierarchyForPackage = GetExistingPackageHierarchy(packageId, objectTypeIdForPackage);
            if (assignmentHierarchyForPackage != null)
            {
                if (categoryId != AppConsts.NONE && categoryId != null)
                {
                    Int32 objectTypeIdForCategory = GetLargeObjectTypeIdByCode(ObjectType.Compliance_Category.GetStringValue());
                    AssignmentHierarchy assignmentHierarchyForCategory = GetExistingCategoryHierarchy(categoryId, assignmentHierarchyForPackage.AssignmentHierarchyID, objectTypeIdForCategory);
                    if (assignmentHierarchyForCategory != null)
                    {
                        if (itemId != AppConsts.NONE && itemId != null)
                        {
                            Int32 objectTypeIdForItem = GetLargeObjectTypeIdByCode(ObjectType.Compliance_Item.GetStringValue());
                            AssignmentHierarchy assignmentHierarchyForItem = GetExistingItemHierarchy(itemId, assignmentHierarchyForCategory.AssignmentHierarchyID, objectTypeIdForItem);
                            if (assignmentHierarchyForItem != null)
                            {
                                if (attributeId != AppConsts.NONE && attributeId != null)
                                {
                                    Int32 objectTypeIdForAttribute = GetLargeObjectTypeIdByCode(ObjectType.Compliance_ATR.GetStringValue());
                                    AssignmentHierarchy assignmentHierarchyForAttribute = GetExistingAttributeHierarchy(attributeId, assignmentHierarchyForItem.AssignmentHierarchyID, objectTypeIdForAttribute);
                                    if (assignmentHierarchyForAttribute != null)
                                    {
                                        return assignmentHierarchyForAttribute.AssignmentHierarchyID;
                                    }
                                    return null;
                                }
                                return assignmentHierarchyForItem.AssignmentHierarchyID;
                            }
                            return null;
                        }
                        return assignmentHierarchyForCategory.AssignmentHierarchyID;
                    }
                    return null;
                }
                return assignmentHierarchyForPackage.AssignmentHierarchyID;
            }
            return null;
        }

        /// <summary>
        ///  Gets the list of Editable Bies for all the attributes in all the items and Category in Package
        /// </summary>
        /// <param name="parentPackageDataID">packageId</param>
        /// <returns></returns>
        public List<ListCategoryEditableBies> GetEditableBiesByPackageId(Int32 packageId)
        {
            return _ClientDBContext.GetEditableBiesByPackage(packageId).ToList();
        }

        #endregion

        #region Subscription Options

        public List<SubscriptionOption> GetSubscriptionOptionsList()
        {
            return _ClientDBContext.SubscriptionOptions.Where(x => x.IsDeleted == false).ToList();
        }

        public void SaveSubscriptionOption(SubscriptionOption newSubscriptionOption, Int32? subscriptionOptionID = null)
        {
            if (subscriptionOptionID.IsNull())
            {
                _ClientDBContext.SubscriptionOptions.AddObject(newSubscriptionOption);
            }
            else
            {
                var subscriptionOptionToUpdate = _ClientDBContext.SubscriptionOptions.FirstOrDefault(x => x.SubscriptionOptionID == subscriptionOptionID
                    && x.IsDeleted == false);
                if (subscriptionOptionToUpdate.IsNotNull())
                {
                    subscriptionOptionToUpdate.Label = newSubscriptionOption.Label;
                    subscriptionOptionToUpdate.Description = newSubscriptionOption.Description;
                    subscriptionOptionToUpdate.Year = newSubscriptionOption.Year;
                    subscriptionOptionToUpdate.Month = newSubscriptionOption.Month;
                }
            }
            _ClientDBContext.SaveChanges();
        }

        public void DeletSubscriptionOption(SubscriptionOption subscriptionOption)
        {
            var subscriptionOptionToDelete = _ClientDBContext.SubscriptionOptions.FirstOrDefault(x => x.SubscriptionOptionID == subscriptionOption.SubscriptionOptionID
                && x.IsDeleted == false);
            if (subscriptionOptionToDelete.IsNotNull())
            {
                subscriptionOptionToDelete.IsDeleted = true;
                subscriptionOptionToDelete.ModifiedByID = subscriptionOption.ModifiedByID;
                subscriptionOptionToDelete.ModifiedOn = DateTime.Now;
                _ClientDBContext.SaveChanges();
            }
        }

        #endregion

        #region Department, Program Subscriptions and Price

        /// <summary>
        /// To get Price Adjustment List
        /// </summary>
        /// <returns></returns>
        public List<PriceAdjustment> GetPriceAdjustmentList()
        {
            return _ClientDBContext.PriceAdjustments.Where(x => x.IsDeleted == false).ToList();
        }

        /// <summary>
        /// To get Program Packages by Program Map Id
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <returns></returns>
        public List<DeptProgramPackage> GetProgramPackagesByProgramMapId(Int32 deptProgramMappingID)
        {
            List<DeptProgramPackage> mappedPackages = _ClientDBContext.DeptProgramPackages.Include("Orders").Include("CompliancePackage")
                                                                       .Where(obj => obj.DeptProgramMapping.DPM_ID == deptProgramMappingID
                                                                       && obj.DPP_IsDeleted == false && obj.DeptProgramMapping.DPM_IsDeleted == false
                                                                       && obj.CompliancePackage.IsDeleted == false && obj.CompliancePackage.IsActive == true)
                                                                       .OrderBy(x => x.CompliancePackage.PackageName).ToList();

            SecurityRepository securityRepository = new SecurityRepository();
            var tenantList = ((ISecurityRepository)securityRepository).GetTenants(false, false).ToList();
            mappedPackages.Where(t => t.CompliancePackage.TenantID != null)
               .ForEach(t =>
               {
                   var tenant = tenantList.FirstOrDefault(item => item.TenantID == t.CompliancePackage.TenantID.Value);
                   if (tenant.IsNotNull())
                   {
                       t.CompliancePackage.TenantName = tenant.TenantName;
                   }
                   else
                   {
                       t.CompliancePackage.TenantName = String.Empty;
                   }
               });
            return mappedPackages;
        }

        /// <summary>
        /// Get the successor package dropdownlist selectedvalue
        /// </summary>
        /// <param name="DeptProgramMappingID">Source nodeid</param>
        /// <param name="SelectedSuccessorNodeID">target nodeid</param>
        /// <returns></returns>
        public List<MobilityPackageRelation> GetSuccessorPackageIds(Int32 DeptProgramMappingID, Int32 SelectedSuccessorNodeID)
        {
            return _ClientDBContext.MobilityPackageRelations.Where(x => x.InstHierarchyMobility.IHM_HierarchyID == DeptProgramMappingID &&
                x.InstHierarchyMobility.IHM_SuccessorID == SelectedSuccessorNodeID && !x.InstHierarchyMobility.IHM_IsDeleted && !x.MPR_IsDeleted).ToList();
        }

        /// <summary>
        ///  To get Program Packages for the given list of Program Ids.
        /// </summary>
        /// <param name="programIds"></param>
        /// <returns></returns>
        public List<CompliancePackage> GetProgramPackagesByProgramId(Int32 departmentId, List<Int32> programIds)
        {//below commented out code has to be written as per new schema
            IQueryable<DeptProgramPackage> mappedPackages = null;
            //if (programIds.Count() > AppConsts.NONE)
            //{
            //    mappedPackages = _ClientDBContext.DeptProgramPackages
            //                        .Where(obj => programIds.Contains(obj.DeptProgramMapping.DPM_ProgramID)
            //                        && obj.DPP_IsDeleted == false && obj.DeptProgramMapping.DPM_IsDeleted == false
            //                        && obj.CompliancePackage.IsDeleted == false && obj.CompliancePackage.IsActive == true);
            //}
            //else
            //{
            //    mappedPackages = _ClientDBContext.DeptProgramPackages
            //                        .Where(obj => obj.DeptProgramMapping.DPM_OrganizationID == departmentId
            //                        && obj.DPP_IsDeleted == false && obj.DeptProgramMapping.DPM_IsDeleted == false
            //                        && obj.CompliancePackage.IsDeleted == false && obj.CompliancePackage.IsActive == true);
            //}

            return mappedPackages.Select(x => x.CompliancePackage).Distinct().ToList();
        }

        /// <summary>
        /// To get not mapped Compliance Packages
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <returns></returns>
        public List<CompliancePackage> GetNotMappedCompliancePackagesByMapId(Int32 deptProgramMappingID)
        {
            return _ClientDBContext.GetInstituteHierarchyTreePackages(deptProgramMappingID).ToList();
        }

        /// <summary>
        /// To get Institution Nodes By Program Map Id
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <returns></returns>
        public IQueryable<DeptProgramMapping> GetInstitutionNodesByProgramMapId(Int32 deptProgramMappingID)
        {
            return _ClientDBContext.DeptProgramMappings.Include("InstitutionNode").Where(obj => obj.DPM_ParentNodeID == deptProgramMappingID
                                                                       && obj.DPM_IsDeleted == false
                                                                       && obj.InstitutionNode.IN_IsDeleted == false);
        }

        /// <summary>
        /// To get Institution Child Nodes By Program Map Id
        /// </summary>
        /// <param name="deptProgramMappingIDs"></param>
        /// <returns></returns>
        public List<Int32> GetInstitutionChildNodesByProgramMapId(List<Int32> deptProgramMappingIDs)
        {
            //To get Ids who's child exists
            List<Int32> childIDs = _ClientDBContext.DeptProgramMappings.Where(obj => obj.DPM_ParentNodeID != null && deptProgramMappingIDs.Contains(obj.DPM_ParentNodeID.Value)
                                                                       && obj.DPM_IsDeleted == false
                                                                       && obj.InstitutionNode.IN_IsDeleted == false)
                                                                       .Select(x => x.DPM_ParentNodeID.Value).Distinct()
                                                                       .ToList();

            //To get Ids which are associated with packages
            childIDs.AddRange(_ClientDBContext.DeptProgramPackages.Where(x => deptProgramMappingIDs.Contains(x.DPP_DeptProgramMappingID) && x.DPP_IsDeleted == false)
                                                .Select(x => x.DPP_DeptProgramMappingID).Distinct().ToList());

            return childIDs.Distinct().ToList();
        }

        /// <summary>
        /// To save dept Program Package Mapping
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="packageId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="IsCreatedByAdmin"></param>
        /// <param name="_lstSelectedOptionIds"></param>
        /// <param name="paymentApprovalRequiredID"></param>
        /// <returns></returns>
        public Boolean SaveProgramPackageMapping(Int32 deptProgramMappingID, Int32 packageId, Int32 currentUserId, Boolean IsCreatedByAdmin,
            List<Int32> _lstSelectedOptionIds, Int32 paymentApprovalRequiredID)
        {
            var _currentDateTime = DateTime.Now;
            String code = PackageAvailability.AVAILABLE_FOR_ORDER.GetStringValue();
            List<lkpPackageAvailability> pkgAvailability = GetPackageAvailablity();
            Int32 paID = pkgAvailability.FirstOrDefault(x => x.PA_Code == code).PA_ID;

            DeptProgramPackage newMapping = new DeptProgramPackage();

            newMapping.DPP_DeptProgramMappingID = deptProgramMappingID;
            newMapping.DPP_CompliancePackageID = packageId;
            newMapping.DPP_IsDeleted = false;
            newMapping.DPP_CreatedByID = currentUserId;
            newMapping.DPP_CreatedOn = _currentDateTime;
            //newMapping.CPC_IsCreatedByAdmin = IsCreatedByAdmin;
            newMapping.DPP_PackageAvailabilityID = paID;
            newMapping.DPP_PaymentApprovalID = paymentApprovalRequiredID;

            for (int i = 0; i < _lstSelectedOptionIds.Count; i++)
            {
                DeptProgramPackagePaymentOption _dpppo = new DeptProgramPackagePaymentOption();
                _dpppo.DPPPO_DPPID = newMapping.DPP_ID;
                _dpppo.DPPPO_IsDeleted = false;
                _dpppo.DPPPO_CreatedByID = currentUserId;
                _dpppo.DPPPO_CreatedOn = _currentDateTime;
                _dpppo.DPPPO_PaymentOptionID = _lstSelectedOptionIds[i];

                newMapping.DeptProgramPackagePaymentOptions.Add(_dpppo);
            }

            _ClientDBContext.DeptProgramPackages.AddObject(newMapping);

            _ClientDBContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// To save Program Package Mapping Node
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="nodeId"></param>
        /// <param name="nodeName"></param>
        /// <param name="paymentOptions"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public Boolean SaveProgramPackageMappingNode(Int32 tenantId, Int32 deptProgramMappingID, Int32 nodeId, String nodeName, List<Int32> paymentOptions, List<Int32> fileExtensions,
                                                     Int32 currentUserId, String nodeLabel, Boolean isAvailableForOrder, Boolean isEmployment,
                                                     Int32? archivalGracePeriod, Int32? PDFInclusionID, Int32? resultsSentToApplicantID, String splashPageUrl, String ExpirationFrequency,
                                                     Int32? AfterExpirationFrequency, Int32? SubscriptionBeforeExpiry, Int32? SubscriptionAfterExpiry, Int32? SubscriptionExpiryFrequency,
                                                     Int32 paymentApprovalID, Int16 nagEmailNotificationTypeId, Int32? hierarchyNodeExemptedType, Boolean IsCallFromBkgHierarchySetup)
        {
            Int32 maxDisplayOrder = _ClientDBContext.DeptProgramMappings.Where(x => x.DPM_ParentNodeID == deptProgramMappingID).Max(x => x.DPM_DisplayOrder) ?? 0;


            String label = String.Empty;
            DeptProgramMapping newMapping = new DeptProgramMapping();
            newMapping.DPM_ParentNodeID = deptProgramMappingID;
            newMapping.DPM_InstitutionNodeID = nodeId;
            newMapping.DPM_IsDeleted = false;
            newMapping.DPM_CreatedByID = currentUserId;
            newMapping.DPM_CreatedOn = DateTime.Now;
            newMapping.DPM_ArchivalGracePeriod = archivalGracePeriod;
            newMapping.DPM_DisplayOrder = maxDisplayOrder + 1;
            newMapping.DPM_IsAvailableForOrder = isAvailableForOrder;
            newMapping.DPM_IsEmployment = isEmployment; //UAT-1176
            newMapping.DPM_SplashPageUrl = splashPageUrl.IsNullOrEmpty() ? null : splashPageUrl;
            newMapping.DPM_ExpirationFrequency = ExpirationFrequency.IsNullOrEmpty() ? null : ExpirationFrequency;
            newMapping.DPM_AfterExpirationFrequency = AfterExpirationFrequency;
            newMapping.DPM_SubscriptionAfterExpFrequency = SubscriptionAfterExpiry;
            newMapping.DPM_SubscriptionBeforeExpFrequency = SubscriptionBeforeExpiry;
            newMapping.DPM_SubscriptionEmailFrequency = SubscriptionExpiryFrequency;
            newMapping.DPM_PaymentApprovalID = paymentApprovalID;
            newMapping.DPM_BGPDFInclusionID = PDFInclusionID;
            newMapping.DPM_ResultSentToApplicantID = resultsSentToApplicantID;
            if (IsCallFromBkgHierarchySetup)
            {
                newMapping.DPM_NodeExemptedInRotaionID = hierarchyNodeExemptedType;
            }

            //Insert label with parent label
            var deptProgramMapping = _ClientDBContext.DeptProgramMappings.FirstOrDefault(x => x.DPM_ID == deptProgramMappingID && x.DPM_IsDeleted == false);
            if (deptProgramMapping.IsNotNull())
            {
                label = deptProgramMapping.DPM_Label;
                if (nodeLabel.IsNullOrEmpty())
                {
                    label += " > " + nodeName;
                }
                else
                {
                    label += " > " + nodeLabel;
                }

            }
            newMapping.DPM_Label = label;

            DeptProgramPaymentOption tempPaymentOption = null;
            List<DeptProgramPaymentOption> selectedPaymentOptionList = new List<DeptProgramPaymentOption>();

            foreach (var paymentOptionID in paymentOptions)
            {
                tempPaymentOption = new DeptProgramPaymentOption();
                tempPaymentOption.DPPO_PaymentOptionID = paymentOptionID;
                tempPaymentOption.DPPO_IsDeleted = false;
                tempPaymentOption.DPPO_CreatedByID = currentUserId;
                tempPaymentOption.DPPO_CreatedOn = DateTime.Now;
                newMapping.DeptProgramPaymentOptions.Add(tempPaymentOption);
                //selectedPaymentOptionList.Add(tempPaymentOption);
            }

            DeptProgramRestrictedFileExtension tempFileExtension = null;
            List<DeptProgramRestrictedFileExtension> selectedFileExtensionList = new List<DeptProgramRestrictedFileExtension>();

            foreach (var fileExtensionID in fileExtensions)
            {
                tempFileExtension = new DeptProgramRestrictedFileExtension();
                tempFileExtension.DPRFE_FileExtensionID = fileExtensionID;
                tempFileExtension.DPRFE_IsDeleted = false;
                tempFileExtension.DPRFE_CreatedByID = currentUserId;
                tempFileExtension.DPRFE_CreatedOn = DateTime.Now;
                newMapping.DeptProgramRestrictedFileExtensions.Add(tempFileExtension);
            }

            _ClientDBContext.DeptProgramMappings.AddObject(newMapping);

            if (_ClientDBContext.SaveChanges() > 0)
            {
                //UAT-2501: Update default nag email settings for new nodes
                Int32 nagFrequency = AppConsts.SEVEN;
                SaveNagEmailNotifications(tenantId, nagEmailNotificationTypeId, newMapping.DPM_ID, nagFrequency, currentUserId, true);
                if (IsCallFromBkgHierarchySetup)
                {
                    var lkpBkgHierarchyNodeExemptedTypes = _ClientDBContext.lkpBkgHierarchyNodeExemptedTypes.Where(cond => cond.HNET_ID == hierarchyNodeExemptedType && !cond.HNET_IsDeleted).First();


                    InsertExemptedHierarchyNode(deptProgramMappingID, lkpBkgHierarchyNodeExemptedTypes.HNET_Code, currentUserId);
                }
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// To delete Program Package Mapping
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="packageId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public Boolean DeleteProgramPackageMapping(Int32 deptProgramMappingID, Int32 packageId, Int32 currentUserId)
        {
            DeptProgramPackage existingMapping = _ClientDBContext.DeptProgramPackages.FirstOrDefault(obj => obj.DPP_DeptProgramMappingID == deptProgramMappingID && obj.DPP_CompliancePackageID == packageId && obj.DPP_IsDeleted == false);
            if (existingMapping.IsNotNull())
            {
                existingMapping.DPP_IsDeleted = true;
                existingMapping.DPP_ModifiedById = currentUserId;
                existingMapping.DPP_ModifiedOn = DateTime.Now;
                _ClientDBContext.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// To get Dept Program Package Subscription List by Dept Program Package Id
        /// </summary>
        /// <param name="DeptProgramPackageId"></param>
        /// <returns></returns>
        public List<DeptProgramPackageSubscription> GetDeptProgramPackageSubscriptionByProgPackageId(Int32 DeptProgramPackageId)
        {
            return _ClientDBContext.DeptProgramPackageSubscriptions.Where(obj => obj.DeptProgramPackage.DPP_ID == DeptProgramPackageId
                                                                    && obj.DPPS_IsDeleted == false && obj.DeptProgramPackage.DPP_IsDeleted == false).ToList();

        }

        /// <summary>
        /// To get Dept Program Package by Package Id
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public List<DeptProgramPackage> GetDeptProgramPackageByPackageId(Int32 packageId)
        {
            return _ClientDBContext.DeptProgramPackages.Where(obj => obj.DPP_CompliancePackageID == packageId && obj.DPP_IsDeleted == false
                                                        && obj.CompliancePackage.IsDeleted == false && obj.CompliancePackage.IsActive == true).ToList();

        }

        /// <summary>
        /// To save Program Package Subscription Mapping
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <param name="subscriptionIDs"></param>
        /// <param name="priceModelID"></param>
        /// <param name="savedPriceModelId"></param>
        /// <param name="priority"></param>
        /// <param name="currentUserId"></param>
        /// <param name="lstSelectedOptionIds"></param>
        /// <param name="paymentApprovalID"></param>
        /// <returns></returns>
        public Boolean SaveProgramPackageSubscriptionMapping(Int32 deptProgramPackageID, List<Int32> subscriptionIDs, Int32 priceModelID, Int32 savedPriceModelId,
                                                            Int32 priority, Int32 currentUserId, List<Int32> lstSelectedOptionIds, Int32 paymentApprovalID)
        {
            DeptProgramPackageSubscription tempSubscriptionOption = null;
            List<DeptProgramPackageSubscription> selectedSubscriptionOptionList = new List<DeptProgramPackageSubscription>();

            foreach (var subscriptionID in subscriptionIDs)
            {
                tempSubscriptionOption = new DeptProgramPackageSubscription();
                tempSubscriptionOption.DPPS_DeptProgramPackageID = deptProgramPackageID;
                tempSubscriptionOption.DPPS_SubscriptionID = subscriptionID;
                tempSubscriptionOption.DPPS_IsDeleted = false;
                tempSubscriptionOption.DPPS_CreatedByID = currentUserId;
                tempSubscriptionOption.DPPS_CreatedOn = DateTime.Now;
                selectedSubscriptionOptionList.Add(tempSubscriptionOption);
                //_ClientDBContext.DeptProgramPackageSubscriptions.AddObject(newMapping);
            }

            List<DeptProgramPackageSubscription> mapSubscriptionOptionList = _ClientDBContext.DeptProgramPackageSubscriptions.Where(cond => cond.DPPS_DeptProgramPackageID == deptProgramPackageID && cond.DPPS_IsDeleted == false).ToList();
            List<DeptProgramPackageSubscription> subscriptionOptionToDelete = mapSubscriptionOptionList.Where(x => !selectedSubscriptionOptionList.Any(cnd => cnd.DPPS_SubscriptionID == x.DPPS_SubscriptionID)).ToList();
            List<DeptProgramPackageSubscription> SubscriptionOptionToInsert = selectedSubscriptionOptionList.Where(y => !mapSubscriptionOptionList.Any(cd => cd.DPPS_SubscriptionID == y.DPPS_SubscriptionID)).ToList();
            subscriptionOptionToDelete.ForEach(cond =>
            {
                cond.DPPS_IsDeleted = true;
                cond.DPPS_ModifiedByID = currentUserId;
                cond.DPPS_ModifiedOn = DateTime.Now;
            });
            SubscriptionOptionToInsert.ForEach(con =>
            {
                _ClientDBContext.DeptProgramPackageSubscriptions.AddObject(con);
            });

            //Update DeptProgramPackages table
            UpdateDeptProgramPackages(deptProgramPackageID, priceModelID, priority, currentUserId, lstSelectedOptionIds, paymentApprovalID);

            //If already saved PriceModelId > 0 and priceModelID is not equal to savedPriceModelId then delete and reset all price data
            if (savedPriceModelId > 0 && priceModelID != savedPriceModelId)
            {
                DeleteResetPriceData(deptProgramPackageID, currentUserId);
            }
            if (_ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// To save Price and Price Adjustments Detail data
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="parentID"></param>
        /// <param name="mappingID"></param>
        /// <param name="parentSubscriptionID"></param>
        /// <param name="complianceCategoryID"></param>
        /// <param name="price"></param>
        /// <param name="rushOrderAdditionalPrice"></param>
        /// <param name="selectedPriceAdjustmentID"></param>
        /// <param name="priceAdjustmentValue"></param>
        /// <param name="currentUserId"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        public PriceContract SavePriceAdjustmentDetail(Int32 ID, Int32 parentID, Int32 mappingID, Int32 parentSubscriptionID, Int32 complianceCategoryID, Decimal price, Decimal? rushOrderAdditionalPrice,
                                                       Int32 selectedPriceAdjustmentID, Decimal priceAdjustmentValue, Int32 currentUserId, String treeNodeType)
        {
            DeptProgramPackageCategoryPrice deptProgramPackageCategoryPrice = new DeptProgramPackageCategoryPrice();
            DeptProgramPackageCategoryItemPrice deptProgramPackageCategoryItemPrice = new DeptProgramPackageCategoryItemPrice();
            PriceContract priceContract = new PriceContract();

            //Check if tree Node Type is Subscription/Package, Category or Item
            if (treeNodeType.Equals(RuleSetTreeNodeType.Subscription))
            {
                SavePackagePrice(ID, price, rushOrderAdditionalPrice, selectedPriceAdjustmentID, priceAdjustmentValue, currentUserId);
            }
            else if (treeNodeType.Equals(RuleSetTreeNodeType.Category))
            {
                SaveCategoryPrice(ID, parentID, mappingID, price, rushOrderAdditionalPrice, selectedPriceAdjustmentID, priceAdjustmentValue, currentUserId, deptProgramPackageCategoryPrice);
            }
            else
            {
                SaveItemPrice(ID, parentID, mappingID, parentSubscriptionID, complianceCategoryID, price, rushOrderAdditionalPrice, selectedPriceAdjustmentID, priceAdjustmentValue, currentUserId, deptProgramPackageCategoryPrice, deptProgramPackageCategoryItemPrice);
            }

            if (_ClientDBContext.SaveChanges() > 0)
            {
                if (treeNodeType.Equals(RuleSetTreeNodeType.Subscription)) //Subscription
                {
                    return priceContract;
                }
                if (treeNodeType.Equals(RuleSetTreeNodeType.Category)) //Category
                {
                    if (ID == AppConsts.NONE)
                    {
                        priceContract.NewID = deptProgramPackageCategoryPrice.DPPCP_ID;
                    }
                    return priceContract;
                }
                else //Item
                {
                    if (ID == AppConsts.NONE && parentID == AppConsts.NONE)
                    {
                        priceContract.NewID = deptProgramPackageCategoryPrice.DeptProgramPackageCategoryItemPrices.FirstOrDefault().DPPCIP_ID;
                        priceContract.NewParentID = deptProgramPackageCategoryPrice.DPPCP_ID;
                    }
                    else if (ID == AppConsts.NONE)
                    {
                        priceContract.NewID = deptProgramPackageCategoryItemPrice.DPPCIP_ID;
                    }
                    return priceContract;
                }
            }
            return priceContract;
        }

        /// <summary>
        /// To update Price Adjustment Detail
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="priceID"></param>
        /// <param name="selectedPriceAdjustmentID"></param>
        /// <param name="priceAdjustmentValue"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantId"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        public Boolean UpdatePriceAdjustmentDetail(Int32 ID, Int32 priceID, Int32 selectedPriceAdjustmentID, Decimal priceAdjustmentValue, Int32 currentUserId, String treeNodeType)
        {
            //Check if tree Node Type is Subscription/Package, Category or Item
            if (treeNodeType.Equals(RuleSetTreeNodeType.Subscription))
            {
                UpdatePackagePriceAdjustment(ID, priceID, selectedPriceAdjustmentID, priceAdjustmentValue, currentUserId);
            }
            else if (treeNodeType.Equals(RuleSetTreeNodeType.Category))
            {
                UpdateCategoryPriceAdjustment(ID, priceID, selectedPriceAdjustmentID, priceAdjustmentValue, currentUserId);
            }
            else
            {
                UpdateItemPriceAdjustment(ID, priceID, selectedPriceAdjustmentID, priceAdjustmentValue, currentUserId);
            }

            if (_ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// To delete Price Adjustment Data
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="priceID"></param>
        /// <param name="currentUserId"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        public Boolean DeletePriceAdjustmentData(Int32 ID, Int32 priceID, Int32 currentUserId, String treeNodeType)
        {
            //Check if tree Node Type is Subscription/Package, Category or Item
            if (treeNodeType.Equals(RuleSetTreeNodeType.Subscription))
            {
                return DeletePackagePriceAdjustmentData(ID, priceID, currentUserId);
            }
            else if (treeNodeType.Equals(RuleSetTreeNodeType.Category))
            {
                return DeleteCategoryPriceAdjustmentData(ID, priceID, currentUserId);
            }
            else
            {
                return DeleteItemPriceAdjustmentData(ID, priceID, currentUserId);
            }
        }

        /// <summary>
        /// To get Price Adjustment Data by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        public List<PriceContract> GetPriceAdjustmentData(Int32 ID, String treeNodeType)
        {
            //Check if tree Node Type is Subscription/Package, Category or Item
            if (treeNodeType.Equals(RuleSetTreeNodeType.Subscription))
            {
                return GetPackagePriceAdjustments(ID);
            }
            else if (treeNodeType.Equals(RuleSetTreeNodeType.Category))
            {
                return GetCategoryPriceAdjustments(ID);
            }
            else
            {
                return GetItemPriceAdjustments(ID);
            }
        }

        /// <summary>
        /// To get Dept Program Package By ID
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <returns></returns>
        public DeptProgramPackage GetDeptProgramPackageByID(Int32 deptProgramPackageID)
        {
            return _ClientDBContext.DeptProgramPackages.FirstOrDefault(obj => obj.DPP_ID == deptProgramPackageID && obj.DPP_IsDeleted == false);
        }

        /// <summary>
        /// To get Dept Program Package Subscription by ID
        /// </summary>
        /// <param name="deptProgramPackageSubscriptionID"></param>
        /// <returns></returns>
        public DeptProgramPackageSubscription GetDeptProgramPackageSubscriptionByID(Int32 deptProgramPackageSubscriptionID)
        {
            return _ClientDBContext.DeptProgramPackageSubscriptions.FirstOrDefault(obj => obj.DPPS_ID == deptProgramPackageSubscriptionID && obj.DPPS_IsDeleted == false);
        }

        /// <summary>
        /// To get Price
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        public PriceContract GetPrice(Int32 ID, String treeNodeType, Int32 ParentID = 0, Int32 MappingID = 0, Int32 ParentSubscriptionID = 0, Int32 ComplianceCatagoryID = 0, Int32 ItemID = 0)
        {
            //Check if tree Node Type is Subscription/Package, Category or Item
            if (treeNodeType.Equals(RuleSetTreeNodeType.Subscription))
            {
                return GetPackagePrice(ID);
            }
            else if (treeNodeType.Equals(RuleSetTreeNodeType.Category))
            {
                return GetCategoryPrice(ID, ParentID, MappingID);
            }
            else
            {
                return GetItemPrice(ID, ParentSubscriptionID, ComplianceCatagoryID, ItemID);
            }
        }

        /// <summary>
        /// To get total price
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        /*public PriceContract GetTotalPrice(Int32 ID, String treeNodeType)
        {
            //Check if tree Node Type is Subscription/Package, Category or Item
            if (treeNodeType.Equals(RuleSetTreeNodeType.Subscription))
            {
                return GetPackageTotalPrice(ID);
            }
            else if (treeNodeType.Equals(RuleSetTreeNodeType.Category))
            {
                return GetCategoryTotalPrice(ID);
            }
            else
            {
                return GetItemTotalPrice(ID);
            }
        } */

        /// <summary>
        /// To check if Price is Disabled
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="parentSubscriptionID"></param>
        /// <param name="mappingID"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        public Boolean CheckIsPriceDisabled(Int32 parentID, Int32 parentSubscriptionID, Int32 mappingID, String treeNodeType)
        {
            //Check if tree Node Type is Subscription/Package, Category or Item
            if (treeNodeType.Equals(RuleSetTreeNodeType.Subscription))
            {
                return CheckPackagePrice(mappingID); //Use mappingID for Package price
            }
            else if (treeNodeType.Equals(RuleSetTreeNodeType.Category))
            {
                return CheckCategoryPrice(parentID); //Use parentID for Category price
            }
            else
            {
                return CheckItemPrice(parentID, parentSubscriptionID); //Use parentID for Item price
            }
        }

        /// <summary>
        /// To show Message
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="parentSubscriptionID"></param>
        /// <param name="mappingID"></param>
        /// <param name="treeNodeType"></param>
        /// <returns></returns>
        public Boolean ShowMessage(Int32 parentID, Int32 parentSubscriptionID, Int32 mappingID, String treeNodeType)
        {
            //Check if tree Node Type is Subscription/Package, Category or Item
            if (treeNodeType.Equals(RuleSetTreeNodeType.Subscription))
            {
                return false;
            }
            else if (treeNodeType.Equals(RuleSetTreeNodeType.Category))
            {
                return ShowMessageForCategory(parentID); //Use parentID for Category price
            }
            else
            {
                return ShowMessageForItem(parentID, parentSubscriptionID); //Use parentID for Item price
            }
        }

        #region Institute Hierarchy Nodes

        /// <summary>
        /// To get Institution Node Types
        /// </summary>
        /// <returns></returns>
        public IQueryable<InstitutionNodeType> GetInstitutionNodeTypes()
        {
            return _ClientDBContext.InstitutionNodeTypes.Where(x => x.INT_IsDeleted == false);
        }

        /// <summary>
        /// To get Institution Nodes
        /// </summary>
        /// <param name="nodeTypeId"></param>
        /// <returns></returns>
        public List<InstitutionNode> GetInstitutionNodes(Int32 nodeTypeId)
        {
            return _ClientDBContext.InstitutionNodes.Where(x => x.IN_NodeTypeID == nodeTypeId && x.IN_IsDeleted == false).ToList();
        }

        /// <summary>
        /// To delete Program Package Mapping Node
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public Boolean DeleteProgramPackageMappingByID(Int32 deptProgramMappingID, Int32 currentUserId)
        {
            DeptProgramMapping existingMapping = _ClientDBContext.DeptProgramMappings.FirstOrDefault(obj => obj.DPM_ID == deptProgramMappingID && obj.DPM_IsDeleted == false);
            if (existingMapping.IsNotNull())
            {
                existingMapping.DPM_IsDeleted = true;

                existingMapping.DPM_ModifiedByID = currentUserId;
                existingMapping.DPM_ModifiedOn = DateTime.Now;

                //Re-Order Display Sequence of the Records.
                List<DeptProgramMapping> lstDeptProgramMapping = _ClientDBContext.DeptProgramMappings.Where(cond => !cond.DPM_IsDeleted
                                                                                        && cond.DPM_ParentNodeID == existingMapping.DPM_ParentNodeID
                                                                                        && cond.DPM_DisplayOrder > existingMapping.DPM_DisplayOrder).ToList();
                DataTable dtDeptProgMapping = new DataTable();
                dtDeptProgMapping.Columns.Add("DPMID", typeof(Int32));
                dtDeptProgMapping.Columns.Add("DestinationIndex", typeof(Int32));
                dtDeptProgMapping.Columns.Add("currentUserId", typeof(Int32));
                foreach (DeptProgramMapping DPM in lstDeptProgramMapping)
                {
                    dtDeptProgMapping.Rows.Add(new object[] { DPM.DPM_ID, DPM.DPM_DisplayOrder - 1, currentUserId });
                }
                EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand _command = new SqlCommand("dbo.UpdateNodeSequence", con);
                    _command.CommandType = CommandType.StoredProcedure;
                    _command.Parameters.AddWithValue("@typeDPM", dtDeptProgMapping);
                    con.Open();
                    Int32 rowsAffected = _command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        con.Close();
                    }
                    //_ClientDBContext.SaveChanges();
                    //_ClientDBContext.Refresh(RefreshMode.StoreWins, lstDeptProgramMapping);
                }
            }

            if (_ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// To delete dept Program Package by ID
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public Boolean DeleteProgramPackageByID(Int32 deptProgramPackageID, Int32 currentUserId)
        {
            DeptProgramPackage existingMapping = _ClientDBContext.DeptProgramPackages.FirstOrDefault(obj => obj.DPP_ID == deptProgramPackageID && obj.DPP_IsDeleted == false);
            if (existingMapping.IsNotNull())
            {
                existingMapping.DPP_IsDeleted = true;
                existingMapping.DPP_ModifiedById = currentUserId;
                existingMapping.DPP_ModifiedOn = DateTime.Now;
            }
            if (_ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// To save mapped Payment Options and Update the availability of the node, for the Order process
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="paymentOptions"></param>
        /// <param name="isAvailableForOrder"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public Boolean SaveMappedPaymentOptionsNodeAvailability(Int32 deptProgramMappingID, List<Int32> paymentOptions, List<Int32> fileExtensions, Boolean isAvailableForOrder, Boolean isEmployment, Int32 currentUserId,
                                                                String splashPageUrl, String ExpirationFrequency, Int32? AfterExpirationFrequency,
                                                                Int32? SubscriptionBeforeExpiry, Int32? SubscriptionAfterExpiry, Int32? SubscriptionExpiryFrequency,
                                                                String IsAdminDataEntryAllow, Int32 paymentApprovalID, String OptionalCategorySetting,  Int32? PDFInclusionID, Int32? resultsSentToApplicant, Int32? hierarchyNodeExemptedType,
                                                                Boolean IsCallFromBkgHierarchySetup)
        {
            DeptProgramPaymentOption tempPaymentOption = null;
            List<DeptProgramPaymentOption> selectedPaymentOptionList = new List<DeptProgramPaymentOption>();
            DeptProgramRestrictedFileExtension tempFileExtension = null;
            List<DeptProgramRestrictedFileExtension> selectedFileExtensionList = new List<DeptProgramRestrictedFileExtension>();

            var _deptProgramMapping = _ClientDBContext.DeptProgramMappings.Where(dpm => dpm.DPM_ID == deptProgramMappingID).First();
            _deptProgramMapping.DPM_IsAvailableForOrder = isAvailableForOrder;
            #region UAT-1794 : Ability to restrict admin data entry by node.
            if (IsAdminDataEntryAllow != null)
            {
                if (IsAdminDataEntryAllow.ToString() != "")
                    _deptProgramMapping.DPM_IsAdminDataEntryAllowed = IsAdminDataEntryAllow;
                else
                    _deptProgramMapping.DPM_IsAdminDataEntryAllowed = null;
            }
            #endregion

            #region UAT-3683 : Move Optional Category Setting From Client Settings to institution hierarchy with look up
            if (OptionalCategorySetting != null)
            {
                if (OptionalCategorySetting.ToString() != "")
                {
                    if (OptionalCategorySetting.ToString() == "Y")
                        _deptProgramMapping.DPM_OptionalCategorySetting = AppConsts.ONE;
                    else if (OptionalCategorySetting.ToString() == "N")
                        _deptProgramMapping.DPM_OptionalCategorySetting = AppConsts.NONE;
                }

                else
                {
                    _deptProgramMapping.DPM_OptionalCategorySetting = null;
                }

            }
            #endregion
            _deptProgramMapping.DPM_IsEmployment = isEmployment; //UAT-1176
            _deptProgramMapping.DPM_SplashPageUrl = splashPageUrl.IsNullOrEmpty() ? null : splashPageUrl;
            _deptProgramMapping.DPM_ExpirationFrequency = ExpirationFrequency.IsNullOrEmpty() ? null : ExpirationFrequency;
            #region UAT-4060
            _deptProgramMapping.DPM_AfterExpirationFrequency = AfterExpirationFrequency;
            _deptProgramMapping.DPM_SubscriptionBeforeExpFrequency = SubscriptionBeforeExpiry;
            _deptProgramMapping.DPM_SubscriptionAfterExpFrequency = SubscriptionAfterExpiry;
            _deptProgramMapping.DPM_SubscriptionEmailFrequency = SubscriptionExpiryFrequency;
            #endregion

            _deptProgramMapping.DPM_PaymentApprovalID = paymentApprovalID; //UAT-2073
            _deptProgramMapping.DPM_BGPDFInclusionID = PDFInclusionID;
            _deptProgramMapping.DPM_ResultSentToApplicantID = resultsSentToApplicant; // UAT-2842
            if (IsCallFromBkgHierarchySetup)
            {
                _deptProgramMapping.DPM_NodeExemptedInRotaionID = hierarchyNodeExemptedType; //UAT 3268
            }
            _deptProgramMapping.DPM_ModifiedByID = currentUserId;
            _deptProgramMapping.DPM_ModifiedOn = DateTime.Now;



            //Get list of selected mapped payment options
            foreach (var paymentOptionID in paymentOptions)
            {
                tempPaymentOption = new DeptProgramPaymentOption();
                tempPaymentOption.DPPO_DeptProgramMappingID = deptProgramMappingID;
                tempPaymentOption.DPPO_PaymentOptionID = paymentOptionID;
                tempPaymentOption.DPPO_IsDeleted = false;
                tempPaymentOption.DPPO_CreatedOn = DateTime.Now;
                tempPaymentOption.DPPO_CreatedByID = currentUserId;
                selectedPaymentOptionList.Add(tempPaymentOption);
            }

            List<DeptProgramPaymentOption> mapPaymentoptionList = _ClientDBContext.DeptProgramPaymentOptions.Where(cond => cond.DPPO_DeptProgramMappingID == deptProgramMappingID && !cond.DPPO_IsDeleted).ToList();
            List<DeptProgramPaymentOption> paymentOptionsToDelete = mapPaymentoptionList.Where(x => !selectedPaymentOptionList.Any(cnd => cnd.DPPO_PaymentOptionID == x.DPPO_PaymentOptionID)).ToList();
            List<DeptProgramPaymentOption> paymentOptionsToInsert = selectedPaymentOptionList.Where(y => !mapPaymentoptionList.Any(cd => cd.DPPO_PaymentOptionID == y.DPPO_PaymentOptionID)).ToList();

            //To delete already saved Payment Options
            paymentOptionsToDelete.ForEach(cond =>
            {
                cond.DPPO_IsDeleted = true;
                cond.DPPO_ModifiedByID = currentUserId;
                cond.DPPO_ModifiedOn = DateTime.Now;
            });

            //To insert Payment Options
            paymentOptionsToInsert.ForEach(con =>
            {
                _ClientDBContext.DeptProgramPaymentOptions.AddObject(con);
            });

            //Get list of selected file extensions
            foreach (var fileExtensionID in fileExtensions)
            {
                tempFileExtension = new DeptProgramRestrictedFileExtension();
                tempFileExtension.DPRFE_DeptProgramMappingID = deptProgramMappingID;
                tempFileExtension.DPRFE_FileExtensionID = fileExtensionID;
                tempFileExtension.DPRFE_IsDeleted = false;
                tempFileExtension.DPRFE_CreatedOn = DateTime.Now;
                tempFileExtension.DPRFE_CreatedByID = currentUserId;
                selectedFileExtensionList.Add(tempFileExtension);
            }

            List<DeptProgramRestrictedFileExtension> mappedFileExtensionList = _ClientDBContext.DeptProgramRestrictedFileExtensions.Where(cond => cond.DPRFE_DeptProgramMappingID == deptProgramMappingID && !cond.DPRFE_IsDeleted).ToList();
            List<DeptProgramRestrictedFileExtension> fileExtensionsToDelete = mappedFileExtensionList.Where(x => !selectedFileExtensionList.Any(cnd => cnd.DPRFE_FileExtensionID == x.DPRFE_FileExtensionID)).ToList();
            List<DeptProgramRestrictedFileExtension> fileExtensionsToInsert = selectedFileExtensionList.Where(y => !mappedFileExtensionList.Any(cd => cd.DPRFE_FileExtensionID == y.DPRFE_FileExtensionID)).ToList();

            //To delete already saved Payment Options
            fileExtensionsToDelete.ForEach(cond =>
            {
                cond.DPRFE_IsDeleted = true;
                cond.DPRFE_ModifiedByID = currentUserId;
                cond.DPRFE_ModifiedOn = DateTime.Now;
            });

            //To insert Payment Options
            fileExtensionsToInsert.ForEach(con =>
            {
                _ClientDBContext.DeptProgramRestrictedFileExtensions.AddObject(con);
            });

            if (_ClientDBContext.SaveChanges() > 0)
            {
                //Call for bkg hierarchy node
                if (IsCallFromBkgHierarchySetup)
                {
                    var lkpBkgHierarchyNodeExemptedTypes = _ClientDBContext.lkpBkgHierarchyNodeExemptedTypes.Where(cond => cond.HNET_ID == hierarchyNodeExemptedType && !cond.HNET_IsDeleted).First();

                    InsertExemptedHierarchyNode(deptProgramMappingID, lkpBkgHierarchyNodeExemptedTypes.HNET_Code, currentUserId);
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// To get child nodes with Permission
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public ObjectResult<GetChildNodesWithPermission> GetChildNodesWithPermission(Int32 deptProgramMappingID, Int32? currentUserId)
        {
            return _ClientDBContext.GetChildNodesWithPermission(deptProgramMappingID, currentUserId);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// To get Package Price Adjustments
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private List<PriceContract> GetPackagePriceAdjustments(Int32 ID)
        {
            return ClientDBContext.DeptProgramPackagePriceAdjustments.Include("PriceAdjustment").Where(x => x.DPPAC_DeptProgramPackageSubscriptionID == ID
                                                                                                && x.DPPAC_IsDeleted == false && x.PriceAdjustment.IsDeleted == false)
                                                                                                .Select(x => new PriceContract
                                                                                                {
                                                                                                    ID = x.DPPAC_ID,
                                                                                                    PriceAdjustmentID = x.DPPAC_PriceAdjustmentID,
                                                                                                    PriceAdjustmentLabel = x.PriceAdjustment.Label,
                                                                                                    PriceAdjustmentValue = x.DPPAC_Price
                                                                                                }).ToList();
        }

        /// <summary>
        /// To get Category Price Adjustments
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private List<PriceContract> GetCategoryPriceAdjustments(Int32 ID)
        {
            return ClientDBContext.DeptProgramPackageCategoryPriceAdjustments.Include("PriceAdjustment").Where(x => x.DPPCAC_DeptProgramPackageCategoryPriceID == ID
                                                                                                && x.DPPCAC_IsDeleted == false && x.PriceAdjustment.IsDeleted == false)
                                                                                                .Select(x => new PriceContract
                                                                                                {
                                                                                                    ID = x.DPPCAC_ID,
                                                                                                    PriceAdjustmentID = x.DPPCAC_PriceAdjustmentID,
                                                                                                    PriceAdjustmentLabel = x.PriceAdjustment.Label,
                                                                                                    PriceAdjustmentValue = x.DPPCAC_Price
                                                                                                }).ToList();
        }

        /// <summary>
        /// To get Item Price Adjustments
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private List<PriceContract> GetItemPriceAdjustments(Int32 ID)
        {
            return ClientDBContext.DeptProgramPackageCatItemPriceAdjustments.Include("PriceAdjustment").Where(x => x.DPPCIAC_DeptProgramPackageCategoryItemPriceID == ID
                                                                                                && x.DPPCIAC_IsDeleted == false && x.PriceAdjustment.IsDeleted == false)
                                                                                                .Select(x => new PriceContract
                                                                                                {
                                                                                                    ID = x.DPPCIAC_ID,
                                                                                                    PriceAdjustmentID = x.DPPCIAC_PriceAdjustmentID,
                                                                                                    PriceAdjustmentLabel = x.PriceAdjustment.Label,
                                                                                                    PriceAdjustmentValue = x.DPPCIAC_Price
                                                                                                }).ToList();
        }

        /// <summary>
        /// To get Package Price
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private PriceContract GetPackagePrice(Int32 ID)
        {
            PriceContract priceContract = new PriceContract();
            var deptProgramPackageSubscription = ClientDBContext.DeptProgramPackageSubscriptions.FirstOrDefault(obj => obj.DPPS_ID == ID && obj.DPPS_IsDeleted == false);
            if (deptProgramPackageSubscription.IsNotNull())
            {
                if (deptProgramPackageSubscription.DPPS_Price.HasValue)
                {
                    priceContract.Price = deptProgramPackageSubscription.DPPS_Price.Value;
                }
                else
                {
                    priceContract.IsPriceNull = true;
                }
                if (deptProgramPackageSubscription.DPPS_TotalPrice.HasValue)
                {
                    priceContract.TotalPrice = deptProgramPackageSubscription.DPPS_TotalPrice.Value;
                }
                if (deptProgramPackageSubscription.DPPS_RushOrderAdditionalPrice.HasValue)
                {
                    priceContract.RushOrderAdditionalPrice = deptProgramPackageSubscription.DPPS_RushOrderAdditionalPrice.Value;
                }
            }
            else
            {
                priceContract.IsPriceNull = true;
            }
            return priceContract;
        }

        /// <summary>
        /// To get Category Price
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private PriceContract GetCategoryPrice(Int32 ID, Int32 ParentID = 0, Int32 MappingID = 0)
        {
            PriceContract priceContract = new PriceContract();
            Entity.ClientEntity.DeptProgramPackageCategoryPrice deptProgramPackageCategoryPrice = new DeptProgramPackageCategoryPrice();
            if (ID > AppConsts.NONE)
            {
                deptProgramPackageCategoryPrice = ClientDBContext.DeptProgramPackageCategoryPrices.FirstOrDefault(obj => obj.DPPCP_ID == ID && obj.DPPCP_IsDeleted == false);
            }
            else if (ParentID > AppConsts.NONE && MappingID > AppConsts.NONE)
            {
                deptProgramPackageCategoryPrice = ClientDBContext.DeptProgramPackageCategoryPrices.FirstOrDefault(obj => obj.DPPCP_DeptProgramPackageSubscriptionID == ParentID && obj.DPPCP_ComplianceCategoryID == MappingID && obj.DPPCP_IsDeleted == false);
            }

            if (deptProgramPackageCategoryPrice.IsNotNull())
            {
                if (deptProgramPackageCategoryPrice.DPPCP_Price.HasValue)
                {
                    priceContract.Price = deptProgramPackageCategoryPrice.DPPCP_Price.Value;
                }
                else
                {
                    priceContract.IsPriceNull = true;
                }
                if (deptProgramPackageCategoryPrice.DPPCP_TotalPrice.HasValue)
                {
                    priceContract.TotalPrice = deptProgramPackageCategoryPrice.DPPCP_TotalPrice.Value;
                }
                if (deptProgramPackageCategoryPrice.DPPCP_RushOrderAdditionalPrice.HasValue)
                {
                    priceContract.RushOrderAdditionalPrice = deptProgramPackageCategoryPrice.DPPCP_RushOrderAdditionalPrice.Value;
                }
            }
            else
            {
                priceContract.IsPriceNull = true;
            }
            return priceContract;
        }

        /// <summary>
        /// To get Item Price
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private PriceContract GetItemPrice(Int32 ID, Int32 ParentSubscriptionID = 0, Int32 ComplianceCatagoryID = 0, Int32 ItemID = 0)
        {
            PriceContract priceContract = new PriceContract();
            Entity.ClientEntity.DeptProgramPackageCategoryItemPrice DeptProgramPackageCategoryItemPrice = new DeptProgramPackageCategoryItemPrice();
            if (ID > AppConsts.NONE)
            {
                DeptProgramPackageCategoryItemPrice = ClientDBContext.DeptProgramPackageCategoryItemPrices.FirstOrDefault(obj => obj.DPPCIP_ID == ID && obj.DPPCIP_IsDeleted == false);
            }
            else if (ParentSubscriptionID > AppConsts.NONE && ComplianceCatagoryID > AppConsts.NONE)
            {
                //Int32 DPPCPID = ClientDBContext.DeptProgramPackageCategoryPrices.Where(obj => obj.DPPCP_DeptProgramPackageSubscriptionID == ParentSubscriptionID && obj.DPPCP_ComplianceCategoryID == ComplianceCatagoryID && obj.DPPCP_IsDeleted == false).Select(x => x.DPPCP_ID).FirstOrDefault();
                var data = ClientDBContext.DeptProgramPackageCategoryItemPrices.Include("DeptProgramPackageCategoryPrice").Where(obj => obj.DPPCIP_ComplianceItemID == ItemID && obj.DPPCIP_IsDeleted == false).ToList();
                DeptProgramPackageCategoryItemPrice = data.Where(x => x.DeptProgramPackageCategoryPrice.DPPCP_DeptProgramPackageSubscriptionID.ToString().Contains(ParentSubscriptionID.ToString()) && x.DeptProgramPackageCategoryPrice.DPPCP_ComplianceCategoryID.ToString().Contains(ComplianceCatagoryID.ToString())).FirstOrDefault();
            }

            if (DeptProgramPackageCategoryItemPrice.IsNotNull())
            {
                if (DeptProgramPackageCategoryItemPrice.DPPCIP_Price.HasValue)
                {
                    priceContract.Price = DeptProgramPackageCategoryItemPrice.DPPCIP_Price.Value;
                }
                else
                {
                    priceContract.IsPriceNull = true;
                }
                if (DeptProgramPackageCategoryItemPrice.DPPCIP_TotalPrice.HasValue)
                {
                    priceContract.TotalPrice = DeptProgramPackageCategoryItemPrice.DPPCIP_TotalPrice.Value;
                }
                if (DeptProgramPackageCategoryItemPrice.DPPCIP_RushOrderAdditionalPrice.HasValue)
                {
                    priceContract.RushOrderAdditionalPrice = DeptProgramPackageCategoryItemPrice.DPPCIP_RushOrderAdditionalPrice.Value;
                }
            }
            else
            {
                priceContract.IsPriceNull = true;
            }
            return priceContract;
        }

        /// <summary>
        /// To get Package Total Price
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        /*private PriceContract GetPackageTotalPrice(Int32 ID)
        {
            PriceContract priceContract = new PriceContract();
            var deptProgramPackageSubscription = ClientDBContext.DeptProgramPackageSubscriptions.FirstOrDefault(obj => obj.DPPS_ID == ID && obj.DPPS_IsDeleted == false);
            if (deptProgramPackageSubscription.IsNotNull() && deptProgramPackageSubscription.DPPS_TotalPrice.HasValue)
            {
                priceContract.TotalPrice = deptProgramPackageSubscription.DPPS_TotalPrice.Value;
            }
            return priceContract;
        }

        /// <summary>
        /// To get Category Total Price
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private PriceContract GetCategoryTotalPrice(Int32 ID)
        {
            PriceContract priceContract = new PriceContract();
            var deptProgramPackageCategoryPrice = ClientDBContext.DeptProgramPackageCategoryPrices.FirstOrDefault(obj => obj.DPPCP_ID == ID && obj.DPPCP_IsDeleted == false);
            if (deptProgramPackageCategoryPrice.IsNotNull() && deptProgramPackageCategoryPrice.DPPCP_TotalPrice.HasValue)
            {
                priceContract.TotalPrice = deptProgramPackageCategoryPrice.DPPCP_TotalPrice.Value;
            }
            return priceContract;
        }

        /// <summary>
        /// To get Item Total Price
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private PriceContract GetItemTotalPrice(Int32 ID)
        {
            PriceContract priceContract = new PriceContract();
            var DeptProgramPackageCategoryItemPrice = ClientDBContext.DeptProgramPackageCategoryItemPrices.FirstOrDefault(obj => obj.DPPCIP_ID == ID && obj.DPPCIP_IsDeleted == false);
            if (DeptProgramPackageCategoryItemPrice.IsNotNull() && DeptProgramPackageCategoryItemPrice.DPPCIP_TotalPrice.HasValue)
            {
                priceContract.TotalPrice = DeptProgramPackageCategoryItemPrice.DPPCIP_TotalPrice.Value;
            }
            return priceContract;
        } */

        /// <summary>
        /// To get Package Total Price
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="deptProgramPackagePriceAdjustmentID"></param>
        /// <returns></returns>
        private Decimal GetPackageTotalPriceAdjustments(Int32 ID, Int32 deptProgramPackagePriceAdjustmentID = 0)
        {
            Decimal priceAdjustments = 0;
            var deptProgramPackagePriceAdjustmentList = ClientDBContext.DeptProgramPackagePriceAdjustments.Where(obj => obj.DPPAC_DeptProgramPackageSubscriptionID == ID && obj.DPPAC_ID != deptProgramPackagePriceAdjustmentID && obj.DPPAC_IsDeleted == false);
            if (deptProgramPackagePriceAdjustmentList.IsNotNull() && deptProgramPackagePriceAdjustmentList.Any())
            {
                foreach (var item in deptProgramPackagePriceAdjustmentList)
                {
                    priceAdjustments += item.DPPAC_Price;
                }
                return priceAdjustments;
            }
            return 0;
        }

        /// <summary>
        /// To get Category Total Price
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private Decimal GetCategoryTotalPriceAdjustments(Int32 ID, Int32 deptProgramPackageCategoryPriceAdjustmentID = 0)
        {
            Decimal priceAdjustments = 0;
            var deptProgramPackageCategoryPriceAdjustmentList = ClientDBContext.DeptProgramPackageCategoryPriceAdjustments.Where(obj => obj.DPPCAC_DeptProgramPackageCategoryPriceID == ID && obj.DPPCAC_ID != deptProgramPackageCategoryPriceAdjustmentID && obj.DPPCAC_IsDeleted == false);
            if (deptProgramPackageCategoryPriceAdjustmentList.IsNotNull() && deptProgramPackageCategoryPriceAdjustmentList.Any())
            {
                foreach (var item in deptProgramPackageCategoryPriceAdjustmentList)
                {
                    priceAdjustments += item.DPPCAC_Price;
                }
                return priceAdjustments;
            }
            return 0;
        }

        /// <summary>
        /// To get Item Total Price
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private Decimal GetItemTotalPriceAdjustments(Int32 ID, Int32 deptProgramPackageCatItemPriceAdjustmentID = 0)
        {
            Decimal priceAdjustments = 0;
            var deptProgramPackageCatItemPriceAdjustmentList = ClientDBContext.DeptProgramPackageCatItemPriceAdjustments.Where(obj => obj.DPPCIAC_DeptProgramPackageCategoryItemPriceID == ID && obj.DPPCIAC_ID != deptProgramPackageCatItemPriceAdjustmentID && obj.DPPCIAC_IsDeleted == false);
            if (deptProgramPackageCatItemPriceAdjustmentList.IsNotNull() && deptProgramPackageCatItemPriceAdjustmentList.Any())
            {
                foreach (var item in deptProgramPackageCatItemPriceAdjustmentList)
                {
                    priceAdjustments += item.DPPCIAC_Price;
                }
                return priceAdjustments;
            }
            return 0;
        }

        /// <summary>
        /// To check Package Price
        /// </summary>
        /// <param name="mappingID"></param>
        /// <returns></returns>
        private Boolean CheckPackagePrice(Int32 mappingID)
        {
            var deptProgramPackage = _ClientDBContext.DeptProgramPackages.FirstOrDefault(obj => obj.DPP_ID == mappingID && obj.DPP_IsDeleted == false);

            //if Price Model is not package
            if (deptProgramPackage.IsNotNull()
                && deptProgramPackage.lkpPriceModel.IsNotNull()
                && deptProgramPackage.lkpPriceModel.Code != PriceModel.Package.GetStringValue())
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// To check Category Price
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private Boolean CheckCategoryPrice(Int32 parentID)
        {
            var deptProgramPackageSubscription = _ClientDBContext.DeptProgramPackageSubscriptions.FirstOrDefault(obj => obj.DPPS_ID == parentID && obj.DPPS_IsDeleted == false);

            //if Price Model is not Category
            if (deptProgramPackageSubscription.IsNotNull()
                && deptProgramPackageSubscription.DeptProgramPackage.IsNotNull()
                && deptProgramPackageSubscription.DeptProgramPackage.lkpPriceModel.IsNotNull()
                && deptProgramPackageSubscription.DeptProgramPackage.lkpPriceModel.Code != PriceModel.Category.GetStringValue())
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// To check Item Price
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="parentSubscriptionID"></param>
        /// <returns></returns>
        private Boolean CheckItemPrice(Int32 parentID, Int32 parentSubscriptionID)
        {
            //Use ParentSubscriptionID for Item Price node screen if ParentID is 0
            if (parentID == AppConsts.NONE)
            {
                var deptProgramPackageSubscription = _ClientDBContext.DeptProgramPackageSubscriptions.FirstOrDefault(obj => obj.DPPS_ID == parentSubscriptionID && obj.DPPS_IsDeleted == false);

                //if Price Model is not Item, means Price Model is Package or Category
                if (deptProgramPackageSubscription.IsNotNull()
                    && deptProgramPackageSubscription.DeptProgramPackage.IsNotNull()
                    && deptProgramPackageSubscription.DeptProgramPackage.lkpPriceModel.IsNotNull()
                    && deptProgramPackageSubscription.DeptProgramPackage.lkpPriceModel.Code != PriceModel.Item.GetStringValue())
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                var deptProgramPackageCategoryPrice = _ClientDBContext.DeptProgramPackageCategoryPrices.FirstOrDefault(obj => obj.DPPCP_ID == parentID && obj.DPPCP_IsDeleted == false);

                //if Price Model is not Item
                if (deptProgramPackageCategoryPrice.IsNotNull()
                    && deptProgramPackageCategoryPrice.DeptProgramPackageSubscription.IsNotNull()
                    && deptProgramPackageCategoryPrice.DeptProgramPackageSubscription.DeptProgramPackage.IsNotNull()
                    && deptProgramPackageCategoryPrice.DeptProgramPackageSubscription.DeptProgramPackage.lkpPriceModel.IsNotNull()
                    && deptProgramPackageCategoryPrice.DeptProgramPackageSubscription.DeptProgramPackage.lkpPriceModel.Code != PriceModel.Item.GetStringValue())
                {
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// To show Message for Category
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private Boolean ShowMessageForCategory(Int32 parentID)
        {
            var deptProgramPackageSubscription = _ClientDBContext.DeptProgramPackageSubscriptions.FirstOrDefault(obj => obj.DPPS_ID == parentID && obj.DPPS_IsDeleted == false);

            //if Price Model is Package
            if (deptProgramPackageSubscription.IsNotNull()
                && deptProgramPackageSubscription.DeptProgramPackage.IsNotNull()
                && deptProgramPackageSubscription.DeptProgramPackage.lkpPriceModel.IsNotNull()
                && deptProgramPackageSubscription.DeptProgramPackage.lkpPriceModel.Code == PriceModel.Package.GetStringValue())
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// To show Message for Item
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="parentSubscriptionID"></param>
        /// <returns></returns>
        private Boolean ShowMessageForItem(Int32 parentID, Int32 parentSubscriptionID)
        {
            //Use ParentSubscriptionID for Item Price node screen if ParentID is 0
            if (parentID == AppConsts.NONE)
            {
                var deptProgramPackageSubscription = _ClientDBContext.DeptProgramPackageSubscriptions.FirstOrDefault(obj => obj.DPPS_ID == parentSubscriptionID && obj.DPPS_IsDeleted == false);

                //if Price Model is not Item, means Price Model is Package or Category
                if (deptProgramPackageSubscription.IsNotNull()
                    && deptProgramPackageSubscription.DeptProgramPackage.IsNotNull()
                    && deptProgramPackageSubscription.DeptProgramPackage.lkpPriceModel.IsNotNull()
                    && deptProgramPackageSubscription.DeptProgramPackage.lkpPriceModel.Code != PriceModel.Item.GetStringValue())
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                var deptProgramPackageCategoryPrice = _ClientDBContext.DeptProgramPackageCategoryPrices.FirstOrDefault(obj => obj.DPPCP_ID == parentID && obj.DPPCP_IsDeleted == false);

                //if Price Model is not Item, means Price Model is Package or Category
                if (deptProgramPackageCategoryPrice.IsNotNull()
                    && deptProgramPackageCategoryPrice.DeptProgramPackageSubscription.IsNotNull()
                    && deptProgramPackageCategoryPrice.DeptProgramPackageSubscription.DeptProgramPackage.IsNotNull()
                    && deptProgramPackageCategoryPrice.DeptProgramPackageSubscription.DeptProgramPackage.lkpPriceModel.IsNotNull()
                    && deptProgramPackageCategoryPrice.DeptProgramPackageSubscription.DeptProgramPackage.lkpPriceModel.Code != PriceModel.Item.GetStringValue())
                {
                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// To update Dept Program Package Object
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <param name="priceModelID"></param>
        /// <param name="priority"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        private void UpdateDeptProgramPackages(Int32 deptProgramPackageID, Int32 priceModelID, Int32 priority,
                                               Int32 currentUserId, List<Int32> lstSelectedOptionIds, Int32 paymentApprovalID)
        {
            var _currentDatetime = DateTime.Now;
            DeptProgramPackage deptProgramPackage = _ClientDBContext.DeptProgramPackages.Include("DeptProgramPackagePaymentOptions").FirstOrDefault(obj => obj.DPP_ID == deptProgramPackageID && obj.DPP_IsDeleted == false);
            if (deptProgramPackage.IsNotNull())
            {
                deptProgramPackage.DPP_PriceModelID = priceModelID;
                deptProgramPackage.DPP_Priority = priority;
                deptProgramPackage.DPP_PaymentApprovalID = paymentApprovalID;
                deptProgramPackage.DPP_ModifiedById = currentUserId;
                deptProgramPackage.DPP_ModifiedOn = DateTime.Now;

                #region Save/Update Package Level Payment Options

                List<DeptProgramPackagePaymentOption> mappedList = deptProgramPackage.DeptProgramPackagePaymentOptions.Where(cond => cond.DPPPO_IsDeleted == false).ToList();
                List<DeptProgramPackagePaymentOption> optionsToDelete = mappedList.Where(dpppo => !lstSelectedOptionIds.Any(optnId => optnId == dpppo.DPPPO_PaymentOptionID) && dpppo.DPPPO_IsDeleted == false).ToList();
                List<Int32> optionsToInsert = lstSelectedOptionIds.Where(optnId => !mappedList.Any(dpppo => dpppo.DPPPO_PaymentOptionID == optnId)).ToList();

                optionsToDelete.ForEach(cond =>
                {
                    cond.DPPPO_IsDeleted = true;
                    cond.DPPPO_ModifiedByID = currentUserId;
                    cond.DPPPO_ModifiedOn = _currentDatetime;
                });

                for (int i = 0; i < optionsToInsert.Count; i++)
                {
                    _ClientDBContext.DeptProgramPackagePaymentOptions.AddObject
                        (
                        new DeptProgramPackagePaymentOption
                        {
                            DPPPO_DPPID = deptProgramPackage.DPP_ID,
                            DPPPO_PaymentOptionID = optionsToInsert[i],
                            DPPPO_IsDeleted = false,
                            DPPPO_CreatedByID = currentUserId,
                            DPPPO_CreatedOn = _currentDatetime
                        });
                }
                #endregion
            }
        }

        /// <summary>
        /// To delete and reset all price data
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        private void DeleteResetPriceData(Int32 deptProgramPackageID, Int32 currentUserId)
        {
            DeptProgramPackage deptProgramPackage = _ClientDBContext.DeptProgramPackages.FirstOrDefault(obj => obj.DPP_ID == deptProgramPackageID && obj.DPP_IsDeleted == false);

            #region Update all Package, Category and Item price tables

            if (deptProgramPackage.IsNotNull())
            {
                //Update DeptProgramPackageSubscription table to reset all prices
                deptProgramPackage.DeptProgramPackageSubscriptions.Where(a => a.DPPS_IsDeleted == false).ForEach(con =>
                {
                    con.DPPS_Price = null;
                    con.DPPS_TotalPrice = null;
                    con.DPPS_RushOrderAdditionalPrice = null;
                    con.DPPS_ModifiedByID = currentUserId;
                    con.DPPS_ModifiedOn = DateTime.Now;
                });

                //Update DeptProgramPackageCategoryPrice table
                deptProgramPackage.DeptProgramPackageSubscriptions.Where(a => a.DPPS_IsDeleted == false).ForEach(con => con.DeptProgramPackageCategoryPrices
                    .Where(b => b.DPPCP_IsDeleted == false)
                        .ForEach(x =>
                        {
                            x.DPPCP_IsDeleted = true;
                            x.DPPCP_ModifiedByID = currentUserId;
                            x.DPPCP_ModifiedOn = DateTime.Now;
                        }));

                //Update DeptProgramPackageCategoryItemPrice table
                deptProgramPackage.DeptProgramPackageSubscriptions.Where(a => a.DPPS_IsDeleted == false).ForEach(con => con.DeptProgramPackageCategoryPrices
                       .ForEach(x => x.DeptProgramPackageCategoryItemPrices
                           .Where(c => c.DPPCIP_IsDeleted == false)
                               .ForEach(y =>
                               {
                                   y.DPPCIP_IsDeleted = true;
                                   y.DPPCIP_ModifiedByID = currentUserId;
                                   y.DPPCIP_ModifiedOn = DateTime.Now;
                               })));

                #endregion

                #region Update all Package, Category and Item Price Adjustment tables

                //Update DeptProgramPackagePriceAdjustment table
                deptProgramPackage.DeptProgramPackageSubscriptions.Where(a => a.DPPS_IsDeleted == false).ForEach(con => con.DeptProgramPackagePriceAdjustments
                    .Where(b => b.DPPAC_IsDeleted == false)
                        .ForEach(x =>
                        {
                            x.DPPAC_IsDeleted = true;
                            x.DPPAC_ModifiedByID = currentUserId;
                            x.DPPAC_ModifiedOn = DateTime.Now;
                        }));

                //Update DeptProgramPackageCategoryPriceAdjustment table
                deptProgramPackage.DeptProgramPackageSubscriptions.Where(a => a.DPPS_IsDeleted == false).ForEach(con => con.DeptProgramPackageCategoryPrices
                       .ForEach(x => x.DeptProgramPackageCategoryPriceAdjustments
                           .Where(c => c.DPPCAC_IsDeleted == false)
                               .ForEach(y =>
                               {
                                   y.DPPCAC_IsDeleted = true;
                                   y.DPPCAC_ModifiedByID = currentUserId;
                                   y.DPPCAC_ModifiedOn = DateTime.Now;
                               })));

                //Update DeptProgramPackageCatItemPriceAdjustment table
                deptProgramPackage.DeptProgramPackageSubscriptions.Where(a => a.DPPS_IsDeleted == false).ForEach(con => con.DeptProgramPackageCategoryPrices
                        .ForEach(x => x.DeptProgramPackageCategoryItemPrices
                               .ForEach(y => y.DeptProgramPackageCatItemPriceAdjustments
                                   .Where(d => d.DPPCIAC_IsDeleted == false)
                                       .ForEach(z =>
                                       {
                                           z.DPPCIAC_IsDeleted = true;
                                           z.DPPCIAC_ModifiedByID = currentUserId;
                                           z.DPPCIAC_ModifiedOn = DateTime.Now;
                                       }))));

            }
            #endregion
        }

        /// <summary>
        /// To save Package Price Detail data
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="price"></param>
        /// <param name="rushOrderAdditionalPrice"></param>
        /// <param name="selectedPriceAdjustmentID"></param>
        /// <param name="priceAdjustmentValue"></param>
        /// <param name="currentUserId"></param>
        /// <param name="treeNodeType"></param>
        private void SavePackagePrice(Int32 ID, Decimal price, Decimal? rushOrderAdditionalPrice, Int32 selectedPriceAdjustmentID, Decimal priceAdjustmentValue, Int32 currentUserId)
        {
            //Update DeptProgramPackageSubscription table
            DeptProgramPackageSubscription existingMapping = _ClientDBContext.DeptProgramPackageSubscriptions.FirstOrDefault(obj => obj.DPPS_ID == ID && obj.DPPS_IsDeleted == false);
            if (existingMapping.IsNotNull())
            {
                existingMapping.DPPS_Price = price;
                existingMapping.DPPS_TotalPrice = price + priceAdjustmentValue + GetPackageTotalPriceAdjustments(ID);
                existingMapping.DPPS_RushOrderAdditionalPrice = rushOrderAdditionalPrice;
                existingMapping.DPPS_ModifiedByID = currentUserId;
                existingMapping.DPPS_ModifiedOn = DateTime.Now;

                if (selectedPriceAdjustmentID > 0)
                {
                    //Insert in DeptProgramPackagePriceAdjustment table
                    DeptProgramPackagePriceAdjustment newMapping = new DeptProgramPackagePriceAdjustment();
                    newMapping.DPPAC_PriceAdjustmentID = selectedPriceAdjustmentID;
                    newMapping.DPPAC_DeptProgramPackageSubscriptionID = ID;
                    newMapping.DPPAC_Price = priceAdjustmentValue;
                    newMapping.DPPAC_IsDeleted = false;
                    newMapping.DPPAC_CreatedByID = currentUserId;
                    newMapping.DPPAC_CreatedOn = DateTime.Now;

                    _ClientDBContext.DeptProgramPackagePriceAdjustments.AddObject(newMapping);
                }
            }
        }

        /// <summary>
        /// To save Category Price Detail data
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="parentID"></param>
        /// <param name="mappingID"></param>
        /// <param name="price"></param>
        /// <param name="rushOrderAdditionalPrice"></param>
        /// <param name="selectedPriceAdjustmentID"></param>
        /// <param name="priceAdjustmentValue"></param>
        /// <param name="currentUserId"></param>
        private void SaveCategoryPrice(Int32 ID, Int32 parentID, Int32 mappingID, Decimal price, Decimal? rushOrderAdditionalPrice,
                                       Int32 selectedPriceAdjustmentID, Decimal priceAdjustmentValue, Int32 currentUserId, DeptProgramPackageCategoryPrice deptProgramPackageCategoryPrice)
        {
            //ID is 0 then insert records else update the records in DeptProgramPackageCategoryPrice table.
            if (ID == AppConsts.NONE)
            {
                //Insert in DeptProgramPackageCategoryPrice table
                //DeptProgramPackageCategoryPrice deptProgramPackageCategoryPrice = new DeptProgramPackageCategoryPrice();
                deptProgramPackageCategoryPrice.DPPCP_DeptProgramPackageSubscriptionID = parentID;
                deptProgramPackageCategoryPrice.DPPCP_ComplianceCategoryID = mappingID;
                deptProgramPackageCategoryPrice.DPPCP_Price = price;
                deptProgramPackageCategoryPrice.DPPCP_TotalPrice = price + priceAdjustmentValue;
                deptProgramPackageCategoryPrice.DPPCP_RushOrderAdditionalPrice = rushOrderAdditionalPrice;
                deptProgramPackageCategoryPrice.DPPCP_IsDeleted = false;
                deptProgramPackageCategoryPrice.DPPCP_CreatedByID = currentUserId;
                deptProgramPackageCategoryPrice.DPPCP_CreatedOn = DateTime.Now;

                if (selectedPriceAdjustmentID > 0)
                {
                    //Insert in DeptProgramPackageCategoryPriceAdjustment table
                    deptProgramPackageCategoryPrice.DeptProgramPackageCategoryPriceAdjustments.Add(
                                                    new DeptProgramPackageCategoryPriceAdjustment
                                                    {
                                                        DPPCAC_PriceAdjustmentID = selectedPriceAdjustmentID,
                                                        DPPCAC_Price = priceAdjustmentValue,
                                                        DPPCAC_IsDeleted = false,
                                                        DPPCAC_CreatedByID = currentUserId,
                                                        DPPCAC_CreatedOn = DateTime.Now
                                                    });
                }
                _ClientDBContext.DeptProgramPackageCategoryPrices.AddObject(deptProgramPackageCategoryPrice);
            }
            else
            {
                //Update DeptProgramPackageCategoryPrice table
                DeptProgramPackageCategoryPrice existingMapping = _ClientDBContext.DeptProgramPackageCategoryPrices.FirstOrDefault(obj => obj.DPPCP_ID == ID && obj.DPPCP_IsDeleted == false);
                if (existingMapping.IsNotNull())
                {
                    existingMapping.DPPCP_Price = price;
                    existingMapping.DPPCP_TotalPrice = price + priceAdjustmentValue + GetCategoryTotalPriceAdjustments(ID);
                    existingMapping.DPPCP_RushOrderAdditionalPrice = rushOrderAdditionalPrice;
                    existingMapping.DPPCP_ModifiedByID = currentUserId;
                    existingMapping.DPPCP_ModifiedOn = DateTime.Now;

                    if (selectedPriceAdjustmentID > 0)
                    {
                        //Insert in DeptProgramPackageCategoryPriceAdjustment table
                        DeptProgramPackageCategoryPriceAdjustment newMapping = new DeptProgramPackageCategoryPriceAdjustment();
                        newMapping.DPPCAC_PriceAdjustmentID = selectedPriceAdjustmentID;
                        newMapping.DPPCAC_DeptProgramPackageCategoryPriceID = ID;
                        newMapping.DPPCAC_Price = priceAdjustmentValue;
                        newMapping.DPPCAC_IsDeleted = false;
                        newMapping.DPPCAC_CreatedByID = currentUserId;
                        newMapping.DPPCAC_CreatedOn = DateTime.Now;

                        _ClientDBContext.DeptProgramPackageCategoryPriceAdjustments.AddObject(newMapping);
                    }
                }
            }
        }

        /// <summary>
        /// To save Item Price Detail data
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="parentID"></param>
        /// <param name="mappingID"></param>
        /// <param name="parentSubscriptionID"></param>
        /// <param name="complianceCategoryID"></param>
        /// <param name="price"></param>
        /// <param name="rushOrderAdditionalPrice"></param>
        /// <param name="selectedPriceAdjustmentID"></param>
        /// <param name="priceAdjustmentValue"></param>
        /// <param name="currentUserId"></param>
        private void SaveItemPrice(Int32 ID, Int32 parentID, Int32 mappingID, Int32 parentSubscriptionID, Int32 complianceCategoryID, Decimal price, Decimal? rushOrderAdditionalPrice,
                                   Int32 selectedPriceAdjustmentID, Decimal priceAdjustmentValue, Int32 currentUserId, DeptProgramPackageCategoryPrice deptProgramPackageCategoryPrice, DeptProgramPackageCategoryItemPrice deptProgramPackageCategoryItemPrice)
        {
            //If ID is 0 then insert records else update records in DeptProgramPackageCategoryItemPrice table.
            if (ID == AppConsts.NONE)
            {
                //If ID and parentID both are 0 then insert records in DeptProgramPackageCategoryPrice and DeptProgramPackageCategoryItemPrice tables.
                //And Use ParentSubscriptionID and complianceCategoryID for Item Price node screen
                if (parentID == AppConsts.NONE)
                {
                    //Insert in DeptProgramPackageCategoryPrice table

                    deptProgramPackageCategoryPrice.DPPCP_DeptProgramPackageSubscriptionID = parentSubscriptionID;
                    deptProgramPackageCategoryPrice.DPPCP_ComplianceCategoryID = complianceCategoryID;
                    deptProgramPackageCategoryPrice.DPPCP_IsDeleted = false;
                    deptProgramPackageCategoryPrice.DPPCP_CreatedByID = currentUserId;
                    deptProgramPackageCategoryPrice.DPPCP_CreatedOn = DateTime.Now;

                    //Insert in DeptProgramPackageCategoryItemPrice table
                    deptProgramPackageCategoryPrice.DeptProgramPackageCategoryItemPrices.Add(
                                                    new DeptProgramPackageCategoryItemPrice
                                                    {
                                                        DPPCIP_ComplianceItemID = mappingID,
                                                        DPPCIP_Price = price,
                                                        DPPCIP_TotalPrice = price + priceAdjustmentValue,
                                                        DPPCIP_RushOrderAdditionalPrice = rushOrderAdditionalPrice,
                                                        DPPCIP_IsDeleted = false,
                                                        DPPCIP_CreatedByID = currentUserId,
                                                        DPPCIP_CreatedOn = DateTime.Now
                                                    });
                    if (selectedPriceAdjustmentID > 0)
                    {
                        //Insert in DeptProgramPackageCatItemPriceAdjustment table
                        deptProgramPackageCategoryPrice.DeptProgramPackageCategoryItemPrices.ForEach
                                                                                            (x => x.DeptProgramPackageCatItemPriceAdjustments.Add(
                                                                                               new DeptProgramPackageCatItemPriceAdjustment
                                                                                               {
                                                                                                   DPPCIAC_PriceAdjustmentID = selectedPriceAdjustmentID,
                                                                                                   DPPCIAC_Price = priceAdjustmentValue,
                                                                                                   DPPCIAC_IsDeleted = false,
                                                                                                   DPPCIAC_CreatedByID = currentUserId,
                                                                                                   DPPCIAC_CreatedOn = DateTime.Now
                                                                                               }));
                    }
                    _ClientDBContext.DeptProgramPackageCategoryPrices.AddObject(deptProgramPackageCategoryPrice);
                }
                else
                {
                    //Insert in DeptProgramPackageCategoryItemPrice table
                    //DeptProgramPackageCategoryItemPrice deptProgramPackageCategoryItemPrice = new DeptProgramPackageCategoryItemPrice();
                    deptProgramPackageCategoryItemPrice.DPPCIP_DeptProgramPackageCategoryPriceID = parentID;
                    deptProgramPackageCategoryItemPrice.DPPCIP_ComplianceItemID = mappingID;
                    deptProgramPackageCategoryItemPrice.DPPCIP_Price = price;
                    deptProgramPackageCategoryItemPrice.DPPCIP_TotalPrice = price + priceAdjustmentValue;
                    deptProgramPackageCategoryItemPrice.DPPCIP_RushOrderAdditionalPrice = rushOrderAdditionalPrice;
                    deptProgramPackageCategoryItemPrice.DPPCIP_IsDeleted = false;
                    deptProgramPackageCategoryItemPrice.DPPCIP_CreatedByID = currentUserId;
                    deptProgramPackageCategoryItemPrice.DPPCIP_CreatedOn = DateTime.Now;

                    if (selectedPriceAdjustmentID > 0)
                    {
                        //Insert in DeptProgramPackageCatItemPriceAdjustment table
                        deptProgramPackageCategoryItemPrice.DeptProgramPackageCatItemPriceAdjustments.Add(
                                                        new DeptProgramPackageCatItemPriceAdjustment
                                                        {
                                                            DPPCIAC_PriceAdjustmentID = selectedPriceAdjustmentID,
                                                            DPPCIAC_Price = priceAdjustmentValue,
                                                            DPPCIAC_IsDeleted = false,
                                                            DPPCIAC_CreatedByID = currentUserId,
                                                            DPPCIAC_CreatedOn = DateTime.Now
                                                        });
                    }
                    _ClientDBContext.DeptProgramPackageCategoryItemPrices.AddObject(deptProgramPackageCategoryItemPrice);
                }
            }
            else
            {
                //Update DeptProgramPackageCategoryItemPrice table
                DeptProgramPackageCategoryItemPrice existingMapping = _ClientDBContext.DeptProgramPackageCategoryItemPrices.FirstOrDefault(obj => obj.DPPCIP_ID == ID && obj.DPPCIP_IsDeleted == false);
                if (existingMapping.IsNotNull())
                {
                    existingMapping.DPPCIP_Price = price;
                    existingMapping.DPPCIP_TotalPrice = price + priceAdjustmentValue + GetItemTotalPriceAdjustments(ID);
                    existingMapping.DPPCIP_RushOrderAdditionalPrice = rushOrderAdditionalPrice;
                    existingMapping.DPPCIP_ModifiedByID = currentUserId;
                    existingMapping.DPPCIP_ModifiedOn = DateTime.Now;

                    if (selectedPriceAdjustmentID > 0)
                    {
                        //Insert in DeptProgramPackageCatItemPriceAdjustment table
                        DeptProgramPackageCatItemPriceAdjustment newMapping = new DeptProgramPackageCatItemPriceAdjustment();
                        newMapping.DPPCIAC_PriceAdjustmentID = selectedPriceAdjustmentID;
                        newMapping.DPPCIAC_DeptProgramPackageCategoryItemPriceID = ID;
                        newMapping.DPPCIAC_Price = priceAdjustmentValue;
                        newMapping.DPPCIAC_IsDeleted = false;
                        newMapping.DPPCIAC_CreatedByID = currentUserId;
                        newMapping.DPPCIAC_CreatedOn = DateTime.Now;

                        _ClientDBContext.DeptProgramPackageCatItemPriceAdjustments.AddObject(newMapping);
                    }
                }
            }
        }

        /// <summary>
        /// To update Package Price Adjustment
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="priceID"></param>
        /// <param name="selectedPriceAdjustmentID"></param>
        /// <param name="priceAdjustmentValue"></param>
        /// <param name="currentUserId"></param>
        private void UpdatePackagePriceAdjustment(Int32 ID, Int32 priceID, Int32 selectedPriceAdjustmentID, Decimal priceAdjustmentValue, Int32 currentUserId)
        {
            if (selectedPriceAdjustmentID > 0)
            {
                DeptProgramPackagePriceAdjustment DeptProgramPackagePriceAdjustment = _ClientDBContext.DeptProgramPackagePriceAdjustments.FirstOrDefault(obj => obj.DPPAC_ID == ID && obj.DPPAC_IsDeleted == false);
                if (DeptProgramPackagePriceAdjustment.IsNotNull())
                {
                    DeptProgramPackagePriceAdjustment.DPPAC_PriceAdjustmentID = selectedPriceAdjustmentID;
                    DeptProgramPackagePriceAdjustment.DPPAC_Price = priceAdjustmentValue;
                    DeptProgramPackagePriceAdjustment.DPPAC_ModifiedByID = currentUserId;
                    DeptProgramPackagePriceAdjustment.DPPAC_ModifiedOn = DateTime.Now;
                }

                //Update Total Price
                DeptProgramPackageSubscription existingMapping = _ClientDBContext.DeptProgramPackageSubscriptions.FirstOrDefault(obj => obj.DPPS_ID == priceID && obj.DPPS_IsDeleted == false);
                if (existingMapping.IsNotNull())
                {
                    //existingMapping.DPPS_Price = price;
                    existingMapping.DPPS_TotalPrice = existingMapping.DPPS_Price + priceAdjustmentValue + GetPackageTotalPriceAdjustments(priceID, ID);
                    existingMapping.DPPS_ModifiedByID = currentUserId;
                    existingMapping.DPPS_ModifiedOn = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// To update Category Price Adjustment
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="priceID"></param>
        /// <param name="selectedPriceAdjustmentID"></param>
        /// <param name="priceAdjustmentValue"></param>
        /// <param name="currentUserId"></param>
        private void UpdateCategoryPriceAdjustment(Int32 ID, Int32 priceID, Int32 selectedPriceAdjustmentID, Decimal priceAdjustmentValue, Int32 currentUserId)
        {
            if (selectedPriceAdjustmentID > 0)
            {
                DeptProgramPackageCategoryPriceAdjustment deptProgramPackageCategoryPriceAdjustment = _ClientDBContext.DeptProgramPackageCategoryPriceAdjustments.FirstOrDefault(obj => obj.DPPCAC_ID == ID && obj.DPPCAC_IsDeleted == false);
                if (deptProgramPackageCategoryPriceAdjustment.IsNotNull())
                {
                    deptProgramPackageCategoryPriceAdjustment.DPPCAC_PriceAdjustmentID = selectedPriceAdjustmentID;
                    deptProgramPackageCategoryPriceAdjustment.DPPCAC_Price = priceAdjustmentValue;
                    deptProgramPackageCategoryPriceAdjustment.DPPCAC_ModifiedByID = currentUserId;
                    deptProgramPackageCategoryPriceAdjustment.DPPCAC_ModifiedOn = DateTime.Now;
                }

                //Update Total Price
                DeptProgramPackageCategoryPrice existingMapping = _ClientDBContext.DeptProgramPackageCategoryPrices.FirstOrDefault(obj => obj.DPPCP_ID == priceID && obj.DPPCP_IsDeleted == false);
                if (existingMapping.IsNotNull())
                {
                    //existingMapping.DPPCP_Price = price;
                    existingMapping.DPPCP_TotalPrice = existingMapping.DPPCP_Price + priceAdjustmentValue + GetCategoryTotalPriceAdjustments(priceID, ID);
                    existingMapping.DPPCP_ModifiedByID = currentUserId;
                    existingMapping.DPPCP_ModifiedOn = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// To update Item Price Adjustment
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="priceID"></param>
        /// <param name="selectedPriceAdjustmentID"></param>
        /// <param name="priceAdjustmentValue"></param>
        /// <param name="currentUserId"></param>
        private void UpdateItemPriceAdjustment(Int32 ID, Int32 priceID, Int32 selectedPriceAdjustmentID, Decimal priceAdjustmentValue, Int32 currentUserId)
        {
            if (selectedPriceAdjustmentID > 0)
            {
                DeptProgramPackageCatItemPriceAdjustment deptProgramPackageCatItemPriceAdjustment = _ClientDBContext.DeptProgramPackageCatItemPriceAdjustments.FirstOrDefault(obj => obj.DPPCIAC_ID == ID && obj.DPPCIAC_IsDeleted == false);
                if (deptProgramPackageCatItemPriceAdjustment.IsNotNull())
                {
                    deptProgramPackageCatItemPriceAdjustment.DPPCIAC_PriceAdjustmentID = selectedPriceAdjustmentID;
                    deptProgramPackageCatItemPriceAdjustment.DPPCIAC_Price = priceAdjustmentValue;
                    deptProgramPackageCatItemPriceAdjustment.DPPCIAC_ModifiedByID = currentUserId;
                    deptProgramPackageCatItemPriceAdjustment.DPPCIAC_ModifiedOn = DateTime.Now;
                }

                //Update Total Price
                DeptProgramPackageCategoryItemPrice existingMapping = _ClientDBContext.DeptProgramPackageCategoryItemPrices.FirstOrDefault(obj => obj.DPPCIP_ID == priceID && obj.DPPCIP_IsDeleted == false);
                if (existingMapping.IsNotNull())
                {
                    //existingMapping.DPPCIP_Price = price;
                    existingMapping.DPPCIP_TotalPrice = existingMapping.DPPCIP_Price + priceAdjustmentValue + GetItemTotalPriceAdjustments(priceID, ID);
                    existingMapping.DPPCIP_ModifiedByID = currentUserId;
                    existingMapping.DPPCIP_ModifiedOn = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// To check Package Price
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="priceID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        private Boolean DeletePackagePriceAdjustmentData(Int32 ID, Int32 priceID, Int32 currentUserId)
        {
            DeptProgramPackagePriceAdjustment deptProgramPackagePriceAdjustment = _ClientDBContext.DeptProgramPackagePriceAdjustments
                                                                                  .FirstOrDefault(obj => obj.DPPAC_ID == ID && obj.DPPAC_IsDeleted == false);
            if (deptProgramPackagePriceAdjustment != null)
            {
                deptProgramPackagePriceAdjustment.DPPAC_IsDeleted = true;
                deptProgramPackagePriceAdjustment.DPPAC_ModifiedOn = DateTime.Now;
                deptProgramPackagePriceAdjustment.DPPAC_ModifiedByID = currentUserId;

                //Update Total Price
                DeptProgramPackageSubscription existingMapping = _ClientDBContext.DeptProgramPackageSubscriptions.FirstOrDefault(obj => obj.DPPS_ID == priceID && obj.DPPS_IsDeleted == false);
                if (existingMapping.IsNotNull())
                {
                    existingMapping.DPPS_TotalPrice = existingMapping.DPPS_Price + GetPackageTotalPriceAdjustments(priceID, ID);
                    existingMapping.DPPS_ModifiedByID = currentUserId;
                    existingMapping.DPPS_ModifiedOn = DateTime.Now;
                }
                _ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// To check Category Price
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="priceID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        private Boolean DeleteCategoryPriceAdjustmentData(Int32 ID, Int32 priceID, Int32 currentUserId)
        {
            DeptProgramPackageCategoryPriceAdjustment deptProgramPackageCategoryPriceAdjustment = _ClientDBContext.DeptProgramPackageCategoryPriceAdjustments
                                                                                 .FirstOrDefault(obj => obj.DPPCAC_ID == ID && obj.DPPCAC_IsDeleted == false);
            //Delete record in deptProgramPackageCategoryPriceAdjustment table
            if (deptProgramPackageCategoryPriceAdjustment.IsNotNull())
            {
                deptProgramPackageCategoryPriceAdjustment.DPPCAC_IsDeleted = true;
                deptProgramPackageCategoryPriceAdjustment.DPPCAC_ModifiedOn = DateTime.Now;
                deptProgramPackageCategoryPriceAdjustment.DPPCAC_ModifiedByID = currentUserId;

                //Update Total Price
                DeptProgramPackageCategoryPrice existingMapping = _ClientDBContext.DeptProgramPackageCategoryPrices.FirstOrDefault(obj => obj.DPPCP_ID == priceID && obj.DPPCP_IsDeleted == false);
                if (existingMapping.IsNotNull())
                {
                    //existingMapping.DPPCP_Price = price;
                    existingMapping.DPPCP_TotalPrice = existingMapping.DPPCP_Price + GetCategoryTotalPriceAdjustments(priceID, ID);
                    existingMapping.DPPCP_ModifiedByID = currentUserId;
                    existingMapping.DPPCP_ModifiedOn = DateTime.Now;
                }
                _ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// To check Item Price
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="priceID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        private Boolean DeleteItemPriceAdjustmentData(Int32 ID, Int32 priceID, Int32 currentUserId)
        {
            DeptProgramPackageCatItemPriceAdjustment deptProgramPackageCatItemPriceAdjustment = _ClientDBContext.DeptProgramPackageCatItemPriceAdjustments
                                                                                  .FirstOrDefault(obj => obj.DPPCIAC_ID == ID && obj.DPPCIAC_IsDeleted == false);
            //Delete record in deptProgramPackageCatItemPriceAdjustment
            if (deptProgramPackageCatItemPriceAdjustment != null)
            {
                deptProgramPackageCatItemPriceAdjustment.DPPCIAC_IsDeleted = true;
                deptProgramPackageCatItemPriceAdjustment.DPPCIAC_ModifiedOn = DateTime.Now;
                deptProgramPackageCatItemPriceAdjustment.DPPCIAC_ModifiedByID = currentUserId;

                //Update Total Price
                DeptProgramPackageCategoryItemPrice existingMapping = _ClientDBContext.DeptProgramPackageCategoryItemPrices.FirstOrDefault(obj => obj.DPPCIP_ID == priceID && obj.DPPCIP_IsDeleted == false);
                if (existingMapping.IsNotNull())
                {
                    //existingMapping.DPPCIP_Price = price;
                    existingMapping.DPPCIP_TotalPrice = existingMapping.DPPCIP_Price + GetItemTotalPriceAdjustments(priceID, ID);
                    existingMapping.DPPCIP_ModifiedByID = currentUserId;
                    existingMapping.DPPCIP_ModifiedOn = DateTime.Now;
                }
                _ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }




        #endregion

        #endregion

        #region Price Adjustment

        /// <summary>
        /// Method to return all price adjustment.
        /// </summary>
        /// <returns>IQueryable</returns>
        public IQueryable<PriceAdjustment> GetAllPriceAdjustment()
        {
            return _ClientDBContext.PriceAdjustments.Where(cond => !cond.IsDeleted);
        }

        /// <summary>
        /// Get the Price Adjustment by priceAdjustmentId
        /// </summary>
        /// <param name="priceAdjustmentId">priceAdjustmentId</param>
        /// <returns>PriceAdjustment</returns>
        public PriceAdjustment GetPriceAdjustmentById(Int32 priceAdjustmentId)
        {
            return _ClientDBContext.PriceAdjustments.Where(cond => cond.PriceAdjustmentID == priceAdjustmentId && !cond.IsDeleted).FirstOrDefault();
        }

        /// <summary>
        /// Save PriceAdjustment
        /// </summary>
        /// <param name="priceAdjustment">priceAdjustment</param>
        /// <returns>Boolean</returns>
        public Boolean SavePriceAdjustment(PriceAdjustment priceAdjustment)
        {
            _ClientDBContext.PriceAdjustments.AddObject(priceAdjustment);
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Update PriceAdjustment
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean UpdatePriceAdjustment()
        {
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Check Price Adjustment Mapping
        /// </summary>
        /// <param name="priceAdjustmentId">priceAdjustmentId</param>
        /// <returns>Boolean</returns>
        public Boolean IsPriceAdjustmentMapped(Int32 priceAdjustmentId)
        {
            var isMappedWithDeptProgramPackageCategory = _ClientDBContext.DeptProgramPackageCategoryPriceAdjustments.Where(cond => cond.DPPCAC_PriceAdjustmentID == priceAdjustmentId && !cond.DPPCAC_IsDeleted).FirstOrDefault();
            var isMappedWithDeptProgramPackageCatItem = _ClientDBContext.DeptProgramPackageCatItemPriceAdjustments.Where(cond => cond.DPPCIAC_PriceAdjustmentID == priceAdjustmentId && !cond.DPPCIAC_IsDeleted).FirstOrDefault();
            var isMappedWithDeptProgramPackagePriceAdjustment = _ClientDBContext.DeptProgramPackagePriceAdjustments.Where(cond => cond.DPPAC_PriceAdjustmentID == priceAdjustmentId && !cond.DPPAC_IsDeleted).FirstOrDefault();
            if (!isMappedWithDeptProgramPackageCategory.IsNullOrEmpty() || !isMappedWithDeptProgramPackageCatItem.IsNullOrEmpty() || !isMappedWithDeptProgramPackagePriceAdjustment.IsNullOrEmpty())
                return true;
            return false;
        }

        #endregion

        #region Institution Node Type

        public IQueryable<InstitutionNodeType> GetAllInstitutionNodeType()
        {
            return _ClientDBContext.InstitutionNodeTypes.Where(cond => !cond.INT_IsDeleted);
        }
        /// <summary>
        /// Save InstitutionNodeType
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean SaveInstitutionNodeType(InstitutionNodeType institutionNodeType)
        {
            _ClientDBContext.InstitutionNodeTypes.AddObject(institutionNodeType);
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Update InstitutionNodeType
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean UpdateInstitutionNodeType()
        {
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }


        /// <summary>
        /// Get InstitutionNodeType based on the InstitutionNodeTypeId
        /// </summary>
        /// <returns>InstitutionNodeType</returns>
        public InstitutionNodeType GetInstitutionNodeTypeById(Int32 institutionNodeTypeId)
        {
            return _ClientDBContext.InstitutionNodeTypes.Where(cond => cond.INT_ID == institutionNodeTypeId && !cond.INT_IsDeleted).FirstOrDefault();
        }

        /// <summary>
        /// Check InstitutionNodeType Mapping
        /// </summary>
        /// <param name="priceAdjustmentId">InstitutionNodeTypeId</param>
        /// <returns>Boolean</returns>
        public Boolean IsInstitutionNodeTypeMapped(Int32 institutionNodeTypeId)
        {
            List<InstitutionNode> institutionNodeTypes = _ClientDBContext.InstitutionNodes.Where(cond => cond.IN_NodeTypeID == institutionNodeTypeId && !cond.IN_IsDeleted).ToList();
            if (institutionNodeTypes.Count > 0)
                return true;
            return false;
        }
        /// <summary>
        /// Get the Last code used for InstitutionNode  
        /// </summary>
        /// <returns>String</returns>
        public String GetLastInstitutionNodeTypeCode()
        {
            String code = String.Empty;

            if (_ClientDBContext.InstitutionNodeTypes.IsNotNull())
            {
                code = _ClientDBContext.InstitutionNodeTypes.OrderByDescending(cond => cond.INT_CreatedOn).FirstOrDefault().INT_Code;
            }

            return code;
        }

        /// <summary>
        /// To copy package structure and data
        /// </summary>
        /// <param name="compliancePackageID"></param>
        /// <param name="compliancePackageName"></param>
        /// <param name="currentUserId"></param>
        public void CopyPackageStructure(Int32 compliancePackageID, String compliancePackageName, Int32 currentUserId, Boolean updateExistingSub, Int32 srcNodeId, Int32 trgtNodeId)
        {
            _ClientDBContext.CopyPackageStructure(compliancePackageID, compliancePackageName, currentUserId, updateExistingSub, srcNodeId, trgtNodeId);
        }

        public void CopyPackageStructureToMaster(Int32 compliancePackageID, String compliancePackageName, Int32 currentUserId, Int32 tenantID)
        {
            _ClientDBContext.CopyPackageStructureToMaster(compliancePackageID, compliancePackageName, currentUserId, tenantID);
        }

        /// <summary>
        /// To copy package structure to client i.e. Assign package to client
        /// </summary>
        /// <param name="compliancePackageID"></param>
        /// <param name="compliancePackageName"></param>
        /// <param name="currentUserId"></param>
        /// <param name="tenantID"></param>
        public void CopyPackageStructureToClient(Int32 compliancePackageID, String compliancePackageName, Int32 currentUserId, Int32 tenantID)
        {
            _ClientDBContext.CopyPackageStructureToClient(compliancePackageID, compliancePackageName, currentUserId, tenantID);
        }

        #endregion

        #region Manage User Group
        /// <summary>
        /// Method to return all User Group
        /// </summary>
        /// <returns>IQueryable</returns>
        public IQueryable<UserGroup> GetAllUserGroup()
        {
            return _ClientDBContext.UserGroups.Where(cond => !cond.UG_IsDeleted && !cond.UG_IsArchived);
        }

        public String ArchiveUnArchiveUserGroups(List<Int32> lstUserGroupIds, bool isArchive)
        {
            if (!lstUserGroupIds.IsNullOrEmpty())
            {
                if (isArchive)
                {
                    foreach (var userGroupId in lstUserGroupIds)
                    {
                        var userGroup = _ClientDBContext.UserGroups.Where(cond => cond.UG_ID == userGroupId && !cond.UG_IsDeleted && !cond.UG_IsArchived).FirstOrDefault();
                        if (!userGroup.IsNullOrEmpty())
                        {
                            userGroup.UG_IsArchived = true;
                        }
                    }
                }
                else
                {
                    foreach (var userGroupId in lstUserGroupIds)
                    {
                        var userGroup = _ClientDBContext.UserGroups.Where(cond => cond.UG_ID == userGroupId && !cond.UG_IsDeleted && cond.UG_IsArchived).FirstOrDefault();
                        if (!userGroup.IsNullOrEmpty())
                        {
                            userGroup.UG_IsArchived = false;
                        }
                    }
                }
            }
            if (_ClientDBContext.SaveChanges() > 0)
                return "true";
            return "false";
        }

        public List<UserGroup> GetAllUserGroupWithPermission(Int32? currentUserId)
        {
            //return _ClientDBContext.UserGroups.Where(cond => !cond.UG_IsDeleted);
            List<UserGroup> lstUserGroup = new List<UserGroup>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@CurrentUserId", currentUserId)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetUserGroupList", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            UserGroup newUserGroup = new UserGroup();
                            newUserGroup.UG_ID = dr["UG_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["UG_ID"]);
                            newUserGroup.UG_Name = dr["UG_Name"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UG_Name"]);
                            newUserGroup.UG_Description = dr["UG_Description"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UG_Description"]);
                            newUserGroup.HierarchyNodeIdList = dr["HierarchyNode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyNode"]);
                            newUserGroup.HierarchyNodeLabelList = dr["HierarchyNodeLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyNodeLabel"]);
                            newUserGroup.UG_IsArchived = dr["UG_IsArchived"] == DBNull.Value ? false : Convert.ToBoolean(dr["UG_IsArchived"]);
                            lstUserGroup.Add(newUserGroup);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstUserGroup;
        }

        public List<UserGroup> GetAllUserGroupWithPermissionAll(Int32? currentUserId, String selectedHierarchyIds)
        {
            //return _ClientDBContext.UserGroups.Where(cond => !cond.UG_IsDeleted);
            List<UserGroup> lstUserGroup = new List<UserGroup>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@CurrentUserId", currentUserId),
                             new SqlParameter("@DPMIDs",selectedHierarchyIds)

                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetUserGroupListAll", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            UserGroup newUserGroup = new UserGroup();
                            newUserGroup.UG_ID = dr["UG_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["UG_ID"]);
                            newUserGroup.UG_Name = dr["UG_Name"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UG_Name"]);
                            newUserGroup.UG_Description = dr["UG_Description"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UG_Description"]);
                            newUserGroup.HierarchyNodeIdList = dr["HierarchyNode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyNode"]);
                            newUserGroup.HierarchyNodeLabelList = dr["HierarchyNodeLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyNodeLabel"]);
                            newUserGroup.UG_IsArchived = dr["UG_IsArchived"] == DBNull.Value ? false : Convert.ToBoolean(dr["UG_IsArchived"]);
                            lstUserGroup.Add(newUserGroup);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstUserGroup;
        }

        /// <summary>
        /// Method to save user group
        /// </summary>
        /// <param name="userGroup"></param>
        /// <returns>Boolean</returns>
        public Boolean SaveUserGroup(UserGroup userGroup)
        {
            _ClientDBContext.UserGroups.AddObject(userGroup);
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }
        /// <summary>
        /// Method to Update the user group
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean UpdateUserGroup()
        {
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }
        /// <summary>
        /// Method to get User group based on usergroupid
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns>UserGroup</returns>
        public UserGroup GetUserGroupById(Int32 userGroupId)
        {
            return _ClientDBContext.UserGroups.Where(cond => cond.UG_ID == userGroupId && !cond.UG_IsDeleted).FirstOrDefault();
        }
        /// <summary>
        /// Method to check the mapping of userGroupId in ApplicantUserGroupMapping table
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns>Boolean</returns>
        public Boolean IsUserGroupMapped(Int32 userGroupId)
        {
            List<ApplicantUserGroupMapping> applicantUserGroupMapping = _ClientDBContext.ApplicantUserGroupMappings.Where(cond => cond.AUGM_UserGroupID == userGroupId && !cond.AUGM_IsDeleted).ToList();
            if (applicantUserGroupMapping.Count > 0)
                return true;
            return false;
        }
        #endregion

        #region Map USer Hierarchy Permission

        public IQueryable<vwHierarchyPermission> GetHierarchyPermissionList(Int32 hierarchyId)
        {
            return _ClientDBContext.vwHierarchyPermissions.Where(cond => cond.HierarchyID == hierarchyId);
        }


        public List<ComplaincePackageDetails> GetPermittedPackagesByUserID(Int32? orgUserId)
        {
            return _ClientDBContext.GetPermittedPackagesByUserID(orgUserId).ToList();
        }

        public IQueryable<lkpPermission> GetPermissionList()
        {
            return _ClientDBContext.lkpPermissions.Where(cond => cond.PER_IsDeleted == false);
        }

        public Boolean SaveHierarchyPermission(HierarchyPermission hierarchyPermission, List<String> lstHierarchyPermissionTypeCode)
        {
            foreach (String hierarchyPermissionTypeCode in lstHierarchyPermissionTypeCode)
            {
                HierarchyPermission newhierarchyPermission = new HierarchyPermission();
                newhierarchyPermission.HP_HierarchyPermissionTypeID = _ClientDBContext.lkpHierarchyPermissionTypes.Where(cond => cond.HPT_Code.Equals(hierarchyPermissionTypeCode)).FirstOrDefault().HPT_ID;
                newhierarchyPermission.HP_CreatedBy = hierarchyPermission.HP_CreatedBy;
                newhierarchyPermission.HP_CreatedOn = hierarchyPermission.HP_CreatedOn;
                newhierarchyPermission.HP_HierarchyID = hierarchyPermission.HP_HierarchyID;
                newhierarchyPermission.HP_OrganizationUserID = hierarchyPermission.HP_OrganizationUserID;
                newhierarchyPermission.HP_PermissionID = hierarchyPermission.HP_PermissionID;
                if (hierarchyPermissionTypeCode.Equals(HierarchyPermissionTypes.COMPLIANCE.GetStringValue()))
                {
                    newhierarchyPermission.HP_ProfilePermissionID = hierarchyPermission.HP_ProfilePermissionID;
                    newhierarchyPermission.HP_VerificationPermissionID = hierarchyPermission.HP_VerificationPermissionID;
                    newhierarchyPermission.HP_OrderQueuePermissionID = hierarchyPermission.HP_OrderQueuePermissionID;
                    newhierarchyPermission.HP_PackagePermissionID = hierarchyPermission.HP_PackagePermissionID; //UAT 2834
                }
                newhierarchyPermission.HP_IsDeleted = false;
                _ClientDBContext.HierarchyPermissions.AddObject(newhierarchyPermission);
            }
            _ClientDBContext.SaveChanges();
            return true;
        }

        public HierarchyPermission GetHierarchyPermissionByID(Int32 hierarchyPermissionID)
        {
            return _ClientDBContext.HierarchyPermissions.Where(cond => cond.HP_ID == hierarchyPermissionID && cond.HP_IsDeleted == false).FirstOrDefault();


        }

        //public Boolean UpdateHierarchyPermission(HierarchyPermission hierarchyPermission, List<String> lstHierarchyPermissionTypeCode, Int32 hierarchyPermissionID)
        public Boolean UpdateHierarchyPermission()
        {
            //String complianceHierarchypermissionTypeCode = HierarchyPermissionTypes.COMPLIANCE.GetStringValue();
            //List<HierarchyPermission> lstHierarchyPermission = new List<HierarchyPermission>();
            //HierarchyPermission existingHierarchyPermission = GetHierarchyPermissionByID(hierarchyPermissionID);
            //lstHierarchyPermission.Add(existingHierarchyPermission);
            //if (lstHierarchyPermissionTypeCode.Count > AppConsts.ONE && !existingHierarchyPermission.IsNullOrEmpty())
            //{
            //    HierarchyPermission existingHierarchyPermissionOfOtherType = _ClientDBContext.HierarchyPermissions
            //                                                                .Where(cond => cond.HP_HierarchyPermissionTypeID != existingHierarchyPermission.HP_HierarchyPermissionTypeID
            //                                                                && cond.HP_OrganizationUserID == existingHierarchyPermission.HP_OrganizationUserID
            //                                                                && cond.HP_HierarchyID == hierarchyPermission.HP_HierarchyID
            //                                                                && cond.HP_IsDeleted == false
            //                                                                ).FirstOrDefault();
            //    if (!existingHierarchyPermissionOfOtherType.IsNullOrEmpty())
            //    {
            //        lstHierarchyPermission.Add(existingHierarchyPermissionOfOtherType);
            //    }
            //}
            //foreach (HierarchyPermission hierarchyPermissionToUpdate in lstHierarchyPermission)
            //{
            //    hierarchyPermissionToUpdate.HP_PermissionID = hierarchyPermission.HP_PermissionID;
            //    hierarchyPermissionToUpdate.HP_ModifiedBy = hierarchyPermission.HP_ModifiedBy;
            //    hierarchyPermissionToUpdate.HP_ModifiedOn = hierarchyPermission.HP_ModifiedOn;
            //    if (hierarchyPermissionToUpdate.lkpHierarchyPermissionType.IsNotNull() &&
            //        hierarchyPermissionToUpdate.lkpHierarchyPermissionType.HPT_Code.Equals(HierarchyPermissionTypes.COMPLIANCE.GetStringValue()))
            //    {
            //        hierarchyPermissionToUpdate.HP_ProfilePermissionID = hierarchyPermission.HP_ProfilePermissionID;
            //        hierarchyPermissionToUpdate.HP_VerificationPermissionID = hierarchyPermission.HP_VerificationPermissionID;
            //        hierarchyPermissionToUpdate.HP_OrderQueuePermissionID = hierarchyPermission.HP_OrderQueuePermissionID;
            //    }
            //}

            if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean DeleteHierarchyPermission()
        {
            if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// UAT 2834 
        /// </summary>
        /// <param name="orgUserId"></param>
        /// <returns></returns>
        public List<ComplianceCategoryDetails> GetPermittedCategoriesByUserID(Int32? orgUserId)
        {
            return _ClientDBContext.GetPermittedCategoriesByUserID(orgUserId).ToList();
        }

        #endregion

        #region ManageComplianceAttributeGroup
        public IQueryable<ComplianceAttributeGroup> GetAllComplianceAttributeGroup()
        {
            return _ClientDBContext.ComplianceAttributeGroups.Where(obj => !obj.CAG_IsDeleted);
        }
        /// <summary>
        /// Method to save Attribute Group
        /// </summary>
        /// <param name="userGroup"></param>
        /// <returns>Boolean</returns>
        public Boolean SaveAttributeGroup(ComplianceAttributeGroup attributeGroup)
        {
            _ClientDBContext.ComplianceAttributeGroups.AddObject(attributeGroup);
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Method to Update the Attribute Group
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean UpdateAttributeGroup()
        {
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }
        /// <summary>
        /// Method to get Attribute Group based on attributeGroupId
        /// </summary>
        /// <param name="attributeGroupId"></param>
        /// <returns>ComplianceAttributeGroup</returns>
        public ComplianceAttributeGroup GetAttributeGroupById(Int32 attributeGroupId)
        {
            return _ClientDBContext.ComplianceAttributeGroups.Where(cond => cond.CAG_ID == attributeGroupId && !cond.CAG_IsDeleted).FirstOrDefault();
        }
        /// <summary>
        /// Method to check the mapping of userGroupId in ApplicantUserGroupMapping table
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <returns>Boolean</returns>
        public Boolean IsAttributeGroupMapped(Int32 attributeGroupId)
        {
            List<ComplianceAttribute> attributeMapping = _ClientDBContext.ComplianceAttributes.Where(cond => cond.ComplianceAttributeGroupID == attributeGroupId && !cond.IsDeleted && cond.IsActive == true).ToList();
            if (attributeMapping.Count > 0)
                return true;
            return false;
        }
        #endregion

        #region ApplicantDataAuditHistory
        List<ApplicantDataAuditHistory> IComplianceSetupRepository.GetApplicantDataAuditHistory(CustomPagingArgsContract gridCustomPaging, SearchItemDataContract searchItemDataContract)
        {
            try
            {
                //UAT-2069 Passing more than one package or Category id to the sp.
                String packageIds = String.Empty;
                String categoryIds = String.Empty;
                Int32? ItemId = null;
                if (!searchItemDataContract.PackageIDs.IsNullOrWhiteSpace())
                {
                    packageIds = searchItemDataContract.PackageIDs;
                }
                if (!searchItemDataContract.CategoryIDs.IsNullOrWhiteSpace())
                {
                    categoryIds = searchItemDataContract.CategoryIDs;
                }
                if (searchItemDataContract.ItemID != AppConsts.NONE)
                {
                    ItemId = searchItemDataContract.ItemID;
                }
                string orderBy = null;
                string ordDirection = null;

                orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? orderBy : gridCustomPaging.SortExpression;
                ordDirection = gridCustomPaging.SortDirectionDescending == false ? String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? "desc" : null : "desc";

                List<ApplicantDataAuditHistory> applicantDataAuditHistoryList = _ClientDBContext.ApplicantDataAudit(searchItemDataContract.ApplicantFirstName, searchItemDataContract.ApplicantLastName, searchItemDataContract.AdminFirstName, searchItemDataContract.AdminLastName, searchItemDataContract.LoggedInUserTenantId, searchItemDataContract.LoggedInUserId, packageIds, categoryIds, ItemId, searchItemDataContract.FromDate,
                                                                                searchItemDataContract.ToDate, searchItemDataContract.FilterUserGroupID, orderBy, ordDirection, gridCustomPaging.CurrentPageIndex, gridCustomPaging.PageSize, searchItemDataContract.RoleNames).ToList();

                return applicantDataAuditHistoryList;
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
        #endregion

        #region Data Entry Help
        /// <summary>
        /// Gets Data entry help content by web site id and recordId
        /// </summary>
        /// <param name="websiteId"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public InstitutionWebPage GetDataEntryHelpContentByPackageId(Int32 tenantId, Int32? recordId, String recordType, String webSiteWebPageType)
        {
            //String websiteWebPageType = WebsiteWebPageType.DataEntryHelp.GetStringValue();
            if (recordId.IsNotNull())
            {
                return _ClientDBContext.InstitutionWebPages.Where(cond => cond.RecordID == recordId && cond.TenantID == tenantId && cond.lkpWebsiteWebPageType.Code == webSiteWebPageType && cond.lkpRecordType.Code == recordType && !cond.IsDeleted).FirstOrDefault();
            }
            else
            {
                return _ClientDBContext.InstitutionWebPages.Where(cond => cond.RecordID == null && cond.lkpRecordType.Code == recordType && cond.TenantID == tenantId && cond.lkpWebsiteWebPageType.Code == webSiteWebPageType && !cond.IsDeleted).FirstOrDefault();
            }
        }

        /// <summary>
        /// Get WebsiteWebPageType Id by code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Int32 GetWebsiteWebPageTypeIdByCode(String code)
        {
            return _ClientDBContext.lkpWebsiteWebPageTypes.Where(cond => cond.Code == code && !cond.IsDeleted).FirstOrDefault().WebsiteWebPageTypeID;
        }

        /// <summary>
        /// Get RecordType id by code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Int16 GetRecordTypeIdByCode(String code)
        {
            return _ClientDBContext.lkpRecordTypes.Where(cond => cond.Code == code && !cond.IsDeleted).FirstOrDefault().RecordTypeID;
        }

        /// <summary>
        /// Get existing package id list.
        /// </summary>
        /// <param name="websiteId"></param>
        /// <returns></returns>
        public List<Int32?> GetExistingPackageIdList(Int32 tenantId, String recordType, String webSiteWebPageType)
        {
            //String websiteWebPageType = WebsiteWebPageType.DataEntryHelp.GetStringValue();
            return _ClientDBContext.InstitutionWebPages.Where(cond => cond.TenantID == tenantId && cond.lkpWebsiteWebPageType.Code == webSiteWebPageType && !cond.IsDeleted && cond.lkpRecordType.Code == recordType).Select(x => x.RecordID).ToList();
        }

        public Boolean SaveInstitutionWebPage(InstitutionWebPage InstitutionWebPage)
        {
            _ClientDBContext.InstitutionWebPages.AddObject(InstitutionWebPage);
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }
        /// <summary>
        /// Update Institution Web Page
        /// </summary>
        /// <param name="webSiteWebConfigID"></param>
        /// <returns></returns>
        public Boolean UpdateInstitutionWebPage(InstitutionWebPage institutionWebPage)
        {
            if (_ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets Institution web page
        /// </summary>
        /// <param name="webSiteWebPageID"></param>
        /// <returns></returns>
        public InstitutionWebPage GetInstitutionWebPage(Int32 institutionWebPageID)
        {
            return _ClientDBContext.InstitutionWebPages.FirstOrDefault(cond => cond.InstitutionWebPageID == institutionWebPageID && !cond.IsDeleted);
        }
        /// <summary>
        /// Gets Data entry help content by web site id, recordId, recordTypeCode and websiteWebPageTypeCode
        /// </summary>
        /// <param name="webSiteID"></param>
        /// <param name="packageID"></param>
        /// <param name="recordTypeCode"></param>
        /// <param name="websiteWebPageTypeCode"></param>
        /// <returns></returns>
        public InstitutionWebPage GeDateHelpHtmlFromtWebSiteWebPage(Int32 tenantID, Int32 packageID, String recordTypeCode, String websiteWebPageTypeCode)
        {
            InstitutionWebPage institutionWebPage = null;

            if (tenantID.IsNotNull() && packageID.IsNotNull())
            {
                institutionWebPage = new InstitutionWebPage();
                institutionWebPage = _ClientDBContext.InstitutionWebPages.Where(webPage => webPage.TenantID == tenantID && webPage.IsActive == true && webPage.IsDeleted == false
                    && webPage.RecordID == packageID && webPage.lkpRecordType.Code == recordTypeCode
                    && webPage.lkpWebsiteWebPageType.Code == websiteWebPageTypeCode
                    ).FirstOrDefault();

                if (institutionWebPage.IsNull())
                {
                    institutionWebPage = _ClientDBContext.InstitutionWebPages.Where(webPage => webPage.TenantID == tenantID && webPage.IsActive == true && webPage.IsDeleted == false
                       && webPage.lkpWebsiteWebPageType.Code == websiteWebPageTypeCode && webPage.RecordID == null && webPage.lkpRecordType.Code == recordTypeCode
                        ).FirstOrDefault();
                }
                return institutionWebPage;
            }
            else
            {
                return institutionWebPage;
            }
        }
        #endregion

        #region Manage Package Subscription
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientCompliancePackageID"></param>
        /// <param name="clearContext">To check whether need to get fresh data.</param>
        /// <returns></returns>
        public CompliancePackage GetClientCompliancePackageByPackageID(Int32 clientCompliancePackageID, Boolean clearContext)
        {
            if (clearContext)
            {
                //clear current context of entity to get fresh dara from the db.
                ADB_LibertyUniversity_ReviewEntities.ClearContext();
            }
            return _ClientDBContext.CompliancePackages.FirstOrDefault(obj => obj.CompliancePackageID == clientCompliancePackageID && !obj.IsDeleted);
        }

        /// <summary>
        /// Gets the list of packages for the current applicant for data entry form
        /// </summary>
        /// <param name="organisationUserID"></param>
        /// <returns>List of subscribed packages</returns>
        public List<CompliancePackage> GetClientCompliancePackageByClient(Int32 organisationUserID)
        {
            var subscribedpackageIds = _ClientDBContext.PackageSubscriptions.
                    Where(ps => ps.OrganizationUserID == organisationUserID && !ps.IsDeleted && ps.CompliancePackage.IsActive &&
                    !ps.CompliancePackage.IsDeleted).Select(ps => ps.CompliancePackageID).ToList();

            List<CompliancePackage> packgeList = new List<CompliancePackage>();
            foreach (Int32 packageid in subscribedpackageIds)
            {
                CompliancePackage package = _ClientDBContext.CompliancePackages.FirstOrDefault(cp => cp.CompliancePackageID == packageid && !cp.IsDeleted);
                if (package.IsNotNull())
                    packgeList.Add(package);
                else
                    packgeList.Add(new CompliancePackage());
            }
            return packgeList;
        }
        #endregion

        #region DISCLOSURE FORM

        /// <summary>
        /// Method to save the disclosure document in system Document table.
        /// </summary>
        /// <param name="systemDocument">SystemDocument</param>
        /// <returns>Boolean</returns>
        public Boolean SaveDisclosureDocument(SystemDocument systemDocument)
        {
            _ClientDBContext.SystemDocuments.AddObject(systemDocument);
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Get the disclosure document on the basis of recordId, recordTypeId and websitePageTypeId
        /// </summary>
        /// <param name="recordId">recordId</param>
        /// <param name="recordTypeId">recordTypeId</param>
        /// <param name="websiteWebPageTypeId">websiteWebPageTypeId</param>
        /// <returns>SystemDocument</returns>
        public SystemDocument GetDisclosureDocument(Int32 systemDocumentId)
        {
            return _ClientDBContext.SystemDocuments.FirstOrDefault(cond => cond.SystemDocumentID == systemDocumentId && cond.IsDeleted == false);
        }

        /// <summary>
        /// Method that Save the changes  for update.
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean UpdateChanges()
        {
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Get Attached disclosure form list 
        /// </summary>
        /// <param name="websiteId"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public DataTable GetAttachedDisclosureFormList(Int32 tenantId, String recordType, String webSiteWebPageType, CustomPagingArgsContract queueAuditArgsContract)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                string orderBy = null;
                string ordDirection = null;
                orderBy = String.IsNullOrEmpty(queueAuditArgsContract.SortExpression) ? orderBy : queueAuditArgsContract.SortExpression;
                ordDirection = queueAuditArgsContract.SortDirectionDescending == false ? "asc" : "desc";

                SqlCommand command = new SqlCommand("usp_GetAttachedPackageDisclosureForm", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RecordTypeCode", recordType);
                command.Parameters.AddWithValue("@TenantID", tenantId);
                command.Parameters.AddWithValue("@WebSiteWebPageTypeCode", webSiteWebPageType);
                command.Parameters.AddWithValue("@OrderBy", orderBy);
                command.Parameters.AddWithValue("@OrderDirection", ordDirection);
                command.Parameters.AddWithValue("@PageIndex", queueAuditArgsContract.CurrentPageIndex);
                command.Parameters.AddWithValue("@PageSize", queueAuditArgsContract.PageSize);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }

            return new DataTable();
        }
        #endregion

        #region Manage Esigned  documents

        public ApplicantDocument SaveEsignedDocumentAsPdf(String PdfPath, String filename, Int32 fileSize, Int32 documentTypeId, Int32 currentLoggedInUserId, Int32 orgUserID)
        {
            if (!String.IsNullOrEmpty(PdfPath))
            {

                ApplicantDocument applicantDocument = new ApplicantDocument();
                applicantDocument.OrganizationUserID = currentLoggedInUserId;
                applicantDocument.DocumentPath = PdfPath;
                applicantDocument.DocumentType = documentTypeId;
                applicantDocument.FileName = filename;
                applicantDocument.Size = fileSize;
                //Will keep the document in deleted state until the order is completed.
                applicantDocument.IsDeleted = true;
                applicantDocument.CreatedOn = DateTime.Now;
                applicantDocument.CreatedByID = orgUserID;
                _ClientDBContext.ApplicantDocuments.AddObject(applicantDocument);
                _ClientDBContext.SaveChanges();

                return applicantDocument;
            }

            return null;
        }

        public Boolean DeleteDisclaimerPagePdf(Int32 applicantDocumentId, Int32 currentLoggedInUserId)
        {
            if (applicantDocumentId.IsNotNull())
            {
                ApplicantDocument applicantDocument = _ClientDBContext.ApplicantDocuments.Where(obj => obj.ApplicantDocumentID == applicantDocumentId && !obj.IsDeleted).FirstOrDefault();
                if (applicantDocument.IsNotNull())
                {
                    applicantDocument.IsDeleted = true;
                    applicantDocument.ModifiedByID = currentLoggedInUserId;
                    applicantDocument.ModifiedOn = DateTime.Now;
                    _ClientDBContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public ApplicantDocument GetESignedeDocument(Int32 applicantDocumentId, Int32 documentTypeId)
        {
            return _ClientDBContext.ApplicantDocuments.FirstOrDefault(cond => cond.ApplicantDocumentID == applicantDocumentId && cond.DocumentType == documentTypeId);
        }
        #endregion

        #region GET SURVEY MONKEY LINK
        /// <summary>
        /// Get Survey Monkey Link 
        /// </summary>
        public String GetSurveyMonkeyLink(Int32 tenantId, Int32 applicantId, String subEventCode, String packageName, String categoryName, String itemName, Int32 itemId, Int32 packageId, Int32 categoryId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetSurveyMonkeyLink", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantID", tenantId);
                command.Parameters.AddWithValue("@ApplicantID", applicantId);
                command.Parameters.AddWithValue("@SubEventCode", subEventCode);
                command.Parameters.AddWithValue("@PackageName", packageName);
                command.Parameters.AddWithValue("@CategoryName", categoryName);
                command.Parameters.AddWithValue("@ItemName", itemName);
                command.Parameters.AddWithValue("@ItemID", itemId);
                command.Parameters.AddWithValue("@PackageID", packageId);
                command.Parameters.AddWithValue("@CategoryID", categoryId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["SurveyMonkeyLink"].ToString();
                }
            }
            return String.Empty;
        }
        #endregion

        #region Node Deadline and Notifications

        /// <summary>
        /// To get node deadlines
        /// </summary>
        /// <returns></returns>
        public IQueryable<NodeDeadline> GetNodeDeadlines()
        {
            return _ClientDBContext.NodeDeadlines.Include("NodeNotificationMappings").Include("NodeNotificationMappings.NodeNotificationUserGroups")
                                   .Where(x => x.ND_IsDeleted == false);
        }

        public List<Int32> GetCheckedUsergroupIds(Int32 mappingId)
        {
            return _ClientDBContext.NodeNotificationUserGroups.Where(x => !x.NNUG_IsDeleted && x.NNUG_NodeNotificationMappingID == mappingId)
                .Select(x => x.NNUG_UserGroupID).ToList();
        }

        /// <summary>
        /// To get node deadline by ID
        /// </summary>
        /// <param name="NodeDeadlineID"></param>
        /// <returns></returns>
        public NodeDeadline GetNodeDeadlineByID(Int32 NodeDeadlineID)
        {
            return _ClientDBContext.NodeDeadlines.FirstOrDefault(x => x.ND_ID == NodeDeadlineID && x.ND_IsDeleted == false);
        }

        /// <summary>
        /// To save/insert NodeDeadline
        /// </summary>
        /// <param name="nodeDeadline"></param>
        /// <returns></returns>
        public Boolean SaveNodeDeadline(NodeDeadline nodeDeadline)
        {
            _ClientDBContext.NodeDeadlines.AddObject(nodeDeadline);
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// To update Node Deadline
        /// </summary>
        /// <param name="nodeDeadlineId"></param>
        /// <param name="nodeNotificationSettingsContract"></param>
        /// <param name="userGroupIDs"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public Boolean UpdateNodeDeadline(Int32 nodeDeadlineId, NodeNotificationSettingsContract nodeNotificationSettingsContract,
                                            List<Int32> userGroupIDs, Int32 currentUserId)
        {
            var modifiedOn = DateTime.Now;
            //Update NodeDeadline table
            NodeDeadline nodeDeadline = GetNodeDeadlineByID(nodeDeadlineId);
            nodeDeadline.ND_Name = nodeNotificationSettingsContract.NodeDeadlineName;
            nodeDeadline.ND_Description = nodeNotificationSettingsContract.NodeDeadlineDescription;
            nodeDeadline.ND_DeadlineDate = nodeNotificationSettingsContract.DeadlineDate;
            nodeDeadline.ND_ModifiedBy = currentUserId;
            nodeDeadline.ND_ModifiedOn = modifiedOn;

            //Update NodeNotificationMapping table
            //todo: check Deadline code
            var nodeNotificationMappings = nodeDeadline.NodeNotificationMappings.FirstOrDefault(x => !x.NNM_IsDeleted);
            nodeNotificationMappings.NNM_Frequency = nodeNotificationSettingsContract.Frequency;
            nodeNotificationMappings.NNM_DaysBefore = nodeNotificationSettingsContract.DaysBeforeDeadline;
            nodeNotificationMappings.NNM_ModifiedBy = currentUserId;
            nodeNotificationMappings.NNM_ModifiedOn = modifiedOn;

            //Update NodeNotificationUserGroup table
            NodeNotificationUserGroup tempNodeNotificationUserGroup = null;
            List<NodeNotificationUserGroup> selectedNodeNotificationUserGroupList = new List<NodeNotificationUserGroup>();

            foreach (var userGroupID in userGroupIDs)
            {
                tempNodeNotificationUserGroup = new NodeNotificationUserGroup();
                tempNodeNotificationUserGroup.NNUG_NodeNotificationMappingID = nodeNotificationMappings.NNM_ID;
                tempNodeNotificationUserGroup.NNUG_UserGroupID = userGroupID;
                tempNodeNotificationUserGroup.NNUG_IsDeleted = false;
                tempNodeNotificationUserGroup.NNUG_CreatedBy = currentUserId;
                tempNodeNotificationUserGroup.NNUG_CreatedOn = DateTime.Now;
                selectedNodeNotificationUserGroupList.Add(tempNodeNotificationUserGroup);
            }
            List<NodeNotificationUserGroup> mapNodeNotificationUserGroupList = _ClientDBContext.NodeNotificationUserGroups.Where(cond => cond.NNUG_NodeNotificationMappingID == nodeNotificationMappings.NNM_ID && cond.NNUG_IsDeleted == false).ToList();
            List<NodeNotificationUserGroup> nodeNotificationUserGroupToDelete = mapNodeNotificationUserGroupList.Where(x => !selectedNodeNotificationUserGroupList.Any(cnd => cnd.NNUG_UserGroupID == x.NNUG_UserGroupID && cnd.NNUG_NodeNotificationMappingID == x.NNUG_NodeNotificationMappingID)).ToList();
            List<NodeNotificationUserGroup> nodeNotificationUserGroupToInsert = selectedNodeNotificationUserGroupList.Where(y => !mapNodeNotificationUserGroupList.Any(cnd => cnd.NNUG_UserGroupID == y.NNUG_UserGroupID && cnd.NNUG_NodeNotificationMappingID == y.NNUG_NodeNotificationMappingID)).ToList();
            nodeNotificationUserGroupToDelete.ForEach(cond =>
            {
                cond.NNUG_IsDeleted = true;
                cond.NNUG_ModifiedBy = currentUserId;
                cond.NNUG_ModifiedOn = modifiedOn;
            });
            nodeNotificationUserGroupToInsert.ForEach(con =>
            {
                _ClientDBContext.NodeNotificationUserGroups.AddObject(con);
            });

            if (_ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// To delete Node Deadline
        /// </summary>
        /// <param name="nodeDeadline"></param>
        /// <returns></returns>
        public Boolean DeleteNodeDeadline()
        {
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// To save Nag Email Notifications
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="nagEmailNotificationTypeId"></param>
        /// <param name="hierarchyNodeID"></param>
        /// <param name="nagFrequency"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public Boolean SaveNagEmailNotifications(Int32 tenantId, Int16 nagEmailNotificationTypeId, Int32 hierarchyNodeID, Int32? nagFrequency, Int32 currentUserId, Boolean isActive)
        {
            var dateTimeNow = DateTime.Now;
            ComplianceDataRepository complianceDataRepository = new ComplianceDataRepository(tenantId);
            Int32 institutionRootNodeID = ((IComplianceDataRepository)complianceDataRepository).GetInstitutionDPMID();

            NodeNotificationMapping nodeNotificationMapping = new NodeNotificationMapping();
            if (hierarchyNodeID > 0)
            {
                //If root node record does not exist then insert else update
                NodeNotificationMapping _tempRootNodeNotificationMapping = _ClientDBContext.NodeNotificationMappings.FirstOrDefault(x => x.NNM_HierarchyNodeID == institutionRootNodeID
                                                && x.NNM_NodeNotificationTypeID == nagEmailNotificationTypeId && x.NNM_IsDeleted == false);
                //If root node record does not exist then insert
                if (_tempRootNodeNotificationMapping.IsNull())
                {
                    NodeNotificationMapping _rootNodeNotificationMappingToInsert = new NodeNotificationMapping();
                    _rootNodeNotificationMappingToInsert.NNM_HierarchyNodeID = institutionRootNodeID;
                    _rootNodeNotificationMappingToInsert.NNM_NodeNotificationTypeID = nagEmailNotificationTypeId;
                    _rootNodeNotificationMappingToInsert.NNM_Frequency = nagFrequency;
                    _rootNodeNotificationMappingToInsert.NNM_IsActive = isActive;
                    _rootNodeNotificationMappingToInsert.NNM_IsDeleted = false;
                    _rootNodeNotificationMappingToInsert.NNM_CreatedBy = currentUserId;
                    _rootNodeNotificationMappingToInsert.NNM_CreatedOn = dateTimeNow;
                    _ClientDBContext.NodeNotificationMappings.AddObject(_rootNodeNotificationMappingToInsert);
                }
                else //else update root node record if Hierarchy Node ID is institution root node ID
                {
                    if (institutionRootNodeID == hierarchyNodeID)
                    {
                        _tempRootNodeNotificationMapping.NNM_Frequency = nagFrequency;
                        _tempRootNodeNotificationMapping.NNM_IsActive = isActive;
                        _tempRootNodeNotificationMapping.NNM_ModifiedBy = currentUserId;
                        _tempRootNodeNotificationMapping.NNM_ModifiedOn = dateTimeNow;
                    }
                }
                //if other node then Insert/Update other node
                if (institutionRootNodeID != hierarchyNodeID)
                {
                    nodeNotificationMapping = _ClientDBContext.NodeNotificationMappings.FirstOrDefault(x => x.NNM_HierarchyNodeID == hierarchyNodeID
                                                    && x.NNM_NodeNotificationTypeID == nagEmailNotificationTypeId && x.NNM_IsDeleted == false);
                    //If node does not exist then insert
                    if (nodeNotificationMapping.IsNull())
                    {
                        NodeNotificationMapping _nodeNotificationMappingToInsert = new NodeNotificationMapping();
                        _nodeNotificationMappingToInsert.NNM_HierarchyNodeID = hierarchyNodeID;
                        _nodeNotificationMappingToInsert.NNM_NodeNotificationTypeID = nagEmailNotificationTypeId;
                        _nodeNotificationMappingToInsert.NNM_Frequency = nagFrequency;
                        _nodeNotificationMappingToInsert.NNM_IsActive = isActive;
                        _nodeNotificationMappingToInsert.NNM_IsDeleted = false;
                        _nodeNotificationMappingToInsert.NNM_CreatedBy = currentUserId;
                        _nodeNotificationMappingToInsert.NNM_CreatedOn = dateTimeNow;
                        _ClientDBContext.NodeNotificationMappings.AddObject(_nodeNotificationMappingToInsert);
                    }
                    else //else update the existing record
                    {
                        nodeNotificationMapping.NNM_Frequency = nagFrequency;
                        nodeNotificationMapping.NNM_IsActive = isActive;
                        nodeNotificationMapping.NNM_ModifiedBy = currentUserId;
                        nodeNotificationMapping.NNM_ModifiedOn = dateTimeNow;
                    }
                }
                if (_ClientDBContext.SaveChanges() > 0)
                    return true;
                return false;
            }

            return false;
        }

        /// <summary>
        /// To get Node Notification Mapping by NodeID 
        /// </summary>
        /// <param name="nagEmailNotificationTypeId"></param>
        /// <param name="hierarchyNodeID"></param>
        /// <returns></returns>
        public NodeNotificationMapping GetNodeNotificationMappingByNodeID(Int16 nagEmailNotificationTypeId, Int32 hierarchyNodeID)
        {
            return _ClientDBContext.NodeNotificationMappings.FirstOrDefault(x => x.NNM_NodeNotificationTypeID == nagEmailNotificationTypeId
                && x.NNM_HierarchyNodeID == hierarchyNodeID && x.NNM_IsDeleted == false);
        }

        #endregion

        #region Backround Package Detials
        /// <summary>
        /// Get tree data for backround package. 
        /// </summary>
        /// <param name="bkgPackageId">bkgPackageId</param>
        /// <returns></returns>
        public List<Entity.ClientEntity.GetBkgPackageDetailTree> GetBkgPackageDetailTree(Int32 bkgPackageId)
        {
            return _ClientDBContext.usp_GetBKGPackageDetailTreeList(bkgPackageId).ToList();
        }

        public String GetDeptProgMappingLabel(Int32 NodeId)
        {
            DeptProgramMapping deptProgMapping = _ClientDBContext.DeptProgramMappings.FirstOrDefault(x => x.DPM_ID == NodeId && !x.DPM_IsDeleted);
            if (deptProgMapping.IsNotNull())
            {
                return deptProgMapping.DPM_Label;
            }

            return String.Empty;
        }

        public String GetBkgpackageOfNode(Int32 BkgPackageHierarchyMappingID)
        {
            return _ClientDBContext.BkgPackageHierarchyMappings.Include("BackgroundPackage").FirstOrDefault(x => x.BPHM_ID == BkgPackageHierarchyMappingID && !x.BPHM_IsDeleted
                && !x.BackgroundPackage.BPA_IsDeleted).BackgroundPackage.BPA_Name;
        }
        public String GetCompliancePackafeOfNode(Int32 DeptProgramPackageId)
        {
            return _ClientDBContext.DeptProgramPackages.Include("CompliancePackage").FirstOrDefault(x => x.DPP_ID == DeptProgramPackageId && !x.DPP_IsDeleted
                && !x.CompliancePackage.IsDeleted).CompliancePackage.PackageName;

        }
        #endregion

        public SystemDocument GetServiceFormDocument(Int32 systemDocumentId)
        {
            return _ClientDBContext.SystemDocuments.FirstOrDefault(cond => cond.SystemDocumentID == systemDocumentId && cond.IsDeleted == false);
        }

        public List<CommunicationCCUsersList> GetCCusers(Int32 communicationSubEventId, Int32 tenantId, String hierarchyNodeID)
        {
            return _ClientDBContext.GetCommunicationCCusersDataWithNodePermission(communicationSubEventId, tenantId, hierarchyNodeID).ToList();
        }

        #region Disociation Work

        public String GetCategoryDissociationStatus(Int32 categoryId, Int32 packageId)
        {
            Boolean ifCategoryExistInOtherPackage = _ClientDBContext.CompliancePackageCategories.Any(cond => cond.CPC_PackageID != packageId && cond.CPC_CategoryID == categoryId && !cond.CPC_IsDeleted);
            if (ifCategoryExistInOtherPackage)
            {
                return AppConsts.DISSOCIATION_BUTTON_VISIBLE;
            }
            return AppConsts.DISSOCIATION_BUTTON_HIDDEN;
        }

        public String GetItemDissociationStatus(Int32 packageId, Int32 categoryId, Int32 itemId)
        {
            Boolean ifItemExistInOtherCategory = _ClientDBContext.ComplianceCategoryItems.Any(cond => cond.CCI_ItemID == itemId && cond.CCI_CategoryID != categoryId && !cond.CCI_IsDeleted);
            Boolean ifCategoryExistInOtherPackage = _ClientDBContext.CompliancePackageCategories.Any(cond => cond.CPC_PackageID != packageId && cond.CPC_CategoryID == categoryId && !cond.CPC_IsDeleted);
            if (ifItemExistInOtherCategory)
            {
                if (!ifCategoryExistInOtherPackage)
                {
                    return AppConsts.DISSOCIATION_BUTTON_VISIBLE;
                }
                else
                {
                    return AppConsts.DISSOCIATION_BUTTON_ALL;
                }
            }
            else
            {
                return AppConsts.DISSOCIATION_BUTTON_HIDDEN;
            }

        }

        public String GetAttributeDissociationStatus(Int32 packageId, Int32 categoryId, Int32 itemId, Int32 attrId)
        {
            Boolean ifAttributeExistInOtherItem = _ClientDBContext.ComplianceItemAttributes.Any(cond => cond.CIA_AttributeID == attrId && cond.CIA_ItemID != itemId && !cond.CIA_IsDeleted);
            Boolean ifCategoryExistInOtherPackage = _ClientDBContext.CompliancePackageCategories.Any(cond => cond.CPC_PackageID != packageId && cond.CPC_CategoryID == categoryId && !cond.CPC_IsDeleted);
            Boolean ifItemExistInOtherCategory = _ClientDBContext.ComplianceCategoryItems.Any(cond => cond.CCI_ItemID == itemId && cond.CCI_CategoryID != categoryId && !cond.CCI_IsDeleted);

            if (ifAttributeExistInOtherItem)
            {
                if (!ifItemExistInOtherCategory && !ifCategoryExistInOtherPackage)
                {
                    return AppConsts.DISSOCIATION_BUTTON_VISIBLE;
                }
                else
                {
                    return AppConsts.DISSOCIATION_BUTTON_ALL;
                }
            }
            else
            {
                return AppConsts.DISSOCIATION_BUTTON_HIDDEN;
            }
        }

        public Int32 DissociateCategory(Int32 tenantId, Int32 categoryId, String packageIDs, Int32 currentLoggedInUserId)
        {

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_DisassociateCategoryItems", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageIDs", packageIDs);
                command.Parameters.AddWithValue("@CategoryIDs", categoryId.ToString());
                command.Parameters.AddWithValue("@DissociationType", "Category");
                command.Parameters.AddWithValue("@UserID", currentLoggedInUserId);
                command.Parameters.AddWithValue("@TenantId", tenantId);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return (int)ds.Tables[0].Rows[0]["NewDataId"];
                }
            }
            return 0;
        }

        public Int32 DissociateItem(Int32 tenantId, Int32 packageId, String categoryIds, Int32 itemId, Int32 currentLoggedInUserId)
        {

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_DisassociateCategoryItems", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageIDs", packageId.ToString());
                command.Parameters.AddWithValue("@CategoryIDs", categoryIds);
                command.Parameters.AddWithValue("@ItemID", itemId);
                command.Parameters.AddWithValue("@DissociationType", "Item");
                command.Parameters.AddWithValue("@UserID", currentLoggedInUserId);
                command.Parameters.AddWithValue("@TenantId", tenantId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return (int)ds.Tables[0].Rows[0]["NewDataId"];
                }
            }
            return 0;
        }
        //UAT-4348

        public bool IsAllowedOverrideDate(Int32 ItemId)
        {
            bool IsAllowed = false;
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("AllowedOverrideDate", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ItemId", ItemId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    IsAllowed = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAllowed"]);
                }
            }
            return IsAllowed;
        }
        public Int32 DissociateAttribute(Int32 tenantId, Int32 packageId, Int32 categoryId, String itemIds, Int32 attrId, Int32 currentLoggedInUserId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_DisassociateAttribute", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageID", packageId);
                command.Parameters.AddWithValue("@CategoryID", categoryId);
                command.Parameters.AddWithValue("@ItemIDs", itemIds);
                command.Parameters.AddWithValue("@AttributeId", attrId);
                command.Parameters.AddWithValue("@UserID", currentLoggedInUserId);
                command.Parameters.AddWithValue("@TenantId", tenantId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return (int)ds.Tables[0].Rows[0]["NewAttributeId"];
                }
            }
            return 0;
        }

        /// <summary>
        /// UAT-2582: Getting Packages associated with cateogry 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        List<CompliancePackage> IComplianceSetupRepository.GetCompliancePackagesAssociatedtoCat(Int32 categoryID, Int32 CurrentPackageID)
        {
            return _ClientDBContext.CompliancePackageCategories.Where(con => con.CPC_CategoryID == categoryID && con.CPC_PackageID != CurrentPackageID && !con.CPC_IsDeleted && !con.CompliancePackage.IsDeleted && !con.ComplianceCategory.IsDeleted).Select(sel => sel.CompliancePackage).ToList();
        }

        /// <summary>
        /// UAT-2582: Getting Categories associated with Items 
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        List<ComplianceCategory> IComplianceSetupRepository.GetComplianceCategoriesAssociatedtoItem(Int32 itemId, Int32 currentCategoryID)
        {
            return _ClientDBContext.ComplianceCategoryItems.Where(con => con.CCI_ItemID == itemId && con.CCI_CategoryID != currentCategoryID && !con.CCI_IsDeleted && !con.ComplianceCategory.IsDeleted && !con.ComplianceItem.IsDeleted).Select(sel => sel.ComplianceCategory).ToList();
        }

        /// <summary>
        /// UAT-4267 :- Getting Items associated with Attributes
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="currentCategoryID"></param>
        /// <param name="currentAttributeID"></param>
        /// <returns></returns>
        List<ComplianceItem> IComplianceSetupRepository.GetComplianceItemsAssociatedtoAttributes(Int32 itemId, Int32 currentCategoryID, Int32 currentAttributeID)
        {

            return _ClientDBContext.ComplianceItemAttributes.Where(con => con.CIA_AttributeID == currentAttributeID && con.CIA_ItemID != itemId && !con.CIA_IsDeleted && !con.ComplianceAttribute.IsDeleted && !con.ComplianceItem.IsDeleted).Select(sel => sel.ComplianceItem).ToList();
        }
        #endregion


        #region Instruction Text
        public int GetItemAttributeMappingID(ComplianceItemAttribute itemAttributeMapping)
        {
            return _ClientDBContext.ComplianceItemAttributes.Where(cond => cond.CIA_AttributeID == itemAttributeMapping.CIA_AttributeID && cond.CIA_ItemID == itemAttributeMapping.CIA_ItemID && !cond.CIA_IsDeleted && cond.CIA_IsActive).Select(x => x.CIA_ID).FirstOrDefault();
        }

        public Boolean SaveInstructionText(ComplianceAttributeContract complianceAttributeContract, int loggedInUserId)
        {
            AttributeInstruction attributeInstruction = new AttributeInstruction();
            attributeInstruction.AI_AssignmentHierarchyID = complianceAttributeContract.AssignmentHierarchyID;
            attributeInstruction.AI_ComplianceItemAttributeID = complianceAttributeContract.ItemAttributeMappingID;
            attributeInstruction.AI_InstructionText = complianceAttributeContract.InstructionText;
            attributeInstruction.AI_IsDeleted = false;
            attributeInstruction.AI_CreatedBy = loggedInUserId;
            attributeInstruction.AI_CreatedOn = DateTime.UtcNow;
            _ClientDBContext.AttributeInstructions.AddObject(attributeInstruction);
            if (_ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        public Boolean UpdateInstructionText(ComplianceAttributeContract complianceAttributeContract, Int32 AttrID, Int32 ItemId, Int32 CategoryId, Int32 PackageId)
        {
            Int32 AI_ID = 0;
            String tmpAI_ID = GetAttributeInstructionID(PackageId, CategoryId, ItemId, AttrID, complianceAttributeContract.ItemAttributeMappingID.Value);
            if (!tmpAI_ID.IsNullOrEmpty())
            {
                AI_ID = Convert.ToInt32(tmpAI_ID);
            }
            if (AI_ID != AppConsts.NONE)
            {
                AttributeInstruction attributeInstruction = _ClientDBContext.AttributeInstructions.Where(cond => cond.AI_ID == AI_ID && cond.AI_IsDeleted == false).FirstOrDefault();

                if (attributeInstruction.IsNotNull())
                {
                    if (complianceAttributeContract.InstructionText == string.Empty)
                    {
                        attributeInstruction.AI_IsDeleted = true;
                    }
                    else
                    {
                        attributeInstruction.AI_InstructionText = complianceAttributeContract.InstructionText;
                    }
                    attributeInstruction.AI_ModifiedBy = complianceAttributeContract.loggedInUserId;
                    attributeInstruction.AI_ModifiedOn = DateTime.UtcNow;
                    if (_ClientDBContext.SaveChanges() > 0)
                        return true;
                }
            }
            else
            {
                //Case 2 : when User empty the textbox and save the record and just after it again enter some value in the textbox.
                complianceAttributeContract.AssignmentHierarchyID = GetAssignmentHierarchyID(PackageId, CategoryId, ItemId, AttrID);
                AttributeInstruction attributeInstruction = new AttributeInstruction();
                attributeInstruction.AI_AssignmentHierarchyID = complianceAttributeContract.AssignmentHierarchyID;
                attributeInstruction.AI_ComplianceItemAttributeID = complianceAttributeContract.ItemAttributeMappingID;
                attributeInstruction.AI_InstructionText = complianceAttributeContract.InstructionText;
                attributeInstruction.AI_IsDeleted = false;
                attributeInstruction.AI_CreatedBy = complianceAttributeContract.loggedInUserId.Value;
                attributeInstruction.AI_CreatedOn = DateTime.UtcNow;
                _ClientDBContext.AttributeInstructions.AddObject(attributeInstruction);
                if (_ClientDBContext.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private String GetAttributeInstructionID(Int32 packageId, Int32 categoryId, Int32 itemId, Int32 attributeId, Int32 CIA_Id)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetInstTextAndAsignHierarchyIdForAttribute", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageId", packageId);
                command.Parameters.AddWithValue("@CategoryId", categoryId);
                command.Parameters.AddWithValue("@ItemId", itemId);
                command.Parameters.AddWithValue("@AttributeId", attributeId);
                command.Parameters.AddWithValue("@ComplianceItemAttributeId", CIA_Id);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return Convert.ToString(ds.Tables[0].Rows[0]["AI_ID"]);
                }
            }
            return String.Empty;
        }

        public Int32 GetAssignmentHierarchyID(Int32 packageId, Int32 categoryId, Int32 itemId, Int32 attributeId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetInstTextAndAsignHierarchyIdForAttribute", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageId", packageId);
                command.Parameters.AddWithValue("@CategoryId", categoryId);
                command.Parameters.AddWithValue("@ItemId", itemId);
                command.Parameters.AddWithValue("@AttributeId", attributeId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0]["AssignmentHierarchyId"]);
                }
            }
            return 0;
        }

        public String GetAttributeInstructionText(ComplianceAttributeContract complianceAttributeContract, int AttrID, int ItemId, int CategoryId, int PackageId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetInstTextAndAsignHierarchyIdForAttribute", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageId", PackageId);
                command.Parameters.AddWithValue("@CategoryId", CategoryId);
                command.Parameters.AddWithValue("@ItemId", ItemId);
                command.Parameters.AddWithValue("@AttributeId", AttrID);
                command.Parameters.AddWithValue("@ComplianceItemAttributeId", complianceAttributeContract.ItemAttributeMappingID.Value);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToString(ds.Tables[0].Rows[0]["InstructionText"]).IsNullOrEmpty() ? String.Empty : Convert.ToString(ds.Tables[0].Rows[0]["InstructionText"]);
                }
            }
            return String.Empty;
        }


        /// <summary>
        /// Gets all the rows from AttributeInstruction
        /// </summary>
        /// <returns>List</returns>
        public List<AttributeInstruction> GetAllAttributeInstruction()
        {

            return ClientDBContext.AttributeInstructions.Where(x => x.AI_IsDeleted == false).ToList();
        }
        #endregion

        /// <summary>
        /// To Set Compliance Package Availability For Order
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <param name="currentUserId"></param>
        /// <param name="paID"></param>
        /// <returns></returns>
        public Boolean SetCompliancePkgAvailabilityForOrder(Int32 deptProgramPackageID, Int32 currentUserId, Int32 paID)
        {
            DeptProgramPackage deptProgPkg = _ClientDBContext.DeptProgramPackages.FirstOrDefault(x => x.DPP_ID == deptProgramPackageID && !x.DPP_IsDeleted);
            if (deptProgPkg.IsNotNull())
            {
                deptProgPkg.DPP_ModifiedById = currentUserId;
                deptProgPkg.DPP_ModifiedOn = DateTime.Now;
                deptProgPkg.DPP_PackageAvailabilityID = paID;

                if (_ClientDBContext.SaveChanges() > 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// To Get whether Compliance Package Available For Order or not
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <returns></returns>
        public Boolean IsCompliancePkgAvailableForOrder(Int32 deptProgramPackageID)
        {
            DeptProgramPackage deptProgPkg = _ClientDBContext.DeptProgramPackages.Include("lkpPackageAvailability").FirstOrDefault(x => x.DPP_ID == deptProgramPackageID && !x.DPP_IsDeleted);
            if (deptProgPkg.IsNotNull() && deptProgPkg.lkpPackageAvailability.IsNotNull())
            {
                String code = PackageAvailability.AVAILABLE_FOR_ORDER.GetStringValue();
                if (deptProgPkg.lkpPackageAvailability.PA_Code == code)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// To Get Package Availability
        /// </summary>
        /// <returns></returns>
        public List<lkpPackageAvailability> GetPackageAvailablity()
        {
            return _ClientDBContext.lkpPackageAvailabilities.Where(x => !x.PA_IsDeleted).ToList();
        }

        public Boolean IsAutoRenewInvoiceOrderForPackage(Int32 deptProgramPackageID)
        {
            DeptProgramPackage deptProgPkg = _ClientDBContext.DeptProgramPackages.FirstOrDefault(x => x.DPP_ID == deptProgramPackageID && !x.DPP_IsDeleted);
            if (deptProgPkg.IsNotNull())
            {
                if (deptProgPkg.DPP_IsAutoRenewInvoiceOrder.HasValue)
                    return deptProgPkg.DPP_IsAutoRenewInvoiceOrder.Value;
            }
            return false;
        }

        /// <summary>
        /// To Set whether to do Auto Renew Invoice Order For Package
        /// </summary>
        /// <param name="deptProgramPackageID"></param>
        /// <param name="currentUserId"></param>
        /// <param name="paID"></param>
        /// <returns></returns>
        public Boolean SetAutoRenewInvoiceOrderForPackage(Int32 deptProgramPackageID, Int32 currentUserId, Boolean IsAutoRenewInvoiceOrder)
        {
            DeptProgramPackage deptProgPkg = _ClientDBContext.DeptProgramPackages.FirstOrDefault(x => x.DPP_ID == deptProgramPackageID && !x.DPP_IsDeleted);
            if (deptProgPkg.IsNotNull())
            {
                deptProgPkg.DPP_ModifiedById = currentUserId;
                deptProgPkg.DPP_ModifiedOn = DateTime.Now;
                deptProgPkg.DPP_IsAutoRenewInvoiceOrder = IsAutoRenewInvoiceOrder;

                if (_ClientDBContext.SaveChanges() > 0)
                    return true;
            }
            return false;
        }

        public DataTable GetUserNodePermissionForVerificationAndProfile(Int32 orgUserID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[dbo].[usp_GetUserNodePermissionForVerificationAndProfile]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CurrentLoggedInUserId", orgUserID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        /// <summary>
        /// Method to save Archival Grace Period to DepatmentProgramMapping
        /// </summary>
        /// <param name="DPM_ID"></param>
        /// <param name="archivalGracePeriod"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public Boolean SaveArchivalGracePeriod(Int32 DPM_ID, Int32? archivalGracePeriod, Int32 currentUserId)
        {
            DeptProgramMapping deptProgramMapping = _ClientDBContext.DeptProgramMappings.Where(x => x.DPM_ID == DPM_ID).FirstOrDefault();
            if (deptProgramMapping.IsNotNull())
            {

                deptProgramMapping.DPM_ArchivalGracePeriod = archivalGracePeriod;
                deptProgramMapping.DPM_ModifiedByID = currentUserId;
                deptProgramMapping.DPM_ModifiedOn = DateTime.Now;

                if (_ClientDBContext.SaveChanges() > 0)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Method to get Effective Archival Grace Period to DepatmentProgramMapping
        /// </summary>
        /// <param name="DPM_ID"></param>
        /// <param name="archivalGracePeriod"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public Dictionary<String, Int32> GetEffectiveArchivalGracePeriod(Int32 DPM_ID, Int32 currentUserId)
        {
            List<DeptProgramMapping> lstDeptProgramMapping = _ClientDBContext.DeptProgramMappings.Where(x => !x.DPM_IsDeleted).ToList();
            //Int32 effectiveArchivalGracePeriod = AppConsts.MINUS_ONE;
            Int32? currentDPMId = DPM_ID;
            Int32? currentArchivalGracePeriod = AppConsts.MINUS_ONE;
            Int32? currentDPMParentID = AppConsts.MINUS_ONE;
            //Boolean needEffectiveArchival = false;
            Int32 counter = AppConsts.NONE;
            Dictionary<String, Int32> dicResult = new Dictionary<String, Int32>();

            if (lstDeptProgramMapping.IsNotNull() && lstDeptProgramMapping.Count > 0)
            {
                while (currentDPMId != null)
                {
                    counter++;
                    currentArchivalGracePeriod = lstDeptProgramMapping.Where(x => x.DPM_ID == currentDPMId).Select(x => x.DPM_ArchivalGracePeriod).FirstOrDefault();
                    currentDPMParentID = lstDeptProgramMapping.Where(x => x.DPM_ID == currentDPMId).Select(x => x.DPM_ParentNodeID).FirstOrDefault(); ;

                    if (currentArchivalGracePeriod.IsNotNull())
                    {
                        //effectiveArchivalGracePeriod = currentArchivalGracePeriod.Value;
                        dicResult.Add("EffectiveArchivalGracePeriod", currentArchivalGracePeriod.Value);
                        break;
                    }
                    if (currentDPMParentID.IsNotNull())
                    {
                        currentDPMId = currentDPMParentID.Value;
                    }
                    else
                    {
                        currentDPMId = null;
                    }
                }
            }
            if (counter == AppConsts.ONE)
                dicResult.Add("NeedEffectiveArchival", 0);//false
            else
                dicResult.Add("NeedEffectiveArchival", 1);//true
            return dicResult;
        }


        public Boolean SaveRecieptDocument(String pdfDocPath, String filename, Int32 fileSize, Int32 documentTypeID, Int32 currentLoggedInUserID, Int32 OrderID, Int32 orgUserID)
        {
            if (!String.IsNullOrEmpty(pdfDocPath))
            {
                ApplicantDocument applicantDocument = new ApplicantDocument();
                applicantDocument.OrganizationUserID = currentLoggedInUserID;
                applicantDocument.DocumentPath = pdfDocPath;
                applicantDocument.DocumentType = documentTypeID;
                applicantDocument.FileName = filename;
                applicantDocument.Size = fileSize;
                applicantDocument.IsDeleted = false;
                applicantDocument.CreatedOn = DateTime.Now;
                applicantDocument.CreatedByID = orgUserID;
                _ClientDBContext.ApplicantDocuments.AddObject(applicantDocument);
                if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
                {
                    Order _order = _ClientDBContext.Orders.Where(cond => cond.OrderID == OrderID && !cond.IsDeleted).FirstOrDefault();
                    //Update Order table 
                    if (_order.IsNotNull())
                    {
                        _order.ApplicantDocumentID = applicantDocument.ApplicantDocumentID;
                        _order.ModifiedByID = orgUserID;
                        _order.ModifiedOn = DateTime.Now;
                        _ClientDBContext.SaveChanges();
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }


        public Order GetOrderFromOrderID(int orderID)
        {
            return _ClientDBContext.Orders.Where(cond => cond.OrderID == orderID && !cond.IsDeleted).FirstOrDefault();
        }


        public ApplicantDocument GetRecieptDocumentDataForOrderID(int orderID)
        {
            Order order = _ClientDBContext.Orders.Where(cond => cond.OrderID == orderID && !cond.IsDeleted).FirstOrDefault();
            if (order.ApplicantDocumentID.IsNotNull())
            {
                return _ClientDBContext.ApplicantDocuments.Where(x => x.ApplicantDocumentID == order.ApplicantDocumentID && !x.IsDeleted).FirstOrDefault();
            }
            return new ApplicantDocument();
        }

        DataTable IComplianceSetupRepository.GetCommunicationCopySettingsOverride()
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[dbo].[usp_GetCommunicationNodeCopySettingData]", con);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        Boolean IComplianceSetupRepository.CheckIfCommunicationNodeSettingExistForSelectednode(Int32 hierarchyNodeId, Int32 orgUserId)
        {
            return _ClientDBContext.CommunicationNodeCopySettings.Any(cond => cond.CNCS_OrganizationUserID == orgUserId && cond.CNCS_HierarchyID == hierarchyNodeId
                                                                        && !cond.CNCS_IsDeleted);
        }

        Boolean IComplianceSetupRepository.SaveCommunicationNodeCopySetting(CommunicationNodeCopySetting communicationNodeCopySetting, List<INTSOF.UI.Contract.Templates.CommunicationSettingsSubEventsContract> communicationSettingsSubEventsContractList)
        {
            if (!communicationSettingsSubEventsContractList.IsNullOrEmpty())
            {
                communicationSettingsSubEventsContractList.ForEach(d => communicationNodeCopySetting.CommunicationNodeSubeventsCopySettings.Add(new CommunicationNodeSubeventsCopySetting
                {
                    CNSCS_CreatedBy = communicationNodeCopySetting.CNCS_CreatedBy,
                    CNSCS_CreatedOn = DateTime.Now,
                    CNSCS_IsDeleted = false,
                    CNSCS_SettingID = d.NodeCopySettingID,
                    CNSCS_SubEventID = d.CommunciationSubEventID
                }));
            }
            _ClientDBContext.CommunicationNodeCopySettings.AddObject(communicationNodeCopySetting);

            if (_ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        Boolean IComplianceSetupRepository.UpdateCommunicationNodeCopySetting(Int32 communicationNodeCopySettingID, Int32 nodeCopySettingID, Int32 currentLoggedInUserID, List<INTSOF.UI.Contract.Templates.CommunicationSettingsSubEventsContract> communicationSettingsSubEventsContractList)
        {
            CommunicationNodeCopySetting communicationNodeCopySetting = _ClientDBContext.CommunicationNodeCopySettings.Where(cond => cond.CNCS_ID == communicationNodeCopySettingID
                                                                                                                       && !cond.CNCS_IsDeleted).FirstOrDefault();
            if (communicationNodeCopySetting.IsNullOrEmpty())
            {
                return false;
            }
            var oldRecords = communicationNodeCopySetting.CommunicationNodeSubeventsCopySettings.ToList();
            if (!communicationSettingsSubEventsContractList.IsNullOrEmpty())
            {
                communicationSettingsSubEventsContractList.ForEach(d => communicationNodeCopySetting.CommunicationNodeSubeventsCopySettings.Add(new CommunicationNodeSubeventsCopySetting
                {
                    CNSCS_CreatedBy = communicationNodeCopySetting.CNCS_CreatedBy,
                    CNSCS_CreatedOn = DateTime.Now,
                    CNSCS_IsDeleted = false,
                    CNSCS_SettingID = d.NodeCopySettingID,
                    CNSCS_SubEventID = d.CommunciationSubEventID
                }));
            }

            communicationNodeCopySetting.CNCS_NodeCopySettingID = nodeCopySettingID;
            communicationNodeCopySetting.CNCS_ModifiedBy = currentLoggedInUserID;
            communicationNodeCopySetting.CNCS_ModifiedOn = DateTime.Now;

            oldRecords.ForEach(d =>
            {
                d.CNSCS_IsDeleted = true;
                d.CNSCS_ModifiedBy = currentLoggedInUserID;
                d.CNSCS_ModifiedOn = DateTime.Now;
            });
            if (ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        Boolean IComplianceSetupRepository.DeleteCommunicationNodeCopySetting(Int32 communicationNodeCopySettingID, Int32 currentLoggedInUserID)
        {
            CommunicationNodeCopySetting communicationNodeCopySetting = _ClientDBContext.CommunicationNodeCopySettings.Where(cond => cond.CNCS_ID == communicationNodeCopySettingID
                                                                                                                       && !cond.CNCS_IsDeleted).FirstOrDefault();
            if (communicationNodeCopySetting.IsNullOrEmpty())
            {
                return false;
            }

            communicationNodeCopySetting.CNCS_IsDeleted = true;
            communicationNodeCopySetting.CNCS_ModifiedBy = currentLoggedInUserID;
            communicationNodeCopySetting.CNCS_ModifiedOn = DateTime.Now;
            var communicationNodeSubeventsCopySettingsList = communicationNodeCopySetting.CommunicationNodeSubeventsCopySettings.ToList();
            communicationNodeSubeventsCopySettingsList.ForEach(d =>
            {
                d.CNSCS_IsDeleted = true;
                d.CNSCS_ModifiedBy = currentLoggedInUserID;
                d.CNSCS_ModifiedOn = DateTime.Now;
            });
            if (ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        String IComplianceSetupRepository.GetFormattedString(Int32 orgUserID, Boolean isOrgUserProfileID)
        {
            return _ClientDBContext.GetFormattedString(orgUserID, isOrgUserProfileID).FirstOrDefault();
        }

        #region UAT-1185 New Compliance Package Type
        public List<lkpCompliancePackageType> GetCompliancePackageTypes()
        {
            return _ClientDBContext.lkpCompliancePackageTypes.ToList();
        }
        #endregion

        #region UAT-1137: Remove student ability to enter data and preserve ability to see explanatory note and to submit exceptions
        public List<LargeContent> GetExplanatoryNotesForItems(List<Int32> objectIds, Int32 objectTypeID, Int32 contentTypeID)
        {
            return ClientDBContext.LargeContents.Where(obj => objectIds.Contains(obj.LC_ObjectID) && obj.LC_LargeContentTypeID == contentTypeID
                                                       && obj.LC_ObjectTypeID == objectTypeID && obj.LC_IsDeleted == false).ToList();
        }
        #endregion

        #region UAt-1209: As an application admin, I should be able to enter a date range for when a category should be compliance required/not required.
        /// <summary>
        /// get categories required for compliance action.
        /// </summary>
        /// <returns></returns>
        DataTable IComplianceSetupRepository.GetCategoriesRqdForComplianceAction()
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[dbo].[usp_GetCategoriesRqdForComplianceAction]", con);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();

        }

        /// <summary>
        /// Used for deleting old records from history table.
        /// </summary>
        /// <param name="cpc_Id"></param>
        void IComplianceSetupRepository.DeletePreviousComplianceRqdActionHistory(Int32 cpc_Id, Int32 currentUserId)
        {
            List<ComplianceRqdActionHistory> complianceRqdActionHistories = ClientDBContext.ComplianceRqdActionHistories.Where(cond => cond.CRAH_CPCID == cpc_Id && !cond.CRAH_IsDeleted).ToList();
            foreach (ComplianceRqdActionHistory complianceRqdActionHistory in complianceRqdActionHistories)
            {
                complianceRqdActionHistory.CRAH_IsDeleted = true;
                complianceRqdActionHistory.CRAH_ModifiedByID = currentUserId;
                complianceRqdActionHistory.CRAH_ModifiedOn = DateTime.Now;
            }
            ClientDBContext.SaveChanges();
        }
        #endregion

        /// <summary>
        /// Method is used to get Institution Configuration Detail
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        InstitutionConfigurationDetailsContract IComplianceSetupRepository.GetInstitutionConfigurationDetails(Int32 hierarchyNodeID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetInstitutionConfigurationDetailsData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@HierarchyID", hierarchyNodeID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                InstitutionConfigurationDetailsContract institutionConfigurationDetailsContract = new InstitutionConfigurationDetailsContract();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    institutionConfigurationDetailsContract.PackageDetailsList = ds.Tables[0].AsEnumerable().Select(col =>
                          new InstitutionConfigurationPackageDetails
                          {
                              PackageID = Convert.ToInt32(col["PackageID"]),
                              PackageName = Convert.ToString(col["PackageName"]),
                              PaymentMethods = Convert.ToString(col["PaymentMethods"]),
                              PackageType = Convert.ToString(col["PackageType"]),
                              IsCompliancePackage = Convert.ToBoolean(col["IsCompliancePackage"]),
                              IsParentPackage = Convert.ToBoolean(col["IsParentPackage"]),
                              Fee = col["Fee"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(col["Fee"]),
                              SubscriptionOption = col["SubscriptionOption"] == DBNull.Value ? String.Empty : Convert.ToString(col["SubscriptionOption"]),
                              PackageHierarchyID = Convert.ToInt32(col["PackageHierarchyID"]),
                          }).ToList();
                }
                else
                {
                    institutionConfigurationDetailsContract.PackageDetailsList = new List<InstitutionConfigurationPackageDetails>();
                }


                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > AppConsts.NONE)
                {
                    institutionConfigurationDetailsContract.SplashScreenURL = Convert.ToString(ds.Tables[1].Rows[0]["SplashScreenURL"]);
                }
                else
                {
                    institutionConfigurationDetailsContract.SplashScreenURL = String.Empty;
                }

                if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > AppConsts.NONE)
                {
                    institutionConfigurationDetailsContract.AdministratorsDetailsList = ds.Tables[2].AsEnumerable().Select(col =>
                      new InstitutionConfigurationAdministratorDetails
                      {
                          OrganizationUserId = Convert.ToInt32(col["OrganizationUserId"]),
                          UserFirstName = Convert.ToString(col["UserFirstName"]),
                          UserLastName = Convert.ToString(col["UserLastName"]),
                          UserName = Convert.ToString(col["UserName"]),
                          ComliancePermissionName = col["ComliancePermissionName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ComliancePermissionName"]),
                          OrderQueuePermissionName = col["OrderQueuePermissionName"] == DBNull.Value ? String.Empty : Convert.ToString(col["OrderQueuePermissionName"]),
                          ProfilePermissionName = col["ProfilePermissionName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ProfilePermissionName"]),
                          VerificationPermissionName = col["VerificationPermissionName"] == DBNull.Value ? String.Empty : Convert.ToString(col["VerificationPermissionName"]),
                          BkgPermissionName = col["BkgPermissionName"] == DBNull.Value ? String.Empty : Convert.ToString(col["BkgPermissionName"]),
                          PackagePermissionName = col["PackagePermissionName"] == DBNull.Value ? String.Empty : Convert.ToString(col["PackagePermissionName"]), //UAT-3369
                          EmailAddress = col["EmailAddress"] == DBNull.Value ? String.Empty : Convert.ToString(col["EmailAddress"]),
                          IsActive = col["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(col["IsActive"])
                      }).ToList();
                }
                else
                {
                    institutionConfigurationDetailsContract.AdministratorsDetailsList = new List<InstitutionConfigurationAdministratorDetails>();
                }

                return institutionConfigurationDetailsContract;
            }


        }

        /// <summary>
        /// Method is used to get Institution Configuration Detail
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        ScreeningDetailsForConfigurationContract IComplianceSetupRepository.GetScreeningDetailsForInstitutionConfiguration(Int32 hierarchyNodeID, Int32 packageID, Int32 packageHierarchyNodeID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_ScreeningDetailsForInstitutionConfiguration", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@HierarchyNodeID", hierarchyNodeID);
                command.Parameters.AddWithValue("@PackageID", packageID);
                command.Parameters.AddWithValue("@PackageHierarchyNodeID", packageHierarchyNodeID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                ScreeningDetailsForConfigurationContract screeningDetailsForConfigurationContract = new ScreeningDetailsForConfigurationContract();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    screeningDetailsForConfigurationContract.BackgroundPackageDetailsList = ds.Tables[0].AsEnumerable().Select(col =>
                          new BackgroundPackageDetailsForConfigurationContract
                          {
                              HierarchyLabel = Convert.ToString(col["HierarchyLabel"]),
                              PackageName = Convert.ToString(col["PackageName"]),
                              PackageSvcGrpID = col["PackageSvcGrpID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["PackageSvcGrpID"]),
                              ServiceGroupID = col["ServiceGroupID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["ServiceGroupID"]),
                              ServiceGroupName = col["ServiceGroupName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ServiceGroupName"]),
                              PackageServiceID = col["PackageServiceID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["PackageServiceID"]),
                              ServiceID = col["ServiceID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["ServiceID"]),
                              ServiceName = col["ServiceName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ServiceName"]),
                              QuantityIncluded = col["QuantityIncluded"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["QuantityIncluded"]),
                              PackageServiceItemID = col["PackageServiceItemID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["PackageServiceItemID"]),
                              PackageServiceItemName = col["PackageServiceItemName"] == DBNull.Value ? String.Empty : Convert.ToString(col["PackageServiceItemName"]),
                              AdditionalOccurencePriceType = col["AdditionalOccurencePriceType"] == DBNull.Value ? String.Empty : Convert.ToString(col["AdditionalOccurencePriceType"]),
                              QuantityGroup = col["QuantityGroup"] == DBNull.Value ? String.Empty : Convert.ToString(col["QuantityGroup"]),
                              GlobalFeeTypeCode = col["GlobalFeeTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(col["GlobalFeeTypeCode"]),
                              PackageServiceItemFeeID = col["PackageServiceItemFeeID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["PackageServiceItemFeeID"]),
                              GlobalFeeName = col["GlobalFeeName"] == DBNull.Value ? String.Empty : Convert.ToString(col["GlobalFeeName"]),
                              AdditionalOccurencePriceAmount = col["AdditionalOccurencePriceAmount"] == DBNull.Value ? (Decimal?)null : Convert.ToInt32(col["AdditionalOccurencePriceAmount"]),
                          }).ToList();
                }
                else
                {
                    screeningDetailsForConfigurationContract.BackgroundPackageDetailsList = new List<BackgroundPackageDetailsForConfigurationContract>();
                }


                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > AppConsts.NONE)
                {
                    screeningDetailsForConfigurationContract.ServiceFormDetailsList = ds.Tables[1].AsEnumerable().Select(col =>
                      new ServiceFormDetailsForConfigurationContract
                      {
                          ServiceID = Convert.ToInt32(col["ServiceID"]),
                          ServiceAtachedFormID = Convert.ToInt32(col["ServiceAtachedFormID"]),
                          ServiceAtachedFormName = Convert.ToString(col["ServiceAtachedFormName"]),
                          SendAutomatically = Convert.ToBoolean(col["SendAutomatically"]),
                          DocumentName = col["DocumentName"] == DBNull.Value ? String.Empty : Convert.ToString(col["DocumentName"]),
                          DocumentPath = col["DocumentPath"] == DBNull.Value ? String.Empty : Convert.ToString(col["DocumentPath"]),
                          SystemDocumentID = col["SystemDocumentID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(col["SystemDocumentID"]),
                      }).ToList();
                }
                else
                {
                    screeningDetailsForConfigurationContract.ServiceFormDetailsList = new List<ServiceFormDetailsForConfigurationContract>();
                }

                if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > AppConsts.NONE)
                {
                    screeningDetailsForConfigurationContract.ServiceItemFeeDetailsList = ds.Tables[2].AsEnumerable().Select(col =>
                      new ServiceItemFeeDetailsForConfigurationContract
                      {
                          PackageServiceID = Convert.ToInt32(col["PackageServiceID"]),
                          PackageServiceItemID = Convert.ToInt32(col["PackageServiceItemID"]),
                          PackageServiceItemFeeID = Convert.ToInt32(col["PackageServiceItemFeeID"]),
                          FeeItemType = Convert.ToString(col["FeeItemType"]),
                          LocalFeeTypeCode = Convert.ToString(col["LocalFeeTypeCode"]),
                      }).ToList();
                }
                else
                {
                    screeningDetailsForConfigurationContract.ServiceItemFeeDetailsList = new List<ServiceItemFeeDetailsForConfigurationContract>();
                }

                return screeningDetailsForConfigurationContract;
            }


        }


        /// <summary>
        /// Method is used to get compliance package Detail
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        CompliancePkgDetailContract IComplianceSetupRepository.GetCompliancePkgDetails(Int32 hierarchyNodeID, Int32 packageId, Int32 packageHierarchyID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetCompliancePkgDetails", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageId", packageId);
                command.Parameters.AddWithValue("@SelectedDpm_Id", hierarchyNodeID);
                command.Parameters.AddWithValue("@PackageDpm_Id", packageHierarchyID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                CompliancePkgDetailContract compliancePkgDetails = new CompliancePkgDetailContract();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    compliancePkgDetails.SubscriptionOptionDetails = ds.Tables[0].AsEnumerable().Select(col =>
                      new SubscriptionOptionDetail
                      {
                          SubscriptionOptionLabel = col["SubscriptionOption"] == DBNull.Value ? String.Empty : Convert.ToString(col["SubscriptionOption"]),
                          Price = col["Price"] == DBNull.Value ? String.Empty : Convert.ToString(col["Price"]),
                      }).ToList();
                }
                else
                {
                    compliancePkgDetails.SubscriptionOptionDetails = new List<SubscriptionOptionDetail>();
                }

                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > AppConsts.NONE)
                {

                    compliancePkgDetails.ReviewerTypeDetails = ds.Tables[1].AsEnumerable().Select(col =>
                          new ReviewerTypeDetail
                          {
                              AssignmentHierarchyID = Convert.ToInt32(col["AssignmentHierarchyID"]),
                              ParentAssignmentHierarchyId = col["ParentAssignmentHierarchyId"] == DBNull.Value ? (int?)null : Convert.ToInt32(col["ParentAssignmentHierarchyId"]),
                              ObjectName = Convert.ToString(col["ObjectName"]),
                              ReviewerType = Convert.ToString(col["ReviewerType"]),
                              ObjectTypeCode = Convert.ToString(col["ObjectTypeCode"])
                          }).ToList();
                }
                else
                {
                    compliancePkgDetails.ReviewerTypeDetails = new List<ReviewerTypeDetail>();
                }

                if (ds.Tables.Count > 1 && ds.Tables[2].Rows.Count > AppConsts.NONE)
                {
                    compliancePkgDetails.PackageName = Convert.ToString(ds.Tables[2].Rows[0]["PackageName"]);
                }
                if (ds.Tables.Count > 2 && ds.Tables[3].Rows.Count > AppConsts.NONE)
                {
                    compliancePkgDetails.NodeLabel = Convert.ToString(ds.Tables[3].Rows[0]["NodeLabel"]);
                }
                return compliancePkgDetails;
            }


        }

        #region GETTING INSTITUTION HIERARCHY LIST FOR COMMON SCREENS
        ObjectResult<GetDepartmentTree> IComplianceSetupRepository.GetInstituteHierarchyTreeCommon(Int32? currentUserID, string IsRequestFromAddRotationScreen)
        {
            return _ClientDBContext.GetInstituteHierarchyTreeCommon(currentUserID, IsRequestFromAddRotationScreen);
        }
        #endregion

        /// <summary>
        /// Returns whether the Compliance is Required for given settings - UAT 1543
        /// </summary>
        /// <param name="isComplianceReq"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Boolean IComplianceSetupRepository.GetComplianceRqdByDateRange(Boolean isComplianceReq, DateTime? startDate, DateTime? endDate)
        {
            var _isComplianceReq = _ClientDBContext.udf_GetComplianceRqdOnBasisOfDateRange(isComplianceReq, startDate, endDate).First();
            return Convert.ToBoolean(_isComplianceReq);
        }

        /// <summary>
        /// Method to Get Disclosure Document by System Document ID 
        /// </summary>
        /// <param name="aWSUseS3"></param>
        public List<ClientSystemDocument> GetClientSystemDocumentListByDocTypeID(Int32 docTypeID)
        {
            return _ClientDBContext.ClientSystemDocuments.Where(cond => cond.CSD_DocumentTypeID == docTypeID && !cond.CSD_IsDeleted).ToList();
        }

        #region UAT 1559 As an admin, I should be able to attach a form to be completed as a immunization package attribute.

        /// <summary>
        /// Get compliance view Document from Client System document. 
        /// </summary>
        /// <param name="docTypeID"></param>
        /// <returns></returns>
        public List<Entity.ClientEntity.ClientSystemDocument> GetComplianceViewDocuments(Int32 docTypeID)
        {
            return _ClientDBContext.ClientSystemDocuments.Where(x => x.CSD_DocumentTypeID == docTypeID && !x.CSD_IsDeleted).ToList();
        }

        /// <summary>
        /// Delete compliance view document from Client System document.
        /// </summary>
        /// <param name="systemDocumentID"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public Boolean DeleteComplianceViewDocument(Int32 systemDocumentID, Int32 currentUserId)
        {
            Entity.ClientEntity.ClientSystemDocument sysDoc = _ClientDBContext.ClientSystemDocuments.FirstOrDefault(condition => condition.CSD_ID == systemDocumentID);
            sysDoc.CSD_IsDeleted = true;
            sysDoc.CSD_ModifiedByID = currentUserId;
            sysDoc.CSD_ModifiedOn = DateTime.Now;

            if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            else
                return false;
        }

        public Boolean IsDocumentMappedWithAttribute(Int32 systemDocumentID)
        {
            return _ClientDBContext.ComplianceAttributeDocuments.Any(cad => cad.CAD_DocumentID == systemDocumentID && !cad.CAD_IsDeleted);
        }

        /// <summary>
        /// Update compliance view document in Client System document.
        /// </summary>
        /// <param name="attributeDocument"></param>
        /// <returns></returns>
        public Boolean UpdateComplianceViewDocument(Entity.ClientEntity.ClientSystemDocument attributeDocument)
        {
            Entity.ClientEntity.ClientSystemDocument sysDoc = _ClientDBContext.ClientSystemDocuments.FirstOrDefault(condition => condition.CSD_ID == attributeDocument.CSD_ID);
            if (sysDoc.IsNotNull())
            {
                sysDoc.CSD_Description = attributeDocument.CSD_Description;
                sysDoc.CSD_ModifiedByID = attributeDocument.CSD_ModifiedByID;
                sysDoc.CSD_ModifiedOn = DateTime.Now;

                if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Save compliance view document in System document.
        /// </summary>
        /// <param name="lstDisclosureDocuments"></param>
        /// <returns></returns>
        public Boolean SaveComplianceViewDocument(List<Entity.ClientEntity.ClientSystemDocument> lstViewDocuments)
        {
            foreach (Entity.ClientEntity.ClientSystemDocument sysDoc in lstViewDocuments)
            {
                _ClientDBContext.ClientSystemDocuments.AddObject(sysDoc);
            }

            if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            else
                return false;
        }

        public List<DocumentFieldMapping> GetDocumentFieldMapping(Int32 clientSystemDocumentID)
        {
            return _ClientDBContext.DocumentFieldMappings.Include("lkpDocumentFieldType_").Where(cond => cond.DFM_SystemDocumentID == clientSystemDocumentID && !cond.DFM_IsDeleted).ToList();
        }

        public bool UpdateDocumentFieldMapping(Int32 documentFieldMappingID, Int32 documentFieldTypeID, Int32 loggedInUdserID)
        {
            DocumentFieldMapping documentFieldMapping = _ClientDBContext.DocumentFieldMappings.Where(x => x.DFM_ID == documentFieldMappingID && !x.DFM_IsDeleted).FirstOrDefault();
            if (documentFieldMapping.IsNotNull())
            {
                documentFieldMapping.DFM_DocumentFieldTypeID = documentFieldTypeID;
                documentFieldMapping.DFM_ModifiedBy = loggedInUdserID;
                documentFieldMapping.DFM_ModifiedOn = DateTime.Now;
                documentFieldMapping.DFM_IsDeleted = false;
                if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        public String GetCategoryNamesByCategoryIds(String categoryIds)
        {
            String categoryNames = string.Empty;
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetCategoryNamesByCategoryIds", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CategoryIds", categoryIds);

                if (con.State == ConnectionState.Closed)
                    con.Open();

                categoryNames = Convert.ToString(command.ExecuteScalar());
                con.Close();
            }
            return categoryNames;
        }

        public String GetItemNamesByItemIds(String itemIds)
        {
            String itemNames = string.Empty;
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetItemNamesByItemIds", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ItemIds", itemIds);

                if (con.State == ConnectionState.Closed)
                    con.Open();

                itemNames = Convert.ToString(command.ExecuteScalar());
                con.Close();
            }
            return itemNames;
        }

        public List<CommunicationCCUsersList> GetCCusersWithNodePermissionAndCCUserSettings(Int32 communicationSubEventId, Int32 tenantId, Int32? hierarchyNodeID, Int32 objectTypeId, Int32 recordId)
        {
            return _ClientDBContext.usp_GetCommunicationCCusersDataWithNodePermissionAndCCUserSettings(communicationSubEventId, tenantId, hierarchyNodeID, objectTypeId, recordId).ToList();
        }

        #region Manage Shot Series

        List<ItemSery> IComplianceSetupRepository.GetItemShotSeries(Int32 categoryId)
        {
            List<ItemSery> itemShotSeriesList = ClientDBContext.ItemSeries
                                                                   .Where(cond => cond.IS_CategoryID == categoryId && !cond.IS_IsDeleted).ToList();

            return itemShotSeriesList;
        }

        Boolean IComplianceSetupRepository.AddNewShotSeries(ItemSery itemSeries)
        {
            _ClientDBContext.ItemSeries.AddObject(itemSeries);
            if (_ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        Boolean IComplianceSetupRepository.DeleteItemSeries(Int32 itemSeriesId, Int32 currentLoggedInUserId)
        {
            ItemSery existingItemSeries = _ClientDBContext.ItemSeries.Where(cond => cond.IS_ID == itemSeriesId && !cond.IS_IsDeleted).FirstOrDefault();
            if (!existingItemSeries.IsNullOrEmpty())
            {
                existingItemSeries.IS_IsDeleted = true;
                existingItemSeries.IS_ModifiedByID = currentLoggedInUserId;
                existingItemSeries.IS_ModifiedOn = DateTime.Now;
                if (_ClientDBContext.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public ItemSery GetCurrentItemSeriesInfo(Int32 currentSeriesID)
        {
            return _ClientDBContext.ItemSeries.Where(obj => obj.IS_ID == currentSeriesID && !obj.IS_IsDeleted).FirstOrDefault();
        }

        Boolean IComplianceSetupRepository.UpdateItemSeries(ItemSery itemSeries)
        {
            ItemSery seriesToUpdate = _ClientDBContext.ItemSeries.Where(cond => cond.IS_ID == itemSeries.IS_ID && !cond.IS_IsDeleted).FirstOrDefault();
            if (!seriesToUpdate.IsNullOrEmpty())
            {
                seriesToUpdate.IS_Details = itemSeries.IS_Details;
                seriesToUpdate.IS_Description = itemSeries.IS_Description;
                seriesToUpdate.IS_ModifiedByID = itemSeries.IS_ModifiedByID;
                seriesToUpdate.IS_ModifiedOn = itemSeries.IS_ModifiedOn;
                seriesToUpdate.IS_Name = itemSeries.IS_Name;
                seriesToUpdate.IS_Label = itemSeries.IS_Label;
                seriesToUpdate.IS_IsActive = itemSeries.IS_IsActive;
                seriesToUpdate.IS_IsAvailablePostApproval = itemSeries.IS_IsAvailablePostApproval;
                seriesToUpdate.IS_RuleExecutionOrder = itemSeries.IS_RuleExecutionOrder;
                if (_ClientDBContext.SaveChanges() > 0)
                {
                    return true;
                }
            }

            return false;
        }

        void IComplianceSetupRepository.SaveSeriesData(Int32 seriesId, List<Int32> lstItemIds, Dictionary<Int32, Boolean> dicAttributeIds, Int32 currentUserId)
        {
            DateTime _currentDateTime = DateTime.Now;

            #region Manage the Items

            short _itemOrderId = AppConsts.NONE;

            List<ItemSeriesItem> _lstDBMappedItemSeriesItems = _ClientDBContext.ItemSeriesItems.Where(isi => isi.ISI_ItemSeriesID == seriesId && isi.ISI_IsDeleted == false).ToList();
            List<Int32> _lstDBMappedItemIds = _lstDBMappedItemSeriesItems.Select(isi => isi.ISI_ItemID).ToList();

            var _lstToRemove = _lstDBMappedItemSeriesItems.Where(isi => !lstItemIds.Contains(isi.ISI_ItemID)).ToList();

            var _lstToUpdate = _lstDBMappedItemSeriesItems.Where(isi => lstItemIds.Contains(isi.ISI_ItemID)).ToList();

            //Reset the ItemSeriesItemDisplay Order after Removal of any item from list
            if (!_lstToRemove.IsNullOrEmpty())
            {
                foreach (var isi in _lstToUpdate)
                {
                    _itemOrderId += AppConsts.ONE;
                    isi.ISI_ItemOrder = _itemOrderId;
                    isi.ISI_ModifiedByID = currentUserId;
                    isi.ISI_ModifiedOn = _currentDateTime;
                }
            }
            else
            {
                if (!_lstToUpdate.IsNullOrEmpty())
                {
                    _itemOrderId = _lstToUpdate.Max(ord => ord.ISI_ItemOrder);
                }
            }

            List<Int16> _lstRemovedDisplayOrders = new List<Int16>();

            // Remove Items which are Unselected from the UI
            foreach (var isiRemove in _lstToRemove)
            {
                isiRemove.ISI_IsDeleted = true;
                isiRemove.ISI_ModifiedByID = currentUserId;
                isiRemove.ISI_ModifiedOn = _currentDateTime;

                _lstRemovedDisplayOrders.Add(isiRemove.ISI_ItemOrder);

                // Delete the mapping from ItemSeriesAttributeMap can be removed
                foreach (var isiAttrMap in isiRemove.ItemSeriesAttributeMaps)
                {
                    isiAttrMap.ISAM_IsDeleted = true;
                    isiAttrMap.ISAM_ModifiedByID = currentUserId;
                    isiAttrMap.ISAM_ModifiedOn = _currentDateTime;
                }
            }

            // Add Items which are not Part of the Database Items and selected for first time
            var _lstToAdd = lstItemIds.Where(isi => !_lstDBMappedItemIds.Contains(isi)).ToList();

            foreach (var itemId in _lstToAdd)
            {
                _itemOrderId += AppConsts.ONE;
                ItemSeriesItem _itemSeriesItem = new ItemSeriesItem();
                _itemSeriesItem.ISI_ItemSeriesID = seriesId;
                _itemSeriesItem.ISI_ItemOrder = Convert.ToInt16(_itemOrderId);
                _itemSeriesItem.ISI_ItemID = itemId;
                _itemSeriesItem.ISI_CreatedByID = currentUserId;
                _itemSeriesItem.ISI_CreatedOn = _currentDateTime;
                _itemSeriesItem.ISI_IsDeleted = false;

                _ClientDBContext.ItemSeriesItems.AddObject(_itemSeriesItem);
            }

            #endregion

            List<ItemSeriesAttribute> _lstDBMappedItemSeriesAttrs = _ClientDBContext.ItemSeriesAttributes.Where(isa => isa.ISA_ItemSeriesID == seriesId && isa.ISA_IsDeleted == false).ToList();

            List<Int32> _lstDBMappedAttrIds = _lstDBMappedItemSeriesAttrs.Select(isi => isi.ISA_AttributeID).ToList();
            var _lstAttrToRemove = _lstDBMappedItemSeriesAttrs.Where(isa => !dicAttributeIds.ContainsKey(isa.ISA_AttributeID)).ToList();

            // Remove Items which are Unselected from the UI
            foreach (var itemSerAttr in _lstAttrToRemove)
            {
                itemSerAttr.ISA_IsDeleted = true;
                itemSerAttr.ISA_ModifiedByID = currentUserId;
                itemSerAttr.ISA_ModifiedOn = _currentDateTime;
            }

            // Add Attributes which are not Part of the Database and selected for first time
            foreach (var attribute in dicAttributeIds)
            {
                if (!_lstDBMappedAttrIds.Contains(attribute.Key))
                {
                    ItemSeriesAttribute _itemSeriesAttribute = new ItemSeriesAttribute();
                    _itemSeriesAttribute.ISA_ItemSeriesID = seriesId;
                    _itemSeriesAttribute.ISA_AttributeID = attribute.Key;
                    _itemSeriesAttribute.ISA_IsKeyAttribute = attribute.Value;
                    _itemSeriesAttribute.ISA_IsDeleted = false;
                    _itemSeriesAttribute.ISA_CreatedByID = currentUserId;
                    _itemSeriesAttribute.ISA_CreatedOn = _currentDateTime;

                    _ClientDBContext.ItemSeriesAttributes.AddObject(_itemSeriesAttribute);
                }
                else
                {
                    //Update the Key attribute value
                    var keyAttributeTpUpdate = _lstDBMappedItemSeriesAttrs.FirstOrDefault(cnd => cnd.ISA_AttributeID == attribute.Key && cnd.ISA_IsKeyAttribute != attribute.Value);
                    if (!keyAttributeTpUpdate.IsNullOrEmpty())
                    {
                        keyAttributeTpUpdate.ISA_IsKeyAttribute = attribute.Value;
                        keyAttributeTpUpdate.ISA_ModifiedByID = currentUserId;
                        keyAttributeTpUpdate.ISA_ModifiedOn = _currentDateTime;
                    }
                }
            }

            _ClientDBContext.SaveChanges();
        }

        /// <summary>
        /// Add New ItemSeriesItem, on table mapping new click
        /// </summary>
        /// <param name="seriesId"></param>
        /// <param name="itemId"></param>
        /// <param name="currentUserId"></param>
        void IComplianceSetupRepository.SaveItemSeriesItem(Int32 seriesId, Int32 itemId, Int32 currentUserId)
        {
            var _maxOrder = _ClientDBContext.ItemSeriesItems.Where(isi => isi.ISI_ItemSeriesID == seriesId).Max(isi => isi.ISI_ItemOrder);

            ItemSeriesItem _itemSeriesItem = new ItemSeriesItem();
            _itemSeriesItem.ISI_ItemSeriesID = seriesId;
            _itemSeriesItem.ISI_ItemOrder = Convert.ToInt16(_maxOrder + 1);
            _itemSeriesItem.ISI_ItemID = itemId;
            _itemSeriesItem.ISI_CreatedByID = currentUserId;
            _itemSeriesItem.ISI_CreatedOn = DateTime.Now;
            _itemSeriesItem.ISI_IsDeleted = false;

            _ClientDBContext.ItemSeriesItems.AddObject(_itemSeriesItem);
            _ClientDBContext.SaveChanges();
        }

        /// <summary>
        /// Remove Item from ItemSeriesItem, on table mapping Remove click
        /// </summary>
        /// <param name="itemSeriesItemId"></param>
        /// <param name="currentUserId"></param>
        void IComplianceSetupRepository.RemoveItemSeriesItem(Int32 itemSeriesItemId, Int32 currentUserId)
        {
            var _itemSeriesToRemove = _ClientDBContext.ItemSeriesItems.First(isi => isi.ISI_ID == itemSeriesItemId);
            _itemSeriesToRemove.ISI_IsDeleted = true;
            _itemSeriesToRemove.ISI_ModifiedByID = currentUserId;
            _itemSeriesToRemove.ISI_ModifiedOn = DateTime.Now;

            _ClientDBContext.SaveChanges();
        }

        #endregion


        #region Serier data return
        DataSet IComplianceSetupRepository.GetSeriesData(Int32 seriesId)
        {
            DataSet SeriesData = new DataSet();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetSeriesData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SeriesId", seriesId);

                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                sqlDataAdapter.Fill(SeriesData);
                sqlDataAdapter.Dispose();
                con.Close();
            }
            return SeriesData;
        }

        /// <summary>
        /// Save-Update the mapping of the Item Attributes with the Series Attributes.
        /// </summary>
        /// <param name="lstSeriesItemContract"></param>
        /// <param name="currentUserId"></param>
        void IComplianceSetupRepository.SaveUpdateSeriesMapping(List<SeriesItemContract> lstSeriesItemContract, Int32 currentUserId)
        {
            var _sereisId = lstSeriesItemContract.First().ItemSeriesId;
            DateTime _currentDateTime = DateTime.Now;

            // Get ID's of the ItemSeriesAttributeMaps to be Updated
            List<Int32> _lstExistingMappingIds = lstSeriesItemContract.Where(isic => isic.ItemSeriesAttributeMapId != AppConsts.NONE).Select(isic => isic.ItemSeriesAttributeMapId).ToList();

            // Get the ItemSeriesAttributeMaps to be Updated, from database
            List<ItemSeriesAttributeMap> _lstExistingMappings = _ClientDBContext.ItemSeriesAttributeMaps.Where(isam => _lstExistingMappingIds.Contains(isam.ISAM_ID) && isam.ISAM_IsDeleted == false).ToList();

            List<Int32> _lstExistingItemSeriesItemIds = lstSeriesItemContract.Select(isic => isic.ItemSeriesItemId).Distinct().ToList();
            List<ItemSeriesItem> _lstExistingItemSeriesItems = _ClientDBContext.ItemSeriesItems.Where(isi => _lstExistingItemSeriesItemIds.Contains(isi.ISI_ID) && isi.ISI_IsDeleted == false).ToList();

            // Includes both the Items and AttributeMapping level Data
            var _lstItemSeriesItemsToUpdate = lstSeriesItemContract.Where(isi => isi.ItemSeriesItemId != AppConsts.NONE).ToList();

            // Get the Newly selected Items to be added to the list.
            var _lstItemSeriesItemsToAdd = lstSeriesItemContract.Where(isi => isi.ItemSeriesItemId == AppConsts.NONE).Select(isi => isi.UniqueIdentifier).Distinct().ToList();

            #region Manage Mapping Update of Existing ItemSeriesItems

            foreach (var isam in _lstItemSeriesItemsToUpdate)
            {
                // Add any New Mapping for existing ItemSeriesItem
                if (isam.ItemSeriesAttributeMapId == AppConsts.NONE)
                {
                    ItemSeriesAttributeMap _itemSeriesAttrMap = new ItemSeriesAttributeMap();
                    _itemSeriesAttrMap.ISAM_ItemSeriesItemID = isam.ItemSeriesItemId;
                    _itemSeriesAttrMap.ISAM_ItemSeriesAttributeID = isam.ISAM_ItemSeriesAttrId;
                    _itemSeriesAttrMap.ISAM_AttributeID = isam.SelectedAttributeId;
                    _itemSeriesAttrMap.ISAM_CreatedByID = currentUserId;
                    _itemSeriesAttrMap.ISAM_CreatedOn = _currentDateTime;
                    _ClientDBContext.ItemSeriesAttributeMaps.AddObject(_itemSeriesAttrMap);

                    //Update the Shuffle Status and Display order for already added Items
                    var _currentItemSeriesItem = _lstExistingItemSeriesItems.Where(isi => isi.ISI_ID == isam.ItemSeriesItemId).First();
                    _currentItemSeriesItem.ISI_ItemStatusPostDataShuffleID = isam.PostShuffleStatusId;
                    _currentItemSeriesItem.ISI_ItemOrder = Convert.ToInt16(isam.ItemSeriesItemOrder);
                    _currentItemSeriesItem.ISI_ModifiedByID = currentUserId;
                    _currentItemSeriesItem.ISI_ModifiedOn = _currentDateTime;
                }
                else // Update existing Mapping for existing ItemSeriesItem
                {
                    // Update the Selected Attribute Mapping for already existing mappings. 
                    var _currentIsam = _lstExistingMappings.Where(eisam => eisam.ISAM_ID == isam.ItemSeriesAttributeMapId).First();
                    _currentIsam.ISAM_AttributeID = isam.SelectedAttributeId;
                    _currentIsam.ISAM_ModifiedByID = currentUserId;
                    _currentIsam.ISAM_ModifiedOn = _currentDateTime;

                    //Update the Shuffle Status and Display order for already added Items
                    _currentIsam.ItemSeriesItem.ISI_ItemStatusPostDataShuffleID = isam.PostShuffleStatusId;
                    _currentIsam.ItemSeriesItem.ISI_ItemOrder = Convert.ToInt16(isam.ItemSeriesItemOrder);
                    _currentIsam.ItemSeriesItem.ISI_ModifiedByID = currentUserId;
                    _currentIsam.ItemSeriesItem.ISI_ModifiedOn = _currentDateTime;
                }
            }

            #endregion

            #region Add new ItemSeriesItems and their mappings.

            // Add the newly selected ItemSeries
            foreach (var isiToAdd in _lstItemSeriesItemsToAdd)
            {
                SeriesItemContract _isiData = lstSeriesItemContract.Where(isi => isi.UniqueIdentifier == isiToAdd).First();

                ItemSeriesItem _itemSeriesItem = new ItemSeriesItem();
                _itemSeriesItem.ISI_ItemSeriesID = _isiData.ItemSeriesId;
                _itemSeriesItem.ISI_ItemOrder = Convert.ToInt16(_isiData.ItemSeriesItemOrder);
                _itemSeriesItem.ISI_ItemID = _isiData.CmpItemId;
                _itemSeriesItem.ISI_IsDeleted = false;
                _itemSeriesItem.ISI_CreatedByID = currentUserId;
                _itemSeriesItem.ISI_CreatedOn = DateTime.Now;
                _itemSeriesItem.ISI_ItemStatusPostDataShuffleID = _isiData.PostShuffleStatusId;

                List<SeriesItemContract> _lstAttributeMapping = lstSeriesItemContract.Where(isi => isi.UniqueIdentifier == isiToAdd).ToList();

                foreach (var item in _lstAttributeMapping)
                {
                    ItemSeriesAttributeMap _itemSeriesAttrMap = new ItemSeriesAttributeMap();
                    _itemSeriesAttrMap.ItemSeriesItem = _itemSeriesItem;
                    _itemSeriesAttrMap.ISAM_ItemSeriesAttributeID = item.ISAM_ItemSeriesAttrId;
                    _itemSeriesAttrMap.ISAM_AttributeID = item.SelectedAttributeId;
                    _itemSeriesAttrMap.ISAM_CreatedByID = currentUserId;
                    _itemSeriesAttrMap.ISAM_CreatedOn = _currentDateTime;
                    _ClientDBContext.ItemSeriesAttributeMaps.AddObject(_itemSeriesAttrMap);
                }

                _ClientDBContext.ItemSeriesItems.AddObject(_itemSeriesItem);
            }
            #endregion

            #region Remove (any ItemSeriesItem and it's Attribute Mapping) AND ItemSeriesAttributeMappings which were "UnSelected" from UI

            // Delete the ItemSeriesItems which are marked as deleted from the UI, using Delete action link
            var _lstItemSeriesItemToRemove = _ClientDBContext.ItemSeriesItems.Where(isi => isi.ISI_ItemSeriesID == _sereisId
                              && !_lstExistingItemSeriesItemIds.Contains(isi.ISI_ID) && isi.ISI_IsDeleted == false).ToList();

            foreach (var isi in _lstItemSeriesItemToRemove)
            {
                isi.ISI_ModifiedByID = currentUserId;
                isi.ISI_IsDeleted = true;
                isi.ISI_ModifiedOn = _currentDateTime;

                foreach (var isam in isi.ItemSeriesAttributeMaps)
                {
                    isam.ISAM_ModifiedByID = currentUserId;
                    isam.ISAM_ModifiedOn = _currentDateTime;
                    isam.ISAM_IsDeleted = true;
                }
            }

            // ItemSeriesAttribute Mappings to be removed from database, for any existing ItemSeriesItem
            foreach (var itemSeriesItem in _lstExistingItemSeriesItems)
            {
                foreach (var itemSeriesAttrMapping in itemSeriesItem.ItemSeriesAttributeMaps.Where(isam => isam.ISAM_IsDeleted == false).ToList())
                {
                    // ISAM_ID != 0 is required as the new mappings to be added are not part of existing mapping ids
                    if (itemSeriesAttrMapping.ISAM_ID != AppConsts.NONE && !_lstExistingMappingIds.Contains(itemSeriesAttrMapping.ISAM_ID))
                    {
                        itemSeriesAttrMapping.ISAM_ModifiedByID = currentUserId;
                        itemSeriesAttrMapping.ISAM_ModifiedOn = _currentDateTime;
                        itemSeriesAttrMapping.ISAM_IsDeleted = true;
                    }
                }
            }

            #endregion

            _ClientDBContext.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstItemIds"></param>
        /// <returns></returns>
        public List<ComplianceAttribute> GetComplianceAttributesByItemIds(List<Int32> lstItemIds)
        {
            var _calculatedAttrTypeCode = ComplianceAttributeType.Calculated.GetStringValue();

            List<ComplianceItemAttribute> lst = _ClientDBContext.ComplianceItemAttributes
                    .Include("ComplianceAttribute")
                    .Where(cia =>
                        lstItemIds.Contains(cia.CIA_ItemID)
                        && !cia.ComplianceAttribute.IsDeleted && cia.ComplianceAttribute.IsActive
                        && !cia.CIA_IsDeleted && cia.CIA_IsActive && cia.ComplianceAttribute.lkpComplianceAttributeType.Code != _calculatedAttrTypeCode).ToList();
            return lst.Select(s => s.ComplianceAttribute).Distinct().ToList();
        }

        #endregion


        List<GetShotSeriesTree> IComplianceSetupRepository.GetShotSeriesTreeData()
        {
            return ClientDBContext.Usp_GetShotSeriesTree().ToList();
            //EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            //using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            //{

            //    SqlCommand command = new SqlCommand("[dbo].[Usp_GetShotSeriesTree]", con);
            //    command.CommandType = CommandType.StoredProcedure;
            //    SqlDataAdapter adp = new SqlDataAdapter();
            //    adp.SelectCommand = command;
            //    DataSet ds = new DataSet();
            //    adp.Fill(ds);
            //    if (ds.Tables.Count > 0)
            //    {
            //        IEnumerable<DataRow> rows = ds.Tables[0].AsEnumerable();
            //        return rows.Select(col => new GetRuleSetTree
            //        {
            //            TreeNodeTypeID = col["TreeNodeTypeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["TreeNodeTypeID"]),
            //            DataID = col["DataID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["DataID"]),
            //            ParentDataID = col["ParentDataID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ParentDataID"]),
            //            Level = col["Level"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["Level"]),
            //            NodeID = col["NodeID"] == DBNull.Value ? String.Empty : Convert.ToString(col["NodeID"]),
            //            ParentNodeID = col["ParentNodeID"] == DBNull.Value ? String.Empty : Convert.ToString(col["ParentNodeID"]),
            //            Value = col["Value"] == DBNull.Value ? String.Empty : Convert.ToString(col["Value"]),
            //            UICode = col["UICode"] == DBNull.Value ? String.Empty : Convert.ToString(col["UICode"])
            //        }).ToList();
            //    }
            //}
            //return new List<GetRuleSetTree>();
        }

        /// <summary>
        /// get the item id from ItemSeriesItem table on basis on series id
        /// </summary>
        /// <param name="seriesId">selected series id</param>
        /// <returns></returns>
        public List<Int32> GetItemSeriesItemsBySeriesId(Int32 seriesId)
        {
            return _ClientDBContext.ItemSeriesItems.Where(isi => isi.ISI_ItemSeriesID == seriesId && !isi.ISI_IsDeleted).Select(s => s.ISI_ItemID).ToList();
        }

        /// <summary>
        /// get the Attribute id from ItemSeriesAttribute table on basis on series id
        /// </summary>
        /// <param name="seriesId">selected series id</param>
        /// <returns></returns>
        public List<Int32> GetItemSeriesAttributeBySeriesId(Int32 seriesId)
        {
            return _ClientDBContext.ItemSeriesAttributes.Where(isa => isa.ISA_ItemSeriesID == seriesId && !isa.ISA_IsDeleted).Select(s => s.ISA_AttributeID).ToList();
        }
        public Int32 GetItemSeriesKeyAttributeBySeriesId(Int32 seriesId)
        {
            return _ClientDBContext.ItemSeriesAttributes.FirstOrDefault(isa => isa.ISA_ItemSeriesID == seriesId && isa.ISA_IsKeyAttribute && !isa.ISA_IsDeleted).ISA_AttributeID;
        }

        List<CompliancePackage> IComplianceSetupRepository.GetPackagesRelatedToCategory(Int32 categoryId)
        {
            List<CompliancePackageCategory> lstPkgCategory = _ClientDBContext.CompliancePackageCategories.Where(cond => cond.CPC_CategoryID == categoryId && !cond.CPC_IsDeleted).ToList();
            if (lstPkgCategory.IsNullOrEmpty())
            {
                return new List<CompliancePackage>();
            }
            return lstPkgCategory.Select(cond => cond.CompliancePackage).ToList();
        }

        /// <summary>
        /// Check if Series Mapped Attribute exist
        /// </summary>
        /// <param name="itemSeriesId"></param>
        /// <returns></returns>
        Boolean IComplianceSetupRepository.CheckIfSeriesMappedAttrExist(Int32 itemSeriesId)
        {
            List<ItemSeriesItem> lstItemSeriesItem = _ClientDBContext.ItemSeriesItems.Where(con => con.ISI_ItemSeriesID == itemSeriesId && !con.ISI_IsDeleted).ToList();

            if (!lstItemSeriesItem.IsNullOrEmpty())
            {
                var lstItemSeriesItemID = lstItemSeriesItem.Select(x => x.ISI_ID).ToList();
                return _ClientDBContext.ItemSeriesAttributeMaps.Any(x => lstItemSeriesItemID.Contains(x.ISAM_ItemSeriesItemID) && !x.ISAM_IsDeleted);
            }
            return false;
        }

        Dictionary<Int32, Boolean> IComplianceSetupRepository.GetComplianceRqdForPackage(Int32 packageId)
        {
            Dictionary<Int32, Boolean> lstComplianceRqdForCategories = new Dictionary<Int32, Boolean>();
            EntityConnection connection = ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@PackageId", packageId)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetComplianceRqdForPackage", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lstComplianceRqdForCategories.Add(Convert.ToInt32(dr["CPC_ID"]), Convert.ToBoolean(dr["ComplianceRequired"]));
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstComplianceRqdForCategories;
        }


        #region UAT 1560 WB: We should be able to add documents that need to be signed to the order process.
        List<GenericSystemDocumentMappingContract> IComplianceSetupRepository.GetGenericSystemDocumentMapping(Int32 recordID, Int32 recordTypeID)
        {
            List<GenericSystemDocumentMappingContract> lstGenericSystemDocumentMappingContract = new List<GenericSystemDocumentMappingContract>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@RecordID", recordID),
                             new SqlParameter("@RecordTypeID", recordTypeID)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetGenericSystemDocumentMapping", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            GenericSystemDocumentMappingContract genericSystemDocumentMappingContract = new GenericSystemDocumentMappingContract();
                            genericSystemDocumentMappingContract.SystemDocMappingID = dr["SystemDocMappingID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SystemDocMappingID"]);
                            genericSystemDocumentMappingContract.SystemDocID = dr["SystemDocID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["SystemDocID"]);
                            genericSystemDocumentMappingContract.RecordTypeName = dr["RecordTypeName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RecordTypeName"]);
                            genericSystemDocumentMappingContract.RecordID = dr["RecordID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RecordID"]);
                            genericSystemDocumentMappingContract.DocumentFileName = dr["DocumentFileName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DocumentFileName"]);
                            genericSystemDocumentMappingContract.IsOperational = dr["IsOperational"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dr["IsOperational"]);
                            genericSystemDocumentMappingContract.SendToStudent = dr["SendToStudent"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dr["SendToStudent"]);

                            lstGenericSystemDocumentMappingContract.Add(genericSystemDocumentMappingContract);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstGenericSystemDocumentMappingContract;
        }

        public string SaveAdditionalDocumentMapping(Int32 recordID, Int32 recordTypeID, List<Int32> lstSelectedDocumentsID, Int32 loggedInUserID)
        {
            foreach (Int32 sysDocID in lstSelectedDocumentsID)
            {
                GenericSystemDocumentMapping genericSystemDocumentMapping = new GenericSystemDocumentMapping();
                genericSystemDocumentMapping.GSDM_RecordID = recordID;
                genericSystemDocumentMapping.GSDM_RecordTypeID = Convert.ToInt16(recordTypeID);
                genericSystemDocumentMapping.GSDM_SystemDocumentID = sysDocID;
                genericSystemDocumentMapping.GSDM_IsDeleted = false;
                genericSystemDocumentMapping.GSDM_CreatedOn = DateTime.Now;
                genericSystemDocumentMapping.GSDM_CreatedBy = loggedInUserID;

                _ClientDBContext.AddToGenericSystemDocumentMappings(genericSystemDocumentMapping);
            }
            if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
            {
                return "Additional document mapped successfully.";
            }
            return String.Empty;
        }

        public string DeleteAdditionalDocumentMapping(Int32 docMappingID, Int32 loggedInUserID)
        {
            GenericSystemDocumentMapping genericSystemDocumentMapping = _ClientDBContext.GenericSystemDocumentMappings.Where(cond => cond.GSDM_ID == docMappingID && !cond.GSDM_IsDeleted).FirstOrDefault();
            genericSystemDocumentMapping.GSDM_IsDeleted = true;
            genericSystemDocumentMapping.GSDM_ModifiedBy = loggedInUserID;
            genericSystemDocumentMapping.GSDM_ModifiedOn = DateTime.Now;
            if (_ClientDBContext.SaveChanges() > AppConsts.NONE)
            {
                return "Additional document deleted successfully.";
            }
            return String.Empty;
        }


        public ApplicantDocument SaveEsignedAdditionalDocumentAsPdf(String PdfPath, String filename, Int32 fileSize, Int32 documentTypeId, Int32 currentLoggedInUserId,
            Int32 orgUserID, Int16 dataEntryDocStatusId, Boolean isSearchableOnly)
        {
            if (!String.IsNullOrEmpty(PdfPath))
            {

                ApplicantDocument applicantDocument = new ApplicantDocument();
                applicantDocument.OrganizationUserID = currentLoggedInUserId;
                applicantDocument.DocumentPath = PdfPath;
                applicantDocument.DocumentType = documentTypeId;
                applicantDocument.FileName = filename;
                applicantDocument.Size = fileSize;
                //Will keep the document in deleted state until the order is completed.
                applicantDocument.IsDeleted = true;
                applicantDocument.CreatedOn = DateTime.Now;
                applicantDocument.CreatedByID = orgUserID;
                applicantDocument.IsSearchableOnly = isSearchableOnly;
                if (dataEntryDocStatusId > AppConsts.NONE)
                {
                    applicantDocument.DataEntryDocumentStatusID = dataEntryDocStatusId;
                }
                _ClientDBContext.ApplicantDocuments.AddObject(applicantDocument);
                _ClientDBContext.SaveChanges();

                return applicantDocument;
            }

            return null;
        }
        #endregion

        public Dictionary<Int32, String> GetDefaultPermissionForClientAdmin(Int32 currentUserID)
        {
            Dictionary<Int32, String> dicDefaultPermission = new Dictionary<Int32, String>();
            String fullAccessPermissionCode = LkpPermission.FullAccess.GetStringValue();
            HierarchyPermission rootNodePermission = _ClientDBContext.HierarchyPermissions
                                                        .Where(cond => cond.HP_OrganizationUserID == currentUserID
                                                        && cond.DeptProgramMapping.DPM_ParentNodeID == null
                                                        && cond.lkpPermission.PER_Code == fullAccessPermissionCode
                                                        && cond.HP_IsDeleted != true
                                                        && !cond.DeptProgramMapping.DPM_IsDeleted
                                                        ).FirstOrDefault();
            if (!rootNodePermission.IsNullOrEmpty())
            {
                dicDefaultPermission.Add(rootNodePermission.HP_HierarchyID.Value, rootNodePermission.DeptProgramMapping.DPM_Label);
            }
            return dicDefaultPermission;
        }

        #region UAT-1812:Creation of an Approval/rejection summary for applicant logins
        List<DataTable> IComplianceSetupRepository.GetAppSummaryDataAfterLastLogin(Int32 currentLoggedInUserID, DateTime? lastLoginTime)
        {
            EntityConnection connection = ClientDBContext.Connection as EntityConnection;
            List<DataTable> dataTableList = new List<DataTable>();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[dbo].[usp_GetApplicantDataSinceLastLogin]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrgUserID", currentLoggedInUserID);
                command.Parameters.AddWithValue("@LastLoginTime", lastLoginTime);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    dataTableList.Add(ds.Tables[0]);
                    dataTableList.Add(ds.Tables[1]);
                }
            }
            return dataTableList;
        }
        #endregion

        List<SeriesAttributeContract> IComplianceSetupRepository.GetSeriesDetailsForShuffleTest(Int32 seriesID)
        {
            List<SeriesAttributeContract> lstSeriesAttributeContract = new List<SeriesAttributeContract>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@ItemSeriesID", seriesID)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetSeriesDetailsForShuffleTest", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SeriesAttributeContract seriesAttributeContract = new SeriesAttributeContract();
                            seriesAttributeContract.CmpItemSeriesId = dr["ItemSeriesID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ItemSeriesID"]);
                            seriesAttributeContract.CmpItemName = dr["ComplianceItemName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ComplianceItemName"]);
                            seriesAttributeContract.ItemSeriesItemOrder = dr["ItemOrder"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ItemOrder"]);
                            seriesAttributeContract.CmpItemId = dr["ComplianceItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ComplianceItemID"]);
                            seriesAttributeContract.CmpItemSeriesName = dr["ItemSeriesName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ItemSeriesName"]);
                            seriesAttributeContract.CmpItemSeriesItemId = dr["ItemSeriesItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ItemSeriesItemID"]);
                            seriesAttributeContract.ItemSeriesAttributeId = dr["ItemSeriesAttributeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ItemSeriesAttributeID"]);
                            seriesAttributeContract.CmpAttributeId = dr["ComplianceAttributeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ComplianceAttributeID"]);
                            seriesAttributeContract.CmpAttributeName = dr["ComplianceAttributeName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ComplianceAttributeName"]);
                            seriesAttributeContract.IsKeyAttribute = dr["IsKeyAttribute"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsKeyAttribute"]);
                            seriesAttributeContract.CmpAttributeDatatypeCode = dr["AttributeDataTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AttributeDataTypeCode"]);
                            seriesAttributeContract.OptionText = dr["OptionText"] == DBNull.Value ? String.Empty : Convert.ToString(dr["OptionText"]);
                            seriesAttributeContract.OptionValue = dr["OptionValue"] == DBNull.Value ? String.Empty : Convert.ToString(dr["OptionValue"]);
                            lstSeriesAttributeContract.Add(seriesAttributeContract);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstSeriesAttributeContract;
        }

        Dictionary<ShotSeriesSaveResponse, List<SeriesAttributeContract>> IComplianceSetupRepository.GetSeriesDetailsAfterShuffleTest(Int32 seriesID, Int32 systemUserID,
                                                                                                         String seriesAttributeXML, String ruleMappingXML, Int32 selectedPackageID)
        {

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("Usp_ShotSeriesShuffleTest", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ItemSeriesID", seriesID);
                command.Parameters.AddWithValue("@SystemUserID", systemUserID);
                command.Parameters.AddWithValue("@SeriesAttributeXML", seriesAttributeXML);
                command.Parameters.AddWithValue("@RuleMappingXML", ruleMappingXML);
                command.Parameters.AddWithValue("@SelectedPackageID", selectedPackageID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                List<SeriesAttributeContract> lstSeriesAttributeContract = new List<SeriesAttributeContract>();
                ShotSeriesSaveResponse saveResponse = new ShotSeriesSaveResponse();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    String dtSaveResponse = Convert.ToString(ds.Tables[0].Rows[0]["ResultXML"]);

                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(dtSaveResponse);
                    XmlNode nodeStatus = null;
                    nodeStatus = xml.SelectSingleNode("Result/Status");
                    XmlNodeList nodeMessages = xml.SelectNodes("Result/Messages/Message");
                    String statusCode = String.Empty;
                    String statusName = String.Empty;
                    if (nodeStatus.IsNullOrEmpty())
                    {
                        nodeStatus = xml.SelectSingleNode("Result");
                    }
                    statusCode = Convert.ToString(nodeStatus["Code"].InnerText);
                    statusName = Convert.ToString(nodeStatus["Name"].InnerText);
                    StringBuilder sbMessage = new StringBuilder();
                    if (!nodeMessages.IsNullOrEmpty())
                    {
                        foreach (XmlNode xmlNode in nodeMessages)
                        {
                            if (xmlNode.IsNotNull())
                            {
                                sbMessage.Append(xmlNode.InnerText + "<br/>");
                            }
                        }
                    }
                    saveResponse.StatusCode = Convert.ToInt32(statusCode);
                    saveResponse.Message = Convert.ToString(sbMessage);
                    saveResponse.StatusName = statusName;
                }


                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > AppConsts.NONE)
                {
                    lstSeriesAttributeContract = ds.Tables[1].AsEnumerable().Select(col =>
                         new SeriesAttributeContract
                         {
                             CmpItemName = col["ComplianceItemName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ComplianceItemName"]),
                             ItemSeriesItemOrder = col["ItemOrder"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ItemOrder"]),
                             CmpItemId = col["ComplianceItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ComplianceItemID"]),
                             CmpItemSeriesItemId = col["ItemSeriesItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["ItemSeriesItemID"]),
                             NewStatusID = col["NewStatusID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["NewStatusID"]),
                             CmpAttributeValue = col["AttributeValue"] == DBNull.Value ? String.Empty : Convert.ToString(col["AttributeValue"]),
                             NewStatusName = col["NewStatusName"] == DBNull.Value ? String.Empty : Convert.ToString(col["NewStatusName"]),
                             CmpAttributeId = col["AttributeId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["AttributeId"]),
                             CmpAttributeName = col["AttributeName"] == DBNull.Value ? String.Empty : Convert.ToString(col["AttributeName"]),
                         }).ToList();
                }
                else
                {
                    lstSeriesAttributeContract = new List<SeriesAttributeContract>();
                }

                Dictionary<ShotSeriesSaveResponse, List<SeriesAttributeContract>> dicResponseDictionary = new Dictionary<ShotSeriesSaveResponse, List<SeriesAttributeContract>>();
                dicResponseDictionary.Add(saveResponse, lstSeriesAttributeContract);
                return dicResponseDictionary;
            }
        }

        List<RuleDetailsForTestContract> IComplianceSetupRepository.GetSeriesRuleDetailsForShuffleTest(Int32 seriesID, Int32 selectedPackageiD)
        {
            List<RuleDetailsForTestContract> lstSeriesRuleDetailsForShuffleTestContract = new List<RuleDetailsForTestContract>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@ItemSeriesID", seriesID),
                             new SqlParameter("@SelectedPackageiD", selectedPackageiD)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetSeriesRuleDetailsForShuffleTest", sqlParameterCollection))
                {
                    GetRuleDetailsForTestContractList(lstSeriesRuleDetailsForShuffleTestContract, dr);
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstSeriesRuleDetailsForShuffleTestContract;
        }

        List<RuleDetailsForTestContract> IComplianceSetupRepository.GetDetailsForComplianceRuleTest(Int32 ruleMappingID)
        {
            List<RuleDetailsForTestContract> lstSeriesRuleDetailsForShuffleTestContract = new List<RuleDetailsForTestContract>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@RuleMappingID", ruleMappingID)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetDetailsForComplianceRuleTest", sqlParameterCollection))
                {
                    GetRuleDetailsForTestContractList(lstSeriesRuleDetailsForShuffleTestContract, dr);
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstSeriesRuleDetailsForShuffleTestContract;
        }

        private static void GetRuleDetailsForTestContractList(List<RuleDetailsForTestContract> lstSeriesRuleDetailsForShuffleTestContract, SqlDataReader dr)
        {
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    RuleDetailsForTestContract seriesRuleDetailsForShuffleTestContract = new RuleDetailsForTestContract();
                    seriesRuleDetailsForShuffleTestContract.RuleMappingID = dr["RuleMappingID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RuleMappingID"]);
                    seriesRuleDetailsForShuffleTestContract.RuleMappingDetailID = dr["RuleMappingDetailID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RuleMappingDetailID"]);
                    seriesRuleDetailsForShuffleTestContract.ConstantTpeID = dr["ConstantTpeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ConstantTpeID"]);
                    seriesRuleDetailsForShuffleTestContract.ConstantTypeCode = dr["ConstantTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ConstantTypeCode"]);
                    seriesRuleDetailsForShuffleTestContract.ConstantValue = dr["ConstantValue"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ConstantValue"]);
                    seriesRuleDetailsForShuffleTestContract.ObjectTypeID = dr["ObjectTypeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ObjectTypeID"]);
                    seriesRuleDetailsForShuffleTestContract.ObjectTypeCode = dr["ObjectTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ObjectTypeCode"]);
                    seriesRuleDetailsForShuffleTestContract.ObjectID = dr["ObjectID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ObjectID"]);
                    seriesRuleDetailsForShuffleTestContract.ObjectMappingTypeID = dr["ObjectMappingTypeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ObjectMappingTypeID"]);
                    seriesRuleDetailsForShuffleTestContract.ObjectMappingTypeCode = dr["ObjectMappingTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ObjectMappingTypeCode"]);
                    seriesRuleDetailsForShuffleTestContract.ComplianceCategoryID = dr["ComplianceCategoryID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ComplianceCategoryID"]);
                    seriesRuleDetailsForShuffleTestContract.CategoryName = dr["CategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CategoryName"]);
                    seriesRuleDetailsForShuffleTestContract.ComplianceItemID = dr["ComplianceItemID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ComplianceItemID"]);
                    seriesRuleDetailsForShuffleTestContract.ItemName = dr["ItemName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ItemName"]);
                    seriesRuleDetailsForShuffleTestContract.ComplianceAttributeID = dr["ComplianceAttributeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ComplianceAttributeID"]);
                    seriesRuleDetailsForShuffleTestContract.AttributeName = dr["AttributeName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AttributeName"]);
                    seriesRuleDetailsForShuffleTestContract.ComplianceAttributeDataTypeCode = dr["ComplianceAttributeDataTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ComplianceAttributeDataTypeCode"]);
                    seriesRuleDetailsForShuffleTestContract.OptionText = dr["OptionText"] == DBNull.Value ? String.Empty : Convert.ToString(dr["OptionText"]);
                    seriesRuleDetailsForShuffleTestContract.OptionValue = dr["OptionValue"] == DBNull.Value ? String.Empty : Convert.ToString(dr["OptionValue"]);
                    seriesRuleDetailsForShuffleTestContract.PlaceHolderName = dr["PlaceHolderName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PlaceHolderName"]);
                    seriesRuleDetailsForShuffleTestContract.UIExpression = dr["UIExpression"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UIExpression"]);
                    lstSeriesRuleDetailsForShuffleTestContract.Add(seriesRuleDetailsForShuffleTestContract);
                }
            }
        }

        /// <summary>
        /// UAT-2043:For Data Entry:  Quick Package Copy Across Tenants
        /// </summary>
        /// <param name="TenantId"></param>
        /// <param name="compliancePackageID"></param>
        /// <param name="compliancePackageName"></param>
        /// <param name="currentUserId"></param>
        /// <param name="SelectedtenantId"></param>
        public void CopyPackageStructureToOtherClient(Int32 TenantId, Int32 compliancePackageID, String compliancePackageName, Int32 currentUserId, Int32 SelectedtenantId)
        {
            SecurityContext.CopyTrackingPackage(TenantId, compliancePackageID, compliancePackageName, currentUserId, SelectedtenantId);
        }
        #region UAT-2159 : Show Category Explanatory note as a mouseover on the category name on the student data entry screen.
        Dictionary<Int32, String> IComplianceSetupRepository.GetExplanatoryNotesForCategory(Int32 packageId)
        {
            Dictionary<Int32, String> dicCatExplanatoryNotes = new Dictionary<Int32, String>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@packageId", packageId)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetExplanatoryNotesForCategory", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Int32 categoryId = dr["CategoryID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["CategoryID"]);
                            String explanatoryNotes = dr["ExplanatoryNotes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ExplanatoryNotes"]);
                            dicCatExplanatoryNotes.Add(categoryId, explanatoryNotes);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return dicCatExplanatoryNotes;
        }
        #endregion

        public List<CopyDataQueue> GetDataForCopyToRequirement(Int32 trackingItmDataObjTypeID, Int32 trackingSubsDataObjTypeID, Int32 rotSubsObjTypeID, Int32 chunkSize)
        {
            return _ClientDBContext.CopyDataQueues
                                    .Where(cond => !cond.CDQ_IsCopied
                                            && !cond.CDQ_IsDeleted
                                            && cond.CDQ_IsActive
                                            && (cond.CDQ_SourceObjectTypeID == trackingItmDataObjTypeID || cond.CDQ_SourceObjectTypeID == trackingSubsDataObjTypeID)
                                            && cond.CDQ_TargetObjectTypeID == rotSubsObjTypeID
                                            )
                                    .OrderBy(cond => cond.CDQ_SourceObjectID)
                                    .ThenBy(cond => cond.CDQ_TargetObjectID)
                                    .Take(chunkSize).ToList();
        }

        public DataTable CopyComplianceDataToRequirement(Int32 LoggedInUserID, String ItemDataIds, String RPSIds)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_CopyTrackingDataToRequirement", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", LoggedInUserID);
                command.Parameters.AddWithValue("@ComplianceItemIds", ItemDataIds);
                command.Parameters.AddWithValue("@RPSIds", RPSIds);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        String IComplianceSetupRepository.GetComplianceAttributeDatatypeByAttributeID(Int32 tenantID, Int32 complianceAttrID)
        {
            return _ClientDBContext.ComplianceAttributes.Where(x => x.ComplianceAttributeID == complianceAttrID && x.IsActive && !x.IsDeleted).FirstOrDefault().lkpComplianceAttributeDatatype.Code;
        }


        public List<UniversalCategoryMapping> GetUniversalCategoryMappings(String universalMappingTypeCode)
        {
            return ClientDBContext.UniversalCategoryMappings.Include("lkpUniversalMappingType").Where(cond => !cond.UCM_IsDeleted
                                                                                                    && cond.lkpUniversalMappingType.LUMT_Code == universalMappingTypeCode).ToList();
        }

        public List<UniversalItemMapping> GetUniversalItemMappings(List<Int32> universalCategoryIds)
        {
            return ClientDBContext.UniversalItemMappings.Where(cond => !cond.UIM_IsDeleted
                                                                    && universalCategoryIds.Contains(cond.UIM_UniversalCategoryMappingID)).ToList();
        }

        public List<UniversalAttributeMapping> GetUniversalAttributeMappings(List<Int32> universalItemIds)
        {
            return ClientDBContext.UniversalAttributeMappings.Where(cond => !cond.UAM_IsDeleted
                                                                    && universalItemIds.Contains(cond.UAM_UniversalItemMappingID)).ToList();
        }

        public List<UniversalAttributeOptionMapping> GetUniversalAttributeOptionMappings(List<Int32> universalAttrIds)
        {
            return ClientDBContext.UniversalAttributeOptionMappings.Where(cond => !cond.UAOM_IsDeleted
                                                                    && universalAttrIds.Contains(cond.UAOM_UniversalAttributeMappingID)).ToList();
        }

        public UniversalCategoryMapping CopyUniversalCategorMaapingyToClient(Int32 categoryId, UniversalCategoryMapping universalCategoryMapping, Int32 currentUserId)
        {
            UniversalCategoryMapping universalCategoryMappingInDb = ClientDBContext.UniversalCategoryMappings.FirstOrDefault(con => con.UCM_CategoryID == categoryId
                                                                                                                              && !con.UCM_IsDeleted);
            if (universalCategoryMappingInDb.IsNullOrEmpty())
            {
                universalCategoryMappingInDb = new UniversalCategoryMapping();
                universalCategoryMappingInDb.UCM_UniversalMappingTypeID = universalCategoryMapping.UCM_UniversalMappingTypeID;
                universalCategoryMappingInDb.UCM_UniversalCategoryID = universalCategoryMapping.UCM_UniversalCategoryID;
                universalCategoryMappingInDb.UCM_CategoryID = categoryId;
                universalCategoryMappingInDb.UCM_IsDeleted = false;
                universalCategoryMappingInDb.UCM_CreatedOn = DateTime.Now;
                universalCategoryMappingInDb.UCM_CreatedBy = currentUserId;
                ClientDBContext.UniversalCategoryMappings.AddObject(universalCategoryMappingInDb);
                ClientDBContext.SaveChanges();
            }
            return universalCategoryMappingInDb;
        }

        public UniversalItemMapping CopyUniversalItemMaapingToClient(Int32 catItemMaapingId, Int32 universalCategoryId, UniversalItemMapping universalItemMapping, Int32 currentUserId)
        {
            UniversalItemMapping universalItemMappingInDb = ClientDBContext.UniversalItemMappings.FirstOrDefault(con => con.UIM_CategoryItemMappingID == catItemMaapingId
                                                                                                                      && con.UIM_UniversalCategoryMappingID == universalCategoryId
                                                                                                                      && !con.UIM_IsDeleted);
            if (universalItemMappingInDb.IsNullOrEmpty())
            {
                universalItemMappingInDb = new UniversalItemMapping();
                universalItemMappingInDb.UIM_UniversalCategoryMappingID = universalCategoryId;
                universalItemMappingInDb.UIM_UniversalCategoryItemMappingID = universalItemMapping.UIM_UniversalCategoryItemMappingID;
                universalItemMappingInDb.UIM_CategoryItemMappingID = catItemMaapingId;
                universalItemMappingInDb.UIM_IsDeleted = false;
                universalItemMappingInDb.UIM_CreatedOn = DateTime.Now;
                universalItemMappingInDb.UIM_CreatedBy = currentUserId;
                ClientDBContext.UniversalItemMappings.AddObject(universalItemMappingInDb);
                ClientDBContext.SaveChanges();
            }
            return universalItemMappingInDb;
        }

        public UniversalAttributeMapping CopyUniversalAttrMaapingToClient(Int32 attrItemMaapingId, Int32 universalItemId, UniversalAttributeMapping universalAttrMappingInMaster
                                                                           , Int32 currentUserId, List<UniversalAttributeOptionMapping> lstUniversalAttOptionMappingInMaster
                                                                           , ComplianceAttribute cmpAttributeInMaster, ComplianceAttribute cmpAttributeInClient)
        {
            UniversalAttributeMapping universalAttrMappingInDb = ClientDBContext.UniversalAttributeMappings.FirstOrDefault(con => con.UAM_UniversalItemMappingID == universalItemId
                                                                                                                      && con.UAM_ItemAttributeMappingID == attrItemMaapingId
                                                                                                                      && !con.UAM_IsDeleted);
            if (universalAttrMappingInDb.IsNullOrEmpty())
            {
                universalAttrMappingInDb = new UniversalAttributeMapping();
                universalAttrMappingInDb.UAM_UniversalItemMappingID = universalItemId;
                universalAttrMappingInDb.UAM_UniversalItemAttributeMappingID = universalAttrMappingInMaster.UAM_UniversalItemAttributeMappingID;
                universalAttrMappingInDb.UAM_ItemAttributeMappingID = attrItemMaapingId;
                universalAttrMappingInDb.UAM_IsDeleted = false;
                universalAttrMappingInDb.UAM_CreatedOn = DateTime.Now;
                universalAttrMappingInDb.UAM_CreatedBy = currentUserId;

                List<UniversalAttributeInputTypeMapping> lstUniversalAttributeInputTypeMapping = universalAttrMappingInMaster.UniversalAttributeInputTypeMappings.Where(cond => !cond.UAITM_IsDeleted).ToList();
                foreach (UniversalAttributeInputTypeMapping mapping in lstUniversalAttributeInputTypeMapping)
                {
                    UniversalAttributeInputTypeMapping inputMapping = new UniversalAttributeInputTypeMapping();
                    inputMapping.UAITM_UniversalItemAttributeMappingID = mapping.UAITM_UniversalItemAttributeMappingID;
                    inputMapping.UAITM_InputPriority = mapping.UAITM_InputPriority;
                    inputMapping.UAITM_IsDeleted = false;
                    inputMapping.UAITM_CreatedOn = DateTime.Now;
                    inputMapping.UAITM_CreatedBy = currentUserId;
                    universalAttrMappingInDb.UniversalAttributeInputTypeMappings.Add(inputMapping);
                }
                ClientDBContext.UniversalAttributeMappings.AddObject(universalAttrMappingInDb);
                ClientDBContext.SaveChanges();

                List<ComplianceAttributeOption> lstAttrOptionInMaster = cmpAttributeInMaster.ComplianceAttributeOptions.Where(sel => !sel.IsDeleted).ToList();
                List<ComplianceAttributeOption> lstAttrOptionInClient = cmpAttributeInClient.ComplianceAttributeOptions.Where(sel => !sel.IsDeleted).ToList();

                List<UniversalAttributeOptionMapping> lstUniversalAttOptionMappings = lstUniversalAttOptionMappingInMaster.Where(cond => cond.UAOM_UniversalAttributeMappingID == universalAttrMappingInMaster.UAM_ID).ToList();
                foreach (UniversalAttributeOptionMapping uniAttrOptionInMaster in lstUniversalAttOptionMappings)
                {
                    ComplianceAttributeOption attrOptionInMaster = lstAttrOptionInMaster.FirstOrDefault(cond => cond.ComplianceAttributeOptionID == uniAttrOptionInMaster.UAOM_AttributeOptionID);
                    if (!attrOptionInMaster.IsNullOrEmpty())
                    {
                        ComplianceAttributeOption attrOptionInClient = lstAttrOptionInClient.FirstOrDefault(cond => cond.OptionValue == attrOptionInMaster.OptionValue);
                        UniversalAttributeOptionMapping newAttributeOptionMapping = new UniversalAttributeOptionMapping();
                        newAttributeOptionMapping.UAOM_UniversalAttributeOptionID = uniAttrOptionInMaster.UAOM_UniversalAttributeOptionID;
                        newAttributeOptionMapping.UAOM_UniversalAttributeMappingID = universalAttrMappingInDb.UAM_ID;
                        newAttributeOptionMapping.UAOM_AttributeOptionID = attrOptionInClient.ComplianceAttributeOptionID;
                        newAttributeOptionMapping.UAOM_IsDeleted = false;
                        newAttributeOptionMapping.UAOM_CreatedOn = DateTime.Now;
                        newAttributeOptionMapping.UAOM_CreatedBy = currentUserId;
                        ClientDBContext.UniversalAttributeOptionMappings.AddObject(newAttributeOptionMapping);
                    }
                }
                ClientDBContext.SaveChanges();
            }
            return universalAttrMappingInDb;
        }

        #region UAT:2411
        public List<InstitutionConfigurationPackageDetails> GetInstitutionConfigurationBundlePackageDetailsList(Int32 bundlePackageID, Int32 hierarchyID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetInstitutionConfigurationBundlePackageDetailsData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BundleID", bundlePackageID);
                command.Parameters.AddWithValue("@HierarchyID", hierarchyID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                List<InstitutionConfigurationPackageDetails> PackageDetailsList = new List<InstitutionConfigurationPackageDetails>();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    PackageDetailsList = ds.Tables[0].AsEnumerable().Select(col =>
                          new InstitutionConfigurationPackageDetails
                          {
                              PackageID = Convert.ToInt32(col["PackageID"]),
                              PackageName = Convert.ToString(col["PackageName"]),
                              PaymentMethods = col["PaymentMethods"] == DBNull.Value ? String.Empty : Convert.ToString(col["PaymentMethods"]),
                              PackageType = Convert.ToString(col["PackageType"]),
                              IsCompliancePackage = Convert.ToBoolean(col["IsCompliancePackage"]),
                              // IsParentPackage = Convert.ToBoolean(col["IsParentPackage"]),
                              Fee = col["Fee"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(col["Fee"]),
                              SubscriptionOption = col["SubscriptionOption"] == DBNull.Value ? String.Empty : Convert.ToString(col["SubscriptionOption"]),
                              PackageHierarchyID = Convert.ToInt32(col["PackageHierarchyID"]),
                          }).ToList();
                }
                else
                {
                    PackageDetailsList = new List<InstitutionConfigurationPackageDetails>();
                }

                return PackageDetailsList;
            }
        }
        #endregion

        //UAT-2339
        ObjectResult<GetDepartmentTree> IComplianceSetupRepository.GetInstituteHierarchyTreewithPermissions(Int32? currentUserID)
        {
            return _ClientDBContext.GetInstituteHierarchyTreeCommon(currentUserID, string.Empty);
        }
        //UAT 2506
        public List<AdminDataAuditHistory> GetAdminDocumentDataAuditHistory(AdminDataAuditHistory parameterContact, CustomPagingArgsContract customPagingArgsContract)
        {
            List<AdminDataAuditHistory> adminDataAuditHistoryList = new List<AdminDataAuditHistory>();
            String orderBy = "CreatedOn";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@DocumentName", parameterContact.DocumentName),
                            new SqlParameter("@ApplicantFirstName", parameterContact.ApplicantFirstName),
                            new SqlParameter("@ApplicantLastName", parameterContact.ApplicantLastName),
                            new SqlParameter("@AdminFirstName", parameterContact.AdminFirstName),
                            new SqlParameter("@AdminLastName", parameterContact.AdminLastName),
                            new SqlParameter("@SelectedTenantId", parameterContact.SelectedTenantID),
                            new SqlParameter("@LoggedInUserId", parameterContact.AdminLoggedInUserID),
                            new SqlParameter("@ActionType", parameterContact.ActionTypeID),
                            new SqlParameter("@DiscardReason", parameterContact.DiscardReasonId),
                            new SqlParameter("@FromDate", parameterContact.FromDate),
                            new SqlParameter("@ToDate", parameterContact.ToDate),
                            new SqlParameter("@OrderBy", orderBy),
                            new SqlParameter("@OrderDirection", ordDirection),
                            new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                            new SqlParameter("@PageSize", customPagingArgsContract.PageSize)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAdminDataAuditHistory", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            AdminDataAuditHistory adminDataAuditHistory = new AdminDataAuditHistory();
                            adminDataAuditHistory.QueueRecordID = dr["QueueRecordID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QueueRecordID"]);
                            adminDataAuditHistory.ApplicantDocumentID = dr["ApplicantDocumentID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ApplicantDocumentID"]);
                            adminDataAuditHistory.DocumentName = dr["DocumentName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DocumentName"]);
                            adminDataAuditHistory.ApplicantFirstName = dr["ApplicantFirstName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ApplicantFirstName"]);
                            adminDataAuditHistory.ApplicantLastName = dr["ApplicantLastName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ApplicantLastName"]);
                            adminDataAuditHistory.AdminFirstName = dr["AdminFirstName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AdminFirstName"]);
                            adminDataAuditHistory.AdminLastName = dr["AdminLastName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AdminLastName"]);
                            adminDataAuditHistory.ActionType = dr["ActionType"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ActionType"]);
                            adminDataAuditHistory.Changes = dr["Changes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Changes"]);
                            adminDataAuditHistory.DiscardReasonId = dr["DiscardReasonId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DiscardReasonId"]);
                            adminDataAuditHistory.DiscardReason = dr["DiscardReason"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DiscardReason"]);
                            adminDataAuditHistory.DiscardNote = dr["DiscardNote"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DiscardNote"]);
                            adminDataAuditHistory.CreatedById = dr["CreatedById"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CreatedById"]);
                            adminDataAuditHistory.CreatedOn = dr["CreatedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["CreatedOn"]);
                            adminDataAuditHistory.School = dr["School"] == DBNull.Value ? String.Empty : Convert.ToString(dr["School"]);
                            adminDataAuditHistory.AssignOn = dr["AssignOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["AssignOn"]);
                            adminDataAuditHistory.AssignToName = dr["AssignToName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AssignToName"]);
                            adminDataAuditHistory.AssignByName = dr["AssignByName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AssignByName"]);
                            adminDataAuditHistory.TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0;
                            adminDataAuditHistoryList.Add(adminDataAuditHistory);
                        }
                    }
                }
            }
            return adminDataAuditHistoryList;
        }
        public List<AdminDataAuditHistory> GetDocumentAssignmentAuditHistory(AdminDataAuditHistory parameterContact)
        {
            var dataList = base.SecurityContext.FlatDataEntryQueueHistories.Where(a => a.FQEQH_FlatQueueID == parameterContact.QueueRecordID && a.FDEQH_TenantID == parameterContact.SelectedTenantID).ToList();
            List<AdminDataAuditHistory> objListDocumentAsignmentAuditHistory = new List<AdminDataAuditHistory>();
            foreach (var item in dataList)
            {
                AdminDataAuditHistory objSingleItem = new AdminDataAuditHistory();
                objSingleItem.QueueRecordID = item.FQEQH_FlatQueueID;
                objSingleItem.AssignToUserName = item.FDEQH_AssignToUserName;
                objSingleItem.AssignToUserID = item.FDEQH_AssignToUserID;
                objSingleItem.AssignmentDate = item.FDEQH_CreatedOn;
                objListDocumentAsignmentAuditHistory.Add(objSingleItem);
            }
            return objListDocumentAsignmentAuditHistory;
        }

        //UAT-2717
        public List<CompliancePackage> GetTenantCompliancePackage(Int32 tenantId)
        {
            List<CompliancePackage> compliancePackages = new List<CompliancePackage>();
            if (!tenantId.IsNullOrEmpty() && tenantId > AppConsts.NONE)
            {
                compliancePackages = ClientDBContext.CompliancePackages.Where(obj => obj.IsDeleted == false).ToList();
            }
            return compliancePackages;
        }

        //UAT-2386
        List<DeptProgramMapping> IComplianceSetupRepository.GetChildNodesByNodeID(Int32 NodeID)
        {
            List<DeptProgramMapping> lstDeptProgramMapping = new List<DeptProgramMapping>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@NodeID", NodeID)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetChildNodesbyPartentNodeID", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            DeptProgramMapping DeptProgramMapping = new DeptProgramMapping();
                            DeptProgramMapping.DPM_ID = dr["DPM_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["DPM_ID"]);
                            DeptProgramMapping.DPM_Label = dr["DPM_Label"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DPM_Label"]);
                            lstDeptProgramMapping.Add(DeptProgramMapping);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstDeptProgramMapping;
        }

        //UAT-2744
        List<GetDepartmentTree> IComplianceSetupRepository.GetInstituteHierarchyPackageTree(Int32? orgUserID, String compliancePackageTypeCode, Boolean isCompliancePackage, Boolean fetchNoAccessNodes)
        {
            List<GetDepartmentTree> lstDeptTree = new List<GetDepartmentTree>();
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@OrgUserID", orgUserID),
                             new SqlParameter("@compliancePackageTypeCode", compliancePackageTypeCode),
                             new SqlParameter("@IsCompliancePackage", isCompliancePackage),
                             new SqlParameter("@fetchNoAccessNodes", fetchNoAccessNodes)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetInstituteHierarchyPackageTree", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            GetDepartmentTree getDepartmentTree = new GetDepartmentTree();
                            getDepartmentTree.TreeNodeTypeID = dr["TreeNodeTypeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["TreeNodeTypeID"]);
                            getDepartmentTree.NodeID = dr["NodeID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["NodeID"]);
                            getDepartmentTree.Level = dr["Level"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["Level"]); ;
                            getDepartmentTree.DataID = dr["DataID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["DataID"]);
                            getDepartmentTree.Value = dr["Value"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Value"]);
                            getDepartmentTree.UICode = dr["UICode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UICode"]);
                            getDepartmentTree.IsLabel = dr["IsLabel"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsLabel"]);
                            getDepartmentTree.Associated = dr["Associated"] == DBNull.Value ? false : Convert.ToBoolean(dr["Associated"]);
                            getDepartmentTree.MappingID = dr["MappingID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["MappingID"]);
                            getDepartmentTree.EntityID = dr["EntityID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["EntityID"]); ;
                            getDepartmentTree.ParentNodeID = dr["ParentNodeID"] == DBNull.Value ? null : Convert.ToString(dr["ParentNodeID"]);
                            lstDeptTree.Add(getDepartmentTree);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstDeptTree;
        }


        public DataTable GetAutomaticPackageInvitations(Int32 chunkSize)
        {
            EntityConnection connection = ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[dbo].[usp_GetAutomaticPackageInvitations]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();
        }

        public Boolean UpdateAutomaticPackageInvitationsEmailStatus(List<Int32> AIP_Ids, Int32 backgroundProcessUserId)
        {
            var automaticPackageInvitationMailLogsList = ClientDBContext.AutomaticPackageInvitations.Where(cnd => AIP_Ids.Contains(cnd.AIP_ID)).ToList();
            automaticPackageInvitationMailLogsList.ForEach(s =>
            {
                s.AIP_EmailSentStatusID = AppConsts.ONE;
                s.AIP_ModifiedOn = DateTime.Now;
                s.AIP_ModifiedBy = backgroundProcessUserId;
            });  //1 = Send, 2= Pending

            ClientDBContext.SaveChanges();
            return true;
        }

        //UAT-2924: Add upcoming expirations to Since You Been Gone popup as part of the not compliant categories
        public List<UpcomingCategoryExpirationContract> GetUpcomingExpirationcategoryByLoginId(Int32 currentUserID)
        {
            List<UpcomingCategoryExpirationContract> upcomingExpirationCategoies = new List<UpcomingCategoryExpirationContract>();
            upcomingExpirationCategoies = ClientDBContext.vwUpcomingExpirations.Where(obj => obj.StudentID == currentUserID && obj.NonComplianceDate != null)
                                        .Select(a => new UpcomingCategoryExpirationContract
                                        {
                                            Category = a.Category,
                                            CategoryComplianceExpiryDate = (a.NonComplianceDate),
                                            //InstitutionHierarchyLabel = (a.DPM_Label)
                                        }).Distinct().ToList();
            return upcomingExpirationCategoies;
        }




        #region UAT-2985:
        public UniversalFieldMapping CopyUniversalFieldMaapingToClient(Int32 itemAttributeMappingId, Int32 categoryItemMappingId, Int32 universalFieldID, UniversalFieldMapping universalFieldMappingInMaster
                                                                           , Int32 currentUserId, List<UniversalFieldOptionMapping> lstUniversalFieldOptionMappingInMaster
                                                                           , ComplianceAttribute cmpAttributeInMaster, ComplianceAttribute cmpAttributeInClient)
        {
            UniversalFieldMapping universalFieldMappingInDb = ClientDBContext.UniversalFieldMappings.FirstOrDefault(con => con.UFM_CategoryItemMappingID == categoryItemMappingId
                                                                                                                      && con.UFM_ItemAttributeMappingID == itemAttributeMappingId
                                                                                                                      && con.UFM_UniversalFieldID == universalFieldID
                                                                                                                      && con.lkpUniversalMappingType.LUMT_Code == "AAAA"
                                                                                                                      && !con.UFM_IsDeleted);
            if (universalFieldMappingInDb.IsNullOrEmpty())
            {
                universalFieldMappingInDb = new UniversalFieldMapping();
                universalFieldMappingInDb.UFM_CategoryItemMappingID = categoryItemMappingId;
                universalFieldMappingInDb.UFM_UniversalFieldID = universalFieldMappingInMaster.UFM_UniversalFieldID;
                universalFieldMappingInDb.UFM_ItemAttributeMappingID = itemAttributeMappingId;
                universalFieldMappingInDb.UFM_UniversalMappingTypeID = universalFieldMappingInMaster.UFM_UniversalMappingTypeID;
                universalFieldMappingInDb.UFM_IsDeleted = false;
                universalFieldMappingInDb.UFM_CreatedOn = DateTime.Now;
                universalFieldMappingInDb.UFM_CreatedBy = currentUserId;

                List<UniversalFieldInputTypeMapping> lstUniversalFieldInputTypeMapping = universalFieldMappingInMaster.UniversalFieldInputTypeMappings.Where(cond => !cond.UFITM_IsDeleted).ToList();
                foreach (UniversalFieldInputTypeMapping mapping in lstUniversalFieldInputTypeMapping)
                {
                    UniversalFieldInputTypeMapping inputMapping = new UniversalFieldInputTypeMapping();
                    inputMapping.UFITM_UniversalFieldID = mapping.UFITM_UniversalFieldID;
                    inputMapping.UFITM_InputPriority = mapping.UFITM_InputPriority;
                    inputMapping.UFITM_IsDeleted = false;
                    inputMapping.UFITM_CreatedOn = DateTime.Now;
                    inputMapping.UFITM_CreatedBy = currentUserId;
                    universalFieldMappingInDb.UniversalFieldInputTypeMappings.Add(inputMapping);
                }
                //ClientDBContext.UniversalFieldMappings.AddObject(universalFieldMappingInDb);
                //ClientDBContext.SaveChanges();

                List<ComplianceAttributeOption> lstAttrOptionInMaster = cmpAttributeInMaster.ComplianceAttributeOptions.Where(sel => !sel.IsDeleted).ToList();
                List<ComplianceAttributeOption> lstAttrOptionInClient = cmpAttributeInClient.ComplianceAttributeOptions.Where(sel => !sel.IsDeleted).ToList();

                List<UniversalFieldOptionMapping> lstUniversalFieldOptionMappings = lstUniversalFieldOptionMappingInMaster.Where(cond => cond.UFOM_UniversalFieldMappingID == universalFieldMappingInMaster.UFM_ID).ToList();
                foreach (UniversalFieldOptionMapping uniFieldOptionInMaster in lstUniversalFieldOptionMappings)
                {
                    ComplianceAttributeOption attrOptionInMaster = lstAttrOptionInMaster.FirstOrDefault(cond => cond.ComplianceAttributeOptionID == uniFieldOptionInMaster.UFOM_AttributeOptionID);
                    if (!attrOptionInMaster.IsNullOrEmpty())
                    {
                        ComplianceAttributeOption attrOptionInClient = lstAttrOptionInClient.FirstOrDefault(cond => cond.OptionValue == attrOptionInMaster.OptionValue);
                        UniversalFieldOptionMapping newFieldOptionMapping = new UniversalFieldOptionMapping();
                        newFieldOptionMapping.UFOM_UniversalFieldOptionID = uniFieldOptionInMaster.UFOM_UniversalFieldOptionID;
                        //newFieldOptionMapping.UFOM_UniversalFieldMappingID = universalFieldMappingInDb.UFM_ID;
                        newFieldOptionMapping.UFOM_AttributeOptionID = attrOptionInClient.ComplianceAttributeOptionID;
                        newFieldOptionMapping.UFOM_IsDeleted = false;
                        newFieldOptionMapping.UFOM_CreatedOn = DateTime.Now;
                        newFieldOptionMapping.UFOM_CreatedBy = currentUserId;
                        universalFieldMappingInDb.UniversalFieldOptionMappings.Add(newFieldOptionMapping);
                        //ClientDBContext.UniversalFieldOptionMappings.AddObject(newFieldOptionMapping);
                    }
                }
                ClientDBContext.UniversalFieldMappings.AddObject(universalFieldMappingInDb);
                ClientDBContext.SaveChanges();
            }
            return universalFieldMappingInDb;
        }



        public List<UniversalFieldMapping> GetUniversalFieldMappings(String mappingTypeCode)
        {
            return ClientDBContext.UniversalFieldMappings.Include("lkpUniversalMappingType").Where(cond => !cond.UFM_IsDeleted
                                                                                                               && cond.lkpUniversalMappingType.LUMT_Code == mappingTypeCode).ToList();
        }


        public List<UniversalFieldOptionMapping> GetUniversalFieldOptionMappings(List<Int32> UFM_Ids)
        {
            return ClientDBContext.UniversalFieldOptionMappings.Where(cond => !cond.UFOM_IsDeleted
                                                                              && UFM_Ids.Contains(cond.UFOM_UniversalFieldMappingID)).ToList();
        }
        #endregion

        public void GetCategoryItemAttributeMappingID(Int32 complianceCategoryID, Int32 complianceItemID, Int32 complianceAttributeID, ref Int32 complianceCategoryItemID, ref Int32 complianceItemAttributeID)
        {
            complianceCategoryItemID = 0;
            complianceItemAttributeID = 0;

            var complianceCategoryItem = ClientDBContext.ComplianceCategoryItems.Where(cond => cond.CCI_CategoryID == complianceCategoryID
                                                                                       && cond.CCI_ItemID == complianceItemID
                                                                                       && !cond.CCI_IsDeleted
                                                                                       && cond.CCI_IsActive
                                                                               ).FirstOrDefault();

            if (!complianceCategoryItem.IsNullOrEmpty())
                complianceCategoryItemID = complianceCategoryItem.CCI_ID;

            var complianceItemAttribute = ClientDBContext.ComplianceItemAttributes.Where(cond => cond.CIA_ItemID == complianceItemID
                                                                                       && cond.CIA_AttributeID == complianceAttributeID
                                                                                       && !cond.CIA_IsDeleted
                                                                                       && cond.CIA_IsActive
                                                                               ).FirstOrDefault();

            if (!complianceItemAttribute.IsNullOrEmpty())
                complianceItemAttributeID = complianceItemAttribute.CIA_ID;
        }

        private Boolean InsertExemptedHierarchyNode(Int32 hierarchyNodeID, String hierarchyNodeExemptedTypeCode, Int32 currentUserId)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("ams.usp_InsertExemptedHierarchyNode", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@HierarchyNodeID", hierarchyNodeID);
                command.Parameters.AddWithValue("@NodeExemptedInRotationTypeCode", hierarchyNodeExemptedTypeCode);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentUserId);

                if (con.State == ConnectionState.Closed)
                    con.Open();
                command.ExecuteNonQuery();
                con.Close();

                return true;

            }
            return false;
        }

        public List<PackageBundleContract> GetPackageIncludedInBundle(Int32 ID)
        {

            var listBackgroundBundlePackage = ClientDBContext.PackageBundleNodePackages.Include("BkgPackageHierarchyMapping").Include("BackgroundPackage").Where(x => x.PBNP_PackageBundleID == ID
                                                                                                && x.PBNP_IsDeleted == false && x.BkgPackageHierarchyMapping != null && x.BkgPackageHierarchyMapping.BPHM_IsDeleted == false && x.BkgPackageHierarchyMapping.BPHM_IsActive == true && x.BkgPackageHierarchyMapping.BackgroundPackage.BPA_IsDeleted == false && x.BkgPackageHierarchyMapping.BackgroundPackage.BPA_IsActive == true)
                                                                                                .Select(g => new PackageBundleContract { PackageName = (String.IsNullOrEmpty(g.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Name) ? g.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Label : g.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Name), PackageType = "Screening" }).OrderBy(x => x.PackageName).ToList();



            var listComplianceBundlePackage = ClientDBContext.PackageBundleNodePackages.Include("DeptProgramPackages").Include("CompliancePackage").Where(x => x.PBNP_PackageBundleID == ID
                                                                                               && x.PBNP_IsDeleted == false && x.DeptProgramPackage != null && x.DeptProgramPackage.DPP_IsDeleted == false && x.DeptProgramPackage.CompliancePackage.IsDeleted == false && x.DeptProgramPackage.CompliancePackage.IsActive == true)
                                                                                               .Select(g => new PackageBundleContract { PackageName = (string.IsNullOrEmpty(g.DeptProgramPackage.CompliancePackage.PackageName) ? g.DeptProgramPackage.CompliancePackage.PackageLabel : g.DeptProgramPackage.CompliancePackage.PackageName), PackageType = (g.DeptProgramPackage.CompliancePackage.lkpCompliancePackageType.CPT_Code == "IMNZ" ? "Immunization" : "Administrative") }).OrderBy(x => x.PackageName).ToList();


            return listComplianceBundlePackage.Union(listBackgroundBundlePackage).ToList();
        }
        #region UAT-3566
        public String GetLastRuleAppliedDate(Int32 associationHierarchyId)
        {
            String ruleAppliedDate = String.Empty;
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetLatestRuleAppliedDate", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@assignmentHierarchyId", associationHierarchyId);

                if (con.State == ConnectionState.Closed)
                    con.Open();

                var result = command.ExecuteScalar();

                if (!result.IsNullOrEmpty())
                    ruleAppliedDate = Convert.ToString(Convert.ToDateTime(result).ToShortDateString());
                else
                    ruleAppliedDate = Convert.ToString(result);
                con.Close();
                return ruleAppliedDate;
            }

        }
        #endregion

        public PackageBundleNodeMapping GetPackageBundleNodeMapping(Int32 packageBundleId, Int32 deptProgramMappingId)
        {
            return ClientDBContext.PackageBundleNodeMappings.Where(c => c.PBNM_IsDeleted == false && c.PBNM_DeptProgramMappingID == deptProgramMappingId && c.PBNM_PackageBundleID == packageBundleId).FirstOrDefault();
        }

        public Boolean UpdatePackageBundleNodeMapping(Int32 packageBundleId, Int32 deptProgramMappingId, Boolean isBundleExclusive, Int32 currentLoggedInUserId)
        {
            PackageBundleNodeMapping packageBundleNodeMappings = ClientDBContext.PackageBundleNodeMappings.Where(c => c.PBNM_IsDeleted == false && c.PBNM_DeptProgramMappingID == deptProgramMappingId && c.PBNM_PackageBundleID == packageBundleId).FirstOrDefault();

            if (packageBundleNodeMappings.IsNotNull())
            {
                packageBundleNodeMappings.PBNM_IsExclusive = isBundleExclusive;
                packageBundleNodeMappings.PBNM_ModifiedByID = currentLoggedInUserId;
                packageBundleNodeMappings.PBNM_ModifiedOn = DateTime.Now;
                _ClientDBContext.SaveChanges();
                return true;
            }
            return false;
        }

        #region UAT-3896
        public String GetHierarchyTextForBundle(Int32 packageId)
        {
            String bundleHierarchyMapping = String.Empty;
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetBundlePackageHierarchy", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageId", packageId);

                if (con.State == ConnectionState.Closed)
                    con.Open();

                var result = command.ExecuteScalar();

                if (!result.IsNullOrEmpty())
                    bundleHierarchyMapping = Convert.ToString(result);
                else
                    bundleHierarchyMapping = Convert.ToString(result);
                con.Close();
                return bundleHierarchyMapping;
            }
        }
        #endregion

        #region UAT-3951: Rejection Reason

        IQueryable<Entity.RejectionReason> IComplianceSetupRepository.GetRejectionReasons()
        {
            return this.SecurityContext.RejectionReasons.Where(x => !x.RR_IsDeleted);
        }

        Boolean IComplianceSetupRepository.SaveUpdateRejectionReason(Entity.RejectionReason rejectionReasonData)
        {
            if (rejectionReasonData.RR_ID > 0)
            {
                Entity.RejectionReason rejectionReasonDB = this.SecurityContext.RejectionReasons.FirstOrDefault(cnd => cnd.RR_ID == rejectionReasonData.RR_ID
                                                                                                                && !cnd.RR_IsDeleted);
                if (!rejectionReasonDB.IsNullOrEmpty())
                {
                    rejectionReasonDB.RR_ReasonText = rejectionReasonData.RR_ReasonText;
                    rejectionReasonDB.RR_Name = rejectionReasonData.RR_Name;
                    rejectionReasonDB.RR_RejectionReasonCategoryID = rejectionReasonData.RR_RejectionReasonCategoryID;
                    rejectionReasonDB.RR_ModifiedBy = rejectionReasonData.RR_ModifiedBy;
                    rejectionReasonDB.RR_ModifiedOn = rejectionReasonData.RR_ModifiedOn;
                }
            }
            else
            {
                this.SecurityContext.RejectionReasons.AddObject(rejectionReasonData);
            }
            if (this.SecurityContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }


        Boolean IComplianceSetupRepository.DeleteRejectionReason(Int32 rejectionReasonID, Int32 loggedInUserID)
        {
            Entity.RejectionReason rejectionReasonDB = this.SecurityContext.RejectionReasons.FirstOrDefault(cnd => cnd.RR_ID == rejectionReasonID
                                                                                                                && !cnd.RR_IsDeleted);

            if (!rejectionReasonDB.IsNullOrEmpty())
            {

                rejectionReasonDB.RR_IsDeleted = true;
                rejectionReasonDB.RR_ModifiedBy = loggedInUserID;
                rejectionReasonDB.RR_ModifiedOn = DateTime.Now;

                if (this.SecurityContext.SaveChanges() > 0)
                {
                    return true;
                }

            }
            return false;
        }

        List<Entity.RejectionReason> IComplianceSetupRepository.GetRejectionReasonByIDs(List<Int32> lstRejectReasonIDs)
        {
            return this.SecurityContext.RejectionReasons.Where(x => lstRejectReasonIDs.Contains(x.RR_ID) && !x.RR_IsDeleted).ToList();
        }

        #endregion

        #region UAT-3873
        /// <summary>
        /// To get Program Packages by HierarchyMappingIdId
        /// </summary>
        /// <param name="deptProgramMappingID"></param>
        /// <returns></returns>
        public List<NodePackagesDetails> GetProgramAvailablePackagesByProgramMapId(Int32 deptProgramMappingID)
        {
            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetNodeBackgroundPackages", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@dpmId", deptProgramMappingID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                List<NodePackagesDetails> lstNodePackagesDetails = new List<NodePackagesDetails>();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    lstNodePackagesDetails = ds.Tables[0].AsEnumerable().Select(col =>
                       new NodePackagesDetails
                       {
                           PackageName = Convert.ToString(col["PackageName"]),
                           PackageType = Convert.ToString(col["PackageType"]),
                           PackageId = Convert.ToInt32(col["PackageId"]),
                           IsBundlePackage = Convert.ToBoolean(col["IsBundlePackage"]) == true ? "Yes" : "No",

                           //Convert.ToBoolean(col["IsBundlePackage"]),

                       }).ToList();
                }
                else
                {
                    lstNodePackagesDetails = new List<NodePackagesDetails>();
                }

                return lstNodePackagesDetails;
            }
        }
        #endregion

        public IQueryable<DeptProgramRestrictedFileExtension> GetMappedDeptProgramFileExtensions(Int32 depProgramMappingId)
        {
            return _ClientDBContext.DeptProgramRestrictedFileExtensions.Where(cond => cond.DPRFE_DeptProgramMappingID == depProgramMappingId && !cond.DPRFE_IsDeleted).AsQueryable();
        }

        //UAT-4278

        public List<DiscardedDocumentAuditContract> GetDiscardedDocumentDataAuditHistory(DiscardedDocumentAuditContract parameterContact, CustomPagingArgsContract customPagingArgsContract)
        {
            List<DiscardedDocumentAuditContract> adminDataAuditHistoryList = new List<DiscardedDocumentAuditContract>();
            String orderBy = "CreatedOn";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = base.SecurityContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@ApplicantFirstName", parameterContact.ApplicantFirstName),
                            new SqlParameter("@ApplicantLastName", parameterContact.ApplicantLastName),
                            new SqlParameter("@SelectedTenantId", parameterContact.SelectedTenantID),
                            new SqlParameter("@LoggedInUserId", parameterContact.AdminLoggedInUserID),
                            new SqlParameter("@FromDate", parameterContact.FromDate),
                            new SqlParameter("@ToDate", parameterContact.ToDate),
                            new SqlParameter("@OrderBy", orderBy),
                            new SqlParameter("@OrderDirection", ordDirection),
                            new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                            new SqlParameter("@PageSize", customPagingArgsContract.PageSize)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAdminDiscardedDocumentAuditHistory", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            DiscardedDocumentAuditContract discardedDocumentAuditContract = new DiscardedDocumentAuditContract();
                            discardedDocumentAuditContract.QueueRecordID = dr["QueueRecordID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["QueueRecordID"]);
                            discardedDocumentAuditContract.DocumentName = dr["DocumentName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DocumentName"]);
                            discardedDocumentAuditContract.ApplicantFirstName = dr["ApplicantFirstName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ApplicantFirstName"]);
                            discardedDocumentAuditContract.ApplicantLastName = dr["ApplicantLastName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ApplicantLastName"]);
                            discardedDocumentAuditContract.AdminFirstName = dr["AdminFirstName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AdminFirstName"]);
                            discardedDocumentAuditContract.AdminLastName = dr["AdminLastName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AdminLastName"]);
                            discardedDocumentAuditContract.DiscardedReason = dr["DiscardReason"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DiscardReason"]);
                            discardedDocumentAuditContract.DiscardByUserID = dr["DiscardByUserID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DiscardByUserID"]);
                            discardedDocumentAuditContract.DiscardedOn = dr["DiscardOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["DiscardOn"]);
                            discardedDocumentAuditContract.InstitutionName = dr["InstitutionName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["InstitutionName"]);
                            discardedDocumentAuditContract.TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0;
                            discardedDocumentAuditContract.DiscardedCount = dr["DiscardedCount"] != DBNull.Value ? Convert.ToInt32(dr["DiscardedCount"]) : 0;
                            adminDataAuditHistoryList.Add(discardedDocumentAuditContract);
                        }
                    }
                }
            }
            return adminDataAuditHistoryList;
        }

        #region Admin Entry Portal

        public List<DeptProgramAdminEntryAcctSetting> GetDeptProgramAdminEntryAcctSettings(Int32 depProgramMappingId)
        {
            return _ClientDBContext.DeptProgramAdminEntryAcctSettings.Where(xx => xx.DPAEAS_IsDeleted == false && xx.DPAEAS_DeptProgramMappingID == depProgramMappingId).ToList();
        }

        public bool SaveNodeSettingsForAdminEntry(Int32 depProgramMappingId, List<DeptProgramAdminEntryAcctSetting> deptProgramAdminEntryAcctSettingList)
        {
            var isRecordAvailable = _ClientDBContext.DeptProgramAdminEntryAcctSettings.Where(xx => xx.DPAEAS_IsDeleted == false).Any(x => x.DPAEAS_DeptProgramMappingID == depProgramMappingId);
            if (isRecordAvailable)
            {
                //to update entries for which each record that is updated
                List<DeptProgramAdminEntryAcctSetting> listAlreadySaved = _ClientDBContext.DeptProgramAdminEntryAcctSettings.Where(xx => xx.DPAEAS_IsDeleted == false && xx.DPAEAS_DeptProgramMappingID == depProgramMappingId).ToList();

                foreach (var item in deptProgramAdminEntryAcctSettingList)
                {
                    DeptProgramAdminEntryAcctSetting deptProgramAdminEntryAcctSetting = listAlreadySaved.Where(x => x.DPAEAS_AdminEntryAccountSettingID == item.DPAEAS_AdminEntryAccountSettingID).FirstOrDefault();
                    bool isValueChanged = deptProgramAdminEntryAcctSetting.DPAEAS_SettingValue.Equals(item.DPAEAS_SettingValue);
                    //if (!isValueChanged)
                    //{
                    deptProgramAdminEntryAcctSetting.DPAEAS_SettingValue = item.DPAEAS_SettingValue;
                    deptProgramAdminEntryAcctSetting.DPAEAS_ModifiedOn = DateTime.Now;
                    deptProgramAdminEntryAcctSetting.DPAEAS_ModifiedBy = item.DPAEAS_CreatedBy;
                    //}
                }
            }
            else
            {
                //add case
                foreach (var item in deptProgramAdminEntryAcctSettingList)
                {
                    DeptProgramAdminEntryAcctSetting deptProgramAdminEntryAcctSetting = new DeptProgramAdminEntryAcctSetting();
                    deptProgramAdminEntryAcctSetting.DPAEAS_AdminEntryAccountSettingID = item.DPAEAS_AdminEntryAccountSettingID;
                    deptProgramAdminEntryAcctSetting.DPAEAS_DeptProgramMappingID = item.DPAEAS_DeptProgramMappingID;
                    deptProgramAdminEntryAcctSetting.DPAEAS_SettingValue = item.DPAEAS_SettingValue;
                    deptProgramAdminEntryAcctSetting.DPAEAS_IsDeleted = item.DPAEAS_IsDeleted;
                    deptProgramAdminEntryAcctSetting.DPAEAS_CreatedOn = DateTime.Now;
                    deptProgramAdminEntryAcctSetting.DPAEAS_CreatedBy = item.DPAEAS_CreatedBy;
                    _ClientDBContext.DeptProgramAdminEntryAcctSettings.AddObject(deptProgramAdminEntryAcctSetting);
                }
            }
            if (_ClientDBContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        #endregion

        #region UAT 4522
        Boolean IComplianceSetupRepository.UserGranularPermissionDigestion(Int32 organizationUserId, String entityCode, Int32 hierarchyNodeId, Int32 currentLoggedInUserId)
        {

            Boolean IsRootNode = _ClientDBContext.DeptProgramMappings.Where(cond => cond.DPM_ID == hierarchyNodeId && !cond.DPM_IsDeleted && cond.DPM_ParentNodeID == null).Any();
            if (IsRootNode)
            {
                return true;
            }

            EntityConnection connection = _ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("dbo.UserGranularPermissionDigestion", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrganizationUserId", organizationUserId);
                command.Parameters.AddWithValue("@EntityCode", entityCode);
                command.Parameters.AddWithValue("@DPM_ID", hierarchyNodeId);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);

                if (con.State == ConnectionState.Closed)
                    con.Open();
                command.ExecuteNonQuery();
                con.Close();
                return true;
            }
        }

        #endregion

        #region UAT-4558

        private static void UpdateComplianceFileAttributeDocs(ComplianceAttribute complianceAttribute, ComplianceAttribute complianceAttributeInDb)
        {
            List<ComplianceAttributeDocMapping> lstDocs = complianceAttribute.ComplianceAttributeDocMappings.ToList();
            List<ComplianceAttributeDocMapping> lstDocInDB = complianceAttributeInDb.ComplianceAttributeDocMappings.Where(cond => !cond.CADM_IsDeleted).ToList();
            if (!lstDocInDB.IsNullOrEmpty() && lstDocInDB.Count > AppConsts.NONE)
            {
                //Delete Removed docs
                lstDocInDB.ForEach(docInDb =>
                {
                    if (!lstDocs.Select(Sel => Sel.CADM_SystemDocumentID).ToList().Contains(docInDb.CADM_SystemDocumentID))
                    {
                        docInDb.CADM_IsDeleted = true;
                        docInDb.CADM_ModifiedBy = complianceAttribute.ModifiedByID;
                        docInDb.CADM_ModifiedOn = complianceAttribute.ModifiedOn;
                    }

                });

                //Add new docs
                if (!lstDocs.IsNullOrEmpty() && lstDocs.Count > AppConsts.NONE)
                {
                    lstDocs.ForEach(newDoc =>
                    {
                        if (!lstDocInDB.Select(Sel => Sel.CADM_SystemDocumentID).ToList().Contains(newDoc.CADM_SystemDocumentID))
                        {
                            complianceAttributeInDb.ComplianceAttributeDocMappings.Add(new ComplianceAttributeDocMapping()
                            {
                                CADM_CreatedBy = newDoc.CADM_CreatedBy > AppConsts.NONE ? newDoc.CADM_CreatedBy : complianceAttribute.ModifiedByID.Value,
                                CADM_CreatedOn = !newDoc.CADM_CreatedOn.IsNullOrEmpty() ? newDoc.CADM_CreatedOn : complianceAttribute.ModifiedOn.Value,
                                CADM_IsDeleted = false,
                                CADM_SystemDocumentID = newDoc.CADM_SystemDocumentID
                            });
                        }
                    });
                }
            }
            else if (!lstDocs.IsNullOrEmpty() && lstDocs.Count > AppConsts.NONE)
            {
                if (complianceAttributeInDb.ComplianceAttributeDocMappings.IsNull())
                {
                    complianceAttributeInDb.ComplianceAttributeDocMappings = new EntityCollection<ComplianceAttributeDocMapping>();
                }
                lstDocs.ForEach(newDoc =>
                {
                    complianceAttributeInDb.ComplianceAttributeDocMappings.Add(new ComplianceAttributeDocMapping()
                    {
                        CADM_CreatedBy = newDoc.CADM_CreatedBy,
                        CADM_CreatedOn = newDoc.CADM_CreatedOn,
                        CADM_IsDeleted = false,
                        CADM_SystemDocumentID = newDoc.CADM_SystemDocumentID
                    });
                });
            }
        }
        #endregion

        #region UAT-5198
        public Dictionary<String, String> IsCategoriesAvailableinSelectedPackages(List<Int32> lstPacakgeIds, List<Int32> lstCategoryIds, List<Tuple<Int32, Int32, Int32>> mappings)
        {
            Dictionary<String, String> dictMissingPkgObjectPairs = new Dictionary<String, String>();
            List<Int32> _lstItemMapped = mappings.Where(co => co.Item2 == 3).Select(sel => sel.Item3).ToList();
            List<Int32> _lstAttributesMapped = mappings.Where(co => co.Item2 == 4).Select(sel => sel.Item3).ToList();

            var cpc = _ClientDBContext.CompliancePackageCategories.Where(con => lstPacakgeIds.Contains(con.CPC_PackageID) && !con.CPC_IsDeleted && con.CPC_IsActive).Distinct().ToList();

            List<Int32> CategoryNotInPackage = new List<Int32>();
            List<ComplianceItem> lstComplianceItems = new List<ComplianceItem>();
            List<ComplianceAttribute> lstComplianceAttributes = new List<ComplianceAttribute>();
            string itemObjectType = ObjectType.Compliance_Item.GetStringValue();
            string attrObjectType = ObjectType.Compliance_ATR.GetStringValue();
            List<lkpObjectType> objectTypeList = GetlkpObjectType();
            lkpObjectType objectTypeItem = objectTypeList.FirstOrDefault(x => x.OT_Code.Equals(itemObjectType));
            lkpObjectType objectTypeAttr = objectTypeList.FirstOrDefault(x => x.OT_Code.Equals(attrObjectType));            

            if (!_lstItemMapped.IsNullOrEmpty())
            {
                lstComplianceItems = _ClientDBContext.ComplianceItems.Where(Con => _lstItemMapped.Contains(Con.ComplianceItemID) && !Con.IsDeleted).ToList();
            }

            if (!_lstAttributesMapped.IsNullOrEmpty())
            {
                lstComplianceAttributes= _ClientDBContext.ComplianceAttributes.Where(Con => _lstAttributesMapped.Contains(Con.ComplianceAttributeID) && !Con.IsDeleted).ToList();
            }            

            foreach (var item in lstPacakgeIds.Distinct())
            {
                var _packageCategoryMapping = cpc.Where(con => con.CPC_PackageID == item).ToList();
                //CategoryNotInPackage = lstCategoryIds;
                CategoryNotInPackage = new List<Int32>(lstCategoryIds);                
                CategoryNotInPackage.RemoveAll(id => !_packageCategoryMapping.Where(sel => sel.CPC_CategoryID == id).IsNullOrEmpty());

                if (!CategoryNotInPackage.IsNullOrEmpty())
                {
                    string pkgName = _packageCategoryMapping.FirstOrDefault().CompliancePackage.PackageName;
                    String objectNotMapped = string.Empty;
                    foreach (var mapping in mappings.Where(sel => CategoryNotInPackage.Contains(sel.Item1)))
                    {
                        if (mapping.Item2 == objectTypeItem.OT_ID)
                        {
                            if (objectNotMapped == String.Empty)
                            {
                                objectNotMapped = lstComplianceItems.Where(cond => cond.ComplianceItemID == mapping.Item3).FirstOrDefault().Name;
                            }
                            else
                            {
                                objectNotMapped = objectNotMapped + "," + lstComplianceItems.Where(cond => cond.ComplianceItemID == mapping.Item3).FirstOrDefault().Name;

                            }
                        }

                        if (mapping.Item2 == objectTypeAttr.OT_ID)
                        {
                            if (objectNotMapped == String.Empty)
                            {
                                objectNotMapped = lstComplianceAttributes.Where(con => con.ComplianceAttributeID == mapping.Item3).FirstOrDefault().Name;
                            }
                            else
                            {
                                objectNotMapped = objectNotMapped + "," + lstComplianceAttributes.Where(con => con.ComplianceAttributeID == mapping.Item3).FirstOrDefault().Name;
                            }
                        }
                    }
                    dictMissingPkgObjectPairs.Add(pkgName, objectNotMapped);
                }
            }

            return dictMissingPkgObjectPairs;
        }

        public AssignmentHierarchy GetAssignmentHierarchyByRuleSetId(int RuleSetId)
        {
            RuleSet objRuleSet = new RuleSet();
            objRuleSet = _ClientDBContext.RuleSets.Where(x => x.RLS_ID == RuleSetId).FirstOrDefault();

            AssignmentHierarchy objAssignmentHierarchy = new AssignmentHierarchy();
            objAssignmentHierarchy = _ClientDBContext.AssignmentHierarchies.Where(x => x.AssignmentHierarchyID == objRuleSet.RLS_AssignmentHierarchyID && x.IsDeleted == false).FirstOrDefault();            

            return objAssignmentHierarchy;
        }

        #endregion
    }
}