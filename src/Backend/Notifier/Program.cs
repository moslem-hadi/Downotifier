using Notifier.Services;
using Notifier.Services.Notify;
using Shared.Extensions;
using Shared.Messaging;
using Shared.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMailerSend(options =>
{
    options.ApiToken = builder.Configuration["MailSender:ApiToken"];
    options.SenderEmail = builder.Configuration["MailSender:SenderEmail"];
    options.SenderName = builder.Configuration["MailSender:SenderName"];
});
builder.Services.AddScoped<INotifyService, EmailNotifyService>();
builder.Services.AddSingleton<INotifierContext, NotifierContext>();
builder.Services.AddMessaging()
    .AddSerialization()
    .AddRabbitMQMessaging()
    .AddHostedService<MessagingBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/", () =>
{
    return "Notifier";
})
.WithName("Notifier")
.WithOpenApi();

app.Run();
 