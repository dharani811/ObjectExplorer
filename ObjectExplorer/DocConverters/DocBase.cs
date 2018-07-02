using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExplorer.DocConverters
{
    internal class DocBase : NotifyUI
    {
        #region Private Members

        private string _title = null;
        private bool _isDirty = false;
        private bool isNewDoc = false;
        private bool _isVisible = true;
        private string _contentId = null;
        private bool _isSelected = false;
        private bool _isActive = false;
        private Guid tempId;

        #endregion

        #region Public Property

        public string Name
        {
            get;
            private set;
        }
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    NotifyPropertyChanged("IsDirty");
                    NotifyPropertyChanged("FileName");
                }
            }
        }
        public Guid TempId
        {
            get
            {
                return tempId;
            }

            set
            {
                tempId = value;
            }
        }
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    NotifyPropertyChanged("IsVisible");
                }
            }
        }
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                    NotifyPropertyChanged("TruncTitle");

                }
            }
        }

        public string TruncTitle
        {
            get { return _title.Length > 10 ? _title.Substring(0, 5) + "....." : _title; }
        }

        public string ContentId
        {
            get { return _contentId; }
            set
            {
                if (_contentId != value)
                {
                    _contentId = value;
                    NotifyPropertyChanged("ContentId");
                }
            }
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    NotifyPropertyChanged("IsActive");
                }
            }
        }
        public bool IsNewDoc
        {
            get { return isNewDoc; }
            set
            {
                if (isNewDoc != value)
                {
                    isNewDoc = value;
                    NotifyPropertyChanged("IsNewDoc");
                }
            }
        }

        #endregion

        #region Constructors

        public DocBase(string name)
        {
            Name = name;
            Title = name;
        }
        public DocBase()
            : base()
        {
        }

        #endregion
    }
}
