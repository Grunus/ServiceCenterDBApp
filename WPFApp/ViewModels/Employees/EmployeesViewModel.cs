using CommunityToolkit.Mvvm.Input;
using Library.Models;
using Library.Stores;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using WPFApp.Commands;
using WPFApp.Services;

namespace WPFApp.ViewModels.Employees
{
    public partial class EmployeesViewModel : ViewModelBase
    {
        private readonly EmployeesStore _employeesStore;

        public ICollectionView EmployeesCollectionView { get; init; }

        private string _employeesFilter = String.Empty;

        public string EmployeesFilter
        {
            get => _employeesFilter;
            set
            {
                SetProperty(ref _employeesFilter, value);
                EmployeesCollectionView.Refresh();
            }
        }

        private bool FilterEmployees(object obj)
        {
            if (obj is Employee employee)
            {
                return employee.EmployeeId.ToString().Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase)
                    || employee.LastName.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase)
                    || employee.FirstName.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase)
                    || employee.MiddleName.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase)
                    || employee.PhoneNumber.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase)
                    || employee.Email.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase)
                    || employee.Specialization.Contains(EmployeesFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        private string _employeesSortPropertyName = String.Empty;

        public string EmployeesSortPropertyName
        {
            get => _employeesSortPropertyName;
            set
            {
                SetProperty(ref _employeesSortPropertyName, value);
                RefreshSort();
            }
        }

        public ICommand ChangeEmployeesSortPropertyNameCommand { get; init; }

        private ListSortDirection _employeesSortDirection = ListSortDirection.Ascending;

        public bool IsEmployeesSortDescending
        {
            get
            {
                if (_employeesSortDirection == ListSortDirection.Descending)
                    return true;
                else 
                    return false;
            }
            set
            {
                if (value)
                    SetProperty(ref _employeesSortDirection, ListSortDirection.Descending);
                else
                    SetProperty(ref _employeesSortDirection, ListSortDirection.Ascending);
                RefreshSort();
            }
        }

        private void RefreshSort()
        {
            EmployeesCollectionView.SortDescriptions.Clear();
            if (EmployeesSortPropertyName != String.Empty)
                EmployeesCollectionView.SortDescriptions.Add(new SortDescription(EmployeesSortPropertyName, _employeesSortDirection));
            EmployeesCollectionView.Refresh();
        }

        public ICommand NavigateAddEmployeesCommand { get; init; }
        public ICommand NavigateUpdateEmployeesCommand { get; init; }
        public ICommand NavigateDeleteEmployeesCommand { get; init; }

        public EmployeesViewModel(EmployeesStore employeesStore,
            INavigationService addEmployeesNavigationService,
            INavigationService updateEmployeesNavigationService,
            INavigationService deleteEmployeesNavigationService)
        {
            _employeesStore = employeesStore;

            EmployeesCollectionView = CollectionViewSource.GetDefaultView(employeesStore.Employees);
            EmployeesCollectionView.Filter = FilterEmployees;

            ChangeEmployeesSortPropertyNameCommand = new RelayCommand<string>(parameter =>
            {
                EmployeesSortPropertyName = parameter;
            });

            NavigateAddEmployeesCommand = NavigateCommand.Create(addEmployeesNavigationService);
            NavigateUpdateEmployeesCommand = NavigateCommand.Create(updateEmployeesNavigationService);
            NavigateDeleteEmployeesCommand = NavigateCommand.Create(deleteEmployeesNavigationService);
        }
    }
}
