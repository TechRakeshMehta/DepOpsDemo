using System;
namespace INTSOF.AuthNet.Business
{
    public interface IReportingGateway {
        System.Collections.Generic.List<Batch> GetSettledBatchList(DateTime from, DateTime to);
        System.Collections.Generic.List<Batch> GetSettledBatchList();
        Transaction GetTransactionDetails(string transactionID);
        System.Collections.Generic.List<Transaction> GetTransactionList(DateTime from, DateTime to);
        System.Collections.Generic.List<Transaction> GetTransactionList();
        System.Collections.Generic.List<Transaction> GetTransactionList(string batchId);
    }
}
