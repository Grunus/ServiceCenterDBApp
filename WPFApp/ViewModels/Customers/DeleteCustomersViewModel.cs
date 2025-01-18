using CommunityToolkit.Mvvm.Input;
using Library.Models;
using Library.Services;
using Library.Stores;
using System.Windows.Input;
using WPFApp.Commands;
using WPFApp.Helpers;
using WPFApp.Services;

namespace WPFApp.ViewModels.Customers
{
    public class DeleteCustomersViewModel : ViewModelBase
    {
        private readonly CustomersStore _customersStore;

        public IEnumerable<Customer> Customers => _customersStore.Customers;

        public ICommand DeleteCustomerCommand { get; init; }

        public ICommand ReturnCommand { get; init; }

        public DeleteCustomersViewModel(CustomersStore customersStore, INavigationService closeNavigationService)
        {
            _customersStore = customersStore;
            ReturnCommand = NavigateCommand.Create(closeNavigationService);

            Func<Task> _deleteCustomer = async () =>
            {
                var dataService = new GenericDataService<Customer>();

                try
                {
                    await dataService.Delete(SelectedCustomer.CustomerId);
                    await _customersStore.ResetCustomers();
                    SelectedCustomer = null;
                }
                catch (Exception ex)
                {
                    MessageDisplayHelper.DisplayError(ex);
                }
            };

            Func<bool> _canDeleteCustomer = () =>
            {
                return SelectedCustomer != null;
            };

            DeleteCustomerCommand = new AsyncRelayCommand(_deleteCustomer, _canDeleteCustomer);
        }

        private Customer _selectedCustomer = null;

        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                SetProperty(ref _selectedCustomer, value);
                Refresh();
            }
        }

        private void Refresh()
        {
            ((AsyncRelayCommand)DeleteCustomerCommand).NotifyCanExecuteChanged();
        }
    }
}
