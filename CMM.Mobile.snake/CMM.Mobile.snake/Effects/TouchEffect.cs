using Microsoft.Maui.Controls;

namespace CMM.Mobile.snake.Effects
{
    public class TouchEffect : RoutingEffect
    {
        public event TouchActionEventHandler TouchAction;

        public TouchEffect() : base("CMM.Mobile.snake.TouchEffect")
        {
            Capture = true;
        }

        public bool Capture { get; set; }

        public void OnTouchAction(Element element, TouchActionEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine($"TouchEffect: Touch detected"); // Debug log
            TouchAction?.Invoke(element, args);
        }
    }
} 