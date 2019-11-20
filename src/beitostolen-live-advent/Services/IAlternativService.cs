using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using beitostolen_live_api.Models;

namespace beitostolen_live_api.Services
{
    public interface IAlternativService
    {
        Task<bool> Update(IList<Alternative> alternatives);
        Task<bool> Delete(IList<int> ids);
    }
}
