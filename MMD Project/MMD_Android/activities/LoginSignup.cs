using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MMD_Core_v2;
<<<<<<< HEAD
using MMD_Core_v2.Services;
=======
using Newtonsoft.Json;
using Xamarin.Auth;
>>>>>>> 0441dec70c41243ae670420d7e20f4e1e8296345

namespace MMD_Android.activities
{
    [Activity(Label = "MMD_Android", MainLauncher = true, Icon = "@drawable/icon")]
    class LoginSignup : BaseActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.LoginSignup);

            Button login = FindViewById<Button>(Resource.Id.button_Login);
            login.Click += LoginButtonClick;

            Button signup = FindViewById<Button>(Resource.Id.button_Signup);
            signup.Click += SignupButtonClick;
        }

        void LoginButtonClick(object sender, EventArgs e)
        {
            UserService service = new UserService();

<<<<<<< HEAD
            var email = FindViewById<EditText>(Resource.Id.email_input_field);
            var password = FindViewById<EditText>(Resource.Id.password_input_field);

            User logedUser = service.loginUser(email.Text, password.Text);

            //option 1
            var taskDetails = new Intent(this, typeof(Home));
            taskDetails.PutExtra("User", logedUser.ToString());
            StartActivity(taskDetails);

            //option 2
            //StartActivity(typeof(Home));
=======
			var emailField = FindViewById<EditText> (Resource.Id.email_input_field);
			var passwordField = FindViewById<EditText> (Resource.Id.password_input_field);

			User logedUser = service.loginUser(emailField.Text, passwordField.Text);

            if (logedUser != null && logedUser.Token != null)
            {
                var account = new Account();
                account.Username = logedUser.Nickname;
                account.Properties.Add("auth_token", logedUser.Token);

                AccountStore.Create(this).Save(account, "MMD");

                var homeActivity = new Intent(this, typeof(Home));
                homeActivity.PutExtra("User", JsonConvert.SerializeObject(logedUser));
                StartActivity(homeActivity);
            }
            else
            {
                var builder = new AlertDialog.Builder(this);
                builder.SetTitle("Login failed");
                builder.SetMessage("Wrong credentials");
                builder.SetPositiveButton("Ok", delegate {});
                builder.Create().Show();
            }
>>>>>>> 0441dec70c41243ae670420d7e20f4e1e8296345
        }

        void SignupButtonClick(object sender, EventArgs e)
        {
            //TODO : uncomment the following line
            //StartActivity(typeof(Home));
            StartActivity(typeof(Miki));
        }
    }
}