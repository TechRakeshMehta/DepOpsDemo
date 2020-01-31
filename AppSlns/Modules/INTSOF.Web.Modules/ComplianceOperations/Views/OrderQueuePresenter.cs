using System;
using System.Collections.Generic;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using System.Linq;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Xml;
using Business.Interfaces;
using ExternalVendors.ClearStarVendor;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public class OrderQueuePresenter : Presenter<IOrderQueueView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
            if (SecurityManager.DefaultTenantID == View.TenantId)
            {
                View.ShowClientDropDown = true;
                View.lstTenant = ComplianceDataManager.getClientTenant();
            }
            else
            {
                View.ShowClientDropDown = false;
            }
            GetGranularPermissionForDOBandSSN();//UAT-806
        }

        /// <summary>
        /// Gets the tenant id for the looged in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Gets the data from table ApplicantComplianceDataItems.
        /// </summary>
        //public void GetOrderQueueData()
        //{
        //    SearchItemDataContract searchItemDataContract = new SearchItemDataContract();
        //    //searchItemDataContract.LstStatusCode = new List<String>();
        //    searchItemDataContract.ClientID = View.TenantId;
        //    //Checks if the logged in user is admin and some client is selected from the dropdown.
        //    if (searchItemDataContract.ClientID == SecurityManager.DefaultTenantID && View.SelectedTenantId != AppConsts.NONE)
        //    {
        //        searchItemDataContract.ClientID = View.SelectedTenantId;
        //    }
        //    if (!(searchItemDataContract.ClientID == SecurityManager.DefaultTenantID && View.SelectedTenantId == AppConsts.NONE))
        //    {
        //        try
        //        {
        //            if (View.SelectedOrderStatusCode.Count() > AppConsts.NONE)
        //            {
        //                ///////////////======>                //pass comma separated String for SelectedOrderStatusCode
        //                searchItemDataContract.LstStatusCode = View.SelectedOrderStatusCode;
        //            }
        //            if (View.SelectedPaymentTypeCode.Count() > AppConsts.NONE)
        //            {
        //                ///////////////======>                //pass comma separated String for SelectedPaymentTypeCode
        //                searchItemDataContract.LstPaymentType = View.SelectedPaymentTypeCode;
        //            }
        //            if (View.ShowOnlyRushOrders == true)
        //                searchItemDataContract.ShowOnlyRushOrder = View.ShowOnlyRushOrders; ///////////////======>   send null otherwise
        //            if (View.SelectedArchiveStateCode.IsNotNull())
        //            {
        //                //========>  to be removed, as no filter  for archived/unarchived orders
        //                searchItemDataContract.LstArchiveState = View.SelectedArchiveStateCode;
        //            }
        //            if (View.lstSelectedOrderPkgType.Count() > AppConsts.NONE)
        //            {
        //                ///////////////======>                //pass comma separated String for OrderPackageType
        //                searchItemDataContract.lstOrderPackageTypes = View.lstSelectedOrderPkgType;
        //            }
        //            else
        //            {
        //                searchItemDataContract.lstOrderPackageTypes = new List<String>();
        //                searchItemDataContract.lstOrderPackageTypes.Add(OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue());
        //                searchItemDataContract.lstOrderPackageTypes.Add(OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue());
        //                searchItemDataContract.lstOrderPackageTypes.Add(OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue());
        //            }

        //            //searchItemDataContract.DepartmentID = View.SelectedDepartment;
        //            //searchItemDataContract.LstProgramID = View.SelectedPrograms;
        //            //searchItemDataContract.LstPackageID = View.SelectedPackages;

        //            searchItemDataContract.DisallowApostropheConversion = true; //////===> to be removed

        //            searchItemDataContract.ApplicantFirstName = View.FirstNameSearch;
        //            searchItemDataContract.ApplicantLastName = View.LastNameSearch;
        //            searchItemDataContract.OrderID = View.OrderNumberSearch;
        //            searchItemDataContract.LstOrderCreatedDate = View.LstOrderCrtdDate; /////==> send in format of from date and to date
        //            searchItemDataContract.LstOrderPaidDate = View.LstOrderPaidDate;/////==> send in format of from date and to date
        //            searchItemDataContract.SelectedDPMIds = View.DeptProgramMappingID;

        //            // START UAT-539 Rajeev Jha 11 Aug 2014
        //            //Populating List of node it with all children node of selected hierarchy including selected one
        //            //Still keeping above DeptProgramMappingID value so that filter can be retained after coming back from detail page
        //            if (View.DeptProgramMappingID != String.Empty)
        //            {
        //                ///////////////======>                  //to be removed, just pass DeptProgramMappingID as parameter to SP as @TargetHierarchyNodeIds
        //                searchItemDataContract.LstNodeId = ComplianceDataManager.GetChildInstituteNodeIDs(View.DeptProgramMappingID, searchItemDataContract.ClientID);
        //            }
        //            // END UAT-539 Rajeev Jha 11 Aug 2014

        //            if (View.GridCustomPaging.SortExpression.IsNullOrEmpty())
        //                View.GridCustomPaging.SortDirectionDescending = true;
        //            View.GridCustomPaging.DefaultSortExpression = QueueConstants.ORDER_QUEUE_DEFAULT_SORTING_FIELDS;
        //            View.GridCustomPaging.SecondarySortExpression = QueueConstants.ORDER_QUEUE_SECONDARY_SORTING_FIELDS;
        //            View.SetSearchItemDataContract = searchItemDataContract;
        //            if (!(View.SelectedOrderStatusCode.Count() > AppConsts.NONE))
        //            {
        //                searchItemDataContract.LstStatusCode = new List<String>();
        //                searchItemDataContract.LstStatusCode.Add(ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue());
        //                searchItemDataContract.LstStatusCode.Add(ApplicantOrderStatus.Cancellation_Requested.GetStringValue());
        //                searchItemDataContract.LstStatusCode.Add(ApplicantOrderStatus.Cancelled.GetStringValue());
        //                searchItemDataContract.LstStatusCode.Add(ApplicantOrderStatus.Paid.GetStringValue());
        //                searchItemDataContract.LstStatusCode.Add(ApplicantOrderStatus.Payment_Rejected.GetStringValue());
        //                searchItemDataContract.LstStatusCode.Add(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue());
        //            }
        //            if (View.TenantId != SecurityManager.DefaultTenantID)
        //            {
        //                ///////////////======>                  //to be removed, just pass CurrentLoggedInUserID as parameter to SP
        //                //UAT-1181: Ability to restrict additional nodes to the order queue
        //                //searchItemDataContract.LstUserNodePermissions = ComplianceDataManager.GetUserNodePermissions(searchItemDataContract.ClientID, View.CurrentLoggedInUserId, searchItemDataContract.ClientID).Select(col => col.DPM_ID).ToList();
        //                searchItemDataContract.LstUserNodePermissions = ComplianceDataManager.GetUserNodeOrderPermissions(searchItemDataContract.ClientID, View.CurrentLoggedInUserId).Select(col => col.DPM_ID).ToList();
        //            }
        //            else
        //            {
        //                searchItemDataContract.LstUserNodePermissions = null;
        //            }

        //            View.lstOrderQueue = ComplianceDataManager.PerformSearchForOrderQueue(searchItemDataContract, View.GridCustomPaging);
        //            View.VirtualPageCount = View.GridCustomPaging.VirtualPageCount;
        //            View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
        //        }
        //        catch (Exception e)
        //        {
        //            View.lstOrderQueue = null;
        //            throw e;
        //        }
        //    }
        //}
        // For UAT -2379
        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }


        /// <summary>
        /// Gets the data from table ApplicantComplianceDataItems.
        /// </summary>
        public void GetOrderQueueData()
        {
            OrderApprovalQueueContract orderApprovalQueueContract = new OrderApprovalQueueContract();

            orderApprovalQueueContract = GetOrderApprovalQueueContract();

            if (!(orderApprovalQueueContract.ClientID == SecurityManager.DefaultTenantID && View.SelectedTenantId == AppConsts.NONE))
            {
                View.GridCustomPaging.DefaultSortExpression = QueueConstants.ORDER_QUEUE_DEFAULT_SORTING_FIELDS;
                View.GridCustomPaging.SecondarySortExpression = QueueConstants.ORDER_QUEUE_SECONDARY_SORTING_FIELDS;
                orderApprovalQueueContract.GridCustomPagingArguments = View.GridCustomPaging;

               
                View.lstOrderQueue = ComplianceDataManager.GetOrderApprovalQueueData(orderApprovalQueueContract, View.GridCustomPaging, View.isBkgScreen);
                if (View.lstOrderQueue.IsNotNull() && View.lstOrderQueue.Count > 0)
                {
                    if (View.lstOrderQueue[0].TotalCount > 0)
                    {
                        View.VirtualRecordCount = View.lstOrderQueue[0].TotalCount;
                    }
                    View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                }
                else
                {
                    View.VirtualRecordCount = 0;
                    View.CurrentPageIndex = 1;
                }
                orderApprovalQueueContract.GridCustomPagingArguments.CurrentPageIndex = View.CurrentPageIndex;
                orderApprovalQueueContract.GridCustomPagingArguments.VirtualPageCount = View.VirtualRecordCount;
                View.SetOrderApprovalQueueContract = orderApprovalQueueContract;
            }
            else
            {
                View.VirtualRecordCount = 0;
                View.CurrentPageIndex = 1;
            }
        }

        private OrderApprovalQueueContract GetOrderApprovalQueueContract()
        {
            OrderApprovalQueueContract orderApprovalQueueContract = new OrderApprovalQueueContract();
            orderApprovalQueueContract.ClientID = View.TenantId;
            //Checks if the logged in user is admin and some client is selected from the dropdown.
            if (orderApprovalQueueContract.ClientID == SecurityManager.DefaultTenantID && View.SelectedTenantId != AppConsts.NONE)
            {
                orderApprovalQueueContract.ClientID = View.SelectedTenantId;
            }

            if (!(orderApprovalQueueContract.ClientID == SecurityManager.DefaultTenantID && View.SelectedTenantId == AppConsts.NONE))
            {
                if (View.SelectedOrderStatusCode.IsNullOrEmpty())
                {
                    List<String> lstStatusCode = new List<String>();
                    lstStatusCode.Add(ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue());
                    lstStatusCode.Add(ApplicantOrderStatus.Cancellation_Requested.GetStringValue());
                    lstStatusCode.Add(ApplicantOrderStatus.Cancelled.GetStringValue());
                    lstStatusCode.Add(ApplicantOrderStatus.Paid.GetStringValue());
                    lstStatusCode.Add(ApplicantOrderStatus.Payment_Rejected.GetStringValue());
                    lstStatusCode.Add(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue());
                    lstStatusCode.Add(ApplicantOrderStatus.Online_Payment_Not_Completed.GetStringValue());
                    lstStatusCode.Add(ApplicantOrderStatus.Pending_School_Approval.GetStringValue());
                    //UAT 2379 ---include Payment due status in search also
                    lstStatusCode.Add(ApplicantOrderStatus.Payment_Due.GetStringValue());
                    orderApprovalQueueContract.OrderStatusCode = String.Join(","
                                                                        , lstStatusCode.ToArray());
                }
                else
                {
                    orderApprovalQueueContract.LstStatusCode = View.SelectedOrderStatusCode;
                    orderApprovalQueueContract.OrderStatusCode = String.Join(","
                                                                        , View.SelectedOrderStatusCode.ToArray());
                }

                if (View.SelectedPaymentTypeCode.Count() > AppConsts.NONE)
                {
                    orderApprovalQueueContract.PaymentTypeCode = String.Join(","
                                                                            , View.SelectedPaymentTypeCode.ToArray());
                    orderApprovalQueueContract.LstPaymentType = View.SelectedPaymentTypeCode;
                }
                else
                {
                    orderApprovalQueueContract.PaymentTypeCode = null;
                }

                if (View.ShowOnlyRushOrders == true)
                {
                    orderApprovalQueueContract.ShowOnlyRushOrder = View.ShowOnlyRushOrders;
                }
                if (View.lstSelectedOrderPkgType.IsNullOrEmpty())
                {
                    orderApprovalQueueContract.lstOrderPackageTypes = new List<String>();
                    orderApprovalQueueContract.lstOrderPackageTypes.Add(OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue());
                    orderApprovalQueueContract.lstOrderPackageTypes.Add(OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue());
                    orderApprovalQueueContract.lstOrderPackageTypes.Add(OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue());
                    //UAT-3077
                    orderApprovalQueueContract.lstOrderPackageTypes.Add(OrderPackageTypes.TRACKING_ITEM_PAYMENT.GetStringValue());
                    orderApprovalQueueContract.lstOrderPackageTypes.Add(OrderPackageTypes.REQUIREMENT_ITEM_PAYMENT.GetStringValue());

                    orderApprovalQueueContract.OrderPackageTypeCode = String.Join(","
                                                                        , orderApprovalQueueContract.lstOrderPackageTypes.ToArray());
                }
                else
                {
                    orderApprovalQueueContract.lstOrderPackageTypes = View.lstSelectedOrderPkgType;
                    orderApprovalQueueContract.OrderPackageTypeCode = String.Join(","
                                                                        , View.lstSelectedOrderPkgType.ToArray());
                }

                orderApprovalQueueContract.FirstName = String.IsNullOrEmpty(View.FirstNameSearch) ? null : View.FirstNameSearch;
                orderApprovalQueueContract.LastName = String.IsNullOrEmpty(View.LastNameSearch) ? null : View.LastNameSearch;
                orderApprovalQueueContract.SSN = ApplicationDataManager.GetSSNForFilters(View.ApplicantSSN);
                if (!View.OrderNumberSearch.IsNullOrEmpty())
                {
                    orderApprovalQueueContract.OrderNumber = View.OrderNumberSearch;
                }
                if (!View.LstOrderCrtdDate.IsNullOrEmpty())
                {
                    orderApprovalQueueContract.OrderFromDate = View.LstOrderCrtdDate[0];
                    orderApprovalQueueContract.OrderToDate = View.LstOrderCrtdDate[1];
                }
                if (!View.LstOrderPaidDate.IsNullOrEmpty())
                {
                    orderApprovalQueueContract.OrderPaidFromDate = View.LstOrderPaidDate[0];
                    orderApprovalQueueContract.OrderPaidToDate = View.LstOrderPaidDate[1];
                }
                orderApprovalQueueContract.LstOrderCreatedDate = View.LstOrderCrtdDate;
                orderApprovalQueueContract.LstOrderPaidDate = View.LstOrderPaidDate;
                if (!View.DeptProgramMappingID.IsNullOrEmpty())
                {
                    orderApprovalQueueContract.DeptProgramMappingIDs = View.DeptProgramMappingID;
                }
                else
                {
                    orderApprovalQueueContract.DeptProgramMappingIDs = null;
                }
                if (View.CurrentLoggedInUserId.IsNotNull())
                {
                    orderApprovalQueueContract.LoggedInUserId = (View.TenantId == SecurityManager.DefaultTenantID) ? (Int32?)null : View.CurrentLoggedInUserId;
                }

            }
            return orderApprovalQueueContract;
        }

        public void GetOrdersIdsToSelectAllRecords()
        {
            OrderApprovalQueueContract orderApprovalQueueContract = new OrderApprovalQueueContract();

            orderApprovalQueueContract = GetOrderApprovalQueueContract();

            if (!(orderApprovalQueueContract.ClientID == SecurityManager.DefaultTenantID && View.SelectedTenantId == AppConsts.NONE))
            {
                View.GridCustomPaging.DefaultSortExpression = QueueConstants.ORDER_QUEUE_DEFAULT_SORTING_FIELDS;
                View.GridCustomPaging.SecondarySortExpression = QueueConstants.ORDER_QUEUE_SECONDARY_SORTING_FIELDS;
                orderApprovalQueueContract.GridCustomPagingArguments = View.GridCustomPaging;
                orderApprovalQueueContract.GridCustomPagingArguments.PageSize = View.VirtualPageCount;

                List<OrderContract> lstOrderContract = ComplianceDataManager.GetOrderApprovalQueueData(orderApprovalQueueContract, View.GridCustomPaging,View.isBkgScreen);
                View.SelectedOrderIds = new Dictionary<int, bool>();

                lstOrderContract = lstOrderContract.Where(cond => (cond.IsInvoiceApproval == true && cond.IsInvoiceApprovalInitiated == false)
                                                               || cond.IsCardWithApproval == true).ToList();

                if (!lstOrderContract.IsNullOrEmpty())
                {
                    foreach (var item in lstOrderContract)
                    {
                        View.SelectedOrderIds.Add(item.OrderId, true);
                    }
                }
            }
            else
            {
                View.SelectedOrderIds = new Dictionary<int, bool>();
            }
        }

        /// <summary>
        /// Gets the list of Payment status.
        /// </summary>
        public void GetOrderStatusList()
        {
            View.lstOrderStatus = ComplianceDataManager.GetOrderStatusList(View.SelectedTenantId);
        }

        /// <summary>
        /// Gets the list of payment type.
        /// </summary>
        public void GetPaymentTypeList()
        {
            View.lstPaymentType = ComplianceDataManager.GetPaymentTypeList(View.SelectedTenantId);
        }

        /// <summary>
        /// Gets the list of Archive State.
        /// </summary>
        public void GetArchiveStateList()
        {
            View.lstArchiveState = ComplianceDataManager.GetArchiveStateList(View.SelectedTenantId);
        }

        /// <summary>
        /// Gets the list of rows for the Order Status.
        /// </summary>
        //public void GetDepartmentsList()
        //{

        //    //View.lstDepartment =
        //        SecurityManager.GetDepartments(View.SelectedTenantId).Select(x => new ComplianceDetail
        //    {
        //        Name = x.OrganizationName,
        //        ID = x.OrganizationID
        //    }).ToList();
        //}

        /// <summary>
        /// Gets the list of Programs.
        /// </summary>
        //public void GetProgramsList(Boolean showAllPrograms)
        //{
        //    if (showAllPrograms)
        //    {
        //        //View.lstProgram = 
        //             SecurityManager.GetAllProgramsForTenantID(View.SelectedTenantId).Select(x => new ComplianceDetail
        //        {
        //            Name = x.ProgramStudy,
        //            ID = x.AdminProgramStudyID
        //        }).ToList();

        //    }
        //    else
        //    {
        //        //View.lstProgram = 
        //            SecurityManager.GetDeptFromDeptPrgMaping(View.SelectedDepartment).Select(x => new ComplianceDetail
        //        {
        //            Name = x.AdminProgramStudy.ProgramStudy,
        //            ID = x.DPM_ProgramID
        //        }).ToList();
        //    }
        //}

        /// <summary>
        /// Gets the list of packages.
        /// </summary>
        public void GetPackagesList(Boolean showAllPackages)
        {
            //    if (showAllPackages)
            //{
            //View.lstPackage = ComplianceSetupManager.GetCompliancePackage(View.SelectedTenantId).Select(x => new ComplianceDetail
            //{
            //    Name = x.PackageName,
            //    ID = x.CompliancePackageID
            //}).ToList();
            //}
            //else
            //{
            //    View.lstPackage = ComplianceSetupManager.GetProgramPackagesByProgramId(View.SelectedDepartment, View.SelectedPrograms, View.SelectedTenantId).Select(x => new ComplianceDetail
            //    {
            //        Name = x.PackageName,
            //        ID = x.CompliancePackageID
            //    }).ToList();
            //}
        }

        public void SaveApproveInvoiceOrdersValueSet()
        {
            List<Int32> lstApproveInvoiceOrders = new List<Int32>(View.SelectedOrderIds.Where(cond => cond.Value).Select(cond => cond.Key));
            if (lstApproveInvoiceOrders.Count <= 0)
            {
                View.InfoMessage = "Please select at least one Order to be approved.";
            }
            else
            {
                String pndgTaskStsTypeCode = TaskStatusType.PENDING.GetStringValue();
                String invcTaskTypeCode = TaskType.INVOICEORDERBULKAPPROVE.GetStringValue();
                Int32 pndgTaskStsTypeID = LookupManager.GetLookUpData<lkpTaskStatusType>(View.SelectedTenantId).FirstOrDefault(condition => condition.TaskStatusTypeCode.Trim() == pndgTaskStsTypeCode && condition.IsActive == true && condition.IsDeleted == false).TaskStatusTypeID;
                Int32 invcTaskTypeID = LookupManager.GetLookUpData<lkpTaskType>(View.SelectedTenantId).FirstOrDefault(condition => condition.TaskTypeCode.Trim() == invcTaskTypeCode && condition.IsActive == true && condition.IsDeleted == false).TaskTypeID;
                List<ScheduledTask> lstScheduleTasks = new List<ScheduledTask>();
                Guid taskGroup = Guid.NewGuid();
                Boolean allTaskScheduled = false;
                foreach (Int32 orderId in lstApproveInvoiceOrders)
                {
                    String invoicePaymentOption = PaymentOptions.InvoiceWithApproval.GetStringValue();
                    List<OrderPaymentDetail> lstOrderPaymentDetail = ComplianceDataManager.GetOrdrPaymentDetailOfOrderByPaymentOpt(View.SelectedTenantId, orderId, invoicePaymentOption);
                    foreach (OrderPaymentDetail orderPaymentDetail in lstOrderPaymentDetail)
                    {
                        String parameter = CreateXml(orderId, orderPaymentDetail);
                        ScheduledTask scheduleTask = new ScheduledTask();
                        scheduleTask.ST_TaskGroup = taskGroup;
                        scheduleTask.ST_TaskStatusID = pndgTaskStsTypeID;
                        scheduleTask.ST_TaskTypeID = invcTaskTypeID;
                        scheduleTask.ST_Parameters = parameter;
                        scheduleTask.ST_IsDeleted = false;
                        scheduleTask.ST_CreatedByID = View.CurrentLoggedInUserId;
                        scheduleTask.ST_CreatedOn = DateTime.Now;
                        scheduleTask.ST_RecordID = orderId;
                        lstScheduleTasks.Add(scheduleTask);
                    }
                }
                if (lstScheduleTasks.Count > 0)
                {
                    allTaskScheduled = ComplianceDataManager.SaveScheduleTask(lstScheduleTasks, View.SelectedTenantId);
                }

                if (allTaskScheduled)
                {
                    View.SuccessMessage = "Payment status for the selected orders are queued for approval. Payment status will update as each order's approval completes.";
                }
                else
                {
                    View.ErrorMessage = "Some error occurred. Please try again.";
                }
            }


        }
        #region UAT-3954
        public List<String> IsOrderExistForCurrentYear(String orderIds)
        {
            if (IsDuplicateAlertEnabled())
            {
                return ComplianceDataManager.IsOrderExistForCurrentYear(orderIds, View.SelectedTenantId);
            }
            else
                return new List<String>();
        }
        private Boolean IsDuplicateAlertEnabled()
        {
            var Code = Setting.ORDER_QUEUE_DUPLICATE_ORDER_POPUP_ALERT.GetStringValue();
            return ComplianceDataManager.GetBkgOrderNoteSetting(View.SelectedTenantId, Code);
        }
        #endregion  
        public String CreateXml(Int32 orderId, OrderPaymentDetail orderPaymentDetail)
        {
            var serializer = new XmlSerializer(typeof(ScheduledTaskContract));
            var sb = new StringBuilder();

            /*UAT-916
             * OrderPaymentDetail orderPaymentDetail = ComplianceDataManager.GetOrderDetailById(View.SelectedTenantId, orderId);*/

            ScheduledTaskContract xmlData = new ScheduledTaskContract();

            xmlData.OrderId = orderId;
            xmlData.ReferenceNumber = View.ReferenceNumber;
            xmlData.ApprovedBy = View.CurrentLoggedInUserId;
            xmlData.ApprovedDate = DateTime.UtcNow.ToString();
            if (orderPaymentDetail.IsNotNull())
            {
                Boolean _isCompPkgInclude = orderPaymentDetail.OrderPkgPaymentDetails.Any(x => x.lkpOrderPackageType.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue());
                if (_isCompPkgInclude)
                {
                    xmlData.PackageId = orderPaymentDetail.Order.DeptProgramPackage.IsNotNull() ? orderPaymentDetail.Order.DeptProgramPackage.DPP_CompliancePackageID : 0;
                }
                else
                {
                    xmlData.PackageId = 0;
                }
                xmlData.OrganisationUserId = orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID;
                DateTime expirydate = DateTime.Now;
                if (orderPaymentDetail.Order.SubscriptionYear.HasValue)
                {
                    expirydate = expirydate.AddYears(orderPaymentDetail.Order.SubscriptionYear.Value);
                }
                if (orderPaymentDetail.Order.SubscriptionMonth.HasValue)
                {
                    expirydate = expirydate.AddMonths(orderPaymentDetail.Order.SubscriptionMonth.Value);
                }
                xmlData.ExpiryDate = expirydate;
                /*UAT-916
                 * xmlData.OrderStatusCode = orderPaymentDetail.Order.lkpOrderStatu.Code;*/
                xmlData.OrderStatusCode = orderPaymentDetail.lkpOrderStatu.Code;
                //Set this property for OrderPaymentDetail information [UAT-916]
                xmlData.OrderPaymentDetailId = orderPaymentDetail.OPD_ID;
            }
            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, xmlData);
            }
            return sb.ToString();
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        public void GetGranularPermissionForDOBandSSN()
        {
            View.IsDOBDisable = false;
            View.SSNPermissionCode = EnumSystemPermissionCode.FULL_PERMISSION.GetStringValue();
            Dictionary<String, String> dicPermissions = new Dictionary<String, String>();
            if (SecurityManager.GetUserGranularPermission(View.CurrentLoggedInUserId, out dicPermissions))
            {
                if (dicPermissions.ContainsKey(EnumSystemEntity.DOB.GetStringValue()) && dicPermissions[EnumSystemEntity.DOB.GetStringValue()].ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
                {
                    View.IsDOBDisable = true;
                }
                if (dicPermissions.ContainsKey(EnumSystemEntity.SSN.GetStringValue()))
                {
                    View.SSNPermissionCode = dicPermissions[EnumSystemEntity.SSN.GetStringValue()];
                }
            }
        }

        /// <summary>
        /// Getting Masked SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetMaskedSSN(String unMaskedSSN)
        {
            return ApplicationDataManager.GetMaskedSSN(unMaskedSSN);
        }

        #endregion

        #region UAT-916 :WB: As an application admin, I should be able to define payment options at the package level in addition to the node level
        public OrderPaymentDetail GetOrderPaymentDetailForOrder(Int32 orderId)
        {
            String compliancePackageTypeCode = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();
            OrderPaymentDetail tempOrderPaymentDetail = new OrderPaymentDetail();
            List<OrderPkgPaymentDetail> tempOrderPkgPaymentDetail = new List<OrderPkgPaymentDetail>();
            tempOrderPkgPaymentDetail = ComplianceDataManager.GetOrderPkgPaymentDetailsByOrderID(View.SelectedTenantId, orderId, compliancePackageTypeCode);
            if (!tempOrderPkgPaymentDetail.IsNullOrEmpty())
            {
                tempOrderPaymentDetail = tempOrderPkgPaymentDetail.Select(slct => slct.OrderPaymentDetail).FirstOrDefault();
            }
            return tempOrderPaymentDetail;
        }
        #endregion

        /// <summary>
        /// Gets the Lookup ams.lkpOrderPackageTypes
        /// </summary>
        public void GetOrderPackageTypes()
        {
            View.lstOrderPackageType = LookupManager.GetLookUpData<lkpOrderPackageType>(View.SelectedTenantId);
        }

        public void GetSSNSetting()
        {
            View.IsSSNDisabled = (View.SelectedTenantId > 0 && View.SelectedTenantId != SecurityManager.DefaultTenantID) ? ComplianceDataManager.GetSSNSetting(View.SelectedTenantId, Setting.DISABLE_SSN.GetStringValue()) : false;
        }

        /// <summary>
        /// Getting Formmated SSN
        /// </summary>
        /// <param name="unformattedSSN"></param>
        /// <returns></returns>
        public String GetFormattedSSN(String unformattedSSN)
        {
            return ApplicationDataManager.GetFormattedSSN(unformattedSSN);
        }

    }
}




