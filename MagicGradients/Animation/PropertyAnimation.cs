using System;
using Xamarin.Forms;

namespace MagicGradients.Animation
{
    public abstract class PropertyAnimation<TValue> : GradientAnimation
    {
        public TValue From { get; set; } = default;
        public TValue To { get; set; } = default;
        public BindableProperty TargetProperty { get; set; } = default;

        public override void OnPrepare()
        {
            if (TargetProperty == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            SetDefaultFrom((TValue)Target.GetValue(TargetProperty));
        }

        public override void OnReset()
        {
            // Reset
            //Target.SetValue(TargetProperty, From);

            // Revert
            var tmp = From;
            From = To;
            To = tmp;
        }

        private void SetDefaultFrom(TValue value)
        {
            From = From.Equals(default(TValue)) ? value : From;
        }
    }
}
