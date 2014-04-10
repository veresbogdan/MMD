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

namespace ArcMenuLib.Elements.Dependencies
{
    internal class AnimationListener : Java.Lang.Object, Android.Views.Animations.Animation.IAnimationListener
    {
        public delegate void OnAnimationEndType();
        public event OnAnimationEndType OnAnimationEndEvent;

        private bool m_isLast;
        public AnimationListener(bool isLast)
        {
            m_isLast = isLast;
        }

        public void OnAnimationEnd(Android.Views.Animations.Animation animation)
        {
            if (m_isLast && OnAnimationEndEvent != null)
            {
                OnAnimationEndEvent();
            }
        }

        public void OnAnimationRepeat(Android.Views.Animations.Animation animation)
        {
        }

        public void OnAnimationStart(Android.Views.Animations.Animation animation)
        {
        }
    }
}