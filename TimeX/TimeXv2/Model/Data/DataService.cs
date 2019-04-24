using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
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
            Static.Properties.Instance.IsLoaded = false;
            var result = await _actionContext.Actions.Include($"{nameof(Checkpoint)}s").FirstOrDefaultAsync(a => a.Uid == uid);
            Static.Properties.Instance.IsLoaded = true;
            return result;
        }
        #endregion

        #region GetActionsListAsync
        public async Task<List<Action>> GetActionsListAsync(bool isFullLoad)
        {
            Static.Properties.Instance.IsLoaded = false;
            try
            {
                DbQuery<Action> query = _actionContext.Actions;
                if (isFullLoad)
                {
                    query = query.Include($"{nameof(Checkpoint)}s");
                }
                var result = await query.ToListAsync();
                Static.Properties.Instance.IsLoaded = true;
                return result;
            }
            catch (Exception)
            {
                Static.Properties.Instance.IsLoaded = true;
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
            Static.Properties.Instance.IsLoaded = false;
            try
            {
                value.Uid = Guid.NewGuid().ToString();
                _actionContext.Actions.Add(value);
                await _actionContext.SaveChangesAsync();
                Static.Properties.Instance.IsLoaded = true;
                return value.Uid;
            }
            catch (Exception)
            {
                Static.Properties.Instance.IsLoaded = true;
                return null;
            }
        }
        #endregion

        #region DeleteActionAsync
        public async Task<bool> DeleteActionAsync(string uid)
        {
            Static.Properties.Instance.IsLoaded = false;
            try
            {
                _actionContext.Actions.Remove(await GetActionByUidAsync(uid));
                await _actionContext.SaveChangesAsync();
                Static.Properties.Instance.IsLoaded = true;
                return true;
            }
            catch (Exception)
            {
                Static.Properties.Instance.IsLoaded = true;
                return false;
            }
        }
        #endregion

        #region UpdateActionAsync
        public async Task<bool> UpdateActionAsync(Action value)
        {
            Static.Properties.Instance.IsLoaded = false;
            try
            {
                var updatableAction = await GetActionByUidAsync(value.Uid);
                value.CopyPropertiesTo(updatableAction);
                await _actionContext.SaveChangesAsync();
                Static.Properties.Instance.IsLoaded = true;
                return true;
            }
            catch (Exception)
            {
                Static.Properties.Instance.IsLoaded = true;
                return false;
            }
        }
        #endregion

        #endregion
    }
}
