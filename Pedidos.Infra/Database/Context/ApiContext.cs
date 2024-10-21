using Microsoft.EntityFrameworkCore;
using Pedidos.Domain.Entities;


namespace Pedidos.Infra.Database.Context;
public class ApiContext(DbContextOptions<ApiContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderProduct> OrderProducts { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Configurando a Entidade OrderProduct 
        modelBuilder.Entity<OrderProduct>()
            .HasKey(op => new { op.OrderId, op.ProductId }); //PK composta

        //Configurando relacionamento
        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId);

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Product)
            .WithMany() //Sem colleção de OrderProduct na entidade Product
            .HasForeignKey(op => op.ProductId);

    }
}
