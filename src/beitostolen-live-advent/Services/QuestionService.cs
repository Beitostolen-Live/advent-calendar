using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using beitostolen_live_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace beitostolen_live_api.Services
{
    public class QuestionService : IQuestionService
    {
        private const string CACHEKEY = nameof(QuestionService);

        private readonly IMemoryCache _cache;
        private readonly DbContextOptions<ApplicationDbContext> _ctxOptions;

        public QuestionService(
            IMemoryCache cache,
            DbContextOptions<ApplicationDbContext> ctxOptions)
        {
            _cache = cache;
            _ctxOptions = ctxOptions;
        }

        public async Task AddQuestion(Question question)
        {
            using (var context = new ApplicationDbContext(_ctxOptions))
            {
                await context.Questions.AddAsync(question);

                await context.SaveChangesAsync();

                _cache.Remove(CACHEKEY);
            }
        }

        public async Task<IList<Question>> GetQuestions(ClaimsPrincipal user, bool disableCache)
        {
            return await FetchAllQuestion(user, disableCache);
        }

        public async Task<Question> GetQuestion(DateTime dateTime, ClaimsPrincipal user, bool disableCache)
        {
            var question = await FetchAllQuestion(user, disableCache);

            return question.Where(w => w.QuestionForDate == dateTime).FirstOrDefault();
        }

        public async Task<Question> GetQuestion(int id, ClaimsPrincipal user, bool disableCache)
        {
            var question = await FetchAllQuestion(user, disableCache);

            return question.Where(w => w.Id == id).FirstOrDefault();
        }

        public async Task<Question> UpdateQuestion(Question question)
        {
            using (var context = new ApplicationDbContext(_ctxOptions))
            {
                var updatedQuestion = context.Questions.Update(question);

                await context.SaveChangesAsync();

                _cache.Remove(CACHEKEY);

                return updatedQuestion.Entity;
            }
        }

        private async Task<IList<Question>> FetchAllQuestion(ClaimsPrincipal user, bool disableCache)
        {
            if(user.Identity.IsAuthenticated && disableCache)
            {
                var questions = await FetchQuestions();
                return questions.Questions;
            }

            var result = _cache.Get<QuestionList>(CACHEKEY);

            if(result == null)
            {
                result = await FetchQuestions();

                _cache.Set(CACHEKEY, result, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                });
            }

            return result.Questions;
        }

        private async Task<QuestionList> FetchQuestions()
        {
            using(var context = new ApplicationDbContext(_ctxOptions))
            {
                var questions = await context.Questions.Include(i => i.Alternatives).ToListAsync();

                return new QuestionList
                {
                    Questions = questions
                };
            }
        }
    }
}
