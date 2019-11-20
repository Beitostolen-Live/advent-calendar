using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using beitostolen_live_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Options;

namespace beitostolen_live_api.Services
{
    public class WinnerService : IWinnerService
	{
        private DbContextOptions<ApplicationDbContext> _ctxOptions;

        public WinnerService(
            DbContextOptions<ApplicationDbContext> ctxOptions)
        {
            _ctxOptions = ctxOptions;
        }

        public async Task<bool> Add(Winner winner)
        {
            using (var context = new ApplicationDbContext(_ctxOptions))
            {
                context.Attach(winner.Alternative);

                await context.Winners.AddAsync(winner);

                await context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<IList<Winner>> Get(int year)
        {
            using (var context = new ApplicationDbContext(_ctxOptions))
            {
                return await context.Winners
                    .Include(i => i.Alternative)
                    .Include(i => i.Alternative.Question)
                    .Where(w => w.Alternative.Registered.Year == year)
                    .ToListAsync();
            }
        }

        public async Task<Winner> GetWinner(int id)
        {
            using (var context = new ApplicationDbContext(_ctxOptions))
            {
                return await context.Winners
                    .Include(i => i.Alternative)
                    .Include(i => i.Alternative.Answear)
                    .Include(i => i.Alternative.Question)
                    .Include(i => i.Alternative.Question.Alternatives)
                    .FirstOrDefaultAsync(w => w.Id == id);
            }
        }

        public async Task<bool> Update(Winner winner, bool updateWinner = false)
        {
            using (var context = new ApplicationDbContext(_ctxOptions))
            {
                context.Attach(winner.Alternative);
                if(updateWinner)
                    context.Entry(winner.Alternative).State = EntityState.Modified;
                else
                    context.Entry(winner.Alternative).State = EntityState.Unchanged;

                context.Winners.Update(winner);

                await context.SaveChangesAsync();

                return true;
            }
        }
    }
}
