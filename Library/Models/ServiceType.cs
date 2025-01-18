namespace Library.Models
{
    public partial class ServiceType
    {
        public int ServiceTypeId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal MinPrice { get; set; }

        public TimeSpan? EstimatedTime { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
