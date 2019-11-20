using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using beitostolen_live_api.Models;
using beitostolen_live_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace beitostolen_live_api.Pages.Admin.Responses
{
    public class WinnerModel : PageModel
    {
        private readonly IWinnerService _winnerService;
        private readonly IResponseService _responseService;

        public WinnerModel(
            IWinnerService winnerService,
            IResponseService responseService)
        {
            _winnerService = winnerService;
            _responseService = responseService;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public Winner Winner { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<SelectListItem> StatusList { get; set; }

        public async  Task<IActionResult> OnGetAsync()
        {
            await Load();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _winnerService.Update(Winner);

            await Load();

            return Page();
        }

        public async Task<IActionResult> OnPostAsyncRepick()
        {
            var responses = await _responseService.GetResponses(Winner.StartDate, Winner.EndDate, Winner.OnlyWithCorrectAnswer);

            var random = new Random();
            var winnerIndex = random.Next(0, responses.Count - 1);

            Winner.Status = WinnerStatus.InProgress;
            Winner.Alternative = responses[winnerIndex];

            _ = await _winnerService.Update(Winner, true);

            await Load();

            return Page();
        }

        private async Task Load()
        {
            StatusList = Enum.GetValues(typeof(WinnerStatus)).Cast<WinnerStatus>()
                .Select(s => new SelectListItem
                {
                    Text = s.ToString(),
                    Value = s.ToString()
                }).ToList();

            Winner = await _winnerService.GetWinner(Id);
        }
    }
}
