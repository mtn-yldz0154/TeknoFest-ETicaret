namespace ElektronikWebUI.Models
{
    public class OrderModel
    {
       
        public int Shipping { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string ExpairationMonth { get; set; }
        public string ExpairationYear { get; set; }
        public string Cvc { get; set; }     
        

        public CartModel CartModel { get; set; }
    }
}
