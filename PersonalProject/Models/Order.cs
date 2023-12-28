namespace PersonalProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public DateTime TimeStamp { get; set; }


        public List<ProductsOrder>? productsOrders { get; set; }

    }
}
