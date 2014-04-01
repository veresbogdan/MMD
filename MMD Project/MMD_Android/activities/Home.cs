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
    [Activity(Label = "My Activity")]
    public class Home : BaseActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			String extra = Intent.GetStringExtra ("User") ?? "Data not available";

            // Create your application here
			TextView textView = new TextView(this);
			textView.TextSize = 30;
			textView.Text = extra;

			// Set the text view as the activity layout
			SetContentView(textView);
			//SetContentView(Resource.Layout.Home);
        }
    }
}