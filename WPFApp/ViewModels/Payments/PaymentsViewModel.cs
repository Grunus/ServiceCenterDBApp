using CommunityToolkit.Mvvm.Input;
using Library.Models;
using Library.Stores;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using WPFApp.Commands;
using WPFApp.Services;

namespace WPFApp.ViewModels.Payments
{
    public class PaymentsViewModel : ViewModelBase
    {
        private readonly PaymentsStore _paymentsStore;

        public ICollectionView PaymentsCollectionView { get; init; }

        private string _paymentsFilter = String.Empty;

        public string PaymentsFilter
        {
            get => _paymentsFilter;
            set
            {
                SetProperty(ref _paymentsFilter, value);
                PaymentsCollectionView.Refresh();
            }
        }

        private bool FilterPayments(object obj)
        {
            if (obj is Payment payment)
            {
                return payment.PaymentId.ToString().Contains(PaymentsFilter, StringComparison.InvariantCultureIgnoreCase)
                    || payment.OrderId.ToString().Contains(PaymentsFilter, StringComparison.InvariantCultureIgnoreCase)
                    || payment.Amount.ToString().Contains(PaymentsFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        private string _paymentsSortPropertyName = String.Empty;

        public string PaymentsSortPropertyName
        {
            get => _paymentsSortPropertyName;
            set
            {
                SetProperty(ref _paymentsSortPropertyName, value);
                RefreshSort();
            }
        }

        public ICommand ChangePaymentsSortPropertyNameCommand { get; init; }

        private ListSortDirection _paymentsSortDirection = ListSortDirection.Ascending;

        public bool IsPaymentsSortDescending
        {
            get
            {
                if (_paymentsSortDirection == ListSortDirection.Descending)
                    return true;
                else 
                    return false;
            }
            set
            {
                if (value)
                    SetProperty(ref _paymentsSortDirection, ListSortDirection.Descending);
                else
                    SetProperty(ref _paymentsSortDirection, ListSortDirection.Ascending);
                RefreshSort();
            }
        }

        private void RefreshSort()
        {
            PaymentsCollectionView.SortDescriptions.Clear();
            if (PaymentsSortPropertyName != String.Empty)
                PaymentsCollectionView.SortDescriptions.Add(new SortDescription(PaymentsSortPropertyName, _paymentsSortDirection));
            PaymentsCollectionView.Refresh();
        }

        public ICommand NavigateAddPaymentsCommand { get; init; }
        public ICommand NavigateUpdatePaymentsCommand { get; init; }
        public ICommand NavigateDeletePaymentsCommand { get; init; }

        public PaymentsViewModel(PaymentsStore paymentsStore,
            INavigationService addPaymentsNavigationService,
            INavigationService updatePaymentsNavigationService,
            INavigationService deletePaymentsNavigationService)
        {
            _paymentsStore = paymentsStore;

            paymentsStore.ResetPayments();

            PaymentsCollectionView = CollectionViewSource.GetDefaultView(paymentsStore.Payments);
            PaymentsCollectionView.Filter = FilterPayments;

            ChangePaymentsSortPropertyNameCommand = new RelayCommand<string>(parameter =>
            {
                PaymentsSortPropertyName = parameter;
            });

            NavigateAddPaymentsCommand = NavigateCommand.Create(addPaymentsNavigationService);
            NavigateUpdatePaymentsCommand = NavigateCommand.Create(updatePaymentsNavigationService);
            NavigateDeletePaymentsCommand = NavigateCommand.Create(deletePaymentsNavigationService);
        }
    }
}
