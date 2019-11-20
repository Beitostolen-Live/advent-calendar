using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using beitostolen_live_api.Models;

namespace beitostolen_live_api.Services
{
    public interface IWinnerService
    {
        Task<bool> Add(Winner winner);

        Task<IList<Winner>> Get(int year);

        Task<Winner> GetWinner(int id);

        Task<bool> Update(Winner winner, bool updateWinner = false);
    }
}
