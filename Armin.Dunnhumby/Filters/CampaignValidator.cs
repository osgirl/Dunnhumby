using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Armin.Dunnhumby.Web.Models;
using FluentValidation;

namespace Armin.Dunnhumby.Web.Filters
{
    public class CampaignValidator : AbstractValidator<CampaignInputModel>
    {
        public CampaignValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty()
                .WithMessage("Name is Required");

            RuleFor(m => m.ProductId)
                .NotEmpty()
                .WithMessage("Product Id is Required");

            RuleFor(m => m.Start)
                .NotEmpty()
                .WithMessage("Start Date is Required");

            RuleFor(m => m.End)
                .NotEmpty()
                .WithMessage("End date is required")
                .GreaterThan(m => m.Start)
                .WithMessage("End date must after Start date");
        }
    }
}
