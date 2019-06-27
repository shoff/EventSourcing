using System;
using Domain.Queries;

namespace EventSourcing.Sample.Clients.Contracts.Clients.Queries
{
    using ValueObjects;

    public class GetClientView: IQuery<ClientView>
    {
        public Guid Id { get; }

        public GetClientView(Guid id)
        {
            Id = id;
        }
    }
}
