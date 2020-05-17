using Xamarin.Forms;

namespace MagicGradients.Animation
{
    public class ColorAnimation : PropertyAnimation<Color>
    {
        public override Xamarin.Forms.Animation OnAnimate() => new Xamarin.Forms.Animation(x =>
        {
            var value = AnimationHelper.GetColorValue(From, To, x);
            Target.SetValue(TargetProperty, value);
        });
    }
}
