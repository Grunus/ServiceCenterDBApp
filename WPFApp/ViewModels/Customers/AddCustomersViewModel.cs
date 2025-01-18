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
    public class AddCustomersViewModel : ViewModelBase
    {
        private readonly CustomersStore _customersStore;

        public ICommand AddCustomerCommand { get; init; }

        public ICommand ResetCommand { get; init; }

        public ICommand ReturnCommand { get; init; }

        public AddCustomersViewModel(CustomersStore customersStore, INavigationService closeNavigationService)
        {
            _customersStore = customersStore;
            ReturnCommand = NavigateCommand.Create(closeNavigationService);

            Func<Task> _addCustomer = async () =>
            {
                var customer = new Customer()
                {
                    LastName = _lastName,
                    FirstName = _firstName,
                    MiddleName = _middleName,
                    PhoneNumber = _phoneNumber,
                    Email = _email,
                };

                var dataService = new GenericDataService<Customer>();

                try
                {
                    await dataService.Add(customer);
                    await _customersStore.ResetCustomers();
                }
                catch (Exception ex)
                {
                    MessageDisplayHelper.DisplayError(ex);
                }
            };

            Func<bool> _canAddCustomer = () =>
            {
                return !string.IsNullOrWhiteSpace(LastName)
                    && !string.IsNullOrWhiteSpace(FirstName)
                    && !string.IsNullOrWhiteSpace(MiddleName)
                    && !string.IsNullOrWhiteSpace(PhoneNumber);
            };

            AddCustomerCommand = new AsyncRelayCommand(_addCustomer, _canAddCustomer);

            ResetCommand = new RelayCommand(() =>
            {
                LastName = String.Empty;
                FirstName = String.Empty;
                MiddleName = String.Empty;
                PhoneNumber = String.Empty;
                Email = String.Empty;
            });

            ResetCommand.Execute(null);
        }

        private void Refresh()
        {
            ((AsyncRelayCommand)AddCustomerCommand).NotifyCanExecuteChanged();
        }

        private string _lastName;

        public string LastName 
        {
            get => _lastName;
            set
            {
                SetProperty(ref _lastName, value);
                Refresh();
            }
        }

        private string _firstName;

        public string FirstName 
        {
            get => _firstName;
            set
            {
                SetProperty(ref _firstName, value);
                Refresh();
            }
        }

        private string _middleName;

        public string MiddleName 
        {
            get => _middleName;
            set
            {
                SetProperty(ref _middleName, value);
                Refresh();
            }
        }

        private string _phoneNumber;

        public string PhoneNumber 
        {
            get => _phoneNumber;
            set
            {
                SetProperty(ref _phoneNumber, value);
                Refresh();
            }
        }

        private string? _email = null;

        public string? Email 
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    SetProperty(ref _email, null);
                else
                    SetProperty(ref _email, value);
                Refresh();
            }
        }
    }
}
