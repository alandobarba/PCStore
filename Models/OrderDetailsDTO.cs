namespace P_CStore.Models
{
    public class OrderDetailsDTO
    {
        public int IdProduct { get; set; }
        public byte[] Picture { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}
