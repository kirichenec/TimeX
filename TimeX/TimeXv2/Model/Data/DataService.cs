using System;
using System.Linq;
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

        #region GetActionByUid
        public Action GetActionByUid(string uid)
        {
            return _actionContext.Actions.Include($"{nameof(Checkpoint)}s").FirstOrDefault(a => a.Uid == uid);
        }
        #endregion

        #region QueryableActions
        public IQueryable<Action> QueryableActions()
        {
            return _actionContext.Actions.AsQueryable();
        }
        #endregion

        #region AddAction
        public string AddAction(Action value)
        {
            try
            {
                value.Uid = Guid.NewGuid().ToString();
                _actionContext.Actions.Add(value);
                _actionContext.SaveChanges();
                return value.Uid;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region DeleteAction
        public bool DeleteAction(string uid)
        {
            try
            {
                _actionContext.Actions.Remove(GetActionByUid(uid));
                _actionContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region UpdateAction
        public bool UpdateAction(Action value)
        {
            try
            {
                var updatableAction = GetActionByUid(value.Uid);
                value.CopyPropertiesTo(updatableAction);
                _actionContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #endregion
    }
}
