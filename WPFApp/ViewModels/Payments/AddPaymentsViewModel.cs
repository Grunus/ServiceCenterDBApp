using CommunityToolkit.Mvvm.Input;
using Library.Models;
using Library.Services;
using Library.Stores;
using System.Windows.Input;
using WPFApp.Commands;
using WPFApp.Helpers;
using WPFApp.Services;

namespace WPFApp.ViewModels.Payments
{
    public class AddPaymentsViewModel : ViewModelBase
    {
        private readonly PaymentsStore _paymentsStore;

        public ICommand AddPaymentCommand { get; init; }

        public ICommand ResetCommand { get; init; }

        public ICommand ReturnCommand { get; init; }

        public AddPaymentsViewModel(PaymentsStore paymentsStore, INavigationService closeNavigationService)
        {
            _paymentsStore = paymentsStore;
            ReturnCommand = NavigateCommand.Create(closeNavigationService);

            Func<Task> _addPayment = async () =>
            {
                var payment = new Payment()
                {
                    OrderId = int.Parse(_orderId),
                    Amount = Math.Round(decimal.Parse(_amount), 2)
                };

                var dataService = new GenericDataService<Payment>();

                try
                {
                    await dataService.Add(payment);
                    await _paymentsStore.ResetPayments();
                }
                catch (Exception ex)
                {
                    MessageDisplayHelper.DisplayError(ex);
                }
            };

            Func<bool> _canAddPayment = () =>
            {
                return int.TryParse(OrderId, out _)
                    && decimal.TryParse(Amount, out _);
            };

            AddPaymentCommand = new AsyncRelayCommand(_addPayment, _canAddPayment);

            ResetCommand = new RelayCommand(() =>
            {
                OrderId = String.Empty;
                Amount = String.Empty;
            });

            ResetCommand.Execute(null);
        }

        private void Refresh()
        {
            ((AsyncRelayCommand)AddPaymentCommand).NotifyCanExecuteChanged();
        }

        private string _orderId;

        public string OrderId
        {
            get => _orderId;
            set
            {
                SetProperty(ref _orderId, value);
                Refresh();
            }
        }

        private string _amount;

        public string Amount
        {
            get => _amount;
            set
            {
                SetProperty(ref _amount, value);
                Refresh();
            }
        }
    }
}
