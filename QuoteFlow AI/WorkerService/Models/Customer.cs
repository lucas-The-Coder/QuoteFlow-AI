namespace WorkerService.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}