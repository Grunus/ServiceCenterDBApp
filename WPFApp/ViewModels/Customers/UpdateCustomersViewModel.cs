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
    public class UpdateCustomersViewModel : ViewModelBase
    {
        private readonly CustomersStore _customersStore;

        public IEnumerable<Customer> Customers => _customersStore.Customers;

        public ICommand UpdateCustomerCommand { get; init; }

        public ICommand ResetCommand { get; init; }

        public ICommand ReturnCommand { get; init; }

        public UpdateCustomersViewModel(CustomersStore customersStore, INavigationService closeNavigationService)
        {
            _customersStore = customersStore;
            ReturnCommand = NavigateCommand.Create(closeNavigationService);

            Func<Task> _updateCustomer = async () =>
            {
                var customer = new Customer()
                {
                    CustomerId = _selectedCustomer.CustomerId,
                    LastName = _lastName,
                    FirstName = _firstName,
                    MiddleName = _middleName,
                    PhoneNumber = _phoneNumber,
                    Email = _email,
                };

                var dataService = new GenericDataService<Customer>();

                try
                {
                    await dataService.Update(customer);
                    int selectedCustomerId = SelectedCustomer.CustomerId;
                    await _customersStore.ResetCustomers();
                    SelectedCustomer = Customers.First(customer => customer.CustomerId == selectedCustomerId);
                }
                catch (Exception ex)
                {
                    MessageDisplayHelper.DisplayError(ex);
                }
            };

            Func<bool> _canUpdateCustomer = () =>
            {
                return !string.IsNullOrWhiteSpace(_lastName)
                    && !string.IsNullOrWhiteSpace(_firstName)
                    && !string.IsNullOrWhiteSpace(_middleName)
                    && !string.IsNullOrWhiteSpace(_phoneNumber)
                    && (_lastName != SelectedCustomer.LastName ||
                        _firstName != SelectedCustomer.FirstName ||
                        _middleName != SelectedCustomer.MiddleName ||
                        _phoneNumber != SelectedCustomer.PhoneNumber ||
                        _email != SelectedCustomer.Email);
            };

            UpdateCustomerCommand = new AsyncRelayCommand(_updateCustomer, _canUpdateCustomer);

            Action _reset = () => 
            {
                if (SelectedCustomer == null)
                    return;

                LastName = SelectedCustomer.LastName;
                FirstName = SelectedCustomer.FirstName;
                MiddleName = SelectedCustomer.MiddleName;
                PhoneNumber = SelectedCustomer.PhoneNumber;
                Email = SelectedCustomer.Email;
            };

            Func<bool> _canReset = () =>
            {
                return SelectedCustomer != null
                    && (_lastName != SelectedCustomer.LastName
                    || _firstName != SelectedCustomer.FirstName
                    || _middleName != SelectedCustomer.MiddleName
                    || _phoneNumber != SelectedCustomer.PhoneNumber
                    || _email != SelectedCustomer.Email);
            };

            ResetCommand = new RelayCommand(_reset, _canReset);
        }

        private Customer _selectedCustomer = null;

        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                SetProperty(ref _selectedCustomer, value);
                CustomerIsSelected = true;

                ResetCommand.Execute(null);
            }
        }

        private bool _customerIsSelected = false;

        public bool CustomerIsSelected
        {
            get => _customerIsSelected;
            set => SetProperty(ref _customerIsSelected, value);
        }

        private void Refresh()
        {
            ((AsyncRelayCommand)UpdateCustomerCommand).NotifyCanExecuteChanged();
            ((RelayCommand)ResetCommand).NotifyCanExecuteChanged();
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
