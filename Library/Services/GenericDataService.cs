using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Library.Services
{
    public class GenericDataService<T> : IDataService<T> where T : class
    {
        public async Task<T> Add(T entity)
        {
            using ServiceCenterDBContext context = ServiceCenterDBContextFactory.CreateDbContext();

            EntityEntry<T> addedResult = await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();

            return addedResult.Entity;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            using ServiceCenterDBContext context = ServiceCenterDBContextFactory.CreateDbContext();

            IEnumerable<T> entities = await context.Set<T>().ToListAsync();
            return entities;
        }

        public IEnumerable<T> GetAllNotAsync()
        {
            using ServiceCenterDBContext context = ServiceCenterDBContextFactory.CreateDbContext();

            IEnumerable<T> entities = context.Set<T>().ToList<T>();
            return entities;
        }

        public async Task<T> Update(T entity)
        {
            using ServiceCenterDBContext context = ServiceCenterDBContextFactory.CreateDbContext();

            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();

            return entity;
        }

        public async Task<T> Delete(int id)
        {
            using ServiceCenterDBContext context = ServiceCenterDBContextFactory.CreateDbContext();

            var entities = await context.Set<T>().ToListAsync<T>();
            T entity = entities.FirstOrDefault(e => GetEntityId(e) == id);
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();

            return entity;
        }

        private static int GetEntityId(T e)
        {
            if (e is Customer customer)
                return customer.CustomerId;
            else if (e is Employee employee)
                return employee.EmployeeId;
            else if (e is Order order)
                return order.OrderId;
            else if (e is Payment payment)
                return payment.PaymentId;
            else if (e is ServiceType serviceType)
                return serviceType.ServiceTypeId;
            else
                return 0;
        }
    }
}
