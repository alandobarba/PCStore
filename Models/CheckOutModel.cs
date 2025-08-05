namespace P_CStore.Models
{
    public class CheckOutModel
    {
        public CheckOutModel()
        {
            lstCart = new List<CartViewModel>();
        }
        public StripeModel stripeModel { get; set; }
        public CustomerDTO customer { get; set; }
        public List<CartViewModel> lstCart { get; set; }
    }
}
