using System.Diagnostics;

namespace MagicGradients.Animation
{
    public class DoubleAnimation : PropertyAnimation<double>
    {
        protected override void OnAnimate(double progress)
        {
            var value = AnimationHelper.GetDoubleValue(From, To, progress);
            Debug.WriteLine($"Value: {value}");
            Target.SetValue(TargetProperty, value);
        }
    }
}
