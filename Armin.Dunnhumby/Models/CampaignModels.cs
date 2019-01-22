using System;
using System.ComponentModel.DataAnnotations;
using Armin.Dunnhumby.Web.Entities;
using Microsoft.AspNetCore.Http;

namespace Armin.Dunnhumby.Web.Models
{
    public class CampaignInputModel
    {
        public Campaign ToEntity()
        {
            return new Campaign
            {
                Name = Name,
                Description = Description,
                End = End,
                ProductId = ProductId,
                Start = Start
            };
        }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int ProductId { get; set; }


        [Required]
        public DateTime Start { get; set; }

        public DateTime? End { get; set; }
    }


    public class CampaignOutputModel
    {
        public static CampaignOutputModel FromEntity(Campaign p)
        {
            var model = new CampaignOutputModel
            {
                Id = p.Id,
                LastUpdate = p.LastUpdate,
                Name = p.Name,
                ProductId = p.ProductId,
                Start = p.Start,
                End = p.End,
                Description = p.Description,
                ProductName = p.Product == null ? "" : p.Product.Name
            };

            return model;
        }



        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime? LastUpdate { get; set; }
        public string Description { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

    }
}