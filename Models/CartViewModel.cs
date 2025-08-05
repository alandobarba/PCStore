namespace P_CStore.Models
{
    public class CartViewModel
    {
        public int IdCart { get; set; }
        public int IdUser { get; set; }

        public int IdProduct { get; set; }

        public string Title { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public byte[] Picture { get; set; }
    }
}
