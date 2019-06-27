namespace EventSourcing.Sample.Transactions.Views.Accounts.Handlers
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AccountSummary;
    using Contracts.Accounts.Queries;
    using Contracts.Accounts.ValueObjects;
    using global::Domain.Queries;
    using Marten;

    public class GetAccountHandler: IQueryHandler<GetAccount, AccountSummary>
    {
        private readonly IDocumentSession _session;

        public GetAccountHandler(IDocumentSession session)
        {
            _session = session;
        }

        public Task<AccountSummary> Handle(GetAccount message, CancellationToken cancellationToken = default(CancellationToken))
        {
            return _session
                .Query<AccountSummaryView>()
                .Select(a => new AccountSummary
                {
                    AccountId = a.AccountId,
                    Balance = a.Balance,
                    ClientId = a.ClientId,
                    Number = a.Number,
                    TransactionsCount = a.TransactionsCount
                })
                .FirstOrDefaultAsync(p => p.AccountId == message.AccountId, cancellationToken);
        }
    }
}
