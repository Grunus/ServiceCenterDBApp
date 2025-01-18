using CommunityToolkit.Mvvm.Input;
using Library.Enums;
using Library.Models;
using Library.Services;
using Library.Stores;
using System.Windows;
using System.Windows.Input;
using WPFApp.Commands;
using WPFApp.Helpers;
using WPFApp.Services;

namespace WPFApp.ViewModels.Orders
{
    public class UpdateOrdersViewModel : ViewModelBase
    {
        private readonly OrdersStore _ordersStore;

        public IEnumerable<Order> Orders => _ordersStore.Orders;

        public ICommand UpdateOrderCommand { get; init; }

        public ICommand ResetCommand { get; init; }

        public ICommand ReturnCommand { get; init; }

        public UpdateOrdersViewModel(OrdersStore ordersStore, INavigationService closeNavigationService)
        {
            _ordersStore = ordersStore;
            ReturnCommand = NavigateCommand.Create(closeNavigationService);

            Func<Task> _updateOrder = async () =>
            {
                var order = new Order()
                {
                    OrderId = _selectedOrder.OrderId,
                    CustomerId = int.Parse(_customerId),
                    ServiceTypeId = int.Parse(_serviceTypeId),
                    EmployeeId = int.Parse(_employeeId),
                    Description = _description,
                    Price = Math.Round(decimal.Parse(_price), 2),
                    Status = _status,
                    PlacedAt = _placedAt,
                    DueBy = _dueBy,
                };

                var dataService = new GenericDataService<Order>();

                try
                {
                    await dataService.Update(order);
                    int selectedOrderId = SelectedOrder.OrderId;
                    await _ordersStore.ResetOrders();
                    SelectedOrder = Orders.First(order => order.OrderId == selectedOrderId);
                }
                catch (Exception ex)
                {
                    MessageDisplayHelper.DisplayError(ex);
                }
            };

            Func<bool> _canUpdateOrder = () =>
            {
                return int.TryParse(_customerId, out _)
                    && int.TryParse(_serviceTypeId, out _)
                    && int.TryParse(_employeeId, out _)
                    && !string.IsNullOrWhiteSpace(_description)
                    && decimal.TryParse(_price, out _)
                    && (int.Parse(_customerId) != SelectedOrder.CustomerId ||
                        int.Parse(_serviceTypeId) != SelectedOrder.ServiceTypeId ||
                        decimal.Parse(_price) != SelectedOrder.Price ||
                        _status != SelectedOrder.Status ||
                        _placedAt != SelectedOrder.PlacedAt ||
                        _dueBy != SelectedOrder.DueBy);
            };

            UpdateOrderCommand = new AsyncRelayCommand(_updateOrder, _canUpdateOrder);

            Action _reset = () =>
            {
                if (SelectedOrder == null)
                    return;

                CustomerId = SelectedOrder.CustomerId.ToString();
                ServiceTypeId = SelectedOrder.ServiceTypeId.ToString();
                EmployeeId = SelectedOrder.EmployeeId.ToString();
                Description = SelectedOrder.Description;
                Price = SelectedOrder.Price.ToString();
                Status = SelectedOrder.Status;
                _placedAt = SelectedOrder.PlacedAt;
                OnPropertyChanged(nameof(PlacedAt));
                Refresh();
                _dueBy = SelectedOrder.DueBy;
                OnPropertyChanged(nameof(DueBy));
                Refresh();
            };

            Func<bool> _canReset = () =>
            {
                return SelectedOrder != null
                    && (!int.TryParse(_customerId, out _) || int.Parse(_customerId) != SelectedOrder.CustomerId
                    || !int.TryParse(_serviceTypeId, out _) || int.Parse(_serviceTypeId) != SelectedOrder.ServiceTypeId
                    || !int.TryParse(_employeeId, out _) || int.Parse(_employeeId) != SelectedOrder.EmployeeId
                    || _description != SelectedOrder.Description
                    || !decimal.TryParse(_price, out _) || decimal.Parse(_price) != SelectedOrder.Price
                    || _status != SelectedOrder.Status
                    || _placedAt != SelectedOrder.PlacedAt
                    || _dueBy != SelectedOrder.DueBy);
            };

            ResetCommand = new RelayCommand(_reset, _canReset);
        }

        private Order _selectedOrder = null;

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                SetProperty(ref _selectedOrder, value);
                OrderIsSelected = true;

                ResetCommand.Execute(null);
            }
        }

        private bool _orderIsSelected = false;

        public bool OrderIsSelected
        {
            get => _orderIsSelected;
            set => SetProperty(ref _orderIsSelected, value);
        }

        private void Refresh()
        {
            ((AsyncRelayCommand)UpdateOrderCommand).NotifyCanExecuteChanged();
            ((RelayCommand)ResetCommand).NotifyCanExecuteChanged();
        }

        private string _customerId;

        public string CustomerId
        {
            get => _customerId;
            set
            {
                SetProperty(ref _customerId, value);
                Refresh();
            }
        }

        private string _serviceTypeId;

        public string ServiceTypeId
        {
            get => _serviceTypeId;
            set
            {
                SetProperty(ref _serviceTypeId, value);
                Refresh();
            }
        }

        private string _employeeId;

        public string EmployeeId
        {
            get => _employeeId;
            set
            {
                SetProperty(ref _employeeId, value);
                Refresh();
            }
        }

        private string _description;

        public string Description
        {
            get => _description;
            set
            {
                SetProperty(ref _description, value);
                Refresh();
            }
        }

        private string _price;

        public string Price
        {
            get => _price;
            set
            {
                SetProperty(ref _price, value);
                Refresh();
            }
        }

        private OrderStatus _status;

        public OrderStatus Status
        {
            get => _status;
            set
            {
                SetProperty(ref _status, value);
                Refresh();
            }
        }

        private DateOnly _placedAt;

        public DateTime PlacedAt
        {
            get => new DateTime(_placedAt, new TimeOnly());
            set
            {
                SetProperty(ref _placedAt, DateOnly.FromDateTime(value));
                Refresh();
            }
        }

        private DateOnly _dueBy;

        public DateTime DueBy
        {
            get => new DateTime(_dueBy, new TimeOnly());
            set
            {
                SetProperty(ref _dueBy, DateOnly.FromDateTime(value));
                Refresh();
            }
        }
    }
}
