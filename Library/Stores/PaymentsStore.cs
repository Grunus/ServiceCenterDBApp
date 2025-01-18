using Library.Models;
using System.Collections.ObjectModel;

namespace Library.Stores
{
    public class PaymentsStore : DataStore<Payment>
    {
        public PaymentsStore()
            : base()
        {
        }

        public ObservableCollection<Payment> Payments => Data;

        public async Task ResetPayments()
        {
            await ResetData();
        }
    }
}