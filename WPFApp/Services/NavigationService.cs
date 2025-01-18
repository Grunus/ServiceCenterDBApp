using WPFApp.Stores;
using WPFApp.ViewModels;

namespace WPFApp.Services
{
    public class NavigationService<TViewModel> : INavigationService 
        where TViewModel : ViewModelBase
    {
        // Could be just navigation store, could be modal navigation store
        private readonly INavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;

        public NavigationService(INavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
