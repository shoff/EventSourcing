namespace EventSourcing.Sample.Clients.Contracts.Clients.ValueObjects
{
    using System;
    using System.Collections.Generic;

    public class ClientView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> AccountsNumbers { get; set; }
    }
}
