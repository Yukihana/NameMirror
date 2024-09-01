using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace NameMirror.ViewContexts.AboutViewContext;

public sealed partial class AboutContextLogic : ObservableObject
{
    // Version

    [ObservableProperty]
    private string _productVersion = "PRODUCT_VERSION_PLACEHOLDER";

    [ObservableProperty]
    private string _dotNetVersion = "DOTNET_VERSION_PLACEHOLDER";

    // Disclosures

    [ObservableProperty]
    private string _featuresContent = "FEATURES_PLACEHOLDER";

    [ObservableProperty]
    private string _usageTermsContent = "USAGE_TERMS_PLACEHOLDER";

    [ObservableProperty]
    private string _disclaimerContent = "DISCLAIMER_PLACEHOLDER";

    [ObservableProperty]
    private string _copyrightsContent = "COPYRIGHTS_PLACEHOLDER";

    [ObservableProperty]
    private string _attributionContent = "ATTRIBUTION_PLACEHOLDER";

    [ObservableProperty]
    private string _accreditationsContent = "ACCREDITATIONS_PLACEHOLDER";
}