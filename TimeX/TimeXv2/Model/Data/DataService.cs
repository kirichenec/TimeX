using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using TimeXv2.Extensions;
using UniversalKLibrary.Classic.Helpers;

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

        #region Methods

        #region GetActionByUidAsync
        public async Task<Action> GetActionByUidAsync(string uid)
        {
            Static.Properties.Instance.IsQueryExecuted = false;
            var result = await _actionContext.Actions.Include($"{nameof(Checkpoint)}s").FirstOrDefaultAsync(a => a.Uid == uid);
            Static.Properties.Instance.IsQueryExecuted = true;
            return result;
        }
        #endregion

        #region GetActionsListAsync
        public async Task<List<Action>> GetActionsListAsync(bool isFullLoad)
        {
            Static.Properties.Instance.IsQueryExecuted = false;
            try
            {
                DbQuery<Action> query = _actionContext.Actions;
                if (isFullLoad)
                {
                    query = query.Include($"{nameof(Checkpoint)}s");
                }
                var result = await query.ToListAsync();
                Static.Properties.Instance.IsQueryExecuted = true;
                return result;
            }
            catch (Exception)
            {
                Static.Properties.Instance.IsQueryExecuted = true;
                return null;
            }
        }
        #endregion

        #region GetCheckpointByUidAsync
        public async Task<Checkpoint> GetCheckpointByUidAsync(string uid)
        {
            Static.Properties.Instance.IsQueryExecuted = false;
            var result = await _actionContext.Checkpoints.FirstOrDefaultAsync(chk => chk.Uid == uid);
            Static.Properties.Instance.IsQueryExecuted = true;
            return result;
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
            Static.Properties.Instance.IsQueryExecuted = false;
            try
            {
                value.Uid = Guid.NewGuid().ToString();
                _actionContext.Actions.Add(value);
                await _actionContext.SaveChangesAsync();
                Static.Properties.Instance.IsQueryExecuted = true;
                return value.Uid;
            }
            catch (Exception)
            {
                Static.Properties.Instance.IsQueryExecuted = true;
                return null;
            }
        }
        #endregion

        #region DeleteActionAsync
        public async Task<bool> DeleteActionAsync(string uid)
        {
            Static.Properties.Instance.IsQueryExecuted = false;
            try
            {
                _actionContext.Actions.Remove(await GetActionByUidAsync(uid));
                await _actionContext.SaveChangesAsync();
                Static.Properties.Instance.IsQueryExecuted = true;
                return true;
            }
            catch (Exception)
            {
                Static.Properties.Instance.IsQueryExecuted = true;
                return false;
            }
        }
        #endregion

        #region UpdateActionAsync
        public async Task<bool> UpdateActionAsync(Action value)
        {
            Static.Properties.Instance.IsQueryExecuted = false;
            try
            {
                var updatableAction = await GetActionByUidAsync(value.Uid);

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
                newCheckpoints.ForEach(newChk => updatableAction.Checkpoints.Add(new Checkpoint(newChk)));

                await _actionContext.SaveChangesAsync();
                Static.Properties.Instance.IsQueryExecuted = true;
                return true;
            }
            catch (Exception e)
            {
                Static.Properties.Instance.IsQueryExecuted = true;
                return false;
            }
        }
        #endregion

        #region UpdateCheckpointAsync
        public async Task<bool> UpdateCheckpointAsync(Checkpoint value)
        {
            Static.Properties.Instance.IsQueryExecuted = false;
            try
            {
                var updatableCheckpoint = await GetCheckpointByUidAsync(value.Uid);
                value.CopyPropertiesTo(updatableCheckpoint);
                await _actionContext.SaveChangesAsync();
                Static.Properties.Instance.IsQueryExecuted = true;
                return true;
            }
            catch
            {
                Static.Properties.Instance.IsQueryExecuted = true;
                return false;
            }
        }
        #endregion

        #endregion
    }
}