using CommunityToolkit.Mvvm.Input;
using Library.Enums;
using Library.Models;
using Library.Services;
using Library.Stores;
using System.Windows.Input;
using WPFApp.Commands;
using WPFApp.Helpers;
using WPFApp.Services;

namespace WPFApp.ViewModels.Orders
{
    public class AddOrdersViewModel : ViewModelBase
    {
        private readonly OrdersStore _ordersStore;

        public ICommand AddOrderCommand { get; init; }

        public ICommand ResetCommand { get; init; }

        public ICommand ReturnCommand { get; init; }

        public AddOrdersViewModel(OrdersStore ordersStore, INavigationService closeNavigationService)
        {
            _ordersStore = ordersStore;
            ReturnCommand = NavigateCommand.Create(closeNavigationService);

            Func<Task> _addOrder = async () =>
            {
                var order = new Order()
                {
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
                    await dataService.Add(order);
                    await _ordersStore.ResetOrders();
                }
                catch (Exception ex)
                {
                    MessageDisplayHelper.DisplayError(ex);
                }
            };

            Func<bool> _canAddOrder = () =>
            {
                return int.TryParse(CustomerId, out _)
                    && int.TryParse(ServiceTypeId, out _)
                    && int.TryParse(EmployeeId, out _)
                    && !string.IsNullOrWhiteSpace(Description)
                    && decimal.TryParse(Price, out _);
            };

            AddOrderCommand = new AsyncRelayCommand(_addOrder, _canAddOrder);

            ResetCommand = new RelayCommand(() =>
            {
                CustomerId = String.Empty;
                ServiceTypeId = String.Empty;
                EmployeeId = String.Empty;
                Description = String.Empty;
                Price = String.Empty;
                Status = OrderStatus.InProgress;
                PlacedAt = DateTime.Now;
                DueBy = DateTime.Now;
            });

            ResetCommand.Execute(null);
        }

        private void Refresh()
        {
            ((AsyncRelayCommand)AddOrderCommand).NotifyCanExecuteChanged();
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
