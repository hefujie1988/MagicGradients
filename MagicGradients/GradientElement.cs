using MagicGradients.Animation;
using Xamarin.Forms;

namespace MagicGradients
{
    public abstract class GradientElement : BindableObject, IGradientVisualElement
    {
        public IGradientVisualElement Parent { get; set; }

        //private GradientAnimation _animation;
        //public GradientAnimation Animation
        //{
        //    get => _animation;
        //    set
        //    {
        //        _animation = value;
        //        _animation.AttachTo(this);
        //    }
        //}

        public void InvalidateCanvas()
        {
            Parent?.InvalidateCanvas();
        }

        //public virtual void BeginAnimation(VisualElement animator)
        //{
        //    Animation?.Begin(animator);
        //}

        protected override void OnPropertyChanged(string propertyName = null)
        {
            InvalidateCanvas();
        }
    }
}
