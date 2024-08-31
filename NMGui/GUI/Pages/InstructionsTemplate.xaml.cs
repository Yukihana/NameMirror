using NMGui.Resources.HardCoded;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace NMGui.GUI.Pages
{
    /// <summary>
    /// Interaction logic for Tasks.xaml
    /// </summary>
    public partial class InstructionsTemplate : Page
    {
        public InstructionsTemplate()
        {
            InitializeComponent();
        }

        public void SetData(string name)
        {
            var raw = typeof(InstructionsData)
                .GetProperty(name, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)?
                .GetValue(null);
            // Validate
            if (raw is not List<KeyValuePair<string, string>> data)
            {
                return;
            }

            // Dump
            Title = name.ToUpperInvariant();
            for (int i = 0; i < data.Count; i++)
            {
                // Make row
                InstructionsGrid.RowDefinitions.Add(
                    new() { Height = GridLength.Auto });

                // Make header
                Border header = new()
                {
                    Child = new TextBlock() { Text = data[i].Key }
                };
                InstructionsGrid.Children.Add(header);
                Grid.SetRow(header, i);

                // Make details
                Border details = new()
                {
                    Child = new TextBlock() { Text = data[i].Value, TextWrapping = TextWrapping.WrapWithOverflow }
                };
                InstructionsGrid.Children.Add(details);
                Grid.SetRow(details, i);
                Grid.SetColumn(details, 1);
            }
        }
    }
}