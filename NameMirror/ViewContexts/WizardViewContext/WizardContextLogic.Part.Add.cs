using NameMirror.Types;
using NameMirror.ViewContexts.Shared;

namespace NameMirror.ViewContexts.WizardViewContext;

public partial class WizardContextLogic
{
    private void AddTargetFiles()
    {
        var files = _services.FileInputService.AddFiles(FileInputReason.AddTargets, fromFolder: false);
        if (files is null)
        {
            _services.LogService.Log("debug", "Adding targets from files was either cancelled or has failed.", this);
            return;
        }
        foreach (var file in files)
            ContextData.Targets.Add(FilePath.Create(file));

        UpdateNavigation();
    }

    private void AddTargetFolder()
    {
        var files = _services.FileInputService.AddFiles(FileInputReason.AddTargets, fromFolder: true);
        if (files is null)
        {
            _services.LogService.Log("debug", "Adding targets from folder was either cancelled or has failed.", this);
            return;
        }
        foreach (var file in files)
            ContextData.Targets.Add(FilePath.Create(file));

        UpdateNavigation();
    }

    private void ExecuteClearTargets()
    {
        ContextData.Targets.Clear();
        UpdateNavigation();
    }

    private void AddReferenceFiles()
    {
        var files = _services.FileInputService.AddFiles(FileInputReason.AddReferences, fromFolder: false);
        if (files is null)
        {
            _services.LogService.Log("debug", "Adding references from files was either cancelled or has failed.", this);
            return;
        }
        foreach (var file in files)
            ContextData.References.Add(FilePath.Create(file));

        UpdateNavigation();
    }

    private void AddReferenceFolder()
    {
        var files = _services.FileInputService.AddFiles(FileInputReason.AddReferences, fromFolder: true);
        if (files is null)
        {
            _services.LogService.Log("debug", "Adding references from folder was either cancelled or has failed.", this);
            return;
        }
        foreach (var file in files)
            ContextData.References.Add(FilePath.Create(file));

        UpdateNavigation();
    }

    private void ExecuteClearReferences()
    {
        ContextData.References.Clear();
        UpdateNavigation();
    }
}