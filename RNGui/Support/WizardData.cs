using System.ComponentModel;

namespace CSX.Wpf.Y2022.RNGui.Support;

internal class WizardData : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private int _pageIndex = 0;

    public int PageIndex
    {
        get => _pageIndex;
        set
        {
            if (_pageIndex != value)
            {
                _pageIndex = value;
                PropertyChanged?.Invoke(this, new(nameof(PageIndex)));
            }
        }
    }
}