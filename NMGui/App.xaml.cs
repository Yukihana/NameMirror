using System.Reflection;
using System;
using System.Windows;

namespace NMGui;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Application version

        Version version = Assembly.GetExecutingAssembly().GetName().Version;
        Resources["AssemblyVersion"] = version.ToString();

        // Platform

        string platform = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
        Resources["FrameworkVersion"] = platform;
        /*
#if NET5_0_OR_GREATER
        string platform = Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
#elif NETFRAMEWORK
        string platform = $".NET Framework {Environment.Version}";
#endif
        Resources["FrameworkVersion"] = platform;
        */
    }
}