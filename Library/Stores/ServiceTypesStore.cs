using Library.Models;
using System.Collections.ObjectModel;

namespace Library.Stores
{
    public class ServiceTypesStore : DataStore<ServiceType>
    {
        public ServiceTypesStore()
            : base()
        {
        }

        public ObservableCollection<ServiceType> ServiceTypes => Data;

        public async Task ResetServiceTypes()
        {
            await ResetData();
        }
    }
}
