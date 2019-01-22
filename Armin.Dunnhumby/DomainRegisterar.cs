using Armin.Dunnhumby.Web.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace Armin.Dunnhumby.Web
{
    public static class DependancyRegisterar
    {
        public static void AddDependancies(this IServiceCollection service)
        {
            service.AddScoped<IProductStore, ProductStore>();
            service.AddScoped<ICampaignStore, CampaignStore>();

        }
    }
}
