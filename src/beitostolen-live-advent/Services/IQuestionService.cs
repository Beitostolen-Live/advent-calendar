using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using beitostolen_live_api.Models;

namespace beitostolen_live_api.Services
{
    public interface IQuestionService
    {
        Task<IList<Question>> GetQuestions(ClaimsPrincipal user, bool disableCache);
        Task<Question> GetQuestion(DateTime dateTime, ClaimsPrincipal user, bool disableCache);
        Task<Question> GetQuestion(int id, ClaimsPrincipal user, bool disableCache);
        Task<Question> UpdateQuestion(Question question);
        Task AddQuestion(Question question);
    }
}
