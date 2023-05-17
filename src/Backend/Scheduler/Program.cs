using Hangfire;
using Scheduler.Services;
using Shared.Messaging;
using Shared.Messaging.Pulsar;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMessaging()
    //.AddRabbitMQMessaging()
    .AddPulsar()
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


builder.Services.AddHangfire(x => x.UseSqlServerStorage(@"Data Source=.;Initial Catalog=hangfire;Integrated Security=True;Pooling=False"));
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
