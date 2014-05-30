using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MMD_Core_v2.Models;
using MMD_Core_v2.Services;

namespace MMD_Android.activities
{
    [Activity(Label = "Test screen Miki")]
    public class Miki : BaseActivity
    {
        private CategoriesAdapter m_categories;
        private List<ImagesAdapter> m_imageAdapters;
        private bool m_isImageListActive;
        private ListView m_listView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.Miki);

            ThreadPool.QueueUserWorkItem(
                state =>
                {
                    ImagingData data = new ImagingData();
                    data.Init();
                    RunOnUiThread(new Action(() =>
                    {
                        InitializeCategoriesList(data);
                        InitializeImagesList(data);
                    }
                    ));
                }
                );
        }

        private void InitializeCategoriesList(ImagingData data)
        {
            m_listView = FindViewById<ListView>(Resource.Id.listview_Categories);
            m_categories = new CategoriesAdapter(this, data.Categories);
            m_listView.ItemClick += OnClick_Category;

            m_listView.Adapter = m_categories;
        }

        private void InitializeImagesList(ImagingData data)
        {
            m_imageAdapters = new List<ImagesAdapter>(data.ImagesList.Count);
            //todo : improve this shit
            foreach (KeyValuePair<string, IList<ImageData>> kvp in data.ImagesList)
            {
                m_imageAdapters.Add(new ImagesAdapter(this, kvp.Value));
            }
        }

        private void OnClick_Category(object sender, AdapterView.ItemClickEventArgs e)
        {
                var adapter = m_listView.Adapter as CategoriesAdapter;
                var categ = adapter.Categories[e.Position];

                //todo : don't rely on multiple indexing stuff like: e.position, indexing in the categories dictionary, etc...
                m_listView.Adapter = m_imageAdapters[e.Position];
                m_listView.ItemClick -= OnClick_Category;
                m_listView.ItemClick += OnClick_Image;
                m_isImageListActive = true;
        }

        async private void OnClick_Image(object sender, AdapterView.ItemClickEventArgs e)
        {
            //todo show the image here
            var imageView = FindViewById<ImageView>(Resource.Id.imageView_Categ);
            var adapter= m_listView.Adapter as ImagesAdapter;
            var imgId = adapter.Images[e.Position].Image;

            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;

            var rawImage = await ImageDownload.Get(imgId);
            Bitmap bitmap = await BitmapFactory.DecodeByteArrayAsync(rawImage, 0, rawImage.Length);
            imageView.SetImageBitmap(bitmap);
            //options.InSampleSize = options.OutWidth > options.OutHeight ? options.OutHeight / imageview.Height : options.OutWidth / imageview.Width;
            //options.InSampleSize = options.OutWidth > options.OutHeight ? options.OutHeight / imageView.Height : options.OutWidth / imageView.Width;
            //options.InJustDecodeBounds = false;

            //Bitmap bitmap = await BitmapFactory.DecodeFileAsync(localPath, options);
        }

        public override void OnBackPressed()
        {
            if (m_isImageListActive)
            {
                m_listView.ItemClick -= OnClick_Image;
                m_listView.ItemClick += OnClick_Category;
                m_listView.Adapter = m_categories;

                m_isImageListActive = false;
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }

    class CategoriesAdapter : BaseAdapter
    {
        IList<Category> m_categories;
        private Activity m_activity;

        public IList<Category> Categories
        {
            get { return m_categories; }
        }

        public CategoriesAdapter(Activity activity, IList<Category> categories)
        {
            m_categories = categories;
            m_activity = activity;
        }

        public override int Count
        {
            get { return m_categories.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            //could wrap a category into a java.lang.object
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? m_activity.LayoutInflater.Inflate(Resource.Layout.CategoryElementLayout, parent, false);
            var title = view.FindViewById<TextView>(Resource.Id.textView_Title);//todo : rename the textview
            title.Text = m_categories[position].Name;
            return view;
        }
    }

    class ImagesAdapter : BaseAdapter
    {
        IList<ImageData> m_images;
        private Activity m_activity;

        public IList<ImageData> Images
        {
            get { return m_images; }
        }

        public ImagesAdapter(Activity activity, IList<ImageData> images)
        {
            m_images = images;
            m_activity = activity;
        }

        public override int Count
        {
            get { return m_images.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            //could wrap a category into a java.lang.object
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? m_activity.LayoutInflater.Inflate(Resource.Layout.CategoryElementLayout, parent, false);
            var title = view.FindViewById<TextView>(Resource.Id.textView_Title);//todo : rename the textview
            title.Text = m_images[position].Image;
            return view;
        }
    }
}