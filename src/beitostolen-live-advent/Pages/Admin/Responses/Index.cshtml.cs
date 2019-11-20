using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using beitostolen_live_api.Models;
using beitostolen_live_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace beitostolen_live_api.Pages.Admin.Responses
{
    public class IndexModel : PageModel
    {
        private readonly IWinnerService _winnerService;
        private readonly IResponseService _responseService;

        public IndexModel(
            IWinnerService winnerService,
            IResponseService responseService)
        {
            _winnerService = winnerService;
            _responseService = responseService;
        }

        [BindProperty]
        public Winner NewWinner { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<Winner> Winners { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<ResponseGroup> Responses { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Winners = await _winnerService.Get(DateTime.UtcNow.Year);
            var responses = await _responseService.GetResponses(2019);

            Responses = responses.GroupBy(g => g.Question)
                        .Select(s => new ResponseGroup
                        {
                            Question = s.Key,
                            Responses = s.ToList()
                        }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var responses = await _responseService.GetResponses(NewWinner.StartDate, NewWinner.EndDate, NewWinner.OnlyWithCorrectAnswer);

            var random = new Random();
            var winnerIndex = random.Next(0, responses.Count-1);

            NewWinner.Status = WinnerStatus.InProgress;
            NewWinner.Alternative = responses[winnerIndex];

            await _winnerService.Add(NewWinner);

            return Page();
        }

        public class ResponseGroup
        {
            public Question Question { get; set; }
            public IList<Response> Responses { get; set; }
        }
    }
}
