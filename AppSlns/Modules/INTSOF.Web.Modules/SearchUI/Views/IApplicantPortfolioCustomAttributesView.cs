using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
namespace CoreWeb.Search.Views
{
    public interface IApplicantPortfolioCustomAttributesView
    {
        #region Properties

        Int32 CurrentUserId
        {
            get;
        }
        List<Int32> ListDepartmentProgramIds
        {
            get;
            set;
        }
        List<DeptProgramMapping> DepartmentProgramMapping
        {
            get;
            set;
        }
        Int32 TenantId
        {
            get;
        }

        IApplicantPortfolioCustomAttributesView CurrentViewContext
        {
            get;
        } 

        #endregion
    }
}




