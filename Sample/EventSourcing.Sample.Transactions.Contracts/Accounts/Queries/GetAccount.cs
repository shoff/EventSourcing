namespace EventSourcing.Sample.Transactions.Contracts.Accounts.Queries
{
    using System;
    using Domain.Queries;
    using ValueObjects;

    public class GetAccount: IQuery<AccountSummary>
    {
        public Guid AccountId { get; private set; }

        public GetAccount(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}
