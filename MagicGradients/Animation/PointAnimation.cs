using Xamarin.Forms;

namespace MagicGradients.Animation
{
    public class PointAnimation : PropertyAnimation<Point>
    {
        protected override Point GetProgressValue(double progress)
        {
            return AnimationHelper.GetPointValue(From, To, progress);
        }
    }
}
