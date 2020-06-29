﻿using Android.App;
using Android.Widget;
using Android.OS;
using AndroidX.AppCompat.App;
using Com.Braintreepayments.Api.Interfaces;

namespace BraintreeQs
{
    [Activity(Label = "BraintreeQs", MainLauncher = true, Icon = "@mipmap/ic_launcher", Theme = "@style/MyTheme")]
    public class MainActivity : AppCompatActivity
    {
        int count = 1;
        IThreeDSecurePrepareLookupListener x;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += delegate { button.Text = $"{count++} clicks!"; };
        }
    }
}

