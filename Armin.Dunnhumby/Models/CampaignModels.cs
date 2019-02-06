using System;
using System.ComponentModel.DataAnnotations;
using Armin.Dunnhumby.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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

        [Required] public string Name { get; set; }

        public string Description { get; set; }

        public int ProductId { get; set; }


        [Required] public DateTime Start { get; set; }

        public DateTime End { get; set; }
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

                Start = p.Start,
                End = p.End,
                Description = p.Description,
                Product = new CampaignProductOutputModel(p.ProductId, p.Product == null ? "" : p.Product.Name),
                IsActive = p.Start <= DateTime.Now && p.End >= DateTime.Now
            };

            return model;
        }


        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime? LastUpdate { get; set; }
        public string Description { get; set; }

        public CampaignProductOutputModel Product { get; set; } = new CampaignProductOutputModel();

        [JsonProperty]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Start { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime End { get; set; }

        public bool IsActive { get; set; }
    }

    public class CampaignProductOutputModel
    {
        public CampaignProductOutputModel()
        {
        }

        public CampaignProductOutputModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}