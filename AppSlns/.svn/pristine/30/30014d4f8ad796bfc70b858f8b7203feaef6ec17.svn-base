using System;
using System.Collections.Generic;
using System.Web.UI;
using INTSOF.Utils;

namespace CoreWeb.BkgOperations.Views
{
    public partial class ClientAdminOrderDetailPageMain : BaseWebPage
    {
        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                //Int32 menuID = Convert.ToInt32(Request.QueryString.Get("menuID"));

                #region UAT-8444
                Int32 menuID = 0;
                Int32 orderPkgSvcGroupID = 0;
                String parentScreenName = "";

                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    menuID = Convert.ToInt32(args.GetValue("menuID"));
                    if (args.ContainsKey(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME) && args.GetValue(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME).ToString().ToLower() == AppConsts.BKG_ORDER_REVIEW_QUEUE.ToLower())
                    {
                        parentScreenName = args.GetValue(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME);
                        orderPkgSvcGroupID = Convert.ToInt32(args.GetValue(AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID));
                    }
                }
                #endregion

                switch (menuID)
                {
                    case AppConsts.ONE:
                        {
                            //Control userControl = Page.LoadControl(@"~\BkgOperations\UserControl\BkgOrderSummary.ascx");
                            //phDynamic.Controls.Add(userControl);
                            //break;
                            BkgOrderSummary bkgOrderSummary = (BkgOrderSummary)LoadControl("~/BkgOperations/UserControl/BkgOrderSummary.ascx");
                            phDynamic.Controls.Add(bkgOrderSummary);
                            bkgOrderSummary.ParentScreenName = parentScreenName;
                            bkgOrderSummary.OrderPackageSvcGrpID = orderPkgSvcGroupID;
                            break;
                        }

                    case AppConsts.TWO:
                        {
                            Control userControl = Page.LoadControl(@"~\BkgOperations\UserControl\BkgOrderServiceGroups.ascx");
                            phDynamic.Controls.Add(userControl);
                            break;
                        }

                    case AppConsts.THREE:
                        {
                            Control userControl = Page.LoadControl(@"~\BkgOperations\UserControl\BkgOrderHistory.ascx");
                            phDynamic.Controls.Add(userControl);
                            break;
                        }
                    //UAT-764
                    case AppConsts.FOUR:
                        {
                            Control userControl = LoadControl(@"~/BkgOperations/UserControl/DisclosureAndReleaseForm.ascx");
                            phDynamic.Controls.Add(userControl);
                            break;
                        }

                    //UAT - 1111 - As a client admin, I should be able to see Residential History on the background order details screen
                    case AppConsts.FIVE:
                        {
                            Control userControl = LoadControl(@"~/BkgOperations/UserControl/NewBkgOrderResidentialHistories.ascx");
                            phDynamic.Controls.Add(userControl);
                            break;
                        }
                }

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #endregion

    }


}