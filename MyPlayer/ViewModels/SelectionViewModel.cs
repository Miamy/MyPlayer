using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.ViewModels
{
    public class SelectionViewModel : BaseViewModel
    {
        protected bool _isSelected = true;
        public virtual bool IsSelected
        {
            get => _isSelected;
            set => Set(ref _isSelected, value);
        }

        protected bool _isExpanded = true;
        public virtual bool IsExpanded
        {
            get => _isExpanded;
            set => Set(ref _isExpanded, value);
        }
    }
}
