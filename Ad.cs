namespace AdApi
{
    public class Ad
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        // Relationships
        public int SellerId { get; set; }
        public Seller Seller { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
