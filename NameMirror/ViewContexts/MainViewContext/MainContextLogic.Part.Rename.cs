using CommunityToolkit.Mvvm.Input;
using System.Linq;

namespace NameMirror.ViewContexts.MainViewContext;

public partial class MainContextLogic
{
    // Commands

    public RelayCommand RenameAllCommand { get; }
    public RelayCommand RenameSelectedCommand { get; }

    // Handlers

    private bool CanRenameAll() => Data.AtLeastOneTask; // Create AtleastOneTaskReady

    private void RenameAll()
        => RenameFiles(Data.Tasks.Where(x => x.Ready));

    private bool CanRenameSelected() => Data.AtLeastOneSelected; // Use AtleastOneSelected + Ready

    private void RenameSelected()
        => RenameFiles(Data.Selection.Where(x => x.Ready));
}