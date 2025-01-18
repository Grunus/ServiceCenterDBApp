using CommunityToolkit.Mvvm.Input;
using Library.Models;
using Library.Services;
using Library.Stores;
using System.Windows.Input;
using WPFApp.Commands;
using WPFApp.Helpers;
using WPFApp.Services;

namespace WPFApp.ViewModels.ServiceTypes
{
    public class DeleteServiceTypesViewModel : ViewModelBase
    {
        private readonly ServiceTypesStore _serviceTypesStore;

        public IEnumerable<ServiceType> ServiceTypes => _serviceTypesStore.ServiceTypes;

        public ICommand DeleteServiceTypeCommand { get; init; }

        public ICommand ReturnCommand { get; init; }

        public DeleteServiceTypesViewModel(ServiceTypesStore serviceTypesStore, INavigationService closeNavigationService)
        {
            _serviceTypesStore = serviceTypesStore;
            ReturnCommand = NavigateCommand.Create(closeNavigationService);

            Func<Task> _deleteServiceType = async () =>
            {
                var dataService = new GenericDataService<ServiceType>();

                try
                {
                    await dataService.Delete(SelectedServiceType.ServiceTypeId);
                    await _serviceTypesStore.ResetServiceTypes();
                    SelectedServiceType = null;
                }
                catch (Exception ex)
                {
                    MessageDisplayHelper.DisplayError(ex);
                }
            };

            Func<bool> _canDeleteServiceType = () =>
            {
                return SelectedServiceType != null;
            };

            DeleteServiceTypeCommand = new AsyncRelayCommand(_deleteServiceType, _canDeleteServiceType);
        }

        private ServiceType _selectedServiceType = null;

        public ServiceType SelectedServiceType
        {
            get => _selectedServiceType;
            set
            {
                SetProperty(ref _selectedServiceType, value);
                Refresh();
            }
        }

        private void Refresh()
        {
            ((AsyncRelayCommand)DeleteServiceTypeCommand).NotifyCanExecuteChanged();
        }
    }
}
