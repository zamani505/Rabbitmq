using RabbitSample.Services;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

namespace RabbitSample.Extentions
{
    public static class RabbitExtetions
    {
        public static MessageConsumer Listener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<MessageConsumer>();
            var life = app.ApplicationServices.GetService<IApplicationLifetime>();
            life.ApplicationStarted.Register(OnStarted);

            //press Ctrl+C to reproduce if your app runs in Kestrel as a console app
            life.ApplicationStopping.Register(OnStopping);
            return app;
        }
        private static void OnStarted()
        {
            Listener.CreateConsumerChannel();
        }

        private static void OnStopping()
        {
            Listener.Disconnect();
        }
    }
}
