using Hangfire;
using Hangfire.Logging;
using ProtoBuf.Meta;
using Scheduler.Services;
using Serilog;
using Shared.Messaging;
using Shared.Messaging.Pulsar;
using Shared.Serialization;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((ctx, lc) => lc
       .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
       .Enrich.FromLogContext()
       .ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddHostedService<ConsumeRabbitMQHostedService>();
builder.Services.AddMessaging()
    .AddSerialization()
    .AddRabbitMQMessaging()
    //.AddPulsar()
    .AddHostedService<MessagingBackgroundService>();

//var queue = "job";

//var factory = new ConnectionFactory
//{
//    HostName = "localhost",
//    UserName = "guest",
//    Password = "guest",
//    Port = 5672
//};
//var connection = factory.CreateConnection();
//using var channel = connection.CreateModel();
//channel.QueueDeclare(queue,
//                     durable: false,
//                     exclusive: false,
//                     autoDelete: false,
//                     arguments: null);

//var consumer = new EventingBasicConsumer(channel);
//consumer.Received += (model, eventArgs) =>
//{
//    var body = eventArgs.Body.ToArray();
//    var message = JsonSerializer.Deserialize<ApiCallJobCreatedEvent>(Encoding.UTF8.GetString(body));
//    //handler.Invoke(message);
//};
////read the message
//channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);

var hangfireConnection = builder.Configuration["ConnectionStrings:Hangfire"];

Log.Fatal(hangfireConnection);
builder.Services.AddHangfire(x => x.UseSqlServerStorage(hangfireConnection));
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHangfireDashboard();
app.UseAuthorization();

app.MapControllers();

app.Run();
