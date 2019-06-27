using System;
using Domain.Events;

namespace EventSourcing.Sample.Clients.Contracts.Clients.Events
{
    using ValueObjects;

    public class ClientCreated: IEvent
    {
        public Guid ClientId { get; }
        public ClientInfo Data { get; }

        public ClientCreated(Guid clientId, ClientInfo data)
        {
            ClientId = clientId;
            Data = data;
        }
    }
}
