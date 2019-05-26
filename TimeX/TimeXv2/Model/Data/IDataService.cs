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
        Task<List<Action>> GetActionsListAsync(bool isFullLoad = false);
        Task<Checkpoint> GetCheckpointByUidAsync(int uid);
        string GetDataBasePath();
        IQueryable<Action> QueryableActions();
        /// <summary>
        /// Set new path in connectionString
        /// </summary>
        /// <param name="newPath">Path</param>
        /// <returns>Path updated</returns>
        Task<bool> SetDataBasePathAsync(string newPath);
        Task<bool> UpdateActionAsync(Action value);
        Task<bool> UpdateCheckpointAsync(Checkpoint value);
    }
}
