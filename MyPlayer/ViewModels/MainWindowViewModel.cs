﻿using LibVLCSharp.Shared;
using MyPlayer.Models.Classes;
using MyPlayer.Models.Interfaces;
using MyPlayer.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MyPlayer.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private ISong _current;
        public ISong Current
        { 
            get => _current;
            set => Set(ref _current, value);
        }
        public ICommand ShowQueueCommand { get; set; }
        public ICommand ShowSettingsCommand { get; set; }
        public ICommand PlayCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand PrevCommand { get; set; }
        public ICommand LoopCommand { get; set; }

        public IPathElement Cover
        {
            get
            {
                if (Current == null)
                {
                    return null;
                }
                return Current.MediaInfo.Cover;
            }
        }

        public IQueue Queue { get; set; }

        private bool _isPlaying = false;

        public bool IsPlaying
        {
            get => _isPlaying;
            set => Set(ref _isPlaying, value);
        }

        private LibVLC LibVLC { get; set; }

        private MediaPlayer _mediaPlayer;
        public MediaPlayer MediaPlayer
        {
            get => _mediaPlayer;
            private set => Set(ref _mediaPlayer, value);
        }

        public MainWindowViewModel()
        {
            CreateCommands();

            Queue = new Queue();
            //Queue.PropertyChanged += PropertyChanged;

            try
            {
                var files = PathScanner.ProceedRoot(@"/storage/emulated/0/Music/");
                //var files = PathScanner.ProceedRoot(@"/storage/2743-1D07/Music/");
                Queue.AddRange2(files);
            }
            catch (UnauthorizedAccessException)
            {
                //throw;
            }
            catch (FileNotFoundException)
            {
                //throw;
            }

            Current = Queue.GetDefault();

            Initialize();
        }




        #region Commands
        private void CreateCommands()
        {
            ShowQueueCommand = new Command(ShowQueueAction, CanShowQueue);
            ShowSettingsCommand = new Command(ShowSettingsAction, CanShowSettings);
            PlayCommand = new Command(PlayAction, CanPlay);
            NextCommand = new Command(NextAction, CanNext);
            PrevCommand = new Command(PrevAction, CanPrev);
            LoopCommand = new Command(LoopAction);
        }

        private void LoopAction(object obj)
        {
            Queue.SwitchLoopType();
        }
        private bool CanPrev(object arg)
        {
            return true;
        }

        private void PrevAction(object obj)
        {
            Current = Queue.Prev(Current);
        }

        private bool CanNext(object arg)
        {
            return true;
        }

        private void NextAction(object obj)
        {
            Current = Queue.Next(Current);
        }

        private bool CanShowQueue(object arg)
        {
            return true;
        }

        private async void ShowQueueAction(object obj)
        {
            var page = new QueuePage(Queue);
            await Application.Current.MainPage.Navigation.PushAsync(page, false);
        }
        private bool CanShowSettings(object arg)
        {
            return true;
        }

        private async void ShowSettingsAction(object obj)
        {
            var page = new SettingsPage();
            await Application.Current.MainPage.Navigation.PushAsync(page, false);
        }

        private bool CanPlay(object arg)
        {
            return true;
        }


        private void PlayAction(object obj)
        {
            if (!File.Exists(Current.Container.FullPath))
            {
                return;
            }

            IsPlaying = !IsPlaying;
            if (IsPlaying)
            {
                //Queue.Play();
                    MediaPlayer.Play();
            }
            else
            {
                //Queue.Pause();
                MediaPlayer.Pause();
            }
        }
        #endregion

 
        private void Initialize()
        {
            Core.Initialize();

            LibVLC = new LibVLC();
            MediaPlayer = new MediaPlayer(LibVLC)
            {
                Media = new Media(LibVLC, Current.Container.FullPath, FromType.FromPath)
            };
        }      
    }
}
