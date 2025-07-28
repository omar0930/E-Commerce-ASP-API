namespace Store.Repository.Cart
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string PictureUrl { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
    }
}