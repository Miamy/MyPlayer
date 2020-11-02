using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace MyPlayer.ViewModels
{
    public class VisualObject<T> : BaseModel where T : IMediaBase
    {
        private IQueueViewModel _owner;
        public IQueueViewModel Owner
        {
            get => _owner;
            set
            {
                if (_owner != null)
                {
                    _owner.PropertyChanged -= OwnerPropertyChanged;
                }
                _owner = value;
                if (_owner != null)
                {
                    _owner.PropertyChanged += OwnerPropertyChanged;
                }
            }
        }

        protected int HeaderHeight { get; set; } = 40;
        protected int ItemHeight { get; set; } = 30;

      

        protected bool _isSelected = true;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                Set(ref _isSelected, value);
                if (Children != null)
                {
                    foreach (var child in Children)
                    {
                        child.IsSelected = value;
                    }
                }
            }
        }

        protected bool _isExpanded = true;

        public bool IsExpanded
        {
            get => _isExpanded;
            set => Set(ref _isExpanded, value);
        }


        public bool IsVisible
        {
            get
            {
                var searchIsEmpty = string.IsNullOrWhiteSpace(Owner.SearchText);
                return searchIsEmpty || Name.Contains(Owner.SearchText, StringComparison.InvariantCultureIgnoreCase) || 
                    (Children != null && Children.Any(child => child.IsVisible));
            }
        }

        private ICollection<VisualObject<IMediaBase>> children;
        public ICollection<VisualObject<IMediaBase>> Children
        {
            get => children;
            set
            {
                children = value;
            }
        }

        private T _data;

        public T Data
        {
            get => _data;
            set
            {
                _data = value;
                if (_data?.Children != null)
                {
                    Children = new ObservableCollection<VisualObject<IMediaBase>>(_data.Children.Select(child => new VisualObject<IMediaBase>(child, Owner)));
                    //_childrenHeight = Children.Sum(child => child.Height);
                }
            }
        }

        //private int _childrenHeight = 0;

        public string Name => Data?.Name;
        public int Height
        {
            get
            {
                if (!IsVisible)
                {
                    return 0;
                }
                var isSong = Children == null;
                if (isSong)
                {
                    return Owner.ShowSongs ? ItemHeight : 0;
                }
                else
                {
                    return //_childrenHeight;
                        Owner.ShowAlbums || Data is IArtist ? Children.Sum(child => child.Height) + HeaderHeight : 0;
                }
            }
        }


        public VisualObject(T data, IQueueViewModel owner)
        {
            Owner = owner;
            Data = data;
        }

        private void OwnerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Height")
            {
                RaisePropertyChanged(nameof(Height));
            }
            if (e.PropertyName == "AllSelected")
            {
                IsSelected = _owner.AllSelected;
            }
            if (e.PropertyName == "SearchText")
            {
                RaisePropertyChanged(nameof(IsVisible));
            }
        }
    }
}
