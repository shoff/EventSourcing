namespace EventSourcing.Sample.Transactions.Contracts.Transactions.Events
{
    using System;
    using Domain.Events;

    public class NewInflowRecorded: IEvent
    {
        public Guid FromAccountId { get; }
        public Guid ToAccountId { get; }
        public Inflow Inflow { get; }

        public NewInflowRecorded(Guid fromAccountId, Guid toAccountId, Inflow inflow)
        {
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Inflow = inflow;
        }
    }
}
