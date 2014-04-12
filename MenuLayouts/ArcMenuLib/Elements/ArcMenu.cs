using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using arcmenulib.elements.Dependencies;

namespace arcmenulib.elements
{
    public class ArcMenu : RelativeLayout
    {
        private ArcLayout mArcLayout;
        private ImageView mHintView;

        public ArcMenu(Context context)
            : base(context)
        {
            init(context);
        }

        public ArcMenu(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            init(context);
            applyAttrs(attrs);
        }

        private void init(Context context)
        {
            LayoutInflater li = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            li.Inflate(Resource.Layout.arc_menu, this);

            mHintView = FindViewById<ImageView>(Resource.Id.control_hint);
            mArcLayout = FindViewById<ArcLayout>(Resource.Id.item_layout);

            ViewGroup controlLayout = FindViewById<ViewGroup>(Resource.Id.control_layout);
            controlLayout.SetOnTouchListener(new OnTouchListener(mArcLayout, mHintView));
            controlLayout.Clickable = true;
        }

        private void applyAttrs(IAttributeSet attrs)
        {
            if (attrs != null)
            {
                TypedArray a = this.Context.ObtainStyledAttributes(attrs, Resource.Styleable.ArcLayout, 0, 0);

                float fromDegrees = a.GetFloat(Resource.Styleable.ArcLayout_fromDegrees, ArcLayout.DEFAULT_FROM_DEGREES);
                float toDegrees = a.GetFloat(Resource.Styleable.ArcLayout_toDegrees, ArcLayout.DEFAULT_TO_DEGREES);
                mArcLayout.setArc(fromDegrees, toDegrees);

                int defaultChildSize = mArcLayout.ChildSize;
                int newChildSize = a.GetDimensionPixelSize(Resource.Styleable.ArcLayout_childSize, defaultChildSize);
                mArcLayout.ChildSize = newChildSize;

                a.Recycle();
            }
        }

        public void addItem(View item, IOnClickListener listener)
        {
            mArcLayout.AddView(item);
            item.SetOnClickListener(getItemClickListener(listener));
        }

        private IOnClickListener getItemClickListener(IOnClickListener listener)
        {
            var res = new OnClicklistener(mArcLayout, mHintView);
            res.OnAnimationEnd += () =>
            {
                PostDelayed(new Action(
                    () =>
                    {
                        itemDidDisappear();
                    }), 0);
            };
            return res;
        }

        private void itemDidDisappear()
        {
            int itemCount = mArcLayout.ChildCount;
            for (int i = 0; i < itemCount; i++)
            {
                View item = mArcLayout.GetChildAt(i);
                item.ClearAnimation();
            }

            mArcLayout.switchState(false);
        }
    }
}