using Yona.Core.ViewModels.Tracks;

namespace Yona.Desktop.DesignData;

internal class DesignTrackPanelViewModel : TrackPanelViewModel
{
    public DesignTrackPanelViewModel()
        : base(new() { Name = "Test Track", Category = "Category", Tags = ["Tag 1", "Tag 2", "Tag 3"], OutputPath = "Test/Output/File.hca" }, null!)
    {
    }
}
