using System.ComponentModel;

namespace NameMirror.ViewContexts.AboutViewContext;

public sealed partial class AboutContextLogic : INotifyPropertyChanged
{
    // Interface : INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Version

    private string _productVersion = "PRODUCT_VERSION_PLACEHOLDER";

    public string ProductVersion
    {
        get => _productVersion;
        set
        {
            if (_productVersion != value)
            {
                _productVersion = value;
                OnPropertyChanged(nameof(ProductVersion));
            }
        }
    }

    private string _dotNetVersion = "DOTNET_VERSION_PLACEHOLDER";

    public string DotNetVersion
    {
        get => _dotNetVersion;
        set
        {
            if (_dotNetVersion != value)
            {
                _dotNetVersion = value;
                OnPropertyChanged(nameof(DotNetVersion));
            }
        }
    }

    // Disclosures

    private string _featuresContent = "FEATURES_PLACEHOLDER";

    public string FeaturesContent
    {
        get => _featuresContent;
        set
        {
            if (_featuresContent != value)
            {
                _featuresContent = value;
                OnPropertyChanged(nameof(FeaturesContent));
            }
        }
    }

    private string _usageTermsContent = "USAGE_TERMS_PLACEHOLDER";

    public string UsageTermsContent
    {
        get => _usageTermsContent;
        set
        {
            if (_usageTermsContent != value)
            {
                _usageTermsContent = value;
                OnPropertyChanged(nameof(UsageTermsContent));
            }
        }
    }

    private string _disclaimerContent = "DISCLAIMER_PLACEHOLDER";

    public string DisclaimerContent
    {
        get => _disclaimerContent;
        set
        {
            if (_disclaimerContent != value)
            {
                _disclaimerContent = value;
                OnPropertyChanged(nameof(DisclaimerContent));
            }
        }
    }

    private string _copyrightsContent = "COPYRIGHTS_PLACEHOLDER";

    public string CopyrightsContent
    {
        get => _copyrightsContent;
        set
        {
            if (_copyrightsContent != value)
            {
                _copyrightsContent = value;
                OnPropertyChanged(nameof(CopyrightsContent));
            }
        }
    }

    private string _attributionContent = "ATTRIBUTION_PLACEHOLDER";

    public string AttributionContent
    {
        get => _attributionContent;
        set
        {
            if (_attributionContent != value)
            {
                _attributionContent = value;
                OnPropertyChanged(nameof(AttributionContent));
            }
        }
    }

    private string _accreditationsContent = "ACCREDITATIONS_PLACEHOLDER";

    public string AccreditationsContent
    {
        get => _accreditationsContent;
        set
        {
            if (_accreditationsContent != value)
            {
                _accreditationsContent = value;
                OnPropertyChanged(nameof(AccreditationsContent));
            }
        }
    }
}