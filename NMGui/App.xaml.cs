using System;
using System.IO;
using System.Reflection;
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

        ReadRuntimeInformation();
        LoadLegals();
    }

    private void ReadRuntimeInformation()
    {
        // Application version

        Version version = Assembly.GetExecutingAssembly().GetName().Version;
        Resources["AssemblyVersion"] = version.ToString();

        // Platform

        string platform = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
        Resources["FrameworkVersion"] = platform;
    }

    private void LoadLegals()
    {
        var assembly = Assembly.GetExecutingAssembly();
        string[] disclosures = ["Accreditations", "Attribution", "Copyrights", "Disclaimer", "Features", "UsageTerms"];

        foreach (var disclosure in disclosures)
        {
            string embedPath = $"NMGui.Resources.Disclosures.{disclosure}.txt";
            string resourceId = $"NameMirror{disclosure}";

            try
            {
                using Stream stream = assembly.GetManifestResourceStream(embedPath);
                using StreamReader reader = new(stream);

                Resources[resourceId] = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Resources[resourceId] = $"Failed to load disclosure from {embedPath}. Reason: {ex}.";
            }
        }
    }
}