using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Armin.Dunnhumby.Domain.Entities;
using Armin.Dunnhumby.Domain.Helpers;
using Armin.Dunnhumby.Domain.Stores;

namespace Armin.Dunnhumby.Domain.Services
{
    public interface ICampaignService : ICampaignStore
    {
        PagedResult<Campaign> ActiveList(int page, int pageSize = 10);
    }
}
