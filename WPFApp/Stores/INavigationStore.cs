using WPFApp.ViewModels;

namespace WPFApp.Stores
{
    public interface INavigationStore
    {
        ViewModelBase CurrentViewModel { set; }
    }
}
