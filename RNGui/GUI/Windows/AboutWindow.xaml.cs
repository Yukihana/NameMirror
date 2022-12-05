using RNGui.BuildInfo;
using System;
using System.ComponentModel;
using System.Windows;

namespace CSX.Wpf.Y2022.RNGui.Views.Windows
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window, INotifyPropertyChanged
    {
        public AboutWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [CompilationTimeString]
        private const string CompilationDateTimeUTC = "";

        public static string BuildTime => CompilationDateTimeUTC;
    }
}
