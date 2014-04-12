using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using arcmenulib.elements.Dependencies;

namespace arcmenulib.elements
{
    public class ArcLayout : ViewGroup
    {
        public const float DEFAULT_FROM_DEGREES = 270.0f;
        public const float DEFAULT_TO_DEGREES = 360.0f;
        private const int MIN_RADIUS = 100;

        private float mFromDegrees = DEFAULT_FROM_DEGREES;
        private float mToDegrees = DEFAULT_TO_DEGREES;

        private int mChildSize;//children will be set to the same size
        private int mChildPadding = 5;
        private int mLayoutPadding = 10;

        private int mRadius;
        private bool mExpanded = false;

        public bool IsExpanded
        {
            get { return mExpanded; }
            private set { mExpanded = value; }
        }

        public int ChildSize
        {
            get { return mChildSize; }
            set
            {
                if (mChildSize == value || value < 0)
                {
                    return;
                }

                mChildSize = value;

                RequestLayout();
            }
        }


        public ArcLayout(Context context)
            : base(context)
        {
        }

        public ArcLayout(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            if (attrs != null)
            {
                TypedArray a = Context.ObtainStyledAttributes(attrs, Resource.Styleable.ArcLayout, 0, 0);
                mFromDegrees = a.GetFloat(Resource.Styleable.ArcLayout_fromDegrees, DEFAULT_FROM_DEGREES);
                mToDegrees = a.GetFloat(Resource.Styleable.ArcLayout_toDegrees, DEFAULT_TO_DEGREES);
                mChildSize = Math.Max(a.GetDimensionPixelSize(Resource.Styleable.ArcLayout_childSize, 0), 0);

                a.Recycle();
            }
        }

        private static int computeRadius(float arcDegrees, int childCount, int childSize,
            int childPadding, int minRadius)
        {
            if (childCount < 2)
            {
                return minRadius;
            }

            float perDegrees = arcDegrees / (childCount - 1);
            float perHalfDegrees = perDegrees / 2;
            int perSize = childSize + childPadding;

            int radius = (int)((perSize / 2) / Math.Sin(ToRadians(perHalfDegrees)));

            return Math.Max(radius, minRadius);
        }

        private static Rect computeChildFrame(int centerX, int centerY, int radius, float degrees,
            int size)
        {
            double childCenterX = centerX + radius * Math.Cos(ToRadians(degrees));
            double childCenterY = centerY + radius * Math.Sin(ToRadians(degrees));

            return new Rect((int)(childCenterX - size / 2), (int)(childCenterY - size / 2),
                    (int)(childCenterX + size / 2), (int)(childCenterY + size / 2));
        }

        private static double ToRadians(float angle)
        {
            return (Math.PI / 180) * angle;
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int radius = mRadius = computeRadius(Math.Abs(mToDegrees - mFromDegrees), ChildCount, mChildSize,
                mChildPadding, MIN_RADIUS);
            int size = radius * 2 + mChildSize + mChildPadding + mLayoutPadding * 2;

            SetMeasuredDimension(size, size);

            //todo : improve
            int count = ChildCount;
            for (int i = 0; i < count; i++)
            {
                GetChildAt(i).Measure(MeasureSpec.MakeMeasureSpec(mChildSize, MeasureSpecMode.Exactly),
                        MeasureSpec.MakeMeasureSpec(mChildSize, MeasureSpecMode.Exactly));
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            int centerX = Width / 2;
            int centerY = Height / 2;
            int radius = mExpanded ? mRadius : 0;

            //todo : improve
            int childCount = ChildCount;
            float perDegrees = (mToDegrees - mFromDegrees) / (childCount - 1);

            float degrees = mFromDegrees;
            for (int i = 0; i < childCount; i++)
            {
                Rect frame = computeChildFrame(centerX, centerY, radius, degrees, mChildSize);
                degrees += perDegrees;
                GetChildAt(i).Layout(frame.Left, frame.Top, frame.Right, frame.Bottom);
            }
        }

        internal void switchState(bool showAnimation)
        {
            if (showAnimation)
            {
                for (int i = 0; i < ChildCount; i++)
                {
                    bindChildAnimation(GetChildAt(i), i, 300);
                }
            }

            mExpanded = !mExpanded;

            if (!showAnimation)
            {
                RequestLayout();
            }

            Invalidate();
        }

        #region SwitchState helper methods

        private void bindChildAnimation(View child, int index, long duration)
        {
            bool expanded = mExpanded;
            int centerX = Width / 2;
            int centerY = Height / 2;
            int radius = expanded ? 0 : mRadius;

            int childCount = ChildCount;
            float perDegrees = (mToDegrees - mFromDegrees) / (childCount - 1);
            Rect frame = computeChildFrame(centerX, centerY, radius, mFromDegrees + index * perDegrees, mChildSize);

            int toXDelta = frame.Left - child.Left;
            int toYDelta = frame.Top - child.Top;

            IInterpolator interpolator;
            if (mExpanded)
                interpolator = new AccelerateInterpolator();
            else
                interpolator = new OvershootInterpolator(1.5f);
            long startOffset = computeStartOffset(childCount, mExpanded, index, 0.1f, duration, interpolator);

            Animation animation = mExpanded ? createShrinkAnimation(0, toXDelta, 0, toYDelta, startOffset, duration,
                    interpolator) : createExpandAnimation(0, toXDelta, 0, toYDelta, startOffset, duration, interpolator);

            bool isLast = getTransformedIndex(expanded, childCount, index) == childCount - 1;
            var animationListener = new AnimationListener(isLast);
            animationListener.OnAnimationEndEvent += () =>
                {
                    PostDelayed(new Action(
                    () =>
                    {
                        onAllAnimationsEnd();
                    }), 0);
                };
            animation.SetAnimationListener(animationListener);
            child.Animation = animation;
        }

        private static long computeStartOffset(int childCount, bool expanded, int index,
    float delayPercent, long duration, IInterpolator interpolator)
        {
            float delay = delayPercent * duration;
            long viewDelay = (long)(getTransformedIndex(expanded, childCount, index) * delay);
            float totalDelay = delay * childCount;

            float normalizedDelay = viewDelay / totalDelay;
            normalizedDelay = interpolator.GetInterpolation(normalizedDelay);

            return (long)(normalizedDelay * totalDelay);
        }

        private static int getTransformedIndex(bool expanded, int count, int index)
        {
            if (expanded)
            {
                return count - 1 - index;
            }

            return index;
        }

        private static Animation createExpandAnimation(float fromXDelta, float toXDelta, float fromYDelta, float toYDelta,
                long startOffset, long duration, IInterpolator interpolator)
        {
            Animation animation = new RotateAndTranslateAnimation(0, toXDelta, 0, toYDelta, 0, 720);
            animation.StartOffset = startOffset;
            animation.Duration = duration;
            animation.Interpolator = interpolator;
            animation.FillAfter = true;

            return animation;
        }

        private static Animation createShrinkAnimation(float fromXDelta, float toXDelta, float fromYDelta, float toYDelta,
            long startOffset, long duration, IInterpolator interpolator)
        {
            AnimationSet animationSet = new AnimationSet(false);
            animationSet.FillAfter = true;

            long preDuration = duration / 2;
            Animation rotateAnimation = new RotateAnimation(0, 360, Dimension.RelativeToSelf, 0.5f,
                    Dimension.RelativeToSelf, 0.5f);
            rotateAnimation.StartOffset = startOffset;
            rotateAnimation.Duration = preDuration;
            rotateAnimation.Interpolator = new LinearInterpolator();
            rotateAnimation.FillAfter = true;

            animationSet.AddAnimation(rotateAnimation);

            Animation translateAnimation = new RotateAndTranslateAnimation(0, toXDelta, 0, toYDelta, 360, 720);
            translateAnimation.StartOffset = startOffset + preDuration;
            translateAnimation.Duration = duration - preDuration;
            translateAnimation.Interpolator = interpolator;
            translateAnimation.FillAfter = true;

            animationSet.AddAnimation(translateAnimation);

            return animationSet;
        }

        private void onAllAnimationsEnd()
        {
            for (int i = 0; i < ChildCount; i++)
            {
                GetChildAt(i).ClearAnimation();
            }

            RequestLayout();
        }
        #endregion

        internal void setArc(float fromDegrees, float toDegrees)
        {
            if (mFromDegrees == fromDegrees && mToDegrees == toDegrees)
            {
                return;
            }

            mFromDegrees = fromDegrees;
            mToDegrees = toDegrees;
            RequestLayout();
        }
    }
}