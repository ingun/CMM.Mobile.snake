using CMM.Mobile.snake;
using Microsoft.Maui.Controls.Platform;

[assembly: ExportEffect(typeof(PlatformTouchEffect), "TouchEffect")]
namespace CMM.Mobile.snake
{
    public class PlatformTouchEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
         
        }

        protected override void OnDetached()
        {
           
        }
    }
}
