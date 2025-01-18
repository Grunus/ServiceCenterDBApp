using Library.Services;
using System.Collections.ObjectModel;

namespace Library.Stores
{
    public abstract class DataStore<T> where T : class
    {
        protected ObservableCollection<T> Data { get; init; }

        protected DataStore() 
        {
            var dataService = new GenericDataService<T>();
            Data = new ObservableCollection<T>(dataService.GetAllNotAsync());
        }

        protected async Task ResetData()
        {
            Data.Clear();
            var newData = await new GenericDataService<T>().GetAll();
            foreach (var entry in newData)
            {
                Data.Add(entry);
            }
        }
    }
}
