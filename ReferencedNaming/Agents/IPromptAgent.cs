namespace ReferencedNaming.Agents
{
    public interface IPromptAgent
    {
        public void PlaySound(string soundType);

        public void Alert(
            string message,
            string title = "Alert",
            string level = "information"
            );

        /// <summary>
        /// Blueprint for defining a messagebox with customizable buttons.
        /// Note that a negative button index starts counting backward from the last button.
        /// When positive, 0 is the first item. When negative, -1 is the last item.
        /// </summary>
        /// <param name="message">The message content of the dialog.</param>
        /// <param name="title">The title of the dialog window.</param>
        /// <param name="buttons">A semicolon separated string concatenation of button names.</param>
        /// <param name="level">The type of notification the dialog is supposed to raise.</param>
        /// <param name="defaultButtonIndex">The array index of the default confirmation button. Default: 0, the first button.</param>
        /// <param name="cancelButtonIndex">The array index of the default cancel button. Disabled if the dialog has only one button. Default: -1, the last button.</param>
        /// <param name="customImage">Optional custom image data. AlertImage.Custom is required for this to work.</param>
        /// <returns>Index of the button clicked by the user</returns>
        public string? Query(
            string message,
            string title = "Query",
            string buttons = "OK;Cancel",
            string level = "information",
            string defaultButton = "OK",
            string cancelButton = "Cancel",
            object? customImage = null
            );

        public bool Validate(string? result, string valid = "OK");
        public bool Invalidate(string? result, string invalid = "Cancel");
    }
}