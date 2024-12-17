using UIKit;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Controls.Platform;
using CMM.Mobile.snake;
using Microsoft.Maui.Controls;

[assembly: ExportEffect(typeof(PlatformTouchEffect), "TouchEffect")]
namespace CMM.Mobile.snake
{
    public class PlatformTouchEffect : PlatformEffect
    {
        private UITapGestureRecognizer tapRecognizer;
        private TouchEffect touchEffect;

        protected override void OnAttached()
        {
            touchEffect = (TouchEffect)Element.Effects.FirstOrDefault(e => e is TouchEffect);
            if (touchEffect == null)
                return;

            tapRecognizer = new UITapGestureRecognizer(OnTap);
            if (Control != null)
            {
                Control.AddGestureRecognizer(tapRecognizer);
            }
            else
            {
                Container.AddGestureRecognizer(tapRecognizer);
            }
        }

        protected override void OnDetached()
        {
            if (Control != null)
            {
                Control.RemoveGestureRecognizer(tapRecognizer);
            }
            else
            {
                Container.RemoveGestureRecognizer(tapRecognizer);
            }
        }

        private void OnTap(UITapGestureRecognizer recognizer)
        {
            var location = recognizer.LocationInView(Control ?? Container);
            touchEffect.OnTouchAction(Element, new TouchActionEventArgs((long)recognizer.Handle, TouchActionType.Released, new Point(location.X, location.Y), true));
        }
    }
}
