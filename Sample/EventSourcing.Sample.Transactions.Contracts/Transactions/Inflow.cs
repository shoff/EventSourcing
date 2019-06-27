namespace EventSourcing.Sample.Transactions.Contracts.Transactions
{
    using System;

    public class Inflow: ITransaction
    {
        public decimal Ammount { get; }

        public DateTime Timestamp { get; }

        public Inflow(decimal ammount, DateTime timestamp)
        {
            Ammount = ammount;
            Timestamp = timestamp;
        }
    }
}
