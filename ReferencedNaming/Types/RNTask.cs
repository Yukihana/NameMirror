using CSX.DotNet6.Y2022.ReferencedNaming.Agents;
using System;
using System.ComponentModel;

namespace CSX.DotNet6.Y2022.ReferencedNaming.Types
{
    public class RNTask : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public Exception? LastException { get; set; } = null;
        private const string StatusChars = " ●✔✖";


        // Callbacks
        public Func<string, string> GetFullPath;
        public Func<string, string> GetFilename;
        public Func<string, string> GetExtension;
        public Func<string, string> GetFilenameWithoutExtension;
        public Func<string, string> GetDirectory;
        public Func<string, string, string> Combine;


        // Properties : Primary
        #region Original Path
        private string originalPath = string.Empty;
        public string OriginalPath
        {
            get => originalPath;
            set
            {
                if (!originalPath.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    originalPath = GetFullPath(value);
                    PropertyChanged?.Invoke(this, new(nameof(OriginalPath)));

                    // Derived
                    OriginalFilename = GetFilename(value);
                    OriginalExtension = GetExtension(value);
                    OriginalFilenameWithoutExtension = GetFilenameWithoutExtension(value);
                    OriginalDirectory = GetDirectory(value);

                    // Update
                    GeneratePreview();
                    UpdateStatus();
                }
            }
        }
        #endregion
        #region Reference Path
        private string referencePath = string.Empty;
        public string ReferencePath
        {
            get => referencePath;
            set
            {
                referencePath = value;
                PropertyChanged?.Invoke(this, new(nameof(ReferencePath)));

                // Derived
                ReferenceFilename = GetFilename(value);
                ReferenceFilenameWithoutExtension = GetFilenameWithoutExtension(value);
                ReferenceDirectory = GetDirectory(value);

                // Status
                GeneratePreview();
                UpdateStatus();
            }
        }
        #endregion
        #region Preview Filename
        private string previewFilename = string.Empty;
        public string PreviewFilename
        {
            get => previewFilename;
            private set
            {
                if (!previewFilename.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    previewFilename = value;
                    PropertyChanged?.Invoke(this, new(nameof(PreviewFilename)));

                    // Derived
                    PreviewPath = Combine(OriginalDirectory, PreviewFilename);

                    // Update
                    UpdateStatus();
                }
            }
        }
        #endregion
        #region IsSelected
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    PropertyChanged?.Invoke(this, new(nameof(IsSelected)));
                }
            }
        }
        #endregion


        // Properties : Original Derived
        #region Filename
        private string originalFilename = string.Empty;
        public string OriginalFilename
        {
            get => originalFilename;
            private set
            {
                if (!originalFilename.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    originalFilename = value;
                    PropertyChanged?.Invoke(this, new(nameof(OriginalFilename)));
                }
            }
        }
        #endregion
        #region Filename Without Extension
        private string originalFilenameWithoutExtension = string.Empty;
        public string OriginalFilenameWithoutExtension
        {
            get => originalFilenameWithoutExtension;
            private set
            {
                if (!originalFilenameWithoutExtension.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    originalFilenameWithoutExtension = value;
                    PropertyChanged?.Invoke(this, new(nameof(OriginalFilenameWithoutExtension)));
                }
            }
        }
        #endregion
        #region Extension
        private string originalExtension = string.Empty;
        public string OriginalExtension
        {
            get => originalExtension;
            private set
            {
                if (!originalExtension.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    originalExtension = value;
                    PropertyChanged?.Invoke(this, new(nameof(OriginalExtension)));
                }
            }
        }
        #endregion
        #region Directory
        private string originalDirectory = string.Empty;
        public string OriginalDirectory
        {
            get => originalDirectory;
            private set
            {
                if (!originalDirectory.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    originalDirectory = value;
                    PropertyChanged?.Invoke(this, new(nameof(OriginalDirectory)));
                }
            }
        }
        #endregion


        // Properties : Reference Derived
        #region Filename
        private string referenceFilename = string.Empty;
        public string ReferenceFilename
        {
            get => referenceFilename;
            private set
            {
                if (!referenceFilename.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    referenceFilename = value;
                    PropertyChanged?.Invoke(this, new(nameof(ReferenceFilename)));
                }
            }
        }
        #endregion
        #region Filename Without Extension
        private string referenceFilenameWithoutExtension = string.Empty;
        public string ReferenceFilenameWithoutExtension
        {
            get => referenceFilenameWithoutExtension;
            private set
            {
                if (!referenceFilenameWithoutExtension.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    referenceFilenameWithoutExtension = value;
                    PropertyChanged?.Invoke(this, new(nameof(ReferenceFilenameWithoutExtension)));
                }
            }
        }
        #endregion
        #region Directory
        private string referenceDirectory = string.Empty;
        public string ReferenceDirectory
        {
            get => referenceDirectory;
            private set
            {
                if (!referenceDirectory.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    referenceDirectory = value;
                    PropertyChanged?.Invoke(this, new(nameof(ReferenceDirectory)));
                }
            }
        }
        #endregion


        // Properties : Preview
        #region Generated Filename
        private string generatedFilename = string.Empty;
        public string GeneratedFilename
        {
            get => generatedFilename;
            private set
            {
                if (!generatedFilename.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    // Alter actual only if actual matched generated before the change
                    if (PreviewFilename.Equals(generatedFilename))
                    {
                        PreviewFilename = value;
                    }

                    generatedFilename = value;
                    PropertyChanged?.Invoke(this, new(nameof(GeneratedFilename)));

                    // No more changes required here, as everything will be taken care of by actual preview property handler
                }
            }
        }
        #endregion
        #region Preview Path
        private string previewPath = string.Empty;
        public string PreviewPath
        {
            get => previewPath;
            private set
            {
                if (!previewPath.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    previewPath = value;
                    PropertyChanged?.Invoke(this, new(nameof(PreviewPath)));
                }
            }
        }
        #endregion


        // Properties : Status
        #region Ready
        private bool ready = false;
        public bool Ready
        {
            get => ready;
            private set
            {
                if (ready != value)
                {
                    ready = value;
                    PropertyChanged?.Invoke(this, new(nameof(Ready)));
                    UpdateStatusText();
                }
            }
        }
        #endregion
        #region Success Status
        private bool? successStatus = null;
        public bool? SuccessStatus
        {
            get => successStatus;
            set
            {
                if (successStatus != value)
                {
                    successStatus = value;
                    PropertyChanged?.Invoke(this, new(nameof(SuccessStatus)));
                    UpdateStatusText();
                }
            }
        }
        #endregion
        #region StatusIndex
        private byte statusIndex = 0;
        public byte StatusIndex
        {
            get => statusIndex;
            set
            {
                if (statusIndex != value)
                {
                    statusIndex = value;
                    PropertyChanged?.Invoke(this, new(nameof(StatusIndex)));
                }
            }
        }
        #endregion


        // Ctor
        public RNTask(IFileSystemAgent agent)
        {
            GetFullPath = agent.GetFullPath;
            GetFilename = agent.GetFilename;
            GetDirectory = agent.GetDirectory;
            GetFilenameWithoutExtension = agent.GetFilenameWithoutExtension;
            GetExtension = agent.GetExtension;
            Combine = agent.Combine;
        }


        public void GeneratePreview()
            => GeneratedFilename = ReferenceFilenameWithoutExtension + OriginalExtension;
        private void UpdateStatus()
        {
            Ready =
                (!string.IsNullOrWhiteSpace(OriginalDirectory)) &&
                (!string.IsNullOrEmpty(ReferenceFilenameWithoutExtension));
        }
        private void UpdateStatusText()
        {
            if (SuccessStatus == false)
                StatusIndex = 3;
            else if (SuccessStatus == true)
                StatusIndex = 2;
            else if (Ready)
                StatusIndex = 1;
            else
                StatusIndex = 0;
        }
    }
}
