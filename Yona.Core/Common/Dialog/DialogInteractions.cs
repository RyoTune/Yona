using ReactiveUI;

namespace Yona.Core.Common.Dialog;

public class DialogInteraction<TViewModel> : Interaction<TViewModel, bool>
{
}

public class FileSelectInteraction : Interaction<FileSelectOptions, string[]>
{
}

public class FolderSelectInteraction : Interaction<FolderSelectOptions, string[]>
{
}

public class SaveFileInteraction : Interaction<SaveFileOptions, string[]>
{
}
