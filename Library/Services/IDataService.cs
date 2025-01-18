namespace Library.Services
{
    public interface IDataService<T>
    {
        Task<T> Add(T entity);

        Task<IEnumerable<T>> GetAll();

        IEnumerable<T> GetAllNotAsync();

        Task<T> Update(T entity);

        Task<T> Delete(int id);
    }
}
