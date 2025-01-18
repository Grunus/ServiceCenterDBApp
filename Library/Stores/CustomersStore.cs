using Library.Models;
using System.Collections.ObjectModel;

namespace Library.Stores
{
    public class CustomersStore : DataStore<Customer>
    {
        public CustomersStore()
            : base()
        {
        }

        public ObservableCollection<Customer> Customers => Data;

        public async Task ResetCustomers()
        {
            await ResetData();
        }
    }
}
