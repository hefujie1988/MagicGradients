using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MagicGradients.Animation
{
    public abstract class PropertyAnimation<TValue> : GradientAnimation
    {
        public TValue From { get; set; } = default;
        public TValue To { get; set; } = default;
        public BindableProperty TargetProperty { get; set; } = default;

        protected override Task BeginAnimation()
        {
            if (TargetProperty == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            SetDefaultFrom((TValue)Target.GetValue(TargetProperty));

            var taskCompletionSource = new TaskCompletionSource<bool>();
            var animation = new Xamarin.Forms.Animation(OnAnimate);

            Animator.Animate(TargetProperty.PropertyName, animation,
                length: Duration,
                easing: EasingHelper.GetEasing(Easing),
                finished: (v, c) =>
                {
                    if (RepeatForever)
                        Target.SetValue(TargetProperty, From);
                    else
                        taskCompletionSource.SetResult(c);
                },
                repeat: () => RepeatForever);

            return taskCompletionSource.Task;
        }

        protected virtual void SetDefaultFrom(TValue value)
        {
            From = From.Equals(default(TValue)) ? value : From;
        }

        protected abstract void OnAnimate(double progress);
    }
}
