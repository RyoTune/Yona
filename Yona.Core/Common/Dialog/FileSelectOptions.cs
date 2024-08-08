namespace Yona.Core.Common.Dialog;

public class FileSelectOptions : DialogOptions
{
    public bool AllowMultiple { get; set; }

    public string? Filter { get; set; }
}
