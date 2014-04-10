using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ArcMenuLib.Elements
{
    public class RayLayout : ViewGroup
    {
        public RayLayout(Context context)
            : base(context)
        {
        }

        public RayLayout(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            throw new NotImplementedException();
        }
    }
}