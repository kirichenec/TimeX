using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using TimeXv2.Extensions;

namespace TimeXv2.Model.Data
{
    public class DataService : IDataService
    {
        #region ctor
        public DataService(ActionContext timeXcontext)
        {
            _timeXcontext = timeXcontext;
        }
        #endregion

        #region Services
        private readonly ActionContext _timeXcontext;
        #endregion

        #region Fields
        private const double _debugDelaySeconds = 1;
        #endregion

        #region Methods

        #region AddActionAsync
        public async Task<bool> AddActionAsync(Action value)
        {
            await DebugDelay();

            _timeXcontext.Actions.Add(value);
            var result = await _timeXcontext.SaveChangesAsync().ConfigureAwait(false);
            return result > 0;
        }
        #endregion

        #region DebugDelay
        private static async Task DebugDelay()
        {
#if DEBUG
            await Task.Delay(TimeSpan.FromSeconds(_debugDelaySeconds)).ConfigureAwait(false);
#endif
        }
        #endregion

        #region DeleteActionAsync
        public async Task<bool> DeleteActionAsync(int uid)
        {
            await DebugDelay();

            _timeXcontext.Actions.Remove(await GetActionByUidAsync(uid).ConfigureAwait(false));
            await _timeXcontext.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
        #endregion

        #region GetActionByUidAsync
        public async Task<Action> GetActionByUidAsync(int uid)
        {
            await DebugDelay();

            var result =
                    uid == 0 ?
                    null :
                    await _timeXcontext.Actions.Include(a => a.Checkpoints).FirstOrDefaultAsync(a => a.Uid == uid).ConfigureAwait(false);
            return result;
        }
        #endregion

        #region GetActionsListAsync
        public async Task<List<Action>> GetActionsListAsync(bool isFullLoad)
        {
            await DebugDelay();

            DbQuery<Action> query = _timeXcontext.Actions;
            if (isFullLoad)
            {
                query = query.Include($"{nameof(Checkpoint)}s");
            }
            var result = await query.ToListAsync().ConfigureAwait(false);
            return result;
        }
        #endregion

        #region GetCheckpointByUidAsync
        public async Task<Checkpoint> GetCheckpointByUidAsync(int uid)
        {
            await DebugDelay();

            var result = await _timeXcontext.Checkpoints.Include(c => c.ParentAction).FirstOrDefaultAsync(chk => chk.Uid == uid).ConfigureAwait(false);
            return result;
        }
        #endregion

        #region GetDataBaseConnectionString
        public string GetDataBasePath()
        {
            DbConnectionStringBuilder builder = new DbConnectionStringBuilder
            {
                ConnectionString = _timeXcontext.Database.Connection.ConnectionString
            };

            return builder["Data Source"] as string;
        }
        #endregion

        #region QueryableActions
        public IQueryable<Action> QueryableActions()
        {
            return _timeXcontext.Actions.AsQueryable();
        }
        #endregion

        #region SetDataBaseConnectionString
        public async Task<bool> SetDataBasePathAsync(string newPath)
        {
            var newConnectionString = "Data Source=" + newPath;
            if (_timeXcontext.Database.Connection.ConnectionString == newConnectionString)
            {
                return await Task.FromResult(false);
            }

            _timeXcontext.Database.Connection.ConnectionString = newConnectionString;

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings[$"{nameof(TimeXv2)}Context"].ConnectionString = _timeXcontext.Database.Connection.ConnectionString;
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
            return await Task.FromResult(true);
        }
        #endregion

        #region UpdateActionAsync
        public async Task<bool> UpdateActionAsync(Action value)
        {
            await DebugDelay();

            var updatableAction = await GetActionByUidAsync(value.Uid).ConfigureAwait(false);

            updatableAction.Name = value.Name;
            updatableAction.StartTime = value.StartTime;

            foreach (var chk in updatableAction.Checkpoints)
            {
                var tempChk = value.Checkpoints.FirstOrDefault(vChk => vChk.Uid == chk.Uid);
                if (tempChk == null)
                {
                    updatableAction.Checkpoints.Remove(chk);
                }
                else
                {
                    chk.CheckedDate = tempChk.CheckedDate;
                    chk.Duration = tempChk.Duration;
                    chk.IsOrderNeeded = tempChk.IsOrderNeeded;
                    chk.Name = tempChk.Name;
                    chk.Order = tempChk.Order;
                    chk.StartTime = tempChk.StartTime;
                }
            }

            var updatableChkUids = updatableAction.Checkpoints.Select(ch => ch.Uid);
            var newCheckpoints = value.Checkpoints.Where(chk => !updatableChkUids.Contains(chk.Uid));
            newCheckpoints.ForEach(newChk => updatableAction.Checkpoints.Add(new Checkpoint(newChk, parent: updatableAction)));

            await _timeXcontext.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
        #endregion

        #region UpdateCheckpointAsync
        public async Task<bool> UpdateCheckpointAsync(Checkpoint value)
        {
            await DebugDelay();

            var updatableCheckpoint = await GetCheckpointByUidAsync(value.Uid).ConfigureAwait(false);

            value.CopyPropertiesTo(updatableCheckpoint, parent: value.ParentAction);

            await _timeXcontext.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
        #endregion

        #endregion
    }
}