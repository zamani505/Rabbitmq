using RabbitMQ.Client;
using RabbitSample.Contracts;
using RabbitSample.Extentions;
using RabbitSample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IConnectionService, ConnectionService>();
builder.Services.AddScoped<IMessageProducer, MessageProducer>();
var hostName = "172.16.21.31";
var port = "15672";
builder.Services.AddSingleton<MessageConsumer>(sp =>
{
    var factory = new ConnectionFactory()
    {

        Uri = new Uri($"amqp:/guest:guest@{hostName}:{port}"),
        VirtualHost = "/",
        UserName = "guest",
        Password = "guest"
    };
    return new MessageConsumer(factory);

});
builder.Services.AddOptions();
var app = builder.Build();
app.UseRouting();
//app.MapGet("/", () => "Hello World!");
app.UseRabbitListener();
app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
app.Run();