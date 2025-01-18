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
    public class DeletePaymentsViewModel : ViewModelBase
    {
        private readonly PaymentsStore _paymentsStore;

        public IEnumerable<Payment> Payments => _paymentsStore.Payments;

        public ICommand DeletePaymentCommand { get; init; }

        public ICommand ReturnCommand { get; init; }

        public DeletePaymentsViewModel(PaymentsStore paymentsStore, INavigationService closeNavigationService)
        {
            _paymentsStore = paymentsStore;
            ReturnCommand = NavigateCommand.Create(closeNavigationService);

            Func<Task> _deletePayment = async () =>
            {
                var dataService = new GenericDataService<Payment>();

                try
                {
                    await dataService.Delete(SelectedPayment.PaymentId);
                    await _paymentsStore.ResetPayments();
                    SelectedPayment = null;
                }
                catch (Exception ex)
                {
                    MessageDisplayHelper.DisplayError(ex);
                }
            };

            Func<bool> _canDeletePayment = () =>
            {
                return SelectedPayment != null;
            };

            DeletePaymentCommand = new AsyncRelayCommand(_deletePayment, _canDeletePayment);
        }

        private Payment _selectedPayment = null;

        public Payment SelectedPayment
        {
            get => _selectedPayment;
            set
            {
                SetProperty(ref _selectedPayment, value);
                Refresh();
            }
        }

        private void Refresh()
        {
            ((AsyncRelayCommand)DeletePaymentCommand).NotifyCanExecuteChanged();
        }
    }
}
