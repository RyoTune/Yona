using DynamicData.Binding;
using System.ComponentModel;
using System.Reactive.Linq;
using Yona.Core.Common;

namespace Yona.Core.Extensions;

public static class SavableFileExtensions
{
    public const int SAVE_BUFFER_MS = 250;

    public static IDisposable AutosaveWithChanges<T>(this SavableFile<T> savable)
        where T : INotifyPropertyChanged, new() =>
        savable.Data.WhenAnyPropertyChanged()
            .Throttle(TimeSpan.FromMilliseconds(SAVE_BUFFER_MS))
            .Subscribe(_ => savable.Save());
}
