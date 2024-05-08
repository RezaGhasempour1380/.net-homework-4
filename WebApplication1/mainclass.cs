namespace mainclass {
    public class Customer
    {
        public int id { get; set; }
        public string ?Name { get; set; }
        public string ?Surname { get; set; }
        public string ?Email { get; set; }
        
        // Navigation property for related Orders
        //public List<Orders> ?Orders { get; set; }
    }

    public class Product
    {
        public int id { get; set; }
        public string ?Name { get; set; }
        public decimal Price { get; set; }
        
        // Navigation property for related OrderDetails
        //public List<OrderDetails> ?OrderDetails { get; set; }
    }

    public class Orders
    {
        public int id { get; set; }
        public string ?Number { get; set; }
        public string ?State { get; set; }
        public DateTime OrderDate { get; set; }
        public int Customerid { get; set; }
        
        // Navigation properties
        //public Customer ?Customer { get; set; }
        //public List<OrderDetails> ?OrderDetails { get; set; }
    }

    public class OrderDetails
    {
        public int id { get; set; }
        public int Productid { get; set; }
        public int Amount { get; set; }
        public int Orderid { get; set; }
        
        // Navigation properties
        //public Product ?Product { get; set; }
        //public Orders ?Order { get; set; }
    }

    public class OrderDto
    {
        public int OrderId { get; set; }
        public string ?OrderNumber { get; set; }
        public string ?OrderState { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> ?OrderItems { get; set; }
    }

    public class OrderItemDto
    {
        public int Amount { get; set; }
        public string ?ProductName { get; set; }
        public decimal ProductPrice { get; set; }
    }

        public class loginInfo
    {
        public string ?Username { get; set; }
        public string ?Password { get; set; }
    }
    public class UpdateProductModel
    {
        public int Id { get; set; }
        public string? UpdatedProductName { get; set; }
        public decimal UpdatedProductPrice { get; set; }
    }

        public class NewProductModel
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }
}
