using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MMD_Android.activities
{
    [Activity(Label = "Homescreen")]
    public class Home : BaseActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            String extra = Intent.GetStringExtra("User") ?? "Data not available";

            // Create your application here
            SetContentView(Resource.Layout.Home);

            TextView textView = FindViewById<TextView>(Resource.Id.textView_Username);
            textView.Text = extra;

            Button goToBogdan = FindViewById<Button>(Resource.Id.button_Veres);
            Button goToMiki = FindViewById<Button>(Resource.Id.button_Miki);

            goToBogdan.Click += (sender, args) =>
            {
                var bogdan = new Intent(this, typeof(Bogdan));
                StartActivity(bogdan);
            };

            goToMiki.Click += (sender, args) =>
            {
                var miki = new Intent(this, typeof(Miki));
                StartActivity(miki);
            };
        }
    }
}
