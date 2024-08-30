using CSX.Wpf.Dialogs.Types;
using System;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CSX.Wpf.Dialogs.Windows;

/// <summary>
/// Interaction logic for QueryDialog.xaml
/// </summary>
public partial class QueryDialog : Window
{
    // Data

    public string Message
    {
        get => DialogMessage.Text;
        set => DialogMessage.Text = value;
    }

    private AlertLevel level = AlertLevel.Information;

    public AlertLevel Level
    {
        get => level;
        set => level = value;
    }

    /// <summary>
    /// Semicolon separated strings
    /// </summary>
    /// <param name="buttonsText"></param>
    public void AddButtons(string buttonsConfig)
    {
        // Sanitize data
#if NET_5_0_OR_GREATER
        var buttonsText = buttonsConfig.Split(";", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
#elif NETFRAMEWORK
        var buttonsText = buttonsConfig.Split(';').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
#endif

        // Clear and populate
        ButtonsPanel.Children.Clear();
        foreach (string btn in buttonsText)
        {
            ButtonsPanel.Children.Add(new Button() { Content = btn, Tag = btn });
        }

        // Fallback incase of empty array
        if (ButtonsPanel.Children.Count == 0)
        {
            ButtonsPanel.Children.Add(new Button() { Content = "OK", Tag = "OK" });
        }
    }

    private string defaultButton = "OK";

    public string DefaultButton
    {
        get => defaultButton;
        set
        {
            if (defaultButton != value)
            {
                defaultButton = value;
            }
        }
    }

    private string cancelButton = "Cancel";

    public string CancelButton
    {
        get => cancelButton;
        set
        {
            if (cancelButton != value)
            {
                cancelButton = value;
            }
        }
    }

    // Init

    public QueryDialog()
    {
        InitializeComponent();

        MaxWidth = Math.Round(SystemParameters.MaximizedPrimaryScreenWidth * 0.7, MidpointRounding.ToEven);
        MaxHeight = Math.Round(SystemParameters.MaximizedPrimaryScreenHeight * 0.9, MidpointRounding.ToEven);
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        PlayAudio();
        DoActivation();
    }

    private void DoActivation()
    {
        Activate();
        foreach (Button button in ButtonsPanel.Children)
        {
            if (button.Tag.Equals(DefaultButton))
            {
                button.IsDefault = true;
                button.Focus();
                break;
            }
        }
    }

    private void PlayAudio()
    {
        var sound = Level switch
        {
            AlertLevel.Query => SystemSounds.Question,
            AlertLevel.Warning => SystemSounds.Exclamation,
            AlertLevel.Error => SystemSounds.Hand,
            _ => SystemSounds.Asterisk,
        };
        sound.Play();
    }

    // Dialog Interaction

    private void ResponseClicked(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            Finish((string)button.Tag);
        }
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Finish(CancelButton);
        }
    }

    // Result

    private string? Result = null;

    public string? GetResult() => Result;

    private void Finish(string btnText)
    {
        Result = btnText == CancelButton ? null : btnText;
        Close();
    }
}