namespace EventSourcing.Sample.Transactions.Domain.Accounts
{
    using System;
    using Contracts.Accounts.Events;
    using Contracts.Transactions;
    using Contracts.Transactions.Events;
    using global::Domain.Events;

    public class Account: EventSourcedAggregate
    {
        public Guid ClientId { get; private set; }

        public decimal Balance { get; private set; }

        public string Number { get; private set; }

        public Account()
        {
        }

        public Account(Guid clientId, IAccountNumberGenerator accountNumberGenerator)
        {
            var @event = new NewAccountCreated
            {
                AccountId = Guid.NewGuid(),
                ClientId = clientId,
                Number = accountNumberGenerator.Generate()
            };

            Apply(@event);
            Append(@event);
        }

        public void RecordInflow(Guid fromId, decimal ammount)
        {
            var @event = new NewInflowRecorded(fromId, Id, new Inflow(ammount, DateTime.Now));
            Apply(@event);
            Append(@event);
        }

        public void RecordOutflow(Guid toId, decimal ammount)
        {
            var @event = new NewOutflowRecorded(Id, toId, new Outflow(ammount, DateTime.Now));
            Apply(@event);
            Append(@event);
        }

        public void Apply(NewAccountCreated @event)
        {
            Id = @event.AccountId;
            ClientId = @event.ClientId;
            Number = @event.Number;
        }

        public void Apply(NewInflowRecorded @event)
        {
            Balance += @event.Inflow.Ammount;
        }

        public void Apply(NewOutflowRecorded @event)
        {
            Balance -= @event.Outflow.Ammount;
        }
    }
}
