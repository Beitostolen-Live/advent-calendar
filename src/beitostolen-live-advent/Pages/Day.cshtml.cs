using System;
using System.Threading.Tasks;
using beitostolen_live_api.Models;
using beitostolen_live_api.Services;
using Microsoft.AspNetCore.Mvc;
using beitostolen_live_api.Extentions;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace beitostolen_live_api.Pages
{
    public class DayModel : FakeDateModelPage
    {
        private readonly IQuestionService _questionService;
        private readonly IResponseService _responseService;

        public DayModel(
            IOptions<AppSettings> appSettings,
            IQuestionService questionService,
            IResponseService responseService)
            : base(appSettings)
        {
            _questionService = questionService;
            _responseService = responseService;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public Question Question { get; set; }

        [BindProperty]
        public int SelectedAlternativId { get; set; }

        public bool IsActive => CurrentDate.BetweenDate(_startDate, _endDate) && CurrentDate.Day == Id;

        public async Task<IActionResult> OnGetAsync()
        {
            if(User.Identity.IsAuthenticated && !User.Claims.Any(c => c.Type == ClaimTypes.Email))
            {
                return Redirect($"/Account/signinfacebook?id={Id}");
            }

            Question = await _questionService.GetQuestion(new DateTime(DateTime.Today.Year, 12, Id), User, false);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Question = await _questionService.GetQuestion(new DateTime(DateTime.Today.Year, 12, Id), User, false);

            var selectedAlternative = Question.Alternatives.Where(w => w.Id == SelectedAlternativId).FirstOrDefault();
            var response = new Response
            {
                Name = User.Identity.Name,
                Email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                Question = Question,
                Registered = DateTime.UtcNow,
                Answear = selectedAlternative
            };

            var rs = await _responseService.AddResponse(response);

            return RedirectToPage("ThankYou");
        }
    }
}
