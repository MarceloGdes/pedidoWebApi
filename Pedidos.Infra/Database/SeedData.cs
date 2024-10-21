
using Pedidos.Domain.Entities;
using Pedidos.Infra.Database.Context;

namespace Pedidos.Infra.Database;
public static class SeedData
{
    //Populando a tabela de produto

    public static void Seed(ApiContext context)
    {

        context.Products.AddRange(

            new Product { Name = "Notebook", Price = 3000 },
            new Product { Name = "Smartphone", Price = 1575.80 },
            new Product { Name = "Monitor", Price = 776.89 }

        );

        context.SaveChanges();
    }
}
