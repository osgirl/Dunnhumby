using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Armin.Dunnhumby.Web.Entities
{
    public class Campaign : EntityBase
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }


        [Required]
        public DateTime Start { get; set; }

        public DateTime? End { get; set; }
    }
}
