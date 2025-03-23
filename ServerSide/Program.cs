using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Refit;
using ServerSide.Database;
using ServerSide.Handlers;
using ServerSide.Repositories.Class;
using ServerSide.Services;
using ServerSide.Services.WhatsappSender.ApiDefinition;
using Sinch.Fax.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql("server=localhost;database=hairbellodb;user=root;password=ali12300;",
        new MySqlServerVersion(new Version(8, 0, 32)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure()
    ));
builder.Services.AddSwaggerGen();

//repositories
builder.Services.AddScoped<BarberRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<BarberScheduleRepository>();
builder.Services.AddScoped<ServiceRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<OrderServiceRepository>();
builder.Services.AddScoped<PreviewImageRepository>();
builder.Services.AddScoped<PhonSenderService>();
builder.Services.AddScoped<ConfigRepository>();
builder.Services.AddScoped<DateTimeService>();
builder.Services.AddHostedService<ReminderBackgroundService>();

builder.Services.AddOptions<ServerSide.Services.WhatsappSender.ApiDefinition.WhatsappSenderSettings>()
    .BindConfiguration(ServerSide.Services.WhatsappSender.ApiDefinition.WhatsappSenderSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddScoped<WhatsappService>();
builder.Services.AddRefitClient<ServerSide.Services.WhatsappSender.ApiDefinition.IWhatsappApi>()
    .ConfigureHttpClient((sp, client) =>
    {
        var settings = sp.GetRequiredService<IOptions<ServerSide.Services.WhatsappSender.ApiDefinition.WhatsappSenderSettings>>().Value;
        client.BaseAddress = new Uri(settings.BaseUrl);
        client.DefaultRequestHeaders.Add("X-RapidAPI-Key", settings.RapidKey);
        client.DefaultRequestHeaders.Add("X-RapidAPI-Host", settings.RapidHost);
    }
    );

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();
app.UseCors("AllowAll");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
