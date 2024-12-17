using Microsoft.Extensions.Logging;

namespace CMM.Mobile.snake
{
    public static class MauiProgramExtensions
    {
        public static MauiAppBuilder UseSharedMauiApp(this MauiAppBuilder builder)
        {
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureEffects(effects =>
                {
                    effects.Add<TouchEffect, PlatformTouchEffect>();
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder;
        }
    }
}
