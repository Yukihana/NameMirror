using CSX.DotNet6.Logging.Types;

namespace CSX.DotNet6.Logging.Support
{
    internal static class Converters
    {
        internal static LogLevel Sanitize(this string levelString)
        {
            return levelString.Trim().ToLowerInvariant() switch
            {
                // TODO




                _ => LogLevel.Information,
            };
        }
    }
}