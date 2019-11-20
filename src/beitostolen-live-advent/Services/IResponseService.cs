using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using beitostolen_live_api.Models;

namespace beitostolen_live_api.Services
{
    public interface IResponseService
    {
        Task<IList<Response>> GetResponses(int year);
        Task<IList<Response>> GetResponses(DateTime startDate, DateTime endDate, bool withCorrectAnswer);
        Task<bool> AddResponse(Response response);
    }
}
