namespace P_CStore.Models
{
    public class OrderListDTO
    {
        public OrderListDTO()
        {
            LstPicture = new List<byte[]>();
        }
        public int IdTransaction { get; set; }
        public List<byte[]> LstPicture { get; set; }
        public DateOnly StartDate { get; set; }
    }
}
