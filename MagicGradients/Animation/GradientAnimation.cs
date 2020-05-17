using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MagicGradients.Animation
{
    public abstract class GradientAnimation : BindableObject
    {
        public uint Duration { get; set; } = 1000;
        public int Delay { get; set; } = 0;
        public bool RepeatForever { get; set; } = false;
        public EasingType Easing { get; set; } = EasingType.Linear;
        public BindableObject Target { get; set; } = default;
        public VisualElement Animator { get; private set; } = default;

        public async Task Begin(VisualElement animator)
        {
            Animator = animator;

            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            OnPrepare();

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
                    if (RepeatForever)
                        OnReset();
                    else
                        taskCompletionSource.SetResult(c);
                },
                repeat: () => RepeatForever);

            return taskCompletionSource.Task;
        }
        
        public virtual void OnPrepare() { }
        public abstract Xamarin.Forms.Animation OnAnimate();
        public virtual void OnReset() { }

        public void End()
        {
            ViewExtensions.CancelAnimations(Animator);
        }

        public void AttachTo(BindableObject parent)
        {
            if (Target == null)
            {
                Target = parent;
            }
        }
    }
}
