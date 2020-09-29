using MyPlayer.Models.Classes;
using MyPlayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyPlayer.ViewModels
{
    public class BrowseViewModel : BaseViewModel
    {
        public IPathElement Current { get; set; }

        private bool _isRoot = true;
        public bool IsRoot
        {
            get => _isRoot;
            set
            {
                Set(ref _isRoot, value);
                RaisePropertyChanged("IsNotRoot");
                Run(() => ((Command)GotoRootCommand).ChangeCanExecute());
            }
        }

        public bool IsNotRoot => !IsRoot;

        public List<RootInfo> RootFolders { get; set; }
        public ObservableCollection<IPathElement> Data { get; set; }

        private object _selectedItem;
        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;

                if (_selectedItem == null)
                    return;

                OpenCardCommand.Execute(_selectedItem);

                SelectedItem = null;
            }
        }
        public ICommand OpenCardCommand { get; set; }
        public ICommand OpenFolderCommand { get; set; }
        public ICommand GotoRootCommand { get; set; }
        public ICommand GotoParentCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        public BrowseViewModel()
        {
            Data = new ObservableCollection<IPathElement>();
            BuildRoot();
            CreateCommands();
        }

        private void Run(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }
        private void BuildRoot()
        {
            RootFolders = new List<RootInfo>
            {
                new RootInfo("Internal storage", "")
            };

            var storages = Environment.GetLogicalDrives().Where(drive => drive.IndexOf("storage") > 0).ToList();
            if (storages.Count == 0)
            {
                return;
            }
            RootFolders[0].Path = storages[0];
            if (storages.Count > 1)
            {
                RootFolders.Add(new RootInfo("SD card", storages[1]));
            }
        }

        #region Commands
        private void CreateCommands()
        {
            RefreshCommand = new Command(RefreshAction);
            OpenCardCommand = new Command(OpenCardAction);
            OpenFolderCommand = new Command(OpenFolderAction);
            GotoRootCommand = new Command(GotoRootAction, CanGotoRoot);
            GotoParentCommand = new Command(GotoParentAction, CanGotoParent);
        }

        private bool CanGotoParent(object arg)
        {
            return true;
        }

        private void GotoParentAction(object obj)
        {
            throw new NotImplementedException();
        }

        private void OpenFolderAction(object obj)
        {
            throw new NotImplementedException();
        }

        private void RefreshAction(object obj)
        {
            throw new NotImplementedException();
        }

        private void GotoRootAction(object obj)
        {
            IsRoot = true;
        }

        private bool CanGotoRoot(object arg)
        {
            return IsNotRoot;
        }

        private void OpenCardAction(object obj)
        {
            IsRoot = false;

            var root = obj as RootInfo;
            var path = root.Path + "/0/";
            var options = new EnumerationOptions { IgnoreInaccessible = true };
            
            var data = Directory.GetDirectories(path, "*", options).OrderBy(item => item);
            foreach (var item in data)
            {
                Data.Add(new PathElement(item));
            }

            data = Directory.GetFiles(path, "*.*", options).OrderBy(item => item);
            foreach (var item in data)
            {
                Data.Add(new PathElement(item));
            }
        }

        #endregion


    }
}
