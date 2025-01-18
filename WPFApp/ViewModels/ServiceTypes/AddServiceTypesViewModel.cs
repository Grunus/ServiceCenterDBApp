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
    public class AddServiceTypesViewModel : ViewModelBase
    {
        private readonly ServiceTypesStore _serviceTypesStore;

        public ICommand AddServiceTypeCommand { get; init; }

        public ICommand ResetCommand { get; init; }

        public ICommand ReturnCommand { get; init; }

        public AddServiceTypesViewModel(ServiceTypesStore serviceTypesStore, INavigationService closeNavigationService)
        {
            _serviceTypesStore = serviceTypesStore;
            ReturnCommand = NavigateCommand.Create(closeNavigationService);

            Func<Task> _addServiceType = async () =>
            {
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

                var serviceType = new ServiceType()
                {
                    Name = _name,
                    Description = _description,
                    MinPrice = decimal.Parse(_minPrice),
                    EstimatedTime = _estimatedTime == null ? null : getEstimatedTime()
                };

                var dataService = new GenericDataService<ServiceType>();

                try
                {
                    await dataService.Add(serviceType);
                    await _serviceTypesStore.ResetServiceTypes();
                }
                catch (Exception ex)
                {
                    MessageDisplayHelper.DisplayError(ex);
                }
            };

            Func<bool> _canAddServiceType = () =>
            {
                return !string.IsNullOrWhiteSpace(Name)
                    && decimal.TryParse(MinPrice, out _)
                    && (EstimatedTime == null || Regex.IsMatch(EstimatedTime, "^([0-9]{1,2}):([0-9]{1,2}):([0-9]{1,2}):([0-9]{1,2})$"));
            };

            AddServiceTypeCommand = new AsyncRelayCommand(_addServiceType, _canAddServiceType);

            ResetCommand = new RelayCommand(() =>
            {
                Name = String.Empty;
                Description = String.Empty;
                MinPrice = String.Empty;
                EstimatedTime = String.Empty;
            });

            ResetCommand.Execute(null);
        }

        private void Refresh()
        {
            ((AsyncRelayCommand)AddServiceTypeCommand).NotifyCanExecuteChanged();
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
