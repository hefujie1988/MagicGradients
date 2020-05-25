using Xamarin.Forms;

namespace MagicGradients.Animation
{
    public class ColorAnimation : PropertyAnimation<Color>
    {
        protected override Color GetProgressValue(double progress)
        {
            return AnimationHelper.GetColorValue(From, To, progress);
        }
    }
}
