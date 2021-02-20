using MyPlayer.Models;
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

        protected bool _isExpanded = true;

        public bool IsExpanded
        {
            get => _isExpanded;
            set => Set(ref _isExpanded, value);
        }

        public bool SearchTextPresent
        {
            get
            {
                return Owner.SearchIsEmpty || Name.Contains(Owner.SearchText, StringComparison.InvariantCultureIgnoreCase) || 
                    (Data.HasChildren && Children.Any(child => child.SearchTextPresent));
            }
        }

        public IList<VisualObject<IMediaBase>> Children { get; set; }

        private T _data;

        public T Data
        {
            get => _data;
            set
            {
                _data = value;
                if (_data?.Children != null)
                {
                    Children = new List<VisualObject<IMediaBase>>(_data.Children.Select(child => new VisualObject<IMediaBase>(child, Owner)));
                }
                IsExpanded = _data is IArtist;
            }
        }      

        public string Name => Data?.Name;

        public VisualObject(T data, IQueueViewModel owner)
        {
            Owner = owner;
            Data = data;
        }

        private void OwnerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Height" || e.PropertyName == "ExpandAlbums" || e.PropertyName == "ExpandSongs")
            {
            }
            if (e.PropertyName == "AllSelected")
            {
                Data.IsSelected = _owner.AllSelected;
            }
            if (e.PropertyName == "SearchText")
            {
                RaisePropertyChanged(nameof(SearchTextPresent));
            }
        }

      
    }
}
