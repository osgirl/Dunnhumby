using Armin.Dunnhumby.Web.Filters;
using Armin.Dunnhumby.Web.Models;
using Armin.Dunnhumby.Domain.Services;
using Armin.Dunnhumby.Domain.Stores;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Armin.Dunnhumby.Web
{
    public static class DependancyRegistrar
    {
        public static void AddDependancies(this IServiceCollection service)
        {
            Domain.DependancyRegisterar.AddDependancies(service);

            service.AddTransient<IValidator<CampaignInputModel>, CampaignValidator>();
        }
    }
}
