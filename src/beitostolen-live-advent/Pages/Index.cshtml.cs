using System;
using beitostolen_live_api.Extentions;
using beitostolen_live_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace beitostolen_live_api.Pages
{
    public class IndexModel : FakeDateModelPage
    {
        public IndexModel(
            IOptions<AppSettings> appSettings)
            : base(appSettings)
        {
        }

        [BindProperty]
        public bool IsActive => CurrentDate.BetweenDate(_startDate, _endDate);
    }
}
