using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeXv2.Model.Data
{
    public interface IDataService
    {
        Task<bool> AddActionAsync(Action value);
        Task<bool> DeleteActionAsync(int uid);
        Task<Action> GetActionByUidAsync(int uid);
        Task<Checkpoint> GetCheckpointByUidAsync(int uid);
        Task<List<Action>> GetActionsListAsync(bool isFullLoad = false);
        IQueryable<Action> QueryableActions();
        // TODO: Make first try in all methods
        Task<bool> UpdateActionAsync(Action value, bool firstTry = true);
        Task<bool> UpdateCheckpointAsync(Checkpoint value);
    }
}
