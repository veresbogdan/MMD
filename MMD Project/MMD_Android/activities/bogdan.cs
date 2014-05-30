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
using Xamarin.Auth;
using System.Threading.Tasks;
using System.Json;

namespace MMD_Android.activities
{
    [Activity(Label = "Test screen Bogdan")]
    public class Bogdan : BaseActivity
    {
        void LoginToFacebook(bool allowCancel)
        {
            var auth = new OAuth2Authenticator(
                clientId: "635793476502695",
                scope: "",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

            auth.AllowCancel = allowCancel;
 
            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += (s, ee) =>
            {
                if (!ee.IsAuthenticated)
                {
                    var builder = new AlertDialog.Builder(this);
                    builder.SetMessage("Not Authenticated");
                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();
                    return;
                }

                // Now that we're logged in, make a OAuth2 request to get the user's info.
                var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, ee.Account);
                request.GetResponseAsync().ContinueWith(t =>
                {
                    var builder = new AlertDialog.Builder(this);
                    if (t.IsFaulted)
                    {
                        builder.SetTitle("Error");
                        builder.SetMessage(t.Exception.Flatten().InnerException.ToString());
                    }
                    else if (t.IsCanceled)
                        builder.SetTitle("Task Canceled");
                    else
                    {
                        var obj = JsonValue.Parse(t.Result.GetResponseText());

                        builder.SetTitle("Logged in");
                        builder.SetMessage("Name: " + obj["name"]);

                        //Store the account
                        AccountStore.Create(this).Save(ee.Account, "Facebook");

                        StartActivity(typeof(NewA));
                    }

                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();
                }, UIScheduler);
            };

            var intent = auth.GetUI(this);
            StartActivity(intent);
        }

        private static readonly TaskScheduler UIScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        protected override void OnCreate(Bundle bundle)
        {          
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Bogdan);

            var facebook = FindViewById<Button>(Resource.Id.FacebookButton);
            facebook.Click += delegate { LoginToFacebook(true); };

            var facebookNoCancel = FindViewById<Button>(Resource.Id.FacebookButtonNoCancel);
            facebookNoCancel.Click += delegate { LoginToFacebook(false); };

        }

        public override void StartActivity(Intent intent)
        {
            base.StartActivity(intent); 
        }
    }
}