using MassTransit;
using Microsoft.EntityFrameworkCore;
using SALES.Application.Interfaces;
using SALES.Persistence.Context;
using SALES.Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

// Registrar MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(
        typeof(Program).Assembly,
        typeof(IApplicationDbContext).Assembly
    ));

// Configurar DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions => {
        // ESTA ES LA LÍNEA QUE TE FALTA:
        sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    })
);

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());

//// Registrar Servicio HTTP de Carrito
//builder.Services.AddHttpClient<ICartIntegrationService, CartIntegrationService>(client =>
//{
//    // AQUÍ EL PUERTO DONDE CORRE EL PROYECTO SHOPPING_CART
//    client.BaseAddress = new Uri("https://localhost:7281/");
//});

//// Registrar Servicio HTTP de Productos
//builder.Services.AddHttpClient<IProductIntegrationService, ProductIntegrationService>(client =>
//{
//    // AQUÍ EL PUERTO DONDE CORRE EL PROYECTO PRODUCT
//    client.BaseAddress = new Uri("http://localhost:5041/");
//});

// Registrar Servicio de Ventas
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<IGetOrdersService, GetOrdersService>();
// Program.cs o DependencyInjection.cs
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        // Usamos el endpoint de Amazon MQ
        cfg.Host(new Uri("amqps://b-f19a7ab2-7079-4163-b0c6-854fe583469f.mq.us-east-1.on.aws:5671"), h =>
        {
            // AQUÍ DEBES PONER EL USUARIO Y CONTRASEÑA QUE CREASTE EN AWS CONSOLE
            h.Username("admin");
            h.Password("martinez1234");

            h.UseSsl(s =>
            {
                s.Protocol = System.Security.Authentication.SslProtocols.Tls12;
            });
        });
    });
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar CORS para API Gateway
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowApiGateway", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sales API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowApiGateway");
app.UseAuthorization();
app.MapControllers();
app.Run();
