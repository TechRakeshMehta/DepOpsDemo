using System;
using System.Collections.Generic;
using Entity;

namespace DAL.Interfaces
{
    public interface IReportRepository
    {
        Report GetReportByCode(string reportCode);

        Boolean SaveReportFavouriteParameter(ReportFavouriteParameter reportFavouriteParameter);

        List<ReportFavouriteParameter> GetReportFavouriteParametersByUserID(Int32 currentLoggedInUserID);

        ReportFavouriteParameter GetReportFavouriteParameterByID(Int32 selectedFavParamID);

        Boolean UpdateFavParamReportParamMapping(Dictionary<Int32, String> dicUpdatedParameters, ReportFavouriteParameter favParam);
        Boolean DeleteFavParamReportParamMapping(string RPF_ids, Int32 CurrentUserId);
    }
}
