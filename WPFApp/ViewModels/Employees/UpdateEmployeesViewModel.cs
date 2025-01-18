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
    public class UpdateEmployeesViewModel : ViewModelBase
    {
        private readonly EmployeesStore _employeesStore;

        public IEnumerable<Employee> Employees => _employeesStore.Employees;

        public ICommand UpdateEmployeeCommand { get; init; }

        public ICommand ResetCommand { get; init; }

        public ICommand ReturnCommand { get; init; }

        public UpdateEmployeesViewModel(EmployeesStore employeesStore, INavigationService closeNavigationService)
        {
            _employeesStore = employeesStore;
            ReturnCommand = NavigateCommand.Create(closeNavigationService);

            Func<Task> _updateEmployee = async () =>
            {
                var employee = new Employee()
                {
                    EmployeeId = _selectedEmployee.EmployeeId,
                    LastName = _lastName,
                    FirstName = _firstName,
                    MiddleName = _middleName,
                    PhoneNumber = _phoneNumber,
                    Email = _email,
                    Specialization = _specialization
                };

                var dataService = new GenericDataService<Employee>();

                try
                {
                    await dataService.Update(employee);
                    int selectedEmployeeId = SelectedEmployee.EmployeeId;
                    await _employeesStore.ResetEmployees();
                    SelectedEmployee = Employees.First(employee => employee.EmployeeId == selectedEmployeeId);
                }
                catch (Exception ex)
                {
                    MessageDisplayHelper.DisplayError(ex);
                }
            };

            Func<bool> _canUpdateEmployee = () =>
            {
                return !string.IsNullOrWhiteSpace(_lastName)
                    && !string.IsNullOrWhiteSpace(_firstName)
                    && !string.IsNullOrWhiteSpace(_middleName)
                    && !string.IsNullOrWhiteSpace(_phoneNumber)
                    && !string.IsNullOrWhiteSpace(_email)
                    && !string.IsNullOrWhiteSpace(_specialization)
                    && (_lastName != SelectedEmployee.LastName ||
                        _firstName != SelectedEmployee.FirstName ||
                        _middleName != SelectedEmployee.MiddleName ||
                        _phoneNumber != SelectedEmployee.PhoneNumber ||
                        _email != SelectedEmployee.Email ||
                        _specialization != SelectedEmployee.Specialization);
            };

            UpdateEmployeeCommand = new AsyncRelayCommand(_updateEmployee, _canUpdateEmployee);

            Action _reset = () =>
            {
                if (SelectedEmployee == null)
                    return;

                LastName = SelectedEmployee.LastName;
                FirstName = SelectedEmployee.FirstName;
                MiddleName = SelectedEmployee.MiddleName;
                PhoneNumber = SelectedEmployee.PhoneNumber;
                Email = SelectedEmployee.Email;
                Specialization = SelectedEmployee.Specialization;
            };

            Func<bool> _canReset = () =>
            {
                return SelectedEmployee != null
                    && (_lastName != SelectedEmployee.LastName
                    || _firstName != SelectedEmployee.FirstName
                    || _middleName != SelectedEmployee.MiddleName
                    || _phoneNumber != SelectedEmployee.PhoneNumber
                    || _email != SelectedEmployee.Email
                    || _specialization != SelectedEmployee.Specialization);
            };

            ResetCommand = new RelayCommand(_reset, _canReset);
        }

        private Employee _selectedEmployee = null;

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                SetProperty(ref _selectedEmployee, value);
                EmployeeIsSelected = true;

                ResetCommand.Execute(null);
            }
        }

        private bool _employeeIsSelected = false;

        public bool EmployeeIsSelected
        {
            get => _employeeIsSelected;
            set => SetProperty(ref _employeeIsSelected, value);
        }

        private void Refresh()
        {
            ((AsyncRelayCommand)UpdateEmployeeCommand).NotifyCanExecuteChanged();
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

        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                Refresh();
            }
        }

        private string _specialization;

        public string Specialization
        {
            get => _specialization;
            set
            {
                SetProperty(ref _specialization, value);
                Refresh();
            }
        }
    }
}
