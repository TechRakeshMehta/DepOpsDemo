using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IManageRejectionReasonView
    {

        IQueryable<RejectionReason> ListRejectionReason
        {
            get;
            set;
        }
        String ReasonText
        {
            get;
            set;
        }

        String ReasonName
        {
            get;
            set;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }
        Int32 RejectionReasonCategoryId
        {
            get;
            set;
        }
        Int32 RejectionReasonID
        {
            get;
            set;
        }
        Int32 TenantId
        {
            get;
            set;
        }

        Int32 CurrentUserId
        {
            get;
        }
        List<lkpRejectionReasonCategory> ListRejectionCategory
        {
            get;
            set;
        }

    }
}
