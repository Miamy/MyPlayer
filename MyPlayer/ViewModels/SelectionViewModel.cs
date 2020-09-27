using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlayer.ViewModels
{
    public class SelectionViewModel : BaseViewModel
    {
        private bool _isSelected = true;
        public virtual bool IsSelected
        {
            get => _isSelected;
            set => Set(nameof(IsSelected), ref _isSelected, value);
        }
    }
}
