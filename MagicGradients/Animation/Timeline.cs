using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MagicGradients.Animation
{
    public abstract class Timeline : BindableObject
    {
        public uint Duration { get; set; } = 0;
        public int Delay { get; set; } = 0;
        public RepeatBehavior RepeatBehavior { get; set; }
        public bool AutoReverse { get; set; }
        public EasingType Easing { get; set; } = EasingType.Linear;
        public BindableObject Target { get; set; } = default;
        public VisualElement Animator { get; private set; } = default;

        public async Task Begin(VisualElement animator)
        {
            Animator = animator;
            OnBegin();

            if (Delay > 0)
            {
                await Task.Delay(Delay);
            }

            await BeginAnimation();
        }

        protected virtual Task BeginAnimation()
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            Animator.Animate(Guid.NewGuid().ToString(), OnAnimate(),
                length: Duration,
                easing: EasingHelper.GetEasing(Easing),
                finished: (v, c) =>
                {
                    if (RepeatBehavior == RepeatBehavior.Forever)
                        OnRepeat();
                    else
                        taskCompletionSource.SetResult(c);
                },
                repeat: () => RepeatBehavior == RepeatBehavior.Forever);

            return taskCompletionSource.Task;
        }

        public virtual void OnBegin()
        {
            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }
        }

        public virtual void OnRepeat() { }
        public abstract Xamarin.Forms.Animation OnAnimate();

        public void End()
        {
            ViewExtensions.CancelAnimations(Animator);
        }
    }
}
