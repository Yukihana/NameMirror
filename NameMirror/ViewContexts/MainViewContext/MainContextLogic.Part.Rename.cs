using NameMirror.Commands;
using System.Linq;

namespace NameMirror.ViewContexts.MainViewContext;

public partial class MainContextLogic
{
    // Commands

    public ActionCommand RenameAllCommand { get; }
    public ActionCommand RenameSelectedCommand { get; }

    // Handlers

    private bool CanRenameAll(object? parameter) => Data.AtLeastOneTask; // Create AtleastOneTaskReady

    private void RenameAll(object? parameter)
        => RenameFiles(Data.Tasks.Where(x => x.Ready));

    private bool CanRenameSelected(object? parameter) => Data.AtLeastOneSelected; // Use AtleastOneSelected + Ready

    private void RenameSelected(object? parameter)
        => RenameFiles(Data.Selection.Where(x => x.Ready));
}