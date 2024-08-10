using Yona.Core.ViewModels.Dashboard.Home;

namespace Yona.Desktop.DesignData;

internal class DesignHomeViewModel : HomeViewModel
{
    public DesignHomeViewModel()
        : base(new(new(), null!), new(new(), null!), null!, null!)
    {
    }
}
