using NameMirror.ViewContexts.Shared;
using System.Collections.Generic;

namespace NameMirror.ViewContexts.MainViewContext;

public partial class MainContextLogic
{
    private void AddTasks(IList<RenameTask> tasks)
    {
        foreach (RenameTask task in tasks)
        {
            // Change this viewmodel to use RenameTask fully.
            // This conversion should be temporary.
            Data.Tasks.Add(new()
            {
                OriginalPath = task.Target.FullPath,
                ReferencePath = task.Reference.FullPath,
            });
        }
    }
}