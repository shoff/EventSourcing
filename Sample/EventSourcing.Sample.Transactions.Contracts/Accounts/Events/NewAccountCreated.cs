namespace EventSourcing.Sample.Transactions.Contracts.Accounts.Events
{
    using System;
    using Domain.Events;

    public class NewAccountCreated: IEvent
    {
        public Guid AccountId { get; set; }

        public Guid ClientId { get; set; }

        public string Number { get; set; }
    }
}
