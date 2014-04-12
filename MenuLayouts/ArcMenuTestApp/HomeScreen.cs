using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using arcmenulib.elements;

namespace ArcMenuTestApp
{
    [Activity(Label = "ArcMenuTestApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class HomeScreen : Activity
    {
        private static int[] ITEM_DRAWABLES = { Resource.Drawable.composer_camera, Resource.Drawable.composer_music,
            Resource.Drawable.composer_place, Resource.Drawable.composer_sleep, Resource.Drawable.composer_thought, Resource.Drawable.composer_with };

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.main);

            ArcMenu arcMenu = FindViewById<ArcMenu>(Resource.Id.arc_menu3);
            ArcMenu arcMenu2 = FindViewById<ArcMenu>(Resource.Id.arc_menu_2);

            initArcMenu(arcMenu, ITEM_DRAWABLES);
            initArcMenu(arcMenu2, ITEM_DRAWABLES);
        }

        private void initArcMenu(ArcMenu menu, int[] itemDrawables)
        {
            int itemCount = itemDrawables.Length;
            for (int i = 0; i < itemCount; i++)
            {
                ImageView item = new ImageView(this);
                item.SetImageResource(itemDrawables[i]);

                int position = i;
                menu.addItem(item, new OnClickListenerEmpty());
            }
        }

        class OnClickListenerEmpty : Java.Lang.Object, Android.Views.View.IOnClickListener
        {
            public void OnClick(View v)
            {
            }
        }
    }
}
