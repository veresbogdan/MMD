using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace ArcMenuLib.Elements.Dependencies
{
    //todo : rename this class in order to reflect its purpose
    internal class OnClicklistener : Java.Lang.Object, Android.Views.View.IOnClickListener
    {
        public delegate void OnAnimationEndMethod();
        public event OnAnimationEndMethod OnAnimationEnd;

        private ArcLayout m_arcLayout;
        private ImageView m_hintView;

        public OnClicklistener(ArcLayout arcLayout, ImageView hintView)
        {
            m_arcLayout = arcLayout;
            m_hintView = hintView;
        }

        public void OnClick(View viewClicked)
        {
            Animation animation = bindItemAnimation(viewClicked, true, 400);
            animation.AnimationEnd += (sender, e) =>
                {
                    if (OnAnimationEnd != null)
                    {
                        OnAnimationEnd();
                    }
                };

            int itemCount = m_arcLayout.ChildCount;
            for (int i = 0; i < itemCount; i++)
            {
                View item = m_arcLayout.GetChildAt(i);
                if (viewClicked != item)
                {
                    bindItemAnimation(item, false, 300);
                }
            }
        }

        private Animation bindItemAnimation(View child, bool isClicked, long duration)
        {
            Animation animation = createItemDisapperAnimation(duration, isClicked);
            child.Animation = animation;

            return animation;
        }

        private static Animation createItemDisapperAnimation(long duration, bool isClicked)
        {
            AnimationSet animationSet = new AnimationSet(true);
            animationSet.AddAnimation(
                new ScaleAnimation(
                    1.0f,
                    isClicked ? 2.0f : 0.0f,
                    1.0f,
                    isClicked ? 2.0f : 0.0f,
                    Dimension.RelativeToSelf,
                    0.5f,
                    Dimension.RelativeToSelf,
                    0.5f));
            animationSet.AddAnimation(new AlphaAnimation(1.0f, 0.0f));

            animationSet.Duration = duration;
            animationSet.Interpolator = new DecelerateInterpolator();
            animationSet.FillAfter = true;

            return animationSet;
        }
    }
}