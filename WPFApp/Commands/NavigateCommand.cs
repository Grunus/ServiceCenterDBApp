using CommunityToolkit.Mvvm.Input;
using WPFApp.Services;

namespace WPFApp.Commands
{
    public static class NavigateCommand
    {
        public static RelayCommand Create(INavigationService navigationService)
        {
            return new RelayCommand(() => navigationService.Navigate());
        }
    }
}
