using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace NameMirror.ViewContexts.WizardViewContext;

public partial class WizardLogEntryContext : ObservableObject
{
    [ObservableProperty]
    private DateTime _timeStamp = DateTime.Now;

    [ObservableProperty]
    private string _message = string.Empty;
}