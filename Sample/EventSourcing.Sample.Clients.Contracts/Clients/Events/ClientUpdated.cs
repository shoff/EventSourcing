using System;
using Domain.Events;

namespace EventSourcing.Sample.Clients.Contracts.Clients.Events
{
    using ValueObjects;

    public class ClientUpdated: IEvent
    {
        public Guid ClientId { get; }
        public ClientInfo Data { get; }

        public ClientUpdated(Guid clientId, ClientInfo data)
        {
            ClientId = clientId;
            Data = data;
        }
    }
}
