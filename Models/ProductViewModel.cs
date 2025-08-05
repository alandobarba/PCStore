namespace P_CStore.Models
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            LstPictures = new List<PicturesModel>();
        }
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string Title { get; set; }
        public string Information { get; set; }
        public decimal SellingPrice { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public List<PicturesModel> LstPictures { get; set; }
    }
}
