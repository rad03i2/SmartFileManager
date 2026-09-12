using Microsoft.UI.Xaml;
using SmartFileManager.Models;
using SmartFileManager.Services;
using System.IO;
using System.Linq;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace SmartFileManager;

public sealed partial class MainWindow : Window
{
    private readonly RenameService _renameService = new();
    private readonly DuplicateFinderService _duplicateFinderService = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void RenameButton_Click(object sender, RoutedEventArgs e)
    {
        var folder = await PickFolderAsync();
        if (folder is null) return;

        var options = new RenameOptions(Prefix: "File_", StartNumber: 1, Padding: 3);
        var preview = _renameService.CreatePreview(folder.Path, options).Take(25).ToList();

        OutputText.Text = "Batch Rename Preview:\n\n" + string.Join("\n", preview.Select(x => $"{x.OldName}  ->  {x.NewName}"));
    }

    private async void DuplicateButton_Click(object sender, RoutedEventArgs e)
    {
        var folder = await PickFolderAsync();
        if (folder is null) return;

        OutputText.Text = "Scanning, please wait...";
        var groups = await _duplicateFinderService.FindDuplicatesAsync(folder.Path);

        if (groups.Count == 0)
        {
            OutputText.Text = "No duplicate files found.";
            return;
        }

        OutputText.Text = string.Join("\n\n", groups.Select((group, index) =>
            $"Group {index + 1}:\n" + string.Join("\n", group.Files.Select(file => file.FullPath))));
    }

    private async Task<Windows.Storage.StorageFolder?> PickFolderAsync()
    {
        var picker = new FolderPicker();
        picker.FileTypeFilter.Add("*");
        InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(this));
        return await picker.PickSingleFolderAsync();
    }
}
