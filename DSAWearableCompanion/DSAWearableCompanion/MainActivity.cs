using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.Wearable.Views;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.Wearable.Activity;
using Java.Interop;
using Android.Views.Animations;
using DSAWearableCompanion.Storage;

namespace DSAWearableCompanion
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : WearableActivity
    {
        TextView textView;
        AppStorage Storage;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main);

            Storage = new AppStorage(this);

            textView = FindViewById<TextView>(Resource.Id.text);
            SetAmbientEnabled();
        }
    }
}


