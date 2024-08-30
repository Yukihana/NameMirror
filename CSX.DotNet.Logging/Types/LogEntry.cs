using System;
using System.ComponentModel;

namespace CSX.DotNet.Logging.Types;

public class LogEntry : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    // Business Data
    private readonly DateTime _timeStamp;

    private readonly string _message;
    private readonly LogLevel _level;
    private readonly string _source;
    private readonly string _details;
    private readonly string _linkText;
    private readonly string _linkAction; // if link is uri, open directory. else send to main, which will handle task delegation from there (hook to event)
    private bool _saved = false;

    // Bindable
    public DateTime TimeStamp => _timeStamp;

    public string Message => _message;
    public LogLevel Level => _level;
    public string Source => _source;
    public string Details => _details;
    public string LinkText => _linkText;
    public string LinkAction => _linkAction;

    public bool Saved
    {
        get => _saved;
        set
        {
            _saved = value;
            PropertyChanged?.Invoke(this, new(nameof(Saved)));
        }
    }

    // Ctor
    public LogEntry(
        string message,
        LogLevel level = LogLevel.Information,
        string source = "",
        string details = "",
        string linkText = "",
        string linkAction = "")
    {
        _timeStamp = DateTime.Now;
        this._message = message;
        this._level = level;
        this._source = source;
        this._details = details;
        this._linkText = linkText;
        this._linkAction = linkAction;
    }
}