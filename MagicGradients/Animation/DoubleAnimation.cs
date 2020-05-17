using System.Diagnostics;

namespace MagicGradients.Animation
{
    public class DoubleAnimation : PropertyAnimation<double>
    {
        public override Xamarin.Forms.Animation OnAnimate() => new Xamarin.Forms.Animation(x =>
        {
            var value = AnimationHelper.GetDoubleValue(From, To, x);
            Debug.WriteLine($"Value: {value}");
            Target.SetValue(TargetProperty, value);
        });
    }
}
