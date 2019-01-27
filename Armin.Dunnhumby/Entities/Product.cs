using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Armin.Dunnhumby.Web.Entities
{
    public class Product : EntityBase
    {
        public Product()
        {
            Price = 0;
        }

        [Required]
        public string Name { get; set; }


        [Required]
        public decimal Price { get; set; }


        public string Description { get; set; }


        public virtual List<Campaign> Campaigns { get; set; }
    }
}
