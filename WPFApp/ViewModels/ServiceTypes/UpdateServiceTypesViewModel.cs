using CommunityToolkit.Mvvm.Input;
using Library.Models;
using Library.Services;
using Library.Stores;
using System.Text.RegularExpressions;
using System.Windows.Input;
using WPFApp.Commands;
using WPFApp.Helpers;
using WPFApp.Services;

namespace WPFApp.ViewModels.ServiceTypes
{
    public class UpdateServiceTypesViewModel : ViewModelBase
    {
        private readonly ServiceTypesStore _serviceTypesStore;

        public IEnumerable<ServiceType> ServiceTypes => _serviceTypesStore.ServiceTypes;

        public ICommand UpdateServiceTypeCommand { get; init; }

        public ICommand ResetCommand { get; init; }

        public ICommand ReturnCommand { get; init; }

        public UpdateServiceTypesViewModel(ServiceTypesStore serviceTypesStore, INavigationService closeNavigationService)
        {
            _serviceTypesStore = serviceTypesStore;
            ReturnCommand = NavigateCommand.Create(closeNavigationService);

            TimeSpan getEstimatedTime()
            {
                var result = Regex.Match(_estimatedTime, "^([0-9]{1,2}):([0-9]{1,2}):([0-9]{1,2}):([0-9]{1,2})$");
                return new TimeSpan(
                    int.Parse(result.Groups[1].Value),
                    int.Parse(result.Groups[2].Value),
                    int.Parse(result.Groups[3].Value),
                    int.Parse(result.Groups[4].Value)
                    );
            }

            Func<Task> _updateServiceType = async () =>
            {
                var serviceType = new ServiceType()
                {
                    //Setting the existing serviceType id ensures that we really update the existing entry
                    ServiceTypeId = _selectedServiceType.ServiceTypeId, //Important!!!
                    Name = _name,
                    Description = _description,
                    MinPrice = decimal.Parse(_minPrice),
                    EstimatedTime = _estimatedTime == null ? null : getEstimatedTime()
                };

                var dataService = new GenericDataService<ServiceType>();

                try
                {
                    await dataService.Update(serviceType);
                    int selectedServiceTypeId = SelectedServiceType.ServiceTypeId;
                    await _serviceTypesStore.ResetServiceTypes();
                    SelectedServiceType = ServiceTypes.First(serviceType => serviceType.ServiceTypeId == selectedServiceTypeId);
                }
                catch (Exception ex)
                {
                    MessageDisplayHelper.DisplayError(ex);
                }
            };

            //I don't know what I'm doing here...
            Func<bool> _canUpdateServiceType = () =>
            {
                return !string.IsNullOrWhiteSpace(_name)
                    && decimal.TryParse(_minPrice, out _)
                    && (EstimatedTime == null || Regex.IsMatch(EstimatedTime, "^([0-9]{1,2}):([0-9]{1,2}):([0-9]{1,2}):([0-9]{1,2})$"))
                    && (_name != SelectedServiceType.Name ||
                        _description != SelectedServiceType.Description ||
                        decimal.Parse(_minPrice) != SelectedServiceType.MinPrice ||
                        (EstimatedTime == null ? null != SelectedServiceType.EstimatedTime : getEstimatedTime() != SelectedServiceType.EstimatedTime));
            };

            UpdateServiceTypeCommand = new AsyncRelayCommand(_updateServiceType, _canUpdateServiceType);

            Action _reset = () =>
            {
                if (SelectedServiceType == null)
                    return;

                Name = SelectedServiceType.Name;
                Description = SelectedServiceType.Description;
                MinPrice = SelectedServiceType.MinPrice.ToString();
                EstimatedTime = SelectedServiceType.EstimatedTime.ToString();
            };

            Func<bool> _canReset = () =>
            {
                return SelectedServiceType != null
                    && (_name != SelectedServiceType.Name || 
                        _description != SelectedServiceType.Description ||
                        (!decimal.TryParse(_minPrice, out _) || decimal.Parse(_minPrice) != SelectedServiceType.MinPrice) ||
                        ((_estimatedTime == null && null != SelectedServiceType.EstimatedTime) ||
                         (_estimatedTime != null && !Regex.IsMatch(_estimatedTime, "^([0-9]{1,2}):([0-9]{1,2}):([0-9]{1,2}):([0-9]{1,2})$")) ||
                         (_estimatedTime != null
                          && Regex.IsMatch(_estimatedTime, "^([0-9]{1,2}):([0-9]{1,2}):([0-9]{1,2}):([0-9]{1,2})$")
                          && getEstimatedTime() != SelectedServiceType.EstimatedTime)));
            };

            ResetCommand = new RelayCommand(_reset, _canReset);
        }

        private ServiceType _selectedServiceType = null;

        public ServiceType SelectedServiceType
        {
            get => _selectedServiceType;
            set
            {
                SetProperty(ref _selectedServiceType, value);
                ServiceTypeIsSelected = true;

                ResetCommand.Execute(null);
            }
        }

        private bool _serviceTypeIsSelected = false;

        public bool ServiceTypeIsSelected
        {
            get => _serviceTypeIsSelected;
            set => SetProperty(ref _serviceTypeIsSelected, value);
        }

        private void Refresh()
        {
            ((AsyncRelayCommand)UpdateServiceTypeCommand).NotifyCanExecuteChanged();
            ((RelayCommand)ResetCommand).NotifyCanExecuteChanged();
        }

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                Refresh();
            }
        }

        private string? _description = null;

        public string? Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    SetProperty(ref _description, null);
                else
                    SetProperty(ref _description, value);
                Refresh();
            }
        }

        private string _minPrice;

        public string MinPrice
        {
            get => _minPrice;
            set
            {
                SetProperty(ref _minPrice, value);
                Refresh();
            }
        }

        private string? _estimatedTime;

        public string? EstimatedTime
        {
            get => _estimatedTime;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    SetProperty(ref _estimatedTime, null);
                else
                    SetProperty(ref _estimatedTime, value);
                Refresh();
            }
        }
    }
}
