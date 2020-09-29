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

        public ObservableCollection<IPathElement> Data { get; set; }

        private IPathElement _selectedItem;
        public IPathElement SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;

                if (_selectedItem == null)
                    return;

                OpenFolderCommand.Execute(_selectedItem);

                //_selectedItem = null;
            }
        }
        public ICommand OpenFolderCommand { get; set; }
        public ICommand GotoRootCommand { get; set; }
        public ICommand GotoParentCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand SelectFolderCommand { get; set; }
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
            Data.Clear();
            var storages = Environment.GetLogicalDrives().Where(drive => drive.IndexOf("storage") > 0).ToList();
            if (storages.Count == 0)
            {
                return;
            }

            Data.Add(new PathElement("Internal storage", storages[0] + "/0"));
            if (storages.Count > 1)
            {
                Data.Add(new PathElement("SD card", storages[1]));
            }
        }

        #region Commands
        private void CreateCommands()
        {
            RefreshCommand = new Command(RefreshAction);
            OpenFolderCommand = new Command(OpenFolderAction);
            GotoRootCommand = new Command(GotoRootAction, CanGotoRoot);
            GotoParentCommand = new Command(GotoParentAction, CanGotoParent);
            SelectFolderCommand = new Command(SelectFolderAction, CanSelectFolder);
        }

        private bool CanSelectFolder(object arg)
        {
            return SelectedItem != null && !SelectedItem.IsVirtual;
        }

        private async void SelectFolderAction(object obj)
        {
            var settings = new SettingsViewModel();
            settings.RootFolder = SelectedItem.FullPath;
            settings.Save();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private bool CanGotoParent(object arg)
        {
            return SelectedItem?.Parent != null;
        }

        private void GotoParentAction(object obj)
        {
            SelectedItem = SelectedItem.Parent;
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

    
        private void OpenFolderAction(object obj)
        {
            IsRoot = false;

            var root = obj as PathElement;
            if (root.IsFile)
            {
                return;            
            }
            _selectedItem = root;
            Run(() => ((Command)SelectFolderCommand).ChangeCanExecute());

            var path = root.FullPath;
            var options = new EnumerationOptions { IgnoreInaccessible = true };

            Data.Clear();

            var data = Directory.GetDirectories(path, "*", options).OrderBy(item => item);
            foreach (var item in data)
            {
                Data.Add(new PathElement(root, item));
            }

            data = Directory.GetFiles(path, "*.*", options).OrderBy(item => item);
            foreach (var item in data)
            {
                Data.Add(new PathElement(root, item));
            }
        }
        #endregion


    }
}
