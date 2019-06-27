namespace EventSourcing.Sample.Transactions.Contracts.Transactions.Events
{
    using System;
    using Domain.Events;

    public class NewOutflowRecorded: IEvent
    {
        public Guid FromAccountId { get; }
        public Guid ToAccountId { get; }
        public Outflow Outflow { get; }

        public NewOutflowRecorded(Guid fromAccountId, Guid toAccountId, Outflow outflow)
        {
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Outflow = outflow;
        }
    }
}
