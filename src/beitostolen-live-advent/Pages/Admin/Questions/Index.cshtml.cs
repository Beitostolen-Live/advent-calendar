using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using beitostolen_live_api.Extentions;
using beitostolen_live_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace beitostolen_live_api.Pages.Admin.Questions
{
    public class IndexModel : PageModel
    {
        private readonly IQuestionService _questionService;

        public IndexModel(
            IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [BindProperty]
        public string Question { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        [BindProperty]
        public DateTime Date { get; set; } = new DateTime(DateTime.Today.Year, 12, 1);

        [BindProperty]
        public string Alternative1 { get; set; }

        [BindProperty]
        public bool IsCirrect1 { get; set; }

        [BindProperty]
        public string Alternative2 { get; set; }

        [BindProperty]
        public bool IsCirrect2 { get; set; }

        [BindProperty]
        public string Alternative3 { get; set; }

        [BindProperty]
        public bool IsCirrect3 { get; set; }

        [BindProperty(SupportsGet = true)]
        public IList<Models.Question> Questions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Questions = await _questionService.GetQuestions(User, false);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var q = new Models.Question
            {
                Text = Question,
                QuestionForDate = Date,
                Image = ImageFile != null ? ImageFile.GetBase64HtmlString() : string.Empty,
                Alternatives = new List<Models.Alternative>
                {
                    new Models.Alternative
                    {
                        Description = Alternative1,
                        IsCorrect = IsCirrect1
                    },
                    new Models.Alternative
                    {
                        Description = Alternative2,
                        IsCorrect = IsCirrect2
                    },
                    new Models.Alternative
                    {
                        Description = Alternative3,
                        IsCorrect = IsCirrect3
                    }
                }
            };

            await _questionService.AddQuestion(q);

            return RedirectToPage("/Admin/Questions");
        }
    }
}
