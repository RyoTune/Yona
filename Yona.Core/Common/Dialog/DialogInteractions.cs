using ReactiveUI;

namespace Yona.Core.Common.Dialog;

public class FileSelectInteraction : Interaction<FileSelectOptions, string[]>
{
}

public class FolderSelectInteraction : Interaction<FolderSelectOptions, string[]>
{
}

public class SaveFileInteraction : Interaction<SaveFileOptions, string[]>
{
}
