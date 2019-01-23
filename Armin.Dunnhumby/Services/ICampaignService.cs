using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Armin.Dunnhumby.Web.Entities;
using Armin.Dunnhumby.Web.Helpers;
using Armin.Dunnhumby.Web.Stores;

namespace Armin.Dunnhumby.Web.Services
{
    public interface ICampaignService : ICampaignStore
    {
        PagedResult<Campaign> ActiveList(int page, int pageSize = 10);
    }
}
