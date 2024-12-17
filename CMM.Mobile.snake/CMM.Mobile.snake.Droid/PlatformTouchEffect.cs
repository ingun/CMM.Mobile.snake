using Android.Views;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using CMM.Mobile.snake;
using Microsoft.Maui.Controls;

[assembly: ExportEffect(typeof(PlatformTouchEffect), "TouchEffect")]
namespace CMM.Mobile.snake.Droid
{
    public class PlatformTouchEffect : PlatformEffect
    {
        private Android.Views.View view;
        private bool capture;

        protected override void OnAttached()
        {
            view = Control ?? Container;
            if (view != null)
            {
                view.Touch += OnTouch;
            }
        }

        protected override void OnDetached()
        {
            if (view != null)
            {
                view.Touch -= OnTouch;
            }
        }

        private void OnTouch(object sender, Android.Views.View.TouchEventArgs e)
        {
            var touchEffect = (TouchEffect)Element.Effects.FirstOrDefault(eff => eff is TouchEffect);
            if (touchEffect == null) return;

            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    touchEffect.OnTouchAction(Element, new TouchActionEventArgs(e.Event.GetPointerId(e.Event.ActionIndex), TouchActionType.Pressed, new Point(e.Event.GetX(), e.Event.GetY()), true));
                    capture = touchEffect.Capture;
                    break;
                case MotionEventActions.Move:
                    if (capture)
                    {
                        touchEffect.OnTouchAction(Element, new TouchActionEventArgs(e.Event.GetPointerId(e.Event.ActionIndex), TouchActionType.Moved, new Point(e.Event.GetX(), e.Event.GetY()), true));
                    }
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    touchEffect.OnTouchAction(Element, new TouchActionEventArgs(e.Event.GetPointerId(e.Event.ActionIndex), TouchActionType.Released, new Point(e.Event.GetX(), e.Event.GetY()), false));
                    break;
            }
        }
    }
}
