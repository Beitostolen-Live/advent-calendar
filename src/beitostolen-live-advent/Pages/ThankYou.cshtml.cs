using System;
using System.Threading.Tasks;
using beitostolen_live_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace beitostolen_live_api.Pages
{
    public class ThankYouModel : FakeDateModelPage
    {
        private readonly AppSettings _appSettings;

        public ThankYouModel(
            IOptions<AppSettings> appSettings)
            : base(appSettings)
        {
            _appSettings = appSettings.Value;
        }

        [BindProperty(SupportsGet = true)]
        public string BaseUrl { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await Task.Delay(0);

            BaseUrl = _appSettings.BaseUrl;

            return Page();
        }
    }
}
