using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using beitostolen_live_api.Extentions;
using beitostolen_live_api.Models;
using beitostolen_live_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace beitostolen_live_api.Pages.Admin.Questions
{
    public class EditModel : PageModel
    {
        private readonly IQuestionService _questionService;
        private readonly IAlternativService _alternativService;

        public EditModel(
            IQuestionService questionService,
            IAlternativService alternativService)
        {
            _questionService = questionService;
            _alternativService = alternativService;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public DateTime Date { get; set; }

        [BindProperty]
        public string Question { get; set; }

        [BindProperty]
        public string ImageString { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        [BindProperty]
        public IList<AlternativModel> Alternatives { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var question = await _questionService.GetQuestion(Id, User, false);

            var tmp = new List<AlternativModel>();
            foreach (var a in question.Alternatives)
            {
                tmp.Add(new AlternativModel { Alternative = a, ShouldDelete = false });
            }


            Question = question.Text;
            Date = question.QuestionForDate;
            ImageString = question.Image;
            Alternatives = tmp;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var editQuestion = new Question
            {
                Id = Id,
                Image = ImageFile == null ? ImageString : ImageFile.GetBase64HtmlString(),
                QuestionForDate = Date,
                Text = Question,
                Alternatives = Alternatives.Select(s => s.Alternative).ToList()
            };

            var editAlternatives =
                Alternatives.Where(w => w.ShouldDelete == false)
                            .Select(s => new Alternative
                            {
                                Id = s.Alternative.Id,
                                Description = s.Alternative.Description,
                                IsCorrect = s.Alternative.IsCorrect,
                                Question = s.Alternative.Question
                            }).ToList();
            var deleteAlternatives = Alternatives
                .Where(w => w.ShouldDelete)
                .Select(s => s.Alternative.Id)
                .ToList();

            var questionTask = _questionService.UpdateQuestion(editQuestion);
            var deleteAlternativTask = _alternativService.Delete(deleteAlternatives);
            //var editAlternativTask = _alternativService.Update(editAlternatives);

            await Task.WhenAll(questionTask, deleteAlternativTask);

            return RedirectToPage("/Admin/Questions/Index");
        }

        public class AlternativModel
        {
            public Alternative Alternative { get; set; }
            public bool ShouldDelete { get; set; }
        }
    }
}
