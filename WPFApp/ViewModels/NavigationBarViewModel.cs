using System.Windows.Input;
using WPFApp.Commands;
using WPFApp.Services;

namespace WPFApp.ViewModels
{
    public class NavigationBarViewModel : ViewModelBase
    {
        public ICommand NavigateCustomersCommand { get; }
        public ICommand NavigateEmployeesCommand { get; }
        public ICommand NavigateOrdersCommand { get; }
        public ICommand NavigatePaymentsCommand { get; }
        public ICommand NavigateServiceTypesCommand { get; }

        public NavigationBarViewModel(INavigationService customersNavigationService,
            INavigationService employeesNavigationService,
            INavigationService ordersNavigationService,
            INavigationService paymentsNavigationService,
            INavigationService serviceTypesNavigationService)
        {
            NavigateCustomersCommand = NavigateCommand.Create(customersNavigationService);
            NavigateEmployeesCommand = NavigateCommand.Create(employeesNavigationService);
            NavigateOrdersCommand = NavigateCommand.Create(ordersNavigationService);
            NavigatePaymentsCommand = NavigateCommand.Create(paymentsNavigationService);
            NavigateServiceTypesCommand = NavigateCommand.Create(serviceTypesNavigationService);
        }
    }
}
