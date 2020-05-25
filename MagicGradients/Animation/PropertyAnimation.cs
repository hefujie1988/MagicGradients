using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace MagicGradients.Animation
{
    public abstract class PropertyAnimation<TValue> : Timeline
    {
        public TValue From { get; set; } = default;
        public TValue To { get; set; } = default;
        public BindableProperty TargetProperty { get; set; } = default;

        public override void OnBegin()
        {
            base.OnBegin();

            if (TargetProperty == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            SetDefaultFrom((TValue)Target.GetValue(TargetProperty));
        }

        public override Xamarin.Forms.Animation OnAnimate() => new Xamarin.Forms.Animation(x =>
        {
            var value = GetProgressValue(x);
            Target.SetValue(TargetProperty, value);
        },
        easing: EasingHelper.GetEasing(Easing),
        finished: () =>
        {
            Debug.WriteLine("Finished Property");
            OnFinished();
        });

        protected override void OnFinished()
        {
            if (AutoReverse)
            {
                var tmp = From;
                From = To;
                To = tmp;
            }
        }

        protected abstract TValue GetProgressValue(double progress);

        private void SetDefaultFrom(TValue value)
        {
            From = From.Equals(default(TValue)) ? value : From;
        }
    }
}
