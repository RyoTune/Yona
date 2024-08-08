namespace Yona.Core.Common.Dialog;

public class SaveFileOptions : FileSelectOptions
{
    public string? DefaultExtension { get; set; }

    public bool ShowOverwritePrompt { get; set; } = true;
}
