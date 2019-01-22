using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Armin.Dunnhumby.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace Armin.Dunnhumby.Web.Data.Seed
{
    // https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding
    public class ProductsSeed
    {
        public static void Seed(ModelBuilder builder)
        {
            builder.Entity<Product>().HasData(
                new Product {Id = 1, Name = "Asus UX580", Price = 1750m});


            builder.Entity<Campaign>().HasData(
                new Campaign { Id = 1, Name = "Cyber Monday", Start = DateTime.Now, ProductId = 1});
        }
    }
}