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
    internal class RotateAndTranslateAnimation : Animation
    {
        private Dimension mFromXType = Dimension.Absolute;
        private Dimension mToXType = Dimension.Absolute;
        private Dimension mFromYType = Dimension.Absolute;
        private Dimension mToYType = Dimension.Absolute;
        private float mFromXValue = 0.0f;
        private float mToXValue = 0.0f;
        private float mFromYValue = 0.0f;
        private float mToYValue = 0.0f;
        private float mFromXDelta;
        private float mToXDelta;
        private float mFromYDelta;
        private float mToYDelta;
        private float mFromDegrees;
        private float mToDegrees;

        private Dimension mPivotXType = Dimension.Absolute;
        private Dimension mPivotYType = Dimension.Absolute;
        private float mPivotXValue = 0.0f;
        private float mPivotYValue = 0.0f;
        private float mPivotX;
        private float mPivotY;

        /// <summary>
        /// To use when building a TranslateAnimation from code
        /// </summary>
        /// <param name="fromXDelta">Change in X coordinate to apply at the start of the animation</param>
        /// <param name="toXDelta">Change in X coordinate to apply at the end of the animation</param>
        /// <param name="fromYDelta">Change in Y coordinate to apply at the start of the animation</param>
        /// <param name="toYDelta">Change in Y coordinate to apply at the end of the animation</param>
        /// <param name="fromDegrees">Rotation offset to apply at the start of the animation.</param>
        /// <param name="toDegrees">Rotation offset to apply at the end of the animation.</param>
        public RotateAndTranslateAnimation(float fromXDelta, float toXDelta, float fromYDelta, float toYDelta,
            float fromDegrees, float toDegrees)
        {
            mFromXValue = fromXDelta;
            mToXValue = toXDelta;
            mFromYValue = fromYDelta;
            mToYValue = toYDelta;

            mFromXType = Dimension.Absolute;
            mToXType = Dimension.Absolute;
            mFromYType = Dimension.Absolute;
            mToYType = Dimension.Absolute;

            mFromDegrees = fromDegrees;
            mToDegrees = toDegrees;

            mPivotXValue = 0.5f;
            mPivotXType = Dimension.RelativeToSelf;
            mPivotYValue = 0.5f;
            mPivotYType = Dimension.RelativeToSelf;
        }

        protected override void ApplyTransformation(float interpolatedTime, Transformation t)
        {
            float degrees = mFromDegrees + ((mToDegrees - mFromDegrees) * interpolatedTime);
            if (mPivotX == 0.0f && mPivotY == 0.0f)
            {
                t.Matrix.SetRotate(degrees);
            }
            else
            {
                t.Matrix.SetRotate(degrees, mPivotX, mPivotY);
            }

            float dx = mFromXDelta;
            float dy = mFromYDelta;
            if (mFromXDelta != mToXDelta)
            {
                dx = mFromXDelta + ((mToXDelta - mFromXDelta) * interpolatedTime);
            }
            if (mFromYDelta != mToYDelta)
            {
                dy = mFromYDelta + ((mToYDelta - mFromYDelta) * interpolatedTime);
            }

            t.Matrix.PostTranslate(dx, dy);
        }

        public override void Initialize(int width, int height, int parentWidth, int parentHeight)
        {
            base.Initialize(width, height, parentWidth, parentHeight);
            mFromXDelta = ResolveSize(mFromXType, mFromXValue, width, parentWidth);
            mToXDelta = ResolveSize(mToXType, mToXValue, width, parentWidth);
            mFromYDelta = ResolveSize(mFromYType, mFromYValue, height, parentHeight);
            mToYDelta = ResolveSize(mToYType, mToYValue, height, parentHeight);

            mPivotX = ResolveSize(mPivotXType, mPivotXValue, width, parentWidth);
            mPivotY = ResolveSize(mPivotYType, mPivotYValue, height, parentHeight);
        }
    }
}