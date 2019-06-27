namespace EventSourcing.Sample.Transactions.Views.Accounts.AllAccountsSummary
{
    using System;
    using System.Linq;
    using Contracts.Accounts.Events;
    using Contracts.Transactions.Events;
    using Marten;
    using Marten.Events.Projections;

    public class AllAccountsSummaryViewProjection: ViewProjection<AllAccountsSummaryView, Guid>
    {
        public AllAccountsSummaryViewProjection()
        {
            ProjectEventToSingleRecord<NewAccountCreated>((view, @event) => view.ApplyEvent(@event));
            ProjectEventToSingleRecord<NewInflowRecorded>((view, @event) => view.ApplyEvent(@event));
            ProjectEventToSingleRecord<NewOutflowRecorded>((view, @event) => view.ApplyEvent(@event));
        }

        private ViewProjection<AllAccountsSummaryView, Guid> ProjectEventToSingleRecord<TEvent>(Action<AllAccountsSummaryView, TEvent> handler) where TEvent : class
        {
            return ProjectEvent((documentSession, ev) => FindIdOfRecord(documentSession) ?? Guid.NewGuid(), handler);
        }

        private Guid? FindIdOfRecord(IDocumentSession documentSession)
        {
            return documentSession.Query<AllAccountsSummaryView>()
                               .Select(t => (Guid?)t.Id).SingleOrDefault();
        }
    }
}
