using CSX.DotNet.Logging.Types;

namespace CSX.DotNet.Logging.Support
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