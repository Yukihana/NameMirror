using CSX.Wpf.Dialogs.Support;
using CSX.Wpf.Dialogs.Windows;
using System;
using System.Media;
using System.Windows;

namespace CSX.Wpf.Dialogs
{
    public class Activator
    {
        public virtual void PlaySound(string soundType)
        {
            (soundType switch
            {
                "exclamation" => SystemSounds.Exclamation,
                "beep" => SystemSounds.Beep,
                "question" => SystemSounds.Question,
                "hand" => SystemSounds.Hand,
                _ => SystemSounds.Asterisk,
            }).Play();
        }

        public virtual void Alert(string message, string title = "Alert", string level = "information")
            => MessageBox.Show(message, title, MessageBoxButton.OK, level.ToMessageBoxImage());


        public virtual string? Query(
            string message,
            string title = "Query",
            string buttons = "OK;Cancel",
            string level = "information",
            string defaultButton = "OK",
            string cancelButton = "Cancel",
            object? customImage = null
            )
        {
            // Prep
            QueryDialog query = new()
            {
                Title = title,
                Message = message,
                Level = level.ToAlertLevel(),
            };
            query.AddButtons(buttons);
            query.DefaultButton = defaultButton;
            query.CancelButton = cancelButton;

            // Show
            query.ShowDialog();

            // Wrap up
            return query.GetResult();
        }


        public virtual bool Validate(string? result, string valid = "OK")
            => result != null && result.Equals(valid, StringComparison.OrdinalIgnoreCase);
        public virtual bool Invalidate(string? result, string invalid = "Cancel")
            => result == null || result.Equals(invalid, StringComparison.OrdinalIgnoreCase);
    }
}