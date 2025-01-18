using CommunityToolkit.Mvvm.Input;
using Library.Models;
using Library.Services;
using Library.Stores;
using System.Windows.Input;
using WPFApp.Commands;
using WPFApp.Helpers;
using WPFApp.Services;

namespace WPFApp.ViewModels.Orders
{
    public class DeleteOrdersViewModel : ViewModelBase
    {
        private readonly OrdersStore _ordersStore;

        public IEnumerable<Order> Orders => _ordersStore.Orders;

        public ICommand DeleteOrderCommand { get; init; }

        public ICommand ReturnCommand { get; init; }

        public DeleteOrdersViewModel(OrdersStore ordersStore, INavigationService closeNavigationService)
        {
            _ordersStore = ordersStore;
            ReturnCommand = NavigateCommand.Create(closeNavigationService);

            Func<Task> _deleteOrder = async () =>
            {
                var dataService = new GenericDataService<Order>();

                try
                {
                    await dataService.Delete(SelectedOrder.OrderId);
                    await _ordersStore.ResetOrders();
                    SelectedOrder = null;
                }
                catch (Exception ex)
                {
                    MessageDisplayHelper.DisplayError(ex);
                }
            };

            Func<bool> _canDeleteOrder = () =>
            {
                return SelectedOrder != null;
            };

            DeleteOrderCommand = new AsyncRelayCommand(_deleteOrder, _canDeleteOrder);
        }

        private Order _selectedOrder = null;

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                SetProperty(ref _selectedOrder, value);
                Refresh();
            }
        }

        private void Refresh()
        {
            ((AsyncRelayCommand)DeleteOrderCommand).NotifyCanExecuteChanged();
        }
    }
}
