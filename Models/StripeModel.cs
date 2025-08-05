using Stripe;
using System;

namespace P_CStore.Models
{
    public class StripeModel
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string StripeToken { get; set; }
        public string PaymentMethod { get; set; }
        public string Last4Digits { get; set; }
        public string Brand { get; set; }

    }
}
