namespace NMGui.Resources;

public static partial class DisclosureIndices
{
    public const string DisclosuresBasePath = $"NMGui.Resources.Disclosures";

    public const string FeaturesFileId = "Features.txt";
    public const string UsageTermsFileId = "UsageTerms.txt";
    public const string DisclaimerFileId = "Disclaimer.txt";
    public const string CopyrightsFileId = "Copyrights.txt";
    public const string AttributionFileId = "Attribution.txt";
    public const string AccreditationsFileId = "Accreditations.txt";

    public static string Features => $"{DisclosuresBasePath}.{FeaturesFileId}";
    public static string UsageTerms => $"{DisclosuresBasePath}.{UsageTermsFileId}";
    public static string Disclaimer => $"{DisclosuresBasePath}.{DisclaimerFileId}";
    public static string Copyrights => $"{DisclosuresBasePath}.{CopyrightsFileId}";
    public static string Attribution => $"{DisclosuresBasePath}.{AttributionFileId}";
    public static string Accreditations => $"{DisclosuresBasePath}.{AccreditationsFileId}";
}