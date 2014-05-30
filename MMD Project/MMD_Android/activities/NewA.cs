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

namespace MMD_Android.activities
{
    [Activity(Label = "My Activity")]
    public class NewA : BaseActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.NewA);

            IEnumerable<Account> accounts = AccountStore.Create(this).FindAccountsForService("Facebook");

            var builder = new AlertDialog.Builder(this);
            String message = "Properties: ";
            builder.SetTitle("Saved Logged in:");

            foreach (Account a in accounts) 
            {
                foreach (String s in a.Properties.Keys)
                {
                    message += s + ": " + a.Properties[s] + "\n";
                }     
            }

            builder.SetMessage(message);
            builder.SetPositiveButton("Ok", (o, e) => { });
            builder.Create().Show();
        }
    }
}