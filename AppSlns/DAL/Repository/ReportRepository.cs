using System;
using System.Linq;
using System.Data.Entity.Core.Objects;

#region Application Specific
using DAL.Interfaces;
using Entity;
using System.Collections.Generic;
#endregion

namespace DAL.Repository
{
    public class ReportRepository : BaseRepository, IReportRepository
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables
        private SysXAppDBEntities _dbNavigation;
        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public ReportRepository()
        {
            _dbNavigation = base.Context;
        }

        #endregion

        #region interface members
        Report IReportRepository.GetReportByCode(string reportCode)
        {
            return CompiledGetReportByCode(_dbNavigation, reportCode);
        }

        Boolean IReportRepository.SaveReportFavouriteParameter(ReportFavouriteParameter reportFavouriteParameter)
        {
            _dbNavigation.ReportFavouriteParameters.AddObject(reportFavouriteParameter);
            if (_dbNavigation.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }


        List<ReportFavouriteParameter> IReportRepository.GetReportFavouriteParametersByUserID(Int32 currentLoggedInUserID)
        {
            return _dbNavigation.ReportFavouriteParameters.Where(cond => cond.RFP_UserID == currentLoggedInUserID && !cond.RFP_IsDeleted).ToList(); //UAT-3052
        }

        ReportFavouriteParameter IReportRepository.GetReportFavouriteParameterByID(Int32 selectedFavParamID)
        {
            return _dbNavigation.ReportFavouriteParameters.Where(cond => cond.RFP_ID == selectedFavParamID && !cond.RFP_IsDeleted).FirstOrDefault();
        }

        Boolean IReportRepository.UpdateFavParamReportParamMapping(Dictionary<Int32, String> dicUpdatedParameters, ReportFavouriteParameter favParam)
        {

            ReportFavouriteParameter reportFavouriteParameterToBeUpdated = _dbNavigation.ReportFavouriteParameters
                                            .Where(cond => cond.RFP_ID == favParam.RFP_ID && !cond.RFP_IsDeleted).FirstOrDefault();
            if (!reportFavouriteParameterToBeUpdated.IsNull())
            {
                reportFavouriteParameterToBeUpdated.RFP_Name = favParam.RFP_Name;
                reportFavouriteParameterToBeUpdated.RFP_Description = favParam.RFP_Description;
                reportFavouriteParameterToBeUpdated.RFP_ModifiedByID = favParam.RFP_ModifiedByID;
                reportFavouriteParameterToBeUpdated.RFP_ModifiedOn = favParam.RFP_ModifiedOn;
                foreach (Int32 key in dicUpdatedParameters.Keys)
                {
                    FavParamReportParamMapping mappingToBeUpdated = reportFavouriteParameterToBeUpdated.FavParamReportParamMappings
                                                                    .Where(cond => cond.FPRPM_ID == key && !cond.FPRPM_IsDeleted).FirstOrDefault();
                    if (!mappingToBeUpdated.IsNull())
                    {
                        mappingToBeUpdated.FPRPM_Value = dicUpdatedParameters[key];
                        mappingToBeUpdated.FPRPM_ModifiedByID = favParam.RFP_ModifiedByID;
                        mappingToBeUpdated.FPRPM_ModifiedOn = favParam.RFP_ModifiedOn;
                    }
                }
                _dbNavigation.SaveChanges();
                return true;
            }
            return false;
        }
        Boolean IReportRepository.DeleteFavParamReportParamMapping(string RPF_ids, Int32 CurrentUserId)
        {
            List<Int32> rpf_Ids = RPF_ids.Split(',').Select(Int32.Parse).ToList();
            List<ReportFavouriteParameter> lstreportFavouriteParameterToBeUpdated = _dbNavigation.ReportFavouriteParameters
                                      .Where(cond => rpf_Ids.Contains(cond.RFP_ID) && !cond.RFP_IsDeleted).ToList();
            if (lstreportFavouriteParameterToBeUpdated.Any())
            {
                foreach (ReportFavouriteParameter reportFavouriteParameterToBeUpdated in lstreportFavouriteParameterToBeUpdated)
                {
                    if (!reportFavouriteParameterToBeUpdated.IsNull())
                    {
                        reportFavouriteParameterToBeUpdated.RFP_IsDeleted = true;
                        reportFavouriteParameterToBeUpdated.RFP_ModifiedByID = CurrentUserId;
                        reportFavouriteParameterToBeUpdated.RFP_ModifiedOn = DateTime.Now;
                        foreach(FavParamReportParamMapping mappingToBeUpdated in reportFavouriteParameterToBeUpdated.FavParamReportParamMappings.Where(s=>!s.FPRPM_IsDeleted).ToList())
                        {
                            mappingToBeUpdated.FPRPM_IsDeleted = true;
                        }
                    }
                }

                _dbNavigation.SaveChanges();
                return true;
            }
            return true;
        }

        #endregion

        #region Compiled Queries
        public static readonly Func<SysXAppDBEntities, string, Report> CompiledGetReportByCode = CompiledQuery.Compile<SysXAppDBEntities, string, Report>((dbNavigation, reportCode) => dbNavigation.Reports.FirstOrDefault(r => r.RP_Code.Equals(reportCode, StringComparison.OrdinalIgnoreCase)));
        #endregion
    }
}
