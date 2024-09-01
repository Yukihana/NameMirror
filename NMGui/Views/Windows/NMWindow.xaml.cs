using CSX.DotNet.Logging.Logic;
using NMGui.Agents;
using NameMirror.ViewContexts.NMViewContext;
using NameMirror.Types;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;

namespace NMGui.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class NameMirrorWindow : RibbonWindow
{
    private readonly NMContextLogic _mainLogic;
    private readonly LogManagerLogic _logLogic;
    private TutorWindow? TutWin = null;
    private AboutWindow? AbWin = null;

    // Initialise
    public NameMirrorWindow()
    {
        InitializeComponent();

        // Context : RNLogic
        _mainLogic = new(new Handler());
        DataContext = _mainLogic;

        // Context : LogLogic
        _logLogic = new(MyConfig.LogsPath, MyConfig.LogsPrefix);
        LogPartition.DataContext = _logLogic;

        // Log
        _mainLogic.LogAction = _logLogic.EnterLog;
    }

    // Loaded followup
    private void Window_Loaded(object sender, RoutedEventArgs e) => _mainLogic.OnInterfaceLoaded();

    // Selection changed update
    private void TaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_mainLogic.Data.IsBusy)
            return;

        _mainLogic.Data.Selection.Clear();
        foreach (var n in FileList.SelectedItems)
        {
            _mainLogic.Data.Selection.Add((RNTask)n);
        }
    }

    // Scroll into view
    private void DataGrid_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
    {
        if (sender is DataGrid s)
        {
            s.ScrollIntoView(s.Items[s.Items.Count - 1]);
        }
    }

    // Pre-ViewModel HyperBridges
    private void AddFiles(object sender, RoutedEventArgs e)
    {
        if (_mainLogic.Handler.FileSystemAgent.GetFiles("Add files to be renamed...", MyConfig.LastTasksPath, 0) is string[] paths)
            _mainLogic.AddTasks(paths);
        else
            _logLogic.EnterLog("Cancelled adding tasks", "debug", sender);
    }

    private void InsertFiles(object sender, RoutedEventArgs e)
    {
        if (_mainLogic.Handler.FileSystemAgent.GetFiles("Insert files to be renamed...", MyConfig.LastTasksPath, 0) is string[] paths)
            _mainLogic.AddTasks(paths, true);
        else
            _logLogic.EnterLog("Cancelled inserting tasks", "debug", sender);
    }

    private void AppendReferences(object sender, RoutedEventArgs e)
    {
        if (_mainLogic.Handler.FileSystemAgent.GetFiles("Append references : Add files to copy names from...", MyConfig.LastTasksPath, 0) is string[] paths)
            _mainLogic.AddReferences(paths);
        else
            _logLogic.EnterLog("Cancelled adding references (Append)", "debug", sender);
    }

    private void ReplaceReferencesAt(object sender, RoutedEventArgs e)
    {
        if (_mainLogic.Handler.FileSystemAgent.GetFiles("Replace references at selection : Add files to copy names from...", MyConfig.LastTasksPath, 0) is string[] paths)
            _mainLogic.AddReferences(paths, AddReferencesMode.ReplaceAt);
        else
            _logLogic.EnterLog("Cancelled adding references (Replace At)", "debug", sender);
    }

    private void ReplaceAllReferences(object sender, RoutedEventArgs e)
    {
        if (_mainLogic.Handler.FileSystemAgent.GetFiles("Replace all references : Add files to copy names from...", MyConfig.LastTasksPath, 0) is string[] paths)
            _mainLogic.AddReferences(paths, AddReferencesMode.ReplaceAll);
        else
            _logLogic.EnterLog("Cancelled adding references (Replace All)", "debug", sender);
    }

    // Misc system functions
    private void CreateInstance(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("New instance?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            Process.Start(Assembly.GetExecutingAssembly().Location);
        }
    }

    private void WindowRestart(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Restart?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            Close();
            Process.Start(Assembly.GetExecutingAssembly().Location);
        }
    }

    private void ShowTutor(object sender, RoutedEventArgs e)
    {
        if (TutWin == null)
        {
            TutWin = new();
            TutWin.Closed += (object? sender, EventArgs e) => { TutWin = null; };
        }

        TutWin.Show();
        TutWin.Activate();
    }

    private void ShowAbout(object sender, RoutedEventArgs e)
    {
        if (AbWin == null)
        {
            AbWin = new();
            AbWin.Closed += (object? sender, EventArgs e) => { AbWin = null; };
        }

        AbWin.Show();
        AbWin.Activate();
    }

    private void WindowExit(object sender, RoutedEventArgs e)
    {
        if (!_mainLogic.IsExitReady())
        {
            if (MessageBox.Show("Some files have still not been renamed. Continue with exiting?", "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }
        }

        Close();
    }

    private void StartWizard(object sender, RoutedEventArgs e)
    {
        WizardWindow wiz = new();
        wiz.ShowDialog();
    }
}