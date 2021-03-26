namespace PaymentGateway.Configuration
{
    public interface IPaymentsDatabaseConfig
    {
        public string PaymentsCollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}
