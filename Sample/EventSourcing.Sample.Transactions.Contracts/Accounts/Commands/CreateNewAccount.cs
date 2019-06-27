namespace EventSourcing.Sample.Transactions.Contracts.Accounts.Commands
{
    using System;
    using Domain.Commands;

    public class CreateNewAccount: ICommand
    {
        public Guid ClientId { get; set; }
    }
}
