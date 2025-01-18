using CommunityToolkit.Mvvm.Input;
using Library.Models;
using Library.Stores;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using WPFApp.Commands;
using WPFApp.Services;

namespace WPFApp.ViewModels.Customers
{
    public partial class CustomersViewModel : ViewModelBase
    {
        private readonly CustomersStore _customersStore;

        public ICollectionView CustomersCollectionView { get; init; }

        private string _customersFilter = String.Empty;

        public string CustomersFilter
        {
            get => _customersFilter;
            set
            {
                SetProperty(ref _customersFilter, value);
                CustomersCollectionView.Refresh();
            }
        }

        private bool FilterCustomers(object obj)
        {
            if (obj is Customer customer)
            {
                return customer.CustomerId.ToString().Contains(CustomersFilter, StringComparison.InvariantCultureIgnoreCase)
                    || customer.LastName.Contains(CustomersFilter, StringComparison.InvariantCultureIgnoreCase)
                    || customer.FirstName.Contains(CustomersFilter, StringComparison.InvariantCultureIgnoreCase)
                    || customer.MiddleName.Contains(CustomersFilter, StringComparison.InvariantCultureIgnoreCase)
                    || customer.PhoneNumber.Contains(CustomersFilter, StringComparison.InvariantCultureIgnoreCase)
                    || (customer.Email != null && customer.Email.Contains(CustomersFilter, StringComparison.InvariantCultureIgnoreCase));
            }
            return false;
        }

        private string _customersSortPropertyName = String.Empty;

        public string CustomersSortPropertyName
        {
            get => _customersSortPropertyName;
            set
            {
                SetProperty(ref _customersSortPropertyName, value);
                RefreshSort();
            }
        }

        public ICommand ChangeCustomersSortPropertyNameCommand { get; init; }

        private ListSortDirection _customersSortDirection = ListSortDirection.Ascending;

        public bool IsCustomersSortDescending
        {
            get
            {
                if (_customersSortDirection == ListSortDirection.Descending)
                    return true;
                else 
                    return false;
            }
            set
            {
                if (value)
                    SetProperty(ref _customersSortDirection, ListSortDirection.Descending);
                else
                    SetProperty(ref _customersSortDirection, ListSortDirection.Ascending);
                RefreshSort();
            }
        }

        private void RefreshSort()
        {
            CustomersCollectionView.SortDescriptions.Clear();
            if (CustomersSortPropertyName != String.Empty)
                CustomersCollectionView.SortDescriptions.Add(new SortDescription(CustomersSortPropertyName, _customersSortDirection));
            CustomersCollectionView.Refresh();
        }

        public ICommand NavigateAddCustomersCommand { get; init; }
        public ICommand NavigateUpdateCustomersCommand { get; init; }
        public ICommand NavigateDeleteCustomersCommand { get; init; }

        public CustomersViewModel(CustomersStore customersStore,
            INavigationService addCustomersNavigationService,
            INavigationService updateCustomersNavigationService,
            INavigationService deleteCustomersNavigationService)
        {
            _customersStore = customersStore;

            CustomersCollectionView = CollectionViewSource.GetDefaultView(customersStore.Customers);
            CustomersCollectionView.Filter = FilterCustomers;

            ChangeCustomersSortPropertyNameCommand = new RelayCommand<string>(parameter =>
            {
                CustomersSortPropertyName = parameter;
            });

            NavigateAddCustomersCommand = NavigateCommand.Create(addCustomersNavigationService);
            NavigateUpdateCustomersCommand = NavigateCommand.Create(updateCustomersNavigationService);
            NavigateDeleteCustomersCommand = NavigateCommand.Create(deleteCustomersNavigationService);
        }
    }
}
