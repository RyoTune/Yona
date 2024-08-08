namespace Yona.Core.Common.Dialog;

public class FileSelectOptions
{
    public string? Title { get; set; }

    public bool AllowMultiple { get; set; }

    public string? SuggestedStartLocation { get; set; }

    public string? SuggestedFileName { get; set; }

    public string? Filter { get; set; }
}
