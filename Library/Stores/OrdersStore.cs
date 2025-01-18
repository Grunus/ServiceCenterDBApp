using Library.Models;
using System.Collections.ObjectModel;

namespace Library.Stores
{
    public class OrdersStore : DataStore<Order>
    {
        public OrdersStore()
            : base()
        {
        }

        public ObservableCollection<Order> Orders => Data;

        public async Task ResetOrders()
        {
            await ResetData();
        }
    }
}
