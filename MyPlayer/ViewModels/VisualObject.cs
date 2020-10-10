﻿using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace MyPlayer.ViewModels
{
    public class VisualObject<T> : BindableObject where T : IMediaBase
    {
        private QueueViewModel _owner;
        protected int HeaderHeight { get; set; } = 40;
        protected int ItemHeight { get; set; } = 30;

        protected void Set<F>(ref F field, F value, [CallerMemberName] string propertyName = null)
        {
            if (field == null && value != null || field != null && !field.Equals(value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }


        protected bool _isSelected = true;
        public virtual bool IsSelected
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

        public virtual bool IsExpanded
        {
            get => _isExpanded;
            set => Set(ref _isExpanded, value);
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
                    Children = new List<VisualObject<IMediaBase>>(_data.Children.Select(child => new VisualObject<IMediaBase>(child, _owner)));
                }
            }
        }



        public string Name => Data?.Name;
        public int Height
        {
            get
            {
                var isSong = Children == null;
                if (isSong)
                {
                    return _owner.ShowSongs ? ItemHeight : 0;
                }
                else
                {
                    //return _owner.ShowAlbums ? (Data != null ? Data.Count * ItemHeight : 0) + HeaderHeight : 0;
                    return _owner.ShowAlbums ? Children.Sum(child => child.Height) + HeaderHeight : 0;
                }
            }
        }

        public VisualObject(T data, QueueViewModel owner)
        {
            _owner = owner;
            Data = data;
            _owner.PropertyChanged += _ownerPropertyChanged;
        }

        private void _ownerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TotalHeight")
            {
                //OnPropertyChanging(nameof(Height));
                OnPropertyChanged(nameof(Height));
            }
        }

        internal void DoPropertyChanged(string name)
        {
            OnPropertyChanged(name);
            if (Children != null)
            {
                foreach (var child in Children)
                {
                    child.OnPropertyChanged(name);
                }
            }
        }
    }
}
