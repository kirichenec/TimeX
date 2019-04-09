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

        private readonly ActionContext _actionContext;

        public Action GetActionByUid(string uid)
        {
            return _actionContext.Actions.Include($"{nameof(Checkpoint)}s").FirstOrDefault(a => a.Uid == uid);
        }

        public IQueryable<Action> QueryableActions()
        {
            return _actionContext.Actions.AsQueryable();
        }

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
    }
}
