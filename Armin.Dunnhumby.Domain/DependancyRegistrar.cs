using Armin.Dunnhumby.Domain.Services;
using Armin.Dunnhumby.Domain.Stores;

using Microsoft.Extensions.DependencyInjection;

namespace Armin.Dunnhumby.Domain
{
    public static class DependancyRegisterar
    {
        public static void AddDependancies(this IServiceCollection service)
        {
            service.AddScoped<IProductStore, ProductStore>();
            service.AddScoped<ICampaignStore, CampaignStore>();
            service.AddScoped<ICampaignService, CampaignService>();

        }
    }
}
