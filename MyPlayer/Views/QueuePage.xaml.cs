using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPlayer.CommonClasses;
using MyPlayer.Models.Classes;
using MyPlayer.Models.Interfaces;
using MyPlayer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyPlayer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QueuePage : ContentPage
    {
        private readonly IQueueViewModel _model;
        private readonly IStorage _storage;
        public QueuePage(IQueue queue, IStorage storage)
        {
            InitializeComponent();

            _storage = storage ?? throw new ArgumentNullException("storage");
            if (queue == null)
            {
                throw new ArgumentNullException("queue");
            }
            _model = new QueueViewModel(queue);
            _model.PropertyChanged += ModelPropertyChanged;

            BuildTree();
            BindingContext = _model;
        }

        private const string Tag = "Children";
        private void BuildTree()
        {
            ParentLayout.BatchBegin();
            try
            {
                ParentLayout.Children.Clear();
                if (_model.Artists == null)
                {
                    return;
                }

                foreach (var artist in _model.Artists)
                {
                    var artistLayout = LayoutSelector(artist);
                    artistLayout.IsVisible = artist.SearchTextPresent;

                    var childContainer = GetContainer(artistLayout);
                    foreach (var album in artist.Children)
                    {
                        var albumLayout = LayoutSelector(album);
                        albumLayout.IsVisible = artist.IsExpanded && album.SearchTextPresent;

                        FillChildren(album, albumLayout);

                        childContainer.Children.Add(albumLayout);
                    }
                    ParentLayout.Children.Add(artistLayout);
                }
            }
            finally
            {
                ParentLayout.BatchCommit();
            }
        }

        private void FillChildren(VisualObject<IMediaBase> data, StackLayout parent)
        {
            if (data.IsExpanded || _model.SearchIsNotEmpty)
            {
                foreach (var child in data.Children)
                {
                    var childLayout = LayoutSelector(child);
                    childLayout.IsVisible = child.SearchTextPresent;

                    parent.Children.Add(childLayout);
                }
            }
        }

        private StackLayout CreateWholeStackLayout(VisualObject<IMediaBase> data, Color color, int horizontalOffcet, int verticalOffcet, int fontSize)
        {
            var layout = CreateCommonStackLayout(data, verticalOffcet);

            var title = CreateTitleStackLayout();

            var checkbox = CreateCheckBox(data, color, horizontalOffcet);
            title.Children.Add(checkbox);

            if (data.Data.HasChildren)
            {
                var image = CreateImage(data);
                title.Children.Add(image);
            }

            var label = CreateNameLabel(data, color, fontSize);
            title.Children.Add(label);

            label = CreateDurationLabel(data, color, fontSize);
            title.Children.Add(label);

            layout.Children.Add(title);

            var childrenLayout = CreateChildrenStackLayout();
            layout.Children.Add(childrenLayout);

            return layout;
        }

        private StackLayout CreateCommonStackLayout(VisualObject<IMediaBase> data, int offcet)
        {
            var layout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Margin = new Thickness(0, offcet, 0, 0),
                BindingContext = data
            };
            return layout;
        }

        private StackLayout CreateChildrenStackLayout()
        {
            var layout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Margin = new Thickness(0, 0, 0, 0),
                AutomationId = Tag
            };
            return layout;
        }

        private StackLayout CreateTitleStackLayout()
        {
            var layout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HeightRequest = 28
            };
            return layout;
        }

        private CheckBox CreateCheckBox(VisualObject<IMediaBase> data, Color color, int offcet)
        {
            var checkbox = new CheckBox()
            {
                Margin = new Thickness(2, 0, offcet, 0),
                IsChecked = data.Data.IsSelected,
                BindingContext = data,
                Color = color,
            };
            checkbox.CheckedChanged += (s, e) =>
            {
                var checkbox = (CheckBox)s;
                var data = (VisualObject<IMediaBase>)checkbox.BindingContext;
                data.Data.IsSelected = !data.Data.IsSelected;
            };

            return checkbox;
        }
        private Image CreateImage(VisualObject<IMediaBase> data)
        {
            string GetSource()
            {
                return data.IsExpanded ? "expanded.png" : "collapsed.png";
            }

            var image = new Image()
            {
                Source = GetSource(),
                Margin = new Thickness(12, 0, 8, 0),
                Aspect = Aspect.AspectFit,
                HeightRequest = 20,
                WidthRequest = 20,
                BindingContext = data,
            };
            var gesture = new TapGestureRecognizer()
            {
                NumberOfTapsRequired = 1,
            };
            gesture.Tapped += (s, e) =>
            {
                var image = (Image)s;
                var data = (VisualObject<IMediaBase>)image.BindingContext;
                data.IsExpanded = !data.IsExpanded;
                ExpandNode(ParentLayout, data);
                image.Source = GetSource();
            };
            image.GestureRecognizers.Add(gesture);

            return image;
        }



        private Label CreateNameLabel(VisualObject<IMediaBase> data, Color color, double size)
        {
            var label = new Label()
            {
                Text = data.Name,
                FontSize = size,
                TextColor = color,
            };

            var gesture = new TapGestureRecognizer()
            {
                NumberOfTapsRequired = 1
            };

            gesture.Command = _model.PlayTappedCommand;
            gesture.CommandParameter = data;
            label.GestureRecognizers.Add(gesture);

            return label;
        }


        private Label CreateDurationLabel(VisualObject<IMediaBase> data, Color color, double size)
        {
            var label = new Label()
            {
                Text = data.Data.Duration.ToString("hh\\:mm\\:ss"),
                FontSize = size,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                WidthRequest = 100,
                HorizontalTextAlignment = TextAlignment.End,
                TextColor = color,
            };

            return label;
        }

        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ExpandAlbums" || e.PropertyName == "ExpandSongs" || e.PropertyName == "SearchText")
            {
                BuildTree();
            }
            if (e.PropertyName == "AllSelected")
            {
                ParentLayout.BatchBegin();
                try
                {
                    SelectTree(ParentLayout, _model.AllSelected);
                }
                finally
                {
                    ParentLayout.BatchCommit();
                }
            }
        }

        private void SelectTree(Layout parent, bool selected)
        {
            foreach (var child in parent.Children)
            {
                if (child is CheckBox checkBox)
                {
                    checkBox.IsChecked = selected;
                }
                else if (child is Layout layout)
                {
                    SelectTree(layout, selected);
                }
            }
        }

        private void ExpandNode(Layout parent, VisualObject<IMediaBase> data)
        {
            foreach (var child in parent.Children)
            {
                if (child is StackLayout layout)
                {
                    if (layout.BindingContext == data)
                    {
                        var childContainer = GetContainer(layout);
                        childContainer.Children.Clear();

                        FillChildren(data, childContainer);
                        return;
                    }
                    else
                    {
                        ExpandNode(layout, data);
                    }
                }
            }
        }

        protected override async void OnDisappearing()
        {
            await Task.Run(() => _storage.SaveQueue(_model.Queue));
            base.OnDisappearing();
        }

        private StackLayout CreateArtistStackLayout(VisualObject<IMediaBase> data)
        {
            return CreateWholeStackLayout(data, Color.Red, 0, 6, 14);
        }
        private StackLayout CreateAlbumStackLayout(VisualObject<IMediaBase> data)
        {
            return CreateWholeStackLayout(data, Color.Orange, 20, 0, 13);
        }
        private StackLayout CreateSongStackLayout(VisualObject<IMediaBase> data)
        {
            return CreateWholeStackLayout(data, Color.Yellow, 60, 0, 12);
        }

        private StackLayout LayoutSelector(VisualObject<IMediaBase> data)
        {
            return data.Data switch
            {
                Artist _ => CreateArtistStackLayout(data),
                Album _ => CreateAlbumStackLayout(data),
                _ => CreateSongStackLayout(data),
            };
        }

        private StackLayout GetContainer(StackLayout layout)
        {
            return (StackLayout)layout.Children.First(child => child.AutomationId == Tag);
        }
    }
}