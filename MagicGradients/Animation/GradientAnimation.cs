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

        protected abstract Task BeginAnimation();

        public async Task Begin(VisualElement animator)
        {
            Animator = animator;

            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            if (Delay > 0)
            {
                await Task.Delay(Delay);
            }

            await BeginAnimation();
        }

        public void End()
        {
            ViewExtensions.CancelAnimations(Animator);
            //AnimationExtensions.AbortAnimation()
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
