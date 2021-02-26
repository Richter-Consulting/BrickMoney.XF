using System.Collections.Generic;
using System.Threading.Tasks;
using BrickMoney.Models.Api;

namespace BrickMoney.Services
{
    public interface IApiService
    {
        Task<IList<SetBasicInfo>> GetNewSets();
    }
}
