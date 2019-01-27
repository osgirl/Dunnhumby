using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Armin.Dunnhumby.Web.Data;
using Armin.Dunnhumby.Web.Entities;

namespace Armin.Dunnhumby.Tests.IntegrationTests
{
    public class Utilities
    {
        public static void PrepareDbForTests(ApplicationDbContext db)
        {
            var pro1 = new Product {Name = "Product 1", Price = 10};
            var pro2 = new Product {Name = "Product 2", Price = 20};
            var pro3 = new Product {Name = "Product 3", Price = 30};
            var pro4 = new Product {Name = "Product 4", Price = 40};

            pro1.Campaigns = new List<Campaign>
            {
                new Campaign
                {
                    Name = "Campaign 1 P1",
                    Start = DateTime.Now,
                    End = DateTime.Now.AddDays(5)
                },
                new Campaign
                {
                    Name = "Campaign 2 P1 Expired",
                    Start = DateTime.Now.AddDays(-2),
                    End = DateTime.Now.AddDays(-1)
                }
            };

            pro2.Campaigns = new List<Campaign>
            {
                new Campaign
                {
                    Name = "Campaign 3 P2",
                    Start = DateTime.Now,
                    End = DateTime.Now.AddDays(3)
                },
                new Campaign
                {
                    Name = "Campaign 4 P2 Future",
                    Start = DateTime.Now.AddDays(2),
                    End = DateTime.Now.AddDays(3)
                }
            };

            pro3.Campaigns = new List<Campaign>
            {
                new Campaign
                {
                    Name = "Campaign 5 P3",
                    Start = DateTime.Now,
                    End = DateTime.Now.AddDays(7)
                },
                new Campaign
                {
                    Name = "Campaign 6 P3",
                    Start = DateTime.Now.AddDays(-2),
                    End = DateTime.Now.AddDays(3)
                }
            };


            var products = new List<Product>
            {
                pro1,
                pro2,
                pro3,
                pro4
            };


            db.Products.AddRange(products);
            db.SaveChanges();
        }
    }
}