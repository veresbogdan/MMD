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
using MMD_Core_v2;

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
			UserService service = new UserService ();

			var email = FindViewById<EditText> (Resource.Id.email_input_field);
			var password = FindViewById<EditText> (Resource.Id.password_input_field);

			User logedUser = service.loginUser (email.Text, password.Text);

            //option 1
			var taskDetails = new Intent(this, typeof(Home));
			taskDetails.PutExtra("User", logedUser.ToString());
            StartActivity(taskDetails);

            //option 2
			//StartActivity(typeof(Home));
        }

        void SignupButtonClick(object sender, EventArgs e)
        {
            StartActivity(typeof(Home));
        }
    }
}