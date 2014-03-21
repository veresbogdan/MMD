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
    public class BaseActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            this.OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_right);
        }

        public override void StartActivity(Intent intent)
        {
            base.StartActivity(intent);
            this.OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_right);
        }

        public override void Finish()
        {
            base.Finish();
            this.OverridePendingTransition(Resource.Animation.slide_in_right, Resource.Animation.slide_out_right);
        }
    }
}