using NameMirror.Types;
using NameMirror.ViewContexts.MainViewContext;
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
public partial class MainWindow : RibbonWindow
{
    private readonly App _app;
    private readonly MainContextLogic _mainLogic;
    private TutorWindow? TutWin = null;
    private AboutWindow? AbWin = null;

    // Initialise
    public MainWindow()
    {
        InitializeComponent();

        _app = Application.Current as App
            ?? throw new InvalidOperationException("Unable to access members from the application entry class.");

        // Context : Main
        _mainLogic = new();
        DataContext = _mainLogic;

        // Context : LogPartition Logic
        LogPartition.DataContext = _app.LoggingService.ContextLogic;
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