using NameMirror.ViewContexts.AboutViewContext;
using NMGui.Resources.Indices;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NMGui.Views.Windows;

/// <summary>
/// Interaction logic for AboutWindow.xaml
/// </summary>
public partial class AboutWindow : Window
{
    private readonly AboutContextLogic _logic;

    public AboutWindow()
    {
        InitializeComponent();

        _logic = new();
        DataContext = _logic;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        /*
        if (DesignerProperties.GetIsInDesignMode(this))
            return;
        */
        Task.Run(async () =>
        {
            try
            {
                await SetupContext();
            }
            catch { }
        });
    }

    private async Task SetupContext()
    {
        await Task.Yield();

        Assembly assembly = Assembly.GetExecutingAssembly();
        Version productVersion = assembly.GetName().Version;
        string dotnetVersion = RuntimeInformation.FrameworkDescription;

        string featuresContent = await LoadManifestResourceContent(assembly, DisclosureIndices.Features);
        string usageTermsContent = await LoadManifestResourceContent(assembly, DisclosureIndices.UsageTerms);
        string disclaimerContent = await LoadManifestResourceContent(assembly, DisclosureIndices.Disclaimer);
        string copyrightsContent = await LoadManifestResourceContent(assembly, DisclosureIndices.Copyrights);
        string attributionContent = await LoadManifestResourceContent(assembly, DisclosureIndices.Attribution);
        string accreditationsContent = await LoadManifestResourceContent(assembly, DisclosureIndices.Accreditations);

        await Application.Current.Dispatcher.BeginInvoke(() =>
        {
            _logic.ProductVersion = productVersion.ToString();
            _logic.DotNetVersion = dotnetVersion;

            _logic.FeaturesContent = featuresContent;
            _logic.UsageTermsContent = usageTermsContent;
            _logic.DisclaimerContent = disclaimerContent;
            _logic.CopyrightsContent = copyrightsContent;
            _logic.AttributionContent = attributionContent;
            _logic.AccreditationsContent = accreditationsContent;
        });

        await Task.Yield();
    }

    private async Task<string> LoadManifestResourceContent(Assembly assembly, string resourcePath)
    {
        try
        {
            using Stream stream = assembly.GetManifestResourceStream(resourcePath);
            using StreamReader reader = new(stream);

            string content = await reader.ReadToEndAsync();
            return content;
        }
        catch (Exception ex)
        {
            return $"Failed to load disclosure from {resourcePath}. Reason: {ex}.";
        }
    }

    private void ScrollTo(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button ||
            button.Tag is not string tag ||
            string.IsNullOrEmpty(tag))
            return;

        foreach (var child in DisclosuresContainer.Children)
        {
            if (child is not TextBlock block || block.Name != tag)
                continue;

            var position = block.TransformToVisual(DisclosuresContainer).Transform(new Point(0, 0));
            DisclosuresScroller.ScrollToVerticalOffset(position.Y);
        }
    }
}