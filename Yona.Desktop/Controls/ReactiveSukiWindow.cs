using Avalonia;
using ReactiveUI;
using SukiUI.Controls;

namespace Yona.Desktop.Controls;

public class ReactiveSukiWindow<TViewModel> : SukiWindow, IViewFor<TViewModel> where TViewModel : class
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("AvaloniaProperty", "AVP1002", Justification = "Generic avalonia property is expected here.")]
    public static readonly StyledProperty<TViewModel?> ViewModelProperty = AvaloniaProperty
            .Register<ReactiveSukiWindow<TViewModel>, TViewModel?>(nameof(ViewModel));

    /// <summary>
    /// Initializes a new instance of the <see cref="ReactiveWindow{TViewModel}"/> class.
    /// </summary>
    public ReactiveSukiWindow()
    {
        // This WhenActivated block calls ViewModel's WhenActivated
        // block if the ViewModel implements IActivatableViewModel.
        this.WhenActivated(disposables => { });
    }

    /// <summary>
    /// The ViewModel.
    /// </summary>
    public TViewModel? ViewModel
    {
        get => GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TViewModel?)value;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == DataContextProperty)
        {
            if (ReferenceEquals(change.OldValue, ViewModel)
                && change.NewValue is null or TViewModel)
            {
                SetCurrentValue(ViewModelProperty, change.NewValue);
            }
        }
        else if (change.Property == ViewModelProperty)
        {
            if (ReferenceEquals(change.OldValue, DataContext))
            {
                SetCurrentValue(DataContextProperty, change.NewValue);
            }
        }
    }
}
