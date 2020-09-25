using MyPlayer.Models.Interfaces;
using MyPlayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyPlayer.Views
{
    public partial class QueuePage : ContentPage
    {
        private readonly QueueViewModel _model;

        public QueuePage(IQueue queue)
        {
            InitializeComponent();
            _model = new QueueViewModel(queue);
            BindingContext = _model;
        }

    }
}