using CommunityToolkit.Mvvm.Input;
using Library.Miscellaneous;
using Library.Models;
using Library.Stores;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using WPFApp.Commands;
using WPFApp.Services;

namespace WPFApp.ViewModels.Orders
{
    public class OrdersViewModel : ViewModelBase
    {
        private readonly OrdersStore _ordersStore;

        public ICollectionView OrdersCollectionView { get; init; }

        private string _ordersFilter = String.Empty;

        public string OrdersFilter
        {
            get => _ordersFilter;
            set
            {
                SetProperty(ref _ordersFilter, value);
                OrdersCollectionView.Refresh();
            }
        }

        private bool FilterOrders(object obj)
        {
            if (obj is Order order)
            {
                return order.OrderId.ToString().Contains(OrdersFilter, StringComparison.InvariantCultureIgnoreCase)
                    || order.CustomerId.ToString().Contains(OrdersFilter, StringComparison.InvariantCultureIgnoreCase)
                    || order.ServiceTypeId.ToString().Contains(OrdersFilter, StringComparison.InvariantCultureIgnoreCase)
                    || order.EmployeeId.ToString().Contains(OrdersFilter, StringComparison.InvariantCultureIgnoreCase)
                    || order.Description.Contains(OrdersFilter, StringComparison.InvariantCultureIgnoreCase)
                    || order.Price.ToString().Contains(OrdersFilter, StringComparison.InvariantCultureIgnoreCase)
                    || order.Status.GetEnumDescription().Contains(OrdersFilter, StringComparison.InvariantCultureIgnoreCase)
                    || order.PlacedAt.ToShortDateString().Contains(OrdersFilter, StringComparison.InvariantCultureIgnoreCase)
                    || order.DueBy.ToShortDateString().Contains(OrdersFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        private string _ordersSortPropertyName = String.Empty;

        public string OrdersSortPropertyName
        {
            get => _ordersSortPropertyName;
            set
            {
                SetProperty(ref _ordersSortPropertyName, value);
                RefreshSort();
            }
        }

        public ICommand ChangeOrdersSortPropertyNameCommand { get; init; }

        private ListSortDirection _ordersSortDirection = ListSortDirection.Ascending;

        public bool IsOrdersSortDescending
        {
            get
            {
                if (_ordersSortDirection == ListSortDirection.Descending)
                    return true;
                else 
                    return false;
            }
            set
            {
                if (value)
                    SetProperty(ref _ordersSortDirection, ListSortDirection.Descending);
                else
                    SetProperty(ref _ordersSortDirection, ListSortDirection.Ascending);
                RefreshSort();
            }
        }

        private void RefreshSort()
        {
            OrdersCollectionView.SortDescriptions.Clear();
            if (OrdersSortPropertyName != String.Empty)
                OrdersCollectionView.SortDescriptions.Add(new SortDescription(OrdersSortPropertyName, _ordersSortDirection));
            OrdersCollectionView.Refresh();
        }

        public ICommand NavigateAddOrdersCommand { get; init; }
        public ICommand NavigateUpdateOrdersCommand { get; init; }
        public ICommand NavigateDeleteOrdersCommand { get; init; }

        public OrdersViewModel(OrdersStore ordersStore,
            INavigationService addOrdersNavigationService,
            INavigationService updateOrdersNavigationService,
            INavigationService deleteOrdersNavigationService)
        {
            _ordersStore = ordersStore;

            ordersStore.ResetOrders();

            OrdersCollectionView = CollectionViewSource.GetDefaultView(ordersStore.Orders);
            OrdersCollectionView.Filter = FilterOrders;

            ChangeOrdersSortPropertyNameCommand = new RelayCommand<string>(parameter =>
            {
                OrdersSortPropertyName = parameter;
            });

            NavigateAddOrdersCommand = NavigateCommand.Create(addOrdersNavigationService);
            NavigateUpdateOrdersCommand = NavigateCommand.Create(updateOrdersNavigationService);
            NavigateDeleteOrdersCommand = NavigateCommand.Create(deleteOrdersNavigationService);
        }
    }
}
