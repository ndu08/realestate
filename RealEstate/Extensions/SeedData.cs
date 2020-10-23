using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Extensions
{
    public static class SeedData
    {
        public static void EnsureSeeded(this IApplicationBuilder app)
        {
            using (var service = app.ApplicationServices.CreateScope())
            {
                var dbContext = service.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                dbContext.Database.Migrate();

                if (!dbContext.Services.Any())
                {
                    dbContext.Services.AddRange(
                        new Service { Type = "Rent" },
                        new Service { Type = "Sell" },
                        new Service { Type = "Buy" }
                        );

                    dbContext.SaveChanges();
                }
            }
        }
    }
}
