using System;
using System.Collections.Generic;
using EventSourcing.Sample.Clients.Contracts.Clients.Events;
using Marten.Events.Projections;

namespace EventSourcing.Sample.Transactions.Views.Clients
{
    using Contracts.Accounts.Events;
    using Sample.Clients.Contracts.Clients.ValueObjects;

    public class ClientsViewProjection: ViewProjection<ClientView, Guid>
    {
        public ClientsViewProjection()
        {
            ProjectEvent<ClientCreated>(ev => ev.ClientId, (view, @event) => ApplyEvent(view, @event));
            ProjectEvent<ClientUpdated>(ev => ev.ClientId, (view, @event) => ApplyEvent(view, @event));
            ProjectEvent<NewAccountCreated>(ev => ev.ClientId, (view, @event) => ApplyEvent(view, @event));
            DeleteEvent<ClientDeleted>(ev => ev.ClientId);
        }

        internal void ApplyEvent(ClientView view, ClientCreated @event)
        {
            view.Id = @event.ClientId;
            view.Name = @event.Data.Name;
            view.Email = @event.Data.Email;
            view.AccountsNumbers = new List<string>();
        }

        internal void ApplyEvent(ClientView view, ClientUpdated @event)
        {
            view.Name = @event.Data.Name;
            view.Email = @event.Data.Email;
        }

        internal void ApplyEvent(ClientView view, NewAccountCreated @event)
        {
            view.AccountsNumbers.Add(@event.Number);
        }
    }
}
