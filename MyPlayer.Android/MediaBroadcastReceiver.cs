using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPlayer.Droid
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { "android.intent.action.MEDIA_BUTTON", "android.intent.action.MEDIA_SEARCH"/*, "android.intent.action.MUSIC_PLAYER"*/ })]
    public class MediaBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            //throw new NotImplementedException();
        }
    }
}