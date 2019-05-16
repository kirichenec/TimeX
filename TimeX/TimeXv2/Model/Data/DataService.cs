using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using TimeXv2.Extensions;

namespace TimeXv2.Model.Data
{
    public class DataService : IDataService
    {
        #region ctor
        public DataService(ActionContext context)
        {
            _actionContext = context;
        }
        #endregion

        #region Services
        private readonly ActionContext _actionContext;
        #endregion

        #region Fields
        private const double _debugDelaySeconds = 3;
        #endregion

        #region Methods

        #region DebugDelay
        private static async Task DebugDelay()
        {
#if DEBUG
            await Task.Delay(TimeSpan.FromSeconds(_debugDelaySeconds)).ConfigureAwait(false);
#endif
        }
        #endregion

        #region GetActionByUidAsync
        public async Task<Action> GetActionByUidAsync(string uid)
        {
            await DebugDelay();

            var result =
                uid == null ?
                null :
                await _actionContext.Actions.Include(a => a.Checkpoints).FirstOrDefaultAsync(a => a.Uid == uid).ConfigureAwait(false);
            return result;
        }
        #endregion

        #region GetActionsListAsync
        public async Task<List<Action>> GetActionsListAsync(bool isFullLoad)
        {
            await DebugDelay();

            try
            {
                DbQuery<Action> query = _actionContext.Actions;
                if (isFullLoad)
                {
                    query = query.Include($"{nameof(Checkpoint)}s");
                }
                var result = await query.ToListAsync().ConfigureAwait(false);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region GetCheckpointByUidAsync
        public async Task<Checkpoint> GetCheckpointByUidAsync(string uid)
        {
            await DebugDelay();
            try
            {
                var result = await _actionContext.Checkpoints.Include(c => c.ParentAction).FirstOrDefaultAsync(chk => chk.Uid == uid).ConfigureAwait(false);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region QueryableActions
        public IQueryable<Action> QueryableActions()
        {
            return _actionContext.Actions.AsQueryable();
        }
        #endregion

        #region AddActionAsync
        public async Task<string> AddActionAsync(Action value)
        {
            await DebugDelay();

            try
            {
                value.Uid = Guid.NewGuid().ToString();
                _actionContext.Actions.Add(value);
                await _actionContext.SaveChangesAsync().ConfigureAwait(false);
                return value.Uid;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region DeleteActionAsync
        public async Task<bool> DeleteActionAsync(string uid)
        {
            await DebugDelay();

            try
            {
                _actionContext.Actions.Remove(await GetActionByUidAsync(uid).ConfigureAwait(false));
                await _actionContext.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region UpdateActionAsync
        public async Task<bool> UpdateActionAsync(Action value, bool firstTry = true)
        {
            await DebugDelay();

            try
            {
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

                await _actionContext.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch (EntityException dbException)
            {
                var sqliteException = dbException.InnerException as SQLiteException;
                if (sqliteException?.ErrorCode == 5)
                {
                    if (firstTry)
                    {
                        return await UpdateActionAsync(value, false).ConfigureAwait(false);
                    }
                }
                return false;
            }
            catch (DbUpdateConcurrencyException)
            {
                // data removed or changed
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region UpdateCheckpointAsync
        public async Task<bool> UpdateCheckpointAsync(Checkpoint value)
        {
            await DebugDelay();

            try
            {
                var updatableCheckpoint = await GetCheckpointByUidAsync(value.Uid).ConfigureAwait(false);

                value.CopyPropertiesTo(updatableCheckpoint, parent: value.ParentAction);

                await _actionContext.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #endregion
    }
}