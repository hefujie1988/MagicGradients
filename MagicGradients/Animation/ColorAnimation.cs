using Xamarin.Forms;

namespace MagicGradients.Animation
{
    public class ColorAnimation : PropertyAnimation<Color>
    {
        protected override void OnAnimate(double progress)
        {
            var value = AnimationHelper.GetColorValue(From, To, progress);
            Target.SetValue(TargetProperty, value);
        }
    }
}
