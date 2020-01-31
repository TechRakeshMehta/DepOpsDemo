using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.Search.Views
{
    public interface IApplicantPortfolioSearchMasterView
    {
        Int32 TenantId
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        /// <summary>
        /// To set or get Search Instance Id
        /// </summary>
        Int32 SearchInstanceId
        {
            get;
            set;
        }

        /// <summary>
        /// To set or get error message
        /// </summary>
        String ErrorMessage
        {
            get;
            set;
        }
    }
}




