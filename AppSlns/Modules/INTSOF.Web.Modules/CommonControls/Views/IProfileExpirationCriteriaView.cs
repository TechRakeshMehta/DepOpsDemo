using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.SharedDataEntity;

namespace CoreWeb.CommonControls.Views
{
    public interface IProfileExpirationCriteriaView
    {
        List<lkpInvitationExpirationType> lstExpirationTypes { set; }

        DateTime? ExpirationDate { get; set; }

        String ExpirationTypeCode { get; set; }

        Int32? MaxViews { get; set; }

        String ExpireOption { get; set; }

        Boolean ResetExpirationCriteria { set; }
    }
}
