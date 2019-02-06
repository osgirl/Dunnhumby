using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Armin.Dunnhumby.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Armin.Dunnhumby.Domain.Data.Seed
{
    public class ProductsSeed
    {
        public static bool SeedData { get; set; } = true; 
        public static void Seed(ModelBuilder builder)
        {
            if (!SeedData) return;
            
            builder.Entity<Product>().HasData(
                new Product {Id = 1, Name = "Asus UX580", Price = 1750m});


            builder.Entity<Campaign>().HasData(
                new Campaign
                {
                    Id = 1,
                    Name = "Cyber Monday",
                    Start = DateTime.Now,
                    End = DateTime.Now.AddDays(10),
                    ProductId = 1
                },
                new Campaign
                {
                    Id = 2,
                    Name = "Expired Campaign",
                    Start = DateTime.Now.AddDays(-1),
                    End = DateTime.Now,
                    ProductId = 1
                }
            );
        }
    }
}