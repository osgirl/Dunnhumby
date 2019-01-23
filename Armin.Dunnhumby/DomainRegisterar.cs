using Armin.Dunnhumby.Web.Filters;
using Armin.Dunnhumby.Web.Models;
using Armin.Dunnhumby.Web.Services;
using Armin.Dunnhumby.Web.Stores;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Armin.Dunnhumby.Web
{
    public static class DependancyRegisterar
    {
        public static void AddDependancies(this IServiceCollection service)
        {
            service.AddScoped<IProductStore, ProductStore>();
            service.AddScoped<ICampaignStore, CampaignStore>();
            service.AddScoped<ICampaignService, CampaignService>();

            service.AddTransient<IValidator<CampaignInputModel>, CampaignValidator>();
        }
    }
}
