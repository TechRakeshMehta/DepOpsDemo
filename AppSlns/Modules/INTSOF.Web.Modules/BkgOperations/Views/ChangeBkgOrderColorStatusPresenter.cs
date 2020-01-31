using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public class ChangeBkgOrderColorStatusPresenter : Presenter<IChangeBkgOrderColorStatusView>
    {

        public List<OrderFlags> GetInstitutionStatusColor()
        {
            List<OrderFlags> lstInstitutionFlag = new List<OrderFlags>();
            if (View.TenantID > AppConsts.NONE)
            {
                var tempInstColorFlag = BackgroundProcessOrderManager.GetInstitutionStatusColor(View.TenantID);
                if (!tempInstColorFlag.IsNullOrEmpty())
                {
                    lstInstitutionFlag = tempInstColorFlag.Select(col =>
                            new OrderFlags
                            {
                                IOF_ID = col.IOF_ID,
                                OFL_FileName = col.lkpOrderFlag.OFL_FileName,
                                OFL_Tooltip = col.lkpOrderFlag.OFL_Tooltip,
                                OFL_FilePath = col.lkpOrderFlag.OFL_FilePath
                            }).ToList();
                }
                else
                {
                    lstInstitutionFlag.Insert(0, new OrderFlags { IOF_ID = 0, OFL_FileName = "--Select--", OFL_FilePath = String.Empty, OFL_Tooltip = String.Empty });
                }
                return lstInstitutionFlag;
            }
            lstInstitutionFlag.Insert(0, new OrderFlags { IOF_ID = 0, OFL_FileName = "--Select--", OFL_FilePath = String.Empty, OFL_Tooltip = String.Empty });
            return lstInstitutionFlag;
        }

        public Boolean UpdateBkgOrderColorFlag()
        {
            if (View.SelectedColorFlag > AppConsts.NONE)
            {
                return BackgroundProcessOrderManager.UpdateOrderColorFlag(View.TenantID, View.SelectedColorFlag, View.OrderID, View.CurrentUserId);
            }
            return false;
        }

    }
}
