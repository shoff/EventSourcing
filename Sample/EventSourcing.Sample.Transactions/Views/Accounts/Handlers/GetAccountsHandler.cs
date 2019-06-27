namespace EventSourcing.Sample.Transactions.Views.Accounts.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AccountSummary;
    using Contracts.Accounts.Queries;
    using Contracts.Accounts.ValueObjects;
    using global::Domain.Queries;
    using Marten;

    public class GetAccountsHandler: IQueryHandler<GetAccounts, IEnumerable<AccountSummary>>
    {
        private readonly IDocumentSession _session;

        public GetAccountsHandler(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<AccountSummary>> Handle(GetAccounts message, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _session.Query<AccountSummaryView>()
                .Select(
                    a => new AccountSummary
                    {
                        AccountId = a.AccountId,
                        Balance = a.Balance,
                        ClientId = a.ClientId,
                        Number = a.Number,
                        TransactionsCount = a.TransactionsCount
                    }
                ).Where(p => p.ClientId == message.ClientId)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
