namespace CMM.Mobile.snake.Droid
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
       builder
           .UseMauiApp<App>()
           // Handlers'ı yapılandır
           .ConfigureMauiHandlers(handlers =>
           {
               // Platform specific handlers
           })
           // Effects'i yapılandır
           .ConfigureEffects(effects =>
           {
               effects.Add<TouchEffect, PlatformTouchEffect>();
           });
        return builder.Build();
        }
    }
}
