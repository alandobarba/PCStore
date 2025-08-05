namespace P_CStore.Models
{
    public class CustomerDTO
    {
        public string StripeCustomerId { get; set; }
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PaymentMethod { get; set; }
        public string Last4Digits { get; set; }
        public string Brand { get; set; }
    }
}
