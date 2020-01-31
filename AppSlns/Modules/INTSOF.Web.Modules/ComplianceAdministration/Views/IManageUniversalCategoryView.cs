using Entity.SharedDataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IManageUniversalCategoryView
    {
        IManageUniversalCategoryView CurrentViewContext { get; }

        Int32 UniversalCategoryID { get; set; }

        Boolean IsAddMode { get; set; }

        String CategoryName
        {
            get;
            set;
        }

        //String CategoryLabel
        //{
        //    get;
        //    set;
        //}

        //String Description
        //{
        //    get;
        //    set;
        //}

        List<UniversalCategory> lstUniversalCategory { get; set; }

        Int32 CurrentUserId { get; }

        List<UniversalItem> lstUniversalItems { get; set; }

        String Messgae { get; set; }
    }
}
