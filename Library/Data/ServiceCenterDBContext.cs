using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public partial class ServiceCenterDBContext : DbContext
{
    public ServiceCenterDBContext(DbContextOptions<ServiceCenterDBContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum("order_status", ["in progress", "waiting for payment", "completed"]);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.HasIndex(e => e.Email, "customers_email_key").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "customers_phone_number_key").IsUnique();

            entity.Property(e => e.CustomerId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("customer_id");
            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.FullName)
                .HasComputedColumnSql("(((((last_name)::text || ' '::text) || (first_name)::text) || ' '::text) || (middle_name)::text)", true)
                .HasColumnType("character varying")
                .HasColumnName("full_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("middle_name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(13)
                .HasColumnName("phone_number");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("employees_pkey");

            entity.ToTable("employees");

            entity.HasIndex(e => e.Email, "employees_email_key").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "employees_phone_number_key").IsUnique();

            entity.Property(e => e.EmployeeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("employee_id");
            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.FullName)
                .HasComputedColumnSql("(((((last_name)::text || ' '::text) || (first_name)::text) || ' '::text) || (middle_name)::text)", true)
                .HasColumnType("character varying")
                .HasColumnName("full_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("middle_name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(13)
                .HasColumnName("phone_number");
            entity.Property(e => e.Specialization).HasColumnName("specialization");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("order_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DueBy)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("due_by");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.PlacedAt)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("placed_at");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasColumnType("order_status");
            entity.Property(e => e.ServiceTypeId).HasColumnName("service_type_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("fk_customer_id");

            entity.HasOne(d => d.Employee).WithMany(p => p.Orders)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("fk_employee_id");

            entity.HasOne(d => d.ServiceType).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ServiceTypeId)
                .HasConstraintName("fk_service_type_id");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("payments_pkey");

            entity.ToTable("payments");

            entity.Property(e => e.PaymentId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("payment_id");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.OrderId).HasColumnName("order_id");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_order_id");
        });

        modelBuilder.Entity<ServiceType>(entity =>
        {
            entity.HasKey(e => e.ServiceTypeId).HasName("service_types_pkey");

            entity.ToTable("service_types");

            entity.HasIndex(e => e.Name, "service_types_name_key").IsUnique();

            entity.Property(e => e.ServiceTypeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("service_type_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EstimatedTime).HasColumnName("estimated_time");
            entity.Property(e => e.MinPrice)
                .HasPrecision(10, 2)
                .HasColumnName("min_price");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
