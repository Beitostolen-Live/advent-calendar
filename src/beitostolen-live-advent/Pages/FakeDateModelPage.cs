using System;
using beitostolen_live_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace beitostolen_live_api.Pages
{
    public abstract class FakeDateModelPage : PageModel
    {
        private DateTime _currentDate = DateTime.Today;
        private readonly AppSettings _appSettings;

        protected readonly DateTime _startDate = new DateTime(DateTime.Today.Year, 12, 1);
        protected readonly DateTime _endDate = new DateTime(DateTime.Today.Year, 12, 24);

        public FakeDateModelPage(
            IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime CurrentDate
        {
            get
            {
                return _currentDate;
            }
            set
            {
                _currentDate = _appSettings.TestingMode ? value : DateTime.Today;
            }
        }
    }
}
