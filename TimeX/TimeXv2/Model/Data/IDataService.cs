using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeXv2.Model.Data
{
    public interface IDataService
    {
        Task<string> AddActionAsync(Action value);
        Task<bool> DeleteActionAsync(string uid);
        Task<Action> GetActionByUidAsync(string uid);
        Task<Checkpoint> GetCheckpointByUidAsync(string uid);
        Task<List<Action>> GetActionsListAsync(bool isFullLoad = false);
        IQueryable<Action> QueryableActions();
        Task<bool> UpdateActionAsync(Action value);
        Task<bool> UpdateCheckpointAsync(Checkpoint value);
    }
}
