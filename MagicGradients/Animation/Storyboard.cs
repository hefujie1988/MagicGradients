using System.Collections.Generic;
using Xamarin.Forms;

namespace MagicGradients.Animation
{
    [ContentProperty(nameof(Animations))]
    public class Storyboard : GradientAnimation
    {
        public static readonly BindableProperty BeginAtProperty =
            BindableProperty.CreateAttached("BeginAt", typeof(double), typeof(Storyboard), 0d);

        public static readonly BindableProperty FinishAtProperty =
            BindableProperty.CreateAttached("FinishAt", typeof(double), typeof(Storyboard), 1d);

        public static double GetBeginAt(BindableObject view) => (double)view.GetValue(BeginAtProperty);
        public static void SetBeginAt(BindableObject view, double value) => view.SetValue(BeginAtProperty, value);
        
        public static double GetFinishAt(BindableObject view) => (double)view.GetValue(FinishAtProperty);
        public static void SetFinishAt(BindableObject view, double value) => view.SetValue(FinishAtProperty, value);

        public List<GradientAnimation> Animations { get; set; } = new List<GradientAnimation>();

        public override void OnPrepare()
        {
            foreach (var anim in Animations)
            {
                if (anim.Target == null)
                    anim.Target = Target;
            }
        }

        public override Xamarin.Forms.Animation OnAnimate()
        {
            var animation = new Xamarin.Forms.Animation();

            foreach (var anim in Animations)
            {
                var beginAt = GetBeginAt(anim);
                var finishAt = GetFinishAt(anim);
                animation.Add(beginAt, finishAt, anim.OnAnimate());
            }

            return animation;
        }

        public override void OnReset()
        {
            foreach (var anim in Animations)
            {
                anim.OnReset();
            }
        }
    }
}
