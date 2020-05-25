namespace MagicGradients.Animation
{
    public class DoubleAnimation : PropertyAnimation<double>
    {
        protected override double GetProgressValue(double progress)
        {
            return AnimationHelper.GetDoubleValue(From, To, progress);
        }
    }
}
