using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class EventBus: IEventBus
    {
        private readonly IMediator mediator;

        public EventBus(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task Publish<TEvent>(params TEvent[] events) where TEvent : IEvent
        {
            foreach (var @event in events)
            {
                await mediator.Publish(@event);
            }
        }
    }
}
