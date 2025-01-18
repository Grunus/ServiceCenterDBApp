using Library.Models;
using System.Collections.ObjectModel;

namespace Library.Stores
{
    public class EmployeesStore : DataStore<Employee>
    {
        public EmployeesStore()
            : base()
        {
        }

        public ObservableCollection<Employee> Employees => Data;

        public async Task ResetEmployees()
        {
            await ResetData();
        }
    }
}
