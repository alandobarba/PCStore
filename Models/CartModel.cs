namespace P_CStore.Models
{
    public class CartModel
    {
        public int IdCart { get; set; }

        public int IdUser { get; set; }

        public int IdProduct { get; set; }

        public int Quantity { get; set; }

        public bool IsCheckedOut { get; set; }
    }
}
