namespace P_CStore.Models
{
	public class IndexModel
	{
		public int IdCategory { get; set; }
		public int IdProduct { get; set; }
		public string ProductName { get; set; }
		public string ProductDescription { get; set; }
		public decimal SellingPrice { get; set; }
        public byte[] Picture { get; set; }
    }
}
