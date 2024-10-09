using DA.Entites;
using Microsoft.Data.SqlClient;

namespace DA.InitialSetup;

public static class DbInitializer
{
    public static void Seed(AppContext context)
    {
        int maxRetries = 5;
        int delay = 2000;

        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                context.Database.EnsureCreated();

                // Check if any products exist
                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                        new Product { Name = "Wireless Headphones", Price = 10.99M },
                        new Product { Name = "Smartwatch", Price = 19.99M },
                        new Product { Name = "Bluetooth Speaker", Price = 29.99M }
                    );

                    context.SaveChanges();
                }
                
                break; // Exit loop if successful
            }
            catch (SqlException)
            {
                if (i == maxRetries - 1) throw;
                Thread.Sleep(delay); 
            }
        }
    }
}
