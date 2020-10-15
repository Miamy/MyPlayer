using LibVLCSharp.Shared;
using MyPlayer.CommonClasses;
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
            set
            {
                Set(ref _current, value);
                if (_initialized && Current?.Container != null)
                {
                    //MediaPlayer.Media = null;
                    if (MediaPlayer.Media != null)
                    {
                        MediaPlayer.Media.ParsedChanged -= MediaPlayerMediaParsedChanged;
                    }
                    MediaPlayer.Media = new Media(LibVLC, Current.Container.FullPath, FromType.FromPath);
                    MediaPlayer.Media.ParsedChanged += MediaPlayerMediaParsedChanged;
                    MediaPlayer.Media.Parse();
                }
            }
        }


        public TimeSpan Length => MediaPlayer.Media.Duration == -1 ? TimeSpan.Zero : TimeSpan.FromMilliseconds(MediaPlayer.Media.Duration);

        private float _position = 0;
        public float Position
        {
            get => _position;
            set
            {
                Set(ref _position, value);
                MediaPlayer.Position = value;
            }
        }

        public TimeSpan PositionTS
        {
            get => MediaPlayer.Media.Duration == -1 ? TimeSpan.Zero : TimeSpan.FromMilliseconds(MediaPlayer.Position * MediaPlayer.Media.Duration);
        }

        public string MediaInfo
        {
            get
            {
                if (MediaPlayer == null)
                {
                    return "-";
                }
                var media = MediaPlayer.Media;
                if (media == null || !media.IsParsed || media.Tracks.Length == 0)
                {
                    return "-";
                }
                var track = media.Tracks[0];
                var bitrate = track.Bitrate == 0 ? "VBR" : $"{track.Bitrate / 1000} kB/s"; 
                return $"{bitrate}, {track.Data.Audio.Channels} ch, {track.Data.Audio.Rate} Hz";
            }
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
                return Current.Album?.Cover;
            }
        }

        public IQueueViewModel QueueViewModel { get; set; }

        private bool _isPlaying = false;

        public bool IsPlaying
        {
            get => MediaPlayer.IsPlaying;  //_isPlaying;
            set => Set(ref _isPlaying, value);
        }

        private LibVLC LibVLC { get; set; }

        private MediaPlayer _mediaPlayer;
        public MediaPlayer MediaPlayer
        {
            get => _mediaPlayer;
            private set => Set(ref _mediaPlayer, value);
        }

        private bool _initialized = false;

        public MainWindowViewModel()
        {
            CreateCommands();

            Settings.Instance.PropertyChanged += SettingsPropertyChanged;

            QueueViewModel = Storage.LoadQueue();
            if (QueueViewModel == null)
            {
                QueueViewModel = new QueueViewModel();
            }
            //Queue.PropertyChanged += PropertyChanged;

            Initialize();
            LoadMusicFolder();
            Current = QueueViewModel.GetDefault();
        }


        private void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("RootFolder"))
            {
                LoadMusicFolder();
            }
        }

        private void LoadMusicFolder()
        {
            try
            {
                var root = Settings.Instance.RootFolder;
                if (string.IsNullOrWhiteSpace(root))
                {
                    root = @"/storage/emulated/0/Music/";
                    //root = @"/storage/2743-1D07/Music/";
                }
                QueueViewModel.AddFromRoot(root);
            }
            catch (UnauthorizedAccessException)
            {
                //throw;
            }
            catch (FileNotFoundException)
            {
                //throw;
            }
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
            QueueViewModel.SwitchLoopType();
        }
        private bool CanPrev(object arg)
        {
            return true;
        }

        private void PrevAction(object obj)
        {
            Current = QueueViewModel.Prev(Current);
            PlayCurrent();
        }

        private bool CanNext(object arg)
        {
            return true;
        }

        private void NextAction(object obj)
        {
            Current = QueueViewModel.Next(Current);
            PlayCurrent();
        }

        private bool CanShowQueue(object arg)
        {
            return true;
        }

        private async void ShowQueueAction(object obj)
        {
            MessagingCenter.Subscribe(this, "SongSelected", (BaseViewModel sender, IMediaBase song) =>
            {
                MessagingCenter.Unsubscribe<BaseViewModel, IMediaBase>(this, "SongSelected");
                Current = (ISong)song;
            });

            var page = new QueuePage(QueueViewModel);
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

            if (IsPlaying)
            {
                MediaPlayer.Pause();
            }
            else
            {
                PlayCurrent();
            }
            IsPlaying = !IsPlaying;
        }
        #endregion


        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            Core.Initialize();
            LibVLC = new LibVLC();
            MediaPlayer = new MediaPlayer(LibVLC);
            MediaPlayer.MediaChanged += MediaPlayerMediaChanged;
            MediaPlayer.PositionChanged += MediaPlayerPositionChanged;
            _initialized = true;
        }

        private void MediaPlayerMediaParsedChanged(object sender, MediaParsedChangedEventArgs e)
        {
            RaisePropertyChanged("Length");
            RaisePropertyChanged("MediaInfo");
        }

        private TimeSpan lastPosition = TimeSpan.Zero;
        private void MediaPlayerPositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            //Position = (long)(MediaPlayer.Length * e.Position);
            Position = e.Position;
            if ((PositionTS - lastPosition).Milliseconds >= 200)
            {
                RaisePropertyChanged("PositionTS");
            }
            lastPosition = PositionTS;
        }

        private void MediaPlayerMediaChanged(object sender, MediaPlayerMediaChangedEventArgs e)
        {
           
        }

        private void PlayCurrent()
        {
            if (Current?.Container == null)
            {
                return;
            }
            if (!File.Exists(Current.Container.FullPath))
            {
                return;
            }
            if (MediaPlayer.Media == null)
            {
                return;
            }

            MediaPlayer.Play();
        }
    }
}
