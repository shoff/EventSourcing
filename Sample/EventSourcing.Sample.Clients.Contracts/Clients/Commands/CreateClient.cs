using System;
using Domain.Commands;

namespace EventSourcing.Sample.Clients.Contracts.Clients.Commands
{
    using ValueObjects;

    public class CreateClient: ICommand
    {
        public Guid? Id { get; set; }
        public ClientInfo Data { get; }

        public CreateClient(Guid? id, ClientInfo data)
        {
            Id = id;
            Data = data;
        }
    }
}
