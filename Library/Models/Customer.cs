namespace Library.Models
{
    public partial class Customer
    {
        public int CustomerId { get; set; }

        public string LastName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string MiddleName { get; set; } = null!;

        public string? FullName { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public string? Email { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
