using System;
using System.Linq;

namespace TimeXv2.Model.Data
{
    public interface IDataService
    {
        string AddAction(Action value);
        bool DeleteAction(string uid);
        Action GetActionByUid(string uid);
        IQueryable<Action> QueryableActions();
        bool UpdateAction(Action value);
    }
}
