using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MyPlayer.Controls
{
    public class UnderlineTrigger : TriggerAction<Entry>
    {
        public VisualElement Control { get; set; }
        public bool Value { get; set; }

        protected override void Invoke(Entry sender)
        {
            Control.IsVisible = !Value;
        }
    }
}
