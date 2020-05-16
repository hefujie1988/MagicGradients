using MagicGradients.Animation;
using MagicGradients.Renderers;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace MagicGradients
{
    [ContentProperty(nameof(GradientSource))]
    public class GradientView : SKCanvasView, IGradientVisualElement
    {
        static GradientView()
        {
            StyleSheets.RegisterStyle("background", typeof(GradientView), nameof(GradientSourceProperty));
        }

        public static readonly BindableProperty GradientSourceProperty = BindableProperty.Create(nameof(GradientSource), 
            typeof(IGradientSource), typeof(GradientView), propertyChanged: OnGradientSourceChanged);

        public IGradientSource GradientSource
        {
            get => (IGradientSource)GetValue(GradientSourceProperty);
            set => this.SetValue(GradientSourceProperty, value);
        }

        private GradientAnimation _animation;
        public GradientAnimation Animation
        {
            get => _animation;
            set
            {
                _animation = value;
                _animation.AttachTo(this);
            }
        }

        static void OnGradientSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var gradientView = (GradientView)bindable;

            if (oldValue != null)
            {
                ((GradientElement)oldValue).Parent = null;
            }

            if (newValue != null)
            {
                var element = (GradientElement)newValue;
                element.Parent = gradientView;
                gradientView.BeginAnimation();
            }

            gradientView.InvalidateSurface();
        }

        public void BeginAnimation()
        {
            Animation?.Begin(this);
            ((GradientElement)GradientSource)?.BeginAnimation(this);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (GradientSource != null && GradientSource is BindableObject bindable)
            {
                SetInheritedBindingContext(bindable, BindingContext);
            }
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            var canvas = e.Surface.Canvas;
            canvas.Clear();

            if (GradientSource == null)
                return;

            using (var paint = new SKPaint())
            {
                var context = new RenderContext(canvas, paint, e.Info);

                foreach (var gradient in GradientSource.GetGradients())
                {
                    gradient.Measure(e.Info.Width, e.Info.Height);
                    gradient.Render(context);
                }
            }
        }

        public void InvalidateCanvas()
        {
            InvalidateSurface();
        }

        //public static readonly BindableProperty AnimationProperty =
        //    BindableProperty.CreateAttached("Animation", typeof(GradientAnimation), typeof(Animator), null, propertyChanged: OnAnimationChanged);

        //public static GradientAnimation GetAnimation(BindableObject view)
        //{
        //    return (GradientAnimation)view.GetValue(AnimationProperty);
        //}

        //public static void SetAnimation(BindableObject view, GradientAnimation value)
        //{
        //    view.SetValue(AnimationProperty, value);
        //}

        //private static void OnAnimationChanged(BindableObject bindable, object oldvalue, object newvalue)
        //{
        //    if (oldvalue != null)
        //    {
        //        ((GradientAnimation)oldvalue).Target = null;
        //        ((GradientAnimation)oldvalue).Animator = null;
        //    }

        //    if (newvalue != null)
        //    {
        //        ((GradientAnimation)oldvalue).Target = (GradientElement)bindable;
        //        //((GradientAnimation)oldvalue).Animator = this;
        //        //((GradientAnimation)newvalue).Begin();

        //        //((GradientAnimation)newvalue).Target = (GradientElement)bindable;
        //        //((GradientAnimation)newvalue).Begin();
        //    }
        //}
    }
}
