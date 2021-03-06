﻿using Armin.Dunnhumby.Domain.Data.Seed;
using Armin.Dunnhumby.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Armin.Dunnhumby.Domain.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ProductsSeed.Seed(builder);
            base.OnModelCreating(builder);
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Campaign> Campaigns { get; set; }
    }
}
