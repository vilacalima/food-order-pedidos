using Amazon.SQS;
using FoodOrder.Pedidos.Application.Feature.Checkout;
using FoodOrder.Pedidos.Application.Interfaces;
using FoodOrder.Pedidos.Application.Service;
using FoodOrder.Pedidos.Application.UseCase.Pedidos;
using FoodOrder.Pedidos.Application.UseCase.Pedidos.Interface;
using FoodOrder.Pedidos.Domain.Messaging;
using FoodOrder.Pedidos.Domain.Repository;
using FoodOrder.Pedidos.Infrastructure.Configurations;
using FoodOrder.Pedidos.Infrastructure.Data;
using FoodOrder.Pedidos.Infrastructure.Messaging.Producers;
using FoodOrder.Pedidos.Infrastructure.Repository;
using FoodOrder.Pedidos.Presentation.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IConnectionStringProvider, ConnectionStringProvider>();
builder.Services.AddDbContext<PedidosDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(CheckoutCommand).Assembly));
builder.Services.AddTransient<IPedidoUseCase, PedidoUseCase>();
builder.Services.AddTransient<IPedidoRepository, PedidoRepository>();
builder.Services.AddTransient<ISacolaRepository, SacolaRepository>();
builder.Services.AddTransient<ISacolaProdutoRepository, SacolaProdutoRepository>();
builder.Services.AddHttpClient<IProdutoHttpService, ProdutoHttpService>();

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonSQS>();
builder.Services.AddTransient<ISqsMessageSender, SqsMessageSender>();

builder.Services.AddLogging(configure => {
    configure.AddConsole();
    configure.AddDebug();
});

var app = builder.Build();

// Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ApplyMigrations();
app.UseAuthorization();
app.MapControllers();
app.Run();