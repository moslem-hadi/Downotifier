using Notifier.Services.Notify;
using Shared.Extensions;

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
 