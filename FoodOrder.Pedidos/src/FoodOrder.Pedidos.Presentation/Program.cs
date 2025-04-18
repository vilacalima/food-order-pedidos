using FoodOrder.Pedidos.Application.DependencyInjection;
using FoodOrder.Pedidos.Application.UseCase.Pedidos;
using FoodOrder.Pedidos.Application.UseCase.Pedidos.Interface;
using FoodOrder.Pedidos.Domain.Repository;
using FoodOrder.Pedidos.Infrastructure.Configurations;
using FoodOrder.Pedidos.Infrastructure.Data;
using FoodOrder.Pedidos.Infrastructure.DependencyInjection;
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
builder.Services.AddTransient<IPedidoUseCase, PedidoUseCase>();
builder.Services.AddTransient<IPedidoRepository, PedidoRepository>();
builder.Services.AddTransient<ISacolaRepository, SacolaRepository>();
builder.Services.AddTransient<ISacolaProdutoRepository, SacolaProdutoRepository>();
builder.Services.AddTransient<IPedidoStatusRepository, PedidoStatusRepository>();

//builder.Services.AddTransient<IPagtoWebhookUseCase, PagtoWebhookUseCase>();
//builder.Services.AddHttpClient<IMercadoPagoExternalService, MercadoPagoExternalService>();

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

app.UseAuthorization();
app.MapControllers();
app.Run();
