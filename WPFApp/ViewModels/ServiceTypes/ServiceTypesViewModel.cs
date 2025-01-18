using CommunityToolkit.Mvvm.Input;
using Library.Models;
using Library.Stores;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using WPFApp.Commands;
using WPFApp.Services;

namespace WPFApp.ViewModels.ServiceTypes
{
    public class ServiceTypesViewModel : ViewModelBase
    {
        private readonly ServiceTypesStore _serviceTypesStore;

        public ICollectionView ServiceTypesCollectionView { get; init; }

        private string _serviceTypesFilter = String.Empty;

        public string ServiceTypesFilter
        {
            get => _serviceTypesFilter;
            set
            {
                SetProperty(ref _serviceTypesFilter, value);
                ServiceTypesCollectionView.Refresh();
            }
        }

        private bool FilterServiceTypes(object obj)
        {
            if (obj is ServiceType serviceType)
            {
                return serviceType.ServiceTypeId.ToString().Contains(ServiceTypesFilter, StringComparison.InvariantCultureIgnoreCase)
                    || serviceType.Name.Contains(ServiceTypesFilter, StringComparison.InvariantCultureIgnoreCase)
                    || (serviceType.Description != null && serviceType.Description.Contains(ServiceTypesFilter, StringComparison.InvariantCultureIgnoreCase))
                    || serviceType.MinPrice.ToString().Contains(ServiceTypesFilter, StringComparison.InvariantCultureIgnoreCase)
                    || (serviceType.EstimatedTime != null && serviceType.EstimatedTime.ToString().Contains(ServiceTypesFilter, StringComparison.InvariantCultureIgnoreCase));
            }
            return false;
        }

        private string _serviceTypesSortPropertyName = String.Empty;

        public string ServiceTypesSortPropertyName
        {
            get => _serviceTypesSortPropertyName;
            set
            {
                SetProperty(ref _serviceTypesSortPropertyName, value);
                RefreshSort();
            }
        }

        public ICommand ChangeServiceTypesSortPropertyNameCommand { get; init; }

        private ListSortDirection _serviceTypesSortDirection = ListSortDirection.Ascending;

        public bool IsServiceTypesSortDescending
        {
            get
            {
                if (_serviceTypesSortDirection == ListSortDirection.Descending)
                    return true;
                else 
                    return false;
            }
            set
            {
                if (value)
                    SetProperty(ref _serviceTypesSortDirection, ListSortDirection.Descending);
                else
                    SetProperty(ref _serviceTypesSortDirection, ListSortDirection.Ascending);
                RefreshSort();
            }
        }

        private void RefreshSort()
        {
            ServiceTypesCollectionView.SortDescriptions.Clear();
            if (ServiceTypesSortPropertyName != String.Empty)
                ServiceTypesCollectionView.SortDescriptions.Add(new SortDescription(ServiceTypesSortPropertyName, _serviceTypesSortDirection));
            ServiceTypesCollectionView.Refresh();
        }

        public ICommand NavigateAddServiceTypesCommand { get; init; }
        public ICommand NavigateUpdateServiceTypesCommand { get; init; }
        public ICommand NavigateDeleteServiceTypesCommand { get; init; }

        public ServiceTypesViewModel(ServiceTypesStore serviceTypesStore,
            INavigationService addServiceTypesNavigationService,
            INavigationService updateServiceTypesNavigationService,
            INavigationService deleteServiceTypesNavigationService)
        {
            _serviceTypesStore = serviceTypesStore;

            ServiceTypesCollectionView = CollectionViewSource.GetDefaultView(serviceTypesStore.ServiceTypes);
            ServiceTypesCollectionView.Filter = FilterServiceTypes;

            ChangeServiceTypesSortPropertyNameCommand = new RelayCommand<string>(parameter =>
            {
                ServiceTypesSortPropertyName = parameter;
            });

            NavigateAddServiceTypesCommand = NavigateCommand.Create(addServiceTypesNavigationService);
            NavigateUpdateServiceTypesCommand = NavigateCommand.Create(updateServiceTypesNavigationService);
            NavigateDeleteServiceTypesCommand = NavigateCommand.Create(deleteServiceTypesNavigationService);
        }
    }
}
