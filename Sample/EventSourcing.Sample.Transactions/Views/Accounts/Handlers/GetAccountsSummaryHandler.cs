using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Queries;
using EventSourcing.Sample.Transactions.Contracts.Accounts.Queries;
using Marten;

namespace EventSourcing.Sample.Transactions.Views.Accounts.Handlers
{
    using AllAccountsSummary;
    using AllAccountsSummary = Contracts.Accounts.ValueObjects.AllAccountsSummary;

    public class GetAccountsSummaryHandler: IQueryHandler<GetAccountsSummary, AllAccountsSummary>
    {
        private readonly IDocumentSession session;

        public GetAccountsSummaryHandler(IDocumentSession session)
        {
            this.session = session;
        }

        public Task<AllAccountsSummary> Handle(GetAccountsSummary message, CancellationToken cancellationToken = default)
        {
            return session.Query<AllAccountsSummaryView>()
                  .Select(
                      a => new AllAccountsSummary
                      {
                          TotalBalance = a.TotalBalance,
                          TotalCount = a.TotalCount,
                          TotalTransactionsCount = a.TotalTransactionsCount
                      }
                  )
                  .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
