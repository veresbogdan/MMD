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
    internal class OnTouchListener : Java.Lang.Object, Android.Views.View.IOnTouchListener
    {
        private ArcLayout m_arcLayout;
        private ImageView m_hintView;

        public OnTouchListener(ArcLayout arcLayout, ImageView hintView)
        {
            m_arcLayout = arcLayout;
            m_hintView = hintView;
        }

        public bool OnTouch(Android.Views.View v, MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
            {
                m_hintView.StartAnimation(createHintSwitchAnimation(m_arcLayout.IsExpanded));
                m_arcLayout.switchState(true);
            }

            return false;
        }

        private static Animation createHintSwitchAnimation(bool expanded)
        {
            Animation animation = new RotateAnimation(expanded ? 45 : 0, expanded ? 0 : 45, Dimension.RelativeToSelf,
                    0.5f, Dimension.RelativeToSelf, 0.5f);
            animation.StartOffset = 0;
            animation.Duration = 100;
            animation.Interpolator = new DecelerateInterpolator();
            animation.FillAfter = true;

            return animation;
        }
    }
}