namespace Domain.Events
{
    using System.Collections.Generic;
    using Aggregates;

    public interface IEventSourcedAggregate: IAggregate
    {
        Queue<IEvent> PendingEvents { get; }
    }
}
