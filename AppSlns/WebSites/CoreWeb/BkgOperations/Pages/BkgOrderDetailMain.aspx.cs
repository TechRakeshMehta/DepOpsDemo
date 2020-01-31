using System;
using System.Web.UI;
using INTSOF.Utils;
using System.Collections.Generic;

namespace CoreWeb.BkgOperations.Views
{
    public partial class BkgOrderDetailMain : BaseWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Int32 menuID = 0;
                Int32 orderPkgSvcGroupID = 0;
                String parentScreenName = "";
                Int32 supplementAutomationStatusID = 0;

                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    menuID = Convert.ToInt32(args.GetValue("menuID"));
                    if (args.ContainsKey(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME) && args.GetValue(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME).ToString().ToLower() == AppConsts.BKG_ORDER_REVIEW_QUEUE.ToLower())
                    {
                        parentScreenName = args.GetValue(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME);
                        orderPkgSvcGroupID = Convert.ToInt32(args.GetValue(AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID));
                        supplementAutomationStatusID = Convert.ToInt32(args.GetValue(AppConsts.QUERYSTRING_SUPPLEMENT_AUTOMATION_STATUS_ID));
                    }
                }
                Control userControl = null;
                switch (menuID)
                {
                    case AppConsts.MINUS_ONE:
                        {
                            //userControl = LoadControl("~/BkgOperations/UserControl/OrderDetailPage.ascx");
                            //phDynamic.Controls.Add(userControl);
                            //UAT-844 chnages
                            OrderDetailPage orderDetailPage = (OrderDetailPage)LoadControl("~/BkgOperations/UserControl/OrderDetailPage.ascx");
                            phDynamic.Controls.Add(orderDetailPage);
                            orderDetailPage.ParentScreenName = parentScreenName;
                            orderDetailPage.OrderPackageSvcGrpID = orderPkgSvcGroupID;
                            orderDetailPage.SupplementAutomationStatusID = supplementAutomationStatusID;
                            break;
                        }
                    case AppConsts.MINUS_TWO:
                        {
                            userControl = LoadControl("~/BkgOperations/UserControl/BkgPackagesOrdered.ascx");
                            phDynamic.Controls.Add(userControl);
                            break;
                        }
                    case AppConsts.MINUS_THREE:
                        {
                            userControl = LoadControl("~/BkgOperations/UserControl/BkgOrderServiceLinePriceInfo.ascx");
                            phDynamic.Controls.Add(userControl);
                            break;
                        }

                    case AppConsts.MINUS_FOUR:
                        {
                            userControl = LoadControl("~/BkgOperations/UserControl/BkgNotes.ascx");
                            phDynamic.Controls.Add(userControl);
                            break;
                        }
                    case AppConsts.MINUS_FIVE: 
                        {
                            userControl = LoadControl("~/BkgOperations/UserControl/NewBkgOrderResidentialHistories.ascx");
                            phDynamic.Controls.Add(userControl);
                            break;
                        }

                    case AppConsts.MINUS_SIX:
                        {
                            userControl = LoadControl("~/BkgOperations/UserControl/BkgOrderHistory.ascx");
                            phDynamic.Controls.Add(userControl);
                            break;
                        }
                    case AppConsts.MINUS_SEVEN:
                        {
                            userControl = LoadControl("~/BkgOperations/UserControl/BkgOrderServiceGroups.ascx");
                            phDynamic.Controls.Add(userControl);
                            break;
                        }

                    case AppConsts.MINUS_EIGHT:
                        {
                            userControl = LoadControl("~/BkgOperations/UserControl/OrderNotificationHistory.ascx");
                            phDynamic.Controls.Add(userControl);
                            break;
                        }
                    case AppConsts.MINUS_NINE:
                        {
                            userControl = LoadControl("~/BkgOperations/UserControl/DisclosureAndReleaseForm.ascx");
                            phDynamic.Controls.Add(userControl);
                            break;
                        }
                    default:
                        if (menuID > 0)
                        {
                            userControl = LoadControl("~/BkgOperations/UserControl/BkgOrderCustomForm.ascx");
                            phDynamic.Controls.Add(userControl);
                        }
                        break;
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
    }


}