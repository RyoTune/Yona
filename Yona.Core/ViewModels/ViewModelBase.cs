using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using System.ComponentModel;

namespace Yona.Core.ViewModels;

public partial class ViewModelBase : ObservableObject, IReactiveObject
{
    public ViewModelBase()
    {
        this.SubscribePropertyChangingEvents();
        this.SubscribePropertyChangedEvents();
    }

    public void RaisePropertyChanged(PropertyChangedEventArgs args) => this.OnPropertyChanged(args);

    public void RaisePropertyChanging(PropertyChangingEventArgs args) => this.OnPropertyChanging(args);
}
