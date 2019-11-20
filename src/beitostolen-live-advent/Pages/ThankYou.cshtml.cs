using System;
using System.Threading.Tasks;
using beitostolen_live_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace beitostolen_live_api.Pages
{
    public class ThankYouModel : FakeDateModelPage
    {
        public ThankYouModel(
            IOptions<AppSettings> appSettings)
            : base(appSettings)
        { }

        public async Task<IActionResult> OnGetAsync()
        {
            await Task.Delay(0);

            return Page();
        }
    }
}
