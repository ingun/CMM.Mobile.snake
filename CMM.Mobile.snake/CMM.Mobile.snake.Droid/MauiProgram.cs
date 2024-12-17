namespace CMM.Mobile.snake.Droid
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureEffects(effects =>
                {
                    effects.Add<TouchEffect, PlatformTouchEffect>();
                });

            return builder.Build();
        }
    }
}
