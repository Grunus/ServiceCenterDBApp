using Library.Enums;
using Microsoft.EntityFrameworkCore;
using Library.Converters;

namespace Library.Data
{
    public static class ServiceCenterDBContextFactory
    {
        static ServiceCenterDBContextFactory()
        {
            _optionsBuilder = new DbContextOptionsBuilder<ServiceCenterDBContext>();

            _optionsBuilder.UseNpgsql("Host=localhost:Your_Database_Port;Database=Your_Database_Name;Username=Your_Username;Password=Your_Password;IncludeErrorDetail=True",
                o => o.MapEnum<OrderStatus>(enumName: "order_status", nameTranslator: new NpgsqlOrderStatusEnumTranslator()));
        }

        private static DbContextOptionsBuilder<ServiceCenterDBContext> _optionsBuilder;

        public static ServiceCenterDBContext CreateDbContext()
        {
            return new ServiceCenterDBContext(_optionsBuilder.Options);
        }
    }
}
