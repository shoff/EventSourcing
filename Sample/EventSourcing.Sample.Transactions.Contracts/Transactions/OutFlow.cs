namespace EventSourcing.Sample.Transactions.Contracts.Transactions
{
    using System;

    public class Outflow: ITransaction
    {
        public decimal Ammount { get; }

        public DateTime Timestamp { get; }

        public Outflow(decimal ammount, DateTime timestamp)
        {
            Ammount = ammount;
            Timestamp = timestamp;
        }
    }
}
