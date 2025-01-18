using Android.Views;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using CMM.Mobile.snake;
using Microsoft.Maui.Controls;
using CMM.Mobile.snake.Droid;

[assembly: ExportEffect(typeof(PlatformTouchEffect), "TouchEffect")]
namespace CMM.Mobile.snake.Droid
{
    public class PlatformTouchEffect : PlatformEffect
    {
        private Android.Views.View view;
        private bool capture;

        protected override void OnAttached()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("PlatformTouchEffect: OnAttached called");
                view = Control ?? Container;
                if (view != null)
                {
                    view.Enabled = true;
                    view.Clickable = true;
                    view.LongClickable = true;
                    view.Touch += OnTouch;
                    System.Diagnostics.Debug.WriteLine("PlatformTouchEffect: Touch event attached successfully");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("PlatformTouchEffect: View is null!");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PlatformTouchEffect Error: {ex.Message}");
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
            try
            {
                System.Diagnostics.Debug.WriteLine($"PlatformTouchEffect: Touch event received - Action: {e.Event.Action}");
                
                var touchEffect = (TouchEffect)Element.Effects.FirstOrDefault(eff => eff is TouchEffect);
                if (touchEffect == null)
                {
                    System.Diagnostics.Debug.WriteLine("PlatformTouchEffect: TouchEffect is null!");
                    return;
                }

                e.Handled = true;

                switch (e.Event.Action)
                {
                    case MotionEventActions.Down:
                        System.Diagnostics.Debug.WriteLine("Touch DOWN detected");
                        touchEffect.OnTouchAction(Element, new TouchActionEventArgs(
                            e.Event.GetPointerId(e.Event.ActionIndex),
                            TouchActionType.Pressed,
                            new Point(e.Event.GetX(), e.Event.GetY()),
                            true));
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"OnTouch Error: {ex.Message}");
            }
        }
    }
}
