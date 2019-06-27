using System;
using Domain.Commands;

namespace EventSourcing.Sample.Clients.Contracts.Clients.Commands
{
    using ValueObjects;

    public class UpdateClient: ICommand
    {
        public Guid Id { get; }
        public ClientInfo Data { get; }

        public UpdateClient(Guid id, ClientInfo data)
        {
            Id = id;
            Data = data;
        }
    }
}
