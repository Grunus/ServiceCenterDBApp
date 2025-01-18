using Library.Enums;

namespace Library.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public int ServiceTypeId { get; set; }

        public int EmployeeId { get; set; }

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public OrderStatus Status { get; set; }

        public DateOnly PlacedAt { get; set; }

        public DateOnly DueBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual Customer Customer { get; set; } = null!;

        public virtual Employee Employee { get; set; } = null!;

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        public virtual ServiceType ServiceType { get; set; } = null!;
    }
}
