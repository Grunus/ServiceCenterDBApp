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
    public class UpdatePaymentsViewModel : ViewModelBase
    {
        private readonly PaymentsStore _paymentsStore;

        public IEnumerable<Payment> Payments => _paymentsStore.Payments;

        public ICommand UpdatePaymentCommand { get; init; }

        public ICommand ResetCommand { get; init; }

        public ICommand ReturnCommand { get; init; }

        public UpdatePaymentsViewModel(PaymentsStore paymentsStore, INavigationService closeNavigationService)
        {
            _paymentsStore = paymentsStore;
            ReturnCommand = NavigateCommand.Create(closeNavigationService);

            Func<Task> _updatePayment = async () =>
            {
                var payment = new Payment()
                {
                    //Setting the existing payment id ensured that we really update the existing entry
                    PaymentId = _selectedPayment.PaymentId, //Important!!!
                    OrderId = int.Parse(_orderId),
                    Amount = Math.Round(decimal.Parse(_amount), 2)
                };

                var dataService = new GenericDataService<Payment>();

                try
                {
                    await dataService.Update(payment);
                    int selectedPaymentId = SelectedPayment.PaymentId;
                    await _paymentsStore.ResetPayments();
                    SelectedPayment = Payments.First(payment => payment.PaymentId == selectedPaymentId);
                }
                catch (Exception ex)
                {
                    MessageDisplayHelper.DisplayError(ex);
                }
            };

            Func<bool> _canUpdatePayment = () =>
            {
                return int.TryParse(_orderId, out _)
                    && decimal.TryParse(_amount, out _)
                    && (int.Parse(_orderId) != SelectedPayment.OrderId ||
                        decimal.Parse(_amount) != SelectedPayment.Amount);
            };

            UpdatePaymentCommand = new AsyncRelayCommand(_updatePayment, _canUpdatePayment);

            Action _reset = () =>
            {
                if (SelectedPayment == null)
                    return;

                OrderId = SelectedPayment.OrderId.ToString();
                Amount = SelectedPayment.Amount.ToString();
            };

            Func<bool> _canReset = () =>
            {
                return SelectedPayment != null
                    && ((!int.TryParse(_orderId, out _) || int.Parse(_orderId) != SelectedPayment.OrderId)
                    || (!decimal.TryParse(_amount, out _) || decimal.Parse(_amount) != SelectedPayment.Amount));
            };

            ResetCommand = new RelayCommand(_reset, _canReset);
        }

        private Payment _selectedPayment = null;

        public Payment SelectedPayment
        {
            get => _selectedPayment;
            set
            {
                SetProperty(ref _selectedPayment, value);
                PaymentIsSelected = true;

                ResetCommand.Execute(null);
            }
        }

        private bool _paymentIsSelected = false;

        public bool PaymentIsSelected
        {
            get => _paymentIsSelected;
            set => SetProperty(ref _paymentIsSelected, value);
        }

        private void Refresh()
        {
            ((AsyncRelayCommand)UpdatePaymentCommand).NotifyCanExecuteChanged();
            ((RelayCommand)ResetCommand).NotifyCanExecuteChanged();
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
