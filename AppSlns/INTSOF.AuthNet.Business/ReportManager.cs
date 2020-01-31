using System;
using System.Collections.Generic;


namespace INTSOF.AuthNet.Business
{
    public class ReportManager : BaseAuthNetManager
    {

        //string loginID = ConfigurationManager.AppSettings["Authorize.Net-ApiLogin"];
        //string transactionKey = ConfigurationManager.AppSettings["Authorize.Net-TranKey"];
        //string testMode = ConfigurationManager.AppSettings["Authorize.Net-IsTestRequest"];
        /*
        ServiceMode serviceMode;

        public ReportManager()
        {
            if (base.IsAuthNetInTestMode == true)
                serviceMode = ServiceMode.Test;
            else
                serviceMode = ServiceMode.Live;
        }

        public bool FetchTransctions() 
        {
            try 
            {
                var gate = new ReportingGateway(base.ApiLogin, base.ApiTransKey);
                var transaction = gate.GetTransactionList();

                return true;
            }
            catch 
            {
                return false;
            }
        }

        public bool FetchBatches()
        {
            try
            {

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Transaction GetTransactionDetail(string TransactionId)
        {
            try
            {
                var gate = new ReportingGateway(base.ApiLogin, base.ApiTransKey);
                Transaction transaction = gate.GetTransactionDetails(TransactionId);
                return transaction;
            }
            catch
            {
                return null;
            }
        }

        public List<WinningBidder> GetHoldToChargeBidders() 
        {
            return new PaymentRepository().GetHoldToChargeBidders();
        }

        public List<vw_AuthNetTransction> GetAuthNetTransctionReport(DateTime From, DateTime To, AuthNetTransctionReportType authNetTransctionReportType)
        {
            return new PaymentRepository().GetAuthNetTransactionReport(From, To, authNetTransctionReportType);
        }

        public List<WinningBidder> GetHoldToVoidBidders()
        {
            return new PaymentRepository().GetHoldToVoidBidders();
        }

        public List<WinningBidder> GetChargeToRevertBidders()
        {
            return new PaymentRepository().GetChargeToRevertBidders();
        }

        public WinningBidder GetSettledCharge() 
        {
            return null;
        }
        
        public bool SubmitHoldToChargeToAuthNet()
        {
            String transId;
            String authCode;
            Decimal holdAmount;
            Boolean isChargeApproved = false;

            AuthNetRequest authNetRequest;
            WinningBidderTransaction winningBidderTransaction;
            WinningBidOperation winningBidOperation;
            BidRepository bidRepository = new BidRepository();
            PaymentManager paymentManager = new PaymentManager();
            PaymentRepository paymentRepository = new PaymentRepository();
            
            List<WinningBidder> winningBidders = paymentRepository.GetHoldToChargeBidders();

            foreach (WinningBidder winningBidder in winningBidders)
            {
                try
                {
                    winningBidderTransaction = bidRepository.GetWinningBidderTransactionByWinningBidderId(winningBidder.WinningBidderId);
                    if (winningBidderTransaction.IsNull())
                    {
                        AuctionsEventLog.WriteLogEntry(BaseAuthNetManager.ClassModule + BaseAuthNetManager.ShowTrace() + Environment.NewLine + "winningBidderTransaction is null for winningBidder.WinningBidderId = " + winningBidder.WinningBidderId.ToString(),
                                    System.Diagnostics.EventLogEntryType.Information); 
                        continue;
                    }

                    authNetRequest = bidRepository.GetAuthNetReqResByAuthNetRequestId(winningBidderTransaction.AuthNetRequest.AuthNetRequestId);
                    if (authNetRequest.IsNull())
                    {
                        AuctionsEventLog.WriteLogEntry(BaseAuthNetManager.ClassModule + BaseAuthNetManager.ShowTrace() + Environment.NewLine + "authNetRequest is null for winningBidder.WinningBidderId = " + winningBidderTransaction.AuthNetRequest.AuthNetRequestId.ToString(),
                                    System.Diagnostics.EventLogEntryType.Information); 
                        continue;
                    }
                    AuthNetRequest newAuthNetRequest = winningBidderTransaction.AuthNetRequest1;
                    AuthNetResponse newAuthNetResponse = new AuthNetResponse();
                    authCode = authNetRequest.AuthNetResponse.FirstOrDefault().x_auth_code;
                    transId = authNetRequest.AuthNetResponse.FirstOrDefault().x_trans_id;
                    holdAmount = (winningBidderTransaction.HoldAmount.IsNull() ? bidRepository.GetPropertyHoldAmount(winningBidder.Property.PropID) : Convert.ToDecimal(winningBidderTransaction.HoldAmount));

                    isChargeApproved = paymentManager.ChargeHold(holdAmount, transId, authCode, ref newAuthNetRequest, ref newAuthNetResponse);

                    winningBidderTransaction.IsHoldCaptured = isChargeApproved;
                    winningBidderTransaction.AuthNetRequest1 = paymentRepository.GetAuthNetRequestById(newAuthNetRequest.AuthNetRequestId);
                    winningBidOperation = winningBidder.WinningBidOperation.FirstOrDefault();
                    winningBidOperation.IsChargeSubmittedToAuthNet = true;
                    bidRepository.Save();
                    //bidRepository.SendCardChargedEmailToBidder(winningBidder.WinningBidderId);
                }
                catch (Exception ex)
                {
                    AuctionsEventLog.WriteLogEntry(BaseAuthNetManager.ClassModule + BaseAuthNetManager.ShowTrace() + Environment.NewLine + ex.Message,
                         System.Diagnostics.EventLogEntryType.Error); 

                    continue;
                }
            }
            return true;
        }
        
        public bool SubmitHoldToVoidToAuthNet()
        {
            return true; // when the property is sold to a user with in <30> days, remove the hold on this card.
        }

        public bool SubmitChargeToRevertToAuthNet()
        {
            String transId;
            //String authCode;
            Decimal holdAmount;
            Boolean isChargeReversed = false;

            AuthNetRequest authNetRequest;
            WinningBidderTransaction winningBidderTransaction;
            WinningBidOperation winningBidOperation;
            BidRepository bidRepository = new BidRepository();
            PaymentManager paymentManager = new PaymentManager();
            PaymentRepository paymentRepository = new PaymentRepository();
            ReportingGateway reportingGateway = new ReportingGateway(base.ApiLogin, base.ApiTransKey, serviceMode);

            List<WinningBidder> winningBidders = paymentRepository.GetChargeToRevertBidders();

            foreach (WinningBidder winningBidder in winningBidders)
            {
                try
                {
                    winningBidderTransaction = bidRepository.GetWinningBidderTransactionByWinningBidderId(winningBidder.WinningBidderId);
                    if (winningBidderTransaction.IsNull())
                    {
                        AuctionsEventLog.WriteLogEntry(BaseAuthNetManager.ClassModule + BaseAuthNetManager.ShowTrace() + Environment.NewLine + "winningBidderTransaction is null for winningBidder.WinningBidderId = " + winningBidder.WinningBidderId.ToString(),
                                    System.Diagnostics.EventLogEntryType.Information); 
                        continue;
                    }

                    authNetRequest = bidRepository.GetAuthNetReqResByAuthNetRequestId(winningBidderTransaction.AuthNetRequest.AuthNetRequestId);
                    if (authNetRequest.IsNull())
                    {
                        AuctionsEventLog.WriteLogEntry(BaseAuthNetManager.ClassModule + BaseAuthNetManager.ShowTrace() + Environment.NewLine + "authNetRequest is null for winningBidder.WinningBidderId = " + winningBidderTransaction.AuthNetRequest.AuthNetRequestId.ToString(),
                                     System.Diagnostics.EventLogEntryType.Information); 
                        continue;
                    }
                    AuthNetRequest newAuthNetRequest = winningBidderTransaction.AuthNetRequest2;
                    AuthNetResponse newAuthNetResponse = new AuthNetResponse();
                    //authCode = authNetRequest.AuthNetResponse.FirstOrDefault().x_auth_code;
                    transId = authNetRequest.AuthNetResponse.FirstOrDefault().x_trans_id;
                    holdAmount = (winningBidderTransaction.HoldAmount.IsNull() ? bidRepository.GetPropertyHoldAmount(winningBidder.Property.PropID) : Convert.ToDecimal(winningBidderTransaction.HoldAmount));

                    AuthNet.Business.Transaction authNetTransactions = reportingGateway.GetTransactionDetails(transId);
                    
                    isChargeReversed = paymentManager.RevertCharge(holdAmount, transId, authNetTransactions.CardNumber, ref newAuthNetRequest, ref newAuthNetResponse);

                    winningBidderTransaction.IsChargeReverted = isChargeReversed;
                    winningBidderTransaction.AuthNetRequest2 = paymentRepository.GetAuthNetRequestById(newAuthNetRequest.AuthNetRequestId);
                    winningBidOperation = winningBidder.WinningBidOperation.FirstOrDefault();
                    winningBidOperation.IsReversalSubmittedToAuthNet = true;
                    bidRepository.Save();
                    //bidRepository.SendCardChargedEmailToBidder(winningBidder.WinningBidderId);
                }
                catch (Exception ex)
                {
                    AuctionsEventLog.WriteLogEntry(BaseAuthNetManager.ClassModule + BaseAuthNetManager.ShowTrace() + Environment.NewLine + ex.Message,
                                    System.Diagnostics.EventLogEntryType.Error); 
                    continue;
                }
            }
            return true;
        }

        public bool SyncAuthNetTrans()
        {            
            PaymentRepository paymentRepository = new PaymentRepository();
            ReportingGateway reportingGateway = new ReportingGateway(base.ApiLogin, base.ApiTransKey, serviceMode);
            BidRepository bidRepository = new BidRepository();
            try
            {
                WinningBidOperation winningBidOperation;
                WinningBidderTransaction winningBidderTransaction; 
                List<vw_AuthNetTransction> allTransToSettle = paymentRepository.GetAllTransactionForSettlement();
                foreach (vw_AuthNetTransction tranToSettle in allTransToSettle)
                {
                    try
                    {
                        winningBidOperation = bidRepository.GetBidOperationsByWinningBidderId(tranToSettle.WinningBidderId, null);
                        winningBidderTransaction = bidRepository.GetWinningBidderTransactionByWinningBidderId(tranToSettle.WinningBidderId);

                        if (winningBidOperation.IsChargeSettled == false && !String.IsNullOrEmpty(tranToSettle.Capture_x_trans_id) && tranToSettle.Capture_x_trans_id != "0") // Hold To Capture
                        {                            
                            try
                            {
                                AuthNet.Business.Transaction authNetTransactions = reportingGateway.GetTransactionDetails(tranToSettle.Capture_x_trans_id);
                            
                                if (!String.IsNullOrEmpty(authNetTransactions.BatchSettlementID) && authNetTransactions.Status == "settledSuccessfully") //batch is settled
                                {
                                    //Set the flags accordingly                                    
                                    winningBidOperation.IsChargeSettled = true;
                                    winningBidderTransaction.IsHoldCaptured = true;
                                    bidRepository.SendCardChargedEmailToBidder(winningBidderTransaction.WinningBidder.WinningBidderId);
                                }
                            }
                            catch (Exception ex)
                            {
                                //Expecting Records not Found from AuthNet.
                                AuctionsEventLog.WriteLogEntry(BaseAuthNetManager.ClassModule + BaseAuthNetManager.ShowTrace() + Environment.NewLine + ex.Message,
                                    System.Diagnostics.EventLogEntryType.Error); 
                            }
                        }

                        if (winningBidOperation.IsReversalSettled == false && !String.IsNullOrEmpty(tranToSettle.Reverse_x_trans_id) && tranToSettle.Reverse_x_trans_id != "0") //Capture to Reverse/refund
                        {
                            try
                            {
                                AuthNet.Business.Transaction authNetTransactions = reportingGateway.GetTransactionDetails(tranToSettle.Reverse_x_trans_id);

                                if (!String.IsNullOrEmpty(authNetTransactions.BatchSettlementID) && authNetTransactions.Status == "refundSettledSuccessfully") //batch is settled
                                {
                                    //Set the flags accordingly                                    
                                    winningBidOperation.IsReversalSettled = true;
                                    winningBidderTransaction.IsChargeReverted = true;
                                    bidRepository.SendCardCreditedEmailToBidder(winningBidderTransaction.WinningBidder.WinningBidderId);
                                }
                            }
                            catch (Exception ex)
                            {
                                //Expecting Records not Found from AuthNet.
                                AuctionsEventLog.WriteLogEntry(BaseAuthNetManager.ClassModule + BaseAuthNetManager.ShowTrace() + Environment.NewLine + ex.Message,
                                    System.Diagnostics.EventLogEntryType.Error); 
                            }
                        }

                        if (winningBidderTransaction.IsTranVoid == false && !String.IsNullOrEmpty(tranToSettle.Void_x_trans_id) && tranToSettle.Void_x_trans_id != "0") //Hold Void
                        {
                            try
                            {
                                AuthNet.Business.Transaction authNetTransactions = reportingGateway.GetTransactionDetails(tranToSettle.Void_x_trans_id);
                                if (!String.IsNullOrEmpty(authNetTransactions.BatchSettlementID) && authNetTransactions.Status == "voided") //batch is settled
                                {
                                    //Set the flags accordingly                                    
                                    winningBidderTransaction.IsTranVoid = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                //Expecting Records not Found from AuthNet.
                                AuctionsEventLog.WriteLogEntry(BaseAuthNetManager.ClassModule + BaseAuthNetManager.ShowTrace() + Environment.NewLine + ex.Message,
                                    System.Diagnostics.EventLogEntryType.Error); 
                            }
                        }

                        bidRepository.Save(); //Finally save the changes per iteration
                    }
                    catch (Exception ex)
                    {
                        AuctionsEventLog.WriteLogEntry(BaseAuthNetManager.ClassModule + BaseAuthNetManager.ShowTrace() + Environment.NewLine + ex.Message,
                              System.Diagnostics.EventLogEntryType.Error); 

                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                AuctionsEventLog.WriteLogEntry(BaseAuthNetManager.ClassModule + BaseAuthNetManager.ShowTrace() + Environment.NewLine + ex.Message, 
                    System.Diagnostics.EventLogEntryType.Error); 

                return false;
            }
            return true;
        }
    
         */
    }
}
