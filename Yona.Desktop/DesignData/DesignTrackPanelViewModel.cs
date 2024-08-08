using System;
using Yona.Core.ViewModels.TrackPanel;

namespace Yona.Desktop.DesignData;

internal class DesignTrackPanelViewModel : TrackPanelViewModel
{
    public DesignTrackPanelViewModel()
        : base(new() { Name = Guid.NewGuid().ToString(), Category = "Category", Tags = ["Tag 1", "Tag 2", "Tag 3"], OutputPath = "Test/Output/File.hca" }, null!)
    {
    }
}
