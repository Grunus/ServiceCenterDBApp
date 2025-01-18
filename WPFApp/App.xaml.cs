using Library.Stores;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WPFApp.Services;
using WPFApp.Stores;
using WPFApp.ViewModels;
using WPFApp.ViewModels.Customers;
using WPFApp.ViewModels.Employees;
using WPFApp.ViewModels.Orders;
using WPFApp.ViewModels.Payments;
using WPFApp.ViewModels.ServiceTypes;

namespace WPFApp
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<NavigationStore>();
            services.AddSingleton<ModalNavigationStore>();

            services.AddSingleton<CustomersStore>();
            services.AddSingleton<EmployeesStore>();
            services.AddSingleton<OrdersStore>();
            services.AddSingleton<PaymentsStore>();
            services.AddSingleton<ServiceTypesStore>();

            services.AddSingleton<INavigationService>(s => CreateCustomersNavigationService(s));
            services.AddSingleton<CloseModalNavigationService>();

            #region CustomersViewModels
            services.AddTransient<CustomersViewModel>(s => new CustomersViewModel(
                s.GetRequiredService<CustomersStore>(),
                CreateAddCustomersNavigationService(s),
                CreateUpdateCustomersNavigationService(s),
                CreateDeleteCustomersNavigationService(s)
                ));

            services.AddTransient<AddCustomersViewModel>(s => new AddCustomersViewModel(
                s.GetRequiredService<CustomersStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient<UpdateCustomersViewModel>(s => new UpdateCustomersViewModel(
                s.GetRequiredService<CustomersStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient<DeleteCustomersViewModel>(s => new DeleteCustomersViewModel(
                s.GetRequiredService<CustomersStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));
            #endregion

            #region EmployeesViewModels
            services.AddTransient<EmployeesViewModel>(s => new EmployeesViewModel(
                s.GetRequiredService<EmployeesStore>(),
                CreateAddEmployeesNavigationService(s),
                CreateUpdateEmployeesNavigationService(s),
                CreateDeleteEmployeesNavigationService(s)));

            services.AddTransient<AddEmployeesViewModel>(s => new AddEmployeesViewModel(
                s.GetRequiredService<EmployeesStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient<UpdateEmployeesViewModel>(s => new UpdateEmployeesViewModel(
                s.GetRequiredService<EmployeesStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient<DeleteEmployeesViewModel>(s => new DeleteEmployeesViewModel(
                s.GetRequiredService<EmployeesStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));
            #endregion

            #region OrdersViewModels
            services.AddTransient<OrdersViewModel>(s => new OrdersViewModel(
                s.GetRequiredService<OrdersStore>(),
                CreateAddOrdersNavigationService(s),
                CreateUpdateOrdersNavigationService(s),
                CreateDeleteOrdersNavigationService(s)
                ));

            services.AddTransient<AddOrdersViewModel>(s => new AddOrdersViewModel(
                s.GetRequiredService<OrdersStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient<UpdateOrdersViewModel>(s => new UpdateOrdersViewModel(
                s.GetRequiredService<OrdersStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient<DeleteOrdersViewModel>(s => new DeleteOrdersViewModel(
                s.GetRequiredService<OrdersStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));
            #endregion

            #region PaymentsViewModels
            services.AddTransient<PaymentsViewModel>(s => new PaymentsViewModel(
               s.GetRequiredService<PaymentsStore>(),
               CreateAddPaymentsNavigationService(s),
               CreateUpdatePaymentsNavigationService(s),
               CreateDeletePaymentsNavigationService(s)
               ));

            services.AddTransient<AddPaymentsViewModel>(s => new AddPaymentsViewModel(
                s.GetRequiredService<PaymentsStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient<UpdatePaymentsViewModel>(s => new UpdatePaymentsViewModel(
                s.GetRequiredService<PaymentsStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient<DeletePaymentsViewModel>(s => new DeletePaymentsViewModel(
                s.GetRequiredService<PaymentsStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));
            #endregion

            #region ServiceTypesViewModels
            services.AddTransient<ServiceTypesViewModel>(s => new ServiceTypesViewModel(
                s.GetRequiredService<ServiceTypesStore>(),
                CreateAddServiceTypesNavigationService(s),
                CreateUpdateServiceTypesNavigationService(s),
                CreateDeleteServiceTypesNavigationService(s)
                ));

            services.AddTransient<AddServiceTypesViewModel>(s => new AddServiceTypesViewModel(
                s.GetRequiredService<ServiceTypesStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient<UpdateServiceTypesViewModel>(s => new UpdateServiceTypesViewModel(
                s.GetRequiredService<ServiceTypesStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient<DeleteServiceTypesViewModel>(s => new DeleteServiceTypesViewModel(
                s.GetRequiredService<ServiceTypesStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));
            #endregion

            services.AddSingleton<NavigationBarViewModel>(CreateNavigationBarViewModel);
            services.AddSingleton<MainViewModel>();

            services.AddSingleton<MainWindow>(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;

            INavigationService initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>();
            initialNavigationService.Navigate();

            MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        #region NavigationBarViewModel
        private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
        {
            return new NavigationBarViewModel(
                CreateCustomersNavigationService(serviceProvider),
                CreateEmployeesNavigationService(serviceProvider),
                CreateOrdersNavigationService(serviceProvider),
                CreatePaymentsNavigationService(serviceProvider),
                CreateServiceTypesNavigationService(serviceProvider));
        }
        #endregion

        #region CustomersNavigationServices
        private INavigationService CreateCustomersNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<CustomersViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<CustomersViewModel>());
        }

        private INavigationService CreateAddCustomersNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<AddCustomersViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<AddCustomersViewModel>());
        }

        private INavigationService CreateUpdateCustomersNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<UpdateCustomersViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<UpdateCustomersViewModel>());
        }

        private INavigationService CreateDeleteCustomersNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<DeleteCustomersViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<DeleteCustomersViewModel>());
        }
        #endregion

        #region EmployeesNavigationServices
        private INavigationService CreateEmployeesNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<EmployeesViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<EmployeesViewModel>());
        }

        private INavigationService CreateAddEmployeesNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<AddEmployeesViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<AddEmployeesViewModel>());
        }

        private INavigationService CreateUpdateEmployeesNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<UpdateEmployeesViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<UpdateEmployeesViewModel>());
        }

        private INavigationService CreateDeleteEmployeesNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<DeleteEmployeesViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<DeleteEmployeesViewModel>());
        }
        #endregion

        #region OrdersNavigationServices
        private INavigationService CreateOrdersNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<OrdersViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<OrdersViewModel>());
        }

        private INavigationService CreateAddOrdersNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<AddOrdersViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<AddOrdersViewModel>());
        }

        private INavigationService CreateUpdateOrdersNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<UpdateOrdersViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<UpdateOrdersViewModel>());
        }

        private INavigationService CreateDeleteOrdersNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<DeleteOrdersViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<DeleteOrdersViewModel>());
        }
        #endregion

        #region PaymentsNavigationServices
        private INavigationService CreatePaymentsNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<PaymentsViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<PaymentsViewModel>());
        }

        private INavigationService CreateAddPaymentsNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<AddPaymentsViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<AddPaymentsViewModel>());
        }

        private INavigationService CreateUpdatePaymentsNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<UpdatePaymentsViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<UpdatePaymentsViewModel>());
        }

        private INavigationService CreateDeletePaymentsNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<DeletePaymentsViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<DeletePaymentsViewModel>());
        }
        #endregion

        #region ServiceTypesNavigationServices
        private INavigationService CreateServiceTypesNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<ServiceTypesViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<ServiceTypesViewModel>());
        }

        private INavigationService CreateAddServiceTypesNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<AddServiceTypesViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<AddServiceTypesViewModel>());
        }

        private INavigationService CreateUpdateServiceTypesNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<UpdateServiceTypesViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<UpdateServiceTypesViewModel>());
        }

        private INavigationService CreateDeleteServiceTypesNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<DeleteServiceTypesViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<DeleteServiceTypesViewModel>());
        }
        #endregion
    }
}
