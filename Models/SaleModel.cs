namespace P_CStore.Models
{
    public class SaleModel
    {
        public int IdProduct { get; set; }
        public int IdTransaction { get; set; }
        public int Quantity { get; set; }
        public decimal Payment { get; set; }
    }
}
