using CommunityToolkit.Mvvm.ComponentModel;

namespace WPFApp.ViewModels
{
    public class ViewModelBase : ObservableObject, IDisposable
    {
        public virtual void Dispose() { }
    }
}
