namespace CSX.DotNet.Logging.Types
{
    public enum LogLevel : byte
    {
        Default = 0,
        Information = 0,

        Details = 10,
        Notes = 20,
        Noise = 30,

        Query = 50,

        Important = 100,
        Summary = 110,

        Exclamation = 200,
        Warning = 210,
        Error = 220,

        Debug = 250,
    }
}