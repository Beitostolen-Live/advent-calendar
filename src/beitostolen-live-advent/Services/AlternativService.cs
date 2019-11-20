using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using beitostolen_live_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace beitostolen_live_api.Services
{
    public class AlternativService : IAlternativService
	{
        private DbContextOptions<ApplicationDbContext> _ctxOptions;

        public AlternativService(
            DbContextOptions<ApplicationDbContext> ctxOptions)
        {
            _ctxOptions = ctxOptions;
        }

        public async Task<bool> Delete(IList<int> ids)
        {
            using (var context = new ApplicationDbContext(_ctxOptions))
            {
                var tmp = new List<Alternative>();

                foreach(var id in ids)
                {
                    var deleted = await context.Alternatives.Where(w => w.Id == id).FirstOrDefaultAsync();

                    if(deleted != null)
                        tmp.Add(deleted);
                }

                context.Alternatives.RemoveRange(tmp);

                await context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<bool> Update(IList<Alternative> alternatives)
        {
            using (var context = new ApplicationDbContext(_ctxOptions))
            {
                context.Alternatives.UpdateRange(alternatives);

                await context.SaveChangesAsync();

                return true;
            }
        }
    }
}
