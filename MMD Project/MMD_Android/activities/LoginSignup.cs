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
    [Activity(Label = "MMD_Android", MainLauncher = true, Icon = "@drawable/icon")]
    class LoginSignup : BaseActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.LoginSignup);

            Button login = FindViewById<Button>(Resource.Id.button_Login);
            login.Click += LoginButtonClick;

            Button signup = FindViewById<Button>(Resource.Id.button_Signup);
            signup.Click += SignupButtonClick;
        }

        void LoginButtonClick(object sender, EventArgs e)
        {
            //option 1
            //var taskDetails = new Intent(this, typeof(TaskDetailsScreen));
            //taskDetails.PutExtra("TaskID", tasks[e.Position].ID);
            //StartActivity(taskDetails);

            //option 2
            StartActivity(typeof(Home));
        }

        void SignupButtonClick(object sender, EventArgs e)
        {
            StartActivity(typeof(Home));
        }
    }
}