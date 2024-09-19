using NameMirror.ViewContexts.Shared;
using System.Collections.Generic;

namespace NameMirror.ViewContexts.MainViewContext;

public interface IRenameTaskReceptor
{
    void SetTasks(IList<RenameTask> tasks, bool clearExisting = false);
}