using FoodOrder.Pedidos.Domain.Entities;
using FoodOrder.Pedidos.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FoodOrder.Pedidos.Infrastructure.Data
{
    public class PedidosDbContext : DbContext
    {
        private readonly string _connectionString;
        private readonly IConnectionStringProvider _connectionStringProvider;

        public PedidosDbContext(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public PedidosDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(_connectionString))
            {
                optionsBuilder.UseNpgsql(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("numero_pedido_seq", schema: "public")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.ToTable("pedidos");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.NumeroPedido)
                      .HasDefaultValueSql("nextval('public.numero_pedido_seq')");
                entity.Property(p => p.ClienteId).IsRequired();
                entity.Property(p => p.PedidoStatusId).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoStatus> PedidoStatus { get; set; }
        public DbSet<Sacola> Sacola { get; set; }
        public DbSet<SacolaProduto> SacolasProdutos { get; set; }

    }
}
