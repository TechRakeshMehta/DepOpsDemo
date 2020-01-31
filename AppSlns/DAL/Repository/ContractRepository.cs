using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ContractManagement;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using INTSOF.Utils;
using System.Data.Entity.Core.Objects.DataClasses;

namespace DAL.Repository
{
    public class ContractRepository : ClientBaseRepository, IContractRepository
    {
        #region Private Variables

        private ADB_LibertyUniversity_ReviewEntities _dbContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public ContractRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }

        #endregion

        #region Methods

        #region Manage Contracts

        List<ContractManagementContract> IContractRepository.GetContractSearch(ContractManagementContract objContract, CustomPagingArgsContract customPagingArgsContract, Int32 currentLoggedInUserTenantId)
        {
            List<ContractManagementContract> objContractdata = new List<ContractManagementContract>();

            String orderBy = "ContractId";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        { 
                            new SqlParameter("@AffiliationName", objContract.AffiliationName),  
                            new SqlParameter("@SiteName", objContract.Sites),  
                            new SqlParameter("@StartDate", objContract.StartDate), 
                            new SqlParameter("@EndDate", objContract.EndDate), 
                            new SqlParameter("@ExpirationTypeIds", objContract.ExpTypeIds), 
                            new SqlParameter("@HierarchyNodeIds", objContract.HierarchyNodes), 
                            new SqlParameter("@OrderBy", orderBy),
                            new SqlParameter("@OrderDirection", ordDirection),
                            new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                            new SqlParameter("@PageSize", customPagingArgsContract.PageSize),
                            new SqlParameter("@CurrentLoggedInUserId", objContract.CurrentLoggedInUserId),
                            new SqlParameter("@CurrentLoggedInUserTenantId", currentLoggedInUserTenantId),
                            new SqlParameter("@SearchType", objContract.SearchType),
                            new SqlParameter("@ContractTypeIdList", objContract.ContractTypeIdList),
                            new SqlParameter("@DocumentStatusId", objContract.DocumentStatusId)                          
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetContractSearch", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ContractManagementContract objdata = new ContractManagementContract();
                            objdata.ContractId = dr["ContractId"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(dr["ContractId"]);
                            objdata.AffiliationName = Convert.ToString(dr["AffiliationName"]);
                            objdata.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                            objdata.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                            objdata.ExpCode = Convert.ToString(dr["ExpirationTypeCode"]);
                            objdata.HierarchyNodes = Convert.ToString(dr["HierarchyNodes"]);
                            objdata.Contacts = Convert.ToString(dr["ContactName"]);
                            objdata.Sites = Convert.ToString(dr["SiteName"]);
                            objdata.DaysBeforeFrequency = Convert.ToString(dr["DaysBefore"]);
                            objdata.AfterFrequency = dr["AfterFrequency"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["AfterFrequency"]);
                            objdata.TermMonths = dr["TermMonths"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["TermMonths"]);
                            objdata.ExpirationDate = dr["ExpirationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ExpirationDate"]);
                            objdata.ExpTypeId = dr["ExpirationTypeId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["ExpirationTypeId"]);
                            objdata.Notes = Convert.ToString(dr["Notes"]);
                            objdata.ContractTypeIdList = dr["ContractTypeID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ContractTypeID"]);
                            objdata.ContractTypeNames = dr["ContractTypeNames"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ContractTypeNames"]);
                            objdata.TotalRecordCount = dr["TotalCount"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["TotalCount"]);
                            objContractdata.Add(objdata);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return objContractdata;
        }

        List<Int32> IContractRepository.GetContractIDs(ContractManagementContract objContract, CustomPagingArgsContract customPagingArgsContract, Int32 currentLoggedInUserTenantId, Int32 VirtualRecordCount)
        {
            List<Int32> ContractIDs = new List<Int32>();
            //max sie of int
            Int32 pageSize = VirtualRecordCount > 2147483647 ? 2147483647 : VirtualRecordCount;
            String orderBy = "ContractId";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        { 
                            new SqlParameter("@AffiliationName", objContract.AffiliationName),  
                            new SqlParameter("@SiteName", objContract.Sites),  
                            new SqlParameter("@StartDate", objContract.StartDate), 
                            new SqlParameter("@EndDate", objContract.EndDate), 
                            new SqlParameter("@ExpirationTypeIds", objContract.ExpTypeIds), 
                            new SqlParameter("@HierarchyNodeIds", objContract.HierarchyNodes), 
                            new SqlParameter("@OrderBy", orderBy),
                            new SqlParameter("@OrderDirection", ordDirection),
                            new SqlParameter("@PageIndex",1),
                            new SqlParameter("@PageSize", pageSize),
                            new SqlParameter("@CurrentLoggedInUserId", objContract.CurrentLoggedInUserId),
                            new SqlParameter("@CurrentLoggedInUserTenantId", currentLoggedInUserTenantId),
                            new SqlParameter("@SearchType", objContract.SearchType),
                            new SqlParameter("@ContractTypeIdList", objContract.ContractTypeIdList),
                            new SqlParameter("@DocumentStatusId", objContract.DocumentStatusId)                          
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetContractSearch", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Int32 id;
                            id = dr["ContractId"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(dr["ContractId"]);
                            ContractIDs.Add(id);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return ContractIDs;
        }
        /// <summary>
        /// Save the new Contract and its related data.
        /// </summary>
        /// <param name="contractData"></param>
        /// <param name="currentUserId"></param>
        void IContractRepository.SaveContract(ContractManagementContract contractData, Int32 currentUserId, List<lkpContractExpirationType> lkpContractExpirationType)
        {
            var _currentDateTime = DateTime.Now;
            Contract _contract = new Contract();
            _contract.CON_AffiliationName = contractData.AffiliationName;
            _contract.CON_StartDate = contractData.StartDate;
            _contract.CON_EndDate = contractData.EndDate;
            _contract.CON_ExpirationTypeID = contractData.ExpTypeId;
            _contract.CON_ExpirationDate = contractData.ExpirationDate;
            _contract.CON_TermMonths = contractData.TermMonths;
            _contract.CON_DaysBeforeFrequency = contractData.DaysBeforeFrequency;
            _contract.CON_AfterFrequency = contractData.AfterFrequency;
            _contract.CON_Notes = contractData.Notes;
            _contract.CON_IsDeleted = false;
            _contract.CON_CreatedByID = currentUserId;
            _contract.CON_CreatedOn = _currentDateTime;


            List<Int32> contractTypeToBeMapped = new List<Int32>();
            if (!contractData.ContractTypeIdList.IsNullOrEmpty())
            {
                contractTypeToBeMapped = contractData.ContractTypeIdList.Split(',').Select(int.Parse).ToList();
                foreach (Int32 contractType in contractTypeToBeMapped)
                {
                    ContractTypesContractSiteMapping contractTypesContractSiteMapping = new ContractTypesContractSiteMapping();
                    //contractTypesContractSiteMapping.CTCSM_ContractID=
                    contractTypesContractSiteMapping.CTCSM_ContractTypeID = contractType;
                    contractTypesContractSiteMapping.CTCSM_IsDeleted = false;
                    contractTypesContractSiteMapping.CTCSM_CreatedByID = currentUserId;
                    contractTypesContractSiteMapping.CTCSM_CreatedOn = _currentDateTime;
                    _contract.ContractTypesContractSiteMappings.Add(contractTypesContractSiteMapping);

                }
            }


            if (!contractData.lstNodeIds.IsNullOrEmpty())
            {
                foreach (var nodeId in contractData.lstNodeIds)
                {
                    if (_contract.ContractNodeMappings.IsNullOrEmpty())
                    {
                        _contract.ContractNodeMappings = new EntityCollection<ContractNodeMapping>();
                    }

                    _contract.ContractNodeMappings.Add(new ContractNodeMapping
                    {
                        CNM_DeptProgramMappingID = nodeId,
                        CNM_CreatedByID = currentUserId,
                        CNM_CreatedOn = _currentDateTime,
                        CNM_IsDeleted = false,
                        Contract = _contract
                    });
                }
            }

            List<lkpDocumentType> lstDocType = _dbContext.lkpDocumentTypes.ToList();

            if (!contractData.lstSites.IsNullOrEmpty())
            {
                if (_contract.ContractNodeMappings.IsNullOrEmpty())
                {
                    _contract.ContractSitesContractMappings = new EntityCollection<ContractSitesContractMapping>();
                }

                foreach (var site in contractData.lstSites)
                {
                    var _selectedExpType = lkpContractExpirationType.Where(cet => cet.CET_Code == site.ExpTypeCode).FirstOrDefault();
                    if (_selectedExpType.IsNotNull())
                    {
                        site.ExpTypeId = _selectedExpType.CET_ID;
                    }
                    GenerateContractSiteInstance(currentUserId, _currentDateTime, _contract, site, lstDocType);
                }
            }

            if (!contractData.lstContacts.IsNullOrEmpty())
            {
                if (_contract.ContractContactMappings.IsNullOrEmpty())
                {
                    _contract.ContractContactMappings = new EntityCollection<ContractContactMapping>();
                }

                foreach (var contact in contractData.lstContacts)
                {
                    ContractContactMapping _contractContactMapping = GenerateContractContactInstance(currentUserId, _currentDateTime, _contract, contact);
                    _contract.ContractContactMappings.Add(_contractContactMapping);
                }
            }

            if (!contractData.ContractDocumentContractList.IsNullOrEmpty())
            {
                _contract.ContractDocumentMappings = new EntityCollection<ContractDocumentMapping>();

                foreach (ContractDocumentContract docContract in contractData.ContractDocumentContractList)
                {
                    ContractDocumentMapping contractDocumentMapping = GetContractDocumentInstance(currentUserId, lstDocType, docContract);
                    _contract.ContractDocumentMappings.Add(contractDocumentMapping);
                }
            }
            _dbContext.Contracts.AddObject(_contract);
            _dbContext.SaveChanges();
        }

        private static ContractDocumentMapping GetContractDocumentInstance(Int32 currentUserId, List<lkpDocumentType> lstDocType, ContractDocumentContract docContract)
        {
            ContractDocumentMapping contractDocumentMapping = new ContractDocumentMapping();
            contractDocumentMapping.CDM_IsDeleted = false;
            contractDocumentMapping.CDM_ParentID = docContract.ParentDocID;
            contractDocumentMapping.CDM_IsActive = true;
            contractDocumentMapping.CDM_CreatedByID = currentUserId;
            contractDocumentMapping.CDM_CreatedOn = DateTime.Now;
            contractDocumentMapping.CDM_DocumentName = docContract.DocumentName;
            contractDocumentMapping.CDM_StartDate = docContract.DocStartDate;
            contractDocumentMapping.CDM_EndDate = docContract.DocEndDate;
            contractDocumentMapping.CDM_ContractDocumentStatusID = docContract.DocStatusID;

            contractDocumentMapping.ClientSystemDocument = new ClientSystemDocument()
            {
                CSD_FileName = docContract.DocFileName,
                CSD_DocumentPath = docContract.DocPath,
                CSD_Description = String.Empty,
                CSD_DocumentTypeID = lstDocType.Where(cond => cond.DMT_Code == docContract.DocTypeCode).FirstOrDefault().DMT_ID,
                CSD_IsDeleted = false,
                CSD_CreatedByID = currentUserId,
                CSD_CreatedOn = DateTime.Now,
                CSD_Size = docContract.DocSize,
            };
            return contractDocumentMapping;
        }

        private static void GenerateContractSiteInstance(Int32 currentUserId, DateTime _currentDateTime, Contract _contract, SiteContract site, List<lkpDocumentType> lstDocType)
        {
            ContractSite _contractSite = new ContractSite();
            _contractSite.CS_Name = site.SiteName;
            _contractSite.CS_Address = site.SiteAddress;
            _contractSite.CS_IsDeleted = false;
            _contractSite.CS_CreatedByID = currentUserId;
            _contractSite.CS_CreatedOn = _currentDateTime;

            _contractSite.CS_StartDate = site.StartDate;
            _contractSite.CS_EndDate = site.EndDate;
            _contractSite.CS_ExpirationTypeID = site.ExpTypeId;
            _contractSite.CS_ExpirationDate = site.ExpirationDate;
            _contractSite.CS_TermMonths = site.TermMonths;
            _contractSite.CS_DaysBeforeFrequency = site.DaysBeforeFrequency;
            _contractSite.CS_AfterFrequency = site.AfterFrequency;
            _contractSite.CS_Notes = site.Notes;
            if (site.lstSiteDocumentContract.IsNotNull())
            {
                foreach (var siteDoc in site.lstSiteDocumentContract)
                {
                    SiteDocumentMapping siteDocumentMapping = GenerateSiteDocumentInstance(currentUserId, lstDocType, siteDoc);
                    _contractSite.SiteDocumentMappings.Add(siteDocumentMapping);
                }
            }

            ContractSitesContractMapping _contractSiteMapping = new ContractSitesContractMapping();
            _contractSiteMapping.ContractSite = _contractSite;
            _contractSiteMapping.Contract = _contract;
            _contractSiteMapping.CSCM_CreatedByID = currentUserId;
            _contractSiteMapping.CSCM_CreatedOn = _currentDateTime;
            _contractSiteMapping.CSCM_IsDeleted = false;

            if (site.lstSiteContacts.IsNotNull())
            {
                foreach (var siteContact in site.lstSiteContacts)
                {
                    ContractContactMapping siteContactMapping = GenerateContractContactInstance(currentUserId, _currentDateTime, _contract, siteContact);
                    _contractSiteMapping.ContractContactMappings.Add(siteContactMapping);
                }
            }

            List<Int32> contractTypeToBeMapped = new List<Int32>();
            if (!site.ContractTypeIdList.IsNullOrEmpty())
            {
                contractTypeToBeMapped = site.ContractTypeIdList.Split(',').Select(int.Parse).ToList();
                foreach (Int32 contractType in contractTypeToBeMapped)
                {
                    ContractTypesContractSiteMapping contractTypesContractSiteMapping = new ContractTypesContractSiteMapping();
                    contractTypesContractSiteMapping.Contract = _contract;
                    contractTypesContractSiteMapping.CTCSM_ContractTypeID = contractType;
                    contractTypesContractSiteMapping.CTCSM_IsDeleted = false;
                    contractTypesContractSiteMapping.CTCSM_CreatedByID = currentUserId;
                    contractTypesContractSiteMapping.CTCSM_CreatedOn = _currentDateTime;
                    _contractSiteMapping.ContractTypesContractSiteMappings.Add(contractTypesContractSiteMapping);
                }
            }
            _contract.ContractSitesContractMappings.Add(_contractSiteMapping);

        }

        private static SiteDocumentMapping GenerateSiteDocumentInstance(Int32 currentUserId, List<lkpDocumentType> lstDocType, SiteDocumentContract siteDoc)
        {
            SiteDocumentMapping siteDocumentMapping = new SiteDocumentMapping();
            siteDocumentMapping.SDM_IsDeleted = false;
            siteDocumentMapping.SDM_ParentID = siteDoc.ParentDocID;
            siteDocumentMapping.SDM_IsActive = true;
            siteDocumentMapping.SDM_CreatedByID = currentUserId;
            siteDocumentMapping.SDM_CreatedOn = DateTime.Now;
            siteDocumentMapping.SDM_DocumentName = siteDoc.DocumentName;
            siteDocumentMapping.SDM_StartDate = siteDoc.DocStartDate;
            siteDocumentMapping.SDM_EndDate = siteDoc.DocEndDate;
            siteDocumentMapping.SDM_ContractDocumentStatusID = siteDoc.DocStatusID;

            siteDocumentMapping.ClientSystemDocument = new ClientSystemDocument()
            {
                CSD_FileName = siteDoc.DocFileName,
                CSD_DocumentPath = siteDoc.DocPath,
                CSD_Description = String.Empty,
                CSD_DocumentTypeID = lstDocType.Where(cond => cond.DMT_Code == siteDoc.DocTypeCode).FirstOrDefault().DMT_ID,
                CSD_IsDeleted = false,
                CSD_CreatedByID = currentUserId,
                CSD_CreatedOn = DateTime.Now,
                CSD_Size = siteDoc.DocSize,
            };
            return siteDocumentMapping;
        }

        private static ContractContactMapping GenerateContractContactInstance(Int32 currentUserId, DateTime _currentDateTime, Contract _contract, ContactContract contact)
        {
            ContractContact _contractContact = new ContractContact();
            _contractContact.CC_FirstName = contact.FirstName;
            _contractContact.CC_LastName = contact.LastName;
            _contractContact.CC_Title = contact.Title;
            _contractContact.CC_Phone = contact.Phone;
            _contractContact.CC_Email = contact.Email;
            _contractContact.CC_IsDeleted = false;
            _contractContact.CC_CreatedByID = currentUserId;
            _contractContact.CC_CreatedOn = _currentDateTime;
            //UAT-2447
            _contractContact.CC_IsInternationalPhone = contact.IsInternationalPhone;

            ContractContactMapping _contractContactMapping = new ContractContactMapping();
            _contractContactMapping.ContractContact = _contractContact;
            _contractContactMapping.Contract = _contract;
            _contractContactMapping.CCM_CreatedByID = currentUserId;
            _contractContactMapping.CCM_CreatedOn = _currentDateTime;
            _contractContactMapping.CCM_IsDeleted = false;
            return _contractContactMapping;
        }

        /// <summary>
        /// Update the Contract
        /// </summary>
        /// <param name="contractData"></param>
        /// <param name="currentUserId"></param>
        void IContractRepository.UpdateContract(ContractManagementContract contractData, Int32 currentUserId, List<lkpContractExpirationType> lstLkpContractExpirationType)
        {
            var _currentDateTime = DateTime.Now;
            var _contractToUpdate = _dbContext.Contracts.Where(con => con.CON_ID == contractData.ContractId).First();

            _contractToUpdate.CON_AffiliationName = contractData.AffiliationName;
            _contractToUpdate.CON_StartDate = contractData.StartDate;
            _contractToUpdate.CON_EndDate = contractData.EndDate;
            _contractToUpdate.CON_ExpirationTypeID = contractData.ExpTypeId;
            _contractToUpdate.CON_ExpirationDate = contractData.ExpirationDate;
            _contractToUpdate.CON_TermMonths = contractData.TermMonths;
            _contractToUpdate.CON_DaysBeforeFrequency = contractData.DaysBeforeFrequency;
            _contractToUpdate.CON_AfterFrequency = contractData.AfterFrequency;
            _contractToUpdate.CON_Notes = contractData.Notes;

            _contractToUpdate.CON_ModifiedByID = currentUserId;
            _contractToUpdate.CON_ModifiedOn = _currentDateTime;


            if (contractData.lstNodeIds.IsNotNull())
            {
                var _lstExistingNodeMappings = _contractToUpdate.ContractNodeMappings.Where(cnm => cnm.CNM_IsDeleted == false).ToList();
                var _lstNodeMappingsToDelete = _lstExistingNodeMappings.Where(cnm => !contractData.lstNodeIds.Contains(cnm.CNM_DeptProgramMappingID)).ToList();
                var _lstNodeMappingsToAdd = new List<Int32>();

                foreach (var nodeId in contractData.lstNodeIds)
                {
                    if (!_lstExistingNodeMappings.Any(cnm => cnm.CNM_DeptProgramMappingID == nodeId))
                    {
                        _lstNodeMappingsToAdd.Add(nodeId);
                    }
                }

                foreach (var cnm in _lstNodeMappingsToDelete)
                {
                    cnm.CNM_IsDeleted = true;
                    cnm.CNM_ModifiedByID = currentUserId;
                    cnm.CNM_ModifiedOn = _currentDateTime;
                }

                foreach (var cnm in _lstNodeMappingsToAdd)
                {
                    ContractNodeMapping _contractNodeMapping = new ContractNodeMapping();
                    _contractNodeMapping.CNM_IsDeleted = false;
                    _contractNodeMapping.CNM_CreatedByID = currentUserId;
                    _contractNodeMapping.CNM_CreatedOn = DateTime.Now;
                    _contractNodeMapping.CNM_DeptProgramMappingID = cnm;
                    _contractNodeMapping.Contract = _contractToUpdate;

                    _contractToUpdate.ContractNodeMappings.Add(_contractNodeMapping);
                }
            }

            List<Int32> contractTypeToBeMapped = new List<Int32>();
            if (!contractData.ContractTypeIdList.IsNull())
            {
                if (!contractData.ContractTypeIdList.IsNullOrEmpty())
                {
                    contractTypeToBeMapped = contractData.ContractTypeIdList.Split(',').Select(int.Parse).ToList();
                }
                var _lstExistingContractType = _contractToUpdate.ContractTypesContractSiteMappings.Where(cnm => cnm.CTCSM_IsDeleted == false && cnm.CTCSM_ContractSitesContractMappingID.IsNull()).ToList();
                var _lstExistingContractTypeToDelete = _lstExistingContractType.Where(cnm => !contractTypeToBeMapped.Contains(cnm.CTCSM_ContractTypeID)).ToList();

                var _lstContractTypeToAdd = new List<Int32>();

                foreach (var contractType in contractTypeToBeMapped)
                {
                    if (!_lstExistingContractType.Any(cnm => cnm.CTCSM_ContractTypeID == contractType))
                    {
                        _lstContractTypeToAdd.Add(contractType);
                    }
                }

                foreach (var contractType in _lstExistingContractTypeToDelete)
                {
                    contractType.CTCSM_IsDeleted = true;
                    contractType.CTCSM_ModifiedByID = currentUserId;
                    contractType.CTCSM_ModifiedOn = _currentDateTime;
                }

                foreach (Int32 contractType in _lstContractTypeToAdd)
                {
                    ContractTypesContractSiteMapping contractTypesContractSiteMapping = new ContractTypesContractSiteMapping();
                    contractTypesContractSiteMapping.CTCSM_ContractTypeID = contractType;
                    contractTypesContractSiteMapping.CTCSM_IsDeleted = false;
                    contractTypesContractSiteMapping.CTCSM_CreatedByID = currentUserId;
                    contractTypesContractSiteMapping.CTCSM_CreatedOn = _currentDateTime;
                    _contractToUpdate.ContractTypesContractSiteMappings.Add(contractTypesContractSiteMapping);

                }
            }

            List<lkpDocumentType> lstDocType = _dbContext.lkpDocumentTypes.ToList();

            if (!contractData.lstContacts.IsNullOrEmpty())
            {
                foreach (var contact in contractData.lstContacts)
                {
                    var _contractContactMapping = _contractToUpdate.ContractContactMappings.Where(ccm => ccm.CCM_ID == contact.ContractContactMappingId).FirstOrDefault();

                    if (contact.IsTempDeleted)
                    {
                        // Handle the case when a new Contact was added but removed before final save
                        if (_contractContactMapping.IsNotNull())
                        {
                            _contractContactMapping.CCM_IsDeleted = true;
                            _contractContactMapping.CCM_ModifiedByID = currentUserId;
                            _contractContactMapping.CCM_ModifiedOn = _currentDateTime;
                        }
                    }
                    else
                    {
                        if (contact.ContractContactMappingId == AppConsts.NONE)
                        {
                            // Add Contact  
                            ContractContactMapping contractContactMapping = GenerateContractContactInstance(currentUserId, _currentDateTime, _contractToUpdate, contact);
                            _contractToUpdate.ContractContactMappings.Add(contractContactMapping);
                        }
                        else
                        {
                            // Update Existing Contact
                            _contractContactMapping.ContractContact.CC_FirstName = contact.FirstName;
                            _contractContactMapping.ContractContact.CC_LastName = contact.LastName;
                            _contractContactMapping.ContractContact.CC_Phone = contact.Phone;
                            _contractContactMapping.ContractContact.CC_Email = contact.Email;
                            _contractContactMapping.ContractContact.CC_Title = contact.Title;
                            _contractContactMapping.ContractContact.CC_ModifiedByID = currentUserId;
                            _contractContactMapping.ContractContact.CC_ModifiedOn = _currentDateTime;
                            //UAt-2447
                            _contractContactMapping.ContractContact.CC_IsInternationalPhone = contact.IsInternationalPhone;
                        }
                    }
                }
            }

            if (!contractData.lstSites.IsNullOrEmpty())
            {
                foreach (var site in contractData.lstSites)
                {
                    var _contractSitetMapping = _contractToUpdate.ContractSitesContractMappings.Where(cscm => cscm.CSCM_ID == site.ContractSiteMappingId).FirstOrDefault();

                    if (site.IsTempDeleted)
                    {
                        // Handle the case when a new Site was added but removed later on
                        if (_contractSitetMapping.IsNotNull())
                        {
                            _contractSitetMapping.CSCM_IsDeleted = true;
                            _contractSitetMapping.CSCM_ModifiedByID = currentUserId;
                            _contractSitetMapping.CSCM_ModifiedOn = _currentDateTime;
                            List<SiteDocumentMapping> lstSiteDocumentMapping = _contractSitetMapping.ContractSite.SiteDocumentMappings.Where(cond => !cond.SDM_IsDeleted).ToList();
                            foreach (SiteDocumentMapping _siteDocumentMapping in lstSiteDocumentMapping)
                            {
                                _siteDocumentMapping.SDM_IsDeleted = true;
                                _siteDocumentMapping.SDM_ModifiedByID = currentUserId;
                                _siteDocumentMapping.SDM_ModifiedOn = _currentDateTime;
                            }
                        }
                    }
                    else
                    {
                        var _selectedExpType = lstLkpContractExpirationType.Where(cet => cet.CET_Code == site.ExpTypeCode).FirstOrDefault();
                        if (_selectedExpType.IsNotNull())
                        {
                            site.ExpTypeId = _selectedExpType.CET_ID;
                        }
                        if (site.ContractSiteMappingId == AppConsts.NONE)
                        {
                            // Add new Site
                            GenerateContractSiteInstance(currentUserId, _currentDateTime, _contractToUpdate, site, lstDocType);
                        }
                        else
                        {
                            // Update Existing Site
                            _contractSitetMapping.ContractSite.CS_Name = site.SiteName;
                            _contractSitetMapping.ContractSite.CS_Address = site.SiteAddress;
                            _contractSitetMapping.ContractSite.CS_StartDate = site.StartDate;
                            _contractSitetMapping.ContractSite.CS_EndDate = site.EndDate;
                            _contractSitetMapping.ContractSite.CS_ExpirationTypeID = site.ExpTypeId;
                            _contractSitetMapping.ContractSite.CS_ExpirationDate = site.ExpirationDate;
                            _contractSitetMapping.ContractSite.CS_TermMonths = site.TermMonths;
                            _contractSitetMapping.ContractSite.CS_DaysBeforeFrequency = site.DaysBeforeFrequency;
                            _contractSitetMapping.ContractSite.CS_AfterFrequency = site.AfterFrequency;
                            _contractSitetMapping.ContractSite.CS_Notes = site.Notes;
                            _contractSitetMapping.ContractSite.CS_ModifiedByID = currentUserId;
                            _contractSitetMapping.ContractSite.CS_ModifiedOn = _currentDateTime;
                            foreach (SiteDocumentContract document in site.lstSiteDocumentContract)
                            {
                                SiteDocumentMapping contractDocMapping = _contractSitetMapping.ContractSite.SiteDocumentMappings.Where
                                                                                    (sdm => sdm.SDM_ID == document.SiteDocumentMappingID).FirstOrDefault();

                                UpdateSiteDocumentList(currentUserId, _currentDateTime, lstDocType, _contractSitetMapping, document, contractDocMapping);
                            }

                            foreach (var contact in site.lstSiteContacts)
                            {
                                var _siteContactMapping = _contractSitetMapping.ContractContactMappings.Where(ccm => ccm.CCM_ID == contact.ContractContactMappingId).FirstOrDefault();

                                UpdateSiteContactsList(currentUserId, _currentDateTime, _contractToUpdate, _contractSitetMapping, contact, _siteContactMapping);
                            }

                            List<Int32> contractTypeToBeUpdated = new List<Int32>();
                            if (!site.ContractTypeIdList.IsNull())
                            {
                                if (!site.ContractTypeIdList.IsNullOrEmpty())
                                {
                                    contractTypeToBeUpdated = site.ContractTypeIdList.Split(',').Select(int.Parse).ToList();
                                }

                                UpdateSiteContractType(currentUserId, _currentDateTime, contractTypeToBeUpdated, _contractSitetMapping, _contractToUpdate.CON_ID);
                            }
                        }
                    }
                }
            }

            #region Contract Document

            if (!contractData.ContractDocumentContractList.IsNullOrEmpty())
            {

                foreach (ContractDocumentContract document in contractData.ContractDocumentContractList)
                {
                    ContractDocumentMapping contractDocMapping = _contractToUpdate.ContractDocumentMappings.Where(cdm => cdm.CDM_ID == document.ContractDocumentMappingID).FirstOrDefault();

                    if (document.IsDeleted)
                    {
                        // Handle the case when a new Contact was added but removed before final save
                        if (contractDocMapping.IsNotNull())
                        {
                            contractDocMapping.CDM_IsDeleted = true;
                            contractDocMapping.CDM_ModifiedID = currentUserId;
                            contractDocMapping.CDM_ModifiedOn = _currentDateTime;
                        }
                    }
                    else
                    {
                        if (document.ContractDocumentMappingID == AppConsts.NONE)
                        {
                            // Add Contact  
                            ContractDocumentMapping newContractDocMapping = GetContractDocumentInstance(currentUserId, lstDocType, document);
                            _contractToUpdate.ContractDocumentMappings.Add(newContractDocMapping);
                        }
                        else if (!document.IsActive)
                        {
                            contractDocMapping.CDM_IsActive = false;
                            contractDocMapping.CDM_EndDate = document.DocEndDate;
                            contractDocMapping.CDM_ModifiedID = currentUserId;
                            contractDocMapping.CDM_ModifiedOn = _currentDateTime;
                        }
                        else if (document.IsNewVersion)
                        {
                            ContractDocumentMapping newContractDocMapping = new ContractDocumentMapping()
                            {
                                CDM_IsActive = true,
                                CDM_IsDeleted = false,
                                CDM_ParentID = document.ParentDocID,
                                CDM_CSDID = document.ClientSystemDocumentID,
                                CDM_DocumentName = document.DocumentName,
                                CDM_StartDate = _currentDateTime,
                                CDM_CreatedByID = currentUserId,
                                CDM_CreatedOn = _currentDateTime,
                                CDM_ContractDocumentStatusID = document.DocStatusID
                            };
                        }
                        else
                        {
                            // Update Existing Contact
                            contractDocMapping.CDM_DocumentName = document.DocumentName;
                            contractDocMapping.CDM_ModifiedID = currentUserId;
                            contractDocMapping.CDM_ModifiedOn = _currentDateTime;
                            contractDocMapping.CDM_ContractDocumentStatusID = document.DocStatusID;
                            contractDocMapping.ClientSystemDocument.CSD_FileName = document.DocFileName;
                            contractDocMapping.ClientSystemDocument.CSD_DocumentPath = document.DocPath;
                            contractDocMapping.ClientSystemDocument.CSD_Size = document.DocSize;
                            contractDocMapping.ClientSystemDocument.CSD_ModifiedByID = currentUserId;
                            contractDocMapping.ClientSystemDocument.CSD_ModifiedOn = _currentDateTime;
                        }
                    }
                }
            }

            #endregion

            _dbContext.SaveChanges();
        }

        private static void UpdateSiteContractType(Int32 currentUserId, DateTime _currentDateTime, List<Int32> contractTypeToBeMapped, ContractSitesContractMapping _contractSitetMapping, Int32 contractId)
        {
            var _lstExistingContractType = _contractSitetMapping.ContractTypesContractSiteMappings.Where(cnm => cnm.CTCSM_IsDeleted == false).ToList();
            var _lstExistingContractTypeToDelete = _lstExistingContractType.Where(cnm => !contractTypeToBeMapped.Contains(cnm.CTCSM_ContractTypeID)).ToList();

            var _lstContractTypeToAdd = new List<Int32>();

            foreach (var contractType in contractTypeToBeMapped)
            {
                if (!_lstExistingContractType.Any(cnm => cnm.CTCSM_ContractTypeID == contractType))
                {
                    _lstContractTypeToAdd.Add(contractType);
                }
            }

            foreach (var contractType in _lstExistingContractTypeToDelete)
            {
                contractType.CTCSM_IsDeleted = true;
                contractType.CTCSM_ModifiedByID = currentUserId;
                contractType.CTCSM_ModifiedOn = _currentDateTime;
            }

            foreach (Int32 contractType in _lstContractTypeToAdd)
            {
                ContractTypesContractSiteMapping contractTypesContractSiteMapping = new ContractTypesContractSiteMapping();
                contractTypesContractSiteMapping.CTCSM_ContractID = contractId;
                contractTypesContractSiteMapping.CTCSM_ContractTypeID = contractType;
                contractTypesContractSiteMapping.CTCSM_IsDeleted = false;
                contractTypesContractSiteMapping.CTCSM_CreatedByID = currentUserId;
                contractTypesContractSiteMapping.CTCSM_CreatedOn = _currentDateTime;
                _contractSitetMapping.ContractTypesContractSiteMappings.Add(contractTypesContractSiteMapping);
            }
        }

        private static void UpdateSiteContactsList(Int32 currentUserId, DateTime _currentDateTime, Contract _contractToUpdate, ContractSitesContractMapping _contractSitetMapping, ContactContract contact, ContractContactMapping _siteContactMapping)
        {
            if (contact.IsTempDeleted)
            {
                // Handle the case when a new Contact was added but removed before final save
                if (_siteContactMapping.IsNotNull())
                {
                    _siteContactMapping.ContractContact.CC_IsDeleted = true;
                    _siteContactMapping.ContractContact.CC_ModifiedByID = currentUserId;
                    _siteContactMapping.ContractContact.CC_ModifiedOn = _currentDateTime;
                    _siteContactMapping.CCM_IsDeleted = true;
                    _siteContactMapping.CCM_ModifiedByID = currentUserId;
                    _siteContactMapping.CCM_ModifiedOn = _currentDateTime;
                }
            }
            else
            {
                if (contact.ContractContactMappingId == AppConsts.NONE)
                {
                    // Add Contact  
                    ContractContactMapping contractContactMapping = GenerateContractContactInstance(currentUserId, _currentDateTime, _contractToUpdate, contact);
                    _contractSitetMapping.ContractContactMappings.Add(contractContactMapping);
                }
                else
                {
                    // Update Existing Contact
                    _siteContactMapping.ContractContact.CC_FirstName = contact.FirstName;
                    _siteContactMapping.ContractContact.CC_LastName = contact.LastName;
                    _siteContactMapping.ContractContact.CC_Phone = contact.Phone;
                    _siteContactMapping.ContractContact.CC_Email = contact.Email;
                    _siteContactMapping.ContractContact.CC_Title = contact.Title;
                    _siteContactMapping.ContractContact.CC_ModifiedByID = currentUserId;
                    _siteContactMapping.ContractContact.CC_ModifiedOn = _currentDateTime;
                }
            }
        }

        private static void UpdateSiteDocumentList(Int32 currentUserId, DateTime _currentDateTime, List<lkpDocumentType> lstDocType, ContractSitesContractMapping _contractSitetMapping, SiteDocumentContract document, SiteDocumentMapping contractDocMapping)
        {
            if (document.IsDeleted)
            {
                // Handle the case when a new Contact was added but removed before final save
                if (contractDocMapping.IsNotNull())
                {
                    contractDocMapping.SDM_IsDeleted = true;
                    contractDocMapping.SDM_ModifiedByID = currentUserId;
                    contractDocMapping.SDM_ModifiedOn = _currentDateTime;
                }
            }
            else
            {
                if (document.SiteDocumentMappingID == AppConsts.NONE)
                {
                    // Add Contact  
                    SiteDocumentMapping newSiteDocMapping = GenerateSiteDocumentInstance(currentUserId, lstDocType, document);
                    _contractSitetMapping.ContractSite.SiteDocumentMappings.Add(newSiteDocMapping);
                }
                else if (!document.IsActive)
                {
                    contractDocMapping.SDM_IsActive = false;
                    contractDocMapping.SDM_EndDate = document.DocEndDate;
                    contractDocMapping.SDM_ModifiedByID = currentUserId;
                    contractDocMapping.SDM_ModifiedOn = _currentDateTime;
                }
                else if (document.IsNewVersion)
                {
                    SiteDocumentMapping newSiteDocMapping = new SiteDocumentMapping()
                    {
                        SDM_IsActive = true,
                        SDM_IsDeleted = false,
                        SDM_ParentID = document.ParentDocID,
                        SDM_CSDID = document.ClientSystemDocumentID,
                        SDM_DocumentName = document.DocumentName,
                        SDM_StartDate = _currentDateTime,
                        SDM_CreatedByID = currentUserId,
                        SDM_CreatedOn = _currentDateTime,
                        SDM_ContractDocumentStatusID = (document.DocStatusID.IsNull() || document.DocStatusID > AppConsts.NONE) ? document.DocStatusID : null
                    };
                    _contractSitetMapping.ContractSite.SiteDocumentMappings.Add(newSiteDocMapping);
                }
                else
                {
                    // Update Existing Contact
                    contractDocMapping.SDM_DocumentName = document.DocumentName;
                    contractDocMapping.SDM_ModifiedByID = currentUserId;
                    contractDocMapping.SDM_ModifiedOn = _currentDateTime;
                    contractDocMapping.SDM_ContractDocumentStatusID = (document.DocStatusID.IsNull() || document.DocStatusID > AppConsts.NONE) ? document.DocStatusID : null;
                    contractDocMapping.ClientSystemDocument.CSD_FileName = document.DocFileName;
                    contractDocMapping.ClientSystemDocument.CSD_DocumentPath = document.DocPath;
                    contractDocMapping.ClientSystemDocument.CSD_Size = document.DocSize;
                    contractDocMapping.ClientSystemDocument.CSD_ModifiedByID = currentUserId;
                    contractDocMapping.ClientSystemDocument.CSD_ModifiedOn = _currentDateTime;
                }
            }
        }

        Boolean IContractRepository.DeleteContract(Int32 contractID, Int32 currentUserId)
        {
            Contract contractToBeDeleted = _dbContext.Contracts.Where(cond => cond.CON_ID == contractID).FirstOrDefault();
            if (!contractToBeDeleted.IsNullOrEmpty())
            {
                contractToBeDeleted.CON_IsDeleted = true;
                contractToBeDeleted.CON_ModifiedByID = currentUserId;
                contractToBeDeleted.CON_ModifiedOn = DateTime.Now;
                _dbContext.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// Get detils of the selected Contract
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        ContractManagementContract IContractRepository.GetContractDetails(Int32 contractId)
        {
            var _contract = _dbContext.Contracts.Where(con => con.CON_ID == contractId && con.CON_IsDeleted == false).First();

            return new ContractManagementContract
            {
                ExpTypeId = _contract.CON_ExpirationTypeID,
                StartDate = _contract.CON_StartDate,
                EndDate = _contract.CON_EndDate,
                ExpirationDate = _contract.CON_ExpirationDate,
                DaysBeforeFrequency = _contract.CON_DaysBeforeFrequency,
                AfterFrequency = _contract.CON_AfterFrequency,
                TermMonths = _contract.CON_TermMonths,
                ExpTypeCode = _contract.lkpContractExpirationType.IsNull() ? null : _contract.lkpContractExpirationType.CET_Code,
                AffiliationName = _contract.CON_AffiliationName,
                lstNodeIds = _contract.ContractNodeMappings.Where(cnm => cnm.CNM_IsDeleted == false).Select(cnm => cnm.CNM_DeptProgramMappingID).ToList(),
                Notes = _contract.CON_Notes,
                HierarchyNodes = GetHierarchyNodes(_contract.ContractNodeMappings.ToList())
            };
        }

        /// <summary>
        /// Gets the List of Contract Sites and Contacts
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        Tuple<List<SiteContract>, List<ContactContract>> IContractRepository.GetContractSitesContacts(Int32 contractId)
        {
            var _contract = _dbContext.Contracts.Where(con => con.CON_ID == contractId && con.CON_IsDeleted == false).First();

            List<ContactContract> _lstContacts = new List<ContactContract>();
            foreach (var ccm in _contract.ContractContactMappings.Where(cond => !cond.CCM_IsDeleted && cond.CCM_ContractSitesContractMappingID.IsNull()))
            {
                _lstContacts.Add(new ContactContract
                {
                    FirstName = ccm.ContractContact.CC_FirstName,
                    LastName = ccm.ContractContact.CC_LastName,
                    Email = ccm.ContractContact.CC_Email,
                    Phone = ccm.ContractContact.CC_Phone,
                    Title = ccm.ContractContact.CC_Title,
                    ContactId = ccm.CCM_ContractID,
                    ContractContactMappingId = ccm.CCM_ID,
                    //UAT-2447
                    IsInternationalPhone = ccm.ContractContact.CC_IsInternationalPhone
                });
            }

            List<SiteContract> _lstSites = new List<SiteContract>();
            foreach (var csm in _contract.ContractSitesContractMappings.Where(cond => !cond.CSCM_IsDeleted))
            {
                SiteContract newSiteContract = new SiteContract();

                newSiteContract.SiteName = csm.ContractSite.CS_Name;
                newSiteContract.SiteAddress = csm.ContractSite.CS_Address;
                newSiteContract.SiteId = csm.CSCM_ContractSiteID;
                newSiteContract.ContractSiteMappingId = csm.CSCM_ID;

                newSiteContract.StartDate = csm.ContractSite.CS_StartDate;
                newSiteContract.EndDate = csm.ContractSite.CS_EndDate;
                newSiteContract.ExpTypeId = csm.ContractSite.CS_ExpirationTypeID;
                newSiteContract.ExpTypeCode = csm.ContractSite.CS_ExpirationTypeID.IsNotNull() ? csm.ContractSite.lkpContractExpirationType.CET_Code : String.Empty;
                newSiteContract.ExpirationDate = csm.ContractSite.CS_ExpirationDate;
                newSiteContract.TermMonths = csm.ContractSite.CS_TermMonths;
                newSiteContract.DaysBeforeFrequency = csm.ContractSite.CS_DaysBeforeFrequency;
                newSiteContract.AfterFrequency = csm.ContractSite.CS_AfterFrequency;
                newSiteContract.Notes = csm.ContractSite.CS_Notes;

                newSiteContract.lstSiteDocumentContract = new List<SiteDocumentContract>();
                foreach (var sdm in csm.ContractSite.SiteDocumentMappings.Where(cond => !cond.SDM_IsDeleted).ToList())
                {
                    SiteDocumentContract newSiteDocumentContract = new SiteDocumentContract();

                    newSiteDocumentContract.ClientSystemDocumentID = sdm.ClientSystemDocument.CSD_ID;
                    newSiteDocumentContract.DocSize = sdm.ClientSystemDocument.CSD_Size.HasValue ? sdm.ClientSystemDocument.CSD_Size.Value : 0;
                    newSiteDocumentContract.SiteDocumentMappingID = sdm.SDM_ID;
                    newSiteDocumentContract.DocEndDate = sdm.SDM_EndDate;
                    newSiteDocumentContract.DocStartDate = sdm.SDM_StartDate;
                    newSiteDocumentContract.DocumentName = sdm.SDM_DocumentName;
                    newSiteDocumentContract.ParentDocID = sdm.SDM_ParentID;
                    newSiteDocumentContract.DocTypeCode = sdm.ClientSystemDocument.lkpDocumentType.DMT_Code;
                    newSiteDocumentContract.DocPath = sdm.ClientSystemDocument.CSD_DocumentPath;
                    newSiteDocumentContract.DocTypeID = sdm.ClientSystemDocument.lkpDocumentType.DMT_ID;
                    newSiteDocumentContract.DocFileName = sdm.ClientSystemDocument.CSD_FileName;
                    newSiteDocumentContract.DocumentTypeName = sdm.ClientSystemDocument.lkpDocumentType.DMT_Name;
                    newSiteDocumentContract.IsDeleted = sdm.SDM_IsDeleted;
                    newSiteDocumentContract.IsActive = sdm.SDM_IsActive.HasValue ? sdm.SDM_IsActive.Value : true;
                    newSiteContract.lstSiteDocumentContract.Add(newSiteDocumentContract);
                }
                newSiteContract.lstSiteContacts = new List<ContactContract>();

                List<Int32> contractTypeids = new List<Int32>();
                foreach (var x in csm.ContractTypesContractSiteMappings.Where(cond => !cond.CTCSM_IsDeleted))
                {
                    contractTypeids.Add(x.CTCSM_ContractTypeID);
                }
                newSiteContract.ContractTypeIdList = String.Join(",", contractTypeids);

                _lstSites.Add(newSiteContract);

            }
            return new Tuple<List<SiteContract>, List<ContactContract>>(_lstSites, _lstContacts);
        }

        #endregion

        #region Manage Contract Types

        /// <summary>
        /// Get Contract Types
        /// </summary>
        /// <returns></returns>
        IQueryable<ContractType> IContractRepository.GetContractTypes()
        {
            return _dbContext.ContractTypes.Where(cond => !cond.CT_IsDeleted);
        }

        /// <summary>
        /// Get Contract Type by Id
        /// </summary>
        /// <param name="contractTypeId"></param>
        /// <returns></returns>
        ContractType IContractRepository.GetContractTypeById(Int32 contractTypeId)
        {
            return _dbContext.ContractTypes.FirstOrDefault(con => con.CT_ID == contractTypeId && !con.CT_IsDeleted);
        }

        /// <summary>
        /// Save Contract Types
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns></returns>
        Boolean IContractRepository.SaveContractTypes(ContractType contractType)
        {
            _dbContext.ContractTypes.AddObject(contractType);

            if (_dbContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Update Contract Types
        /// </summary>
        /// <returns></returns>
        Boolean IContractRepository.UpdateContractTypes()
        {
            if (_dbContext.SaveChanges() > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Is Contract Type mapped
        /// </summary>
        /// <param name="contractTypeId"></param>
        /// <returns></returns>
        public Boolean IsContractTypeMapped(Int32 contractTypeId)
        {
            List<ContractTypesContractSiteMapping> contractTypes = _dbContext.ContractTypesContractSiteMappings.Where(cond => cond.CTCSM_ContractTypeID == contractTypeId
                                                                                             && !cond.CTCSM_IsDeleted && !cond.Contract.CON_IsDeleted).ToList();

            if (contractTypes.Any())
                return true;
            return false;
        }

        /// <summary>
        /// Get Last Contract Type Code
        /// </summary>
        /// <returns></returns>
        public String GetLastContractTypeCode()
        {
            String code = String.Empty;

            if (!_dbContext.ContractTypes.IsNullOrEmpty())
            {
                code = _dbContext.ContractTypes.OrderByDescending(cond => cond.CT_ID).FirstOrDefault().CT_Code;
            }
            return code;
        }

        #endregion

        #region ContractNotification

        /// <summary>
        /// Method to get EmailAddress and other data for Notification Email to be send
        /// </summary>
        /// <param name="SubEventCode"></param>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        List<ContractManagementContract> IContractRepository.GetContractNotificationData(String SubEventCode, int TenantID, int Chunksize)
        {
            List<ContractManagementContract> objContractNotificationdata = new List<ContractManagementContract>();

            //  String orderBy = "ContractId";
            // String ordDirection = null;

            // orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            // ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        { 
                            new SqlParameter("@SubEventCode", SubEventCode),  
                            new SqlParameter("@TenantID", TenantID),
                              new SqlParameter("@ChunkSize", Chunksize)
                          
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetContractNotificationDetails", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ContractManagementContract objdata = new ContractManagementContract();
                            objdata.ContractId = dr["ContractId"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(dr["ContractId"]);
                            objdata.AffiliationName = Convert.ToString(dr["AffiliationName"]);
                            objdata.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                            objdata.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                            objdata.ExpirationDate = dr["ExpirationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ExpirationDate"]); ;
                            objdata.OrganisationUserID = Convert.ToInt32(dr["OrgUserId"]);
                            objdata.UserName = Convert.ToString(dr["UserName"]);
                            objdata.UserEmailAddress = Convert.ToString(dr["EmailAddress"]);
                            objContractNotificationdata.Add(objdata);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return objContractNotificationdata;
        }

        #endregion


        List<ContractManagementContract> IContractRepository.GetSiteNotificationDetails(String SubEventCode, int TenantID, int Chunksize)
        {
            List<ContractManagementContract> objNotificationdata = new List<ContractManagementContract>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        { 
                            new SqlParameter("@SubEventCode", SubEventCode),  
                            new SqlParameter("@TenantID", TenantID),
                              new SqlParameter("@ChunkSize", Chunksize)
                          
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetSiteNotificationDetails", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ContractManagementContract objdata = new ContractManagementContract();
                            objdata.SiteID = dr["SiteID"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(dr["SiteID"]);
                            objdata.SiteName = Convert.ToString(dr["SiteName"]);
                            objdata.StartDate = dr["SiteStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["SiteStartDate"]);
                            objdata.EndDate = dr["SiteEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["SiteEndDate"]);
                            objdata.ExpirationDate = dr["SiteExpirationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["SiteExpirationDate"]); ;
                            objdata.OrganisationUserID = Convert.ToInt32(dr["OrgUserId"]);
                            objdata.UserName = Convert.ToString(dr["UserName"]);
                            objdata.UserEmailAddress = Convert.ToString(dr["EmailAddress"]);
                            objNotificationdata.Add(objdata);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return objNotificationdata;
        }

        #region Contract Document

        /// <summary>
        /// Get the list of Documents associated with a Contract
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="tenantId"></param>
        List<ContractDocumentContract> IContractRepository.GetContractDocuments(Int32 contractId)
        {
            var lstContractDocumentMappings = _dbContext.ContractDocumentMappings
                                                                            .Where(cdm => cdm.CDM_ContractID == contractId
                                                                            && !cdm.CDM_IsDeleted
                                                                            && !cdm.ClientSystemDocument.CSD_IsDeleted
                                                                            ).ToList();
            List<ContractDocumentContract> contractDocumentContractList = new List<ContractDocumentContract>();

            foreach (var contractDocumentMapping in lstContractDocumentMappings)
            {
                contractDocumentContractList.Add(new ContractDocumentContract
                {
                    ClientSystemDocumentID = contractDocumentMapping.ClientSystemDocument.CSD_ID,
                    DocSize = contractDocumentMapping.ClientSystemDocument.CSD_Size.HasValue ? contractDocumentMapping.ClientSystemDocument.CSD_Size.Value : 0,
                    ContractDocumentMappingID = contractDocumentMapping.CDM_ID,
                    ContractID = contractDocumentMapping.Contract.CON_ID,
                    DocEndDate = contractDocumentMapping.CDM_EndDate,
                    DocStartDate = contractDocumentMapping.CDM_StartDate,
                    DocumentName = contractDocumentMapping.CDM_DocumentName,
                    ParentDocID = contractDocumentMapping.CDM_ParentID,
                    DocTypeCode = contractDocumentMapping.ClientSystemDocument.lkpDocumentType.DMT_Code,
                    DocPath = contractDocumentMapping.ClientSystemDocument.CSD_DocumentPath,
                    DocTypeID = contractDocumentMapping.ClientSystemDocument.lkpDocumentType.DMT_ID,
                    DocFileName = contractDocumentMapping.ClientSystemDocument.CSD_FileName,
                    DocumentTypeName = contractDocumentMapping.ClientSystemDocument.lkpDocumentType.DMT_Name,
                    IsDeleted = contractDocumentMapping.CDM_IsDeleted,
                    IsActive = contractDocumentMapping.CDM_IsActive.HasValue ? contractDocumentMapping.CDM_IsActive.Value : true,
                    DocStatusID = contractDocumentMapping.CDM_ContractDocumentStatusID.HasValue ? contractDocumentMapping.CDM_ContractDocumentStatusID.Value : AppConsts.NONE,
                    DocStatusName = contractDocumentMapping.CDM_ContractDocumentStatusID.HasValue ? contractDocumentMapping.lkpContractDocumentStatu.CDS_Name : String.Empty,
                });
            }

            return contractDocumentContractList;
        }

        #endregion

        /// <summary>
        /// Get the list of Documents associated with a Site
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="tenantId"></param>
        List<SiteDocumentContract> IContractRepository.GetSiteDocuments(Int32 siteId)
        {
            List<SiteDocumentMapping> lstSiteDocumentMappings = _dbContext.SiteDocumentMappings
                                                                            .Where(sdm => sdm.SDM_SiteID == siteId
                                                                            && !sdm.SDM_IsDeleted
                                                                            && !sdm.ClientSystemDocument.CSD_IsDeleted
                                                                            ).ToList();
            List<SiteDocumentContract> siteDocumentContractList = new List<SiteDocumentContract>();

            foreach (var siteDocumentMapping in lstSiteDocumentMappings)
            {
                siteDocumentContractList.Add(new SiteDocumentContract
                {
                    ClientSystemDocumentID = siteDocumentMapping.ClientSystemDocument.CSD_ID,
                    DocSize = siteDocumentMapping.ClientSystemDocument.CSD_Size.HasValue ? siteDocumentMapping.ClientSystemDocument.CSD_Size.Value : 0,
                    SiteDocumentMappingID = siteDocumentMapping.SDM_ID,
                    ContractID = siteDocumentMapping.ContractSite.CS_ID,
                    DocEndDate = siteDocumentMapping.SDM_EndDate,
                    DocStartDate = siteDocumentMapping.SDM_StartDate,
                    DocumentName = siteDocumentMapping.SDM_DocumentName,
                    ParentDocID = siteDocumentMapping.SDM_ParentID,
                    DocTypeCode = siteDocumentMapping.ClientSystemDocument.lkpDocumentType.DMT_Code,
                    DocPath = siteDocumentMapping.ClientSystemDocument.CSD_DocumentPath,
                    DocTypeID = siteDocumentMapping.ClientSystemDocument.lkpDocumentType.DMT_ID,
                    DocFileName = siteDocumentMapping.ClientSystemDocument.CSD_FileName,
                    DocumentTypeName = siteDocumentMapping.ClientSystemDocument.lkpDocumentType.DMT_Name,
                    IsDeleted = siteDocumentMapping.SDM_IsDeleted,
                    IsActive = siteDocumentMapping.SDM_IsActive.HasValue ? siteDocumentMapping.SDM_IsActive.Value : true,
                    DocStatusID = siteDocumentMapping.SDM_ContractDocumentStatusID.HasValue ? siteDocumentMapping.SDM_ContractDocumentStatusID.Value : AppConsts.NONE,
                    DocStatusName = siteDocumentMapping.SDM_ContractDocumentStatusID.HasValue ? siteDocumentMapping.lkpContractDocumentStatu.CDS_Name : String.Empty,
                });
            }
            return siteDocumentContractList;
        }


        List<ContractSite> IContractRepository.GetContractsites(Int32 contractId)
        {
            return ClientDBContext.ContractSitesContractMappings.Where(cond => cond.CSCM_ContractID == contractId && !cond.CSCM_IsDeleted).Select(sel => sel.ContractSite).ToList();
        }

        List<ContactContract> IContractRepository.GetSiteContacts(Int32 contractId, Int32 siteId)
        {
            var _contractSiteMappings = _dbContext.ContractSitesContractMappings.Where(con => con.CSCM_ContractID == contractId && con.CSCM_IsDeleted == false
                                                                                    && con.CSCM_ContractSiteID == siteId).First();


            List<ContactContract> _lstContacts = new List<ContactContract>();
            foreach (var ccm in _contractSiteMappings.ContractContactMappings.Where(cond => !cond.CCM_IsDeleted))
            {
                _lstContacts.Add(new ContactContract
                {
                    FirstName = ccm.ContractContact.CC_FirstName,
                    LastName = ccm.ContractContact.CC_LastName,
                    Email = ccm.ContractContact.CC_Email,
                    Phone = ccm.ContractContact.CC_Phone,
                    Title = ccm.ContractContact.CC_Title,
                    ContactId = ccm.CCM_ContractID,
                    ContractContactMappingId = ccm.CCM_ID,
                    //UAT-2447
                    IsInternationalPhone = ccm.ContractContact.CC_IsInternationalPhone
                });
            }
            return _lstContacts;
        }

        #endregion

        private String GetHierarchyNodes(List<ContractNodeMapping> lstContractNodeMapping)
        {
            StringBuilder _sb = new StringBuilder();

            foreach (var cnm in lstContractNodeMapping)
            {
                if (cnm.CNM_IsDeleted == false)
                {
                    _sb.Append(cnm.DeptProgramMapping.DPM_Label + ",");
                }
            }
            var _hierarchy = String.Empty;
            if (_sb.Length > 0)
            {
                _hierarchy = Convert.ToString(_sb);
                _hierarchy = _hierarchy.Substring(0, _hierarchy.LastIndexOf(','));
            }

            return _hierarchy;
        }
    }
}
