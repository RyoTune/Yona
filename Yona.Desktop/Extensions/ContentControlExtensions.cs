using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ReactiveUI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Yona.Core.Common.Dialog;
using Yona.Core.Common.Interactions;
using Yona.Desktop.Controls;

namespace Yona.Desktop.Extensions;

internal static class ContentControlExtensions
{
    public static async Task HandleFileSelect(this ContentControl control, IInteractionContext<FileSelectOptions, string[]> interaction)
    {
        try
        {
            var storage = TopLevel.GetTopLevel(control)!.StorageProvider;
            var files = await storage.OpenFilePickerAsync(new()
            {
                Title = interaction.Input.Title,
                AllowMultiple = interaction.Input.AllowMultiple,
                SuggestedFileName = interaction.Input.SuggestedFileName,
                SuggestedStartLocation = await storage.TryGetFolderFromPathAsync(interaction.Input.SuggestedStartLocation!),
                FileTypeFilter = CreateFilterList(interaction.Input.Filter),
            });

            var filePaths = files.Select(x => x.Path.LocalPath).ToArray();
            interaction.SetOutput(filePaths);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to handle file select dialog.");
        }
    }

    public static async Task HandleFolderSelect(this ContentControl control, IInteractionContext<FolderSelectOptions, string[]> interaction)
    {
        try
        {
            var storage = TopLevel.GetTopLevel(control)!.StorageProvider;
            var folders = await storage.OpenFolderPickerAsync(new()
            {
                Title = interaction.Input.Title,
                AllowMultiple = interaction.Input.AllowMultiple,
                SuggestedFileName = interaction.Input.SuggestedFileName,
                SuggestedStartLocation = await storage.TryGetFolderFromPathAsync(interaction.Input.SuggestedStartLocation!),
            });

            var folderPaths = folders.Select(x => x.Path.LocalPath).ToArray();
            interaction.SetOutput(folderPaths);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to handle folder select dialog.");
        }
    }


    public static async Task HandleSaveFile(this ContentControl control, IInteractionContext<SaveFileOptions, string?> interaction)
    {
        try
        {
            var storage = TopLevel.GetTopLevel(control)!.StorageProvider;
            var file = await storage.SaveFilePickerAsync(new()
            {
                Title = interaction.Input.Title,
                SuggestedFileName = interaction.Input.SuggestedFileName,
                SuggestedStartLocation = await storage.TryGetFolderFromPathAsync(interaction.Input.SuggestedStartLocation!),
                DefaultExtension = interaction.Input.DefaultExtension,
                FileTypeChoices = CreateFilterList(interaction.Input.Filter),
                ShowOverwritePrompt = interaction.Input.ShowOverwritePrompt,
            });

            interaction.SetOutput(file?.Path.LocalPath);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to handle save file dialog.");
        }
    }

    public static void HandleCloseInteraction(this ContentControl control, IInteractionContext<Unit, Unit> interaction)
    {
        try
        {
            ((Window)TopLevel.GetTopLevel(control)!).Close();
            interaction.SetOutput(new());
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to handle close.");
        }
    }

    public static async Task HandleConfirmInteraction(this ContentControl control, IInteractionContext<ConfirmOptions, bool> interaction)
    {
        var window = (Window)TopLevel.GetTopLevel(control)!;
        var confirmWindow = new ConfirmWindow() { DataContext = interaction.Input };
        var result = await confirmWindow.ShowDialog<bool>(window);
        interaction.SetOutput(result);
    }

    private static List<FilePickerFileType> CreateFilterList(string? filter)
    {
        var filters = new List<FilePickerFileType>();
        if (filter != null)
        {
            try
            {
                var filterSplit = filter.Split('|');
                for (int i = 0; i < filterSplit.Length; i += 2)
                {
                    var filterName = filterSplit[0];
                    var filterTypes = filterSplit[i + 1].Split(';');
                    var newFilter = new FilePickerFileType(filterName)
                    {
                        Patterns = filterTypes,
                    };

                    filters.Add(newFilter);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to create file filter list.");
            }
        }

        return filters;
    }
}
