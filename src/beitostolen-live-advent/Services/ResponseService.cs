using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using beitostolen_live_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace beitostolen_live_api.Services
{
    public class ResponseService : IResponseService
	{
        private DbContextOptions<ApplicationDbContext> _ctxOptions;

        public ResponseService(
            DbContextOptions<ApplicationDbContext> ctxOptions)
        {
            _ctxOptions = ctxOptions;
        }

        public async Task<bool> AddResponse(Response response)
        {
            using (var context = new ApplicationDbContext(_ctxOptions))
            {
                var hasResponded = context.Responses
                    .Any(w => w.Email == response.Email && w.Question.Id == response.Question.Id);

                if (hasResponded) return false;

                context.Attach(response.Question);
                context.Attach(response.Answear);

                await context.Responses.AddAsync(response);

                var rs = await context.SaveChangesAsync();

                return rs > 0;
            }
        }

        public async Task<IList<Response>> GetResponses(int year)
        {
            using(var context = new ApplicationDbContext(_ctxOptions))
            {
                return await context.Responses
                    .Include(i => i.Question)
                    .Include(i => i.Answear)
                    .Where(w => w.Registered.Year == year)
                    .OrderBy(o => o.Question.QuestionForDate)
                    .ToListAsync();
            }
        }

        public async Task<IList<Response>> GetResponses(DateTime startDate, DateTime endDate, bool withCorrectAnswer)
        {
            using (var context = new ApplicationDbContext(_ctxOptions))
            {
                if (withCorrectAnswer)
                {
                    return await context.Responses
                        .Where(w =>
                            w.Registered.Date >= startDate.Date &&
                            w.Registered.Date <= endDate.Date &&
                            w.Answear.IsCorrect == true)
                        .OrderBy(o => o.Question.QuestionForDate)
                        .ToListAsync();
                }

                return await context.Responses
                        .Where(w =>
                            w.Registered.Date >= startDate.Date &&
                            w.Registered.Date <= endDate.Date)
                        .OrderBy(o => o.Question.QuestionForDate)
                        .ToListAsync();
            }
        }
    }
}
