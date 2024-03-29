namespace EventSourcing.Sample.Transactions.Contracts.Accounts.ValueObjects
{
    using System;

    public class AccountSummary
    {
        public Guid AccountId { get; set; }
        public Guid ClientId { get; set; }
        public string Number { get; set; }
        public decimal Balance { get; set; }
        public int TransactionsCount { get; set; }
    }
}
