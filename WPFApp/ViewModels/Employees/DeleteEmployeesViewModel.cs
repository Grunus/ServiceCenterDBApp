using CommunityToolkit.Mvvm.Input;
using Library.Models;
using Library.Services;
using Library.Stores;
using System.Windows.Input;
using WPFApp.Commands;
using WPFApp.Helpers;
using WPFApp.Services;

namespace WPFApp.ViewModels.Employees
{
    public class DeleteEmployeesViewModel : ViewModelBase
    {
        private readonly EmployeesStore _employeesStore;

        public IEnumerable<Employee> Employees => _employeesStore.Employees;

        public ICommand DeleteEmployeeCommand { get; init; }

        public ICommand ReturnCommand { get; init; }

        public DeleteEmployeesViewModel(EmployeesStore employeesStore, INavigationService closeNavigationService)
        {
            _employeesStore = employeesStore;
            ReturnCommand = NavigateCommand.Create(closeNavigationService);

            Func<Task> _deleteEmployee = async () =>
            {
                var dataService = new GenericDataService<Employee>();

                try
                {
                    await dataService.Delete(SelectedEmployee.EmployeeId);
                    await _employeesStore.ResetEmployees();
                    SelectedEmployee = null;
                }
                catch (Exception ex)
                {
                    MessageDisplayHelper.DisplayError(ex);
                }
            };

            Func<bool> _canDeleteEmployee = () =>
            {
                return SelectedEmployee != null;
            };

            DeleteEmployeeCommand = new AsyncRelayCommand(_deleteEmployee, _canDeleteEmployee);
        }

        private Employee _selectedEmployee = null;

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                SetProperty(ref _selectedEmployee, value);
                Refresh();
            }
        }

        private void Refresh()
        {
            ((AsyncRelayCommand)DeleteEmployeeCommand).NotifyCanExecuteChanged();
        }
    }
}
