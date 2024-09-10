using NameMirror.Commands;
using System.Linq;

namespace NameMirror.ViewContexts.MainViewContext;

public partial class MainContextLogic
{
    // Commands : Rename
    private readonly ActionCommand renameAllCommand;

    public ActionCommand RenameAllCommand => renameAllCommand;

    private bool CanRenameAll(object? parameter) => Data.AtLeastOneTask; // Create AtleastOneTaskReady

    private void RenameAll(object? parameter)
        => RenameFiles(Data.Tasks.Where(x => x.Ready));

    private readonly ActionCommand renameSelectedCommand;
    public ActionCommand RenameSelectedCommand => renameSelectedCommand;

    private bool CanRenameSelected(object? parameter) => Data.AtLeastOneSelected; // Use AtleastOneSelected + Ready

    private void RenameSelected(object? parameter)
        => RenameFiles(Data.Selection.Where(x => x.Ready));
}