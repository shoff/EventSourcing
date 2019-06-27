namespace EventSourcing.Sample.Transactions.Contracts.Accounts.Queries
{
    using System;
    using System.Collections.Generic;
    using Domain.Queries;
    using ValueObjects;

    public class GetAccounts: IQuery<IEnumerable<AccountSummary>>
    {
        public Guid ClientId { get; private set; }

        public GetAccounts(Guid clientId)
        {
            ClientId = clientId;
        }
    }
}
