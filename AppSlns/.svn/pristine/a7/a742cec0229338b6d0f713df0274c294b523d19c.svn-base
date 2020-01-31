using Business.RepoManagers;
using CoreWeb.ComplianceOperations.Views;
using CoreWeb.FingerPrintSetUp.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace CoreWeb.ComplianceOperations.Pages
{
    public partial class OrderPaymentDetails : BaseWebPage, IRescheduleAppointmentView
    {

        private RescheduleAppointmentPresenter _presenter = new RescheduleAppointmentPresenter();
        private Int32 _tenantId;
        public RescheduleAppointmentPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public String ParentControl
        {
            get
            {
                if (ViewState["ParentControl"].IsNotNull())
                {
                    return Convert.ToString(ViewState["ParentControl"]);
                }
                return null;
            }
            set
            {
                ViewState["ParentControl"] = value;
            }
        }

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        public IRescheduleAppointmentView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public int SelectedSlotID
        {
            get
            {
                if (!ViewState["SelectedSlotID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["SelectedSlotID"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedSlotID"] = value;
            }
        }
        public List<Int32> BopIds { get; set; }
        public Int32 SubscriptionOptionID { get; set; }
        public int CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public int SelectedTenantId
        {
            get
            {
                if (!ViewState["SelectedTenantId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["SelectedTenantId"]);
                }
                return 0;
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }
        public AppointmentSlotContract AppointSlotContract
        {
            get
            {
                if (!ViewState["AppointSlotContract"].IsNullOrEmpty())
                    return (AppointmentSlotContract)(ViewState["AppointSlotContract"]);
                return new AppointmentSlotContract();
            }
            set
            {
                ViewState["AppointSlotContract"] = value;
            }
        }
        public int TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    //_tenantId = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }
        public Boolean IsFingerPrintSvcAvailable
        {
            get;
            set;
        }
        public Boolean IsPassportPhotoSvcAvailable
        {
            get;
            set;
        }
        public int OrderId
        {
            get
            {
                if (!ViewState["OrderId"].IsNull())
                {
                    return (Int32)ViewState["OrderId"];
                }
                return 0;
            }
            set
            {
                ViewState["OrderId"] = value;
            }
        }

        public int RescheduleType
        {
            get
            {
                if (!ViewState["RescheduleType"].IsNull())
                {
                    return (Int32)ViewState["RescheduleType"];
                }
                return 0;
            }
            set
            {
                ViewState["RescheduleType"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            CaptureQueryString();
            ScheduleLocationUpdateControl.enableDiableSaveButton += new AppointmentScheduleLocationUpdate.EnableDiableSaveButton(EnableNextButton);
            ucAppointmentRescheduler.eventEnableSaveButton += new FingerPrintSetUp.Views.AppointmentRescheduler.EnableSaveButton(EnableSaveButton);
            ScheduleLocationUpdateControl.OrderID = CurrentViewContext.OrderId;
            ScheduleLocationUpdateControl.IsFingerPrintSvcSelected = CurrentViewContext.IsFingerPrintSvcAvailable;
            ScheduleLocationUpdateControl.IsPassportPhotoSvcSelected = CurrentViewContext.IsPassportPhotoSvcAvailable;
            OrderId = CurrentViewContext.OrderId;
            if (!Page.IsPostBack)
            {
                ScheduleLocationUpdateControl.TenantId = CurrentViewContext.TenantId;
                ScheduleLocationUpdateControl.ResetPreviousRescheduleData();
                var changeLocation = LocationUpdateAllowed();
                var isFingerPrintRejected = IsFingerPrintRejected();
                if (changeLocation)
                {
                    divUCScheduleLocationUpdateControl.Style["display"] = "";
                    ScheduleLocationUpdateControl.ResetGrid();
                    ScheduleLocationUpdateControl.ShowHideFingerPrintOrderType(!isFingerPrintRejected);
                }

                //divUCScheduleLocationUpdateControl.Visible = true;
                btnSelectAppointment.Visible = changeLocation;
                btnSelectAppointment.Enabled = false;
                dvUCAppointmentRescheduler.Visible = !changeLocation;
                btnSaveAppointment.Visible = !changeLocation;
                btnSaveAppointment.Enabled = false;
                BindAppointmentData();
                dvAppointmentButtons.Visible = true;
                //lnkChangeAppointment.Enabled = false;
            }
            //dvAppointmentButtons.Visible = true;
        }



        public void EnableNextButton(bool status)
        {
            btnSelectAppointment.Enabled = status;
        }
        public void EnableSaveButton()
        {
            btnSaveAppointment.Visible = true;
            btnSaveAppointment.Enabled = true;
        }

        protected void btnSelectAppointment_Click(object sender, EventArgs e)
        {
            var selectedLocation = ScheduleLocationUpdateControl.GetSelectedLocation();
            if (selectedLocation.IsNullOrEmpty() || selectedLocation.LocationId <= 0)
            {
                ShowErrorInfoMessage(Resources.Language.PLZSELLOC);
                return;
            }
            if (selectedLocation.IsEventCode)
            {
                SaveAppointmentInfo(selectedLocation);
            }
            //isValidRescheduleLocation(selectedLocation.LocationId, OrderId);
            //if (RescheduleType == (int)ReshduleOrderType.Invalid)
            //{
            //    ShowErrorInfoMessage(Resources.Language.ACCOUNTINFO);
            //}
            //else
            //{
            divUCScheduleLocationUpdateControl.Style["display"] = "none";
            dvUCAppointmentRescheduler.Visible = true;
            ucAppointmentRescheduler.LocationId = selectedLocation.LocationId;
            btnSelectAppointment.Visible = false;
            btnSaveAppointment.Visible = true;
            //ucAppointmentRescheduler.SelectedSlotDate = CurrentViewContext.AppointSlotContract.SlotDate;
            //ucAppointmentRescheduler.SelectedSlotStartTime = Convert.ToString(CurrentViewContext.AppointSlotContract.SlotStartTimeTimeSpanFormat);
            ucAppointmentRescheduler.IsOrderPaymentDetailScreen = true;
            ucAppointmentRescheduler.IsCreateOrderScreen = false;
            ucAppointmentRescheduler.BindAppointmentRescheduler();
            hdnLocId.Value = Convert.ToString(selectedLocation.LocationId);

            // }
        }

        private void SaveAppointmentInfo(FingerPrintAppointmentContract selectedLocation)
        {

            Order currentOrder = Presenter.GetOrderByOrderId(OrderId);
            bool IsFingerPrintSvc, IsPassportPhotoSvc;
            //CABS - to check if order has any additional services(Fingerprint card or passport photo)
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Presenter.GetBkgOrderServiceDetails(currentOrder.OrderID));
            XmlNodeList elemlist = xmlDoc.GetElementsByTagName("ServiceType");
            IsFingerPrintSvc = new List<XmlNode>(elemlist.Cast<XmlNode>()).Any(x => x.InnerXml == BkgServiceType.FingerPrint_Card.GetStringValue());
            IsPassportPhotoSvc = new List<XmlNode>(elemlist.Cast<XmlNode>()).Any(x => x.InnerXml == BkgServiceType.Passport_Photo.GetStringValue());



            if (CurrentViewContext.AppointSlotContract.ApplicantAppointmentId > AppConsts.NONE)
            {
                ReserveSlotContract reserveSlotContractRes = null;
                var IsOnsiteAppointment = false;
                if (selectedLocation != null && selectedLocation.IsEventCode)
                {
                    CurrentViewContext.SelectedSlotID = Convert.ToInt32(selectedLocation.SlotID);
                    reserveSlotContractRes = Presenter.ReserveSlot();
                    CurrentViewContext.AppointSlotContract.ReservedSlotID = reserveSlotContractRes.ReservedSlotID;
                    CurrentViewContext.AppointSlotContract.SlotID = Convert.ToInt32(selectedLocation.SlotID);
                    CurrentViewContext.AppointSlotContract.LocationId = selectedLocation.LocationId;
                    //CurrentViewContext.AppointSlotContract.IsOnsiteAppointment = true;
                    IsOnsiteAppointment = true;
                    CurrentViewContext.AppointSlotContract.IsEventType = true;
                    CurrentViewContext.AppointSlotContract.EventName = selectedLocation.EventName;
                    CurrentViewContext.AppointSlotContract.EventDescription = selectedLocation.EventDescription;
                }
                else
                {
                    //CurrentViewContext.AppointSlotContract.IsOnsiteAppointment = false;
                    IsOnsiteAppointment = false;
                    CurrentViewContext.AppointSlotContract.IsEventType = false;
                    CurrentViewContext.SelectedSlotID = ucAppointmentRescheduler.SlotRescheduleContract.SlotID;
                    reserveSlotContractRes = Presenter.ReserveSlot();
                    CurrentViewContext.AppointSlotContract.ReservedSlotID = reserveSlotContractRes.ReservedSlotID;
                    CurrentViewContext.AppointSlotContract.SlotID = ucAppointmentRescheduler.SlotRescheduleContract.SlotID;
                    CurrentViewContext.AppointSlotContract.SlotDate = ucAppointmentRescheduler.SlotRescheduleContract.SlotDate;
                    CurrentViewContext.AppointSlotContract.SlotStartTime = ucAppointmentRescheduler.SlotRescheduleContract.SlotStartTime;
                    CurrentViewContext.AppointSlotContract.SlotEndTime = ucAppointmentRescheduler.SlotRescheduleContract.SlotEndTime;
                    CurrentViewContext.AppointSlotContract.LocationId = ucAppointmentRescheduler.SlotRescheduleContract.LocationId;
                }

                if ((selectedLocation != null && selectedLocation.IsEventCode) ||
                    (!reserveSlotContractRes.IsNullOrEmpty() && reserveSlotContractRes.ReservedSlotID > AppConsts.NONE && reserveSlotContractRes.IsAvailable))
                {
                    //CurrentViewContext.AppointSlotContract.ReservedSlotID = reserveSlotContractRes.ReservedSlotID;
                    //CurrentViewContext.AppointSlotContract.SlotID = ucAppointmentRescheduler.SlotRescheduleContract.SlotID;
                    //CurrentViewContext.AppointSlotContract.SlotDate = ucAppointmentRescheduler.SlotRescheduleContract.SlotDate;
                    //CurrentViewContext.AppointSlotContract.SlotStartTime = ucAppointmentRescheduler.SlotRescheduleContract.SlotStartTime;
                    //CurrentViewContext.AppointSlotContract.SlotEndTime = ucAppointmentRescheduler.SlotRescheduleContract.SlotEndTime;
                    //CurrentViewContext.AppointSlotContract.LocationId = ucAppointmentRescheduler.SlotRescheduleContract.LocationId;
                    //if (Presenter.SaveRescheduledAppointment())

                    //CurrentViewContext.AppointSlotContract.ReservedSlotID = ucAppointmentRescheduler.SlotRescheduleContract.ReservedSlotID;
                    var isLocationUpdateAllowed = LocationUpdateAllowed();
                    var isFingerPrintRejected = IsFingerPrintRejected();
                    ReserveSlotContract reserveSlotContract = Presenter.SubmitApplicantAppointment(isLocationUpdateAllowed, IsOnsiteAppointment, isFingerPrintRejected);
                    if (!reserveSlotContract.IsNullOrEmpty() && reserveSlotContract.ApplicantAppointmentID > AppConsts.NONE)
                    {
                        if (!String.IsNullOrEmpty(reserveSlotContract.ErrorMsg))
                        {
                            // base.ShowErrorInfoMessage(reserveSlotContract.ErrorMsg);
                            lblSuccessMessage.Visible = false;
                            lblErrorMessage.Visible = true;
                            lblInfoMessage.Visible = false;
                            lblErrorMessage.Text = reserveSlotContract.ErrorMsg;
                        }
                        else
                        {
                            // base.ShowSuccessMessage(Resources.Language.APNMNTRSCHDLSCS);
                            lblErrorMessage.Visible = false;
                            lblSuccessMessage.Visible = true;
                            lblInfoMessage.Visible = false;
                            //lblSuccessMessage.Text = "Two factor authentication has been configured successfully.";
                            lblSuccessMessage.Text = Resources.Language.APNMNTRSCHDLSCS;
                            //   divUCScheduleLocationUpdateControl.Style["display"] = "none";
                            Presenter.SendAppointmentRescheduleNotification(isLocationUpdateAllowed);
                            BindAppointmentData();
                            //  ShowHideAppointmentInfo();
                            ResetUCAppointmentRescheduler();
                            //  lnkChangeAppointment.Enabled = true;
                            //  dvUCAppointmentRescheduler.Visible = false;
                            //dvAppointmentButtons.Visible = false;

                            //hide buttons in case of saved Successfully
                            btnSaveAppointment.Visible = false;
                            btnSaveAppointment.Enabled = false;
                            btnCancel.Visible = false;
                            btnCancel.Enabled = false;
                            btnGoBack.Text = Resources.Language.GOBCKTODSHBRD;
                            btnGoBack.ToolTip = Resources.Language.CLKTORTNDSHBRD;
                            btnGoBack.Visible = true;
                            btnGoBack.Enabled = true;
                            RedirectToDashboardFromRescheduling("success");
                        }
                    }
                    else
                    {
                        lblSuccessMessage.Visible = false;
                        lblErrorMessage.Visible = false;
                        lblInfoMessage.Visible = true;
                        lblInfoMessage.Text = Resources.Language.SELSLOTNOLNGRSELSANTHR;
                    }
                }
                else
                {
                    lblSuccessMessage.Visible = false;
                    lblErrorMessage.Visible = false;
                    lblInfoMessage.Visible = true;
                    lblInfoMessage.Text = Resources.Language.SELSLOTNOLNGRSELSANTHR;
                }
            }

        }


        private void ResetUCAppointmentRescheduler()
        {
            ucAppointmentRescheduler.SelectedSlotDate = (DateTime?)null;
            ucAppointmentRescheduler.BindAppointmentRescheduler();
            var rescheduleCalender = (ucAppointmentRescheduler.FindControl("dpRescheduler") as INTERSOFT.WEB.UI.WebControls.WclCalendar);
            if (!rescheduleCalender.IsNullOrEmpty())
                rescheduleCalender.SelectedDate = new DateTime();
        }

        private void BindAppointmentData()
        {
            Presenter.GetBkgOrderWithAppointmentData();
            if (!CurrentViewContext.AppointSlotContract.IsNullOrEmpty()
                && !CurrentViewContext.AppointSlotContract.IsOutOfStateAppointment)
            {
                TimeSpan startTime = CurrentViewContext.AppointSlotContract.SlotStartTimeTimeSpanFormat;
                TimeSpan endTime = CurrentViewContext.AppointSlotContract.SlotEndTimeTimeSpanFormat;
                if (CurrentViewContext.AppointSlotContract.SlotDate.IsNotNull())
                {
                    DateTime slotStartDateTime = CurrentViewContext.AppointSlotContract.SlotDate.Value.Add(startTime);
                    DateTime slotEndDateTime = CurrentViewContext.AppointSlotContract.SlotDate.Value.Add(endTime);
                }



                // txtLocationName.Text = String.IsNullOrEmpty(CurrentViewContext.AppointSlotContract.LocationName) ? String.Empty : CurrentViewContext.AppointSlotContract.LocationName;
                // txtLocationAddress.Text = String.IsNullOrEmpty(CurrentViewContext.AppointSlotContract.LocationAddress) ? String.Empty : CurrentViewContext.AppointSlotContract.LocationAddress;
                // lblSiteDescription.Text = String.IsNullOrEmpty(CurrentViewContext.AppointSlotContract.LocDescription) ? String.Empty : CurrentViewContext.AppointSlotContract.LocDescription;
                //txtAppointmentStatus.Text= String.IsNullOrEmpty(CurrentViewContext.AppointSlotContract.AppointmentStatus) ? String.Empty : CurrentViewContext.AppointSlotContract.AppointmentStatus;
                //Bug-93
                // lblAppointmentDateTime.Text = slotStartDateTime.ToString("MM/dd/yyyy") + " (" + slotStartDateTime.ToString("hh:mm tt") + " - " + slotEndDateTime.ToString("hh:mm tt") + ") ";
                //lnkChangeAppointment.Text = slotStartDateTime.ToString("dd/MM/yyyy HH:mm") + " - " + slotEndDateTime.ToString("dd/MM/yyyy HH:mm");

                //ucAppointmentRescheduler.SlotId = CurrentViewContext.AppointSlotContract.SlotID;
                ucAppointmentRescheduler.LocationId = CurrentViewContext.AppointSlotContract.LocationId;
                //ucAppointmentRescheduler.SelectedSlotDate = CurrentViewContext.AppointSlotContract.SlotDate;
                //ucAppointmentRescheduler.SelectedSlotStartTime = Convert.ToString(CurrentViewContext.AppointSlotContract.SlotStartTimeTimeSpanFormat);
                ucAppointmentRescheduler.IsOrderPaymentDetailScreen = true;
                ucAppointmentRescheduler.IsCreateOrderScreen = false;
                hdnLocId.Value = Convert.ToString(CurrentViewContext.AppointSlotContract.LocationId);
            }
        }
        Boolean IsFingerPrintRejected()
        {
            if (CurrentViewContext.AppointSlotContract.AppointmentStatusCode == FingerPrintAppointmentStatus.FINGERPRINT_FILE_ERROR.GetStringValue()
                   || CurrentViewContext.AppointSlotContract.AppointmentStatusCode == FingerPrintAppointmentStatus.FINGERPRINT_FILE_REJECTED.GetStringValue()
                   || CurrentViewContext.AppointSlotContract.AppointmentStatusCode == FingerPrintAppointmentStatus.CBI_FINGERPRINT_FILE_REJECTED.GetStringValue()
                   || CurrentViewContext.AppointSlotContract.AppointmentStatusCode == FingerPrintAppointmentStatus.REJECTED_BY_ABI.GetStringValue())
            {
                return true;
            }

            return false;
        }

        Boolean LocationUpdateAllowed()
        {
            if (IsFingerPrintRejected())
            {
                return true;
            }

            if (!(CurrentViewContext.AppointSlotContract.IsOutOfStateAppointment))
            {
                return true;
            }

            return false;
        }

        protected void btnSaveAppointment_Click(object sender, EventArgs e)
        {
            try
            {
                SaveAppointmentInfo(null);
                btnSaveAppointment.Enabled = false;
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                BindAppointmentData();
                // lnkChangeAppointment.Enabled = true;
                ResetUCAppointmentRescheduler();
                RedirectToDashboard();
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

        /// <summary>
        /// To route Page Back
        /// </summary>
        /// <param name="showSuccessMessage"></param>
        private void RedirectToDashboard()
        {
            String url = String.Format(CurrentViewContext.ParentControl);
            Response.Redirect(url);
        }

        private void RedirectToDashboardFromRescheduling(string AppointmentStatus)
        {
            String url = String.Format(CurrentViewContext.ParentControl);
            Response.Redirect(url + "?RescheduleAppointmentStatus=" + AppointmentStatus);
        }
        private void CaptureQueryString()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);
                if (args.ContainsKey("TenantId"))
                {
                    CurrentViewContext.TenantId = Convert.ToInt32(args["TenantId"]);
                }
                if (args.ContainsKey("OrderId"))
                {
                    CurrentViewContext.OrderId = Convert.ToInt32(args["OrderId"]);
                }
                if (args.ContainsKey("hdfFingerPrint"))
                {
                    CurrentViewContext.IsFingerPrintSvcAvailable = Convert.ToBoolean(args["hdfFingerPrint"]);
                }
                if (args.ContainsKey("hdfPassport"))
                {
                    CurrentViewContext.IsPassportPhotoSvcAvailable = Convert.ToBoolean(args["hdfPassport"]);
                }
                if (args.ContainsKey("Parent"))
                {
                    CurrentViewContext.ParentControl = Convert.ToString(args["Parent"]);
                }
                if (args.ContainsKey("SubscriptionOptionID"))
                {
                    CurrentViewContext.SubscriptionOptionID = Convert.ToInt32(args["SubscriptionOptionID"]);
                }
            }
        }



        //private void isValidRescheduleLocation(int LocationId, int OrderId)
        //{

        //    List<BackgroundPackagesContract> lstPackages = new List<BackgroundPackagesContract>();
        //    var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();

        //        Int32 tenantID = CurrentViewContext.TenantId;
        //        if (tenantID > 0)
        //        {

        //            LocationContract OrderLocationDetails = Presenter.GetLocationByOrderId(OrderId, tenantID);
        //            if (OrderLocationDetails != null)
        //            {
        //                if (LocationId == OrderLocationDetails.LocationID)
        //                RescheduleType=(int)ReshduleOrderType.Valid;
        //            }


        //            Dictionary<Int32, Int32> lstLocationHierarchy = ApiSecurityManager.GetLocationHierarchy(tenantID, LocationId);
        //            var _defaultNodeId = ComplianceDataManager.GetDefaultNodeId(tenantID);
        //            var SelectedHierarchyNodeID = lstLocationHierarchy.OrderByDescending(x => x.Key).FirstOrDefault().Value;
        //            if (!lstLocationHierarchy.Values.Contains(_defaultNodeId))
        //            {
        //                //Set the default node id in SelectedHierarchyNodeIds
        //                lstLocationHierarchy.Add(AppConsts.NONE, _defaultNodeId);
        //            }

        //            //lstPackages = GetBackgroundPackages(lstLocationHierarchy, organizationUserID, tenantID);
        //            //List<BackgroundServiceContract> lstbkgServices = Presenter.GetOrderBackgroundServices(OrderId, tenantID);
        //            //// convert in int list
        //            //// Check service diffrence validation 
        //            //List<String> serciveIds = lstbkgServices.Select(x => x.BST_Code).ToList();
        //            //bool DiffServices = lstPackages.All(x => serciveIds.Contains(x.ServiceCode));
        //            //if (!DiffServices)
        //            //    return (int)ReshduleOrderType.Invalid;



        //            //// if any addtiotnal service is  not shiped  then we need to refund validation 
        //            //bool additionalServicesNotshipped = Presenter.AdditionalServicesNotshipped(OrderId, tenantID);

        //            //if(additionalServicesNotshipped)
        //            //{
        //            //    return (int)ReshduleOrderType.Invalid;
        //            //}

        //            // payment ==> Modifi Ship  payment page else printer yes  then end 
        //            LocationContract SelectedLocationDetails = Presenter.GetLocationByLocationid(LocationId, tenantID);

        //            if (OrderLocationDetails.IsPrinterAvailable == true && SelectedLocationDetails.IsPrinterAvailable == true)
        //            {
        //                    RescheduleType = (int)ReshduleOrderType.Valid;

        //            }
        //            if (OrderLocationDetails.IsPrinterAvailable == false && SelectedLocationDetails.IsPrinterAvailable == false)
        //            {
        //                    RescheduleType = (int)ReshduleOrderType.Valid;

        //             }
        //            if (OrderLocationDetails.IsPrinterAvailable == true && SelectedLocationDetails.IsPrinterAvailable == false)
        //            {
        //                    RescheduleType = (int)ReshduleOrderType.ValidWithmailing;
        //            }
        //            if (OrderLocationDetails.IsPrinterAvailable == false && SelectedLocationDetails.IsPrinterAvailable == true)
        //            {
        //                    RescheduleType = (int)ReshduleOrderType.Invalid;
        //            }
        //        }
        //            RescheduleType = (int)ReshduleOrderType.Invalid;
        //}


        //public List<BackgroundPackagesContract> GetBackgroundPackages(Dictionary<Int32, Int32> dicDPMIds, Int32 organizationUserId, Int32 tenantId)
        //{
        //    dicDPMIds = dicDPMIds.OrderByDescending(dpmId => dpmId.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

        //    StringBuilder _sbDMPIds = new StringBuilder();
        //    _sbDMPIds.Append("<DPMIds>");
        //    foreach (var _dpmId in dicDPMIds)
        //    {
        //        _sbDMPIds.Append("<DPMId nodeLevel='" + _dpmId.Key + "'>" + _dpmId.Value + "</DPMId>");
        //    }
        //    _sbDMPIds.Append("</DPMIds>");

        //    List<BackgroundPackagesContract> _tempList = ApiSecurityManager.GetBackgroundPackages(Convert.ToString(_sbDMPIds), organizationUserId, tenantId, true);
        //    List<BackgroundPackagesContract> _finalList = new List<BackgroundPackagesContract>();

        //    foreach (var _dpmId in dicDPMIds)
        //    {
        //        if (_tempList.Any(bp => bp.NodeLevel == _dpmId.Key))
        //        {
        //            _tempList.Where(bp => bp.NodeLevel == _dpmId.Key).ForEach(x =>
        //            {
        //                x.InsitutionHierarchyNodeID = _dpmId.Value;
        //            }
        //            );
        //            _finalList.AddRange(_tempList.Where(bp => bp.NodeLevel == _dpmId.Key).ToList());
        //            break;
        //        }
        //    }
        //    if (_finalList.IsNotNull() && _finalList.Count > AppConsts.NONE)
        //        _finalList = IncludeParentNodeBackgroundPackages(_tempList, dicDPMIds, _finalList);

        //    return _finalList.OrderBy(x => x.DisplayOrder).ToList();
        //}

        //private List<BackgroundPackagesContract> IncludeParentNodeBackgroundPackages(List<BackgroundPackagesContract> _tempList, Dictionary<Int32, Int32> dicDPMIds, List<BackgroundPackagesContract> _finalList)
        //{
        //    Dictionary<Int32, Int32> dicDPMId = dicDPMIds.OrderByDescending(dpmId => dpmId.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
        //    dicDPMId.Remove(dicDPMIds.Where(x => x.Key == _finalList.FirstOrDefault().NodeLevel).FirstOrDefault().Key);

        //    foreach (var _dpmId in dicDPMId)
        //    {
        //        if (_tempList.Any(bp => bp.NodeLevel == _dpmId.Key))
        //        {
        //            _finalList.AddRange(_tempList.Where(bp => bp.NodeLevel == _dpmId.Key && _finalList.All(x => x.PackageTypeCode != bp.PackageTypeCode)).ToList());
        //        }
        //    }

        //    return _finalList;
        //}

        private void SetSessionDataToRescheduling(Int32 orderId, Int32 subscriptionOptionId, Order currentOrder)
        {
            Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
            Session.Remove(AppConsts.DISCLOSURE_ACCEPTED);
            Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);
            ApplicantOrderCart applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;

            if (applicantOrderCart == null)
            {
                applicantOrderCart = new ApplicantOrderCart();
                applicantOrderCart.lstApplicantOrder = new List<ApplicantOrder>();
                applicantOrderCart.lstApplicantOrder.Add(new ApplicantOrder() { });
            }
            applicantOrderCart.lstApplicantOrder[0].OrderId = orderId;
            applicantOrderCart.AddOrderStageTrackID(OrderStages.Disclaimer);
            //applicantOrderCart.lstApplicantOrder[0].OrderId = orderId;
            applicantOrderCart.IsReadOnly = true;
            if (!currentOrder.IsNullOrEmpty())
            {
                Boolean isBkgPackageIncluded = false;
                Boolean ifInvoiceIsOnlyPaymentOptions = false;
                List<OrderPaymentDetail> PaymentDetailList = Presenter.GetOrderPaymentDetails(currentOrder);
                isBkgPackageIncluded = Presenter.IsBackgroundPackageIncluded(PaymentDetailList);
                String compPkgTypeCode = String.Empty;
                applicantOrderCart.SelectedHierarchyNodeID = currentOrder.SelectedNodeID;

                if (isBkgPackageIncluded)
                {
                    List<BkgOrderPackage> bkgOrderPkgLst = new List<BkgOrderPackage>();
                    bkgOrderPkgLst = Presenter.GetBkgOrderPackageDetail(PaymentDetailList);

                    AddBackgroundPackageDataToSession(applicantOrderCart, bkgOrderPkgLst);
                    //GenerateCustomFormData(applicantOrderCart, currentOrder.OrderID);
                }
                else if (!applicantOrderCart.IsNullOrEmpty() && !applicantOrderCart.lstApplicantOrder.IsNullOrEmpty())
                {
                    applicantOrderCart.lstApplicantOrder[0].lstPackages = null;
                }
                applicantOrderCart.IfInvoiceIsOnlyPaymentOptionAvailableAtNodeLevel = ifInvoiceIsOnlyPaymentOptions;
                SetApplicantProfileDataInSession(applicantOrderCart, currentOrder);

                applicantOrderCart.OrderRequestType = OrderRequestType.ModifyShipping.GetStringValue();
                applicantOrderCart.IsLocationServiceTenant = true;
                // if (IsLocationServiceTenant)
                // {
                if (applicantOrderCart.FingerPrintData.IsNull())
                    applicantOrderCart.FingerPrintData = new FingerPrintAppointmentContract();
                //  applicantOrderCart.FingerPrintData.CBIUniqueID = lstDataForCustomForm.Where(cond => cond.AttributName == "CBIUniqueID").Select(sel => sel.Value).FirstOrDefault();
                //}
                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, applicantOrderCart);
            }
        }

        private void GetPricing(DeptProgramPackageSubscription deptProgramPackageSubscription, out Decimal netPrice, out Decimal actualPrice)
        {
            netPrice = 0;
            actualPrice = deptProgramPackageSubscription.DPPS_TotalPrice == null ? 0 : deptProgramPackageSubscription.DPPS_TotalPrice.Value;
            netPrice = actualPrice;
        }
        private void AddBackgroundPackageDataToSession(ApplicantOrderCart applicantOrderCart, List<BkgOrderPackage> bkgOrderPkgLst)
        {
            List<BackgroundPackagesContract> _lstBackgroundPackages = new List<BackgroundPackagesContract>();

            bkgOrderPkgLst.ForEach(bop =>
            {
                #region UAT-1867: Added this check to reolve issue Price was not dispaying in completeOrder process for bkgPackage, Added this check temporarily Need to verify again
                List<lkpPaymentOption> lstPaymentOptions = new List<lkpPaymentOption>();
                Boolean? IsInvoiceOnlyAtPackageLevel = null;
                List<BkgPackagePaymentOption> lstBkgPackagePaymentOptions = bop.BkgPackageHierarchyMapping.BkgPackagePaymentOptions.Where(cond => !cond.BPPO_IsDeleted).ToList();
                if (lstBkgPackagePaymentOptions.IsNotNull() && lstBkgPackagePaymentOptions.Count > 0)
                {
                    lstPaymentOptions = lstBkgPackagePaymentOptions.Select(col => col.lkpPaymentOption).ToList();
                }
                if (lstPaymentOptions.Count == 0)
                {
                    IsInvoiceOnlyAtPackageLevel = null;
                }
                else if (lstPaymentOptions.Count == 1)
                {
                    IsInvoiceOnlyAtPackageLevel = lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue() || t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue());
                }
                else if (lstPaymentOptions.Count == 2)
                {
                    IsInvoiceOnlyAtPackageLevel = lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()) && lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue());
                }
                else
                {
                    IsInvoiceOnlyAtPackageLevel = false;
                }
                #endregion


                _lstBackgroundPackages.Add(new BackgroundPackagesContract
                {
                    BPAId = bop.BkgPackageHierarchyMapping.BackgroundPackage.BPA_ID,
                    BPAName = String.IsNullOrEmpty(bop.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Label) ?
                                            bop.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Name : bop.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Label,
                    IsExclusive = bop.BkgPackageHierarchyMapping.BPHM_IsExclusive,
                    BPHMId = bop.BkgPackageHierarchyMapping.BPHM_ID,
                    BasePrice = (bop.BOP_BasePrice.IsNullOrEmpty() ? AppConsts.NONE : bop.BOP_BasePrice.Value),
                    //+ (bop.BOP_TotalLineItemPrice.IsNullOrEmpty() ? AppConsts.NONE : bop.BOP_TotalLineItemPrice.Value) 
                    TotalBkgPackagePrice = (bop.BOP_BasePrice.IsNullOrEmpty() ? AppConsts.NONE : bop.BOP_BasePrice.Value)
                                                    + (bop.BOP_TotalLineItemPrice.IsNullOrEmpty() ? AppConsts.NONE : bop.BOP_TotalLineItemPrice.Value),
                    TotalLineItemPrice = bop.BOP_TotalLineItemPrice.IsNullOrEmpty() ? AppConsts.NONE : bop.BOP_TotalLineItemPrice.Value,
                    IsInvoiceOnlyAtPackageLevel = IsInvoiceOnlyAtPackageLevel

                });
            });
            applicantOrderCart.lstApplicantOrder[0].lstPackages = _lstBackgroundPackages;
        }
        private void SetApplicantProfileDataInSession(ApplicantOrderCart applicantOrderCart, Order currentOrder)
        {

            Entity.ClientEntity.OrganizationUserProfile organizationUserProfile = new Entity.ClientEntity.OrganizationUserProfile();
            organizationUserProfile.FirstName = currentOrder.OrganizationUserProfile.FirstName;
            organizationUserProfile.LastName = currentOrder.OrganizationUserProfile.LastName;
            organizationUserProfile.MiddleName = currentOrder.OrganizationUserProfile.MiddleName;
            organizationUserProfile.Gender = currentOrder.OrganizationUserProfile.Gender;
            organizationUserProfile.DOB = currentOrder.OrganizationUserProfile.DOB;
            //organizationUserProfile.PrimaryEmailAddress = txtPrimaryEmail.Text;
            //
            organizationUserProfile.PrimaryEmailAddress = currentOrder.OrganizationUserProfile.PrimaryEmailAddress;
            organizationUserProfile.SecondaryEmailAddress = currentOrder.OrganizationUserProfile.SecondaryEmailAddress;
            organizationUserProfile.SecondaryPhone = currentOrder.OrganizationUserProfile.SecondaryPhone;
            organizationUserProfile.SSN = Presenter.GetDecryptedSSN(currentOrder.OrganizationUserProfile.OrganizationUserProfileID);
            organizationUserProfile.OrganizationUserProfileID = currentOrder.OrganizationUserProfile.OrganizationUserProfileID;
            organizationUserProfile.PhoneNumber = currentOrder.OrganizationUserProfile.PhoneNumber;
            organizationUserProfile.OrganizationUserID = currentOrder.OrganizationUserProfile.OrganizationUserID;
            organizationUserProfile.AddressHandleID = currentOrder.OrganizationUserProfile.AddressHandleID;
            organizationUserProfile.IsActive = true;
            //UAT 4243

            organizationUserProfile.UserTypeID = currentOrder.OrganizationUserProfile.UserTypeID;

            organizationUserProfile.AddressHandle = new AddressHandle
            {
                AddressHandleID = currentOrder.OrganizationUserProfile.AddressHandle.AddressHandleID
            };

            var Addresses = currentOrder.OrganizationUserProfile.AddressHandle.Addresses.FirstOrDefault();
            organizationUserProfile.AddressHandle.Addresses = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Address>();
            organizationUserProfile.AddressHandle.Addresses.Add(new Address
            {
                AddressHandleID = currentOrder.OrganizationUserProfile.AddressHandle.AddressHandleID,
                Address1 = Addresses.Address1,
                Address2 = Addresses.Address2,
                ZipCodeID = Addresses.ZipCodeID
            });

            if (applicantOrderCart != null)
            {
                String clientMachineIP = currentOrder.OrderMachineIP;
                Boolean isUserGroupCustomAttributeExist = false;
                isUserGroupCustomAttributeExist = Presenter.IsUserGroupCustomAttributeExist(CustomAttributeUseTypeContext.Hierarchy.GetStringValue(), currentOrder.SelectedNodeID.Value);
                applicantOrderCart.AddOrganizationUserProfile(organizationUserProfile, false, clientMachineIP);

                applicantOrderCart.IsUserGroupCustomAttributeExist = isUserGroupCustomAttributeExist;
                if (isUserGroupCustomAttributeExist)
                {
                    //TPDO:25/01/2016
                    applicantOrderCart.lstCustomAttributeUserGroupIDs = new List<Int32>();
                    //applicantOrderCart.lstCustomAttributeUserGroupIDs = (caOtherDetails).GetUserGroupCustomAttributeValues();
                }

                //TPDO:25/01/2016
                //applicantOrderCart.lstApplicantOrder[0].IsSendBackgroundReport = chkSendBkgReport.Checked;
                applicantOrderCart.lstApplicantOrder[0].MVRIsValidDriverLicenseAndState = false;

                #region UAT-1578 : Addition of SMS notification
                applicantOrderCart.lstApplicantOrder[0].IsReceiveTextNotification = false;
                applicantOrderCart.lstApplicantOrder[0].PhoneNumber = String.Empty;
                #endregion

                //Get Residential history for current order
                List<ResidentialHistoryProfile> lstResidentialHistory = new List<ResidentialHistoryProfile>();
                lstResidentialHistory = currentOrder.OrganizationUserProfile.ResidentialHistoryProfiles.Where(resHis => !resHis.RHIP_IsDeleted).ToList();

                List<PersonAliasProfile> lstPersonAlias = currentOrder.OrganizationUserProfile.PersonAliasProfiles.Where(alias => !alias.PAP_IsDeleted).ToList();

                ResidentialHistoryProfile currentAddressDB = lstResidentialHistory.FirstOrDefault(cnd => cnd.RHIP_IsCurrentAddress && !cnd.RHIP_IsDeleted);

                var addressLookup = Presenter.GetAddressLookupByHandlerId(Convert.ToString(currentAddressDB.Address.AddressHandleID));

                PreviousAddressContract currentAddress = new PreviousAddressContract();
                currentAddress.Address1 = currentAddressDB.Address.Address1;
                currentAddress.Address2 = currentAddressDB.Address.Address2;
                currentAddress.ZipCodeID = currentAddressDB.Address.ZipCodeID.Value;
                currentAddress.Zipcode = addressLookup.ZipCode;
                currentAddress.CityName = addressLookup.CityName;
                currentAddress.StateName = addressLookup.FullStateName;
                currentAddress.Country = addressLookup.CountryName;
                //currentAddress.CountryId = addressLookup.CountryId;
                if (currentAddressDB.Address.ZipCodeID.Value > 0)
                {
                    currentAddress.CountyName = addressLookup.CountyName;
                }
                currentAddress.ResidenceStartDate = currentAddressDB.RHIP_ResidenceStartDate;
                currentAddress.ResidenceEndDate = currentAddressDB.RHIP_ResidenceEndDate;
                currentAddress.isCurrent = currentAddressDB.RHIP_IsCurrentAddress;
                currentAddress.ResHistorySeqOrdID = currentAddressDB.RHIP_SequenceOrder.IsNullOrEmpty() ? AppConsts.ONE : currentAddressDB.RHIP_SequenceOrder.Value;
                applicantOrderCart.lstPrevAddresses = new List<PreviousAddressContract>();
                applicantOrderCart.lstPrevAddresses.Add(currentAddress);
                Int32 resHisSequenceNo = AppConsts.ONE;
                foreach (var addressHist in lstResidentialHistory.Where(add => !add.RHIP_IsDeleted && !add.RHIP_IsCurrentAddress).ToList())
                {
                    resHisSequenceNo += AppConsts.ONE;
                    var prevAddressLookup = Presenter.GetAddressLookupByHandlerId(Convert.ToString(addressHist.Address.AddressHandleID));

                    PreviousAddressContract prevAddress = new PreviousAddressContract();
                    prevAddress.Address1 = addressHist.Address.Address1;
                    prevAddress.Address2 = addressHist.Address.Address2;
                    prevAddress.ZipCodeID = addressHist.Address.ZipCodeID.Value;
                    prevAddress.Zipcode = prevAddressLookup.ZipCode;
                    prevAddress.CityName = prevAddressLookup.CityName;
                    prevAddress.StateName = prevAddressLookup.FullStateName;
                    prevAddress.Country = prevAddressLookup.CountryName;
                    //prevAddress.CountryId = prevAddressExt.AE_CountryID;
                    if (addressHist.Address.ZipCodeID.Value > 0)
                    {
                        prevAddress.CountyName = prevAddressLookup.CountyName;
                    }
                    prevAddress.ResidenceStartDate = addressHist.RHIP_ResidenceStartDate;
                    prevAddress.ResidenceEndDate = addressHist.RHIP_ResidenceEndDate;
                    prevAddress.isCurrent = addressHist.RHIP_IsCurrentAddress;
                    prevAddress.ResHistorySeqOrdID = addressHist.RHIP_SequenceOrder.IsNullOrEmpty() ? resHisSequenceNo : addressHist.RHIP_SequenceOrder.Value;
                    //applicantOrderCart.lstPrevAddresses = new List<PreviousAddressContract>();
                    applicantOrderCart.lstPrevAddresses.Add(prevAddress);
                    applicantOrderCart.IsResidentialHistoryVisible = true;
                }

                applicantOrderCart.lstPersonAlias = new List<PersonAliasContract>();

                Int32 sequenceNo = AppConsts.NONE;

                lstPersonAlias.ForEach(alias =>
                {
                    sequenceNo += 1;
                    PersonAliasContract personAlias = new PersonAliasContract();
                    personAlias.FirstName = alias.PAP_FirstName;
                    personAlias.LastName = alias.PAP_LastName;
                    personAlias.ID = alias.PAP_ID;
                    personAlias.AliasSequenceId = sequenceNo;
                    //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                    personAlias.MiddleName = alias.PAP_MiddleName;

                    applicantOrderCart.lstPersonAlias.Add(personAlias);
                });

                applicantOrderCart.IsAccountUpdated = false;
            }
        }
    }
}