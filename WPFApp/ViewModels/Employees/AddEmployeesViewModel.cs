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
    public class AddEmployeesViewModel : ViewModelBase
    {
        private readonly EmployeesStore _employeesStore;

        public ICommand AddEmployeeCommand { get; init; }

        public ICommand ResetCommand { get; init; }

        public ICommand ReturnCommand { get; init; }

        public AddEmployeesViewModel(EmployeesStore employeesStore, INavigationService closeNavigationService)
        {
            _employeesStore = employeesStore;
            ReturnCommand = NavigateCommand.Create(closeNavigationService);

            Func<Task> _addEmployee = async () =>
            {
                var employee = new Employee()
                {
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
                    await dataService.Add(employee);
                    await _employeesStore.ResetEmployees();
                }
                catch (Exception ex)
                {
                    MessageDisplayHelper.DisplayError(ex);
                }
            };

            Func<bool> _canAddEmployee = () =>
            {
                return !string.IsNullOrWhiteSpace(LastName)
                    && !string.IsNullOrWhiteSpace(FirstName)
                    && !string.IsNullOrWhiteSpace(MiddleName)
                    && !string.IsNullOrWhiteSpace(PhoneNumber)
                    && !string.IsNullOrWhiteSpace(Email)
                    && !string.IsNullOrWhiteSpace(Specialization);
            };

            AddEmployeeCommand = new AsyncRelayCommand(_addEmployee, _canAddEmployee);

            ResetCommand = new RelayCommand(() =>
            {
                LastName = String.Empty;
                FirstName = String.Empty;
                MiddleName = String.Empty;
                PhoneNumber = String.Empty;
                Email = String.Empty;
                Specialization = String.Empty;
            });

            ResetCommand.Execute(null);
        }

        private void Refresh()
        {
            ((AsyncRelayCommand)AddEmployeeCommand).NotifyCanExecuteChanged();
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
