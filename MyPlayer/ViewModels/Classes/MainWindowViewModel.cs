using LibVLCSharp.Shared;
using MyPlayer.CommonClasses;
using MyPlayer.Models;
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
    public class MainWindowViewModel : BaseModel
    {
        private ISong _current;
        public ISong Current
        {
            get => _current;
            set
            {
                Set(ref _current, value);
                Cover = "";
                if (_initialized)
                {
                    if (MediaPlayer?.Media != null)
                    {
                        MediaPlayer.Media.ParsedChanged -= MediaPlayerMediaParsedChanged;
                        MediaPlayer.Media.StateChanged -= MediaPlayerMediaStateChanged;
                        MediaPlayer.Media.Dispose();
                    }
                    MediaPlayer = null;

                    if (Current?.Container != null)
                    {
                        CreatePlayer();

                        MediaPlayer.Media = new Media(LibVLC, Current.Container, FromType.FromPath);
                        MediaPlayer.Media.ParsedChanged += MediaPlayerMediaParsedChanged;
                        MediaPlayer.Media.Parse();
                        MediaPlayer.Media.StateChanged += MediaPlayerMediaStateChanged;

                        if (Current.Album.Covers.Count > 0)
                        {
                            Cover = Current.Album.Covers[0];
                        }
                    }
                }
            }
        }


        public TimeSpan Length
        {
            get
            {
                if (MediaPlayer == null)
                {
                    return TimeSpan.Zero;
                }
                var media = MediaPlayer.Media;
                if (media == null)
                {
                    return TimeSpan.Zero;
                }
                return media.Duration == -1 ? TimeSpan.Zero : TimeSpan.FromMilliseconds(media.Duration);
            }
        }

        private float _position = 0;
        public float Position
        {
            get => _position;
            set
            {
                Set(ref _position, value);
                if (MediaPlayer != null && Math.Abs(MediaPlayer.Position - value) > 1e-5)
                {
                    MediaPlayer.Position = value;
                }
            }
        }

        public TimeSpan PositionTS
        {
            get
            {
                if (MediaPlayer == null)
                {
                    return TimeSpan.Zero;
                }
                var media = MediaPlayer.Media;
                if (media == null)
                {
                    return TimeSpan.Zero;
                }
                return media.Duration == -1 ? TimeSpan.Zero : TimeSpan.FromMilliseconds(MediaPlayer.Position * media.Duration);
            }
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
        public ICommand NextCoverCommand { get; set; }
        public ICommand PreviousCoverCommand { get; set; }
        public ICommand ShowLyricsCommand { get; set; }

        public IQueue Queue { get; set; }
        private IStorage Storage { get; set; }
        private ISettings Settings { get; set; }

        //public bool IsPlaying => MediaPlayer.Media?.State == VLCState.Playing;  

        private bool _isPlaying = false;
        public bool IsPlaying
        {
            get => _isPlaying;
            set => Set(ref _isPlaying, value);
        }
        public LoopType LoopType => Queue.LoopType;

        private LibVLC LibVLC { get; set; }

        private MediaPlayer _mediaPlayer;
        public MediaPlayer MediaPlayer
        {
            get => _mediaPlayer;
            private set => Set(ref _mediaPlayer, value);
        }

        private bool _initialized = false;

        private string _cover;
        public string Cover
        {
            get => _cover;
            set => Set(ref _cover, value);
        }

        private bool _showLyrics;
        public bool ShowLyrics 
        { 
            get => _showLyrics;
            set
            {
                Set(ref _showLyrics, value);
                RaisePropertyChanged(nameof(LyricsImage));
            }
        }
        public string LyricsImage => ShowLyrics ? "description_grey_36x36.png" : "description_white_36x36.png";

        public MainWindowViewModel()
        {
            Initialize();

            CreateCommands();

            Settings = new Settings();
            Settings.PropertyChanged += SettingsPropertyChanged;

            Queue = new Queue();
            Queue.PropertyChanged += QueuePropertyChanged;

            Storage = new Storage();
            Storage.LoadQueue(Queue);
            Storage.LoadSettings(Settings);

        }

        private void QueuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Current"))
            {
                Current = Queue.Current;
            }
        }

        private void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("RootFolder"))
            {
                Storage.SaveSettings(Settings);
                LoadMusicFolder();
                if (IsPlaying)
                {
                    PlayCurrent(true);
                }
            }
        }

        private void LoadMusicFolder()
        {
            try
            {
                var root = Settings.RootFolder;
                if (string.IsNullOrWhiteSpace(root))
                {
                    root = @"/storage/emulated/0/Music/";
                    //root = @"/storage/2743-1D07/Music/";
                }
                Queue.Fill(root);

                Current = Queue.Current;
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
            NextCoverCommand = new Command(NextCoverAction);
            PreviousCoverCommand = new Command(PreviousCoverAction);
            ShowLyricsCommand = new Command(ShowLyricsAction);
        }

        private void ShowLyricsAction(object obj)
        {
            ShowLyrics = !ShowLyrics;
        }

        private void PreviousCoverAction(object obj)
        {
            if (Current?.Album.Covers.Count == 0)
            {
                return;
            }

            var cover = Current.Album.Covers.Previous(Cover);
            if (cover == null)
            {
                cover = Current.Album.Covers.Last();
            }
            Cover = cover;
        }

        private void NextCoverAction(object obj)
        {
            if (Current?.Album.Covers.Count == 0)
            {
                return;
            }

            var cover = Current.Album.Covers.Next(Cover);
            if (cover == null)
            {
                cover = Current.Album.Covers.First();
            }
            Cover = cover;
        }

        private void LoopAction(object obj)
        {
            Queue.SwitchLoopType();
            RaisePropertyChanged(nameof(LoopType));
        }
        private bool CanPrev(object arg)
        {
            return true;
        }

        private void PrevAction(object obj)
        {
            Current = Queue.Prev(Current);
            if (IsPlaying)
            {
                PlayCurrent(true);
            }
        }

        private bool CanNext(object arg)
        {
            return true;
        }

        private void NextAction(object obj)
        {
            Current = Queue.Next(Current);
            if (IsPlaying)
            {
                PlayCurrent(true);
            }
        }

        private bool CanShowQueue(object arg)
        {
            return true;
        }

        private async void ShowQueueAction(object obj)
        {
            MessagingCenter.Subscribe(this, "SongSelected", (BaseModel sender, ISong song) =>
            {
                MessagingCenter.Unsubscribe<BaseModel, ISong>(this, "SongSelected");
                Current = song;
                PlayCurrent(true);
            });

            var page = await QueuePage.CreateQueuePage(Queue, Storage);
            await App.Navigation.PushAsync(page, false);
        }
        private bool CanShowSettings(object arg)
        {
            return true;
        }

        private async void ShowSettingsAction(object obj)
        {
            var page = new SettingsPage(Settings);
            await App.Navigation.PushAsync(page, false);
        }

        private bool CanPlay(object arg)
        {
            return true;
        }


        private void PlayAction(object obj)
        {
            if (IsPlaying)
            {
                MediaPlayer?.Pause();
            }
            else
            {
                PlayCurrent(false);
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
            _initialized = true;
        }

        private void CreatePlayer()
        {
            MediaPlayer = new MediaPlayer(LibVLC);
            MediaPlayer.MediaChanged += MediaPlayerMediaChanged;
            MediaPlayer.PositionChanged += MediaPlayerPositionChanged;
            //MediaPlayer.Stopped += MediaPlayerStopped;
            MediaPlayer.EndReached += MediaPlayerEndReached;
        }

        private void MediaPlayerMediaStateChanged(object sender, MediaStateChangedEventArgs e)
        {
            //RaisePropertyChanged(nameof(IsPlaying));
        }


        private void MediaPlayerEndReached(object sender, EventArgs e)
        {
            NextAction(null);
        }

        private void MediaPlayerStopped(object sender, EventArgs e)
        {

        }

        private void MediaPlayerMediaParsedChanged(object sender, MediaParsedChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Length));
            RaisePropertyChanged(nameof(MediaInfo));
        }

        private TimeSpan lastPosition = TimeSpan.Zero;

        private void MediaPlayerPositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            _position = e.Position;
            if ((PositionTS - lastPosition).Milliseconds >= 200)
            {
                RaisePropertyChanged(nameof(PositionTS));
                RaisePropertyChanged(nameof(Position));
            }
            lastPosition = PositionTS;
        }

        private void MediaPlayerMediaChanged(object sender, MediaPlayerMediaChangedEventArgs e)
        {

        }

        private void PlayCurrent(bool resetPosition)
        {
            if (Current?.Container == null)
            {
                return;
            }
            if (!File.Exists(Current.Container))
            {
                return;
            }
            if (resetPosition)
            {
                Position = 0;
            }

            MediaPlayer.Play();
            //RaisePropertyChanged(nameof(IsPlaying));
        }
    }
}
